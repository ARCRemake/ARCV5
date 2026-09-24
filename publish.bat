@echo off
dotnet publish ARCRemake -c Release -r linux-x64 --self-contained true -p:PublishTrimmed=false
dotnet publish UpdateServices -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -p:IncludeNativeLibrariesForSelfExtract=true
pause