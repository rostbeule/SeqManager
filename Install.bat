@echo off

REM Merke dir das aktuelle Ausführungsverzeichnis
set "CurrentDirectory=%CD%"

REM Baue die Anwendung
echo.
echo Building .NET Core application...

set SolutionFile=SeqManager.sln
set Configuration=Release

dotnet build %SolutionFile% -c %Configuration% >nul

if %errorlevel% neq 0 (
  echo Error: Build failed!
  pause
  exit /b %errorlevel%
) else (
  echo Build succeeded.
  timeout /nobreak /t 1 >nul
)

REM Kopiere benötigte Files
echo.
echo Copying Files to SeqManager_v1...

set SourceDirectory=.\SeqManager\bin\Release\net8.0-windows
set TargetDirectory=SeqManager_v1.0

mkdir %TargetDirectory%

copy "%SourceDirectory%\Abstractions.dll" %TargetDirectory%\
copy "%SourceDirectory%\Autofac.dll" %TargetDirectory%\
copy "%SourceDirectory%\SeqManager.dll" %TargetDirectory%\
copy "%SourceDirectory%\SeqManager.exe" %TargetDirectory%\
copy "%SourceDirectory%\SeqManager.runtimeconfig.json" %TargetDirectory%\
copy "%SourceDirectory%\Services.dll" %TargetDirectory%\

echo Files copied successfully.

REM Lösche unnötige Files und Verzeichnisse
echo.
echo Deleting unnecessary Files and Directories...

cd %CurrentDirectory%

REM Lösche alle Dateien rekursiv, außer das Batch-Skript und das TargetDirectory
for %%i in (*) do (
  if /i not "%%i"=="%~nx0" if /i not "%%i"=="%TargetDirectory%" del /q /s "%%i"
)

REM Lösche alle Verzeichnisse rekursiv, außer das TargetDirectory
for /d %%i in (*) do (
  if /i not "%%i"=="%TargetDirectory%" rd /s /q "%%i"
)

echo Cleanup completed.

REM Lösche das Batch-Skript selbst
del /q "%~nx0"

exit /b 0
