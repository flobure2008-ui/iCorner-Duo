using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace iCornerDuo
{
    // =========================================================================
    // ENUMS & CONFIGURATION
    // =========================================================================
    public enum CurveType
    {
        SquircleApple = 0,
        ClassicCircle = 1
    }

    public enum CornerAnimationMode
    {
        Static = 0,              // Liseré fixe classique
        RotatingGradient = 1,    // Rotation chromatique 360° continue
        BreathingPulse = 2,      // Respiration lumineuse douce
        AudioBeatSync = 3,       // Réactif aux basses / musique en temps réel !
        SparkleShimmer = 4,      // Scintillement d'étoiles dans les angles
        NeonChase = 5            // Onde néon pulsante décalée par angle
    }

    public enum TransitionStyle
    {
        ComboUltimate = 0,       // Pont Liquide + Siri Wave + Particules + Flash Angles
        LiquidBridge = 1,        // Pont liquide métaballe Apple Duo
        SiriIntelligenceGlow = 2,// Onde chromatique Apple Intelligence
        ElectricPlasmaPortal = 3,// Éclairs & arcs plasma électriques
        CosmicParticleBurst = 4, // Explosion de particules & étoiles
        SonarShockwave = 5,      // Ondes de choc concentriques (Ripples)
        NeonLaserBlade = 6,      // Faisceau laser néon sur la bordure
        DynamicIslandBounce = 7  // Focus pilule Dynamic Island + Pulse Angles
    }

    public enum ColorTheme
    {
        AppleIntelligence = 0,   // Cyan, Violet, Magenta, Amber
        iOSBlueCyan = 1,         // Electric Blue, Cyan, Indigo
        SpotifyNeonGreen = 2,    // Spotify Green, Mint, Emerald
        AppleMusicRose = 3,      // Apple Music Rose, Crimson, Pink
        YouTubeCrimson = 4,      // YouTube Red, Fiery Orange, Magenta
        CyberpunkRainbow = 5,    // Arc-en-ciel RGB 360° animé
        SunsetPro = 6,           // Corail, Rose, Pourpre, Or
        UltraVioletNebula = 7,   // Violet cosmique, Indigo, Rose néon
        MagmaGold = 8,           // Or solaire, Orange lave, Rouge braise
        ArcticAurora = 9,        // Vert aurore, Cyan polaire, Bleu glacier
        SakuraPastel = 10,       // Rose cerisier, Lavande, Pêche
        MinimalMonochrome = 11,  // Argent, Blanc pur, Noir obsidienne
        CustomColors = 12        // Couleurs personnalisées par l'utilisateur
    }

    public enum IslandVisibilityMode
    {
        AlwaysVisible = 0,       // Toujours affiché en haut de l'écran
        WhenMediaOrTransition = 1,// Affiché quand une musique joue ou en transition
        OnlyOnTransition = 2,    // Uniquement lors du changement d'écran
        Disabled = 3             // Désactivé
    }

    public enum IslandPlacementMode
    {
        FollowActiveScreen = 0,  // Suit l'écran où se trouve la souris
        PrimaryScreenOnly = 1    // Toujours sur l'écran principal
    }

    public enum IslandBorderAnimation
    {
        SiriRotatingGlow = 0,    // Halo rotatif multicolore Apple Intelligence
        AudioReactiveHalo = 1,   // Halo qui pulse avec la musique
        BreathingGlass = 2,      // Liseré verre respirant subtil
        MinimalClean = 3         // Bordure noire épurée sans halo externe
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

    public class AppSettings
    {
        // Corner Settings
        public bool Enabled = true;
        public int CornerRadius = 38;
        public CurveType Curve = CurveType.SquircleApple;
        public bool TopLeft = true;
        public bool TopRight = true;
        public bool BottomLeft = true;
        public bool BottomRight = true;
        public bool CornerBezelGlow = true;
        public int BezelGlowOpacity = 65; // 0..100
        public int BezelThickness = 25;   // 10..80 (div by 10 -> 1.0px to 8.0px)
        public CornerAnimationMode CornerAnim = CornerAnimationMode.RotatingGradient;
        public int CornerAnimSpeed = 100; // 20..250 %

        // Transition Settings
        public bool EnableTransition = true;
        public bool EnableMagneticProximity = true;
        public TransitionStyle Style = TransitionStyle.ComboUltimate;
        public ColorTheme Theme = ColorTheme.AppleIntelligence;
        public int DurationMs = 1000;      // 350..2200
        public int IntensityPercent = 120; // 50..200

        // Custom Colors
        public int CustomColor1Argb = Color.FromArgb(255, 0, 240, 255).ToArgb();
        public int CustomColor2Argb = Color.FromArgb(255, 255, 0, 128).ToArgb();

        // Dynamic Island Settings
        public IslandVisibilityMode IslandMode = IslandVisibilityMode.AlwaysVisible;
        public IslandPlacementMode IslandPlacement = IslandPlacementMode.FollowActiveScreen;
        public IslandBorderAnimation IslandBorder = IslandBorderAnimation.SiriRotatingGlow;
        public int IslandScalePercent = 105; // 70..160
        public bool IslandExpandOnHover = true;
        public bool IslandShowClockWhenIdle = true;
        public bool IslandAutoAdaptBrandColor = true; // Adapts glow to Spotify Green / Apple Music Rose / YouTube Red!

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
                sb.AppendFormat("  \"IslandPlacement\": {0},\n", (int)IslandPlacement);
                sb.AppendFormat("  \"IslandBorder\": {0},\n", (int)IslandBorder);
                sb.AppendFormat("  \"IslandScalePercent\": {0},\n", IslandScalePercent);
                sb.AppendFormat("  \"IslandExpandOnHover\": {0},\n", IslandExpandOnHover.ToString().ToLower());
                sb.AppendFormat("  \"IslandShowClockWhenIdle\": {0},\n", IslandShowClockWhenIdle.ToString().ToLower());
                sb.AppendFormat("  \"IslandAutoAdaptBrandColor\": {0}\n", IslandAutoAdaptBrandColor.ToString().ToLower());
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
                    s.IslandPlacement = (IslandPlacementMode)ReadInt(json, "IslandPlacement", (int)s.IslandPlacement);
                    s.IslandBorder = (IslandBorderAnimation)ReadInt(json, "IslandBorder", (int)s.IslandBorder);
                    s.IslandScalePercent = ReadInt(json, "IslandScalePercent", s.IslandScalePercent);
                    s.IslandExpandOnHover = ReadBool(json, "IslandExpandOnHover", s.IslandExpandOnHover);
                    s.IslandShowClockWhenIdle = ReadBool(json, "IslandShowClockWhenIdle", s.IslandShowClockWhenIdle);
                    s.IslandAutoAdaptBrandColor = ReadBool(json, "IslandAutoAdaptBrandColor", s.IslandAutoAdaptBrandColor);
                }
            }
            catch { }
            return s;
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
    }

    // =========================================================================
    // WIN32 INTEROP + MEDIA KEYS + COREAUDIO REAL-TIME PEAK METER
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

        // Media Virtual Key Codes
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

    // =========================================================================
    // WINDOWS COREAUDIO PEAK METER (REAL-TIME AUDIO BEAT & SPECTRUM DETECTION)
    // =========================================================================
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
        public float CurrentAudioPeak = 0f;     // Smooth 0..1 audio level
        public float RawAudioPeak = 0f;
        public bool IsAudioActive = false;

        public MediaSourceType ActiveSource = MediaSourceType.None;
        public string TrackTitle = "Aucune musique détectée";
        public string TrackArtist = "Spotify • Apple Music • YouTube";
        public IntPtr ActiveMediaWindowHandle = IntPtr.Zero;

        private long lastWindowScanTicks = 0;
        private Random rng = new Random();
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
                // eRender = 0, eMultimedia = 1
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
            // 1. Read real-time audio peak from Windows CoreAudio
            float peak = 0f;
            if (audioMeter != null)
            {
                try { audioMeter.GetPeakValue(out peak); } catch { }
            }
            RawAudioPeak = peak;
            // Smooth attack/decay
            if (peak > CurrentAudioPeak)
                CurrentAudioPeak = CurrentAudioPeak * 0.45f + peak * 0.55f;
            else
                CurrentAudioPeak = CurrentAudioPeak * 0.86f + peak * 0.14f;

            IsAudioActive = (CurrentAudioPeak > 0.015f);

            // 2. Update 18 animated spectrum bars driven by real audio peak (or subtle idle wave)
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

            // 3. Periodically scan media windows (every ~1.5 seconds)
            long now = DateTime.UtcNow.Ticks;
            if ((now - lastWindowScanTicks) > 15000000L) // 1.5s
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
                IntPtr foundHwnd = IntPtr.Zero;

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

                    // 1. SPOTIFY DESKTOP APP
                    if (pname == "spotify")
                    {
                        foundHwnd = p.MainWindowHandle;
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
                        break; // Priority to Spotify desktop
                    }

                    // 2. APPLE MUSIC / ITUNES DESKTOP APP
                    if (pname == "applemusic" || pname == "itunes")
                    {
                        foundHwnd = p.MainWindowHandle;
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

                    // 3. BROWSERS (YouTube, YouTube Music, Spotify Web, Apple Music Web, Deezer, SoundCloud)
                    if (pname == "chrome" || pname == "msedge" || pname == "firefox" || pname == "brave" || pname == "opera" || pname == "vivaldi")
                    {
                        string clean = StripBrowserSuffix(wtitle);
                        if (clean.IndexOf("YouTube Music", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            foundHwnd = p.MainWindowHandle;
                            foundSource = MediaSourceType.YouTube;
                            foundTitle = clean.Replace("- YouTube Music", "").Replace("YouTube Music", "").Trim();
                            if (string.IsNullOrEmpty(foundTitle)) foundTitle = "YouTube Music";
                            foundArtist = "YouTube Music";
                        }
                        else if (clean.IndexOf("YouTube", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            foundHwnd = p.MainWindowHandle;
                            foundSource = MediaSourceType.YouTube;
                            foundTitle = clean.Replace("- YouTube", "").Replace("YouTube", "").Trim();
                            if (string.IsNullOrEmpty(foundTitle)) foundTitle = "Vidéo / Musique YouTube";
                            foundArtist = "YouTube";
                        }
                        else if (clean.IndexOf("Apple Music", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            foundHwnd = p.MainWindowHandle;
                            foundSource = MediaSourceType.AppleMusic;
                            foundTitle = clean.Replace("- Apple Music", "").Replace("Apple Music", "").Trim();
                            if (string.IsNullOrEmpty(foundTitle)) foundTitle = "Apple Music Web";
                            foundArtist = "Apple Music";
                        }
                        else if (clean.IndexOf("Spotify", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            foundHwnd = p.MainWindowHandle;
                            foundSource = MediaSourceType.Spotify;
                            foundTitle = clean.Replace("- Spotify", "").Replace("Spotify", "").Trim();
                            if (string.IsNullOrEmpty(foundTitle)) foundTitle = "Spotify Web Player";
                            foundArtist = "Spotify";
                        }
                        else if (clean.IndexOf("Deezer", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                 clean.IndexOf("SoundCloud", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            foundHwnd = p.MainWindowHandle;
                            foundSource = MediaSourceType.BrowserMedia;
                            foundTitle = clean;
                            foundArtist = "Lecteur Web";
                        }
                    }

                    // 4. VLC / WINDOWS MEDIA PLAYER
                    if (pname == "vlc" || pname == "music.ui" || pname == "mediaplayer")
                    {
                        foundHwnd = p.MainWindowHandle;
                        foundSource = MediaSourceType.SystemAudio;
                        foundTitle = wtitle.Replace("- Lecteur multimédia VLC", "").Replace("- VLC media player", "").Trim();
                        foundArtist = "Lecteur Multimédia";
                    }
                }

                if (foundSource != MediaSourceType.None)
                {
                    ActiveSource = foundSource;
                    TrackTitle = foundTitle;
                    TrackArtist = foundArtist;
                    ActiveMediaWindowHandle = foundHwnd;
                }
                else if (IsAudioActive)
                {
                    ActiveSource = MediaSourceType.SystemAudio;
                    TrackTitle = "Audio Système en cours";
                    TrackArtist = "Flux audio actif • Contrôles multimédias";
                }
                else
                {
                    ActiveSource = MediaSourceType.None;
                    TrackTitle = "iCorner Duo Music Hub";
                    TrackArtist = "Survolez pour contrôler Spotify • Apple Music • YouTube";
                }
            }
            catch { }
        }

        private static string StripBrowserSuffix(string title)
        {
            string t = title.Trim();
            // Strip leading notification count like "(1) "
            if (t.StartsWith("(") && t.IndexOf(") ") > 0 && t.IndexOf(") ") < 6)
            {
                t = t.Substring(t.IndexOf(") ") + 2).Trim();
            }
            string[] suffixes = new string[] {
                " - Google Chrome",
                " - Microsoft​ Edge",
                " - Microsoft Edge",
                " — Mozilla Firefox",
                " - Mozilla Firefox",
                " - Brave",
                " - Opera",
                " - Vivaldi"
            };
            foreach (string s in suffixes)
            {
                if (t.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                {
                    t = t.Substring(0, t.Length - s.Length).Trim();
                }
            }
            // Also strip Edge "and X more pages"
            int edgeMore = t.IndexOf(" et ", StringComparison.OrdinalIgnoreCase);
            if (edgeMore > 0 && t.IndexOf(" page", edgeMore, StringComparison.OrdinalIgnoreCase) > 0)
            {
                t = t.Substring(0, edgeMore).Trim();
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
                        NativeMethods.ShowWindow(sp[0].MainWindowHandle, 9); // SW_RESTORE
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
        public static Color[] GetPalette(AppSettings settings, float timeSeconds, MediaSourceType activeMedia = MediaSourceType.None)
        {
            // Optional automatic brand color adaptation when music is actively playing
            if (settings.IslandAutoAdaptBrandColor && activeMedia != MediaSourceType.None)
            {
                if (activeMedia == MediaSourceType.Spotify && settings.Theme == ColorTheme.SpotifyNeonGreen)
                {
                    // Handled below
                }
            }

            switch (settings.Theme)
            {
                case ColorTheme.iOSBlueCyan:
                    return new Color[] {
                        Color.FromArgb(255, 0, 210, 255),
                        Color.FromArgb(255, 10, 132, 255),
                        Color.FromArgb(255, 94, 92, 230),
                        Color.FromArgb(255, 110, 235, 255)
                    };
                case ColorTheme.SpotifyNeonGreen:
                    return new Color[] {
                        Color.FromArgb(255, 29, 185, 84),   // Spotify Official Green
                        Color.FromArgb(255, 30, 215, 96),   // Neon Green
                        Color.FromArgb(255, 0, 235, 165),   // Mint Cyan
                        Color.FromArgb(255, 110, 255, 145)
                    };
                case ColorTheme.AppleMusicRose:
                    return new Color[] {
                        Color.FromArgb(255, 250, 36, 60),   // Apple Music Red-Rose
                        Color.FromArgb(255, 255, 55, 95),   // Hot Pink
                        Color.FromArgb(255, 191, 90, 242),  // Purple
                        Color.FromArgb(255, 255, 135, 160)
                    };
                case ColorTheme.YouTubeCrimson:
                    return new Color[] {
                        Color.FromArgb(255, 255, 0, 51),    // YouTube Crimson
                        Color.FromArgb(255, 255, 85, 0),    // Fiery Orange
                        Color.FromArgb(255, 255, 0, 128),   // Magenta
                        Color.FromArgb(255, 255, 190, 30)   // Gold
                    };
                case ColorTheme.CyberpunkRainbow:
                    return new Color[] {
                        FromHsv((timeSeconds * 90f) % 360f, 0.9f, 1.0f),
                        FromHsv((timeSeconds * 90f + 90f) % 360f, 0.9f, 1.0f),
                        FromHsv((timeSeconds * 90f + 180f) % 360f, 0.9f, 1.0f),
                        FromHsv((timeSeconds * 90f + 270f) % 360f, 0.9f, 1.0f)
                    };
                case ColorTheme.SunsetPro:
                    return new Color[] {
                        Color.FromArgb(255, 255, 69, 58),
                        Color.FromArgb(255, 255, 55, 95),
                        Color.FromArgb(255, 191, 90, 242),
                        Color.FromArgb(255, 255, 159, 10)
                    };
                case ColorTheme.UltraVioletNebula:
                    return new Color[] {
                        Color.FromArgb(255, 165, 60, 255),
                        Color.FromArgb(255, 94, 92, 230),
                        Color.FromArgb(255, 255, 60, 195),
                        Color.FromArgb(255, 70, 190, 255)
                    };
                case ColorTheme.MagmaGold:
                    return new Color[] {
                        Color.FromArgb(255, 255, 200, 20),
                        Color.FromArgb(255, 255, 105, 0),
                        Color.FromArgb(255, 255, 35, 35),
                        Color.FromArgb(255, 255, 235, 110)
                    };
                case ColorTheme.ArcticAurora:
                    return new Color[] {
                        Color.FromArgb(255, 0, 255, 160),
                        Color.FromArgb(255, 0, 205, 255),
                        Color.FromArgb(255, 80, 120, 255),
                        Color.FromArgb(255, 140, 255, 220)
                    };
                case ColorTheme.SakuraPastel:
                    return new Color[] {
                        Color.FromArgb(255, 255, 155, 200),
                        Color.FromArgb(255, 205, 155, 255),
                        Color.FromArgb(255, 255, 195, 160),
                        Color.FromArgb(255, 150, 220, 255)
                    };
                case ColorTheme.MinimalMonochrome:
                    return new Color[] {
                        Color.FromArgb(255, 245, 247, 255),
                        Color.FromArgb(255, 195, 205, 220),
                        Color.FromArgb(255, 150, 160, 175),
                        Color.FromArgb(255, 255, 255, 255)
                    };
                case ColorTheme.CustomColors:
                    Color c1 = Color.FromArgb(255, Color.FromArgb(settings.CustomColor1Argb));
                    Color c2 = Color.FromArgb(255, Color.FromArgb(settings.CustomColor2Argb));
                    Color mid = Interpolate(c1, c2, 0.5f);
                    return new Color[] { c1, c2, mid, Interpolate(c1, Color.White, 0.35f) };
                case ColorTheme.AppleIntelligence:
                default:
                    return new Color[] {
                        Color.FromArgb(255, 64, 205, 255),  // Siri Cyan
                        Color.FromArgb(255, 185, 80, 255),  // Intelligence Purple
                        Color.FromArgb(255, 255, 70, 160),  // Rose Magenta
                        Color.FromArgb(255, 255, 165, 60)   // Warm Amber
                    };
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
                if (clickThrough)
                {
                    cp.ExStyle |= NativeMethods.WS_EX_TRANSPARENT;
                }
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
    // CORNER OVERLAY MANAGER PER MONITOR (WITH CONTINUOUS ANIMATIONS!)
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
        public float PulseGlowAmount = 0f; // 0..1 during transition entrance

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
            int size,
            int radius,
            CornerPosition pos,
            AppSettings settings,
            float pulseGlow,
            Color[] themeColors,
            float animPhase = 0f,
            float audioPeak = 0f)
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

                // Determine dynamic color based on CornerAnimationMode
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

                // 1. Multi-layer soft neon glow along the corner curve
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

                // 2. Crisp Bezel Line
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

                // 3. Optional Sparkle Stars along the curve
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

                // 4. Pitch-black outer corner mask so the screen corner is physically rounded
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
                double cosT = Math.Cos(t);
                double sinT = Math.Sin(t);

                double u = Math.Pow(cosT, 2.0 / n) * radius;
                double v = Math.Pow(sinT, 2.0 / n) * radius;

                float px = centerX - dx * (float)u;
                float py = centerY - dy * (float)v;
                pts[i] = new PointF(px, py);
            }
            return pts;
        }

        private static GraphicsPath CreateCornerCurvePath(int size, int radius, CornerPosition pos, CurveType curveType)
        {
            GraphicsPath path = new GraphicsPath();
            PointF[] pts = GetCornerCurvePoints(size, radius, pos, curveType);
            path.AddLines(pts);
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
            {
                if (CornerForms[i] != null) CornerForms[i].EnsureTopMost();
            }
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
    // INTERACTIVE DYNAMIC ISLAND MUSIC PLAYER & LIVE HUB
    // =========================================================================
    public class DynamicIslandWidget : IDisposable
    {
        private LayeredOverlayForm islandForm;
        private float expandProgress = 0f; // 0 = Compact Pill, 1 = Expanded Music Hub
        private float transitionBoost = 0f; // Boost when switching screens
        private float currentScreenCenterX = -1f;
        private float currentScreenTopY = 8f;

        private Rectangle currentWindowRect = Rectangle.Empty;
        private int hoveredButtonIndex = -1; // 0=Prev, 1=PlayPause, 2=Next, 3=VolDown, 4=VolUp, 5=Spotify, 6=AppleMusic, 7=YouTube, 8=Theme, 9=Settings
        private bool wasMouseDown = false;
        private float buttonFlashEffect = 0f;
        private string transientBannerText = null;
        private long transientBannerUntilTicks = 0;

        public event Action OnOpenSettingsRequested;
        public event Action OnCycleThemeRequested;

        public DynamicIslandWidget()
        {
            islandForm = new LayeredOverlayForm(true);
            islandForm.Show();
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
            if (!settings.Enabled || settings.IslandMode == IslandVisibilityMode.Disabled)
            {
                islandForm.Hide();
                return;
            }

            if (transitionBoost > 0f)
            {
                transitionBoost = Math.Max(0f, transitionBoost - 0.025f);
            }

            bool shouldShow = true;
            if (settings.IslandMode == IslandVisibilityMode.OnlyOnTransition)
            {
                shouldShow = (transitionBoost > 0.02f);
            }
            else if (settings.IslandMode == IslandVisibilityMode.WhenMediaOrTransition)
            {
                shouldShow = (media.IsAudioActive || media.ActiveSource != MediaSourceType.None || transitionBoost > 0.02f);
            }

            if (!shouldShow)
            {
                islandForm.Hide();
                return;
            }

            Rectangle targetScreen = (settings.IslandPlacement == IslandPlacementMode.PrimaryScreenOnly)
                ? primaryScreenBounds
                : activeScreenBounds;

            float targetCenterX = targetScreen.Left + targetScreen.Width / 2f;
            float targetTopY = targetScreen.Top + 8f;

            if (currentScreenCenterX < -9000f || Math.Abs(currentScreenCenterX - targetCenterX) > 5000f)
            {
                currentScreenCenterX = targetCenterX;
                currentScreenTopY = targetTopY;
            }
            else
            {
                // Smooth glide when switching monitors!
                currentScreenCenterX = currentScreenCenterX * 0.78f + targetCenterX * 0.22f;
                currentScreenTopY = currentScreenTopY * 0.78f + targetTopY * 0.22f;
            }

            float scale = settings.IslandScalePercent / 100f;

            // Check if cursor is hovering over the Dynamic Island
            bool isHovering = false;
            if (settings.IslandExpandOnHover && currentWindowRect != Rectangle.Empty)
            {
                Rectangle hitBox = currentWindowRect;
                hitBox.Inflate(10, 8);
                isHovering = hitBox.Contains(cursorPos);
            }

            // Smooth spring interpolation for expandProgress (0..1)
            float targetExpand = isHovering ? 1.0f : 0.0f;
            expandProgress = expandProgress * 0.80f + targetExpand * 0.20f;
            if (Math.Abs(expandProgress - targetExpand) < 0.005f) expandProgress = targetExpand;

            if (buttonFlashEffect > 0f) buttonFlashEffect = Math.Max(0f, buttonFlashEffect - 0.08f);

            // Calculate dimensions from Compact Pill -> Expanded Media Hub
            float compactW = 290f * scale;
            float compactH = 38f * scale;
            float expandedW = 440f * scale;
            float expandedH = 166f * scale;

            float curPillW = (compactW + (expandedW - compactW) * expandProgress) * (1.0f + transitionBoost * 0.06f);
            float curPillH = (compactH + (expandedH - compactH) * expandProgress) * (1.0f + transitionBoost * 0.05f);

            int margin = 22;
            int winW = (int)curPillW + margin * 2;
            int winH = (int)curPillH + margin * 2;
            int winX = (int)(currentScreenCenterX - winW / 2f);
            int winY = (int)currentScreenTopY - margin / 2;

            currentWindowRect = new Rectangle(winX + margin, winY + margin / 2, (int)curPillW, (int)curPillH);

            // Handle interactive hover & mouse clicks when expanded!
            hoveredButtonIndex = -1;
            if (expandProgress > 0.65f && isHovering)
            {
                Point localPt = new Point(cursorPos.X - winX, cursorPos.Y - winY);
                HandleExpandedInteraction(localPt, margin, margin / 2, curPillW, curPillH, scale, media);
            }
            else
            {
                wasMouseDown = (NativeMethods.GetAsyncKeyState(0x01) & 0x8000) != 0;
            }

            // Render Dynamic Island Bitmap
            using (Bitmap bmp = new Bitmap(winW, winH, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.Clear(Color.Transparent);

                RectangleF pillRect = new RectangleF(margin, margin / 2f, curPillW, curPillH);
                float cornerRad = Math.Min(curPillH / 2f, (19f + 15f * expandProgress) * scale);

                Color accent1 = palette[0];
                Color accent2 = palette[1 % palette.Length];
                if (settings.IslandAutoAdaptBrandColor && media.ActiveSource != MediaSourceType.None)
                {
                    accent1 = ThemeColorEngine.GetMediaBrandColor(media.ActiveSource, palette[0]);
                }

                using (GraphicsPath pillPath = ScreenTransitionAnimator.CreateRoundedRectPath(pillRect, cornerRad))
                {
                    // 1. Animated Outer Glow / Border Halo
                    DrawIslandOuterGlow(g, pillRect, pillPath, settings.IslandBorder, accent1, accent2, palette, globalTime, media.CurrentAudioPeak, transitionBoost);

                    // 2. Deep Obsidian Glass Pill Body
                    using (SolidBrush bodyBrush = new SolidBrush(Color.FromArgb(248, 8, 9, 14)))
                    {
                        g.FillPath(bodyBrush, pillPath);
                    }

                    // 3. Subtle Inner Top Specular Highlight
                    using (LinearGradientBrush glassGrad = new LinearGradientBrush(
                        pillRect,
                        Color.FromArgb(180, accent1),
                        Color.FromArgb(140, accent2),
                        (globalTime * 45f) % 360f))
                    using (Pen rimPen = new Pen(glassGrad, 1.6f))
                    {
                        g.DrawPath(rimPen, pillPath);
                    }
                }

                // 4. Draw Compact Content vs Expanded Media Player Content
                if (expandProgress < 0.45f)
                {
                    float compactAlpha = 1.0f - (expandProgress / 0.45f);
                    DrawCompactIslandContent(g, pillRect, scale, compactAlpha, media, accent1, accent2, globalTime, settings.IslandShowClockWhenIdle);
                }
                else
                {
                    float expAlpha = (expandProgress - 0.45f) / 0.55f;
                    DrawExpandedIslandPlayer(g, pillRect, scale, expAlpha, media, accent1, accent2, palette, globalTime);
                }

                if (!islandForm.Visible) islandForm.Show();
                islandForm.SetBitmap(bmp, winX, winY, 255);
            }
        }

        private void DrawIslandOuterGlow(
            Graphics g,
            RectangleF pillRect,
            GraphicsPath pillPath,
            IslandBorderAnimation borderMode,
            Color accent1,
            Color accent2,
            Color[] palette,
            float globalTime,
            float audioPeak,
            float transBoost)
        {
            if (borderMode == IslandBorderAnimation.MinimalClean && transBoost <= 0.02f) return;

            float glowIntensity = 0.55f + transBoost * 0.45f;
            if (borderMode == IslandBorderAnimation.AudioReactiveHalo)
            {
                glowIntensity = 0.25f + audioPeak * 1.15f + transBoost * 0.4f;
            }
            else if (borderMode == IslandBorderAnimation.BreathingGlass)
            {
                glowIntensity = 0.25f + 0.35f * (float)(0.5 + 0.5 * Math.Sin(globalTime * 2.2f));
            }

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
                    RectangleF.Inflate(pillRect, 12, 12),
                    Color.FromArgb(alpha, cA),
                    Color.FromArgb(alpha, cB),
                    (globalTime * 65f) % 360f))
                using (Pen glowPen = new Pen(gb, layer * 3.0f))
                {
                    g.DrawPath(glowPen, pillPath);
                }
            }
        }

        private void DrawCompactIslandContent(
            Graphics g,
            RectangleF pill,
            float scale,
            float alphaF,
            LiveMediaAndAudioTracker media,
            Color accent1,
            Color accent2,
            float globalTime,
            bool showClock)
        {
            int alpha = (int)(Math.Max(0f, Math.Min(1f, alphaF)) * 255);
            if (alpha <= 5) return;

            // Left spinning Vinyl / Album Disc
            float discSize = pill.Height * 0.64f;
            float discX = pill.X + pill.Height * 0.20f;
            float discY = pill.Y + (pill.Height - discSize) / 2f;

            using (LinearGradientBrush discBrush = new LinearGradientBrush(
                new RectangleF(discX, discY, discSize, discSize),
                Color.FromArgb(alpha, accent1),
                Color.FromArgb(alpha, accent2),
                (globalTime * 120f) % 360f))
            {
                g.FillEllipse(discBrush, discX, discY, discSize, discSize);
            }
            // Inner vinyl hole
            float hole = discSize * 0.32f;
            using (SolidBrush centerHole = new SolidBrush(Color.FromArgb(alpha, 10, 11, 16)))
            {
                g.FillEllipse(centerHole, discX + (discSize - hole) / 2f, discY + (discSize - hole) / 2f, hole, hole);
            }

            // Center Text (Transient banner OR Track Title OR Clock)
            string displayTitle = media.TrackTitle;
            if (transientBannerText != null && DateTime.UtcNow.Ticks < transientBannerUntilTicks)
            {
                displayTitle = transientBannerText;
            }
            else if (media.ActiveSource == MediaSourceType.None && !media.IsAudioActive && showClock)
            {
                displayTitle = DateTime.Now.ToString("HH:mm") + "  •  iCorner Duo";
            }

            if (displayTitle.Length > 26) displayTitle = displayTitle.Substring(0, 25) + "…";

            using (Font f = new Font("Segoe UI Semibold", 8.8f * scale, FontStyle.Bold))
            using (SolidBrush tb = new SolidBrush(Color.FromArgb(alpha, 242, 245, 255)))
            {
                float textX = discX + discSize + 9f * scale;
                float textY = pill.Y + (pill.Height - 16f * scale) / 2f;
                g.DrawString(displayTitle, f, tb, textX, textY);
            }

            // Right Live 6-Bar Audio Waveform Visualizer
            int barCount = 6;
            float barW = 3.0f * scale;
            float barGap = 2.6f * scale;
            float totalWaveW = barCount * (barW + barGap);
            float waveStartX = pill.Right - totalWaveW - 12f * scale;
            float maxBarH = pill.Height * 0.56f;

            for (int i = 0; i < barCount; i++)
            {
                float level = media.SpectrumBars[i * 2];
                float bh = Math.Max(4f * scale, maxBarH * level);
                float bx = waveStartX + i * (barW + barGap);
                float by = pill.Y + (pill.Height - bh) / 2f;

                Color barCol = ThemeColorEngine.Interpolate(accent1, accent2, i / (float)(barCount - 1));
                using (SolidBrush bb = new SolidBrush(Color.FromArgb(alpha, barCol)))
                {
                    g.FillRectangle(bb, bx, by, barW, bh);
                }
            }
        }

        private void DrawExpandedIslandPlayer(
            Graphics g,
            RectangleF pill,
            float scale,
            float alphaF,
            LiveMediaAndAudioTracker media,
            Color accent1,
            Color accent2,
            Color[] palette,
            float globalTime)
        {
            int alpha = (int)(Math.Max(0f, Math.Min(1f, alphaF)) * 255);
            if (alpha <= 5) return;

            float pad = 16f * scale;

            // 1. Album Art / Rotating Disc on top-left
            float artSize = 48f * scale;
            RectangleF artRect = new RectangleF(pill.X + pad, pill.Y + 14f * scale, artSize, artSize);
            using (GraphicsPath artPath = ScreenTransitionAnimator.CreateRoundedRectPath(artRect, 12f * scale))
            using (LinearGradientBrush artBrush = new LinearGradientBrush(artRect, Color.FromArgb(alpha, accent1), Color.FromArgb(alpha, accent2), (globalTime * 80f) % 360f))
            {
                g.FillPath(artBrush, artPath);
            }
            // Music note icon inside album art
            using (Font iconFont = new Font("Segoe UI Symbol", 16f * scale, FontStyle.Bold))
            using (SolidBrush whiteBrush = new SolidBrush(Color.FromArgb(alpha, 255, 255, 255)))
            {
                g.DrawString("♫", iconFont, whiteBrush, artRect.X + 9f * scale, artRect.Y + 7f * scale);
            }

            // 2. Source Badge + Track Title + Artist
            string sourceBadge = GetSourceBadgeText(media.ActiveSource);
            float textLeft = artRect.Right + 12f * scale;
            float textMaxW = pill.Width - (textLeft - pill.X) - 115f * scale;

            using (Font badgeFont = new Font("Segoe UI Semibold", 7.5f * scale, FontStyle.Bold))
            using (Font titleFont = new Font("Segoe UI Semibold", 10.2f * scale, FontStyle.Bold))
            using (Font artistFont = new Font("Segoe UI", 8.5f * scale, FontStyle.Regular))
            using (SolidBrush badgeBrush = new SolidBrush(Color.FromArgb(alpha, accent1)))
            using (SolidBrush titleBrush = new SolidBrush(Color.FromArgb(alpha, 250, 252, 255)))
            using (SolidBrush artistBrush = new SolidBrush(Color.FromArgb((int)(alpha * 0.75f), 180, 190, 215)))
            {
                g.DrawString(sourceBadge, badgeFont, badgeBrush, textLeft, pill.Y + 13f * scale);

                string tTitle = media.TrackTitle;
                if (tTitle.Length > 28) tTitle = tTitle.Substring(0, 27) + "…";
                g.DrawString(tTitle, titleFont, titleBrush, textLeft, pill.Y + 27f * scale);

                string tArtist = media.TrackArtist;
                if (tArtist.Length > 36) tArtist = tArtist.Substring(0, 35) + "…";
                g.DrawString(tArtist, artistFont, artistBrush, textLeft, pill.Y + 46f * scale);
            }

            // 3. Top-Right 12-Bar Live Spectrum Visualizer
            int specCount = 12;
            float sbW = 3.2f * scale;
            float sbGap = 2.5f * scale;
            float specRightX = pill.Right - pad - specCount * (sbW + sbGap);
            float specMaxH = 34f * scale;
            float specCenterY = pill.Y + 38f * scale;

            for (int i = 0; i < specCount; i++)
            {
                float lv = media.SpectrumBars[i];
                float bh = Math.Max(4f * scale, specMaxH * lv);
                Color bc = ThemeColorEngine.Interpolate(accent1, accent2, i / (float)(specCount - 1));
                using (SolidBrush b = new SolidBrush(Color.FromArgb(alpha, bc)))
                {
                    g.FillRectangle(b, specRightX + i * (sbW + sbGap), specCenterY - bh / 2f, sbW, bh);
                }
            }

            // 4. Interactive Transport Control Row (Prev, Play/Pause, Next, Vol-, Vol+)
            RectangleF[] btnRects = GetExpandedButtonRects(pill.X, pill.Y, pill.Width, pill.Height, scale);
            string[] btnLabels = new string[] { "⏮", "⏯", "⏭", "🔉", "🔊" };

            using (Font btnFont = new Font("Segoe UI Symbol", 11.5f * scale, FontStyle.Bold))
            {
                for (int i = 0; i < 5; i++)
                {
                    bool hovered = (hoveredButtonIndex == i);
                    RectangleF br = btnRects[i];
                    using (GraphicsPath bp = ScreenTransitionAnimator.CreateRoundedRectPath(br, br.Height / 2f))
                    {
                        Color bgCol = (i == 1)
                            ? (hovered ? accent1 : Color.FromArgb(alpha, 45, 52, 74))
                            : (hovered ? Color.FromArgb(alpha, 55, 62, 88) : Color.FromArgb(alpha, 24, 27, 38));

                        using (SolidBrush bb = new SolidBrush(bgCol))
                        {
                            g.FillPath(bb, bp);
                        }
                        if (hovered)
                        {
                            using (Pen hp = new Pen(Color.FromArgb(alpha, accent1), 1.5f))
                            {
                                g.DrawPath(hp, bp);
                            }
                        }
                    }

                    SizeF sz = g.MeasureString(btnLabels[i], btnFont);
                    using (SolidBrush fb = new SolidBrush(Color.FromArgb(alpha, Color.White)))
                    {
                        g.DrawString(btnLabels[i], btnFont, fb, br.X + (br.Width - sz.Width) / 2f, br.Y + (br.Height - sz.Height) / 2f + 1f);
                    }
                }
            }

            // 5. Bottom Quick-Launch / Switch Bar (Spotify, Apple Music, YouTube, Theme, Settings)
            string[] chipLabels = new string[] { "🟢 Spotify", "🔴 Apple Music", "▶️ YouTube", "🎨 Thème", "⚙️" };
            using (Font chipFont = new Font("Segoe UI Semibold", 7.8f * scale, FontStyle.Bold))
            {
                for (int i = 5; i < 10; i++)
                {
                    bool hovered = (hoveredButtonIndex == i);
                    RectangleF cr = btnRects[i];
                    using (GraphicsPath cp = ScreenTransitionAnimator.CreateRoundedRectPath(cr, cr.Height / 2f))
                    {
                        Color chipBg = hovered
                            ? Color.FromArgb(alpha, 58, 66, 94)
                            : Color.FromArgb(alpha, 20, 22, 32);
                        using (SolidBrush cb = new SolidBrush(chipBg))
                        {
                            g.FillPath(cb, cp);
                        }
                        using (Pen borderP = new Pen(hovered ? Color.FromArgb(alpha, accent1) : Color.FromArgb(alpha / 3, 120, 130, 160), 1f))
                        {
                            g.DrawPath(borderP, cp);
                        }
                    }

                    string lbl = chipLabels[i - 5];
                    SizeF sz = g.MeasureString(lbl, chipFont);
                    using (SolidBrush tb = new SolidBrush(Color.FromArgb(alpha, 235, 240, 255)))
                    {
                        g.DrawString(lbl, chipFont, tb, cr.X + (cr.Width - sz.Width) / 2f, cr.Y + (cr.Height - sz.Height) / 2f);
                    }
                }
            }
        }

        private RectangleF[] GetExpandedButtonRects(float px, float py, float pw, float ph, float scale)
        {
            RectangleF[] rects = new RectangleF[10];

            // Row 1: Transport Controls (centered at Y = py + 74 * scale)
            float row1Y = py + 72f * scale;
            float btnH = 34f * scale;
            float smallW = 46f * scale;
            float playW = 64f * scale;
            float gap = 10f * scale;
            float totalRow1 = smallW * 4 + playW + gap * 4;
            float startX = px + (pw - totalRow1) / 2f;

            rects[0] = new RectangleF(startX, row1Y, smallW, btnH); // Prev
            rects[1] = new RectangleF(startX + smallW + gap, row1Y, playW, btnH); // Play/Pause
            rects[2] = new RectangleF(startX + smallW + playW + gap * 2, row1Y, smallW, btnH); // Next
            rects[3] = new RectangleF(startX + smallW * 2 + playW + gap * 3, row1Y, smallW, btnH); // Vol-
            rects[4] = new RectangleF(startX + smallW * 3 + playW + gap * 4, row1Y, smallW, btnH); // Vol+

            // Row 2: Quick Service Chips (Spotify, Apple Music, YouTube, Theme, Settings)
            float row2Y = py + 118f * scale;
            float chipH = 26f * scale;
            float cGap = 6f * scale;
            float wSpot = 78f * scale;
            float wApple = 98f * scale;
            float wYT = 82f * scale;
            float wTheme = 72f * scale;
            float wSet = 36f * scale;
            float totalRow2 = wSpot + wApple + wYT + wTheme + wSet + cGap * 4;
            float cx = px + (pw - totalRow2) / 2f;

            rects[5] = new RectangleF(cx, row2Y, wSpot, chipH); cx += wSpot + cGap;
            rects[6] = new RectangleF(cx, row2Y, wApple, chipH); cx += wApple + cGap;
            rects[7] = new RectangleF(cx, row2Y, wYT, chipH); cx += wYT + cGap;
            rects[8] = new RectangleF(cx, row2Y, wTheme, chipH); cx += wTheme + cGap;
            rects[9] = new RectangleF(cx, row2Y, wSet, chipH);

            return rects;
        }

        private void HandleExpandedInteraction(Point localPt, float px, float py, float pw, float ph, float scale, LiveMediaAndAudioTracker media)
        {
            RectangleF[] rects = GetExpandedButtonRects(px, py, pw, ph, scale);
            for (int i = 0; i < rects.Length; i++)
            {
                if (rects[i].Contains(localPt.X, localPt.Y))
                {
                    hoveredButtonIndex = i;
                    break;
                }
            }

            bool isMouseDown = (NativeMethods.GetAsyncKeyState(0x01) & 0x8000) != 0;
            if (isMouseDown && !wasMouseDown && hoveredButtonIndex >= 0)
            {
                buttonFlashEffect = 1.0f;
                switch (hoveredButtonIndex)
                {
                    case 0:
                        NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PREV_TRACK);
                        ShowTransientBanner("⏮ Piste précédente", 1200);
                        break;
                    case 1:
                        NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PLAY_PAUSE);
                        ShowTransientBanner("⏯ Lecture / Pause", 1200);
                        break;
                    case 2:
                        NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_NEXT_TRACK);
                        ShowTransientBanner("⏭ Piste suivante", 1200);
                        break;
                    case 3:
                        NativeMethods.SendMediaKey(NativeMethods.VK_VOLUME_DOWN);
                        NativeMethods.SendMediaKey(NativeMethods.VK_VOLUME_DOWN);
                        ShowTransientBanner("🔉 Volume -", 900);
                        break;
                    case 4:
                        NativeMethods.SendMediaKey(NativeMethods.VK_VOLUME_UP);
                        NativeMethods.SendMediaKey(NativeMethods.VK_VOLUME_UP);
                        ShowTransientBanner("🔊 Volume +", 900);
                        break;
                    case 5:
                        media.LaunchOrFocusService("spotify");
                        break;
                    case 6:
                        media.LaunchOrFocusService("applemusic");
                        break;
                    case 7:
                        media.LaunchOrFocusService("youtube");
                        break;
                    case 8:
                        if (OnCycleThemeRequested != null) OnCycleThemeRequested();
                        break;
                    case 9:
                        if (OnOpenSettingsRequested != null) OnOpenSettingsRequested();
                        break;
                }
            }
            wasMouseDown = isMouseDown;
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
                default: return " DYNAMIC ISLAND HUB";
            }
        }

        public void EnsureTopMost()
        {
            if (islandForm != null) islandForm.EnsureTopMost();
        }

        public void Dispose()
        {
            if (islandForm != null)
            {
                islandForm.Close();
                islandForm.Dispose();
            }
        }
    }

    // =========================================================================
    // MULTI-SCREEN TRANSITION ANIMATOR (8 APPLE & NEON TRANSITION STYLES)
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

        public void TriggerTransition(
            Rectangle fromBounds,
            Rectangle toBounds,
            Point cursorPos,
            MonitorCorners targetCorners,
            AppSettings settings)
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

        public void UpdateProximity(
            Rectangle currentBounds,
            ScreenEdge nearEdge,
            Point cursorPos,
            float proximityFactor,
            AppSettings settings,
            Color[] palette)
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

            RenderEdgePortal(
                entryEdgeForm,
                currentBounds,
                nearEdge,
                cursorPos,
                proximityFactor * 0.42f,
                proximityFactor * 0.78f,
                scale,
                settings.Style,
                palette,
                true,
                0f);
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
                if (TargetMonitorCorners != null)
                {
                    TargetMonitorCorners.PulseGlowAmount = 0f;
                }
                return;
            }

            float scale = settings.IntensityPercent / 100f;

            // 1. Corner Pulse on entered monitor
            if (TargetMonitorCorners != null)
            {
                float cornerPulse = (float)(Math.Sin(Math.Min(1.0, t * 1.25) * Math.PI));
                TargetMonitorCorners.PulseGlowAmount = Math.Max(0f, cornerPulse);
            }

            // 2. Render Entry & Exit Portals
            if (settings.Style != TransitionStyle.DynamicIslandBounce)
            {
                float entryEnvelope = (t < 0.22f)
                    ? EaseOutBack(t / 0.22f)
                    : 1.0f - EaseInOutCubic((t - 0.22f) / 0.78f);

                float entryAlpha = (t < 0.14f) ? (t / 0.14f) : (1.0f - (t - 0.14f) / 0.86f);

                RenderEdgePortal(
                    entryEdgeForm,
                    entryScreenBounds,
                    entryEdge,
                    crossingPoint,
                    entryEnvelope,
                    Math.Max(0f, Math.Min(1f, entryAlpha)),
                    scale,
                    settings.Style,
                    palette,
                    false,
                    t);

                if (t < 0.46f && exitScreenBounds != entryScreenBounds)
                {
                    float exitT = t / 0.46f;
                    float exitEnvelope = (1.0f - EaseOutCubic(exitT)) * 0.65f;
                    float exitAlpha = (1.0f - exitT) * 0.85f;
                    RenderEdgePortal(
                        exitEdgeForm,
                        exitScreenBounds,
                        exitEdge,
                        crossingPoint,
                        exitEnvelope,
                        exitAlpha,
                        scale,
                        settings.Style,
                        palette,
                        true,
                        t);
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

        private void RenderEdgePortal(
            LayeredOverlayForm form,
            Rectangle screenBounds,
            ScreenEdge edge,
            Point cursor,
            float bulgeAmount,
            float alphaFactor,
            float scale,
            TransitionStyle style,
            Color[] palette,
            bool isExitSide,
            float rawTime)
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

                // 1. Chromatic Wave (for Combo, Siri Glow, Liquid Bridge, Plasma, Particle Burst)
                if (style == TransitionStyle.ComboUltimate ||
                    style == TransitionStyle.SiriIntelligenceGlow ||
                    style == TransitionStyle.LiquidBridge ||
                    style == TransitionStyle.ElectricPlasmaPortal ||
                    style == TransitionStyle.CosmicParticleBurst)
                {
                    DrawChromaticEdgeWave(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor, scale, palette, rawTime);
                }

                // 2. Liquid Metaball Bridge
                if (style == TransitionStyle.ComboUltimate ||
                    style == TransitionStyle.LiquidBridge)
                {
                    DrawLiquidBridgeMetaball(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor, scale, palette, isExitSide, rawTime);
                }

                // 3. Cosmic Particles & Stars Burst
                if (!isExitSide && (style == TransitionStyle.ComboUltimate || style == TransitionStyle.CosmicParticleBurst))
                {
                    DrawCosmicParticles(g, winW, winH, edge, centerAlong, alphaFactor, scale, palette, rawTime);
                }

                // 4. Electric Plasma Lightning Arcs
                if (style == TransitionStyle.ElectricPlasmaPortal)
                {
                    DrawPlasmaLightning(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor, scale, palette, rawTime);
                }

                // 5. Sonar Shockwave Ripples
                if (style == TransitionStyle.SonarShockwave)
                {
                    DrawSonarShockwave(g, winW, winH, edge, centerAlong, alphaFactor, scale, palette, rawTime);
                }

                // 6. Neon Laser Blade
                if (style == TransitionStyle.NeonLaserBlade)
                {
                    DrawNeonLaserBlade(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor, scale, palette);
                }

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
            int particleCount = 16;
            for (int i = 0; i < particleCount; i++)
            {
                float seed = (i * 1.37f) + 0.5f;
                float speed = 0.65f + (i % 5) * 0.22f;
                float pTime = (rawTime * speed);
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
                {
                    g.FillEllipse(glowB, pt.X - pRadius * 1.8f, pt.Y - pRadius * 1.8f, pRadius * 3.6f, pRadius * 3.6f);
                }
                using (SolidBrush coreB = new SolidBrush(Color.FromArgb(pAlpha, Color.White)))
                {
                    g.FillEllipse(coreB, pt.X - pRadius * 0.7f, pt.Y - pRadius * 0.7f, pRadius * 1.4f, pRadius * 1.4f);
                }
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
                Color c = palette[ring % palette.Length];
                using (Pen rp = new Pen(Color.FromArgb(rAlpha, c), (4.5f - 3f * rt) * scale))
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
            if (d <= 1f)
            {
                path.AddRectangle(rect);
                return path;
            }
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

        private static float EaseOutCubic(float x)
        {
            float f = 1f - x;
            return 1f - f * f * f;
        }

        private static float EaseInOutCubic(float x)
        {
            return x < 0.5f ? 4f * x * x * x : 1f - (float)Math.Pow(-2f * x + 2f, 3) / 2f;
        }

        private static float EaseOutBack(float x)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;
            return 1f + c3 * (float)Math.Pow(x - 1f, 3) + c1 * (float)Math.Pow(x - 1f, 2);
        }

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

        private Timer tickTimer;
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

            tickTimer = new Timer();
            tickTimer.Interval = 16; // ~60 FPS
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
            Color[] palette = ThemeColorEngine.GetPalette(Settings, GlobalTimeSeconds, MediaTracker.ActiveSource);
            for (int i = 0; i < screens.Length; i++)
            {
                MonitorCorners mc = new MonitorCorners(screens[i]);
                mc.UpdateCorners(Settings, palette, GlobalTimeSeconds, MediaTracker.CurrentAudioPeak);
                Monitors.Add(mc);
            }

            NativeMethods.POINT pt;
            if (NativeMethods.GetCursorPos(out pt))
            {
                lastMonitorIndex = FindMonitorIndex(new Point(pt.X, pt.Y));
            }
        }

        public void RefreshAllCorners()
        {
            float t = GlobalTimeSeconds;
            Color[] palette = ThemeColorEngine.GetPalette(Settings, t, MediaTracker.ActiveSource);
            foreach (var m in Monitors)
            {
                m.UpdateCorners(Settings, palette, t, MediaTracker.CurrentAudioPeak);
            }
            Settings.Save();
        }

        private int FindMonitorIndex(Point pt)
        {
            for (int i = 0; i < Monitors.Count; i++)
            {
                if (Monitors[i].Bounds.Contains(pt)) return i;
            }
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
            Color[] palette = ThemeColorEngine.GetPalette(Settings, t, MediaTracker.ActiveSource);

            Point cursorPt = Point.Empty;
            NativeMethods.POINT rawPt;
            if (NativeMethods.GetCursorPos(out rawPt))
            {
                cursorPt = new Point(rawPt.X, rawPt.Y);
            }

            int currentIdx = (Monitors.Count > 0) ? FindMonitorIndex(cursorPt) : -1;

            // 1. Check screen crossing
            if (Monitors.Count > 0 && currentIdx >= 0)
            {
                if (lastMonitorIndex != -1 && currentIdx != lastMonitorIndex && lastMonitorIndex < Monitors.Count)
                {
                    Animator.TriggerTransition(
                        Monitors[lastMonitorIndex].Bounds,
                        Monitors[currentIdx].Bounds,
                        cursorPt,
                        Monitors[currentIdx],
                        Settings);

                    Rectangle tb = Monitors[currentIdx].Bounds;
                    DynamicIsland.TriggerTransitionNotification("Écran " + (currentIdx + 1), tb.Width + "×" + tb.Height);
                }
                else if (Monitors.Count > 1)
                {
                    CheckMagneticProximity(currentIdx, cursorPt, palette);
                }
                lastMonitorIndex = currentIdx;
            }

            // 2. Step inter-screen transition animation
            Animator.StepAnimation(Settings, palette);

            // 3. Update animated corners (at ~30 FPS when continuous corner animation is active, or 60 FPS during transition)
            cornerAnimTickCounter++;
            bool needsContinuousCornerUpdate = (Settings.CornerAnim != CornerAnimationMode.Static || Settings.Theme == ColorTheme.CyberpunkRainbow);
            if (Animator.IsBusy || (needsContinuousCornerUpdate && cornerAnimTickCounter % 2 == 0))
            {
                float phase = t * (Settings.CornerAnimSpeed / 100f);
                foreach (var m in Monitors)
                {
                    m.UpdateCorners(Settings, palette, phase, MediaTracker.CurrentAudioPeak);
                }
            }

            // 4. Update Interactive Dynamic Island
            if (Monitors.Count > 0)
            {
                Rectangle activeBounds = (currentIdx >= 0 && currentIdx < Monitors.Count) ? Monitors[currentIdx].Bounds : Screen.PrimaryScreen.Bounds;
                Rectangle primaryBounds = Screen.PrimaryScreen.Bounds;
                DynamicIsland.UpdateAndRender(Settings, activeBounds, primaryBounds, cursorPt, MediaTracker, palette, t);
            }

            // 5. Periodically re-assert TOPMOST
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
            {
                targetIdx = FindMonitorIndex(new Point(rawPt.X, rawPt.Y));
            }
            int fromIdx = (Monitors.Count > 1) ? ((targetIdx + 1) % Monitors.Count) : targetIdx;

            Rectangle toB = Monitors[targetIdx].Bounds;
            Rectangle fromB = Monitors[fromIdx].Bounds;

            Point demoPoint = new Point(toB.Left + 4, toB.Top + toB.Height / 2);
            if (Monitors.Count > 1 && fromB.Left > toB.Left)
            {
                demoPoint = new Point(toB.Right - 4, toB.Top + toB.Height / 2);
            }

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
            trayIcon.Text = "iCorner Duo — Bords Arrondis & Dynamic Island";
            trayIcon.Icon = LoadAppIcon() ?? SystemIcons.Application;
            trayIcon.Visible = true;

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem openItem = new ToolStripMenuItem("⚙ Ouvrir iCorner Duo Studio");
            openItem.Font = new Font(openItem.Font, FontStyle.Bold);
            openItem.Click += (s, e) => ShowSettingsWindow();

            ToolStripMenuItem demoItem = new ToolStripMenuItem("✨ Tester l'animation de transition");
            demoItem.Click += (s, e) => TriggerDemoTransition();

            ToolStripMenuItem playPauseItem = new ToolStripMenuItem("⏯ Lecture / Pause Musique");
            playPauseItem.Click += (s, e) => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PLAY_PAUSE);

            ToolStripMenuItem nextTrackItem = new ToolStripMenuItem("⏭ Piste suivante");
            nextTrackItem.Click += (s, e) => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_NEXT_TRACK);

            ToolStripMenuItem toggleItem = new ToolStripMenuItem(Settings.Enabled ? "✅ Actif (Cliquer pour suspendre)" : "⏸ Suspendu (Cliquer pour activer)");
            toggleItem.Click += (s, e) =>
            {
                Settings.Enabled = !Settings.Enabled;
                toggleItem.Text = Settings.Enabled ? "✅ Actif (Cliquer pour suspendre)" : "⏸ Suspendu (Cliquer pour activer)";
                RefreshAllCorners();
            };

            ToolStripMenuItem exitItem = new ToolStripMenuItem("❌ Quitter iCorner Duo");
            exitItem.Click += (s, e) =>
            {
                trayIcon.Visible = false;
                Application.Exit();
            };

            menu.Items.Add(openItem);
            menu.Items.Add(demoItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(playPauseItem);
            menu.Items.Add(nextTrackItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(toggleItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(exitItem);

            trayIcon.ContextMenuStrip = menu;
            trayIcon.DoubleClick += (s, e) => ShowSettingsWindow();
        }

        public void ShowSettingsWindow()
        {
            if (settingsForm == null || settingsForm.IsDisposed)
            {
                settingsForm = new SettingsForm(this);
            }
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
    // SETTINGS STUDIO WINDOW (3 FULL CARDS: CORNERS, TRANSITIONS & COLORS, DYNAMIC ISLAND)
    // =========================================================================
    public class SettingsForm : Form
    {
        private OverlayController controller;
        private Label radiusValueLabel;
        private Label glowValueLabel;
        private Label thicknessValueLabel;
        private Label durationValueLabel;
        private Label intensityValueLabel;
        private Label islandSizeValueLabel;
        private Label monitorsInfoLabel;
        private Panel previewBox;
        private ComboBox themeCombo;
        private Button customCol1Btn;
        private Button customCol2Btn;

        public SettingsForm(OverlayController ctrl)
        {
            controller = ctrl;
            Text = "iCorner Duo Studio — Bords Arrondis, Transitions & Dynamic Island Music";
            Size = new Size(1120, 770);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(13, 14, 20);
            ForeColor = Color.FromArgb(240, 242, 250);
            Font = new Font("Segoe UI", 9.2f, FontStyle.Regular);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            Icon appIco = OverlayController.LoadAppIcon();
            if (appIco != null) Icon = appIco;

            BuildUI();

            controller.OnMonitorsChanged += UpdateMonitorSummary;
            controller.OnSettingsUpdatedExternally += () =>
            {
                if (themeCombo != null && (int)controller.Settings.Theme < themeCombo.Items.Count)
                    themeCombo.SelectedIndex = (int)controller.Settings.Theme;
                if (previewBox != null) previewBox.Invalidate();
            };
        }

        private void BuildUI()
        {
            // Top Header Banner
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 84,
                BackColor = Color.FromArgb(20, 22, 32)
            };
            header.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (LinearGradientBrush grad = new LinearGradientBrush(
                    header.ClientRectangle,
                    Color.FromArgb(55, 64, 200, 255),
                    Color.FromArgb(55, 220, 65, 185),
                    12f))
                {
                    e.Graphics.FillRectangle(grad, header.ClientRectangle);
                }
                using (Pen bottomLine = new Pen(Color.FromArgb(75, 110, 160, 255), 1f))
                {
                    e.Graphics.DrawLine(bottomLine, 0, header.Height - 1, header.Width, header.Height - 1);
                }
                if (Icon != null)
                {
                    e.Graphics.DrawIcon(Icon, new Rectangle(20, 16, 52, 52));
                }
            };

            Label titleLbl = new Label
            {
                Text = "iCorner Duo Studio  •  Pro Edition",
                Font = new Font("Segoe UI Semibold", 16.5f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(84, 14)
            };

            Label subLbl = new Label
            {
                Text = "Bords arrondis animés • Transitions inter-écrans Apple Duo • Dynamic Island Music Player (Spotify, Apple Music, YouTube)",
                Font = new Font("Segoe UI", 9.2f, FontStyle.Regular),
                ForeColor = Color.FromArgb(185, 200, 230),
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(86, 48)
            };

            Button testBtn = new Button
            {
                Text = "✨ Tester la transition",
                Size = new Size(195, 42),
                Location = new Point(885, 21),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            testBtn.FlatAppearance.BorderSize = 0;
            testBtn.Click += (s, e) => controller.TriggerDemoTransition();

            header.Controls.Add(titleLbl);
            header.Controls.Add(subLbl);
            header.Controls.Add(testBtn);
            Controls.Add(header);

            // =================================================================
            // COLUMN 1: BORDS ARRONDIS & ANIMATIONS DES ANGLES
            // =================================================================
            Panel col1 = CreateCard(18, 100, 348, 615, "📐 1. Bords Arrondis & Angles Animés");
            Controls.Add(col1);

            CheckBox enableCornersChk = CreateCheckBox("Activer les bords arrondis et overlays", 16, 42, controller.Settings.Enabled);
            enableCornersChk.CheckedChanged += (s, e) =>
            {
                controller.Settings.Enabled = enableCornersChk.Checked;
                controller.RefreshAllCorners();
                previewBox.Invalidate();
            };
            col1.Controls.Add(enableCornersChk);

            // Radius Slider
            col1.Controls.Add(CreateLabel("Rayon de l'arrondi :", 16, 74));
            radiusValueLabel = CreateValueLabel(controller.Settings.CornerRadius + " px", 275, 74);
            col1.Controls.Add(radiusValueLabel);

            TrackBar radiusSlider = CreateSlider(12, 94, 322, 0, 110, controller.Settings.CornerRadius);
            radiusSlider.ValueChanged += (s, e) =>
            {
                controller.Settings.CornerRadius = radiusSlider.Value;
                radiusValueLabel.Text = radiusSlider.Value + " px";
                controller.RefreshAllCorners();
                previewBox.Invalidate();
            };
            col1.Controls.Add(radiusSlider);

            // Curve Type
            col1.Controls.Add(CreateLabel("Géométrie de la courbe :", 16, 130));
            ComboBox curveCombo = CreateComboBox(16, 152, 314, new string[] {
                " Squircle Apple (Courbure continue iOS/macOS)",
                "◯ Cercle Classique (Arc circulaire standard)"
            }, (int)controller.Settings.Curve);
            curveCombo.SelectedIndexChanged += (s, e) =>
            {
                controller.Settings.Curve = (CurveType)curveCombo.SelectedIndex;
                controller.RefreshAllCorners();
                previewBox.Invalidate();
            };
            col1.Controls.Add(curveCombo);

            // Continuous Corner Animation Mode
            col1.Controls.Add(CreateLabel("Animation continue des 4 angles :", 16, 188));
            ComboBox cornerAnimCombo = CreateComboBox(16, 210, 314, new string[] {
                "✨ Fixe élégant (Classique)",
                "🌈 Rotation Chromatique 360° (Apple Glow)",
                "💨 Respiration Lumineuse Douce (Breathing)",
                "🎵 Réactif à la Musique / Basses (Audio Beat Sync)",
                "⭐ Scintillement d'Étoiles (Sparkle Shimmer)",
                "⚡ Onde Néon Tournante (Neon Chase)"
            }, (int)controller.Settings.CornerAnim);
            cornerAnimCombo.SelectedIndexChanged += (s, e) =>
            {
                controller.Settings.CornerAnim = (CornerAnimationMode)cornerAnimCombo.SelectedIndex;
                controller.RefreshAllCorners();
                previewBox.Invalidate();
            };
            col1.Controls.Add(cornerAnimCombo);

            // Active Corners Checkboxes
            CheckBox tlChk = CreateCheckBox("Haut-Gauche", 16, 248, controller.Settings.TopLeft);
            CheckBox trChk = CreateCheckBox("Haut-Droit", 175, 248, controller.Settings.TopRight);
            CheckBox blChk = CreateCheckBox("Bas-Gauche", 16, 274, controller.Settings.BottomLeft);
            CheckBox brChk = CreateCheckBox("Bas-Droit", 175, 274, controller.Settings.BottomRight);
            EventHandler cornerCheckHandler = (s, e) =>
            {
                controller.Settings.TopLeft = tlChk.Checked;
                controller.Settings.TopRight = trChk.Checked;
                controller.Settings.BottomLeft = blChk.Checked;
                controller.Settings.BottomRight = brChk.Checked;
                controller.RefreshAllCorners();
                previewBox.Invalidate();
            };
            tlChk.CheckedChanged += cornerCheckHandler;
            trChk.CheckedChanged += cornerCheckHandler;
            blChk.CheckedChanged += cornerCheckHandler;
            brChk.CheckedChanged += cornerCheckHandler;
            col1.Controls.Add(tlChk);
            col1.Controls.Add(trChk);
            col1.Controls.Add(blChk);
            col1.Controls.Add(brChk);

            // Bezel Glow & Thickness
            CheckBox bezelChk = CreateCheckBox("Activer le liseré lumineux (Bezel Glow)", 16, 306, controller.Settings.CornerBezelGlow);
            bezelChk.CheckedChanged += (s, e) =>
            {
                controller.Settings.CornerBezelGlow = bezelChk.Checked;
                controller.RefreshAllCorners();
                previewBox.Invalidate();
            };
            col1.Controls.Add(bezelChk);

            col1.Controls.Add(CreateLabel("Intensité lumineuse du liseré :", 16, 334));
            glowValueLabel = CreateValueLabel(controller.Settings.BezelGlowOpacity + " %", 275, 334);
            col1.Controls.Add(glowValueLabel);

            TrackBar glowSlider = CreateSlider(12, 354, 322, 0, 100, controller.Settings.BezelGlowOpacity);
            glowSlider.ValueChanged += (s, e) =>
            {
                controller.Settings.BezelGlowOpacity = glowSlider.Value;
                glowValueLabel.Text = glowSlider.Value + " %";
                controller.RefreshAllCorners();
                previewBox.Invalidate();
            };
            col1.Controls.Add(glowSlider);

            col1.Controls.Add(CreateLabel("Épaisseur du trait :", 16, 390));
            thicknessValueLabel = CreateValueLabel((controller.Settings.BezelThickness / 10.0).ToString("0.0") + " px", 275, 390);
            col1.Controls.Add(thicknessValueLabel);

            TrackBar thickSlider = CreateSlider(12, 410, 322, 10, 75, controller.Settings.BezelThickness);
            thickSlider.ValueChanged += (s, e) =>
            {
                controller.Settings.BezelThickness = thickSlider.Value;
                thicknessValueLabel.Text = (thickSlider.Value / 10.0).ToString("0.0") + " px";
                controller.RefreshAllCorners();
                previewBox.Invalidate();
            };
            col1.Controls.Add(thickSlider);

            // Live Preview Box
            previewBox = new Panel
            {
                Location = new Point(16, 450),
                Size = new Size(314, 72),
                BackColor = Color.FromArgb(30, 34, 50)
            };
            previewBox.Paint += PaintPreviewBox;
            col1.Controls.Add(previewBox);

            // Detected Monitors Summary
            monitorsInfoLabel = new Label
            {
                Location = new Point(16, 532),
                Size = new Size(218, 72),
                Font = new Font("Segoe UI", 8.4f, FontStyle.Regular),
                ForeColor = Color.FromArgb(160, 210, 255)
            };
            UpdateMonitorSummary();
            col1.Controls.Add(monitorsInfoLabel);

            Button refreshMonBtn = new Button
            {
                Text = "🔄 Écrans",
                Location = new Point(240, 544),
                Size = new Size(90, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(40, 45, 66),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            refreshMonBtn.FlatAppearance.BorderSize = 0;
            refreshMonBtn.Click += (s, e) =>
            {
                controller.RebuildMonitors();
                UpdateMonitorSummary();
            };
            col1.Controls.Add(refreshMonBtn);

            // =================================================================
            // COLUMN 2: TRANSITIONS INTER-ÉCRANS & PALETTES DE COULEURS
            // =================================================================
            Panel col2 = CreateCard(380, 100, 348, 615, "🌊 2. Transitions & Couleurs (13 Thèmes)");
            Controls.Add(col2);

            CheckBox enableTransChk = CreateCheckBox("Activer la transition au changement d'écran", 16, 42, controller.Settings.EnableTransition);
            enableTransChk.CheckedChanged += (s, e) =>
            {
                controller.Settings.EnableTransition = enableTransChk.Checked;
                controller.Settings.Save();
            };
            col2.Controls.Add(enableTransChk);

            CheckBox magProxChk = CreateCheckBox("Effet magnétique à l'approche du bord d'écran", 16, 70, controller.Settings.EnableMagneticProximity);
            magProxChk.CheckedChanged += (s, e) =>
            {
                controller.Settings.EnableMagneticProximity = magProxChk.Checked;
                controller.Settings.Save();
            };
            col2.Controls.Add(magProxChk);

            // 8 Transition Styles
            col2.Controls.Add(CreateLabel("Style d'animation de passage d'écran (8 styles) :", 16, 106));
            ComboBox styleCombo = CreateComboBox(16, 128, 314, new string[] {
                "✨ Combo Ultime Pro (Pont Liquide + Siri + Étoiles)",
                "💧 Apple Liquid Bridge (Pont métaballe liquide)",
                "🌈 Apple Intelligence (Onde chromatique Siri)",
                "⚡ Portail Plasma & Éclairs Électriques",
                "🪐 Explosion d'Étoiles & Particules Cosmiques",
                "🌊 Onde de Choc Sonar (Anneaux concentriques)",
                "🔥 Lame Laser Néon (Faisceau haute intensité)",
                "💊 Dynamic Island Bounce (Pilule & Rebond Angles)"
            }, (int)controller.Settings.Style);
            styleCombo.SelectedIndexChanged += (s, e) =>
            {
                controller.Settings.Style = (TransitionStyle)styleCombo.SelectedIndex;
                controller.Settings.Save();
                controller.TriggerDemoTransition();
            };
            col2.Controls.Add(styleCombo);

            // 13 Color Themes
            col2.Controls.Add(CreateLabel("Palette de couleurs globale (13 thèmes) :", 16, 168));
            themeCombo = CreateComboBox(16, 190, 314, new string[] {
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
            themeCombo.SelectedIndexChanged += (s, e) =>
            {
                controller.Settings.Theme = (ColorTheme)themeCombo.SelectedIndex;
                controller.RefreshAllCorners();
                previewBox.Invalidate();
                controller.TriggerDemoTransition();
            };
            col2.Controls.Add(themeCombo);

            // Custom Color Picker Buttons
            col2.Controls.Add(CreateLabel("Couleurs personnalisées :", 16, 232));
            customCol1Btn = new Button
            {
                Text = "Couleur 1",
                Location = new Point(16, 254),
                Size = new Size(150, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(255, Color.FromArgb(controller.Settings.CustomColor1Argb)),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            customCol1Btn.Click += (s, e) => PickCustomColor(1);

            customCol2Btn = new Button
            {
                Text = "Couleur 2",
                Location = new Point(180, 254),
                Size = new Size(150, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(255, Color.FromArgb(controller.Settings.CustomColor2Argb)),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            customCol2Btn.Click += (s, e) => PickCustomColor(2);
            col2.Controls.Add(customCol1Btn);
            col2.Controls.Add(customCol2Btn);

            // Duration Slider
            col2.Controls.Add(CreateLabel("Durée de l'animation de transition :", 16, 302));
            durationValueLabel = CreateValueLabel(controller.Settings.DurationMs + " ms", 265, 302);
            col2.Controls.Add(durationValueLabel);

            TrackBar durSlider = CreateSlider(12, 322, 322, 350, 2000, controller.Settings.DurationMs);
            durSlider.ValueChanged += (s, e) =>
            {
                controller.Settings.DurationMs = durSlider.Value;
                durationValueLabel.Text = durSlider.Value + " ms";
                controller.Settings.Save();
            };
            col2.Controls.Add(durSlider);

            // Intensity Slider
            col2.Controls.Add(CreateLabel("Taille / Amplitude des effets :", 16, 364));
            intensityValueLabel = CreateValueLabel(controller.Settings.IntensityPercent + " %", 270, 364);
            col2.Controls.Add(intensityValueLabel);

            TrackBar intSlider = CreateSlider(12, 384, 322, 60, 190, controller.Settings.IntensityPercent);
            intSlider.ValueChanged += (s, e) =>
            {
                controller.Settings.IntensityPercent = intSlider.Value;
                intensityValueLabel.Text = intSlider.Value + " %";
                controller.Settings.Save();
            };
            col2.Controls.Add(intSlider);

            // Corner Animation Speed Slider
            col2.Controls.Add(CreateLabel("Vitesse d'animation des couleurs :", 16, 426));
            Label speedValLbl = CreateValueLabel(controller.Settings.CornerAnimSpeed + " %", 270, 426);
            col2.Controls.Add(speedValLbl);

            TrackBar speedSlider = CreateSlider(12, 446, 322, 25, 250, controller.Settings.CornerAnimSpeed);
            speedSlider.ValueChanged += (s, e) =>
            {
                controller.Settings.CornerAnimSpeed = speedSlider.Value;
                speedValLbl.Text = speedSlider.Value + " %";
                controller.Settings.Save();
            };
            col2.Controls.Add(speedSlider);

            // Quick Theme Preset Buttons
            Label presetLbl = CreateLabel("Raccourcis d'ambiances :", 16, 488);
            col2.Controls.Add(presetLbl);

            Button pSpot = CreateMiniPresetButton("🟢 Mode Spotify", 16, 512, 100, Color.FromArgb(25, 165, 75), () => ApplyQuickPreset(ColorTheme.SpotifyNeonGreen, CornerAnimationMode.AudioBeatSync));
            Button pApple = CreateMiniPresetButton("🔴 Apple Music", 123, 512, 100, Color.FromArgb(215, 35, 65), () => ApplyQuickPreset(ColorTheme.AppleMusicRose, CornerAnimationMode.RotatingGradient));
            Button pCyber = CreateMiniPresetButton("🌈 Full RGB", 230, 512, 100, Color.FromArgb(135, 50, 220), () => ApplyQuickPreset(ColorTheme.CyberpunkRainbow, CornerAnimationMode.RotatingGradient));
            col2.Controls.Add(pSpot);
            col2.Controls.Add(pApple);
            col2.Controls.Add(pCyber);

            CheckBox startupChk = CreateCheckBox("Lancer automatiquement avec Windows", 16, 565, IsStartupEnabled());
            startupChk.CheckedChanged += (s, e) => SetStartup(startupChk.Checked);
            col2.Controls.Add(startupChk);

            // =================================================================
            // COLUMN 3: DYNAMIC ISLAND MUSIC PLAYER (SPOTIFY / APPLE MUSIC / YOUTUBE)
            // =================================================================
            Panel col3 = CreateCard(742, 100, 348, 615, "💊 3. Dynamic Island Music & Hub");
            Controls.Add(col3);

            col3.Controls.Add(CreateLabel("Mode d'affichage du Dynamic Island :", 16, 42));
            ComboBox islandModeCombo = CreateComboBox(16, 64, 314, new string[] {
                "🟢 Toujours affiché en haut de l'écran (Recommandé)",
                "🎵 Affiché quand une musique joue ou en transition",
                "🌊 Uniquement lors du changement d'écran",
                "❌ Désactivé"
            }, (int)controller.Settings.IslandMode);
            islandModeCombo.SelectedIndexChanged += (s, e) =>
            {
                controller.Settings.IslandMode = (IslandVisibilityMode)islandModeCombo.SelectedIndex;
                controller.Settings.Save();
            };
            col3.Controls.Add(islandModeCombo);

            col3.Controls.Add(CreateLabel("Position multi-écrans du Dynamic Island :", 16, 104));
            ComboBox islandPosCombo = CreateComboBox(16, 126, 314, new string[] {
                "🖱️ Suivre l'écran actif (Glisse d'un écran à l'autre !)",
                "🖥️ Toujours rester sur l'écran principal"
            }, (int)controller.Settings.IslandPlacement);
            islandPosCombo.SelectedIndexChanged += (s, e) =>
            {
                controller.Settings.IslandPlacement = (IslandPlacementMode)islandPosCombo.SelectedIndex;
                controller.Settings.Save();
            };
            col3.Controls.Add(islandPosCombo);

            col3.Controls.Add(CreateLabel("Animation du contour du Dynamic Island :", 16, 166));
            ComboBox islandBorderCombo = CreateComboBox(16, 188, 314, new string[] {
                "🌈 Halo Rotatif Multicolore (Apple Intelligence)",
                "🎵 Halo Réactif aux Basses & à la Musique",
                "💨 Liseré Verre Respirant Subtil",
                "⚫ Noir Obsidienne Épuré (Sans halo externe)"
            }, (int)controller.Settings.IslandBorder);
            islandBorderCombo.SelectedIndexChanged += (s, e) =>
            {
                controller.Settings.IslandBorder = (IslandBorderAnimation)islandBorderCombo.SelectedIndex;
                controller.Settings.Save();
            };
            col3.Controls.Add(islandBorderCombo);

            // Island Size Slider
            col3.Controls.Add(CreateLabel("Taille du Dynamic Island :", 16, 228));
            islandSizeValueLabel = CreateValueLabel(controller.Settings.IslandScalePercent + " %", 270, 228);
            col3.Controls.Add(islandSizeValueLabel);

            TrackBar islandSizeSlider = CreateSlider(12, 248, 322, 75, 155, controller.Settings.IslandScalePercent);
            islandSizeSlider.ValueChanged += (s, e) =>
            {
                controller.Settings.IslandScalePercent = islandSizeSlider.Value;
                islandSizeValueLabel.Text = islandSizeSlider.Value + " %";
                controller.Settings.Save();
            };
            col3.Controls.Add(islandSizeSlider);

            CheckBox hoverExpandChk = CreateCheckBox("Ouvrir le lecteur complet au survol de la souris", 16, 288, controller.Settings.IslandExpandOnHover);
            hoverExpandChk.CheckedChanged += (s, e) =>
            {
                controller.Settings.IslandExpandOnHover = hoverExpandChk.Checked;
                controller.Settings.Save();
            };
            col3.Controls.Add(hoverExpandChk);

            CheckBox autoBrandChk = CreateCheckBox("Adapter la couleur à l'app (Vert Spotify, Rose Apple, Rouge YT)", 16, 316, controller.Settings.IslandAutoAdaptBrandColor);
            autoBrandChk.CheckedChanged += (s, e) =>
            {
                controller.Settings.IslandAutoAdaptBrandColor = autoBrandChk.Checked;
                controller.Settings.Save();
            };
            col3.Controls.Add(autoBrandChk);

            CheckBox clockIdleChk = CreateCheckBox("Afficher l'heure dans la pilule quand aucune musique ne joue", 16, 344, controller.Settings.IslandShowClockWhenIdle);
            clockIdleChk.CheckedChanged += (s, e) =>
            {
                controller.Settings.IslandShowClockWhenIdle = clockIdleChk.Checked;
                controller.Settings.Save();
            };
            col3.Controls.Add(clockIdleChk);

            // Direct Media Controls & Service Launchers inside Settings too!
            col3.Controls.Add(CreateLabel("Contrôles & Lancement Rapide des Services :", 16, 382));

            Button btnSpot = CreateMiniPresetButton("🟢 Ouvrir Spotify", 16, 406, 100, Color.FromArgb(24, 150, 68), () => controller.MediaTracker.LaunchOrFocusService("spotify"));
            Button btnApple = CreateMiniPresetButton("🔴 Apple Music", 123, 406, 100, Color.FromArgb(200, 35, 60), () => controller.MediaTracker.LaunchOrFocusService("applemusic"));
            Button btnYT = CreateMiniPresetButton("▶️ YouTube Music", 230, 406, 100, Color.FromArgb(195, 25, 45), () => controller.MediaTracker.LaunchOrFocusService("youtube"));
            col3.Controls.Add(btnSpot);
            col3.Controls.Add(btnApple);
            col3.Controls.Add(btnYT);

            Button btnPrev = CreateMiniPresetButton("⏮ Précédent", 16, 446, 100, Color.FromArgb(40, 45, 66), () => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PREV_TRACK));
            Button btnPlay = CreateMiniPresetButton("⏯ Play / Pause", 123, 446, 100, Color.FromArgb(0, 110, 225), () => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PLAY_PAUSE));
            Button btnNext = CreateMiniPresetButton("⏭ Suivant", 230, 446, 100, Color.FromArgb(40, 45, 66), () => NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_NEXT_TRACK));
            col3.Controls.Add(btnPrev);
            col3.Controls.Add(btnPlay);
            col3.Controls.Add(btnNext);

            Label tipBox = new Label
            {
                Location = new Point(16, 490),
                Size = new Size(314, 64),
                BackColor = Color.FromArgb(28, 32, 46),
                ForeColor = Color.FromArgb(185, 200, 230),
                Font = new Font("Segoe UI", 8.3f, FontStyle.Regular),
                Padding = new Padding(8),
                Text = "💡 Survolez la pilule Dynamic Island tout en haut de votre écran avec la souris : elle s'ouvre avec égaliseur temps réel, titre en cours et boutons cliquables !"
            };
            col3.Controls.Add(tipBox);

            Button minimizeBtn = new Button
            {
                Text = "📥 Réduire dans la barre système (Actif en fond)",
                Location = new Point(16, 564),
                Size = new Size(314, 38),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(42, 48, 70),
                ForeColor = Color.FromArgb(225, 235, 255),
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            minimizeBtn.FlatAppearance.BorderSize = 0;
            minimizeBtn.Click += (s, e) => Hide();
            col3.Controls.Add(minimizeBtn);
        }

        private void PickCustomColor(int slot)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.FullOpen = true;
                cd.Color = (slot == 1)
                    ? Color.FromArgb(255, Color.FromArgb(controller.Settings.CustomColor1Argb))
                    : Color.FromArgb(255, Color.FromArgb(controller.Settings.CustomColor2Argb));

                if (cd.ShowDialog(this) == DialogResult.OK)
                {
                    if (slot == 1)
                    {
                        controller.Settings.CustomColor1Argb = cd.Color.ToArgb();
                        customCol1Btn.BackColor = cd.Color;
                    }
                    else
                    {
                        controller.Settings.CustomColor2Argb = cd.Color.ToArgb();
                        customCol2Btn.BackColor = cd.Color;
                    }
                    controller.Settings.Theme = ColorTheme.CustomColors;
                    themeCombo.SelectedIndex = (int)ColorTheme.CustomColors;
                    controller.RefreshAllCorners();
                    previewBox.Invalidate();
                }
            }
        }

        private void ApplyQuickPreset(ColorTheme theme, CornerAnimationMode cornerMode)
        {
            controller.Settings.Theme = theme;
            controller.Settings.CornerAnim = cornerMode;
            if (themeCombo != null) themeCombo.SelectedIndex = (int)theme;
            controller.RefreshAllCorners();
            previewBox.Invalidate();
            controller.TriggerDemoTransition();
        }

        private Button CreateMiniPresetButton(string text, int x, int y, int w, Color bg, Action onClick)
        {
            Button b = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(w, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = bg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 8.2f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            b.Click += (s, e) => onClick();
            return b;
        }

        private void PaintPreviewBox(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = previewBox.ClientRectangle;

            Color[] palette = ThemeColorEngine.GetPalette(controller.Settings, controller.GlobalTimeSeconds);
            using (LinearGradientBrush wallpaper = new LinearGradientBrush(r,
                Color.FromArgb(255, 25, 45, 88),
                Color.FromArgb(255, 78, 32, 102),
                25f))
            {
                g.FillRectangle(wallpaper, r);
            }

            int previewRad = Math.Min(56, controller.Settings.CornerRadius);
            if (controller.Settings.Enabled && previewRad > 2)
            {
                int sz = previewRad + 20;
                using (Bitmap tlBmp = MonitorCorners.RenderCornerBitmap(sz, previewRad, CornerPosition.TopLeft, controller.Settings, 0.35f, palette, controller.GlobalTimeSeconds, 0.5f))
                using (Bitmap trBmp = MonitorCorners.RenderCornerBitmap(sz, previewRad, CornerPosition.TopRight, controller.Settings, 0.35f, palette, controller.GlobalTimeSeconds, 0.5f))
                {
                    g.DrawImage(tlBmp, 0, 0);
                    g.DrawImage(trBmp, r.Width - tlBmp.Width, 0);
                }
            }

            using (Font f = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold))
            using (SolidBrush b = new SolidBrush(Color.FromArgb(235, 255, 255, 255)))
            {
                string txt = "Aperçu • " + controller.Settings.CornerRadius + " px • " + controller.Settings.Theme;
                SizeF sz = g.MeasureString(txt, f);
                g.DrawString(txt, f, b, (r.Width - sz.Width) / 2f, (r.Height - sz.Height) / 2f + 4f);
            }
        }

        private void UpdateMonitorSummary()
        {
            if (monitorsInfoLabel == null) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("🖥 {0} écran(s) actif(s) :\n", controller.Monitors.Count);
            for (int i = 0; i < controller.Monitors.Count; i++)
            {
                Rectangle b = controller.Monitors[i].Bounds;
                sb.AppendFormat("• Écran {0} : {1}×{2} ({3},{4})\n", i + 1, b.Width, b.Height, b.X, b.Y);
            }
            monitorsInfoLabel.Text = sb.ToString();
        }

        private Panel CreateCard(int x, int y, int w, int h, string title)
        {
            Panel p = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = Color.FromArgb(22, 24, 35)
            };
            Label hdr = new Label
            {
                Text = title,
                Font = new Font("Segoe UI Semibold", 10.8f, FontStyle.Bold),
                ForeColor = Color.FromArgb(110, 205, 255),
                Location = new Point(14, 12),
                AutoSize = true
            };
            p.Controls.Add(hdr);
            return p;
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
                BackColor = Color.FromArgb(22, 24, 35)
            };
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = Color.FromArgb(205, 212, 232)
            };
        }

        private Label CreateValueLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 210, 255)
            };
        }

        private CheckBox CreateCheckBox(string text, int x, int y, bool isChecked)
        {
            return new CheckBox
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Checked = isChecked,
                ForeColor = Color.FromArgb(230, 235, 250),
                Cursor = Cursors.Hand
            };
        }

        private ComboBox CreateComboBox(int x, int y, int w, string[] items, int selectedIndex)
        {
            ComboBox cb = new ComboBox
            {
                Location = new Point(x, y),
                Width = w,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(34, 38, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            cb.Items.AddRange(items);
            if (selectedIndex >= 0 && selectedIndex < items.Length)
                cb.SelectedIndex = selectedIndex;
            else
                cb.SelectedIndex = 0;
            return cb;
        }

        private bool IsStartupEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false))
                {
                    if (key != null) return key.GetValue("iCornerDuo") != null;
                }
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
                        if (enable)
                            key.SetValue("iCornerDuo", "\"" + Application.ExecutablePath + "\" --tray");
                        else
                            key.DeleteValue("iCornerDuo", false);
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
                    Timer welcomeTimer = new Timer { Interval = 450 };
                    welcomeTimer.Tick += (s, e) =>
                    {
                        welcomeTimer.Stop();
                        welcomeTimer.Dispose();
                        controller.TriggerDemoTransition();
                    };
                    welcomeTimer.Start();
                }

                Application.Run();
            }
        }
    }
}
