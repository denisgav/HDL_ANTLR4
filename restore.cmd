@echo off
setlocal

set SOLUTION_NAME=HDL_ANTLR4.sln

tools\NuGet.exe restore %SOLUTION_NAME%