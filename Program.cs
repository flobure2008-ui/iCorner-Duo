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

namespace iCornerDuo
{
    // =========================================================================
    // CONFIGURATION & SETTINGS
    // =========================================================================
    public enum CurveType
    {
        SquircleApple = 0,
        ClassicCircle = 1
    }

    public enum TransitionStyle
    {
        ComboUltimate = 0,       // Liquid Bridge + Siri Edge Wave + Dynamic Island + Corner Pulse
        LiquidBridge = 1,        // Apple Universal Control / iPhone Duo Liquid Metaball Bridge
        SiriIntelligenceGlow = 2,// Apple Intelligence Chromatic Wave
        DynamicIslandPill = 3    // iPhone Dynamic Island Top Pill + Corner Pulse
    }

    public enum ColorTheme
    {
        AppleIntelligence = 0,   // Cyan, Magenta, Violet, Warm Orange
        iOSBlueCyan = 1,         // Electric Blue, Cyan, Indigo
        SunsetPro = 2,           // Coral, Magenta, Gold
        MinimalMonochrome = 3    // Pure Silver / White / Obsidian
    }

    public class AppSettings
    {
        public bool Enabled = true;
        public int CornerRadius = 36;
        public CurveType Curve = CurveType.SquircleApple;
        public bool TopLeft = true;
        public bool TopRight = true;
        public bool BottomLeft = true;
        public bool BottomRight = true;
        public bool CornerBezelGlow = true;
        public int BezelGlowOpacity = 45; // 0..100
        public bool HideInFullscreen = false;

        // Transition settings
        public bool EnableTransition = true;
        public bool EnableMagneticProximity = true;
        public TransitionStyle Style = TransitionStyle.ComboUltimate;
        public ColorTheme Theme = ColorTheme.AppleIntelligence;
        public int DurationMs = 950;      // 300..2000
        public int IntensityPercent = 115; // 50..200

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
                sb.AppendFormat("  \"HideInFullscreen\": {0},\n", HideInFullscreen.ToString().ToLower());
                sb.AppendFormat("  \"EnableTransition\": {0},\n", EnableTransition.ToString().ToLower());
                sb.AppendFormat("  \"EnableMagneticProximity\": {0},\n", EnableMagneticProximity.ToString().ToLower());
                sb.AppendFormat("  \"Style\": {0},\n", (int)Style);
                sb.AppendFormat("  \"Theme\": {0},\n", (int)Theme);
                sb.AppendFormat("  \"DurationMs\": {0},\n", DurationMs);
                sb.AppendFormat("  \"IntensityPercent\": {0}\n", IntensityPercent);
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
                    s.HideInFullscreen = ReadBool(json, "HideInFullscreen", s.HideInFullscreen);
                    s.EnableTransition = ReadBool(json, "EnableTransition", s.EnableTransition);
                    s.EnableMagneticProximity = ReadBool(json, "EnableMagneticProximity", s.EnableMagneticProximity);
                    s.Style = (TransitionStyle)ReadInt(json, "Style", (int)s.Style);
                    s.Theme = (ColorTheme)ReadInt(json, "Theme", (int)s.Theme);
                    s.DurationMs = ReadInt(json, "DurationMs", s.DurationMs);
                    s.IntensityPercent = ReadInt(json, "IntensityPercent", s.IntensityPercent);
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
    // WIN32 INTEROP FOR LAYERED CLICK-THROUGH WINDOWS & DPI AWARENESS
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
        public const uint SWP_SHOWWINDOW = 0x0040;

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
        public static extern bool SetProcessDPIAware();

        [DllImport("shcore.dll")]
        public static extern int SetProcessDpiAwareness(int value);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDpiAwarenessContext(IntPtr dpiContext);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        public static void EnablePerMonitorDpiAwareness()
        {
            try
            {
                // DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = -4
                SetProcessDpiAwarenessContext(new IntPtr(-4));
                return;
            }
            catch { }
            try
            {
                SetProcessDpiAwareness(2);
                return;
            }
            catch { }
            try
            {
                SetProcessDPIAware();
            }
            catch { }
        }
    }

    // =========================================================================
    // BASE LAYERED OVERLAY FORM (CLICK-THROUGH, TOPMOST, PER-PIXEL ALPHA)
    // =========================================================================
    public class LayeredOverlayForm : Form
    {
        public LayeredOverlayForm()
        {
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
                              NativeMethods.WS_EX_TRANSPARENT |
                              NativeMethods.WS_EX_TOPMOST |
                              NativeMethods.WS_EX_TOOLWINDOW |
                              NativeMethods.WS_EX_NOACTIVATE;
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
        public float PulseGlowAmount = 0f; // 0..1 during transition entrance!
        public Color[] ActiveThemeColors;

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

        public void UpdateCorners(AppSettings settings, Color[] themeColors)
        {
            ActiveThemeColors = themeColors;
            if (!settings.Enabled || settings.CornerRadius <= 2)
            {
                for (int i = 0; i < 4; i++) CornerForms[i].Hide();
                return;
            }

            bool[] active = new bool[] { settings.TopLeft, settings.TopRight, settings.BottomLeft, settings.BottomRight };
            int r = settings.CornerRadius;
            int glowExtra = (PulseGlowAmount > 0.01f) ? (int)(26 * PulseGlowAmount) : 0;
            int effectiveRadius = r + (int)(PulseGlowAmount * 6f); // Subtle spring expansion on entrance!
            int boxSize = effectiveRadius + 16 + glowExtra;

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

                using (Bitmap bmp = RenderCornerBitmap(boxSize, effectiveRadius, pos, settings, PulseGlowAmount, themeColors))
                {
                    CornerForms[i].SetBitmap(bmp, x, y, 255);
                }
            }
        }

        public static Bitmap RenderCornerBitmap(int size, int radius, CornerPosition pos, AppSettings settings, float pulseGlow, Color[] themeColors)
        {
            Bitmap bmp = new Bitmap(size, size, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.Clear(Color.Transparent);

                // Build the curve path from the corner out to distance 'radius'
                GraphicsPath cornerMask = CreateCornerMaskPath(size, radius, pos, settings.Curve);
                GraphicsPath curveLine = CreateCornerCurvePath(size, radius, pos, settings.Curve);

                // 1. Optional pulse chromatic glow inside the screen corner during transition!
                if (pulseGlow > 0.01f && themeColors != null && themeColors.Length >= 2)
                {
                    Color c1 = themeColors[0];
                    Color c2 = themeColors[(int)pos % themeColors.Length];
                    for (int layer = 5; layer >= 1; layer--)
                    {
                        float width = layer * 4.5f * pulseGlow;
                        int alpha = (int)(pulseGlow * (110 / layer));
                        if (alpha > 255) alpha = 255;
                        using (Pen glowPen = new Pen(Color.FromArgb(alpha, c2), width))
                        {
                            glowPen.StartCap = LineCap.Round;
                            glowPen.EndCap = LineCap.Round;
                            g.DrawPath(glowPen, curveLine);
                        }
                    }
                }

                // 2. Subtle inner Apple bezel highlight along the curve
                if (settings.CornerBezelGlow && settings.BezelGlowOpacity > 0)
                {
                    int bezelAlpha = (int)(settings.BezelGlowOpacity * 2.1f);
                    if (bezelAlpha > 255) bezelAlpha = 255;
                    Color bezelColor = (pulseGlow > 0.05f && themeColors != null)
                        ? InterpolateColor(Color.FromArgb(bezelAlpha, 220, 230, 255), Color.FromArgb(Math.Min(255, bezelAlpha + 80), themeColors[0]), pulseGlow)
                        : Color.FromArgb(bezelAlpha, 235, 240, 255);

                    using (Pen bezelPen = new Pen(bezelColor, 2.2f))
                    {
                        g.DrawPath(bezelPen, curveLine);
                    }
                }

                // 3. Fill the outer corner mask with pure pitch black so the screen corner looks physically rounded!
                using (SolidBrush blackBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0)))
                {
                    g.FillPath(blackBrush, cornerMask);
                }
            }
            return bmp;
        }

        // Generates the curve points for either Apple Squircle (superellipse n=4.2) or Classic Circle
        private static PointF[] GetCornerCurvePoints(int size, int radius, CornerPosition pos, CurveType curveType)
        {
            int steps = 40;
            PointF[] pts = new PointF[steps + 1];
            double n = (curveType == CurveType.SquircleApple) ? 4.2 : 2.0;

            // Corner origin (where the physical screen corner is)
            float cx = (pos == CornerPosition.TopRight || pos == CornerPosition.BottomRight) ? size : 0;
            float cy = (pos == CornerPosition.BottomLeft || pos == CornerPosition.BottomRight) ? size : 0;

            // Direction signs towards the interior of the screen
            float dx = (pos == CornerPosition.TopRight || pos == CornerPosition.BottomRight) ? -1f : 1f;
            float dy = (pos == CornerPosition.BottomLeft || pos == CornerPosition.BottomRight) ? -1f : 1f;

            // Center of curvature at (cx + dx * radius, cy + dy * radius)
            float centerX = cx + dx * radius;
            float centerY = cy + dy * radius;

            for (int i = 0; i <= steps; i++)
            {
                double t = (Math.PI / 2.0) * ((double)i / steps);
                double cosT = Math.Cos(t);
                double sinT = Math.Sin(t);

                // Superellipse parameterization: u = cos^(2/n), v = sin^(2/n)
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

            // Close the path around the physical outer corner (cx, cy) with 1px overshoot to prevent seam leaks
            float ox = (cx == 0) ? -1f : size + 1f;
            float oy = (cy == 0) ? -1f : size + 1f;

            path.AddLine(pts[pts.Length - 1], new PointF(ox, oy));
            path.AddLine(new PointF(ox, oy), pts[0]);
            path.CloseFigure();
            return path;
        }

        public static Color InterpolateColor(Color a, Color b, float t)
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
    // APPLE DUO / LIQUID BRIDGE / SIRI EDGE / DYNAMIC ISLAND TRANSITION ENGINE
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
        private LayeredOverlayForm dynamicIslandForm;

        private bool isAnimating = false;
        private bool isProximityActive = false;
        private long animStartTicks = 0;
        private int animDurationMs = 950;

        private Rectangle entryScreenBounds;
        private Rectangle exitScreenBounds;
        private ScreenEdge entryEdge;
        private ScreenEdge exitEdge;
        private Point crossingPoint;
        private string targetScreenLabel = "Écran 1";
        private string targetScreenRes = "3440 × 1440";

        public MonitorCorners TargetMonitorCorners;

        public ScreenTransitionAnimator()
        {
            entryEdgeForm = new LayeredOverlayForm();
            exitEdgeForm = new LayeredOverlayForm();
            dynamicIslandForm = new LayeredOverlayForm();
        }

        public bool IsBusy
        {
            get { return isAnimating || isProximityActive; }
        }

        public static Color[] GetThemePalette(ColorTheme theme)
        {
            switch (theme)
            {
                case ColorTheme.iOSBlueCyan:
                    return new Color[] {
                        Color.FromArgb(255, 0, 195, 255),
                        Color.FromArgb(255, 10, 132, 255),
                        Color.FromArgb(255, 94, 92, 230),
                        Color.FromArgb(255, 100, 210, 255)
                    };
                case ColorTheme.SunsetPro:
                    return new Color[] {
                        Color.FromArgb(255, 255, 69, 58),
                        Color.FromArgb(255, 255, 55, 95),
                        Color.FromArgb(255, 191, 90, 242),
                        Color.FromArgb(255, 255, 159, 10)
                    };
                case ColorTheme.MinimalMonochrome:
                    return new Color[] {
                        Color.FromArgb(255, 245, 245, 250),
                        Color.FromArgb(255, 200, 205, 215),
                        Color.FromArgb(255, 160, 165, 175),
                        Color.FromArgb(255, 255, 255, 255)
                    };
                case ColorTheme.AppleIntelligence:
                default:
                    return new Color[] {
                        Color.FromArgb(255, 64, 200, 255),  // Siri Cyan
                        Color.FromArgb(255, 185, 80, 255),  // Intelligence Purple
                        Color.FromArgb(255, 255, 75, 160),  // Rose Magenta
                        Color.FromArgb(255, 255, 165, 70)   // Warm Amber
                    };
            }
        }

        public void TriggerTransition(
            Rectangle fromBounds,
            Rectangle toBounds,
            Point cursorPos,
            MonitorCorners targetCorners,
            int screenIndex,
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

            targetScreenLabel = "Écran " + (screenIndex + 1);
            targetScreenRes = toBounds.Width + " × " + toBounds.Height;

            // Determine which edge of toBounds the cursor is closest to
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

        // Renders magnetic pre-glow when cursor approaches a shared inter-monitor edge
        public void UpdateProximity(
            Rectangle currentBounds,
            ScreenEdge nearEdge,
            Point cursorPos,
            float proximityFactor, // 0..1 (1 = right on the border)
            AppSettings settings)
        {
            if (isAnimating) return; // Don't interrupt active transition

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
            Color[] palette = GetThemePalette(settings.Theme);
            float scale = settings.IntensityPercent / 100f;

            // Render a subtle magnetic liquid bulge + glow at the edge being approached
            RenderEdgePortal(
                entryEdgeForm,
                currentBounds,
                nearEdge,
                cursorPos,
                proximityFactor * 0.42f, // pre-bulge progress
                proximityFactor * 0.75f, // alpha
                scale,
                settings.Style,
                palette,
                true);
        }

        public void StepAnimation(AppSettings settings)
        {
            if (!isAnimating) return;

            double elapsedMs = (DateTime.UtcNow.Ticks - animStartTicks) / 10000.0;
            float t = (float)(elapsedMs / animDurationMs);

            if (t >= 1.0f)
            {
                isAnimating = false;
                entryEdgeForm.Hide();
                exitEdgeForm.Hide();
                dynamicIslandForm.Hide();
                if (TargetMonitorCorners != null)
                {
                    TargetMonitorCorners.PulseGlowAmount = 0f;
                    TargetMonitorCorners.UpdateCorners(settings, GetThemePalette(settings.Theme));
                }
                return;
            }

            Color[] palette = GetThemePalette(settings.Theme);
            float scale = settings.IntensityPercent / 100f;

            // 1. Update Corner Pulse on the entered monitor
            if (TargetMonitorCorners != null)
            {
                // Smooth bell curve peaking at t = 0.25
                float cornerPulse = (float)(Math.Sin(Math.Min(1.0, t * 1.25) * Math.PI));
                cornerPulse = Math.Max(0f, cornerPulse);
                TargetMonitorCorners.PulseGlowAmount = cornerPulse;
                TargetMonitorCorners.UpdateCorners(settings, palette);
            }

            // 2. Render Entry & Exit Edge Portals (Liquid Bridge + Siri Edge Wave)
            if (settings.Style == TransitionStyle.ComboUltimate ||
                settings.Style == TransitionStyle.LiquidBridge ||
                settings.Style == TransitionStyle.SiriIntelligenceGlow)
            {
                // Entry portal on new monitor: elastic spring out then smooth fade
                float entryEnvelope = (t < 0.22f)
                    ? EaseOutBack(t / 0.22f)
                    : 1.0f - EaseInOutCubic((t - 0.22f) / 0.78f);

                float entryAlpha = (t < 0.15f) ? (t / 0.15f) : (1.0f - (t - 0.15f) / 0.85f);

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

                // Exit portal on previous monitor: snaps back into the bezel quickly (0..0.45)
                if (t < 0.48f && exitScreenBounds != entryScreenBounds)
                {
                    float exitT = t / 0.48f;
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

            // 3. Render Apple Dynamic Island Pill at top-center of entered screen
            if (settings.Style == TransitionStyle.ComboUltimate ||
                settings.Style == TransitionStyle.DynamicIslandPill)
            {
                RenderDynamicIsland(entryScreenBounds, t, scale, palette);
            }
            else
            {
                dynamicIslandForm.Hide();
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
            float rawTime = 0f)
        {
            if (alphaFactor <= 0.01f || bulgeAmount <= 0.005f)
            {
                form.Hide();
                return;
            }

            bool verticalEdge = (edge == ScreenEdge.Left || edge == ScreenEdge.Right);
            int portalLength = (int)(Math.Min(verticalEdge ? screenBounds.Height : screenBounds.Width, (int)(680 * scale)));
            int portalDepth = (int)(175 * scale);
            if (portalLength < 120) portalLength = 120;
            if (portalDepth < 60) portalDepth = 60;

            int winW = verticalEdge ? portalDepth : portalLength;
            int winH = verticalEdge ? portalLength : portalDepth;

            // Position the window along the screen edge centered on cursor
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
            else // Bottom
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

                // A. Chromatic Siri / Apple Intelligence Edge Glow Wave
                if (style == TransitionStyle.ComboUltimate ||
                    style == TransitionStyle.SiriIntelligenceGlow ||
                    style == TransitionStyle.LiquidBridge)
                {
                    float glowIntensity = (style == TransitionStyle.LiquidBridge) ? 0.65f : 1.0f;
                    DrawChromaticEdgeWave(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor * glowIntensity, scale, palette, rawTime);
                }

                // B. Apple Liquid Metaball Bridge (Black liquid notch stretching from the bezel + glowing rim)
                if (style == TransitionStyle.ComboUltimate ||
                    style == TransitionStyle.LiquidBridge)
                {
                    DrawLiquidBridgeMetaball(g, winW, winH, edge, centerAlong, bulgeAmount, alphaFactor, scale, palette, isExitSide, rawTime);
                }

                if (!form.Visible) form.Show();
                form.SetBitmap(bmp, winX, winY, 255);
            }
        }

        private void DrawChromaticEdgeWave(
            Graphics g,
            int w,
            int h,
            ScreenEdge edge,
            float centerAlong,
            float bulge,
            float alpha,
            float scale,
            Color[] palette,
            float rawTime)
        {
            bool vertical = (edge == ScreenEdge.Left || edge == ScreenEdge.Right);
            float length = vertical ? h : w;
            float maxReach = (vertical ? w : h) * 0.78f * Math.Min(1.3f, bulge * 1.15f);
            if (maxReach < 6f) return;

            // Draw layered soft elliptical chromatic ribbons along the edge
            for (int i = 0; i < 4; i++)
            {
                Color c = palette[i % palette.Length];
                float offsetAlong = (float)Math.Sin(rawTime * 8.0 + i * 1.6) * (42f * scale);
                float segHalfLen = (length * (0.42f - i * 0.06f)) * (0.6f + 0.4f * bulge);
                float reach = maxReach * (1.0f - i * 0.16f);

                int layerAlpha = (int)(alpha * (155 - i * 28));
                if (layerAlpha <= 2) continue;
                if (layerAlpha > 255) layerAlpha = 255;

                using (GraphicsPath wavePath = BuildSmoothEdgeBulgePath(w, h, edge, centerAlong + offsetAlong, segHalfLen, reach))
                {
                    using (PathGradientBrush pgb = new PathGradientBrush(wavePath))
                    {
                        pgb.CenterColor = Color.FromArgb(layerAlpha, c);
                        pgb.SurroundColors = new Color[] { Color.FromArgb(0, c) };
                        g.FillPath(pgb, wavePath);
                    }
                }
            }
        }

        private void DrawLiquidBridgeMetaball(
            Graphics g,
            int w,
            int h,
            ScreenEdge edge,
            float centerAlong,
            float bulge,
            float alpha,
            float scale,
            Color[] palette,
            bool isExitSide,
            float rawTime)
        {
            float maxDepth = (isExitSide ? 36f : 56f) * scale * bulge;
            float halfSpan = (isExitSide ? 85f : 115f) * scale * (0.55f + 0.45f * bulge);
            if (maxDepth < 3f) return;

            using (GraphicsPath liquidPath = BuildSmoothEdgeBulgePath(w, h, edge, centerAlong, halfSpan, maxDepth))
            {
                // 1. Outer luminous neon rim around the black liquid bridge
                for (int glow = 3; glow >= 1; glow--)
                {
                    int rimAlpha = (int)(alpha * (190 / glow));
                    if (rimAlpha > 255) rimAlpha = 255;
                    Color rimColor = MonitorCorners.InterpolateColor(palette[0], palette[1], (float)(0.5 + 0.5 * Math.Sin(rawTime * 10.0)));
                    using (Pen rimPen = new Pen(Color.FromArgb(rimAlpha, rimColor), glow * 2.8f))
                    {
                        g.DrawPath(rimPen, liquidPath);
                    }
                }

                // 2. Pitch-black Apple Dynamic Island liquid body
                int bodyAlpha = (int)(Math.Min(1.0f, alpha * 1.35f) * 255);
                using (SolidBrush blackBrush = new SolidBrush(Color.FromArgb(bodyAlpha, 4, 4, 8)))
                {
                    g.FillPath(blackBrush, liquidPath);
                }

                // 3. Detached floating liquid droplet / spark jumping into the new screen!
                if (!isExitSide && rawTime > 0.06f && rawTime < 0.55f)
                {
                    float dropT = (rawTime - 0.06f) / 0.49f;
                    float dropDist = maxDepth + (48f * scale * EaseOutCubic(dropT));
                    float dropRadius = (11f * scale) * (1.0f - dropT);
                    if (dropRadius > 1.5f)
                    {
                        PointF dropCenter = GetPointFromEdge(w, h, edge, centerAlong, dropDist);
                        int dropAlpha = (int)((1.0f - dropT) * alpha * 240);
                        using (SolidBrush dropGlow = new SolidBrush(Color.FromArgb(dropAlpha / 2, palette[0])))
                        {
                            g.FillEllipse(dropGlow, dropCenter.X - dropRadius * 1.8f, dropCenter.Y - dropRadius * 1.8f, dropRadius * 3.6f, dropRadius * 3.6f);
                        }
                        using (SolidBrush dropCore = new SolidBrush(Color.FromArgb(dropAlpha, 255, 255, 255)))
                        {
                            g.FillEllipse(dropCore, dropCenter.X - dropRadius * 0.7f, dropCenter.Y - dropRadius * 0.7f, dropRadius * 1.4f, dropRadius * 1.4f);
                        }
                    }
                }
            }
        }

        // Builds an organic Apple-style cubic bezier metaball bulge anchored on the given screen edge
        private static GraphicsPath BuildSmoothEdgeBulgePath(
            int w,
            int h,
            ScreenEdge edge,
            float centerAlong,
            float halfSpan,
            float depth)
        {
            GraphicsPath path = new GraphicsPath();
            float start = centerAlong - halfSpan;
            float end = centerAlong + halfSpan;

            if (edge == ScreenEdge.Left)
            {
                PointF p0 = new PointF(-2f, start);
                PointF p1 = new PointF(-2f, start + halfSpan * 0.55f);
                PointF p2 = new PointF(depth, centerAlong - halfSpan * 0.38f);
                PointF tip = new PointF(depth, centerAlong);
                PointF p3 = new PointF(depth, centerAlong + halfSpan * 0.38f);
                PointF p4 = new PointF(-2f, end - halfSpan * 0.55f);
                PointF p5 = new PointF(-2f, end);
                path.AddBezier(p0, p1, p2, tip);
                path.AddBezier(tip, p3, p4, p5);
                path.CloseFigure();
            }
            else if (edge == ScreenEdge.Right)
            {
                float bx = w + 2f;
                PointF p0 = new PointF(bx, start);
                PointF p1 = new PointF(bx, start + halfSpan * 0.55f);
                PointF p2 = new PointF(w - depth, centerAlong - halfSpan * 0.38f);
                PointF tip = new PointF(w - depth, centerAlong);
                PointF p3 = new PointF(w - depth, centerAlong + halfSpan * 0.38f);
                PointF p4 = new PointF(bx, end - halfSpan * 0.55f);
                PointF p5 = new PointF(bx, end);
                path.AddBezier(p0, p1, p2, tip);
                path.AddBezier(tip, p3, p4, p5);
                path.CloseFigure();
            }
            else if (edge == ScreenEdge.Top)
            {
                PointF p0 = new PointF(start, -2f);
                PointF p1 = new PointF(start + halfSpan * 0.55f, -2f);
                PointF p2 = new PointF(centerAlong - halfSpan * 0.38f, depth);
                PointF tip = new PointF(centerAlong, depth);
                PointF p3 = new PointF(centerAlong + halfSpan * 0.38f, depth);
                PointF p4 = new PointF(end - halfSpan * 0.55f, -2f);
                PointF p5 = new PointF(end, -2f);
                path.AddBezier(p0, p1, p2, tip);
                path.AddBezier(tip, p3, p4, p5);
                path.CloseFigure();
            }
            else // Bottom
            {
                float by = h + 2f;
                PointF p0 = new PointF(start, by);
                PointF p1 = new PointF(start + halfSpan * 0.55f, by);
                PointF p2 = new PointF(centerAlong - halfSpan * 0.38f, h - depth);
                PointF tip = new PointF(centerAlong, h - depth);
                PointF p3 = new PointF(centerAlong + halfSpan * 0.38f, h - depth);
                PointF p4 = new PointF(end - halfSpan * 0.55f, by);
                PointF p5 = new PointF(end, by);
                path.AddBezier(p0, p1, p2, tip);
                path.AddBezier(tip, p3, p4, p5);
                path.CloseFigure();
            }
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

        // Renders the sleek iPhone Dynamic Island pill at the top center of the newly entered screen
        private void RenderDynamicIsland(Rectangle screenBounds, float t, float scale, Color[] palette)
        {
            // Expand in (0..0.24), hold (0.24..0.72), retract smoothly (0.72..1.0)
            float openFactor;
            if (t < 0.24f) openFactor = EaseOutBack(t / 0.24f);
            else if (t < 0.72f) openFactor = 1.0f;
            else openFactor = 1.0f - EaseInOutCubic((t - 0.72f) / 0.28f);

            if (openFactor <= 0.02f)
            {
                dynamicIslandForm.Hide();
                return;
            }

            int maxWinW = (int)(340 * scale);
            int maxWinH = (int)(86 * scale);
            int winX = screenBounds.Left + (screenBounds.Width - maxWinW) / 2;
            int winY = screenBounds.Top + 10;

            float pillW = (90f + 185f * Math.Min(1.12f, openFactor)) * scale;
            float pillH = (18f + 26f * Math.Min(1.08f, openFactor)) * scale;
            float pillX = (maxWinW - pillW) / 2f;
            float pillY = 8f * Math.Min(1.0f, openFactor);

            using (Bitmap bmp = new Bitmap(maxWinW, maxWinH, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.Clear(Color.Transparent);

                int alpha = (int)(Math.Min(1f, openFactor * 1.25f) * 255);
                RectangleF pillRect = new RectangleF(pillX, pillY, pillW, pillH);

                using (GraphicsPath pillPath = CreateRoundedRectPath(pillRect, pillH / 2f))
                {
                    // Outer chromatic halo around the Dynamic Island
                    for (int i = 3; i >= 1; i--)
                    {
                        int glowAlpha = (int)(alpha * (0.32f / i));
                        using (Pen glowPen = new Pen(Color.FromArgb(glowAlpha, palette[0]), i * 3.2f))
                        {
                            g.DrawPath(glowPen, pillPath);
                        }
                    }

                    // Deep obsidian pill fill
                    using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(alpha, 8, 8, 12)))
                    {
                        g.FillPath(bgBrush, pillPath);
                    }

                    // Subtle glass top border
                    using (LinearGradientBrush borderGrad = new LinearGradientBrush(
                        pillRect,
                        Color.FromArgb((int)(alpha * 0.75f), palette[0]),
                        Color.FromArgb((int)(alpha * 0.55f), palette[2]),
                        LinearGradientMode.Horizontal))
                    using (Pen borderPen = new Pen(borderGrad, 1.5f))
                    {
                        g.DrawPath(borderPen, pillPath);
                    }
                }

                // Content inside the Dynamic Island once expanded enough
                if (openFactor > 0.45f)
                {
                    float contentAlphaF = Math.Min(1f, (openFactor - 0.45f) / 0.4f);
                    int cAlpha = (int)(contentAlphaF * 255);

                    // Left glowing orb icon
                    float orbSize = pillH * 0.46f;
                    float orbX = pillX + pillH * 0.32f;
                    float orbY = pillY + (pillH - orbSize) / 2f;
                    using (LinearGradientBrush orbBrush = new LinearGradientBrush(
                        new RectangleF(orbX, orbY, orbSize, orbSize),
                        Color.FromArgb(cAlpha, palette[0]),
                        Color.FromArgb(cAlpha, palette[1]),
                        45f))
                    {
                        g.FillEllipse(orbBrush, orbX, orbY, orbSize, orbSize);
                    }

                    // Screen title & resolution badge
                    using (Font titleFont = new Font("Segoe UI Semibold", 9.5f * scale, FontStyle.Bold))
                    using (Font resFont = new Font("Segoe UI", 8.2f * scale, FontStyle.Regular))
                    using (SolidBrush whiteBrush = new SolidBrush(Color.FromArgb(cAlpha, 245, 247, 255)))
                    using (SolidBrush subBrush = new SolidBrush(Color.FromArgb((int)(cAlpha * 0.7f), 175, 185, 205)))
                    {
                        float textX = orbX + orbSize + 10f * scale;
                        float textY = pillY + (pillH - 18f * scale) / 2f;
                        g.DrawString(targetScreenLabel, titleFont, whiteBrush, textX, textY);

                        SizeF titleSize = g.MeasureString(targetScreenLabel, titleFont);
                        g.DrawString("•  " + targetScreenRes, resFont, subBrush, textX + titleSize.Width + 2f, textY + 1.5f * scale);
                    }
                }

                if (!dynamicIslandForm.Visible) dynamicIslandForm.Show();
                dynamicIslandForm.SetBitmap(bmp, winX, winY, 255);
            }
        }

        public static GraphicsPath CreateRoundedRectPath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float d = radius * 2f;
            if (d > rect.Width) d = rect.Width;
            if (d > rect.Height) d = rect.Height;
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
            if (dynamicIslandForm != null) { dynamicIslandForm.Close(); dynamicIslandForm.Dispose(); }
        }
    }

    // =========================================================================
    // MAIN OVERLAY CONTROLLER (MULTI-MONITOR + CURSOR TRACKER + TRAY ICON)
    // =========================================================================
    public class OverlayController : IDisposable
    {
        public AppSettings Settings;
        public List<MonitorCorners> Monitors = new List<MonitorCorners>();
        public ScreenTransitionAnimator Animator;

        private Timer tickTimer;
        private int lastMonitorIndex = -1;
        private int topMostTickCounter = 0;
        private NotifyIcon trayIcon;
        private SettingsForm settingsForm;

        public event Action OnMonitorsChanged;

        public OverlayController()
        {
            Settings = AppSettings.Load();
            Animator = new ScreenTransitionAnimator();

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

        public void RebuildMonitors()
        {
            foreach (var m in Monitors) m.Dispose();
            Monitors.Clear();

            Screen[] screens = Screen.AllScreens;
            Color[] palette = ScreenTransitionAnimator.GetThemePalette(Settings.Theme);
            for (int i = 0; i < screens.Length; i++)
            {
                MonitorCorners mc = new MonitorCorners(screens[i]);
                mc.UpdateCorners(Settings, palette);
                Monitors.Add(mc);
            }

            // Initialize current cursor monitor index
            NativeMethods.POINT pt;
            if (NativeMethods.GetCursorPos(out pt))
            {
                lastMonitorIndex = FindMonitorIndex(new Point(pt.X, pt.Y));
            }
        }

        public void RefreshAllCorners()
        {
            Color[] palette = ScreenTransitionAnimator.GetThemePalette(Settings.Theme);
            foreach (var m in Monitors)
            {
                m.UpdateCorners(Settings, palette);
            }
            Settings.Save();
        }

        private int FindMonitorIndex(Point pt)
        {
            for (int i = 0; i < Monitors.Count; i++)
            {
                if (Monitors[i].Bounds.Contains(pt)) return i;
            }
            // Fallback: closest monitor
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
            // 1. Track cursor across monitors
            NativeMethods.POINT rawPt;
            if (NativeMethods.GetCursorPos(out rawPt) && Monitors.Count > 0)
            {
                Point pt = new Point(rawPt.X, rawPt.Y);
                int currentIdx = FindMonitorIndex(pt);

                if (lastMonitorIndex != -1 && currentIdx != lastMonitorIndex && currentIdx >= 0 && lastMonitorIndex < Monitors.Count)
                {
                    // Cursor just crossed from one screen to another!
                    Animator.TriggerTransition(
                        Monitors[lastMonitorIndex].Bounds,
                        Monitors[currentIdx].Bounds,
                        pt,
                        Monitors[currentIdx],
                        currentIdx,
                        Settings);
                }
                else if (currentIdx >= 0 && Monitors.Count > 1 && !Animator.IsBusy)
                {
                    // Check magnetic proximity to another monitor's shared border
                    CheckMagneticProximity(currentIdx, pt);
                }
                else if (currentIdx >= 0 && Monitors.Count > 1)
                {
                    CheckMagneticProximity(currentIdx, pt);
                }

                lastMonitorIndex = currentIdx;
            }

            // 2. Step transition animation at 60 FPS
            Animator.StepAnimation(Settings);

            // 3. Periodically re-assert TOPMOST so taskbar or fullscreen transitions don't hide corners
            topMostTickCounter++;
            if (topMostTickCounter >= 90) // Every ~1.5 sec
            {
                topMostTickCounter = 0;
                foreach (var m in Monitors) m.EnsureTopMost();
            }
        }

        private void CheckMagneticProximity(int currentIdx, Point pt)
        {
            Rectangle cur = Monitors[currentIdx].Bounds;
            int threshold = 68; // pixels from shared edge
            float bestProx = 0f;
            ScreenEdge bestEdge = ScreenEdge.Left;

            for (int otherIdx = 0; otherIdx < Monitors.Count; otherIdx++)
            {
                if (otherIdx == currentIdx) continue;
                Rectangle other = Monitors[otherIdx].Bounds;

                // Check if other monitor is to the Left of cur
                if (Math.Abs(other.Right - cur.Left) <= 150 && pt.Y >= other.Top - 80 && pt.Y <= other.Bottom + 80)
                {
                    int dist = pt.X - cur.Left;
                    if (dist >= 0 && dist < threshold)
                    {
                        float p = 1f - (float)dist / threshold;
                        if (p > bestProx) { bestProx = p; bestEdge = ScreenEdge.Left; }
                    }
                }
                // Check if other monitor is to the Right of cur
                if (Math.Abs(other.Left - cur.Right) <= 150 && pt.Y >= other.Top - 80 && pt.Y <= other.Bottom + 80)
                {
                    int dist = cur.Right - pt.X;
                    if (dist >= 0 && dist < threshold)
                    {
                        float p = 1f - (float)dist / threshold;
                        if (p > bestProx) { bestProx = p; bestEdge = ScreenEdge.Right; }
                    }
                }
                // Check if other monitor is Above cur
                if (Math.Abs(other.Bottom - cur.Top) <= 150 && pt.X >= other.Left - 80 && pt.X <= other.Right + 80)
                {
                    int dist = pt.Y - cur.Top;
                    if (dist >= 0 && dist < threshold)
                    {
                        float p = 1f - (float)dist / threshold;
                        if (p > bestProx) { bestProx = p; bestEdge = ScreenEdge.Top; }
                    }
                }
                // Check if other monitor is Below cur
                if (Math.Abs(other.Top - cur.Bottom) <= 150 && pt.X >= other.Left - 80 && pt.X <= other.Right + 80)
                {
                    int dist = cur.Bottom - pt.Y;
                    if (dist >= 0 && dist < threshold)
                    {
                        float p = 1f - (float)dist / threshold;
                        if (p > bestProx) { bestProx = p; bestEdge = ScreenEdge.Bottom; }
                    }
                }
            }

            Animator.UpdateProximity(cur, bestEdge, pt, bestProx, Settings);
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

            // Pick a dramatic crossing point on the left or right edge of the active monitor
            Point demoPoint = new Point(toB.Left + 4, toB.Top + toB.Height / 2);
            if (Monitors.Count > 1 && fromB.Left > toB.Left)
            {
                demoPoint = new Point(toB.Right - 4, toB.Top + toB.Height / 2);
            }

            Animator.TriggerTransition(fromB, toB, demoPoint, Monitors[targetIdx], targetIdx, Settings);
        }

        private void SetupTrayIcon()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "iCorner Duo — Bords Arrondis & Transitions Apple";
            trayIcon.Icon = CreateTrayIcon();
            trayIcon.Visible = true;

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem openItem = new ToolStripMenuItem("⚙ Ouvrir les paramètres");
            openItem.Font = new Font(openItem.Font, FontStyle.Bold);
            openItem.Click += (s, e) => ShowSettingsWindow();

            ToolStripMenuItem demoItem = new ToolStripMenuItem("✨ Tester l'animation de transition");
            demoItem.Click += (s, e) => TriggerDemoTransition();

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
            menu.Items.Add(toggleItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(exitItem);

            trayIcon.ContextMenuStrip = menu;
            trayIcon.DoubleClick += (s, e) => ShowSettingsWindow();
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

        private Icon CreateTrayIcon()
        {
            Icon appIco = LoadAppIcon();
            if (appIco != null) return appIco;

            using (Bitmap bmp = new Bitmap(32, 32, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                RectangleF rect = new RectangleF(2, 2, 28, 28);
                using (GraphicsPath path = ScreenTransitionAnimator.CreateRoundedRectPath(rect, 9f))
                {
                    using (LinearGradientBrush bg = new LinearGradientBrush(rect,
                        Color.FromArgb(255, 64, 200, 255),
                        Color.FromArgb(255, 195, 70, 255),
                        45f))
                    {
                        g.FillPath(bg, path);
                    }
                }

                RectangleF inner = new RectangleF(6, 6, 20, 20);
                using (GraphicsPath innerPath = ScreenTransitionAnimator.CreateRoundedRectPath(inner, 6f))
                using (SolidBrush dark = new SolidBrush(Color.FromArgb(255, 14, 15, 22)))
                {
                    g.FillPath(dark, innerPath);
                }

                return Icon.FromHandle(bmp.GetHicon());
            }
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
            foreach (var m in Monitors) m.Dispose();
            if (trayIcon != null) { trayIcon.Visible = false; trayIcon.Dispose(); }
        }
    }

    // =========================================================================
    // MODERN APPLE-STYLE DARK CONTROL CENTER SETTINGS WINDOW
    // =========================================================================
    public class SettingsForm : Form
    {
        private OverlayController controller;
        private Label radiusValueLabel;
        private Label glowValueLabel;
        private Label durationValueLabel;
        private Label intensityValueLabel;
        private Label monitorsInfoLabel;
        private Panel previewBox;

        public SettingsForm(OverlayController ctrl)
        {
            controller = ctrl;
            Text = "iCorner Duo — Bords Arrondis & Transitions Multi-Écrans";
            Size = new Size(760, 740);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(15, 16, 22);
            ForeColor = Color.FromArgb(240, 242, 250);
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            Icon appIco = OverlayController.LoadAppIcon();
            if (appIco != null) Icon = appIco;

            BuildUI();

            controller.OnMonitorsChanged += UpdateMonitorSummary;
        }

        private void BuildUI()
        {
            // Header Banner
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 82,
                BackColor = Color.FromArgb(22, 24, 34)
            };
            header.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (LinearGradientBrush grad = new LinearGradientBrush(
                    header.ClientRectangle,
                    Color.FromArgb(45, 64, 200, 255),
                    Color.FromArgb(45, 200, 75, 255),
                    15f))
                {
                    e.Graphics.FillRectangle(grad, header.ClientRectangle);
                }
                using (Pen bottomLine = new Pen(Color.FromArgb(60, 120, 150, 255), 1f))
                {
                    e.Graphics.DrawLine(bottomLine, 0, header.Height - 1, header.Width, header.Height - 1);
                }
                if (Icon != null)
                {
                    e.Graphics.DrawIcon(Icon, new Rectangle(18, 15, 52, 52));
                }
            };

            Label titleLbl = new Label
            {
                Text = "iCorner Duo Studio",
                Font = new Font("Segoe UI Semibold", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(80, 14)
            };

            Label subLbl = new Label
            {
                Text = "Bords arrondis adaptatifs multi-écrans & transitions fluides style Apple iPhone / Universal Control",
                Font = new Font("Segoe UI", 9.0f, FontStyle.Regular),
                ForeColor = Color.FromArgb(185, 195, 220),
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(82, 48)
            };

            Button testBtn = new Button
            {
                Text = "✨ Tester la transition",
                Size = new Size(182, 42),
                Location = new Point(545, 20),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9.8f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            testBtn.FlatAppearance.BorderSize = 0;
            testBtn.Click += (s, e) => controller.TriggerDemoTransition();

            header.Controls.Add(titleLbl);
            header.Controls.Add(subLbl);
            header.Controls.Add(testBtn);
            Controls.Add(header);

            // Left Column: Corner Settings + Monitor Info
            Panel leftCard = CreateCard(22, 98, 345, 585, "📐 Bords Arrondis des Écrans");
            Controls.Add(leftCard);

            CheckBox enableCornersChk = CreateCheckBox("Activer les bords arrondis sur mes écrans", 18, 44, controller.Settings.Enabled);
            enableCornersChk.CheckedChanged += (s, e) =>
            {
                controller.Settings.Enabled = enableCornersChk.Checked;
                controller.RefreshAllCorners();
                previewBox.Invalidate();
            };
            leftCard.Controls.Add(enableCornersChk);

            // Radius Slider
            Label radTitle = CreateLabel("Rayon de l'arrondi :", 18, 82);
            radiusValueLabel = CreateValueLabel(controller.Settings.CornerRadius + " px", 265, 82);
            TrackBar radiusSlider = new TrackBar
            {
                Minimum = 0,
                Maximum = 110,
                Value = Math.Min(110, Math.Max(0, controller.Settings.CornerRadius)),
                TickStyle = TickStyle.None,
                Location = new Point(14, 104),
                Size = new Size(315, 32),
                BackColor = Color.FromArgb(25, 27, 38)
            };
            radiusSlider.ValueChanged += (s, e) =>
            {
                controller.Settings.CornerRadius = radiusSlider.Value;
                radiusValueLabel.Text = radiusSlider.Value + " px";
                controller.RefreshAllCorners();
                previewBox.Invalidate();
            };
            leftCard.Controls.Add(radTitle);
            leftCard.Controls.Add(radiusValueLabel);
            leftCard.Controls.Add(radiusSlider);

            // Curve Type
            Label curveTitle = CreateLabel("Style géométrique de l'angle :", 18, 145);
            ComboBox curveCombo = CreateComboBox(18, 168, 308, new string[] {
                "Squircle Apple (Courbure continue iOS / macOS)",
                "Cercle Classique (Arc circulaire standard)"
            }, (int)controller.Settings.Curve);
            curveCombo.SelectedIndexChanged += (s, e) =>
            {
                controller.Settings.Curve = (CurveType)curveCombo.SelectedIndex;
                controller.RefreshAllCorners();
                previewBox.Invalidate();
            };
            leftCard.Controls.Add(curveTitle);
            leftCard.Controls.Add(curveCombo);

            // Active Corners Checkboxes
            Label activeCornersTitle = CreateLabel("Angles actifs :", 18, 210);
            CheckBox tlChk = CreateCheckBox("Haut-Gauche", 18, 234, controller.Settings.TopLeft);
            CheckBox trChk = CreateCheckBox("Haut-Droit", 175, 234, controller.Settings.TopRight);
            CheckBox blChk = CreateCheckBox("Bas-Gauche", 18, 264, controller.Settings.BottomLeft);
            CheckBox brChk = CreateCheckBox("Bas-Droit", 175, 264, controller.Settings.BottomRight);

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

            leftCard.Controls.Add(activeCornersTitle);
            leftCard.Controls.Add(tlChk);
            leftCard.Controls.Add(trChk);
            leftCard.Controls.Add(blChk);
            leftCard.Controls.Add(brChk);

            // Inner Bezel Highlight
            CheckBox bezelChk = CreateCheckBox("Liseré lumineux fin (Style Bezel iPhone)", 18, 302, controller.Settings.CornerBezelGlow);
            bezelChk.CheckedChanged += (s, e) =>
            {
                controller.Settings.CornerBezelGlow = bezelChk.Checked;
                controller.RefreshAllCorners();
                previewBox.Invalidate();
            };
            leftCard.Controls.Add(bezelChk);

            Label glowTitle = CreateLabel("Opacité du liseré :", 18, 334);
            glowValueLabel = CreateValueLabel(controller.Settings.BezelGlowOpacity + " %", 265, 334);
            TrackBar glowSlider = new TrackBar
            {
                Minimum = 0,
                Maximum = 100,
                Value = Math.Min(100, Math.Max(0, controller.Settings.BezelGlowOpacity)),
                TickStyle = TickStyle.None,
                Location = new Point(14, 355),
                Size = new Size(315, 32),
                BackColor = Color.FromArgb(25, 27, 38)
            };
            glowSlider.ValueChanged += (s, e) =>
            {
                controller.Settings.BezelGlowOpacity = glowSlider.Value;
                glowValueLabel.Text = glowSlider.Value + " %";
                controller.RefreshAllCorners();
                previewBox.Invalidate();
            };
            leftCard.Controls.Add(glowTitle);
            leftCard.Controls.Add(glowValueLabel);
            leftCard.Controls.Add(glowSlider);

            // Live Interactive Preview Box
            Label prevTitle = CreateLabel("Aperçu en direct de l'angle :", 18, 395);
            leftCard.Controls.Add(prevTitle);

            previewBox = new Panel
            {
                Location = new Point(18, 420),
                Size = new Size(308, 76),
                BackColor = Color.FromArgb(32, 36, 52)
            };
            previewBox.Paint += PaintPreviewBox;
            leftCard.Controls.Add(previewBox);

            // Detected Monitors Box
            monitorsInfoLabel = new Label
            {
                Location = new Point(18, 506),
                Size = new Size(215, 68),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(165, 210, 255)
            };
            UpdateMonitorSummary();
            leftCard.Controls.Add(monitorsInfoLabel);

            Button refreshMonBtn = new Button
            {
                Text = "🔄 Détecter",
                Location = new Point(238, 518),
                Size = new Size(88, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(40, 44, 62),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            refreshMonBtn.FlatAppearance.BorderSize = 0;
            refreshMonBtn.Click += (s, e) =>
            {
                controller.RebuildMonitors();
                UpdateMonitorSummary();
            };
            leftCard.Controls.Add(refreshMonBtn);

            // Right Column: Apple Duo Screen Transition Settings
            Panel rightCard = CreateCard(382, 98, 345, 585, "🌊 Transition Inter-Écrans (Apple Duo)");
            Controls.Add(rightCard);

            CheckBox enableTransChk = CreateCheckBox("Activer l'animation au passage d'un écran à l'autre", 18, 44, controller.Settings.EnableTransition);
            enableTransChk.CheckedChanged += (s, e) =>
            {
                controller.Settings.EnableTransition = enableTransChk.Checked;
                controller.Settings.Save();
            };
            rightCard.Controls.Add(enableTransChk);

            CheckBox magProxChk = CreateCheckBox("Effet magnétique quand le curseur approche du bord", 18, 76, controller.Settings.EnableMagneticProximity);
            magProxChk.CheckedChanged += (s, e) =>
            {
                controller.Settings.EnableMagneticProximity = magProxChk.Checked;
                controller.Settings.Save();
            };
            rightCard.Controls.Add(magProxChk);

            // Transition Style Selector
            Label styleTitle = CreateLabel("Style d'animation Apple :", 18, 118);
            ComboBox styleCombo = CreateComboBox(18, 142, 308, new string[] {
                "✨ Combo Ultime (Pont Liquide + Siri Glow + Dynamic Island)",
                "💧 Apple Liquid Bridge (Pont liquide au bord de l'écran)",
                "🌈 Apple Intelligence (Onde chromatique Siri & Angles)",
                "💊 Dynamic Island (Pilule animée haut d'écran + Pulse)"
            }, (int)controller.Settings.Style);
            styleCombo.SelectedIndexChanged += (s, e) =>
            {
                controller.Settings.Style = (TransitionStyle)styleCombo.SelectedIndex;
                controller.Settings.Save();
                controller.TriggerDemoTransition();
            };
            rightCard.Controls.Add(styleTitle);
            rightCard.Controls.Add(styleCombo);

            // Color Theme Selector
            Label themeTitle = CreateLabel("Palette de couleurs de la transition :", 18, 188);
            ComboBox themeCombo = CreateComboBox(18, 212, 308, new string[] {
                "🎨 Apple Intelligence (Cyan / Violet / Magenta / Or)",
                "💎 iOS Electric Blue (Bleu / Cyan / Indigo)",
                "🌅 Sunset Pro (Corail / Rose / Pourpre / Ambre)",
                "⚪ Minimal Monochrome (Argent / Blanc pur / Noir)"
            }, (int)controller.Settings.Theme);
            themeCombo.SelectedIndexChanged += (s, e) =>
            {
                controller.Settings.Theme = (ColorTheme)themeCombo.SelectedIndex;
                controller.RefreshAllCorners();
                previewBox.Invalidate();
                controller.TriggerDemoTransition();
            };
            rightCard.Controls.Add(themeTitle);
            rightCard.Controls.Add(themeCombo);

            // Duration Slider
            Label durTitle = CreateLabel("Durée de la transition :", 18, 260);
            durationValueLabel = CreateValueLabel(controller.Settings.DurationMs + " ms", 250, 260);
            TrackBar durSlider = new TrackBar
            {
                Minimum = 350,
                Maximum = 1800,
                Value = Math.Min(1800, Math.Max(350, controller.Settings.DurationMs)),
                TickStyle = TickStyle.None,
                Location = new Point(14, 282),
                Size = new Size(315, 32),
                BackColor = Color.FromArgb(25, 27, 38)
            };
            durSlider.ValueChanged += (s, e) =>
            {
                controller.Settings.DurationMs = durSlider.Value;
                durationValueLabel.Text = durSlider.Value + " ms";
                controller.Settings.Save();
            };
            rightCard.Controls.Add(durTitle);
            rightCard.Controls.Add(durationValueLabel);
            rightCard.Controls.Add(durSlider);

            // Intensity Slider
            Label intTitle = CreateLabel("Amplitude / Taille de l'effet :", 18, 325);
            intensityValueLabel = CreateValueLabel(controller.Settings.IntensityPercent + " %", 255, 325);
            TrackBar intSlider = new TrackBar
            {
                Minimum = 60,
                Maximum = 180,
                Value = Math.Min(180, Math.Max(60, controller.Settings.IntensityPercent)),
                TickStyle = TickStyle.None,
                Location = new Point(14, 347),
                Size = new Size(315, 32),
                BackColor = Color.FromArgb(25, 27, 38)
            };
            intSlider.ValueChanged += (s, e) =>
            {
                controller.Settings.IntensityPercent = intSlider.Value;
                intensityValueLabel.Text = intSlider.Value + " %";
                controller.Settings.Save();
            };
            rightCard.Controls.Add(intTitle);
            rightCard.Controls.Add(intensityValueLabel);
            rightCard.Controls.Add(intSlider);

            // Startup with Windows Checkbox
            CheckBox startupChk = CreateCheckBox("Lancer automatiquement au démarrage de Windows", 18, 395, IsStartupEnabled());
            startupChk.CheckedChanged += (s, e) => SetStartup(startupChk.Checked);
            rightCard.Controls.Add(startupChk);

            // Explanation / Tips Box
            Label tipBox = new Label
            {
                Location = new Point(18, 435),
                Size = new Size(308, 86),
                BackColor = Color.FromArgb(30, 34, 48),
                ForeColor = Color.FromArgb(190, 200, 225),
                Font = new Font("Segoe UI", 8.6f, FontStyle.Regular),
                Padding = new Padding(10),
                Text = "💡 Astuce : Déplacez simplement votre souris d'un écran à l'autre pour déclencher le pont liquide et l'onde lumineuse à l'endroit exact où votre curseur traverse la bordure !"
            };
            rightCard.Controls.Add(tipBox);

            // Hide to Tray Button
            Button minimizeBtn = new Button
            {
                Text = "📥 Réduire dans la barre système (Actif en fond)",
                Location = new Point(18, 532),
                Size = new Size(308, 38),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(42, 47, 68),
                ForeColor = Color.FromArgb(225, 232, 255),
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            minimizeBtn.FlatAppearance.BorderSize = 0;
            minimizeBtn.Click += (s, e) => Hide();
            rightCard.Controls.Add(minimizeBtn);
        }

        private void PaintPreviewBox(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = previewBox.ClientRectangle;

            Color[] palette = ScreenTransitionAnimator.GetThemePalette(controller.Settings.Theme);
            using (LinearGradientBrush wallpaper = new LinearGradientBrush(r,
                Color.FromArgb(255, 28, 52, 98),
                Color.FromArgb(255, 88, 38, 110),
                30f))
            {
                g.FillRectangle(wallpaper, r);
            }

            // Draw simulated corners on left & right of preview
            int previewRad = Math.Min(60, controller.Settings.CornerRadius);
            if (controller.Settings.Enabled && previewRad > 2)
            {
                using (Bitmap tlBmp = RenderPreviewCorner(previewRad, CornerPosition.TopLeft))
                using (Bitmap trBmp = RenderPreviewCorner(previewRad, CornerPosition.TopRight))
                {
                    g.DrawImage(tlBmp, 0, 0);
                    g.DrawImage(trBmp, r.Width - tlBmp.Width, 0);
                }
            }

            using (Font f = new Font("Segoe UI Semibold", 9f, FontStyle.Bold))
            using (SolidBrush b = new SolidBrush(Color.FromArgb(230, 255, 255, 255)))
            {
                string txt = controller.Settings.Curve == CurveType.SquircleApple
                    ? "Courbe Squircle Continue (" + controller.Settings.CornerRadius + " px)"
                    : "Arc Circulaire Classique (" + controller.Settings.CornerRadius + " px)";
                SizeF sz = g.MeasureString(txt, f);
                g.DrawString(txt, f, b, (r.Width - sz.Width) / 2f, (r.Height - sz.Height) / 2f + 6f);
            }
        }

        private Bitmap RenderPreviewCorner(int rad, CornerPosition pos)
        {
            int size = rad + 16;
            Color[] palette = ScreenTransitionAnimator.GetThemePalette(controller.Settings.Theme);
            return MonitorCorners.RenderCornerBitmap(size, rad, pos, controller.Settings, 0.25f, palette);
        }

        private void UpdateMonitorSummary()
        {
            if (monitorsInfoLabel == null) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("🖥 {0} écran(s) détecté(s) :\n", controller.Monitors.Count);
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
                BackColor = Color.FromArgb(25, 27, 38)
            };
            Label hdr = new Label
            {
                Text = title,
                Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(115, 205, 255),
                Location = new Point(16, 12),
                AutoSize = true
            };
            p.Controls.Add(hdr);
            return p;
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = Color.FromArgb(210, 215, 235)
            };
        }

        private Label CreateValueLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 205, 255)
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
                BackColor = Color.FromArgb(36, 40, 56),
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
                    if (key != null)
                    {
                        return key.GetValue("iCornerDuo") != null;
                    }
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
                // Hide to system tray instead of killing the overlays when closing the settings window
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
                    // Trigger an initial subtle welcome transition after 400ms so the user sees it right away!
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
