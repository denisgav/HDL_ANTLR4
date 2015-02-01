@echo off
setlocal

set SOLUTION_NAME=HDL_ANTLR4.sln

:: Check prerequisites
if not defined VS100COMNTOOLS (
    if not defined VS120COMNTOOLS (
        if not defined VS140COMNTOOLS (
            echo "Error: build.cmd should be run from a Visual Studio (2010 or above) Command Prompt."
            exit /b 1
        ) else set TOOLS_DIR=%VS140COMNTOOLS%
    ) else set TOOLS_DIR=%VS120COMNTOOLS%
) else set TOOLS_DIR=%VS100COMNTOOLS%
call "%TOOLS_DIR%\vsvars32.bat"

:: Enforce package restore to avoid build issues. See http://go.microsoft.com/fwlink/?LinkID=317568 for more details
tools\NuGet.exe restore %SOLUTION_NAME%

set config=%1
if "%config%" == "" (
    set config=Debug
)

msbuild %SOLUTION_NAME% /p:Configuration="%config%" /flp:LogFile=msbuild.log;Verbosity=Normal
