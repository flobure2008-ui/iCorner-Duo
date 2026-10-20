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

namespace iCorner
{
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

    public enum GlassColorPalette
    {
        AetherPrism = 0,
        FrostWhiteSilver = 1,
        OceanicCyan = 2,
        EmeraldMint = 3,
        CrimsonVelvet = 4,
        SolarAmber = 5,
        BorealAurora = 6,
        UltravioletMist = 7,
        SakuraBlossom = 8,
        ObsidianMonochrome = 9,
        ChromaticSpectrum = 10,
        CustomGlassPalette = 11
    }

    public class AppSettings
    {
        public bool Enabled = true;
        public int CornerRadius = 36;
        public CurveGeometry Curve = CurveGeometry.HyperbolicSquircle;
        public bool TopLeft = true;
        public bool TopRight = true;
        public bool BottomLeft = true;
        public bool BottomRight = true;
        public bool CornerBezelGlow = true;
        public int BezelGlowOpacity = 48;
        public int BezelThickness = 18;
        public CornerEffectMode CornerAnim = CornerEffectMode.MinimalFrost;
        public int CornerAnimSpeed = 85;
        public GlassColorPalette Theme = GlassColorPalette.AetherPrism;
        public int CustomColor1Argb = Color.FromArgb(255, 110, 220, 255).ToArgb();
        public int CustomColor2Argb = Color.FromArgb(255, 185, 130, 255).ToArgb();

        private static string SettingsPath
        {
            get
            {
                string dir = Path.GetDirectoryName(Application.ExecutablePath);
                return Path.Combine(dir, "icorner_settings.json");
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
                sb.AppendLine(string.Format("  \"Theme\": {0},", (int)Theme));
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
                s.Theme = (GlassColorPalette)ReadInt(json, "Theme", (int)s.Theme);
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
                        Color.FromArgb(255, 110, 215, 255),
                        Color.FromArgb(255, 180, 130, 255),
                        Color.FromArgb(255, 245, 125, 185),
                        Color.FromArgb(255, 255, 190, 120)
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
                    g.FillPath(baseBrush, path);

                using (LinearGradientBrush sheen = new LinearGradientBrush(
                    rect,
                    Color.FromArgb((int)(28 * borderAlphaRatio), 255, 255, 255),
                    Color.FromArgb((int)(12 * borderAlphaRatio), accentTint),
                    35f))
                    g.FillPath(sheen, path);

                using (LinearGradientBrush rimGrad = new LinearGradientBrush(
                    rect,
                    Color.FromArgb((int)(85 * borderAlphaRatio), Interpolate(Color.White, accentTint, 0.35f)),
                    Color.FromArgb((int)(28 * borderAlphaRatio), accentTint),
                    45f))
                using (Pen rimPen = new Pen(rimGrad, 1.2f))
                    g.DrawPath(rimPen, path);
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

    public class LayeredOverlayForm : Form
    {
        public LayeredOverlayForm()
        {
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
                cp.ExStyle |= NativeMethods.WS_EX_TRANSPARENT;
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

    public enum CornerPosition { TopLeft = 0, TopRight = 1, BottomLeft = 2, BottomRight = 3 }

    public class MonitorCorners
    {
        public Screen Monitor;
        public Rectangle Bounds;
        public LayeredOverlayForm[] CornerForms = new LayeredOverlayForm[4];

        public MonitorCorners(Screen screen)
        {
            Monitor = screen;
            Bounds = screen.Bounds;
            for (int i = 0; i < 4; i++)
            {
                CornerForms[i] = new LayeredOverlayForm();
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
            float totalGlow = Math.Min(1.1f, beatExtra);

            int glowExtra = 22;
            int effectiveRadius = r + (int)(beatExtra * 4f);
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

                    using (SolidBrush maskBrush = new SolidBrush(Color.Black))
                        g.FillPath(maskBrush, cornerMask);

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
                    cornerVertex = new PointF(0, 0); pStart = new PointF(0, radius); pEnd = new PointF(radius, 0); break;
                case CornerPosition.TopRight:
                    cornerVertex = new PointF(size, 0); pStart = new PointF(size - radius, 0); pEnd = new PointF(size, radius); break;
                case CornerPosition.BottomLeft:
                    cornerVertex = new PointF(0, size); pStart = new PointF(radius, size); pEnd = new PointF(0, size - radius); break;
                case CornerPosition.BottomRight:
                default:
                    cornerVertex = new PointF(size, size); pStart = new PointF(size, size - radius); pEnd = new PointF(size - radius, size); break;
            }

            path.AddLine(cornerVertex, pStart);
            List<PointF> curvePts = GenerateCurvePoints(size, radius, pos, geom);
            if (curvePts.Count > 1) path.AddCurve(curvePts.ToArray(), 0.5f);
            else path.AddLine(pStart, pEnd);
            path.AddLine(pEnd, cornerVertex);
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath CreateCornerCurvePath(int size, int radius, CornerPosition pos, CurveGeometry geom)
        {
            GraphicsPath path = new GraphicsPath();
            List<PointF> pts = GenerateCurvePoints(size, radius, pos, geom);
            if (pts.Count > 1) path.AddCurve(pts.ToArray(), 0.5f);
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
                    case CornerPosition.TopLeft: theta = Math.PI - (i / (double)steps) * (Math.PI / 2.0); break;
                    case CornerPosition.TopRight: theta = (Math.PI / 2.0) - (i / (double)steps) * (Math.PI / 2.0); break;
                    case CornerPosition.BottomLeft: theta = Math.PI + (i / (double)steps) * (Math.PI / 2.0); break;
                    case CornerPosition.BottomRight:
                    default: theta = (3.0 * Math.PI / 2.0) + (i / (double)steps) * (Math.PI / 2.0); break;
                }

                double cosT = Math.Cos(theta); double sinT = Math.Sin(theta);
                double sCos = Math.Sign(cosT); double sSin = Math.Sign(sinT);
                double xRel = radius * sCos * Math.Pow(Math.Abs(cosT), 2.0 / n);
                double yRel = radius * sSin * Math.Pow(Math.Abs(sinT), 2.0 / n);

                float cx, cy;
                switch (pos)
                {
                    case CornerPosition.TopLeft: cx = radius; cy = radius; break;
                    case CornerPosition.TopRight: cx = size - radius; cy = radius; break;
                    case CornerPosition.BottomLeft: cx = radius; cy = size - radius; break;
                    case CornerPosition.BottomRight:
                    default: cx = size - radius; cy = size - radius; break;
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
            Text = text; Location = new Point(x, y); Size = new Size(w, h);
            baseColor = color; DoubleBuffered = true; Cursor = Cursors.Hand;
            Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold); ForeColor = Color.White;
        }

        protected override void OnMouseEnter(EventArgs e) { isHovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { isHovered = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { isPressed = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { isPressed = false; Invalidate(); base.OnMouseUp(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            Color fill = isPressed ? GlassThemeEngine.Interpolate(baseColor, Color.Black, 0.25f)
                       : isHovered ? GlassThemeEngine.Interpolate(baseColor, Color.White, 0.20f) : baseColor;
            GlassThemeEngine.DrawFrostedGlassSurface(g, r, 10f, fill, fill, 240, 1.0f);
            TextRenderer.DrawText(g, Text, Font, ClientRectangle, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }
    }

    public class GlassCardPanel : Panel
    {
        public string Title { get; set; }
        public GlassCardPanel(int x, int y, int w, int h, string title)
        {
            Location = new Point(x, y); Size = new Size(w, h); Title = title;
            DoubleBuffered = true; BackColor = Color.FromArgb(16, 19, 28);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            GlassThemeEngine.DrawFrostedGlassSurface(g, r, 18f, BackColor, Color.FromArgb(60, 85, 130), 245, 0.85f);
            if (!string.IsNullOrEmpty(Title))
            {
                using (Font tf = new Font("Segoe UI Semibold", 10.2f, FontStyle.Bold))
                using (SolidBrush tb = new SolidBrush(Color.FromArgb(240, 245, 255)))
                    g.DrawString(Title, tf, tb, 18, 16);
                using (Pen sep = new Pen(Color.FromArgb(30, 255, 255, 255), 1f))
                    g.DrawLine(sep, 16, 44, Width - 16, 44);
            }
        }
    }

    public class GlassToggleSwitch : Control
    {
        public bool Checked { get; set; }
        public event EventHandler CheckedChanged;

        public GlassToggleSwitch(string text, int x, int y, int w, bool isChecked)
        {
            Text = text; Location = new Point(x, y); Size = new Size(w, 28);
            Checked = isChecked; DoubleBuffered = true; Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 9.2f); ForeColor = Color.FromArgb(220, 228, 245);
        }

        protected override void OnClick(EventArgs e)
        {
            Checked = !Checked; Invalidate();
            if (CheckedChanged != null) CheckedChanged(this, EventArgs.Empty);
            base.OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF pill = new RectangleF(2, 4, 38, 20);
            Color pillCol = Checked ? Color.FromArgb(40, 140, 250) : Color.FromArgb(32, 38, 52);
            GlassThemeEngine.DrawFrostedGlassSurface(g, pill, 10f, pillCol, Checked ? Color.FromArgb(120, 200, 255) : Color.FromArgb(70, 85, 110), 240, 1.0f);
            float knobX = Checked ? (pill.Right - 18) : (pill.Left + 2);
            RectangleF knob = new RectangleF(knobX, pill.Top + 2, 16, 16);
            using (SolidBrush kb = new SolidBrush(Color.White)) g.FillEllipse(kb, knob);
            Rectangle textRect = new Rectangle(48, 0, Width - 48, Height);
            TextRenderer.DrawText(g, Text, Font, textRect, ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
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
            Location = new Point(x, y); Size = new Size(w, 26); Minimum = min; Maximum = max;
            currentValue = val; DoubleBuffered = true; Cursor = Cursors.Hand;
        }

        protected override void OnMouseDown(MouseEventArgs e) { isDragging = true; UpdateFromMouse(e.X); base.OnMouseDown(e); }
        protected override void OnMouseMove(MouseEventArgs e) { if (isDragging) UpdateFromMouse(e.X); base.OnMouseMove(e); }
        protected override void OnMouseUp(MouseEventArgs e) { isDragging = false; base.OnMouseUp(e); }

        private void UpdateFromMouse(int mouseX)
        {
            float trackLeft = 10f; float trackRight = Width - 10f;
            float ratio = (mouseX - trackLeft) / (trackRight - trackLeft);
            ratio = Math.Max(0f, Math.Min(1f, ratio));
            int newVal = Minimum + (int)(ratio * (Maximum - Minimum));
            if (newVal != currentValue)
            {
                currentValue = newVal; Invalidate();
                if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            float trackY = Height / 2f - 3f; float trackH = 6f; float trackW = Width - 20f;
            RectangleF trackBg = new RectangleF(10f, trackY, trackW, trackH);
            using (GraphicsPath trackPath = GlassThemeEngine.CreateRoundedRect(trackBg, 3f))
            using (SolidBrush b = new SolidBrush(Color.FromArgb(28, 34, 48))) g.FillPath(b, trackPath);

            float ratio = (Maximum > Minimum) ? (float)(currentValue - Minimum) / (Maximum - Minimum) : 0f;
            float fillW = trackW * ratio;
            if (fillW > 4f)
            {
                RectangleF fillRect = new RectangleF(10f, trackY, fillW, trackH);
                using (GraphicsPath fillPath = GlassThemeEngine.CreateRoundedRect(fillRect, 3f))
                using (LinearGradientBrush fgb = new LinearGradientBrush(fillRect, Color.FromArgb(50, 130, 245), Color.FromArgb(100, 200, 255), 0f))
                    g.FillPath(fgb, fillPath);
            }
            float knobCenterX = 10f + fillW;
            RectangleF knob = new RectangleF(knobCenterX - 8f, Height / 2f - 8f, 16f, 16f);
            using (SolidBrush kb = new SolidBrush(Color.White)) g.FillEllipse(kb, knob);
            using (Pen kp = new Pen(Color.FromArgb(60, 140, 255), 2f)) g.DrawEllipse(kp, knob);
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
            Location = new Point(x, y); Size = new Size(w, h); DoubleBuffered = true; BackColor = Color.FromArgb(16, 20, 30);
            innerBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(16, 20, 30), ForeColor = Color.FromArgb(235, 240, 255),
                Font = new Font("Segoe UI", 9.2f), Location = new Point(10, (h - 22) / 2), Size = new Size(w - 20, 22)
            };
            if (items != null) innerBox.Items.AddRange(items);
            if (selectedIdx >= 0 && selectedIdx < innerBox.Items.Count) innerBox.SelectedIndex = selectedIdx;
            Controls.Add(innerBox);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            GlassThemeEngine.DrawFrostedGlassSurface(g, r, 12f, BackColor, Color.FromArgb(60, 85, 130), 245, 0.8f);
        }
    }

    public class GlassRoundedBox : Panel
    {
        public float CornerRadius { get; set; }
        public GlassRoundedBox(int x, int y, int w, int h, float radius = 16f)
        {
            Location = new Point(x, y); Size = new Size(w, h); CornerRadius = radius;
            DoubleBuffered = true; BackColor = Color.FromArgb(16, 20, 30);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            GlassThemeEngine.DrawFrostedGlassSurface(g, r, CornerRadius, BackColor, Color.FromArgb(60, 85, 130), 245, 0.8f);
        }
    }

    public class OverlayController : IDisposable
    {
        public AppSettings Settings;
        public List<MonitorCorners> Monitors = new List<MonitorCorners>();
        public CoreAudioPeakMeter AudioMeter;
        private System.Windows.Forms.Timer tickTimer;
        private int topMostTickCounter = 0;
        private int cornerAnimTickCounter = 0;
        private long startTicks = DateTime.UtcNow.Ticks;
        private NotifyIcon trayIcon;
        private SettingsForm settingsForm;

        public OverlayController()
        {
            Settings = AppSettings.Load();
            AudioMeter = new CoreAudioPeakMeter();
            RebuildMonitors();
            SetupTrayIcon();

            SystemEvents.DisplaySettingsChanged += (s, e) => RebuildMonitors();

            tickTimer = new System.Windows.Forms.Timer { Interval = 16 };
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
        }

        public void RefreshAllCorners()
        {
            float t = GlobalTimeSeconds;
            Color[] palette = GlassThemeEngine.GetPalette(Settings, t);
            foreach (var m in Monitors) m.UpdateCorners(Settings, palette, t, AudioMeter.CurrentAudioPeak);
            Settings.Save();
        }

        private void OnTick(object sender, EventArgs e)
        {
            float t = GlobalTimeSeconds;
            AudioMeter.UpdateTick();
            Color[] palette = GlassThemeEngine.GetPalette(Settings, t);

            cornerAnimTickCounter++;
            bool needsAnim = (Settings.CornerAnim != CornerEffectMode.MinimalFrost || Settings.Theme == GlassColorPalette.ChromaticSpectrum);
            if (needsAnim && cornerAnimTickCounter % 2 == 0)
            {
                float phase = t * (Settings.CornerAnimSpeed / 100f);
                foreach (var m in Monitors) m.UpdateCorners(Settings, palette, phase, AudioMeter.CurrentAudioPeak);
            }

            topMostTickCounter++;
            if (topMostTickCounter >= 90)
            {
                topMostTickCounter = 0;
                foreach (var m in Monitors) m.EnsureTopMost();
            }
        }

        private void SetupTrayIcon()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "iCorner — Coins Organiques Multi-Écrans";
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Visible = true;

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem openItem = new ToolStripMenuItem("Ouvrir iCorner");
            openItem.Font = new Font(openItem.Font, FontStyle.Bold);
            openItem.Click += (s, e) => ShowSettingsWindow();

            ToolStripMenuItem toggleItem = new ToolStripMenuItem("Activer / Désactiver");
            toggleItem.Click += (s, e) => { Settings.Enabled = !Settings.Enabled; RefreshAllCorners(); };

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Quitter iCorner");
            exitItem.Click += (s, e) => { trayIcon.Visible = false; Application.Exit(); };

            menu.Items.Add(openItem);
            menu.Items.Add(toggleItem);
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
            foreach (var m in Monitors) m.Dispose();
            if (trayIcon != null) { trayIcon.Visible = false; trayIcon.Dispose(); }
        }
    }

    public class SettingsForm : Form
    {
        private OverlayController controller;
        private GlassRoundedBox cornerPreviewBox;
        private GlassRoundedComboBox themeCombo;
        private System.Windows.Forms.Timer uiPreviewTimer;

        public SettingsForm(OverlayController ctrl)
        {
            controller = ctrl;
            Text = "iCorner — Coins d'Écran Organiques";
            Size = new Size(820, 680);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(11, 13, 20);
            ForeColor = Color.FromArgb(238, 242, 252);
            Font = new Font("Segoe UI", 9.2f);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            BuildUI();

            uiPreviewTimer = new System.Windows.Forms.Timer { Interval = 40 };
            uiPreviewTimer.Tick += (s, e) =>
            {
                if (Visible && WindowState != FormWindowState.Minimized && cornerPreviewBox != null)
                    cornerPreviewBox.Invalidate();
            };
            uiPreviewTimer.Start();
        }

        protected override CreateParams CreateParams
        {
            get { CreateParams cp = base.CreateParams; cp.ExStyle |= 0x02000000; return cp; }
        }

        private void BuildUI()
        {
            // Header
            DoubleBufferedPanel header = new DoubleBufferedPanel { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(14, 18, 30) };
            header.Paint += (s, e) =>
            {
                Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = header.ClientRectangle;
                using (LinearGradientBrush grad = new LinearGradientBrush(r, Color.FromArgb(14, 18, 30), Color.FromArgb(20, 14, 32), 0f))
                    g.FillRectangle(grad, r);

                Color[] pal = GlassThemeEngine.GetPalette(controller.Settings, controller.GlobalTimeSeconds);
                using (Font tf = new Font("Segoe UI Semibold", 15f, FontStyle.Bold))
                using (Font sf = new Font("Segoe UI", 8.8f))
                using (SolidBrush wb = new SolidBrush(Color.White))
                using (SolidBrush ab = new SolidBrush(pal[0]))
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(160, 175, 205)))
                {
                    g.DrawString("iCorner", tf, wb, 20, 12);
                    SizeF sz = g.MeasureString("iCorner", tf);
                    g.DrawString("Pure", tf, ab, 20 + sz.Width - 4, 12);
                    g.DrawString("Arrondis d'écran organiques haute définition pour Windows • 0% CPU", sf, sb, 22, 42);
                }
            };
            Controls.Add(header);

            // Left Card: Courbures & Dalles
            GlassCardPanel leftCard = new GlassCardPanel(18, 86, 375, 535, "✦ Géométrie Pure & Dalles Moniteurs");
            Controls.Add(leftCard);

            GlassToggleSwitch enableChk = new GlassToggleSwitch("Activer les bords arrondis", 18, 54, 330, controller.Settings.Enabled);
            enableChk.CheckedChanged += (s, e) => { controller.Settings.Enabled = enableChk.Checked; controller.RefreshAllCorners(); };
            leftCard.Controls.Add(enableChk);

            leftCard.Controls.Add(CreateLabel("Rayon de courbure (0 à 110 px) :", 18, 96));
            Label radVal = CreateValueLabel(controller.Settings.CornerRadius + " px", 300, 96);
            leftCard.Controls.Add(radVal);

            GlassSlider radSlider = new GlassSlider(18, 120, 336, 0, 110, controller.Settings.CornerRadius);
            radSlider.ValueChanged += (s, e) => { controller.Settings.CornerRadius = radSlider.Value; radVal.Text = radSlider.Value + " px"; controller.RefreshAllCorners(); };
            leftCard.Controls.Add(radSlider);

            leftCard.Controls.Add(CreateLabel("Profil géométrique :", 18, 160));
            GlassRoundedComboBox curveCombo = new GlassRoundedComboBox(18, 184, 336, 34, new string[] {
                "Superellipse Hyper-Continue (n=4.2)",
                "Arc Circulaire Classique (n=2.0)"
            }, (int)controller.Settings.Curve);
            curveCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Curve = (CurveGeometry)curveCombo.SelectedIndex; controller.RefreshAllCorners(); };
            leftCard.Controls.Add(curveCombo);

            leftCard.Controls.Add(CreateLabel("Angles actifs :", 18, 228));
            GlassToggleSwitch tl = new GlassToggleSwitch("Haut-Gauche", 18, 254, 155, controller.Settings.TopLeft);
            GlassToggleSwitch tr = new GlassToggleSwitch("Haut-Droit", 190, 254, 155, controller.Settings.TopRight);
            GlassToggleSwitch bl = new GlassToggleSwitch("Bas-Gauche", 18, 288, 155, controller.Settings.BottomLeft);
            GlassToggleSwitch br = new GlassToggleSwitch("Bas-Droit", 190, 288, 155, controller.Settings.BottomRight);
            EventHandler ch = (s, e) => { controller.Settings.TopLeft = tl.Checked; controller.Settings.TopRight = tr.Checked; controller.Settings.BottomLeft = bl.Checked; controller.Settings.BottomRight = br.Checked; controller.RefreshAllCorners(); };
            tl.CheckedChanged += ch; tr.CheckedChanged += ch; bl.CheckedChanged += ch; br.CheckedChanged += ch;
            leftCard.Controls.Add(tl); leftCard.Controls.Add(tr); leftCard.Controls.Add(bl); leftCard.Controls.Add(br);

            // Right Card: Liseré & Effets
            GlassCardPanel rightCard = new GlassCardPanel(412, 86, 375, 535, "✨ Liseré Spéculaire & Thèmes");
            Controls.Add(rightCard);

            GlassToggleSwitch glowChk = new GlassToggleSwitch("Liseré spéculaire & biseau lumineux", 18, 54, 330, controller.Settings.CornerBezelGlow);
            glowChk.CheckedChanged += (s, e) => { controller.Settings.CornerBezelGlow = glowChk.Checked; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(glowChk);

            rightCard.Controls.Add(CreateLabel("Opacité du liseré biseauté :", 18, 96));
            Label glowVal = CreateValueLabel(controller.Settings.BezelGlowOpacity + " %", 300, 96);
            rightCard.Controls.Add(glowVal);
            GlassSlider glowSlider = new GlassSlider(18, 120, 336, 0, 100, controller.Settings.BezelGlowOpacity);
            glowSlider.ValueChanged += (s, e) => { controller.Settings.BezelGlowOpacity = glowSlider.Value; glowVal.Text = glowSlider.Value + " %"; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(glowSlider);

            rightCard.Controls.Add(CreateLabel("Animation des angles :", 18, 160));
            GlassRoundedComboBox animCombo = new GlassRoundedComboBox(18, 184, 336, 34, new string[] {
                "Fixe Dépoli (0% CPU)",
                "Ondulation Spectrale",
                "Respiration Atmosphérique",
                "Synchronisation Son Système",
                "Lueur Stellaire Subtile",
                "Onde Boréale Décalée"
            }, (int)controller.Settings.CornerAnim);
            animCombo.SelectedIndexChanged += (s, e) => { controller.Settings.CornerAnim = (CornerEffectMode)animCombo.SelectedIndex; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(animCombo);

            rightCard.Controls.Add(CreateLabel("Palette de couleur :", 18, 228));
            themeCombo = new GlassRoundedComboBox(18, 252, 336, 34, new string[] {
                "Prisme Éther (Cyan / Lavande / Rose)",
                "Verre Givre & Argent Pur",
                "Océan Polaire",
                "Émeraude Menthe Givrée",
                "Velours Rubis & Corail",
                "Ambre Solaire & Champagne",
                "Aurore Boréale",
                "Brume Ultraviolette",
                "Fleur de Sakura",
                "Monochrome Obsidienne",
                "Spectre Continu 360°",
                "Palette Personnalisée"
            }, (int)controller.Settings.Theme);
            themeCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Theme = (GlassColorPalette)themeCombo.SelectedIndex; controller.RefreshAllCorners(); };
            rightCard.Controls.Add(themeCombo);

            // Preview Box
            rightCard.Controls.Add(CreateLabel("Aperçu en direct :", 18, 298));
            cornerPreviewBox = new GlassRoundedBox(18, 322, 336, 180, 18f);
            cornerPreviewBox.Paint += (s, e) =>
            {
                Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias; g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                Rectangle r = new Rectangle(0, 0, cornerPreviewBox.Width, cornerPreviewBox.Height);
                using (LinearGradientBrush simBg = new LinearGradientBrush(r, Color.FromArgb(28, 48, 85), Color.FromArgb(70, 35, 95), 45f))
                    g.FillRectangle(simBg, r);

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
            };
            rightCard.Controls.Add(cornerPreviewBox);
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label { Text = text, Location = new Point(x, y), AutoSize = true, BackColor = Color.FromArgb(16, 19, 28), ForeColor = Color.FromArgb(195, 205, 228) };
        }

        private Label CreateValueLabel(string text, int x, int y)
        {
            return new Label { Text = text, Location = new Point(x, y), AutoSize = true, BackColor = Color.FromArgb(16, 19, 28), Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold), ForeColor = Color.FromArgb(115, 210, 255) };
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) { e.Cancel = true; Hide(); return; }
            base.OnFormClosing(e);
        }
    }

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
                if (!startInTray) controller.ShowSettingsWindow();
                Application.Run();
            }
        }
    }
}
