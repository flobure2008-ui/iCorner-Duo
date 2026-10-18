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
        SystemSound = 5
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
        public CapsuleVisibilityMode IslandMode = CapsuleVisibilityMode.AlwaysPresent;
        public CapsuleSpawnStyle SpawnAnim = CapsuleSpawnStyle.LiquidGlassDrop;
        public CapsuleScreenPlacement IslandPlacement = CapsuleScreenPlacement.FollowActiveCursor;
        public CapsuleBorderStyle IslandBorder = CapsuleBorderStyle.FrostedGlassRim;
        public int IslandScalePercent = 100;
        public bool IslandExpandOnHover = true;
        public bool IslandShowClockWhenIdle = true;
        public bool IslandAutoAdaptBrandColor = true;

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
                case AudioAppSource.Spotify: return Color.FromArgb(255, 55, 215, 120);
                case AudioAppSource.WebMusic: return Color.FromArgb(255, 245, 90, 125);
                case AudioAppSource.YouTube: return Color.FromArgb(255, 250, 75, 95);
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
        public string Mood = "Serein ✦";
        public string SpeechBubbleText = "";
        private long speechUntilTicks = 0;
        private float pulseBoost = 0f;
        private float smoothHoverY = 0f;
        private float smoothLookX = 0f;
        private float smoothLookY = 0f;

        private static readonly string[] MinimalQuotes = new string[] {
            "Système optique synchronisé ✦",
            "Onde sonore détectée, résonance active ♪",
            "Mode concentration prêt.",
            "Survole la capsule pour déployer l'interface.",
            "Prêt à interagir via l'onglet IA."
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
                Mood = "Transcendant ✧";
                Say("Évolution atteinte • Niveau " + Level, 3800);
            }
            else
            {
                Mood = "Harmonieux ✦";
                Say(MinimalQuotes[rng.Next(MinimalQuotes.Length)], 3200);
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
            float freq = isAudioPlaying ? 6.0f : 2.2f;
            float amp = isAudioPlaying ? 3.5f : 1.8f;
            smoothHoverY = (float)Math.Sin(globalTime * freq) * amp - pulseBoost * 4f;
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
            float r = Math.Min(bounds.Width, bounds.Height) * 0.34f;

            // Smoothly interpolated eye-tracking vector (prevents jitter!)
            float targetLX = 0f, targetLY = 0f;
            if (followEyes && companionScreenCenter != Point.Empty)
            {
                float dx = cursorScreenPos.X - companionScreenCenter.X;
                float dy = cursorScreenPos.Y - companionScreenCenter.Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                if (dist > 2f)
                {
                    float f = Math.Min(1f, dist / 240f);
                    targetLX = (dx / dist) * r * 0.24f * f;
                    targetLY = (dy / dist) * r * 0.24f * f;
                }
            }
            smoothLookX = smoothLookX * 0.75f + targetLX * 0.25f;
            smoothLookY = smoothLookY * 0.75f + targetLY * 0.25f;

            // Soft Glassmorphic Ambient Aura
            float auraR = r * (1.28f + audioPeak * 0.28f + pulseBoost * 0.22f);
            using (GraphicsPath auraPath = new GraphicsPath())
            {
                auraPath.AddEllipse(cx - auraR, cy - auraR, auraR * 2f, auraR * 2f);
                using (PathGradientBrush pgb = new PathGradientBrush(auraPath))
                {
                    pgb.CenterColor = Color.FromArgb(65, accent1);
                    pgb.SurroundColors = new Color[] { Color.FromArgb(0, accent1) };
                    g.FillPath(pgb, auraPath);
                }
            }

            switch (entity)
            {
                case CompanionEntity.LuminaOrb:
                    DrawLuminaOrb(g, cx, cy, r, smoothLookX, smoothLookY, accent1, accent2, globalTime);
                    break;
                case CompanionEntity.KitsuneMinimal:
                    DrawKitsuneMinimal(g, cx, cy, r, smoothLookX, smoothLookY, accent1, accent2, globalTime);
                    break;
                case CompanionEntity.DracoCrystal:
                    DrawDracoCrystal(g, cx, cy, r, smoothLookX, smoothLookY, accent1, accent2, globalTime);
                    break;
                case CompanionEntity.NyxQuantum:
                    DrawNyxQuantum(g, cx, cy, r, smoothLookX, smoothLookY, accent1, accent2, globalTime);
                    break;
                case CompanionEntity.KuroNeko:
                    DrawKuroNeko(g, cx, cy, r, smoothLookX, smoothLookY, accent1, accent2, globalTime);
                    break;
                case CompanionEntity.AxiomPrism:
                    DrawAxiomPrism(g, cx, cy, r, smoothLookX, smoothLookY, accent1, accent2, globalTime);
                    break;
            }

            g.SmoothingMode = oldMode;
        }

        private void DrawGlassSphereBody(Graphics g, float cx, float cy, float r, Color accent1, Color accent2)
        {
            RectangleF bodyRect = new RectangleF(cx - r, cy - r, r * 2f, r * 2f);
            using (SolidBrush darkGlass = new SolidBrush(Color.FromArgb(235, 14, 17, 26)))
            using (LinearGradientBrush sheen = new LinearGradientBrush(bodyRect, Color.FromArgb(65, 255, 255, 255), Color.FromArgb(15, accent1), 45f))
            using (Pen specularRim = new Pen(Color.FromArgb(140, GlassThemeEngine.Interpolate(Color.White, accent1, 0.5f)), 1.4f))
            {
                g.FillEllipse(darkGlass, bodyRect);
                g.FillEllipse(sheen, bodyRect);
                g.DrawEllipse(specularRim, bodyRect);
            }
            // Top crescent glass reflection
            RectangleF highlightRect = new RectangleF(cx - r * 0.65f, cy - r * 0.82f, r * 1.3f, r * 0.65f);
            using (LinearGradientBrush hl = new LinearGradientBrush(highlightRect, Color.FromArgb(55, Color.White), Color.FromArgb(0, Color.White), 90f))
            {
                g.FillEllipse(hl, highlightRect);
            }
        }

        private void DrawLuminaOrb(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t)
        {
            // Tilted orbital glass ring
            GraphicsState st = g.Save();
            g.TranslateTransform(cx, cy);
            g.RotateTransform((float)Math.Sin(t * 1.5f) * 18f - 15f);
            using (Pen ringPen = new Pen(Color.FromArgb(130, c2), 1.5f))
            {
                g.DrawEllipse(ringPen, -r * 1.38f, -r * 0.38f, r * 2.76f, r * 0.76f);
            }
            g.Restore(st);

            DrawGlassSphereBody(g, cx, cy, r, c1, c2);
            DrawMinimalGlassEyes(g, cx, cy, r, lx, ly, c1, t);
        }

        private void DrawKitsuneMinimal(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t)
        {
            // Smooth floating minimalist curved ears (detached glass petals)
            float earWave = (float)Math.Sin(t * 3.0f) * r * 0.06f;
            for (int side = -1; side <= 1; side += 2)
            {
                GraphicsState st = g.Save();
                g.TranslateTransform(cx + side * r * 0.62f, cy - r * 0.78f + earWave * side);
                g.RotateTransform(side * 24f);
                RectangleF earRect = new RectangleF(-r * 0.28f, -r * 0.45f, r * 0.56f, r * 0.9f);
                using (SolidBrush eb = new SolidBrush(Color.FromArgb(220, 16, 20, 30)))
                using (Pen ep = new Pen(Color.FromArgb(160, c1), 1.4f))
                {
                    g.FillEllipse(eb, earRect);
                    g.DrawEllipse(ep, earRect);
                }
                g.Restore(st);
            }

            DrawGlassSphereBody(g, cx, cy, r * 0.95f, c1, c2);
            DrawMinimalGlassEyes(g, cx, cy + r * 0.04f, r, lx, ly, c1, t);
        }

        private void DrawDracoCrystal(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t)
        {
            // Translucent frosted glass wings
            float flap = (float)Math.Sin(t * 5.0f) * r * 0.22f;
            for (int side = -1; side <= 1; side += 2)
            {
                PointF[] wing = new PointF[] {
                    new PointF(cx + side * r * 0.6f, cy),
                    new PointF(cx + side * r * 1.45f, cy - r * 0.65f + flap),
                    new PointF(cx + side * r * 1.25f, cy + r * 0.35f + flap * 0.5f)
                };
                using (SolidBrush wb = new SolidBrush(Color.FromArgb(85, c2)))
                using (Pen wp = new Pen(Color.FromArgb(140, c1), 1.2f))
                {
                    g.FillClosedCurve(wb, wing);
                    g.DrawClosedCurve(wp, wing);
                }
            }

            DrawGlassSphereBody(g, cx, cy, r * 0.92f, c1, c2);
            DrawMinimalGlassEyes(g, cx, cy, r, lx, ly, c1, t);
        }

        private void DrawNyxQuantum(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t)
        {
            // 3 orbiting quantum micro-pearls
            for (int i = 0; i < 3; i++)
            {
                double ang = t * 2.4 + i * (Math.PI * 2.0 / 3.0);
                float ox = cx + (float)Math.Cos(ang) * r * 1.28f;
                float oy = cy + (float)Math.Sin(ang) * r * 0.55f;
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(210, i % 2 == 0 ? c1 : c2)))
                {
                    g.FillEllipse(sb, ox - 2.8f, oy - 2.8f, 5.6f, 5.6f);
                }
            }

            DrawGlassSphereBody(g, cx, cy, r * 0.92f, c1, c2);
            DrawMinimalGlassEyes(g, cx, cy, r, lx, ly, Color.White, t);
        }

        private void DrawKuroNeko(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t)
        {
            // Minimalist rounded cat ears integrated into silhouette
            for (int side = -1; side <= 1; side += 2)
            {
                PointF[] ear = new PointF[] {
                    new PointF(cx + side * r * 0.35f, cy - r * 0.72f),
                    new PointF(cx + side * r * 0.78f, cy - r * 1.08f),
                    new PointF(cx + side * r * 0.85f, cy - r * 0.32f)
                };
                using (SolidBrush eb = new SolidBrush(Color.FromArgb(240, 14, 17, 26)))
                using (Pen ep = new Pen(Color.FromArgb(150, c1), 1.4f))
                {
                    g.FillClosedCurve(eb, ear, FillMode.Alternate, TensionMode);
                    g.DrawClosedCurve(ep, ear, TensionMode, FillMode.Alternate);
                }
            }

            DrawGlassSphereBody(g, cx, cy, r * 0.94f, c1, c2);
            DrawMinimalGlassEyes(g, cx, cy + r * 0.05f, r, lx, ly, c1, t);
        }

        private const float TensionMode = 0.35f;

        private void DrawAxiomPrism(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t)
        {
            // Smooth rotating glass diamond/octahedron
            float pulse = (float)Math.Sin(t * 3.0f) * r * 0.06f;
            PointF[] diamond = new PointF[] {
                new PointF(cx, cy - r * 1.12f - pulse),
                new PointF(cx + r * 1.05f + pulse, cy),
                new PointF(cx, cy + r * 1.12f + pulse),
                new PointF(cx - r * 1.05f - pulse, cy)
            };
            using (SolidBrush db = new SolidBrush(Color.FromArgb(230, 15, 19, 30)))
            using (Pen dp = new Pen(Color.FromArgb(180, c1), 1.5f))
            {
                g.FillPolygon(db, diamond);
                g.DrawPolygon(dp, diamond);
            }

            // Single central luminous optic sensor
            float er = r * 0.30f;
            using (SolidBrush glow = new SolidBrush(Color.FromArgb(90, c1)))
            using (SolidBrush core = new SolidBrush(Color.White))
            {
                g.FillEllipse(glow, cx + lx - er * 1.4f, cy + ly - er * 1.4f, er * 2.8f, er * 2.8f);
                g.FillEllipse(core, cx + lx - er * 0.65f, cy + ly - er * 0.65f, er * 1.3f, er * 1.3f);
            }
        }

        private void DrawMinimalGlassEyes(Graphics g, float cx, float cy, float r, float lx, float ly, Color accent, float t)
        {
            bool blink = ((t % 4.4f) > 4.25f);
            float spacing = r * 0.34f;
            float ew = Math.Max(3.2f, r * 0.24f);
            float eh = blink ? 1.5f : Math.Max(4.2f, r * 0.34f);

            using (SolidBrush glowB = new SolidBrush(Color.FromArgb(95, accent)))
            using (SolidBrush coreB = new SolidBrush(Color.FromArgb(245, 245, 250, 255)))
            {
                // Soft glow behind eyes
                g.FillEllipse(glowB, cx - spacing + lx - ew * 0.8f, cy + ly - eh * 0.8f, ew * 1.6f, eh * 1.6f);
                g.FillEllipse(glowB, cx + spacing + lx - ew * 0.8f, cy + ly - eh * 0.8f, ew * 1.6f, eh * 1.6f);

                // Crisp pill eyes
                g.FillEllipse(coreB, cx - spacing + lx - ew / 2f, cy + ly - eh / 2f, ew, eh);
                g.FillEllipse(coreB, cx + spacing + lx - ew / 2f, cy + ly - eh / 2f, ew, eh);
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

                        if (pname == "chrome" || pname == "msedge" || pname == "firefox" || pname == "brave" || pname == "opera" || pname == "vivaldi")
                        {
                            string clean = StripBrowserSuffix(wtitle);
                            if (clean.IndexOf("YouTube", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                foundSource = AudioAppSource.YouTube;
                                foundTitle = clean.Replace("- YouTube Music", "").Replace("YouTube Music", "").Replace("- YouTube", "").Replace("YouTube", "").Trim();
                                if (string.IsNullOrEmpty(foundTitle)) foundTitle = "YouTube";
                                foundArtist = "Flux YouTube";
                            }
                            else if (clean.IndexOf("Spotify", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                foundSource = AudioAppSource.Spotify;
                                foundTitle = clean.Replace("- Spotify", "").Replace("Spotify", "").Trim();
                                if (string.IsNullOrEmpty(foundTitle)) foundTitle = "Spotify Web";
                                foundArtist = "Spotify";
                            }
                            else if (clean.IndexOf("Deezer", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                     clean.IndexOf("SoundCloud", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                     clean.IndexOf("Music", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                foundSource = AudioAppSource.WebMusic;
                                foundTitle = clean;
                                foundArtist = "Streaming Audio";
                            }
                        }
                    }

                    if (foundSource != AudioAppSource.None)
                    {
                        ActiveSource = foundSource;
                        TrackTitle = foundTitle;
                        TrackArtist = foundArtist;
                    }
                    else if (IsAudioActive)
                    {
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
            capsuleForm.Show();

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
                capsuleForm.Hide();
                return;
            }

            if (transitionBoost > 0f) transitionBoost = Math.Max(0f, transitionBoost - 0.03f);

            Rectangle targetScreen = (settings.IslandPlacement == CapsuleScreenPlacement.PrimaryMonitorFixed)
                ? primaryScreenBounds : activeScreenBounds;

            bool wantVisible = true;
            if (settings.IslandMode == CapsuleVisibilityMode.TransitionOnly)
                wantVisible = (transitionBoost > 0.02f || TimerEngine.FinishedAlertActive);
            else if (settings.IslandMode == CapsuleVisibilityMode.SmartContextual)
                wantVisible = (media.IsAudioActive || media.ActiveSource != AudioAppSource.None ||
                               TimerEngine.IsRunning || TimerEngine.FinishedAlertActive ||
                               CompanionEngine.HasActiveSpeech || transitionBoost > 0.02f);
            else if (settings.IslandMode == CapsuleVisibilityMode.TopEdgeHoverWake)
            {
                bool nearTop = (cursorPos.Y <= targetScreen.Top + 42 &&
                                Math.Abs(cursorPos.X - (targetScreen.Left + targetScreen.Width / 2)) < 280);
                wantVisible = nearTop || expandProgress > 0.05f || transitionBoost > 0.02f || TimerEngine.FinishedAlertActive;
            }

            float targetSpawn = wantVisible ? 1.0f : 0.0f;
            spawnFactor = spawnFactor * 0.82f + targetSpawn * 0.18f;
            if (spawnFactor <= 0.015f && !wantVisible)
            {
                spawnFactor = 0f;
                capsuleForm.Hide();
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
            expandProgress = expandProgress * 0.80f + targetExpand * 0.20f;
            if (Math.Abs(expandProgress - targetExpand) < 0.004f) expandProgress = targetExpand;

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

                // 1. Soft Ambient Outer Aura (Glassmorphic Diffuse Shadow/Glow)
                DrawCapsuleAmbientAura(g, pillRect, cornerRad, settings.IslandBorder, accent1, accent2, palette, globalTime, media.CurrentAudioPeak, transitionBoost);

                // 2. Frosted Glass Capsule Surface
                GlassThemeEngine.DrawFrostedGlassSurface(g, pillRect, cornerRad, Color.FromArgb(10, 12, 18), accent1, 244, 1.0f);

                // 3. Compact vs Expanded Content
                if (expandProgress < 0.42f)
                {
                    float cAlpha = (1.0f - expandProgress / 0.42f) * spawnFactor;
                    DrawCompactCapsule(g, pillRect, scale, cAlpha, media, accent1, accent2, globalTime, settings);
                }
                else
                {
                    float eAlpha = ((expandProgress - 0.42f) / 0.58f) * spawnFactor;
                    DrawExpandedGlassHub(g, pillRect, scale, eAlpha, media, accent1, accent2, globalTime, settings, cursorPos, companionScreenCenter);
                }

                // 4. Companion Entity (when anchored outside capsule)
                if (settings.Companion != CompanionEntity.None && settings.CompanionPos != CompanionAnchor.IntegratedInside)
                {
                    CompanionEngine.DrawCompanion(
                        g, companionRect, settings.Companion, cursorPos, companionScreenCenter,
                        accent1, accent2, globalTime, media.CurrentAudioPeak, settings.CompanionFollowCursorEyes);

                    if (settings.CompanionShowSpeechBubbles && CompanionEngine.HasActiveSpeech)
                    {
                        DrawGlassSpeechPill(g, companionRect, winW, CompanionEngine.SpeechBubbleText, accent1);
                    }
                }

                if (!capsuleForm.Visible) capsuleForm.Show();
                byte masterAlpha = (byte)Math.Max(0, Math.Min(255, (int)(spawnFactor * 255)));
                capsuleForm.SetBitmap(bmp, winX, winY, masterAlpha);
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
                float orbSize = pill.Height * 0.52f;
                float oy = pill.Y + (pill.Height - orbSize) / 2f;
                using (LinearGradientBrush orbBrush = new LinearGradientBrush(
                    new RectangleF(leftOffset, oy, orbSize, orbSize),
                    Color.FromArgb(alpha, accent1), Color.FromArgb(alpha, accent2), (globalTime * 90f) % 360f))
                {
                    g.FillEllipse(orbBrush, leftOffset, oy, orbSize, orbSize);
                }
                leftOffset += orbSize + 9f * scale;
            }

            // Center Label
            string centerText = media.TrackTitle;
            Color textColor = Color.FromArgb(alpha, 238, 242, 252);

            if (transientBannerText != null && DateTime.UtcNow.Ticks < transientBannerUntilTicks)
                centerText = transientBannerText;
            else if (TimerEngine.IsRunning || TimerEngine.FinishedAlertActive)
                centerText = TimerEngine.FormattedTime + "  •  " + TimerEngine.TimerLabel;
            else if (media.ActiveSource == AudioAppSource.None && !media.IsAudioActive && settings.IslandShowClockWhenIdle)
                centerText = DateTime.Now.ToString("HH:mm") + "   •   " + settings.CompanionName;

            if (centerText.Length > 26) centerText = centerText.Substring(0, 25) + "…";

            using (SolidBrush tb = new SolidBrush(textColor))
            {
                SizeF tsz = g.MeasureString(centerText, fontMedBold);
                g.DrawString(centerText, fontMedBold, tb, leftOffset, pill.Y + (pill.Height - tsz.Height) / 2f);
            }

            // Right Minimalist 5-Bar Equalizer
            int barCount = 5;
            float barW = 2.6f * scale;
            float barGap = 2.8f * scale;
            float waveStartX = pill.Right - barCount * (barW + barGap) - 12f * scale;
            float maxBarH = pill.Height * 0.48f;

            for (int i = 0; i < barCount; i++)
            {
                float bh = Math.Max(3.5f * scale, maxBarH * media.SpectrumBars[i * 2]);
                float bx = waveStartX + i * (barW + barGap);
                float by = pill.Y + (pill.Height - bh) / 2f;
                Color barCol = GlassThemeEngine.Interpolate(accent1, accent2, i / (float)(barCount - 1));
                using (SolidBrush bb = new SolidBrush(Color.FromArgb(alpha, barCol)))
                {
                    g.FillRectangle(bb, bx, by, barW, bh);
                }
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
            string[] tabTitles = new string[] { "Audio", "Minuteur", "Compagnon", "IA (" + settings.ActiveAiProvider + ")", "⚙" };

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

            // Minimalist Glass Disc Card
            float artSize = 46f * scale;
            RectangleF artRect = new RectangleF(pill.X + pad, bodyTop, artSize, artSize);
            GlassThemeEngine.DrawFrostedGlassSurface(g, artRect, 12f * scale, Color.FromArgb(20, 24, 36), accent1, alpha, 0.8f);
            using (SolidBrush wb = new SolidBrush(Color.FromArgb(alpha, accent1)))
            {
                g.FillEllipse(wb, artRect.X + artSize * 0.34f, artRect.Y + artSize * 0.34f, artSize * 0.32f, artSize * 0.32f);
            }

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
            string[] labels = new string[] { "⏮", "⏯", "⏭", "🔉", "🔊", "Spotify", "YouTube", "Web Audio", "Palette" };
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
                string quote = CompanionEngine.HasActiveSpeech ? CompanionEngine.SpeechBubbleText : "Interagissez ou changez d'entité ci-dessous.";
                if (quote.Length > 46) quote = quote.Substring(0, 44) + "…";
                g.DrawString(quote, fontSubReg, wb, tx, bodyTop + 42f * scale);
            }

            RectangleF[] btns = GetActionButtonsForCurrentTab(pill, scale);
            string[] labels = new string[] { "Stimuler (+XP)", "Changer d'Entité", "Ancrage (" + settings.CompanionPos + ")", "Réflexion IA" };
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
            float w1 = 80f * scale, w2 = 88f * scale, w3 = 96f * scale, w4 = 108f * scale, w5 = 32f * scale;
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
                    case 6: media.LaunchOrFocusService("youtube"); break;
                    case 7: media.LaunchOrFocusService("webmusic"); break;
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
            switch (src)
            {
                case AudioAppSource.Spotify: return "SPOTIFY AUDIO";
                case AudioAppSource.WebMusic: return "STREAMING WEB";
                case AudioAppSource.YouTube: return "YOUTUBE MEDIA";
                case AudioAppSource.SystemSound: return "FLUX SYSTÈME";
                default: return "NOVA AUDIO";
            }
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

                if (!form.Visible) form.Show();
                form.SetBitmap(bmp, winX, winY, 255);
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
    public class GlassCardPanel : Panel
    {
        public string CardTitle = "";
        private Font titleFont = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);

        public GlassCardPanel(int x, int y, int w, int h, string title)
        {
            Location = new Point(x, y);
            Size = new Size(w, h);
            CardTitle = title;
            BackColor = Color.Transparent;
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(1, 1, Width - 3, Height - 3);
            GlassThemeEngine.DrawFrostedGlassSurface(e.Graphics, r, 16f, Color.FromArgb(18, 21, 32), Color.FromArgb(110, 195, 255), 245, 0.85f);

            if (!string.IsNullOrEmpty(CardTitle))
            {
                using (SolidBrush tb = new SolidBrush(Color.FromArgb(145, 215, 255)))
                {
                    e.Graphics.DrawString(CardTitle, titleFont, tb, 18, 14);
                }
                using (Pen sep = new Pen(Color.FromArgb(28, 255, 255, 255), 1f))
                {
                    e.Graphics.DrawLine(sep, 18, 40, Width - 18, 40);
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
        private Button[] navButtons = new Button[5];
        private Panel[] pages = new Panel[5];
        private int activePageIndex = 0;

        private Panel cornerPreviewBox;
        private Panel companionPreviewBox;
        private ComboBox themeCombo;
        private Label aiStatusLabel;
        private TextBox aiChatTestInput;
        private System.Windows.Forms.Timer uiPreviewTimer;

        public SettingsForm(OverlayController ctrl)
        {
            controller = ctrl;
            Text = "iCorner Duo — Glassmorphism Studio";
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

            uiPreviewTimer = new System.Windows.Forms.Timer { Interval = 55 };
            uiPreviewTimer.Tick += (s, e) =>
            {
                if (Visible)
                {
                    if (activePageIndex == 0 && cornerPreviewBox != null) cornerPreviewBox.Invalidate();
                    if (activePageIndex == 3 && companionPreviewBox != null) companionPreviewBox.Invalidate();
                }
            };
            uiPreviewTimer.Start();
        }

        private void BuildMinimalGlassStudio()
        {
            // 1. Minimalist Frosted Header
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 74,
                BackColor = Color.FromArgb(14, 17, 26)
            };
            header.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (LinearGradientBrush grad = new LinearGradientBrush(
                    header.ClientRectangle, Color.FromArgb(28, 110, 215, 255), Color.FromArgb(18, 180, 130, 255), 10f))
                {
                    e.Graphics.FillRectangle(grad, header.ClientRectangle);
                }
                using (Pen bottomLine = new Pen(Color.FromArgb(35, 255, 255, 255), 1f))
                {
                    e.Graphics.DrawLine(bottomLine, 0, header.Height - 1, header.Width, header.Height - 1);
                }
                if (Icon != null) e.Graphics.DrawIcon(Icon, new Rectangle(20, 13, 48, 48));
            };

            Label titleLbl = new Label
            {
                Text = "iCorner Duo  •  Glass Studio",
                Font = new Font("Segoe UI Semibold", 15.5f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(80, 13)
            };
            Label subLbl = new Label
            {
                Text = "Bords arrondis adaptatifs • Transitions optiques • Capsule Nova (Audio & Minuteur) • Compagnon & IA",
                Font = new Font("Segoe UI", 8.8f),
                ForeColor = Color.FromArgb(165, 180, 210),
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(82, 43)
            };

            Button testTransBtn = CreateGlassBtn("Tester la Transition", 845, 18, 170, 38, Color.FromArgb(40, 115, 215), () => controller.TriggerDemoTransition());
            header.Controls.Add(titleLbl);
            header.Controls.Add(subLbl);
            header.Controls.Add(testTransBtn);
            Controls.Add(header);

            // 2. Left Glass Sidebar
            Panel sidebar = new Panel
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
                "1.  Angles & Courbures",
                "2.  Transitions & Palettes",
                "3.  Capsule Nova & Focus",
                "4.  Compagnon Minimal",
                "5.  Intelligence IA (API)"
            };

            for (int i = 0; i < 5; i++)
            {
                int idx = i;
                Button nb = new Button
                {
                    Text = navLabels[i],
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(14, 0, 0, 0),
                    Location = new Point(14, 18 + i * 50),
                    Size = new Size(206, 42),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI Semibold", 9.3f, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                nb.FlatAppearance.BorderSize = 1;
                nb.Click += (s, e) => SwitchPage(idx);
                navButtons[i] = nb;
                sidebar.Controls.Add(nb);
            }

            GlassToggleSwitch startupToggle = new GlassToggleSwitch("Démarrer avec Windows", 16, 515, 204, IsStartupEnabled());
            startupToggle.CheckedChanged += (s, e) => SetStartup(startupToggle.Checked);
            sidebar.Controls.Add(startupToggle);

            Button hideTrayBtn = CreateGlassBtn("Réduire en tâche de fond", 14, 558, 206, 36, Color.FromArgb(26, 31, 46), () => Hide());
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
            activePageIndex = idx;
            for (int i = 0; i < 5; i++)
            {
                pages[i].Visible = (i == idx);
                navButtons[i].BackColor = (i == idx) ? Color.FromArgb(35, 95, 195) : Color.FromArgb(19, 22, 34);
                navButtons[i].ForeColor = (i == idx) ? Color.White : Color.FromArgb(175, 185, 210);
                navButtons[i].FlatAppearance.BorderColor = (i == idx) ? Color.FromArgb(110, 195, 255) : Color.FromArgb(28, 34, 50);
            }
        }

        private Panel BuildPage1Corners()
        {
            Panel page = CreatePageContainer();

            GlassCardPanel leftCard = new GlassCardPanel(18, 16, 372, 585, "Géométrie & Rayon des Angles");
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
            ComboBox curveCombo = CreateComboBox(18, 188, 336, new string[] {
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
            Label monLbl = new Label { Location = new Point(18, 340), Size = new Size(336, 75), ForeColor = Color.FromArgb(145, 205, 255), BackColor = Color.FromArgb(18, 21, 32), Text = msb.ToString() };
            leftCard.Controls.Add(monLbl);

            leftCard.Controls.Add(CreateGlassBtn("Actualiser la topologie d'écrans", 18, 425, 336, 36, Color.FromArgb(28, 34, 52), () => controller.RebuildMonitors()));

            // Right Card
            GlassCardPanel rightCard = new GlassCardPanel(406, 16, 372, 585, "Finition Glassmorphique & Aperçu");
            page.Controls.Add(rightCard);

            GlassToggleSwitch bezelChk = new GlassToggleSwitch("Liseré spéculaire en verre dépoli", 18, 54, 330, controller.Settings.CornerBezelGlow);
            bezelChk.CheckedChanged += (s, e) => { controller.Settings.CornerBezelGlow = bezelChk.Checked; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(bezelChk);

            rightCard.Controls.Add(CreateLabel("Comportement lumineux des angles :", 18, 96));
            ComboBox animCombo = CreateComboBox(18, 120, 336, new string[] {
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
            cornerPreviewBox = new Panel { Location = new Point(18, 386), Size = new Size(336, 125), BackColor = Color.FromArgb(22, 26, 40) };
            cornerPreviewBox.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = cornerPreviewBox.ClientRectangle;
                Color[] pal = GlassThemeEngine.GetPalette(controller.Settings, controller.GlobalTimeSeconds);
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
            };
            rightCard.Controls.Add(cornerPreviewBox);

            return page;
        }

        private Panel BuildPage2Transitions()
        {
            Panel page = CreatePageContainer();

            GlassCardPanel leftCard = new GlassCardPanel(18, 16, 372, 585, "Transitions Optiques Inter-Écrans");
            page.Controls.Add(leftCard);

            GlassToggleSwitch enTrans = new GlassToggleSwitch("Activer la transition au passage d'écran", 18, 54, 330, controller.Settings.EnableTransition);
            enTrans.CheckedChanged += (s, e) => { controller.Settings.EnableTransition = enTrans.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(enTrans);

            GlassToggleSwitch magChk = new GlassToggleSwitch("Halo magnétique à l'approche de la bordure", 18, 88, 330, controller.Settings.EnableMagneticProximity);
            magChk.CheckedChanged += (s, e) => { controller.Settings.EnableMagneticProximity = magChk.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(magChk);

            leftCard.Controls.Add(CreateLabel("Style visuel de la transition :", 18, 132));
            ComboBox styleCombo = CreateComboBox(18, 156, 336, new string[] {
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
            GlassCardPanel rightCard = new GlassCardPanel(406, 16, 372, 585, "Palettes Glassmorphiques (12 Thèmes)");
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
            Button c1Btn = CreateGlassBtn("Teinte 1", 18, 154, 162, 36, Color.FromArgb(255, Color.FromArgb(controller.Settings.CustomColor1Argb)), null);
            c1Btn.ForeColor = Color.Black;
            c1Btn.Click += (s, e) => PickCustomColor(1, c1Btn);

            Button c2Btn = CreateGlassBtn("Teinte 2", 192, 154, 162, 36, Color.FromArgb(255, Color.FromArgb(controller.Settings.CustomColor2Argb)), null);
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

            GlassCardPanel leftCard = new GlassCardPanel(18, 16, 372, 585, "Capsule Nova & Système d'Apparition");
            page.Controls.Add(leftCard);

            leftCard.Controls.Add(CreateLabel("Mode de présence de la capsule :", 18, 54));
            ComboBox visCombo = CreateComboBox(18, 78, 336, new string[] {
                "Toujours présente en haut de l'écran",
                "Apparition au survol du bord supérieur (Hot-Edge)",
                "Contextuelle (Audio, Minuteur, Compagnon ou Transition)",
                "Uniquement lors d'un changement d'écran",
                "Masquée"
            }, (int)controller.Settings.IslandMode);
            visCombo.SelectedIndexChanged += (s, e) => { controller.Settings.IslandMode = (CapsuleVisibilityMode)visCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(visCombo);

            leftCard.Controls.Add(CreateLabel("Cinématique d'apparition :", 18, 122));
            ComboBox spawnCombo = CreateComboBox(18, 146, 336, new string[] {
                "Goutte en Verre Liquide (Liquid Drop)",
                "Déploiement Horizontal Doux",
                "Fondu Optique & Zoom Subtil",
                "Ouverture Prismatique",
                "Condensation Halo"
            }, (int)controller.Settings.SpawnAnim);
            spawnCombo.SelectedIndexChanged += (s, e) => { controller.Settings.SpawnAnim = (CapsuleSpawnStyle)spawnCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(spawnCombo);

            leftCard.Controls.Add(CreateLabel("Ancrage multi-écrans :", 18, 190));
            ComboBox posCombo = CreateComboBox(18, 214, 336, new string[] {
                "Suivre l'écran actif du curseur (Glissement fluide)",
                "Fixer sur le moniteur principal"
            }, (int)controller.Settings.IslandPlacement);
            posCombo.SelectedIndexChanged += (s, e) => { controller.Settings.IslandPlacement = (CapsuleScreenPlacement)posCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(posCombo);

            leftCard.Controls.Add(CreateLabel("Finition du contour de la capsule :", 18, 258));
            ComboBox borderCombo = CreateComboBox(18, 282, 336, new string[] {
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
            GlassCardPanel rightCard = new GlassCardPanel(406, 16, 372, 585, "Minuteur Focus & Contrôles Audio");
            page.Controls.Add(rightCard);

            rightCard.Controls.Add(CreateLabel("Sessions Focus & Minuteur rapide :", 18, 54));
            rightCard.Controls.Add(CreateGlassBtn("Minuteur 1 min", 18, 80, 162, 36, Color.FromArgb(32, 42, 64), () => controller.DynamicIsland.TimerEngine.StartCountdown(60, "1 Min")));
            rightCard.Controls.Add(CreateGlassBtn("Minuteur 5 min", 192, 80, 162, 36, Color.FromArgb(32, 42, 64), () => controller.DynamicIsland.TimerEngine.StartCountdown(300, "5 Min")));
            rightCard.Controls.Add(CreateGlassBtn("Session Focus 25 min", 18, 124, 162, 36, Color.FromArgb(40, 105, 195), () => controller.DynamicIsland.TimerEngine.StartCountdown(1500, "Focus 25m")));
            rightCard.Controls.Add(CreateGlassBtn("Chronomètre", 192, 124, 162, 36, Color.FromArgb(32, 42, 64), () => controller.DynamicIsland.TimerEngine.SwitchToStopwatch()));

            rightCard.Controls.Add(CreateGlassBtn("Démarrer / Pause", 18, 168, 215, 34, Color.FromArgb(28, 34, 52), () => controller.DynamicIsland.TimerEngine.ToggleStartPause()));
            rightCard.Controls.Add(CreateGlassBtn("Réinitialiser", 242, 168, 112, 34, Color.FromArgb(28, 34, 52), () => controller.DynamicIsland.TimerEngine.Reset()));

            rightCard.Controls.Add(CreateLabel("Contrôles Audio Universels :", 18, 224));
            rightCard.Controls.Add(CreateGlassBtn("Spotify", 18, 250, 106, 34, Color.FromArgb(24, 95, 65), () => controller.MediaTracker.LaunchOrFocusService("spotify")));
            rightCard.Controls.Add(CreateGlassBtn("YouTube", 133, 250, 106, 34, Color.FromArgb(115, 32, 48), () => controller.MediaTracker.LaunchOrFocusService("youtube")));
            rightCard.Controls.Add(CreateGlassBtn("Web Audio", 248, 250, 106, 34, Color.FromArgb(38, 46, 68), () => controller.MediaTracker.LaunchOrFocusService("webmusic")));

            rightCard.Controls.Add(CreateGlassBtn("⏮ Précédent", 18, 292, 106, 34, Color.FromArgb(28, 34, 52), () => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PREV_TRACK)));
            rightCard.Controls.Add(CreateGlassBtn("⏯ Lecture", 133, 292, 106, 34, Color.FromArgb(40, 115, 215), () => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PLAY_PAUSE)));
            rightCard.Controls.Add(CreateGlassBtn("⏭ Suivant", 248, 292, 106, 34, Color.FromArgb(28, 34, 52), () => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_NEXT_TRACK)));

            return page;
        }

        private Panel BuildPage4Companion()
        {
            Panel page = CreatePageContainer();

            GlassCardPanel leftCard = new GlassCardPanel(18, 16, 372, 585, "Entité Compagnon Minimaliste");
            page.Controls.Add(leftCard);

            leftCard.Controls.Add(CreateLabel("Choix de l'entité :", 18, 54));
            ComboBox specCombo = CreateComboBox(18, 78, 336, new string[] {
                "Aucun compagnon (Désactivé)",
                "Lumina (Orbe Céleste en Verre Dépoli)",
                "Kitsune-V (Esprit Renard Néo-Minimal)",
                "Aether-Draco (Wyvern de Cristal Translucide)",
                "Nyx-Specter (Entité Quantique & Satellites)",
                "Kuro-Neko (Félin Bionique Minimaliste)",
                "Axiom-Core (Prisme Octaédrique Optique)"
            }, (int)controller.Settings.Companion);
            specCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Companion = (CompanionEntity)specCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(specCombo);

            leftCard.Controls.Add(CreateLabel("Nom de l'entité :", 18, 124));
            TextBox nameBox = new TextBox
            {
                Location = new Point(18, 148),
                Width = 336,
                Text = controller.Settings.CompanionName,
                BackColor = Color.FromArgb(24, 28, 42),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            nameBox.TextChanged += (s, e) => { controller.Settings.CompanionName = nameBox.Text; controller.Settings.Save(); };
            leftCard.Controls.Add(nameBox);

            leftCard.Controls.Add(CreateLabel("Ancrage autour de la capsule :", 18, 190));
            ComboBox posCombo = CreateComboBox(18, 214, 336, new string[] {
                "En lévitation douce à droite de la capsule",
                "Posé au-dessus de la capsule",
                "Intégré à l'intérieur de la capsule"
            }, (int)controller.Settings.CompanionPos);
            posCombo.SelectedIndexChanged += (s, e) => { controller.Settings.CompanionPos = (CompanionAnchor)posCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(posCombo);

            leftCard.Controls.Add(CreateLabel("Échelle du compagnon :", 18, 258));
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
            GlassCardPanel rightCard = new GlassCardPanel(406, 16, 372, 585, "Aperçu Optique en Direct");
            page.Controls.Add(rightCard);

            companionPreviewBox = new Panel { Location = new Point(18, 54), Size = new Size(336, 215), BackColor = Color.FromArgb(14, 17, 26) };
            companionPreviewBox.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Color[] pal = GlassThemeEngine.GetPalette(controller.Settings, controller.GlobalTimeSeconds);
                CompanionEntity sp = controller.Settings.Companion == CompanionEntity.None ? CompanionEntity.LuminaOrb : controller.Settings.Companion;
                Point screenCenter = companionPreviewBox.PointToScreen(new Point(companionPreviewBox.Width / 2, companionPreviewBox.Height / 2));
                controller.DynamicIsland.CompanionEngine.DrawCompanion(
                    g, new RectangleF(88, 28, 160, 160), sp, Cursor.Position, screenCenter,
                    pal[0], pal[1 % pal.Length], controller.GlobalTimeSeconds, controller.MediaTracker.CurrentAudioPeak, true);
            };
            rightCard.Controls.Add(companionPreviewBox);

            rightCard.Controls.Add(CreateGlassBtn("Stimuler l'entité (+20 XP)", 18, 286, 336, 38, Color.FromArgb(40, 115, 215), () => controller.DynamicIsland.CompanionEngine.InteractBoost()));
            rightCard.Controls.Add(CreateGlassBtn("Générer une réflexion IA", 18, 334, 336, 38, Color.FromArgb(65, 55, 130), () =>
            {
                controller.DynamicIsland.AiEngine.SendPromptAsync(controller.Settings, "Dis une phrase courte, apaisante et minimaliste.", reply => controller.DynamicIsland.CompanionEngine.Say(reply, 5500));
            }));

            return page;
        }

        private Panel BuildPage5Ai()
        {
            Panel page = CreatePageContainer();

            GlassCardPanel leftCard = new GlassCardPanel(18, 16, 372, 585, "Connecteurs API (Gemini, Claude, ChatGPT, Mistral)");
            page.Controls.Add(leftCard);

            leftCard.Controls.Add(CreateLabel("Moteur IA actif :", 18, 54));
            ComboBox provCombo = CreateComboBox(18, 78, 336, new string[] {
                "Google Gemini (gemini-2.5-flash)",
                "Anthropic Claude (claude-3-5-sonnet)",
                "OpenAI ChatGPT (gpt-4o-mini)",
                "Mistral AI (mistral-small-latest)"
            }, (int)controller.Settings.ActiveAiProvider);
            provCombo.SelectedIndexChanged += (s, e) => { controller.Settings.ActiveAiProvider = (AiService)provCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(provCombo);

            leftCard.Controls.Add(CreateLabel("Clé API Gemini & Modèle :", 18, 120));
            TextBox gemKey = CreateApiInputBox(18, 142, 212, controller.Settings.GeminiApiKey, true);
            TextBox gemMod = CreateApiInputBox(236, 142, 118, controller.Settings.GeminiModel, false);
            gemKey.TextChanged += (s, e) => { controller.Settings.GeminiApiKey = gemKey.Text.Trim(); controller.Settings.Save(); };
            gemMod.TextChanged += (s, e) => { controller.Settings.GeminiModel = gemMod.Text.Trim(); controller.Settings.Save(); };
            leftCard.Controls.Add(gemKey); leftCard.Controls.Add(gemMod);

            leftCard.Controls.Add(CreateLabel("Clé API Claude & Modèle :", 18, 178));
            TextBox clKey = CreateApiInputBox(18, 200, 212, controller.Settings.ClaudeApiKey, true);
            TextBox clMod = CreateApiInputBox(236, 200, 118, controller.Settings.ClaudeModel, false);
            clKey.TextChanged += (s, e) => { controller.Settings.ClaudeApiKey = clKey.Text.Trim(); controller.Settings.Save(); };
            clMod.TextChanged += (s, e) => { controller.Settings.ClaudeModel = clMod.Text.Trim(); controller.Settings.Save(); };
            leftCard.Controls.Add(clKey); leftCard.Controls.Add(clMod);

            leftCard.Controls.Add(CreateLabel("Clé API ChatGPT & Modèle :", 18, 236));
            TextBox oaKey = CreateApiInputBox(18, 258, 212, controller.Settings.OpenAiApiKey, true);
            TextBox oaMod = CreateApiInputBox(236, 258, 118, controller.Settings.OpenAiModel, false);
            oaKey.TextChanged += (s, e) => { controller.Settings.OpenAiApiKey = oaKey.Text.Trim(); controller.Settings.Save(); };
            oaMod.TextChanged += (s, e) => { controller.Settings.OpenAiModel = oaMod.Text.Trim(); controller.Settings.Save(); };
            leftCard.Controls.Add(oaKey); leftCard.Controls.Add(oaMod);

            leftCard.Controls.Add(CreateLabel("Clé API Mistral & Modèle :", 18, 294));
            TextBox miKey = CreateApiInputBox(18, 316, 212, controller.Settings.MistralApiKey, true);
            TextBox miMod = CreateApiInputBox(236, 316, 118, controller.Settings.MistralModel, false);
            miKey.TextChanged += (s, e) => { controller.Settings.MistralApiKey = miKey.Text.Trim(); controller.Settings.Save(); };
            miMod.TextChanged += (s, e) => { controller.Settings.MistralModel = miMod.Text.Trim(); controller.Settings.Save(); };
            leftCard.Controls.Add(miKey); leftCard.Controls.Add(miMod);

            leftCard.Controls.Add(CreateLabel("Directive système (Personnalité) :", 18, 356));
            TextBox sysBox = new TextBox
            {
                Location = new Point(18, 380),
                Size = new Size(336, 92),
                Multiline = true,
                Text = controller.Settings.AiSystemPrompt,
                BackColor = Color.FromArgb(24, 28, 42),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            sysBox.TextChanged += (s, e) => { controller.Settings.AiSystemPrompt = sysBox.Text; controller.Settings.Save(); };
            leftCard.Controls.Add(sysBox);

            // Right Card
            GlassCardPanel rightCard = new GlassCardPanel(406, 16, 372, 585, "Console de Dialogue IA");
            page.Controls.Add(rightCard);

            aiStatusLabel = new Label
            {
                Location = new Point(18, 54),
                Size = new Size(336, 220),
                BackColor = Color.FromArgb(14, 17, 26),
                ForeColor = Color.FromArgb(225, 234, 250),
                Padding = new Padding(12),
                Text = controller.DynamicIsland.AiEngine.LastResponse
            };
            rightCard.Controls.Add(aiStatusLabel);

            rightCard.Controls.Add(CreateLabel("Message :", 18, 290));
            aiChatTestInput = new TextBox
            {
                Location = new Point(18, 314),
                Size = new Size(336, 28),
                BackColor = Color.FromArgb(24, 28, 42),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "Bonjour, présente-toi en une phrase minimaliste."
            };
            rightCard.Controls.Add(aiChatTestInput);

            rightCard.Controls.Add(CreateGlassBtn("Envoyer la requête à l'IA", 18, 354, 336, 40, Color.FromArgb(40, 115, 215), () =>
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

        private TextBox CreateApiInputBox(int x, int y, int w, string val, bool isPassword)
        {
            TextBox tb = new TextBox { Location = new Point(x, y), Width = w, Text = val, BackColor = Color.FromArgb(24, 28, 42), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            if (isPassword) tb.UseSystemPasswordChar = true;
            return tb;
        }

        private void PickCustomColor(int slot, Button btn)
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
            return new Panel { Location = new Point(235, 74), Size = new Size(805, 635), BackColor = Color.FromArgb(11, 13, 20) };
        }

        private Button CreateGlassBtn(string text, int x, int y, int w, int h, Color bg, Action onClick)
        {
            Button b = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(w, h),
                FlatStyle = FlatStyle.Flat,
                BackColor = bg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9.0f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Color.FromArgb(55, 255, 255, 255);
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

        private ComboBox CreateComboBox(int x, int y, int w, string[] items, int selectedIndex)
        {
            ComboBox cb = new ComboBox { Location = new Point(x, y), Width = w, DropDownStyle = ComboBoxStyle.DropDownList, BackColor = Color.FromArgb(26, 30, 44), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            cb.Items.AddRange(items);
            cb.SelectedIndex = (selectedIndex >= 0 && selectedIndex < items.Length) ? selectedIndex : 0;
            return cb;
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
