using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace iCornerDuo
{
    // =========================================================================
    // 100% ORIGINAL GLASSMORPHIC & MINIMALIST ENUMS (NO APPLE REFERENCES)
    // =========================================================================
    public enum CurveGeometry
    {
        HyperbolicSquircle = 0,     // Courbe hyper-continue douce (n=4.2)
        ClassicCircular = 1         // Arc circulaire géométrique (n=2.0)
    }

    public enum CornerEffectMode
    {
        MinimalFrost = 0,           // Liseré verre dépoli statique (0% CPU au repos)
        PrismaticFlow = 1,          // Flux prismatique lent et fluide
        SoftBreathing = 2,          // Respiration atmosphérique douce
        AudioPulseSync = 3,         // Pulsation synchronisée sur le son
        StarlightDust = 4,          // Lueur stellaire subtile
        HorizonWave = 5             // Onde boréale décalée
    }

    public enum PortalTransitionStyle
    {
        GlassMorphCombo = 0,        // Pont Liquide Verre + Aura Prismatique + Particules
        LiquidGlassBridge = 1,      // Goutte liquide minimaliste entre les deux écrans
        PrismaticAuraWave = 2,      // Halo atmosphérique diffus sur la bordure
        QuantumArcPortal = 3,       // Filament énergétique fin et épuré
        StardustDrift = 4,          // Nuage de micro-particules lumineuses
        RippleResonance = 5,        // Ondes concentriques minimalistes
        LaserHorizonLine = 6,       // Ligne optique fine haute précision
        CapsulePulseOnly = 7        // Focus sur la Capsule Nova et les angles
    }

    public enum GlassColorPalette
    {
        AetherPrism = 0,            // Cyan Glacé, Lavande, Rose Pâle, Ambre Doux
        FrostWhiteSilver = 1,       // Verre Blanc Pur, Argent, Gris Cristal
        OceanicCyan = 2,            // Bleu Azur, Cyan Polaire, Indigo Profond
        EmeraldMint = 3,            // Vert Émeraude, Menthe Givrée, Jade
        CrimsonVelvet = 4,          // Rose Rubis, Corail Doux, Pourpre
        SolarAmber = 5,             // Or Doux, Ambre Chaud, Cuivre
        BorealAurora = 6,           // Vert Boréal, Turquoise, Violet Nuit
        UltravioletMist = 7,        // Violet Brumeux, Bleu Cosmique, Rose Pastel
        SakuraBlossom = 8,          // Rose Poudré, Lavande Douce, Ivoire
        ObsidianMonochrome = 9,     // Gris Titane, Platine, Noir Mat
        ChromaticSpectrum = 10,     // Spectre fluide lent 360°
        CustomGlassPalette = 11     // Palette personnalisée à 2 teintes
    }

    public enum CapsuleVisibilityMode
    {
        AlwaysPresent = 0,          // Toujours visible en haut de l'écran
        TopEdgeHoverWake = 1,       // Apparaît uniquement quand la souris touche le bord haut
        SmartContextual = 2,        // Apparaît si musique, minuteur, compagnon ou transition
        TransitionOnly = 3,         // Uniquement lors d'un passage d'écran
        Hidden = 4                  // Désactivé
    }

    public enum CapsuleSpawnStyle
    {
        LiquidGlassDrop = 0,        // Descente fluide avec élasticité douce
        HorizontalUnfold = 1,       // Déploiement horizontal minimaliste
        SoftFadeScale = 2,          // Fondu optique et zoom subtil
        PrismSnap = 3,              // Ouverture optique instantanée
        HaloCondense = 4            // Condensation lumineuse
    }

    public enum CapsuleScreenPlacement
    {
        FollowActiveCursor = 0,     // Glisse vers l'écran actif du curseur
        PrimaryMonitorFixed = 1     // Fixé sur le moniteur principal
    }

    public enum CapsuleBorderStyle
    {
        FrostedGlassRim = 0,        // Liseré verre dépoli spéculaire (Minimaliste)
        PrismaticRotatingRim = 1,   // Liseré prismatique rotatif doux
        AudioReactiveRim = 2,       // Liseré réactif au spectre audio
        PureMatteObsidian = 3       // Noir mat sans liseré externe
    }

    public enum CapsuleTab
    {
        AudioPlayer = 0,
        FocusTimer = 1,
        Companion = 2,
        AiBrain = 3
    }

    public enum CompanionEntity
    {
        None = 0,
        LuminaOrb = 1,              // Orbe Céleste en Verre Dépoli & Anneau Orbital
        KitsuneMinimal = 2,         // Esprit Renard Géométrique Néo-Minimaliste
        DracoCrystal = 3,           // Gardien Wyvern de Cristal Translucide
        NyxQuantum = 4,             // Entité Quantique & Satellites Lumineux
        KuroNeko = 5,               // Félin Bionique Minimaliste Mat
        AxiomPrism = 6              // Prisme Octaédrique IA en Lévitation
    }

    public enum CompanionAnchor
    {
        FloatingRight = 0,          // En lévitation douce à droite de la capsule
        PerchedOnTop = 1,           // Posé au-dessus de la capsule
        IntegratedInside = 2        // Intégré à gauche dans la capsule compacte
    }

    public enum AiService
    {
        Gemini = 0,
        Claude = 1,
        ChatGPT = 2,
        Mistral = 3
    }

    public enum AudioAppSource
    {
        None = 0,
        Spotify = 1,
        WebMusic = 2,
        YouTube = 3,
        DesktopPlayer = 4,
        SystemSound = 5,
        AppleMusic = 6,
        Deezer = 7,
        SoundCloud = 8,
        Tidal = 9,
        VLC = 10,
        AmazonMusic = 11,
        MediaPlayer = 12
    }

    // =========================================================================
    // PERSISTENT SETTINGS
    // =========================================================================
    public class AppSettings
    {
        // 1. Corners
        public bool Enabled = true;
        public int CornerRadius = 36;
        public CurveGeometry Curve = CurveGeometry.HyperbolicSquircle;
        public bool TopLeft = true;
        public bool TopRight = true;
        public bool BottomLeft = true;
        public bool BottomRight = true;
        public bool CornerBezelGlow = true;
        public int BezelGlowOpacity = 48;
        public int BezelThickness = 18; // 1.8px default minimalist rim
        public CornerEffectMode CornerAnim = CornerEffectMode.MinimalFrost;
        public int CornerAnimSpeed = 85;

        // 2. Transitions & Glass Palettes
        public bool EnableTransition = true;
        public bool EnableMagneticProximity = true;
        public PortalTransitionStyle Style = PortalTransitionStyle.GlassMorphCombo;
        public GlassColorPalette Theme = GlassColorPalette.AetherPrism;
        public int DurationMs = 850;
        public int IntensityPercent = 100;
        public int CustomColor1Argb = Color.FromArgb(255, 110, 220, 255).ToArgb();
        public int CustomColor2Argb = Color.FromArgb(255, 185, 130, 255).ToArgb();

        // 3. Nova Capsule & Spawn System
        public CapsuleVisibilityMode IslandMode = CapsuleVisibilityMode.Hidden;
        public CapsuleSpawnStyle SpawnAnim = CapsuleSpawnStyle.LiquidGlassDrop;
        public CapsuleScreenPlacement IslandPlacement = CapsuleScreenPlacement.FollowActiveCursor;
        public CapsuleBorderStyle IslandBorder = CapsuleBorderStyle.FrostedGlassRim;
        public int IslandScalePercent = 100;
        public bool IslandExpandOnHover = true;
        public bool IslandShowClockWhenIdle = true;
        public bool IslandAutoAdaptBrandColor = true;
        public bool TrackChangeAnimation = true;
        public bool FullscreenMorph = true;
        public bool ShowDetectedAppBadge = true;

        // 4. Companion Settings
        public CompanionEntity Companion = CompanionEntity.LuminaOrb;
        public CompanionAnchor CompanionPos = CompanionAnchor.FloatingRight;
        public int CompanionScalePercent = 100;
        public bool CompanionFollowCursorEyes = true;
        public bool CompanionDanceToMusic = true;
        public bool CompanionShowSpeechBubbles = true;
        public string CompanionName = "Lumina";

        // 5. AI Multi-Provider Settings
        public AiService ActiveAiProvider = AiService.Gemini;
        public string GeminiApiKey = "";
        public string GeminiModel = "gemini-2.5-flash";
        public string ClaudeApiKey = "";
        public string ClaudeModel = "claude-3-5-sonnet-latest";
        public string OpenAiApiKey = "";
        public string OpenAiModel = "gpt-4o-mini";
        public string MistralApiKey = "";
        public string MistralModel = "mistral-small-latest";
        public string AiSystemPrompt = "Tu es Lumina, une intelligence minimaliste intégrée dans la barre Nova sur Windows. Réponds toujours en français de façon élégante, concise et utile (maximum 2 phrases courtes).";

        private static string SettingsPath
        {
            get
            {
                string dir = Path.GetDirectoryName(Application.ExecutablePath);
                return Path.Combine(dir, "settings.json");
            }
        }

        public void Save()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("{");
                sb.AppendFormat("  \"Enabled\": {0},\n", Enabled.ToString().ToLower());
                sb.AppendFormat("  \"CornerRadius\": {0},\n", CornerRadius);
                sb.AppendFormat("  \"Curve\": {0},\n", (int)Curve);
                sb.AppendFormat("  \"TopLeft\": {0},\n", TopLeft.ToString().ToLower());
                sb.AppendFormat("  \"TopRight\": {0},\n", TopRight.ToString().ToLower());
                sb.AppendFormat("  \"BottomLeft\": {0},\n", BottomLeft.ToString().ToLower());
                sb.AppendFormat("  \"BottomRight\": {0},\n", BottomRight.ToString().ToLower());
                sb.AppendFormat("  \"CornerBezelGlow\": {0},\n", CornerBezelGlow.ToString().ToLower());
                sb.AppendFormat("  \"BezelGlowOpacity\": {0},\n", BezelGlowOpacity);
                sb.AppendFormat("  \"BezelThickness\": {0},\n", BezelThickness);
                sb.AppendFormat("  \"CornerAnim\": {0},\n", (int)CornerAnim);
                sb.AppendFormat("  \"CornerAnimSpeed\": {0},\n", CornerAnimSpeed);
                sb.AppendFormat("  \"EnableTransition\": {0},\n", EnableTransition.ToString().ToLower());
                sb.AppendFormat("  \"EnableMagneticProximity\": {0},\n", EnableMagneticProximity.ToString().ToLower());
                sb.AppendFormat("  \"Style\": {0},\n", (int)Style);
                sb.AppendFormat("  \"Theme\": {0},\n", (int)Theme);
                sb.AppendFormat("  \"DurationMs\": {0},\n", DurationMs);
                sb.AppendFormat("  \"IntensityPercent\": {0},\n", IntensityPercent);
                sb.AppendFormat("  \"CustomColor1Argb\": {0},\n", CustomColor1Argb);
                sb.AppendFormat("  \"CustomColor2Argb\": {0},\n", CustomColor2Argb);
                sb.AppendFormat("  \"IslandMode\": {0},\n", (int)IslandMode);
                sb.AppendFormat("  \"SpawnAnim\": {0},\n", (int)SpawnAnim);
                sb.AppendFormat("  \"IslandPlacement\": {0},\n", (int)IslandPlacement);
                sb.AppendFormat("  \"IslandBorder\": {0},\n", (int)IslandBorder);
                sb.AppendFormat("  \"IslandScalePercent\": {0},\n", IslandScalePercent);
                sb.AppendFormat("  \"IslandExpandOnHover\": {0},\n", IslandExpandOnHover.ToString().ToLower());
                sb.AppendFormat("  \"IslandShowClockWhenIdle\": {0},\n", IslandShowClockWhenIdle.ToString().ToLower());
                sb.AppendFormat("  \"IslandAutoAdaptBrandColor\": {0},\n", IslandAutoAdaptBrandColor.ToString().ToLower());
                sb.AppendFormat("  \"TrackChangeAnimation\": {0},\n", TrackChangeAnimation.ToString().ToLower());
                sb.AppendFormat("  \"FullscreenMorph\": {0},\n", FullscreenMorph.ToString().ToLower());
                sb.AppendFormat("  \"ShowDetectedAppBadge\": {0},\n", ShowDetectedAppBadge.ToString().ToLower());
                sb.AppendFormat("  \"Companion\": {0},\n", (int)Companion);
                sb.AppendFormat("  \"CompanionPos\": {0},\n", (int)CompanionPos);
                sb.AppendFormat("  \"CompanionScalePercent\": {0},\n", CompanionScalePercent);
                sb.AppendFormat("  \"CompanionFollowCursorEyes\": {0},\n", CompanionFollowCursorEyes.ToString().ToLower());
                sb.AppendFormat("  \"CompanionDanceToMusic\": {0},\n", CompanionDanceToMusic.ToString().ToLower());
                sb.AppendFormat("  \"CompanionShowSpeechBubbles\": {0},\n", CompanionShowSpeechBubbles.ToString().ToLower());
                sb.AppendFormat("  \"CompanionName\": \"{0}\",\n", EscapeJson(CompanionName));
                sb.AppendFormat("  \"ActiveAiProvider\": {0},\n", (int)ActiveAiProvider);
                sb.AppendFormat("  \"GeminiApiKey\": \"{0}\",\n", EscapeJson(GeminiApiKey));
                sb.AppendFormat("  \"GeminiModel\": \"{0}\",\n", EscapeJson(GeminiModel));
                sb.AppendFormat("  \"ClaudeApiKey\": \"{0}\",\n", EscapeJson(ClaudeApiKey));
                sb.AppendFormat("  \"ClaudeModel\": \"{0}\",\n", EscapeJson(ClaudeModel));
                sb.AppendFormat("  \"OpenAiApiKey\": \"{0}\",\n", EscapeJson(OpenAiApiKey));
                sb.AppendFormat("  \"OpenAiModel\": \"{0}\",\n", EscapeJson(OpenAiModel));
                sb.AppendFormat("  \"MistralApiKey\": \"{0}\",\n", EscapeJson(MistralApiKey));
                sb.AppendFormat("  \"MistralModel\": \"{0}\",\n", EscapeJson(MistralModel));
                sb.AppendFormat("  \"AiSystemPrompt\": \"{0}\"\n", EscapeJson(AiSystemPrompt));
                sb.AppendLine("}");
                File.WriteAllText(SettingsPath, sb.ToString(), Encoding.UTF8);
            }
            catch { }
        }

        public static AppSettings Load()
        {
            AppSettings s = new AppSettings();
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath, Encoding.UTF8);
                    s.Enabled = ReadBool(json, "Enabled", s.Enabled);
                    s.CornerRadius = ClampInt(ReadInt(json, "CornerRadius", s.CornerRadius), 0, 110);
                    s.Curve = (CurveGeometry)ClampInt(ReadInt(json, "Curve", (int)s.Curve), 0, 1);
                    s.TopLeft = ReadBool(json, "TopLeft", s.TopLeft);
                    s.TopRight = ReadBool(json, "TopRight", s.TopRight);
                    s.BottomLeft = ReadBool(json, "BottomLeft", s.BottomLeft);
                    s.BottomRight = ReadBool(json, "BottomRight", s.BottomRight);
                    s.CornerBezelGlow = ReadBool(json, "CornerBezelGlow", s.CornerBezelGlow);
                    s.BezelGlowOpacity = ClampInt(ReadInt(json, "BezelGlowOpacity", s.BezelGlowOpacity), 0, 100);
                    s.BezelThickness = ClampInt(ReadInt(json, "BezelThickness", s.BezelThickness), 10, 75);
                    s.CornerAnim = (CornerEffectMode)ClampInt(ReadInt(json, "CornerAnim", (int)s.CornerAnim), 0, 5);
                    s.CornerAnimSpeed = ClampInt(ReadInt(json, "CornerAnimSpeed", s.CornerAnimSpeed), 25, 250);
                    s.EnableTransition = ReadBool(json, "EnableTransition", s.EnableTransition);
                    s.EnableMagneticProximity = ReadBool(json, "EnableMagneticProximity", s.EnableMagneticProximity);
                    s.Style = (PortalTransitionStyle)ClampInt(ReadInt(json, "Style", (int)s.Style), 0, 7);
                    s.Theme = (GlassColorPalette)ClampInt(ReadInt(json, "Theme", (int)s.Theme), 0, 11);
                    s.DurationMs = ClampInt(ReadInt(json, "DurationMs", s.DurationMs), 350, 2000);
                    s.IntensityPercent = ClampInt(ReadInt(json, "IntensityPercent", s.IntensityPercent), 60, 190);
                    s.CustomColor1Argb = ReadInt(json, "CustomColor1Argb", s.CustomColor1Argb);
                    s.CustomColor2Argb = ReadInt(json, "CustomColor2Argb", s.CustomColor2Argb);
                    s.IslandMode = (CapsuleVisibilityMode)ClampInt(ReadInt(json, "IslandMode", (int)s.IslandMode), 0, 4);
                    s.SpawnAnim = (CapsuleSpawnStyle)ClampInt(ReadInt(json, "SpawnAnim", (int)s.SpawnAnim), 0, 4);
                    s.IslandPlacement = (CapsuleScreenPlacement)ClampInt(ReadInt(json, "IslandPlacement", (int)s.IslandPlacement), 0, 1);
                    s.IslandBorder = (CapsuleBorderStyle)ClampInt(ReadInt(json, "IslandBorder", (int)s.IslandBorder), 0, 3);
                    s.IslandScalePercent = ClampInt(ReadInt(json, "IslandScalePercent", s.IslandScalePercent), 75, 150);
                    s.IslandExpandOnHover = ReadBool(json, "IslandExpandOnHover", s.IslandExpandOnHover);
                    s.IslandShowClockWhenIdle = ReadBool(json, "IslandShowClockWhenIdle", s.IslandShowClockWhenIdle);
                    s.IslandAutoAdaptBrandColor = ReadBool(json, "IslandAutoAdaptBrandColor", s.IslandAutoAdaptBrandColor);
                    s.TrackChangeAnimation = ReadBool(json, "TrackChangeAnimation", s.TrackChangeAnimation);
                    s.FullscreenMorph = ReadBool(json, "FullscreenMorph", s.FullscreenMorph);
                    s.ShowDetectedAppBadge = ReadBool(json, "ShowDetectedAppBadge", s.ShowDetectedAppBadge);
                    s.Companion = (CompanionEntity)ClampInt(ReadInt(json, "Companion", (int)s.Companion), 0, 6);
                    s.CompanionPos = (CompanionAnchor)ClampInt(ReadInt(json, "CompanionPos", (int)s.CompanionPos), 0, 2);
                    s.CompanionScalePercent = ClampInt(ReadInt(json, "CompanionScalePercent", s.CompanionScalePercent), 70, 165);
                    s.CompanionFollowCursorEyes = ReadBool(json, "CompanionFollowCursorEyes", s.CompanionFollowCursorEyes);
                    s.CompanionDanceToMusic = ReadBool(json, "CompanionDanceToMusic", s.CompanionDanceToMusic);
                    s.CompanionShowSpeechBubbles = ReadBool(json, "CompanionShowSpeechBubbles", s.CompanionShowSpeechBubbles);
                    s.CompanionName = ReadString(json, "CompanionName", s.CompanionName);
                    s.ActiveAiProvider = (AiService)ClampInt(ReadInt(json, "ActiveAiProvider", (int)s.ActiveAiProvider), 0, 3);
                    s.GeminiApiKey = ReadString(json, "GeminiApiKey", s.GeminiApiKey);
                    s.GeminiModel = ReadString(json, "GeminiModel", s.GeminiModel);
                    s.ClaudeApiKey = ReadString(json, "ClaudeApiKey", s.ClaudeApiKey);
                    s.ClaudeModel = ReadString(json, "ClaudeModel", s.ClaudeModel);
                    s.OpenAiApiKey = ReadString(json, "OpenAiApiKey", s.OpenAiApiKey);
                    s.OpenAiModel = ReadString(json, "OpenAiModel", s.OpenAiModel);
                    s.MistralApiKey = ReadString(json, "MistralApiKey", s.MistralApiKey);
                    s.MistralModel = ReadString(json, "MistralModel", s.MistralModel);
                    s.AiSystemPrompt = ReadString(json, "AiSystemPrompt", s.AiSystemPrompt);
                }
            }
            catch { }
            return s;
        }

        private static int ClampInt(int v, int min, int max) { return v < min ? min : (v > max ? max : v); }
        private static string EscapeJson(string s) { return string.IsNullOrEmpty(s) ? "" : s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", " ").Replace("\n", " "); }

        private static bool ReadBool(string json, string key, bool def)
        {
            string pattern = "\"" + key + "\":";
            int idx = json.IndexOf(pattern, StringComparison.OrdinalIgnoreCase);
            if (idx < 0) return def;
            string sub = json.Substring(idx + pattern.Length).Trim();
            if (sub.StartsWith("true", StringComparison.OrdinalIgnoreCase)) return true;
            if (sub.StartsWith("false", StringComparison.OrdinalIgnoreCase)) return false;
            return def;
        }

        private static int ReadInt(string json, string key, int def)
        {
            string pattern = "\"" + key + "\":";
            int idx = json.IndexOf(pattern, StringComparison.OrdinalIgnoreCase);
            if (idx < 0) return def;
            string sub = json.Substring(idx + pattern.Length).Trim();
            int end = 0;
            while (end < sub.Length && (char.IsDigit(sub[end]) || sub[end] == '-')) end++;
            int val;
            if (end > 0 && int.TryParse(sub.Substring(0, end), out val)) return val;
            return def;
        }

        private static string ReadString(string json, string key, string def)
        {
            string pattern = "\"" + key + "\":";
            int idx = json.IndexOf(pattern, StringComparison.OrdinalIgnoreCase);
            if (idx < 0) return def;
            int firstQuote = json.IndexOf('"', idx + pattern.Length);
            if (firstQuote < 0) return def;
            StringBuilder sb = new StringBuilder();
            bool escaped = false;
            for (int i = firstQuote + 1; i < json.Length; i++)
            {
                char c = json[i];
                if (escaped) { sb.Append(c); escaped = false; }
                else if (c == '\\') { escaped = true; }
                else if (c == '"') { return sb.ToString(); }
                else { sb.Append(c); }
            }
            return def;
        }
    }

    // =========================================================================
    // GLASSMORPHISM RENDERING UTILITIES & PALETTE ENGINE
    // =========================================================================
    public static class GlassThemeEngine
    {
        public static Color[] GetPalette(AppSettings settings, float timeSeconds)
        {
            switch (settings.Theme)
            {
                case GlassColorPalette.FrostWhiteSilver:
                    return new Color[] { Color.FromArgb(255, 235, 242, 255), Color.FromArgb(255, 180, 195, 220), Color.FromArgb(255, 210, 220, 238), Color.FromArgb(255, 255, 255, 255) };
                case GlassColorPalette.OceanicCyan:
                    return new Color[] { Color.FromArgb(255, 70, 210, 255), Color.FromArgb(255, 50, 135, 245), Color.FromArgb(255, 100, 115, 240), Color.FromArgb(255, 150, 235, 255) };
                case GlassColorPalette.EmeraldMint:
                    return new Color[] { Color.FromArgb(255, 52, 215, 135), Color.FromArgb(255, 40, 185, 160), Color.FromArgb(255, 110, 245, 190), Color.FromArgb(255, 160, 255, 215) };
                case GlassColorPalette.CrimsonVelvet:
                    return new Color[] { Color.FromArgb(255, 245, 75, 115), Color.FromArgb(255, 215, 65, 165), Color.FromArgb(255, 255, 130, 150), Color.FromArgb(255, 175, 95, 235) };
                case GlassColorPalette.SolarAmber:
                    return new Color[] { Color.FromArgb(255, 255, 185, 75), Color.FromArgb(255, 245, 120, 65), Color.FromArgb(255, 255, 215, 130), Color.FromArgb(255, 235, 90, 90) };
                case GlassColorPalette.BorealAurora:
                    return new Color[] { Color.FromArgb(255, 75, 240, 185), Color.FromArgb(255, 70, 190, 245), Color.FromArgb(255, 145, 115, 245), Color.FromArgb(255, 160, 250, 225) };
                case GlassColorPalette.UltravioletMist:
                    return new Color[] { Color.FromArgb(255, 175, 115, 255), Color.FromArgb(255, 115, 110, 240), Color.FromArgb(255, 235, 115, 210), Color.FromArgb(255, 130, 200, 255) };
                case GlassColorPalette.SakuraBlossom:
                    return new Color[] { Color.FromArgb(255, 255, 170, 205), Color.FromArgb(255, 210, 170, 250), Color.FromArgb(255, 255, 205, 185), Color.FromArgb(255, 175, 225, 255) };
                case GlassColorPalette.ObsidianMonochrome:
                    return new Color[] { Color.FromArgb(255, 190, 198, 215), Color.FromArgb(255, 140, 148, 165), Color.FromArgb(255, 220, 225, 235), Color.FromArgb(255, 165, 172, 188) };
                case GlassColorPalette.ChromaticSpectrum:
                    return new Color[] {
                        FromHsv((timeSeconds * 55f) % 360f, 0.72f, 1.0f),
                        FromHsv((timeSeconds * 55f + 90f) % 360f, 0.72f, 1.0f),
                        FromHsv((timeSeconds * 55f + 180f) % 360f, 0.72f, 1.0f),
                        FromHsv((timeSeconds * 55f + 270f) % 360f, 0.72f, 1.0f)
                    };
                case GlassColorPalette.CustomGlassPalette:
                    Color c1 = Color.FromArgb(255, Color.FromArgb(settings.CustomColor1Argb));
                    Color c2 = Color.FromArgb(255, Color.FromArgb(settings.CustomColor2Argb));
                    return new Color[] { c1, c2, Interpolate(c1, c2, 0.5f), Interpolate(c1, Color.White, 0.4f) };
                case GlassColorPalette.AetherPrism:
                default:
                    return new Color[] {
                        Color.FromArgb(255, 110, 215, 255), // Frosted Ice Cyan
                        Color.FromArgb(255, 180, 130, 255), // Soft Lavender Violet
                        Color.FromArgb(255, 245, 125, 185), // Pastel Rose
                        Color.FromArgb(255, 255, 190, 120)  // Warm Champagne
                    };
            }
        }

        public static Color GetAudioSourceColor(AudioAppSource src, Color fallback)
        {
            switch (src)
            {
                case AudioAppSource.Spotify: return Color.FromArgb(255, 30, 215, 96);
                case AudioAppSource.AppleMusic: return Color.FromArgb(255, 250, 45, 85); // Vibrant Apple Music Pink/Red
                case AudioAppSource.YouTube: return Color.FromArgb(255, 255, 30, 45);
                case AudioAppSource.Deezer: return Color.FromArgb(255, 162, 56, 255);
                case AudioAppSource.SoundCloud: return Color.FromArgb(255, 255, 85, 0);
                case AudioAppSource.Tidal: return Color.FromArgb(255, 0, 235, 220);
                case AudioAppSource.VLC: return Color.FromArgb(255, 255, 136, 0);
                case AudioAppSource.AmazonMusic: return Color.FromArgb(255, 37, 211, 238);
                case AudioAppSource.MediaPlayer: return Color.FromArgb(255, 0, 120, 215);
                case AudioAppSource.WebMusic: return Color.FromArgb(255, 245, 90, 125);
                default: return fallback;
            }
        }

        public static Color Interpolate(Color a, Color b, float t)
        {
            if (t < 0f) t = 0f;
            if (t > 1f) t = 1f;
            return Color.FromArgb(
                (int)(a.A + (b.A - a.A) * t),
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t)
            );
        }

        public static Color FromHsv(float hue, float saturation, float value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);
            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));
            if (hi == 0) return Color.FromArgb(255, v, t, p);
            if (hi == 1) return Color.FromArgb(255, q, v, p);
            if (hi == 2) return Color.FromArgb(255, p, v, t);
            if (hi == 3) return Color.FromArgb(255, p, q, v);
            if (hi == 4) return Color.FromArgb(255, t, p, v);
            return Color.FromArgb(255, v, p, q);
        }

        // Draws a true minimalist frosted glass pill/card with inner sheen & 1px specular border
        public static void DrawFrostedGlassSurface(
            Graphics g, RectangleF rect, float cornerRadius,
            Color baseDarkTint, Color accentTint, int glassOpacity = 238, float borderAlphaRatio = 1.0f)
        {
            if (rect.Width <= 2f || rect.Height <= 2f) return;
            using (GraphicsPath path = CreateRoundedRect(rect, cornerRadius))
            {
                // 1. Deep Frosted Matte Base
                using (SolidBrush baseBrush = new SolidBrush(Color.FromArgb(glassOpacity, baseDarkTint)))
                {
                    g.FillPath(baseBrush, path);
                }

                // 2. Subtle Diagonal Frosted Glass Sheen (Top-Left to Bottom-Right)
                using (LinearGradientBrush sheen = new LinearGradientBrush(
                    rect,
                    Color.FromArgb((int)(28 * borderAlphaRatio), 255, 255, 255),
                    Color.FromArgb((int)(12 * borderAlphaRatio), accentTint),
                    35f))
                {
                    g.FillPath(sheen, path);
                }

                // 3. Hairline 1.2px Specular Glass Rim (brighter top-left, softer bottom-right)
                using (LinearGradientBrush rimGrad = new LinearGradientBrush(
                    rect,
                    Color.FromArgb((int)(85 * borderAlphaRatio), Interpolate(Color.White, accentTint, 0.35f)),
                    Color.FromArgb((int)(28 * borderAlphaRatio), accentTint),
                    45f))
                using (Pen rimPen = new Pen(rimGrad, 1.2f))
                {
                    g.DrawPath(rimPen, path);
                }
            }
        }

        // Draws an ultra-luxurious Liquid Glass surface with droplet meniscus, caustic diffusion, and chromatic edge
        public static void DrawLiquidGlassSurface(
            Graphics g, RectangleF rect, float cornerRadius,
            Color baseDarkTint, Color accentTint, int glassOpacity = 242, float borderAlphaRatio = 1.0f, float globalTime = 0f)
        {
            if (rect.Width <= 2f || rect.Height <= 2f) return;
            using (GraphicsPath path = CreateRoundedRect(rect, cornerRadius))
            {
                // 1. Deep Obsidian / Liquid Crystal Base
                using (SolidBrush baseBrush = new SolidBrush(Color.FromArgb(glassOpacity, baseDarkTint)))
                {
                    g.FillPath(baseBrush, path);
                }

                // 2. Caustic ambient glow from inside (soft radial bloom)
                using (GraphicsPath innerCaustic = CreateRoundedRect(RectangleF.Inflate(rect, -2f, -2f), Math.Max(2f, cornerRadius - 2f)))
                using (LinearGradientBrush causticGrad = new LinearGradientBrush(
                    rect,
                    Color.FromArgb((int)(34 * borderAlphaRatio), accentTint),
                    Color.FromArgb(0, accentTint),
                    90f))
                {
                    g.FillPath(causticGrad, innerCaustic);
                }

                // 3. Liquid Droplet Meniscus (curved specular gloss highlight across top 45%)
                float hlHeight = Math.Min(rect.Height * 0.48f, 28f);
                if (hlHeight > 4f)
                {
                    RectangleF hlRect = new RectangleF(rect.X + 1.5f, rect.Y + 1.2f, rect.Width - 3f, hlHeight);
                    using (GraphicsPath hlPath = CreateRoundedRect(hlRect, Math.Min(cornerRadius, hlHeight / 2f)))
                    using (LinearGradientBrush hlBrush = new LinearGradientBrush(
                        hlRect,
                        Color.FromArgb((int)(68 * borderAlphaRatio), 255, 255, 255),
                        Color.FromArgb(0, 255, 255, 255),
                        90f))
                    {
                        g.FillPath(hlBrush, hlPath);
                    }
                }

                // 4. Subtle Bottom Refraction Bounce Line
                float botH = Math.Min(rect.Height * 0.25f, 10f);
                if (botH > 3f)
                {
                    RectangleF botRect = new RectangleF(rect.X + 4f, rect.Bottom - botH - 1.2f, rect.Width - 8f, botH);
                    using (GraphicsPath botPath = CreateRoundedRect(botRect, botH / 2f))
                    using (LinearGradientBrush botBrush = new LinearGradientBrush(
                        botRect,
                        Color.FromArgb(0, 255, 255, 255),
                        Color.FromArgb((int)(18 * borderAlphaRatio), Interpolate(Color.White, accentTint, 0.4f)),
                        90f))
                    {
                        g.FillPath(botBrush, botPath);
                    }
                }

                // 5. Chromatic Dispersion Hairline Rim (1.25px)
                using (LinearGradientBrush rimGrad = new LinearGradientBrush(
                    rect,
                    Color.FromArgb((int)(130 * borderAlphaRatio), Interpolate(Color.White, accentTint, 0.25f)),
                    Color.FromArgb((int)(38 * borderAlphaRatio), accentTint),
                    60f))
                using (Pen rimPen = new Pen(rimGrad, 1.25f))
                {
                    g.DrawPath(rimPen, path);
                }
            }
        }

        public static GraphicsPath CreateRoundedRect(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float d = radius * 2f;
            if (d > rect.Width) d = rect.Width;
            if (d > rect.Height) d = rect.Height;
            if (d <= 1.5f) { path.AddRectangle(rect); return path; }
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    // =========================================================================
    // MULTI-PROVIDER AI ASSISTANT ENGINE (GEMINI, CLAUDE, CHATGPT, MISTRAL)
    // =========================================================================
    public class AiAssistantEngine
    {
        public bool IsBusy = false;
        public string LastResponse = "Bonjour. Configurez votre clé API (Gemini, Claude, ChatGPT ou Mistral) ou posez-moi directement une question.";

        public AiAssistantEngine()
        {
            try { ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Tls; } catch { }
        }

        public void SendPromptAsync(AppSettings settings, string userPrompt, Action<string> callback = null)
        {
            if (IsBusy) return;
            IsBusy = true;
            LastResponse = "Réflexion en cours (" + settings.ActiveAiProvider + ")...";

            ThreadPool.QueueUserWorkItem(_ =>
            {
                string reply = QueryProviderSync(settings, userPrompt);
                IsBusy = false;
                LastResponse = reply;
                if (callback != null)
                {
                    try { callback(reply); } catch { }
                }
            });
        }

        private string QueryProviderSync(AppSettings s, string prompt)
        {
            try
            {
                string sys = string.IsNullOrEmpty(s.AiSystemPrompt)
                    ? "Tu es un assistant minimaliste et concis. Réponds en français en 1 à 2 phrases."
                    : s.AiSystemPrompt;

                switch (s.ActiveAiProvider)
                {
                    case AiService.Gemini:
                        return CallGemini(s.GeminiApiKey, s.GeminiModel, sys, prompt);
                    case AiService.Claude:
                        return CallClaude(s.ClaudeApiKey, s.ClaudeModel, sys, prompt);
                    case AiService.ChatGPT:
                        return CallOpenAiCompatible("https://api.openai.com/v1/chat/completions", s.OpenAiApiKey, s.OpenAiModel, sys, prompt, "ChatGPT");
                    case AiService.Mistral:
                        return CallOpenAiCompatible("https://api.mistral.ai/v1/chat/completions", s.MistralApiKey, s.MistralModel, sys, prompt, "Mistral");
                    default:
                        return "Service non configuré.";
                }
            }
            catch (WebException wex)
            {
                string errBody = "";
                try
                {
                    if (wex.Response != null)
                        using (StreamReader sr = new StreamReader(wex.Response.GetResponseStream(), Encoding.UTF8))
                            errBody = sr.ReadToEnd();
                }
                catch { }
                string msg = ExtractJsonField(errBody, "message");
                return !string.IsNullOrEmpty(msg) ? ("Erreur API : " + msg) : ("Erreur réseau : " + wex.Message);
            }
            catch (Exception ex)
            {
                return "Erreur : " + ex.Message;
            }
        }

        private string CallGemini(string apiKey, string model, string sysPrompt, string userPrompt)
        {
            if (string.IsNullOrEmpty(apiKey))
                return "Clé API Gemini requise (onglet Intelligence IA dans les paramètres).";
            string mdl = string.IsNullOrEmpty(model) ? "gemini-2.5-flash" : model.Trim();
            string url = "https://generativelanguage.googleapis.com/v1beta/models/" + mdl + ":generateContent?key=" + apiKey.Trim();
            string body = "{\"system_instruction\":{\"parts\":[{\"text\":\"" + EscapeJsonString(sysPrompt) + "\"}]},\"contents\":[{\"role\":\"user\",\"parts\":[{\"text\":\"" + EscapeJsonString(userPrompt) + "\"}]}]}";
            string resp = HttpPostJson(url, body, null);
            string text = ExtractJsonField(resp, "text");
            return string.IsNullOrEmpty(text) ? "Réponse reçue." : UnescapeJsonString(text);
        }

        private string CallClaude(string apiKey, string model, string sysPrompt, string userPrompt)
        {
            if (string.IsNullOrEmpty(apiKey))
                return "Clé API Claude requise (onglet Intelligence IA dans les paramètres).";
            string mdl = string.IsNullOrEmpty(model) ? "claude-3-5-sonnet-latest" : model.Trim();
            string body = "{\"model\":\"" + EscapeJsonString(mdl) + "\",\"max_tokens\":260,\"system\":\"" + EscapeJsonString(sysPrompt) + "\",\"messages\":[{\"role\":\"user\",\"content\":\"" + EscapeJsonString(userPrompt) + "\"}]}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers["x-api-key"] = apiKey.Trim();
            headers["anthropic-version"] = "2023-06-01";
            string resp = HttpPostJson("https://api.anthropic.com/v1/messages", body, headers);
            string text = ExtractJsonField(resp, "text");
            return string.IsNullOrEmpty(text) ? "Réponse reçue." : UnescapeJsonString(text);
        }

        private string CallOpenAiCompatible(string url, string apiKey, string model, string sysPrompt, string userPrompt, string label)
        {
            if (string.IsNullOrEmpty(apiKey))
                return "Clé API " + label + " requise (onglet Intelligence IA dans les paramètres).";
            string mdl = string.IsNullOrEmpty(model) ? (label == "Mistral" ? "mistral-small-latest" : "gpt-4o-mini") : model.Trim();
            string body = "{\"model\":\"" + EscapeJsonString(mdl) + "\",\"max_tokens\":260,\"messages\":[{\"role\":\"system\",\"content\":\"" + EscapeJsonString(sysPrompt) + "\"},{\"role\":\"user\",\"content\":\"" + EscapeJsonString(userPrompt) + "\"}]}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers["Authorization"] = "Bearer " + apiKey.Trim();
            string resp = HttpPostJson(url, body, headers);
            string content = ExtractJsonField(resp, "content");
            return string.IsNullOrEmpty(content) ? "Réponse reçue." : UnescapeJsonString(content);
        }

        private static string HttpPostJson(string url, string jsonBody, Dictionary<string, string> headers)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json; charset=utf-8";
            req.Timeout = 18000;
            if (headers != null)
                foreach (var kv in headers) req.Headers[kv.Key] = kv.Value;
            byte[] bytes = Encoding.UTF8.GetBytes(jsonBody);
            req.ContentLength = bytes.Length;
            using (Stream st = req.GetRequestStream()) st.Write(bytes, 0, bytes.Length);
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                return sr.ReadToEnd();
        }

        private static string EscapeJsonString(string s) { return string.IsNullOrEmpty(s) ? "" : s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n"); }
        private static string UnescapeJsonString(string s) { return string.IsNullOrEmpty(s) ? "" : s.Replace("\\n", "\n").Replace("\\r", "").Replace("\\\"", "\"").Replace("\\\\", "\\"); }

        private static string ExtractJsonField(string json, string fieldName)
        {
            if (string.IsNullOrEmpty(json)) return "";
            string pattern = "\"" + fieldName + "\"";
            int idx = json.IndexOf(pattern, StringComparison.OrdinalIgnoreCase);
            if (idx < 0) return "";
            int colon = json.IndexOf(':', idx + pattern.Length);
            if (colon < 0) return "";
            int firstQuote = json.IndexOf('"', colon + 1);
            if (firstQuote < 0) return "";
            StringBuilder sb = new StringBuilder();
            bool escaped = false;
            for (int i = firstQuote + 1; i < json.Length; i++)
            {
                char c = json[i];
                if (escaped) { if (c == 'n') sb.Append('\n'); else if (c != 'r') sb.Append(c); escaped = false; }
                else if (c == '\\') escaped = true;
                else if (c == '"') return sb.ToString();
                else sb.Append(c);
            }
            return sb.ToString();
        }
    }

    // =========================================================================
    // FOCUS TIMER & STOPWATCH ENGINE
    // =========================================================================
    public class FocusTimerEngine
    {
        public bool IsRunning = false;
        public bool IsStopwatchMode = false;
        public double TotalSeconds = 300;
        public double RemainingSeconds = 300;
        public double ElapsedStopwatchSeconds = 0;
        public string TimerLabel = "Minuteur";
        public bool FinishedAlertActive = false;
        private long lastTickTime = 0;
        private long alertUntilTicks = 0;

        public event Action OnTimerFinished;

        public void StartCountdown(int seconds, string label = "Minuteur")
        {
            IsStopwatchMode = false;
            TotalSeconds = Math.Max(5, seconds);
            RemainingSeconds = TotalSeconds;
            TimerLabel = label;
            IsRunning = true;
            FinishedAlertActive = false;
            lastTickTime = DateTime.UtcNow.Ticks;
        }

        public void AddSeconds(int sec)
        {
            if (IsStopwatchMode) return;
            TotalSeconds = Math.Max(10, TotalSeconds + sec);
            RemainingSeconds = Math.Max(0, RemainingSeconds + sec);
        }

        public void ToggleStartPause()
        {
            if (!IsRunning && !IsStopwatchMode && RemainingSeconds <= 0) RemainingSeconds = TotalSeconds;
            IsRunning = !IsRunning;
            lastTickTime = DateTime.UtcNow.Ticks;
            FinishedAlertActive = false;
        }

        public void Reset()
        {
            IsRunning = false;
            FinishedAlertActive = false;
            if (IsStopwatchMode) ElapsedStopwatchSeconds = 0;
            else RemainingSeconds = TotalSeconds;
        }

        public void SwitchToStopwatch()
        {
            IsStopwatchMode = true;
            ElapsedStopwatchSeconds = 0;
            TimerLabel = "Chrono";
            IsRunning = true;
            lastTickTime = DateTime.UtcNow.Ticks;
        }

        public void StepTick()
        {
            long now = DateTime.UtcNow.Ticks;
            if (FinishedAlertActive && now > alertUntilTicks) FinishedAlertActive = false;
            if (!IsRunning) { lastTickTime = now; return; }

            double dt = (now - lastTickTime) / 10000000.0;
            lastTickTime = now;
            if (dt <= 0 || dt > 2.0) return;

            if (IsStopwatchMode)
            {
                ElapsedStopwatchSeconds += dt;
            }
            else
            {
                RemainingSeconds -= dt;
                if (RemainingSeconds <= 0)
                {
                    RemainingSeconds = 0;
                    IsRunning = false;
                    FinishedAlertActive = true;
                    alertUntilTicks = now + 50000000L;
                    try { System.Media.SystemSounds.Exclamation.Play(); } catch { }
                    if (OnTimerFinished != null) OnTimerFinished();
                }
            }
        }

        public float ProgressRatio
        {
            get
            {
                if (IsStopwatchMode) return (float)((ElapsedStopwatchSeconds % 60.0) / 60.0);
                if (TotalSeconds <= 0) return 0f;
                return (float)Math.Max(0.0, Math.Min(1.0, RemainingSeconds / TotalSeconds));
            }
        }

        public string FormattedTime
        {
            get
            {
                double sec = IsStopwatchMode ? ElapsedStopwatchSeconds : Math.Ceiling(RemainingSeconds);
                int total = (int)Math.Max(0, sec);
                return (total / 60).ToString("00") + ":" + (total % 60).ToString("00");
            }
        }
    }

    // =========================================================================
    // REDESIGNED MINIMALIST GLASSMORPHIC COMPANION ENGINE (6 SMOOTH ENTITIES)
    // =========================================================================
    public class GlassCompanionEngine
    {
        public int Level = 1;
        public int Xp = 20;
        public string Mood = "Heureux 💖";
        public string SpeechBubbleText = "";
        private long speechUntilTicks = 0;
        private float pulseBoost = 0f;
        private float smoothHoverY = 0f;
        private float smoothLookX = 0f;
        private float smoothLookY = 0f;

        private static readonly string[] MinimalQuotes = new string[] {
            "Coucou toi ! Je veille sur ton écran ✨",
            "Trop bien cette musique, j'adore le rythme ! ♪",
            "Mode concentration activé, tu vas gérer ✦",
            "Câlin virtuel ! +20 XP bien mérités 💖",
            "Je suis tout doux et en verre dépoli ~",
            "Survole la capsule pour me voir de plus près !",
            "Je reste avec toi pour toute ta session 🐾",
            "Fais une pause si tes yeux fatiguent ☕"
        };
        private Random rng = new Random();

        public void Say(string text, int durationMs = 4200)
        {
            SpeechBubbleText = text;
            speechUntilTicks = DateTime.UtcNow.Ticks + durationMs * 10000L;
            pulseBoost = 1.0f;
        }

        public void InteractBoost()
        {
            Xp += 20;
            if (Xp >= 100)
            {
                Level++;
                Xp -= 100;
                Mood = "Épanoui ✧";
                Say("✨ Évolution ! Je passe Niveau " + Level + " !", 4200);
            }
            else
            {
                Mood = "Heureux 💖";
                Say(MinimalQuotes[rng.Next(MinimalQuotes.Length)], 3500);
            }
            pulseBoost = 1.0f;
        }

        public bool HasActiveSpeech
        {
            get { return !string.IsNullOrEmpty(SpeechBubbleText) && DateTime.UtcNow.Ticks < speechUntilTicks; }
        }

        public void StepTick(float globalTime, bool isAudioPlaying)
        {
            if (pulseBoost > 0f) pulseBoost = Math.Max(0f, pulseBoost - 0.03f);
            float freq = isAudioPlaying ? 5.5f : 2.0f;
            float amp = isAudioPlaying ? 3.2f : 1.6f;
            // Compound floating harmonic motion for natural organic buoyancy
            smoothHoverY = (float)(Math.Sin(globalTime * freq) * amp + Math.Sin(globalTime * freq * 0.5f) * (amp * 0.4f)) - pulseBoost * 5f;
        }

        public void DrawCompanion(
            Graphics g, RectangleF bounds, CompanionEntity entity,
            Point cursorScreenPos, Point companionScreenCenter,
            Color accent1, Color accent2, float globalTime, float audioPeak, bool followEyes)
        {
            if (entity == CompanionEntity.None || bounds.Width < 8f) return;

            SmoothingMode oldMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float cx = bounds.X + bounds.Width / 2f;
            float cy = bounds.Y + bounds.Height / 2f + smoothHoverY;
            float r = Math.Min(bounds.Width, bounds.Height) * 0.35f;

            // Smooth eye-tracking interpolation
            float targetLX = 0f, targetLY = 0f;
            if (followEyes && companionScreenCenter != Point.Empty)
            {
                float dx = cursorScreenPos.X - companionScreenCenter.X;
                float dy = cursorScreenPos.Y - companionScreenCenter.Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                if (dist > 2f)
                {
                    float f = Math.Min(1f, dist / 220f);
                    targetLX = (dx / dist) * r * 0.22f * f;
                    targetLY = (dy / dist) * r * 0.22f * f;
                }
            }
            smoothLookX = smoothLookX * 0.72f + targetLX * 0.28f;
            smoothLookY = smoothLookY * 0.72f + targetLY * 0.28f;

            // Warm Glassmorphic Ambient Aura
            float auraR = r * (1.32f + audioPeak * 0.30f + pulseBoost * 0.25f);
            using (GraphicsPath auraPath = new GraphicsPath())
            {
                auraPath.AddEllipse(cx - auraR, cy - auraR, auraR * 2f, auraR * 2f);
                using (PathGradientBrush pgb = new PathGradientBrush(auraPath))
                {
                    pgb.CenterColor = Color.FromArgb(70, accent1);
                    pgb.SurroundColors = new Color[] { Color.FromArgb(0, accent1) };
                    g.FillPath(pgb, auraPath);
                }
            }

            bool isAudioActive = audioPeak > 0.06f;

            switch (entity)
            {
                case CompanionEntity.LuminaOrb:
                    DrawLuminaOrb(g, cx, cy, r, smoothLookX, smoothLookY, accent1, accent2, globalTime, isAudioActive);
                    break;
                case CompanionEntity.KitsuneMinimal:
                    DrawKitsuneMinimal(g, cx, cy, r, smoothLookX, smoothLookY, accent1, accent2, globalTime, isAudioActive);
                    break;
                case CompanionEntity.DracoCrystal:
                    DrawDracoCrystal(g, cx, cy, r, smoothLookX, smoothLookY, accent1, accent2, globalTime, isAudioActive);
                    break;
                case CompanionEntity.NyxQuantum:
                    DrawNyxQuantum(g, cx, cy, r, smoothLookX, smoothLookY, accent1, accent2, globalTime, isAudioActive);
                    break;
                case CompanionEntity.KuroNeko:
                    DrawKuroNeko(g, cx, cy, r, smoothLookX, smoothLookY, accent1, accent2, globalTime, isAudioActive);
                    break;
                case CompanionEntity.AxiomPrism:
                    DrawAxiomPrism(g, cx, cy, r, smoothLookX, smoothLookY, accent1, accent2, globalTime, isAudioActive);
                    break;
            }

            // Floating heart particle on interaction
            if (pulseBoost > 0.08f)
            {
                float hpY = cy - r * 1.2f - (1f - pulseBoost) * 18f;
                int hAlpha = (int)(pulseBoost * 230);
                using (Font hf = new Font("Segoe UI Symbol", Math.Max(8f, r * 0.45f), FontStyle.Bold, GraphicsUnit.Pixel))
                using (SolidBrush hb = new SolidBrush(Color.FromArgb(hAlpha, 255, 110, 160)))
                {
                    g.DrawString("♥", hf, hb, cx - r * 0.22f, hpY);
                }
            }

            g.SmoothingMode = oldMode;
        }

        private void DrawGlassSphereBody(Graphics g, float cx, float cy, float r, Color accent1, Color accent2)
        {
            RectangleF bodyRect = new RectangleF(cx - r, cy - r, r * 2f, r * 2f);
            using (SolidBrush darkGlass = new SolidBrush(Color.FromArgb(238, 14, 18, 28)))
            using (LinearGradientBrush sheen = new LinearGradientBrush(bodyRect, Color.FromArgb(70, 255, 255, 255), Color.FromArgb(20, accent1), 45f))
            using (Pen specularRim = new Pen(Color.FromArgb(145, GlassThemeEngine.Interpolate(Color.White, accent1, 0.45f)), 1.35f))
            {
                g.FillEllipse(darkGlass, bodyRect);
                g.FillEllipse(sheen, bodyRect);
                g.DrawEllipse(specularRim, bodyRect);
            }
            // Top liquid meniscus crescent
            RectangleF highlightRect = new RectangleF(cx - r * 0.65f, cy - r * 0.84f, r * 1.3f, r * 0.65f);
            using (LinearGradientBrush hl = new LinearGradientBrush(highlightRect, Color.FromArgb(60, Color.White), Color.FromArgb(0, Color.White), 90f))
            {
                g.FillEllipse(hl, highlightRect);
            }
        }

        // Elegant celestial crystal eyes with star reflection highlights
        private void DrawCelestialGlassEyes(Graphics g, float cx, float cy, float r, float lx, float ly, Color accent, float t)
        {
            bool blink = ((t % 5.0f) > 4.82f);
            float spacing = r * 0.35f;
            float ew = Math.Max(3.2f, r * 0.22f);
            float eh = Math.Max(4.5f, r * 0.32f);
            float ey = cy + ly - r * 0.04f;

            if (blink)
            {
                using (Pen eyePen = new Pen(Color.FromArgb(235, Color.White), 1.6f))
                {
                    eyePen.StartCap = LineCap.Round;
                    eyePen.EndCap = LineCap.Round;
                    g.DrawArc(eyePen, cx - spacing + lx - ew / 2f, ey, ew, eh * 0.45f, 200, 140);
                    g.DrawArc(eyePen, cx + spacing + lx - ew / 2f, ey, ew, eh * 0.45f, 200, 140);
                }
            }
            else
            {
                for (int side = -1; side <= 1; side += 2)
                {
                    float ex = cx + side * spacing + lx;
                    RectangleF eyeRect = new RectangleF(ex - ew / 2f, ey - eh / 2f, ew, eh);

                    // Ambient luminous aura
                    using (SolidBrush auraB = new SolidBrush(Color.FromArgb(85, accent)))
                        g.FillEllipse(auraB, RectangleF.Inflate(eyeRect, 3f, 3f));

                    // Deep obsidian-sapphire iris
                    using (LinearGradientBrush irisB = new LinearGradientBrush(
                        eyeRect, Color.FromArgb(240, 12, 16, 28), Color.FromArgb(245, GlassThemeEngine.Interpolate(Color.White, accent, 0.7f)), 90f))
                    {
                        g.FillEllipse(irisB, eyeRect);
                    }

                    // Crystal primary twinkle
                    using (SolidBrush wb = new SolidBrush(Color.White))
                    {
                        g.FillEllipse(wb, ex - ew * 0.25f, ey - eh * 0.30f, ew * 0.42f, eh * 0.42f);
                        g.FillEllipse(wb, ex + ew * 0.12f, ey + eh * 0.14f, ew * 0.20f, eh * 0.20f);
                    }
                }
            }
        }

        private void DrawLuminaOrb(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t, bool isAudio)
        {
            // 1. Dual Gyroscopic 3D Rings
            float rot1 = (t * 22f) % 360f;
            float rot2 = (-t * 18f + 60f) % 360f;

            GraphicsState gs1 = g.Save();
            g.TranslateTransform(cx, cy);
            g.RotateTransform(35f);
            using (Pen ring1 = new Pen(Color.FromArgb(170, GlassThemeEngine.Interpolate(Color.White, c1, 0.4f)), 1.4f))
            {
                g.DrawEllipse(ring1, -r * 1.35f, -r * 0.42f, r * 2.70f, r * 0.84f);
            }
            // Rotating diamond star on ring 1
            float rad1 = rot1 * (float)Math.PI / 180f;
            float sx1 = (float)Math.Cos(rad1) * r * 1.35f;
            float sy1 = (float)Math.Sin(rad1) * r * 0.42f;
            using (SolidBrush sb = new SolidBrush(Color.White))
            {
                g.FillEllipse(sb, sx1 - 2.5f, sy1 - 2.5f, 5f, 5f);
            }
            g.Restore(gs1);

            GraphicsState gs2 = g.Save();
            g.TranslateTransform(cx, cy);
            g.RotateTransform(-40f);
            using (Pen ring2 = new Pen(Color.FromArgb(140, c2), 1.2f))
            {
                g.DrawEllipse(ring2, -r * 1.25f, -r * 0.38f, r * 2.50f, r * 0.76f);
            }
            float rad2 = rot2 * (float)Math.PI / 180f;
            float sx2 = (float)Math.Cos(rad2) * r * 1.25f;
            float sy2 = (float)Math.Sin(rad2) * r * 0.38f;
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(230, c1)))
            {
                g.FillEllipse(sb, sx2 - 2.2f, sy2 - 2.2f, 4.4f, 4.4f);
            }
            g.Restore(gs2);

            // 2. Bioluminescent core & obsidian glass body
            DrawGlassSphereBody(g, cx, cy, r, c1, c2);

            // Pulsating internal celestial core
            float corePulse = (float)Math.Sin(t * 3.5f) * r * 0.08f;
            float cr = r * 0.35f + corePulse;
            using (GraphicsPath cp = new GraphicsPath())
            {
                cp.AddEllipse(cx - cr, cy - cr, cr * 2f, cr * 2f);
                using (PathGradientBrush pb = new PathGradientBrush(cp))
                {
                    pb.CenterColor = Color.FromArgb(190, Color.White);
                    pb.SurroundColors = new Color[] { Color.FromArgb(0, c1) };
                    g.FillPath(pb, cp);
                }
            }

            DrawCelestialGlassEyes(g, cx, cy, r, lx, ly, c1, t);
        }

        private void DrawKitsuneMinimal(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t, bool isAudio)
        {
            // 1. Ethereal silk light tail in flowing sweeping arc
            float tailSweep = (float)Math.Sin(t * 2.2f) * r * 0.20f;
            PointF[] tailPts = new PointF[] {
                new PointF(cx + r * 0.45f, cy + r * 0.45f),
                new PointF(cx + r * 1.35f + tailSweep, cy + r * 0.35f),
                new PointF(cx + r * 1.55f + tailSweep, cy - r * 0.50f),
                new PointF(cx + r * 1.15f, cy - r * 0.85f),
                new PointF(cx + r * 0.95f, cy - r * 0.30f),
                new PointF(cx + r * 0.80f, cy + r * 0.15f)
            };
            using (LinearGradientBrush tailBrush = new LinearGradientBrush(
                new RectangleF(cx, cy - r, r * 1.8f, r * 2f),
                Color.FromArgb(170, c1), Color.FromArgb(40, c2), 45f))
            using (Pen tailPen = new Pen(Color.FromArgb(180, Color.White), 1.2f))
            {
                g.FillClosedCurve(tailBrush, tailPts, FillMode.Winding, 0.45f);
                g.DrawClosedCurve(tailPen, tailPts, 0.45f, FillMode.Winding);
            }

            // 2. Sacred Crystal Fox Ears (Aurora borealis gradient)
            float earTwitch = lx * 0.08f;
            for (int side = -1; side <= 1; side += 2)
            {
                GraphicsState st = g.Save();
                g.TranslateTransform(cx + side * r * 0.58f, cy - r * 0.80f + earTwitch * side);
                g.RotateTransform(side * 20f);
                RectangleF earRect = new RectangleF(-r * 0.26f, -r * 0.52f, r * 0.52f, r * 1.05f);
                using (LinearGradientBrush eb = new LinearGradientBrush(earRect, Color.FromArgb(235, Color.White), Color.FromArgb(220, 15, 18, 30), 90f))
                using (Pen ep = new Pen(Color.FromArgb(180, c1), 1.4f))
                {
                    g.FillEllipse(eb, earRect);
                    g.DrawEllipse(ep, earRect);
                }
                // Inner crystalline auroral core
                RectangleF innerRect = new RectangleF(-r * 0.14f, -r * 0.36f, r * 0.28f, r * 0.72f);
                using (LinearGradientBrush inb = new LinearGradientBrush(innerRect, Color.FromArgb(210, c1), Color.FromArgb(180, c2), 90f))
                    g.FillEllipse(inb, innerRect);
                g.Restore(st);
            }

            DrawGlassSphereBody(g, cx, cy, r * 0.94f, c1, c2);

            // Sacred golden crest mark on forehead
            using (SolidBrush crestB = new SolidBrush(Color.FromArgb(210, 255, 225, 140)))
            {
                PointF[] crest = new PointF[] {
                    new PointF(cx, cy - r * 0.65f),
                    new PointF(cx + 3.5f, cy - r * 0.48f),
                    new PointF(cx, cy - r * 0.32f),
                    new PointF(cx - 3.5f, cy - r * 0.48f)
                };
                g.FillPolygon(crestB, crest);
            }

            DrawCelestialGlassEyes(g, cx, cy + r * 0.04f, r, lx, ly, c1, t);
        }

        private void DrawDracoCrystal(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t, bool isAudio)
        {
            // 1. Translucent faceted crystal wyvern wings
            float flap = (float)Math.Sin(t * 4.2f) * r * 0.24f;
            for (int side = -1; side <= 1; side += 2)
            {
                PointF[] wing = new PointF[] {
                    new PointF(cx + side * r * 0.50f, cy + r * 0.05f),
                    new PointF(cx + side * r * 1.60f, cy - r * 0.75f + flap),
                    new PointF(cx + side * r * 1.45f, cy - r * 0.15f + flap * 0.7f),
                    new PointF(cx + side * r * 1.30f, cy + r * 0.40f + flap * 0.4f),
                    new PointF(cx + side * r * 0.70f, cy + r * 0.25f)
                };
                using (LinearGradientBrush wb = new LinearGradientBrush(
                    new RectangleF(cx - r * 1.7f, cy - r, r * 3.4f, r * 2f),
                    Color.FromArgb(130, c2), Color.FromArgb(35, Color.White), 45f))
                using (Pen wp = new Pen(Color.FromArgb(180, c1), 1.3f))
                {
                    g.FillPolygon(wb, wing);
                    g.DrawPolygon(wp, wing);
                    // Glowing wing vein
                    g.DrawLine(wp, cx + side * r * 0.50f, cy + r * 0.05f, cx + side * r * 1.60f, cy - r * 0.75f + flap);
                }
            }

            // 2. Sculpted polished quartz horns
            for (int side = -1; side <= 1; side += 2)
            {
                PointF[] horn = new PointF[] {
                    new PointF(cx + side * r * 0.30f, cy - r * 0.75f),
                    new PointF(cx + side * r * 0.68f, cy - r * 1.25f),
                    new PointF(cx + side * r * 0.65f, cy - r * 0.70f)
                };
                using (LinearGradientBrush hb = new LinearGradientBrush(
                    new RectangleF(cx - r, cy - r * 1.3f, r * 2f, r), Color.FromArgb(235, Color.White), Color.FromArgb(210, c1), 90f))
                using (Pen hp = new Pen(Color.FromArgb(200, Color.White), 1.2f))
                {
                    g.FillPolygon(hb, horn);
                    g.DrawPolygon(hp, horn);
                }
            }

            DrawGlassSphereBody(g, cx, cy, r * 0.93f, c1, c2);

            // Pulsating ruby core gemstone
            float gemPulse = (float)Math.Sin(t * 3.8f) * 1.8f;
            RectangleF gemRect = new RectangleF(cx - 4.5f - gemPulse / 2f, cy + r * 0.18f - gemPulse / 2f, 9f + gemPulse, 9f + gemPulse);
            using (SolidBrush gemB = new SolidBrush(Color.FromArgb(210, 255, 80, 110)))
            using (Pen gemP = new Pen(Color.FromArgb(220, Color.White), 1.1f))
            {
                g.FillEllipse(gemB, gemRect);
                g.DrawEllipse(gemP, gemRect);
            }

            DrawCelestialGlassEyes(g, cx, cy - r * 0.02f, r, lx, ly, c1, t);
        }

        private void DrawNyxQuantum(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t, bool isAudio)
        {
            // 1. Fluid opalescent cloud-drop base
            float wave = (float)Math.Sin(t * 3.6f) * r * 0.08f;
            RectangleF opalRect = new RectangleF(cx - r * 0.95f, cy - r * 0.95f + wave, r * 1.9f, r * 1.9f);
            using (LinearGradientBrush ob = new LinearGradientBrush(opalRect, Color.FromArgb(230, 18, 22, 38), Color.FromArgb(200, 32, 28, 52), 45f))
            using (Pen op = new Pen(Color.FromArgb(170, GlassThemeEngine.Interpolate(Color.White, c1, 0.5f)), 1.4f))
            {
                g.FillEllipse(ob, opalRect);
                g.DrawEllipse(op, opalRect);
            }

            // Internal opalescent iridescent sheen
            RectangleF sheenR = new RectangleF(cx - r * 0.65f, cy - r * 0.70f + wave, r * 1.3f, r * 0.75f);
            using (LinearGradientBrush shb = new LinearGradientBrush(sheenR, Color.FromArgb(75, 230, 245, 255), Color.FromArgb(0, 255, 255, 255), 90f))
            {
                g.FillEllipse(shb, sheenR);
            }

            // 2. 3 Golden-spiral celestial star pearls
            for (int i = 0; i < 3; i++)
            {
                double ang = t * 2.2 + i * (Math.PI * 2.0 / 3.0);
                float ox = cx + (float)Math.Cos(ang) * r * 1.34f;
                float oy = cy + (float)Math.Sin(ang) * r * 0.65f;
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(235, i % 2 == 0 ? Color.White : c1)))
                using (SolidBrush gb = new SolidBrush(Color.FromArgb(90, c1)))
                {
                    g.FillEllipse(gb, ox - 5f, oy - 5f, 10f, 10f);
                    g.FillEllipse(sb, ox - 2.5f, oy - 2.5f, 5f, 5f);
                }
            }

            DrawCelestialGlassEyes(g, cx, cy + wave, r, lx, ly, Color.White, t);
        }

        private void DrawKuroNeko(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t, bool isAudio)
        {
            // 1. Sleek sculptural cat ears in deep smoked onyx & amethyst
            for (int side = -1; side <= 1; side += 2)
            {
                PointF[] ear = new PointF[] {
                    new PointF(cx + side * r * 0.28f, cy - r * 0.72f),
                    new PointF(cx + side * r * 0.74f, cy - r * 1.18f),
                    new PointF(cx + side * r * 0.88f, cy - r * 0.28f)
                };
                using (LinearGradientBrush eb = new LinearGradientBrush(
                    new RectangleF(cx - r, cy - r * 1.2f, r * 2f, r), Color.FromArgb(245, 12, 14, 22), Color.FromArgb(220, 24, 20, 36), 90f))
                using (Pen ep = new Pen(Color.FromArgb(170, c1), 1.35f))
                {
                    g.FillClosedCurve(eb, ear, FillMode.Alternate, 0.35f);
                    g.DrawClosedCurve(ep, ear, 0.35f, FillMode.Alternate);
                }
                // Amethyst interior glow
                PointF[] inner = new PointF[] {
                    new PointF(cx + side * r * 0.38f, cy - r * 0.65f),
                    new PointF(cx + side * r * 0.70f, cy - r * 1.02f),
                    new PointF(cx + side * r * 0.78f, cy - r * 0.38f)
                };
                using (SolidBrush inb = new SolidBrush(Color.FromArgb(175, 160, 110, 240)))
                    g.FillClosedCurve(inb, inner, FillMode.Alternate, 0.35f);
            }

            DrawGlassSphereBody(g, cx, cy, r * 0.95f, c1, c2);

            // Floating golden stardust orb above head
            float orbY = cy - r * 1.15f + (float)Math.Sin(t * 3.0f) * 2f;
            using (SolidBrush sob = new SolidBrush(Color.FromArgb(220, 255, 215, 120)))
            using (SolidBrush aob = new SolidBrush(Color.FromArgb(80, 255, 215, 120)))
            {
                g.FillEllipse(aob, cx - 6f, orbY - 6f, 12f, 12f);
                g.FillEllipse(sob, cx - 2.5f, orbY - 2.5f, 5f, 5f);
            }

            // Hairline laser-whiskers (subtle & chic)
            using (Pen wp = new Pen(Color.FromArgb(110, 255, 255, 255), 1.0f))
            {
                g.DrawLine(wp, cx - r * 0.40f, cy + r * 0.12f, cx - r * 0.92f, cy + r * 0.06f);
                g.DrawLine(wp, cx - r * 0.40f, cy + r * 0.22f, cx - r * 0.92f, cy + r * 0.28f);
                g.DrawLine(wp, cx + r * 0.40f, cy + r * 0.12f, cx + r * 0.92f, cy + r * 0.06f);
                g.DrawLine(wp, cx + r * 0.40f, cy + r * 0.22f, cx + r * 0.92f, cy + r * 0.28f);
            }

            DrawCelestialGlassEyes(g, cx, cy + r * 0.04f, r, lx, ly, Color.FromArgb(140, 190, 255), t);
        }

        private void DrawAxiomPrism(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t, bool isAudio)
        {
            // 1. Rotating Holographic Equatorial Ring
            GraphicsState st = g.Save();
            g.TranslateTransform(cx, cy);
            g.RotateTransform((float)Math.Sin(t * 1.5f) * 16f - 12f);
            using (Pen ringPen = new Pen(Color.FromArgb(170, c1), 1.5f))
            {
                g.DrawEllipse(ringPen, -r * 1.35f, -r * 0.35f, r * 2.70f, r * 0.70f);
            }
            g.Restore(st);

            // 2. Beveled Sapphire Crystal Sphere in Brushed Titanium
            DrawGlassSphereBody(g, cx, cy, r * 0.94f, c1, c2);

            // Curved OLED Glass Visor
            RectangleF visor = new RectangleF(cx - r * 0.70f, cy - r * 0.34f, r * 1.40f, r * 0.68f);
            using (GraphicsPath vp = GlassThemeEngine.CreateRoundedRect(visor, visor.Height / 2f))
            {
                using (SolidBrush screenB = new SolidBrush(Color.FromArgb(245, 8, 10, 16)))
                    g.FillPath(screenB, vp);
                using (Pen rimP = new Pen(Color.FromArgb(150, c1), 1.25f))
                    g.DrawPath(rimP, vp);
            }

            // Animated Vector Optical Core Eye (futuristic iris dilation)
            float eyeDilate = (float)(0.65 + 0.35 * Math.Sin(t * 3.0f));
            float er = r * 0.20f * eyeDilate;
            float ecx = cx + lx * 0.5f;
            float ecy = cy + ly * 0.5f;
            using (SolidBrush coreGlow = new SolidBrush(Color.FromArgb(90, c1)))
            using (SolidBrush coreDot = new SolidBrush(Color.White))
            using (Pen pupilPen = new Pen(Color.FromArgb(220, c1), 1.2f))
            {
                g.FillEllipse(coreGlow, ecx - er * 1.5f, ecy - er * 1.5f, er * 3f, er * 3f);
                g.DrawEllipse(pupilPen, ecx - er, ecy - er, er * 2f, er * 2f);
                g.FillEllipse(coreDot, ecx - 2f, ecy - 2f, 4f, 4f);
            }
        }
    }

    // =========================================================================
    // WIN32 INTEROP & COREAUDIO REAL-TIME PEAK METER
    // =========================================================================
    public static class NativeMethods
    {
        public const int WS_EX_LAYERED = 0x00080000;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_TOPMOST = 0x00000008;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_EX_NOACTIVATE = 0x08000000;

        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;
        public const int ULW_ALPHA = 0x00000002;

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOACTIVATE = 0x0010;

        public const byte VK_VOLUME_DOWN = 0xAE;
        public const byte VK_VOLUME_UP = 0xAF;
        public const byte VK_MEDIA_NEXT_TRACK = 0xB0;
        public const byte VK_MEDIA_PREV_TRACK = 0xB1;
        public const byte VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const uint KEYEVENTF_KEYUP = 0x0002;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT { public int X; public int Y; public POINT(int x, int y) { X = x; Y = y; } }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE { public int cx; public int cy; public SIZE(int x, int y) { cx = x; cy = y; } }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION { public byte BlendOp; public byte BlendFlags; public byte SourceConstantAlpha; public byte AlphaFormat; }

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        [DllImport("shcore.dll")]
        public static extern int SetProcessDpiAwareness(int value);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDpiAwarenessContext(IntPtr dpiContext);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT { public int Left; public int Top; public int Right; public int Bottom; }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        public static bool IsForegroundFullscreen(Rectangle screenBounds, out string appTitle)
        {
            appTitle = "";
            try
            {
                IntPtr fg = GetForegroundWindow();
                if (fg == IntPtr.Zero || !IsWindowVisible(fg)) return false;

                StringBuilder cls = new StringBuilder(64);
                GetClassName(fg, cls, 64);
                string cname = cls.ToString();
                if (cname == "Progman" || cname == "WorkerW" || cname == "Shell_TrayWnd") return false;

                RECT r;
                if (!GetWindowRect(fg, out r)) return false;

                // Check if foreground window covers or exceeds the screen bounds
                bool covers = (r.Left <= screenBounds.Left + 2 &&
                               r.Top <= screenBounds.Top + 2 &&
                               r.Right >= screenBounds.Right - 2 &&
                               r.Bottom >= screenBounds.Bottom - 2);
                if (covers)
                {
                    StringBuilder sb = new StringBuilder(128);
                    GetWindowText(fg, sb, 128);
                    appTitle = sb.ToString();
                    if (string.IsNullOrEmpty(appTitle)) appTitle = "Plein Écran";
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static void SendMediaKey(byte vk)
        {
            try
            {
                keybd_event(vk, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
                keybd_event(vk, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
            }
            catch { }
        }

        public static void EnablePerMonitorDpiAwareness()
        {
            try { SetProcessDpiAwarenessContext(new IntPtr(-4)); return; } catch { }
            try { SetProcessDpiAwareness(2); return; } catch { }
            try { SetProcessDPIAware(); } catch { }
        }
    }

    [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class MMDeviceEnumeratorComObject { }

    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceEnumerator
    {
        int EnumAudioEndpoints(int dataFlow, int dwStateMask, out IntPtr ppDevices);
        int GetDefaultAudioEndpoint(int dataFlow, int role, out IMMDevice ppEndpoint);
    }

    [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDevice
    {
        int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
    }

    [Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioMeterInformation
    {
        int GetPeakValue(out float pfPeak);
    }

    public class LiveMediaAndAudioTracker
    {
        private IAudioMeterInformation audioMeter;
        public float CurrentAudioPeak = 0f;
        public bool IsAudioActive = false;

        public AudioAppSource ActiveSource = AudioAppSource.None;
        public string TrackTitle = "Aucun média actif";
        public string TrackArtist = "Spotify • YouTube • Lecteur Web";

        private long lastWindowScanTicks = 0;
        public float[] SpectrumBars = new float[16];

        public LiveMediaAndAudioTracker()
        {
            InitializeCoreAudio();
            ScanMediaWindowsAsync();
        }

        private void InitializeCoreAudio()
        {
            try
            {
                IMMDeviceEnumerator enumerator = (IMMDeviceEnumerator)(new MMDeviceEnumeratorComObject());
                IMMDevice device;
                if (enumerator.GetDefaultAudioEndpoint(0, 1, out device) == 0 && device != null)
                {
                    Guid iidMeter = new Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064");
                    object obj;
                    if (device.Activate(ref iidMeter, 23, IntPtr.Zero, out obj) == 0 && obj != null)
                        audioMeter = (IAudioMeterInformation)obj;
                }
            }
            catch { }
        }

        public void UpdateTick(float globalTime)
        {
            float peak = 0f;
            if (audioMeter != null)
            {
                try { audioMeter.GetPeakValue(out peak); } catch { }
            }
            if (peak > CurrentAudioPeak)
                CurrentAudioPeak = CurrentAudioPeak * 0.45f + peak * 0.55f;
            else
                CurrentAudioPeak = CurrentAudioPeak * 0.86f + peak * 0.14f;

            IsAudioActive = (CurrentAudioPeak > 0.012f);

            for (int i = 0; i < SpectrumBars.Length; i++)
            {
                float target;
                if (IsAudioActive)
                {
                    float shape = (float)Math.Sin((i / (float)SpectrumBars.Length) * Math.PI);
                    float w1 = (float)Math.Sin(globalTime * 10.5f + i * 0.85f) * 0.5f + 0.5f;
                    target = Math.Min(1.0f, CurrentAudioPeak * 1.4f * (0.35f + 0.65f * shape) * (0.45f + 0.55f * w1));
                    if (target < 0.10f) target = 0.10f;
                }
                else
                {
                    target = 0.10f + 0.05f * (float)Math.Sin(globalTime * 2.5f + i * 0.6f);
                }
                SpectrumBars[i] = SpectrumBars[i] * 0.65f + target * 0.35f;
            }

            long now = DateTime.UtcNow.Ticks;
            if ((now - lastWindowScanTicks) > 20000000L) // Scan every 2.0s on background thread for zero UI lag!
            {
                lastWindowScanTicks = now;
                ScanMediaWindowsAsync();
            }
        }

        public int TrackVersion = 0;

        public static string GetSourceName(AudioAppSource src)
        {
            switch (src)
            {
                case AudioAppSource.Spotify: return "Spotify";
                case AudioAppSource.AppleMusic: return "Apple Music";
                case AudioAppSource.YouTube: return "YouTube";
                case AudioAppSource.Deezer: return "Deezer";
                case AudioAppSource.SoundCloud: return "SoundCloud";
                case AudioAppSource.Tidal: return "Tidal";
                case AudioAppSource.VLC: return "VLC";
                case AudioAppSource.AmazonMusic: return "Amazon Music";
                case AudioAppSource.MediaPlayer: return "Lecteur Windows";
                case AudioAppSource.WebMusic: return "Lecteur Web";
                case AudioAppSource.SystemSound: return "Audio Système";
                default: return "Nova";
            }
        }

        private void ScanMediaWindowsAsync()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    Process[] procs = Process.GetProcesses();
                    AudioAppSource foundSource = AudioAppSource.None;
                    string foundTitle = null;
                    string foundArtist = null;

                    foreach (Process p in procs)
                    {
                        string pname = "";
                        string wtitle = "";
                        try
                        {
                            if (p.MainWindowHandle == IntPtr.Zero) continue;
                            pname = p.ProcessName.ToLowerInvariant();
                            wtitle = p.MainWindowTitle;
                            if (string.IsNullOrEmpty(wtitle)) continue;
                        }
                        catch { continue; }
                        finally { try { p.Dispose(); } catch { } }

                        // Apple Music (Native app or iTunes)
                        if (pname == "applemusic" || pname == "itunes")
                        {
                            foundSource = AudioAppSource.AppleMusic;
                            if (wtitle.Contains(" - "))
                            {
                                int dash = wtitle.IndexOf(" - ");
                                foundArtist = wtitle.Substring(0, dash).Trim();
                                foundTitle = wtitle.Substring(dash + 3).Trim();
                            }
                            else
                            {
                                foundTitle = wtitle;
                                foundArtist = "Apple Music";
                            }
                            break;
                        }

                        // Spotify
                        if (pname == "spotify")
                        {
                            foundSource = AudioAppSource.Spotify;
                            if (wtitle.StartsWith("Spotify", StringComparison.OrdinalIgnoreCase))
                            {
                                foundTitle = "Spotify";
                                foundArtist = "En pause";
                            }
                            else if (wtitle.Contains(" - "))
                            {
                                int dash = wtitle.IndexOf(" - ");
                                foundArtist = wtitle.Substring(0, dash).Trim();
                                foundTitle = wtitle.Substring(dash + 3).Trim();
                            }
                            else
                            {
                                foundTitle = wtitle;
                                foundArtist = "Spotify";
                            }
                            break;
                        }

                        // VLC Media Player
                        if (pname == "vlc")
                        {
                            foundSource = AudioAppSource.VLC;
                            foundTitle = wtitle.Replace(" - Lecteur multimédia VLC", "").Replace(" - VLC media player", "").Trim();
                            foundArtist = "VLC";
                            break;
                        }

                        // Deezer native
                        if (pname == "deezer")
                        {
                            foundSource = AudioAppSource.Deezer;
                            foundTitle = wtitle;
                            foundArtist = "Deezer";
                            break;
                        }

                        // Tidal native
                        if (pname == "tidal")
                        {
                            foundSource = AudioAppSource.Tidal;
                            foundTitle = wtitle;
                            foundArtist = "Tidal";
                            break;
                        }

                        // Web Browsers
                        if (pname == "chrome" || pname == "msedge" || pname == "firefox" || pname == "brave" || pname == "opera" || pname == "vivaldi")
                        {
                            string clean = StripBrowserSuffix(wtitle);
                            if (clean.IndexOf("Apple Music", StringComparison.OrdinalIgnoreCase) >= 0 || clean.IndexOf("music.apple.com", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                foundSource = AudioAppSource.AppleMusic;
                                foundTitle = clean.Replace("- Apple Music", "").Replace("Apple Music", "").Trim();
                                if (string.IsNullOrEmpty(foundTitle)) foundTitle = "Apple Music";
                                foundArtist = "Apple Music Web";
                                break;
                            }
                            else if (clean.IndexOf("YouTube", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                foundSource = AudioAppSource.YouTube;
                                foundTitle = clean.Replace("- YouTube Music", "").Replace("YouTube Music", "").Replace("- YouTube", "").Replace("YouTube", "").Trim();
                                if (string.IsNullOrEmpty(foundTitle)) foundTitle = "YouTube";
                                foundArtist = "Flux YouTube";
                                break;
                            }
                            else if (clean.IndexOf("Spotify", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                foundSource = AudioAppSource.Spotify;
                                foundTitle = clean.Replace("- Spotify", "").Replace("Spotify", "").Trim();
                                if (string.IsNullOrEmpty(foundTitle)) foundTitle = "Spotify Web";
                                foundArtist = "Spotify";
                                break;
                            }
                            else if (clean.IndexOf("Deezer", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                foundSource = AudioAppSource.Deezer;
                                foundTitle = clean.Replace("- Deezer", "").Trim();
                                foundArtist = "Deezer Web";
                                break;
                            }
                            else if (clean.IndexOf("SoundCloud", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                foundSource = AudioAppSource.SoundCloud;
                                foundTitle = clean.Replace("- SoundCloud", "").Trim();
                                foundArtist = "SoundCloud";
                                break;
                            }
                            else if (clean.IndexOf("Tidal", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                foundSource = AudioAppSource.Tidal;
                                foundTitle = clean;
                                foundArtist = "Tidal Web";
                                break;
                            }
                            else if (clean.IndexOf("Music", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                foundSource = AudioAppSource.WebMusic;
                                foundTitle = clean;
                                foundArtist = "Streaming Audio";
                            }
                        }
                    }

                    if (foundSource != AudioAppSource.None)
                    {
                        if (foundTitle != TrackTitle || foundArtist != TrackArtist || foundSource != ActiveSource)
                            TrackVersion++;

                        ActiveSource = foundSource;
                        TrackTitle = foundTitle;
                        TrackArtist = foundArtist;
                    }
                    else if (IsAudioActive)
                    {
                        if (ActiveSource != AudioAppSource.SystemSound)
                            TrackVersion++;

                        ActiveSource = AudioAppSource.SystemSound;
                        TrackTitle = "Flux Audio Actif";
                        TrackArtist = "Sortie Système";
                    }
                    else
                    {
                        ActiveSource = AudioAppSource.None;
                        TrackTitle = "Nova Capsule";
                        TrackArtist = "Audio • Focus • Compagnon • IA";
                    }
                }
                catch { }
            });
        }

        private static string StripBrowserSuffix(string title)
        {
            string t = title.Trim();
            if (t.StartsWith("(") && t.IndexOf(") ") > 0 && t.IndexOf(") ") < 6)
                t = t.Substring(t.IndexOf(") ") + 2).Trim();
            string[] suffixes = new string[] {
                " - Google Chrome", " - Microsoft​ Edge", " - Microsoft Edge",
                " — Mozilla Firefox", " - Mozilla Firefox", " - Brave", " - Opera", " - Vivaldi"
            };
            foreach (string s in suffixes)
                if (t.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    t = t.Substring(0, t.Length - s.Length).Trim();
            return t;
        }

        public void LaunchOrFocusService(string service)
        {
            try
            {
                if (service == "spotify")
                {
                    Process[] sp = Process.GetProcessesByName("Spotify");
                    if (sp.Length > 0 && sp[0].MainWindowHandle != IntPtr.Zero)
                    {
                        NativeMethods.ShowWindow(sp[0].MainWindowHandle, 9);
                        NativeMethods.SetForegroundWindow(sp[0].MainWindowHandle);
                        return;
                    }
                    try { Process.Start("spotify:"); return; } catch { }
                    Process.Start("https://open.spotify.com");
                }
                else if (service == "applemusic")
                {
                    Process[] ap = Process.GetProcessesByName("AppleMusic");
                    if (ap.Length == 0) ap = Process.GetProcessesByName("iTunes");
                    if (ap.Length > 0 && ap[0].MainWindowHandle != IntPtr.Zero)
                    {
                        NativeMethods.ShowWindow(ap[0].MainWindowHandle, 9);
                        NativeMethods.SetForegroundWindow(ap[0].MainWindowHandle);
                        return;
                    }
                    try { Process.Start("itms:"); return; } catch { }
                    try { Process.Start("music:"); return; } catch { }
                    Process.Start("https://music.apple.com");
                }
                else if (service == "youtube")
                {
                    Process.Start("https://music.youtube.com");
                }
                else if (service == "webmusic")
                {
                    Process.Start("https://www.deezer.com");
                }
            }
            catch { }
        }
    }

    // =========================================================================
    // BASE LAYERED OVERLAY FORM (OPTIMIZED REUSABLE BUFFER)
    // =========================================================================
    public class LayeredOverlayForm : Form
    {
        private bool clickThrough = true;

        public LayeredOverlayForm(bool isClickThrough = true)
        {
            clickThrough = isClickThrough;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= NativeMethods.WS_EX_LAYERED |
                              NativeMethods.WS_EX_TOPMOST |
                              NativeMethods.WS_EX_TOOLWINDOW |
                              NativeMethods.WS_EX_NOACTIVATE;
                if (clickThrough) cp.ExStyle |= NativeMethods.WS_EX_TRANSPARENT;
                return cp;
            }
        }

        protected override bool ShowWithoutActivation { get { return true; } }

        public void SetBitmap(Bitmap bmp, int x, int y, byte opacity = 255)
        {
            if (IsDisposed || bmp == null) return;
            if (!IsHandleCreated) CreateHandle();

            IntPtr screenDc = NativeMethods.GetDC(IntPtr.Zero);
            IntPtr memDc = NativeMethods.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bmp.GetHbitmap(Color.FromArgb(0));
                oldBitmap = NativeMethods.SelectObject(memDc, hBitmap);

                NativeMethods.SIZE size = new NativeMethods.SIZE(bmp.Width, bmp.Height);
                NativeMethods.POINT pointSource = new NativeMethods.POINT(0, 0);
                NativeMethods.POINT topPos = new NativeMethods.POINT(x, y);

                NativeMethods.BLENDFUNCTION blend = new NativeMethods.BLENDFUNCTION();
                blend.BlendOp = NativeMethods.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = opacity;
                blend.AlphaFormat = NativeMethods.AC_SRC_ALPHA;

                NativeMethods.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, NativeMethods.ULW_ALPHA);
            }
            finally
            {
                if (hBitmap != IntPtr.Zero)
                {
                    NativeMethods.SelectObject(memDc, oldBitmap);
                    NativeMethods.DeleteObject(hBitmap);
                }
                NativeMethods.DeleteDC(memDc);
                NativeMethods.ReleaseDC(IntPtr.Zero, screenDc);
            }
        }

        public void EnsureTopMost()
        {
            if (!IsDisposed && IsHandleCreated && Visible)
            {
                NativeMethods.SetWindowPos(Handle, NativeMethods.HWND_TOPMOST, 0, 0, 0, 0,
                    NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOACTIVATE);
            }
        }
    }

    // =========================================================================
    // CORNER OVERLAY MANAGER PER MONITOR (0% CPU IDLE CACHING)
    // =========================================================================
    public enum CornerPosition { TopLeft = 0, TopRight = 1, BottomLeft = 2, BottomRight = 3 }

    public class MonitorCorners
    {
        public Screen Monitor;
        public Rectangle Bounds;
        public LayeredOverlayForm[] CornerForms = new LayeredOverlayForm[4];
        public float PulseGlowAmount = 0f;

        public MonitorCorners(Screen screen)
        {
            Monitor = screen;
            Bounds = screen.Bounds;
            for (int i = 0; i < 4; i++)
            {
                CornerForms[i] = new LayeredOverlayForm(true);
                CornerForms[i].Show();
            }
        }

        public void UpdateCorners(AppSettings settings, Color[] themeColors, float animPhase, float audioPeak)
        {
            if (!settings.Enabled || settings.CornerRadius <= 2)
            {
                for (int i = 0; i < 4; i++) CornerForms[i].Hide();
                return;
            }

            bool[] active = new bool[] { settings.TopLeft, settings.TopRight, settings.BottomLeft, settings.BottomRight };
            int r = settings.CornerRadius;
            float beatExtra = (settings.CornerAnim == CornerEffectMode.AudioPulseSync) ? (audioPeak * 0.7f) : 0f;
            float totalGlow = Math.Min(1.1f, PulseGlowAmount + beatExtra);

            int glowExtra = 22;
            int effectiveRadius = r + (int)(PulseGlowAmount * 5f) + (int)(beatExtra * 4f);
            int boxSize = effectiveRadius + glowExtra;

            for (int i = 0; i < 4; i++)
            {
                if (!active[i]) { CornerForms[i].Hide(); continue; }
                if (!CornerForms[i].Visible) CornerForms[i].Show();

                CornerPosition pos = (CornerPosition)i;
                int x = (pos == CornerPosition.TopRight || pos == CornerPosition.BottomRight) ? (Bounds.Right - boxSize) : Bounds.Left;
                int y = (pos == CornerPosition.BottomLeft || pos == CornerPosition.BottomRight) ? (Bounds.Bottom - boxSize) : Bounds.Top;

                using (Bitmap bmp = RenderCornerBitmap(boxSize, effectiveRadius, pos, settings, totalGlow, themeColors, animPhase, audioPeak))
                {
                    CornerForms[i].SetBitmap(bmp, x, y, 255);
                }
            }
        }

        public static Bitmap RenderCornerBitmap(
            int size, int radius, CornerPosition pos, AppSettings settings,
            float pulseGlow, Color[] themeColors, float animPhase = 0f, float audioPeak = 0f)
        {
            Bitmap bmp = new Bitmap(size, size, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.Clear(Color.Transparent);

                using (GraphicsPath cornerMask = CreateCornerMaskPath(size, radius, pos, settings.Curve))
                using (GraphicsPath curveLine = CreateCornerCurvePath(size, radius, pos, settings.Curve))
                {
                    Color cPrimary = (themeColors != null && themeColors.Length > 0) ? themeColors[0] : Color.White;
                    Color cSecondary = (themeColors != null && themeColors.Length > 1) ? themeColors[1] : Color.Silver;
                    float cornerOffset = (int)pos * 0.8f;

                    float animGlowBoost = 0f;
                    if (settings.CornerAnim == CornerEffectMode.PrismaticFlow && themeColors != null)
                    {
                        float wave = (float)(0.5 + 0.5 * Math.Sin(animPhase * 2.0f + cornerOffset));
                        int idxA = ((int)(animPhase + (int)pos)) % themeColors.Length;
                        int idxB = (idxA + 1) % themeColors.Length;
                        cPrimary = GlassThemeEngine.Interpolate(themeColors[idxA], themeColors[idxB], wave);
                        animGlowBoost = 0.28f;
                    }
                    else if (settings.CornerAnim == CornerEffectMode.SoftBreathing)
                    {
                        float breath = (float)(0.5 + 0.5 * Math.Sin(animPhase * 1.8f));
                        cPrimary = GlassThemeEngine.Interpolate(cPrimary, cSecondary, breath);
                        animGlowBoost = 0.12f + 0.35f * breath;
                    }
                    else if (settings.CornerAnim == CornerEffectMode.AudioPulseSync)
                    {
                        cPrimary = GlassThemeEngine.Interpolate(cPrimary, cSecondary, Math.Min(1f, audioPeak * 1.3f));
                        animGlowBoost = 0.10f + audioPeak * 0.85f;
                    }
                    else if (settings.CornerAnim == CornerEffectMode.HorizonWave)
                    {
                        float chase = (float)Math.Max(0.0, Math.Sin(animPhase * 2.6f - (int)pos * 1.57f));
                        animGlowBoost = chase * 0.6f;
                    }

                    float effectiveGlow = Math.Min(1.0f, pulseGlow + animGlowBoost * (settings.BezelGlowOpacity / 100f));

                    // 1. Soft Frosted Glass Ambient Aura inside the curve
                    if (settings.CornerBezelGlow && effectiveGlow > 0.02f)
                    {
                        for (int layer = 3; layer >= 1; layer--)
                        {
                            float width = layer * 3.2f * Math.Max(0.5f, effectiveGlow);
                            int alpha = (int)(Math.Min(1f, effectiveGlow) * (75 / layer));
                            if (alpha <= 2) continue;
                            using (Pen glowPen = new Pen(Color.FromArgb(Math.Min(255, alpha), cPrimary), width))
                            {
                                glowPen.StartCap = LineCap.Round;
                                glowPen.EndCap = LineCap.Round;
                                g.DrawPath(glowPen, curveLine);
                            }
                        }
                    }

                    // 2. Crisp Minimalist Glass Specular Rim
                    if (settings.CornerBezelGlow && settings.BezelGlowOpacity > 0)
                    {
                        int bezelAlpha = (int)(settings.BezelGlowOpacity * 2.2f);
                        if (bezelAlpha > 255) bezelAlpha = 255;
                        Color bezelColor = (settings.CornerAnim == CornerEffectMode.MinimalFrost && pulseGlow <= 0.05f)
                            ? GlassThemeEngine.Interpolate(Color.FromArgb(bezelAlpha, 235, 242, 255), Color.FromArgb(bezelAlpha, cPrimary), 0.4f)
                            : Color.FromArgb(bezelAlpha, cPrimary);

                        float penWidth = Math.Max(1.0f, settings.BezelThickness / 10f);
                        using (Pen bezelPen = new Pen(bezelColor, penWidth))
                        {
                            bezelPen.StartCap = LineCap.Round;
                            bezelPen.EndCap = LineCap.Round;
                            g.DrawPath(bezelPen, curveLine);
                        }
                    }

                    // 3. Pure Pitch-Black Corner Mask
                    using (SolidBrush blackBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0)))
                    {
                        g.FillPath(blackBrush, cornerMask);
                    }
                }
            }
            return bmp;
        }

        private static PointF[] GetCornerCurvePoints(int size, int radius, CornerPosition pos, CurveGeometry curveType)
        {
            int steps = 36;
            PointF[] pts = new PointF[steps + 1];
            double n = (curveType == CurveGeometry.HyperbolicSquircle) ? 4.2 : 2.0;

            float cx = (pos == CornerPosition.TopRight || pos == CornerPosition.BottomRight) ? size : 0;
            float cy = (pos == CornerPosition.BottomLeft || pos == CornerPosition.BottomRight) ? size : 0;
            float dx = (pos == CornerPosition.TopRight || pos == CornerPosition.BottomRight) ? -1f : 1f;
            float dy = (pos == CornerPosition.BottomLeft || pos == CornerPosition.BottomRight) ? -1f : 1f;

            float centerX = cx + dx * radius;
            float centerY = cy + dy * radius;

            for (int i = 0; i <= steps; i++)
            {
                double t = (Math.PI / 2.0) * ((double)i / steps);
                double u = Math.Pow(Math.Cos(t), 2.0 / n) * radius;
                double v = Math.Pow(Math.Sin(t), 2.0 / n) * radius;
                pts[i] = new PointF(centerX - dx * (float)u, centerY - dy * (float)v);
            }
            return pts;
        }

        private static GraphicsPath CreateCornerCurvePath(int size, int radius, CornerPosition pos, CurveGeometry curveType)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLines(GetCornerCurvePoints(size, radius, pos, curveType));
            return path;
        }

        private static GraphicsPath CreateCornerMaskPath(int size, int radius, CornerPosition pos, CurveGeometry curveType)
        {
            GraphicsPath path = new GraphicsPath();
            PointF[] pts = GetCornerCurvePoints(size, radius, pos, curveType);
            path.AddLines(pts);

            float cx = (pos == CornerPosition.TopRight || pos == CornerPosition.BottomRight) ? size : 0;
            float cy = (pos == CornerPosition.BottomLeft || pos == CornerPosition.BottomRight) ? size : 0;
            float ox = (cx == 0) ? -1f : size + 1f;
            float oy = (cy == 0) ? -1f : size + 1f;

            path.AddLine(pts[pts.Length - 1], new PointF(ox, oy));
            path.AddLine(new PointF(ox, oy), pts[0]);
            path.CloseFigure();
            return path;
        }

        public void EnsureTopMost()
        {
            for (int i = 0; i < 4; i++)
                if (CornerForms[i] != null) CornerForms[i].EnsureTopMost();
        }

        public void Dispose()
        {
            for (int i = 0; i < 4; i++)
                if (CornerForms[i] != null) { CornerForms[i].Close(); CornerForms[i].Dispose(); }
        }
    }

    // =========================================================================
    // GLASSMORPHIC AI CHAT POPUP
    // =========================================================================
    public class GlassAiChatPopup : Form
    {
        private TextBox inputBox;
        private Label responseLabel;
        private ComboBox providerCombo;
        private AppSettings settings;
        private AiAssistantEngine aiEngine;
        private GlassCompanionEngine companion;

        public GlassAiChatPopup(AppSettings s, AiAssistantEngine ai, GlassCompanionEngine comp, Point topCenterScreen)
        {
            settings = s;
            aiEngine = ai;
            companion = comp;

            Text = "Nova IA — Dialogue Rapide";
            Size = new Size(450, 260);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(topCenterScreen.X - 225, topCenterScreen.Y + 205);
            BackColor = Color.FromArgb(14, 16, 24);
            ForeColor = Color.FromArgb(240, 244, 255);
            TopMost = true;
            Font = new Font("Segoe UI", 9.2f);

            Label topLbl = new Label
            {
                Text = "✦ Dialogue avec " + s.CompanionName + " :",
                Location = new Point(14, 14),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(140, 215, 255)
            };
            Controls.Add(topLbl);

            providerCombo = new ComboBox
            {
                Location = new Point(265, 11),
                Width = 155,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(24, 28, 40),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            providerCombo.Items.AddRange(new string[] { "Gemini", "Claude", "ChatGPT", "Mistral" });
            providerCombo.SelectedIndex = (int)s.ActiveAiProvider;
            providerCombo.SelectedIndexChanged += (sender, e) =>
            {
                settings.ActiveAiProvider = (AiService)providerCombo.SelectedIndex;
                settings.Save();
            };
            Controls.Add(providerCombo);

            responseLabel = new Label
            {
                Location = new Point(14, 44),
                Size = new Size(406, 110),
                BackColor = Color.FromArgb(20, 23, 34),
                ForeColor = Color.FromArgb(225, 232, 248),
                Padding = new Padding(10),
                Text = aiEngine.LastResponse
            };
            Controls.Add(responseLabel);

            inputBox = new TextBox
            {
                Location = new Point(14, 166),
                Size = new Size(310, 28),
                BackColor = Color.FromArgb(24, 28, 40),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9.5f)
            };
            inputBox.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; SendCurrentPrompt(); }
            };
            Controls.Add(inputBox);

            Button sendBtn = new Button
            {
                Text = "Envoyer",
                Location = new Point(332, 164),
                Size = new Size(88, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(45, 125, 225),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            sendBtn.FlatAppearance.BorderSize = 0;
            sendBtn.Click += (sender, e) => SendCurrentPrompt();
            Controls.Add(sendBtn);
        }

        private void SendCurrentPrompt()
        {
            string q = inputBox.Text.Trim();
            if (string.IsNullOrEmpty(q)) return;
            inputBox.Text = "";
            responseLabel.Text = "Réflexion en cours (" + settings.ActiveAiProvider + ")...";
            aiEngine.SendPromptAsync(settings, q, reply =>
            {
                if (!IsDisposed && IsHandleCreated)
                {
                    BeginInvoke((Action)(() =>
                    {
                        responseLabel.Text = reply;
                        string shortBubble = reply.Length > 68 ? reply.Substring(0, 65) + "…" : reply;
                        companion.Say(shortBubble, 6000);
                    }));
                }
            });
        }
    }

    // =========================================================================
    // NOVA CAPSULE HUB (MINIMALIST GLASSMORPHISM + AUDIO + TIMER + COMPANION + AI)
    // =========================================================================
    public class NovaCapsuleWidget : IDisposable
    {
        private LayeredOverlayForm capsuleForm;
        public CapsuleTab CurrentTab = CapsuleTab.AudioPlayer;
        public FocusTimerEngine TimerEngine = new FocusTimerEngine();
        public GlassCompanionEngine CompanionEngine = new GlassCompanionEngine();
        public AiAssistantEngine AiEngine = new AiAssistantEngine();

        private float spawnFactor = 1.0f;
        private float expandProgress = 0f;
        private float transitionBoost = 0f;
        private float currentScreenCenterX = -9999f;
        private float currentScreenTopY = 10f;

        private Rectangle currentInteractiveScreenRect = Rectangle.Empty;
        private int hoveredTabIndex = -1;
        private int hoveredActionIndex = -1;
        private bool wasMouseDown = false;
        private string transientBannerText = null;
        private long transientBannerUntilTicks = 0;
        private GlassAiChatPopup activeAiDialog = null;

        // Music track change, app badge animation, and fullscreen morphing states
        private float trackAnim = 0f;
        private float appBadgeAnim = 0f;
        private int lastTrackVersion = -1;
        private AudioAppSource lastSource = AudioAppSource.None;
        private long lastFsCheckTicks = 0;
        private bool fsActive = false;
        private long fsHoldUntilTicks = 0;
        private float fsProgress = 0f;
        private long lastWantVisibleTicks = 0;

        // Cached Fonts to avoid per-frame allocations
        private Font fontSmallBold = new Font("Segoe UI Semibold", 8.0f, FontStyle.Bold);
        private Font fontMedBold = new Font("Segoe UI Semibold", 9.0f, FontStyle.Bold);
        private Font fontTitleBold = new Font("Segoe UI Semibold", 10.2f, FontStyle.Bold);
        private Font fontSubReg = new Font("Segoe UI", 8.4f, FontStyle.Regular);
        private Font fontTimerBig = new Font("Segoe UI Semibold", 21f, FontStyle.Bold);

        public event Action OnOpenSettingsRequested;
        public event Action OnCycleThemeRequested;
        public event Action OnTimerFinishedBurstRequested;

        public NovaCapsuleWidget()
        {
            capsuleForm = new LayeredOverlayForm(true);

            TimerEngine.OnTimerFinished += () =>
            {
                ShowTransientBanner("Minuteur terminé", 4500);
                CompanionEngine.Say("Session terminée ✦", 5000);
                transitionBoost = 1.0f;
                if (OnTimerFinishedBurstRequested != null) OnTimerFinishedBurstRequested();
            };
        }

        public void TriggerTransitionNotification(string screenName, string res)
        {
            transitionBoost = 1.0f;
            ShowTransientBanner(screenName + "  •  " + res, 1900);
        }

        public void HideNow()
        {
            if (capsuleForm != null && capsuleForm.Visible) capsuleForm.Hide();
            spawnFactor = 0f;
            expandProgress = 0f;
        }

        public void ShowTransientBanner(string text, int durationMs)
        {
            transientBannerText = text;
            transientBannerUntilTicks = DateTime.UtcNow.Ticks + durationMs * 10000L;
        }

        public void UpdateAndRender(
            AppSettings settings, Rectangle activeScreenBounds, Rectangle primaryScreenBounds,
            Point cursorPos, LiveMediaAndAudioTracker media, Color[] palette, float globalTime)
        {
            TimerEngine.StepTick();
            CompanionEngine.StepTick(globalTime, media.IsAudioActive);

            if (!settings.Enabled || settings.IslandMode == CapsuleVisibilityMode.Hidden)
            {
                if (capsuleForm.Visible) capsuleForm.Hide();
                spawnFactor = 0f;
                expandProgress = 0f;
                return;
            }

            if (transitionBoost > 0f) transitionBoost = Math.Max(0f, transitionBoost - 0.03f);

            // --- Track change & app detection ---
            bool realSource = media.ActiveSource != AudioAppSource.None && media.ActiveSource != AudioAppSource.SystemSound;
            if (media.TrackVersion != lastTrackVersion)
            {
                if (lastTrackVersion >= 0 && settings.TrackChangeAnimation && realSource)
                    trackAnim = 1f;
                lastTrackVersion = media.TrackVersion;
            }
            if (media.ActiveSource != lastSource)
            {
                if (realSource && settings.ShowDetectedAppBadge)
                {
                    appBadgeAnim = 1f;
                    ShowTransientBanner(LiveMediaAndAudioTracker.GetSourceName(media.ActiveSource) + " détecté", 1700);
                }
                lastSource = media.ActiveSource;
            }
            if (trackAnim > 0f) trackAnim = Math.Max(0f, trackAnim - 0.011f);
            if (appBadgeAnim > 0f) appBadgeAnim = Math.Max(0f, appBadgeAnim - 0.02f);

            Rectangle targetScreen = (settings.IslandPlacement == CapsuleScreenPlacement.PrimaryMonitorFixed)
                ? primaryScreenBounds : activeScreenBounds;

            // --- Fullscreen (immersive) detection every 400 ms ---
            long nowT = DateTime.UtcNow.Ticks;
            if (nowT - lastFsCheckTicks > 4000000L)
            {
                lastFsCheckTicks = nowT;
                string app = "";
                bool fs = settings.FullscreenMorph && NativeMethods.IsForegroundFullscreen(targetScreen, out app);
                if (fs && !fsActive)
                {
                    fsActive = true;
                    fsHoldUntilTicks = nowT + 17000000L;
                    ShowTransientBanner("Mode Immersif  •  " + app, 1600);
                    transitionBoost = 0.8f;
                }
                else if (!fs && fsActive)
                {
                    fsActive = false;
                    transitionBoost = 1.0f;
                    ShowTransientBanner("Retour au bureau", 1300);
                }
            }
            bool peekTop = cursorPos.Y <= targetScreen.Top + 8 && Math.Abs(cursorPos.X - (targetScreen.Left + targetScreen.Width / 2)) < 220;
            float fsTarget = (fsActive && nowT > fsHoldUntilTicks && !peekTop && expandProgress < 0.05f) ? 1f : 0f;
            fsProgress += (fsTarget - fsProgress) * 0.10f;
            if (Math.Abs(fsTarget - fsProgress) < 0.003f) fsProgress = fsTarget;

            bool wantVisible = true;
            if (settings.IslandMode == CapsuleVisibilityMode.Hidden)
                wantVisible = false;
            else if (settings.IslandMode == CapsuleVisibilityMode.TransitionOnly)
                wantVisible = (transitionBoost > 0.02f || TimerEngine.FinishedAlertActive || trackAnim > 0f);
            else if (settings.IslandMode == CapsuleVisibilityMode.SmartContextual)
                wantVisible = (media.IsAudioActive || realSource ||
                               TimerEngine.IsRunning || TimerEngine.FinishedAlertActive ||
                               CompanionEngine.HasActiveSpeech || transitionBoost > 0.02f);
            else if (settings.IslandMode == CapsuleVisibilityMode.TopEdgeHoverWake)
            {
                bool nearTop = (cursorPos.Y <= targetScreen.Top + 42 &&
                                Math.Abs(cursorPos.X - (targetScreen.Left + targetScreen.Width / 2)) < 280);
                wantVisible = nearTop || expandProgress > 0.05f || transitionBoost > 0.02f || TimerEngine.FinishedAlertActive || trackAnim > 0f;
            }

            // Hysteresis: stay visible 2.5 s after the last "wanted" frame => no blinking (only if not hidden)
            if (settings.IslandMode == CapsuleVisibilityMode.Hidden) lastWantVisibleTicks = 0;
            else if (wantVisible) lastWantVisibleTicks = nowT;
            else if (nowT - lastWantVisibleTicks < 25000000L) wantVisible = true;

            float targetSpawn = wantVisible ? 1.0f : 0.0f;
            spawnFactor = spawnFactor * 0.82f + targetSpawn * 0.18f;
            if (spawnFactor <= 0.015f && !wantVisible)
            {
                spawnFactor = 0f;
                if (capsuleForm.Visible) capsuleForm.Hide();
                return;
            }

            float targetCenterX = targetScreen.Left + targetScreen.Width / 2f;
            float targetTopY = targetScreen.Top + 10f;

            if (currentScreenCenterX < -9000f || Math.Abs(currentScreenCenterX - targetCenterX) > 5000f)
            {
                currentScreenCenterX = targetCenterX;
                currentScreenTopY = targetTopY;
            }
            else
            {
                currentScreenCenterX = currentScreenCenterX * 0.80f + targetCenterX * 0.20f;
                currentScreenTopY = currentScreenTopY * 0.80f + targetTopY * 0.20f;
            }

            float scale = settings.IslandScalePercent / 100f;

            bool isHovering = false;
            if (settings.IslandExpandOnHover && currentInteractiveScreenRect != Rectangle.Empty)
            {
                Rectangle hitBox = currentInteractiveScreenRect;
                hitBox.Inflate(10, 8);
                isHovering = hitBox.Contains(cursorPos);
            }

            float targetExpand = isHovering ? 1.0f : 0.0f;
            float expandRate = (targetExpand > expandProgress) ? 0.24f : 0.17f;
            expandProgress += (targetExpand - expandProgress) * expandRate;
            if (Math.Abs(expandProgress - targetExpand) < 0.003f) expandProgress = targetExpand;

            // Spawn transform
            float spawnScaleX = 1.0f, spawnScaleY = 1.0f, spawnOffsetY = 0f;
            if (spawnFactor < 0.99f)
            {
                switch (settings.SpawnAnim)
                {
                    case CapsuleSpawnStyle.LiquidGlassDrop:
                        spawnOffsetY = -42f * (1f - spawnFactor);
                        spawnScaleX = 0.65f + 0.35f * spawnFactor;
                        spawnScaleY = 1.22f - 0.22f * spawnFactor;
                        break;
                    case CapsuleSpawnStyle.HorizontalUnfold:
                        spawnScaleX = 0.20f + 0.80f * spawnFactor;
                        break;
                    case CapsuleSpawnStyle.SoftFadeScale:
                        spawnScaleX = 0.82f + 0.18f * spawnFactor;
                        spawnScaleY = 0.82f + 0.18f * spawnFactor;
                        break;
                    case CapsuleSpawnStyle.PrismSnap:
                        spawnScaleX = 1.25f - 0.25f * spawnFactor;
                        spawnScaleY = Math.Max(0.12f, spawnFactor);
                        break;
                    case CapsuleSpawnStyle.HaloCondense:
                        spawnScaleX = 1.15f - 0.15f * spawnFactor;
                        spawnScaleY = 1.15f - 0.15f * spawnFactor;
                        break;
                }
            }

            float compactW = (TimerEngine.IsRunning ? 305f : 280f) * scale;
            float compactH = 36f * scale;
            float expandedW = 465f * scale;
            float expandedH = 196f * scale;

            float rawPillW = (compactW + (expandedW - compactW) * expandProgress) * (1.0f + transitionBoost * 0.04f);
            float rawPillH = (compactH + (expandedH - compactH) * expandProgress) * (1.0f + transitionBoost * 0.03f);

            float curPillW = Math.Max(40f, rawPillW * spawnScaleX);
            float curPillH = Math.Max(16f, rawPillH * spawnScaleY);

            // Track change: capsule "breathes" wider for a moment
            if (trackAnim > 0f)
            {
                float bump = (float)Math.Sin((1f - trackAnim) * Math.PI);
                curPillW += 52f * scale * bump * (1f - expandProgress);
                curPillH += 5f * scale * bump * (1f - expandProgress);
            }

            // Fullscreen: capsule morphs into a slim luminous "immersive line"
            if (fsProgress > 0.001f)
            {
                float e = fsProgress < 0.5f ? 2f * fsProgress * fsProgress : 1f - (float)Math.Pow(-2f * fsProgress + 2f, 2) / 2f;
                float wPhase = Math.Min(1f, e * 1.4f);           // width shrinks first
                float hPhase = Math.Max(0f, (e - 0.25f) / 0.75f); // then height collapses
                curPillW = curPillW + (130f * scale - curPillW) * wPhase;
                curPillH = curPillH + (5f * scale - curPillH) * hPhase;
            }
            spawnOffsetY -= 7f * scale * fsProgress;

            // Generous canvas margins so companion, aura, and speech bubble NEVER clip!
            int padX = (settings.Companion != CompanionEntity.None) ? (int)(115 * scale) : 28;
            int padTop = (settings.Companion != CompanionEntity.None && settings.CompanionPos == CompanionAnchor.PerchedOnTop)
                ? (int)(54 * scale) : 22;
            int padBottom = 28;

            int winW = (int)curPillW + padX * 2;
            int winH = (int)curPillH + padTop + padBottom;
            int winX = (int)(currentScreenCenterX - winW / 2f);
            int winY = (int)(currentScreenTopY - padTop + spawnOffsetY);

            RectangleF pillRect = new RectangleF(padX, padTop, curPillW, curPillH);
            currentInteractiveScreenRect = new Rectangle(winX + padX - 18, winY + 4, (int)curPillW + 36, (int)curPillH + padTop);

            Point localPt = new Point(cursorPos.X - winX, cursorPos.Y - winY);
            RectangleF companionRect = GetCompanionRect(pillRect, settings, scale);
            Point companionScreenCenter = new Point(
                winX + (int)(companionRect.X + companionRect.Width / 2f),
                winY + (int)(companionRect.Y + companionRect.Height / 2f));

            hoveredTabIndex = -1;
            hoveredActionIndex = -1;
            if (isHovering || (settings.Companion != CompanionEntity.None && companionRect.Contains(localPt.X, localPt.Y)))
            {
                HandleMouseInteraction(localPt, pillRect, companionRect, scale, settings, media, new Point((int)currentScreenCenterX, (int)currentScreenTopY));
            }
            else
            {
                wasMouseDown = (NativeMethods.GetAsyncKeyState(0x01) & 0x8000) != 0;
            }

            using (Bitmap bmp = new Bitmap(winW, winH, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.Clear(Color.Transparent);

                Color accent1 = palette[0];
                Color accent2 = palette[1 % palette.Length];
                if (TimerEngine.FinishedAlertActive)
                {
                    accent1 = Color.FromArgb(255, 255, 175, 70);
                    accent2 = Color.FromArgb(255, 255, 95, 85);
                }
                else if (settings.IslandAutoAdaptBrandColor && media.ActiveSource != AudioAppSource.None)
                {
                    accent1 = GlassThemeEngine.GetAudioSourceColor(media.ActiveSource, palette[0]);
                }

                float cornerRad = Math.Min(curPillH / 2f, (18f + 10f * expandProgress) * scale);
                float contentK = Math.Max(0f, 1f - fsProgress * 2.2f);

                // 1. Soft Ambient Outer Aura (Glassmorphic Diffuse Shadow/Glow)
                if (contentK > 0.02f)
                    DrawCapsuleAmbientAura(g, pillRect, cornerRad, settings.IslandBorder, accent1, accent2, palette, globalTime, media.CurrentAudioPeak, transitionBoost + (float)Math.Sin(trackAnim * Math.PI) * 0.5f);

                // 2. Liquid Glass Capsule Surface with droplet meniscus & chromatic edge
                GlassThemeEngine.DrawLiquidGlassSurface(g, pillRect, cornerRad, Color.FromArgb(9, 12, 20), accent1, 245, 1.0f, globalTime);

                // 3. Compact vs Expanded Content
                if (expandProgress < 0.42f)
                {
                    float cAlpha = (1.0f - expandProgress / 0.42f) * spawnFactor * contentK;
                    DrawCompactCapsule(g, pillRect, scale, cAlpha, media, accent1, accent2, globalTime, settings);
                }
                else
                {
                    float eAlpha = ((expandProgress - 0.42f) / 0.58f) * spawnFactor;
                    DrawExpandedGlassHub(g, pillRect, scale, eAlpha, media, accent1, accent2, globalTime, settings, cursorPos, companionScreenCenter);
                }

                // 3b. Immersive fullscreen line
                if (fsProgress > 0.35f)
                    DrawImmersiveLine(g, pillRect, (fsProgress - 0.35f) / 0.65f, globalTime, media.CurrentAudioPeak, accent1, accent2, palette);

                // 4. Companion Entity (when anchored outside capsule)
                if (fsProgress < 0.2f && settings.Companion != CompanionEntity.None && settings.CompanionPos != CompanionAnchor.IntegratedInside)
                {
                    CompanionEngine.DrawCompanion(
                        g, companionRect, settings.Companion, cursorPos, companionScreenCenter,
                        accent1, accent2, globalTime, media.CurrentAudioPeak, settings.CompanionFollowCursorEyes);

                    if (settings.CompanionShowSpeechBubbles && CompanionEngine.HasActiveSpeech)
                    {
                        DrawGlassSpeechPill(g, companionRect, winW, CompanionEngine.SpeechBubbleText, accent1);
                    }
                }

                byte masterAlpha = (byte)Math.Max(0, Math.Min(255, (int)(spawnFactor * 255 * (1f - 0.25f * fsProgress))));
                capsuleForm.SetBitmap(bmp, winX, winY, masterAlpha);
                if (!capsuleForm.Visible) capsuleForm.Show();
            }
        }

        private RectangleF GetCompanionRect(RectangleF pillRect, AppSettings settings, float scale)
        {
            float cSize = 40f * (settings.CompanionScalePercent / 100f) * scale;
            if (settings.CompanionPos == CompanionAnchor.PerchedOnTop)
                return new RectangleF(pillRect.Right - cSize - 20f * scale, pillRect.Y - cSize * 0.76f, cSize, cSize);
            if (settings.CompanionPos == CompanionAnchor.FloatingRight)
                return new RectangleF(pillRect.Right + 10f * scale, pillRect.Y + (pillRect.Height - cSize) / 2f, cSize, cSize);
            return new RectangleF(pillRect.X + 7f * scale, pillRect.Y + (pillRect.Height - cSize * 0.72f) / 2f, cSize * 0.72f, cSize * 0.72f);
        }

        private void DrawGlassSpeechPill(Graphics g, RectangleF compRect, int canvasWidth, string text, Color accent)
        {
            string clean = text.Length > 42 ? text.Substring(0, 40) + "…" : text;
            SizeF sz = g.MeasureString(clean, fontSmallBold);
            float bw = sz.Width + 18f;
            float bh = 22f;
            float bx = Math.Max(6f, Math.Min(canvasWidth - bw - 6f, compRect.X - bw + compRect.Width * 0.4f));
            float by = Math.Max(3f, compRect.Y - bh + 4f);

            RectangleF bRect = new RectangleF(bx, by, bw, bh);
            GlassThemeEngine.DrawFrostedGlassSurface(g, bRect, bh / 2f, Color.FromArgb(14, 17, 26), accent, 240, 0.9f);
            using (SolidBrush tb = new SolidBrush(Color.FromArgb(245, 245, 250, 255)))
            {
                g.DrawString(clean, fontSmallBold, tb, bx + 9f, by + (bh - sz.Height) / 2f);
            }
        }

        private void DrawCompactCapsule(
            Graphics g, RectangleF pill, float scale, float alphaF,
            LiveMediaAndAudioTracker media, Color accent1, Color accent2, float globalTime, AppSettings settings)
        {
            int alpha = (int)(Math.Max(0f, Math.Min(1f, alphaF)) * 255);
            if (alpha <= 5) return;

            float leftOffset = pill.X + pill.Height * 0.26f;

            // Left Icon: Integrated Companion OR Minimalist Progress Ring OR Audio Orb
            if (settings.Companion != CompanionEntity.None && settings.CompanionPos == CompanionAnchor.IntegratedInside)
            {
                float cs = pill.Height * 0.78f;
                RectangleF cr = new RectangleF(leftOffset, pill.Y + (pill.Height - cs) / 2f, cs, cs);
                CompanionEngine.DrawCompanion(g, cr, settings.Companion, Cursor.Position, Point.Empty, accent1, accent2, globalTime, media.CurrentAudioPeak, false);
                leftOffset += cs + 7f * scale;
            }
            else if (TimerEngine.IsRunning || TimerEngine.FinishedAlertActive)
            {
                float ringSize = pill.Height * 0.56f;
                float rx = leftOffset;
                float ry = pill.Y + (pill.Height - ringSize) / 2f;
                using (Pen bgRing = new Pen(Color.FromArgb(alpha / 4, accent1), 2.2f))
                using (Pen fgRing = new Pen(Color.FromArgb(alpha, accent1), 2.2f))
                {
                    g.DrawEllipse(bgRing, rx, ry, ringSize, ringSize);
                    g.DrawArc(fgRing, rx, ry, ringSize, ringSize, -90f, 360f * TimerEngine.ProgressRatio);
                }
                leftOffset += ringSize + 8f * scale;
            }
            else
            {
                float orbSize = pill.Height * 0.62f;
                float oy = pill.Y + (pill.Height - orbSize) / 2f;
                bool hasApp = settings.ShowDetectedAppBadge && media.ActiveSource != AudioAppSource.None && media.ActiveSource != AudioAppSource.SystemSound;
                if (hasApp)
                {
                    float pop = (float)Math.Sin(Math.Max(appBadgeAnim, trackAnim) * Math.PI) * 3f * scale;
                    DrawAppBadge(g, new RectangleF(leftOffset - pop / 2f, oy - pop / 2f, orbSize + pop, orbSize + pop), media.ActiveSource, accent1, accent2, alpha, orbSize * 0.30f);
                }
                else
                {
                    using (LinearGradientBrush orbBrush = new LinearGradientBrush(
                        new RectangleF(leftOffset, oy, orbSize, orbSize),
                        Color.FromArgb(alpha, accent1), Color.FromArgb(alpha, accent2), (globalTime * 90f) % 360f))
                    {
                        g.FillEllipse(orbBrush, leftOffset + orbSize * 0.08f, oy + orbSize * 0.08f, orbSize * 0.84f, orbSize * 0.84f);
                    }
                }
                if (trackAnim > 0f) DrawTrackChangeFx(g, new PointF(leftOffset + orbSize / 2f, oy + orbSize / 2f), pill, scale, accent1, accent2, alphaF);
                leftOffset += orbSize + 9f * scale;
            }

            // Center Label
            string centerText = media.TrackTitle;
            float slide = 0f;
            float textFade = 1f;

            if (transientBannerText != null && DateTime.UtcNow.Ticks < transientBannerUntilTicks)
                centerText = transientBannerText;
            else if (TimerEngine.IsRunning || TimerEngine.FinishedAlertActive)
                centerText = TimerEngine.FormattedTime + "  •  " + TimerEngine.TimerLabel;
            else if (media.ActiveSource == AudioAppSource.None && !media.IsAudioActive && settings.IslandShowClockWhenIdle)
                centerText = DateTime.Now.ToString("HH:mm") + "   •   " + settings.CompanionName;
            else if (trackAnim > 0f)
            {
                centerText = "♪  " + media.TrackTitle;
                float k = Math.Max(0f, (trackAnim - 0.55f) / 0.45f); // first 45% of the anim: slide-in
                slide = 26f * scale * k * k;
                textFade = 1f - k;
            }

            if (centerText.Length > 26) centerText = centerText.Substring(0, 25) + "…";

            using (SolidBrush tb = new SolidBrush(Color.FromArgb((int)(alpha * textFade), 238, 242, 252)))
            {
                SizeF tsz = g.MeasureString(centerText, fontMedBold);
                g.DrawString(centerText, fontMedBold, tb, leftOffset + slide, pill.Y + (pill.Height - tsz.Height) / 2f);
            }

            // Right Minimalist 5-Bar Equalizer (bursts on track change)
            int barCount = 5;
            float barW = 2.6f * scale;
            float barGap = 2.8f * scale;
            float waveStartX = pill.Right - barCount * (barW + barGap) - 12f * scale;
            float maxBarH = pill.Height * 0.48f;
            float burst = 1f + 0.8f * (float)Math.Sin(trackAnim * Math.PI);

            for (int i = 0; i < barCount; i++)
            {
                float bh = Math.Min(pill.Height * 0.72f, Math.Max(3.5f * scale, maxBarH * media.SpectrumBars[i * 2] * burst));
                float bx = waveStartX + i * (barW + barGap);
                float by = pill.Y + (pill.Height - bh) / 2f;
                Color barCol = GlassThemeEngine.Interpolate(accent1, accent2, i / (float)(barCount - 1));
                using (GraphicsPath bp = GlassThemeEngine.CreateRoundedRect(new RectangleF(bx, by, barW, bh), barW / 2f))
                using (SolidBrush bb = new SolidBrush(Color.FromArgb(alpha, barCol)))
                {
                    g.FillPath(bb, bp);
                }
            }
        }

        private Font fontBadge = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);
        private Font fontNotes = new Font("Segoe UI Symbol", 10f, FontStyle.Bold);

        private static string GetAppGlyph(AudioAppSource src)
        {
            switch (src)
            {
                case AudioAppSource.Spotify: return "S";
                case AudioAppSource.AppleMusic: return "♫";
                case AudioAppSource.YouTube: return "▶";
                case AudioAppSource.Deezer: return "D";
                case AudioAppSource.SoundCloud: return "☁";
                case AudioAppSource.Tidal: return "◆";
                case AudioAppSource.VLC: return "▲";
                case AudioAppSource.AmazonMusic: return "a";
                case AudioAppSource.MediaPlayer: return "▷";
                case AudioAppSource.WebMusic: return "♪";
                default: return "♪";
            }
        }

        private void DrawAppBadge(Graphics g, RectangleF r, AudioAppSource src, Color accent1, Color accent2, int alpha, float radius)
        {
            Color brand = GlassThemeEngine.GetAudioSourceColor(src, accent1);
            Color brand2 = GlassThemeEngine.Interpolate(brand, src == AudioAppSource.None ? accent2 : Color.Black, 0.35f);
            using (GraphicsPath p = GlassThemeEngine.CreateRoundedRect(r, radius))
            {
                using (LinearGradientBrush b = new LinearGradientBrush(r, Color.FromArgb(alpha, GlassThemeEngine.Interpolate(brand, Color.White, 0.12f)), Color.FromArgb(alpha, brand2), 60f))
                    g.FillPath(b, p);
                using (Pen rim = new Pen(Color.FromArgb(alpha / 3, 255, 255, 255), 1f))
                    g.DrawPath(rim, p);
            }
            string glyph = GetAppGlyph(src);
            using (Font f = new Font(glyph.Length == 1 && char.IsLetter(glyph[0]) ? "Segoe UI Black" : "Segoe UI Symbol", Math.Max(6f, r.Height * 0.42f), FontStyle.Bold, GraphicsUnit.Pixel))
            using (SolidBrush wb = new SolidBrush(Color.FromArgb(alpha, Color.White)))
            {
                SizeF sz = g.MeasureString(glyph, f);
                g.DrawString(glyph, f, wb, r.X + (r.Width - sz.Width) / 2f + 0.5f, r.Y + (r.Height - sz.Height) / 2f);
            }
        }

        private void DrawTrackChangeFx(Graphics g, PointF center, RectangleF pill, float scale, Color accent1, Color accent2, float alphaF)
        {
            float p = 1f - trackAnim; // 0 -> 1
            // Expanding glass ripples
            for (int ring = 0; ring < 2; ring++)
            {
                float rp = p * 1.3f - ring * 0.25f;
                if (rp <= 0f || rp >= 1f) continue;
                float rad = (8f + 42f * rp) * scale;
                int a = (int)((1f - rp) * 170 * alphaF);
                using (Pen pen = new Pen(Color.FromArgb(Math.Max(0, a), ring == 0 ? accent1 : accent2), (2.2f - 1.4f * rp) * scale))
                    g.DrawEllipse(pen, center.X - rad, center.Y - rad, rad * 2f, rad * 2f);
            }
            // Floating music notes rising above the capsule
            string[] notes = new string[] { "♪", "♫", "♪", "♬" };
            for (int i = 0; i < notes.Length; i++)
            {
                float np = p * 1.25f - i * 0.12f;
                if (np <= 0f || np >= 1f) continue;
                float nx = center.X + (14f + i * 16f) * scale + (float)Math.Sin(np * 6.0 + i) * 5f * scale;
                float ny = pill.Y + pill.Height * 0.5f - np * (pill.Height * 0.5f + 20f * scale);
                int a = (int)(Math.Sin(np * Math.PI) * 240 * alphaF);
                using (SolidBrush nb = new SolidBrush(Color.FromArgb(Math.Max(0, a), GlassThemeEngine.Interpolate(accent1, accent2, i / 3f))))
                    g.DrawString(notes[i], fontNotes, nb, nx, ny);
            }
        }

        private void DrawImmersiveLine(Graphics g, RectangleF pill, float k, float globalTime, float audioPeak, Color accent1, Color accent2, Color[] palette)
        {
            if (k <= 0.01f) return;
            int a = (int)(Math.Min(1f, k) * 255);
            float shift = (float)((globalTime * 0.6) % 1.0);
            using (GraphicsPath p = GlassThemeEngine.CreateRoundedRect(pill, pill.Height / 2f))
            using (LinearGradientBrush lg = new LinearGradientBrush(pill, accent1, accent2, 0f))
            {
                ColorBlend cb = new ColorBlend(5);
                Color c0 = palette[0], c1 = palette[1 % palette.Length], c2 = palette[2 % palette.Length];
                cb.Colors = new Color[] { Color.FromArgb(a, c0), Color.FromArgb(a, c1), Color.FromArgb(a, c2), Color.FromArgb(a, c1), Color.FromArgb(a, c0) };
                cb.Positions = new float[] { 0f, 0.1f + shift * 0.35f, 0.5f, Math.Max(0.51f, Math.Min(0.99f, 0.9f - shift * 0.4f)), 1f };
                lg.InterpolationColors = cb;
                g.FillPath(lg, p);
            }
            // audio-reactive glow halo under the line
            float glow = 3f + audioPeak * 10f;
            using (GraphicsPath gp = GlassThemeEngine.CreateRoundedRect(RectangleF.Inflate(pill, glow, glow * 0.6f), pill.Height / 2f + glow))
            using (PathGradientBrush pg = new PathGradientBrush(gp))
            {
                pg.CenterColor = Color.FromArgb((int)(a * 0.35f), accent1);
                pg.SurroundColors = new Color[] { Color.FromArgb(0, accent1) };
                g.FillPath(pg, gp);
            }
        }

        private void DrawExpandedGlassHub(
            Graphics g, RectangleF pill, float scale, float alphaF,
            LiveMediaAndAudioTracker media, Color accent1, Color accent2,
            float globalTime, AppSettings settings, Point cursorPos, Point companionCenter)
        {
            int alpha = (int)(Math.Max(0f, Math.Min(1f, alphaF)) * 255);
            if (alpha <= 5) return;

            // Top Glass Tab Bar
            RectangleF[] tabRects = GetTopTabRects(pill, scale);
            string[] tabTitles = new string[] { "🎵 Audio", "⏱️ Focus", "🐾 Mascotte", "✨ IA", "⚙" };

            for (int i = 0; i < tabRects.Length; i++)
            {
                bool isSelected = (i < 4 && (int)CurrentTab == i);
                bool isHovered = (hoveredTabIndex == i);
                RectangleF tr = tabRects[i];

                using (GraphicsPath tp = GlassThemeEngine.CreateRoundedRect(tr, tr.Height / 2f))
                {
                    Color bg = isSelected
                        ? Color.FromArgb((int)(alpha * 0.32f), accent1)
                        : (isHovered ? Color.FromArgb((int)(alpha * 0.22f), 255, 255, 255) : Color.FromArgb((int)(alpha * 0.10f), 255, 255, 255));
                    using (SolidBrush b = new SolidBrush(bg)) g.FillPath(b, tp);

                    if (isSelected || isHovered)
                    {
                        using (Pen p = new Pen(Color.FromArgb((int)(alpha * 0.55f), isSelected ? accent1 : Color.White), 1.0f))
                            g.DrawPath(p, tp);
                    }
                }

                SizeF sz = g.MeasureString(tabTitles[i], fontSmallBold);
                using (SolidBrush tb = new SolidBrush(Color.FromArgb(alpha, isSelected ? Color.White : Color.FromArgb(200, 210, 230))))
                {
                    g.DrawString(tabTitles[i], fontSmallBold, tb, tr.X + (tr.Width - sz.Width) / 2f, tr.Y + (tr.Height - sz.Height) / 2f);
                }
            }

            if (CurrentTab == CapsuleTab.AudioPlayer)
                DrawAudioTabBody(g, pill, scale, alpha, media, accent1, accent2);
            else if (CurrentTab == CapsuleTab.FocusTimer)
                DrawTimerTabBody(g, pill, scale, alpha, accent1);
            else if (CurrentTab == CapsuleTab.Companion)
                DrawCompanionTabBody(g, pill, scale, alpha, settings, accent1, accent2, globalTime, media.CurrentAudioPeak, cursorPos, companionCenter);
            else if (CurrentTab == CapsuleTab.AiBrain)
                DrawAiTabBody(g, pill, scale, alpha, settings, accent1);
        }

        private void DrawAudioTabBody(Graphics g, RectangleF pill, float scale, int alpha, LiveMediaAndAudioTracker media, Color accent1, Color accent2)
        {
            float pad = 18f * scale;
            float bodyTop = pill.Y + 44f * scale;

            // App artwork tile (detected app badge + track-change pulse)
            float artSize = 46f * scale;
            RectangleF artRect = new RectangleF(pill.X + pad, bodyTop, artSize, artSize);
            float pulse = (float)Math.Sin(trackAnim * Math.PI);
            RectangleF artAnim = RectangleF.Inflate(artRect, pulse * 4f * scale, pulse * 4f * scale);
            DrawAppBadge(g, artAnim, media.ActiveSource, accent1, accent2, alpha, 12f * scale);

            // Track Metadata
            float textLeft = artRect.Right + 12f * scale;
            using (SolidBrush badgeB = new SolidBrush(Color.FromArgb(alpha, accent1)))
            using (SolidBrush titleB = new SolidBrush(Color.FromArgb(alpha, 245, 247, 255)))
            using (SolidBrush artistB = new SolidBrush(Color.FromArgb((int)(alpha * 0.7f), 175, 185, 210)))
            {
                g.DrawString(GetAudioSourceLabel(media.ActiveSource), fontSmallBold, badgeB, textLeft, bodyTop);
                string tTitle = media.TrackTitle.Length > 30 ? media.TrackTitle.Substring(0, 29) + "…" : media.TrackTitle;
                g.DrawString(tTitle, fontTitleBold, titleB, textLeft, bodyTop + 14f * scale);
                string tArtist = media.TrackArtist.Length > 38 ? media.TrackArtist.Substring(0, 37) + "…" : media.TrackArtist;
                g.DrawString(tArtist, fontSubReg, artistB, textLeft, bodyTop + 32f * scale);
            }

            // 10-Bar Minimalist Spectrum on Right
            int specCount = 10;
            float sbW = 3.0f * scale;
            float sbGap = 2.6f * scale;
            float specRightX = pill.Right - pad - specCount * (sbW + sbGap);
            float specCenterY = bodyTop + 23f * scale;
            for (int i = 0; i < specCount; i++)
            {
                float bh = Math.Max(4f * scale, 32f * scale * media.SpectrumBars[i]);
                Color bc = GlassThemeEngine.Interpolate(accent1, accent2, i / (float)(specCount - 1));
                using (SolidBrush b = new SolidBrush(Color.FromArgb(alpha, bc)))
                    g.FillRectangle(b, specRightX + i * (sbW + sbGap), specCenterY - bh / 2f, sbW, bh);
            }

            RectangleF[] btns = GetActionButtonsForCurrentTab(pill, scale);
            string[] labels = new string[] { "⏮", "⏯", "⏭", "🔉", "🔊", "Spotify", "Apple Music", "YouTube", "Palette" };
            DrawGlassButtonsGrid(g, btns, labels, alpha, accent1, 1);
        }

        private void DrawTimerTabBody(Graphics g, RectangleF pill, float scale, int alpha, Color accent1)
        {
            float bodyTop = pill.Y + 44f * scale;
            float ringSize = 56f * scale;
            float rx = pill.X + 24f * scale;
            float ry = bodyTop + 4f * scale;

            using (Pen bgRing = new Pen(Color.FromArgb(alpha / 5, accent1), 4.0f * scale))
            using (Pen fgRing = new Pen(Color.FromArgb(alpha, accent1), 4.0f * scale))
            {
                fgRing.StartCap = LineCap.Round;
                fgRing.EndCap = LineCap.Round;
                g.DrawEllipse(bgRing, rx, ry, ringSize, ringSize);
                g.DrawArc(fgRing, rx, ry, ringSize, ringSize, -90f, 360f * TimerEngine.ProgressRatio);
            }

            using (SolidBrush accB = new SolidBrush(Color.FromArgb(alpha, accent1)))
            using (SolidBrush whiteB = new SolidBrush(Color.FromArgb(alpha, Color.White)))
            {
                g.DrawString(TimerEngine.FormattedTime, fontTimerBig, whiteB, rx + ringSize + 16f * scale, ry + 1f * scale);
                string status = (TimerEngine.IsRunning ? "ACTIF  •  " : "PAUSE  •  ") + TimerEngine.TimerLabel.ToUpper();
                g.DrawString(status, fontSmallBold, accB, rx + ringSize + 18f * scale, ry + 39f * scale);
            }

            RectangleF[] btns = GetActionButtonsForCurrentTab(pill, scale);
            string[] labels = new string[] {
                TimerEngine.IsRunning ? "Pause" : "Démarrer",
                "Reset", "1 min", "5 min", "Focus 25m", "+1 min", "Chrono"
            };
            DrawGlassButtonsGrid(g, btns, labels, alpha, accent1, 0);
        }

        private void DrawCompanionTabBody(
            Graphics g, RectangleF pill, float scale, int alpha, AppSettings settings,
            Color accent1, Color accent2, float globalTime, float audioPeak, Point cursorPos, Point companionCenter)
        {
            float bodyTop = pill.Y + 44f * scale;
            RectangleF avatarBox = new RectangleF(pill.X + 20f * scale, bodyTop, 62f * scale, 62f * scale);
            GlassThemeEngine.DrawFrostedGlassSurface(g, avatarBox, 14f * scale, Color.FromArgb(18, 22, 32), accent1, alpha, 0.8f);

            CompanionEntity ent = (settings.Companion == CompanionEntity.None) ? CompanionEntity.LuminaOrb : settings.Companion;
            CompanionEngine.DrawCompanion(g, avatarBox, ent, cursorPos, companionCenter, accent1, accent2, globalTime, audioPeak, true);

            float tx = avatarBox.Right + 14f * scale;
            using (SolidBrush wb = new SolidBrush(Color.FromArgb(alpha, Color.White)))
            using (SolidBrush cb = new SolidBrush(Color.FromArgb(alpha, accent1)))
            {
                g.DrawString(settings.CompanionName + "   •   Niv. " + CompanionEngine.Level + " (" + CompanionEngine.Xp + "%)", fontTitleBold, wb, tx, bodyTop + 2f * scale);
                g.DrawString("Entité : " + ent + "   •   État : " + CompanionEngine.Mood, fontSubReg, cb, tx, bodyTop + 23f * scale);
                string quote = CompanionEngine.HasActiveSpeech ? CompanionEngine.SpeechBubbleText : "Fais-moi un câlin ou découvre mes animations 💖";
                if (quote.Length > 48) quote = quote.Substring(0, 46) + "…";
                g.DrawString(quote, fontSubReg, wb, tx, bodyTop + 42f * scale);
            }

            RectangleF[] btns = GetActionButtonsForCurrentTab(pill, scale);
            string[] labels = new string[] { "💖 Câlin (+20 XP)", "🐾 Mascotte Suivante", "📍 Ancrage (" + settings.CompanionPos + ")", "✨ Dialogue IA" };
            DrawGlassButtonsGrid(g, btns, labels, alpha, accent1, 0);
        }

        private void DrawAiTabBody(Graphics g, RectangleF pill, float scale, int alpha, AppSettings settings, Color accent1)
        {
            float bodyTop = pill.Y + 44f * scale;
            RectangleF respBox = new RectangleF(pill.X + 18f * scale, bodyTop, pill.Width - 36f * scale, 64f * scale);
            GlassThemeEngine.DrawFrostedGlassSurface(g, respBox, 12f * scale, Color.FromArgb(16, 19, 28), accent1, alpha, 0.7f);

            using (SolidBrush tb = new SolidBrush(Color.FromArgb(alpha, 232, 238, 252)))
            {
                RectangleF textArea = RectangleF.Inflate(respBox, -10f * scale, -7f * scale);
                g.DrawString(AiEngine.LastResponse, fontSubReg, tb, textArea);
            }

            RectangleF[] btns = GetActionButtonsForCurrentTab(pill, scale);
            string[] labels = new string[] { "Ouvrir Dialogue", "Modèle : " + settings.ActiveAiProvider, "Conseil Focus", "Idée Créative" };
            DrawGlassButtonsGrid(g, btns, labels, alpha, accent1, 0);
        }

        private void DrawGlassButtonsGrid(Graphics g, RectangleF[] btns, string[] labels, int alpha, Color accent, int primaryIndex)
        {
            int count = Math.Min(btns.Length, labels.Length);
            for (int i = 0; i < count; i++)
            {
                bool hovered = (hoveredActionIndex == i);
                RectangleF br = btns[i];
                using (GraphicsPath bp = GlassThemeEngine.CreateRoundedRect(br, br.Height / 2f))
                {
                    Color bg = (i == primaryIndex)
                        ? Color.FromArgb((int)(alpha * (hovered ? 0.45f : 0.28f)), accent)
                        : Color.FromArgb((int)(alpha * (hovered ? 0.24f : 0.12f)), 255, 255, 255);
                    using (SolidBrush bb = new SolidBrush(bg)) g.FillPath(bb, bp);

                    using (Pen borderP = new Pen(Color.FromArgb((int)(alpha * (hovered ? 0.75f : 0.25f)), hovered ? accent : Color.White), 1.0f))
                        g.DrawPath(borderP, bp);
                }

                SizeF sz = g.MeasureString(labels[i], fontSmallBold);
                using (SolidBrush fb = new SolidBrush(Color.FromArgb(alpha, Color.White)))
                {
                    g.DrawString(labels[i], fontSmallBold, fb, br.X + (br.Width - sz.Width) / 2f, br.Y + (br.Height - sz.Height) / 2f);
                }
            }
        }

        private RectangleF[] GetTopTabRects(RectangleF pill, float scale)
        {
            float y = pill.Y + 10f * scale;
            float h = 24f * scale;
            float gap = 6f * scale;
            float w1 = 82f * scale, w2 = 82f * scale, w3 = 96f * scale, w4 = 76f * scale, w5 = 32f * scale;
            float total = w1 + w2 + w3 + w4 + w5 + gap * 4;
            float x = pill.X + (pill.Width - total) / 2f;

            RectangleF[] r = new RectangleF[5];
            r[0] = new RectangleF(x, y, w1, h); x += w1 + gap;
            r[1] = new RectangleF(x, y, w2, h); x += w2 + gap;
            r[2] = new RectangleF(x, y, w3, h); x += w3 + gap;
            r[3] = new RectangleF(x, y, w4, h); x += w4 + gap;
            r[4] = new RectangleF(x, y, w5, h);
            return r;
        }

        private RectangleF[] GetActionButtonsForCurrentTab(RectangleF pill, float scale)
        {
            if (CurrentTab == CapsuleTab.AudioPlayer)
            {
                RectangleF[] r = new RectangleF[9];
                float row1Y = pill.Y + 100f * scale;
                float btnH = 30f * scale;
                float smallW = 46f * scale, playW = 64f * scale, gap = 9f * scale;
                float total1 = smallW * 4 + playW + gap * 4;
                float sx = pill.X + (pill.Width - total1) / 2f;

                r[0] = new RectangleF(sx, row1Y, smallW, btnH); sx += smallW + gap;
                r[1] = new RectangleF(sx, row1Y, playW, btnH); sx += playW + gap;
                r[2] = new RectangleF(sx, row1Y, smallW, btnH); sx += smallW + gap;
                r[3] = new RectangleF(sx, row1Y, smallW, btnH); sx += smallW + gap;
                r[4] = new RectangleF(sx, row1Y, smallW, btnH);

                float row2Y = pill.Y + 142f * scale;
                float chipH = 25f * scale, cGap = 8f * scale, cw = 94f * scale;
                float cx = pill.X + (pill.Width - (cw * 4 + cGap * 3)) / 2f;
                for (int i = 0; i < 4; i++) { r[5 + i] = new RectangleF(cx, row2Y, cw, chipH); cx += cw + cGap; }
                return r;
            }
            else if (CurrentTab == CapsuleTab.FocusTimer)
            {
                RectangleF[] r = new RectangleF[7];
                float row1Y = pill.Y + 112f * scale, row2Y = pill.Y + 146f * scale, h = 26f * scale, gap = 8f * scale;
                float w = 96f * scale;
                float sx = pill.X + (pill.Width - (w * 4 + gap * 3)) / 2f;
                r[0] = new RectangleF(sx, row1Y, w, h); sx += w + gap;
                r[1] = new RectangleF(sx, row1Y, w, h); sx += w + gap;
                r[5] = new RectangleF(sx, row1Y, w, h); sx += w + gap;
                r[6] = new RectangleF(sx, row1Y, w, h);

                float w2 = 130f * scale;
                float sx2 = pill.X + (pill.Width - (w2 * 3 + gap * 2)) / 2f;
                r[2] = new RectangleF(sx2, row2Y, w2, h); sx2 += w2 + gap;
                r[3] = new RectangleF(sx2, row2Y, w2, h); sx2 += w2 + gap;
                r[4] = new RectangleF(sx2, row2Y, w2, h);
                return r;
            }
            else
            {
                RectangleF[] r = new RectangleF[4];
                float row1Y = pill.Y + 114f * scale, row2Y = pill.Y + 148f * scale;
                float w = 198f * scale, h = 27f * scale, gap = 10f * scale;
                float sx = pill.X + (pill.Width - (w * 2 + gap)) / 2f;
                r[0] = new RectangleF(sx, row1Y, w, h);
                r[1] = new RectangleF(sx + w + gap, row1Y, w, h);
                r[2] = new RectangleF(sx, row2Y, w, h);
                r[3] = new RectangleF(sx + w + gap, row2Y, w, h);
                return r;
            }
        }

        private void HandleMouseInteraction(
            Point localPt, RectangleF pill, RectangleF companionRect, float scale,
            AppSettings settings, LiveMediaAndAudioTracker media, Point screenTopCenter)
        {
            bool isMouseDown = (NativeMethods.GetAsyncKeyState(0x01) & 0x8000) != 0;
            bool clicked = (isMouseDown && !wasMouseDown);
            wasMouseDown = isMouseDown;

            if (settings.Companion != CompanionEntity.None && companionRect.Contains(localPt.X, localPt.Y))
            {
                if (clicked) CompanionEngine.InteractBoost();
                return;
            }

            if (expandProgress < 0.6f) return;

            RectangleF[] tabs = GetTopTabRects(pill, scale);
            for (int i = 0; i < tabs.Length; i++)
            {
                if (tabs[i].Contains(localPt.X, localPt.Y))
                {
                    hoveredTabIndex = i;
                    if (clicked)
                    {
                        if (i < 4) CurrentTab = (CapsuleTab)i;
                        else if (OnOpenSettingsRequested != null) OnOpenSettingsRequested();
                    }
                    return;
                }
            }

            RectangleF[] btns = GetActionButtonsForCurrentTab(pill, scale);
            for (int i = 0; i < btns.Length; i++)
            {
                if (btns[i].Contains(localPt.X, localPt.Y))
                {
                    hoveredActionIndex = i;
                    if (clicked) ExecuteTabAction(i, settings, media, screenTopCenter);
                    return;
                }
            }
        }

        private void ExecuteTabAction(int idx, AppSettings settings, LiveMediaAndAudioTracker media, Point screenTopCenter)
        {
            if (CurrentTab == CapsuleTab.AudioPlayer)
            {
                switch (idx)
                {
                    case 0: NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PREV_TRACK); break;
                    case 1: NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PLAY_PAUSE); break;
                    case 2: NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_NEXT_TRACK); break;
                    case 3: NativeMethods.SendMediaKey(NativeMethods.VK_VOLUME_DOWN); NativeMethods.SendMediaKey(NativeMethods.VK_VOLUME_DOWN); break;
                    case 4: NativeMethods.SendMediaKey(NativeMethods.VK_VOLUME_UP); NativeMethods.SendMediaKey(NativeMethods.VK_VOLUME_UP); break;
                    case 5: media.LaunchOrFocusService("spotify"); break;
                    case 6: media.LaunchOrFocusService("applemusic"); break;
                    case 7: media.LaunchOrFocusService("youtube"); break;
                    case 8: if (OnCycleThemeRequested != null) OnCycleThemeRequested(); break;
                }
            }
            else if (CurrentTab == CapsuleTab.FocusTimer)
            {
                switch (idx)
                {
                    case 0: TimerEngine.ToggleStartPause(); break;
                    case 1: TimerEngine.Reset(); break;
                    case 2: TimerEngine.StartCountdown(60, "1 Min"); CompanionEngine.Say("Minuteur 1m actif ✦"); break;
                    case 3: TimerEngine.StartCountdown(300, "5 Min"); CompanionEngine.Say("Minuteur 5m actif ✦"); break;
                    case 4: TimerEngine.StartCountdown(1500, "Focus 25m"); CompanionEngine.Say("Session Focus 25m ✦"); break;
                    case 5: TimerEngine.AddSeconds(60); break;
                    case 6: TimerEngine.SwitchToStopwatch(); break;
                }
            }
            else if (CurrentTab == CapsuleTab.Companion)
            {
                switch (idx)
                {
                    case 0: CompanionEngine.InteractBoost(); break;
                    case 1:
                        settings.Companion = (CompanionEntity)(((int)settings.Companion % 6) + 1);
                        settings.Save();
                        CompanionEngine.Say("Forme : " + settings.Companion);
                        break;
                    case 2:
                        settings.CompanionPos = (CompanionAnchor)(((int)settings.CompanionPos + 1) % 3);
                        settings.Save();
                        break;
                    case 3:
                        CurrentTab = CapsuleTab.AiBrain;
                        AiEngine.SendPromptAsync(settings, "Donne une pensée courte et inspirante en une phrase.", reply => CompanionEngine.Say(reply, 5500));
                        break;
                }
            }
            else if (CurrentTab == CapsuleTab.AiBrain)
            {
                switch (idx)
                {
                    case 0: OpenQuickAiDialog(settings, screenTopCenter); break;
                    case 1:
                        settings.ActiveAiProvider = (AiService)(((int)settings.ActiveAiProvider + 1) % 4);
                        settings.Save();
                        break;
                    case 2:
                        AiEngine.SendPromptAsync(settings, "Donne un conseil minimaliste de concentration en une phrase.", reply => CompanionEngine.Say(reply, 5500));
                        break;
                    case 3:
                        AiEngine.SendPromptAsync(settings, "Donne un fait scientifique fascinant en une phrase courte.", reply => CompanionEngine.Say(reply, 5500));
                        break;
                }
            }
        }

        public void OpenQuickAiDialog(AppSettings settings, Point screenTopCenter)
        {
            if (activeAiDialog == null || activeAiDialog.IsDisposed)
                activeAiDialog = new GlassAiChatPopup(settings, AiEngine, CompanionEngine, screenTopCenter);
            activeAiDialog.Show();
            activeAiDialog.BringToFront();
            activeAiDialog.Activate();
        }

        private void DrawCapsuleAmbientAura(
            Graphics g, RectangleF pillRect, float cornerRad,
            CapsuleBorderStyle borderMode, Color accent1, Color accent2, Color[] palette,
            float globalTime, float audioPeak, float transBoost)
        {
            if (borderMode == CapsuleBorderStyle.PureMatteObsidian && transBoost <= 0.02f) return;

            float intensity = 0.35f + transBoost * 0.45f;
            if (borderMode == CapsuleBorderStyle.AudioReactiveRim)
                intensity = 0.20f + audioPeak * 0.95f + transBoost * 0.4f;
            else if (borderMode == CapsuleBorderStyle.FrostedGlassRim)
                intensity = 0.25f + transBoost * 0.45f;

            using (GraphicsPath path = GlassThemeEngine.CreateRoundedRect(pillRect, cornerRad))
            {
                for (int layer = 3; layer >= 1; layer--)
                {
                    int alpha = (int)(Math.Min(1.0f, intensity) * (68 / layer));
                    if (alpha <= 2) continue;
                    using (LinearGradientBrush gb = new LinearGradientBrush(
                        RectangleF.Inflate(pillRect, 10, 10),
                        Color.FromArgb(alpha, accent1), Color.FromArgb(alpha, accent2), (globalTime * 45f) % 360f))
                    using (Pen glowPen = new Pen(gb, layer * 2.6f))
                    {
                        g.DrawPath(glowPen, path);
                    }
                }
            }
        }

        private static string GetAudioSourceLabel(AudioAppSource src)
        {
            if (src == AudioAppSource.None) return "NOVA AUDIO";
            if (src == AudioAppSource.SystemSound) return "FLUX SYSTÈME";
            return LiveMediaAndAudioTracker.GetSourceName(src).ToUpperInvariant() + "  •  EN LECTURE";
        }

        public void EnsureTopMost()
        {
            if (capsuleForm != null) capsuleForm.EnsureTopMost();
        }

        public void Dispose()
        {
            if (activeAiDialog != null && !activeAiDialog.IsDisposed) activeAiDialog.Close();
            if (capsuleForm != null) { capsuleForm.Close(); capsuleForm.Dispose(); }
            fontSmallBold.Dispose(); fontMedBold.Dispose(); fontTitleBold.Dispose(); fontSubReg.Dispose(); fontTimerBig.Dispose();
        }
    }

    // =========================================================================
    // GLASSMORPHIC MULTI-SCREEN TRANSITION ENGINE
    // =========================================================================
    public enum ScreenEdge { Left, Right, Top, Bottom }

    public class ScreenTransitionAnimator
    {
        private LayeredOverlayForm entryEdgeForm;
        private LayeredOverlayForm exitEdgeForm;

        private bool isAnimating = false;
        private bool isProximityActive = false;
        private long animStartTicks = 0;
        private int animDurationMs = 850;

        private Rectangle entryScreenBounds;
        private Rectangle exitScreenBounds;
        private ScreenEdge entryEdge;
        private ScreenEdge exitEdge;
        private Point crossingPoint;

        public MonitorCorners TargetMonitorCorners;

        public ScreenTransitionAnimator()
        {
            entryEdgeForm = new LayeredOverlayForm(true);
            exitEdgeForm = new LayeredOverlayForm(true);
        }

        public bool IsBusy { get { return isAnimating || isProximityActive; } }

        public void TriggerTransition(Rectangle fromBounds, Rectangle toBounds, Point cursorPos, MonitorCorners targetCorners, AppSettings settings)
        {
            if (!settings.Enabled || !settings.EnableTransition) return;

            exitScreenBounds = fromBounds;
            entryScreenBounds = toBounds;
            crossingPoint = cursorPos;
            TargetMonitorCorners = targetCorners;
            animDurationMs = Math.Max(300, settings.DurationMs);
            animStartTicks = DateTime.UtcNow.Ticks;
            isAnimating = true;
            isProximityActive = false;

            int distLeft = Math.Abs(cursorPos.X - toBounds.Left);
            int distRight = Math.Abs(cursorPos.X - toBounds.Right);
            int distTop = Math.Abs(cursorPos.Y - toBounds.Top);
            int distBottom = Math.Abs(cursorPos.Y - toBounds.Bottom);

            int minDist = Math.Min(Math.Min(distLeft, distRight), Math.Min(distTop, distBottom));
            if (minDist == distLeft) { entryEdge = ScreenEdge.Left; exitEdge = ScreenEdge.Right; }
            else if (minDist == distRight) { entryEdge = ScreenEdge.Right; exitEdge = ScreenEdge.Left; }
            else if (minDist == distTop) { entryEdge = ScreenEdge.Top; exitEdge = ScreenEdge.Bottom; }
            else { entryEdge = ScreenEdge.Bottom; exitEdge = ScreenEdge.Top; }
        }

        public void UpdateProximity(Rectangle currentBounds, ScreenEdge nearEdge, Point cursorPos, float proximityFactor, AppSettings settings, Color[] palette)
        {
            if (isAnimating) return;
            if (!settings.Enabled || !settings.EnableTransition || !settings.EnableMagneticProximity || proximityFactor <= 0.02f)
            {
                if (isProximityActive) { entryEdgeForm.Hide(); isProximityActive = false; }
                return;
            }
            isProximityActive = true;
            float scale = settings.IntensityPercent / 100f;
            RenderEdgePortal(entryEdgeForm, currentBounds, nearEdge, cursorPos, proximityFactor * 0.36f, proximityFactor * 0.68f, scale, settings.Style, palette, true, 0f);
        }

        public void StepAnimation(AppSettings settings, Color[] palette)
        {
            if (!isAnimating) return;

            double elapsedMs = (DateTime.UtcNow.Ticks - animStartTicks) / 10000.0;
            float t = (float)(elapsedMs / animDurationMs);

            if (t >= 1.0f)
            {
                isAnimating = false;
                entryEdgeForm.Hide();
                exitEdgeForm.Hide();
                if (TargetMonitorCorners != null) TargetMonitorCorners.PulseGlowAmount = 0f;
                return;
            }

            float scale = settings.IntensityPercent / 100f;
            if (TargetMonitorCorners != null)
            {
                float cornerPulse = (float)(Math.Sin(Math.Min(1.0, t * 1.25) * Math.PI));
                TargetMonitorCorners.PulseGlowAmount = Math.Max(0f, cornerPulse);
            }

            if (settings.Style != PortalTransitionStyle.CapsulePulseOnly)
            {
                float entryEnvelope = (t < 0.22f) ? EaseOutBack(t / 0.22f) : 1.0f - EaseInOutCubic((t - 0.22f) / 0.78f);
                float entryAlpha = (t < 0.14f) ? (t / 0.14f) : (1.0f - (t - 0.14f) / 0.86f);

                RenderEdgePortal(entryEdgeForm, entryScreenBounds, entryEdge, crossingPoint, entryEnvelope, Math.Max(0f, Math.Min(1f, entryAlpha)), scale, settings.Style, palette, false, t);

                if (t < 0.44f && exitScreenBounds != entryScreenBounds)
                {
                    float exitT = t / 0.44f;
                    RenderEdgePortal(exitEdgeForm, exitScreenBounds, exitEdge, crossingPoint, (1.0f - EaseOutCubic(exitT)) * 0.55f, (1.0f - exitT) * 0.75f, scale, settings.Style, palette, true, t);
                }
                else
                {
                    exitEdgeForm.Hide();
                }
            }
            else
            {
                entryEdgeForm.Hide();
                exitEdgeForm.Hide();
            }
        }

        private void RenderEdgePortal(LayeredOverlayForm form, Rectangle screenBounds, ScreenEdge edge, Point cursor, float bulgeAmount, float alphaFactor, float scale, PortalTransitionStyle style, Color[] palette, bool isExitSide, float rawTime)
        {
            if (alphaFactor <= 0.01f || bulgeAmount <= 0.005f) { form.Hide(); return; }

            bool verticalEdge = (edge == ScreenEdge.Left || edge == ScreenEdge.Right);
            int portalLength = (int)(Math.Min(verticalEdge ? screenBounds.Height : screenBounds.Width, (int)(660 * scale)));
            int portalDepth = (int)(180 * scale);
            if (portalLength < 120) portalLength = 120;
            if (portalDepth < 60) portalDepth = 60;

            int winW = verticalEdge ? portalDepth : portalLength;
            int winH = verticalEdge ? portalLength : portalDepth;

            int winX = screenBounds.Left, winY = screenBounds.Top;
            if (edge == ScreenEdge.Left) { winX = screenBounds.Left; winY = Clamp(cursor.Y - winH / 2, screenBounds.Top, screenBounds.Bottom - winH); }
            else if (edge == ScreenEdge.Right) { winX = screenBounds.Right - winW; winY = Clamp(cursor.Y - winH / 2, screenBounds.Top, screenBounds.Bottom - winH); }
            else if (edge == ScreenEdge.Top) { winX = Clamp(cursor.X - winW / 2, screenBounds.Left, screenBounds.Right - winW); winY = screenBounds.Top; }
            else { winX = Clamp(cursor.X - winW / 2, screenBounds.Left, screenBounds.Right - winW); winY = screenBounds.Bottom - winH; }

            using (Bitmap bmp = new Bitmap(winW, winH, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.Clear(Color.Transparent);

                float centerAlong = verticalEdge
                    ? Clamp(cursor.Y - winY, (int)(portalLength * 0.2f), (int)(portalLength * 0.8f))
                    : Clamp(cursor.X - winX, (int)(portalLength * 0.2f), (int)(portalLength * 0.8f));

                if (style == PortalTransitionStyle.GlassMorphCombo || style == PortalTransitionStyle.PrismaticAuraWave || style == PortalTransitionStyle.LiquidGlassBridge || style == PortalTransitionStyle.QuantumArcPortal || style == PortalTransitionStyle.StardustDrift)
                    DrawPrismaticGlassAura(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor, scale, palette, rawTime);

                if (style == PortalTransitionStyle.GlassMorphCombo || style == PortalTransitionStyle.LiquidGlassBridge)
                    DrawFrostedLiquidBridge(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor, scale, palette, isExitSide);

                if (!isExitSide && (style == PortalTransitionStyle.GlassMorphCombo || style == PortalTransitionStyle.StardustDrift))
                    DrawStardustParticles(g, winW, winH, edge, centerAlong, alphaFactor, scale, palette, rawTime);

                if (style == PortalTransitionStyle.QuantumArcPortal)
                    DrawQuantumFilaments(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor, scale, palette, rawTime);

                if (style == PortalTransitionStyle.RippleResonance)
                    DrawGlassRipples(g, winW, winH, edge, centerAlong, alphaFactor, scale, palette, rawTime);

                if (style == PortalTransitionStyle.LaserHorizonLine)
                    DrawHorizonOpticLine(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor, scale, palette);

                form.SetBitmap(bmp, winX, winY, 255);
                if (!form.Visible) form.Show();
            }
        }

        private void DrawPrismaticGlassAura(Graphics g, int w, int h, ScreenEdge edge, float centerAlong, float bulge, float alpha, float scale, Color[] palette, float rawTime)
        {
            bool vertical = (edge == ScreenEdge.Left || edge == ScreenEdge.Right);
            float length = vertical ? h : w;
            float maxReach = (vertical ? w : h) * 0.72f * Math.Min(1.2f, bulge * 1.1f);
            if (maxReach < 6f) return;

            for (int i = 0; i < 3; i++)
            {
                Color c = palette[i % palette.Length];
                float offsetAlong = (float)Math.Sin(rawTime * 7.0 + i * 1.5) * (36f * scale);
                float segHalfLen = (length * (0.40f - i * 0.06f)) * (0.6f + 0.4f * bulge);
                float reach = maxReach * (1.0f - i * 0.18f);
                int layerAlpha = (int)(alpha * (125 - i * 28));
                if (layerAlpha <= 2) continue;

                using (GraphicsPath wavePath = BuildSmoothEdgeBulgePath(w, h, edge, centerAlong + offsetAlong, segHalfLen, reach))
                using (PathGradientBrush pgb = new PathGradientBrush(wavePath))
                {
                    pgb.CenterColor = Color.FromArgb(Math.Min(255, layerAlpha), c);
                    pgb.SurroundColors = new Color[] { Color.FromArgb(0, c) };
                    g.FillPath(pgb, wavePath);
                }
            }
        }

        private void DrawFrostedLiquidBridge(Graphics g, int w, int h, ScreenEdge edge, float centerAlong, float bulge, float alpha, float scale, Color[] palette, bool isExitSide)
        {
            float maxDepth = (isExitSide ? 32f : 52f) * scale * bulge;
            float halfSpan = (isExitSide ? 82f : 112f) * scale * (0.55f + 0.45f * bulge);
            if (maxDepth < 3f) return;

            using (GraphicsPath liquidPath = BuildSmoothEdgeBulgePath(w, h, edge, centerAlong, halfSpan, maxDepth))
            {
                int bodyAlpha = (int)(Math.Min(1.0f, alpha * 1.3f) * 240);
                using (SolidBrush glassBody = new SolidBrush(Color.FromArgb(bodyAlpha, 8, 10, 16)))
                {
                    g.FillPath(glassBody, liquidPath);
                }
                int rimAlpha = (int)(alpha * 195);
                using (Pen specularRim = new Pen(Color.FromArgb(Math.Min(255, rimAlpha), GlassThemeEngine.Interpolate(Color.White, palette[0], 0.45f)), 1.6f))
                {
                    g.DrawPath(specularRim, liquidPath);
                }
            }
        }

        private void DrawStardustParticles(Graphics g, int w, int h, ScreenEdge edge, float centerAlong, float alpha, float scale, Color[] palette, float rawTime)
        {
            for (int i = 0; i < 12; i++)
            {
                float seed = (i * 1.43f) + 0.5f;
                float pTime = rawTime * (0.7f + (i % 4) * 0.2f);
                if (pTime > 1f) continue;

                float dist = (12f + 125f * EaseOutCubic(pTime)) * scale * (0.5f + 0.5f * (float)Math.Sin(seed * 3.1f));
                float spread = (float)Math.Sin(seed * 7.3f) * (150f * scale) * (0.3f + 0.7f * pTime);
                float pRadius = (4.8f - 3.2f * pTime) * scale;
                if (pRadius < 1f) continue;

                PointF pt = GetPointFromEdge(w, h, edge, centerAlong + spread, dist);
                int pAlpha = (int)(alpha * (1.0f - pTime) * 210);
                if (pAlpha <= 4) continue;

                using (SolidBrush coreB = new SolidBrush(Color.FromArgb(pAlpha, GlassThemeEngine.Interpolate(Color.White, palette[i % palette.Length], 0.4f))))
                    g.FillEllipse(coreB, pt.X - pRadius, pt.Y - pRadius, pRadius * 2f, pRadius * 2f);
            }
        }

        private void DrawQuantumFilaments(Graphics g, int w, int h, ScreenEdge edge, float centerAlong, float bulge, float alpha, float scale, Color[] palette, float rawTime)
        {
            for (int arc = 0; arc < 2; arc++)
            {
                PointF[] pts = new PointF[11];
                for (int s = 0; s < pts.Length; s++)
                {
                    float frac = (s / (float)(pts.Length - 1)) - 0.5f;
                    float along = centerAlong + frac * 240f * scale;
                    float wave = (s == 0 || s == pts.Length - 1) ? 2f : (float)Math.Sin(rawTime * 22.0 + s * 1.4 + arc * 2.1) * 24f * scale * bulge + 22f * scale * bulge;
                    pts[s] = GetPointFromEdge(w, h, edge, along, Math.Max(2f, wave));
                }
                int a = (int)(alpha * 195);
                using (Pen p = new Pen(Color.FromArgb(a, GlassThemeEngine.Interpolate(Color.White, palette[arc % palette.Length], 0.5f)), 1.6f * scale))
                    g.DrawCurve(p, pts, 0.4f);
            }
        }

        private void DrawGlassRipples(Graphics g, int w, int h, ScreenEdge edge, float centerAlong, float alpha, float scale, Color[] palette, float rawTime)
        {
            PointF origin = GetPointFromEdge(w, h, edge, centerAlong, 0f);
            for (int ring = 0; ring < 3; ring++)
            {
                float rt = rawTime * 1.25f - ring * 0.18f;
                if (rt <= 0f || rt >= 1f) continue;
                float rad = rt * 155f * scale;
                int rAlpha = (int)((1f - rt) * alpha * 190);
                using (Pen rp = new Pen(Color.FromArgb(rAlpha, GlassThemeEngine.Interpolate(Color.White, palette[ring % palette.Length], 0.5f)), (2.5f - 1.5f * rt) * scale))
                    g.DrawEllipse(rp, origin.X - rad, origin.Y - rad, rad * 2f, rad * 2f);
            }
        }

        private void DrawHorizonOpticLine(Graphics g, int w, int h, ScreenEdge edge, float centerAlong, float bulge, float alpha, float scale, Color[] palette)
        {
            float halfLen = 220f * scale * (0.4f + 0.6f * bulge);
            PointF p1 = GetPointFromEdge(w, h, edge, centerAlong - halfLen, 2.5f);
            PointF p2 = GetPointFromEdge(w, h, edge, centerAlong + halfLen, 2.5f);

            for (int layer = 3; layer >= 1; layer--)
            {
                int a = (int)(alpha * (190 / layer));
                using (Pen p = new Pen(Color.FromArgb(a, layer == 1 ? Color.White : palette[0]), layer * 2.8f * scale))
                {
                    p.StartCap = LineCap.Round; p.EndCap = LineCap.Round;
                    g.DrawLine(p, p1, p2);
                }
            }
        }

        private static GraphicsPath BuildSmoothEdgeBulgePath(int w, int h, ScreenEdge edge, float centerAlong, float halfSpan, float depth)
        {
            GraphicsPath path = new GraphicsPath();
            float start = centerAlong - halfSpan, end = centerAlong + halfSpan;

            if (edge == ScreenEdge.Left)
            {
                path.AddBezier(new PointF(-2f, start), new PointF(-2f, start + halfSpan * 0.55f), new PointF(depth, centerAlong - halfSpan * 0.38f), new PointF(depth, centerAlong));
                path.AddBezier(new PointF(depth, centerAlong), new PointF(depth, centerAlong + halfSpan * 0.38f), new PointF(-2f, end - halfSpan * 0.55f), new PointF(-2f, end));
            }
            else if (edge == ScreenEdge.Right)
            {
                float bx = w + 2f;
                path.AddBezier(new PointF(bx, start), new PointF(bx, start + halfSpan * 0.55f), new PointF(w - depth, centerAlong - halfSpan * 0.38f), new PointF(w - depth, centerAlong));
                path.AddBezier(new PointF(w - depth, centerAlong), new PointF(w - depth, centerAlong + halfSpan * 0.38f), new PointF(bx, end - halfSpan * 0.55f), new PointF(bx, end));
            }
            else if (edge == ScreenEdge.Top)
            {
                path.AddBezier(new PointF(start, -2f), new PointF(start + halfSpan * 0.55f, -2f), new PointF(centerAlong - halfSpan * 0.38f, depth), new PointF(centerAlong, depth));
                path.AddBezier(new PointF(centerAlong, depth), new PointF(centerAlong + halfSpan * 0.38f, depth), new PointF(end - halfSpan * 0.55f, -2f), new PointF(end, -2f));
            }
            else
            {
                float by = h + 2f;
                path.AddBezier(new PointF(start, by), new PointF(start + halfSpan * 0.55f, by), new PointF(centerAlong - halfSpan * 0.38f, h - depth), new PointF(centerAlong, h - depth));
                path.AddBezier(new PointF(centerAlong, h - depth), new PointF(centerAlong + halfSpan * 0.38f, h - depth), new PointF(end - halfSpan * 0.55f, by), new PointF(end, by));
            }
            path.CloseFigure();
            return path;
        }

        private static PointF GetPointFromEdge(int w, int h, ScreenEdge edge, float along, float dist)
        {
            switch (edge)
            {
                case ScreenEdge.Left: return new PointF(dist, along);
                case ScreenEdge.Right: return new PointF(w - dist, along);
                case ScreenEdge.Top: return new PointF(along, dist);
                default: return new PointF(along, h - dist);
            }
        }

        private static int Clamp(float val, int min, int max) { return max < min ? min : (val < min ? min : (val > max ? max : (int)val)); }
        private static float EaseOutCubic(float x) { float f = 1f - x; return 1f - f * f * f; }
        private static float EaseInOutCubic(float x) { return x < 0.5f ? 4f * x * x * x : 1f - (float)Math.Pow(-2f * x + 2f, 3) / 2f; }
        private static float EaseOutBack(float x) { const float c1 = 1.70158f; const float c3 = c1 + 1f; return 1f + c3 * (float)Math.Pow(x - 1f, 3) + c1 * (float)Math.Pow(x - 1f, 2); }

        public void Dispose()
        {
            if (entryEdgeForm != null) { entryEdgeForm.Close(); entryEdgeForm.Dispose(); }
            if (exitEdgeForm != null) { exitEdgeForm.Close(); exitEdgeForm.Dispose(); }
        }
    }

    // =========================================================================
    // CUSTOM GLASSMORPHIC UI CONTROLS FOR STUDIO SETTINGS WINDOW (NO FLICKER!)
    // =========================================================================
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            UpdateStyles();
        }
    }

    public class GlassButton : Control
    {
        private bool hover, pressed;
        private Color tint;
        public GlassButton(string text, int x, int y, int w, int h, Color bg)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            Text = text; Location = new Point(x, y); Size = new Size(w, h);
            tint = bg; ForeColor = Color.White; Cursor = Cursors.Hand;
            Font = new Font("Segoe UI Semibold", 9.0f, FontStyle.Bold);
        }
        public override Color BackColor { get { return tint; } set { tint = value; Invalidate(); } }
        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; pressed = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { pressed = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { pressed = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Color under = Parent != null ? Parent.BackColor : Color.FromArgb(18, 21, 32);
            if (Parent is GlassCardPanel) under = Color.FromArgb(18, 21, 32);
            e.Graphics.Clear(under);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            RectangleF r = new RectangleF(0.5f, 0.5f + (pressed ? 1f : 0f), Width - 1.5f, Height - 1.5f - (pressed ? 1f : 0f));
            Color top = GlassThemeEngine.Interpolate(tint, Color.White, hover ? 0.22f : 0.10f);
            Color bot = GlassThemeEngine.Interpolate(tint, Color.Black, 0.18f);
            using (GraphicsPath p = GlassThemeEngine.CreateRoundedRect(r, Math.Min(12f, r.Height / 2f)))
            {
                using (LinearGradientBrush b = new LinearGradientBrush(r, top, bot, 90f)) g.FillPath(b, p);
                using (Pen rim = new Pen(Color.FromArgb(hover ? 120 : 55, 255, 255, 255), 1f)) g.DrawPath(rim, p);
            }
            TextRenderer.DrawText(g, Text, Font, Rectangle.Round(r), ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }

    public class GlassNavButton : Control
    {
        private bool hover, selected;
        private string glyph;
        private Font glyphFont = new Font("Segoe UI Symbol", 11f);
        public bool Selected { get { return selected; } set { selected = value; Invalidate(); } }
        public GlassNavButton(string glyphText, string text, int x, int y, int w, int h)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            glyph = glyphText; Text = text; Location = new Point(x, y); Size = new Size(w, h);
            Cursor = Cursors.Hand; Font = new Font("Segoe UI Semibold", 9.4f, FontStyle.Bold);
        }
        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(13, 15, 23));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            RectangleF r = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            if (selected || hover)
            {
                using (GraphicsPath p = GlassThemeEngine.CreateRoundedRect(r, 12f))
                {
                    if (selected)
                    {
                        using (LinearGradientBrush b = new LinearGradientBrush(r, Color.FromArgb(70, 80, 170, 255), Color.FromArgb(25, 160, 110, 255), 0f)) g.FillPath(b, p);
                        using (Pen rim = new Pen(Color.FromArgb(90, 140, 210, 255), 1f)) g.DrawPath(rim, p);
                    }
                    else using (SolidBrush b = new SolidBrush(Color.FromArgb(14, 255, 255, 255))) g.FillPath(b, p);
                }
            }
            if (selected)
                using (SolidBrush ind = new SolidBrush(Color.FromArgb(255, 120, 210, 255)))
                using (GraphicsPath ip = GlassThemeEngine.CreateRoundedRect(new RectangleF(4, Height / 2f - 9, 3.5f, 18), 1.75f))
                    g.FillPath(ind, ip);
            Color fc = selected ? Color.White : Color.FromArgb(hover ? 225 : 165, 180, 195, 225);
            using (SolidBrush gb = new SolidBrush(selected ? Color.FromArgb(130, 215, 255) : fc))
                g.DrawString(glyph, glyphFont, gb, 16, (Height - 20) / 2f);
            TextRenderer.DrawText(g, Text, Font, new Rectangle(44, 0, Width - 48, Height), fc, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        }
    }

    public class GlassCardPanel : Panel
    {
        public string CardTitle = "";
        private Font titleFont = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);

        public GlassCardPanel(int x, int y, int w, int h, string title)
        {
            Location = new Point(x, y);
            Size = new Size(w, h);
            CardTitle = title;
            BackColor = Color.FromArgb(11, 13, 20);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(1, 1, Width - 3, Height - 3);
            GlassThemeEngine.DrawFrostedGlassSurface(e.Graphics, r, 20f, Color.FromArgb(18, 21, 32), Color.FromArgb(110, 195, 255), 245, 0.88f);

            if (!string.IsNullOrEmpty(CardTitle))
            {
                using (SolidBrush tb = new SolidBrush(Color.FromArgb(155, 222, 255)))
                {
                    e.Graphics.DrawString(CardTitle, titleFont, tb, 20, 15);
                }
                using (Pen sep = new Pen(Color.FromArgb(28, 255, 255, 255), 1f))
                {
                    e.Graphics.DrawLine(sep, 20, 42, Width - 20, 42);
                }
            }
            base.OnPaint(e);
        }
    }

    public class GlassToggleSwitch : Control
    {
        private bool isChecked = false;
        private float animPos = 0f;
        public event EventHandler CheckedChanged;

        public bool Checked
        {
            get { return isChecked; }
            set { isChecked = value; animPos = value ? 1f : 0f; Invalidate(); }
        }

        public GlassToggleSwitch(string text, int x, int y, int width, bool initial)
        {
            Text = text;
            Location = new Point(x, y);
            Size = new Size(width, 28);
            isChecked = initial;
            animPos = initial ? 1f : 0f;
            Cursor = Cursors.Hand;
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 9.2f);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            isChecked = !isChecked;
            animPos = isChecked ? 1f : 0f;
            Invalidate();
            if (CheckedChanged != null) CheckedChanged(this, EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.FromArgb(18, 21, 32));

            RectangleF pill = new RectangleF(2, 4, 38, 20);
            using (GraphicsPath path = GlassThemeEngine.CreateRoundedRect(pill, 10f))
            {
                Color trackCol = isChecked ? Color.FromArgb(220, 65, 165, 255) : Color.FromArgb(180, 36, 42, 60);
                using (SolidBrush b = new SolidBrush(trackCol)) e.Graphics.FillPath(b, path);
                using (Pen p = new Pen(Color.FromArgb(60, 255, 255, 255), 1f)) e.Graphics.DrawPath(p, path);
            }

            float thumbX = 5f + animPos * 16f;
            using (SolidBrush tb = new SolidBrush(Color.White))
            {
                e.Graphics.FillEllipse(tb, thumbX, 6.5f, 15f, 15f);
            }

            using (SolidBrush textB = new SolidBrush(Color.FromArgb(230, 236, 250)))
            {
                e.Graphics.DrawString(Text, Font, textB, 48, 4);
            }
        }
    }

    public class GlassSlider : Control
    {
        public int Minimum = 0;
        public int Maximum = 100;
        private int val = 50;
        private bool dragging = false;
        public event EventHandler ValueChanged;

        public int Value
        {
            get { return val; }
            set { val = Math.Max(Minimum, Math.Min(Maximum, value)); Invalidate(); }
        }

        public GlassSlider(int x, int y, int width, int min, int max, int initial)
        {
            Location = new Point(x, y);
            Size = new Size(width, 26);
            Minimum = min;
            Maximum = max;
            Value = initial;
            Cursor = Cursors.Hand;
            DoubleBuffered = true;
        }

        protected override void OnMouseDown(MouseEventArgs e) { dragging = true; UpdateFromMouse(e.X); }
        protected override void OnMouseMove(MouseEventArgs e) { if (dragging) UpdateFromMouse(e.X); }
        protected override void OnMouseUp(MouseEventArgs e) { dragging = false; }

        private void UpdateFromMouse(int mouseX)
        {
            float pad = 10f;
            float ratio = (mouseX - pad) / (Width - pad * 2f);
            ratio = Math.Max(0f, Math.Min(1f, ratio));
            int newVal = Minimum + (int)Math.Round(ratio * (Maximum - Minimum));
            if (newVal != val)
            {
                val = newVal;
                Invalidate();
                if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.FromArgb(18, 21, 32));

            float pad = 10f;
            float trackW = Width - pad * 2f;
            float ratio = (Maximum > Minimum) ? (val - Minimum) / (float)(Maximum - Minimum) : 0f;
            float fillW = Math.Max(4f, trackW * ratio);

            RectangleF trackRect = new RectangleF(pad, 10, trackW, 6);
            using (GraphicsPath tp = GlassThemeEngine.CreateRoundedRect(trackRect, 3f))
            using (SolidBrush tb = new SolidBrush(Color.FromArgb(34, 40, 58)))
            {
                e.Graphics.FillPath(tb, tp);
            }

            RectangleF fillRect = new RectangleF(pad, 10, fillW, 6);
            using (GraphicsPath fp = GlassThemeEngine.CreateRoundedRect(fillRect, 3f))
            using (LinearGradientBrush fb = new LinearGradientBrush(fillRect, Color.FromArgb(80, 195, 255), Color.FromArgb(150, 120, 255), 0f))
            {
                e.Graphics.FillPath(fb, fp);
            }

            float thumbX = pad + trackW * ratio;
            using (SolidBrush glow = new SolidBrush(Color.FromArgb(65, 110, 210, 255)))
            using (SolidBrush thumb = new SolidBrush(Color.White))
            {
                e.Graphics.FillEllipse(glow, thumbX - 10f, 3f, 20f, 20f);
                e.Graphics.FillEllipse(thumb, thumbX - 6.5f, 6.5f, 13f, 13f);
            }
        }
    }

    public class GlassRoundedInput : Panel
    {
        private TextBox innerBox;
        private bool isFocused = false;
        private bool isHovered = false;

        public TextBox InnerTextBox { get { return innerBox; } }
        public override string Text
        {
            get { return innerBox != null ? innerBox.Text : base.Text; }
            set { if (innerBox != null) innerBox.Text = value; base.Text = value; }
        }

        public GlassRoundedInput(int x, int y, int w, int h, string val, bool isPassword, bool multiline)
        {
            Location = new Point(x, y);
            Size = new Size(w, h);
            BackColor = Color.FromArgb(18, 21, 32);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            UpdateStyles();

            innerBox = new TextBox
            {
                Location = new Point(14, multiline ? 10 : (h - 20) / 2),
                Size = new Size(w - 28, multiline ? h - 20 : 20),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(20, 24, 38),
                ForeColor = Color.FromArgb(240, 244, 255),
                Font = new Font("Segoe UI", 9.5f),
                Multiline = multiline,
                Text = val
            };
            if (isPassword) innerBox.UseSystemPasswordChar = true;
            if (multiline) innerBox.ScrollBars = ScrollBars.Vertical;

            innerBox.TextChanged += (s, e) => { if (TextChanged != null) TextChanged(this, e); };
            innerBox.Enter += (s, e) => { isFocused = true; Invalidate(); };
            innerBox.Leave += (s, e) => { isFocused = false; Invalidate(); };
            innerBox.MouseEnter += (s, e) => { isHovered = true; Invalidate(); };
            innerBox.MouseLeave += (s, e) => { isHovered = false; Invalidate(); };

            Controls.Add(innerBox);
        }

        public new event EventHandler TextChanged;

        protected override void OnMouseEnter(EventArgs e) { isHovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { isHovered = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnClick(EventArgs e) { innerBox.Focus(); base.OnClick(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(1f, 1f, Width - 2.5f, Height - 2.5f);
            float radius = Math.Min(16f, Height / 2f);

            Color bg = Color.FromArgb(20, 24, 38);
            using (GraphicsPath path = GlassThemeEngine.CreateRoundedRect(r, radius))
            {
                using (SolidBrush b = new SolidBrush(bg)) e.Graphics.FillPath(b, path);

                Color rimCol = isFocused
                    ? Color.FromArgb(190, 80, 185, 255)
                    : (isHovered ? Color.FromArgb(110, 140, 190, 255) : Color.FromArgb(42, 255, 255, 255));
                float rimWidth = isFocused ? 1.6f : 1.0f;
                using (Pen p = new Pen(rimCol, rimWidth)) e.Graphics.DrawPath(p, path);

                // Top delicate specular reflection
                RectangleF specRect = new RectangleF(r.X + 2, r.Y + 1, r.Width - 4, Math.Max(2f, r.Height * 0.35f));
                using (GraphicsPath sp = GlassThemeEngine.CreateRoundedRect(specRect, radius * 0.6f))
                using (LinearGradientBrush sb = new LinearGradientBrush(specRect, Color.FromArgb(18, 255, 255, 255), Color.FromArgb(0, 255, 255, 255), 90f))
                {
                    e.Graphics.FillPath(sb, sp);
                }
            }
        }
    }

    public class GlassRoundedComboBox : Panel
    {
        private ComboBox innerCombo;
        private bool isHovered = false;

        public ComboBox InnerComboBox { get { return innerCombo; } }
        public event EventHandler SelectedIndexChanged;

        public int SelectedIndex
        {
            get { return innerCombo != null ? innerCombo.SelectedIndex : -1; }
            set { if (innerCombo != null) innerCombo.SelectedIndex = value; }
        }

        public GlassRoundedComboBox(int x, int y, int w, int h, string[] items, int selectedIndex)
        {
            Location = new Point(x, y);
            Size = new Size(w, h);
            BackColor = Color.FromArgb(18, 21, 32);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            UpdateStyles();

            innerCombo = new ComboBox
            {
                Location = new Point(12, (h - 22) / 2),
                Width = w - 24,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(20, 24, 38),
                ForeColor = Color.FromArgb(235, 242, 255),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.2f)
            };
            if (items != null) innerCombo.Items.AddRange(items);
            if (selectedIndex >= 0 && items != null && selectedIndex < items.Length)
                innerCombo.SelectedIndex = selectedIndex;
            else if (items != null && items.Length > 0)
                innerCombo.SelectedIndex = 0;

            innerCombo.SelectedIndexChanged += (s, e) => { if (SelectedIndexChanged != null) SelectedIndexChanged(this, e); };
            innerCombo.MouseEnter += (s, e) => { isHovered = true; Invalidate(); };
            innerCombo.MouseLeave += (s, e) => { isHovered = false; Invalidate(); };

            Controls.Add(innerCombo);
        }

        protected override void OnMouseEnter(EventArgs e) { isHovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { isHovered = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(1f, 1f, Width - 2.5f, Height - 2.5f);
            float radius = Math.Min(16f, Height / 2f);

            Color bg = Color.FromArgb(20, 24, 38);
            using (GraphicsPath path = GlassThemeEngine.CreateRoundedRect(r, radius))
            {
                using (SolidBrush b = new SolidBrush(bg)) e.Graphics.FillPath(b, path);

                Color rimCol = isHovered ? Color.FromArgb(120, 140, 200, 255) : Color.FromArgb(42, 255, 255, 255);
                using (Pen p = new Pen(rimCol, 1.0f)) e.Graphics.DrawPath(p, path);

                RectangleF specRect = new RectangleF(r.X + 2, r.Y + 1, r.Width - 4, r.Height * 0.35f);
                using (GraphicsPath sp = GlassThemeEngine.CreateRoundedRect(specRect, radius * 0.6f))
                using (LinearGradientBrush sb = new LinearGradientBrush(specRect, Color.FromArgb(18, 255, 255, 255), Color.FromArgb(0, 255, 255, 255), 90f))
                {
                    e.Graphics.FillPath(sb, sp);
                }
            }
        }
    }

    public class GlassRoundedBox : Panel
    {
        public float CornerRadius = 18f;
        public Color SurfaceColor = Color.FromArgb(16, 20, 30);
        public Color BorderColor = Color.FromArgb(42, 255, 255, 255);

        public GlassRoundedBox(int x, int y, int w, int h, float radius)
        {
            Location = new Point(x, y);
            Size = new Size(w, h);
            CornerRadius = radius;
            BackColor = Color.FromArgb(18, 21, 32);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(1f, 1f, Width - 2.5f, Height - 2.5f);
            using (GraphicsPath path = GlassThemeEngine.CreateRoundedRect(r, CornerRadius))
            {
                using (SolidBrush b = new SolidBrush(SurfaceColor)) e.Graphics.FillPath(b, path);
                using (Pen p = new Pen(BorderColor, 1.0f)) e.Graphics.DrawPath(p, path);

                RectangleF spec = new RectangleF(r.X + 2, r.Y + 1, r.Width - 4, Math.Max(2f, r.Height * 0.28f));
                using (GraphicsPath sp = GlassThemeEngine.CreateRoundedRect(spec, CornerRadius * 0.6f))
                using (LinearGradientBrush sb = new LinearGradientBrush(spec, Color.FromArgb(16, 255, 255, 255), Color.FromArgb(0, 255, 255, 255), 90f))
                {
                    e.Graphics.FillPath(sb, sp);
                }
            }
            base.OnPaint(e);
        }
    }

    // =========================================================================
    // MAIN OVERLAY CONTROLLER
    // =========================================================================
    public class OverlayController : IDisposable
    {
        public AppSettings Settings;
        public List<MonitorCorners> Monitors = new List<MonitorCorners>();
        public ScreenTransitionAnimator Animator;
        public NovaCapsuleWidget DynamicIsland;
        public LiveMediaAndAudioTracker MediaTracker;

        private System.Windows.Forms.Timer tickTimer;
        private int lastMonitorIndex = -1;
        private int topMostTickCounter = 0;
        private int cornerAnimTickCounter = 0;
        private long startTicks = DateTime.UtcNow.Ticks;
        private NotifyIcon trayIcon;
        private SettingsForm settingsForm;

        public event Action OnMonitorsChanged;
        public event Action OnSettingsUpdatedExternally;

        public OverlayController()
        {
            Settings = AppSettings.Load();
            Animator = new ScreenTransitionAnimator();
            DynamicIsland = new NovaCapsuleWidget();
            MediaTracker = new LiveMediaAndAudioTracker();

            DynamicIsland.OnOpenSettingsRequested += ShowSettingsWindow;
            DynamicIsland.OnTimerFinishedBurstRequested += TriggerDemoTransition;
            DynamicIsland.OnCycleThemeRequested += () =>
            {
                int next = ((int)Settings.Theme + 1) % 11;
                Settings.Theme = (GlassColorPalette)next;
                RefreshAllCorners();
                DynamicIsland.ShowTransientBanner("Palette : " + Settings.Theme.ToString(), 1400);
                if (OnSettingsUpdatedExternally != null) OnSettingsUpdatedExternally();
            };

            RebuildMonitors();
            SetupTrayIcon();

            SystemEvents.DisplaySettingsChanged += (s, e) =>
            {
                RebuildMonitors();
                if (OnMonitorsChanged != null) OnMonitorsChanged();
            };

            tickTimer = new System.Windows.Forms.Timer();
            tickTimer.Interval = 16;
            tickTimer.Tick += OnTick;
            tickTimer.Start();
        }

        public float GlobalTimeSeconds { get { return (float)((DateTime.UtcNow.Ticks - startTicks) / 10000000.0); } }

        public void RebuildMonitors()
        {
            foreach (var m in Monitors) m.Dispose();
            Monitors.Clear();

            Screen[] screens = Screen.AllScreens;
            Color[] palette = GlassThemeEngine.GetPalette(Settings, GlobalTimeSeconds);
            for (int i = 0; i < screens.Length; i++)
            {
                MonitorCorners mc = new MonitorCorners(screens[i]);
                mc.UpdateCorners(Settings, palette, GlobalTimeSeconds, MediaTracker.CurrentAudioPeak);
                Monitors.Add(mc);
            }

            NativeMethods.POINT pt;
            if (NativeMethods.GetCursorPos(out pt))
                lastMonitorIndex = FindMonitorIndex(new Point(pt.X, pt.Y));
        }

        public void RefreshAllCorners()
        {
            float t = GlobalTimeSeconds;
            Color[] palette = GlassThemeEngine.GetPalette(Settings, t);
            foreach (var m in Monitors)
                m.UpdateCorners(Settings, palette, t, MediaTracker.CurrentAudioPeak);
            Settings.Save();
        }

        private int FindMonitorIndex(Point pt)
        {
            for (int i = 0; i < Monitors.Count; i++)
                if (Monitors[i].Bounds.Contains(pt)) return i;
            int bestIdx = 0;
            double bestDist = double.MaxValue;
            for (int i = 0; i < Monitors.Count; i++)
            {
                Rectangle b = Monitors[i].Bounds;
                double cx = Math.Max(b.Left, Math.Min(pt.X, b.Right));
                double cy = Math.Max(b.Top, Math.Min(pt.Y, b.Bottom));
                double d = (pt.X - cx) * (pt.X - cx) + (pt.Y - cy) * (pt.Y - cy);
                if (d < bestDist) { bestDist = d; bestIdx = i; }
            }
            return bestIdx;
        }

        private void OnTick(object sender, EventArgs e)
        {
            float t = GlobalTimeSeconds;
            MediaTracker.UpdateTick(t);
            Color[] palette = GlassThemeEngine.GetPalette(Settings, t);

            Point cursorPt = Point.Empty;
            NativeMethods.POINT rawPt;
            if (NativeMethods.GetCursorPos(out rawPt))
                cursorPt = new Point(rawPt.X, rawPt.Y);

            int currentIdx = (Monitors.Count > 0) ? FindMonitorIndex(cursorPt) : -1;

            if (Monitors.Count > 0 && currentIdx >= 0)
            {
                if (lastMonitorIndex != -1 && currentIdx != lastMonitorIndex && lastMonitorIndex < Monitors.Count)
                {
                    Animator.TriggerTransition(Monitors[lastMonitorIndex].Bounds, Monitors[currentIdx].Bounds, cursorPt, Monitors[currentIdx], Settings);
                    Rectangle tb = Monitors[currentIdx].Bounds;
                    DynamicIsland.TriggerTransitionNotification("Écran " + (currentIdx + 1), tb.Width + "×" + tb.Height);
                }
                else if (Monitors.Count > 1)
                {
                    CheckMagneticProximity(currentIdx, cursorPt, palette);
                }
                lastMonitorIndex = currentIdx;
            }

            Animator.StepAnimation(Settings, palette);

            cornerAnimTickCounter++;
            bool needsContinuousCornerUpdate = (Settings.CornerAnim != CornerEffectMode.MinimalFrost || Settings.Theme == GlassColorPalette.ChromaticSpectrum);
            if (Animator.IsBusy || (needsContinuousCornerUpdate && cornerAnimTickCounter % 2 == 0))
            {
                float phase = t * (Settings.CornerAnimSpeed / 100f);
                foreach (var m in Monitors)
                    m.UpdateCorners(Settings, palette, phase, MediaTracker.CurrentAudioPeak);
            }

            if (Monitors.Count > 0)
            {
                Rectangle activeBounds = (currentIdx >= 0 && currentIdx < Monitors.Count) ? Monitors[currentIdx].Bounds : Screen.PrimaryScreen.Bounds;
                DynamicIsland.UpdateAndRender(Settings, activeBounds, Screen.PrimaryScreen.Bounds, cursorPt, MediaTracker, palette, t);
            }

            topMostTickCounter++;
            if (topMostTickCounter >= 90)
            {
                topMostTickCounter = 0;
                foreach (var m in Monitors) m.EnsureTopMost();
                DynamicIsland.EnsureTopMost();
            }
        }

        private void CheckMagneticProximity(int currentIdx, Point pt, Color[] palette)
        {
            Rectangle cur = Monitors[currentIdx].Bounds;
            int threshold = 68;
            float bestProx = 0f;
            ScreenEdge bestEdge = ScreenEdge.Left;

            for (int otherIdx = 0; otherIdx < Monitors.Count; otherIdx++)
            {
                if (otherIdx == currentIdx) continue;
                Rectangle other = Monitors[otherIdx].Bounds;

                if (Math.Abs(other.Right - cur.Left) <= 160 && pt.Y >= other.Top - 80 && pt.Y <= other.Bottom + 80)
                {
                    int dist = pt.X - cur.Left;
                    if (dist >= 0 && dist < threshold) { float p = 1f - (float)dist / threshold; if (p > bestProx) { bestProx = p; bestEdge = ScreenEdge.Left; } }
                }
                if (Math.Abs(other.Left - cur.Right) <= 160 && pt.Y >= other.Top - 80 && pt.Y <= other.Bottom + 80)
                {
                    int dist = cur.Right - pt.X;
                    if (dist >= 0 && dist < threshold) { float p = 1f - (float)dist / threshold; if (p > bestProx) { bestProx = p; bestEdge = ScreenEdge.Right; } }
                }
                if (Math.Abs(other.Bottom - cur.Top) <= 160 && pt.X >= other.Left - 80 && pt.X <= other.Right + 80)
                {
                    int dist = pt.Y - cur.Top;
                    if (dist >= 0 && dist < threshold) { float p = 1f - (float)dist / threshold; if (p > bestProx) { bestProx = p; bestEdge = ScreenEdge.Top; } }
                }
                if (Math.Abs(other.Top - cur.Bottom) <= 160 && pt.X >= other.Left - 80 && pt.X <= other.Right + 80)
                {
                    int dist = cur.Bottom - pt.Y;
                    if (dist >= 0 && dist < threshold) { float p = 1f - (float)dist / threshold; if (p > bestProx) { bestProx = p; bestEdge = ScreenEdge.Bottom; } }
                }
            }
            Animator.UpdateProximity(cur, bestEdge, pt, bestProx, Settings, palette);
        }

        public void TriggerDemoTransition()
        {
            if (Monitors.Count == 0) return;
            int targetIdx = 0;
            NativeMethods.POINT rawPt;
            if (NativeMethods.GetCursorPos(out rawPt))
                targetIdx = FindMonitorIndex(new Point(rawPt.X, rawPt.Y));
            int fromIdx = (Monitors.Count > 1) ? ((targetIdx + 1) % Monitors.Count) : targetIdx;

            Rectangle toB = Monitors[targetIdx].Bounds;
            Rectangle fromB = Monitors[fromIdx].Bounds;
            Point demoPoint = new Point(toB.Left + 4, toB.Top + toB.Height / 2);
            if (Monitors.Count > 1 && fromB.Left > toB.Left)
                demoPoint = new Point(toB.Right - 4, toB.Top + toB.Height / 2);

            Animator.TriggerTransition(fromB, toB, demoPoint, Monitors[targetIdx], Settings);
            DynamicIsland.TriggerTransitionNotification("Écran " + (targetIdx + 1), toB.Width + "×" + toB.Height);
        }

        public static Icon LoadAppIcon()
        {
            try
            {
                string icoFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "icon.ico");
                if (File.Exists(icoFile)) return new Icon(icoFile);
                Icon assoc = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                if (assoc != null) return assoc;
            }
            catch { }
            return null;
        }

        private void SetupTrayIcon()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "iCorner Duo — Glass Studio";
            trayIcon.Icon = LoadAppIcon() ?? SystemIcons.Application;
            trayIcon.Visible = true;

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem openItem = new ToolStripMenuItem("Ouvrir le Studio de Configuration");
            openItem.Font = new Font(openItem.Font, FontStyle.Bold);
            openItem.Click += (s, e) => ShowSettingsWindow();

            ToolStripMenuItem demoItem = new ToolStripMenuItem("Tester la Transition Inter-Écrans");
            demoItem.Click += (s, e) => TriggerDemoTransition();

            ToolStripMenuItem timerItem = new ToolStripMenuItem("Lancer Session Focus (25 min)");
            timerItem.Click += (s, e) => DynamicIsland.TimerEngine.StartCountdown(1500, "Focus 25m");

            ToolStripMenuItem aiItem = new ToolStripMenuItem("Ouvrir Dialogue IA (" + Settings.ActiveAiProvider + ")");
            aiItem.Click += (s, e) => DynamicIsland.OpenQuickAiDialog(Settings, new Point(Screen.PrimaryScreen.Bounds.Width / 2, 10));

            ToolStripMenuItem playPauseItem = new ToolStripMenuItem("Lecture / Pause Audio");
            playPauseItem.Click += (s, e) => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PLAY_PAUSE);

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Quitter iCorner Duo");
            exitItem.Click += (s, e) => { trayIcon.Visible = false; Application.Exit(); };

            menu.Items.Add(openItem);
            menu.Items.Add(demoItem);
            menu.Items.Add(timerItem);
            menu.Items.Add(aiItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(playPauseItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(exitItem);

            trayIcon.ContextMenuStrip = menu;
            trayIcon.DoubleClick += (s, e) => ShowSettingsWindow();
        }

        public void ShowSettingsWindow()
        {
            if (settingsForm == null || settingsForm.IsDisposed)
                settingsForm = new SettingsForm(this);
            settingsForm.Show();
            settingsForm.BringToFront();
            settingsForm.Activate();
        }

        public void Dispose()
        {
            if (tickTimer != null) { tickTimer.Stop(); tickTimer.Dispose(); }
            if (Animator != null) Animator.Dispose();
            if (DynamicIsland != null) DynamicIsland.Dispose();
            foreach (var m in Monitors) m.Dispose();
            if (trayIcon != null) { trayIcon.Visible = false; trayIcon.Dispose(); }
        }
    }

    // =========================================================================
    // MINIMALIST GLASSMORPHIC STUDIO SETTINGS INTERFACE
    // =========================================================================
    public class SettingsForm : Form
    {
        private OverlayController controller;
        private GlassNavButton[] navButtons = new GlassNavButton[5];
        private Panel[] pages = new Panel[5];
        private int activePageIndex = 0;

        private GlassRoundedBox cornerPreviewBox;
        private GlassRoundedBox companionPreviewBox;
        private GlassRoundedComboBox themeCombo;
        private GlassRoundedBox aiStatusBox;
        private Label aiStatusLabel;
        private GlassRoundedInput aiChatTestInput;
        private System.Windows.Forms.Timer uiPreviewTimer;

        public SettingsForm(OverlayController ctrl)
        {
            controller = ctrl;
            Text = "iCorner Duo — Studio Aura Glass";
            Size = new Size(1050, 740);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(11, 13, 20);
            ForeColor = Color.FromArgb(238, 242, 252);
            Font = new Font("Segoe UI", 9.2f, FontStyle.Regular);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            Icon appIco = OverlayController.LoadAppIcon();
            if (appIco != null) Icon = appIco;

            BuildMinimalGlassStudio();

            uiPreviewTimer = new System.Windows.Forms.Timer { Interval = 40 };
            uiPreviewTimer.Tick += (s, e) =>
            {
                if (Visible && WindowState != FormWindowState.Minimized)
                {
                    if (activePageIndex == 0 && cornerPreviewBox != null) cornerPreviewBox.Invalidate();
                    if (activePageIndex == 3 && companionPreviewBox != null) companionPreviewBox.Invalidate();
                    if (headerPanel != null) headerPanel.Invalidate(new Rectangle(0, headerPanel.Height - 3, headerPanel.Width, 3));
                }
            };
            uiPreviewTimer.Start();
        }

        private Panel headerPanel;

        private void BuildMinimalGlassStudio()
        {
            // 1. Minimalist Frosted Header
            Font hdrTitleFont = new Font("Segoe UI Semibold", 16f, FontStyle.Bold);
            Font hdrSubFont = new Font("Segoe UI", 8.8f);
            DoubleBufferedPanel header = new DoubleBufferedPanel
            {
                Dock = DockStyle.Top,
                Height = 74,
                BackColor = Color.FromArgb(12, 14, 22)
            };
            header.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                Rectangle r = header.ClientRectangle;
                using (LinearGradientBrush grad = new LinearGradientBrush(r, Color.FromArgb(14, 18, 30), Color.FromArgb(20, 14, 32), 0f))
                    g.FillRectangle(grad, r);
                // Halo doux derrière l'icône
                using (GraphicsPath hp = new GraphicsPath())
                {
                    hp.AddEllipse(-10, -40, 180, 150);
                    using (PathGradientBrush pg = new PathGradientBrush(hp))
                    {
                        pg.CenterColor = Color.FromArgb(55, 110, 200, 255);
                        pg.SurroundColors = new Color[] { Color.FromArgb(0, 110, 200, 255) };
                        g.FillPath(pg, hp);
                    }
                }
                // Ligne aurore animée
                float t = controller.GlobalTimeSeconds;
                Color[] pal = GlassThemeEngine.GetPalette(controller.Settings, t);
                using (LinearGradientBrush line = new LinearGradientBrush(new Rectangle(0, r.Height - 2, r.Width, 2), pal[0], pal[1 % pal.Length], 0f))
                {
                    ColorBlend cb = new ColorBlend(4);
                    float shift = (float)(0.5 + 0.4 * Math.Sin(t * 1.2));
                    cb.Colors = new Color[] { Color.FromArgb(0, pal[0]), pal[0], pal[1 % pal.Length], Color.FromArgb(0, pal[2 % pal.Length]) };
                    cb.Positions = new float[] { 0f, Math.Max(0.05f, shift - 0.25f), Math.Min(0.95f, shift + 0.25f), 1f };
                    line.InterpolationColors = cb;
                    g.FillRectangle(line, 0, r.Height - 2, r.Width, 2);
                }
                if (Icon != null) g.DrawIcon(Icon, new Rectangle(20, 13, 48, 48));
                using (SolidBrush wb = new SolidBrush(Color.White))
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(160, 175, 205)))
                {
                    g.DrawString("iCorner Duo", hdrTitleFont, wb, 80, 12);
                    SizeF tsz = g.MeasureString("iCorner Duo", hdrTitleFont);
                    using (SolidBrush ab = new SolidBrush(pal[0]))
                        g.DrawString("Aura Glass", hdrTitleFont, ab, 80 + tsz.Width - 6, 12);
                    g.DrawString("Coins Organiques • Passerelle Liquide • Capsule Aura Glass • Mascottes Célestes • Intelligence", hdrSubFont, sb, 83, 45);
                }
            };

            Control testTransBtn = CreateGlassBtn("Tester la Transition", 845, 18, 170, 38, Color.FromArgb(40, 115, 215), () => controller.TriggerDemoTransition());
            header.Controls.Add(testTransBtn);
            Controls.Add(header);
            headerPanel = header;

            // 2. Left Glass Sidebar
            DoubleBufferedPanel sidebar = new DoubleBufferedPanel
            {
                Location = new Point(0, 74),
                Size = new Size(235, 635),
                BackColor = Color.FromArgb(13, 15, 23)
            };
            sidebar.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Color.FromArgb(30, 255, 255, 255), 1f))
                    e.Graphics.DrawLine(p, sidebar.Width - 1, 0, sidebar.Width - 1, sidebar.Height);
            };
            Controls.Add(sidebar);

            string[] navLabels = new string[] {
                "1.  ✦ Dalles & Courbures",
                "2.  🌊 Passerelle Fluide",
                "3.  💧 Capsule Aura Glass",
                "4.  ✨ Mascottes Célestes",
                "5.  🔮 Intelligence & Dialogue"
            };

            string[] navGlyphs = new string[] { "✦", "🌊", "💧", "✨", "🔮" };
            for (int i = 0; i < 5; i++)
            {
                int idx = i;
                GlassNavButton nb = new GlassNavButton(navGlyphs[i], navLabels[i].Substring(4), 12, 18 + i * 52, 210, 44);
                nb.Click += (s, e) => SwitchPage(idx);
                navButtons[i] = nb;
                sidebar.Controls.Add(nb);
            }

            GlassToggleSwitch startupToggle = new GlassToggleSwitch("Démarrer avec Windows", 16, 515, 204, IsStartupEnabled());
            startupToggle.BackColor = Color.FromArgb(13, 15, 23);
            startupToggle.CheckedChanged += (s, e) => SetStartup(startupToggle.Checked);
            sidebar.Controls.Add(startupToggle);

            Control hideTrayBtn = CreateGlassBtn("Réduire en tâche de fond", 14, 558, 206, 36, Color.FromArgb(26, 31, 46), () => Hide());
            sidebar.Controls.Add(hideTrayBtn);

            // 3. Pages
            pages[0] = BuildPage1Corners();
            pages[1] = BuildPage2Transitions();
            pages[2] = BuildPage3NovaCapsule();
            pages[3] = BuildPage4Companion();
            pages[4] = BuildPage5Ai();

            for (int i = 0; i < 5; i++) Controls.Add(pages[i]);
            SwitchPage(0);
        }

        private void SwitchPage(int idx)
        {
            SuspendLayout();
            activePageIndex = idx;
            for (int i = 0; i < 5; i++)
            {
                pages[i].Visible = (i == idx);
                navButtons[i].Selected = (i == idx);
            }
            ResumeLayout(false);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED : rendu composé de toute la fenêtre => zéro clignotement
                return cp;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            try
            {
                int dark = 1;
                NativeMethods.DwmSetWindowAttribute(Handle, 20, ref dark, 4); // barre de titre sombre
                int round = 2;
                NativeMethods.DwmSetWindowAttribute(Handle, 33, ref round, 4); // coins arrondis Windows 11
            }
            catch { }
        }

        private Panel BuildPage1Corners()
        {
            Panel page = CreatePageContainer();

            GlassCardPanel leftCard = new GlassCardPanel(18, 16, 372, 585, "✦ Géométrie Pure & Dalles Moniteurs");
            page.Controls.Add(leftCard);

            GlassToggleSwitch enableChk = new GlassToggleSwitch("Activer les bords arrondis multi-écrans", 18, 54, 330, controller.Settings.Enabled);
            enableChk.CheckedChanged += (s, e) => { controller.Settings.Enabled = enableChk.Checked; controller.RefreshAllCorners(); };
            leftCard.Controls.Add(enableChk);

            leftCard.Controls.Add(CreateLabel("Rayon de courbure (0 à 110 px) :", 18, 96));
            Label radVal = CreateValueLabel(controller.Settings.CornerRadius + " px", 300, 96);
            leftCard.Controls.Add(radVal);

            GlassSlider radSlider = new GlassSlider(18, 120, 336, 0, 110, controller.Settings.CornerRadius);
            radSlider.ValueChanged += (s, e) => { controller.Settings.CornerRadius = radSlider.Value; radVal.Text = radSlider.Value + " px"; controller.RefreshAllCorners(); };
            leftCard.Controls.Add(radSlider);

            leftCard.Controls.Add(CreateLabel("Profil géométrique de la courbe :", 18, 164));
            GlassRoundedComboBox curveCombo = CreateComboBox(18, 188, 336, new string[] {
                "Courbe Hyper-Continue (Superellipse n=4.2)",
                "Arc Circulaire Classique (n=2.0)"
            }, (int)controller.Settings.Curve);
            curveCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Curve = (CurveGeometry)curveCombo.SelectedIndex; controller.RefreshAllCorners(); };
            leftCard.Controls.Add(curveCombo);

            leftCard.Controls.Add(CreateLabel("Angles actifs :", 18, 232));
            GlassToggleSwitch tl = new GlassToggleSwitch("Haut-Gauche", 18, 258, 155, controller.Settings.TopLeft);
            GlassToggleSwitch tr = new GlassToggleSwitch("Haut-Droit", 190, 258, 155, controller.Settings.TopRight);
            GlassToggleSwitch bl = new GlassToggleSwitch("Bas-Gauche", 18, 292, 155, controller.Settings.BottomLeft);
            GlassToggleSwitch br = new GlassToggleSwitch("Bas-Droit", 190, 292, 155, controller.Settings.BottomRight);
            EventHandler ch = (s, e) => { controller.Settings.TopLeft = tl.Checked; controller.Settings.TopRight = tr.Checked; controller.Settings.BottomLeft = bl.Checked; controller.Settings.BottomRight = br.Checked; controller.RefreshAllCorners(); };
            tl.CheckedChanged += ch; tr.CheckedChanged += ch; bl.CheckedChanged += ch; br.CheckedChanged += ch;
            leftCard.Controls.Add(tl); leftCard.Controls.Add(tr); leftCard.Controls.Add(bl); leftCard.Controls.Add(br);

            StringBuilder msb = new StringBuilder();
            msb.AppendFormat("{0} moniteur(s) actif(s) :\n", controller.Monitors.Count);
            for (int i = 0; i < controller.Monitors.Count; i++)
            {
                Rectangle b = controller.Monitors[i].Bounds;
                msb.AppendFormat("• Écran {0} : {1}×{2} ({3}, {4})\n", i + 1, b.Width, b.Height, b.X, b.Y);
            }
            GlassRoundedBox monBox = new GlassRoundedBox(18, 334, 336, 80, 14f);
            Label monLbl = new Label { Location = new Point(12, 10), Size = new Size(312, 60), ForeColor = Color.FromArgb(145, 205, 255), BackColor = Color.FromArgb(16, 20, 30), Text = msb.ToString() };
            monBox.Controls.Add(monLbl);
            leftCard.Controls.Add(monBox);

            leftCard.Controls.Add(CreateGlassBtn("Actualiser la topologie d'écrans", 18, 425, 336, 36, Color.FromArgb(28, 34, 52), () => controller.RebuildMonitors()));

            // Right Card
            GlassCardPanel rightCard = new GlassCardPanel(406, 16, 372, 585, "👁️ Rendu Spéculaire des Angles");
            page.Controls.Add(rightCard);

            GlassToggleSwitch bezelChk = new GlassToggleSwitch("Liseré spéculaire en verre dépoli", 18, 54, 330, controller.Settings.CornerBezelGlow);
            bezelChk.CheckedChanged += (s, e) => { controller.Settings.CornerBezelGlow = bezelChk.Checked; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(bezelChk);

            rightCard.Controls.Add(CreateLabel("Comportement lumineux des angles :", 18, 96));
            GlassRoundedComboBox animCombo = CreateComboBox(18, 120, 336, new string[] {
                "Verre Dépoli Statique (0% CPU au repos)",
                "Flux Prismatique Fluide",
                "Respiration Atmosphérique Douce",
                "Pulsation Synchronisée sur l'Audio",
                "Lueur Stellaire Subtile",
                "Onde Boréale Décalée"
            }, (int)controller.Settings.CornerAnim);
            animCombo.SelectedIndexChanged += (s, e) => { controller.Settings.CornerAnim = (CornerEffectMode)animCombo.SelectedIndex; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(animCombo);

            rightCard.Controls.Add(CreateLabel("Opacité du reflet spéculaire :", 18, 164));
            Label opVal = CreateValueLabel(controller.Settings.BezelGlowOpacity + " %", 300, 164);
            rightCard.Controls.Add(opVal);
            GlassSlider opSlider = new GlassSlider(18, 188, 336, 0, 100, controller.Settings.BezelGlowOpacity);
            opSlider.ValueChanged += (s, e) => { controller.Settings.BezelGlowOpacity = opSlider.Value; opVal.Text = opSlider.Value + " %"; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(opSlider);

            rightCard.Controls.Add(CreateLabel("Finesse du trait (px) :", 18, 228));
            Label thVal = CreateValueLabel((controller.Settings.BezelThickness / 10.0).ToString("0.0") + " px", 300, 228);
            rightCard.Controls.Add(thVal);
            GlassSlider thSlider = new GlassSlider(18, 252, 336, 10, 65, controller.Settings.BezelThickness);
            thSlider.ValueChanged += (s, e) => { controller.Settings.BezelThickness = thSlider.Value; thVal.Text = (thSlider.Value / 10.0).ToString("0.0") + " px"; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(thSlider);

            rightCard.Controls.Add(CreateLabel("Vitesse du flux :", 18, 292));
            Label spVal = CreateValueLabel(controller.Settings.CornerAnimSpeed + " %", 300, 292);
            rightCard.Controls.Add(spVal);
            GlassSlider spSlider = new GlassSlider(18, 316, 336, 25, 250, controller.Settings.CornerAnimSpeed);
            spSlider.ValueChanged += (s, e) => { controller.Settings.CornerAnimSpeed = spSlider.Value; spVal.Text = spSlider.Value + " %"; controller.Settings.Save(); };
            rightCard.Controls.Add(spSlider);

            rightCard.Controls.Add(CreateLabel("Aperçu temps réel :", 18, 360));
            cornerPreviewBox = new GlassRoundedBox(18, 386, 336, 125, 18f);
            cornerPreviewBox.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = cornerPreviewBox.ClientRectangle;
                Color[] pal = GlassThemeEngine.GetPalette(controller.Settings, controller.GlobalTimeSeconds);
                using (GraphicsPath cp = GlassThemeEngine.CreateRoundedRect(new RectangleF(2, 2, r.Width - 4, r.Height - 4), 16f))
                {
                    g.SetClip(cp);
                    using (LinearGradientBrush wp = new LinearGradientBrush(r, Color.FromArgb(255, 24, 38, 68), Color.FromArgb(255, 52, 28, 76), 25f))
                        g.FillRectangle(wp, r);
                    int pr = Math.Min(65, controller.Settings.CornerRadius);
                    if (controller.Settings.Enabled && pr > 2)
                    {
                        int sz = pr + 22;
                        float ph = controller.GlobalTimeSeconds * (controller.Settings.CornerAnimSpeed / 100f);
                        using (Bitmap b1 = MonitorCorners.RenderCornerBitmap(sz, pr, CornerPosition.TopLeft, controller.Settings, 0.3f, pal, ph, controller.MediaTracker.CurrentAudioPeak))
                        using (Bitmap b2 = MonitorCorners.RenderCornerBitmap(sz, pr, CornerPosition.TopRight, controller.Settings, 0.3f, pal, ph, controller.MediaTracker.CurrentAudioPeak))
                        {
                            g.DrawImage(b1, 0, 0);
                            g.DrawImage(b2, r.Width - b2.Width, 0);
                        }
                    }
                    g.ResetClip();
                }
            };
            rightCard.Controls.Add(cornerPreviewBox);

            return page;
        }

        private Panel BuildPage2Transitions()
        {
            Panel page = CreatePageContainer();

            GlassCardPanel leftCard = new GlassCardPanel(18, 16, 372, 585, "🌊 Passerelle Inter-Écrans & Fluides");
            page.Controls.Add(leftCard);

            GlassToggleSwitch enTrans = new GlassToggleSwitch("Activer la transition au passage d'écran", 18, 54, 330, controller.Settings.EnableTransition);
            enTrans.CheckedChanged += (s, e) => { controller.Settings.EnableTransition = enTrans.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(enTrans);

            GlassToggleSwitch magChk = new GlassToggleSwitch("Halo magnétique à l'approche de la bordure", 18, 88, 330, controller.Settings.EnableMagneticProximity);
            magChk.CheckedChanged += (s, e) => { controller.Settings.EnableMagneticProximity = magChk.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(magChk);

            leftCard.Controls.Add(CreateLabel("Style visuel de la transition :", 18, 132));
            GlassRoundedComboBox styleCombo = CreateComboBox(18, 156, 336, new string[] {
                "Fusion Glassmorphique (Pont Liquide + Aura + Étoiles)",
                "Pont Liquide en Verre Dépoli",
                "Onde Prismatique Atmosphérique",
                "Filaments Quantiques",
                "Nuage de Poussière Stellaire",
                "Résonance Concentrique (Ripples)",
                "Ligne Optique Horizon",
                "Pulsation Capsule & Angles Uniquement"
            }, (int)controller.Settings.Style);
            styleCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Style = (PortalTransitionStyle)styleCombo.SelectedIndex; controller.Settings.Save(); controller.TriggerDemoTransition(); };
            leftCard.Controls.Add(styleCombo);

            leftCard.Controls.Add(CreateLabel("Durée de la transition :", 18, 206));
            Label durVal = CreateValueLabel(controller.Settings.DurationMs + " ms", 290, 206);
            leftCard.Controls.Add(durVal);
            GlassSlider durSlider = new GlassSlider(18, 230, 336, 350, 2000, controller.Settings.DurationMs);
            durSlider.ValueChanged += (s, e) => { controller.Settings.DurationMs = durSlider.Value; durVal.Text = durSlider.Value + " ms"; controller.Settings.Save(); };
            leftCard.Controls.Add(durSlider);

            leftCard.Controls.Add(CreateLabel("Envergure de l'onde :", 18, 274));
            Label intVal = CreateValueLabel(controller.Settings.IntensityPercent + " %", 295, 274);
            leftCard.Controls.Add(intVal);
            GlassSlider intSlider = new GlassSlider(18, 298, 336, 60, 190, controller.Settings.IntensityPercent);
            intSlider.ValueChanged += (s, e) => { controller.Settings.IntensityPercent = intSlider.Value; intVal.Text = intSlider.Value + " %"; controller.Settings.Save(); };
            leftCard.Controls.Add(intSlider);

            leftCard.Controls.Add(CreateGlassBtn("Simuler une traversée d'écran", 18, 355, 336, 40, Color.FromArgb(40, 115, 215), () => controller.TriggerDemoTransition()));

            // Right Card
            GlassCardPanel rightCard = new GlassCardPanel(406, 16, 372, 585, "🎨 Palettes Chromatiques en Verre (12 Thèmes)");
            page.Controls.Add(rightCard);

            rightCard.Controls.Add(CreateLabel("Palette chromatique :", 18, 54));
            themeCombo = CreateComboBox(18, 78, 336, new string[] {
                "Prisme Éther (Cyan Glacé / Lavande / Rose)",
                "Verre Givre & Argent Pur",
                "Océan Polaire (Azur / Cyan / Indigo)",
                "Émeraude & Menthe Givrée",
                "Velours Rubis & Corail",
                "Ambre Solaire & Champagne",
                "Aurore Boréale Nocturne",
                "Brume Ultraviolette",
                "Fleur de Sakura Pastel",
                "Monochrome Obsidienne & Titane",
                "Spectre Chromatique Continu (360°)",
                "Palette Personnalisée (Couleurs 1 & 2)"
            }, (int)controller.Settings.Theme);
            themeCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Theme = (GlassColorPalette)themeCombo.SelectedIndex; controller.RefreshAllCorners(); controller.TriggerDemoTransition(); };
            rightCard.Controls.Add(themeCombo);

            rightCard.Controls.Add(CreateLabel("Teintes personnalisées :", 18, 128));
            Control c1Btn = CreateGlassBtn("Teinte 1", 18, 154, 162, 36, Color.FromArgb(255, Color.FromArgb(controller.Settings.CustomColor1Argb)), null);
            c1Btn.ForeColor = Color.Black;
            c1Btn.Click += (s, e) => PickCustomColor(1, c1Btn);

            Control c2Btn = CreateGlassBtn("Teinte 2", 192, 154, 162, 36, Color.FromArgb(255, Color.FromArgb(controller.Settings.CustomColor2Argb)), null);
            c2Btn.Click += (s, e) => PickCustomColor(2, c2Btn);
            rightCard.Controls.Add(c1Btn);
            rightCard.Controls.Add(c2Btn);

            rightCard.Controls.Add(CreateLabel("Préréglages minimalistes :", 18, 212));
            rightCard.Controls.Add(CreateGlassBtn("Verre Givre Minimal (0% CPU)", 18, 240, 336, 36, Color.FromArgb(32, 38, 56), () => ApplyPreset(GlassColorPalette.FrostWhiteSilver, CornerEffectMode.MinimalFrost)));
            rightCard.Controls.Add(CreateGlassBtn("Émeraude Réactif au Son", 18, 286, 336, 36, Color.FromArgb(24, 95, 68), () => ApplyPreset(GlassColorPalette.EmeraldMint, CornerEffectMode.AudioPulseSync)));
            rightCard.Controls.Add(CreateGlassBtn("Aurore Boréale Respirante", 18, 332, 336, 36, Color.FromArgb(28, 85, 115), () => ApplyPreset(GlassColorPalette.BorealAurora, CornerEffectMode.SoftBreathing)));
            rightCard.Controls.Add(CreateGlassBtn("Prisme Éther Fluide", 18, 378, 336, 36, Color.FromArgb(65, 55, 125), () => ApplyPreset(GlassColorPalette.AetherPrism, CornerEffectMode.PrismaticFlow)));

            return page;
        }

        private Panel BuildPage3NovaCapsule()
        {
            Panel page = CreatePageContainer();

            GlassCardPanel leftCard = new GlassCardPanel(18, 16, 372, 585, "💧 Capsule Aura Glass & Dynamique");
            page.Controls.Add(leftCard);

            leftCard.Controls.Add(CreateLabel("Mode de présence de la capsule :", 18, 54));
            GlassRoundedComboBox visCombo = CreateComboBox(18, 78, 336, new string[] {
                "Toujours présente en haut de l'écran",
                "Apparition au survol du bord supérieur (Hot-Edge)",
                "Contextuelle (Audio, Minuteur, Compagnon ou Transition)",
                "Uniquement lors d'un changement d'écran",
                "Masquée"
            }, (int)controller.Settings.IslandMode);
            visCombo.SelectedIndexChanged += (s, e) => {
                controller.Settings.IslandMode = (CapsuleVisibilityMode)visCombo.SelectedIndex;
                if (controller.Settings.IslandMode == CapsuleVisibilityMode.Hidden)
                    controller.DynamicIsland.HideNow();
                controller.Settings.Save();
            };
            leftCard.Controls.Add(visCombo);

            leftCard.Controls.Add(CreateLabel("Cinématique d'apparition :", 18, 122));
            GlassRoundedComboBox spawnCombo = CreateComboBox(18, 146, 336, new string[] {
                "Goutte en Verre Liquide (Liquid Drop)",
                "Déploiement Horizontal Doux",
                "Fondu Optique & Zoom Subtil",
                "Ouverture Prismatique",
                "Condensation Halo"
            }, (int)controller.Settings.SpawnAnim);
            spawnCombo.SelectedIndexChanged += (s, e) => { controller.Settings.SpawnAnim = (CapsuleSpawnStyle)spawnCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(spawnCombo);

            leftCard.Controls.Add(CreateLabel("Ancrage multi-écrans :", 18, 190));
            GlassRoundedComboBox posCombo = CreateComboBox(18, 214, 336, new string[] {
                "Suivre l'écran actif du curseur (Glissement fluide)",
                "Fixer sur le moniteur principal"
            }, (int)controller.Settings.IslandPlacement);
            posCombo.SelectedIndexChanged += (s, e) => { controller.Settings.IslandPlacement = (CapsuleScreenPlacement)posCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(posCombo);

            leftCard.Controls.Add(CreateLabel("Finition du contour de la capsule :", 18, 258));
            GlassRoundedComboBox borderCombo = CreateComboBox(18, 282, 336, new string[] {
                "Liseré Verre Dépoli Spéculaire (Minimaliste)",
                "Halo Prismatique Rotatif Doux",
                "Halo Réactif au Spectre Audio",
                "Obsidienne Mate Sans Halo"
            }, (int)controller.Settings.IslandBorder);
            borderCombo.SelectedIndexChanged += (s, e) => { controller.Settings.IslandBorder = (CapsuleBorderStyle)borderCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(borderCombo);

            leftCard.Controls.Add(CreateLabel("Échelle de la capsule :", 18, 326));
            Label szVal = CreateValueLabel(controller.Settings.IslandScalePercent + " %", 295, 326);
            leftCard.Controls.Add(szVal);
            GlassSlider szSlider = new GlassSlider(18, 350, 336, 75, 150, controller.Settings.IslandScalePercent);
            szSlider.ValueChanged += (s, e) => { controller.Settings.IslandScalePercent = szSlider.Value; szVal.Text = szSlider.Value + " %"; controller.Settings.Save(); };
            leftCard.Controls.Add(szSlider);

            GlassToggleSwitch hovChk = new GlassToggleSwitch("Déployer les 4 onglets au survol souris", 18, 392, 330, controller.Settings.IslandExpandOnHover);
            hovChk.CheckedChanged += (s, e) => { controller.Settings.IslandExpandOnHover = hovChk.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(hovChk);

            GlassToggleSwitch brandChk = new GlassToggleSwitch("Adapter la teinte à la source média active", 18, 426, 330, controller.Settings.IslandAutoAdaptBrandColor);
            brandChk.CheckedChanged += (s, e) => { controller.Settings.IslandAutoAdaptBrandColor = brandChk.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(brandChk);

            // Right Card
            GlassCardPanel rightCard = new GlassCardPanel(406, 16, 372, 585, "⏱️ Minuteur Focus & Studio Musical");
            page.Controls.Add(rightCard);

            rightCard.Controls.Add(CreateLabel("Sessions Focus & Minuteur rapide :", 18, 54));
            rightCard.Controls.Add(CreateGlassBtn("Minuteur 1 min", 18, 80, 162, 36, Color.FromArgb(32, 42, 64), () => controller.DynamicIsland.TimerEngine.StartCountdown(60, "1 Min")));
            rightCard.Controls.Add(CreateGlassBtn("Minuteur 5 min", 192, 80, 162, 36, Color.FromArgb(32, 42, 64), () => controller.DynamicIsland.TimerEngine.StartCountdown(300, "5 Min")));
            rightCard.Controls.Add(CreateGlassBtn("Session Focus 25 min", 18, 124, 162, 36, Color.FromArgb(40, 105, 195), () => controller.DynamicIsland.TimerEngine.StartCountdown(1500, "Focus 25m")));
            rightCard.Controls.Add(CreateGlassBtn("Chronomètre", 192, 124, 162, 36, Color.FromArgb(32, 42, 64), () => controller.DynamicIsland.TimerEngine.SwitchToStopwatch()));

            rightCard.Controls.Add(CreateGlassBtn("Démarrer / Pause", 18, 168, 215, 34, Color.FromArgb(28, 34, 52), () => controller.DynamicIsland.TimerEngine.ToggleStartPause()));
            rightCard.Controls.Add(CreateGlassBtn("Réinitialiser", 242, 168, 112, 34, Color.FromArgb(28, 34, 52), () => controller.DynamicIsland.TimerEngine.Reset()));

            rightCard.Controls.Add(CreateLabel("Contrôles Audio Universels :", 18, 224));
            rightCard.Controls.Add(CreateGlassBtn("Spotify", 18, 250, 80, 34, Color.FromArgb(24, 95, 65), () => controller.MediaTracker.LaunchOrFocusService("spotify")));
            rightCard.Controls.Add(CreateGlassBtn("Apple Music", 104, 250, 100, 34, Color.FromArgb(150, 40, 70), () => controller.MediaTracker.LaunchOrFocusService("applemusic")));
            rightCard.Controls.Add(CreateGlassBtn("YouTube", 210, 250, 70, 34, Color.FromArgb(115, 32, 48), () => controller.MediaTracker.LaunchOrFocusService("youtube")));
            rightCard.Controls.Add(CreateGlassBtn("Deezer", 286, 250, 68, 34, Color.FromArgb(70, 45, 120), () => controller.MediaTracker.LaunchOrFocusService("webmusic")));

            GlassToggleSwitch trackChk = new GlassToggleSwitch("Animation au changement de titre", 18, 340, 330, controller.Settings.TrackChangeAnimation);
            trackChk.CheckedChanged += (s, e) => { controller.Settings.TrackChangeAnimation = trackChk.Checked; controller.Settings.Save(); };
            rightCard.Controls.Add(trackChk);

            GlassToggleSwitch fsChk = new GlassToggleSwitch("Morphing en mode plein écran", 18, 374, 330, controller.Settings.FullscreenMorph);
            fsChk.CheckedChanged += (s, e) => { controller.Settings.FullscreenMorph = fsChk.Checked; controller.Settings.Save(); };
            rightCard.Controls.Add(fsChk);

            GlassToggleSwitch appChk = new GlassToggleSwitch("Afficher l'app détectée dans la capsule", 18, 408, 330, controller.Settings.ShowDetectedAppBadge);
            appChk.CheckedChanged += (s, e) => { controller.Settings.ShowDetectedAppBadge = appChk.Checked; controller.Settings.Save(); };
            rightCard.Controls.Add(appChk);

            rightCard.Controls.Add(CreateGlassBtn("⏮ Précédent", 18, 292, 106, 34, Color.FromArgb(28, 34, 52), () => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PREV_TRACK)));
            rightCard.Controls.Add(CreateGlassBtn("⏯ Lecture", 133, 292, 106, 34, Color.FromArgb(40, 115, 215), () => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PLAY_PAUSE)));
            rightCard.Controls.Add(CreateGlassBtn("⏭ Suivant", 248, 292, 106, 34, Color.FromArgb(28, 34, 52), () => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_NEXT_TRACK)));

            return page;
        }

        private Panel BuildPage4Companion()
        {
            Panel page = CreatePageContainer();

            GlassCardPanel leftCard = new GlassCardPanel(18, 16, 372, 585, "✨ Mascottes Célestes & Évolutions");
            page.Controls.Add(leftCard);

            leftCard.Controls.Add(CreateLabel("Choix de la mascotte céleste :", 18, 54));
            GlassRoundedComboBox specCombo = CreateComboBox(18, 78, 336, new string[] {
                "Aucune mascotte (Désactivée)",
                "✨ Lumina (Sphère d'Étoile Gyroscopique Céleste)",
                "🦊 Kitsune (Renard Boréal d'Aurore Éthérée)",
                "💎 Draco (Wyverne de Cristal & Gemmes Taillées)",
                "🫧 Nyx (Goutte Stellaire Opalescente & Perles)",
                "🐾 Kuro-Neko (Félin d'Obsidienne Fumé & Améthyste)",
                "🔮 Axiom (Sphère Saphir & Couronne Titane Gravée)"
            }, (int)controller.Settings.Companion);
            specCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Companion = (CompanionEntity)specCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(specCombo);

            leftCard.Controls.Add(CreateLabel("Nom de la mascotte :", 18, 124));
            GlassRoundedInput nameBox = new GlassRoundedInput(18, 148, 336, 32, controller.Settings.CompanionName, false, false);
            nameBox.TextChanged += (s, e) => { controller.Settings.CompanionName = nameBox.Text; controller.Settings.Save(); };
            leftCard.Controls.Add(nameBox);

            leftCard.Controls.Add(CreateLabel("Ancrage autour de la capsule :", 18, 190));
            GlassRoundedComboBox posCombo = CreateComboBox(18, 214, 336, new string[] {
                "En lévitation douce à droite de la capsule",
                "Posé au-dessus de la capsule",
                "Intégré à l'intérieur de la capsule"
            }, (int)controller.Settings.CompanionPos);
            posCombo.SelectedIndexChanged += (s, e) => { controller.Settings.CompanionPos = (CompanionAnchor)posCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(posCombo);

            leftCard.Controls.Add(CreateLabel("Échelle de la mascotte :", 18, 258));
            Label cSzVal = CreateValueLabel(controller.Settings.CompanionScalePercent + " %", 295, 258);
            leftCard.Controls.Add(cSzVal);
            GlassSlider cSzSlider = new GlassSlider(18, 282, 336, 70, 165, controller.Settings.CompanionScalePercent);
            cSzSlider.ValueChanged += (s, e) => { controller.Settings.CompanionScalePercent = cSzSlider.Value; cSzVal.Text = cSzSlider.Value + " %"; controller.Settings.Save(); };
            leftCard.Controls.Add(cSzSlider);

            GlassToggleSwitch eyeChk = new GlassToggleSwitch("Suivi oculaire fluide du curseur", 18, 326, 330, controller.Settings.CompanionFollowCursorEyes);
            eyeChk.CheckedChanged += (s, e) => { controller.Settings.CompanionFollowCursorEyes = eyeChk.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(eyeChk);

            GlassToggleSwitch bubChk = new GlassToggleSwitch("Bulles de dialogue en verre dépoli", 18, 360, 330, controller.Settings.CompanionShowSpeechBubbles);
            bubChk.CheckedChanged += (s, e) => { controller.Settings.CompanionShowSpeechBubbles = bubChk.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(bubChk);

            // Right Card
            GlassCardPanel rightCard = new GlassCardPanel(406, 16, 372, 585, "💎 Miroir Astral & Émanations de la Mascotte");
            page.Controls.Add(rightCard);

            companionPreviewBox = new GlassRoundedBox(18, 54, 336, 215, 18f);
            companionPreviewBox.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = companionPreviewBox.ClientRectangle;
                using (GraphicsPath cp = GlassThemeEngine.CreateRoundedRect(new RectangleF(2, 2, r.Width - 4, r.Height - 4), 16f))
                {
                    g.SetClip(cp);
                    Color[] pal = GlassThemeEngine.GetPalette(controller.Settings, controller.GlobalTimeSeconds);
                    CompanionEntity sp = controller.Settings.Companion == CompanionEntity.None ? CompanionEntity.LuminaOrb : controller.Settings.Companion;
                    Point screenCenter = companionPreviewBox.PointToScreen(new Point(companionPreviewBox.Width / 2, companionPreviewBox.Height / 2));
                    controller.DynamicIsland.CompanionEngine.DrawCompanion(
                        g, new RectangleF(88, 28, 160, 160), sp, Cursor.Position, screenCenter,
                        pal[0], pal[1 % pal.Length], controller.GlobalTimeSeconds, controller.MediaTracker.CurrentAudioPeak, true);
                    g.ResetClip();
                }
            };
            rightCard.Controls.Add(companionPreviewBox);

            rightCard.Controls.Add(CreateGlassBtn("💖 Faire un Câlin (+20 XP)", 18, 286, 336, 38, Color.FromArgb(195, 55, 115), () => controller.DynamicIsland.CompanionEngine.InteractBoost()));
            rightCard.Controls.Add(CreateGlassBtn("Générer une réflexion IA", 18, 334, 336, 38, Color.FromArgb(65, 55, 130), () =>
            {
                controller.DynamicIsland.AiEngine.SendPromptAsync(controller.Settings, "Dis une phrase courte, apaisante et minimaliste.", reply => controller.DynamicIsland.CompanionEngine.Say(reply, 5500));
            }));

            return page;
        }

        private Panel BuildPage5Ai()
        {
            Panel page = CreatePageContainer();

            GlassCardPanel leftCard = new GlassCardPanel(18, 16, 372, 585, "✨ Cerveaux IA & Dialogue Créatif");
            page.Controls.Add(leftCard);

            leftCard.Controls.Add(CreateLabel("Moteur IA actif :", 18, 54));
            GlassRoundedComboBox provCombo = CreateComboBox(18, 78, 336, new string[] {
                "Google Gemini (gemini-2.5-flash)",
                "Anthropic Claude (claude-3-5-sonnet)",
                "OpenAI ChatGPT (gpt-4o-mini)",
                "Mistral AI (mistral-small-latest)"
            }, (int)controller.Settings.ActiveAiProvider);
            provCombo.SelectedIndexChanged += (s, e) => { controller.Settings.ActiveAiProvider = (AiService)provCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(provCombo);

            leftCard.Controls.Add(CreateLabel("Clé API Gemini & Modèle :", 18, 120));
            GlassRoundedInput gemKey = CreateApiInputBox(18, 142, 212, controller.Settings.GeminiApiKey, true);
            GlassRoundedInput gemMod = CreateApiInputBox(236, 142, 118, controller.Settings.GeminiModel, false);
            gemKey.TextChanged += (s, e) => { controller.Settings.GeminiApiKey = gemKey.Text.Trim(); controller.Settings.Save(); };
            gemMod.TextChanged += (s, e) => { controller.Settings.GeminiModel = gemMod.Text.Trim(); controller.Settings.Save(); };
            leftCard.Controls.Add(gemKey); leftCard.Controls.Add(gemMod);

            leftCard.Controls.Add(CreateLabel("Clé API Claude & Modèle :", 18, 178));
            GlassRoundedInput clKey = CreateApiInputBox(18, 200, 212, controller.Settings.ClaudeApiKey, true);
            GlassRoundedInput clMod = CreateApiInputBox(236, 200, 118, controller.Settings.ClaudeModel, false);
            clKey.TextChanged += (s, e) => { controller.Settings.ClaudeApiKey = clKey.Text.Trim(); controller.Settings.Save(); };
            clMod.TextChanged += (s, e) => { controller.Settings.ClaudeModel = clMod.Text.Trim(); controller.Settings.Save(); };
            leftCard.Controls.Add(clKey); leftCard.Controls.Add(clMod);

            leftCard.Controls.Add(CreateLabel("Clé API ChatGPT & Modèle :", 18, 236));
            GlassRoundedInput oaKey = CreateApiInputBox(18, 258, 212, controller.Settings.OpenAiApiKey, true);
            GlassRoundedInput oaMod = CreateApiInputBox(236, 258, 118, controller.Settings.OpenAiModel, false);
            oaKey.TextChanged += (s, e) => { controller.Settings.OpenAiApiKey = oaKey.Text.Trim(); controller.Settings.Save(); };
            oaMod.TextChanged += (s, e) => { controller.Settings.OpenAiModel = oaMod.Text.Trim(); controller.Settings.Save(); };
            leftCard.Controls.Add(oaKey); leftCard.Controls.Add(oaMod);

            leftCard.Controls.Add(CreateLabel("Clé API Mistral & Modèle :", 18, 294));
            GlassRoundedInput miKey = CreateApiInputBox(18, 316, 212, controller.Settings.MistralApiKey, true);
            GlassRoundedInput miMod = CreateApiInputBox(236, 316, 118, controller.Settings.MistralModel, false);
            miKey.TextChanged += (s, e) => { controller.Settings.MistralApiKey = miKey.Text.Trim(); controller.Settings.Save(); };
            miMod.TextChanged += (s, e) => { controller.Settings.MistralModel = miMod.Text.Trim(); controller.Settings.Save(); };
            leftCard.Controls.Add(miKey); leftCard.Controls.Add(miMod);

            leftCard.Controls.Add(CreateLabel("Directive système (Personnalité) :", 18, 356));
            GlassRoundedInput sysBox = new GlassRoundedInput(18, 380, 336, 92, controller.Settings.AiSystemPrompt, false, true);
            sysBox.TextChanged += (s, e) => { controller.Settings.AiSystemPrompt = sysBox.Text; controller.Settings.Save(); };
            leftCard.Controls.Add(sysBox);

            // Right Card
            GlassCardPanel rightCard = new GlassCardPanel(406, 16, 372, 585, "💬 Studio de Discussion Instantanée");
            page.Controls.Add(rightCard);

            aiStatusBox = new GlassRoundedBox(18, 54, 336, 220, 16f);
            aiStatusLabel = new Label
            {
                Location = new Point(14, 12),
                Size = new Size(308, 196),
                BackColor = Color.FromArgb(16, 20, 30),
                ForeColor = Color.FromArgb(225, 234, 250),
                Text = controller.DynamicIsland.AiEngine.LastResponse
            };
            aiStatusBox.Controls.Add(aiStatusLabel);
            rightCard.Controls.Add(aiStatusBox);

            rightCard.Controls.Add(CreateLabel("Message :", 18, 290));
            aiChatTestInput = new GlassRoundedInput(18, 314, 336, 34, "Bonjour, présente-toi en une phrase minimaliste.", false, false);
            rightCard.Controls.Add(aiChatTestInput);

            rightCard.Controls.Add(CreateGlassBtn("Envoyer la requête à l'IA", 18, 356, 336, 40, Color.FromArgb(40, 115, 215), () =>
            {
                string prompt = aiChatTestInput.Text.Trim();
                if (string.IsNullOrEmpty(prompt)) return;
                aiStatusLabel.Text = "Requête en cours (" + controller.Settings.ActiveAiProvider + ")...";
                controller.DynamicIsland.AiEngine.SendPromptAsync(controller.Settings, prompt, reply =>
                {
                    if (!IsDisposed && IsHandleCreated)
                    {
                        BeginInvoke((Action)(() =>
                        {
                            aiStatusLabel.Text = reply;
                            controller.DynamicIsland.CompanionEngine.Say(reply, 6000);
                        }));
                    }
                });
            }));

            return page;
        }

        private GlassRoundedInput CreateApiInputBox(int x, int y, int w, string val, bool isPassword)
        {
            return new GlassRoundedInput(x, y, w, 32, val, isPassword, false);
        }

        private void PickCustomColor(int slot, Control btn)
        {
            using (ColorDialog cd = new ColorDialog { FullOpen = true })
            {
                if (cd.ShowDialog(this) == DialogResult.OK)
                {
                    if (slot == 1) controller.Settings.CustomColor1Argb = cd.Color.ToArgb();
                    else controller.Settings.CustomColor2Argb = cd.Color.ToArgb();
                    btn.BackColor = cd.Color;
                    controller.Settings.Theme = GlassColorPalette.CustomGlassPalette;
                    if (themeCombo != null) themeCombo.SelectedIndex = (int)GlassColorPalette.CustomGlassPalette;
                    controller.RefreshAllCorners();
                }
            }
        }

        private void ApplyPreset(GlassColorPalette theme, CornerEffectMode cornerMode)
        {
            controller.Settings.Theme = theme;
            controller.Settings.CornerAnim = cornerMode;
            if (themeCombo != null) themeCombo.SelectedIndex = (int)theme;
            controller.RefreshAllCorners();
            controller.TriggerDemoTransition();
        }

        private Panel CreatePageContainer()
        {
            return new DoubleBufferedPanel { Location = new Point(235, 74), Size = new Size(805, 635), BackColor = Color.FromArgb(11, 13, 20) };
        }

        private Control CreateGlassBtn(string text, int x, int y, int w, int h, Color bg, Action onClick)
        {
            GlassButton b = new GlassButton(text, x, y, w, h, bg);
            if (onClick != null) b.Click += (s, e) => onClick();
            return b;
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label { Text = text, Location = new Point(x, y), AutoSize = true, BackColor = Color.FromArgb(18, 21, 32), ForeColor = Color.FromArgb(195, 205, 228) };
        }

        private Label CreateValueLabel(string text, int x, int y)
        {
            return new Label { Text = text, Location = new Point(x, y), AutoSize = true, BackColor = Color.FromArgb(18, 21, 32), Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold), ForeColor = Color.FromArgb(115, 210, 255) };
        }

        private GlassRoundedComboBox CreateComboBox(int x, int y, int w, string[] items, int selectedIndex)
        {
            return new GlassRoundedComboBox(x, y, w, 34, items, selectedIndex);
        }

        private bool IsStartupEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false))
                    if (key != null) return key.GetValue("iCornerDuo") != null;
            }
            catch { }
            return false;
        }

        private void SetStartup(bool enable)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (key != null)
                    {
                        if (enable) key.SetValue("iCornerDuo", "\"" + Application.ExecutablePath + "\" --tray");
                        else key.DeleteValue("iCornerDuo", false);
                    }
                }
            }
            catch { }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) { e.Cancel = true; Hide(); return; }
            base.OnFormClosing(e);
        }
    }

    // =========================================================================
    // ENTRY POINT
    // =========================================================================
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            NativeMethods.EnablePerMonitorDpiAwareness();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (OverlayController controller = new OverlayController())
            {
                bool startInTray = (args != null && args.Length > 0 && args[0] == "--tray");
                if (!startInTray)
                {
                    controller.ShowSettingsWindow();
                    System.Windows.Forms.Timer welcomeTimer = new System.Windows.Forms.Timer { Interval = 400 };
                    welcomeTimer.Tick += (s, e) =>
                    {
                        welcomeTimer.Stop();
                        welcomeTimer.Dispose();
                        controller.TriggerDemoTransition();
                        controller.DynamicIsland.CompanionEngine.Say("Interface Glassmorphique active ✦", 4500);
                    };
                    welcomeTimer.Start();
                }
                Application.Run();
            }
        }
    }
}
