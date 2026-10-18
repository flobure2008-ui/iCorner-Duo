# iCorner Duo — Édition Glassmorphism & Minimaliste (v2.0)

<p align="center">
  <img src="assets/showcase_icon.jpg" alt="iCorner Duo Icon" width="220" style="border-radius: 36px;" />
</p>

**iCorner Duo** est un studio visuel ultra-léger en **Glassmorphisme minimaliste** pour Windows 10 et Windows 11. Il sculpte des coins d'écran arrondis haute précision sur toutes vos dalles, crée un **Pont Liquide en Verre Dépoli** lors du passage de la souris d'un moniteur à l'autre, et intègre la **Capsule Nova** — un hub interactif en verre givré regroupant lecteur audio universel, minuteur Focus, compagnon minimaliste animé et assistant IA multi-modèles.

---

## Nouveautés de la v2.0 (Refonte Glassmorphisme & Minimaliste)

- **Architecture Zéro-Scintillement & Optimisation CPU/GPU** :
  - Contrôles sur mesure en verre dépoli (`GlassCardPanel`, `GlassToggleSwitch`, `GlassSlider`) avec double-buffering matériel intégral.
  - Mise en cache matricielle des coins statiques (**0 % CPU au repos** en mode *Verre Minimal*).
  - Analyse des fenêtres multimédias déportée sur un thread d'arrière-plan asynchrone sans blocage du rendu 60 FPS.
- **Identité 100 % Originale & Minimaliste** :
  - Courbes géométriques exclusives : **Courbe Hyper-Continue** ($|x|^n + |y|^n = 1$) et **Arc Géométrique Pur**.
  - Palettes lumineuses en verre dépoli : **Prisme Éther**, **Verre Givre**, **Aurore Boréale**, **Néon Cyber**, **Ambre Solaire**, **Océan Abysses**, **Brume Rose**, **Émeraude Zen**.
- **Capsule Nova (Hub Interactif Supérieur à 4 Onglets)** :
  - **Lecteur Audio Universel** : Détection automatique des titres sur *Spotify, YouTube, YouTube Music, Deezer, SoundCloud, Tidal, VLC*, égaliseur spectral fluide et contrôles média globaux.
  - **Minuteur Focus & Chronomètre** : Anneau de progression lumineux, préréglages rapides (+1m, +5m, 25m Focus) et alerte visuelle/sonore.
  - **Compagnon Glassmorphique Minimaliste** : 6 entités géométriques en verre lumineux (**Lumina**, **Kitsune-V**, **Aether-Draco**, **Nyx-Specter**, **Kuro-Neko**, **Axiom-Core**) avec suivi oculaire fluide du curseur, humeurs dynamiques et bulles de dialogue sans aucune troncature.
  - **Assistant IA Multi-Fournisseurs** : Connexion directe via clé API à **Google Gemini**, **Anthropic Claude**, **OpenAI ChatGPT** et **Mistral AI**.
- **5 Animations d'Apparition & Visibilité Contextuelle** :
  - *Goutte Liquide*, *Déploiement Verre*, *Rebond Élastique*, *Glissement Supérieur*, *Flash Quantique*.
  - Modes *Toujours visible*, *Contextuel (Audio / Minuteur / Alerte)* ou *Survol du bord supérieur uniquement*.

---

## Installation & Exécution Rapide

1. Téléchargez **`iCornerDuo.exe`** (ou l'archive `iCornerDuo-v2.0.0-Windows.zip`) depuis la section **[Releases](https://github.com/flobure2008-ui/iCorner-Duo/releases)**.
2. Lancez `iCornerDuo.exe` — aucune installation complexe n'est requise.
3. Cliquez sur la **Capsule Nova** en haut de l'écran ou sur l'icône dans la barre des tâches (près de l'horloge) pour ouvrir le **Studio Glassmorphism**.

### Compilation depuis la source (PowerShell)

```powershell
& "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /target:winexe /win32icon:"icon.ico" /out:"iCornerDuo.exe" /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll "Program.cs"
```

---

## Licence

Distribué sous licence MIT.