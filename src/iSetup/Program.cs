using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace iSetup
{
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

    public class SetupWizardForm : Form
    {
        private int currentStep = 0;
        private string installDir;
        private CheckBox chkDesktop;
        private CheckBox chkStartMenu;
        private CheckBox chkAutoStart;
        private ProgressBar progressBar;
        private Label statusLabel;
        private Button nextBtn;
        private Button cancelBtn;

        public SetupWizardForm()
        {
            Text = "Installation de iSuite — Studio & Shell Glass";
            Size = new Size(680, 520);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(12, 14, 22);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9.2f);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            string localApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            installDir = Path.Combine(localApp, "Programs", "iSuite");

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

        private Panel mainContent;

        private void BuildUI()
        {
            // Header Banner
            Panel header = new Panel { Dock = DockStyle.Top, Height = 74, BackColor = Color.FromArgb(16, 20, 32) };
            header.Paint += (s, e) =>
            {
                Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = header.ClientRectangle;
                using (LinearGradientBrush grad = new LinearGradientBrush(r, Color.FromArgb(16, 20, 32), Color.FromArgb(24, 18, 38), 0f))
                    g.FillRectangle(grad, r);

                using (Font tf = new Font("Segoe UI Semibold", 15f, FontStyle.Bold))
                using (Font sf = new Font("Segoe UI", 8.8f))
                using (SolidBrush wb = new SolidBrush(Color.White))
                using (SolidBrush ab = new SolidBrush(Color.FromArgb(90, 190, 255)))
                using (SolidBrush gb = new SolidBrush(Color.FromArgb(160, 175, 205)))
                {
                    g.DrawString("iSuite", tf, wb, 24, 12);
                    SizeF sz = g.MeasureString("iSuite", tf);
                    g.DrawString("Assistant d'Installation", tf, ab, 24 + sz.Width - 4, 12);
                    g.DrawString("Installez le Shell Windows 12 Glass, le Master Studio et le Store sur votre PC", sf, gb, 26, 42);
                }
            };
            Controls.Add(header);

            // Bottom Navigation Bar
            Panel footer = new Panel { Dock = DockStyle.Bottom, Height = 64, BackColor = Color.FromArgb(10, 12, 18) };
            footer.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Color.FromArgb(30, 255, 255, 255), 1f))
                    e.Graphics.DrawLine(p, 0, 0, footer.Width, 0);
            };

            nextBtn = new Button
            {
                Text = "Suivant >",
                Location = new Point(footer.Width - 140, 14),
                Size = new Size(110, 36),
                BackColor = Color.FromArgb(40, 130, 245),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            nextBtn.FlatAppearance.BorderSize = 0;
            nextBtn.Click += (s, e) => HandleNext();
            footer.Controls.Add(nextBtn);

            cancelBtn = new Button
            {
                Text = "Annuler",
                Location = new Point(footer.Width - 260, 14),
                Size = new Size(100, 36),
                BackColor = Color.FromArgb(24, 28, 42),
                ForeColor = Color.FromArgb(200, 215, 240),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f),
                Cursor = Cursors.Hand
            };
            cancelBtn.FlatAppearance.BorderSize = 0;
            cancelBtn.Click += (s, e) => Close();
            footer.Controls.Add(cancelBtn);

            Controls.Add(footer);

            // Main Content Area
            mainContent = new Panel { Location = new Point(0, 74), Size = new Size(Width, Height - 138), BackColor = Color.FromArgb(12, 14, 22) };
            Controls.Add(mainContent);

            ShowStep(0);
        }

        private void ShowStep(int step)
        {
            currentStep = step;
            mainContent.SuspendLayout();
            mainContent.Controls.Clear();

            if (step == 0) // Welcome & Destination
            {
                Label wLbl = new Label
                {
                    Text = "Bienvenue dans l'assistant d'installation d'iSuite !",
                    Location = new Point(28, 20),
                    AutoSize = true,
                    Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold)
                };
                mainContent.Controls.Add(wLbl);

                Label desc = new Label
                {
                    Text = "Cet assistant va déployer et configurer iSuite sur votre ordinateur :\n\n" +
                           "• iSuite Master Shell (Barre des tâches flottante, Menu Démarrer, Centre d'action)\n" +
                           "• Composants modulaires (Coins d'écran, Passerelle inter-écrans, Dynamic Island, Mascottes)\n" +
                           "• iStore (Marketplace pour ajouter et mettre à jour des extensions)",
                    Location = new Point(30, 60),
                    Size = new Size(600, 100),
                    ForeColor = Color.FromArgb(180, 195, 220)
                };
                mainContent.Controls.Add(desc);

                Label dirLbl = new Label { Text = "Dossier d'installation recommandé :", Location = new Point(30, 185), AutoSize = true };
                mainContent.Controls.Add(dirLbl);

                TextBox dirBox = new TextBox
                {
                    Text = installDir,
                    Location = new Point(30, 215),
                    Size = new Size(500, 28),
                    BackColor = Color.FromArgb(20, 24, 36),
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    ReadOnly = true
                };
                mainContent.Controls.Add(dirBox);

                nextBtn.Text = "Suivant >";
            }
            else if (step == 1) // Options
            {
                Label optLbl = new Label
                {
                    Text = "Options et raccourcis d'installation :",
                    Location = new Point(28, 20),
                    AutoSize = true,
                    Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold)
                };
                mainContent.Controls.Add(optLbl);

                chkDesktop = new CheckBox
                {
                    Text = "Créer un raccourci sur le Bureau pour iSuite",
                    Location = new Point(32, 70),
                    AutoSize = true,
                    Checked = true
                };
                mainContent.Controls.Add(chkDesktop);

                chkStartMenu = new CheckBox
                {
                    Text = "Ajouter iSuite et iStore au Menu Démarrer de Windows",
                    Location = new Point(32, 110),
                    AutoSize = true,
                    Checked = true
                };
                mainContent.Controls.Add(chkStartMenu);

                chkAutoStart = new CheckBox
                {
                    Text = "Lancer automatiquement iSuite au démarrage de votre session Windows",
                    Location = new Point(32, 150),
                    AutoSize = true,
                    Checked = true
                };
                mainContent.Controls.Add(chkAutoStart);

                nextBtn.Text = "Installer";
            }
            else if (step == 2) // Installing
            {
                Label instLbl = new Label
                {
                    Text = "Installation en cours...",
                    Location = new Point(28, 40),
                    AutoSize = true,
                    Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold)
                };
                mainContent.Controls.Add(instLbl);

                progressBar = new ProgressBar
                {
                    Location = new Point(30, 85),
                    Size = new Size(600, 26),
                    Minimum = 0,
                    Maximum = 100,
                    Value = 10
                };
                mainContent.Controls.Add(progressBar);

                statusLabel = new Label
                {
                    Text = "Préparation des fichiers...",
                    Location = new Point(30, 125),
                    Size = new Size(600, 24),
                    ForeColor = Color.FromArgb(170, 190, 220)
                };
                mainContent.Controls.Add(statusLabel);

                nextBtn.Enabled = false;
                cancelBtn.Enabled = false;

                RunInstallation();
            }
            else if (step == 3) // Finished
            {
                Label finLbl = new Label
                {
                    Text = "✓ Installation terminée avec succès !",
                    Location = new Point(28, 30),
                    AutoSize = true,
                    Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(75, 235, 145)
                };
                mainContent.Controls.Add(finLbl);

                Label finDesc = new Label
                {
                    Text = "iSuite et iStore sont désormais prêts à être utilisés sur votre PC.\n" +
                           "Vous pouvez lancer le Master Studio dès maintenant pour personnaliser votre bureau.",
                    Location = new Point(30, 80),
                    Size = new Size(600, 60),
                    ForeColor = Color.FromArgb(190, 205, 230)
                };
                mainContent.Controls.Add(finDesc);

                nextBtn.Text = "Lancer iSuite";
                nextBtn.Enabled = true;
                cancelBtn.Text = "Quitter";
                cancelBtn.Enabled = true;
            }

            mainContent.ResumeLayout(true);
        }

        private void HandleNext()
        {
            if (currentStep == 0) ShowStep(1);
            else if (currentStep == 1) ShowStep(2);
            else if (currentStep == 3)
            {
                // Launch iSuite
                string exe = Path.Combine(installDir, "iSuite.exe");
                if (File.Exists(exe))
                {
                    try { Process.Start(new ProcessStartInfo { FileName = exe, UseShellExecute = true }); } catch { }
                }
                Application.Exit();
            }
        }

        private void RunInstallation()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    if (!Directory.Exists(installDir)) Directory.CreateDirectory(installDir);

                    SetProgress(25, "Copie des fichiers exécutables...");
                    Thread.Sleep(300);

                    string appDir = Path.GetDirectoryName(Application.ExecutablePath);

                    string[] binaries = new string[] {
                        "iSuite.exe", "iStore.exe", "iMaker.exe", "iWidgets.exe", "iVolume.exe", "iAltTab.exe", "iCorner.exe", "iBridge.exe", "iIsland.exe", "iMascot.exe", "iHub.exe"
                    };

                    foreach (var bin in binaries)
                    {
                        string src = Path.Combine(appDir, bin);
                        if (!File.Exists(src)) src = Path.Combine(appDir, "dist", bin);
                        if (!File.Exists(src)) src = Path.Combine(appDir, "..", "dist", bin);

                        if (File.Exists(src))
                        {
                            string dest = Path.Combine(installDir, bin);
                            File.Copy(src, dest, true);
                        }
                    }

                    SetProgress(60, "Configuration des raccourcis...");
                    Thread.Sleep(300);

                    string suiteExe = Path.Combine(installDir, "iSuite.exe");

                    // Windows Startup
                    if (chkAutoStart != null && chkAutoStart.Checked)
                    {
                        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                        {
                            if (key != null) key.SetValue("iSuite", "\"" + suiteExe + "\" --tray");
                        }
                    }

                    // Desktop Shortcut via PowerShell WScript.Shell
                    if (chkDesktop != null && chkDesktop.Checked)
                    {
                        string desk = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                        CreateShortcut(Path.Combine(desk, "iSuite.lnk"), suiteExe, "iSuite — Studio & Shell Glass");
                    }

                    SetProgress(100, "Installation finalisée !");
                    Thread.Sleep(300);
                }
                catch { }

                if (IsHandleCreated && !IsDisposed)
                {
                    BeginInvoke((Action)(() => ShowStep(3)));
                }
            });
        }

        private void SetProgress(int percent, string text)
        {
            if (IsHandleCreated && !IsDisposed)
            {
                BeginInvoke((Action)(() =>
                {
                    if (progressBar != null) progressBar.Value = percent;
                    if (statusLabel != null) statusLabel.Text = text;
                }));
            }
        }

        private void CreateShortcut(string shortcutPath, string targetPath, string description)
        {
            try
            {
                string script = string.Format(
                    "$ws = New-Object -ComObject WScript.Shell; $s = $ws.CreateShortcut('{0}'); $s.TargetPath = '{1}'; $s.Description = '{2}'; $s.Save()",
                    shortcutPath.Replace("'", "''"), targetPath.Replace("'", "''"), description.Replace("'", "''"));
                Process p = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "-NoProfile -Command \"" + script + "\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
                if (p != null) p.WaitForExit();
            }
            catch { }
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
            Application.Run(new SetupWizardForm());
        }
    }
}
