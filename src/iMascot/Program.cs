using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace iMascot
{
    public enum CompanionEntity { LuminaOrb = 0, KitsuneFox = 1, DracoCrystal = 2, NyxStar = 3, KuroNekoCat = 4, AxiomSphere = 5 }

    public class AppSettings
    {
        public bool Enabled = true;
        public CompanionEntity Entity = CompanionEntity.LuminaOrb;
        public int ScalePercent = 100;
        public bool FollowCursorEyes = true;
        public bool DanceToMusic = true;
        public bool ShowSpeechBubbles = true;
        public string CompanionName = "Lumina";
        public int PosX = -1;
        public int PosY = -1;

        private static string SettingsPath
        {
            get
            {
                string dir = Path.GetDirectoryName(Application.ExecutablePath);
                return Path.Combine(dir, "imascot_settings.json");
            }
        }

        public void Save()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine(string.Format("  \"Enabled\": {0},", Enabled ? "true" : "false"));
                sb.AppendLine(string.Format("  \"Entity\": {0},", (int)Entity));
                sb.AppendLine(string.Format("  \"ScalePercent\": {0},", ScalePercent));
                sb.AppendLine(string.Format("  \"FollowCursorEyes\": {0},", FollowCursorEyes ? "true" : "false"));
                sb.AppendLine(string.Format("  \"DanceToMusic\": {0},", DanceToMusic ? "true" : "false"));
                sb.AppendLine(string.Format("  \"ShowSpeechBubbles\": {0},", ShowSpeechBubbles ? "true" : "false"));
                sb.AppendLine(string.Format("  \"CompanionName\": \"{0}\",", CompanionName));
                sb.AppendLine(string.Format("  \"PosX\": {0},", PosX));
                sb.AppendLine(string.Format("  \"PosY\": {0}", PosY));
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
                s.Entity = (CompanionEntity)ReadInt(json, "Entity", (int)s.Entity);
                s.ScalePercent = ReadInt(json, "ScalePercent", s.ScalePercent);
                s.FollowCursorEyes = ReadBool(json, "FollowCursorEyes", s.FollowCursorEyes);
                s.DanceToMusic = ReadBool(json, "DanceToMusic", s.DanceToMusic);
                s.ShowSpeechBubbles = ReadBool(json, "ShowSpeechBubbles", s.ShowSpeechBubbles);
                s.PosX = ReadInt(json, "PosX", s.PosX);
                s.PosY = ReadInt(json, "PosY", s.PosY);
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

    public class CoreAudioMeter
    {
        private IAudioMeterInformation audioMeter;
        public float CurrentAudioPeak = 0f;

        public CoreAudioMeter()
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

        public void Update()
        {
            float peak = 0f;
            if (audioMeter != null) { try { audioMeter.GetPeakValue(out peak); } catch { } }
            if (peak > CurrentAudioPeak) CurrentAudioPeak = CurrentAudioPeak * 0.45f + peak * 0.55f;
            else CurrentAudioPeak = CurrentAudioPeak * 0.86f + peak * 0.14f;
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

    public class CompanionWindow : IDisposable
    {
        private LayeredOverlayForm mascotForm;
        public CoreAudioMeter Audio = new CoreAudioMeter();
        private float hoverY = 0f;
        private float lookX = 0f, lookY = 0f;
        private string speechText = "Coucou ! Je veille sur ton écran ✨";
        private long speechUntilTicks = DateTime.UtcNow.Ticks + 60000000L;

        private bool isDragging = false;
        private Point dragOffset = Point.Empty;
        private Point currentPos;

        public CompanionWindow(AppSettings settings)
        {
            mascotForm = new LayeredOverlayForm();
            Rectangle sb = Screen.PrimaryScreen.Bounds;
            if (settings.PosX >= 0 && settings.PosY >= 0)
                currentPos = new Point(settings.PosX, settings.PosY);
            else
                currentPos = new Point(sb.Right - 220, sb.Bottom - 260);

            mascotForm.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left) { isDragging = true; dragOffset = e.Location; }
            };
            mascotForm.MouseMove += (s, e) =>
            {
                if (isDragging)
                {
                    NativeMethods.POINT pt;
                    if (NativeMethods.GetCursorPos(out pt))
                    {
                        currentPos = new Point(pt.X - dragOffset.X, pt.Y - dragOffset.Y);
                        settings.PosX = currentPos.X;
                        settings.PosY = currentPos.Y;
                    }
                }
            };
            mascotForm.MouseUp += (s, e) =>
            {
                if (isDragging) { isDragging = false; settings.Save(); }
            };

            mascotForm.Show();
        }

        public void Say(string text, int durationMs = 5000)
        {
            speechText = text;
            speechUntilTicks = DateTime.UtcNow.Ticks + (durationMs * 10000L);
        }

        public void UpdateAndRender(AppSettings settings, float globalTime)
        {
            if (!settings.Enabled) { mascotForm.Hide(); return; }

            Audio.Update();
            float peak = Audio.CurrentAudioPeak;

            Point cursorPt = Point.Empty;
            NativeMethods.POINT rawPt;
            if (NativeMethods.GetCursorPos(out rawPt)) cursorPt = new Point(rawPt.X, rawPt.Y);

            float targetLookX = (cursorPt.X - (currentPos.X + 90f)) / 300f;
            float targetLookY = (cursorPt.Y - (currentPos.Y + 90f)) / 300f;
            lookX = lookX * 0.85f + Math.Max(-1f, Math.Min(1f, targetLookX)) * 0.15f;
            lookY = lookY * 0.85f + Math.Max(-1f, Math.Min(1f, targetLookY)) * 0.15f;

            float danceY = (settings.DanceToMusic && peak > 0.05f) ? (float)Math.Sin(globalTime * 14.0) * (peak * 18f) : 0f;
            hoverY = (float)Math.Sin(globalTime * 3.0) * 8f - danceY;

            int winW = 260, winH = 260;
            float scale = settings.ScalePercent / 100f;

            using (Bitmap bmp = new Bitmap(winW, winH, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.Clear(Color.Transparent);

                float cx = 110f, cy = 130f + hoverY;
                float r = 45f * scale;

                Color c1 = Color.FromArgb(255, 110, 215, 255);
                Color c2 = Color.FromArgb(255, 180, 130, 255);

                DrawLuminaOrb(g, cx, cy, r, lookX, lookY, c1, c2, globalTime, peak > 0.05f);

                // Speech Bubble
                if (settings.ShowSpeechBubbles && DateTime.UtcNow.Ticks < speechUntilTicks && !string.IsNullOrEmpty(speechText))
                {
                    DrawSpeechBubble(g, cx + 45f, cy - 65f, speechText, scale);
                }

                mascotForm.SetBitmap(bmp, currentPos.X, currentPos.Y, 255);
                if (!mascotForm.Visible) mascotForm.Show();
            }
        }

        private void DrawLuminaOrb(Graphics g, float cx, float cy, float r, float lx, float ly, Color c1, Color c2, float t, bool isAudio)
        {
            // Gyroscopic 3D Rings
            float rot1 = (t * 22f) % 360f;
            using (Pen ringPen = new Pen(Color.FromArgb(170, 255, 255, 255), 1.8f))
            {
                g.DrawEllipse(ringPen, cx - r * 1.35f, cy - r * 0.55f, r * 2.7f, r * 1.1f);
            }

            // Core Glass Sphere
            using (GraphicsPath p = new GraphicsPath())
            {
                p.AddEllipse(cx - r, cy - r, r * 2f, r * 2f);
                using (PathGradientBrush pgb = new PathGradientBrush(p))
                {
                    pgb.CenterColor = Color.FromArgb(235, Color.White);
                    pgb.SurroundColors = new Color[] { Color.FromArgb(200, c1) };
                    g.FillPath(pgb, p);
                }
            }

            // Eyes following cursor
            float eyeDist = r * 0.36f;
            float eyeR = r * 0.18f;
            float ex1 = cx - eyeDist + lx * 7f;
            float ex2 = cx + eyeDist + lx * 7f;
            float ey = cy - r * 0.08f + ly * 7f;

            using (SolidBrush eb = new SolidBrush(Color.FromArgb(240, 20, 28, 45)))
            {
                g.FillEllipse(eb, ex1 - eyeR, ey - eyeR * 1.25f, eyeR * 2f, eyeR * 2.5f);
                g.FillEllipse(eb, ex2 - eyeR, ey - eyeR * 1.25f, eyeR * 2f, eyeR * 2.5f);
            }
            // Specular Glint
            using (SolidBrush gl = new SolidBrush(Color.White))
            {
                g.FillEllipse(gl, ex1 - eyeR * 0.4f, ey - eyeR * 0.9f, eyeR * 0.8f, eyeR * 0.8f);
                g.FillEllipse(gl, ex2 - eyeR * 0.4f, ey - eyeR * 0.9f, eyeR * 0.8f, eyeR * 0.8f);
            }

            // Cheeks
            using (SolidBrush ch = new SolidBrush(Color.FromArgb(90, 255, 120, 160)))
            {
                g.FillEllipse(ch, cx - r * 0.55f, cy + r * 0.22f, r * 0.32f, r * 0.20f);
                g.FillEllipse(ch, cx + r * 0.23f, cy + r * 0.22f, r * 0.32f, r * 0.20f);
            }
        }

        private void DrawSpeechBubble(Graphics g, float x, float y, string text, float scale)
        {
            using (Font f = new Font("Segoe UI Semibold", 8.8f * scale, FontStyle.Bold))
            {
                SizeF sz = g.MeasureString(text, f);
                RectangleF r = new RectangleF(x, y, Math.Min(160f, sz.Width + 16f), sz.Height + 12f);
                using (GraphicsPath p = new GraphicsPath())
                {
                    p.AddRectangle(r);
                    using (SolidBrush b = new SolidBrush(Color.FromArgb(230, 16, 20, 32))) g.FillPath(b, p);
                    using (Pen pen = new Pen(Color.FromArgb(120, 255, 255, 255), 1.2f)) g.DrawPath(pen, p);
                }
                using (SolidBrush tb = new SolidBrush(Color.White))
                {
                    g.DrawString(text, f, tb, new RectangleF(r.X + 8f, r.Y + 6f, r.Width - 16f, r.Height - 12f));
                }
            }
        }

        public void Dispose()
        {
            if (mascotForm != null) { mascotForm.Hide(); mascotForm.Dispose(); mascotForm = null; }
        }
    }

    public class OverlayController : IDisposable
    {
        public AppSettings Settings;
        public CompanionWindow Companion;
        private System.Windows.Forms.Timer tickTimer;
        private long startTicks = DateTime.UtcNow.Ticks;
        private NotifyIcon trayIcon;

        public OverlayController()
        {
            Settings = AppSettings.Load();
            Companion = new CompanionWindow(Settings);
            SetupTrayIcon();

            tickTimer = new System.Windows.Forms.Timer { Interval = 16 };
            tickTimer.Tick += (s, e) =>
            {
                float t = (float)((DateTime.UtcNow.Ticks - startTicks) / 10000000.0);
                Companion.UpdateAndRender(Settings, t);
            };
            tickTimer.Start();
        }

        private void SetupTrayIcon()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "iMascot — Compagnon Céleste en Verre";
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Visible = true;

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem sayItem = new ToolStripMenuItem("Parler");
            sayItem.Click += (s, e) => Companion.Say("Je suis avec toi ! 💖");

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Quitter iMascot");
            exitItem.Click += (s, e) => { trayIcon.Visible = false; Application.Exit(); };

            menu.Items.Add(sayItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(exitItem);

            trayIcon.ContextMenuStrip = menu;
        }

        public void Dispose()
        {
            if (tickTimer != null) { tickTimer.Stop(); tickTimer.Dispose(); }
            if (Companion != null) Companion.Dispose();
            if (trayIcon != null) { trayIcon.Visible = false; trayIcon.Dispose(); }
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
                Application.Run();
            }
        }
    }
}
