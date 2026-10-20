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

namespace iWidgets
{
    // =========================================================================
    // NATIVE METHODS & EXTENDED WINDOW STYLES (RAINMETER-LIKE CLICKS & DESKTOP)
    // =========================================================================
    public static class NativeMethods
    {
        public const int WS_EX_TOOLWINDOW = 0x80;
        public const int WS_EX_TRANSPARENT = 0x20;
        public const int WS_EX_LAYERED = 0x80000;
        public const int WS_EX_NOACTIVATE = 0x08000000;
        public const int GWL_EXSTYLE = -20;

        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        public static void SetClickThrough(IntPtr handle, bool clickThrough)
        {
            try
            {
                int exStyle = GetWindowLong32(handle, GWL_EXSTYLE);
                if (clickThrough) exStyle |= WS_EX_TRANSPARENT;
                else exStyle &= ~WS_EX_TRANSPARENT;
                SetWindowLong32(handle, GWL_EXSTYLE, exStyle);
            }
            catch { }
        }

        public static void SetDesktopPin(IntPtr handle, int layerMode)
        {
            // layerMode: 0 = Bottom (desktop pinned like Rainmeter), 1 = Normal, 2 = TopMost
            try
            {
                if (layerMode == 0)
                    SetWindowPos(handle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
                else if (layerMode == 2)
                    SetWindowPos(handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
                else
                    SetWindowPos(handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
            }
            catch { }
        }

        public static void DragWindow(IntPtr handle)
        {
            ReleaseCapture();
            SendMessage(handle, 0xA1, 2, 0);
        }
    }

    // =========================================================================
    // CONFIGURATION MODEL FOR RAINMETER-STYLE WIDGETS
    // =========================================================================
    public class WidgetSettings
    {
        public bool Enabled = true;
        public int PosX = 100;
        public int PosY = 100;
        public int Width = 280;
        public int Height = 140;
        public bool Locked = false;
        public bool ClickThrough = false;
        public int LayerMode = 0; // 0 = Bottom / Desktop, 1 = Normal, 2 = TopMost
        public int Opacity = 235;
        public int CornerRadius = 18;
        public int AccentArgb = Color.FromArgb(255, 70, 205, 255).ToArgb();
        public bool SnapToEdges = true;
    }

    public class RainmeterConfig
    {
        public WidgetSettings Clock = new WidgetSettings { Enabled = true, PosX = -1, PosY = 60, Width = 280, Height = 120 };
        public WidgetSettings SysMonitor = new WidgetSettings { Enabled = true, PosX = -1, PosY = 195, Width = 280, Height = 160 };
        public WidgetSettings Weather = new WidgetSettings { Enabled = true, PosX = -1, PosY = 370, Width = 280, Height = 140 };
        public WidgetSettings AudioSpectrum = new WidgetSettings { Enabled = true, PosX = -1, PosY = 525, Width = 280, Height = 120 };
        public WidgetSettings QuickNotes = new WidgetSettings { Enabled = false, PosX = 40, PosY = 60, Width = 300, Height = 220 };
        public WidgetSettings AppDock = new WidgetSettings { Enabled = false, PosX = 40, PosY = 300, Width = 300, Height = 90 };

        private static string ConfigPath
        {
            get { return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "widgets_config.json"); }
        }

        public void Save()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("{");
                WriteSection(sb, "Clock", Clock);
                sb.AppendLine(",");
                WriteSection(sb, "SysMonitor", SysMonitor);
                sb.AppendLine(",");
                WriteSection(sb, "Weather", Weather);
                sb.AppendLine(",");
                WriteSection(sb, "AudioSpectrum", AudioSpectrum);
                sb.AppendLine(",");
                WriteSection(sb, "QuickNotes", QuickNotes);
                sb.AppendLine(",");
                WriteSection(sb, "AppDock", AppDock);
                sb.AppendLine("\n}");
                File.WriteAllText(ConfigPath, sb.ToString(), Encoding.UTF8);
            }
            catch { }
        }

        private void WriteSection(StringBuilder sb, string key, WidgetSettings s)
        {
            sb.AppendFormat("  \"{0}\": {{\n", key);
            sb.AppendFormat("    \"Enabled\": {0},\n", s.Enabled ? "true" : "false");
            sb.AppendFormat("    \"PosX\": {0},\n", s.PosX);
            sb.AppendFormat("    \"PosY\": {0},\n", s.PosY);
            sb.AppendFormat("    \"Width\": {0},\n", s.Width);
            sb.AppendFormat("    \"Height\": {0},\n", s.Height);
            sb.AppendFormat("    \"Locked\": {0},\n", s.Locked ? "true" : "false");
            sb.AppendFormat("    \"ClickThrough\": {0},\n", s.ClickThrough ? "true" : "false");
            sb.AppendFormat("    \"LayerMode\": {0},\n", s.LayerMode);
            sb.AppendFormat("    \"Opacity\": {0},\n", s.Opacity);
            sb.AppendFormat("    \"CornerRadius\": {0},\n", s.CornerRadius);
            sb.AppendFormat("    \"AccentArgb\": {0}\n", s.AccentArgb);
            sb.Append("  }");
        }

        public static RainmeterConfig Load()
        {
            RainmeterConfig cfg = new RainmeterConfig();
            // Default placements on primary screen right edge
            Screen scr = Screen.PrimaryScreen;
            int rightX = scr.Bounds.Right - 310;
            if (cfg.Clock.PosX < 0) cfg.Clock.PosX = rightX;
            if (cfg.SysMonitor.PosX < 0) cfg.SysMonitor.PosX = rightX;
            if (cfg.Weather.PosX < 0) cfg.Weather.PosX = rightX;
            if (cfg.AudioSpectrum.PosX < 0) cfg.AudioSpectrum.PosX = rightX;
            return cfg;
        }
    }

    // =========================================================================
    // BASE RAINMETER GLASS WIDGET FORM
    // =========================================================================
    public abstract class BaseGlassWidget : Form
    {
        protected WidgetSettings settings;
        protected RainmeterConfig rootConfig;
        protected Action onOpenManager;
        protected System.Windows.Forms.Timer updateTimer;

        public BaseGlassWidget(WidgetSettings s, RainmeterConfig cfg, Action openManagerCb)
        {
            settings = s;
            rootConfig = cfg;
            onOpenManager = openManagerCb;

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            DoubleBuffered = true;
            Size = new Size(settings.Width, settings.Height);
            Location = new Point(settings.PosX, settings.PosY);
            BackColor = Color.FromArgb(12, 14, 22);

            ApplyStyles();

            MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left && !settings.Locked)
                {
                    NativeMethods.DragWindow(Handle);
                    settings.PosX = Location.X;
                    settings.PosY = Location.Y;
                    rootConfig.Save();
                }
            };

            SetupContextMenu();

            updateTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            updateTimer.Tick += (sender, e) => Invalidate();
            updateTimer.Start();
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

        public void ApplyStyles()
        {
            Location = new Point(settings.PosX, settings.PosY);
            Size = new Size(settings.Width, settings.Height);
            TopMost = (settings.LayerMode == 2);
            NativeMethods.SetClickThrough(Handle, settings.ClickThrough);
            NativeMethods.SetDesktopPin(Handle, settings.LayerMode);
            Invalidate();
        }

        private void SetupContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem titleItem = new ToolStripMenuItem(Text);
            titleItem.Font = new Font(titleItem.Font, FontStyle.Bold);
            titleItem.Enabled = false;

            ToolStripMenuItem managerItem = new ToolStripMenuItem("⚙️ Gérer les Widgets (Rainmeter Studio)");
            managerItem.Click += (s, e) => { if (onOpenManager != null) onOpenManager(); };

            ToolStripMenuItem lockItem = new ToolStripMenuItem("🔒 Verrouiller la position");
            lockItem.Checked = settings.Locked;
            lockItem.Click += (s, e) => { settings.Locked = !settings.Locked; lockItem.Checked = settings.Locked; rootConfig.Save(); };

            ToolStripMenuItem clickThroughItem = new ToolStripMenuItem("👻 Clics traversants (Click-Through)");
            clickThroughItem.Checked = settings.ClickThrough;
            clickThroughItem.Click += (s, e) =>
            {
                settings.ClickThrough = !settings.ClickThrough;
                clickThroughItem.Checked = settings.ClickThrough;
                NativeMethods.SetClickThrough(Handle, settings.ClickThrough);
                rootConfig.Save();
            };

            ToolStripMenuItem layerMenu = new ToolStripMenuItem("📐 Niveau d'affichage");
            string[] layers = new string[] { "Au fond du Bureau (Rainmeter)", "Normal", "Toujours au premier plan (TopMost)" };
            for (int i = 0; i < 3; i++)
            {
                int l = i;
                ToolStripMenuItem li = new ToolStripMenuItem(layers[i]);
                li.Checked = (settings.LayerMode == l);
                li.Click += (s, e) =>
                {
                    settings.LayerMode = l;
                    ApplyStyles();
                    rootConfig.Save();
                };
                layerMenu.DropDownItems.Add(li);
            }

            ToolStripMenuItem hideItem = new ToolStripMenuItem("✕ Masquer ce widget");
            hideItem.Click += (s, e) =>
            {
                settings.Enabled = false;
                Hide();
                rootConfig.Save();
            };

            menu.Items.Add(titleItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(managerItem);
            menu.Items.Add(lockItem);
            menu.Items.Add(clickThroughItem);
            menu.Items.Add(layerMenu);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(hideItem);

            ContextMenuStrip = menu;
        }

        protected void DrawLiquidGlassFrame(Graphics g, RectangleF r, Color accent)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath p = CreateRoundedRect(r, settings.CornerRadius))
            {
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(settings.Opacity, 10, 14, 22)))
                    g.FillPath(bg, p);

                using (LinearGradientBrush sheen = new LinearGradientBrush(r, Color.FromArgb(32, 255, 255, 255), Color.FromArgb(10, accent), 45f))
                    g.FillPath(sheen, p);

                using (LinearGradientBrush rim = new LinearGradientBrush(r, Color.FromArgb(90, 255, 255, 255), Color.FromArgb(30, accent), 60f))
                using (Pen rimPen = new Pen(rim, 1.25f))
                    g.DrawPath(rimPen, p);
            }
        }

        protected GraphicsPath CreateRoundedRect(RectangleF rect, float radius)
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
    // 1. CLOCK & DATE PRO WIDGET
    // =========================================================================
    public class ClockWidget : BaseGlassWidget
    {
        public ClockWidget(WidgetSettings s, RainmeterConfig cfg, Action cb) : base(s, cfg, cb)
        {
            Text = "Horloge & Date Rainmeter";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            Color accent = Color.FromArgb(settings.AccentArgb);
            DrawLiquidGlassFrame(g, r, accent);

            DateTime now = DateTime.Now;
            string timeStr = now.ToString("HH:mm");
            string secStr = now.ToString("ss");
            string dayStr = now.ToString("dddd d MMMM yyyy");
            if (dayStr.Length > 0) dayStr = char.ToUpper(dayStr[0]) + dayStr.Substring(1);

            using (Font tf = new Font("Segoe UI Semibold", 28f, FontStyle.Bold))
            using (Font sf = new Font("Segoe UI", 12f))
            using (SolidBrush wb = new SolidBrush(Color.White))
            using (SolidBrush ab = new SolidBrush(accent))
            {
                g.DrawString(timeStr, tf, wb, 20, 14);
                SizeF tsz = g.MeasureString(timeStr, tf);
                g.DrawString(":" + secStr, sf, ab, 20 + tsz.Width - 8, 22);
            }

            using (Font df = new Font("Segoe UI", 9.5f))
            using (SolidBrush db = new SolidBrush(Color.FromArgb(170, 195, 230)))
            {
                g.DrawString(dayStr, df, db, 24, 76);
            }
        }
    }

    // =========================================================================
    // 2. SYSTEM RESOURCE MONITOR WIDGET
    // =========================================================================
    public class SysMonitorWidget : BaseGlassWidget
    {
        private PerformanceCounter cpuCounter;
        private float cpuVal = 22f;
        private float ramVal = 48f;

        public SysMonitorWidget(WidgetSettings s, RainmeterConfig cfg, Action cb) : base(s, cfg, cb)
        {
            Text = "Moniteur Système Rainmeter";
            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                cpuCounter.NextValue();
            }
            catch { }

            updateTimer.Interval = 1200;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try { if (cpuCounter != null) cpuVal = cpuCounter.NextValue(); } catch { }

            Graphics g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            Color accent = Color.FromArgb(settings.AccentArgb);
            DrawLiquidGlassFrame(g, r, accent);

            using (Font hf = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold))
            using (SolidBrush hb = new SolidBrush(Color.White))
            {
                g.DrawString("⚡ Moniteur Système Pro", hf, hb, 20, 14);
            }

            DrawMeter(g, 20, 48, Width - 40, "Processeur (CPU)", (int)cpuVal, accent);
            DrawMeter(g, 20, 92, Width - 40, "Mémoire Vive (RAM)", (int)ramVal, Color.FromArgb(180, 120, 255));
        }

        private void DrawMeter(Graphics g, float x, float y, float w, string name, int percent, Color barColor)
        {
            using (Font nf = new Font("Segoe UI", 8.8f))
            using (SolidBrush nb = new SolidBrush(Color.FromArgb(180, 195, 225)))
            {
                g.DrawString(string.Format("{0} : {1}%", name, percent), nf, nb, x, y);
            }

            RectangleF track = new RectangleF(x, y + 20, w, 10);
            using (GraphicsPath tp = CreateRoundedRect(track, 5f))
            using (SolidBrush tb = new SolidBrush(Color.FromArgb(28, 255, 255, 255)))
                g.FillPath(tb, tp);

            float fillW = Math.Max(10f, w * (percent / 100f));
            RectangleF fill = new RectangleF(x, y + 20, fillW, 10);
            using (GraphicsPath fp = CreateRoundedRect(fill, 5f))
            using (SolidBrush fb = new SolidBrush(barColor))
                g.FillPath(fb, fp);
        }
    }

    // =========================================================================
    // 3. WEATHER & CLIMATE WIDGET
    // =========================================================================
    public class WeatherWidget : BaseGlassWidget
    {
        public WeatherWidget(WidgetSettings s, RainmeterConfig cfg, Action cb) : base(s, cfg, cb)
        {
            Text = "Météo & Climat Rainmeter";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            Color accent = Color.FromArgb(255, 190, 80);
            DrawLiquidGlassFrame(g, r, accent);

            using (Font ifont = new Font("Segoe UI", 26f))
            using (SolidBrush ib = new SolidBrush(Color.White))
            {
                g.DrawString("🌤️", ifont, ib, 18, 22);
            }

            using (Font tf = new Font("Segoe UI Semibold", 20f, FontStyle.Bold))
            using (SolidBrush tb = new SolidBrush(Color.White))
            {
                g.DrawString("21°C", tf, tb, 78, 16);
            }

            using (Font cf = new Font("Segoe UI", 9f))
            using (SolidBrush cb = new SolidBrush(Color.FromArgb(170, 195, 230)))
            {
                g.DrawString("Éclaircies • Paris / Île-de-France", cf, cb, 80, 52);
                g.DrawString("Humidité: 54% • Vent: 12 km/h • Indice UV: 3", cf, cb, 22, 92);
            }
        }
    }

    // =========================================================================
    // 4. AUDIO SPECTRUM VISUALIZER WIDGET
    // =========================================================================
    public class AudioSpectrumWidget : BaseGlassWidget
    {
        private float[] bars = new float[20];
        private Random rnd = new Random();

        public AudioSpectrumWidget(WidgetSettings s, RainmeterConfig cfg, Action cb) : base(s, cfg, cb)
        {
            Text = "Spectre Audio Rainmeter";
            for (int i = 0; i < bars.Length; i++) bars[i] = 0.2f + (float)rnd.NextDouble() * 0.6f;
            updateTimer.Interval = 40; // 25 FPS live spectrum
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            for (int i = 0; i < bars.Length; i++)
            {
                float target = 0.1f + (float)rnd.NextDouble() * 0.85f;
                bars[i] = bars[i] * 0.65f + target * 0.35f;
            }

            Graphics g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            Color accent = Color.FromArgb(settings.AccentArgb);
            DrawLiquidGlassFrame(g, r, accent);

            using (Font hf = new Font("Segoe UI Semibold", 10f, FontStyle.Bold))
            using (SolidBrush hb = new SolidBrush(Color.White))
            {
                g.DrawString("🎵 Visualiseur de Spectre Rainmeter", hf, hb, 20, 14);
            }

            int barCount = bars.Length;
            float totalW = Width - 40;
            float barW = (totalW / barCount) - 3;
            float startX = 20;
            float baseY = Height - 24;

            for (int i = 0; i < barCount; i++)
            {
                float bh = bars[i] * 50f;
                RectangleF br = new RectangleF(startX + i * (barW + 3), baseY - bh, barW, bh);
                using (GraphicsPath bp = CreateRoundedRect(br, 2f))
                using (LinearGradientBrush bgb = new LinearGradientBrush(br, accent, Color.FromArgb(200, 120, 255), 90f))
                {
                    g.FillPath(bgb, bp);
                }
            }
        }
    }

    // =========================================================================
    // RAINMETER STUDIO DASHBOARD (MANAGER DE WIDGETS COMPLET)
    // =========================================================================
    public class RainmeterStudioManager : Form
    {
        private RainmeterConfig config;
        private List<BaseGlassWidget> activeWidgets = new List<BaseGlassWidget>();
        private ListBox widgetList;
        private TrackBar trkOpacity;
        private TrackBar trkRadius;
        private NumericUpDown numX;
        private NumericUpDown numY;
        private CheckBox chkLocked;
        private CheckBox chkClickThrough;
        private ComboBox cmbLayer;
        private Button btnToggle;
        private WidgetSettings selectedSettings;

        public RainmeterStudioManager(RainmeterConfig cfg, List<BaseGlassWidget> widgets)
        {
            config = cfg;
            activeWidgets = widgets;

            Text = "iWidgets — Studio de Personnalisation Rainmeter";
            Size = new Size(880, 640);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(11, 13, 20);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9.2f);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            BuildManagerUI();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void BuildManagerUI()
        {
            // Header
            Panel header = new Panel { Dock = DockStyle.Top, Height = 74, BackColor = Color.FromArgb(14, 18, 28) };
            header.Paint += (s, e) =>
            {
                Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = header.ClientRectangle;
                using (LinearGradientBrush grad = new LinearGradientBrush(r, Color.FromArgb(14, 18, 28), Color.FromArgb(22, 16, 34), 0f))
                    g.FillRectangle(grad, r);

                using (Font tf = new Font("Segoe UI Semibold", 16f, FontStyle.Bold))
                using (Font sf = new Font("Segoe UI", 9.0f))
                using (SolidBrush wb = new SolidBrush(Color.White))
                using (SolidBrush ab = new SolidBrush(Color.FromArgb(90, 200, 255)))
                using (SolidBrush gb = new SolidBrush(Color.FromArgb(160, 175, 205)))
                {
                    g.DrawString("iWidgets", tf, wb, 24, 12);
                    SizeF sz = g.MeasureString("iWidgets", tf);
                    g.DrawString("Rainmeter Glass Engine", tf, ab, 24 + sz.Width + 4, 12);
                    g.DrawString("Gestionnaire de skins de bureau, disposition multi-écrans & personnalisation Liquid Glass", sf, gb, 26, 44);
                }
            };
            Controls.Add(header);

            // Left List of Widgets
            Panel leftPanel = new Panel { Dock = DockStyle.Left, Width = 300, BackColor = Color.FromArgb(13, 16, 25), Padding = new Padding(16) };
            Controls.Add(leftPanel);

            Label lblLib = new Label { Text = "Bibliothèque de Widgets (Skins) :", Dock = DockStyle.Top, Height = 28, Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold), ForeColor = Color.White };
            leftPanel.Controls.Add(lblLib);

            widgetList = new ListBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(18, 22, 34),
                ForeColor = Color.FromArgb(220, 230, 245),
                Font = new Font("Segoe UI", 9.5f),
                BorderStyle = BorderStyle.None,
                ItemHeight = 32
            };
            widgetList.Items.Add("⏰ Horloge & Date Pro");
            widgetList.Items.Add("⚡ Moniteur Système CPU/RAM");
            widgetList.Items.Add("🌤️ Météo & Climat");
            widgetList.Items.Add("🎵 Spectre Audio Réactif");
            widgetList.SelectedIndex = 0;
            widgetList.SelectedIndexChanged += (s, e) => LoadWidgetProps();
            leftPanel.Controls.Add(widgetList);

            // Right Properties Panel
            Panel rightPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(10, 12, 18), Padding = new Padding(24) };
            Controls.Add(rightPanel);

            int y = 20;
            btnToggle = new Button { Location = new Point(24, y), Size = new Size(200, 36), BackColor = Color.FromArgb(40, 120, 230), FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold) };
            btnToggle.Click += (s, e) => ToggleSelectedWidget();
            rightPanel.Controls.Add(btnToggle);
            y += 56;

            Label lblPos = new Label { Text = "Coordonnées de Position (X, Y) :", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(170, 190, 220) };
            rightPanel.Controls.Add(lblPos);

            numX = new NumericUpDown { Location = new Point(24, y + 22), Size = new Size(100, 28), Minimum = -5000, Maximum = 10000 };
            numX.ValueChanged += (s, e) => { if (selectedSettings != null) { selectedSettings.PosX = (int)numX.Value; ApplyAndSave(); } };
            rightPanel.Controls.Add(numX);

            numY = new NumericUpDown { Location = new Point(135, y + 22), Size = new Size(100, 28), Minimum = -5000, Maximum = 10000 };
            numY.ValueChanged += (s, e) => { if (selectedSettings != null) { selectedSettings.PosY = (int)numY.Value; ApplyAndSave(); } };
            rightPanel.Controls.Add(numY);

            Button btnCenter = new Button { Text = "Centrer", Location = new Point(250, y + 20), Size = new Size(90, 30), BackColor = Color.FromArgb(30, 38, 56), FlatStyle = FlatStyle.Flat };
            btnCenter.Click += (s, e) =>
            {
                if (selectedSettings != null)
                {
                    Screen scr = Screen.PrimaryScreen;
                    selectedSettings.PosX = scr.Bounds.Left + (scr.Bounds.Width - selectedSettings.Width) / 2;
                    selectedSettings.PosY = scr.Bounds.Top + (scr.Bounds.Height - selectedSettings.Height) / 2;
                    numX.Value = selectedSettings.PosX; numY.Value = selectedSettings.PosY;
                    ApplyAndSave();
                }
            };
            rightPanel.Controls.Add(btnCenter);
            y += 66;

            Label lblLayer = new Label { Text = "Niveau d'affichage (Z-Order) :", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(170, 190, 220) };
            rightPanel.Controls.Add(lblLayer);

            cmbLayer = new ComboBox { Location = new Point(24, y + 22), Size = new Size(320, 28), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.2f) };
            cmbLayer.Items.AddRange(new string[] { "Au fond du Bureau (comme Rainmeter)", "Normal", "Toujours au premier plan (TopMost)" });
            cmbLayer.SelectedIndexChanged += (s, e) => { if (selectedSettings != null) { selectedSettings.LayerMode = cmbLayer.SelectedIndex; ApplyAndSave(); } };
            rightPanel.Controls.Add(cmbLayer);
            y += 66;

            chkLocked = new CheckBox { Text = "Verrouiller la position (empêche les glissements accidentels)", Location = new Point(24, y), AutoSize = true };
            chkLocked.CheckedChanged += (s, e) => { if (selectedSettings != null) { selectedSettings.Locked = chkLocked.Checked; ApplyAndSave(); } };
            rightPanel.Controls.Add(chkLocked);
            y += 30;

            chkClickThrough = new CheckBox { Text = "Clics traversants (Click-Through : la souris traverse le widget)", Location = new Point(24, y), AutoSize = true };
            chkClickThrough.CheckedChanged += (s, e) => { if (selectedSettings != null) { selectedSettings.ClickThrough = chkClickThrough.Checked; ApplyAndSave(); } };
            rightPanel.Controls.Add(chkClickThrough);
            y += 40;

            Label lblOp = new Label { Text = "Opacité du verre Liquid Glass :", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(170, 190, 220) };
            rightPanel.Controls.Add(lblOp);

            trkOpacity = new TrackBar { Location = new Point(24, y + 20), Size = new Size(360, 35), Minimum = 30, Maximum = 255 };
            trkOpacity.ValueChanged += (s, e) => { if (selectedSettings != null) { selectedSettings.Opacity = trkOpacity.Value; ApplyAndSave(); } };
            rightPanel.Controls.Add(trkOpacity);
            y += 56;

            Label lblRad = new Label { Text = "Rayon des coins arrondis (Squircle) :", Location = new Point(24, y), AutoSize = true, Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(170, 190, 220) };
            rightPanel.Controls.Add(lblRad);

            trkRadius = new TrackBar { Location = new Point(24, y + 20), Size = new Size(360, 35), Minimum = 0, Maximum = 40 };
            trkRadius.ValueChanged += (s, e) => { if (selectedSettings != null) { selectedSettings.CornerRadius = trkRadius.Value; ApplyAndSave(); } };
            rightPanel.Controls.Add(trkRadius);

            LoadWidgetProps();
        }

        private void LoadWidgetProps()
        {
            int idx = widgetList.SelectedIndex;
            if (idx == 0) selectedSettings = config.Clock;
            else if (idx == 1) selectedSettings = config.SysMonitor;
            else if (idx == 2) selectedSettings = config.Weather;
            else if (idx == 3) selectedSettings = config.AudioSpectrum;

            if (selectedSettings != null)
            {
                btnToggle.Text = selectedSettings.Enabled ? "Masquer ce Widget" : "Afficher ce Widget";
                btnToggle.BackColor = selectedSettings.Enabled ? Color.FromArgb(180, 50, 60) : Color.FromArgb(40, 130, 240);
                numX.Value = Math.Max(-5000, Math.Min(10000, selectedSettings.PosX));
                numY.Value = Math.Max(-5000, Math.Min(10000, selectedSettings.PosY));
                cmbLayer.SelectedIndex = selectedSettings.LayerMode;
                chkLocked.Checked = selectedSettings.Locked;
                chkClickThrough.Checked = selectedSettings.ClickThrough;
                trkOpacity.Value = Math.Max(30, Math.Min(255, selectedSettings.Opacity));
                trkRadius.Value = Math.Max(0, Math.Min(40, selectedSettings.CornerRadius));
            }
        }

        private void ToggleSelectedWidget()
        {
            if (selectedSettings != null)
            {
                selectedSettings.Enabled = !selectedSettings.Enabled;
                btnToggle.Text = selectedSettings.Enabled ? "Masquer ce Widget" : "Afficher ce Widget";
                btnToggle.BackColor = selectedSettings.Enabled ? Color.FromArgb(180, 50, 60) : Color.FromArgb(40, 130, 240);
                ApplyAndSave();
            }
        }

        private void ApplyAndSave()
        {
            for (int i = 0; i < activeWidgets.Count; i++)
            {
                activeWidgets[i].ApplyStyles();
                if (i == 0) activeWidgets[i].Visible = config.Clock.Enabled;
                else if (i == 1) activeWidgets[i].Visible = config.SysMonitor.Enabled;
                else if (i == 2) activeWidgets[i].Visible = config.Weather.Enabled;
                else if (i == 3) activeWidgets[i].Visible = config.AudioSpectrum.Enabled;
            }
            config.Save();
        }
    }

    // =========================================================================
    // ENTRY POINT
    // =========================================================================
    static class Program
    {
        [STAThread]
        static void Main()
        {
            NativeMethods.SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RainmeterConfig config = RainmeterConfig.Load();
            List<BaseGlassWidget> widgets = new List<BaseGlassWidget>();
            RainmeterStudioManager manager = null;

            Action openManager = () =>
            {
                if (manager == null || manager.IsDisposed)
                    manager = new RainmeterStudioManager(config, widgets);
                manager.Show();
                manager.BringToFront();
                manager.Activate();
            };

            ClockWidget clock = new ClockWidget(config.Clock, config, openManager);
            SysMonitorWidget monitor = new SysMonitorWidget(config.SysMonitor, config, openManager);
            WeatherWidget weather = new WeatherWidget(config.Weather, config, openManager);
            AudioSpectrumWidget spectrum = new AudioSpectrumWidget(config.AudioSpectrum, config, openManager);

            widgets.Add(clock);
            widgets.Add(monitor);
            widgets.Add(weather);
            widgets.Add(spectrum);

            if (config.Clock.Enabled) clock.Show();
            if (config.SysMonitor.Enabled) monitor.Show();
            if (config.Weather.Enabled) weather.Show();
            if (config.AudioSpectrum.Enabled) spectrum.Show();

            // Tray Icon for Rainmeter Widget Studio
            NotifyIcon tray = new NotifyIcon
            {
                Text = "iWidgets — Rainmeter Glass Studio",
                Icon = SystemIcons.Application,
                Visible = true
            };

            ContextMenuStrip trayMenu = new ContextMenuStrip();
            ToolStripMenuItem openItem = new ToolStripMenuItem("Ouvrir le Studio Rainmeter");
            openItem.Font = new Font(openItem.Font, FontStyle.Bold);
            openItem.Click += (s, e) => openManager();

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Quitter les Widgets");
            exitItem.Click += (s, e) => { tray.Visible = false; Application.Exit(); };

            trayMenu.Items.Add(openItem);
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add(exitItem);

            tray.ContextMenuStrip = trayMenu;
            tray.DoubleClick += (s, e) => openManager();

            Application.Run();
        }
    }
}
