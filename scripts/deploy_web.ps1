#TODO
#  napisac ten skrypt, ktory przyjmuje parametry
# roverIp - wiadomo (potrzebne do polaczenia sie przez ssh)

# ktory wykona kolejno: 
# w katalogu src/Scorpio.Web komende "npm run build" -> wtedy pewstanie katalog src/Scorpio.Web/build, ktory trzeba trzeba skopiowac do src/Scorpio.Api/wwwroot (utworzyc jesli nie istnieje)
# w katalogu src/Scorpio.Api wykona komende "dotnet publish -c Release -r linux-arm64" (opublikowany build powstanie w /src/Scorpio.Api/bin/Release/cos tam/publish)
# caly powyzszy katalog publish zipowac
# zipa wyslac przez SSH (SCP) do lazika do katalogu /home/jetson/scorpioweb i ustawic chmod 755 na pliku "Scorpio.Api"

# skrypt bedzie odpalany z windowsa
# do wykonania tego bedzie potrzebne dotnet core 3.0 sdk, node 10.15.3

#-----------------------------------------------------------------------------------------------------------------------------------------------------------------

# function Show-Help {
#     Write-Host "Usage: ./deploy_web.ps1 rover_adress"
# }

function Show-LoadingAnimation {
    param (
        [parameter(Mandatory = $true)]
        [System.Management.Automation.Job]
        $job,
        [parameter(Mandatory = $true)]
        [String]
        $loading_text,
        [parameter(Mandatory = $true)]
        [String]
        $success_text,
        [parameter(Mandatory = $true)]
        [String]
        $fail_text
    )
    
    $progressBar = @("|", "/", "-", "\")
    $i = 0
    while ($job.JobStateInfo.State -eq "Running") {
        $i = $i % $progressBar.Count
        $icon = $progressBar[$i]
        Write-Host -ForegroundColor "Yellow" -NoNewline "[ $($icon) ] $($loading_text)`r"
        Start-Sleep -Milliseconds 125
        $i = $i + 1
    }
    Write-Host -NoNewline "$(" " * 64)`r"
    $job_res = Receive-Job $job -Wait
    if ($job_res.State -ne "Failed") {
        Write-Host -ForegroundColor "Green" "[ ✓ ] $($success_text)"
    }
    else {
        Write-Host -ForegroundColor "Red" "[ X ] $($fail_text)"
        Write-Error $job_res.ChildJobs[0].JobStateInfo.Reason.Message
        exit
    }
}

# if ($args.Count -lt 1) {
#     Show-Help
#     exit
# }

#Read config JSON
try {
    $CONFIG = Get-Content -Path ".\deploy_config.json" | ConvertFrom-Json
}
catch {
    Write-Error "Couldn't find config file"
    exit
}

#Check if npm and dotnet are installed

try {
    $dotnet_version = dotnet --version
}
catch [System.Management.Automation.CommandNotFoundException] {
    $dotnet_version = $false
}

try {
    $node_version = node -v
}
catch [System.Management.Automation.CommandNotFoundException] {
    $node_version = $false
}

try {
    $npm_version = npm --version
}
catch [System.Management.Automation.CommandNotFoundException] {
    $npm_version = $false
}

try {
    $posh = (Get-InstalledModule "Posh-ssh").version 2>&1
    $posh_version = "$($posh.major).$($posh.minor)"
    $nomatch = $posh -match "No match"
    if($nomatch) {
        throw "Module not found"
    }
}
catch {
    $posh = $false
}

if ($npm_version) {
    Write-Host -ForegroundColor "Green" "npm version:" $npm_version
}
else {
    Write-Host -ForegroundColor "Red" "npm not found"
    exit
}

if ($dotnet_version) {
    Write-Host -ForegroundColor "Green" "dotnet version:" $dotnet_version
}
else {
    Write-Host -ForegroundColor "Red" "dotnet not found"
    exit
}

if ($node_version) {
    Write-Host -ForegroundColor "Green" "node version:" $node_version
}
else {
    Write-Host -ForegroundColor "Red" "node not found"
    exit
}

if ($posh) {
    Write-Host -ForegroundColor "Green" "PoSH-SSH version: $($posh_version)"
}
else {
    $install_posh_ssh_job = Start-Job -ArgumentList $PWD -ScriptBlock {
        Start-Process -Wait -FilePath "powershell.exe" -Verb runAs -ArgumentList "-Command", "Install-Module -Name Posh-SSH -RequiredVersion 2.1 -SkipPublisherCheck" 
        Write-Output "Module Installed"
    }
    Show-LoadingAnimation $install_posh_ssh_job "Instaling PoSH-SSH" "PoSH-SSH installed" "PoSH-SSH intallation failed"
}

#Check connection to adress

$ping_rover_job = Start-Job -ArgumentList $CONFIG.adress -ScriptBlock {
    $rover_adress = $args[0]
    Write-Host -ForegroundColor "Green" "Rover adress:" $rover_adress
    Test-Connection -ComputerName $rover_adress -Count 1
}

Show-LoadingAnimation $ping_rover_job "Pinging rover in progress" "Rover adress responded to ping" "Rover adress didn't respond to ping"

#Build webapp

$build_job = Start-Job -ArgumentList $PWD -ScriptBlock {
    Set-Location $args[0]
    $build_job = Start-Job -ArgumentList $PWD -ScriptBlock {
        Set-Location "$($args[0])/../src/Scorpio.Web"
        try {
            npm run build 2>&1
        }
        catch {
            Write-Host -ForegroundColor "Red" "Error while building"
        } 
    }
    $res = Receive-Job $build_job -Wait
    $res_string = $res | Out-String
    $match = [Bool] ($res_string | Select-String -Pattern "Compiled successfully")
    if($match -eq $false) {
        throw "Build error"
    }
    else {
        Write-Output "Build successful"
    }
}

Show-LoadingAnimation $build_job "Building web app in progress" "Building web app done" "Building web app failed"

#Check if /src/Scorpio.Api/wwwroot exists and copy build

$copy_build_job = Start-Job -ArgumentList $PWD -ScriptBlock {
    $build_path = "$($args[0])/../src/Scorpio.Web/build/*"
    Set-Location "$($args[0])/../src/Scorpio.API"
    $dirExists = Test-Path "wwwroot"
    if($dirExists -eq $false) {
        mkdir "wwwroot"
    }
    Remove-Item -recurse "wwwroot\*" -exclude ".gitkeep" -Force
    Copy-Item $build_path -Destination "wwwroot" -Force
    Write-Output "Copy was successful"
}

Show-LoadingAnimation $copy_build_job "Copying build in progress" "Build copied" "Build copy failed"

# #Publish dotnet app

$publish_job = Start-Job -ArgumentList $PWD -ScriptBlock {
    #$build_path = "$($args[0])/../src/Scorpio.API/build/*"
    Set-Location "$($args[0])/../src/Scorpio.API"
    dotnet publish -c Release -r linux-arm64 /p:PublishReadyToRun=false
    Write-Output "Publish was successful"
}

Show-LoadingAnimation $publish_job "Publishing app in progress" "App published" "App publish failed"

# #Zip publish folder

$zip_job = Start-Job -ArgumentList $PWD -ScriptBlock {
    Set-Location "$($args[0])/../src/Scorpio.API"
    $publish_path = "$($args[0])/../src/Scorpio.Api/bin/Release/netcoreapp3.0/linux-arm64/publish/*"
    Compress-Archive -Path $publish_path -DestinationPath "deploy.zip" -Force
    Write-Output "Zip publish was successful"
}

Show-LoadingAnimation $zip_job "Zipping in progress" "Zipped" "Zip failed"

#Connect to rover

$user = $CONFIG.server_username
$pw = ConvertTo-SecureString -String $CONFIG.server_password -AsPlainText -Force
$cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $user, $pw 

$scp_job = Start-Job -ArgumentList $PWD, $cred, $CONFIG.adress -ScriptBlock {
    Set-Location "$($args[0])/../src/Scorpio.API"
    $credential = $args[1]
    $rover_adress = $args[2]
    Set-SCPFile -NoProgress -Force -Credential $credential -ComputerName "$($rover_adress)" -RemotePath "/home/jetson/scorpioweb" -LocalFile "deploy.zip" 3>&1
    Write-Output "SCP success"
}

Show-LoadingAnimation $scp_job "SCP in progress" "Copied to server" "Copy to server failed"

$ssh_job = Start-Job -ArgumentList $cred, $CONFIG.adress -ScriptBlock {
    $credential = $args[0]
    $rover_adress = $args[1]
    #Set-SCPFile -NoProgress -Force -Credential $credential -ComputerName "$($rover_adress)" -RemotePath "/home/test/" -LocalFile "deploy.zip" 3>&1
    New-SSHSession -Credential $credential -ComputerName "$($rover_adress)" 3>&1
    $session_id = (Get-SSHSession | Where-Object "host" -eq "$($rover_adress)").SessionId
    $connected = (Get-SSHSession | Where-Object "host" -eq "$($rover_adress)").Connected
    if([Bool] $connected) {
        Write-Output "SSH success"
    }
    else {
        throw "No SSH session created"
    }
    $res = Invoke-SSHCommand -SessionId $session_id -Command "cd /home/jetson/scorpioweb; unzip -uo deploy.zip; rm deploy.zip; chmod 755 Scorpio.Api" 3>&1
    if($res.ExitStatus -eq 0) {
        Write-Output "SSH success"
    }
    else {
        throw "ExitStatus != 0"
    }
    Remove-SSHSession -SessionId $session_id
}

Show-LoadingAnimation $ssh_job "Unpacking and setting permissions" "Files are ready on the server" "Couldn't unpack and set prenissions on the server"