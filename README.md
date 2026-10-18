<div align="center">
  <img src="icon.png" alt="iCorner Duo Icon" width="160" height="160" />

  #  iCorner Duo

  **Bords arrondis natifs multi-écrans & transitions liquides style Apple iPhone / Universal Control pour Windows**

  [![Platform](https://img.shields.io/badge/Platform-Windows%2010%20%7C%2011-0078D6?style=for-the-badge&logo=windows&logoColor=white)](#)
  [![Language](https://img.shields.io/badge/C%23-.NET%20Native%20Win32-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](#)
  [![Performance](https://img.shields.io/badge/Performance-60%2B%20FPS%20%7C%200%25%20Idle%20CPU-00C853?style=for-the-badge)](#)
</div>

---

## ✨ Présentation

**iCorner Duo** est une application Windows ultra-légère et native (un seul exécutable autonome sans dépendance externe) qui sublime vos écrans avec :
1. **Des angles arrondis parfaits** (courbure continue **Squircle Apple** ou arc circulaire classique) sur tous vos écrans, quelle que soit leur taille, résolution ou orientation (Ultrawide, 16:9, vertical...).
2. **Une animation de transition fluide style Apple iPhone Duo / Universal Control** qui se déclenche automatiquement à l'endroit exact où votre souris passe d'un écran à un autre.

<div align="center">
  <img src="assets/showcase_icon.jpg" alt="iCorner Duo Artwork" width="420" />
</div>

---

## 🚀 Fonctionnalités Principales

### 📐 1. Bords Arrondis Adaptatifs Multi-Écrans
- **Courbure Continue Squircle Apple** : Utilise la formule mathématique d'hyper-ellipse ($x^{4.2} + y^{4.2} = r^{4.2}$) identique au design matériel d'iOS et macOS, ou un **Cercle Classique**.
- **100 % Click-Through & Invisible pour les clics** (`WS_EX_LAYERED | WS_EX_TRANSPARENT`) : Ne gêne jamais vos clics, reste au-dessus de la barre des tâches Windows, et n'apparaît pas dans `Alt+Tab`.
- **Per-Monitor DPI Aware V2** : S'adapte automatiquement à toutes les tailles d'écrans, échelles DPI et branchements/débranchements d'écrans à chaud.
- **Liseré Lumineux (Bezel Glow)** : Ajoute un fin reflet esthétique le long des courbes avec opacité réglable (`0 %` à `100 %`).

### 🌊 2. Transitions Inter-Écrans Style Apple
- **Détection de traversée au pixel près** : L'animation apparaît exactement sur la coordonnée `(X, Y)` où le curseur franchit la bordure entre deux moniteurs.
- **Effet Magnétique de Proximité** : Lorsque le curseur s'approche d'un bord partagé entre deux écrans, le pont liquide commence subtilement à s'étirer avant même la traversée.
- **4 Styles d'Animation Inclus** :
  - **✨ Combo Ultime** : Pont liquide (*Liquid Bridge*) + Onde chromatique *Apple Intelligence* + *Dynamic Island* + pulsation élastique des angles de l'écran rejoint.
  - **💧 Apple Liquid Bridge** : Goutte/pont noir organique (courbes de Bézier) qui s'étire d'un écran à l'autre avec rebond élastique (`EaseOutBack`).
  - **🌈 Apple Intelligence Glow** : Onde lumineuse multicolore le long de la bordure traversée.
  - **💊 Dynamic Island** : Pilule animée en haut de l'écran rejoint affichant le numéro et la résolution de l'écran actif.
- **4 Palettes de Couleurs** :
  - 🎨 *Apple Intelligence* (Cyan, Violet, Magenta, Or)
  - 💎 *iOS Electric Blue* (Bleu électrique, Cyan, Indigo)
  - 🌅 *Sunset Pro* (Corail, Rose, Pourpre, Ambre)
  - ⚪ *Minimal Monochrome* (Argent, Blanc pur, Noir obsidienne)

### ⚙️ 3. Centre de Contrôle & Zone de Notification (System Tray)
- Réglage en temps réel du **rayon (`0` à `110 px`)**, des angles actifs (Haut-Gauche, Haut-Droit, Bas-Gauche, Bas-Droit), de la **durée (`350` à `1800 ms`)** et de l'**amplitude (`60 %` à `180 %`)**.
- Bouton **« ✨ Tester la transition »** pour prévisualiser l'animation instantanément.
- Icône dans la barre système Windows (System Tray) avec menu rapide et option **Lancer automatiquement au démarrage de Windows**.

---

## 📦 Installation & Utilisation

### Option 1 : Lancement direct (Exécutable prêt à l'emploi)
1. Téléchargez ou clonez ce dépôt :
   ```bash
   git clone https://github.com/flobure2008-ui/iCorner-Duo.git
   ```
2. Double-cliquez sur **`iCornerDuo.exe`** (ou sur **`Lancer_iCornerDuo.bat`**).

### Option 2 : Recompiler depuis le code source (`Program.cs`)
Aucune installation lourde n'est requise : Windows inclut nativement le compilateur C# `.NET Framework`.
```powershell
"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /target:winexe /win32icon:"icon.ico" /out:"iCornerDuo.exe" /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll "Program.cs"
```