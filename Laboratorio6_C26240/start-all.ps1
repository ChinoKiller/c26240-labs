#!/usr/bin/env pwsh
Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# Directorios base
$BaseDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$Front = Join-Path $BaseDir "frontend-lab"
$Back = Join-Path $BaseDir "backend-lab"
$LogDir = Join-Path $BaseDir "logs"

# Crear carpeta de logs
New-Item -ItemType Directory -Force -Path $LogDir | Out-Null

# Función para crear nuevas ventanas
function Start-NewWindow {
    param (
        [string]$Command,
        [string]$Title
    )
    Start-Process powershell -ArgumentList "-NoExit", "-Command", $Command -WindowStyle Normal -Wait:$false
}

# Si se puede usar Start-Process (siempre en Windows)
try {
    # Frontend
    Start-NewWindow -Title "Frontend" -Command "
        Set-Location '$Front';
        if (!(Test-Path node_modules)) {
            npm install --no-audit --no-fund;
        }
        npm run serve;
    "

    # Backend
    Start-NewWindow -Title 'Backend' -Command "
        Set-Location '$Back';
        dotnet run;
    "

    exit
}
catch {
    # Si falla, ejecutar en background y guardar logs
}

# ---------------- FALLBACK BACKGROUND MODE -----------------

# Frontend background
$FrontLog = Join-Path $LogDir "frontend.log"
$FrontPIDFile = Join-Path $BaseDir ".frontend.pid"

Start-Process powershell -ArgumentList "-Command", "
    Set-Location '$Front';
    if (!(Test-Path node_modules)) {
        npm install --no-audit --no-fund;
    }
    npm run serve
" -RedirectStandardOutput $FrontLog -RedirectStandardError $FrontLog -WindowStyle Hidden

(Get-Process | Where-Object { $_.Path -like '*powershell*' } | Select-Object -Last 1).Id |
    Out-File $FrontPIDFile

# Backend background
$BackLog = Join-Path $LogDir "backend.log"
$BackPIDFile = Join-Path $BaseDir ".backend.pid"

Start-Process powershell -ArgumentList "-Command", "
    Set-Location '$Back';
    dotnet run
" -RedirectStandardOutput $BackLog -RedirectStandardError $BackLog -WindowStyle Hidden

(Get-Process | Where-Object { $_.Path -like '*powershell*' } | Select-Object -Last 1).Id |
    Out-File $BackPIDFile

Write-Host "Frontend and backend started in background. Logs saved in $LogDir"