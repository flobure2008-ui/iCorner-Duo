$ErrorActionPreference = "Continue"

Write-Host "-> Recuperation du token GitHub..." -ForegroundColor Cyan
$p = @('protocol=https','host=github.com','')
$res = $p | git credential fill
$token = ($res | Select-String '^password=').Line.Substring(9)

if (-not $token) {
    Write-Host "Erreur: Impossible de recuperer le token GitHub." -ForegroundColor Red
    exit 1
}

$headers = @{
    'Authorization' = "Bearer $token"
    'Accept' = 'application/vnd.github+json'
    'User-Agent' = 'iSuite-Release-Publisher'
}

$repo = "flobure2008-ui/iSuite"
$tag = "v1.0.0"
$releaseName = "iSuite v1.0.0 - Liquid Glass Edition & Windows 12 Shell"

# 1. Delete all old releases on GitHub
Write-Host "-> Suppression des anciennes releases sur GitHub..." -ForegroundColor Cyan
try {
    $existing = Invoke-RestMethod -Uri "https://api.github.com/repos/$repo/releases" -Headers $headers -Method Get
    foreach ($r in $existing) {
        Write-Host "   Suppression de la release '$($r.name)' (ID: $($r.id), Tag: $($r.tag_name))..." -ForegroundColor Yellow
        Invoke-RestMethod -Uri "https://api.github.com/repos/$repo/releases/$($r.id)" -Headers $headers -Method Delete
    }
} catch {
    Write-Host "   Remarque: aucune ancienne release a supprimer." -ForegroundColor DarkGray
}

# 2. Delete old remote tags
Write-Host "-> Nettoyage des anciens tags distants..." -ForegroundColor Cyan
$oldTags = @("v2.0.0", "v2.1.0", "v2.2.0", "v2.3.0", "v2.4.0", "v3.0.0", "v3.1.0", "v3.5.0", "v3.6.0")
foreach ($t in $oldTags) {
    try {
        & git push origin --delete $t 2>$null
        Write-Host "   Tag distant '$t' supprime." -ForegroundColor Gray
    } catch { }
}

# 3. Delete old local tags
Write-Host "-> Nettoyage des anciens tags locaux..." -ForegroundColor Cyan
foreach ($t in $oldTags) {
    try {
        & git tag -d $t 2>$null
    } catch { }
}

# 4. Commit and push current codebase to main
Write-Host "-> Commit et push sur 'main'..." -ForegroundColor Cyan
& git add -A
& git commit -m "chore(release): official release v1.0.0" 2>$null
& git push origin main

# 5. Create and push tag v1.0.0
Write-Host "-> Creation et push du tag 'v1.0.0'..." -ForegroundColor Cyan
& git tag -d $tag 2>$null
& git push origin --delete $tag 2>$null
& git tag -a $tag -m "iSuite v1.0.0 - Official Release"
& git push origin $tag

# 6. Create GitHub Release
Write-Host "-> Creation de la release officielle sur GitHub ($tag)..." -ForegroundColor Cyan

$bodyLines = @(
    "# iSuite v1.0.0 - Official Liquid Glass Edition & Windows 12 Shell",
    "",
    "Bienvenue dans la premiere version officielle d'**iSuite** (v1.0.0), l'ecosysteme de personnalisation complete en verre liquide (**Liquid Glass**) pour Windows 10 et Windows 11.",
    "",
    "---",
    "",
    "## Ce qui est inclus dans cette Release 1.0.0",
    "",
    "- **iSuite.exe (Master Shell & Studio)** : Remplace completement la barre des taches native de Windows (Shell_TrayWnd), gestion des applications ouvertes avec bascule en 1 clic, Menu Demarrer acrylique, Centre de Controle lateral et studio unifie.",
    "- **iStore.exe** : Marketplace officielle pour installer, mettre a jour et gerer tous les modules de personnalisation.",
    "- **iMaker.exe** : Studio visuel complet permettant a quiconque de concevoir, tester, compiler en .exe et publier ses propres applications et widgets.",
    "- **iWidgets.exe** : Moteur de widgets de bureau complet facon Rainmeter (Horloge, Moniteur CPU/RAM, Meteo, Spectre Audio, Z-Order au fond du bureau et clics traversants).",
    "- **iVolume.exe** : HUD Audio et Lecteur Liquid Glass remplacant l'OSD Windows.",
    "- **iAltTab.exe** : Mission Control en verre depoli avec recherche instantanee de fenetres.",
    "- **iSuite-Setup.exe** : Assistant d'installation automatique en 1 clic avec raccourcis Bureau et demarrage de session.",
    "- **iHub.exe**, **iCorner.exe**, **iBridge.exe**, **iIsland.exe**, **iMascot.exe** : Modules specialises pour coins d'ecran, transitions multi-ecrans, Dynamic Island et mascottes celestes.",
    "",
    "---",
    "",
    "## Installation Rapide",
    "",
    "1. Telechargez **iSuite-Setup.exe** ci-dessous pour installer automatiquement l'ensemble des modules.",
    "2. Ou telechargez les executables individuels selon vos besoins."
)

$releaseBody = $bodyLines -join "`n"

$releaseObj = @{
    tag_name = $tag
    target_commitish = "main"
    name = $releaseName
    body = $releaseBody
    draft = $false
    prerelease = $false
}

$releaseJson = $releaseObj | ConvertTo-Json

$createdRelease = Invoke-RestMethod -Uri "https://api.github.com/repos/$repo/releases" -Headers $headers -Method Post -Body $releaseJson

Write-Host "Release creee avec succes sur GitHub ! ID: $($createdRelease.id)" -ForegroundColor Green

# 7. Upload binary assets
$uploadUrlBase = $createdRelease.upload_url.Replace("{?name,label}", "")
$distFiles = Get-ChildItem "dist\*.exe"

Write-Host "-> Televersement des $($distFiles.Count) executables dans les assets de la release..." -ForegroundColor Cyan

foreach ($file in $distFiles) {
    Write-Host "   Televersement de $($file.Name)..." -NoNewline
    $uploadUri = "$uploadUrlBase?name=$($file.Name)"
    $bytes = [System.IO.File]::ReadAllBytes($file.FullName)

    $assetHeaders = @{
        'Authorization' = "Bearer $token"
        'Content-Type' = 'application/octet-stream'
        'User-Agent' = 'iSuite-Release-Publisher'
    }

    $uploadResult = Invoke-RestMethod -Uri $uploadUri -Headers $assetHeaders -Method Post -Body $bytes
    Write-Host " [OK]" -ForegroundColor Green
}

Write-Host "`n==========================================================" -ForegroundColor Green
Write-Host "  RELEASE v1.0.0 PUBLIEE AVEC SUCCES SUR GITHUB !" -ForegroundColor White
Write-Host "  URL: $($createdRelease.html_url)" -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Green
