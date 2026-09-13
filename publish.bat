@echo off
dotnet publish UpdateServices -c Release -r win-x86 --self-contained false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

pause