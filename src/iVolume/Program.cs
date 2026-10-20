using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace iVolume
{
    public static class NativeMethods
    {
        public const int WS_EX_LAYERED = 0x80000;
        public const int WS_EX_TOOLWINDOW = 0x80;
        public const int WS_EX_TOPMOST = 0x8;
        public const byte VK_VOLUME_MUTE = 0xAD;
        public const byte VK_VOLUME_DOWN = 0xAE;
        public const byte VK_VOLUME_UP = 0xAF;
        public const byte VK_MEDIA_NEXT_TRACK = 0xB0;
        public const byte VK_MEDIA_PREV_TRACK = 0xB1;
        public const byte VK_MEDIA_STOP = 0xB2;
        public const byte VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const uint KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("user32.dll")]
        public static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        [StructLayout(LayoutKind.Sequential)]
        public struct WindowCompositionAttributeData { public int Attribute; public IntPtr Data; public int SizeOfData; }

        [StructLayout(LayoutKind.Sequential)]
        public struct AccentPolicy { public int AccentState; public int AccentFlags; public int GradientColor; public int AnimationId; }

        public static void EnableWindowGlassBlur(IntPtr handle, Color tintColor, byte opacity)
        {
            try
            {
                int dark = 1;
                DwmSetWindowAttribute(handle, 20, ref dark, 4);
                int round = 2;
                DwmSetWindowAttribute(handle, 33, ref round, 4);
                int backdrop = 3;
                DwmSetWindowAttribute(handle, 38, ref backdrop, 4);
            }
            catch { }

            try
            {
                var policy = new AccentPolicy
                {
                    AccentState = 4, // ACCENT_ENABLE_ACRYLICBLURBEHIND
                    AccentFlags = 2,
                    GradientColor = (opacity << 24) | (tintColor.B << 16) | (tintColor.G << 8) | tintColor.R
                };
                int size = Marshal.SizeOf(policy);
                IntPtr pPolicy = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(policy, pPolicy, false);
                var data = new WindowCompositionAttributeData { Attribute = 19, Data = pPolicy, SizeOfData = size };
                SetWindowCompositionAttribute(handle, ref data);
                Marshal.FreeHGlobal(pPolicy);
            }
            catch { }
        }

        public static void SendMediaKey(byte vk)
        {
            keybd_event(vk, 0, 0, UIntPtr.Zero);
            keybd_event(vk, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
    }

    public class LiquidGlassVolumeForm : Form
    {
        private int volume = 75;
        private bool isPlaying = true;
        private float[] spectrumBars = new float[16];
        private Random rnd = new Random();
        private System.Windows.Forms.Timer animTimer;
        private System.Windows.Forms.Timer autoHideTimer;
        private Rectangle sliderRect = Rectangle.Empty;
        private bool isDragging = false;

        public LiquidGlassVolumeForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            DoubleBuffered = true;
            Size = new Size(380, 160);
            BackColor = Color.FromArgb(12, 14, 22);

            Screen primary = Screen.PrimaryScreen;
            Location = new Point(primary.Bounds.Left + (primary.Bounds.Width - Width) / 2, primary.Bounds.Top + 60);

            for (int i = 0; i < spectrumBars.Length; i++) spectrumBars[i] = 0.2f + (float)rnd.NextDouble() * 0.5f;

            animTimer = new System.Windows.Forms.Timer { Interval = 30 };
            animTimer.Tick += (s, e) =>
            {
                for (int i = 0; i < spectrumBars.Length; i++)
                {
                    float target = isPlaying ? (0.15f + (float)rnd.NextDouble() * 0.8f) : 0.05f;
                    spectrumBars[i] = spectrumBars[i] * 0.7f + target * 0.3f;
                }
                Invalidate();
            };
            animTimer.Start();

            autoHideTimer = new System.Windows.Forms.Timer { Interval = 4500 };
            autoHideTimer.Tick += (s, e) => { autoHideTimer.Stop(); };

            MouseDown += (s, e) =>
            {
                if (sliderRect.Contains(e.Location))
                {
                    isDragging = true;
                    UpdateVolumeFromMouse(e.X);
                }
            };

            MouseMove += (s, e) =>
            {
                if (isDragging) UpdateVolumeFromMouse(e.X);
            };

            MouseUp += (s, e) => isDragging = false;

            MouseWheel += (s, e) =>
            {
                if (e.Delta > 0) AdjustVolume(2);
                else AdjustVolume(-2);
            };
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            NativeMethods.EnableWindowGlassBlur(Handle, Color.FromArgb(10, 14, 22), 235);
        }

        private void AdjustVolume(int delta)
        {
            volume = Math.Max(0, Math.Min(100, volume + delta));
            if (delta > 0) NativeMethods.SendMediaKey(NativeMethods.VK_VOLUME_UP);
            else NativeMethods.SendMediaKey(NativeMethods.VK_VOLUME_DOWN);
            Invalidate();
        }

        private void UpdateVolumeFromMouse(int mouseX)
        {
            float ratio = (mouseX - sliderRect.X) / (float)sliderRect.Width;
            ratio = Math.Max(0f, Math.Min(1f, ratio));
            int newVol = (int)(ratio * 100);
            if (newVol > volume) NativeMethods.SendMediaKey(NativeMethods.VK_VOLUME_UP);
            else if (newVol < volume) NativeMethods.SendMediaKey(NativeMethods.VK_VOLUME_DOWN);
            volume = newVol;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            Color accent = Color.FromArgb(70, 200, 255);

            // Frosted Liquid Glass Card
            using (GraphicsPath p = CreateRoundedRect(r, 22f))
            {
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(240, 10, 12, 18)))
                    g.FillPath(bg, p);

                using (LinearGradientBrush sheen = new LinearGradientBrush(r, Color.FromArgb(38, 255, 255, 255), Color.FromArgb(12, accent), 45f))
                    g.FillPath(sheen, p);

                using (LinearGradientBrush rim = new LinearGradientBrush(r, Color.FromArgb(100, 255, 255, 255), Color.FromArgb(35, accent), 60f))
                using (Pen rimPen = new Pen(rim, 1.3f))
                    g.DrawPath(rimPen, p);
            }

            // Media Badge
            RectangleF badgeRect = new RectangleF(20, 20, 44, 44);
            using (GraphicsPath bp = CreateRoundedRect(badgeRect, 10f))
            using (LinearGradientBrush bgb = new LinearGradientBrush(badgeRect, Color.FromArgb(255, 30, 80, 160), Color.FromArgb(255, 120, 40, 140), 45f))
            {
                g.FillPath(bgb, bp);
            }

            using (Font bf = new Font("Segoe UI", 16f))
            using (SolidBrush bb = new SolidBrush(Color.White))
            {
                g.DrawString("🎵", bf, bb, badgeRect.X + 8, badgeRect.Y + 8);
            }

            // Track title & Artist
            using (Font tf = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold))
            using (Font af = new Font("Segoe UI", 8.8f))
            using (SolidBrush tb = new SolidBrush(Color.White))
            using (SolidBrush ab = new SolidBrush(Color.FromArgb(170, 185, 215)))
            {
                g.DrawString("Starboy (Liquid Glass Remix)", tf, tb, 74, 22);
                g.DrawString("The Weeknd • Apple Music / Spotify", af, ab, 74, 44);
            }

            // Audio Visualizer spectrum bars
            int specStartX = Width - 100;
            int specY = 28;
            for (int i = 0; i < 8; i++)
            {
                float bh = spectrumBars[i] * 28f;
                RectangleF barRect = new RectangleF(specStartX + i * 8, specY + (28 - bh), 4.5f, bh);
                using (GraphicsPath sp = CreateRoundedRect(barRect, 2f))
                using (SolidBrush sb = new SolidBrush(accent))
                    g.FillPath(sb, sp);
            }

            // Volume Slider
            sliderRect = new Rectangle(24, 82, Width - 48, 16);
            using (GraphicsPath sp = CreateRoundedRect(sliderRect, 8f))
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(30, 255, 255, 255)))
                g.FillPath(sb, sp);

            float fillW = Math.Max(16f, sliderRect.Width * (volume / 100f));
            RectangleF fillRect = new RectangleF(sliderRect.X, sliderRect.Y, fillW, 16);
            using (GraphicsPath fp = CreateRoundedRect(fillRect, 8f))
            using (LinearGradientBrush fb = new LinearGradientBrush(fillRect, Color.FromArgb(60, 180, 255), Color.FromArgb(180, 120, 255), 0f))
                g.FillPath(fb, fp);

            // Slider Knob
            using (SolidBrush kb = new SolidBrush(Color.White))
                g.FillEllipse(kb, sliderRect.X + fillW - 14, sliderRect.Y + 1, 14, 14);

            // Bottom status & buttons
            using (Font vf = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold))
            using (SolidBrush vb = new SolidBrush(Color.FromArgb(200, 215, 240)))
            {
                g.DrawString(string.Format("🔊 Volume: {0}%", volume), vf, vb, 24, 118);
            }

            // Media control buttons glyphs
            using (Font cf = new Font("Segoe UI", 12f))
            using (SolidBrush cb = new SolidBrush(Color.White))
            {
                g.DrawString("⏮", cf, cb, Width - 130, 114);
                g.DrawString(isPlaying ? "⏸" : "▶", cf, cb, Width - 95, 114);
                g.DrawString("⏭", cf, cb, Width - 60, 114);
            }
        }

        private GraphicsPath CreateRoundedRect(RectangleF rect, float radius)
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

    static class Program
    {
        [STAThread]
        static void Main()
        {
            NativeMethods.SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LiquidGlassVolumeForm());
        }
    }
}
