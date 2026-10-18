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
    // ENUMS & GLOBAL CONFIGURATION
    // =========================================================================
    public enum CurveType
    {
        SquircleApple = 0,
        ClassicCircle = 1
    }

    public enum CornerAnimationMode
    {
        Static = 0,
        RotatingGradient = 1,
        BreathingPulse = 2,
        AudioBeatSync = 3,
        SparkleShimmer = 4,
        NeonChase = 5
    }

    public enum TransitionStyle
    {
        ComboUltimate = 0,
        LiquidBridge = 1,
        SiriIntelligenceGlow = 2,
        ElectricPlasmaPortal = 3,
        CosmicParticleBurst = 4,
        SonarShockwave = 5,
        NeonLaserBlade = 6,
        DynamicIslandBounce = 7
    }

    public enum ColorTheme
    {
        AppleIntelligence = 0,
        iOSBlueCyan = 1,
        SpotifyNeonGreen = 2,
        AppleMusicRose = 3,
        YouTubeCrimson = 4,
        CyberpunkRainbow = 5,
        SunsetPro = 6,
        UltraVioletNebula = 7,
        MagmaGold = 8,
        ArcticAurora = 9,
        SakuraPastel = 10,
        MinimalMonochrome = 11,
        CustomColors = 12
    }

    public enum IslandVisibilityMode
    {
        AlwaysVisible = 0,          // Toujours affiché
        HotTopEdgeTrigger = 1,      // Apparaît quand la souris approche le haut de l'écran
        SmartAutoActivity = 2,      // Apparaît si musique, minuteur, compagnon ou transition
        OnlyOnTransition = 3,       // Uniquement lors du changement d'écran
        Disabled = 4                // Masqué
    }

    public enum IslandSpawnAnimation
    {
        LiquidDropDown = 0,         // Goutte liquide élastique tombant du haut
        SpringUnfold = 1,           // Déploiement horizontal avec rebond
        WormholePortal = 2,         // Portail dimensionnel lumineux
        CyberNeonSnap = 3,          // Flash laser cyberpunk
        NovaBloom = 4               // Éruption lumineuse condensée
    }

    public enum IslandPlacementMode
    {
        FollowActiveScreen = 0,
        PrimaryScreenOnly = 1
    }

    public enum IslandBorderAnimation
    {
        SiriRotatingGlow = 0,
        AudioReactiveHalo = 1,
        BreathingGlass = 2,
        MinimalClean = 3
    }

    public enum IslandActiveTab
    {
        Music = 0,
        Timer = 1,
        Companion = 2,
        AiAssistant = 3
    }

    public enum CompanionSpecies
    {
        None = 0,
        KitsuneCyberFox = 1,        // Renard Cybernétique Néon (Oreilles + Queues)
        DracoPlasmaDragon = 2,      // Mini Dragon Plasma (Ailes + Souffle)
        SpecterHoloGhost = 3,       // Esprit Holographique Cyberpunk (Anneaux + Visière LED)
        NekoBionicCat = 4,          // Chat Bionique Synthwave (Casque audio + Ronronnement)
        AtlasMechaDrone = 5,        // Drone Sphérique Sci-Fi (Noyau rotatif + Scanner)
        IgnisCosmicFlame = 6        // Élémentaire de Feu Cosmique
    }

    public enum CompanionPositionMode
    {
        PerchedOnIsland = 0,        // Perché sur le bord du Dynamic Island
        InsideIsland = 1,           // Intégré dans la pilule Dynamic Island
        FloatingNextToIsland = 2    // En lévitation à droite du Dynamic Island
    }

    public enum AiProvider
    {
        Gemini = 0,
        Claude = 1,
        ChatGPT = 2,
        Mistral = 3
    }

    public enum MediaSourceType
    {
        None = 0,
        Spotify = 1,
        AppleMusic = 2,
        YouTube = 3,
        BrowserMedia = 4,
        SystemAudio = 5
    }

    // =========================================================================
    // PERSISTENT SETTINGS
    // =========================================================================
    public class AppSettings
    {
        // 1. Corners
        public bool Enabled = true;
        public int CornerRadius = 38;
        public CurveType Curve = CurveType.SquircleApple;
        public bool TopLeft = true;
        public bool TopRight = true;
        public bool BottomLeft = true;
        public bool BottomRight = true;
        public bool CornerBezelGlow = true;
        public int BezelGlowOpacity = 65;
        public int BezelThickness = 25;
        public CornerAnimationMode CornerAnim = CornerAnimationMode.RotatingGradient;
        public int CornerAnimSpeed = 100;

        // 2. Transitions & Colors
        public bool EnableTransition = true;
        public bool EnableMagneticProximity = true;
        public TransitionStyle Style = TransitionStyle.ComboUltimate;
        public ColorTheme Theme = ColorTheme.AppleIntelligence;
        public int DurationMs = 1000;
        public int IntensityPercent = 120;
        public int CustomColor1Argb = Color.FromArgb(255, 0, 240, 255).ToArgb();
        public int CustomColor2Argb = Color.FromArgb(255, 255, 0, 128).ToArgb();

        // 3. Dynamic Island & Spawn System
        public IslandVisibilityMode IslandMode = IslandVisibilityMode.AlwaysVisible;
        public IslandSpawnAnimation SpawnAnim = IslandSpawnAnimation.LiquidDropDown;
        public IslandPlacementMode IslandPlacement = IslandPlacementMode.FollowActiveScreen;
        public IslandBorderAnimation IslandBorder = IslandBorderAnimation.SiriRotatingGlow;
        public int IslandScalePercent = 105;
        public bool IslandExpandOnHover = true;
        public bool IslandShowClockWhenIdle = true;
        public bool IslandAutoAdaptBrandColor = true;

        // 4. Companion Settings (Original Non-Apple Cyber Companion)
        public CompanionSpecies Companion = CompanionSpecies.KitsuneCyberFox;
        public CompanionPositionMode CompanionPos = CompanionPositionMode.PerchedOnIsland;
        public int CompanionScalePercent = 110;
        public bool CompanionFollowCursorEyes = true;
        public bool CompanionDanceToMusic = true;
        public bool CompanionShowSpeechBubbles = true;
        public string CompanionName = "Kitsune-X";

        // 5. AI Multi-Provider Settings (Gemini, Claude, ChatGPT, Mistral)
        public AiProvider ActiveAiProvider = AiProvider.Gemini;
        public string GeminiApiKey = "";
        public string GeminiModel = "gemini-2.5-flash";
        public string ClaudeApiKey = "";
        public string ClaudeModel = "claude-3-5-sonnet-latest";
        public string OpenAiApiKey = "";
        public string OpenAiModel = "gpt-4o-mini";
        public string MistralApiKey = "";
        public string MistralModel = "mistral-small-latest";
        public string AiSystemPrompt = "Tu es le compagnon cybernétique intelligent de l'utilisateur dans son Dynamic Island sur Windows. Réponds toujours en français de façon concise, stylée, utile et chaleureuse (max 2-3 phrases courtes pour tenir dans une bulle).";

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
                    s.CornerRadius = ReadInt(json, "CornerRadius", s.CornerRadius);
                    s.Curve = (CurveType)ReadInt(json, "Curve", (int)s.Curve);
                    s.TopLeft = ReadBool(json, "TopLeft", s.TopLeft);
                    s.TopRight = ReadBool(json, "TopRight", s.TopRight);
                    s.BottomLeft = ReadBool(json, "BottomLeft", s.BottomLeft);
                    s.BottomRight = ReadBool(json, "BottomRight", s.BottomRight);
                    s.CornerBezelGlow = ReadBool(json, "CornerBezelGlow", s.CornerBezelGlow);
                    s.BezelGlowOpacity = ReadInt(json, "BezelGlowOpacity", s.BezelGlowOpacity);
                    s.BezelThickness = ReadInt(json, "BezelThickness", s.BezelThickness);
                    s.CornerAnim = (CornerAnimationMode)ReadInt(json, "CornerAnim", (int)s.CornerAnim);
                    s.CornerAnimSpeed = ReadInt(json, "CornerAnimSpeed", s.CornerAnimSpeed);
                    s.EnableTransition = ReadBool(json, "EnableTransition", s.EnableTransition);
                    s.EnableMagneticProximity = ReadBool(json, "EnableMagneticProximity", s.EnableMagneticProximity);
                    s.Style = (TransitionStyle)ReadInt(json, "Style", (int)s.Style);
                    s.Theme = (ColorTheme)ReadInt(json, "Theme", (int)s.Theme);
                    s.DurationMs = ReadInt(json, "DurationMs", s.DurationMs);
                    s.IntensityPercent = ReadInt(json, "IntensityPercent", s.IntensityPercent);
                    s.CustomColor1Argb = ReadInt(json, "CustomColor1Argb", s.CustomColor1Argb);
                    s.CustomColor2Argb = ReadInt(json, "CustomColor2Argb", s.CustomColor2Argb);
                    s.IslandMode = (IslandVisibilityMode)ReadInt(json, "IslandMode", (int)s.IslandMode);
                    s.SpawnAnim = (IslandSpawnAnimation)ReadInt(json, "SpawnAnim", (int)s.SpawnAnim);
                    s.IslandPlacement = (IslandPlacementMode)ReadInt(json, "IslandPlacement", (int)s.IslandPlacement);
                    s.IslandBorder = (IslandBorderAnimation)ReadInt(json, "IslandBorder", (int)s.IslandBorder);
                    s.IslandScalePercent = ReadInt(json, "IslandScalePercent", s.IslandScalePercent);
                    s.IslandExpandOnHover = ReadBool(json, "IslandExpandOnHover", s.IslandExpandOnHover);
                    s.IslandShowClockWhenIdle = ReadBool(json, "IslandShowClockWhenIdle", s.IslandShowClockWhenIdle);
                    s.IslandAutoAdaptBrandColor = ReadBool(json, "IslandAutoAdaptBrandColor", s.IslandAutoAdaptBrandColor);
                    s.Companion = (CompanionSpecies)ReadInt(json, "Companion", (int)s.Companion);
                    s.CompanionPos = (CompanionPositionMode)ReadInt(json, "CompanionPos", (int)s.CompanionPos);
                    s.CompanionScalePercent = ReadInt(json, "CompanionScalePercent", s.CompanionScalePercent);
                    s.CompanionFollowCursorEyes = ReadBool(json, "CompanionFollowCursorEyes", s.CompanionFollowCursorEyes);
                    s.CompanionDanceToMusic = ReadBool(json, "CompanionDanceToMusic", s.CompanionDanceToMusic);
                    s.CompanionShowSpeechBubbles = ReadBool(json, "CompanionShowSpeechBubbles", s.CompanionShowSpeechBubbles);
                    s.CompanionName = ReadString(json, "CompanionName", s.CompanionName);
                    s.ActiveAiProvider = (AiProvider)ReadInt(json, "ActiveAiProvider", (int)s.ActiveAiProvider);
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

        private static string EscapeJson(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", " ").Replace("\n", " ");
        }

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
    // MULTI-PROVIDER AI ASSISTANT ENGINE (GEMINI, CLAUDE, CHATGPT, MISTRAL)
    // =========================================================================
    public class AiAssistantEngine
    {
        public bool IsBusy = false;
        public string LastResponse = "Salut ! Je suis ton compagnon IA. Clique sur 'Poser une question' ou configure ta clé API Gemini / Claude / ChatGPT / Mistral !";
        public string LastError = "";

        public event Action<string> OnResponseReceived;

        public AiAssistantEngine()
        {
            try
            {
                // Enable TLS 1.2 (3072) for modern HTTPS APIs on .NET 4.x
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Tls;
            }
            catch { }
        }

        public void SendPromptAsync(AppSettings settings, string userPrompt, Action<string> callback = null)
        {
            if (IsBusy) return;
            IsBusy = true;
            LastResponse = "⏳ Réflexion en cours (" + settings.ActiveAiProvider + ")...";
            if (OnResponseReceived != null) OnResponseReceived(LastResponse);

            ThreadPool.QueueUserWorkItem(_ =>
            {
                string reply = QueryProviderSync(settings, userPrompt);
                IsBusy = false;
                LastResponse = reply;
                if (OnResponseReceived != null) OnResponseReceived(reply);
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
                    ? "Tu es un compagnon IA concis et sympathique. Réponds en français en 1 à 3 phrases courtes."
                    : s.AiSystemPrompt;

                switch (s.ActiveAiProvider)
                {
                    case AiProvider.Gemini:
                        return CallGemini(s.GeminiApiKey, s.GeminiModel, sys, prompt);
                    case AiProvider.Claude:
                        return CallClaude(s.ClaudeApiKey, s.ClaudeModel, sys, prompt);
                    case AiProvider.ChatGPT:
                        return CallOpenAiCompatible("https://api.openai.com/v1/chat/completions", s.OpenAiApiKey, s.OpenAiModel, sys, prompt, "ChatGPT");
                    case AiProvider.Mistral:
                        return CallOpenAiCompatible("https://api.mistral.ai/v1/chat/completions", s.MistralApiKey, s.MistralModel, sys, prompt, "Mistral");
                    default:
                        return "Fournisseur IA inconnu.";
                }
            }
            catch (WebException wex)
            {
                string errBody = "";
                try
                {
                    if (wex.Response != null)
                    {
                        using (StreamReader sr = new StreamReader(wex.Response.GetResponseStream(), Encoding.UTF8))
                        {
                            errBody = sr.ReadToEnd();
                        }
                    }
                }
                catch { }
                string msg = ExtractJsonField(errBody, "message");
                if (!string.IsNullOrEmpty(msg)) return "⚠️ Erreur API : " + msg;
                return "⚠️ Erreur réseau/API : " + wex.Message;
            }
            catch (Exception ex)
            {
                return "⚠️ Erreur : " + ex.Message;
            }
        }

        private string CallGemini(string apiKey, string model, string sysPrompt, string userPrompt)
        {
            if (string.IsNullOrEmpty(apiKey))
                return "🔑 Clé API Gemini manquante ! Ouvrez Paramètres -> Assistant IA pour entrer votre clé Gemini.";

            string mdl = string.IsNullOrEmpty(model) ? "gemini-2.5-flash" : model.Trim();
            string url = "https://generativelanguage.googleapis.com/v1beta/models/" + mdl + ":generateContent?key=" + apiKey.Trim();

            string body = "{"
                + "\"system_instruction\":{\"parts\":[{\"text\":\"" + EscapeJsonString(sysPrompt) + "\"}]},"
                + "\"contents\":[{\"role\":\"user\",\"parts\":[{\"text\":\"" + EscapeJsonString(userPrompt) + "\"}]}]"
                + "}";

            string resp = HttpPostJson(url, body, null);
            string text = ExtractJsonField(resp, "text");
            return string.IsNullOrEmpty(text) ? "Réponse Gemini reçue (format brut)." : UnescapeJsonString(text);
        }

        private string CallClaude(string apiKey, string model, string sysPrompt, string userPrompt)
        {
            if (string.IsNullOrEmpty(apiKey))
                return "🔑 Clé API Claude manquante ! Ouvrez Paramètres -> Assistant IA pour entrer votre clé Anthropic.";

            string mdl = string.IsNullOrEmpty(model) ? "claude-3-5-sonnet-latest" : model.Trim();
            string url = "https://api.anthropic.com/v1/messages";

            string body = "{"
                + "\"model\":\"" + EscapeJsonString(mdl) + "\","
                + "\"max_tokens\":300,"
                + "\"system\":\"" + EscapeJsonString(sysPrompt) + "\","
                + "\"messages\":[{\"role\":\"user\",\"content\":\"" + EscapeJsonString(userPrompt) + "\"}]"
                + "}";

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers["x-api-key"] = apiKey.Trim();
            headers["anthropic-version"] = "2023-06-01";

            string resp = HttpPostJson(url, body, headers);
            string text = ExtractJsonField(resp, "text");
            return string.IsNullOrEmpty(text) ? "Réponse Claude reçue." : UnescapeJsonString(text);
        }

        private string CallOpenAiCompatible(string url, string apiKey, string model, string sysPrompt, string userPrompt, string label)
        {
            if (string.IsNullOrEmpty(apiKey))
                return "🔑 Clé API " + label + " manquante ! Ouvrez Paramètres -> Assistant IA pour entrer votre clé.";

            string mdl = string.IsNullOrEmpty(model) ? (label == "Mistral" ? "mistral-small-latest" : "gpt-4o-mini") : model.Trim();
            string body = "{"
                + "\"model\":\"" + EscapeJsonString(mdl) + "\","
                + "\"max_tokens\":300,"
                + "\"messages\":["
                + "{\"role\":\"system\",\"content\":\"" + EscapeJsonString(sysPrompt) + "\"},"
                + "{\"role\":\"user\",\"content\":\"" + EscapeJsonString(userPrompt) + "\"}"
                + "]}";

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers["Authorization"] = "Bearer " + apiKey.Trim();

            string resp = HttpPostJson(url, body, headers);
            string content = ExtractJsonField(resp, "content");
            return string.IsNullOrEmpty(content) ? "Réponse " + label + " reçue." : UnescapeJsonString(content);
        }

        private static string HttpPostJson(string url, string jsonBody, Dictionary<string, string> headers)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json; charset=utf-8";
            req.Timeout = 20000;
            if (headers != null)
            {
                foreach (var kv in headers)
                {
                    req.Headers[kv.Key] = kv.Value;
                }
            }
            byte[] bytes = Encoding.UTF8.GetBytes(jsonBody);
            req.ContentLength = bytes.Length;
            using (Stream st = req.GetRequestStream())
            {
                st.Write(bytes, 0, bytes.Length);
            }
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        private static string EscapeJsonString(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
        }

        private static string UnescapeJsonString(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("\\n", "\n").Replace("\\r", "").Replace("\\\"", "\"").Replace("\\\\", "\\");
        }

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
                if (escaped)
                {
                    if (c == 'n') sb.Append('\n');
                    else if (c == 'r') { }
                    else sb.Append(c);
                    escaped = false;
                }
                else if (c == '\\')
                {
                    escaped = true;
                }
                else if (c == '"')
                {
                    return sb.ToString();
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }

    // =========================================================================
    // APPLE-STYLE TIMER, POMODORO & STOPWATCH ENGINE FOR DYNAMIC ISLAND
    // =========================================================================
    public class IslandTimerEngine
    {
        public bool IsRunning = false;
        public bool IsStopwatchMode = false;
        public double TotalSeconds = 300;     // Default 5:00
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
            if (!IsRunning && !IsStopwatchMode && RemainingSeconds <= 0)
            {
                RemainingSeconds = TotalSeconds;
            }
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
            TimerLabel = "Chronomètre";
            IsRunning = true;
            lastTickTime = DateTime.UtcNow.Ticks;
        }

        public void StepTick()
        {
            long now = DateTime.UtcNow.Ticks;
            if (FinishedAlertActive && now > alertUntilTicks)
            {
                FinishedAlertActive = false;
            }

            if (!IsRunning)
            {
                lastTickTime = now;
                return;
            }

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
                    alertUntilTicks = now + 60000000L; // 6s alert
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
                int mins = total / 60;
                int rem = total % 60;
                return mins.ToString("00") + ":" + rem.ToString("00");
            }
        }
    }

    // =========================================================================
    // ORIGINAL CYBER / ANIME / SCI-FI COMPANION ENGINE (NON-APPLE)
    // =========================================================================
    public class CyberCompanionEngine
    {
        public int Level = 1;
        public int Xp = 25;
        public string Mood = "Énergique ⚡";
        public string SpeechBubbleText = "";
        private long speechUntilTicks = 0;
        private float excitementBoost = 0f;
        private float jumpOffset = 0f;

        private static readonly string[] FunQuotes = new string[] {
            "Prêt à explorer tes écrans ! ⚡",
            "Ce son est incroyable ! 🎵",
            "Concentration maximale activée ! 🔥",
            "Survole le Dynamic Island pour voir mes pouvoirs !",
            "Clique sur l'onglet IA pour discuter avec moi ! 🤖",
            "Tes bords arrondis sont ultra propres ✨"
        };
        private Random rng = new Random();

        public void Say(string text, int durationMs = 4500)
        {
            SpeechBubbleText = text;
            speechUntilTicks = DateTime.UtcNow.Ticks + durationMs * 10000L;
            excitementBoost = 1.0f;
        }

        public void InteractPetOrBoost()
        {
            Xp += 15;
            if (Xp >= 100)
            {
                Level++;
                Xp -= 100;
                Mood = "Ultra Évolué 🌟";
                Say("🎉 NIVEAU SUPÉRIEUR ! Niveau " + Level + " atteint !", 4000);
            }
            else
            {
                Mood = "Heureux ♥";
                Say(FunQuotes[rng.Next(FunQuotes.Length)], 3500);
            }
            excitementBoost = 1.0f;
        }

        public bool HasActiveSpeech
        {
            get { return !string.IsNullOrEmpty(SpeechBubbleText) && DateTime.UtcNow.Ticks < speechUntilTicks; }
        }

        public void StepTick(float globalTime, bool isAudioPlaying)
        {
            if (excitementBoost > 0f) excitementBoost = Math.Max(0f, excitementBoost - 0.025f);
            float bounceFreq = isAudioPlaying ? 9.5f : 3.2f;
            float bounceAmp = isAudioPlaying ? 5.5f : 2.5f;
            jumpOffset = (float)Math.Abs(Math.Sin(globalTime * bounceFreq)) * bounceAmp + excitementBoost * 8f;
        }

        public void DrawCompanion(
            Graphics g,
            RectangleF bounds,
            CompanionSpecies species,
            Point cursorScreenPos,
            Point companionScreenCenter,
            Color accent1,
            Color accent2,
            float globalTime,
            float audioPeak,
            bool followEyes)
        {
            if (species == CompanionSpecies.None) return;

            SmoothingMode oldMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float cx = bounds.X + bounds.Width / 2f;
            float cy = bounds.Y + bounds.Height / 2f - jumpOffset * 0.6f;
            float r = Math.Min(bounds.Width, bounds.Height) * 0.40f;

            // Calculate eye look vector towards mouse cursor
            float lookX = 0f, lookY = 0f;
            if (followEyes)
            {
                float dx = cursorScreenPos.X - companionScreenCenter.X;
                float dy = cursorScreenPos.Y - companionScreenCenter.Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                if (dist > 1f)
                {
                    float factor = Math.Min(1f, dist / 250f);
                    lookX = (dx / dist) * r * 0.22f * factor;
                    lookY = (dy / dist) * r * 0.22f * factor;
                }
            }

            // Outer aura ring reactive to audio/excitement
            float auraRad = r * (1.28f + audioPeak * 0.35f + excitementBoost * 0.25f);
            using (SolidBrush auraBrush = new SolidBrush(Color.FromArgb(55, accent1)))
            {
                g.FillEllipse(auraBrush, cx - auraRad, cy - auraRad, auraRad * 2f, auraRad * 2f);
            }

            switch (species)
            {
                case CompanionSpecies.KitsuneCyberFox:
                    DrawKitsuneFox(g, cx, cy, r, lookX, lookY, accent1, accent2, globalTime, audioPeak);
                    break;
                case CompanionSpecies.DracoPlasmaDragon:
                    DrawPlasmaDragon(g, cx, cy, r, lookX, lookY, accent1, accent2, globalTime, audioPeak);
                    break;
                case CompanionSpecies.SpecterHoloGhost:
                    DrawHoloGhost(g, cx, cy, r, lookX, lookY, accent1, accent2, globalTime, audioPeak);
                    break;
                case CompanionSpecies.NekoBionicCat:
                    DrawBionicCat(g, cx, cy, r, lookX, lookY, accent1, accent2, globalTime, audioPeak);
                    break;
                case CompanionSpecies.AtlasMechaDrone:
                    DrawMechaDrone(g, cx, cy, r, lookX, lookY, accent1, accent2, globalTime, audioPeak);
                    break;
                case CompanionSpecies.IgnisCosmicFlame:
                    DrawCosmicFlame(g, cx, cy, r, lookX, lookY, accent1, accent2, globalTime, audioPeak);
                    break;
            }

            // Musical notes floating when audio is playing!
            if (audioPeak > 0.04f)
            {
                float noteY = cy - r * 1.1f - ((globalTime * 28f) % (r * 0.9f));
                float noteX = cx + r * 0.95f + (float)Math.Sin(globalTime * 6f) * 5f;
                using (Font nf = new Font("Segoe UI Symbol", Math.Max(7f, r * 0.48f), FontStyle.Bold))
                using (SolidBrush nb = new SolidBrush(accent1))
                {
                    g.DrawString("♪", nf, nb, noteX, noteY);
                }
            }

            g.SmoothingMode = oldMode;
        }

        private void DrawKitsuneFox(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t, float audio)
        {
            // Swaying glowing tails behind
            for (int tail = -1; tail <= 1; tail += 2)
            {
                float sway = (float)Math.Sin(t * 5.5f + tail) * r * 0.35f;
                PointF[] tailPts = new PointF[] {
                    new PointF(cx, cy + r * 0.3f),
                    new PointF(cx + tail * r * 1.35f, cy - r * 0.2f + sway),
                    new PointF(cx + tail * r * 0.85f, cy + r * 0.65f + sway)
                };
                using (SolidBrush tb = new SolidBrush(Color.FromArgb(180, c2)))
                {
                    g.FillPolygon(tb, tailPts);
                }
            }

            // Pointed Cyber Fox Ears
            float earTwitch = (float)Math.Sin(t * 4.0f) * r * 0.08f;
            PointF[] leftEar = new PointF[] {
                new PointF(cx - r * 0.75f, cy - r * 0.25f),
                new PointF(cx - r * 0.95f, cy - r * 1.35f + earTwitch),
                new PointF(cx - r * 0.18f, cy - r * 0.75f)
            };
            PointF[] rightEar = new PointF[] {
                new PointF(cx + r * 0.75f, cy - r * 0.25f),
                new PointF(cx + r * 0.95f, cy - r * 1.35f - earTwitch),
                new PointF(cx + r * 0.18f, cy - r * 0.75f)
            };

            using (SolidBrush darkBody = new SolidBrush(Color.FromArgb(250, 18, 20, 30)))
            using (Pen neonRim = new Pen(c1, 2.0f))
            {
                g.FillPolygon(darkBody, leftEar);
                g.DrawPolygon(neonRim, leftEar);
                g.FillPolygon(darkBody, rightEar);
                g.DrawPolygon(neonRim, rightEar);

                // Fox head diamond-rounded shape
                PointF[] headPts = new PointF[] {
                    new PointF(cx, cy - r * 0.85f),
                    new PointF(cx + r * 0.95f, cy + r * 0.05f),
                    new PointF(cx, cy + r * 0.85f),
                    new PointF(cx - r * 0.95f, cy + r * 0.05f)
                };
                g.FillPolygon(darkBody, headPts);
                g.DrawPolygon(neonRim, headPts);
            }

            // Glowing Cyber Eyes following cursor
            DrawExpressiveEyes(g, cx, cy - r * 0.05f, r, lx, ly, c1, t);

            // Neon nose
            using (SolidBrush noseB = new SolidBrush(c2))
            {
                g.FillEllipse(noseB, cx - 2.5f, cy + r * 0.35f, 5f, 4f);
            }
        }

        private void DrawPlasmaDragon(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t, float audio)
        {
            // Flapping Dragon Wings
            float wingFlap = (float)Math.Sin(t * 8.0f) * r * 0.45f;
            for (int side = -1; side <= 1; side += 2)
            {
                PointF[] wing = new PointF[] {
                    new PointF(cx + side * r * 0.5f, cy),
                    new PointF(cx + side * r * 1.65f, cy - r * 0.85f + wingFlap),
                    new PointF(cx + side * r * 1.25f, cy + r * 0.45f + wingFlap * 0.5f)
                };
                using (SolidBrush wb = new SolidBrush(Color.FromArgb(190, c2)))
                using (Pen wp = new Pen(c1, 1.5f))
                {
                    g.FillPolygon(wb, wing);
                    g.DrawPolygon(wp, wing);
                }
            }

            // Dragon Horns
            PointF[] lHorn = new PointF[] { new PointF(cx - r * 0.5f, cy - r * 0.6f), new PointF(cx - r * 0.85f, cy - r * 1.4f), new PointF(cx - r * 0.2f, cy - r * 0.8f) };
            PointF[] rHorn = new PointF[] { new PointF(cx + r * 0.5f, cy - r * 0.6f), new PointF(cx + r * 0.85f, cy - r * 1.4f), new PointF(cx + r * 0.2f, cy - r * 0.8f) };
            using (SolidBrush hb = new SolidBrush(c1))
            {
                g.FillPolygon(hb, lHorn);
                g.FillPolygon(hb, rHorn);
            }

            // Dragon Head
            using (SolidBrush body = new SolidBrush(Color.FromArgb(250, 16, 18, 28)))
            using (Pen border = new Pen(c1, 2.0f))
            {
                g.FillEllipse(body, cx - r * 0.85f, cy - r * 0.8f, r * 1.7f, r * 1.6f);
                g.DrawEllipse(border, cx - r * 0.85f, cy - r * 0.8f, r * 1.7f, r * 1.6f);
            }

            DrawExpressiveEyes(g, cx, cy - r * 0.05f, r, lx, ly, c1, t);
        }

        private void DrawHoloGhost(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t, float audio)
        {
            // Orbiting holographic energy ring
            using (Pen orbitPen = new Pen(Color.FromArgb(180, c2), 2.0f))
            {
                g.DrawEllipse(orbitPen, cx - r * 1.3f, cy - r * 0.35f, r * 2.6f, r * 0.7f);
            }

            using (LinearGradientBrush holoGrad = new LinearGradientBrush(
                new RectangleF(cx - r, cy - r, r * 2f, r * 2f),
                Color.FromArgb(230, 18, 24, 38),
                Color.FromArgb(210, c1),
                90f))
            using (Pen rim = new Pen(c1, 2f))
            {
                g.FillEllipse(holoGrad, cx - r * 0.85f, cy - r * 0.85f, r * 1.7f, r * 1.7f);
                g.DrawEllipse(rim, cx - r * 0.85f, cy - r * 0.85f, r * 1.7f, r * 1.7f);
            }

            DrawExpressiveEyes(g, cx, cy, r, lx, ly, Color.White, t);
        }

        private void DrawBionicCat(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t, float audio)
        {
            // Cat Ears
            PointF[] lEar = new PointF[] { new PointF(cx - r * 0.8f, cy - r * 0.2f), new PointF(cx - r * 0.75f, cy - r * 1.15f), new PointF(cx - r * 0.2f, cy - r * 0.75f) };
            PointF[] rEar = new PointF[] { new PointF(cx + r * 0.8f, cy - r * 0.2f), new PointF(cx + r * 0.75f, cy - r * 1.15f), new PointF(cx + r * 0.2f, cy - r * 0.75f) };

            using (SolidBrush body = new SolidBrush(Color.FromArgb(250, 16, 18, 26)))
            using (Pen rim = new Pen(c1, 2f))
            {
                g.FillPolygon(body, lEar); g.DrawPolygon(rim, lEar);
                g.FillPolygon(body, rEar); g.DrawPolygon(rim, rEar);
                g.FillEllipse(body, cx - r * 0.85f, cy - r * 0.78f, r * 1.7f, r * 1.56f);
                g.DrawEllipse(rim, cx - r * 0.85f, cy - r * 0.78f, r * 1.7f, r * 1.56f);
            }

            // Neon Headphones arch when music plays!
            using (Pen hpPen = new Pen(c2, 3.0f))
            {
                g.DrawArc(hpPen, cx - r * 0.98f, cy - r * 0.92f, r * 1.96f, r * 1.6f, 195, 150);
            }

            DrawExpressiveEyes(g, cx, cy, r, lx, ly, c1, t);
        }

        private void DrawMechaDrone(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t, float audio)
        {
            // Rotating outer tech segments
            using (Pen segPen = new Pen(c2, 2.5f))
            {
                float ang = (t * 110f) % 360f;
                g.DrawArc(segPen, cx - r * 1.15f, cy - r * 1.15f, r * 2.3f, r * 2.3f, ang, 70);
                g.DrawArc(segPen, cx - r * 1.15f, cy - r * 1.15f, r * 2.3f, r * 2.3f, ang + 180, 70);
            }

            using (SolidBrush coreB = new SolidBrush(Color.FromArgb(250, 14, 18, 28)))
            using (Pen rim = new Pen(c1, 2.2f))
            {
                g.FillEllipse(coreB, cx - r * 0.85f, cy - r * 0.85f, r * 1.7f, r * 1.7f);
                g.DrawEllipse(rim, cx - r * 0.85f, cy - r * 0.85f, r * 1.7f, r * 1.7f);
            }

            // Central glowing Cyclops AI Eye following cursor
            float er = r * 0.36f;
            using (SolidBrush eyeGlow = new SolidBrush(c1))
            using (SolidBrush pupil = new SolidBrush(Color.White))
            {
                g.FillEllipse(eyeGlow, cx + lx - er, cy + ly - er, er * 2f, er * 2f);
                g.FillEllipse(pupil, cx + lx * 1.2f - er * 0.4f, cy + ly * 1.2f - er * 0.4f, er * 0.8f, er * 0.8f);
            }
        }

        private void DrawCosmicFlame(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t, float audio)
        {
            for (int layer = 3; layer >= 1; layer--)
            {
                float lr = r * (0.65f + layer * 0.22f);
                float flicker = (float)Math.Sin(t * 12f + layer * 2f) * r * 0.15f;
                PointF[] flamePts = new PointF[] {
                    new PointF(cx + flicker, cy - lr * 1.35f),
                    new PointF(cx + lr * 0.9f, cy + lr * 0.1f),
                    new PointF(cx, cy + lr * 0.85f),
                    new PointF(cx - lr * 0.9f, cy + lr * 0.1f)
                };
                Color fc = (layer == 1) ? Color.FromArgb(245, 18, 20, 32) : (layer == 2 ? c1 : Color.FromArgb(160, c2));
                using (SolidBrush fb = new SolidBrush(fc))
                {
                    g.FillClosedCurve(fb, flamePts);
                }
            }

            DrawExpressiveEyes(g, cx, cy + r * 0.05f, r, lx, ly, c1, t);
        }

        private void DrawExpressiveEyes(Graphics g, float cx, float cy, float r, float lx, float ly, Color eyeColor, float t)
        {
            // Periodic natural blink every ~4.2 seconds
            bool blinking = ((t % 4.2f) > 4.04f);
            float eyeOffsetX = r * 0.34f;
            float ew = r * 0.28f;
            float eh = blinking ? 2.0f : r * 0.34f;

            using (SolidBrush eb = new SolidBrush(eyeColor))
            using (SolidBrush shine = new SolidBrush(Color.White))
            {
                // Left Eye
                g.FillEllipse(eb, cx - eyeOffsetX + lx - ew / 2f, cy + ly - eh / 2f, ew, eh);
                // Right Eye
                g.FillEllipse(eb, cx + eyeOffsetX + lx - ew / 2f, cy + ly - eh / 2f, ew, eh);

                if (!blinking)
                {
                    float sr = ew * 0.32f;
                    g.FillEllipse(shine, cx - eyeOffsetX + lx - sr * 0.3f, cy + ly - eh * 0.25f, sr, sr);
                    g.FillEllipse(shine, cx + eyeOffsetX + lx - sr * 0.3f, cy + ly - eh * 0.25f, sr, sr);
                }
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

        public const byte VK_VOLUME_MUTE = 0xAD;
        public const byte VK_VOLUME_DOWN = 0xAE;
        public const byte VK_VOLUME_UP = 0xAF;
        public const byte VK_MEDIA_NEXT_TRACK = 0xB0;
        public const byte VK_MEDIA_PREV_TRACK = 0xB1;
        public const byte VK_MEDIA_STOP = 0xB2;
        public const byte VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const uint KEYEVENTF_KEYUP = 0x0002;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y) { X = x; Y = y; }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public int cx;
            public int cy;
            public SIZE(int x, int y) { cx = x; cy = y; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

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

        public MediaSourceType ActiveSource = MediaSourceType.None;
        public string TrackTitle = "Aucune musique détectée";
        public string TrackArtist = "Spotify • Apple Music • YouTube";

        private long lastWindowScanTicks = 0;
        public float[] SpectrumBars = new float[18];

        public LiveMediaAndAudioTracker()
        {
            InitializeCoreAudio();
            ScanMediaWindows();
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
                    {
                        audioMeter = (IAudioMeterInformation)obj;
                    }
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

            IsAudioActive = (CurrentAudioPeak > 0.015f);

            for (int i = 0; i < SpectrumBars.Length; i++)
            {
                float target;
                if (IsAudioActive)
                {
                    float freqShape = (float)Math.Sin((i / (float)SpectrumBars.Length) * Math.PI);
                    float wave1 = (float)Math.Sin(globalTime * 11.5f + i * 0.85f) * 0.5f + 0.5f;
                    float wave2 = (float)Math.Cos(globalTime * 17.2f - i * 1.3f) * 0.5f + 0.5f;
                    target = Math.Min(1.0f, CurrentAudioPeak * 1.45f * (0.35f + 0.65f * freqShape) * (0.4f + 0.6f * (wave1 * 0.6f + wave2 * 0.4f)));
                    if (target < 0.12f) target = 0.12f;
                }
                else
                {
                    target = 0.12f + 0.08f * (float)Math.Sin(globalTime * 3.0f + i * 0.6f);
                }
                SpectrumBars[i] = SpectrumBars[i] * 0.6f + target * 0.4f;
            }

            long now = DateTime.UtcNow.Ticks;
            if ((now - lastWindowScanTicks) > 15000000L)
            {
                lastWindowScanTicks = now;
                ScanMediaWindows();
            }
        }

        public void ScanMediaWindows()
        {
            try
            {
                Process[] procs = Process.GetProcesses();
                MediaSourceType foundSource = MediaSourceType.None;
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

                    if (pname == "spotify")
                    {
                        foundSource = MediaSourceType.Spotify;
                        if (wtitle.StartsWith("Spotify", StringComparison.OrdinalIgnoreCase))
                        {
                            foundTitle = "Spotify prêt";
                            foundArtist = "En pause • Cliquez sur Lecture";
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

                    if (pname == "applemusic" || pname == "itunes")
                    {
                        foundSource = MediaSourceType.AppleMusic;
                        if (wtitle.Equals("Apple Music", StringComparison.OrdinalIgnoreCase) || wtitle.Equals("iTunes", StringComparison.OrdinalIgnoreCase))
                        {
                            foundTitle = "Apple Music";
                            foundArtist = "Prêt à écouter";
                        }
                        else if (wtitle.Contains(" - "))
                        {
                            int dash = wtitle.IndexOf(" - ");
                            foundTitle = wtitle.Substring(0, dash).Trim();
                            foundArtist = wtitle.Substring(dash + 3).Trim() + " • Apple Music";
                        }
                        else
                        {
                            foundTitle = wtitle;
                            foundArtist = "Apple Music";
                        }
                        break;
                    }

                    if (pname == "chrome" || pname == "msedge" || pname == "firefox" || pname == "brave" || pname == "opera" || pname == "vivaldi")
                    {
                        string clean = StripBrowserSuffix(wtitle);
                        if (clean.IndexOf("YouTube Music", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            foundSource = MediaSourceType.YouTube;
                            foundTitle = clean.Replace("- YouTube Music", "").Replace("YouTube Music", "").Trim();
                            if (string.IsNullOrEmpty(foundTitle)) foundTitle = "YouTube Music";
                            foundArtist = "YouTube Music";
                        }
                        else if (clean.IndexOf("YouTube", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            foundSource = MediaSourceType.YouTube;
                            foundTitle = clean.Replace("- YouTube", "").Replace("YouTube", "").Trim();
                            if (string.IsNullOrEmpty(foundTitle)) foundTitle = "YouTube";
                            foundArtist = "YouTube";
                        }
                        else if (clean.IndexOf("Apple Music", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            foundSource = MediaSourceType.AppleMusic;
                            foundTitle = clean.Replace("- Apple Music", "").Replace("Apple Music", "").Trim();
                            if (string.IsNullOrEmpty(foundTitle)) foundTitle = "Apple Music Web";
                            foundArtist = "Apple Music";
                        }
                        else if (clean.IndexOf("Spotify", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            foundSource = MediaSourceType.Spotify;
                            foundTitle = clean.Replace("- Spotify", "").Replace("Spotify", "").Trim();
                            if (string.IsNullOrEmpty(foundTitle)) foundTitle = "Spotify Web";
                            foundArtist = "Spotify";
                        }
                    }
                }

                if (foundSource != MediaSourceType.None)
                {
                    ActiveSource = foundSource;
                    TrackTitle = foundTitle;
                    TrackArtist = foundArtist;
                }
                else if (IsAudioActive)
                {
                    ActiveSource = MediaSourceType.SystemAudio;
                    TrackTitle = "Audio Système en cours";
                    TrackArtist = "Flux audio actif";
                }
                else
                {
                    ActiveSource = MediaSourceType.None;
                    TrackTitle = "iCorner Duo Hub";
                    TrackArtist = "Musique • Minuteur • Compagnon • IA";
                }
            }
            catch { }
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
            {
                if (t.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    t = t.Substring(0, t.Length - s.Length).Trim();
            }
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
                    Process[] am = Process.GetProcessesByName("AppleMusic");
                    if (am.Length > 0 && am[0].MainWindowHandle != IntPtr.Zero)
                    {
                        NativeMethods.ShowWindow(am[0].MainWindowHandle, 9);
                        NativeMethods.SetForegroundWindow(am[0].MainWindowHandle);
                        return;
                    }
                    Process.Start("https://music.apple.com");
                }
                else if (service == "youtube")
                {
                    Process.Start("https://music.youtube.com");
                }
            }
            catch { }
        }
    }

    // =========================================================================
    // COLOR PALETTES & DYNAMIC COLOR ENGINE
    // =========================================================================
    public static class ThemeColorEngine
    {
        public static Color[] GetPalette(AppSettings settings, float timeSeconds)
        {
            switch (settings.Theme)
            {
                case ColorTheme.iOSBlueCyan:
                    return new Color[] { Color.FromArgb(255, 0, 210, 255), Color.FromArgb(255, 10, 132, 255), Color.FromArgb(255, 94, 92, 230), Color.FromArgb(255, 110, 235, 255) };
                case ColorTheme.SpotifyNeonGreen:
                    return new Color[] { Color.FromArgb(255, 29, 185, 84), Color.FromArgb(255, 30, 215, 96), Color.FromArgb(255, 0, 235, 165), Color.FromArgb(255, 110, 255, 145) };
                case ColorTheme.AppleMusicRose:
                    return new Color[] { Color.FromArgb(255, 250, 36, 60), Color.FromArgb(255, 255, 55, 95), Color.FromArgb(255, 191, 90, 242), Color.FromArgb(255, 255, 135, 160) };
                case ColorTheme.YouTubeCrimson:
                    return new Color[] { Color.FromArgb(255, 255, 0, 51), Color.FromArgb(255, 255, 85, 0), Color.FromArgb(255, 255, 0, 128), Color.FromArgb(255, 255, 190, 30) };
                case ColorTheme.CyberpunkRainbow:
                    return new Color[] {
                        FromHsv((timeSeconds * 90f) % 360f, 0.9f, 1.0f),
                        FromHsv((timeSeconds * 90f + 90f) % 360f, 0.9f, 1.0f),
                        FromHsv((timeSeconds * 90f + 180f) % 360f, 0.9f, 1.0f),
                        FromHsv((timeSeconds * 90f + 270f) % 360f, 0.9f, 1.0f)
                    };
                case ColorTheme.SunsetPro:
                    return new Color[] { Color.FromArgb(255, 255, 69, 58), Color.FromArgb(255, 255, 55, 95), Color.FromArgb(255, 191, 90, 242), Color.FromArgb(255, 255, 159, 10) };
                case ColorTheme.UltraVioletNebula:
                    return new Color[] { Color.FromArgb(255, 165, 60, 255), Color.FromArgb(255, 94, 92, 230), Color.FromArgb(255, 255, 60, 195), Color.FromArgb(255, 70, 190, 255) };
                case ColorTheme.MagmaGold:
                    return new Color[] { Color.FromArgb(255, 255, 200, 20), Color.FromArgb(255, 255, 105, 0), Color.FromArgb(255, 255, 35, 35), Color.FromArgb(255, 255, 235, 110) };
                case ColorTheme.ArcticAurora:
                    return new Color[] { Color.FromArgb(255, 0, 255, 160), Color.FromArgb(255, 0, 205, 255), Color.FromArgb(255, 80, 120, 255), Color.FromArgb(255, 140, 255, 220) };
                case ColorTheme.SakuraPastel:
                    return new Color[] { Color.FromArgb(255, 255, 155, 200), Color.FromArgb(255, 205, 155, 255), Color.FromArgb(255, 255, 195, 160), Color.FromArgb(255, 150, 220, 255) };
                case ColorTheme.MinimalMonochrome:
                    return new Color[] { Color.FromArgb(255, 245, 247, 255), Color.FromArgb(255, 195, 205, 220), Color.FromArgb(255, 150, 160, 175), Color.FromArgb(255, 255, 255, 255) };
                case ColorTheme.CustomColors:
                    Color c1 = Color.FromArgb(255, Color.FromArgb(settings.CustomColor1Argb));
                    Color c2 = Color.FromArgb(255, Color.FromArgb(settings.CustomColor2Argb));
                    return new Color[] { c1, c2, Interpolate(c1, c2, 0.5f), Interpolate(c1, Color.White, 0.35f) };
                case ColorTheme.AppleIntelligence:
                default:
                    return new Color[] { Color.FromArgb(255, 64, 205, 255), Color.FromArgb(255, 185, 80, 255), Color.FromArgb(255, 255, 70, 160), Color.FromArgb(255, 255, 165, 60) };
            }
        }

        public static Color GetMediaBrandColor(MediaSourceType src, Color fallback)
        {
            switch (src)
            {
                case MediaSourceType.Spotify: return Color.FromArgb(255, 30, 215, 96);
                case MediaSourceType.AppleMusic: return Color.FromArgb(255, 250, 45, 72);
                case MediaSourceType.YouTube: return Color.FromArgb(255, 255, 30, 60);
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
    }

    // =========================================================================
    // BASE LAYERED OVERLAY FORM
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

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

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
    // CORNER OVERLAY MANAGER PER MONITOR
    // =========================================================================
    public enum CornerPosition
    {
        TopLeft = 0,
        TopRight = 1,
        BottomLeft = 2,
        BottomRight = 3
    }

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
            float beatExtra = (settings.CornerAnim == CornerAnimationMode.AudioBeatSync) ? (audioPeak * 0.85f) : 0f;
            float totalGlow = Math.Min(1.2f, PulseGlowAmount + beatExtra);

            int glowExtra = (totalGlow > 0.01f || settings.CornerAnim != CornerAnimationMode.Static) ? 28 : 16;
            int effectiveRadius = r + (int)(PulseGlowAmount * 7f) + (int)(beatExtra * 5f);
            int boxSize = effectiveRadius + glowExtra;

            for (int i = 0; i < 4; i++)
            {
                if (!active[i])
                {
                    CornerForms[i].Hide();
                    continue;
                }
                if (!CornerForms[i].Visible) CornerForms[i].Show();

                CornerPosition pos = (CornerPosition)i;
                int x = Bounds.Left;
                int y = Bounds.Top;
                if (pos == CornerPosition.TopRight || pos == CornerPosition.BottomRight)
                    x = Bounds.Right - boxSize;
                if (pos == CornerPosition.BottomLeft || pos == CornerPosition.BottomRight)
                    y = Bounds.Bottom - boxSize;

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

                GraphicsPath cornerMask = CreateCornerMaskPath(size, radius, pos, settings.Curve);
                GraphicsPath curveLine = CreateCornerCurvePath(size, radius, pos, settings.Curve);

                Color cPrimary = (themeColors != null && themeColors.Length > 0) ? themeColors[0] : Color.Cyan;
                Color cSecondary = (themeColors != null && themeColors.Length > 1) ? themeColors[1] : Color.Magenta;
                float cornerOffset = (int)pos * 0.75f;

                float animGlowBoost = 0f;
                if (settings.CornerAnim == CornerAnimationMode.RotatingGradient && themeColors != null)
                {
                    float wave = (float)(0.5 + 0.5 * Math.Sin(animPhase * 2.5f + cornerOffset));
                    int idxA = ((int)(animPhase + (int)pos)) % themeColors.Length;
                    int idxB = (idxA + 1) % themeColors.Length;
                    cPrimary = ThemeColorEngine.Interpolate(themeColors[idxA], themeColors[idxB], wave);
                    animGlowBoost = 0.35f;
                }
                else if (settings.CornerAnim == CornerAnimationMode.BreathingPulse)
                {
                    float breath = (float)(0.5 + 0.5 * Math.Sin(animPhase * 2.0f));
                    cPrimary = ThemeColorEngine.Interpolate(cPrimary, cSecondary, breath);
                    animGlowBoost = 0.15f + 0.45f * breath;
                }
                else if (settings.CornerAnim == CornerAnimationMode.AudioBeatSync)
                {
                    cPrimary = ThemeColorEngine.Interpolate(cPrimary, cSecondary, Math.Min(1f, audioPeak * 1.4f));
                    animGlowBoost = 0.15f + audioPeak * 0.95f;
                }
                else if (settings.CornerAnim == CornerAnimationMode.NeonChase)
                {
                    float chase = (float)Math.Max(0.0, Math.Sin(animPhase * 3.2f - (int)pos * 1.57f));
                    animGlowBoost = chase * 0.75f;
                }
                else if (settings.CornerAnim == CornerAnimationMode.SparkleShimmer)
                {
                    animGlowBoost = 0.28f;
                }

                float effectiveGlow = Math.Min(1.25f, pulseGlow + animGlowBoost * (settings.BezelGlowOpacity / 100f));

                if (settings.CornerBezelGlow && effectiveGlow > 0.02f)
                {
                    for (int layer = 4; layer >= 1; layer--)
                    {
                        float width = layer * 3.8f * Math.Max(0.5f, effectiveGlow);
                        int alpha = (int)(Math.Min(1f, effectiveGlow) * (115 / layer));
                        if (alpha > 255) alpha = 255;
                        if (alpha <= 2) continue;
                        using (Pen glowPen = new Pen(Color.FromArgb(alpha, cPrimary), width))
                        {
                            glowPen.StartCap = LineCap.Round;
                            glowPen.EndCap = LineCap.Round;
                            g.DrawPath(glowPen, curveLine);
                        }
                    }
                }

                if (settings.CornerBezelGlow && settings.BezelGlowOpacity > 0)
                {
                    int bezelAlpha = (int)(settings.BezelGlowOpacity * 2.55f);
                    if (bezelAlpha > 255) bezelAlpha = 255;
                    Color bezelColor = (settings.CornerAnim == CornerAnimationMode.Static && pulseGlow <= 0.05f)
                        ? ThemeColorEngine.Interpolate(Color.FromArgb(bezelAlpha, 225, 235, 255), Color.FromArgb(bezelAlpha, cPrimary), 0.55f)
                        : Color.FromArgb(bezelAlpha, cPrimary);

                    float penWidth = Math.Max(1.2f, settings.BezelThickness / 10f);
                    using (Pen bezelPen = new Pen(bezelColor, penWidth))
                    {
                        bezelPen.StartCap = LineCap.Round;
                        bezelPen.EndCap = LineCap.Round;
                        g.DrawPath(bezelPen, curveLine);
                    }
                }

                if (settings.CornerAnim == CornerAnimationMode.SparkleShimmer && settings.CornerBezelGlow)
                {
                    PointF[] pts = GetCornerCurvePoints(size, radius, pos, settings.Curve);
                    for (int s = 0; s < 3; s++)
                    {
                        float sparkWave = (float)Math.Max(0.0, Math.Sin(animPhase * 4.0f + s * 2.1f + (int)pos));
                        if (sparkWave > 0.2f)
                        {
                            int ptIdx = (int)((pts.Length - 1) * (0.2f + 0.3f * s));
                            PointF sp = pts[ptIdx];
                            float sr = 3.5f * sparkWave;
                            int sa = (int)(sparkWave * 240);
                            using (SolidBrush sb = new SolidBrush(Color.FromArgb(sa, Color.White)))
                            {
                                g.FillEllipse(sb, sp.X - sr, sp.Y - sr, sr * 2f, sr * 2f);
                            }
                        }
                    }
                }

                using (SolidBrush blackBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0)))
                {
                    g.FillPath(blackBrush, cornerMask);
                }
            }
            return bmp;
        }

        private static PointF[] GetCornerCurvePoints(int size, int radius, CornerPosition pos, CurveType curveType)
        {
            int steps = 40;
            PointF[] pts = new PointF[steps + 1];
            double n = (curveType == CurveType.SquircleApple) ? 4.2 : 2.0;

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

        private static GraphicsPath CreateCornerCurvePath(int size, int radius, CornerPosition pos, CurveType curveType)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLines(GetCornerCurvePoints(size, radius, pos, curveType));
            return path;
        }

        private static GraphicsPath CreateCornerMaskPath(int size, int radius, CornerPosition pos, CurveType curveType)
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
            {
                if (CornerForms[i] != null)
                {
                    CornerForms[i].Close();
                    CornerForms[i].Dispose();
                }
            }
        }
    }

    // =========================================================================
    // FLOATING QUICK AI PROMPT DIALOG (OPENS RIGHT UNDER THE DYNAMIC ISLAND)
    // =========================================================================
    public class QuickAiPromptDialog : Form
    {
        private TextBox inputBox;
        private Label responseLabel;
        private ComboBox providerCombo;
        private AppSettings settings;
        private AiAssistantEngine aiEngine;
        private CyberCompanionEngine companion;

        public QuickAiPromptDialog(AppSettings s, AiAssistantEngine ai, CyberCompanionEngine comp, Point topCenterScreen)
        {
            settings = s;
            aiEngine = ai;
            companion = comp;

            Text = "🤖 Assistant IA & Compagnon — Dynamic Island";
            Size = new Size(460, 270);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(topCenterScreen.X - 230, topCenterScreen.Y + 195);
            BackColor = Color.FromArgb(16, 18, 28);
            ForeColor = Color.White;
            TopMost = true;
            Font = new Font("Segoe UI", 9.2f, FontStyle.Regular);

            Label topLbl = new Label
            {
                Text = "💬 Discuter avec " + s.CompanionName + " via :",
                Location = new Point(14, 14),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(110, 210, 255)
            };
            Controls.Add(topLbl);

            providerCombo = new ComboBox
            {
                Location = new Point(265, 11),
                Width = 165,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(30, 34, 48),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            providerCombo.Items.AddRange(new string[] { "✨ Google Gemini", "🧠 Anthropic Claude", "🟢 OpenAI ChatGPT", "🌪️ Mistral AI" });
            providerCombo.SelectedIndex = (int)s.ActiveAiProvider;
            providerCombo.SelectedIndexChanged += (sender, e) =>
            {
                settings.ActiveAiProvider = (AiProvider)providerCombo.SelectedIndex;
                settings.Save();
            };
            Controls.Add(providerCombo);

            responseLabel = new Label
            {
                Location = new Point(14, 44),
                Size = new Size(416, 112),
                BackColor = Color.FromArgb(24, 27, 40),
                ForeColor = Color.FromArgb(230, 238, 255),
                Padding = new Padding(10),
                Text = aiEngine.LastResponse,
                Font = new Font("Segoe UI", 9.0f, FontStyle.Regular)
            };
            Controls.Add(responseLabel);

            inputBox = new TextBox
            {
                Location = new Point(14, 168),
                Size = new Size(320, 28),
                BackColor = Color.FromArgb(30, 34, 50),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9.8f)
            };
            inputBox.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    SendCurrentPrompt();
                }
            };
            Controls.Add(inputBox);

            Button sendBtn = new Button
            {
                Text = "Envoyer 🚀",
                Location = new Point(342, 166),
                Size = new Size(88, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 255),
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
            responseLabel.Text = "⏳ " + settings.ActiveAiProvider + " réfléchit...";
            aiEngine.SendPromptAsync(settings, q, reply =>
            {
                if (!IsDisposed && IsHandleCreated)
                {
                    BeginInvoke((Action)(() =>
                    {
                        responseLabel.Text = reply;
                        string shortBubble = reply.Length > 75 ? reply.Substring(0, 72) + "..." : reply;
                        companion.Say(shortBubble, 6500);
                    }));
                }
            });
        }
    }

    // =========================================================================
    // INTERACTIVE DYNAMIC ISLAND HUB (MUSIC + APPLE TIMER + CYBER COMPANION + AI)
    // =========================================================================
    public class DynamicIslandWidget : IDisposable
    {
        private LayeredOverlayForm islandForm;
        public IslandActiveTab CurrentTab = IslandActiveTab.Music;
        public IslandTimerEngine TimerEngine = new IslandTimerEngine();
        public CyberCompanionEngine CompanionEngine = new CyberCompanionEngine();
        public AiAssistantEngine AiEngine = new AiAssistantEngine();

        private float spawnFactor = 1.0f;   // 0 = Hidden, 1 = Spawned onto screen
        private float expandProgress = 0f;  // 0 = Compact Pill, 1 = Expanded Multi-Tab Hub
        private float transitionBoost = 0f;
        private float currentScreenCenterX = -9999f;
        private float currentScreenTopY = 8f;

        private Rectangle currentPillScreenRect = Rectangle.Empty;
        private int hoveredTabIndex = -1;
        private int hoveredActionIndex = -1;
        private bool wasMouseDown = false;
        private string transientBannerText = null;
        private long transientBannerUntilTicks = 0;
        private QuickAiPromptDialog activeAiDialog = null;

        public event Action OnOpenSettingsRequested;
        public event Action OnCycleThemeRequested;
        public event Action OnTimerFinishedBurstRequested;

        public DynamicIslandWidget()
        {
            islandForm = new LayeredOverlayForm(true);
            islandForm.Show();

            TimerEngine.OnTimerFinished += () =>
            {
                ShowTransientBanner("⏰ MINUTEUR TERMINÉ !", 5000);
                CompanionEngine.Say("⏰ Bip bip ! Ton minuteur est terminé !", 5500);
                transitionBoost = 1.0f;
                if (OnTimerFinishedBurstRequested != null) OnTimerFinishedBurstRequested();
            };
        }

        public void TriggerTransitionNotification(string screenName, string res)
        {
            transitionBoost = 1.0f;
            ShowTransientBanner(screenName + "  •  " + res, 2100);
        }

        public void ShowTransientBanner(string text, int durationMs)
        {
            transientBannerText = text;
            transientBannerUntilTicks = DateTime.UtcNow.Ticks + durationMs * 10000L;
        }

        public void UpdateAndRender(
            AppSettings settings,
            Rectangle activeScreenBounds,
            Rectangle primaryScreenBounds,
            Point cursorPos,
            LiveMediaAndAudioTracker media,
            Color[] palette,
            float globalTime)
        {
            TimerEngine.StepTick();
            CompanionEngine.StepTick(globalTime, media.IsAudioActive);

            if (!settings.Enabled || settings.IslandMode == IslandVisibilityMode.Disabled)
            {
                islandForm.Hide();
                return;
            }

            if (transitionBoost > 0f) transitionBoost = Math.Max(0f, transitionBoost - 0.025f);

            Rectangle targetScreen = (settings.IslandPlacement == IslandPlacementMode.PrimaryScreenOnly)
                ? primaryScreenBounds
                : activeScreenBounds;

            // Determine target visibility based on Spawn System (IslandVisibilityMode)
            bool wantVisible = true;
            if (settings.IslandMode == IslandVisibilityMode.OnlyOnTransition)
            {
                wantVisible = (transitionBoost > 0.02f || TimerEngine.FinishedAlertActive);
            }
            else if (settings.IslandMode == IslandVisibilityMode.SmartAutoActivity)
            {
                wantVisible = (media.IsAudioActive || media.ActiveSource != MediaSourceType.None ||
                               TimerEngine.IsRunning || TimerEngine.FinishedAlertActive ||
                               CompanionEngine.HasActiveSpeech || transitionBoost > 0.02f);
            }
            else if (settings.IslandMode == IslandVisibilityMode.HotTopEdgeTrigger)
            {
                bool nearTopCenter = (cursorPos.Y <= targetScreen.Top + 42 &&
                                      Math.Abs(cursorPos.X - (targetScreen.Left + targetScreen.Width / 2)) < 280);
                wantVisible = nearTopCenter || expandProgress > 0.05f || transitionBoost > 0.02f || TimerEngine.FinishedAlertActive;
            }

            // Animate spawnFactor (0..1)
            float targetSpawn = wantVisible ? 1.0f : 0.0f;
            spawnFactor = spawnFactor * 0.80f + targetSpawn * 0.20f;
            if (spawnFactor <= 0.015f && !wantVisible)
            {
                spawnFactor = 0f;
                islandForm.Hide();
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
                currentScreenCenterX = currentScreenCenterX * 0.78f + targetCenterX * 0.22f;
                currentScreenTopY = currentScreenTopY * 0.78f + targetTopY * 0.22f;
            }

            float scale = settings.IslandScalePercent / 100f;

            // Hover detection
            bool isHovering = false;
            if (settings.IslandExpandOnHover && currentPillScreenRect != Rectangle.Empty)
            {
                Rectangle hitBox = currentPillScreenRect;
                hitBox.Inflate(14, 12);
                isHovering = hitBox.Contains(cursorPos);
            }

            float targetExpand = isHovering ? 1.0f : 0.0f;
            expandProgress = expandProgress * 0.78f + targetExpand * 0.22f;
            if (Math.Abs(expandProgress - targetExpand) < 0.005f) expandProgress = targetExpand;

            // Compute Spawn Animation transform (LiquidDropDown, SpringUnfold, WormholePortal, CyberNeonSnap, NovaBloom)
            float spawnScaleX = 1.0f;
            float spawnScaleY = 1.0f;
            float spawnOffsetY = 0f;
            if (spawnFactor < 0.99f)
            {
                switch (settings.SpawnAnim)
                {
                    case IslandSpawnAnimation.LiquidDropDown:
                        spawnOffsetY = -55f * (1f - spawnFactor);
                        spawnScaleX = 0.55f + 0.45f * spawnFactor;
                        spawnScaleY = 1.35f - 0.35f * spawnFactor;
                        break;
                    case IslandSpawnAnimation.SpringUnfold:
                        spawnScaleX = 0.15f + 0.85f * spawnFactor;
                        spawnScaleY = 0.8f + 0.2f * spawnFactor;
                        break;
                    case IslandSpawnAnimation.WormholePortal:
                        spawnScaleX = spawnFactor;
                        spawnScaleY = spawnFactor;
                        break;
                    case IslandSpawnAnimation.CyberNeonSnap:
                        spawnScaleX = 1.4f - 0.4f * spawnFactor;
                        spawnScaleY = Math.Max(0.08f, spawnFactor);
                        break;
                    case IslandSpawnAnimation.NovaBloom:
                        spawnScaleX = 1.25f - 0.25f * spawnFactor;
                        spawnScaleY = 1.25f - 0.25f * spawnFactor;
                        break;
                }
            }

            float compactW = (TimerEngine.IsRunning ? 320f : 295f) * scale;
            float compactH = 38f * scale;
            float expandedW = 475f * scale;
            float expandedH = 204f * scale;

            float rawPillW = (compactW + (expandedW - compactW) * expandProgress) * (1.0f + transitionBoost * 0.06f);
            float rawPillH = (compactH + (expandedH - compactH) * expandProgress) * (1.0f + transitionBoost * 0.05f);

            float curPillW = Math.Max(40f, rawPillW * spawnScaleX);
            float curPillH = Math.Max(14f, rawPillH * spawnScaleY);

            // Extra top/side margin for Companion perched on or floating next to the Island + Speech Bubble
            int padX = (settings.Companion != CompanionSpecies.None) ? (int)(95 * scale) : 28;
            int padTop = (settings.Companion != CompanionSpecies.None && settings.CompanionPos == CompanionPositionMode.PerchedOnIsland)
                ? (int)(52 * scale) : 20;
            int padBottom = 26;

            int winW = (int)curPillW + padX * 2;
            int winH = (int)curPillH + padTop + padBottom;
            int winX = (int)(currentScreenCenterX - winW / 2f);
            int winY = (int)(currentScreenTopY - padTop + spawnOffsetY);

            RectangleF pillRect = new RectangleF(padX, padTop, curPillW, curPillH);
            currentPillScreenRect = new Rectangle(winX + padX - 24, winY + 4, (int)curPillW + 48, (int)curPillH + padTop);

            // Handle mouse hover & clicks inside the Expanded Island OR clicking the Companion!
            Point localPt = new Point(cursorPos.X - winX, cursorPos.Y - winY);
            RectangleF companionRect = GetCompanionRect(pillRect, settings, scale);
            Point companionScreenCenter = new Point(winX + (int)(companionRect.X + companionRect.Width / 2f), winY + (int)(companionRect.Y + companionRect.Height / 2f));

            hoveredTabIndex = -1;
            hoveredActionIndex = -1;
            if (isHovering)
            {
                HandleMouseInteraction(localPt, pillRect, companionRect, scale, settings, media, new Point((int)currentScreenCenterX, (int)currentScreenTopY));
            }
            else
            {
                wasMouseDown = (NativeMethods.GetAsyncKeyState(0x01) & 0x8000) != 0;
            }

            // Render Bitmap
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
                    accent1 = Color.FromArgb(255, 255, 149, 0); // Apple Timer Amber Alert
                    accent2 = Color.FromArgb(255, 255, 59, 48);
                }
                else if (settings.IslandAutoAdaptBrandColor && media.ActiveSource != MediaSourceType.None)
                {
                    accent1 = ThemeColorEngine.GetMediaBrandColor(media.ActiveSource, palette[0]);
                }

                float cornerRad = Math.Min(curPillH / 2f, (19f + 16f * expandProgress) * scale);
                using (GraphicsPath pillPath = ScreenTransitionAnimator.CreateRoundedRectPath(pillRect, cornerRad))
                {
                    // 1. Outer Halo
                    DrawIslandOuterGlow(g, pillRect, pillPath, settings.IslandBorder, accent1, accent2, palette, globalTime, media.CurrentAudioPeak, transitionBoost);

                    // 2. Deep Obsidian Pill Body
                    using (SolidBrush bodyBrush = new SolidBrush(Color.FromArgb(250, 8, 9, 14)))
                    {
                        g.FillPath(bodyBrush, pillPath);
                    }

                    // 3. Rim Highlight
                    using (LinearGradientBrush glassGrad = new LinearGradientBrush(
                        pillRect, Color.FromArgb(190, accent1), Color.FromArgb(145, accent2), (globalTime * 45f) % 360f))
                    using (Pen rimPen = new Pen(glassGrad, 1.6f))
                    {
                        g.DrawPath(rimPen, pillPath);
                    }
                }

                // 4. Compact vs Expanded Content
                if (expandProgress < 0.42f)
                {
                    float cAlpha = (1.0f - expandProgress / 0.42f) * spawnFactor;
                    DrawCompactIslandContent(g, pillRect, scale, cAlpha, media, accent1, accent2, globalTime, settings);
                }
                else
                {
                    float eAlpha = ((expandProgress - 0.42f) / 0.58f) * spawnFactor;
                    DrawExpandedMultiTabHub(g, pillRect, scale, eAlpha, media, accent1, accent2, palette, globalTime, settings, cursorPos, companionScreenCenter);
                }

                // 5. Draw Original Cyber Companion (if perched on or floating next to the Island)
                if (settings.Companion != CompanionSpecies.None && settings.CompanionPos != CompanionPositionMode.InsideIsland)
                {
                    CompanionEngine.DrawCompanion(
                        g, companionRect, settings.Companion, cursorPos, companionScreenCenter,
                        accent1, accent2, globalTime, media.CurrentAudioPeak, settings.CompanionFollowCursorEyes);

                    if (settings.CompanionShowSpeechBubbles && CompanionEngine.HasActiveSpeech)
                    {
                        DrawCompanionSpeechBubble(g, companionRect, pillRect, CompanionEngine.SpeechBubbleText, accent1, scale);
                    }
                }

                if (!islandForm.Visible) islandForm.Show();
                byte masterAlpha = (byte)Math.Max(0, Math.Min(255, (int)(spawnFactor * 255)));
                islandForm.SetBitmap(bmp, winX, winY, masterAlpha);
            }
        }

        private RectangleF GetCompanionRect(RectangleF pillRect, AppSettings settings, float scale)
        {
            float cSize = 44f * (settings.CompanionScalePercent / 100f) * scale;
            if (settings.CompanionPos == CompanionPositionMode.PerchedOnIsland)
            {
                return new RectangleF(pillRect.Right - cSize - 18f * scale, pillRect.Y - cSize * 0.78f, cSize, cSize);
            }
            else if (settings.CompanionPos == CompanionPositionMode.FloatingNextToIsland)
            {
                return new RectangleF(pillRect.Right + 10f * scale, pillRect.Y + (pillRect.Height - cSize) / 2f, cSize, cSize);
            }
            else
            {
                return new RectangleF(pillRect.X + 8f * scale, pillRect.Y + (pillRect.Height - cSize * 0.75f) / 2f, cSize * 0.75f, cSize * 0.75f);
            }
        }

        private void DrawCompanionSpeechBubble(Graphics g, RectangleF compRect, RectangleF pillRect, string text, Color accent, float scale)
        {
            using (Font f = new Font("Segoe UI Semibold", 8.2f * scale, FontStyle.Bold))
            {
                string clean = text.Length > 48 ? text.Substring(0, 46) + "…" : text;
                SizeF sz = g.MeasureString(clean, f);
                float bw = Math.Min(pillRect.Width * 0.85f, sz.Width + 18f * scale);
                float bh = 24f * scale;
                float bx = Math.Max(4f, compRect.X - bw + compRect.Width * 0.3f);
                float by = Math.Max(2f, compRect.Y - 4f);

                RectangleF bRect = new RectangleF(bx, by, bw, bh);
                using (GraphicsPath bp = ScreenTransitionAnimator.CreateRoundedRectPath(bRect, bh / 2f))
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(240, 14, 16, 26)))
                using (Pen border = new Pen(accent, 1.3f))
                using (SolidBrush tb = new SolidBrush(Color.White))
                {
                    g.FillPath(bg, bp);
                    g.DrawPath(border, bp);
                    g.DrawString(clean, f, tb, bx + 9f * scale, by + (bh - sz.Height) / 2f);
                }
            }
        }

        private void DrawCompactIslandContent(
            Graphics g, RectangleF pill, float scale, float alphaF,
            LiveMediaAndAudioTracker media, Color accent1, Color accent2, float globalTime, AppSettings settings)
        {
            int alpha = (int)(Math.Max(0f, Math.Min(1f, alphaF)) * 255);
            if (alpha <= 5) return;

            float leftOffset = pill.X + pill.Height * 0.22f;

            // Left Element: Companion Inside OR Apple Circular Timer Ring OR Spinning Vinyl
            if (settings.Companion != CompanionSpecies.None && settings.CompanionPos == CompanionPositionMode.InsideIsland)
            {
                float cs = pill.Height * 0.82f;
                RectangleF cr = new RectangleF(leftOffset, pill.Y + (pill.Height - cs) / 2f, cs, cs);
                CompanionEngine.DrawCompanion(g, cr, settings.Companion, Cursor.Position, Point.Empty, accent1, accent2, globalTime, media.CurrentAudioPeak, false);
                leftOffset += cs + 6f * scale;
            }
            else if (TimerEngine.IsRunning || TimerEngine.FinishedAlertActive)
            {
                // Apple iOS Dynamic Island Orange Circular Countdown Ring!
                float ringSize = pill.Height * 0.64f;
                float rx = leftOffset;
                float ry = pill.Y + (pill.Height - ringSize) / 2f;
                Color timerOrange = Color.FromArgb(alpha, 255, 159, 10);
                using (Pen bgRing = new Pen(Color.FromArgb(alpha / 3, 255, 159, 10), 2.6f * scale))
                using (Pen fgRing = new Pen(timerOrange, 2.6f * scale))
                {
                    g.DrawEllipse(bgRing, rx, ry, ringSize, ringSize);
                    g.DrawArc(fgRing, rx, ry, ringSize, ringSize, -90f, 360f * TimerEngine.ProgressRatio);
                }
                leftOffset += ringSize + 8f * scale;
            }
            else
            {
                float discSize = pill.Height * 0.64f;
                float discY = pill.Y + (pill.Height - discSize) / 2f;
                using (LinearGradientBrush discBrush = new LinearGradientBrush(
                    new RectangleF(leftOffset, discY, discSize, discSize),
                    Color.FromArgb(alpha, accent1), Color.FromArgb(alpha, accent2), (globalTime * 120f) % 360f))
                {
                    g.FillEllipse(discBrush, leftOffset, discY, discSize, discSize);
                }
                float hole = discSize * 0.32f;
                using (SolidBrush centerHole = new SolidBrush(Color.FromArgb(alpha, 10, 11, 16)))
                {
                    g.FillEllipse(centerHole, leftOffset + (discSize - hole) / 2f, discY + (discSize - hole) / 2f, hole, hole);
                }
                leftOffset += discSize + 8f * scale;
            }

            // Center Text: Transient banner OR Active Timer Countdown OR Music Track OR Clock
            string centerText = media.TrackTitle;
            Color textColor = Color.FromArgb(alpha, 242, 245, 255);

            if (transientBannerText != null && DateTime.UtcNow.Ticks < transientBannerUntilTicks)
            {
                centerText = transientBannerText;
            }
            else if (TimerEngine.IsRunning || TimerEngine.FinishedAlertActive)
            {
                centerText = "⏱ " + TimerEngine.FormattedTime + " • " + TimerEngine.TimerLabel;
                textColor = Color.FromArgb(alpha, 255, 185, 60);
            }
            else if (media.ActiveSource == MediaSourceType.None && !media.IsAudioActive && settings.IslandShowClockWhenIdle)
            {
                centerText = DateTime.Now.ToString("HH:mm") + "  •  " + settings.CompanionName;
            }

            if (centerText.Length > 25) centerText = centerText.Substring(0, 24) + "…";

            using (Font f = new Font("Segoe UI Semibold", 8.8f * scale, FontStyle.Bold))
            using (SolidBrush tb = new SolidBrush(textColor))
            {
                float textY = pill.Y + (pill.Height - 16f * scale) / 2f;
                g.DrawString(centerText, f, tb, leftOffset, textY);
            }

            // Right Live 6-Bar Audio Waveform
            int barCount = 6;
            float barW = 3.0f * scale;
            float barGap = 2.5f * scale;
            float waveStartX = pill.Right - barCount * (barW + barGap) - 12f * scale;
            float maxBarH = pill.Height * 0.56f;

            for (int i = 0; i < barCount; i++)
            {
                float bh = Math.Max(4f * scale, maxBarH * media.SpectrumBars[i * 2]);
                float bx = waveStartX + i * (barW + barGap);
                float by = pill.Y + (pill.Height - bh) / 2f;
                Color barCol = ThemeColorEngine.Interpolate(accent1, accent2, i / (float)(barCount - 1));
                using (SolidBrush bb = new SolidBrush(Color.FromArgb(alpha, barCol)))
                {
                    g.FillRectangle(bb, bx, by, barW, bh);
                }
            }
        }

        private void DrawExpandedMultiTabHub(
            Graphics g, RectangleF pill, float scale, float alphaF,
            LiveMediaAndAudioTracker media, Color accent1, Color accent2, Color[] palette,
            float globalTime, AppSettings settings, Point cursorPos, Point companionCenter)
        {
            int alpha = (int)(Math.Max(0f, Math.Min(1f, alphaF)) * 255);
            if (alpha <= 5) return;

            // 1. Top Navigation Tab Bar inside the Expanded Dynamic Island (4 Tabs + Settings gear)
            RectangleF[] tabRects = GetTopTabRects(pill, scale);
            string[] tabTitles = new string[] { "🎵 Musique", "⏱️ Minuteur", "🦊 Compagnon", "🤖 IA (" + settings.ActiveAiProvider + ")", "⚙️" };

            using (Font tabFont = new Font("Segoe UI Semibold", 8.0f * scale, FontStyle.Bold))
            {
                for (int i = 0; i < tabRects.Length; i++)
                {
                    bool isSelected = (i < 4 && (int)CurrentTab == i);
                    bool isHovered = (hoveredTabIndex == i);
                    RectangleF tr = tabRects[i];

                    using (GraphicsPath tp = ScreenTransitionAnimator.CreateRoundedRectPath(tr, tr.Height / 2f))
                    {
                        Color bg = isSelected
                            ? Color.FromArgb(alpha, accent1)
                            : (isHovered ? Color.FromArgb(alpha, 48, 55, 78) : Color.FromArgb(alpha, 22, 25, 36));
                        using (SolidBrush b = new SolidBrush(bg))
                        {
                            g.FillPath(b, tp);
                        }
                    }

                    SizeF sz = g.MeasureString(tabTitles[i], tabFont);
                    Color fg = isSelected ? Color.FromArgb(alpha, 10, 12, 18) : Color.FromArgb(alpha, 235, 240, 255);
                    using (SolidBrush tb = new SolidBrush(fg))
                    {
                        g.DrawString(tabTitles[i], tabFont, tb, tr.X + (tr.Width - sz.Width) / 2f, tr.Y + (tr.Height - sz.Height) / 2f);
                    }
                }
            }

            // 2. Render Active Tab Body
            if (CurrentTab == IslandActiveTab.Music)
            {
                DrawMusicTabBody(g, pill, scale, alpha, media, accent1, accent2, globalTime);
            }
            else if (CurrentTab == IslandActiveTab.Timer)
            {
                DrawTimerTabBody(g, pill, scale, alpha, accent1, accent2);
            }
            else if (CurrentTab == IslandActiveTab.Companion)
            {
                DrawCompanionTabBody(g, pill, scale, alpha, settings, accent1, accent2, globalTime, media.CurrentAudioPeak, cursorPos, companionCenter);
            }
            else if (CurrentTab == IslandActiveTab.AiAssistant)
            {
                DrawAiAssistantTabBody(g, pill, scale, alpha, settings, accent1, accent2);
            }
        }

        private void DrawMusicTabBody(Graphics g, RectangleF pill, float scale, int alpha, LiveMediaAndAudioTracker media, Color accent1, Color accent2, float globalTime)
        {
            float pad = 16f * scale;
            float bodyTop = pill.Y + 44f * scale;

            // Album Disc
            float artSize = 48f * scale;
            RectangleF artRect = new RectangleF(pill.X + pad, bodyTop, artSize, artSize);
            using (GraphicsPath artPath = ScreenTransitionAnimator.CreateRoundedRectPath(artRect, 12f * scale))
            using (LinearGradientBrush artBrush = new LinearGradientBrush(artRect, Color.FromArgb(alpha, accent1), Color.FromArgb(alpha, accent2), (globalTime * 80f) % 360f))
            {
                g.FillPath(artBrush, artPath);
            }
            using (Font iconFont = new Font("Segoe UI Symbol", 16f * scale, FontStyle.Bold))
            using (SolidBrush wb = new SolidBrush(Color.FromArgb(alpha, Color.White)))
            {
                g.DrawString("♫", iconFont, wb, artRect.X + 9f * scale, artRect.Y + 7f * scale);
            }

            // Track Info
            float textLeft = artRect.Right + 12f * scale;
            using (Font badgeFont = new Font("Segoe UI Semibold", 7.5f * scale, FontStyle.Bold))
            using (Font titleFont = new Font("Segoe UI Semibold", 10.0f * scale, FontStyle.Bold))
            using (Font artistFont = new Font("Segoe UI", 8.4f * scale, FontStyle.Regular))
            using (SolidBrush badgeB = new SolidBrush(Color.FromArgb(alpha, accent1)))
            using (SolidBrush titleB = new SolidBrush(Color.FromArgb(alpha, 250, 252, 255)))
            using (SolidBrush artistB = new SolidBrush(Color.FromArgb((int)(alpha * 0.75f), 180, 190, 215)))
            {
                g.DrawString(GetSourceBadgeText(media.ActiveSource), badgeFont, badgeB, textLeft, bodyTop);
                string tTitle = media.TrackTitle.Length > 30 ? media.TrackTitle.Substring(0, 29) + "…" : media.TrackTitle;
                g.DrawString(tTitle, titleFont, titleB, textLeft, bodyTop + 14f * scale);
                string tArtist = media.TrackArtist.Length > 38 ? media.TrackArtist.Substring(0, 37) + "…" : media.TrackArtist;
                g.DrawString(tArtist, artistFont, artistB, textLeft, bodyTop + 33f * scale);
            }

            // 12-Bar Spectrum on right
            int specCount = 12;
            float sbW = 3.2f * scale;
            float sbGap = 2.4f * scale;
            float specRightX = pill.Right - pad - specCount * (sbW + sbGap);
            float specCenterY = bodyTop + 24f * scale;
            for (int i = 0; i < specCount; i++)
            {
                float bh = Math.Max(4f * scale, 34f * scale * media.SpectrumBars[i]);
                Color bc = ThemeColorEngine.Interpolate(accent1, accent2, i / (float)(specCount - 1));
                using (SolidBrush b = new SolidBrush(Color.FromArgb(alpha, bc)))
                {
                    g.FillRectangle(b, specRightX + i * (sbW + sbGap), specCenterY - bh / 2f, sbW, bh);
                }
            }

            // Action Buttons (Transport + Service Launchers)
            RectangleF[] btns = GetActionButtonsForCurrentTab(pill, scale);
            string[] labels = new string[] { "⏮", "⏯", "⏭", "🔉", "🔊", "🟢 Spotify", "🔴 Apple Music", "▶️ YouTube", "🎨 Thème" };
            DrawActionButtonsGrid(g, btns, labels, alpha, scale, accent1, 1);
        }

        private void DrawTimerTabBody(Graphics g, RectangleF pill, float scale, int alpha, Color accent1, Color accent2)
        {
            float bodyTop = pill.Y + 46f * scale;
            Color appleAmber = Color.FromArgb(alpha, 255, 159, 10);

            // Big Circular iOS Countdown Ring on Left
            float ringSize = 62f * scale;
            float rx = pill.X + 24f * scale;
            float ry = bodyTop + 4f * scale;

            using (Pen bgRing = new Pen(Color.FromArgb(alpha / 4, 255, 159, 10), 5.0f * scale))
            using (Pen fgRing = new Pen(appleAmber, 5.0f * scale))
            {
                fgRing.StartCap = LineCap.Round;
                fgRing.EndCap = LineCap.Round;
                g.DrawEllipse(bgRing, rx, ry, ringSize, ringSize);
                g.DrawArc(fgRing, rx, ry, ringSize, ringSize, -90f, 360f * TimerEngine.ProgressRatio);
            }

            // Big Digital Countdown Display
            using (Font digiFont = new Font("Segoe UI Semibold", 22f * scale, FontStyle.Bold))
            using (Font subFont = new Font("Segoe UI Semibold", 8.8f * scale, FontStyle.Bold))
            using (SolidBrush amberB = new SolidBrush(appleAmber))
            using (SolidBrush whiteB = new SolidBrush(Color.FromArgb(alpha, Color.White)))
            {
                g.DrawString(TimerEngine.FormattedTime, digiFont, whiteB, rx + ringSize + 16f * scale, ry + 2f * scale);
                string status = TimerEngine.IsRunning
                    ? ("🔥 EN COURS • " + TimerEngine.TimerLabel.ToUpper())
                    : ("⏸ EN PAUSE • " + TimerEngine.TimerLabel.ToUpper());
                g.DrawString(status, subFont, amberB, rx + ringSize + 18f * scale, ry + 42f * scale);
            }

            // Timer Action Buttons: [⏯ Start/Pause] [🔄 Reset] [⏱ 1m] [⏱ 5m] [🍅 25m Pomodoro] [➕ +1m] [⏱ Chrono]
            RectangleF[] btns = GetActionButtonsForCurrentTab(pill, scale);
            string[] labels = new string[] {
                TimerEngine.IsRunning ? "⏸ Pause" : "▶️ Démarrer",
                "🔄 Reset",
                "⏱ 1 min",
                "⏱ 5 min",
                "🍅 25m Pomo",
                "➕ +1 min",
                "⏱️ Chrono"
            };
            DrawActionButtonsGrid(g, btns, labels, alpha, scale, appleAmber, 0);
        }

        private void DrawCompanionTabBody(
            Graphics g, RectangleF pill, float scale, int alpha, AppSettings settings,
            Color accent1, Color accent2, float globalTime, float audioPeak, Point cursorPos, Point companionCenter)
        {
            float bodyTop = pill.Y + 46f * scale;

            // Companion Avatar Showcase Box on Left
            RectangleF avatarBox = new RectangleF(pill.X + 20f * scale, bodyTop, 68f * scale, 68f * scale);
            using (GraphicsPath ap = ScreenTransitionAnimator.CreateRoundedRectPath(avatarBox, 16f * scale))
            using (SolidBrush ab = new SolidBrush(Color.FromArgb(alpha, 18, 22, 34)))
            using (Pen border = new Pen(Color.FromArgb(alpha, accent1), 1.5f))
            {
                g.FillPath(ab, ap);
                g.DrawPath(border, ap);
            }

            CompanionSpecies spec = (settings.Companion == CompanionSpecies.None) ? CompanionSpecies.KitsuneCyberFox : settings.Companion;
            CompanionEngine.DrawCompanion(g, avatarBox, spec, cursorPos, companionCenter, accent1, accent2, globalTime, audioPeak, true);

            // Companion Stats & Mood
            float tx = avatarBox.Right + 14f * scale;
            using (Font nameF = new Font("Segoe UI Semibold", 11f * scale, FontStyle.Bold))
            using (Font infoF = new Font("Segoe UI", 8.6f * scale, FontStyle.Regular))
            using (SolidBrush wb = new SolidBrush(Color.FromArgb(alpha, Color.White)))
            using (SolidBrush cb = new SolidBrush(Color.FromArgb(alpha, accent1)))
            {
                g.DrawString(settings.CompanionName + "  •  Niv. " + CompanionEngine.Level + " (" + CompanionEngine.Xp + "/100 XP)", nameF, wb, tx, bodyTop + 2f * scale);
                g.DrawString("Espèce : " + spec + "   |   Humeur : " + CompanionEngine.Mood, infoF, cb, tx, bodyTop + 24f * scale);
                string quote = CompanionEngine.HasActiveSpeech ? CompanionEngine.SpeechBubbleText : "« Clique sur Caresser, Changer d'espèce ou Discuter IA ! »";
                if (quote.Length > 48) quote = quote.Substring(0, 46) + "…";
                g.DrawString(quote, infoF, wb, tx, bodyTop + 44f * scale);
            }

            // Companion Interactive Buttons
            RectangleF[] btns = GetActionButtonsForCurrentTab(pill, scale);
            string[] labels = new string[] {
                "⚡ Booster / Caresser (+XP)",
                "🦊 Changer de Compagnon",
                "📍 Position (" + settings.CompanionPos + ")",
                "🤖 Demander une blague IA"
            };
            DrawActionButtonsGrid(g, btns, labels, alpha, scale, accent1, 0);
        }

        private void DrawAiAssistantTabBody(Graphics g, RectangleF pill, float scale, int alpha, AppSettings settings, Color accent1, Color accent2)
        {
            float bodyTop = pill.Y + 46f * scale;

            // AI Response Box inside Dynamic Island
            RectangleF respBox = new RectangleF(pill.X + 18f * scale, bodyTop, pill.Width - 36f * scale, 68f * scale);
            using (GraphicsPath rp = ScreenTransitionAnimator.CreateRoundedRectPath(respBox, 12f * scale))
            using (SolidBrush rb = new SolidBrush(Color.FromArgb(alpha, 18, 22, 34)))
            using (Pen border = new Pen(Color.FromArgb(alpha / 2, accent1), 1.2f))
            {
                g.FillPath(rb, rp);
                g.DrawPath(border, rp);
            }

            using (Font respF = new Font("Segoe UI", 8.5f * scale, FontStyle.Regular))
            using (SolidBrush tb = new SolidBrush(Color.FromArgb(alpha, 235, 240, 255)))
            {
                RectangleF textArea = RectangleF.Inflate(respBox, -10f * scale, -8f * scale);
                g.DrawString(AiEngine.LastResponse, respF, tb, textArea);
            }

            // AI Action Buttons: [💬 Poser une question] [🔄 Fournisseur: Gemini/Claude/ChatGPT/Mistral] [✨ Conseil Productivité] [🎲 Fun Fact]
            RectangleF[] btns = GetActionButtonsForCurrentTab(pill, scale);
            string[] labels = new string[] {
                "💬 Ouvrir le Chat IA",
                "🔄 API : " + settings.ActiveAiProvider,
                "✨ Idée / Astuce IA",
                "🎲 Blague Cyber"
            };
            DrawActionButtonsGrid(g, btns, labels, alpha, scale, accent1, 0);
        }

        private void DrawActionButtonsGrid(Graphics g, RectangleF[] btns, string[] labels, int alpha, float scale, Color accent, int primaryIndex)
        {
            using (Font f = new Font("Segoe UI Semibold", 8.3f * scale, FontStyle.Bold))
            {
                int count = Math.Min(btns.Length, labels.Length);
                for (int i = 0; i < count; i++)
                {
                    bool hovered = (hoveredActionIndex == i);
                    RectangleF br = btns[i];
                    using (GraphicsPath bp = ScreenTransitionAnimator.CreateRoundedRectPath(br, br.Height / 2f))
                    {
                        Color bg = (i == primaryIndex)
                            ? (hovered ? accent : Color.FromArgb(alpha, 42, 54, 82))
                            : (hovered ? Color.FromArgb(alpha, 55, 64, 94) : Color.FromArgb(alpha, 24, 28, 40));

                        using (SolidBrush bb = new SolidBrush(bg))
                        {
                            g.FillPath(bb, bp);
                        }
                        using (Pen borderP = new Pen(hovered ? Color.FromArgb(alpha, accent) : Color.FromArgb(alpha / 3, 110, 125, 160), 1.2f))
                        {
                            g.DrawPath(borderP, bp);
                        }
                    }

                    SizeF sz = g.MeasureString(labels[i], f);
                    using (SolidBrush fb = new SolidBrush(Color.FromArgb(alpha, Color.White)))
                    {
                        g.DrawString(labels[i], f, fb, br.X + (br.Width - sz.Width) / 2f, br.Y + (br.Height - sz.Height) / 2f);
                    }
                }
            }
        }

        private RectangleF[] GetTopTabRects(RectangleF pill, float scale)
        {
            float y = pill.Y + 10f * scale;
            float h = 25f * scale;
            float gap = 6f * scale;
            float w1 = 88f * scale;
            float w2 = 92f * scale;
            float w3 = 102f * scale;
            float w4 = 112f * scale;
            float w5 = 34f * scale;
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
            if (CurrentTab == IslandActiveTab.Music)
            {
                RectangleF[] r = new RectangleF[9];
                float row1Y = pill.Y + 104f * scale;
                float btnH = 32f * scale;
                float smallW = 48f * scale;
                float playW = 68f * scale;
                float gap = 10f * scale;
                float total1 = smallW * 4 + playW + gap * 4;
                float sx = pill.X + (pill.Width - total1) / 2f;

                r[0] = new RectangleF(sx, row1Y, smallW, btnH); sx += smallW + gap;
                r[1] = new RectangleF(sx, row1Y, playW, btnH); sx += playW + gap;
                r[2] = new RectangleF(sx, row1Y, smallW, btnH); sx += smallW + gap;
                r[3] = new RectangleF(sx, row1Y, smallW, btnH); sx += smallW + gap;
                r[4] = new RectangleF(sx, row1Y, smallW, btnH);

                float row2Y = pill.Y + 148f * scale;
                float chipH = 26f * scale;
                float cGap = 8f * scale;
                float cw = 98f * scale;
                float total2 = cw * 4 + cGap * 3;
                float cx = pill.X + (pill.Width - total2) / 2f;
                for (int i = 0; i < 4; i++)
                {
                    r[5 + i] = new RectangleF(cx, row2Y, cw, chipH);
                    cx += cw + cGap;
                }
                return r;
            }
            else if (CurrentTab == IslandActiveTab.Timer)
            {
                RectangleF[] r = new RectangleF[7];
                float row1Y = pill.Y + 118f * scale;
                float row2Y = pill.Y + 154f * scale;
                float h = 28f * scale;
                float gap = 8f * scale;

                // Row 1: Start/Pause, Reset, +1 min, Chrono
                float w = 100f * scale;
                float sx = pill.X + (pill.Width - (w * 4 + gap * 3)) / 2f;
                r[0] = new RectangleF(sx, row1Y, w, h); sx += w + gap;
                r[1] = new RectangleF(sx, row1Y, w, h); sx += w + gap;
                r[5] = new RectangleF(sx, row1Y, w, h); sx += w + gap;
                r[6] = new RectangleF(sx, row1Y, w, h);

                // Row 2: 1 min, 5 min, 25m Pomodoro
                float w2 = 135f * scale;
                float sx2 = pill.X + (pill.Width - (w2 * 3 + gap * 2)) / 2f;
                r[2] = new RectangleF(sx2, row2Y, w2, h); sx2 += w2 + gap;
                r[3] = new RectangleF(sx2, row2Y, w2, h); sx2 += w2 + gap;
                r[4] = new RectangleF(sx2, row2Y, w2, h);
                return r;
            }
            else
            {
                // Companion & AI tabs: 2x2 grid of wide action buttons
                RectangleF[] r = new RectangleF[4];
                float row1Y = pill.Y + 120f * scale;
                float row2Y = pill.Y + 156f * scale;
                float w = 205f * scale;
                float h = 29f * scale;
                float gap = 12f * scale;
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

            // Check click on the Companion directly!
            if (settings.Companion != CompanionSpecies.None && companionRect.Contains(localPt.X, localPt.Y))
            {
                if (clicked) CompanionEngine.InteractPetOrBoost();
                return;
            }

            if (expandProgress < 0.6f) return;

            // Check Top Tabs
            RectangleF[] tabs = GetTopTabRects(pill, scale);
            for (int i = 0; i < tabs.Length; i++)
            {
                if (tabs[i].Contains(localPt.X, localPt.Y))
                {
                    hoveredTabIndex = i;
                    if (clicked)
                    {
                        if (i < 4) CurrentTab = (IslandActiveTab)i;
                        else if (OnOpenSettingsRequested != null) OnOpenSettingsRequested();
                    }
                    return;
                }
            }

            // Check Active Tab Action Buttons
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
            if (CurrentTab == IslandActiveTab.Music)
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
            else if (CurrentTab == IslandActiveTab.Timer)
            {
                switch (idx)
                {
                    case 0: TimerEngine.ToggleStartPause(); break;
                    case 1: TimerEngine.Reset(); break;
                    case 2: TimerEngine.StartCountdown(60, "1 Minute"); CompanionEngine.Say("⏱ Minuteur de 1 minute lancé !"); break;
                    case 3: TimerEngine.StartCountdown(300, "5 Minutes"); CompanionEngine.Say("⏱ Minuteur de 5 minutes lancé !"); break;
                    case 4: TimerEngine.StartCountdown(1500, "Pomodoro 25m"); CompanionEngine.Say("🍅 Session Pomodoro 25 min démarrée !"); break;
                    case 5: TimerEngine.AddSeconds(60); break;
                    case 6: TimerEngine.SwitchToStopwatch(); break;
                }
            }
            else if (CurrentTab == IslandActiveTab.Companion)
            {
                switch (idx)
                {
                    case 0:
                        CompanionEngine.InteractPetOrBoost();
                        break;
                    case 1:
                        int nextSpec = ((int)settings.Companion % 6) + 1;
                        settings.Companion = (CompanionSpecies)nextSpec;
                        settings.Save();
                        CompanionEngine.Say("✨ Transformation en " + settings.Companion + " !");
                        break;
                    case 2:
                        settings.CompanionPos = (CompanionPositionMode)(((int)settings.CompanionPos + 1) % 3);
                        settings.Save();
                        break;
                    case 3:
                        CurrentTab = IslandActiveTab.AiAssistant;
                        AiEngine.SendPromptAsync(settings, "Raconte-moi une blague courte de geek ou donne-moi une punchline stylée !", reply => CompanionEngine.Say(reply, 6000));
                        break;
                }
            }
            else if (CurrentTab == IslandActiveTab.AiAssistant)
            {
                switch (idx)
                {
                    case 0:
                        OpenQuickAiDialog(settings, screenTopCenter);
                        break;
                    case 1:
                        settings.ActiveAiProvider = (AiProvider)(((int)settings.ActiveAiProvider + 1) % 4);
                        settings.Save();
                        break;
                    case 2:
                        AiEngine.SendPromptAsync(settings, "Donne-moi un conseil ultra court et motivant de productivité ou de focus pour ma journée !", reply => CompanionEngine.Say(reply, 6000));
                        break;
                    case 3:
                        AiEngine.SendPromptAsync(settings, "Donne-moi un fait insolite et fascinant sur la tech ou l'espace en 2 phrases max !", reply => CompanionEngine.Say(reply, 6000));
                        break;
                }
            }
        }

        public void OpenQuickAiDialog(AppSettings settings, Point screenTopCenter)
        {
            if (activeAiDialog == null || activeAiDialog.IsDisposed)
            {
                activeAiDialog = new QuickAiPromptDialog(settings, AiEngine, CompanionEngine, screenTopCenter);
            }
            activeAiDialog.Show();
            activeAiDialog.BringToFront();
            activeAiDialog.Activate();
        }

        private void DrawIslandOuterGlow(
            Graphics g, RectangleF pillRect, GraphicsPath pillPath,
            IslandBorderAnimation borderMode, Color accent1, Color accent2, Color[] palette,
            float globalTime, float audioPeak, float transBoost)
        {
            if (borderMode == IslandBorderAnimation.MinimalClean && transBoost <= 0.02f) return;

            float glowIntensity = 0.55f + transBoost * 0.45f;
            if (borderMode == IslandBorderAnimation.AudioReactiveHalo)
                glowIntensity = 0.25f + audioPeak * 1.15f + transBoost * 0.4f;
            else if (borderMode == IslandBorderAnimation.BreathingGlass)
                glowIntensity = 0.25f + 0.35f * (float)(0.5 + 0.5 * Math.Sin(globalTime * 2.2f));

            Color cA = accent1;
            Color cB = accent2;
            if (borderMode == IslandBorderAnimation.SiriRotatingGlow && palette.Length >= 3)
            {
                int i1 = ((int)(globalTime * 1.5f)) % palette.Length;
                int i2 = (i1 + 1) % palette.Length;
                float f = (globalTime * 1.5f) - (int)(globalTime * 1.5f);
                cA = ThemeColorEngine.Interpolate(palette[i1], palette[i2], f);
                cB = palette[(i2 + 1) % palette.Length];
            }

            for (int layer = 4; layer >= 1; layer--)
            {
                int alpha = (int)(Math.Min(1.2f, glowIntensity) * (105 / layer));
                if (alpha > 255) alpha = 255;
                if (alpha <= 3) continue;

                using (LinearGradientBrush gb = new LinearGradientBrush(
                    RectangleF.Inflate(pillRect, 12, 12), Color.FromArgb(alpha, cA), Color.FromArgb(alpha, cB), (globalTime * 65f) % 360f))
                using (Pen glowPen = new Pen(gb, layer * 3.0f))
                {
                    g.DrawPath(glowPen, pillPath);
                }
            }
        }

        private static string GetSourceBadgeText(MediaSourceType src)
        {
            switch (src)
            {
                case MediaSourceType.Spotify: return "🟢 SPOTIFY MUSIC";
                case MediaSourceType.AppleMusic: return "🔴 APPLE MUSIC";
                case MediaSourceType.YouTube: return "▶️ YOUTUBE MUSIC";
                case MediaSourceType.BrowserMedia: return "🌐 LECTEUR WEB";
                case MediaSourceType.SystemAudio: return "🔊 AUDIO SYSTÈME";
                default: return "🎵 DYNAMIC ISLAND PLAYER";
            }
        }

        public void EnsureTopMost()
        {
            if (islandForm != null) islandForm.EnsureTopMost();
        }

        public void Dispose()
        {
            if (activeAiDialog != null && !activeAiDialog.IsDisposed) activeAiDialog.Close();
            if (islandForm != null)
            {
                islandForm.Close();
                islandForm.Dispose();
            }
        }
    }

    // =========================================================================
    // MULTI-SCREEN TRANSITION ANIMATOR (8 TRANSITION STYLES)
    // =========================================================================
    public enum ScreenEdge
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public class ScreenTransitionAnimator
    {
        private LayeredOverlayForm entryEdgeForm;
        private LayeredOverlayForm exitEdgeForm;

        private bool isAnimating = false;
        private bool isProximityActive = false;
        private long animStartTicks = 0;
        private int animDurationMs = 1000;

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

        public bool IsBusy
        {
            get { return isAnimating || isProximityActive; }
        }

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
                if (isProximityActive)
                {
                    entryEdgeForm.Hide();
                    isProximityActive = false;
                }
                return;
            }

            isProximityActive = true;
            float scale = settings.IntensityPercent / 100f;
            RenderEdgePortal(entryEdgeForm, currentBounds, nearEdge, cursorPos, proximityFactor * 0.42f, proximityFactor * 0.78f, scale, settings.Style, palette, true, 0f);
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

            if (settings.Style != TransitionStyle.DynamicIslandBounce)
            {
                float entryEnvelope = (t < 0.22f) ? EaseOutBack(t / 0.22f) : 1.0f - EaseInOutCubic((t - 0.22f) / 0.78f);
                float entryAlpha = (t < 0.14f) ? (t / 0.14f) : (1.0f - (t - 0.14f) / 0.86f);

                RenderEdgePortal(entryEdgeForm, entryScreenBounds, entryEdge, crossingPoint, entryEnvelope, Math.Max(0f, Math.Min(1f, entryAlpha)), scale, settings.Style, palette, false, t);

                if (t < 0.46f && exitScreenBounds != entryScreenBounds)
                {
                    float exitT = t / 0.46f;
                    RenderEdgePortal(exitEdgeForm, exitScreenBounds, exitEdge, crossingPoint, (1.0f - EaseOutCubic(exitT)) * 0.65f, (1.0f - exitT) * 0.85f, scale, settings.Style, palette, true, t);
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

        private void RenderEdgePortal(LayeredOverlayForm form, Rectangle screenBounds, ScreenEdge edge, Point cursor, float bulgeAmount, float alphaFactor, float scale, TransitionStyle style, Color[] palette, bool isExitSide, float rawTime)
        {
            if (alphaFactor <= 0.01f || bulgeAmount <= 0.005f)
            {
                form.Hide();
                return;
            }

            bool verticalEdge = (edge == ScreenEdge.Left || edge == ScreenEdge.Right);
            int portalLength = (int)(Math.Min(verticalEdge ? screenBounds.Height : screenBounds.Width, (int)(740 * scale)));
            int portalDepth = (int)(210 * scale);
            if (portalLength < 140) portalLength = 140;
            if (portalDepth < 70) portalDepth = 70;

            int winW = verticalEdge ? portalDepth : portalLength;
            int winH = verticalEdge ? portalLength : portalDepth;

            int winX = screenBounds.Left;
            int winY = screenBounds.Top;

            if (edge == ScreenEdge.Left)
            {
                winX = screenBounds.Left;
                winY = Clamp(cursor.Y - winH / 2, screenBounds.Top, screenBounds.Bottom - winH);
            }
            else if (edge == ScreenEdge.Right)
            {
                winX = screenBounds.Right - winW;
                winY = Clamp(cursor.Y - winH / 2, screenBounds.Top, screenBounds.Bottom - winH);
            }
            else if (edge == ScreenEdge.Top)
            {
                winX = Clamp(cursor.X - winW / 2, screenBounds.Left, screenBounds.Right - winW);
                winY = screenBounds.Top;
            }
            else
            {
                winX = Clamp(cursor.X - winW / 2, screenBounds.Left, screenBounds.Right - winW);
                winY = screenBounds.Bottom - winH;
            }

            using (Bitmap bmp = new Bitmap(winW, winH, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.Clear(Color.Transparent);

                float centerAlong = verticalEdge
                    ? Clamp(cursor.Y - winY, (int)(portalLength * 0.2f), (int)(portalLength * 0.8f))
                    : Clamp(cursor.X - winX, (int)(portalLength * 0.2f), (int)(portalLength * 0.8f));

                if (style == TransitionStyle.ComboUltimate || style == TransitionStyle.SiriIntelligenceGlow || style == TransitionStyle.LiquidBridge || style == TransitionStyle.ElectricPlasmaPortal || style == TransitionStyle.CosmicParticleBurst)
                    DrawChromaticEdgeWave(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor, scale, palette, rawTime);

                if (style == TransitionStyle.ComboUltimate || style == TransitionStyle.LiquidBridge)
                    DrawLiquidBridgeMetaball(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor, scale, palette, isExitSide, rawTime);

                if (!isExitSide && (style == TransitionStyle.ComboUltimate || style == TransitionStyle.CosmicParticleBurst))
                    DrawCosmicParticles(g, winW, winH, edge, centerAlong, alphaFactor, scale, palette, rawTime);

                if (style == TransitionStyle.ElectricPlasmaPortal)
                    DrawPlasmaLightning(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor, scale, palette, rawTime);

                if (style == TransitionStyle.SonarShockwave)
                    DrawSonarShockwave(g, winW, winH, edge, centerAlong, alphaFactor, scale, palette, rawTime);

                if (style == TransitionStyle.NeonLaserBlade)
                    DrawNeonLaserBlade(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor, scale, palette);

                if (!form.Visible) form.Show();
                form.SetBitmap(bmp, winX, winY, 255);
            }
        }

        private void DrawChromaticEdgeWave(Graphics g, int w, int h, ScreenEdge edge, float centerAlong, float bulge, float alpha, float scale, Color[] palette, float rawTime)
        {
            bool vertical = (edge == ScreenEdge.Left || edge == ScreenEdge.Right);
            float length = vertical ? h : w;
            float maxReach = (vertical ? w : h) * 0.78f * Math.Min(1.3f, bulge * 1.15f);
            if (maxReach < 6f) return;

            for (int i = 0; i < 4; i++)
            {
                Color c = palette[i % palette.Length];
                float offsetAlong = (float)Math.Sin(rawTime * 9.0 + i * 1.6) * (46f * scale);
                float segHalfLen = (length * (0.43f - i * 0.06f)) * (0.6f + 0.4f * bulge);
                float reach = maxReach * (1.0f - i * 0.16f);
                int layerAlpha = (int)(alpha * (165 - i * 30));
                if (layerAlpha <= 2) continue;
                if (layerAlpha > 255) layerAlpha = 255;

                using (GraphicsPath wavePath = BuildSmoothEdgeBulgePath(w, h, edge, centerAlong + offsetAlong, segHalfLen, reach))
                using (PathGradientBrush pgb = new PathGradientBrush(wavePath))
                {
                    pgb.CenterColor = Color.FromArgb(layerAlpha, c);
                    pgb.SurroundColors = new Color[] { Color.FromArgb(0, c) };
                    g.FillPath(pgb, wavePath);
                }
            }
        }

        private void DrawLiquidBridgeMetaball(Graphics g, int w, int h, ScreenEdge edge, float centerAlong, float bulge, float alpha, float scale, Color[] palette, bool isExitSide, float rawTime)
        {
            float maxDepth = (isExitSide ? 38f : 62f) * scale * bulge;
            float halfSpan = (isExitSide ? 88f : 122f) * scale * (0.55f + 0.45f * bulge);
            if (maxDepth < 3f) return;

            using (GraphicsPath liquidPath = BuildSmoothEdgeBulgePath(w, h, edge, centerAlong, halfSpan, maxDepth))
            {
                for (int glow = 3; glow >= 1; glow--)
                {
                    int rimAlpha = (int)(alpha * (205 / glow));
                    if (rimAlpha > 255) rimAlpha = 255;
                    Color rimColor = ThemeColorEngine.Interpolate(palette[0], palette[1 % palette.Length], (float)(0.5 + 0.5 * Math.Sin(rawTime * 10.0)));
                    using (Pen rimPen = new Pen(Color.FromArgb(rimAlpha, rimColor), glow * 3.0f))
                    {
                        g.DrawPath(rimPen, liquidPath);
                    }
                }
                int bodyAlpha = (int)(Math.Min(1.0f, alpha * 1.35f) * 255);
                using (SolidBrush blackBrush = new SolidBrush(Color.FromArgb(bodyAlpha, 5, 6, 10)))
                {
                    g.FillPath(blackBrush, liquidPath);
                }
            }
        }

        private void DrawCosmicParticles(Graphics g, int w, int h, ScreenEdge edge, float centerAlong, float alpha, float scale, Color[] palette, float rawTime)
        {
            for (int i = 0; i < 16; i++)
            {
                float seed = (i * 1.37f) + 0.5f;
                float pTime = rawTime * (0.65f + (i % 5) * 0.22f);
                if (pTime > 1f) continue;

                float dist = (15f + 145f * EaseOutCubic(pTime)) * scale * (0.5f + 0.5f * (float)Math.Sin(seed * 3.1f));
                float spread = (float)Math.Sin(seed * 7.3f) * (170f * scale) * (0.3f + 0.7f * pTime);
                float pRadius = (6.5f - 4.5f * pTime) * scale;
                if (pRadius < 1f) continue;

                PointF pt = GetPointFromEdge(w, h, edge, centerAlong + spread, dist);
                Color c = palette[i % palette.Length];
                int pAlpha = (int)(alpha * (1.0f - pTime) * 245);
                if (pAlpha <= 4) continue;

                using (SolidBrush glowB = new SolidBrush(Color.FromArgb(pAlpha / 2, c)))
                    g.FillEllipse(glowB, pt.X - pRadius * 1.8f, pt.Y - pRadius * 1.8f, pRadius * 3.6f, pRadius * 3.6f);
                using (SolidBrush coreB = new SolidBrush(Color.FromArgb(pAlpha, Color.White)))
                    g.FillEllipse(coreB, pt.X - pRadius * 0.7f, pt.Y - pRadius * 0.7f, pRadius * 1.4f, pRadius * 1.4f);
            }
        }

        private void DrawPlasmaLightning(Graphics g, int w, int h, ScreenEdge edge, float centerAlong, float bulge, float alpha, float scale, Color[] palette, float rawTime)
        {
            for (int bolt = 0; bolt < 3; bolt++)
            {
                PointF[] pts = new PointF[9];
                for (int s = 0; s < pts.Length; s++)
                {
                    float frac = (s / (float)(pts.Length - 1)) - 0.5f;
                    float along = centerAlong + frac * 260f * scale;
                    float jag = (s == 0 || s == pts.Length - 1) ? 2f : (float)Math.Sin(rawTime * 45.0 + s * 2.7 + bolt * 1.9) * 35f * scale * bulge + 28f * scale * bulge;
                    pts[s] = GetPointFromEdge(w, h, edge, along, Math.Max(2f, jag));
                }
                Color c = palette[bolt % palette.Length];
                int a = (int)(alpha * 220);
                using (Pen glowP = new Pen(Color.FromArgb(a / 2, c), 6f * scale))
                using (Pen coreP = new Pen(Color.FromArgb(a, Color.White), 2f * scale))
                {
                    g.DrawLines(glowP, pts);
                    g.DrawLines(coreP, pts);
                }
            }
        }

        private void DrawSonarShockwave(Graphics g, int w, int h, ScreenEdge edge, float centerAlong, float alpha, float scale, Color[] palette, float rawTime)
        {
            PointF origin = GetPointFromEdge(w, h, edge, centerAlong, 0f);
            for (int ring = 0; ring < 3; ring++)
            {
                float rt = rawTime * 1.3f - ring * 0.18f;
                if (rt <= 0f || rt >= 1f) continue;
                float rad = rt * 175f * scale;
                int rAlpha = (int)((1f - rt) * alpha * 230);
                using (Pen rp = new Pen(Color.FromArgb(rAlpha, palette[ring % palette.Length]), (4.5f - 3f * rt) * scale))
                {
                    g.DrawEllipse(rp, origin.X - rad, origin.Y - rad, rad * 2f, rad * 2f);
                }
            }
        }

        private void DrawNeonLaserBlade(Graphics g, int w, int h, ScreenEdge edge, float centerAlong, float bulge, float alpha, float scale, Color[] palette)
        {
            float halfLen = 240f * scale * (0.4f + 0.6f * bulge);
            PointF p1 = GetPointFromEdge(w, h, edge, centerAlong - halfLen, 3f);
            PointF p2 = GetPointFromEdge(w, h, edge, centerAlong + halfLen, 3f);

            for (int layer = 5; layer >= 1; layer--)
            {
                int a = (int)(alpha * (220 / layer));
                using (Pen p = new Pen(Color.FromArgb(a, layer == 1 ? Color.White : palette[0]), layer * 4.5f * scale))
                {
                    p.StartCap = LineCap.Round;
                    p.EndCap = LineCap.Round;
                    g.DrawLine(p, p1, p2);
                }
            }
        }

        private static GraphicsPath BuildSmoothEdgeBulgePath(int w, int h, ScreenEdge edge, float centerAlong, float halfSpan, float depth)
        {
            GraphicsPath path = new GraphicsPath();
            float start = centerAlong - halfSpan;
            float end = centerAlong + halfSpan;

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

        public static GraphicsPath CreateRoundedRectPath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float d = radius * 2f;
            if (d > rect.Width) d = rect.Width;
            if (d > rect.Height) d = rect.Height;
            if (d <= 1f) { path.AddRectangle(rect); return path; }
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static int Clamp(float val, int min, int max)
        {
            if (max < min) return min;
            if (val < min) return min;
            if (val > max) return max;
            return (int)val;
        }

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
    // MAIN OVERLAY CONTROLLER
    // =========================================================================
    public class OverlayController : IDisposable
    {
        public AppSettings Settings;
        public List<MonitorCorners> Monitors = new List<MonitorCorners>();
        public ScreenTransitionAnimator Animator;
        public DynamicIslandWidget DynamicIsland;
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
            DynamicIsland = new DynamicIslandWidget();
            MediaTracker = new LiveMediaAndAudioTracker();

            DynamicIsland.OnOpenSettingsRequested += ShowSettingsWindow;
            DynamicIsland.OnTimerFinishedBurstRequested += TriggerDemoTransition;
            DynamicIsland.OnCycleThemeRequested += () =>
            {
                int next = ((int)Settings.Theme + 1) % 12;
                Settings.Theme = (ColorTheme)next;
                RefreshAllCorners();
                DynamicIsland.ShowTransientBanner("🎨 Thème : " + Settings.Theme.ToString(), 1500);
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

        public float GlobalTimeSeconds
        {
            get { return (float)((DateTime.UtcNow.Ticks - startTicks) / 10000000.0); }
        }

        public void RebuildMonitors()
        {
            foreach (var m in Monitors) m.Dispose();
            Monitors.Clear();

            Screen[] screens = Screen.AllScreens;
            Color[] palette = ThemeColorEngine.GetPalette(Settings, GlobalTimeSeconds);
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
            Color[] palette = ThemeColorEngine.GetPalette(Settings, t);
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
            Color[] palette = ThemeColorEngine.GetPalette(Settings, t);

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
            bool needsContinuousCornerUpdate = (Settings.CornerAnim != CornerAnimationMode.Static || Settings.Theme == ColorTheme.CyberpunkRainbow);
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
            int threshold = 70;
            float bestProx = 0f;
            ScreenEdge bestEdge = ScreenEdge.Left;

            for (int otherIdx = 0; otherIdx < Monitors.Count; otherIdx++)
            {
                if (otherIdx == currentIdx) continue;
                Rectangle other = Monitors[otherIdx].Bounds;

                if (Math.Abs(other.Right - cur.Left) <= 160 && pt.Y >= other.Top - 80 && pt.Y <= other.Bottom + 80)
                {
                    int dist = pt.X - cur.Left;
                    if (dist >= 0 && dist < threshold)
                    {
                        float p = 1f - (float)dist / threshold;
                        if (p > bestProx) { bestProx = p; bestEdge = ScreenEdge.Left; }
                    }
                }
                if (Math.Abs(other.Left - cur.Right) <= 160 && pt.Y >= other.Top - 80 && pt.Y <= other.Bottom + 80)
                {
                    int dist = cur.Right - pt.X;
                    if (dist >= 0 && dist < threshold)
                    {
                        float p = 1f - (float)dist / threshold;
                        if (p > bestProx) { bestProx = p; bestEdge = ScreenEdge.Right; }
                    }
                }
                if (Math.Abs(other.Bottom - cur.Top) <= 160 && pt.X >= other.Left - 80 && pt.X <= other.Right + 80)
                {
                    int dist = pt.Y - cur.Top;
                    if (dist >= 0 && dist < threshold)
                    {
                        float p = 1f - (float)dist / threshold;
                        if (p > bestProx) { bestProx = p; bestEdge = ScreenEdge.Top; }
                    }
                }
                if (Math.Abs(other.Top - cur.Bottom) <= 160 && pt.X >= other.Left - 80 && pt.X <= other.Right + 80)
                {
                    int dist = cur.Bottom - pt.Y;
                    if (dist >= 0 && dist < threshold)
                    {
                        float p = 1f - (float)dist / threshold;
                        if (p > bestProx) { bestProx = p; bestEdge = ScreenEdge.Bottom; }
                    }
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
            trayIcon.Text = "iCorner Duo — Bords, Compagnon & IA";
            trayIcon.Icon = LoadAppIcon() ?? SystemIcons.Application;
            trayIcon.Visible = true;

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem openItem = new ToolStripMenuItem("⚙ Ouvrir iCorner Duo Studio");
            openItem.Font = new Font(openItem.Font, FontStyle.Bold);
            openItem.Click += (s, e) => ShowSettingsWindow();

            ToolStripMenuItem demoItem = new ToolStripMenuItem("✨ Tester l'animation de transition");
            demoItem.Click += (s, e) => TriggerDemoTransition();

            ToolStripMenuItem timerItem = new ToolStripMenuItem("🍅 Lancer un Pomodoro (25 min)");
            timerItem.Click += (s, e) => DynamicIsland.TimerEngine.StartCountdown(1500, "Pomodoro 25m");

            ToolStripMenuItem aiItem = new ToolStripMenuItem("🤖 Ouvrir le Chat IA (" + Settings.ActiveAiProvider + ")");
            aiItem.Click += (s, e) => DynamicIsland.OpenQuickAiDialog(Settings, new Point(Screen.PrimaryScreen.Bounds.Width / 2, 10));

            ToolStripMenuItem playPauseItem = new ToolStripMenuItem("⏯ Lecture / Pause Musique");
            playPauseItem.Click += (s, e) => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PLAY_PAUSE);

            ToolStripMenuItem exitItem = new ToolStripMenuItem("❌ Quitter iCorner Duo");
            exitItem.Click += (s, e) =>
            {
                trayIcon.Visible = false;
                Application.Exit();
            };

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
    // REDESIGNED SIDEBAR STUDIO SETTINGS INTERFACE (5 DEDICATED PAGES)
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
            Text = "iCorner Duo Studio Ultimate — Bords Arrondis, Transitions, Dynamic Island, Compagnon & IA";
            Size = new Size(1060, 760);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(12, 13, 19);
            ForeColor = Color.FromArgb(240, 242, 250);
            Font = new Font("Segoe UI", 9.3f, FontStyle.Regular);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            Icon appIco = OverlayController.LoadAppIcon();
            if (appIco != null) Icon = appIco;

            BuildModernSidebarLayout();

            uiPreviewTimer = new System.Windows.Forms.Timer { Interval = 50 };
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

        private void BuildModernSidebarLayout()
        {
            // 1. TOP STUDIO HEADER
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 78,
                BackColor = Color.FromArgb(18, 20, 30)
            };
            header.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (LinearGradientBrush grad = new LinearGradientBrush(
                    header.ClientRectangle, Color.FromArgb(55, 64, 200, 255), Color.FromArgb(55, 220, 65, 185), 12f))
                {
                    e.Graphics.FillRectangle(grad, header.ClientRectangle);
                }
                using (Pen bottomLine = new Pen(Color.FromArgb(70, 110, 160, 255), 1f))
                {
                    e.Graphics.DrawLine(bottomLine, 0, header.Height - 1, header.Width, header.Height - 1);
                }
                if (Icon != null) e.Graphics.DrawIcon(Icon, new Rectangle(18, 13, 50, 50));
            };

            Label titleLbl = new Label
            {
                Text = "iCorner Duo Studio  •  Ultimate Edition",
                Font = new Font("Segoe UI Semibold", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(80, 12)
            };
            Label subLbl = new Label
            {
                Text = "Bords arrondis animés • Transitions Multi-Écrans • Dynamic Island Musique & Minuteur • Compagnon Cyber • API IA (Gemini, Claude, ChatGPT, Mistral)",
                Font = new Font("Segoe UI", 8.8f),
                ForeColor = Color.FromArgb(185, 200, 230),
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(82, 44)
            };

            Button testTransBtn = CreateActionBtn("✨ Tester Transition", 855, 19, 175, 40, Color.FromArgb(0, 122, 255), () => controller.TriggerDemoTransition());
            header.Controls.Add(titleLbl);
            header.Controls.Add(subLbl);
            header.Controls.Add(testTransBtn);
            Controls.Add(header);

            // 2. LEFT NAVIGATION SIDEBAR
            Panel sidebar = new Panel
            {
                Location = new Point(0, 78),
                Size = new Size(245, 650),
                BackColor = Color.FromArgb(16, 18, 26)
            };
            sidebar.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Color.FromArgb(45, 100, 120, 180), 1f))
                {
                    e.Graphics.DrawLine(p, sidebar.Width - 1, 0, sidebar.Width - 1, sidebar.Height);
                }
            };
            Controls.Add(sidebar);

            string[] navLabels = new string[] {
                "📐  1. Bords Arrondis & Angles",
                "🌊  2. Transitions & 13 Couleurs",
                "💊  3. Dynamic Island & Minuteur",
                "🦊  4. Compagnon Cyber (Original)",
                "🤖  5. Assistant IA (4 API)"
            };

            for (int i = 0; i < 5; i++)
            {
                int idx = i;
                Button nb = new Button
                {
                    Text = navLabels[i],
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(12, 0, 0, 0),
                    Location = new Point(12, 18 + i * 52),
                    Size = new Size(220, 44),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                nb.FlatAppearance.BorderSize = 0;
                nb.Click += (s, e) => SwitchPage(idx);
                navButtons[i] = nb;
                sidebar.Controls.Add(nb);
            }

            // Quick status & Startup checkbox at bottom of sidebar
            CheckBox startupChk = CreateCheckBox("Lancer avec Windows", 16, 520, IsStartupEnabled());
            startupChk.CheckedChanged += (s, e) => SetStartup(startupChk.Checked);
            sidebar.Controls.Add(startupChk);

            Button hideTrayBtn = CreateActionBtn("📥 Réduire en barre système", 12, 558, 220, 38, Color.FromArgb(36, 42, 62), () => Hide());
            sidebar.Controls.Add(hideTrayBtn);

            // 3. BUILD THE 5 CONTENT PAGES
            pages[0] = BuildPage1Corners();
            pages[1] = BuildPage2TransitionsAndColors();
            pages[2] = BuildPage3DynamicIslandAndTimer();
            pages[3] = BuildPage4Companion();
            pages[4] = BuildPage5AiAssistant();

            for (int i = 0; i < 5; i++) Controls.Add(pages[i]);
            SwitchPage(0);
        }

        private void SwitchPage(int idx)
        {
            activePageIndex = idx;
            for (int i = 0; i < 5; i++)
            {
                pages[i].Visible = (i == idx);
                navButtons[i].BackColor = (i == idx) ? Color.FromArgb(0, 122, 255) : Color.FromArgb(24, 27, 40);
                navButtons[i].ForeColor = (i == idx) ? Color.White : Color.FromArgb(190, 200, 225);
            }
        }

        // ---------------------------------------------------------------------
        // PAGE 1: BORDS ARRONDIS & ANIMATIONS DES ANGLES
        // ---------------------------------------------------------------------
        private Panel BuildPage1Corners()
        {
            Panel page = CreatePageContainer();

            Panel leftCard = CreateCard(18, 16, 370, 595, "📐 Géométrie & Rayon des Angles");
            page.Controls.Add(leftCard);

            CheckBox enableChk = CreateCheckBox("Activer les bords arrondis sur tous mes écrans", 18, 46, controller.Settings.Enabled);
            enableChk.CheckedChanged += (s, e) => { controller.Settings.Enabled = enableChk.Checked; controller.RefreshAllCorners(); };
            leftCard.Controls.Add(enableChk);

            leftCard.Controls.Add(CreateLabel("Rayon de l'arrondi (0 à 110 px) :", 18, 86));
            Label radVal = CreateValueLabel(controller.Settings.CornerRadius + " px", 295, 86);
            leftCard.Controls.Add(radVal);

            TrackBar radSlider = CreateSlider(14, 108, 340, 0, 110, controller.Settings.CornerRadius);
            radSlider.ValueChanged += (s, e) => { controller.Settings.CornerRadius = radSlider.Value; radVal.Text = radSlider.Value + " px"; controller.RefreshAllCorners(); };
            leftCard.Controls.Add(radSlider);

            leftCard.Controls.Add(CreateLabel("Courbe mathématique de l'angle :", 18, 152));
            ComboBox curveCombo = CreateComboBox(18, 176, 332, new string[] {
                " Squircle Continue (Superellipse $n=4.2$)",
                "◯ Cercle Classique (Arc circulaire standard)"
            }, (int)controller.Settings.Curve);
            curveCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Curve = (CurveType)curveCombo.SelectedIndex; controller.RefreshAllCorners(); };
            leftCard.Controls.Add(curveCombo);

            leftCard.Controls.Add(CreateLabel("Angles actifs par écran :", 18, 222));
            CheckBox tl = CreateCheckBox("Haut-Gauche", 18, 248, controller.Settings.TopLeft);
            CheckBox tr = CreateCheckBox("Haut-Droit", 185, 248, controller.Settings.TopRight);
            CheckBox bl = CreateCheckBox("Bas-Gauche", 18, 278, controller.Settings.BottomLeft);
            CheckBox br = CreateCheckBox("Bas-Droit", 185, 278, controller.Settings.BottomRight);
            EventHandler ch = (s, e) => { controller.Settings.TopLeft = tl.Checked; controller.Settings.TopRight = tr.Checked; controller.Settings.BottomLeft = bl.Checked; controller.Settings.BottomRight = br.Checked; controller.RefreshAllCorners(); };
            tl.CheckedChanged += ch; tr.CheckedChanged += ch; bl.CheckedChanged += ch; br.CheckedChanged += ch;
            leftCard.Controls.Add(tl); leftCard.Controls.Add(tr); leftCard.Controls.Add(bl); leftCard.Controls.Add(br);

            // Monitors detected box
            StringBuilder msb = new StringBuilder();
            msb.AppendFormat("🖥 {0} écran(s) détecté(s) :\n", controller.Monitors.Count);
            for (int i = 0; i < controller.Monitors.Count; i++)
            {
                Rectangle b = controller.Monitors[i].Bounds;
                msb.AppendFormat("• Écran {0} : {1}×{2} (X={3}, Y={4})\n", i + 1, b.Width, b.Height, b.X, b.Y);
            }
            Label monLbl = new Label { Location = new Point(18, 325), Size = new Size(332, 85), ForeColor = Color.FromArgb(150, 210, 255), Text = msb.ToString() };
            leftCard.Controls.Add(monLbl);

            Button refBtn = CreateActionBtn("🔄 Rafraîchir la détection des écrans", 18, 420, 332, 36, Color.FromArgb(38, 44, 66), () => controller.RebuildMonitors());
            leftCard.Controls.Add(refBtn);

            // Right Card: Continuous Corner Animations & Live Preview
            Panel rightCard = CreateCard(404, 16, 370, 595, "✨ Animations Continues des 4 Angles");
            page.Controls.Add(rightCard);

            CheckBox bezelChk = CreateCheckBox("Activer le liseré lumineux (Bezel Glow)", 18, 46, controller.Settings.CornerBezelGlow);
            bezelChk.CheckedChanged += (s, e) => { controller.Settings.CornerBezelGlow = bezelChk.Checked; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(bezelChk);

            rightCard.Controls.Add(CreateLabel("Mode d'animation continue des angles :", 18, 84));
            ComboBox animCombo = CreateComboBox(18, 108, 332, new string[] {
                "✨ Fixe élégant (Classique)",
                "🌈 Rotation Chromatique 360° (Dégradé tournant)",
                "💨 Respiration Lumineuse Douce (Breathing)",
                "🎵 Réactif à la Musique / Basses (Audio Beat Sync)",
                "⭐ Scintillement d'Étoiles (Sparkle Shimmer)",
                "⚡ Onde Néon Tournante (Neon Chase)"
            }, (int)controller.Settings.CornerAnim);
            animCombo.SelectedIndexChanged += (s, e) => { controller.Settings.CornerAnim = (CornerAnimationMode)animCombo.SelectedIndex; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(animCombo);

            rightCard.Controls.Add(CreateLabel("Opacité lumineuse du liseré :", 18, 154));
            Label opVal = CreateValueLabel(controller.Settings.BezelGlowOpacity + " %", 295, 154);
            rightCard.Controls.Add(opVal);
            TrackBar opSlider = CreateSlider(14, 176, 340, 0, 100, controller.Settings.BezelGlowOpacity);
            opSlider.ValueChanged += (s, e) => { controller.Settings.BezelGlowOpacity = opSlider.Value; opVal.Text = opSlider.Value + " %"; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(opSlider);

            rightCard.Controls.Add(CreateLabel("Épaisseur du liseré :", 18, 218));
            Label thVal = CreateValueLabel((controller.Settings.BezelThickness / 10.0).ToString("0.0") + " px", 295, 218);
            rightCard.Controls.Add(thVal);
            TrackBar thSlider = CreateSlider(14, 240, 340, 10, 75, controller.Settings.BezelThickness);
            thSlider.ValueChanged += (s, e) => { controller.Settings.BezelThickness = thSlider.Value; thVal.Text = (thSlider.Value / 10.0).ToString("0.0") + " px"; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(thSlider);

            rightCard.Controls.Add(CreateLabel("Vitesse d'animation :", 18, 282));
            Label spVal = CreateValueLabel(controller.Settings.CornerAnimSpeed + " %", 295, 282);
            rightCard.Controls.Add(spVal);
            TrackBar spSlider = CreateSlider(14, 304, 340, 25, 250, controller.Settings.CornerAnimSpeed);
            spSlider.ValueChanged += (s, e) => { controller.Settings.CornerAnimSpeed = spSlider.Value; spVal.Text = spSlider.Value + " %"; controller.Settings.Save(); };
            rightCard.Controls.Add(spSlider);

            rightCard.Controls.Add(CreateLabel("Aperçu temps réel des angles :", 18, 355));
            cornerPreviewBox = new Panel { Location = new Point(18, 380), Size = new Size(332, 120), BackColor = Color.FromArgb(28, 32, 48) };
            cornerPreviewBox.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = cornerPreviewBox.ClientRectangle;
                Color[] pal = ThemeColorEngine.GetPalette(controller.Settings, controller.GlobalTimeSeconds);
                using (LinearGradientBrush wp = new LinearGradientBrush(r, Color.FromArgb(255, 25, 45, 88), Color.FromArgb(255, 78, 32, 102), 25f))
                    g.FillRectangle(wp, r);
                int pr = Math.Min(65, controller.Settings.CornerRadius);
                if (controller.Settings.Enabled && pr > 2)
                {
                    int sz = pr + 24;
                    float ph = controller.GlobalTimeSeconds * (controller.Settings.CornerAnimSpeed / 100f);
                    using (Bitmap b1 = MonitorCorners.RenderCornerBitmap(sz, pr, CornerPosition.TopLeft, controller.Settings, 0.35f, pal, ph, controller.MediaTracker.CurrentAudioPeak))
                    using (Bitmap b2 = MonitorCorners.RenderCornerBitmap(sz, pr, CornerPosition.TopRight, controller.Settings, 0.35f, pal, ph, controller.MediaTracker.CurrentAudioPeak))
                    {
                        g.DrawImage(b1, 0, 0);
                        g.DrawImage(b2, r.Width - b2.Width, 0);
                    }
                }
            };
            rightCard.Controls.Add(cornerPreviewBox);

            return page;
        }

        // ---------------------------------------------------------------------
        // PAGE 2: TRANSITIONS INTER-ÉCRANS & 13 THÈMES DE COULEURS
        // ---------------------------------------------------------------------
        private Panel BuildPage2TransitionsAndColors()
        {
            Panel page = CreatePageContainer();

            Panel leftCard = CreateCard(18, 16, 370, 595, "🌊 Transitions Inter-Écrans (8 Styles)");
            page.Controls.Add(leftCard);

            CheckBox enTrans = CreateCheckBox("Activer l'animation au passage d'un écran à l'autre", 18, 46, controller.Settings.EnableTransition);
            enTrans.CheckedChanged += (s, e) => { controller.Settings.EnableTransition = enTrans.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(enTrans);

            CheckBox magChk = CreateCheckBox("Effet magnétique quand la souris approche du bord", 18, 76, controller.Settings.EnableMagneticProximity);
            magChk.CheckedChanged += (s, e) => { controller.Settings.EnableMagneticProximity = magChk.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(magChk);

            leftCard.Controls.Add(CreateLabel("Style d'animation de traversée :", 18, 118));
            ComboBox styleCombo = CreateComboBox(18, 142, 332, new string[] {
                "✨ Combo Ultime Pro (Pont Liquide + Siri + Étoiles)",
                "💧 Apple Liquid Bridge (Pont métaballe liquide)",
                "🌈 Apple Intelligence (Onde chromatique Siri)",
                "⚡ Portail Plasma & Éclairs Électriques",
                "🪐 Explosion d'Étoiles & Particules Cosmiques",
                "🌊 Onde de Choc Sonar (Anneaux concentriques)",
                "🔥 Lame Laser Néon (Faisceau haute intensité)",
                "💊 Dynamic Island Bounce (Pilule & Rebond Angles)"
            }, (int)controller.Settings.Style);
            styleCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Style = (TransitionStyle)styleCombo.SelectedIndex; controller.Settings.Save(); controller.TriggerDemoTransition(); };
            leftCard.Controls.Add(styleCombo);

            leftCard.Controls.Add(CreateLabel("Durée de la transition :", 18, 195));
            Label durVal = CreateValueLabel(controller.Settings.DurationMs + " ms", 285, 195);
            leftCard.Controls.Add(durVal);
            TrackBar durSlider = CreateSlider(14, 218, 340, 350, 2000, controller.Settings.DurationMs);
            durSlider.ValueChanged += (s, e) => { controller.Settings.DurationMs = durSlider.Value; durVal.Text = durSlider.Value + " ms"; controller.Settings.Save(); };
            leftCard.Controls.Add(durSlider);

            leftCard.Controls.Add(CreateLabel("Amplitude / Taille de l'effet :", 18, 265));
            Label intVal = CreateValueLabel(controller.Settings.IntensityPercent + " %", 290, 265);
            leftCard.Controls.Add(intVal);
            TrackBar intSlider = CreateSlider(14, 288, 340, 60, 190, controller.Settings.IntensityPercent);
            intSlider.ValueChanged += (s, e) => { controller.Settings.IntensityPercent = intSlider.Value; intVal.Text = intSlider.Value + " %"; controller.Settings.Save(); };
            leftCard.Controls.Add(intSlider);

            Button demoBtn = CreateActionBtn("✨ Déclencher une démonstration maintenant", 18, 350, 332, 44, Color.FromArgb(0, 122, 255), () => controller.TriggerDemoTransition());
            leftCard.Controls.Add(demoBtn);

            // Right Card: 13 Color Themes + Custom Colors
            Panel rightCard = CreateCard(404, 16, 370, 595, "🎨 Palettes de Couleurs (13 Thèmes + Custom)");
            page.Controls.Add(rightCard);

            rightCard.Controls.Add(CreateLabel("Thème de couleurs global :", 18, 46));
            themeCombo = CreateComboBox(18, 70, 332, new string[] {
                "🎨 Apple Intelligence (Cyan / Violet / Magenta / Or)",
                "💎 iOS Electric Blue (Bleu / Cyan / Indigo)",
                "🟢 Spotify Neon Green (Vert Émeraude / Menthe)",
                "🔴 Apple Music Rose (Rose Corail / Framboise / Pourpre)",
                "▶️ YouTube Crimson Glow (Rouge Rubis / Orange / Or)",
                "🌈 Cyberpunk RGB Rainbow (Arc-en-ciel 360° animé)",
                "🌅 Sunset Pro (Corail / Rose / Pourpre / Ambre)",
                "🟣 Ultra Violet Nebula (Violet Cosmique / Rose Néon)",
                "🔥 Magma & Gold (Or Solaire / Lave / Rouge Braise)",
                "❄️ Arctic Aurora (Aurore Boréale / Cyan Polaire)",
                "🌸 Sakura Pastel (Rose Cerisier / Lavande / Pêche)",
                "⚪ Minimal Monochrome (Argent / Blanc Pur / Obsidienne)",
                "🖌️ Couleurs Personnalisées (Choisir ci-dessous)"
            }, (int)controller.Settings.Theme);
            themeCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Theme = (ColorTheme)themeCombo.SelectedIndex; controller.RefreshAllCorners(); controller.TriggerDemoTransition(); };
            rightCard.Controls.Add(themeCombo);

            rightCard.Controls.Add(CreateLabel("Sélecteur de couleurs personnalisées :", 18, 122));
            Button c1Btn = CreateActionBtn("🖌️ Choisir Couleur 1", 18, 148, 160, 38, Color.FromArgb(255, Color.FromArgb(controller.Settings.CustomColor1Argb)), null);
            c1Btn.ForeColor = Color.Black;
            c1Btn.Click += (s, e) => PickCustomColor(1, c1Btn);

            Button c2Btn = CreateActionBtn("🖌️ Choisir Couleur 2", 190, 148, 160, 38, Color.FromArgb(255, Color.FromArgb(controller.Settings.CustomColor2Argb)), null);
            c2Btn.Click += (s, e) => PickCustomColor(2, c2Btn);
            rightCard.Controls.Add(c1Btn);
            rightCard.Controls.Add(c2Btn);

            rightCard.Controls.Add(CreateLabel("Ambiances rapides en 1 clic :", 18, 210));
            rightCard.Controls.Add(CreateActionBtn("🟢 Ambiance Spotify Audio Beat", 18, 238, 332, 38, Color.FromArgb(24, 155, 70), () => ApplyPreset(ColorTheme.SpotifyNeonGreen, CornerAnimationMode.AudioBeatSync)));
            rightCard.Controls.Add(CreateActionBtn("🔴 Ambiance Apple Music Glow", 18, 286, 332, 38, Color.FromArgb(205, 35, 65), () => ApplyPreset(ColorTheme.AppleMusicRose, CornerAnimationMode.RotatingGradient)));
            rightCard.Controls.Add(CreateActionBtn("🌈 Ambiance Cyberpunk Full RGB", 18, 334, 332, 38, Color.FromArgb(125, 45, 215), () => ApplyPreset(ColorTheme.CyberpunkRainbow, CornerAnimationMode.RotatingGradient)));
            rightCard.Controls.Add(CreateActionBtn("❄️ Ambiance Aurore Boréale", 18, 382, 332, 38, Color.FromArgb(20, 145, 175), () => ApplyPreset(ColorTheme.ArcticAurora, CornerAnimationMode.BreathingPulse)));

            return page;
        }

        // ---------------------------------------------------------------------
        // PAGE 3: DYNAMIC ISLAND, SYSTÈME D'APPARITION, MUSIQUE & MINUTEUR APPLE
        // ---------------------------------------------------------------------
        private Panel BuildPage3DynamicIslandAndTimer()
        {
            Panel page = CreatePageContainer();

            Panel leftCard = CreateCard(18, 16, 370, 595, "💊 Système d'Apparition & Dynamic Island");
            page.Controls.Add(leftCard);

            leftCard.Controls.Add(CreateLabel("Condition d'apparition du Dynamic Island :", 18, 46));
            ComboBox visCombo = CreateComboBox(18, 70, 332, new string[] {
                "🟢 Toujours visible en haut de l'écran",
                "🖱️ Apparition au bord supérieur (Hot-Edge : approche souris)",
                "🧠 Intelligent (Apparaît si musique, minuteur, IA ou transition)",
                "🌊 Uniquement lors du changement d'écran",
                "❌ Masqué"
            }, (int)controller.Settings.IslandMode);
            visCombo.SelectedIndexChanged += (s, e) => { controller.Settings.IslandMode = (IslandVisibilityMode)visCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(visCombo);

            leftCard.Controls.Add(CreateLabel("Animation d'apparition (Spawn Animation) :", 18, 114));
            ComboBox spawnCombo = CreateComboBox(18, 138, 332, new string[] {
                "💧 Goutte Liquide Élastique (Liquid Drop-Down)",
                "🌊 Déploiement Horizontal Rebonds (Spring Unfold)",
                "🌀 Portail Dimensionnel (Wormhole Pop)",
                "⚡ Téléportation Cyber-Glitch (Neon Snap)",
                "🔥 Éruption Solaire (Nova Bloom)"
            }, (int)controller.Settings.SpawnAnim);
            spawnCombo.SelectedIndexChanged += (s, e) => { controller.Settings.SpawnAnim = (IslandSpawnAnimation)spawnCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(spawnCombo);

            leftCard.Controls.Add(CreateLabel("Position multi-écrans :", 18, 182));
            ComboBox posCombo = CreateComboBox(18, 206, 332, new string[] {
                "🖱️ Suivre l'écran actif (Glisse d'un écran à l'autre !)",
                "🖥️ Toujours rester sur l'écran principal"
            }, (int)controller.Settings.IslandPlacement);
            posCombo.SelectedIndexChanged += (s, e) => { controller.Settings.IslandPlacement = (IslandPlacementMode)posCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(posCombo);

            leftCard.Controls.Add(CreateLabel("Animation du contour de la pilule :", 18, 250));
            ComboBox borderCombo = CreateComboBox(18, 274, 332, new string[] {
                "🌈 Halo Rotatif Multicolore",
                "🎵 Halo Réactif aux Basses & à la Musique",
                "💨 Liseré Verre Respirant Subtil",
                "⚫ Noir Obsidienne Épuré (Sans halo externe)"
            }, (int)controller.Settings.IslandBorder);
            borderCombo.SelectedIndexChanged += (s, e) => { controller.Settings.IslandBorder = (IslandBorderAnimation)borderCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(borderCombo);

            leftCard.Controls.Add(CreateLabel("Taille du Dynamic Island :", 18, 318));
            Label szVal = CreateValueLabel(controller.Settings.IslandScalePercent + " %", 290, 318);
            leftCard.Controls.Add(szVal);
            TrackBar szSlider = CreateSlider(14, 340, 340, 75, 155, controller.Settings.IslandScalePercent);
            szSlider.ValueChanged += (s, e) => { controller.Settings.IslandScalePercent = szSlider.Value; szVal.Text = szSlider.Value + " %"; controller.Settings.Save(); };
            leftCard.Controls.Add(szSlider);

            CheckBox hovChk = CreateCheckBox("Ouvrir le hub complet (4 onglets) au survol souris", 18, 386, controller.Settings.IslandExpandOnHover);
            hovChk.CheckedChanged += (s, e) => { controller.Settings.IslandExpandOnHover = hovChk.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(hovChk);

            CheckBox brandChk = CreateCheckBox("Adapter la couleur à Spotify / Apple Music / YouTube", 18, 416, controller.Settings.IslandAutoAdaptBrandColor);
            brandChk.CheckedChanged += (s, e) => { controller.Settings.IslandAutoAdaptBrandColor = brandChk.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(brandChk);

            // Right Card: Apple Timer / Pomodoro & Music Controls
            Panel rightCard = CreateCard(404, 16, 370, 595, "⏱️ Minuteur Style Apple & Contrôles Médias");
            page.Controls.Add(rightCard);

            rightCard.Controls.Add(CreateLabel("Lancer un Minuteur / Pomodoro dans le Dynamic Island :", 18, 46));
            rightCard.Controls.Add(CreateActionBtn("⏱️ Minuteur 1 min", 18, 74, 160, 38, Color.FromArgb(230, 125, 10), () => controller.DynamicIsland.TimerEngine.StartCountdown(60, "1 Minute")));
            rightCard.Controls.Add(CreateActionBtn("⏱️ Minuteur 5 min", 190, 74, 160, 38, Color.FromArgb(230, 125, 10), () => controller.DynamicIsland.TimerEngine.StartCountdown(300, "5 Minutes")));
            rightCard.Controls.Add(CreateActionBtn("🍅 Pomodoro 25 min", 18, 120, 160, 38, Color.FromArgb(215, 55, 45), () => controller.DynamicIsland.TimerEngine.StartCountdown(1500, "Pomodoro 25m")));
            rightCard.Controls.Add(CreateActionBtn("⏱️ Chronomètre", 190, 120, 160, 38, Color.FromArgb(45, 115, 215), () => controller.DynamicIsland.TimerEngine.SwitchToStopwatch()));

            rightCard.Controls.Add(CreateActionBtn("⏯ Démarrer / Pause Minuteur", 18, 166, 215, 36, Color.FromArgb(40, 46, 68), () => controller.DynamicIsland.TimerEngine.ToggleStartPause()));
            rightCard.Controls.Add(CreateActionBtn("🔄 Reset", 242, 166, 108, 36, Color.FromArgb(40, 46, 68), () => controller.DynamicIsland.TimerEngine.Reset()));

            rightCard.Controls.Add(CreateLabel("Lecteurs Musicaux & Contrôles Directs :", 18, 224));
            rightCard.Controls.Add(CreateActionBtn("🟢 Ouvrir Spotify", 18, 250, 106, 36, Color.FromArgb(24, 155, 70), () => controller.MediaTracker.LaunchOrFocusService("spotify")));
            rightCard.Controls.Add(CreateActionBtn("🔴 Apple Music", 131, 250, 106, 36, Color.FromArgb(205, 35, 65), () => controller.MediaTracker.LaunchOrFocusService("applemusic")));
            rightCard.Controls.Add(CreateActionBtn("▶️ YouTube Music", 244, 250, 106, 36, Color.FromArgb(200, 25, 45), () => controller.MediaTracker.LaunchOrFocusService("youtube")));

            rightCard.Controls.Add(CreateActionBtn("⏮ Précédent", 18, 296, 106, 36, Color.FromArgb(40, 46, 68), () => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PREV_TRACK)));
            rightCard.Controls.Add(CreateActionBtn("⏯ Play / Pause", 131, 296, 106, 36, Color.FromArgb(0, 122, 255), () => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PLAY_PAUSE)));
            rightCard.Controls.Add(CreateActionBtn("⏭ Suivant", 244, 296, 106, 36, Color.FromArgb(40, 46, 68), () => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_NEXT_TRACK)));

            Label infoBox = new Label
            {
                Location = new Point(18, 355),
                Size = new Size(332, 110),
                BackColor = Color.FromArgb(28, 32, 46),
                ForeColor = Color.FromArgb(190, 205, 235),
                Padding = new Padding(10),
                Text = "💡 Le Dynamic Island possède 4 onglets interactifs quand vous le survolez :\n• 🎵 Musique (Spotify, Apple Music, YouTube)\n• ⏱️ Minuteur & Chronomètre avec anneau circulaire\n• 🦊 Compagnon Cyber interactif\n• 🤖 Assistant IA (Gemini, Claude, ChatGPT, Mistral)"
            };
            rightCard.Controls.Add(infoBox);

            return page;
        }

        // ---------------------------------------------------------------------
        // PAGE 4: COMPAGNON CYBER ORIGINAL (NON-APPLE)
        // ---------------------------------------------------------------------
        private Panel BuildPage4Companion()
        {
            Panel page = CreatePageContainer();

            Panel leftCard = CreateCard(18, 16, 370, 595, "🦊 Choix & Comportement du Compagnon");
            page.Controls.Add(leftCard);

            leftCard.Controls.Add(CreateLabel("Espèce du Compagnon (100% Original, Non-Apple) :", 18, 46));
            ComboBox specCombo = CreateComboBox(18, 70, 332, new string[] {
                "❌ Aucun compagnon (Désactivé)",
                "🦊 Kitsune Cyber-Fox (Renard Néon à queues d'énergie)",
                "🐉 Draco Plasma-Dragon (Mini Dragon ailé cybernétique)",
                "👾 Specter Holo-Ghost (Esprit Holographique & Anneau)",
                "🐱 Neko Bionic-Cat (Chat Synthwave avec casque audio)",
                "🤖 Atlas Mecha-Drone (Sphère Sci-Fi à œil laser)",
                "🔥 Ignis Cosmic-Flame (Élémentaire de Feu Cosmique)"
            }, (int)controller.Settings.Companion);
            specCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Companion = (CompanionSpecies)specCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(specCombo);

            leftCard.Controls.Add(CreateLabel("Nom personnalisé de ton compagnon :", 18, 116));
            TextBox nameBox = new TextBox
            {
                Location = new Point(18, 140),
                Width = 332,
                Text = controller.Settings.CompanionName,
                BackColor = Color.FromArgb(32, 36, 52),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            nameBox.TextChanged += (s, e) => { controller.Settings.CompanionName = nameBox.Text; controller.Settings.Save(); };
            leftCard.Controls.Add(nameBox);

            leftCard.Controls.Add(CreateLabel("Position du compagnon sur l'écran :", 18, 182));
            ComboBox posCombo = CreateComboBox(18, 206, 332, new string[] {
                "🦊 Perché sur le dessus du Dynamic Island",
                "💊 À l'intérieur de la pilule Dynamic Island",
                "🛸 En lévitation à droite du Dynamic Island"
            }, (int)controller.Settings.CompanionPos);
            posCombo.SelectedIndexChanged += (s, e) => { controller.Settings.CompanionPos = (CompanionPositionMode)posCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(posCombo);

            leftCard.Controls.Add(CreateLabel("Taille du compagnon :", 18, 252));
            Label cSzVal = CreateValueLabel(controller.Settings.CompanionScalePercent + " %", 290, 252);
            leftCard.Controls.Add(cSzVal);
            TrackBar cSzSlider = CreateSlider(14, 274, 340, 70, 175, controller.Settings.CompanionScalePercent);
            cSzSlider.ValueChanged += (s, e) => { controller.Settings.CompanionScalePercent = cSzSlider.Value; cSzVal.Text = cSzSlider.Value + " %"; controller.Settings.Save(); };
            leftCard.Controls.Add(cSzSlider);

            CheckBox eyeChk = CreateCheckBox("Les yeux du compagnon suivent le curseur de la souris", 18, 322, controller.Settings.CompanionFollowCursorEyes);
            eyeChk.CheckedChanged += (s, e) => { controller.Settings.CompanionFollowCursorEyes = eyeChk.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(eyeChk);

            CheckBox danceChk = CreateCheckBox("Danse et affiche des notes ♪ quand la musique joue", 18, 352, controller.Settings.CompanionDanceToMusic);
            danceChk.CheckedChanged += (s, e) => { controller.Settings.CompanionDanceToMusic = danceChk.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(danceChk);

            CheckBox bubChk = CreateCheckBox("Afficher les bulles de dialogue et réactions IA", 18, 382, controller.Settings.CompanionShowSpeechBubbles);
            bubChk.CheckedChanged += (s, e) => { controller.Settings.CompanionShowSpeechBubbles = bubChk.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(bubChk);

            // Right Card: Live Companion Interactive Stage
            Panel rightCard = CreateCard(404, 16, 370, 595, "🎮 Aperçu Vivant & Interactions");
            page.Controls.Add(rightCard);

            companionPreviewBox = new Panel { Location = new Point(18, 48), Size = new Size(332, 210), BackColor = Color.FromArgb(16, 19, 30) };
            companionPreviewBox.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Color[] pal = ThemeColorEngine.GetPalette(controller.Settings, controller.GlobalTimeSeconds);
                CompanionSpecies sp = controller.Settings.Companion == CompanionSpecies.None ? CompanionSpecies.KitsuneCyberFox : controller.Settings.Companion;
                Point screenCenter = companionPreviewBox.PointToScreen(new Point(companionPreviewBox.Width / 2, companionPreviewBox.Height / 2));
                controller.DynamicIsland.CompanionEngine.DrawCompanion(
                    g, new RectangleF(86, 25, 160, 160), sp, Cursor.Position, screenCenter,
                    pal[0], pal[1 % pal.Length], controller.GlobalTimeSeconds, controller.MediaTracker.CurrentAudioPeak, true);
            };
            rightCard.Controls.Add(companionPreviewBox);

            rightCard.Controls.Add(CreateActionBtn("⚡ Caresser / Booster le Compagnon (+15 XP)", 18, 275, 332, 40, Color.FromArgb(0, 122, 255), () => controller.DynamicIsland.CompanionEngine.InteractPetOrBoost()));
            rightCard.Controls.Add(CreateActionBtn("💬 Faire parler le Compagnon via l'IA", 18, 325, 332, 40, Color.FromArgb(135, 50, 220), () =>
            {
                controller.DynamicIsland.AiEngine.SendPromptAsync(controller.Settings, "Dis une phrase courte et stylée en tant que compagnon cybernétique à ton utilisateur !", reply => controller.DynamicIsland.CompanionEngine.Say(reply, 6000));
            }));

            return page;
        }

        // ---------------------------------------------------------------------
        // PAGE 5: MULTI-PROVIDER AI SYSTEM (GEMINI, CLAUDE, CHATGPT, MISTRAL)
        // ---------------------------------------------------------------------
        private Panel BuildPage5AiAssistant()
        {
            Panel page = CreatePageContainer();

            Panel leftCard = CreateCard(18, 16, 370, 595, "🔑 Configuration des 4 API IA");
            page.Controls.Add(leftCard);

            leftCard.Controls.Add(CreateLabel("Fournisseur IA Actif :", 18, 46));
            ComboBox provCombo = CreateComboBox(18, 70, 332, new string[] {
                "✨ Google Gemini (Recommandé • Rapide & Gratuit)",
                "🧠 Anthropic Claude (Claude 3.5 Sonnet / Haiku)",
                "🟢 OpenAI ChatGPT (GPT-4o / GPT-4o-mini)",
                "🌪️ Mistral AI (Mistral Small / Large)"
            }, (int)controller.Settings.ActiveAiProvider);
            provCombo.SelectedIndexChanged += (s, e) => { controller.Settings.ActiveAiProvider = (AiProvider)provCombo.SelectedIndex; controller.Settings.Save(); };
            leftCard.Controls.Add(provCombo);

            // 1. Gemini Key & Model
            leftCard.Controls.Add(CreateLabel("Clé API Google Gemini & Modèle :", 18, 114));
            TextBox gemKey = CreateApiInputBox(18, 136, 210, controller.Settings.GeminiApiKey, true);
            TextBox gemMod = CreateApiInputBox(234, 136, 116, controller.Settings.GeminiModel, false);
            gemKey.TextChanged += (s, e) => { controller.Settings.GeminiApiKey = gemKey.Text.Trim(); controller.Settings.Save(); };
            gemMod.TextChanged += (s, e) => { controller.Settings.GeminiModel = gemMod.Text.Trim(); controller.Settings.Save(); };
            leftCard.Controls.Add(gemKey); leftCard.Controls.Add(gemMod);

            // 2. Claude Key & Model
            leftCard.Controls.Add(CreateLabel("Clé API Anthropic Claude & Modèle :", 18, 174));
            TextBox clKey = CreateApiInputBox(18, 196, 210, controller.Settings.ClaudeApiKey, true);
            TextBox clMod = CreateApiInputBox(234, 196, 116, controller.Settings.ClaudeModel, false);
            clKey.TextChanged += (s, e) => { controller.Settings.ClaudeApiKey = clKey.Text.Trim(); controller.Settings.Save(); };
            clMod.TextChanged += (s, e) => { controller.Settings.ClaudeModel = clMod.Text.Trim(); controller.Settings.Save(); };
            leftCard.Controls.Add(clKey); leftCard.Controls.Add(clMod);

            // 3. OpenAI ChatGPT Key & Model
            leftCard.Controls.Add(CreateLabel("Clé API OpenAI ChatGPT & Modèle :", 18, 234));
            TextBox oaKey = CreateApiInputBox(18, 256, 210, controller.Settings.OpenAiApiKey, true);
            TextBox oaMod = CreateApiInputBox(234, 256, 116, controller.Settings.OpenAiModel, false);
            oaKey.TextChanged += (s, e) => { controller.Settings.OpenAiApiKey = oaKey.Text.Trim(); controller.Settings.Save(); };
            oaMod.TextChanged += (s, e) => { controller.Settings.OpenAiModel = oaMod.Text.Trim(); controller.Settings.Save(); };
            leftCard.Controls.Add(oaKey); leftCard.Controls.Add(oaMod);

            // 4. Mistral Key & Model
            leftCard.Controls.Add(CreateLabel("Clé API Mistral AI & Modèle :", 18, 294));
            TextBox miKey = CreateApiInputBox(18, 316, 210, controller.Settings.MistralApiKey, true);
            TextBox miMod = CreateApiInputBox(234, 316, 116, controller.Settings.MistralModel, false);
            miKey.TextChanged += (s, e) => { controller.Settings.MistralApiKey = miKey.Text.Trim(); controller.Settings.Save(); };
            miMod.TextChanged += (s, e) => { controller.Settings.MistralModel = miMod.Text.Trim(); controller.Settings.Save(); };
            leftCard.Controls.Add(miKey); leftCard.Controls.Add(miMod);

            // System Prompt
            leftCard.Controls.Add(CreateLabel("Personnalité / Instructions du Compagnon IA :", 18, 358));
            TextBox sysBox = new TextBox
            {
                Location = new Point(18, 382),
                Size = new Size(332, 95),
                Multiline = true,
                Text = controller.Settings.AiSystemPrompt,
                BackColor = Color.FromArgb(30, 34, 50),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            sysBox.TextChanged += (s, e) => { controller.Settings.AiSystemPrompt = sysBox.Text; controller.Settings.Save(); };
            leftCard.Controls.Add(sysBox);

            // Right Card: Live AI Console & Test
            Panel rightCard = CreateCard(404, 16, 370, 595, "🤖 Console de Chat & Test IA en Direct");
            page.Controls.Add(rightCard);

            aiStatusLabel = new Label
            {
                Location = new Point(18, 48),
                Size = new Size(332, 230),
                BackColor = Color.FromArgb(16, 19, 30),
                ForeColor = Color.FromArgb(225, 235, 255),
                Padding = new Padding(12),
                Text = controller.DynamicIsland.AiEngine.LastResponse
            };
            rightCard.Controls.Add(aiStatusLabel);

            rightCard.Controls.Add(CreateLabel("Envoyer un message au Compagnon IA :", 18, 292));
            aiChatTestInput = new TextBox
            {
                Location = new Point(18, 316),
                Size = new Size(332, 28),
                BackColor = Color.FromArgb(30, 34, 50),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "Salut ! Présente-toi en une phrase stylée !"
            };
            rightCard.Controls.Add(aiChatTestInput);

            Button sendAiBtn = CreateActionBtn("🚀 Envoyer à l'IA & Faire parler le Compagnon", 18, 356, 332, 42, Color.FromArgb(0, 122, 255), () =>
            {
                string prompt = aiChatTestInput.Text.Trim();
                if (string.IsNullOrEmpty(prompt)) return;
                aiStatusLabel.Text = "⏳ Connexion à " + controller.Settings.ActiveAiProvider + " en cours...";
                controller.DynamicIsland.AiEngine.SendPromptAsync(controller.Settings, prompt, reply =>
                {
                    if (!IsDisposed && IsHandleCreated)
                    {
                        BeginInvoke((Action)(() =>
                        {
                            aiStatusLabel.Text = reply;
                            controller.DynamicIsland.CompanionEngine.Say(reply, 6500);
                        }));
                    }
                });
            });
            rightCard.Controls.Add(sendAiBtn);

            return page;
        }

        private TextBox CreateApiInputBox(int x, int y, int w, string val, bool isPassword)
        {
            TextBox tb = new TextBox
            {
                Location = new Point(x, y),
                Width = w,
                Text = val,
                BackColor = Color.FromArgb(30, 34, 50),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
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
                    controller.Settings.Theme = ColorTheme.CustomColors;
                    if (themeCombo != null) themeCombo.SelectedIndex = (int)ColorTheme.CustomColors;
                    controller.RefreshAllCorners();
                }
            }
        }

        private void ApplyPreset(ColorTheme theme, CornerAnimationMode cornerMode)
        {
            controller.Settings.Theme = theme;
            controller.Settings.CornerAnim = cornerMode;
            if (themeCombo != null) themeCombo.SelectedIndex = (int)theme;
            controller.RefreshAllCorners();
            controller.TriggerDemoTransition();
        }

        private Panel CreatePageContainer()
        {
            return new Panel
            {
                Location = new Point(245, 78),
                Size = new Size(805, 650),
                BackColor = Color.FromArgb(12, 13, 19)
            };
        }

        private Panel CreateCard(int x, int y, int w, int h, string title)
        {
            Panel p = new Panel { Location = new Point(x, y), Size = new Size(w, h), BackColor = Color.FromArgb(21, 23, 34) };
            Label hdr = new Label { Text = title, Font = new Font("Segoe UI Semibold", 10.8f, FontStyle.Bold), ForeColor = Color.FromArgb(105, 205, 255), Location = new Point(16, 12), AutoSize = true };
            p.Controls.Add(hdr);
            return p;
        }

        private Button CreateActionBtn(string text, int x, int y, int w, int h, Color bg, Action onClick)
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
            b.FlatAppearance.BorderSize = 0;
            if (onClick != null) b.Click += (s, e) => onClick();
            return b;
        }

        private TrackBar CreateSlider(int x, int y, int w, int min, int max, int val)
        {
            return new TrackBar
            {
                Minimum = min,
                Maximum = max,
                Value = Math.Min(max, Math.Max(min, val)),
                TickStyle = TickStyle.None,
                Location = new Point(x, y),
                Size = new Size(w, 28),
                BackColor = Color.FromArgb(21, 23, 34)
            };
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label { Text = text, Location = new Point(x, y), AutoSize = true, ForeColor = Color.FromArgb(205, 212, 232) };
        }

        private Label CreateValueLabel(string text, int x, int y)
        {
            return new Label { Text = text, Location = new Point(x, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold), ForeColor = Color.FromArgb(80, 210, 255) };
        }

        private CheckBox CreateCheckBox(string text, int x, int y, bool isChecked)
        {
            return new CheckBox { Text = text, Location = new Point(x, y), AutoSize = true, Checked = isChecked, ForeColor = Color.FromArgb(230, 235, 250), Cursor = Cursors.Hand };
        }

        private ComboBox CreateComboBox(int x, int y, int w, string[] items, int selectedIndex)
        {
            ComboBox cb = new ComboBox { Location = new Point(x, y), Width = w, DropDownStyle = ComboBoxStyle.DropDownList, BackColor = Color.FromArgb(32, 36, 52), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
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
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                return;
            }
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
                    System.Windows.Forms.Timer welcomeTimer = new System.Windows.Forms.Timer { Interval = 450 };
                    welcomeTimer.Tick += (s, e) =>
                    {
                        welcomeTimer.Stop();
                        welcomeTimer.Dispose();
                        controller.TriggerDemoTransition();
                        controller.DynamicIsland.CompanionEngine.Say("Salut ! Survole le Dynamic Island ou clique sur moi ! ⚡", 5500);
                    };
                    welcomeTimer.Start();
                }
                Application.Run();
            }
        }
    }
}
