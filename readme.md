### Clean code is a must.

Clean code & software crafsmanship enthusiast. Software engineer.

![Continous Integration](https://github.com/projscorpio/Scorpio.NET/workflows/ci/badge.svg)

## 1 Installing build tools
##### Dotnet SDK 3.0:
refer to microsoft docs

##### Node & npm
```
wget https://nodejs.org/dist/v12.13.0/node-v12.13.0-linux-arm64.tar.xz
tar -xJf node-v12.13.0-linux-arm64.tar.xz
cd node-v12.13.0-linux-arm64
sudo cp -R * /usr/local/
node -v
npm -v
```
Reboot might be required.


## 2 Building Web
#### EVERYTHING scripts (simplest way):
```
cd scripts/linux/
./install_web.sh           # will install front-end dependencies (do only once)
./build_web.sh             # creates web static files in src/Scorpio.Web/build
./build_api.sh             # creates api executable in src/Scorpio.Api/bin/builld
./copy_web_to_api.sh       # copies web to wwwroot of api executable
./run.sh                   # runs api executable
```

#### 2.0 Building Scorpio.API (back-end net core 3.0 app):
Build tools check (required dotnet sdk 3.0):
```
dotnet -v
```
By script:
```
cd scripts/linux
./build_api.sh
```

Manually:
```
cd src/Scorpio.Api
dotnet publish --runtime linux-arm64 --configuration Release --output bin/build
```
Released files are located in src/Scorpio.Api/bin/build
Just use:
```
sudo ./Scorpio.Api
``` 
to launch it.

Configurable parameters are in Scorpio.Api/bin/build/appSettings.Production.json.

#### 2.1 Building Scorpio.Web (front-end React app):
Build tools check: (required node.js):
```
node -v
npm -v
```

By script:
```
cd scripts/linux
./build_web.sh
```

Manually:
```
cd src/Scorpio.Web
npm run build
```

Configurable parameters are in Scorpio.Web/.env.production (yes, leave the backend url empty, as web hosted on same host as API).

## 3 Building GUI
TODO
