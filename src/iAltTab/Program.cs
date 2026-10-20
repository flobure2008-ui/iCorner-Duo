using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace iAltTab
{
    public static class NativeMethods
    {
        public const int WS_EX_TOOLWINDOW = 0x80;
        public const int WS_EX_APPWINDOW = 0x00040000;
        public const int GWL_EXSTYLE = -20;
        public const int SW_RESTORE = 9;
        public const uint WM_CLOSE = 0x0010;

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8) return GetWindowLongPtr64(hWnd, nIndex);
            return GetWindowLongPtr32(hWnd, nIndex);
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

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
                    AccentState = 4,
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
    }

    public class WindowEntry
    {
        public IntPtr Hwnd;
        public string Title;
        public RectangleF Rect;
        public RectangleF CloseRect;
    }

    public class MissionControlForm : Form
    {
        private List<WindowEntry> allWindows = new List<WindowEntry>();
        private List<WindowEntry> filteredWindows = new List<WindowEntry>();
        private string searchText = "";
        private int selectedIndex = 0;
        private int hoveredIndex = -1;
        private int hoveredCloseIndex = -1;

        public MissionControlForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            ShowInTaskbar = false;
            TopMost = true;
            DoubleBuffered = true;
            BackColor = Color.FromArgb(10, 12, 18);

            KeyPreview = true;
            KeyDown += OnKeyDownHandler;

            RefreshWindows();
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
            NativeMethods.EnableWindowGlassBlur(Handle, Color.FromArgb(8, 10, 16), 235);
        }

        private void RefreshWindows()
        {
            allWindows.Clear();
            NativeMethods.EnumWindows((hWnd, lParam) =>
            {
                if (!NativeMethods.IsWindowVisible(hWnd)) return true;
                int len = NativeMethods.GetWindowTextLength(hWnd);
                if (len <= 0) return true;

                StringBuilder titleSb = new StringBuilder(len + 1);
                NativeMethods.GetWindowText(hWnd, titleSb, titleSb.Capacity);
                string title = titleSb.ToString().Trim();
                if (string.IsNullOrEmpty(title)) return true;

                StringBuilder clsSb = new StringBuilder(256);
                NativeMethods.GetClassName(hWnd, clsSb, 256);
                string cls = clsSb.ToString();

                if (cls == "Progman" || cls == "WorkerW" || cls == "Shell_TrayWnd" || cls == "Shell_SecondaryTrayWnd" || cls == "Windows.UI.Core.CoreWindow" || cls.Contains("MissionControlForm")) return true;

                long exStyle = NativeMethods.GetWindowLongPtr(hWnd, NativeMethods.GWL_EXSTYLE).ToInt64();
                if ((exStyle & NativeMethods.WS_EX_TOOLWINDOW) != 0) return true;

                IntPtr owner = NativeMethods.GetWindow(hWnd, 4);
                if (owner != IntPtr.Zero && (exStyle & NativeMethods.WS_EX_APPWINDOW) == 0) return true;

                allWindows.Add(new WindowEntry { Hwnd = hWnd, Title = title });
                return true;
            }, IntPtr.Zero);

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrEmpty(searchText))
            {
                filteredWindows = new List<WindowEntry>(allWindows);
            }
            else
            {
                filteredWindows = allWindows.FindAll(w => w.Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (selectedIndex >= filteredWindows.Count) selectedIndex = Math.Max(0, filteredWindows.Count - 1);
            Invalidate();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            else if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Right)
            {
                if (filteredWindows.Count > 0)
                {
                    selectedIndex = (selectedIndex + 1) % filteredWindows.Count;
                    Invalidate();
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (filteredWindows.Count > 0)
                {
                    selectedIndex = (selectedIndex - 1 + filteredWindows.Count) % filteredWindows.Count;
                    Invalidate();
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                ActivateSelected();
            }
            else if (e.KeyCode == Keys.Back)
            {
                if (searchText.Length > 0)
                {
                    searchText = searchText.Substring(0, searchText.Length - 1);
                    ApplyFilter();
                }
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (!char.IsControl(e.KeyChar))
            {
                searchText += e.KeyChar;
                ApplyFilter();
            }
        }

        private void ActivateSelected()
        {
            if (selectedIndex >= 0 && selectedIndex < filteredWindows.Count)
            {
                IntPtr hwnd = filteredWindows[selectedIndex].Hwnd;
                NativeMethods.ShowWindow(hwnd, NativeMethods.SW_RESTORE);
                NativeMethods.SetForegroundWindow(hwnd);
                Close();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            hoveredIndex = -1;
            hoveredCloseIndex = -1;

            for (int i = 0; i < filteredWindows.Count; i++)
            {
                if (filteredWindows[i].CloseRect.Contains(e.Location))
                {
                    hoveredCloseIndex = i;
                    Invalidate();
                    return;
                }
                if (filteredWindows[i].Rect.Contains(e.Location))
                {
                    hoveredIndex = i;
                    selectedIndex = i;
                    Invalidate();
                    return;
                }
            }
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (hoveredCloseIndex >= 0 && hoveredCloseIndex < filteredWindows.Count)
            {
                IntPtr hwnd = filteredWindows[hoveredCloseIndex].Hwnd;
                NativeMethods.PostMessage(hwnd, NativeMethods.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                allWindows.RemoveAll(w => w.Hwnd == hwnd);
                ApplyFilter();
                return;
            }

            if (hoveredIndex >= 0 && hoveredIndex < filteredWindows.Count)
            {
                ActivateSelected();
                return;
            }

            // Click outside dismisses
            if (e.Y > 200) Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Frosted dark acrylic backdrop
            using (SolidBrush bg = new SolidBrush(Color.FromArgb(235, 8, 10, 16)))
                g.FillRectangle(bg, ClientRectangle);

            // Title
            using (Font hf = new Font("Segoe UI Semibold", 18f, FontStyle.Bold))
            using (SolidBrush hb = new SolidBrush(Color.White))
            {
                g.DrawString("iAltTab • Mission Control Liquid Glass", hf, hb, 80, 50);
            }

            // Search Bar
            RectangleF searchBox = new RectangleF(80, 100, Math.Min(600, Width - 160), 44);
            using (GraphicsPath sp = CreateRoundedRect(searchBox, 12f))
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(28, 255, 255, 255)))
            using (Pen spen = new Pen(Color.FromArgb(60, 255, 255, 255), 1.2f))
            {
                g.FillPath(sb, sp);
                g.DrawPath(spen, sp);
            }

            string prompt = string.IsNullOrEmpty(searchText) ? "Tapez pour filtrer les fenêtres ouvertes..." : searchText;
            using (Font sf = new Font("Segoe UI", 11f))
            using (SolidBrush sb = new SolidBrush(string.IsNullOrEmpty(searchText) ? Color.FromArgb(140, 160, 190) : Color.White))
            {
                g.DrawString("🔍  " + prompt, sf, sb, searchBox.X + 16, searchBox.Y + 11);
            }

            // Cards Grid
            int startX = 80;
            int startY = 175;
            int cardW = 320;
            int cardH = 150;
            int gap = 24;
            int cols = Math.Max(1, (Width - 160) / (cardW + gap));

            for (int i = 0; i < filteredWindows.Count; i++)
            {
                int col = i % cols;
                int row = i / cols;
                RectangleF cardRect = new RectangleF(startX + col * (cardW + gap), startY + row * (cardH + gap), cardW, cardH);
                filteredWindows[i].Rect = cardRect;

                bool isSel = (i == selectedIndex);
                Color accent = Color.FromArgb(70, 200, 255);

                // Liquid Glass Card
                using (GraphicsPath cp = CreateRoundedRect(cardRect, 16f))
                {
                    using (SolidBrush cb = new SolidBrush(isSel ? Color.FromArgb(60, 20, 36, 60) : Color.FromArgb(20, 255, 255, 255)))
                        g.FillPath(cb, cp);

                    using (LinearGradientBrush sheen = new LinearGradientBrush(cardRect, Color.FromArgb(isSel ? 45 : 20, 255, 255, 255), Color.FromArgb(10, accent), 45f))
                        g.FillPath(sheen, cp);

                    using (Pen cpen = new Pen(isSel ? accent : Color.FromArgb(40, 255, 255, 255), isSel ? 2.0f : 1.0f))
                        g.DrawPath(cpen, cp);
                }

                // Window badge / initials
                RectangleF badgeRect = new RectangleF(cardRect.X + 18, cardRect.Y + 18, 38, 38);
                using (GraphicsPath bp = CreateRoundedRect(badgeRect, 10f))
                using (SolidBrush bb = new SolidBrush(Color.FromArgb(80, 24, 32, 50)))
                {
                    g.FillPath(bb, bp);
                }

                string letter = filteredWindows[i].Title.Substring(0, 1).ToUpper();
                using (Font lf = new Font("Segoe UI Semibold", 13f, FontStyle.Bold))
                using (SolidBrush lb = new SolidBrush(accent))
                {
                    SizeF lsz = g.MeasureString(letter, lf);
                    g.DrawString(letter, lf, lb, badgeRect.X + (badgeRect.Width - lsz.Width) / 2f, badgeRect.Y + (badgeRect.Height - lsz.Height) / 2f);
                }

                // Title
                string dispTitle = filteredWindows[i].Title;
                if (dispTitle.Length > 28) dispTitle = dispTitle.Substring(0, 26) + "…";
                using (Font tf = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold))
                using (SolidBrush tb = new SolidBrush(Color.White))
                {
                    g.DrawString(dispTitle, tf, tb, cardRect.X + 66, cardRect.Y + 22);
                }

                // Close Button ✕
                RectangleF closeRect = new RectangleF(cardRect.Right - 36, cardRect.Y + 14, 24, 24);
                filteredWindows[i].CloseRect = closeRect;
                bool isCloseHov = (hoveredCloseIndex == i);

                using (GraphicsPath clp = CreateRoundedRect(closeRect, 6f))
                using (SolidBrush clb = new SolidBrush(isCloseHov ? Color.FromArgb(200, 220, 40, 60) : Color.FromArgb(30, 255, 255, 255)))
                {
                    g.FillPath(clb, clp);
                }

                using (Font cf = new Font("Segoe UI Semibold", 9f, FontStyle.Bold))
                using (SolidBrush cb = new SolidBrush(Color.White))
                {
                    g.DrawString("✕", cf, cb, closeRect.X + 5, closeRect.Y + 3);
                }

                // Bottom preview bar
                using (Font pf = new Font("Segoe UI", 8.8f))
                using (SolidBrush pb = new SolidBrush(Color.FromArgb(160, 180, 210)))
                {
                    g.DrawString("Cliquez pour afficher la fenêtre • Échap pour quitter", pf, pb, cardRect.X + 18, cardRect.Bottom - 30);
                }
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
            Application.Run(new MissionControlForm());
        }
    }
}
