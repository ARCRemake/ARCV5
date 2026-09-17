@echo off
dotnet publish UpdateServices -c Release -r win-x86 --self-contained true -p:PublishTrimmed=true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

pause