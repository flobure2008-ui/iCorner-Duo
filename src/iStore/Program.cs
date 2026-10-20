using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace iStore
{
    public class StoreItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Developer { get; set; }
        public string Version { get; set; }
        public string Rating { get; set; }
        public string Downloads { get; set; }
        public string Summary { get; set; }
        public string ExeName { get; set; }
        public string Glyph { get; set; }
        public Color AccentColor { get; set; }

        public bool IsInstalled
        {
            get
            {
                string p = GetLocalPath();
                return File.Exists(p);
            }
        }

        public string GetLocalPath()
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

    public class StoreMainWindow : Form
    {
        private List<StoreItem> items = new List<StoreItem>();
        private List<Panel> cardPanels = new List<Panel>();
        private Panel catalogContainer;
        private string activeCategory = "Tous";

        public StoreMainWindow()
        {
            Text = "iStore — Le Store d'Applications Modulaires";
            Size = new Size(1100, 780);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(11, 13, 20);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9.2f);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            InitializeStoreItems();
            BuildUI();
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

        private void InitializeStoreItems()
        {
            items.Add(new StoreItem
            {
                Id = "iCorner",
                Title = "iCorner Pure",
                Category = "Multi-Moniteurs",
                Developer = "iSuite Lab",
                Version = "v3.0.0",
                Rating = "★ 4.9 (1.4k)",
                Downloads = "42k téléchargements",
                Summary = "Arrondis d'écran organiques superellipse (n=4.2), multi-moniteurs, liseré biseauté spéculaire & 0% CPU.",
                ExeName = "iCorner.exe",
                Glyph = "✦",
                AccentColor = Color.FromArgb(70, 195, 255)
            });

            items.Add(new StoreItem
            {
                Id = "iBridge",
                Title = "iBridge Duo",
                Category = "Multi-Moniteurs",
                Developer = "iSuite Lab",
                Version = "v3.0.0",
                Rating = "★ 4.9 (890)",
                Downloads = "28k téléchargements",
                Summary = "Passerelle fluide de verre dépoli & halo magnétique d'approche lors du passage du curseur entre écrans.",
                ExeName = "iBridge.exe",
                Glyph = "🌊",
                AccentColor = Color.FromArgb(155, 120, 255)
            });

            items.Add(new StoreItem
            {
                Id = "iIsland",
                Title = "iIsland Dynamic",
                Category = "Widgets Flottants",
                Developer = "iSuite Lab",
                Version = "v3.0.0",
                Rating = "★ 4.8 (2.1k)",
                Downloads = "65k téléchargements",
                Summary = "Dynamic Island liquide au sommet de l'écran avec lecteur média universel, spectre audio & minuteur Focus.",
                ExeName = "iIsland.exe",
                Glyph = "💧",
                AccentColor = Color.FromArgb(50, 225, 175)
            });

            items.Add(new StoreItem
            {
                Id = "iMascot",
                Title = "iMascot Celestial",
                Category = "Widgets Flottants",
                Developer = "iSuite Lab",
                Version = "v3.0.0",
                Rating = "★ 5.0 (3.4k)",
                Downloads = "51k téléchargements",
                Summary = "Compagnons célestes interactifs sculptés en cristal et verre soufflé (Lumina, Kitsune, Draco) qui suivent les yeux.",
                ExeName = "iMascot.exe",
                Glyph = "✨",
                AccentColor = Color.FromArgb(255, 150, 185)
            });

            items.Add(new StoreItem
            {
                Id = "iSuite",
                Title = "iSuite Master Shell",
                Category = "Shell & Bureau",
                Developer = "iSuite Lab",
                Version = "v3.0.0",
                Rating = "★ 5.0 (4.2k)",
                Downloads = "88k téléchargements",
                Summary = "Barre des tâches flottante façon Windows 12, Menu Démarrer acrylique, Centre de Contrôle latéral et studio unifié.",
                ExeName = "iSuite.exe",
                Glyph = "❖",
                AccentColor = Color.FromArgb(80, 150, 255)
            });
        }

        private void BuildUI()
        {
            // Header Bar
            Panel header = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Color.FromArgb(14, 17, 26) };
            header.Paint += (s, e) =>
            {
                Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = header.ClientRectangle;
                using (LinearGradientBrush grad = new LinearGradientBrush(r, Color.FromArgb(14, 17, 26), Color.FromArgb(22, 16, 32), 0f))
                    g.FillRectangle(grad, r);

                using (Font tf = new Font("Segoe UI Semibold", 18f, FontStyle.Bold))
                using (Font sf = new Font("Segoe UI", 9.0f))
                using (SolidBrush wb = new SolidBrush(Color.White))
                using (SolidBrush ab = new SolidBrush(Color.FromArgb(100, 195, 255)))
                using (SolidBrush gray = new SolidBrush(Color.FromArgb(160, 175, 205)))
                {
                    g.DrawString("iStore", tf, wb, 24, 14);
                    SizeF sz = g.MeasureString("iStore", tf);
                    g.DrawString("Marketplace", tf, ab, 24 + sz.Width - 4, 14);
                    g.DrawString("Découvrez, installez et mettez à jour les composants de la suite modulaire", sf, gray, 26, 48);
                }
            };
            Controls.Add(header);

            // Filter Category Tabs
            Panel catBar = new Panel { Location = new Point(0, 80), Size = new Size(Width, 48), BackColor = Color.FromArgb(12, 14, 22) };
            string[] cats = new string[] { "Tous", "Shell & Bureau", "Multi-Moniteurs", "Widgets Flottants" };
            int catX = 24;
            for (int i = 0; i < cats.Length; i++)
            {
                string catName = cats[i];
                Button b = new Button
                {
                    Text = catName,
                    Location = new Point(catX, 8),
                    Size = new Size(130, 32),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = (catName == activeCategory) ? Color.FromArgb(35, 55, 95) : Color.FromArgb(18, 22, 34),
                    ForeColor = (catName == activeCategory) ? Color.White : Color.FromArgb(170, 185, 215),
                    Font = new Font("Segoe UI Semibold", 9.0f, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                b.FlatAppearance.BorderSize = 0;
                b.Click += (s, e) => { activeCategory = catName; RefreshCatalog(); };
                catBar.Controls.Add(b);
                catX += 138;
            }
            Controls.Add(catBar);

            // Catalog Container
            catalogContainer = new Panel
            {
                Location = new Point(24, 140),
                Size = new Size(1035, 580),
                AutoScroll = true,
                BackColor = Color.FromArgb(11, 13, 20)
            };
            Controls.Add(catalogContainer);

            RefreshCatalog();
        }

        private void RefreshCatalog()
        {
            catalogContainer.SuspendLayout();
            catalogContainer.Controls.Clear();

            int cardW = 495;
            int cardH = 175;
            int gap = 18;

            int col = 0;
            int row = 0;

            for (int i = 0; i < items.Count; i++)
            {
                StoreItem item = items[i];
                if (activeCategory != "Tous" && item.Category != activeCategory) continue;

                int x = col * (cardW + gap);
                int y = row * (cardH + gap);

                Panel card = CreateStoreCard(item, x, y, cardW, cardH);
                catalogContainer.Controls.Add(card);

                col++;
                if (col >= 2) { col = 0; row++; }
            }

            catalogContainer.ResumeLayout(true);
        }

        private Panel CreateStoreCard(StoreItem item, int x, int y, int w, int h)
        {
            Panel p = new Panel { Location = new Point(x, y), Size = new Size(w, h), BackColor = Color.FromArgb(16, 20, 30) };
            p.Paint += (s, e) =>
            {
                Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = p.ClientRectangle;
                r.Width -= 1; r.Height -= 1;
                using (GraphicsPath path = CreateRoundedRect(r, 16f))
                {
                    using (SolidBrush b = new SolidBrush(Color.FromArgb(16, 20, 30))) g.FillPath(b, path);
                    using (Pen pen = new Pen(Color.FromArgb(35, 255, 255, 255), 1.2f)) g.DrawPath(pen, path);
                }

                // Badge icon
                Rectangle badge = new Rectangle(18, 18, 52, 52);
                using (GraphicsPath bp = CreateRoundedRect(badge, 14f))
                {
                    using (SolidBrush bb = new SolidBrush(Color.FromArgb(40, item.AccentColor))) g.FillPath(bb, bp);
                    using (Pen bpen = new Pen(Color.FromArgb(120, item.AccentColor), 1.4f)) g.DrawPath(bpen, bp);
                }

                using (Font gf = new Font("Segoe UI Semibold", 16f, FontStyle.Bold))
                using (SolidBrush gfb = new SolidBrush(item.AccentColor))
                {
                    SizeF gsz = g.MeasureString(item.Glyph, gf);
                    g.DrawString(item.Glyph, gf, gfb, badge.X + (badge.Width - gsz.Width) / 2f, badge.Y + (badge.Height - gsz.Height) / 2f);
                }

                // Title & meta
                using (Font tf = new Font("Segoe UI Semibold", 11.5f, FontStyle.Bold))
                using (Font mf = new Font("Segoe UI", 8.5f))
                using (Font sf = new Font("Segoe UI", 8.8f))
                using (SolidBrush wb = new SolidBrush(Color.White))
                using (SolidBrush ab = new SolidBrush(Color.FromArgb(160, 180, 215)))
                using (SolidBrush gray = new SolidBrush(Color.FromArgb(140, 155, 180)))
                {
                    g.DrawString(item.Title, tf, wb, 82, 18);
                    g.DrawString(item.Rating + " • " + item.Downloads, mf, ab, 84, 42);
                    g.DrawString(item.Summary, sf, gray, new RectangleF(18, 80, w - 36, 44));
                }
            };

            // Install or Launch Button
            Button actionBtn = new Button
            {
                Text = item.IsInstalled ? "Ouvrir" : "Installer",
                Location = new Point(w - 120, 18),
                Size = new Size(102, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = item.IsInstalled ? Color.FromArgb(32, 45, 68) : Color.FromArgb(40, 125, 235),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9.0f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            actionBtn.FlatAppearance.BorderSize = 0;
            actionBtn.Click += (s, e) =>
            {
                if (item.IsInstalled)
                {
                    try { Process.Start(new ProcessStartInfo { FileName = item.GetLocalPath(), UseShellExecute = true }); } catch { }
                }
                else
                {
                    actionBtn.Text = "Installation...";
                    actionBtn.Enabled = false;
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        DownloadApp(item);
                        if (!IsDisposed && IsHandleCreated)
                        {
                            BeginInvoke((Action)(() =>
                            {
                                actionBtn.Text = "Ouvrir";
                                actionBtn.BackColor = Color.FromArgb(32, 45, 68);
                                actionBtn.Enabled = true;
                            }));
                        }
                    });
                }
            };
            p.Controls.Add(actionBtn);

            return p;
        }

        private void DownloadApp(StoreItem item)
        {
            try
            {
                string appDir = Path.GetDirectoryName(Application.ExecutablePath);
                string target = Path.Combine(appDir, item.ExeName);

                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", "iStore-Client");
                    string url = "https://github.com/flobure2008-ui/iSuite/releases/download/v3.1.0/" + item.ExeName;
                    wc.DownloadFile(url, target);
                }
            }
            catch
            {
                // Fallback: compile on the fly if source exists
                try
                {
                    string appDir = Path.GetDirectoryName(Application.ExecutablePath);
                    string src = Path.Combine(appDir, "src", item.Id, "Program.cs");
                    if (!File.Exists(src)) src = Path.Combine(appDir, "..", "src", item.Id, "Program.cs");
                    if (File.Exists(src))
                    {
                        string outExe = Path.Combine(appDir, item.ExeName);
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
        }

        private static GraphicsPath CreateRoundedRect(Rectangle rect, float radius)
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
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            NativeMethods.EnablePerMonitorDpiAwareness();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StoreMainWindow());
        }
    }
}
