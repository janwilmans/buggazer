@echo off

set SRC=..\source
set FAILED_BUILDS_LOG=failed-builds.log

rem ----------------------------------------------------------------------------
rem Run compilation
rem ----------------------------------------------------------------------------
call rebuild.cmd %SRC%\SnappyDL.sln x86
call rebuild.cmd %SRC%\SnappyDL.sln x64
call rebuild.cmd %SRC%\Snappy.sln x86
call rebuild.cmd %SRC%\Snappy.sln x64

rem ----------------------------------------------------------------------------
rem Copy files to target folders
rem ----------------------------------------------------------------------------
xcopy /y /d %SRC%\bin\Win32\Release\*.dll any\
xcopy /y /d %SRC%\bin\x64\Release\*.dll any\
xcopy /y /d %SRC%\SnappyPI\bin\Release\*.dll any\
echo F | xcopy /y /d %SRC%\Snappy.Demo\bin\Release\Snappy.Demo.exe any\snappy.exe
exit /b

:end

