# iSuite — Écosystème Modulaire Liquid Glass & Shell Windows 12 (v3.6)

<p align="center">
  <img src="assets/showcase_icon.jpg" alt="iSuite Liquid Glass Icon" width="220" style="border-radius: 42px; box-shadow: 0 12px 32px rgba(0, 180, 255, 0.35);" />
</p>

<p align="center">
  <b>Écosystème unifié de personnalisation Windows en Liquid Glass.</b><br/>
  <i>Remplace complètement la barre des tâches native de Windows, offre 12 applications interconnectées, un Studio de Création d'Apps (iMaker), un moteur de Widgets style Rainmeter, un App Store et un installeur complet.</i>
</p>

---

## 🏛️ Architecture de la Suite (12 Applications Interconnectées)

```
iSuite Ecosystem/
│
├── ❖ iSuite.exe          — Shell Windows 12 & Master Studio (Remplace Shell_TrayWnd, Apps actives, Start, Notifs, Corners, Bridge, Island, Mascotte)
├── 🛍️ iStore.exe         — Marketplace officielle (Catalogue pour télécharger, installer et créer des modules)
├── 🛠️ iMaker.exe         — Studio de Création d'Apps & Widgets (Concevez, testez, compilez et publiez vos propres apps en 1 clic)
├── 📦 iSuite-Setup.exe   — Assistant d'Installation autonome (Raccourcis Bureau/Start, auto-démarrage Windows)
│
├── 📊 iWidgets.exe       — Moteur de Widgets style Rainmeter (Horloge Pro, Moniteur CPU/RAM, Météo, Spectre Audio, Click-through & Z-Order)
├── 🔊 iVolume.exe        — HUD Audio & Média Liquid Glass (Contrôle du volume, visualiseur audio temps réel & lecteur Apple Music/Spotify)
├── 🗂️ iAltTab.exe        — Mission Control & Switcher de Fenêtres plein écran avec recherche instantanée et fermeture en 1 clic
├── ⚡ iHub.exe           — Lanceur rapide flottant et switcher unifié pour démarrer/arrêter chaque module
│
└── 🧩 Modules Écran Spécialisés :
    ├── ✦ iCorner.exe     — Arrondis d'écrans superellipse purs (0% CPU, halo chromatique de bordure)
    ├── 🌊 iBridge.exe     — Passerelle visuelle liquide & portails magnétiques inter-écrans
    ├── 💧 iIsland.exe     — Dynamic Island liquide au sommet de l'écran (médias, minuteur & notifications)
    └── ✨ iMascot.exe     — Compagnons célestes interactifs sculptés en cristal (Lumina, Kitsune, Draco)
```

---

## 🌟 Nouveautés de la v3.6

### 1. 🛠️ `iMaker.exe` — Créez Vos Propres Applications & Widgets
Permet à n'importe quel utilisateur ou développeur de créer ses propres applications et widgets pour l'écosystème :
- **Modèles de départ (Templates)** :
  - *Widget de Bureau Rainmeter* (Horloge, météo, surveillance système, données web).
  - *Application Fenêtre Flottante* (Outil de productivité, calculette verre, tableau de bord).
  - *Spectre Audio & Musique* (Visualiseur d'ondes sonores interactif).
  - *Outil Mémos & Tâches* (Gestionnaire de notes persistantes).
- **Inspecteur Visuel en Verre** : Ajustez le titre, l'identifiant, le glyphe/icône, la catégorie, les dimensions, l'opacité du verre, l'arrondi des coins et les clics traversants.
- **Actions en 1 Clic** :
  - **▶ Tester mon Application** : Compilation et exécution instantanée à chaud.
  - **📦 Compiler & Exporter (.exe)** : Génère un fichier exécutable autonome haute performance avec l'icône de la suite.
  - **🛍️ Publier dans iStore** : Intègre automatiquement la création dans le catalogue local d'iStore dans le dossier `plugins/`.

### 2. 📊 `iWidgets.exe` — Moteur de Widgets de Bureau Style Rainmeter
Un gestionnaire complet de skins et widgets avec une infinité d'options :
- **Collection de Widgets Inclus** :
  - ⏰ **Horloge & Date Pro** : Heure grand format, impulsion fluide des secondes, date et calendrier.
  - ⚡ **Moniteur Système Pro** : Jauges réactives CPU (%) et mémoire vive RAM (%).
  - 🌤️ **Météo & Climat** : Température actuelle, ciel et prévisions.
  - 🎵 **Spectre Audio Rainmeter** : Égaliseur animé à 20 barres dansant au rythme du son du PC.
- **Studio de Configuration Dédié & Options Avancées** :
  - **Niveau d'affichage (Z-Order)** : *Au fond du Bureau (comme Rainmeter - sous toutes les fenêtres)*, *Normal*, ou *Toujours au premier plan (TopMost)*.
  - **Clics Traversants (Click-Through)** : La souris traverse le widget de manière transparente pour ne jamais gêner votre travail.
  - **Verrouillage de position** : Empêche tout déplacement involontaire à la souris.
  - **Curseur d'opacité et de rayon** : Personnalisez la transparence du verre de 10% à 100% et la courbure des coins.
  - **Menu contextuel direct** : Clic droit sur n'importe quel widget sur le bureau pour accéder à tous ses paramètres.

### 3. 🖥️ Remplacement Complet de la Barre des Tâches Windows (`Shell_TrayWnd`)
- Masque automatiquement la barre des tâches native de Windows pour laisser place au dock flottant Windows 12 Liquid Glass.
- Détection et gestion complète des applications ouvertes avec miniatures, activation, minimisation et fermeture en un clic.
- Failsafe de restauration automatique à la fermeture ou en un clic dans la zone de notification.

---

## ⚡ Compilation Rapide de Toute la Suite (12 Exécutables)

Un script PowerShell compile les **12 exécutables** en une seule commande avec `csc.exe` natif et l'icône Liquid Glass :

```powershell
powershell -ExecutionPolicy Bypass -File .\build-all.ps1
```

---

## 📜 Licence

Projet distribué sous licence MIT.
