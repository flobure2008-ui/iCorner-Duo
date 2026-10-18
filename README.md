# iCorner Duo — Édition Liquid Glass & Mascottes Kawaii (v2.2)

<p align="center">
  <img src="assets/showcase_icon.jpg" alt="iCorner Duo Icon" width="220" style="border-radius: 36px;" />
</p>

**iCorner Duo** est un studio visuel ultra-léger en **Liquid Glass & Glassmorphisme** pour Windows 10 et Windows 11. Il sculpte des coins d'écran arrondis haute précision sur toutes vos dalles, crée une **Passerelle Fluide en Verre Dépoli** lors du passage du curseur d'un moniteur à l'autre, et intègre la **Capsule Liquid Glass** — un hub interactif à ménisque liquide regroupant lecteur audio universel, minuteur Focus, mascottes kawaii animées et dialogue IA.

---

## 🌟 Nouveautés de la v2.2

- 🫧 **Capsule en Véritable « Liquid Glass »** :
  - Rendu multi-couches exclusif : base obsidienne liquide, ménisque supérieur spéculaire en goutte d'eau, diffusion caustique interne et liseré de réfraction chromatique 1.25px.
  - Dynamique d'expansion organique avec physique d'amortissement souple (*Spring Damping*) : ouverture rapide et fermeture progressive et soyeuse.
  - Onglets réorganisés avec icônes vives et typographie soignée : `🎵 Audio`, `⏱️ Focus`, `🐾 Mascotte`, `✨ IA`, `⚙ Paramètres`.

- 🐾 **Mascottes Kawaii Ultra-Mignonnes & Évolutives** :
  - **Expressions Kawaii complètes** : Joues roses délicates fardées, grands yeux anime étincelants avec reflet principal et petite étincelle secondaire, clignement en arcs de sourire joyeux `( ˘ ◡ ˘ )`.
  - **Interactions affectueuses** : Bouton **« 💖 Faire un Câlin (+20 XP) »** déclenchant une envolée de cœurs flottants `♥`, un bond d'excitation et des répliques chaleureuses.
  - **Détails vivants pour chaque mascotte** :
    - ✨ **Sparky / Lumina** : Petite fée stellaire avec ailes de cristal battantes, halo doré et poussière d'étoiles.
    - 🦊 **Yume / Kitsune** : Renardeau céleste aux grandes oreilles douces à coussinets roses et queue ondoyante.
    - 🐲 **Puff / Draco** : Bébé dragon de cristal avec petites cornes, ailes battantes et ventre pastel doux.
    - 👻 **Boo / Nyx** : Petit fantôme marshmallow à collerette vaporeuse et 3 perles quantiques lumineuses en orbite.
    - 🐾 **Mochi / Kuro-Neko** : Chaton noir tout rond avec moustaches fines blanches, queue enroulée et bouche en `:3`.
    - 🤖 **Pip / Axiom** : Mini robot Tamagotchi avec deux petites antennes flexibles et visière OLED affichant des émoticônes vivantes `(^ ‿ ^)`, `(♥ ‿ ♥)`, `(> ‿ <)`.

- 🎨 **Interface des Paramètres Redessinée & Rubriques Renommées** :
  - `✦ Dalles & Courbures` : Rayon millimétrique, courbures hyper-continues et aperçu spéculaire en direct.
  - `🌊 Passerelle Fluide` : Transitions inter-écrans liquides, effets magnétiques et 12 palettes chromatiques.
  - `🫧 Capsule Liquid Glass` : Ménisque liquide, apparition cinématique, suivi d'écran et studio audio.
  - `🐾 Mascottes Kawaii` : Galerie des mascottes, échelles, postures, réactivité oculaire et câlins XP.
  - `✨ Intelligence & Dialogue` : Connecteurs directs Gemini, Claude, ChatGPT et Mistral.

- 🛡️ **Zéro-Scintillement & Apple Music** :
  - Double-buffering matériel natif et style `WS_EX_COMPOSITED` garantissant une fluidité absolue à 60 FPS sans aucun scintillement.
  - Détection automatique et couleur signature d'Apple Music, Spotify, YouTube Music, Deezer, Tidal, SoundCloud, VLC.
  - Animation festive de notes musicales flottantes (*♪ ♫ ♬*) lors des changements de chanson.
  - Morphing adaptatif en mode plein écran (ruban d'ambiance ultra-fin de 2 px).

---

## Installation & Exécution Rapide

1. Téléchargez **`iCornerDuo.exe`** ou l'archive zip depuis la section **[Releases](https://github.com/flobure2008-ui/iCorner-Duo/releases)**.
2. Lancez `iCornerDuo.exe` — exécutable portable sans dépendance externe.
3. Survolez la capsule en haut de l'écran pour explorer les onglets, ou cliquez sur `⚙` pour ouvrir le studio complet.

### Compilation depuis la source (PowerShell)

```powershell
& "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /target:winexe /win32icon:"icon.ico" /out:"iCornerDuo.exe" /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll "Program.cs"
```

---

## Licence

Distribué sous licence MIT.