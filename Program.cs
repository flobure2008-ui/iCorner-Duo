using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ScreenCornersApp
{
    // =========================================================================
    // ENUMS & CONFIGURATION MODES
    // =========================================================================
    public enum CurveGeometry
    {
        HyperbolicSquircle = 0,     // Superellipse n=4.2 ultra-douce
        CircularArc = 1             // Arc de cercle classique n=2.0
    }

    public enum CornerEffectMode
    {
        MinimalFrost = 0,           // Verre dépoli pur et fixe (0% CPU)
        PrismaticFlow = 1,          // Ondulation spectrale douce
        SoftBreathing = 2,          // Respiration atmosphérique douce
        AudioPulseSync = 3,         // Pulsation synchronisée sur le son système
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
        CornerPulseOnly = 7         // Pulsation douce des angles de l'écran cible
    }

    public enum GlassColorPalette
    {
        AetherPrism = 0,            // Cyan Glacé, Lavande, Rose Pâle, Ambre Doux
        FrostWhiteSilver = 1,       // Verre Givre & Argent Pur
        OceanicCyan = 2,            // Azur Profond, Cyan Néon, Indigo Polaire
        EmeraldMint = 3,            // Menthe Givrée & Émeraude Organique
        CrimsonVelvet = 4,          // Rubis Velours & Corail Crépusculaire
        SolarAmber = 5,             // Ambre Solaire Doré & Champagne Chaud
        BorealAurora = 6,           // Aurore Boréale Émeraude & Cyan
        UltravioletMist = 7,        // Lavande Électrique & Brume Violette
        SakuraBlossom = 8,          // Fleur de Sakura Pastel & Pêche Douce
        ObsidianMonochrome = 9,     // Titane Fumé & Argent Monochromatique
        ChromaticSpectrum = 10,     // Défilement Spectrale Continu 360°
        CustomGlassPalette = 11     // Teintes Définies par l'Utilisateur
    }

    // =========================================================================
    // APPLICATION SETTINGS
    // =========================================================================
    public class AppSettings
    {
        // 1. Coins d'écran
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

        // 2. Passerelle Inter-Écrans & Palettes
        public bool EnableTransition = true;
        public bool EnableMagneticProximity = true;
        public PortalTransitionStyle Style = PortalTransitionStyle.GlassMorphCombo;
        public GlassColorPalette Theme = GlassColorPalette.AetherPrism;
        public int DurationMs = 850;
        public int IntensityPercent = 100;
        public int CustomColor1Argb = Color.FromArgb(255, 110, 220, 255).ToArgb();
        public int CustomColor2Argb = Color.FromArgb(255, 185, 130, 255).ToArgb();

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
                sb.AppendLine(string.Format("  \"Enabled\": {0},", Enabled ? "true" : "false"));
                sb.AppendLine(string.Format("  \"CornerRadius\": {0},", CornerRadius));
                sb.AppendLine(string.Format("  \"Curve\": {0},", (int)Curve));
                sb.AppendLine(string.Format("  \"TopLeft\": {0},", TopLeft ? "true" : "false"));
                sb.AppendLine(string.Format("  \"TopRight\": {0},", TopRight ? "true" : "false"));
                sb.AppendLine(string.Format("  \"BottomLeft\": {0},", BottomLeft ? "true" : "false"));
                sb.AppendLine(string.Format("  \"BottomRight\": {0},", BottomRight ? "true" : "false"));
                sb.AppendLine(string.Format("  \"CornerBezelGlow\": {0},", CornerBezelGlow ? "true" : "false"));
                sb.AppendLine(string.Format("  \"BezelGlowOpacity\": {0},", BezelGlowOpacity));
                sb.AppendLine(string.Format("  \"BezelThickness\": {0},", BezelThickness));
                sb.AppendLine(string.Format("  \"CornerAnim\": {0},", (int)CornerAnim));
                sb.AppendLine(string.Format("  \"CornerAnimSpeed\": {0},", CornerAnimSpeed));
                sb.AppendLine(string.Format("  \"EnableTransition\": {0},", EnableTransition ? "true" : "false"));
                sb.AppendLine(string.Format("  \"EnableMagneticProximity\": {0},", EnableMagneticProximity ? "true" : "false"));
                sb.AppendLine(string.Format("  \"Style\": {0},", (int)Style));
                sb.AppendLine(string.Format("  \"Theme\": {0},", (int)Theme));
                sb.AppendLine(string.Format("  \"DurationMs\": {0},", DurationMs));
                sb.AppendLine(string.Format("  \"IntensityPercent\": {0},", IntensityPercent));
                sb.AppendLine(string.Format("  \"CustomColor1Argb\": {0},", CustomColor1Argb));
                sb.AppendLine(string.Format("  \"CustomColor2Argb\": {0}", CustomColor2Argb));
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
                if (!File.Exists(SettingsPath)) return s;
                string json = File.ReadAllText(SettingsPath, Encoding.UTF8);
                s.Enabled = ReadBool(json, "Enabled", s.Enabled);
                s.CornerRadius = ReadInt(json, "CornerRadius", s.CornerRadius);
                s.Curve = (CurveGeometry)ReadInt(json, "Curve", (int)s.Curve);
                s.TopLeft = ReadBool(json, "TopLeft", s.TopLeft);
                s.TopRight = ReadBool(json, "TopRight", s.TopRight);
                s.BottomLeft = ReadBool(json, "BottomLeft", s.BottomLeft);
                s.BottomRight = ReadBool(json, "BottomRight", s.BottomRight);
                s.CornerBezelGlow = ReadBool(json, "CornerBezelGlow", s.CornerBezelGlow);
                s.BezelGlowOpacity = ReadInt(json, "BezelGlowOpacity", s.BezelGlowOpacity);
                s.BezelThickness = ReadInt(json, "BezelThickness", s.BezelThickness);
                s.CornerAnim = (CornerEffectMode)ReadInt(json, "CornerAnim", (int)s.CornerAnim);
                s.CornerAnimSpeed = ReadInt(json, "CornerAnimSpeed", s.CornerAnimSpeed);
                s.EnableTransition = ReadBool(json, "EnableTransition", s.EnableTransition);
                s.EnableMagneticProximity = ReadBool(json, "EnableMagneticProximity", s.EnableMagneticProximity);
                s.Style = (PortalTransitionStyle)ReadInt(json, "Style", (int)s.Style);
                s.Theme = (GlassColorPalette)ReadInt(json, "Theme", (int)s.Theme);
                s.DurationMs = ReadInt(json, "DurationMs", s.DurationMs);
                s.IntensityPercent = ReadInt(json, "IntensityPercent", s.IntensityPercent);
                s.CustomColor1Argb = ReadInt(json, "CustomColor1Argb", s.CustomColor1Argb);
                s.CustomColor2Argb = ReadInt(json, "CustomColor2Argb", s.CustomColor2Argb);
            }
            catch { }
            return s;
        }

        private static bool ReadBool(string json, string key, bool fallback)
        {
            string pat = "\"" + key + "\":";
            int idx = json.IndexOf(pat, StringComparison.OrdinalIgnoreCase);
            if (idx < 0) return fallback;
            int start = idx + pat.Length;
            int end = json.IndexOfAny(new char[] { ',', '}', '\r', '\n' }, start);
            if (end < 0) end = json.Length;
            string val = json.Substring(start, end - start).Trim().ToLowerInvariant();
            if (val == "true") return true;
            if (val == "false") return false;
            return fallback;
        }

        private static int ReadInt(string json, string key, int fallback)
        {
            string pat = "\"" + key + "\":";
            int idx = json.IndexOf(pat, StringComparison.OrdinalIgnoreCase);
            if (idx < 0) return fallback;
            int start = idx + pat.Length;
            int end = json.IndexOfAny(new char[] { ',', '}', '\r', '\n' }, start);
            if (end < 0) end = json.Length;
            string val = json.Substring(start, end - start).Trim();
            int res;
            if (int.TryParse(val, out res)) return res;
            return fallback;
        }
    }

    // =========================================================================
    // GLASS THEME & PALETTES ENGINE
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

        public static void DrawFrostedGlassSurface(
            Graphics g, RectangleF rect, float cornerRadius,
            Color baseDarkTint, Color accentTint, int glassOpacity = 238, float borderAlphaRatio = 1.0f)
        {
            if (rect.Width <= 2f || rect.Height <= 2f) return;
            using (GraphicsPath path = CreateRoundedRect(rect, cornerRadius))
            {
                using (SolidBrush baseBrush = new SolidBrush(Color.FromArgb(glassOpacity, baseDarkTint)))
                {
                    g.FillPath(baseBrush, path);
                }

                using (LinearGradientBrush sheen = new LinearGradientBrush(
                    rect,
                    Color.FromArgb((int)(28 * borderAlphaRatio), 255, 255, 255),
                    Color.FromArgb((int)(12 * borderAlphaRatio), accentTint),
                    35f))
                {
                    g.FillPath(sheen, path);
                }

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
    // NATIVE WIN32 INTEROP & CORE AUDIO API
    // =========================================================================
    public static class NativeMethods
    {
        public const int ULW_ALPHA = 2;
        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;

        public const int WS_EX_LAYERED = 0x80000;
        public const int WS_EX_TRANSPARENT = 0x20;
        public const int WS_EX_TOOLWINDOW = 0x80;
        public const int WS_EX_NOACTIVATE = 0x08000000;
        public const int WS_EX_TOPMOST = 0x8;

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOACTIVATE = 0x0010;
        public const uint SWP_SHOWWINDOW = 0x0040;

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
        public static extern bool SetProcessDPIAware();

        [DllImport("shcore.dll")]
        public static extern int SetProcessDpiAwareness(int value);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDpiAwarenessContext(IntPtr dpiContext);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

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

    public class CoreAudioPeakMeter
    {
        private IAudioMeterInformation audioMeter;
        public float CurrentAudioPeak = 0f;

        public CoreAudioPeakMeter()
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

        public void UpdateTick()
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
        }
    }

    // =========================================================================
    // LAYERED OVERLAY FORM (ALPHA TRANSPARENT, CLICK-THROUGH, TOPMOST)
    // =========================================================================
    public class LayeredOverlayForm : Form
    {
        private bool isClickThrough;

        public LayeredOverlayForm(bool clickThrough = true)
        {
            isClickThrough = clickThrough;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
            DoubleBuffered = true;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= NativeMethods.WS_EX_LAYERED;
                cp.ExStyle |= NativeMethods.WS_EX_TOOLWINDOW;
                cp.ExStyle |= NativeMethods.WS_EX_NOACTIVATE;
                cp.ExStyle |= NativeMethods.WS_EX_TOPMOST;
                if (isClickThrough) cp.ExStyle |= NativeMethods.WS_EX_TRANSPARENT;
                return cp;
            }
        }

        protected override bool ShowWithoutActivation { get { return true; } }

        public void EnsureTopMost()
        {
            if (IsHandleCreated && Visible)
            {
                NativeMethods.SetWindowPos(Handle, NativeMethods.HWND_TOPMOST, 0, 0, 0, 0,
                    NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOACTIVATE);
            }
        }

        public void SetBitmap(Bitmap bitmap, int x, int y, byte opacity = 255)
        {
            if (bitmap == null || !IsHandleCreated) return;
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb) return;

            IntPtr screenDC = NativeMethods.GetDC(IntPtr.Zero);
            IntPtr memDC = NativeMethods.CreateCompatibleDC(screenDC);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBitmap = NativeMethods.SelectObject(memDC, hBitmap);

                NativeMethods.SIZE size = new NativeMethods.SIZE(bitmap.Width, bitmap.Height);
                NativeMethods.POINT pointSource = new NativeMethods.POINT(0, 0);
                NativeMethods.POINT topPos = new NativeMethods.POINT(x, y);
                NativeMethods.BLENDFUNCTION blend = new NativeMethods.BLENDFUNCTION();
                blend.BlendOp = NativeMethods.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = opacity;
                blend.AlphaFormat = NativeMethods.AC_SRC_ALPHA;

                NativeMethods.UpdateLayeredWindow(
                    Handle, screenDC, ref topPos, ref size,
                    memDC, ref pointSource, 0, ref blend, NativeMethods.ULW_ALPHA);
            }
            finally
            {
                if (oldBitmap != IntPtr.Zero) NativeMethods.SelectObject(memDC, oldBitmap);
                if (hBitmap != IntPtr.Zero) NativeMethods.DeleteObject(hBitmap);
                NativeMethods.DeleteDC(memDC);
                NativeMethods.ReleaseDC(IntPtr.Zero, screenDC);
            }
        }
    }

    // =========================================================================
    // MONITOR CORNERS CONTROLLER (PER-MONITOR ROUNDED CORNERS & SPECULAR RIM)
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
                        float wave = (float)(0.5 + 0.5 * Math.Sin(animPhase * 2.5f + (int)pos * 1.57f));
                        cPrimary = GlassThemeEngine.Interpolate(cPrimary, cSecondary, wave);
                        animGlowBoost = 0.15f * wave;
                    }
                    else if (settings.CornerAnim == CornerEffectMode.StarlightDust)
                    {
                        float twinkle = (float)(0.5 + 0.5 * Math.Sin(animPhase * 4.2f + (int)pos * 2.1f));
                        animGlowBoost = 0.20f * twinkle;
                    }

                    // 1. Bezel Glow Aura
                    if ((settings.CornerBezelGlow || pulseGlow > 0.05f || animGlowBoost > 0.02f) && curveLine.PointCount > 1)
                    {
                        float baseAlphaRatio = (settings.BezelGlowOpacity / 100f);
                        float glowIntensity = Math.Max(baseAlphaRatio, pulseGlow) + animGlowBoost;
                        glowIntensity = Math.Min(1.3f, glowIntensity);

                        int steps = 7;
                        float maxThick = 24f * glowIntensity;
                        for (int s = steps; s >= 1; s--)
                        {
                            float tStep = (float)s / steps;
                            float w = maxThick * tStep;
                            int alpha = (int)(glowIntensity * 48 * (1.0f - tStep * 0.85f));
                            if (alpha <= 0) continue;
                            Color glowCol = GlassThemeEngine.Interpolate(cPrimary, cSecondary, tStep);
                            using (Pen glowPen = new Pen(Color.FromArgb(alpha, glowCol), w))
                            {
                                glowPen.StartCap = LineCap.Round;
                                glowPen.EndCap = LineCap.Round;
                                g.DrawPath(glowPen, curveLine);
                            }
                        }
                    }

                    // 2. Pure Black Corner Bezel Mask
                    using (SolidBrush maskBrush = new SolidBrush(Color.Black))
                    {
                        g.FillPath(maskBrush, cornerMask);
                    }

                    // 3. Ultra-Crisp Hairline Specular Edge
                    if (curveLine.PointCount > 1)
                    {
                        float rimThickness = Math.Max(1.0f, settings.BezelThickness / 10f);
                        int rimAlpha = Math.Min(255, (int)(180 + pulseGlow * 75));
                        Color rimColor = Color.FromArgb(rimAlpha, GlassThemeEngine.Interpolate(Color.White, cPrimary, 0.35f));
                        using (Pen rimPen = new Pen(rimColor, rimThickness))
                        {
                            rimPen.StartCap = LineCap.Flat;
                            rimPen.EndCap = LineCap.Flat;
                            g.DrawPath(rimPen, curveLine);
                        }
                    }
                }
            }
            return bmp;
        }

        private static GraphicsPath CreateCornerMaskPath(int size, int radius, CornerPosition pos, CurveGeometry geom)
        {
            GraphicsPath path = new GraphicsPath();
            PointF cornerVertex = PointF.Empty;
            PointF pStart = PointF.Empty;
            PointF pEnd = PointF.Empty;

            switch (pos)
            {
                case CornerPosition.TopLeft:
                    cornerVertex = new PointF(0, 0);
                    pStart = new PointF(0, radius);
                    pEnd = new PointF(radius, 0);
                    break;
                case CornerPosition.TopRight:
                    cornerVertex = new PointF(size, 0);
                    pStart = new PointF(size - radius, 0);
                    pEnd = new PointF(size, radius);
                    break;
                case CornerPosition.BottomLeft:
                    cornerVertex = new PointF(0, size);
                    pStart = new PointF(radius, size);
                    pEnd = new PointF(0, size - radius);
                    break;
                case CornerPosition.BottomRight:
                    cornerVertex = new PointF(size, size);
                    pStart = new PointF(size, size - radius);
                    pEnd = new PointF(size - radius, size);
                    break;
            }

            path.AddLine(cornerVertex, pStart);

            List<PointF> curvePts = GenerateCurvePoints(size, radius, pos, geom);
            if (curvePts.Count > 1)
            {
                path.AddCurve(curvePts.ToArray(), 0.5f);
            }
            else
            {
                path.AddLine(pStart, pEnd);
            }

            path.AddLine(pEnd, cornerVertex);
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath CreateCornerCurvePath(int size, int radius, CornerPosition pos, CurveGeometry geom)
        {
            GraphicsPath path = new GraphicsPath();
            List<PointF> pts = GenerateCurvePoints(size, radius, pos, geom);
            if (pts.Count > 1)
                path.AddCurve(pts.ToArray(), 0.5f);
            return path;
        }

        private static List<PointF> GenerateCurvePoints(int size, int radius, CornerPosition pos, CurveGeometry geom)
        {
            List<PointF> pts = new List<PointF>();
            int steps = 24;
            double n = (geom == CurveGeometry.HyperbolicSquircle) ? 4.2 : 2.0;

            for (int i = 0; i <= steps; i++)
            {
                double theta;
                switch (pos)
                {
                    case CornerPosition.TopLeft:
                        theta = Math.PI - (i / (double)steps) * (Math.PI / 2.0);
                        break;
                    case CornerPosition.TopRight:
                        theta = (Math.PI / 2.0) - (i / (double)steps) * (Math.PI / 2.0);
                        break;
                    case CornerPosition.BottomLeft:
                        theta = Math.PI + (i / (double)steps) * (Math.PI / 2.0);
                        break;
                    case CornerPosition.BottomRight:
                    default:
                        theta = (3.0 * Math.PI / 2.0) + (i / (double)steps) * (Math.PI / 2.0);
                        break;
                }

                double cosT = Math.Cos(theta);
                double sinT = Math.Sin(theta);
                double sCos = Math.Sign(cosT);
                double sSin = Math.Sign(sinT);

                double xRel = radius * sCos * Math.Pow(Math.Abs(cosT), 2.0 / n);
                double yRel = radius * sSin * Math.Pow(Math.Abs(sinT), 2.0 / n);

                float cx, cy;
                switch (pos)
                {
                    case CornerPosition.TopLeft:
                        cx = radius; cy = radius;
                        break;
                    case CornerPosition.TopRight:
                        cx = size - radius; cy = radius;
                        break;
                    case CornerPosition.BottomLeft:
                        cx = radius; cy = size - radius;
                        break;
                    case CornerPosition.BottomRight:
                    default:
                        cx = size - radius; cy = size - radius;
                        break;
                }

                pts.Add(new PointF((float)(cx + xRel), (float)(cy - yRel)));
            }

            return pts;
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
                    CornerForms[i].Hide();
                    CornerForms[i].Dispose();
                    CornerForms[i] = null;
                }
            }
        }
    }

    // =========================================================================
    // SCREEN TRANSITION ANIMATOR (ORGANIC GLASS BRIDGE BETWEEN MONITORS)
    // =========================================================================
    public enum ScreenEdge { Left, Right, Top, Bottom }

    public class ScreenTransitionAnimator : IDisposable
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

            if (settings.Style != PortalTransitionStyle.CornerPulseOnly)
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

        private static float EaseOutCubic(float x) { return 1f - (float)Math.Pow(1.0 - x, 3.0); }
        private static float EaseInOutCubic(float x) { return (x < 0.5f) ? 4f * x * x * x : 1f - (float)Math.Pow(-2.0 * x + 2.0, 3.0) / 2f; }
        private static float EaseOutBack(float x) { float c1 = 1.70158f; float c3 = c1 + 1f; return 1f + c3 * (float)Math.Pow(x - 1f, 3.0) + c1 * (float)Math.Pow(x - 1f, 2.0); }
        private static int Clamp(int v, int min, int max) { return (v < min) ? min : ((v > max) ? max : v); }
        private static float Clamp(float v, float min, float max) { return (v < min) ? min : ((v > max) ? max : v); }

        public void Dispose()
        {
            if (entryEdgeForm != null) { entryEdgeForm.Dispose(); entryEdgeForm = null; }
            if (exitEdgeForm != null) { exitEdgeForm.Dispose(); exitEdgeForm = null; }
        }
    }

    // =========================================================================
    // CUSTOM GLASS CONTROLS (ROUNDED, DOUBLE-BUFFERED, ZERO FLICKER)
    // =========================================================================
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }
    }

    public class GlassButton : Control
    {
        private bool isHovered = false;
        private bool isPressed = false;
        private Color baseColor;

        public GlassButton(string text, int x, int y, int w, int h, Color color)
        {
            Text = text;
            Location = new Point(x, y);
            Size = new Size(w, h);
            baseColor = color;
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold);
            ForeColor = Color.White;
        }

        protected override void OnMouseEnter(EventArgs e) { isHovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { isHovered = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { isPressed = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { isPressed = false; Invalidate(); base.OnMouseUp(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            Color fill = isPressed ? GlassThemeEngine.Interpolate(baseColor, Color.Black, 0.25f)
                       : isHovered ? GlassThemeEngine.Interpolate(baseColor, Color.White, 0.20f)
                       : baseColor;

            GlassThemeEngine.DrawFrostedGlassSurface(g, r, 10f, fill, fill, 240, 1.0f);

            TextRenderer.DrawText(g, Text, Font, ClientRectangle, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }
    }

    public class GlassNavButton : Control
    {
        public string Glyph { get; set; }
        public string LabelText { get; set; }
        public bool Selected { get; set; }
        private bool isHovered = false;

        public GlassNavButton(string glyph, string text, int x, int y, int w, int h)
        {
            Glyph = glyph;
            LabelText = text;
            Location = new Point(x, y);
            Size = new Size(w, h);
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        }

        protected override void OnMouseEnter(EventArgs e) { isHovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { isHovered = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(2, 2, Width - 4, Height - 4);
            if (Selected)
            {
                GlassThemeEngine.DrawFrostedGlassSurface(g, r, 12f, Color.FromArgb(24, 38, 65), Color.FromArgb(100, 185, 255), 245, 1.0f);
            }
            else if (isHovered)
            {
                GlassThemeEngine.DrawFrostedGlassSurface(g, r, 12f, Color.FromArgb(18, 24, 38), Color.FromArgb(70, 95, 140), 200, 0.6f);
            }

            Color textColor = Selected ? Color.FromArgb(255, 255, 255) : (isHovered ? Color.FromArgb(220, 232, 255) : Color.FromArgb(160, 172, 198));
            using (SolidBrush b = new SolidBrush(textColor))
            {
                Font gf = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
                g.DrawString(Glyph, gf, b, 14, (Height - 20) / 2);
                g.DrawString(LabelText, Font, b, 38, (Height - 19) / 2);
            }
        }
    }

    public class GlassCardPanel : Panel
    {
        public string Title { get; set; }

        public GlassCardPanel(int x, int y, int w, int h, string title)
        {
            Location = new Point(x, y);
            Size = new Size(w, h);
            Title = title;
            DoubleBuffered = true;
            BackColor = Color.FromArgb(16, 19, 28);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            GlassThemeEngine.DrawFrostedGlassSurface(g, r, 18f, BackColor, Color.FromArgb(60, 85, 130), 245, 0.85f);

            if (!string.IsNullOrEmpty(Title))
            {
                using (Font tf = new Font("Segoe UI Semibold", 10.2f, FontStyle.Bold))
                using (SolidBrush tb = new SolidBrush(Color.FromArgb(240, 245, 255)))
                {
                    g.DrawString(Title, tf, tb, 18, 16);
                }
                using (Pen sep = new Pen(Color.FromArgb(30, 255, 255, 255), 1f))
                {
                    g.DrawLine(sep, 16, 44, Width - 16, 44);
                }
            }
        }
    }

    public class GlassToggleSwitch : Control
    {
        public bool Checked { get; set; }
        public event EventHandler CheckedChanged;

        public GlassToggleSwitch(string text, int x, int y, int w, bool isChecked)
        {
            Text = text;
            Location = new Point(x, y);
            Size = new Size(w, 28);
            Checked = isChecked;
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 9.2f);
            ForeColor = Color.FromArgb(220, 228, 245);
        }

        protected override void OnClick(EventArgs e)
        {
            Checked = !Checked;
            Invalidate();
            if (CheckedChanged != null) CheckedChanged(this, EventArgs.Empty);
            base.OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF pill = new RectangleF(2, 4, 38, 20);
            Color pillCol = Checked ? Color.FromArgb(40, 140, 250) : Color.FromArgb(32, 38, 52);
            GlassThemeEngine.DrawFrostedGlassSurface(g, pill, 10f, pillCol, Checked ? Color.FromArgb(120, 200, 255) : Color.FromArgb(70, 85, 110), 240, 1.0f);

            float knobX = Checked ? (pill.Right - 18) : (pill.Left + 2);
            RectangleF knob = new RectangleF(knobX, pill.Top + 2, 16, 16);
            using (SolidBrush kb = new SolidBrush(Color.White))
            {
                g.FillEllipse(kb, knob);
            }

            Rectangle textRect = new Rectangle(48, 0, Width - 48, Height);
            TextRenderer.DrawText(g, Text, Font, textRect, ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }
    }

    public class GlassSlider : Control
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        private int currentValue;
        public int Value
        {
            get { return currentValue; }
            set { currentValue = Math.Max(Minimum, Math.Min(Maximum, value)); Invalidate(); }
        }
        public event EventHandler ValueChanged;
        private bool isDragging = false;

        public GlassSlider(int x, int y, int w, int min, int max, int val)
        {
            Location = new Point(x, y);
            Size = new Size(w, 26);
            Minimum = min;
            Maximum = max;
            currentValue = val;
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseDown(MouseEventArgs e) { isDragging = true; UpdateFromMouse(e.X); base.OnMouseDown(e); }
        protected override void OnMouseMove(MouseEventArgs e) { if (isDragging) UpdateFromMouse(e.X); base.OnMouseMove(e); }
        protected override void OnMouseUp(MouseEventArgs e) { isDragging = false; base.OnMouseUp(e); }

        private void UpdateFromMouse(int mouseX)
        {
            float trackLeft = 10f;
            float trackRight = Width - 10f;
            float ratio = (mouseX - trackLeft) / (trackRight - trackLeft);
            ratio = Math.Max(0f, Math.Min(1f, ratio));
            int newVal = Minimum + (int)(ratio * (Maximum - Minimum));
            if (newVal != currentValue)
            {
                currentValue = newVal;
                Invalidate();
                if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float trackY = Height / 2f - 3f;
            float trackH = 6f;
            float trackW = Width - 20f;
            RectangleF trackBg = new RectangleF(10f, trackY, trackW, trackH);

            using (GraphicsPath trackPath = GlassThemeEngine.CreateRoundedRect(trackBg, 3f))
            {
                using (SolidBrush b = new SolidBrush(Color.FromArgb(28, 34, 48))) g.FillPath(b, trackPath);
            }

            float ratio = (Maximum > Minimum) ? (float)(currentValue - Minimum) / (Maximum - Minimum) : 0f;
            float fillW = trackW * ratio;
            if (fillW > 4f)
            {
                RectangleF fillRect = new RectangleF(10f, trackY, fillW, trackH);
                using (GraphicsPath fillPath = GlassThemeEngine.CreateRoundedRect(fillRect, 3f))
                using (LinearGradientBrush fgb = new LinearGradientBrush(fillRect, Color.FromArgb(50, 130, 245), Color.FromArgb(100, 200, 255), 0f))
                {
                    g.FillPath(fgb, fillPath);
                }
            }

            float knobCenterX = 10f + fillW;
            RectangleF knob = new RectangleF(knobCenterX - 8f, Height / 2f - 8f, 16f, 16f);
            using (SolidBrush kb = new SolidBrush(Color.White)) g.FillEllipse(kb, knob);
            using (Pen kp = new Pen(Color.FromArgb(60, 140, 255), 2f)) g.DrawEllipse(kp, knob);
        }
    }

    public class GlassRoundedInput : Panel
    {
        private TextBox innerBox;

        public override string Text
        {
            get { return innerBox != null ? innerBox.Text : ""; }
            set { if (innerBox != null) innerBox.Text = value; }
        }

        public new event EventHandler TextChanged
        {
            add { if (innerBox != null) innerBox.TextChanged += value; }
            remove { if (innerBox != null) innerBox.TextChanged -= value; }
        }

        public GlassRoundedInput(int x, int y, int w, int h, string text, bool isPassword, bool multiline)
        {
            Location = new Point(x, y);
            Size = new Size(w, h);
            DoubleBuffered = true;
            BackColor = Color.FromArgb(16, 20, 30);

            innerBox = new TextBox
            {
                Text = text,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(16, 20, 30),
                ForeColor = Color.FromArgb(235, 240, 255),
                Font = new Font("Segoe UI", 9.2f),
                UseSystemPasswordChar = isPassword,
                Multiline = multiline,
                ScrollBars = multiline ? ScrollBars.Vertical : ScrollBars.None
            };

            if (multiline)
            {
                innerBox.Location = new Point(10, 8);
                innerBox.Size = new Size(w - 20, h - 16);
            }
            else
            {
                innerBox.Location = new Point(10, (h - 18) / 2);
                innerBox.Size = new Size(w - 20, 18);
            }

            Controls.Add(innerBox);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            GlassThemeEngine.DrawFrostedGlassSurface(g, r, 12f, BackColor, Color.FromArgb(60, 85, 130), 245, 0.8f);
        }
    }

    public class GlassRoundedComboBox : Panel
    {
        private ComboBox innerBox;

        public int SelectedIndex
        {
            get { return innerBox != null ? innerBox.SelectedIndex : -1; }
            set { if (innerBox != null) innerBox.SelectedIndex = value; }
        }

        public event EventHandler SelectedIndexChanged
        {
            add { if (innerBox != null) innerBox.SelectedIndexChanged += value; }
            remove { if (innerBox != null) innerBox.SelectedIndexChanged -= value; }
        }

        public GlassRoundedComboBox(int x, int y, int w, int h, string[] items, int selectedIdx)
        {
            Location = new Point(x, y);
            Size = new Size(w, h);
            DoubleBuffered = true;
            BackColor = Color.FromArgb(16, 20, 30);

            innerBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(16, 20, 30),
                ForeColor = Color.FromArgb(235, 240, 255),
                Font = new Font("Segoe UI", 9.2f),
                Location = new Point(10, (h - 22) / 2),
                Size = new Size(w - 20, 22)
            };

            if (items != null) innerBox.Items.AddRange(items);
            if (selectedIdx >= 0 && selectedIdx < innerBox.Items.Count)
                innerBox.SelectedIndex = selectedIdx;

            Controls.Add(innerBox);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            GlassThemeEngine.DrawFrostedGlassSurface(g, r, 12f, BackColor, Color.FromArgb(60, 85, 130), 245, 0.8f);
        }
    }

    public class GlassRoundedBox : Panel
    {
        public float CornerRadius { get; set; }

        public GlassRoundedBox(int x, int y, int w, int h, float radius = 16f)
        {
            Location = new Point(x, y);
            Size = new Size(w, h);
            CornerRadius = radius;
            DoubleBuffered = true;
            BackColor = Color.FromArgb(16, 20, 30);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            GlassThemeEngine.DrawFrostedGlassSurface(g, r, CornerRadius, BackColor, Color.FromArgb(60, 85, 130), 245, 0.8f);
        }
    }

    // =========================================================================
    // OVERLAY CONTROLLER (COORDINATES MONITORS, ANIMATIONS & TRAY)
    // =========================================================================
    public class OverlayController : IDisposable
    {
        public AppSettings Settings;
        public List<MonitorCorners> Monitors = new List<MonitorCorners>();
        public ScreenTransitionAnimator Animator;
        public CoreAudioPeakMeter AudioMeter;

        private System.Windows.Forms.Timer tickTimer;
        private int lastMonitorIndex = -1;
        private int topMostTickCounter = 0;
        private int cornerAnimTickCounter = 0;
        private long startTicks = DateTime.UtcNow.Ticks;
        private NotifyIcon trayIcon;
        private SettingsForm settingsForm;

        public event Action OnMonitorsChanged;

        public OverlayController()
        {
            Settings = AppSettings.Load();
            Animator = new ScreenTransitionAnimator();
            AudioMeter = new CoreAudioPeakMeter();

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
                mc.UpdateCorners(Settings, palette, GlobalTimeSeconds, AudioMeter.CurrentAudioPeak);
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
                m.UpdateCorners(Settings, palette, t, AudioMeter.CurrentAudioPeak);
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
            AudioMeter.UpdateTick();
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
                    m.UpdateCorners(Settings, palette, phase, AudioMeter.CurrentAudioPeak);
            }

            topMostTickCounter++;
            if (topMostTickCounter >= 90)
            {
                topMostTickCounter = 0;
                foreach (var m in Monitors) m.EnsureTopMost();
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
            trayIcon.Text = "iCorner Duo — Studio Glassmorphism";
            trayIcon.Icon = LoadAppIcon() ?? SystemIcons.Application;
            trayIcon.Visible = true;

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem openItem = new ToolStripMenuItem("Ouvrir le Studio de Configuration");
            openItem.Font = new Font(openItem.Font, FontStyle.Bold);
            openItem.Click += (s, e) => ShowSettingsWindow();

            ToolStripMenuItem demoItem = new ToolStripMenuItem("Tester la Passerelle Fluide");
            demoItem.Click += (s, e) => TriggerDemoTransition();

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Quitter iCorner Duo");
            exitItem.Click += (s, e) => { trayIcon.Visible = false; Application.Exit(); };

            menu.Items.Add(openItem);
            menu.Items.Add(demoItem);
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
        private GlassNavButton[] navButtons = new GlassNavButton[2];
        private Panel[] pages = new Panel[2];
        private int activePageIndex = 0;

        private GlassRoundedBox cornerPreviewBox;
        private GlassRoundedComboBox themeCombo;
        private System.Windows.Forms.Timer uiPreviewTimer;
        private Panel headerPanel;

        public SettingsForm(OverlayController ctrl)
        {
            controller = ctrl;
            Text = "iCorner Duo — Studio Glassmorphism";
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
                    if (headerPanel != null) headerPanel.Invalidate(new Rectangle(0, headerPanel.Height - 3, headerPanel.Width, 3));
                }
            };
            uiPreviewTimer.Start();
        }

        private void BuildMinimalGlassStudio()
        {
            // 1. Frosted Header
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
                        g.DrawString("Studio", hdrTitleFont, ab, 80 + tsz.Width - 6, 12);
                    g.DrawString("Coins Organiques Multi-Écrans • Passerelle Liquide Inter-Écrans • Design Glassmorphisme Pur", hdrSubFont, sb, 83, 45);
                }
            };

            Control testTransBtn = CreateGlassBtn("Tester la Passerelle", 845, 18, 170, 38, Color.FromArgb(40, 115, 215), () => controller.TriggerDemoTransition());
            header.Controls.Add(testTransBtn);
            Controls.Add(header);
            headerPanel = header;

            // 2. Left Sidebar
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
                "2.  🌊 Passerelle Fluide"
            };
            string[] navGlyphs = new string[] { "✦", "🌊" };

            for (int i = 0; i < 2; i++)
            {
                int idx = i;
                GlassNavButton nb = new GlassNavButton(navGlyphs[i], navLabels[i].Substring(4), 12, 18 + i * 54, 210, 46);
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

            for (int i = 0; i < 2; i++) Controls.Add(pages[i]);
            SwitchPage(0);
        }

        private void SwitchPage(int idx)
        {
            SuspendLayout();
            activePageIndex = idx;
            for (int i = 0; i < 2; i++)
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
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED : zéro scintillement
                return cp;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            try
            {
                int dark = 1;
                NativeMethods.DwmSetWindowAttribute(Handle, 20, ref dark, 4);
                int round = 2;
                NativeMethods.DwmSetWindowAttribute(Handle, 33, ref round, 4);
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

            controller.OnMonitorsChanged += () =>
            {
                if (IsDisposed) return;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0} moniteur(s) actif(s) :\n", controller.Monitors.Count);
                for (int i = 0; i < controller.Monitors.Count; i++)
                {
                    Rectangle b = controller.Monitors[i].Bounds;
                    sb.AppendFormat("• Écran {0} : {1}×{2} ({3}, {4})\n", i + 1, b.Width, b.Height, b.X, b.Y);
                }
                monLbl.Text = sb.ToString();
            };

            // Right Card
            GlassCardPanel rightCard = new GlassCardPanel(406, 16, 372, 585, "✨ Liseré Optique & Résonance Lumineuse");
            page.Controls.Add(rightCard);

            GlassToggleSwitch glowChk = new GlassToggleSwitch("Liseré spéculaire & biseau lumineux", 18, 54, 330, controller.Settings.CornerBezelGlow);
            glowChk.CheckedChanged += (s, e) => { controller.Settings.CornerBezelGlow = glowChk.Checked; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(glowChk);

            rightCard.Controls.Add(CreateLabel("Opacité du liseré biseauté :", 18, 96));
            Label glowVal = CreateValueLabel(controller.Settings.BezelGlowOpacity + " %", 300, 96);
            rightCard.Controls.Add(glowVal);
            GlassSlider glowSlider = new GlassSlider(18, 120, 336, 0, 100, controller.Settings.BezelGlowOpacity);
            glowSlider.ValueChanged += (s, e) => { controller.Settings.BezelGlowOpacity = glowSlider.Value; glowVal.Text = glowSlider.Value + " %"; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(glowSlider);

            rightCard.Controls.Add(CreateLabel("Épaisseur du liseré biseauté :", 18, 156));
            Label thVal = CreateValueLabel((controller.Settings.BezelThickness / 10.0f).ToString("0.0") + " px", 295, 156);
            rightCard.Controls.Add(thVal);
            GlassSlider thSlider = new GlassSlider(18, 180, 336, 10, 45, controller.Settings.BezelThickness);
            thSlider.ValueChanged += (s, e) => { controller.Settings.BezelThickness = thSlider.Value; thVal.Text = (thSlider.Value / 10.0f).ToString("0.0") + " px"; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(thSlider);

            rightCard.Controls.Add(CreateLabel("Animation dynamique des angles :", 18, 222));
            GlassRoundedComboBox animCombo = CreateComboBox(18, 246, 336, new string[] {
                "Fixe Dépoli (0% CPU)",
                "Ondulation Spectrale Prisme",
                "Respiration Atmosphérique Douce",
                "Synchronisation Pulsation Audio",
                "Lueur Stellaire Subtile",
                "Onde Boréale Décalée"
            }, (int)controller.Settings.CornerAnim);
            animCombo.SelectedIndexChanged += (s, e) => { controller.Settings.CornerAnim = (CornerEffectMode)animCombo.SelectedIndex; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(animCombo);

            rightCard.Controls.Add(CreateLabel("Vitesse de pulsation :", 18, 290));
            Label spdVal = CreateValueLabel(controller.Settings.CornerAnimSpeed + " %", 295, 290);
            rightCard.Controls.Add(spdVal);
            GlassSlider spdSlider = new GlassSlider(18, 314, 336, 30, 200, controller.Settings.CornerAnimSpeed);
            spdSlider.ValueChanged += (s, e) => { controller.Settings.CornerAnimSpeed = spdSlider.Value; spdVal.Text = spdSlider.Value + " %"; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(spdSlider);

            rightCard.Controls.Add(CreateLabel("Aperçu spéculaire en direct :", 18, 355));
            cornerPreviewBox = new GlassRoundedBox(18, 380, 336, 185, 18f);
            cornerPreviewBox.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                Rectangle r = new Rectangle(0, 0, cornerPreviewBox.Width, cornerPreviewBox.Height);
                using (LinearGradientBrush simBg = new LinearGradientBrush(r, Color.FromArgb(28, 48, 85), Color.FromArgb(70, 35, 95), 45f))
                {
                    g.FillRectangle(simBg, r);
                }

                int pr = Math.Min(95, controller.Settings.CornerRadius);
                int sz = pr + 30;
                float ph = controller.GlobalTimeSeconds * (controller.Settings.CornerAnimSpeed / 100f);
                Color[] pal = GlassThemeEngine.GetPalette(controller.Settings, controller.GlobalTimeSeconds);

                using (Bitmap b1 = MonitorCorners.RenderCornerBitmap(sz, pr, CornerPosition.TopLeft, controller.Settings, 0.3f, pal, ph, controller.AudioMeter.CurrentAudioPeak))
                using (Bitmap b2 = MonitorCorners.RenderCornerBitmap(sz, pr, CornerPosition.TopRight, controller.Settings, 0.3f, pal, ph, controller.AudioMeter.CurrentAudioPeak))
                {
                    g.DrawImage(b1, 0, 0);
                    g.DrawImage(b2, cornerPreviewBox.Width - sz, 0);
                }

                using (Font pf = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold))
                using (SolidBrush pb = new SolidBrush(Color.FromArgb(215, 235, 255)))
                {
                    string info = string.Format("Rayon : {0} px | {1}", controller.Settings.CornerRadius,
                        controller.Settings.Curve == CurveGeometry.HyperbolicSquircle ? "Superellipse n=4.2" : "Arc n=2.0");
                    SizeF isz = g.MeasureString(info, pf);
                    g.DrawString(info, pf, pb, (cornerPreviewBox.Width - isz.Width) / 2f, cornerPreviewBox.Height - 32);
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
                "Pulsation Angles Cibles Uniquement"
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
                    };
                    welcomeTimer.Start();
                }
                Application.Run();
            }
        }
    }
}
