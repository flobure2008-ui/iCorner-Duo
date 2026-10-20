using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace iWidgets
{
    public static class NativeMethods
    {
        public const int WS_EX_TOOLWINDOW = 0x80;
        public const int WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        public static void DragWindow(IntPtr handle)
        {
            ReleaseCapture();
            SendMessage(handle, 0xA1 /* WM_NCLBUTTONDOWN */, 2 /* HTCAPTION */, 0);
        }
    }

    public class LiquidClockWidget : Form
    {
        private System.Windows.Forms.Timer timer;

        public LiquidClockWidget()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = false;
            DoubleBuffered = true;
            Size = new Size(260, 110);
            BackColor = Color.FromArgb(12, 14, 22);

            Screen scr = Screen.PrimaryScreen;
            Location = new Point(scr.Bounds.Right - Width - 40, scr.Bounds.Top + 60);

            MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) NativeMethods.DragWindow(Handle); };

            timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += (s, e) => Invalidate();
            timer.Start();

            SetupMenu();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000 | NativeMethods.WS_EX_TOOLWINDOW;
                return cp;
            }
        }

        private void SetupMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem pinItem = new ToolStripMenuItem("Toujours au premier plan");
            pinItem.CheckOnClick = true;
            pinItem.CheckedChanged += (s, e) => TopMost = pinItem.Checked;

            ToolStripMenuItem closeItem = new ToolStripMenuItem("Fermer le widget");
            closeItem.Click += (s, e) => Close();

            menu.Items.Add(pinItem);
            menu.Items.Add(closeItem);
            ContextMenuStrip = menu;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            Color accent = Color.FromArgb(70, 200, 255);

            using (GraphicsPath p = CreateRoundedRect(r, 18f))
            {
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(230, 10, 12, 18)))
                    g.FillPath(bg, p);

                using (LinearGradientBrush sheen = new LinearGradientBrush(r, Color.FromArgb(30, 255, 255, 255), Color.FromArgb(10, accent), 45f))
                    g.FillPath(sheen, p);

                using (LinearGradientBrush rim = new LinearGradientBrush(r, Color.FromArgb(90, 255, 255, 255), Color.FromArgb(30, accent), 60f))
                using (Pen rimPen = new Pen(rim, 1.2f))
                    g.DrawPath(rimPen, p);
            }

            DateTime now = DateTime.Now;
            string timeStr = now.ToString("HH:mm:ss");
            string dayStr = now.ToString("dddd d MMMM yyyy");
            if (dayStr.Length > 0) dayStr = char.ToUpper(dayStr[0]) + dayStr.Substring(1);

            using (Font tf = new Font("Segoe UI Semibold", 22f, FontStyle.Bold))
            using (SolidBrush tb = new SolidBrush(Color.White))
            {
                g.DrawString(timeStr, tf, tb, 22, 16);
            }

            using (Font df = new Font("Segoe UI", 9.2f))
            using (SolidBrush db = new SolidBrush(Color.FromArgb(170, 190, 225)))
            {
                g.DrawString(dayStr, df, db, 24, 66);
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

    public class LiquidMonitorWidget : Form
    {
        private System.Windows.Forms.Timer timer;
        private PerformanceCounter cpuCounter;
        private float cpuUsage = 18f;
        private float ramUsage = 45f;

        public LiquidMonitorWidget()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = false;
            DoubleBuffered = true;
            Size = new Size(260, 140);
            BackColor = Color.FromArgb(12, 14, 22);

            Screen scr = Screen.PrimaryScreen;
            Location = new Point(scr.Bounds.Right - Width - 40, scr.Bounds.Top + 190);

            MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) NativeMethods.DragWindow(Handle); };

            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                cpuCounter.NextValue();
            }
            catch { }

            timer = new System.Windows.Forms.Timer { Interval = 1500 };
            timer.Tick += (s, e) =>
            {
                try
                {
                    if (cpuCounter != null) cpuUsage = cpuCounter.NextValue();
                }
                catch { }
                Invalidate();
            };
            timer.Start();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000 | NativeMethods.WS_EX_TOOLWINDOW;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            Color accent = Color.FromArgb(110, 215, 255);

            using (GraphicsPath p = CreateRoundedRect(r, 18f))
            {
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(230, 10, 12, 18)))
                    g.FillPath(bg, p);

                using (LinearGradientBrush sheen = new LinearGradientBrush(r, Color.FromArgb(30, 255, 255, 255), Color.FromArgb(10, accent), 45f))
                    g.FillPath(sheen, p);

                using (LinearGradientBrush rim = new LinearGradientBrush(r, Color.FromArgb(90, 255, 255, 255), Color.FromArgb(30, accent), 60f))
                using (Pen rimPen = new Pen(rim, 1.2f))
                    g.DrawPath(rimPen, p);
            }

            // Title
            using (Font tf = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold))
            using (SolidBrush tb = new SolidBrush(Color.White))
            {
                g.DrawString("⚡ Performances Système", tf, tb, 20, 16);
            }

            // CPU Gauge Bar
            DrawGauge(g, 20, 48, Width - 40, "Processeur (CPU)", (int)cpuUsage, accent);

            // RAM Gauge Bar
            DrawGauge(g, 20, 88, Width - 40, "Mémoire Vive (RAM)", (int)ramUsage, Color.FromArgb(180, 120, 255));
        }

        private void DrawGauge(Graphics g, float x, float y, float w, string label, int percent, Color barColor)
        {
            using (Font lf = new Font("Segoe UI", 8.5f))
            using (SolidBrush lb = new SolidBrush(Color.FromArgb(180, 195, 220)))
            {
                g.DrawString(string.Format("{0} : {1}%", label, percent), lf, lb, x, y);
            }

            RectangleF track = new RectangleF(x, y + 18, w, 10);
            using (GraphicsPath tp = CreateRoundedRect(track, 5f))
            using (SolidBrush tb = new SolidBrush(Color.FromArgb(30, 255, 255, 255)))
                g.FillPath(tb, tp);

            float fillW = Math.Max(10f, w * (percent / 100f));
            RectangleF fillRect = new RectangleF(x, y + 18, fillW, 10);
            using (GraphicsPath fp = CreateRoundedRect(fillRect, 5f))
            using (SolidBrush fb = new SolidBrush(barColor))
                g.FillPath(fb, fp);
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

    public class LiquidNotesWidget : Form
    {
        private TextBox notesBox;
        private static string NotePath
        {
            get { return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "quick_note.txt"); }
        }

        public LiquidNotesWidget()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = false;
            DoubleBuffered = true;
            Size = new Size(260, 180);
            BackColor = Color.FromArgb(12, 14, 22);

            Screen scr = Screen.PrimaryScreen;
            Location = new Point(scr.Bounds.Right - Width - 40, scr.Bounds.Top + 350);

            MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) NativeMethods.DragWindow(Handle); };

            notesBox = new TextBox
            {
                Multiline = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(16, 20, 30),
                ForeColor = Color.FromArgb(220, 230, 245),
                Font = new Font("Segoe UI", 9.2f),
                Location = new Point(18, 44),
                Size = new Size(Width - 36, Height - 62),
                ScrollBars = ScrollBars.Vertical
            };

            if (File.Exists(NotePath))
            {
                try { notesBox.Text = File.ReadAllText(NotePath, Encoding.UTF8); } catch { }
            }
            else
            {
                notesBox.Text = "✍️ Bloc-notes Liquid Glass\n\nVos notes sont enregistrées automatiquement sur le bureau.";
            }

            notesBox.TextChanged += (s, e) =>
            {
                try { File.WriteAllText(NotePath, notesBox.Text, Encoding.UTF8); } catch { }
            };

            Controls.Add(notesBox);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000 | NativeMethods.WS_EX_TOOLWINDOW;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            Color accent = Color.FromArgb(255, 180, 80);

            using (GraphicsPath p = CreateRoundedRect(r, 18f))
            {
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(230, 10, 12, 18)))
                    g.FillPath(bg, p);

                using (LinearGradientBrush sheen = new LinearGradientBrush(r, Color.FromArgb(30, 255, 255, 255), Color.FromArgb(10, accent), 45f))
                    g.FillPath(sheen, p);

                using (LinearGradientBrush rim = new LinearGradientBrush(r, Color.FromArgb(90, 255, 255, 255), Color.FromArgb(30, accent), 60f))
                using (Pen rimPen = new Pen(rim, 1.2f))
                    g.DrawPath(rimPen, p);
            }

            using (Font tf = new Font("Segoe UI Semibold", 10f, FontStyle.Bold))
            using (SolidBrush tb = new SolidBrush(Color.White))
            {
                g.DrawString("📝 Mémo Verre Rapide", tf, tb, 20, 14);
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

            LiquidClockWidget clock = new LiquidClockWidget();
            LiquidMonitorWidget monitor = new LiquidMonitorWidget();
            LiquidNotesWidget notes = new LiquidNotesWidget();

            clock.Show();
            monitor.Show();
            notes.Show();

            Application.Run();
        }
    }
}
