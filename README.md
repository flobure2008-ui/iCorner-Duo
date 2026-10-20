# iSuite — Écosystème Modulaire Liquid Glass & Shell Windows 12 (v3.5)

<p align="center">
  <img src="assets/showcase_icon.jpg" alt="iSuite Liquid Glass Icon" width="220" style="border-radius: 42px; box-shadow: 0 12px 32px rgba(0, 180, 255, 0.35);" />
</p>

<p align="center">
  <b>Écosystème unifié de personnalisation Windows en Liquid Glass.</b><br/>
  <i>Remplace complètement la barre des tâches native de Windows, offre 11 applications interconnectées, un Master Studio, un App Store et un installeur complet.</i>
</p>

---

## 🏛️ Architecture de la Suite (11 Applications Interconnectées)

```
iSuite Ecosystem/
│
├── ❖ iSuite.exe          — Shell Windows 12 & Master Studio (Remplace Shell_TrayWnd, Apps actives, Start, Notifs, Corners, Bridge, Island, Mascotte)
├── 🛍️ iStore.exe         — Marketplace officielle (Catalogue pour télécharger, installer et mettre à jour tous les modules)
├── 📦 iSuite-Setup.exe   — Assistant d'Installation autonome (Raccourcis Bureau/Start, auto-démarrage Windows)
│
├── 🔊 iVolume.exe        — HUD Audio & Média Liquid Glass (Contrôle du volume, visualiseur audio temps réel & lecteur Apple Music/Spotify)
├── 🗂️ iAltTab.exe        — Mission Control & Switcher de Fenêtres plein écran avec recherche instantanée et fermeture en 1 clic
├── 📊 iWidgets.exe       — Tableau de bord de widgets de bureau (Horloge temps réel, jauges CPU/RAM & bloc-notes liquide auto-sauvegardé)
├── ⚡ iHub.exe           — Lanceur rapide flottant et switcher unifié pour démarrer/arrêter chaque module
│
└── 🧩 Modules Écran Spécialisés :
    ├── ✦ iCorner.exe     — Arrondis d'écrans superellipse purs (0% CPU, halo chromatique de bordure)
    ├── 🌊 iBridge.exe     — Passerelle visuelle liquide & portails magnétiques inter-écrans
    ├── 💧 iIsland.exe     — Dynamic Island liquide au sommet de l'écran (médias, minuteur & notifications)
    └── ✨ iMascot.exe     — Compagnons célestes interactifs sculptés en cristal (Lumina, Kitsune, Draco)
```

---

## 🌟 Nouveautés Majeures de la v3.5 (Liquid Glass Everywhere)

### 1. 🖥️ Remplacement Complet de la Barre des Tâches Windows (`Shell_TrayWnd`)
- **Remplacement Intégral & Failsafe** :
  - Masque automatiquement la barre des tâches native de Windows (`Shell_TrayWnd` et `Shell_SecondaryTrayWnd`) pour laisser place au dock flottant Windows 12 Liquid Glass.
  - **Sécurité et restauration automatique garantie** : La barre Windows d'origine est automatiquement restaurée dès la fermeture d'iSuite, lors de l'arrêt du PC, ou en un clic via le menu contextuel du plateau système ou le bouton Failsafe.
- **Gestion Complète des Applications Ouvertes en Direct** :
  - Énumération en temps réel des fenêtres actives (`EnumWindows`).
  - Puces d'applications en verre avec initiales/icônes, titre tronqué et voyant lumineux sous l'application active.
  - **Clic gauche** : Bascule instantanée (`SetForegroundWindow`), restauration si réduite, ou réduction si déjà active.
  - **Clic droit** : Menu contextuel pour Basculer, Réduire, Agrandir ou Fermer la fenêtre (`WM_CLOSE`).
  - **Clic du milieu** : Fermeture rapide de la fenêtre.

### 2. 💎 Esthétique "Liquid Glass" Révolutionnaire
- Nouveau design system basé sur la réfraction optique du verre liquide :
  - Reflets caustiques spéculaires sur l'arête supérieure (liseré 1px dégradé blanc pur à transparent).
  - Teinte de verre dépoli acrylique profond avec diffusion chromatique selon la palette choisie (Prisme Éther, Océan Polaire, Aurore Boréale, etc.).
  - Bords biseautés lumineux avec lueur néon réactive.
  - Nouvelle icône officielle 3D Liquid Glass générée en ultra haute définition.

### 3. 🔊 Nouveaux Outils de Personnalisation Windows
- **`iVolume.exe` (HUD Audio & Média Liquid Glass)** :
  - Capsule flottante au design Apple / Windows 12 remplaçant l'OSD de volume natif de Windows.
  - Slider de volume liquide réactif au glissement de souris et à la molette.
  - Visualiseur de spectre audio animé en direct.
  - Contrôles de lecture (Précédent, Lecture/Pause, Suivant) et affichage du morceau en cours.
- **`iAltTab.exe` (Mission Control & Task Switcher)** :
  - Vue plein écran en verre dépoli affichant toutes les fenêtres ouvertes sous forme de cartes Liquid Glass.
  - **Recherche instantanée** : Tapez n'importe quelle lettre au clavier pour filtrer immédiatement la liste des fenêtres.
  - Bouton de fermeture directe `✕` sur chaque carte.
- **`iWidgets.exe` (Widgets de Bureau Liquid Glass)** :
  - **Horloge & Date dynamique** : Affichage fluide avec impulsion des secondes.
  - **Moniteur de performances** : Jauges réactives CPU (%) et RAM (%).
  - **Bloc-notes rapide** : Mémo translucide sur le bureau avec sauvegarde automatique instantanée sur disque.

### 4. 🎛️ Master Studio & iStore
- **Master Studio (`iSuite.exe`)** : Regroupe 100% de la personnalisation dans une seule interface moderne avec 7 onglets : *Barre & Shell*, *Arrondis d'Écran*, *Passerelle Écrans*, *Dynamic Island*, *Mascottes Célestes*, *Thèmes & Verre*, *Modules & Store*.
- **iStore (`iStore.exe`)** : Catalogue pour lancer, installer et gérer chaque composant en un clic.

---

## 🚀 Téléchargement & Installation

1. Téléchargez **`iSuite-Setup.exe`** ou **`iSuite.exe`** depuis la section **[Releases](https://github.com/flobure2008-ui/iSuite/releases)**.
2. Lancez **`iSuite-Setup.exe`** pour installer automatiquement la suite, créer les raccourcis Bureau / Menu Démarrer et configurer le démarrage automatique.

---

## ⚡ Compilation Rapide de Toute la Suite (11 Exécutables)

Un script PowerShell compile les **11 exécutables** en une seule commande avec `csc.exe` natif et la nouvelle icône Liquid Glass :

```powershell
powershell -ExecutionPolicy Bypass -File .\build-all.ps1
```

Les exécutables sont produits dans le dossier `dist/` :
- `dist/iSuite.exe`
- `dist/iStore.exe`
- `dist/iSuite-Setup.exe`
- `dist/iVolume.exe`
- `dist/iAltTab.exe`
- `dist/iWidgets.exe`
- `dist/iHub.exe`
- `dist/iCorner.exe`
- `dist/iBridge.exe`
- `dist/iIsland.exe`
- `dist/iMascot.exe`

---

## 📜 Licence

Projet distribué sous licence MIT.
