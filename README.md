# iSuite — Écosystème Modulaire en Glassmorphisme pour Windows (v3.0)

<p align="center">
  <img src="assets/showcase_icon.jpg" alt="iSuite Icon" width="220" style="border-radius: 36px;" />
</p>

**iSuite** est une collection d'applications indépendantes et ultra-légères en **Liquid Glass & Glassmorphisme** conçues pour sublimer Windows 10 et Windows 11. Chaque application est autonome, ne consomme que très peu de ressources (~0 à 2% CPU) et peut être utilisée seule ou orchestrée depuis le **Launcher Store `iHub`**.

---

## 📦 Les Applications de la Suite

```
iSuite/
├── ✦ iCorner   — Coins d'écran arrondis organiques purs (0% CPU)
├── 🌊 iBridge   — Passerelle liquide & portail magnétique inter-écrans
├── 💧 iIsland   — Dynamic Island flottante (médias, horloge, focus timer)
├── ✨ iMascot   — Compagnons célestes interactifs en verre (bureau & musique)
└── 🔮 iHub      — Le Launcher & Store pour tout installer, lancer et gérer
```

---

### 1. ✦ iCorner — Arrondis d'Écrans Organiques
- **Courbures continues** : Superellipse mathématique ($n=4.2$) ou Arc Circulaire classique ($n=2.0$).
- Rayon réglable au pixel près (0 à 110 px) avec sélection indépendante des angles.
- **Liseré spéculaire et biseau lumineux** : Rendu optique haute joaillerie avec halo d'ambiance.
- **Modes de résonance** : Fixe dépoli (0% CPU), ondulation spectrale, respiration et synchronisation sonore native.

### 2. 🌊 iBridge — Passerelle Fluide Inter-Écrans
- Détection instantanée des traversées du curseur entre moniteurs pour les configurations multi-écrans.
- **Halo magnétique d'approche** : Onde lumineuse et caustique qui prévient le passage d'une dalle à l'autre.
- **7 Styles de transition** : Fusion Glassmorphique, Pont Liquide en Verre Dépoli, Onde Prismatique, Filaments Quantiques, Poussière Stellaire, Ondes Concentriques, Ligne Optique Horizon.

### 3. 💧 iIsland — Dynamic Island en Verre Liquide
- Capsule flottante au sommet de l'écran avec dynamique d'amortissement souple (*Spring Damping*).
- **Lecteur Média Universel** : Détection automatique Spotify, Apple Music, YouTube, Web et visualiseur de spectre audio en direct (16 barres).
- **Minuteur Focus Pomodoro (25 min)** intégré avec commandes instantanées.
- Horloge stylisée au repos, survol interactif et adaptation intelligente des teintes.

### 4. ✨ iMascot — Compagnons Célestes de Bureau
- Mascottes interactives sculptées en cristal et verre soufflé flottant librement sur votre bureau.
- **Suivi oculaire dynamique** : Le regard de la mascotte suit votre curseur en direct.
- **Danse au rythme de la musique** : Réactivité aux crêtes sonores de votre PC.
- Bulles de dialogue bienveillantes et déplacement libre par glisser-déposer.

### 5. 🔮 iHub — Le Launcher Store Centralisé
- Interface centrale pour visualiser l'état de chaque application (Actif avec consommation RAM en temps réel, Installé ou Disponible).
- Boutons rapides : **Lancer**, **Arrêter**, **Ouvrir les Paramètres**, et **Démarrage automatique avec Windows**.
- Commandes globales : **Tout Lancer** et **Tout Arrêter**.
- Téléchargement et installation automatique des applications manquantes depuis les releases GitHub.

---

## 🚀 Installation Rapide

1. Téléchargez la suite ou simplement **`iHub.exe`** depuis la section **[Releases](https://github.com/flobure2008-ui/iCorner-Duo/releases)**.
2. Lancez `iHub.exe` : depuis le hub, lancez ou installez les modules souhaités en un seul clic !
3. Chaque application est 100% portable sans aucun installateur lourd.

---

## 🛠️ Compilation depuis la Source (PowerShell)

Pour recompiler l'intégralité de la suite (les 5 exécutables) en une seule commande :

```powershell
powershell -ExecutionPolicy Bypass -File .\build-all.ps1
```

Les exécutables autonomes sont générés instantanément dans le dossier `dist/` :
- `dist/iCorner.exe`
- `dist/iBridge.exe`
- `dist/iIsland.exe`
- `dist/iMascot.exe`
- `dist/iHub.exe`

---

## 💡 Renommer le Dépôt GitHub

Pour renommer votre dépôt GitHub en **`iSuite`** :
1. Rendez-vous sur votre dépôt : `https://github.com/flobure2008-ui/iCorner-Duo`
2. Cliquez sur l'onglet **Settings** (Paramètres).
3. Dans la section **Repository name**, saisissez `iSuite` et validez avec **Rename**.
4. En local, mettez à jour l'URL distante avec :
   ```bash
   git remote set-url origin https://github.com/flobure2008-ui/iSuite.git
   ```

---

## 📜 Licence

Projet distribué sous licence MIT.
