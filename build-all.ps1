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
    @{ Name = "iCorner"; Src = "src\iCorner\Program.cs"; Out = "dist\iCorner.exe" },
    @{ Name = "iBridge"; Src = "src\iBridge\Program.cs"; Out = "dist\iBridge.exe" },
    @{ Name = "iIsland"; Src = "src\iIsland\Program.cs"; Out = "dist\iIsland.exe" },
    @{ Name = "iMascot"; Src = "src\iMascot\Program.cs"; Out = "dist\iMascot.exe" },
    @{ Name = "iHub";    Src = "src\iHub\Program.cs";    Out = "dist\iHub.exe" }
)

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "  Compilation de la suite iSuite (5 Applications Modulaires)" -ForegroundColor White
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
