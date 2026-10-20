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

namespace iBridge
{
    public enum PortalTransitionStyle
    {
        GlassMorphCombo = 0,        // Pont Liquide Verre + Aura Prismatique + Particules
        LiquidGlassBridge = 1,      // Goutte liquide minimaliste entre les deux écrans
        PrismaticAuraWave = 2,      // Halo atmosphérique diffus sur la bordure
        QuantumArcPortal = 3,       // Filament énergétique fin et épuré
        StardustDrift = 4,          // Nuage de micro-particules lumineuses
        RippleResonance = 5,        // Ondes concentriques minimalistes
        LaserHorizonLine = 6        // Ligne optique fine haute précision
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
                return Path.Combine(dir, "ibridge_settings.json");
            }
        }

        public void Save()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine(string.Format("  \"Enabled\": {0},", Enabled ? "true" : "false"));
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
            if (t < 0f) t = 0f; if (t > 1f) t = 1f;
            return Color.FromArgb((int)(a.A + (b.A - a.A) * t), (int)(a.R + (b.R - a.R) * t), (int)(a.G + (b.G - a.G) * t), (int)(a.B + (b.B - a.B) * t));
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

        public static void DrawFrostedGlassSurface(Graphics g, RectangleF rect, float cornerRadius, Color baseDarkTint, Color accentTint, int glassOpacity = 238, float borderAlphaRatio = 1.0f)
        {
            if (rect.Width <= 2f || rect.Height <= 2f) return;
            using (GraphicsPath path = CreateRoundedRect(rect, cornerRadius))
            {
                using (SolidBrush baseBrush = new SolidBrush(Color.FromArgb(glassOpacity, baseDarkTint)))
                    g.FillPath(baseBrush, path);
                using (LinearGradientBrush sheen = new LinearGradientBrush(rect, Color.FromArgb((int)(28 * borderAlphaRatio), 255, 255, 255), Color.FromArgb((int)(12 * borderAlphaRatio), accentTint), 35f))
                    g.FillPath(sheen, path);
                using (LinearGradientBrush rimGrad = new LinearGradientBrush(rect, Color.FromArgb((int)(85 * borderAlphaRatio), Interpolate(Color.White, accentTint, 0.35f)), Color.FromArgb((int)(28 * borderAlphaRatio), accentTint), 45f))
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
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        [DllImport("shcore.dll")]
        public static extern int SetProcessDpiAwareness(int value);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDpiAwarenessContext(IntPtr dpiContext);

        public static void EnablePerMonitorDpiAwareness()
        {
            try { SetProcessDpiAwarenessContext(new IntPtr(-4)); return; } catch { }
            try { SetProcessDpiAwareness(2); return; } catch { }
            try { SetProcessDPIAware(); } catch { }
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
                cp.ExStyle |= NativeMethods.WS_EX_LAYERED | NativeMethods.WS_EX_TOOLWINDOW | NativeMethods.WS_EX_NOACTIVATE | NativeMethods.WS_EX_TOPMOST | NativeMethods.WS_EX_TRANSPARENT;
                return cp;
            }
        }

        protected override bool ShowWithoutActivation { get { return true; } }

        public void SetBitmap(Bitmap bitmap, int x, int y, byte opacity = 255)
        {
            if (bitmap == null || !IsHandleCreated || bitmap.PixelFormat != PixelFormat.Format32bppArgb) return;
            IntPtr screenDC = NativeMethods.GetDC(IntPtr.Zero);
            IntPtr memDC = NativeMethods.CreateCompatibleDC(screenDC);
            IntPtr hBitmap = IntPtr.Zero, oldBitmap = IntPtr.Zero;
            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBitmap = NativeMethods.SelectObject(memDC, hBitmap);
                NativeMethods.SIZE size = new NativeMethods.SIZE(bitmap.Width, bitmap.Height);
                NativeMethods.POINT pointSource = new NativeMethods.POINT(0, 0);
                NativeMethods.POINT topPos = new NativeMethods.POINT(x, y);
                NativeMethods.BLENDFUNCTION blend = new NativeMethods.BLENDFUNCTION { BlendOp = NativeMethods.AC_SRC_OVER, BlendFlags = 0, SourceConstantAlpha = opacity, AlphaFormat = NativeMethods.AC_SRC_ALPHA };
                NativeMethods.UpdateLayeredWindow(Handle, screenDC, ref topPos, ref size, memDC, ref pointSource, 0, ref blend, NativeMethods.ULW_ALPHA);
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

    public enum ScreenEdge { Left, Right, Top, Bottom }

    public class ScreenTransitionAnimator : IDisposable
    {
        private LayeredOverlayForm entryEdgeForm;
        private LayeredOverlayForm exitEdgeForm;
        private bool isAnimating = false;
        private bool isProximityActive = false;
        private long animStartTicks = 0;
        private int animDurationMs = 850;
        private Rectangle entryScreenBounds, exitScreenBounds;
        private ScreenEdge entryEdge, exitEdge;
        private Point crossingPoint;

        public ScreenTransitionAnimator()
        {
            entryEdgeForm = new LayeredOverlayForm();
            exitEdgeForm = new LayeredOverlayForm();
        }

        public bool IsBusy { get { return isAnimating || isProximityActive; } }

        public void TriggerTransition(Rectangle fromBounds, Rectangle toBounds, Point cursorPos, AppSettings settings)
        {
            if (!settings.Enabled) return;
            exitScreenBounds = fromBounds;
            entryScreenBounds = toBounds;
            crossingPoint = cursorPos;
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
            if (!settings.Enabled || !settings.EnableMagneticProximity || proximityFactor <= 0.02f)
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
                return;
            }
            float scale = settings.IntensityPercent / 100f;
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

        private void RenderEdgePortal(LayeredOverlayForm form, Rectangle screenBounds, ScreenEdge edge, Point cursor, float bulgeAmount, float alphaFactor, float scale, PortalTransitionStyle style, Color[] palette, bool isExitSide, float rawTime)
        {
            if (alphaFactor <= 0.01f || bulgeAmount <= 0.005f) { form.Hide(); return; }
            bool verticalEdge = (edge == ScreenEdge.Left || edge == ScreenEdge.Right);
            int portalLength = (int)(Math.Min(verticalEdge ? screenBounds.Height : screenBounds.Width, (int)(660 * scale)));
            int portalDepth = (int)(180 * scale);
            if (portalLength < 120) portalLength = 120;
            if (portalDepth < 60) portalDepth = 60;
            int winW = verticalEdge ? portalDepth : portalLength, winH = verticalEdge ? portalLength : portalDepth;
            int winX = screenBounds.Left, winY = screenBounds.Top;

            if (edge == ScreenEdge.Left) { winX = screenBounds.Left; winY = Clamp(cursor.Y - winH / 2, screenBounds.Top, screenBounds.Bottom - winH); }
            else if (edge == ScreenEdge.Right) { winX = screenBounds.Right - winW; winY = Clamp(cursor.Y - winH / 2, screenBounds.Top, screenBounds.Bottom - winH); }
            else if (edge == ScreenEdge.Top) { winX = Clamp(cursor.X - winW / 2, screenBounds.Left, screenBounds.Right - winW); winY = screenBounds.Top; }
            else { winX = Clamp(cursor.X - winW / 2, screenBounds.Left, screenBounds.Right - winW); winY = screenBounds.Bottom - winH; }

            using (Bitmap bmp = new Bitmap(winW, winH, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias; g.PixelOffsetMode = PixelOffsetMode.HighQuality; g.Clear(Color.Transparent);
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
                    g.FillPath(glassBody, liquidPath);
                int rimAlpha = (int)(alpha * 195);
                using (Pen specularRim = new Pen(Color.FromArgb(Math.Min(255, rimAlpha), GlassThemeEngine.Interpolate(Color.White, palette[0], 0.45f)), 1.6f))
                    g.DrawPath(specularRim, liquidPath);
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

        private static int Clamp(int v, int min, int max) { return (v < min) ? min : ((v > max) ? max : v); }
        private static float EaseOutCubic(float x) { return 1f - (float)Math.Pow(1.0 - x, 3.0); }
        private static float EaseInOutCubic(float x) { return (x < 0.5f) ? 4f * x * x * x : 1f - (float)Math.Pow(-2.0 * x + 2.0, 3.0) / 2f; }
        private static float EaseOutBack(float x) { float c1 = 1.70158f; float c3 = c1 + 1f; return 1f + c3 * (float)Math.Pow(x - 1f, 3.0) + c1 * (float)Math.Pow(x - 1f, 2.0); }

        public void Dispose()
        {
            if (entryEdgeForm != null) { entryEdgeForm.Dispose(); entryEdgeForm = null; }
            if (exitEdgeForm != null) { exitEdgeForm.Dispose(); exitEdgeForm = null; }
        }
    }

    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel() { DoubleBuffered = true; SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true); UpdateStyles(); }
    }

    public class GlassButton : Control
    {
        private bool isHovered = false, isPressed = false;
        private Color baseColor;
        public GlassButton(string text, int x, int y, int w, int h, Color color)
        {
            Text = text; Location = new Point(x, y); Size = new Size(w, h); baseColor = color;
            DoubleBuffered = true; Cursor = Cursors.Hand; Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold); ForeColor = Color.White;
        }
        protected override void OnMouseEnter(EventArgs e) { isHovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { isHovered = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { isPressed = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { isPressed = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            Color fill = isPressed ? GlassThemeEngine.Interpolate(baseColor, Color.Black, 0.25f) : (isHovered ? GlassThemeEngine.Interpolate(baseColor, Color.White, 0.20f) : baseColor);
            GlassThemeEngine.DrawFrostedGlassSurface(g, r, 10f, fill, fill, 240, 1.0f);
            TextRenderer.DrawText(g, Text, Font, ClientRectangle, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }
    }

    public class GlassCardPanel : Panel
    {
        public string Title { get; set; }
        public GlassCardPanel(int x, int y, int w, int h, string title) { Location = new Point(x, y); Size = new Size(w, h); Title = title; DoubleBuffered = true; BackColor = Color.FromArgb(16, 19, 28); }
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
            Text = text; Location = new Point(x, y); Size = new Size(w, 28); Checked = isChecked;
            DoubleBuffered = true; Cursor = Cursors.Hand; Font = new Font("Segoe UI", 9.2f); ForeColor = Color.FromArgb(220, 228, 245);
        }
        protected override void OnClick(EventArgs e) { Checked = !Checked; Invalidate(); if (CheckedChanged != null) CheckedChanged(this, EventArgs.Empty); base.OnClick(e); }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF pill = new RectangleF(2, 4, 38, 20);
            Color pillCol = Checked ? Color.FromArgb(40, 140, 250) : Color.FromArgb(32, 38, 52);
            GlassThemeEngine.DrawFrostedGlassSurface(g, pill, 10f, pillCol, Checked ? Color.FromArgb(120, 200, 255) : Color.FromArgb(70, 85, 110), 240, 1.0f);
            float knobX = Checked ? (pill.Right - 18) : (pill.Left + 2);
            using (SolidBrush kb = new SolidBrush(Color.White)) g.FillEllipse(kb, new RectangleF(knobX, pill.Top + 2, 16, 16));
            TextRenderer.DrawText(g, Text, Font, new Rectangle(48, 0, Width - 48, Height), ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }
    }

    public class GlassSlider : Control
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        private int currentValue;
        public int Value { get { return currentValue; } set { currentValue = Math.Max(Minimum, Math.Min(Maximum, value)); Invalidate(); } }
        public event EventHandler ValueChanged;
        private bool isDragging = false;
        public GlassSlider(int x, int y, int w, int min, int max, int val)
        {
            Location = new Point(x, y); Size = new Size(w, 26); Minimum = min; Maximum = max; currentValue = val; DoubleBuffered = true; Cursor = Cursors.Hand;
        }
        protected override void OnMouseDown(MouseEventArgs e) { isDragging = true; UpdateFromMouse(e.X); base.OnMouseDown(e); }
        protected override void OnMouseMove(MouseEventArgs e) { if (isDragging) UpdateFromMouse(e.X); base.OnMouseMove(e); }
        protected override void OnMouseUp(MouseEventArgs e) { isDragging = false; base.OnMouseUp(e); }
        private void UpdateFromMouse(int mouseX)
        {
            float ratio = Math.Max(0f, Math.Min(1f, (mouseX - 10f) / (Width - 20f)));
            int newVal = Minimum + (int)(ratio * (Maximum - Minimum));
            if (newVal != currentValue) { currentValue = newVal; Invalidate(); if (ValueChanged != null) ValueChanged(this, EventArgs.Empty); }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            float trackY = Height / 2f - 3f, trackW = Width - 20f;
            RectangleF trackBg = new RectangleF(10f, trackY, trackW, 6f);
            using (GraphicsPath trackPath = GlassThemeEngine.CreateRoundedRect(trackBg, 3f))
            using (SolidBrush b = new SolidBrush(Color.FromArgb(28, 34, 48))) g.FillPath(b, trackPath);
            float ratio = (Maximum > Minimum) ? (float)(currentValue - Minimum) / (Maximum - Minimum) : 0f;
            float fillW = trackW * ratio;
            if (fillW > 4f)
            {
                RectangleF fillRect = new RectangleF(10f, trackY, fillW, 6f);
                using (GraphicsPath fillPath = GlassThemeEngine.CreateRoundedRect(fillRect, 3f))
                using (LinearGradientBrush fgb = new LinearGradientBrush(fillRect, Color.FromArgb(50, 130, 245), Color.FromArgb(100, 200, 255), 0f))
                    g.FillPath(fgb, fillPath);
            }
            RectangleF knob = new RectangleF(10f + fillW - 8f, Height / 2f - 8f, 16f, 16f);
            using (SolidBrush kb = new SolidBrush(Color.White)) g.FillEllipse(kb, knob);
            using (Pen kp = new Pen(Color.FromArgb(60, 140, 255), 2f)) g.DrawEllipse(kp, knob);
        }
    }

    public class GlassRoundedComboBox : Panel
    {
        private ComboBox innerBox;
        public int SelectedIndex { get { return innerBox != null ? innerBox.SelectedIndex : -1; } set { if (innerBox != null) innerBox.SelectedIndex = value; } }
        public event EventHandler SelectedIndexChanged { add { if (innerBox != null) innerBox.SelectedIndexChanged += value; } remove { if (innerBox != null) innerBox.SelectedIndexChanged -= value; } }
        public GlassRoundedComboBox(int x, int y, int w, int h, string[] items, int selectedIdx)
        {
            Location = new Point(x, y); Size = new Size(w, h); DoubleBuffered = true; BackColor = Color.FromArgb(16, 20, 30);
            innerBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(16, 20, 30), ForeColor = Color.FromArgb(235, 240, 255), Font = new Font("Segoe UI", 9.2f), Location = new Point(10, (h - 22) / 2), Size = new Size(w - 20, 22) };
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

    public class OverlayController : IDisposable
    {
        public AppSettings Settings;
        public ScreenTransitionAnimator Animator;
        private System.Windows.Forms.Timer tickTimer;
        private int lastMonitorIndex = -1;
        private long startTicks = DateTime.UtcNow.Ticks;
        private NotifyIcon trayIcon;
        private SettingsForm settingsForm;

        public OverlayController()
        {
            Settings = AppSettings.Load();
            Animator = new ScreenTransitionAnimator();
            SetupTrayIcon();

            tickTimer = new System.Windows.Forms.Timer { Interval = 16 };
            tickTimer.Tick += OnTick;
            tickTimer.Start();
        }

        public float GlobalTimeSeconds { get { return (float)((DateTime.UtcNow.Ticks - startTicks) / 10000000.0); } }

        private int FindMonitorIndex(Point pt)
        {
            Screen[] screens = Screen.AllScreens;
            for (int i = 0; i < screens.Length; i++)
                if (screens[i].Bounds.Contains(pt)) return i;
            return 0;
        }

        private void OnTick(object sender, EventArgs e)
        {
            float t = GlobalTimeSeconds;
            Color[] palette = GlassThemeEngine.GetPalette(Settings, t);
            Screen[] screens = Screen.AllScreens;

            Point cursorPt = Point.Empty;
            NativeMethods.POINT rawPt;
            if (NativeMethods.GetCursorPos(out rawPt))
                cursorPt = new Point(rawPt.X, rawPt.Y);

            int currentIdx = (screens.Length > 0) ? FindMonitorIndex(cursorPt) : -1;

            if (screens.Length > 0 && currentIdx >= 0)
            {
                if (lastMonitorIndex != -1 && currentIdx != lastMonitorIndex && lastMonitorIndex < screens.Length)
                {
                    Animator.TriggerTransition(screens[lastMonitorIndex].Bounds, screens[currentIdx].Bounds, cursorPt, Settings);
                }
                else if (screens.Length > 1)
                {
                    CheckMagneticProximity(currentIdx, cursorPt, palette, screens);
                }
                lastMonitorIndex = currentIdx;
            }

            Animator.StepAnimation(Settings, palette);
        }

        private void CheckMagneticProximity(int currentIdx, Point pt, Color[] palette, Screen[] screens)
        {
            Rectangle cur = screens[currentIdx].Bounds;
            int threshold = 68;
            float bestProx = 0f;
            ScreenEdge bestEdge = ScreenEdge.Left;

            for (int otherIdx = 0; otherIdx < screens.Length; otherIdx++)
            {
                if (otherIdx == currentIdx) continue;
                Rectangle other = screens[otherIdx].Bounds;

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
            Screen[] screens = Screen.AllScreens;
            if (screens.Length == 0) return;
            int targetIdx = 0;
            NativeMethods.POINT rawPt;
            if (NativeMethods.GetCursorPos(out rawPt))
                targetIdx = FindMonitorIndex(new Point(rawPt.X, rawPt.Y));
            int fromIdx = (screens.Length > 1) ? ((targetIdx + 1) % screens.Length) : targetIdx;

            Rectangle toB = screens[targetIdx].Bounds;
            Rectangle fromB = screens[fromIdx].Bounds;
            Point demoPoint = new Point(toB.Left + 4, toB.Top + toB.Height / 2);
            if (screens.Length > 1 && fromB.Left > toB.Left)
                demoPoint = new Point(toB.Right - 4, toB.Top + toB.Height / 2);

            Animator.TriggerTransition(fromB, toB, demoPoint, Settings);
        }

        private void SetupTrayIcon()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "iBridge — Passerelle Fluide Inter-Écrans";
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Visible = true;

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem openItem = new ToolStripMenuItem("Ouvrir iBridge");
            openItem.Font = new Font(openItem.Font, FontStyle.Bold);
            openItem.Click += (s, e) => ShowSettingsWindow();

            ToolStripMenuItem demoItem = new ToolStripMenuItem("Tester la Transition");
            demoItem.Click += (s, e) => TriggerDemoTransition();

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Quitter iBridge");
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
            if (trayIcon != null) { trayIcon.Visible = false; trayIcon.Dispose(); }
        }
    }

    public class SettingsForm : Form
    {
        private OverlayController controller;

        public SettingsForm(OverlayController ctrl)
        {
            controller = ctrl;
            Text = "iBridge — Passerelle Liquide Inter-Écrans";
            Size = new Size(820, 640);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(11, 13, 20);
            ForeColor = Color.FromArgb(238, 242, 252);
            Font = new Font("Segoe UI", 9.2f);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            BuildUI();
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
                    g.DrawString("iBridge", tf, wb, 20, 12);
                    SizeF sz = g.MeasureString("iBridge", tf);
                    g.DrawString("Duo", tf, ab, 20 + sz.Width - 4, 12);
                    g.DrawString("Passerelle optique et halo magnétique au passage du curseur entre écrans", sf, sb, 22, 42);
                }
            };
            Controls.Add(header);

            // Left Card
            GlassCardPanel leftCard = new GlassCardPanel(18, 86, 375, 495, "🌊 Passerelle & Effets Inter-Écrans");
            Controls.Add(leftCard);

            GlassToggleSwitch enTrans = new GlassToggleSwitch("Activer les transitions d'écran", 18, 54, 330, controller.Settings.Enabled);
            enTrans.CheckedChanged += (s, e) => { controller.Settings.Enabled = enTrans.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(enTrans);

            GlassToggleSwitch magChk = new GlassToggleSwitch("Halo magnétique à l'approche de la bordure", 18, 88, 330, controller.Settings.EnableMagneticProximity);
            magChk.CheckedChanged += (s, e) => { controller.Settings.EnableMagneticProximity = magChk.Checked; controller.Settings.Save(); };
            leftCard.Controls.Add(magChk);

            leftCard.Controls.Add(CreateLabel("Style visuel :", 18, 132));
            GlassRoundedComboBox styleCombo = new GlassRoundedComboBox(18, 156, 336, 34, new string[] {
                "Fusion Glassmorphique (Pont + Halo + Étoiles)",
                "Pont Liquide en Verre Dépoli",
                "Onde Prismatique Atmosphérique",
                "Filaments Quantiques",
                "Nuage de Poussière Stellaire",
                "Résonance Concentrique (Ripples)",
                "Ligne Optique Horizon"
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

            GlassButton testBtn = new GlassButton("Tester la traversée d'écran", 18, 360, 336, 40, Color.FromArgb(40, 115, 215));
            testBtn.Click += (s, e) => controller.TriggerDemoTransition();
            leftCard.Controls.Add(testBtn);

            // Right Card: Palettes
            GlassCardPanel rightCard = new GlassCardPanel(412, 86, 375, 495, "🎨 Palettes Chromatiques en Verre");
            Controls.Add(rightCard);

            rightCard.Controls.Add(CreateLabel("Palette chromatique :", 18, 54));
            GlassRoundedComboBox themeCombo = new GlassRoundedComboBox(18, 78, 336, 34, new string[] {
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
            themeCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Theme = (GlassColorPalette)themeCombo.SelectedIndex; controller.Settings.Save(); controller.TriggerDemoTransition(); };
            rightCard.Controls.Add(themeCombo);

            rightCard.Controls.Add(CreateLabel("Préréglages fluides :", 18, 140));
            rightCard.Controls.Add(CreatePresetBtn("Goutte d'Eau Minimaliste", 18, 168, 336, 36, Color.FromArgb(32, 38, 56), PortalTransitionStyle.LiquidGlassBridge, GlassColorPalette.FrostWhiteSilver));
            rightCard.Controls.Add(CreatePresetBtn("Aurore Boréale Magnétique", 18, 214, 336, 36, Color.FromArgb(28, 85, 115), PortalTransitionStyle.PrismaticAuraWave, GlassColorPalette.BorealAurora));
            rightCard.Controls.Add(CreatePresetBtn("Poussière Stellaire Cosmique", 18, 260, 336, 36, Color.FromArgb(65, 55, 125), PortalTransitionStyle.StardustDrift, GlassColorPalette.AetherPrism));
            rightCard.Controls.Add(CreatePresetBtn("Filaments Quantiques Électriques", 18, 306, 336, 36, Color.FromArgb(24, 95, 68), PortalTransitionStyle.QuantumArcPortal, GlassColorPalette.EmeraldMint));
        }

        private Control CreatePresetBtn(string text, int x, int y, int w, int h, Color bg, PortalTransitionStyle style, GlassColorPalette palette)
        {
            GlassButton b = new GlassButton(text, x, y, w, h, bg);
            b.Click += (s, e) => { controller.Settings.Style = style; controller.Settings.Theme = palette; controller.Settings.Save(); controller.TriggerDemoTransition(); };
            return b;
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
