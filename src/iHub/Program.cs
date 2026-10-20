using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace iHub
{
    public class SuiteAppInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Glyph { get; set; }
        public string Tagline { get; set; }
        public string Description { get; set; }
        public string ExeName { get; set; }
        public Color AccentColor { get; set; }
        public string GitHubAssetUrl { get; set; }

        public bool IsInstalled
        {
            get
            {
                string path = GetLocalExePath();
                return File.Exists(path);
            }
        }

        public Process GetRunningProcess()
        {
            string procName = Path.GetFileNameWithoutExtension(ExeName);
            Process[] procs = Process.GetProcessesByName(procName);
            return (procs != null && procs.Length > 0) ? procs[0] : null;
        }

        public string GetLocalExePath()
        {
            string appDir = Path.GetDirectoryName(Application.ExecutablePath);
            string direct = Path.Combine(appDir, ExeName);
            if (File.Exists(direct)) return direct;

            string inDist = Path.Combine(appDir, "dist", ExeName);
            if (File.Exists(inDist)) return inDist;

            string parentDist = Path.Combine(appDir, "..", "dist", ExeName);
            if (File.Exists(parentDist)) return Path.GetFullPath(parentDist);

            string parentRoot = Path.Combine(appDir, "..", ExeName);
            if (File.Exists(parentRoot)) return Path.GetFullPath(parentRoot);

            return direct;
        }

        public bool IsStartupEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false))
                {
                    if (key != null) return key.GetValue(Id) != null;
                }
            }
            catch { }
            return false;
        }

        public void SetStartup(bool enable)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (key != null)
                    {
                        string p = GetLocalExePath();
                        if (enable && File.Exists(p)) key.SetValue(Id, "\"" + p + "\" --tray");
                        else key.DeleteValue(Id, false);
                    }
                }
            }
            catch { }
        }
    }

    public static class NativeMethods
    {
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

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

    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }
    }

    public class GlassHubButton : Control
    {
        private bool isHovered = false;
        private bool isPressed = false;
        public Color BaseColor { get; set; }

        public GlassHubButton(string text, int x, int y, int w, int h, Color color)
        {
            Text = text;
            Location = new Point(x, y);
            Size = new Size(w, h);
            BaseColor = color;
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
            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);

            Color c = isPressed ? Interpolate(BaseColor, Color.Black, 0.25f)
                    : isHovered ? Interpolate(BaseColor, Color.White, 0.20f)
                    : BaseColor;

            using (GraphicsPath path = CreateRoundedRect(r, 10f))
            {
                using (SolidBrush b = new SolidBrush(Color.FromArgb(230, c))) g.FillPath(b, path);
                using (Pen p = new Pen(Color.FromArgb(120, Interpolate(Color.White, c, 0.35f)), 1.2f)) g.DrawPath(p, path);
            }

            TextRenderer.DrawText(g, Text, Font, ClientRectangle, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }

        private static Color Interpolate(Color a, Color b, float t)
        {
            return Color.FromArgb(
                (int)(a.A + (b.A - a.A) * t),
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));
        }

        private static GraphicsPath CreateRoundedRect(RectangleF rect, float radius)
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

    public class AppCardPanel : Panel
    {
        private SuiteAppInfo app;
        private Action onRefreshRequested;
        private GlassHubButton actionBtn;
        private GlassHubButton settingsBtn;
        private Label statusLabel;
        private CheckBox startupChk;

        public AppCardPanel(SuiteAppInfo appInfo, int x, int y, int w, int h, Action refreshCallback)
        {
            app = appInfo;
            onRefreshRequested = refreshCallback;
            Location = new Point(x, y);
            Size = new Size(w, h);
            DoubleBuffered = true;
            BackColor = Color.FromArgb(15, 18, 27);

            BuildCard();
        }

        private void BuildCard()
        {
            // Status label
            statusLabel = new Label
            {
                Location = new Point(Width - 180, 20),
                Size = new Size(160, 24),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold)
            };
            Controls.Add(statusLabel);

            // Action button (Launch / Stop / Download)
            actionBtn = new GlassHubButton("Lancer", Width - 145, 110, 125, 36, app.AccentColor);
            actionBtn.Click += (s, e) => HandleActionClick();
            Controls.Add(actionBtn);

            // Configure button
            settingsBtn = new GlassHubButton("Paramètres", Width - 260, 110, 105, 36, Color.FromArgb(32, 38, 54));
            settingsBtn.Click += (s, e) => LaunchSettings();
            Controls.Add(settingsBtn);

            // Startup checkbox
            startupChk = new CheckBox
            {
                Text = "Démarrer avec Windows",
                Location = new Point(88, 116),
                AutoSize = true,
                ForeColor = Color.FromArgb(180, 195, 220),
                Font = new Font("Segoe UI", 8.8f),
                Checked = app.IsStartupEnabled()
            };
            startupChk.CheckedChanged += (s, e) => app.SetStartup(startupChk.Checked);
            Controls.Add(startupChk);

            UpdateState();
        }

        public void UpdateState()
        {
            Process proc = app.GetRunningProcess();
            bool running = (proc != null);
            bool installed = app.IsInstalled;

            if (running)
            {
                long memMb = proc.WorkingSet64 / (1024 * 1024);
                statusLabel.Text = string.Format("🟢 En cours ({0} Mo)", memMb);
                statusLabel.ForeColor = Color.FromArgb(70, 245, 150);
                actionBtn.Text = "Arrêter";
                actionBtn.BaseColor = Color.FromArgb(185, 45, 60);
                settingsBtn.Visible = true;
            }
            else if (installed)
            {
                statusLabel.Text = "⚪ Installé (Arrêté)";
                statusLabel.ForeColor = Color.FromArgb(180, 195, 220);
                actionBtn.Text = "Lancer";
                actionBtn.BaseColor = app.AccentColor;
                settingsBtn.Visible = true;
            }
            else
            {
                statusLabel.Text = "🟡 Non installé";
                statusLabel.ForeColor = Color.FromArgb(245, 195, 70);
                actionBtn.Text = "Installer";
                actionBtn.BaseColor = Color.FromArgb(40, 120, 225);
                settingsBtn.Visible = false;
            }
            actionBtn.Invalidate();
        }

        private void HandleActionClick()
        {
            Process proc = app.GetRunningProcess();
            if (proc != null)
            {
                try { proc.Kill(); } catch { }
                UpdateState();
                if (onRefreshRequested != null) onRefreshRequested();
                return;
            }

            if (!app.IsInstalled)
            {
                DownloadOrBuildApp();
                return;
            }

            string path = app.GetLocalExePath();
            try
            {
                Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du lancement : " + ex.Message, "iHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Thread.Sleep(300);
            UpdateState();
            if (onRefreshRequested != null) onRefreshRequested();
        }

        private void LaunchSettings()
        {
            string path = app.GetLocalExePath();
            if (File.Exists(path))
            {
                try { Process.Start(new ProcessStartInfo { FileName = path, Arguments = "", UseShellExecute = true }); } catch { }
            }
        }

        private void DownloadOrBuildApp()
        {
            statusLabel.Text = "Téléchargement...";
            statusLabel.ForeColor = Color.FromArgb(100, 200, 255);
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    // Fallback to building or fetching from repo releases
                    string appDir = Path.GetDirectoryName(Application.ExecutablePath);
                    string target = Path.Combine(appDir, app.ExeName);

                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("User-Agent", "iHub-Launcher");
                        string releaseUrl = "https://github.com/flobure2008-ui/iSuite/releases/download/v3.1.0/" + app.ExeName;
                        wc.DownloadFile(releaseUrl, target);
                    }
                }
                catch
                {
                    // If release download failed, check if source exists locally and compile on the fly!
                    try
                    {
                        string appDir = Path.GetDirectoryName(Application.ExecutablePath);
                        string src = Path.Combine(appDir, "src", app.Id, "Program.cs");
                        if (!File.Exists(src)) src = Path.Combine(appDir, "..", "src", app.Id, "Program.cs");
                        if (File.Exists(src))
                        {
                            string outExe = Path.Combine(appDir, app.ExeName);
                            Process csc = Process.Start(new ProcessStartInfo
                            {
                                FileName = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe",
                                Arguments = string.Format("/nologo /target:winexe /out:\"{0}\" /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll \"{1}\"", outExe, src),
                                CreateNoWindow = true,
                                UseShellExecute = false
                            });
                            csc.WaitForExit();
                        }
                    }
                    catch { }
                }

                if (IsHandleCreated && !IsDisposed)
                {
                    BeginInvoke((Action)(() =>
                    {
                        UpdateState();
                        if (onRefreshRequested != null) onRefreshRequested();
                    }));
                }
            });
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            using (GraphicsPath path = CreateRoundedRect(r, 16f))
            {
                using (SolidBrush b = new SolidBrush(BackColor)) g.FillPath(b, path);
                using (Pen p = new Pen(Color.FromArgb(40, 255, 255, 255), 1.2f)) g.DrawPath(p, path);
            }

            // Glyph Badge
            RectangleF badge = new RectangleF(20, 20, 52, 52);
            using (GraphicsPath bp = CreateRoundedRect(badge, 14f))
            {
                using (SolidBrush bb = new SolidBrush(Color.FromArgb(40, app.AccentColor))) g.FillPath(bb, bp);
                using (Pen bpen = new Pen(Color.FromArgb(120, app.AccentColor), 1.4f)) g.DrawPath(bpen, bp);
            }

            using (Font gf = new Font("Segoe UI Semibold", 16f, FontStyle.Bold))
            using (SolidBrush gfb = new SolidBrush(app.AccentColor))
            {
                SizeF gsz = g.MeasureString(app.Glyph, gf);
                g.DrawString(app.Glyph, gf, gfb, badge.X + (badge.Width - gsz.Width) / 2f, badge.Y + (badge.Height - gsz.Height) / 2f);
            }

            // Titles
            using (Font tf = new Font("Segoe UI Semibold", 12.5f, FontStyle.Bold))
            using (Font tgf = new Font("Segoe UI", 9.0f))
            using (Font df = new Font("Segoe UI", 9.0f))
            using (SolidBrush wb = new SolidBrush(Color.White))
            using (SolidBrush ab = new SolidBrush(Color.FromArgb(160, 180, 215)))
            using (SolidBrush gray = new SolidBrush(Color.FromArgb(130, 145, 175)))
            {
                g.DrawString(app.Name, tf, wb, 86, 20);
                g.DrawString(app.Tagline, tgf, ab, 88, 46);
                g.DrawString(app.Description, df, gray, 88, 70);
            }
        }

        private static GraphicsPath CreateRoundedRect(RectangleF rect, float radius)
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

    public class HubMainWindow : Form
    {
        private List<SuiteAppInfo> apps = new List<SuiteAppInfo>();
        private List<AppCardPanel> cards = new List<AppCardPanel>();
        private System.Windows.Forms.Timer pollTimer;
        private NotifyIcon trayIcon;

        public HubMainWindow()
        {
            Text = "iHub — Launcher & Store de la Suite iSuite";
            Size = new Size(960, 780);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(10, 12, 18);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9.2f);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            InitializeAppDefinitions();
            BuildUI();
            SetupTray();

            pollTimer = new System.Windows.Forms.Timer { Interval = 1600 };
            pollTimer.Tick += (s, e) => RefreshAllCards();
            pollTimer.Start();
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
            try
            {
                int dark = 1;
                NativeMethods.DwmSetWindowAttribute(Handle, 20, ref dark, 4);
                int round = 2;
                NativeMethods.DwmSetWindowAttribute(Handle, 33, ref round, 4);
            }
            catch { }
        }

        private void InitializeAppDefinitions()
        {
            apps.Add(new SuiteAppInfo
            {
                Id = "iCorner",
                Name = "iCorner",
                Glyph = "✦",
                Tagline = "Coins d'Écran Organiques & Liseré Spéculaire",
                Description = "Sculpte des angles superellipse (n=4.2) ou circulaires sur tous vos moniteurs. Ultra-léger (0% CPU).",
                ExeName = "iCorner.exe",
                AccentColor = Color.FromArgb(70, 195, 255)
            });

            apps.Add(new SuiteAppInfo
            {
                Id = "iBridge",
                Name = "iBridge",
                Glyph = "🌊",
                Tagline = "Passerelle Liquide Inter-Écrans",
                Description = "Crée un pont fluide de verre dépoli et un halo magnétique lorsque le curseur traverse vos écrans.",
                ExeName = "iBridge.exe",
                AccentColor = Color.FromArgb(155, 120, 255)
            });

            apps.Add(new SuiteAppInfo
            {
                Id = "iIsland",
                Name = "iIsland",
                Glyph = "💧",
                Tagline = "Dynamic Island Flottante en Verre Liquide",
                Description = "Capsule supérieure avec lecteur audio universel, visualiseur de spectre, horloge et minuteur Focus.",
                ExeName = "iIsland.exe",
                AccentColor = Color.FromArgb(50, 225, 175)
            });

            apps.Add(new SuiteAppInfo
            {
                Id = "iMascot",
                Name = "iMascot",
                Glyph = "✨",
                Tagline = "Compagnons Célestes Interactifs en Cristal",
                Description = "Mascottes flottantes sur le bureau (Lumina, Kitsune, Draco) qui suivent les yeux et dansent au son.",
                ExeName = "iMascot.exe",
                AccentColor = Color.FromArgb(255, 150, 185)
            });
        }

        private void BuildUI()
        {
            // Header
            DoubleBufferedPanel header = new DoubleBufferedPanel { Dock = DockStyle.Top, Height = 95, BackColor = Color.FromArgb(14, 18, 28) };
            header.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = header.ClientRectangle;
                using (LinearGradientBrush grad = new LinearGradientBrush(r, Color.FromArgb(14, 18, 28), Color.FromArgb(24, 16, 34), 0f))
                    g.FillRectangle(grad, r);

                using (Font tf = new Font("Segoe UI Semibold", 18f, FontStyle.Bold))
                using (Font hf = new Font("Segoe UI", 9.2f))
                using (SolidBrush wb = new SolidBrush(Color.White))
                using (SolidBrush ab = new SolidBrush(Color.FromArgb(100, 200, 255)))
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(170, 185, 215)))
                {
                    g.DrawString("iSuite", tf, wb, 24, 16);
                    SizeF sz = g.MeasureString("iSuite", tf);
                    g.DrawString("Hub", tf, ab, 24 + sz.Width - 4, 16);
                    g.DrawString("Studio & Gestionnaire d'Applications Modulaires en Glassmorphisme", hf, sb, 26, 52);
                }
            };

            // Global Actions in Header
            GlassHubButton launchAllBtn = new GlassHubButton("Tout Lancer", 670, 28, 120, 38, Color.FromArgb(40, 140, 250));
            launchAllBtn.Click += (s, e) => LaunchAll();
            header.Controls.Add(launchAllBtn);

            GlassHubButton stopAllBtn = new GlassHubButton("Tout Arrêter", 802, 28, 120, 38, Color.FromArgb(180, 45, 60));
            stopAllBtn.Click += (s, e) => StopAll();
            header.Controls.Add(stopAllBtn);

            Controls.Add(header);

            // Container for app cards
            Panel container = new Panel
            {
                Location = new Point(24, 115),
                Size = new Size(895, 600),
                AutoScroll = true,
                BackColor = Color.FromArgb(10, 12, 18)
            };
            Controls.Add(container);

            int cardY = 0;
            int cardH = 160;
            for (int i = 0; i < apps.Count; i++)
            {
                AppCardPanel card = new AppCardPanel(apps[i], 0, cardY, 875, cardH, RefreshAllCards);
                cards.Add(card);
                container.Controls.Add(card);
                cardY += cardH + 16;
            }
        }

        private void RefreshAllCards()
        {
            if (IsDisposed || !IsHandleCreated) return;
            foreach (var card in cards) card.UpdateState();
        }

        private void LaunchAll()
        {
            foreach (var a in apps)
            {
                if (a.GetRunningProcess() == null && a.IsInstalled)
                {
                    try { Process.Start(new ProcessStartInfo { FileName = a.GetLocalExePath(), UseShellExecute = true }); } catch { }
                }
            }
            Thread.Sleep(300);
            RefreshAllCards();
        }

        private void StopAll()
        {
            foreach (var a in apps)
            {
                Process p = a.GetRunningProcess();
                if (p != null) { try { p.Kill(); } catch { } }
            }
            Thread.Sleep(300);
            RefreshAllCards();
        }

        private void SetupTray()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "iSuite Hub — Launcher Store";
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Visible = true;

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem openItem = new ToolStripMenuItem("Ouvrir iHub Store");
            openItem.Font = new Font(openItem.Font, FontStyle.Bold);
            openItem.Click += (s, e) => { Show(); BringToFront(); Activate(); };

            ToolStripMenuItem startAll = new ToolStripMenuItem("Lancer toutes les applications");
            startAll.Click += (s, e) => LaunchAll();

            ToolStripMenuItem stopAll = new ToolStripMenuItem("Arrêter toutes les applications");
            stopAll.Click += (s, e) => StopAll();

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Quitter iHub");
            exitItem.Click += (s, e) => { trayIcon.Visible = false; Application.Exit(); };

            menu.Items.Add(openItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(startAll);
            menu.Items.Add(stopAll);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(exitItem);

            trayIcon.ContextMenuStrip = menu;
            trayIcon.DoubleClick += (s, e) => { Show(); BringToFront(); Activate(); };
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
        static void Main()
        {
            NativeMethods.EnablePerMonitorDpiAwareness();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HubMainWindow());
        }
    }
}
