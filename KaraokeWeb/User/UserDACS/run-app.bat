@echo off
SET PORT=5001
SET ASPNETCORE_URLS=http://localhost:%PORT%
dotnet publish\UserDACS.dll
pause
