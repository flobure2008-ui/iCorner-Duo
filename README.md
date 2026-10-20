# iSuite — Écosystème Modulaire & Shell Windows 12 Concept Glass (v3.1)

<p align="center">
  <img src="assets/showcase_icon.jpg" alt="iSuite Icon" width="220" style="border-radius: 36px;" />
</p>

**iSuite** est une suite logicielle moderne et complète en **Liquid Glass & Glassmorphisme** pour Windows 10 et Windows 11. Elle réinvente l'expérience Windows en proposant un **Shell complet Windows 12 Concept Glass** (Barre des tâches flottante, Menu Démarrer acrylique, Centre d'action et de notifications), des sous-systèmes interconnectés avec zéro latence, un **Master Studio** regroupant toute la personnalisation dans une seule application, un **App Store dédié (`iStore`)** et un **Assistant d'Installation autonome (`iSuite-Setup`)**.

---

## 🏛️ Architecture de la Suite

```
iSuite Ecosystem/
│
├── ❖ iSuite.exe          — Moteur Central & Master Studio (Shell W12, Tâches, Start, Notifs, Corners, Bridge, Island, Mascot)
├── 🛍️ iStore.exe         — Le Store officiel (Marketplace pour télécharger, installer et mettre à jour les modules)
├── 📦 iSuite-Setup.exe   — L'Assistant d'Installation complet (Raccourcis Bureau/Start, auto-démarrage Windows)
├── 🔮 iHub.exe           — Le Tableau de bord & Launcher rapide
│
└── 🧩 Micro-Applications Autonomes :
    ├── ✦ iCorner.exe     — Coins d'écrans superellipse purs (0% CPU)
    ├── 🌊 iBridge.exe     — Passerelle liquide & portails magnétiques inter-écrans
    ├── 💧 iIsland.exe     — Dynamic Island flottante (médias, horloge, focus timer)
    └── ✨ iMascot.exe     — Compagnons célestes de bureau (yeux réactifs, danse audio)
```

---

## 🌟 Nouveautés Majeures de la v3.1

### 1. 🖥️ Shell Windows 12 Concept Glass (`iSuite.exe`)
- **Barre des Tâches Flottante** :
  - Choix entre le style **Dock Flottant Centré** (façon Windows 12 / macOS) ou **Barre Flottante Étendue**.
  - Rendu en verre dépoli avec reflets spéculaires, liseré biseauté et icônes rétroéclairées.
  - Horloge temps réel avec date compacte et puce active sous le bouton Start.
- **Menu Démarrer Acrylique Glass** :
  - Déploiement instantané au clic sur le bouton Démarrer ou la touche Windows.
  - Champ de recherche universel, grille de tuiles acryliques modernes (*Explorateur, Web, Paramètres, Terminal, etc.*) et profil utilisateur avec bouton d'alimentation.
- **Centre de Contrôle & Notifications Latéral** :
  - Panneau latéral en verre dépoli rétractable depuis le coin inférieur droit.
  - Tuiles rapides avec bascule instantanée : **Wi-Fi**, **Bluetooth**, **Éclairage Nocturne**, **Focus / Ne Pas Déranger**.
  - Sliders haute précision pour le **Volume Système** et la **Luminosité de l'Écran**.
  - Cartes interactives empilées pour les notifications récentes.

### 2. 🎛️ Master Studio de Personnalisation Unifiée
Toutes les options et réglages sont regroupés dans **une seule et unique application moderne** :
- `🖥️ Barre des Tâches & Menu` : Mode dock/étendu, rayon d'arrondi, hauteur, horloge, icônes.
- `🔔 Centre de Contrôle & Notifs` : Sliders audio/luminosité, tuiles d'actions rapides.
- `✦ Coins & Dalles d'Écran` : Rayon millimétrique (0 à 110 px), profil superellipse ($n=4.2$) ou arc ($n=2.0$), liseré biseauté.
- `🌊 Passerelle Inter-Écrans` : 7 styles de transition liquide, durée et intensité des ondes.
- `💧 Dynamic Island` : Mode de placement, échelle, comportement au survol.
- `✨ Mascotte Céleste` : Choix de la créature (Lumina, Kitsune, Draco, etc.), réactivité sonore et oculaire.
- `🎨 Palettes Chromatiques` : 12 thèmes de verre unifiés appliqués instantanément sur toute la suite.

### 3. 🛍️ iStore (`iStore.exe`)
- Véritable **Store d'Applications** en verre avec catalogue classé par catégories (*Shell & Bureau*, *Multi-Moniteurs*, *Widgets Flottants*).
- Fiches détaillées avec notes, nombre de téléchargements, descriptif et bouton **Installer / Ouvrir**.
- Téléchargement et compilation à chaud directement depuis le dépôt GitHub.

### 4. 📦 Assistant d'Installation Dédié (`iSuite-Setup.exe`)
- Installeur graphique autonome pour déployer `iSuite` proprement sur votre système :
  - Destination automatique dans `%LocalAppData%\Programs\iSuite`.
  - Création du raccourci Bureau et raccourci dans le Menu Démarrer de Windows.
  - Configuration du lancement automatique au démarrage de session.
  - Lancement immédiat à la fin de l'installation.

---

## ⚡ Compilation Rapide de Toute la Suite

Un script PowerShell compile les **8 exécutables** en une seule commande avec `csc.exe` natif :

```powershell
powershell -ExecutionPolicy Bypass -File .\build-all.ps1
```

Les exécutables sont produits dans le dossier `dist/` :
- `dist/iSuite.exe`
- `dist/iStore.exe`
- `dist/iSuite-Setup.exe`
- `dist/iHub.exe`
- `dist/iCorner.exe`
- `dist/iBridge.exe`
- `dist/iIsland.exe`
- `dist/iMascot.exe`

---

## 📜 Licence

Projet distribué sous licence MIT.
