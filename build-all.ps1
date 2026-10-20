$csc = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if (-not (Test-Path $csc)) {
    Write-Host "Erreur: csc.exe introuvable." -ForegroundColor Red
    exit 1
}

$rootDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$distDir = Join-Path $rootDir "dist"
if (-not (Test-Path $distDir)) { New-Item -ItemType Directory -Path $distDir -Force | Out-Null }

$icon = Join-Path $rootDir "icon.ico"
$iconParam = if (Test-Path $icon) { "/win32icon:`"$icon`"" } else { "" }

$apps = @(
    @{ Name = "iSuite (Master Shell & Studio)"; Src = "src\iSuite\Program.cs"; Out = "dist\iSuite.exe" },
    @{ Name = "iStore (App Marketplace)";       Src = "src\iStore\Program.cs"; Out = "dist\iStore.exe" },
    @{ Name = "iSuite-Setup (Installer Wizard)";Src = "src\iSetup\Program.cs"; Out = "dist\iSuite-Setup.exe" },
    @{ Name = "iHub (Launcher & Dashboard)";    Src = "src\iHub\Program.cs";   Out = "dist\iHub.exe" },
    @{ Name = "iCorner (Coins Purs)";           Src = "src\iCorner\Program.cs";Out = "dist\iCorner.exe" },
    @{ Name = "iBridge (Passerelle Écrans)";    Src = "src\iBridge\Program.cs";Out = "dist\iBridge.exe" },
    @{ Name = "iIsland (Dynamic Island)";       Src = "src\iIsland\Program.cs";Out = "dist\iIsland.exe" },
    @{ Name = "iMascot (Compagnon Céleste)";    Src = "src\iMascot\Program.cs";Out = "dist\iMascot.exe" }
)

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "  Compilation Complète de la Suite iSuite (8 Exécutables)" -ForegroundColor White
Write-Host "==========================================================" -ForegroundColor Cyan

foreach ($app in $apps) {
    $srcPath = Join-Path $rootDir $app.Src
    $outPath = Join-Path $rootDir $app.Out
    Write-Host "-> Compilation de $($app.Name)..." -NoNewline
    
    $argsList = @(
        "/nologo",
        "/target:winexe",
        "/out:`"$outPath`"",
        "/r:System.dll",
        "/r:System.Drawing.dll",
        "/r:System.Windows.Forms.dll",
        "`"$srcPath`""
    )
    if ($iconParam) { $argsList = @($iconParam) + $argsList }

    $proc = Start-Process -FilePath $csc -ArgumentList $argsList -NoNewWindow -Wait -PassThru
    if ($proc.ExitCode -eq 0) {
        $size = (Get-Item $outPath).Length / 1KB
        Write-Host " [OK] ($([Math]::Round($size)) Ko)" -ForegroundColor Green
    } else {
        Write-Host " [ERREUR]" -ForegroundColor Red
    }
}

Write-Host "`nToutes les applications ont ete compilees dans le dossier dist." -ForegroundColor Cyan
