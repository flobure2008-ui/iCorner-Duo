# iCorner Duo — Édition Glassmorphism & Minimaliste (v2.1)

<p align="center">
  <img src="assets/showcase_icon.jpg" alt="iCorner Duo Icon" width="220" style="border-radius: 36px;" />
</p>

**iCorner Duo** est un studio visuel ultra-léger en **Glassmorphisme minimaliste** pour Windows 10 et Windows 11. Il sculpte des coins d'écran arrondis haute précision sur toutes vos dalles, crée un **Pont Liquide en Verre Dépoli** lors du passage de la souris d'un moniteur à l'autre, et intègre la **Capsule Nova** — un hub interactif en verre givré regroupant lecteur audio universel, minuteur Focus, compagnon minimaliste animé et assistant IA multi-modèles.

---

## Nouveautés de la v2.1

- 🛡️ **Zéro-Scintillement Intégral & Stabilité DWM** :
  - Élimination des clignotements grâce au double-buffering matériel natif et au style étendu `WS_EX_COMPOSITED` (0x02000000).
  - Transition douce avec hystérésis temporelle intelligente de 2,5 s pour éviter toute coupure lors des blancs entre deux morceaux.
  - Fenêtre des paramètres stylisée avec barre de titre sombre et angles adoucis Windows 11 (`DwmSetWindowAttribute`).
- 🎵 **Support Complet d'Apple Music** :
  - Détection automatique d'**Apple Music** (application Windows Store UWP, client iTunes et version web).
  - Halo lumineux signature rose-rouge néon (`#FA2D55`) et raccourci de lancement / bascule rapide depuis la capsule.
- 🏷️ **Badge d'App Source Détectée** :
  - Affichage dynamique du badge de l'application en cours d'écoute (*Apple Music, Spotify, YouTube Music, Deezer, Tidal, SoundCloud, VLC, Lecteur Windows*).
- ✨ **Animation Festive au Changement de Piste** :
  - Envolée de notes de musique flottantes (*♪ ♫ ♬*) et impulsion ondulatoire néon réactive lors de chaque changement de chanson.
  - Glissement horizontal cinématique du titre du morceau.
- 🖥️ **Morphing Plein Écran Adaptatif (Mode Immersif)** :
  - Détection automatique des jeux et vidéos plein écran (F11, YouTube plein écran, etc.).
  - Transformation cinématique de la Capsule en un **Ruban Lumineux d'Ambiance ultra-fin** (2 px) longeant la bordure haute, réagissant discrètement au rythme audio.
  - Geste de survol « Peek » : amenez simplement le curseur sur le bord supérieur pour faire réapparaître instantanément la capsule complète.

---

## Fonctionnalités Principales

- **Coins d'Écran Haute Précision** :
  - Courbes géométriques : **Courbe Hyper-Continue** ($|x|^n + |y|^n = 1$) et **Arc Géométrique Pur**.
  - Rayon et épaisseur entièrement ajustables par écran.
  - 8 Thèmes en verre dépoli : *Prisme Éther, Verre Givre, Aurore Boréale, Néon Cyber, Ambre Solaire, Océan Abysses, Brume Rose, Émeraude Zen*.
- **Capsule Nova (Hub Interactif Supérieur)** :
  - **Lecteur Audio Universel** : Spectre audio en direct, contrôles Lecture/Pause/Suivant/Précédent, synchronisation fluide avec tous vos services de streaming.
  - **Minuteur Focus & Chronomètre** : Anneau de progression lumineux, préréglages rapides (+1m, +5m, 25m Focus).
  - **Compagnons Minimalistes** : 6 entités géométriques lumineuses animées (*Lumina, Kitsune-V, Aether-Draco, Nyx-Specter, Kuro-Neko, Axiom-Core*) avec regard interactif suivant la souris.
  - **Assistant IA Multi-Modèles** : Intégration directe via clé API pour **Google Gemini**, **Anthropic Claude**, **OpenAI ChatGPT** et **Mistral AI**.
- **Transitions Multi-Écrans Liquides** :
  - Pont visuel fluide et dynamique entre deux écrans lors du passage du curseur.

---

## Installation & Exécution Rapide

1. Téléchargez **`iCornerDuo.exe`** (ou l'archive zip) depuis la section **[Releases](https://github.com/flobure2008-ui/iCorner-Duo/releases)**.
2. Lancez `iCornerDuo.exe` — aucune installation n'est requise.
3. Cliquez sur la **Capsule Nova** en haut de l'écran ou sur l'icône dans la zone de notification (systray) pour accéder aux réglages.

### Compilation depuis la source (PowerShell)

```powershell
& "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /target:winexe /win32icon:"icon.ico" /out:"iCornerDuo.exe" /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll "Program.cs"
```

---

## Licence

Distribué sous licence MIT.