cd ../../src/Scorpio.Api
echo 'Building...'
dotnet publish --runtime linux-arm64 --configuration Release --output bin/build
echo 'Build done!'
