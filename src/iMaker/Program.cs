using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace iMaker
{
    public static class NativeMethods
    {
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();
    }

    public class AppProject
    {
        public string Title = "Mon Widget Perso";
        public string AppId = "MonWidget";
        public string Author = "Créateur iSuite";
        public string Version = "1.0.0";
        public string Category = "Widgets Flottants";
        public string Description = "Mon widget personnalisé créé avec le studio iMaker.";
        public string Glyph = "💎";
        public int Width = 320;
        public int Height = 180;
        public int CornerRadius = 18;
        public int Opacity = 240;
        public bool ClickThrough = false;
        public bool TopMost = false;
        public Color AccentColor = Color.FromArgb(70, 200, 255);
        public string TemplateType = "Widget"; // Widget, Tool, Audio
        public string CustomScript = "// Ajoutez votre logique de rendu ou comportement ici\n";
    }

    public class MakerStudioForm : Form
    {
        private AppProject project = new AppProject();
        private TextBox txtTitle;
        private TextBox txtId;
        private TextBox txtGlyph;
        private ComboBox cmbCategory;
        private ComboBox cmbTemplate;
        private NumericUpDown numW;
        private NumericUpDown numH;
        private TrackBar trkOpacity;
        private TrackBar trkCorner;
        private CheckBox chkClickThrough;
        private CheckBox chkTopMost;
        private TextBox txtCode;
        private Panel previewPanel;
        private Label lblStatus;

        public MakerStudioForm()
        {
            Text = "iMaker — Studio de Création d'Applications & Widgets iSuite";
            Size = new Size(1140, 780);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(11, 13, 20);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9.2f);
            DoubleBuffered = true;

            BuildUI();
            UpdateGeneratedCode();
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

        private void BuildUI()
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
                using (SolidBrush gray = new SolidBrush(Color.FromArgb(160, 175, 205)))
                {
                    g.DrawString("iMaker", tf, wb, 24, 12);
                    SizeF sz = g.MeasureString("iMaker", tf);
                    g.DrawString("App & Widget Studio", tf, ab, 24 + sz.Width + 4, 12);
                    g.DrawString("Créez, personnalisez, testez et publiez vos propres applications et skins de bureau", sf, gray, 26, 44);
                }
            };
            Controls.Add(header);

            // Left Properties Panel (Width 400)
            Panel leftPanel = new Panel { Dock = DockStyle.Left, Width = 420, BackColor = Color.FromArgb(13, 16, 25), AutoScroll = true, Padding = new Padding(20) };
            Controls.Add(leftPanel);

            // Center / Right Workspace
            Panel mainWork = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(10, 12, 18), Padding = new Padding(24) };
            Controls.Add(mainWork);

            // Left Controls
            int curY = 16;
            AddHeaderLabel(leftPanel, "1. Métadonnées de l'Application", curY);
            curY += 32;

            AddPropLabel(leftPanel, "Nom de l'application :", curY);
            txtTitle = CreateTextBox(leftPanel, project.Title, curY + 22, 360);
            txtTitle.TextChanged += (s, e) => { project.Title = txtTitle.Text; UpdatePreview(); };
            curY += 56;

            AddPropLabel(leftPanel, "Identifiant unique (ID) :", curY);
            txtId = CreateTextBox(leftPanel, project.AppId, curY + 22, 190);
            txtId.TextChanged += (s, e) => { project.AppId = txtId.Text; };

            AddPropLabel(leftPanel, "Icône / Glyphe :", curY, 220);
            txtGlyph = CreateTextBox(leftPanel, project.Glyph, curY + 22, 140, 220);
            txtGlyph.TextChanged += (s, e) => { project.Glyph = txtGlyph.Text; UpdatePreview(); };
            curY += 56;

            AddPropLabel(leftPanel, "Catégorie :", curY);
            cmbCategory = new ComboBox { Location = new Point(20, curY + 22), Size = new Size(360, 30), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.2f) };
            cmbCategory.Items.AddRange(new string[] { "Widgets Flottants", "Shell & Bureau", "Multi-Moniteurs", "Outils & Utilitaires" });
            cmbCategory.SelectedIndex = 0;
            cmbCategory.SelectedIndexChanged += (s, e) => project.Category = cmbCategory.SelectedItem.ToString();
            leftPanel.Controls.Add(cmbCategory);
            curY += 56;

            AddPropLabel(leftPanel, "Modèle de départ (Template) :", curY);
            cmbTemplate = new ComboBox { Location = new Point(20, curY + 22), Size = new Size(360, 30), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.2f) };
            cmbTemplate.Items.AddRange(new string[] { "Widget de Bureau Rainmeter", "Application Fenêtre Flottante", "Spectre Audio & Musique", "Outil Mémos & Tâches" });
            cmbTemplate.SelectedIndex = 0;
            cmbTemplate.SelectedIndexChanged += (s, e) => { project.TemplateType = cmbTemplate.SelectedItem.ToString(); UpdateGeneratedCode(); UpdatePreview(); };
            leftPanel.Controls.Add(cmbTemplate);
            curY += 66;

            AddHeaderLabel(leftPanel, "2. Style & Dimensions Verre", curY);
            curY += 32;

            AddPropLabel(leftPanel, "Dimensions (Largeur x Hauteur px) :", curY);
            numW = new NumericUpDown { Location = new Point(20, curY + 22), Size = new Size(100, 28), Minimum = 100, Maximum = 1920, Value = project.Width };
            numW.ValueChanged += (s, e) => { project.Width = (int)numW.Value; UpdatePreview(); };
            leftPanel.Controls.Add(numW);

            numH = new NumericUpDown { Location = new Point(135, curY + 22), Size = new Size(100, 28), Minimum = 60, Maximum = 1080, Value = project.Height };
            numH.ValueChanged += (s, e) => { project.Height = (int)numH.Value; UpdatePreview(); };
            leftPanel.Controls.Add(numH);
            curY += 56;

            AddPropLabel(leftPanel, "Opacité du verre Liquid Glass :", curY);
            trkOpacity = new TrackBar { Location = new Point(20, curY + 22), Size = new Size(360, 35), Minimum = 50, Maximum = 255, Value = project.Opacity };
            trkOpacity.ValueChanged += (s, e) => { project.Opacity = trkOpacity.Value; UpdatePreview(); };
            leftPanel.Controls.Add(trkOpacity);
            curY += 56;

            AddPropLabel(leftPanel, "Rayon de courbure des coins (px) :", curY);
            trkCorner = new TrackBar { Location = new Point(20, curY + 22), Size = new Size(360, 35), Minimum = 0, Maximum = 40, Value = project.CornerRadius };
            trkCorner.ValueChanged += (s, e) => { project.CornerRadius = trkCorner.Value; UpdatePreview(); };
            leftPanel.Controls.Add(trkCorner);
            curY += 56;

            chkClickThrough = new CheckBox { Text = "Clics traversants (Click-Through sans interférence)", Location = new Point(20, curY), AutoSize = true, Checked = project.ClickThrough };
            chkClickThrough.CheckedChanged += (s, e) => project.ClickThrough = chkClickThrough.Checked;
            leftPanel.Controls.Add(chkClickThrough);
            curY += 28;

            chkTopMost = new CheckBox { Text = "Toujours au premier plan (TopMost)", Location = new Point(20, curY), AutoSize = true, Checked = project.TopMost };
            chkTopMost.CheckedChanged += (s, e) => project.TopMost = chkTopMost.Checked;
            leftPanel.Controls.Add(chkTopMost);
            curY += 40;

            // Right Area: Split between Visual Live Preview & Generated Code
            Label lblPrev = new Label { Text = "Aperçu Visuel en Direct :", Location = new Point(24, 16), AutoSize = true, Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold) };
            mainWork.Controls.Add(lblPrev);

            previewPanel = new Panel { Location = new Point(24, 46), Size = new Size(project.Width, project.Height), BackColor = Color.Transparent };
            previewPanel.Paint += (s, e) => DrawWidgetPreview(e.Graphics);
            mainWork.Controls.Add(previewPanel);

            // Code Editor
            Label lblCode = new Label { Text = "Code Source C# Généré & Logique du Widget :", Location = new Point(24, 250), AutoSize = true, Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold) };
            mainWork.Controls.Add(lblCode);

            txtCode = new TextBox
            {
                Location = new Point(24, 280),
                Size = new Size(640, 290),
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                BackColor = Color.FromArgb(16, 20, 32),
                ForeColor = Color.FromArgb(180, 225, 255),
                Font = new Font("Consolas", 9.5f),
                WordWrap = false
            };
            mainWork.Controls.Add(txtCode);

            // Action Buttons
            Button btnTest = new Button
            {
                Text = "▶  Tester mon Application",
                Location = new Point(24, 585),
                Size = new Size(200, 42),
                BackColor = Color.FromArgb(35, 120, 230),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9.8f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnTest.Click += (s, e) => TestApp();
            mainWork.Controls.Add(btnTest);

            Button btnExport = new Button
            {
                Text = "📦  Compiler & Exporter (.exe)",
                Location = new Point(235, 585),
                Size = new Size(210, 42),
                BackColor = Color.FromArgb(35, 55, 85),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9.8f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExport.Click += (s, e) => ExportApp();
            mainWork.Controls.Add(btnExport);

            Button btnPublish = new Button
            {
                Text = "🛍️  Publier dans iStore",
                Location = new Point(455, 585),
                Size = new Size(200, 42),
                BackColor = Color.FromArgb(140, 60, 200),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9.8f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPublish.Click += (s, e) => PublishToStore();
            mainWork.Controls.Add(btnPublish);

            lblStatus = new Label
            {
                Text = "Prêt. Modifiez les paramètres à gauche pour adapter votre création.",
                Location = new Point(24, 638),
                Size = new Size(640, 24),
                ForeColor = Color.FromArgb(140, 160, 190)
            };
            mainWork.Controls.Add(lblStatus);
        }

        private void DrawWidgetPreview(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(1, 1, previewPanel.Width - 2, previewPanel.Height - 2);

            // Liquid Glass Background
            using (GraphicsPath p = CreateRoundedRect(r, project.CornerRadius))
            {
                using (SolidBrush bg = new SolidBrush(Color.FromArgb(project.Opacity, 12, 16, 26)))
                    g.FillPath(bg, p);

                using (LinearGradientBrush sheen = new LinearGradientBrush(r, Color.FromArgb(35, 255, 255, 255), Color.FromArgb(12, project.AccentColor), 45f))
                    g.FillPath(sheen, p);

                using (LinearGradientBrush rim = new LinearGradientBrush(r, Color.FromArgb(90, 255, 255, 255), Color.FromArgb(35, project.AccentColor), 60f))
                using (Pen rimPen = new Pen(rim, 1.25f))
                    g.DrawPath(rimPen, p);
            }

            // Title & Glyph
            using (Font gf = new Font("Segoe UI", 16f))
            using (SolidBrush gb = new SolidBrush(Color.White))
            {
                g.DrawString(project.Glyph, gf, gb, 16, 14);
            }

            using (Font tf = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold))
            using (SolidBrush tb = new SolidBrush(Color.White))
            {
                g.DrawString(project.Title, tf, tb, 52, 18);
            }

            // Content preview based on template
            if (project.TemplateType.Contains("Rainmeter") || project.TemplateType.Contains("Widget"))
            {
                using (Font timeF = new Font("Segoe UI Semibold", 20f, FontStyle.Bold))
                using (SolidBrush timeB = new SolidBrush(Color.White))
                {
                    g.DrawString(DateTime.Now.ToString("HH:mm:ss"), timeF, timeB, 20, 56);
                }

                using (Font df = new Font("Segoe UI", 8.8f))
                using (SolidBrush db = new SolidBrush(Color.FromArgb(160, 180, 215)))
                {
                    g.DrawString(DateTime.Now.ToString("dddd d MMMM yyyy"), df, db, 22, 98);
                }
            }
            else if (project.TemplateType.Contains("Audio"))
            {
                for (int i = 0; i < 12; i++)
                {
                    float bh = 15f + (float)(Math.Sin(i * 0.8) + 1.0) * 20f;
                    RectangleF barRect = new RectangleF(24 + i * 14, previewPanel.Height - 30 - bh, 8, bh);
                    using (GraphicsPath bp = CreateRoundedRect(barRect, 3f))
                    using (SolidBrush bb = new SolidBrush(project.AccentColor))
                        g.FillPath(bb, bp);
                }
            }
            else
            {
                using (Font pf = new Font("Segoe UI", 9f))
                using (SolidBrush pb = new SolidBrush(Color.FromArgb(180, 200, 230)))
                {
                    g.DrawString("Interface Liquid Glass interactive", pf, pb, 20, 60);
                    g.DrawString("Personnalisable avec vos propres contrôles", pf, pb, 20, 85);
                }
            }
        }

        private void UpdatePreview()
        {
            previewPanel.Size = new Size(Math.Min(640, project.Width), Math.Min(200, project.Height));
            previewPanel.Invalidate();
            UpdateGeneratedCode();
        }

        private void UpdateGeneratedCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// ==========================================================================");
            sb.AppendLine("// " + project.Title + " - Créé avec iMaker Studio pour iSuite");
            sb.AppendLine("// Auteur: " + project.Author + " | Version: " + project.Version);
            sb.AppendLine("// ==========================================================================");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Drawing;");
            sb.AppendLine("using System.Drawing.Drawing2D;");
            sb.AppendLine("using System.Windows.Forms;");
            sb.AppendLine("using System.Runtime.InteropServices;");
            sb.AppendLine();
            sb.AppendLine("namespace " + project.AppId);
            sb.AppendLine("{");
            sb.AppendLine("    public class CustomWidgetForm : Form");
            sb.AppendLine("    {");
            sb.AppendLine("        public CustomWidgetForm()");
            sb.AppendLine("        {");
            sb.AppendLine("            FormBorderStyle = FormBorderStyle.None;");
            sb.AppendLine("            StartPosition = FormStartPosition.CenterScreen;");
            sb.AppendLine("            ShowInTaskbar = false;");
            sb.AppendLine("            TopMost = " + (project.TopMost ? "true" : "false") + ";");
            sb.AppendLine("            DoubleBuffered = true;");
            sb.AppendLine(string.Format("            Size = new Size({0}, {1});", project.Width, project.Height));
            sb.AppendLine("            BackColor = Color.FromArgb(12, 16, 26);");
            sb.AppendLine();
            sb.AppendLine("            // Permet de glisser-déplacer la fenêtre à la souris");
            sb.AppendLine("            MouseDown += (s, e) => {");
            sb.AppendLine("                if (e.Button == MouseButtons.Left) {");
            sb.AppendLine("                    ReleaseCapture();");
            sb.AppendLine("                    SendMessage(Handle, 0xA1, 2, 0);");
            sb.AppendLine("                }");
            sb.AppendLine("            };");
            sb.AppendLine();
            sb.AppendLine("            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer { Interval = 1000 };");
            sb.AppendLine("            t.Tick += (s, e) => Invalidate();");
            sb.AppendLine("            t.Start();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [DllImport(\"user32.dll\")] public static extern bool ReleaseCapture();");
            sb.AppendLine("        [DllImport(\"user32.dll\")] public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);");
            sb.AppendLine();
            sb.AppendLine("        protected override void OnPaint(PaintEventArgs e)");
            sb.AppendLine("        {");
            sb.AppendLine("            Graphics g = e.Graphics;");
            sb.AppendLine("            g.SmoothingMode = SmoothingMode.AntiAlias;");
            sb.AppendLine("            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);");
            sb.AppendLine();
            sb.AppendLine(string.Format("            // Liquid Glass Rendering ({0}px corner, {1} opacity)", project.CornerRadius, project.Opacity));
            sb.AppendLine("            using (GraphicsPath p = CreateRoundedRect(r, " + project.CornerRadius + "f)) {");
            sb.AppendLine(string.Format("                using (SolidBrush bg = new SolidBrush(Color.FromArgb({0}, 12, 16, 26))) g.FillPath(bg, p);", project.Opacity));
            sb.AppendLine("                using (Pen rim = new Pen(Color.FromArgb(90, 255, 255, 255), 1.2f)) g.DrawPath(rim, p);");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            using (Font f = new Font(\"Segoe UI Semibold\", 11f, FontStyle.Bold))");
            sb.AppendLine("            using (SolidBrush b = new SolidBrush(Color.White)) {");
            sb.AppendLine(string.Format("                g.DrawString(\"{0} {1}\", f, b, 18, 16);", project.Glyph, project.Title));
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        private GraphicsPath CreateRoundedRect(RectangleF rect, float radius) {");
            sb.AppendLine("            GraphicsPath p = new GraphicsPath(); float d = radius * 2f;");
            sb.AppendLine("            p.AddArc(rect.X, rect.Y, d, d, 180, 90);");
            sb.AppendLine("            p.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);");
            sb.AppendLine("            p.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);");
            sb.AppendLine("            p.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);");
            sb.AppendLine("            p.CloseFigure(); return p;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [STAThread]");
            sb.AppendLine("        static void Main() {");
            sb.AppendLine("            Application.EnableVisualStyles();");
            sb.AppendLine("            Application.Run(new CustomWidgetForm());");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            txtCode.Text = sb.ToString();
        }

        private void TestApp()
        {
            lblStatus.Text = "Compilation à la volée et lancement du test...";
            string tempDir = Path.Combine(Path.GetTempPath(), "iMakerBuild_" + project.AppId);
            if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);

            string srcFile = Path.Combine(tempDir, "Program.cs");
            string outFile = Path.Combine(tempDir, project.AppId + ".exe");
            File.WriteAllText(srcFile, txtCode.Text, Encoding.UTF8);

            string csc = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe";
            Process p = Process.Start(new ProcessStartInfo
            {
                FileName = csc,
                Arguments = string.Format("/nologo /target:winexe /out:\"{0}\" /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll \"{1}\"", outFile, srcFile),
                CreateNoWindow = true,
                UseShellExecute = false
            });
            p.WaitForExit();

            if (p.ExitCode == 0 && File.Exists(outFile))
            {
                lblStatus.Text = "✓ Application compilée avec succès ! Exécution en cours...";
                Process.Start(outFile);
            }
            else
            {
                lblStatus.Text = "⚠️ Erreur lors de la compilation.";
            }
        }

        private void ExportApp()
        {
            string appDir = Path.GetDirectoryName(Application.ExecutablePath);
            string distDir = Path.Combine(appDir, "dist");
            if (!Directory.Exists(distDir)) distDir = appDir;

            string outFile = Path.Combine(distDir, project.AppId + ".exe");
            string tempSrc = Path.Combine(Path.GetTempPath(), project.AppId + "_src.cs");
            File.WriteAllText(tempSrc, txtCode.Text, Encoding.UTF8);

            string iconPath = Path.Combine(appDir, "icon.ico");
            string iconArg = File.Exists(iconPath) ? string.Format("/win32icon:\"{0}\"", iconPath) : "";

            string csc = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe";
            Process p = Process.Start(new ProcessStartInfo
            {
                FileName = csc,
                Arguments = string.Format("/nologo /target:winexe {0} /out:\"{1}\" /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll \"{2}\"", iconArg, outFile, tempSrc),
                CreateNoWindow = true,
                UseShellExecute = false
            });
            p.WaitForExit();

            if (p.ExitCode == 0 && File.Exists(outFile))
            {
                lblStatus.Text = "✓ Exécutable exporté avec succès : " + outFile;
                MessageBox.Show("Votre application a été compilée avec succès !\nEmplacement : " + outFile, "iMaker Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblStatus.Text = "⚠️ Échec de l'exportation.";
            }
        }

        private void PublishToStore()
        {
            ExportApp();

            // Save plugin manifest in plugins/
            string appDir = Path.GetDirectoryName(Application.ExecutablePath);
            string pluginsDir = Path.Combine(appDir, "plugins");
            if (!Directory.Exists(pluginsDir)) pluginsDir = Path.Combine(appDir, "..", "plugins");
            if (!Directory.Exists(pluginsDir)) Directory.CreateDirectory(pluginsDir);

            string manifestPath = Path.Combine(pluginsDir, project.AppId + ".json");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine(string.Format("  \"Id\": \"{0}\",", project.AppId));
            sb.AppendLine(string.Format("  \"Title\": \"{0}\",", project.Title));
            sb.AppendLine(string.Format("  \"Category\": \"{0}\",", project.Category));
            sb.AppendLine(string.Format("  \"Developer\": \"{0}\",", project.Author));
            sb.AppendLine(string.Format("  \"Version\": \"{0}\",", project.Version));
            sb.AppendLine(string.Format("  \"Summary\": \"{0}\",", project.Description.Replace("\"", "\\\"")));
            sb.AppendLine(string.Format("  \"ExeName\": \"{0}.exe\",", project.AppId));
            sb.AppendLine(string.Format("  \"Glyph\": \"{0}\",", project.Glyph));
            sb.AppendLine(string.Format("  \"Rating\": \"★ 5.0 (Nouveau)\","));
            sb.AppendLine(string.Format("  \"Downloads\": \"1 téléchargement\""));
            sb.AppendLine("}");

            File.WriteAllText(manifestPath, sb.ToString(), Encoding.UTF8);

            lblStatus.Text = "✓ Publié avec succès dans le catalogue iStore !";
            MessageBox.Show("Votre application a été publiée et intégrée au catalogue iStore !", "iMaker Publication", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddHeaderLabel(Panel p, string text, int y)
        {
            Label l = new Label { Text = text, Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold), ForeColor = Color.FromArgb(90, 200, 255) };
            p.Controls.Add(l);
        }

        private void AddPropLabel(Panel p, string text, int y, int x = 20)
        {
            Label l = new Label { Text = text, Location = new Point(x, y), AutoSize = true, Font = new Font("Segoe UI", 8.8f), ForeColor = Color.FromArgb(170, 190, 220) };
            p.Controls.Add(l);
        }

        private TextBox CreateTextBox(Panel p, string text, int y, int w, int x = 20)
        {
            TextBox tb = new TextBox { Text = text, Location = new Point(x, y), Size = new Size(w, 28), BackColor = Color.FromArgb(20, 26, 40), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 9.2f) };
            p.Controls.Add(tb);
            return tb;
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
            Application.Run(new MakerStudioForm());
        }
    }
}
