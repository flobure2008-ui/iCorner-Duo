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

namespace iIsland
{
    public enum CapsuleTab { AudioPlayer = 0, FocusTimer = 1, QuickSettings = 2 }

    public enum CapsuleScreenPlacement { FollowActiveCursor = 0, PrimaryScreenTop = 1 }

    public enum AudioAppSource
    {
        None = 0, Spotify = 1, AppleMusic = 2, YouTube = 3, Deezer = 4,
        SoundCloud = 5, Tidal = 6, VLC = 7, AmazonMusic = 8, MediaPlayer = 9, WebMusic = 10
    }

    public enum GlassColorPalette
    {
        AetherPrism = 0, FrostWhiteSilver = 1, OceanicCyan = 2, EmeraldMint = 3,
        CrimsonVelvet = 4, SolarAmber = 5, BorealAurora = 6, UltravioletMist = 7,
        SakuraBlossom = 8, ObsidianMonochrome = 9, ChromaticSpectrum = 10, CustomGlassPalette = 11
    }

    public class AppSettings
    {
        public bool Enabled = true;
        public CapsuleScreenPlacement IslandPlacement = CapsuleScreenPlacement.FollowActiveCursor;
        public int IslandScalePercent = 100;
        public bool IslandExpandOnHover = true;
        public bool IslandShowClockWhenIdle = true;
        public bool IslandAutoAdaptBrandColor = true;
        public bool FullscreenMorph = true;
        public GlassColorPalette Theme = GlassColorPalette.AetherPrism;

        private static string SettingsPath
        {
            get
            {
                string dir = Path.GetDirectoryName(Application.ExecutablePath);
                return Path.Combine(dir, "iisland_settings.json");
            }
        }

        public void Save()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine(string.Format("  \"Enabled\": {0},", Enabled ? "true" : "false"));
                sb.AppendLine(string.Format("  \"IslandPlacement\": {0},", (int)IslandPlacement));
                sb.AppendLine(string.Format("  \"IslandScalePercent\": {0},", IslandScalePercent));
                sb.AppendLine(string.Format("  \"IslandExpandOnHover\": {0},", IslandExpandOnHover ? "true" : "false"));
                sb.AppendLine(string.Format("  \"IslandShowClockWhenIdle\": {0},", IslandShowClockWhenIdle ? "true" : "false"));
                sb.AppendLine(string.Format("  \"IslandAutoAdaptBrandColor\": {0},", IslandAutoAdaptBrandColor ? "true" : "false"));
                sb.AppendLine(string.Format("  \"FullscreenMorph\": {0},", FullscreenMorph ? "true" : "false"));
                sb.AppendLine(string.Format("  \"Theme\": {0}", (int)Theme));
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
                s.IslandPlacement = (CapsuleScreenPlacement)ReadInt(json, "IslandPlacement", (int)s.IslandPlacement);
                s.IslandScalePercent = ReadInt(json, "IslandScalePercent", s.IslandScalePercent);
                s.IslandExpandOnHover = ReadBool(json, "IslandExpandOnHover", s.IslandExpandOnHover);
                s.IslandShowClockWhenIdle = ReadBool(json, "IslandShowClockWhenIdle", s.IslandShowClockWhenIdle);
                s.IslandAutoAdaptBrandColor = ReadBool(json, "IslandAutoAdaptBrandColor", s.IslandAutoAdaptBrandColor);
                s.FullscreenMorph = ReadBool(json, "FullscreenMorph", s.FullscreenMorph);
                s.Theme = (GlassColorPalette)ReadInt(json, "Theme", (int)s.Theme);
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

        public static Color GetAudioSourceColor(AudioAppSource src, Color fallback)
        {
            switch (src)
            {
                case AudioAppSource.Spotify: return Color.FromArgb(255, 30, 215, 96);
                case AudioAppSource.AppleMusic: return Color.FromArgb(255, 250, 45, 85);
                case AudioAppSource.YouTube: return Color.FromArgb(255, 255, 30, 45);
                case AudioAppSource.Deezer: return Color.FromArgb(255, 162, 56, 255);
                case AudioAppSource.SoundCloud: return Color.FromArgb(255, 255, 85, 0);
                case AudioAppSource.Tidal: return Color.FromArgb(255, 0, 235, 220);
                case AudioAppSource.VLC: return Color.FromArgb(255, 255, 136, 0);
                case AudioAppSource.AmazonMusic: return Color.FromArgb(255, 37, 211, 238);
                default: return fallback;
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

        public static void DrawLiquidGlassSurface(Graphics g, RectangleF rect, float cornerRadius, Color baseDarkTint, Color accentTint, int glassOpacity = 242, float borderAlphaRatio = 1.0f)
        {
            if (rect.Width <= 2f || rect.Height <= 2f) return;
            using (GraphicsPath path = CreateRoundedRect(rect, cornerRadius))
            {
                using (SolidBrush baseBrush = new SolidBrush(Color.FromArgb(glassOpacity, baseDarkTint)))
                    g.FillPath(baseBrush, path);

                using (GraphicsPath innerCaustic = CreateRoundedRect(RectangleF.Inflate(rect, -2f, -2f), Math.Max(2f, cornerRadius - 2f)))
                using (LinearGradientBrush causticGrad = new LinearGradientBrush(rect, Color.FromArgb((int)(34 * borderAlphaRatio), accentTint), Color.FromArgb(0, accentTint), 90f))
                    g.FillPath(causticGrad, innerCaustic);

                float hlHeight = Math.Min(rect.Height * 0.48f, 28f);
                if (hlHeight > 4f)
                {
                    RectangleF hlRect = new RectangleF(rect.X + 1.5f, rect.Y + 1.2f, rect.Width - 3f, hlHeight);
                    using (GraphicsPath hlPath = CreateRoundedRect(hlRect, Math.Min(cornerRadius, hlHeight / 2f)))
                    using (LinearGradientBrush hlBrush = new LinearGradientBrush(hlRect, Color.FromArgb((int)(68 * borderAlphaRatio), 255, 255, 255), Color.FromArgb(0, 255, 255, 255), 90f))
                        g.FillPath(hlBrush, hlPath);
                }

                using (LinearGradientBrush rimGrad = new LinearGradientBrush(rect, Color.FromArgb((int)(130 * borderAlphaRatio), Interpolate(Color.White, accentTint, 0.25f)), Color.FromArgb((int)(38 * borderAlphaRatio), accentTint), 60f))
                using (Pen rimPen = new Pen(rimGrad, 1.25f))
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
        public const int WS_EX_TOOLWINDOW = 0x80;
        public const int WS_EX_TOPMOST = 0x8;
        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOACTIVATE = 0x0010;
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
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        [DllImport("shcore.dll")]
        public static extern int SetProcessDpiAwareness(int value);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDpiAwarenessContext(IntPtr dpiContext);

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

    public class LiveMediaTracker
    {
        private IAudioMeterInformation audioMeter;
        public float CurrentAudioPeak = 0f;
        public bool IsAudioActive = false;
        public AudioAppSource ActiveSource = AudioAppSource.Spotify;
        public string TrackTitle = "Musique en cours";
        public string TrackArtist = "Lecteur Audio";
        public float[] SpectrumBars = new float[16];

        public LiveMediaTracker()
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
            if (peak > CurrentAudioPeak) CurrentAudioPeak = CurrentAudioPeak * 0.45f + peak * 0.55f;
            else CurrentAudioPeak = CurrentAudioPeak * 0.86f + peak * 0.14f;

            IsAudioActive = (CurrentAudioPeak > 0.015f);

            for (int i = 0; i < SpectrumBars.Length; i++)
            {
                float target;
                if (IsAudioActive)
                {
                    float shape = (float)Math.Sin((i / (float)SpectrumBars.Length) * Math.PI);
                    float w1 = (float)Math.Sin(globalTime * 10.5f + i * 0.85f) * 0.5f + 0.5f;
                    target = Math.Min(1.0f, CurrentAudioPeak * 1.5f * (0.35f + 0.65f * shape) * (0.45f + 0.55f * w1));
                    if (target < 0.10f) target = 0.10f;
                }
                else
                {
                    target = 0.08f + 0.04f * (float)Math.Sin(globalTime * 2.5f + i * 0.6f);
                }
                SpectrumBars[i] = SpectrumBars[i] * 0.65f + target * 0.35f;
            }
        }
    }

    public class FocusTimerEngine
    {
        public bool IsRunning = false;
        public int TotalSeconds = 1500;
        public int RemainingSeconds = 1500;
        private DateTime lastTickUtc = DateTime.UtcNow;

        public void Toggle()
        {
            IsRunning = !IsRunning;
            lastTickUtc = DateTime.UtcNow;
        }

        public void Reset()
        {
            IsRunning = false;
            RemainingSeconds = TotalSeconds;
        }

        public void Update()
        {
            if (!IsRunning) return;
            DateTime now = DateTime.UtcNow;
            if ((now - lastTickUtc).TotalSeconds >= 1.0)
            {
                lastTickUtc = now;
                if (RemainingSeconds > 0) RemainingSeconds--;
                else IsRunning = false;
            }
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
                cp.ExStyle |= NativeMethods.WS_EX_LAYERED | NativeMethods.WS_EX_TOOLWINDOW | NativeMethods.WS_EX_TOPMOST;
                return cp;
            }
        }

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

    public class IslandWidget : IDisposable
    {
        private LayeredOverlayForm islandForm;
        public CapsuleTab CurrentTab = CapsuleTab.AudioPlayer;
        public FocusTimerEngine TimerEngine = new FocusTimerEngine();
        public LiveMediaTracker Media = new LiveMediaTracker();

        private float expandProgress = 0f;
        private Rectangle currentInteractiveRect = Rectangle.Empty;
        private int hoveredTabIndex = -1;
        private int hoveredActionIndex = -1;
        private bool wasMouseDown = false;
        private float currentCenterX = -9999f;

        public event Action OnOpenSettingsRequested;

        public IslandWidget()
        {
            islandForm = new LayeredOverlayForm();
            islandForm.Show();
        }

        public void UpdateAndRender(AppSettings settings, Rectangle screenBounds, Point cursorPt, Color[] palette, float globalTime)
        {
            if (!settings.Enabled) { islandForm.Hide(); return; }

            Media.UpdateTick(globalTime);
            TimerEngine.Update();

            float targetCenterX = (settings.IslandPlacement == CapsuleScreenPlacement.FollowActiveCursor)
                ? (screenBounds.Left + screenBounds.Width / 2f) : (Screen.PrimaryScreen.Bounds.Left + Screen.PrimaryScreen.Bounds.Width / 2f);
            if (currentCenterX < -9000f) currentCenterX = targetCenterX;
            else currentCenterX = currentCenterX * 0.85f + targetCenterX * 0.15f;

            bool isCursorOver = currentInteractiveRect.Contains(cursorPt);
            bool targetExpand = settings.IslandExpandOnHover && isCursorOver;
            float targetExpVal = targetExpand ? 1.0f : 0.0f;
            expandProgress = expandProgress * 0.82f + targetExpVal * 0.18f;

            float scale = settings.IslandScalePercent / 100f;
            float collapsedW = 210f * scale;
            float collapsedH = 38f * scale;
            float expandedW = 460f * scale;
            float expandedH = 160f * scale;

            float curW = collapsedW + (expandedW - collapsedW) * expandProgress;
            float curH = collapsedH + (expandedH - collapsedH) * expandProgress;

            int winW = (int)(curW + 60f);
            int winH = (int)(curH + 40f);
            int winX = (int)(currentCenterX - winW / 2f);
            int winY = screenBounds.Top + 12;

            currentInteractiveRect = new Rectangle((int)(currentCenterX - curW / 2f), winY, (int)curW, (int)curH);

            bool isMouseDown = (Control.MouseButtons == MouseButtons.Left);
            Point relMouse = new Point(cursorPt.X - winX, cursorPt.Y - winY);

            HandleInteractions(relMouse, isMouseDown, winW, curW, curH, scale);

            Color accent = GlassThemeEngine.GetAudioSourceColor(Media.ActiveSource, palette[0]);

            using (Bitmap bmp = new Bitmap(winW, winH, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.Clear(Color.Transparent);

                RectangleF pillRect = new RectangleF((winW - curW) / 2f, 8f, curW, curH);
                float radius = Math.Min(curH / 2f, 24f * scale);

                GlassThemeEngine.DrawLiquidGlassSurface(g, pillRect, radius, Color.FromArgb(10, 12, 18), accent, 244, 1.0f);

                if (expandProgress < 0.25f)
                {
                    DrawCollapsedContent(g, pillRect, scale, accent, globalTime, settings);
                }
                else
                {
                    DrawExpandedContent(g, pillRect, scale, accent, globalTime);
                }

                islandForm.SetBitmap(bmp, winX, winY, 255);
                if (!islandForm.Visible) islandForm.Show();
            }

            wasMouseDown = isMouseDown;
        }

        private void DrawCollapsedContent(Graphics g, RectangleF pill, float scale, Color accent, float globalTime, AppSettings settings)
        {
            float cy = pill.Y + pill.Height / 2f;
            using (SolidBrush dot = new SolidBrush(accent))
                g.FillEllipse(dot, pill.X + 16f * scale, cy - 4f * scale, 8f * scale, 8f * scale);

            string text = settings.IslandShowClockWhenIdle ? DateTime.Now.ToString("HH:mm") : "iIsland";
            using (Font f = new Font("Segoe UI Semibold", 9.8f * scale, FontStyle.Bold))
            using (SolidBrush b = new SolidBrush(Color.White))
            {
                SizeF sz = g.MeasureString(text, f);
                g.DrawString(text, f, b, pill.X + 32f * scale, cy - sz.Height / 2f);
            }

            // Mini audio wave
            for (int i = 0; i < 4; i++)
            {
                float barH = (4f + 10f * Media.SpectrumBars[i * 3]) * scale;
                float bx = pill.Right - (36f - i * 6f) * scale;
                using (SolidBrush wb = new SolidBrush(accent))
                    g.FillRectangle(wb, bx, cy - barH / 2f, 3f * scale, barH);
            }
        }

        private void DrawExpandedContent(Graphics g, RectangleF pill, float scale, Color accent, float globalTime)
        {
            // Tabs Bar
            string[] tabs = new string[] { "🎵 Média", "⏱️ Focus", "⚙️ Réglages" };
            float tabW = 86f * scale;
            for (int i = 0; i < tabs.Length; i++)
            {
                float tx = pill.X + 18f * scale + i * (tabW + 6f);
                RectangleF tr = new RectangleF(tx, pill.Y + 10f * scale, tabW, 26f * scale);
                bool sel = (CurrentTab == (CapsuleTab)i);
                Color tbBg = sel ? Color.FromArgb(50, accent) : (hoveredTabIndex == i ? Color.FromArgb(28, 255, 255, 255) : Color.FromArgb(16, 255, 255, 255));
                using (GraphicsPath tp = GlassThemeEngine.CreateRoundedRect(tr, 8f * scale))
                using (SolidBrush tbb = new SolidBrush(tbBg))
                    g.FillPath(tbb, tp);

                using (Font tf = new Font("Segoe UI Semibold", 8.8f * scale, sel ? FontStyle.Bold : FontStyle.Regular))
                using (SolidBrush txtb = new SolidBrush(sel ? Color.White : Color.FromArgb(180, 200, 225)))
                {
                    SizeF tsz = g.MeasureString(tabs[i], tf);
                    g.DrawString(tabs[i], tf, txtb, tr.X + (tr.Width - tsz.Width) / 2f, tr.Y + (tr.Height - tsz.Height) / 2f);
                }
            }

            // Tab Body
            if (CurrentTab == CapsuleTab.AudioPlayer)
            {
                using (Font tf = new Font("Segoe UI Semibold", 10.5f * scale, FontStyle.Bold))
                using (Font af = new Font("Segoe UI", 9.0f * scale))
                using (SolidBrush wb = new SolidBrush(Color.White))
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(160, 175, 205)))
                {
                    g.DrawString(Media.TrackTitle, tf, wb, pill.X + 22f * scale, pill.Y + 48f * scale);
                    g.DrawString(Media.TrackArtist, af, sb, pill.X + 22f * scale, pill.Y + 70f * scale);
                }

                // Spectrum Bars
                for (int i = 0; i < 16; i++)
                {
                    float bh = Math.Max(3f, 38f * Media.SpectrumBars[i]) * scale;
                    float bx = pill.X + 22f * scale + i * 8f * scale;
                    using (SolidBrush sp = new SolidBrush(accent))
                        g.FillRectangle(sp, bx, pill.Bottom - 48f * scale - bh, 4.5f * scale, bh);
                }

                // Buttons: Prev, Play/Pause, Next
                DrawActionButton(g, pill.Right - 140f * scale, pill.Bottom - 52f * scale, 34f * scale, "⏮", 0, scale);
                DrawActionButton(g, pill.Right - 98f * scale, pill.Bottom - 52f * scale, 34f * scale, "⏯", 1, scale);
                DrawActionButton(g, pill.Right - 56f * scale, pill.Bottom - 52f * scale, 34f * scale, "⏭", 2, scale);
            }
            else if (CurrentTab == CapsuleTab.FocusTimer)
            {
                int m = TimerEngine.RemainingSeconds / 60;
                int s = TimerEngine.RemainingSeconds % 60;
                string tStr = string.Format("{0:00}:{1:00}", m, s);
                using (Font f = new Font("Segoe UI Semibold", 18f * scale, FontStyle.Bold))
                using (SolidBrush wb = new SolidBrush(Color.White))
                    g.DrawString(tStr, f, wb, pill.X + 26f * scale, pill.Y + 54f * scale);

                DrawActionButton(g, pill.Right - 130f * scale, pill.Bottom - 52f * scale, 56f * scale, TimerEngine.IsRunning ? "Pause" : "Démarrer", 3, scale);
                DrawActionButton(g, pill.Right - 66f * scale, pill.Bottom - 52f * scale, 52f * scale, "Reset", 4, scale);
            }
            else
            {
                using (Font f = new Font("Segoe UI", 9.5f * scale))
                using (SolidBrush wb = new SolidBrush(Color.FromArgb(210, 225, 250)))
                    g.DrawString("iIsland — Dynamic Island Portable pour Windows", f, wb, pill.X + 22f * scale, pill.Y + 52f * scale);

                DrawActionButton(g, pill.X + 22f * scale, pill.Bottom - 50f * scale, 160f * scale, "Ouvrir les Paramètres", 5, scale);
            }
        }

        private void DrawActionButton(Graphics g, float x, float y, float w, string label, int actionIdx, float scale)
        {
            RectangleF r = new RectangleF(x, y, w, 30f * scale);
            bool hov = (hoveredActionIndex == actionIdx);
            using (GraphicsPath p = GlassThemeEngine.CreateRoundedRect(r, 8f * scale))
            using (SolidBrush b = new SolidBrush(hov ? Color.FromArgb(60, 255, 255, 255) : Color.FromArgb(30, 255, 255, 255)))
                g.FillPath(b, p);

            using (Font f = new Font("Segoe UI Semibold", 8.8f * scale, FontStyle.Bold))
            using (SolidBrush tb = new SolidBrush(Color.White))
            {
                SizeF sz = g.MeasureString(label, f);
                g.DrawString(label, f, tb, r.X + (r.Width - sz.Width) / 2f, r.Y + (r.Height - sz.Height) / 2f);
            }
        }

        private void HandleInteractions(Point relMouse, bool isMouseDown, int winW, float curW, float curH, float scale)
        {
            float pillX = (winW - curW) / 2f;
            float pillY = 8f;

            hoveredTabIndex = -1;
            if (expandProgress > 0.6f)
            {
                float tabW = 86f * scale;
                for (int i = 0; i < 3; i++)
                {
                    RectangleF tr = new RectangleF(pillX + 18f * scale + i * (tabW + 6f), pillY + 10f * scale, tabW, 26f * scale);
                    if (tr.Contains(relMouse))
                    {
                        hoveredTabIndex = i;
                        if (isMouseDown && !wasMouseDown) CurrentTab = (CapsuleTab)i;
                    }
                }
            }

            hoveredActionIndex = -1;
            if (expandProgress > 0.8f && CurrentTab == CapsuleTab.AudioPlayer)
            {
                float pillRight = pillX + curW;
                float pillBottom = pillY + curH;
                RectangleF rPrev = new RectangleF(pillRight - 140f * scale, pillBottom - 52f * scale, 34f * scale, 30f * scale);
                RectangleF rPlay = new RectangleF(pillRight - 98f * scale, pillBottom - 52f * scale, 34f * scale, 30f * scale);
                RectangleF rNext = new RectangleF(pillRight - 56f * scale, pillBottom - 52f * scale, 34f * scale, 30f * scale);

                if (rPrev.Contains(relMouse)) { hoveredActionIndex = 0; if (isMouseDown && !wasMouseDown) NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PREV_TRACK); }
                else if (rPlay.Contains(relMouse)) { hoveredActionIndex = 1; if (isMouseDown && !wasMouseDown) NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_PLAY_PAUSE); }
                else if (rNext.Contains(relMouse)) { hoveredActionIndex = 2; if (isMouseDown && !wasMouseDown) NativeMethods.SendMediaKey(NativeMethods.VK_MEDIA_NEXT_TRACK); }
            }
            else if (expandProgress > 0.8f && CurrentTab == CapsuleTab.FocusTimer)
            {
                float pillRight = pillX + curW;
                float pillBottom = pillY + curH;
                RectangleF rTog = new RectangleF(pillRight - 130f * scale, pillBottom - 52f * scale, 56f * scale, 30f * scale);
                RectangleF rRes = new RectangleF(pillRight - 66f * scale, pillBottom - 52f * scale, 52f * scale, 30f * scale);
                if (rTog.Contains(relMouse)) { hoveredActionIndex = 3; if (isMouseDown && !wasMouseDown) TimerEngine.Toggle(); }
                else if (rRes.Contains(relMouse)) { hoveredActionIndex = 4; if (isMouseDown && !wasMouseDown) TimerEngine.Reset(); }
            }
            else if (expandProgress > 0.8f && CurrentTab == CapsuleTab.QuickSettings)
            {
                float pillBottom = pillY + curH;
                RectangleF rSet = new RectangleF(pillX + 22f * scale, pillBottom - 50f * scale, 160f * scale, 30f * scale);
                if (rSet.Contains(relMouse)) { hoveredActionIndex = 5; if (isMouseDown && !wasMouseDown && OnOpenSettingsRequested != null) OnOpenSettingsRequested(); }
            }
        }

        public void EnsureTopMost()
        {
            if (islandForm != null && islandForm.IsHandleCreated)
                NativeMethods.SetWindowPos(islandForm.Handle, NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOACTIVATE);
        }

        public void Dispose()
        {
            if (islandForm != null) { islandForm.Hide(); islandForm.Dispose(); islandForm = null; }
        }
    }

    public class OverlayController : IDisposable
    {
        public AppSettings Settings;
        public IslandWidget Island;
        private System.Windows.Forms.Timer tickTimer;
        private long startTicks = DateTime.UtcNow.Ticks;
        private NotifyIcon trayIcon;
        private SettingsForm settingsForm;

        public OverlayController()
        {
            Settings = AppSettings.Load();
            Island = new IslandWidget();
            Island.OnOpenSettingsRequested += ShowSettingsWindow;
            SetupTrayIcon();

            tickTimer = new System.Windows.Forms.Timer { Interval = 16 };
            tickTimer.Tick += OnTick;
            tickTimer.Start();
        }

        public float GlobalTimeSeconds { get { return (float)((DateTime.UtcNow.Ticks - startTicks) / 10000000.0); } }

        private void OnTick(object sender, EventArgs e)
        {
            float t = GlobalTimeSeconds;
            Point cursorPt = Point.Empty;
            NativeMethods.POINT rawPt;
            if (NativeMethods.GetCursorPos(out rawPt)) cursorPt = new Point(rawPt.X, rawPt.Y);

            Color[] pal = GlassThemeEngine.GetPalette(Settings, t);
            Island.UpdateAndRender(Settings, Screen.PrimaryScreen.Bounds, cursorPt, pal, t);
        }

        private void SetupTrayIcon()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "iIsland — Dynamic Island Liquide";
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Visible = true;

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem openItem = new ToolStripMenuItem("Ouvrir les Paramètres");
            openItem.Font = new Font(openItem.Font, FontStyle.Bold);
            openItem.Click += (s, e) => ShowSettingsWindow();

            ToolStripMenuItem timerItem = new ToolStripMenuItem("Démarrer / Pause Focus 25m");
            timerItem.Click += (s, e) => Island.TimerEngine.Toggle();

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Quitter iIsland");
            exitItem.Click += (s, e) => { trayIcon.Visible = false; Application.Exit(); };

            menu.Items.Add(openItem);
            menu.Items.Add(timerItem);
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
            if (Island != null) Island.Dispose();
            if (trayIcon != null) { trayIcon.Visible = false; trayIcon.Dispose(); }
        }
    }

    public class SettingsForm : Form
    {
        private OverlayController controller;

        public SettingsForm(OverlayController ctrl)
        {
            controller = ctrl;
            Text = "iIsland — Paramètres Dynamic Island";
            Size = new Size(540, 480);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(12, 14, 22);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9.2f);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            BuildUI();
        }

        private void BuildUI()
        {
            Label title = new Label { Text = "💧 iIsland — Dynamic Island", Location = new Point(24, 20), AutoSize = true, Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold) };
            Controls.Add(title);

            CheckBox enChk = new CheckBox { Text = "Activer la Capsule Flottante", Location = new Point(28, 70), AutoSize = true, Checked = controller.Settings.Enabled };
            enChk.CheckedChanged += (s, e) => { controller.Settings.Enabled = enChk.Checked; controller.Settings.Save(); };
            Controls.Add(enChk);

            CheckBox hovChk = new CheckBox { Text = "Déployer automatiquement au survol", Location = new Point(28, 105), AutoSize = true, Checked = controller.Settings.IslandExpandOnHover };
            hovChk.CheckedChanged += (s, e) => { controller.Settings.IslandExpandOnHover = hovChk.Checked; controller.Settings.Save(); };
            Controls.Add(hovChk);

            CheckBox clkChk = new CheckBox { Text = "Afficher l'horloge au repos", Location = new Point(28, 140), AutoSize = true, Checked = controller.Settings.IslandShowClockWhenIdle };
            clkChk.CheckedChanged += (s, e) => { controller.Settings.IslandShowClockWhenIdle = clkChk.Checked; controller.Settings.Save(); };
            Controls.Add(clkChk);

            Label scLbl = new Label { Text = "Échelle de la capsule (80% à 130%) :", Location = new Point(28, 185), AutoSize = true };
            Controls.Add(scLbl);

            TrackBar scBar = new TrackBar { Location = new Point(28, 210), Size = new Size(460, 45), Minimum = 80, Maximum = 130, Value = controller.Settings.IslandScalePercent };
            scBar.ValueChanged += (s, e) => { controller.Settings.IslandScalePercent = scBar.Value; controller.Settings.Save(); };
            Controls.Add(scBar);

            Label posLbl = new Label { Text = "Position de l'écran :", Location = new Point(28, 270), AutoSize = true };
            Controls.Add(posLbl);

            ComboBox posCombo = new ComboBox { Location = new Point(28, 295), Size = new Size(460, 30), DropDownStyle = ComboBoxStyle.DropDownList };
            posCombo.Items.AddRange(new string[] { "Suivre l'écran du curseur actif", "Toujours sur l'écran principal" });
            posCombo.SelectedIndex = (int)controller.Settings.IslandPlacement;
            posCombo.SelectedIndexChanged += (s, e) => { controller.Settings.IslandPlacement = (CapsuleScreenPlacement)posCombo.SelectedIndex; controller.Settings.Save(); };
            Controls.Add(posCombo);

            Button okBtn = new Button { Text = "Fermer", Location = new Point(380, 380), Size = new Size(110, 36), BackColor = Color.FromArgb(40, 115, 215), FlatStyle = FlatStyle.Flat };
            okBtn.Click += (s, e) => Hide();
            Controls.Add(okBtn);
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
