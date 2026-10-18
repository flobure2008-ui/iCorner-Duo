# iCorner Duo — Studio Aura Glass & Mascottes Célestes (v2.3)

<p align="center">
  <img src="assets/showcase_icon.jpg" alt="iCorner Duo Icon" width="220" style="border-radius: 36px;" />
</p>

**iCorner Duo** est un studio visuel ultra-léger en **Liquid Glass & Glassmorphisme** pour Windows 10 et Windows 11. Il sculpte des coins d'écran arrondis haute précision sur toutes vos dalles, crée une **Passerelle Fluide en Verre Dépoli** lors du passage du curseur d'un moniteur à l'autre, et intègre la **Capsule Aura Glass** — un bijou d'ingénierie optique à ménisque liquide regroupant lecteur audio universel, minuteur Focus, mascottes célestes en verre sculpté et dialogue IA.

---

## 🌟 Nouveautés de la v2.3

- 💧 **Refonte Complète : Capsule « Aura Glass »** :
  - **Optique Liquid Glass Pure** : Rendu multi-couches exclusif avec base obsidienne liquide, ménisque spéculaire en goutte d'eau, diffusion caustique interne et liseré de réfraction chromatique 1.25px.
  - **Dynamique d'expansion organique** avec physique d'amortissement souple (*Spring Damping*) : ouverture rapide et fermeture progressive et soyeuse.
  - Onglets réorganisés avec icônes vives et typographie soignée : `🎵 Audio`, `⏱️ Focus`, `✨ Mascotte`, `🔮 IA`, `⚙ Paramètres`.

- ✨ **Mascottes Célestes en Verre Ciselé (Design Haute Joaillerie)** :
  - Métamorphose artistique complète : adieu les dessins approximatifs, place à de superbes **sculptures célestes en cristal et verre soufflé** aux reflets spectraux et aux yeux en nébuleuse cosmique :
    - ✨ **Lumina** : Sphère d'énergie stellaire ceinte de doubles anneaux gyroscopiques 3D en or blanc et platine avec micro-étoiles en diamant orbitantes.
    - 🦊 **Kitsune** : Renard boréal avec panache de lumière arctique ondoyante en soie vaporeuse, oreilles en quartz aurore et diadème frontal céleste.
    - 💎 **Draco** : Wyverne géométrique taillée en diamant avec facettes de cristal prismatique, cornes en quartz pur et cœur en rubis incandescent.
    - 🫧 **Nyx** : Goutte d'opale stellaire iridescente en apesanteur avec trois perles planétaires en spirale logarithmique d'or.
    - 🐾 **Kuro-Neko** : Silhouette féline en verre fumé d'obsidienne pure avec intérieur d'oreilles en améthyste précieuse, halo de poussière astrale et yeux galaxie profonds.
    - 🔮 **Axiom** : Sphère en saphir poli enchâssée dans une monture en titane brossé avec anneau équatorial gradué holographique et prisme optique réactif.

- 🔲 **Arrondis Harmonieux Intégraux dans l'Interface des Paramètres** :
  - **Zéro angle droit** : Tous les champs de saisie de texte (`GlassRoundedInput`), listes déroulantes (`GlassRoundedComboBox`), conteneurs d'aperçu temps réel (`GlassRoundedBox`), panneaux de statut et cartes (`GlassCardPanel`) bénéficient désormais de rayons de courbure ultra-doux (14px à 20px) avec liseré de verre biseauté et reflets spéculaires.
  - Interface douce, cohérente et épurée digne d'un studio de design professionnel.

- 🎨 **Rubriques de Navigation Renommées** :
  - `✦ Dalles & Courbures` : Rayon millimétrique, courbures hyper-continues et aperçu spéculaire en direct.
  - `🌊 Passerelle Fluide` : Transitions inter-écrans liquides, effets magnétiques et 12 palettes chromatiques.
  - `💧 Capsule Aura Glass` : Ménisque liquide, cinématiques d'apparition, suivi d'écran et studio audio.
  - `✨ Mascottes Célestes` : Miroir astral, sélection d'entités, réactivité oculaire et résonances.
  - `🔮 Intelligence & Dialogue` : Connecteurs directs Gemini, Claude, ChatGPT et Mistral.

- 🛡️ **Zéro-Scintillement & Compatibilité Audio Maximale** :
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