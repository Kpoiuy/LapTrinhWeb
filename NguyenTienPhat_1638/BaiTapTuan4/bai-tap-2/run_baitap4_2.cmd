@echo off
set DOTNET_CLI_HOME=D:\New folder (2)\.dotnet
set DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
set DOTNET_CLI_TELEMETRY_OPTOUT=1
cd /d D:\New folder (2)\LapTrinhWeb\NguyenTienPhat_1638\BaiTapTuan4\bai-tap-2
dotnet run --no-build --urls http://localhost:5042 > run.log 2> run.err
