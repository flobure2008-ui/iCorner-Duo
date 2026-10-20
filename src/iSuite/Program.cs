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
using Microsoft.Win32;

namespace iSuite
{
    // =========================================================================
    // ENUMS
    // =========================================================================
    public enum TaskbarStyle { FloatingDock = 0, ExtendedBar = 1 }
    public enum CurveGeometry { HyperbolicSquircle = 0, CircularArc = 1 }
    public enum CornerEffectMode { MinimalFrost = 0, PrismaticFlow = 1, SoftBreathing = 2, AudioPulseSync = 3, StarlightDust = 4, HorizonWave = 5 }
    public enum PortalTransitionStyle { GlassMorphCombo = 0, LiquidGlassBridge = 1, PrismaticAuraWave = 2, QuantumArcPortal = 3, StardustDrift = 4, RippleResonance = 5, LaserHorizonLine = 6 }
    public enum CapsuleScreenPlacement { FollowActiveCursor = 0, PrimaryScreenTop = 1 }
    public enum CompanionEntity { LuminaOrb = 0, KitsuneFox = 1, DracoCrystal = 2, NyxStar = 3, KuroNekoCat = 4, AxiomSphere = 5 }
    public enum GlassColorPalette
    {
        AetherPrism = 0, FrostWhiteSilver = 1, OceanicCyan = 2, EmeraldMint = 3,
        CrimsonVelvet = 4, SolarAmber = 5, BorealAurora = 6, UltravioletMist = 7,
        SakuraBlossom = 8, ObsidianMonochrome = 9, ChromaticSpectrum = 10, CustomGlassPalette = 11
    }

    // =========================================================================
    // UNIFIED SUITE CONFIGURATION
    // =========================================================================
    public class SuiteSettings
    {
        // 1. Taskbar & Start Menu (Windows 12 Concept Glass)
        public bool TaskbarEnabled = true;
        public TaskbarStyle BarStyle = TaskbarStyle.FloatingDock;
        public int TaskbarHeight = 56;
        public int TaskbarCornerRadius = 18;
        public int TaskbarOpacity = 238;
        public bool TaskbarShowRunningApps = true;
        public bool TaskbarShowClock = true;
        public bool TaskbarShowActionCenterBtn = true;

        public bool StartMenuEnabled = true;
        public int StartMenuWidth = 540;
        public int StartMenuHeight = 560;

        // 2. Action & Notification Center
        public bool ActionCenterEnabled = true;
        public int ActionCenterWidth = 380;
        public bool QuickToggleWifi = true;
        public bool QuickToggleBluetooth = true;
        public bool QuickToggleNightLight = false;
        public bool QuickToggleFocus = false;
        public int SystemVolume = 75;
        public int SystemBrightness = 85;

        // 3. Screen Corners
        public bool CornersEnabled = true;
        public int CornerRadius = 32;
        public CurveGeometry Curve = CurveGeometry.HyperbolicSquircle;
        public bool TopLeft = true;
        public bool TopRight = true;
        public bool BottomLeft = true;
        public bool BottomRight = true;
        public bool CornerBezelGlow = true;
        public int BezelGlowOpacity = 45;
        public int BezelThickness = 18;
        public CornerEffectMode CornerAnim = CornerEffectMode.MinimalFrost;
        public int CornerAnimSpeed = 85;

        // 4. Inter-Screen Fluid Bridge
        public bool BridgeEnabled = true;
        public bool BridgeMagneticProximity = true;
        public PortalTransitionStyle BridgeStyle = PortalTransitionStyle.GlassMorphCombo;
        public int BridgeDurationMs = 850;
        public int BridgeIntensityPercent = 100;

        // 5. Dynamic Island
        public bool IslandEnabled = true;
        public CapsuleScreenPlacement IslandPlacement = CapsuleScreenPlacement.FollowActiveCursor;
        public int IslandScalePercent = 100;
        public bool IslandExpandOnHover = true;
        public bool IslandShowClockWhenIdle = true;

        // 6. Mascot
        public bool MascotEnabled = false;
        public CompanionEntity MascotEntity = CompanionEntity.LuminaOrb;
        public int MascotScalePercent = 100;
        public bool MascotFollowCursorEyes = true;
        public bool MascotDanceToMusic = true;
        public bool MascotShowSpeechBubbles = true;
        public int MascotPosX = -1;
        public int MascotPosY = -1;

        // 7. Theme & Palettes
        public GlassColorPalette Theme = GlassColorPalette.AetherPrism;
        public int CustomColor1Argb = Color.FromArgb(255, 110, 220, 255).ToArgb();
        public int CustomColor2Argb = Color.FromArgb(255, 185, 130, 255).ToArgb();

        private static string SettingsPath
        {
            get
            {
                string dir = Path.GetDirectoryName(Application.ExecutablePath);
                return Path.Combine(dir, "isuite_config.json");
            }
        }

        public void Save()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("{");
                sb.AppendLine(string.Format("  \"TaskbarEnabled\": {0},", TaskbarEnabled ? "true" : "false"));
                sb.AppendLine(string.Format("  \"BarStyle\": {0},", (int)BarStyle));
                sb.AppendLine(string.Format("  \"TaskbarHeight\": {0},", TaskbarHeight));
                sb.AppendLine(string.Format("  \"TaskbarCornerRadius\": {0},", TaskbarCornerRadius));
                sb.AppendLine(string.Format("  \"TaskbarOpacity\": {0},", TaskbarOpacity));
                sb.AppendLine(string.Format("  \"TaskbarShowRunningApps\": {0},", TaskbarShowRunningApps ? "true" : "false"));
                sb.AppendLine(string.Format("  \"TaskbarShowClock\": {0},", TaskbarShowClock ? "true" : "false"));
                sb.AppendLine(string.Format("  \"TaskbarShowActionCenterBtn\": {0},", TaskbarShowActionCenterBtn ? "true" : "false"));
                sb.AppendLine(string.Format("  \"StartMenuEnabled\": {0},", StartMenuEnabled ? "true" : "false"));
                sb.AppendLine(string.Format("  \"ActionCenterEnabled\": {0},", ActionCenterEnabled ? "true" : "false"));
                sb.AppendLine(string.Format("  \"SystemVolume\": {0},", SystemVolume));
                sb.AppendLine(string.Format("  \"SystemBrightness\": {0},", SystemBrightness));
                sb.AppendLine(string.Format("  \"CornersEnabled\": {0},", CornersEnabled ? "true" : "false"));
                sb.AppendLine(string.Format("  \"CornerRadius\": {0},", CornerRadius));
                sb.AppendLine(string.Format("  \"Curve\": {0},", (int)Curve));
                sb.AppendLine(string.Format("  \"TopLeft\": {0},", TopLeft ? "true" : "false"));
                sb.AppendLine(string.Format("  \"TopRight\": {0},", TopRight ? "true" : "false"));
                sb.AppendLine(string.Format("  \"BottomLeft\": {0},", BottomLeft ? "true" : "false"));
                sb.AppendLine(string.Format("  \"BottomRight\": {0},", BottomRight ? "true" : "false"));
                sb.AppendLine(string.Format("  \"CornerBezelGlow\": {0},", CornerBezelGlow ? "true" : "false"));
                sb.AppendLine(string.Format("  \"BezelGlowOpacity\": {0},", BezelGlowOpacity));
                sb.AppendLine(string.Format("  \"BezelThickness\": {0},", BezelThickness));
                sb.AppendLine(string.Format("  \"CornerAnim\": {0},", (int)CornerAnim));
                sb.AppendLine(string.Format("  \"CornerAnimSpeed\": {0},", CornerAnimSpeed));
                sb.AppendLine(string.Format("  \"BridgeEnabled\": {0},", BridgeEnabled ? "true" : "false"));
                sb.AppendLine(string.Format("  \"BridgeMagneticProximity\": {0},", BridgeMagneticProximity ? "true" : "false"));
                sb.AppendLine(string.Format("  \"BridgeStyle\": {0},", (int)BridgeStyle));
                sb.AppendLine(string.Format("  \"BridgeDurationMs\": {0},", BridgeDurationMs));
                sb.AppendLine(string.Format("  \"BridgeIntensityPercent\": {0},", BridgeIntensityPercent));
                sb.AppendLine(string.Format("  \"IslandEnabled\": {0},", IslandEnabled ? "true" : "false"));
                sb.AppendLine(string.Format("  \"IslandPlacement\": {0},", (int)IslandPlacement));
                sb.AppendLine(string.Format("  \"IslandScalePercent\": {0},", IslandScalePercent));
                sb.AppendLine(string.Format("  \"IslandExpandOnHover\": {0},", IslandExpandOnHover ? "true" : "false"));
                sb.AppendLine(string.Format("  \"IslandShowClockWhenIdle\": {0},", IslandShowClockWhenIdle ? "true" : "false"));
                sb.AppendLine(string.Format("  \"MascotEnabled\": {0},", MascotEnabled ? "true" : "false"));
                sb.AppendLine(string.Format("  \"MascotEntity\": {0},", (int)MascotEntity));
                sb.AppendLine(string.Format("  \"MascotScalePercent\": {0},", MascotScalePercent));
                sb.AppendLine(string.Format("  \"MascotFollowCursorEyes\": {0},", MascotFollowCursorEyes ? "true" : "false"));
                sb.AppendLine(string.Format("  \"MascotDanceToMusic\": {0},", MascotDanceToMusic ? "true" : "false"));
                sb.AppendLine(string.Format("  \"MascotShowSpeechBubbles\": {0},", MascotShowSpeechBubbles ? "true" : "false"));
                sb.AppendLine(string.Format("  \"MascotPosX\": {0},", MascotPosX));
                sb.AppendLine(string.Format("  \"MascotPosY\": {0},", MascotPosY));
                sb.AppendLine(string.Format("  \"Theme\": {0},", (int)Theme));
                sb.AppendLine(string.Format("  \"CustomColor1Argb\": {0},", CustomColor1Argb));
                sb.AppendLine(string.Format("  \"CustomColor2Argb\": {0}", CustomColor2Argb));
                sb.AppendLine("}");
                File.WriteAllText(SettingsPath, sb.ToString(), Encoding.UTF8);
            }
            catch { }
        }

        public static SuiteSettings Load()
        {
            SuiteSettings s = new SuiteSettings();
            try
            {
                if (!File.Exists(SettingsPath)) return s;
                string json = File.ReadAllText(SettingsPath, Encoding.UTF8);
                s.TaskbarEnabled = ReadBool(json, "TaskbarEnabled", s.TaskbarEnabled);
                s.BarStyle = (TaskbarStyle)ReadInt(json, "BarStyle", (int)s.BarStyle);
                s.TaskbarHeight = ReadInt(json, "TaskbarHeight", s.TaskbarHeight);
                s.TaskbarCornerRadius = ReadInt(json, "TaskbarCornerRadius", s.TaskbarCornerRadius);
                s.TaskbarOpacity = ReadInt(json, "TaskbarOpacity", s.TaskbarOpacity);
                s.TaskbarShowRunningApps = ReadBool(json, "TaskbarShowRunningApps", s.TaskbarShowRunningApps);
                s.TaskbarShowClock = ReadBool(json, "TaskbarShowClock", s.TaskbarShowClock);
                s.TaskbarShowActionCenterBtn = ReadBool(json, "TaskbarShowActionCenterBtn", s.TaskbarShowActionCenterBtn);
                s.StartMenuEnabled = ReadBool(json, "StartMenuEnabled", s.StartMenuEnabled);
                s.ActionCenterEnabled = ReadBool(json, "ActionCenterEnabled", s.ActionCenterEnabled);
                s.SystemVolume = ReadInt(json, "SystemVolume", s.SystemVolume);
                s.SystemBrightness = ReadInt(json, "SystemBrightness", s.SystemBrightness);
                s.CornersEnabled = ReadBool(json, "CornersEnabled", s.CornersEnabled);
                s.CornerRadius = ReadInt(json, "CornerRadius", s.CornerRadius);
                s.Curve = (CurveGeometry)ReadInt(json, "Curve", (int)s.Curve);
                s.TopLeft = ReadBool(json, "TopLeft", s.TopLeft);
                s.TopRight = ReadBool(json, "TopRight", s.TopRight);
                s.BottomLeft = ReadBool(json, "BottomLeft", s.BottomLeft);
                s.BottomRight = ReadBool(json, "BottomRight", s.BottomRight);
                s.CornerBezelGlow = ReadBool(json, "CornerBezelGlow", s.CornerBezelGlow);
                s.BezelGlowOpacity = ReadInt(json, "BezelGlowOpacity", s.BezelGlowOpacity);
                s.BezelThickness = ReadInt(json, "BezelThickness", s.BezelThickness);
                s.CornerAnim = (CornerEffectMode)ReadInt(json, "CornerAnim", (int)s.CornerAnim);
                s.CornerAnimSpeed = ReadInt(json, "CornerAnimSpeed", s.CornerAnimSpeed);
                s.BridgeEnabled = ReadBool(json, "BridgeEnabled", s.BridgeEnabled);
                s.BridgeMagneticProximity = ReadBool(json, "BridgeMagneticProximity", s.BridgeMagneticProximity);
                s.BridgeStyle = (PortalTransitionStyle)ReadInt(json, "BridgeStyle", (int)s.BridgeStyle);
                s.BridgeDurationMs = ReadInt(json, "BridgeDurationMs", s.BridgeDurationMs);
                s.BridgeIntensityPercent = ReadInt(json, "BridgeIntensityPercent", s.BridgeIntensityPercent);
                s.IslandEnabled = ReadBool(json, "IslandEnabled", s.IslandEnabled);
                s.IslandPlacement = (CapsuleScreenPlacement)ReadInt(json, "IslandPlacement", (int)s.IslandPlacement);
                s.IslandScalePercent = ReadInt(json, "IslandScalePercent", s.IslandScalePercent);
                s.IslandExpandOnHover = ReadBool(json, "IslandExpandOnHover", s.IslandExpandOnHover);
                s.IslandShowClockWhenIdle = ReadBool(json, "IslandShowClockWhenIdle", s.IslandShowClockWhenIdle);
                s.MascotEnabled = ReadBool(json, "MascotEnabled", s.MascotEnabled);
                s.MascotEntity = (CompanionEntity)ReadInt(json, "MascotEntity", (int)s.MascotEntity);
                s.MascotScalePercent = ReadInt(json, "MascotScalePercent", s.MascotScalePercent);
                s.MascotFollowCursorEyes = ReadBool(json, "MascotFollowCursorEyes", s.MascotFollowCursorEyes);
                s.MascotDanceToMusic = ReadBool(json, "MascotDanceToMusic", s.MascotDanceToMusic);
                s.MascotShowSpeechBubbles = ReadBool(json, "MascotShowSpeechBubbles", s.MascotShowSpeechBubbles);
                s.MascotPosX = ReadInt(json, "MascotPosX", s.MascotPosX);
                s.MascotPosY = ReadInt(json, "MascotPosY", s.MascotPosY);
                s.Theme = (GlassColorPalette)ReadInt(json, "Theme", (int)s.Theme);
                s.CustomColor1Argb = ReadInt(json, "CustomColor1Argb", s.CustomColor1Argb);
                s.CustomColor2Argb = ReadInt(json, "CustomColor2Argb", s.CustomColor2Argb);
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

    // =========================================================================
    // GLASS COLOR ENGINE
    // =========================================================================
    public static class GlassThemeEngine
    {
        public static Color[] GetPalette(SuiteSettings settings, float timeSeconds)
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
                case GlassColorPalette.CustomGlassPalette:
                    Color c1 = Color.FromArgb(255, Color.FromArgb(settings.CustomColor1Argb));
                    Color c2 = Color.FromArgb(255, Color.FromArgb(settings.CustomColor2Argb));
                    return new Color[] { c1, c2, Interpolate(c1, c2, 0.5f), Interpolate(c1, Color.White, 0.4f) };
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

        public static void DrawFrostedGlassSurface(Graphics g, RectangleF rect, float cornerRadius, Color baseDarkTint, Color accentTint, int glassOpacity = 238, float borderAlphaRatio = 1.0f)
        {
            if (rect.Width <= 2f || rect.Height <= 2f) return;
            using (GraphicsPath path = CreateRoundedRect(rect, cornerRadius))
            {
                using (SolidBrush baseBrush = new SolidBrush(Color.FromArgb(glassOpacity, baseDarkTint)))
                    g.FillPath(baseBrush, path);

                using (LinearGradientBrush sheen = new LinearGradientBrush(rect, Color.FromArgb((int)(28 * borderAlphaRatio), 255, 255, 255), Color.FromArgb((int)(12 * borderAlphaRatio), accentTint), 35f))
                    g.FillPath(sheen, path);

                using (LinearGradientBrush rimGrad = new LinearGradientBrush(rect, Color.FromArgb((int)(85 * borderAlphaRatio), Interpolate(Color.White, accentTint, 0.35f)), Color.FromArgb((int)(28 * borderAlphaRatio), accentTint), 45f))
                using (Pen rimPen = new Pen(rimGrad, 1.2f))
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

    // =========================================================================
    // NATIVE WIN32 INTEROP & LAYERED WINDOWS
    // =========================================================================
    public static class NativeMethods
    {
        public const int ULW_ALPHA = 2;
        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;
        public const int WS_EX_LAYERED = 0x80000;
        public const int WS_EX_TRANSPARENT = 0x20;
        public const int WS_EX_TOOLWINDOW = 0x80;
        public const int WS_EX_NOACTIVATE = 0x08000000;
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

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

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

    public class CoreAudioPeakMeter
    {
        private IAudioMeterInformation audioMeter;
        public float CurrentAudioPeak = 0f;

        public CoreAudioPeakMeter()
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

        public void UpdateTick()
        {
            float peak = 0f;
            if (audioMeter != null) { try { audioMeter.GetPeakValue(out peak); } catch { } }
            if (peak > CurrentAudioPeak) CurrentAudioPeak = CurrentAudioPeak * 0.45f + peak * 0.55f;
            else CurrentAudioPeak = CurrentAudioPeak * 0.86f + peak * 0.14f;
        }
    }

    public class LayeredOverlayForm : Form
    {
        private bool isClickThrough;

        public LayeredOverlayForm(bool clickThrough = true)
        {
            isClickThrough = clickThrough;
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
                if (isClickThrough) cp.ExStyle |= NativeMethods.WS_EX_TRANSPARENT;
                return cp;
            }
        }

        protected override bool ShowWithoutActivation { get { return true; } }

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

    // =========================================================================
    // WINDOWS 12 CONCEPT GLASS TASKBAR
    // =========================================================================
    public class GlassTaskbarWidget : IDisposable
    {
        private LayeredOverlayForm taskbarForm;
        private Rectangle barBounds = Rectangle.Empty;
        private int hoveredIconIdx = -1;

        public event Action OnStartButtonClicked;
        public event Action OnActionCenterClicked;
        public event Action OnSettingsClicked;

        private struct TaskbarIcon
        {
            public string Glyph;
            public string Tooltip;
            public Action OnClick;
        }

        private List<TaskbarIcon> icons = new List<TaskbarIcon>();

        public GlassTaskbarWidget()
        {
            taskbarForm = new LayeredOverlayForm(false);

            // Pinned Glass Icons
            icons.Add(new TaskbarIcon { Glyph = "❖", Tooltip = "Menu Démarrer (Windows 12 Glass)", OnClick = () => { if (OnStartButtonClicked != null) OnStartButtonClicked(); } });
            icons.Add(new TaskbarIcon { Glyph = "📁", Tooltip = "Explorateur de fichiers", OnClick = () => { try { Process.Start("explorer.exe"); } catch { } } });
            icons.Add(new TaskbarIcon { Glyph = "🌐", Tooltip = "Navigateur Web", OnClick = () => { try { Process.Start("https://google.com"); } catch { } } });
            icons.Add(new TaskbarIcon { Glyph = "💻", Tooltip = "Terminal PowerShell", OnClick = () => { try { Process.Start("powershell.exe"); } catch { } } });
            icons.Add(new TaskbarIcon { Glyph = "⚙️", Tooltip = "iSuite Studio & Personnalisation", OnClick = () => { if (OnSettingsClicked != null) OnSettingsClicked(); } });
            icons.Add(new TaskbarIcon { Glyph = "🔔", Tooltip = "Centre de Contrôle & Notifications", OnClick = () => { if (OnActionCenterClicked != null) OnActionCenterClicked(); } });

            taskbarForm.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left && hoveredIconIdx >= 0 && hoveredIconIdx < icons.Count)
                    icons[hoveredIconIdx].OnClick();
            };

            taskbarForm.Show();
        }

        public void UpdateAndRender(SuiteSettings settings, Rectangle screenBounds, Point cursorPt, Color[] palette, float globalTime)
        {
            if (!settings.TaskbarEnabled) { taskbarForm.Hide(); return; }

            int barH = settings.TaskbarHeight;
            int barW = (settings.BarStyle == TaskbarStyle.FloatingDock) ? Math.Min(640, screenBounds.Width - 80) : (screenBounds.Width - 32);
            int barX = screenBounds.Left + (screenBounds.Width - barW) / 2;
            int barY = screenBounds.Bottom - barH - (settings.BarStyle == TaskbarStyle.FloatingDock ? 14 : 0);

            barBounds = new Rectangle(barX, barY, barW, barH);

            Point relMouse = new Point(cursorPt.X - barX, cursorPt.Y - barY);
            bool isInside = barBounds.Contains(cursorPt);

            hoveredIconIdx = -1;
            int iconSpacing = 50;
            int iconsStartX = (barW - (icons.Count * iconSpacing)) / 2;

            if (isInside)
            {
                for (int i = 0; i < icons.Count; i++)
                {
                    Rectangle iconRect = new Rectangle(iconsStartX + i * iconSpacing, (barH - 40) / 2, 40, 40);
                    if (iconRect.Contains(relMouse)) hoveredIconIdx = i;
                }
            }

            using (Bitmap bmp = new Bitmap(barW, barH, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.Clear(Color.Transparent);

                RectangleF pill = new RectangleF(0, 0, barW, barH);
                Color accent = palette[0];
                GlassThemeEngine.DrawFrostedGlassSurface(g, pill, settings.TaskbarCornerRadius, Color.FromArgb(12, 14, 22), accent, settings.TaskbarOpacity, 0.9f);

                // Render Icons
                for (int i = 0; i < icons.Count; i++)
                {
                    RectangleF ir = new RectangleF(iconsStartX + i * iconSpacing, (barH - 40) / 2f, 40, 40);
                    bool hov = (hoveredIconIdx == i);

                    if (hov)
                    {
                        using (GraphicsPath ip = GlassThemeEngine.CreateRoundedRect(ir, 10f))
                        using (SolidBrush ib = new SolidBrush(Color.FromArgb(45, 255, 255, 255)))
                            g.FillPath(ib, ip);
                    }

                    using (Font f = new Font("Segoe UI Semibold", 13.5f, FontStyle.Bold))
                    using (SolidBrush b = new SolidBrush(hov ? Color.White : Color.FromArgb(215, 225, 245)))
                    {
                        SizeF sz = g.MeasureString(icons[i].Glyph, f);
                        g.DrawString(icons[i].Glyph, f, b, ir.X + (ir.Width - sz.Width) / 2f, ir.Y + (ir.Height - sz.Height) / 2f);
                    }

                    // Active dot under Start
                    if (i == 0)
                    {
                        using (SolidBrush dot = new SolidBrush(accent))
                            g.FillEllipse(dot, ir.X + ir.Width / 2f - 2f, ir.Bottom - 4f, 4f, 4f);
                    }
                }

                // Clock on the right
                if (settings.TaskbarShowClock)
                {
                    string timeStr = DateTime.Now.ToString("HH:mm");
                    string dateStr = DateTime.Now.ToString("dd/MM");
                    using (Font tf = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold))
                    using (Font df = new Font("Segoe UI", 8.0f))
                    using (SolidBrush tb = new SolidBrush(Color.White))
                    using (SolidBrush db = new SolidBrush(Color.FromArgb(160, 175, 205)))
                    {
                        g.DrawString(timeStr, tf, tb, barW - 74, barH / 2f - 14);
                        g.DrawString(dateStr, df, db, barW - 74, barH / 2f + 1);
                    }
                }

                taskbarForm.SetBitmap(bmp, barX, barY, 255);
                if (!taskbarForm.Visible) taskbarForm.Show();
            }
        }

        public void Dispose()
        {
            if (taskbarForm != null) { taskbarForm.Hide(); taskbarForm.Dispose(); taskbarForm = null; }
        }
    }

    // =========================================================================
    // WINDOWS 12 CONCEPT GLASS START MENU POPUP
    // =========================================================================
    public class GlassStartMenuPopup : Form
    {
        private SuiteSettings settings;
        private Action onOpenSettings;

        public GlassStartMenuPopup(SuiteSettings s, Action openSettingsCb)
        {
            settings = s;
            onOpenSettings = openSettingsCb;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            DoubleBuffered = true;
            Size = new Size(settings.StartMenuWidth, settings.StartMenuHeight);
            BackColor = Color.FromArgb(12, 14, 22);

            Deactivate += (s2, e2) => Hide();
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

        public void Toggle(Point anchor)
        {
            if (Visible) { Hide(); return; }
            Location = new Point(anchor.X - Width / 2, anchor.Y - Height - 16);
            Show();
            BringToFront();
            Activate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            Color accent = Color.FromArgb(70, 185, 255);
            GlassThemeEngine.DrawFrostedGlassSurface(g, r, 20f, BackColor, accent, 246, 1.0f);

            // Search Bar
            RectangleF searchRect = new RectangleF(28, 24, Width - 56, 42);
            using (GraphicsPath sp = GlassThemeEngine.CreateRoundedRect(searchRect, 12f))
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(20, 255, 255, 255)))
            using (Pen spen = new Pen(Color.FromArgb(50, 255, 255, 255), 1.2f))
            {
                g.FillPath(sb, sp);
                g.DrawPath(spen, sp);
            }

            using (Font sf = new Font("Segoe UI", 10.2f))
            using (SolidBrush tb = new SolidBrush(Color.FromArgb(180, 195, 225)))
            {
                g.DrawString("🔍  Rechercher des applications, fichiers, paramètres...", sf, tb, 42, 34);
            }

            // Pinned Apps Header
            using (Font hf = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold))
            using (SolidBrush hb = new SolidBrush(Color.White))
            {
                g.DrawString("Applications Épinglées", hf, hb, 30, 85);
            }

            // Acrylic Glass Tiles (Windows 12 grid)
            string[] appTiles = new string[] {
                "📁 Explorateur", "🌐 Navigateur Web", "⚙️ Paramètres iSuite",
                "🎵 Musique & Média", "💬 Messages", "📸 Photos & Créations",
                "💻 Terminal", "🎮 Espace Jeux", "⏱️ Horloge & Alarme"
            };

            int cols = 3;
            int tileW = 150;
            int tileH = 68;
            int startX = 30;
            int startY = 115;

            for (int i = 0; i < appTiles.Length; i++)
            {
                int c = i % cols;
                int row = i / cols;
                RectangleF tr = new RectangleF(startX + c * (tileW + 12), startY + row * (tileH + 12), tileW, tileH);

                using (GraphicsPath tp = GlassThemeEngine.CreateRoundedRect(tr, 14f))
                using (SolidBrush tb = new SolidBrush(Color.FromArgb(18, 255, 255, 255)))
                using (Pen tpen = new Pen(Color.FromArgb(35, 255, 255, 255), 1.0f))
                {
                    g.FillPath(tb, tp);
                    g.DrawPath(tpen, tp);
                }

                using (Font f = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold))
                using (SolidBrush txtb = new SolidBrush(Color.White))
                {
                    g.DrawString(appTiles[i], f, txtb, tr.X + 14, tr.Y + 24);
                }
            }

            // Bottom user profile bar
            RectangleF bottomBar = new RectangleF(0, Height - 64, Width, 64);
            using (Pen sep = new Pen(Color.FromArgb(25, 255, 255, 255), 1f))
                g.DrawLine(sep, 0, Height - 64, Width, Height - 64);

            // User avatar
            using (SolidBrush avb = new SolidBrush(accent))
                g.FillEllipse(avb, 30, Height - 48, 34, 34);

            using (Font uf = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold))
            using (SolidBrush wb = new SolidBrush(Color.White))
            {
                g.DrawString(Environment.UserName, uf, wb, 76, Height - 42);
            }

            // Power button
            using (Font pf = new Font("Segoe UI Semibold", 12f, FontStyle.Bold))
            using (SolidBrush pb = new SolidBrush(Color.FromArgb(200, 220, 255)))
            {
                g.DrawString("⏻", pf, pb, Width - 48, Height - 45);
            }
        }
    }

    // =========================================================================
    // LATERAL GLASS ACTION & NOTIFICATION CENTER
    // =========================================================================
    public class GlassActionCenterPopup : Form
    {
        private SuiteSettings settings;

        public GlassActionCenterPopup(SuiteSettings s)
        {
            settings = s;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            DoubleBuffered = true;
            Size = new Size(settings.ActionCenterWidth, 680);
            BackColor = Color.FromArgb(12, 14, 22);

            Deactivate += (s2, e2) => Hide();
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

        public void Toggle(Rectangle screenBounds)
        {
            if (Visible) { Hide(); return; }
            Location = new Point(screenBounds.Right - Width - 16, screenBounds.Bottom - Height - 76);
            Show();
            BringToFront();
            Activate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            RectangleF r = new RectangleF(1, 1, Width - 2, Height - 2);
            Color accent = Color.FromArgb(70, 185, 255);
            GlassThemeEngine.DrawFrostedGlassSurface(g, r, 20f, BackColor, accent, 246, 1.0f);

            // Title
            using (Font tf = new Font("Segoe UI Semibold", 12f, FontStyle.Bold))
            using (SolidBrush wb = new SolidBrush(Color.White))
            {
                g.DrawString("Centre de Contrôle & Actions", tf, wb, 24, 20);
            }

            // Quick Action Tiles (Wi-Fi, Bluetooth, Night Light, Focus)
            string[] toggles = new string[] { "📶 Wi-Fi (Actif)", "ᛒ Bluetooth (Connecté)", "🌙 Éclairage Nocturne", "🎯 Focus / Ne Pas Déranger" };
            bool[] states = new bool[] { settings.QuickToggleWifi, settings.QuickToggleBluetooth, settings.QuickToggleNightLight, settings.QuickToggleFocus };

            int startY = 60;
            for (int i = 0; i < 4; i++)
            {
                int col = i % 2;
                int row = i / 2;
                RectangleF tr = new RectangleF(24 + col * 170, startY + row * 64, 160, 54);
                bool on = states[i];

                using (GraphicsPath tp = GlassThemeEngine.CreateRoundedRect(tr, 12f))
                using (SolidBrush tb = new SolidBrush(on ? Color.FromArgb(60, accent) : Color.FromArgb(20, 255, 255, 255)))
                using (Pen tpen = new Pen(on ? accent : Color.FromArgb(40, 255, 255, 255), 1.2f))
                {
                    g.FillPath(tb, tp);
                    g.DrawPath(tpen, tp);
                }

                using (Font f = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold))
                using (SolidBrush txtb = new SolidBrush(on ? Color.White : Color.FromArgb(190, 205, 230)))
                {
                    g.DrawString(toggles[i], f, txtb, tr.X + 12, tr.Y + 18);
                }
            }

            // Sliders: Volume & Brightness
            int sliderY = startY + 145;
            using (Font lf = new Font("Segoe UI", 9f))
            using (SolidBrush lb = new SolidBrush(Color.FromArgb(180, 195, 220)))
            {
                g.DrawString(string.Format("🔊 Volume Système ({0}%)", settings.SystemVolume), lf, lb, 26, sliderY);
                DrawTrackSlider(g, 24, sliderY + 24, Width - 48, settings.SystemVolume / 100f, accent);

                g.DrawString(string.Format("☀️ Luminosité Écran ({0}%)", settings.SystemBrightness), lf, lb, 26, sliderY + 60);
                DrawTrackSlider(g, 24, sliderY + 84, Width - 48, settings.SystemBrightness / 100f, Color.FromArgb(255, 200, 80));
            }

            // Notification Stack Header
            int notifY = sliderY + 140;
            using (Font nh = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold))
            using (SolidBrush wb = new SolidBrush(Color.White))
            {
                g.DrawString("Notifications Récentes", nh, wb, 24, notifY);
            }

            // Sample Glass Notification Cards
            string[] notifs = new string[] {
                "iSuite • Tout fonctionne parfaitement en 60 FPS",
                "Spotify • En cours de lecture sur vos enceintes",
                "Système • Mode Windows 12 Concept Glass actif"
            };

            for (int i = 0; i < notifs.Length; i++)
            {
                RectangleF nr = new RectangleF(24, notifY + 32 + i * 58, Width - 48, 48);
                using (GraphicsPath np = GlassThemeEngine.CreateRoundedRect(nr, 12f))
                using (SolidBrush nb = new SolidBrush(Color.FromArgb(16, 255, 255, 255)))
                using (Pen npen = new Pen(Color.FromArgb(30, 255, 255, 255), 1.0f))
                {
                    g.FillPath(nb, np);
                    g.DrawPath(npen, np);
                }

                using (Font nf = new Font("Segoe UI", 8.8f))
                using (SolidBrush ntb = new SolidBrush(Color.FromArgb(220, 230, 250)))
                {
                    g.DrawString(notifs[i], nf, ntb, nr.X + 14, nr.Y + 14);
                }
            }
        }

        private void DrawTrackSlider(Graphics g, float x, float y, float w, float ratio, Color fill)
        {
            RectangleF track = new RectangleF(x, y, w, 10f);
            using (GraphicsPath tp = GlassThemeEngine.CreateRoundedRect(track, 5f))
            {
                using (SolidBrush tb = new SolidBrush(Color.FromArgb(30, 36, 52))) g.FillPath(tb, tp);
            }

            float fw = w * ratio;
            if (fw > 6f)
            {
                RectangleF fr = new RectangleF(x, y, fw, 10f);
                using (GraphicsPath fp = GlassThemeEngine.CreateRoundedRect(fr, 5f))
                using (SolidBrush fb = new SolidBrush(fill))
                    g.FillPath(fb, fp);
            }
        }
    }

    // =========================================================================
    // MASTER OVERLAY CONTROLLER
    // =========================================================================
    public class MasterOverlayController : IDisposable
    {
        public SuiteSettings Settings;
        public CoreAudioPeakMeter AudioMeter;
        public GlassTaskbarWidget Taskbar;
        public GlassStartMenuPopup StartMenu;
        public GlassActionCenterPopup ActionCenter;

        private System.Windows.Forms.Timer tickTimer;
        private long startTicks = DateTime.UtcNow.Ticks;
        private NotifyIcon trayIcon;
        private MasterStudioForm studioForm;

        public MasterOverlayController()
        {
            Settings = SuiteSettings.Load();
            AudioMeter = new CoreAudioPeakMeter();

            Taskbar = new GlassTaskbarWidget();
            StartMenu = new GlassStartMenuPopup(Settings, ShowStudioWindow);
            ActionCenter = new GlassActionCenterPopup(Settings);

            Taskbar.OnStartButtonClicked += () =>
            {
                Screen scr = Screen.PrimaryScreen;
                StartMenu.Toggle(new Point(scr.Bounds.Left + scr.Bounds.Width / 2, scr.Bounds.Bottom - Settings.TaskbarHeight - 20));
            };

            Taskbar.OnActionCenterClicked += () =>
            {
                ActionCenter.Toggle(Screen.PrimaryScreen.Bounds);
            };

            Taskbar.OnSettingsClicked += ShowStudioWindow;

            SetupTray();

            tickTimer = new System.Windows.Forms.Timer { Interval = 16 };
            tickTimer.Tick += OnTick;
            tickTimer.Start();
        }

        public float GlobalTimeSeconds { get { return (float)((DateTime.UtcNow.Ticks - startTicks) / 10000000.0); } }

        private void OnTick(object sender, EventArgs e)
        {
            float t = GlobalTimeSeconds;
            AudioMeter.UpdateTick();
            Color[] pal = GlassThemeEngine.GetPalette(Settings, t);

            Point cursorPt = Point.Empty;
            NativeMethods.POINT rawPt;
            if (NativeMethods.GetCursorPos(out rawPt)) cursorPt = new Point(rawPt.X, rawPt.Y);

            Taskbar.UpdateAndRender(Settings, Screen.PrimaryScreen.Bounds, cursorPt, pal, t);
        }

        private void SetupTray()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "iSuite — Studio Master & Shell Glass";
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Visible = true;

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem studioItem = new ToolStripMenuItem("Ouvrir iSuite Master Studio");
            studioItem.Font = new Font(studioItem.Font, FontStyle.Bold);
            studioItem.Click += (s, e) => ShowStudioWindow();

            ToolStripMenuItem taskbarToggle = new ToolStripMenuItem("Barre des Tâches Flottante");
            taskbarToggle.Checked = Settings.TaskbarEnabled;
            taskbarToggle.Click += (s, e) => { Settings.TaskbarEnabled = !Settings.TaskbarEnabled; taskbarToggle.Checked = Settings.TaskbarEnabled; Settings.Save(); };

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Quitter iSuite");
            exitItem.Click += (s, e) => { trayIcon.Visible = false; Application.Exit(); };

            menu.Items.Add(studioItem);
            menu.Items.Add(taskbarToggle);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(exitItem);

            trayIcon.ContextMenuStrip = menu;
            trayIcon.DoubleClick += (s, e) => ShowStudioWindow();
        }

        public void ShowStudioWindow()
        {
            if (studioForm == null || studioForm.IsDisposed)
                studioForm = new MasterStudioForm(this);
            studioForm.Show();
            studioForm.BringToFront();
            studioForm.Activate();
        }

        public void Dispose()
        {
            if (tickTimer != null) { tickTimer.Stop(); tickTimer.Dispose(); }
            if (Taskbar != null) Taskbar.Dispose();
            if (StartMenu != null) StartMenu.Dispose();
            if (ActionCenter != null) ActionCenter.Dispose();
            if (trayIcon != null) { trayIcon.Visible = false; trayIcon.Dispose(); }
        }
    }

    // =========================================================================
    // MASTER STUDIO FORM (ALL PERSONALIZATION IN ONE GLORIOUS APP)
    // =========================================================================
    public class MasterStudioForm : Form
    {
        private MasterOverlayController controller;
        private Panel[] pages = new Panel[7];
        private Button[] navButtons = new Button[7];
        private int activePage = 0;

        public MasterStudioForm(MasterOverlayController ctrl)
        {
            controller = ctrl;
            Text = "iSuite — Master Studio & Personnalisation Complète";
            Size = new Size(1060, 750);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(11, 13, 20);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9.2f);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            BuildMasterStudio();
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

        private void BuildMasterStudio()
        {
            // Header
            Panel header = new Panel { Dock = DockStyle.Top, Height = 74, BackColor = Color.FromArgb(14, 18, 28) };
            header.Paint += (s, e) =>
            {
                Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = header.ClientRectangle;
                using (LinearGradientBrush grad = new LinearGradientBrush(r, Color.FromArgb(14, 18, 28), Color.FromArgb(22, 16, 32), 0f))
                    g.FillRectangle(grad, r);

                Color[] pal = GlassThemeEngine.GetPalette(controller.Settings, controller.GlobalTimeSeconds);
                using (Font tf = new Font("Segoe UI Semibold", 16f, FontStyle.Bold))
                using (Font sf = new Font("Segoe UI", 8.8f))
                using (SolidBrush wb = new SolidBrush(Color.White))
                using (SolidBrush ab = new SolidBrush(pal[0]))
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(160, 175, 205)))
                {
                    g.DrawString("iSuite", tf, wb, 24, 12);
                    SizeF sz = g.MeasureString("iSuite", tf);
                    g.DrawString("Master Studio", tf, ab, 24 + sz.Width - 4, 12);
                    g.DrawString("Barre des tâches Windows 12 • Menu Démarrer • Centre d'Action • Coins d'écran • Passerelle • Island", sf, sb, 26, 44);
                }
            };
            Controls.Add(header);

            // Left Sidebar
            Panel sidebar = new Panel { Location = new Point(0, 74), Size = new Size(250, 640), BackColor = Color.FromArgb(13, 15, 23) };
            Controls.Add(sidebar);

            string[] tabs = new string[] {
                "🖥️  Barre des Tâches & Menu",
                "🔔  Centre de Contrôle & Notifs",
                "✦  Coins & Dalles d'Écran",
                "🌊  Passerelle Inter-Écrans",
                "💧  Dynamic Island",
                "✨  Mascotte Céleste",
                "🎨  Palettes & Matériaux Verre"
            };

            for (int i = 0; i < tabs.Length; i++)
            {
                int idx = i;
                Button btn = new Button
                {
                    Text = tabs[i],
                    Location = new Point(12, 16 + i * 50),
                    Size = new Size(226, 42),
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                    ForeColor = (i == 0) ? Color.White : Color.FromArgb(170, 185, 215),
                    BackColor = (i == 0) ? Color.FromArgb(32, 50, 85) : Color.FromArgb(18, 22, 34),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (s, e) => SwitchTab(idx);
                navButtons[i] = btn;
                sidebar.Controls.Add(btn);
            }

            // Build Tab Pages
            pages[0] = BuildPageShell();
            pages[1] = BuildPageActionCenter();
            pages[2] = BuildPageCorners();
            pages[3] = BuildPageBridge();
            pages[4] = BuildPageIsland();
            pages[5] = BuildPageMascot();
            pages[6] = BuildPageTheme();

            for (int i = 0; i < pages.Length; i++) Controls.Add(pages[i]);
            SwitchTab(0);
        }

        private void SwitchTab(int idx)
        {
            SuspendLayout();
            activePage = idx;
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].Visible = (i == idx);
                navButtons[i].BackColor = (i == idx) ? Color.FromArgb(32, 50, 85) : Color.FromArgb(18, 22, 34);
                navButtons[i].ForeColor = (i == idx) ? Color.White : Color.FromArgb(170, 185, 215);
            }
            ResumeLayout(false);
        }

        private Panel CreatePage()
        {
            return new Panel { Location = new Point(250, 74), Size = new Size(800, 640), BackColor = Color.FromArgb(11, 13, 20) };
        }

        private Panel BuildPageShell()
        {
            Panel p = CreatePage();
            AddHeading(p, "🖥️ Barre des Tâches Flottante & Menu Démarrer Windows 12");

            CheckBox tbChk = CreateCheckBox(p, "Activer la barre des tâches flottante en verre dépoli", 28, 65, controller.Settings.TaskbarEnabled);
            tbChk.CheckedChanged += (s, e) => { controller.Settings.TaskbarEnabled = tbChk.Checked; controller.Settings.Save(); };

            AddLabel(p, "Style de la barre des tâches :", 28, 110);
            ComboBox barStyleCombo = CreateCombo(p, 28, 135, 360, new string[] { "Dock Flottant Centré (macOS / Windows 12)", "Barre Flottante Étendue Pleine Largeur" }, (int)controller.Settings.BarStyle);
            barStyleCombo.SelectedIndexChanged += (s, e) => { controller.Settings.BarStyle = (TaskbarStyle)barStyleCombo.SelectedIndex; controller.Settings.Save(); };

            AddLabel(p, "Hauteur de la barre (48 à 68 px) :", 28, 185);
            TrackBar hBar = CreateTrack(p, 28, 210, 360, 48, 68, controller.Settings.TaskbarHeight);
            hBar.ValueChanged += (s, e) => { controller.Settings.TaskbarHeight = hBar.Value; controller.Settings.Save(); };

            AddLabel(p, "Rayon d'arrondi du dock (8 à 28 px) :", 28, 265);
            TrackBar rBar = CreateTrack(p, 28, 290, 360, 8, 28, controller.Settings.TaskbarCornerRadius);
            rBar.ValueChanged += (s, e) => { controller.Settings.TaskbarCornerRadius = rBar.Value; controller.Settings.Save(); };

            CheckBox startChk = CreateCheckBox(p, "Activer le Menu Démarrer Acrylique Glass", 28, 350, controller.Settings.StartMenuEnabled);
            startChk.CheckedChanged += (s, e) => { controller.Settings.StartMenuEnabled = startChk.Checked; controller.Settings.Save(); };

            CheckBox clkChk = CreateCheckBox(p, "Afficher l'horloge et la date sur la barre", 28, 385, controller.Settings.TaskbarShowClock);
            clkChk.CheckedChanged += (s, e) => { controller.Settings.TaskbarShowClock = clkChk.Checked; controller.Settings.Save(); };

            return p;
        }

        private Panel BuildPageActionCenter()
        {
            Panel p = CreatePage();
            AddHeading(p, "🔔 Centre de Contrôle & Notifications Latéral");

            CheckBox acChk = CreateCheckBox(p, "Activer le panneau d'action latéral", 28, 65, controller.Settings.ActionCenterEnabled);
            acChk.CheckedChanged += (s, e) => { controller.Settings.ActionCenterEnabled = acChk.Checked; controller.Settings.Save(); };

            AddLabel(p, "Volume Système initial (0 à 100%) :", 28, 110);
            TrackBar volBar = CreateTrack(p, 28, 135, 360, 0, 100, controller.Settings.SystemVolume);
            volBar.ValueChanged += (s, e) => { controller.Settings.SystemVolume = volBar.Value; controller.Settings.Save(); };

            AddLabel(p, "Luminosité Écran initiale (0 à 100%) :", 28, 190);
            TrackBar brBar = CreateTrack(p, 28, 215, 360, 0, 100, controller.Settings.SystemBrightness);
            brBar.ValueChanged += (s, e) => { controller.Settings.SystemBrightness = brBar.Value; controller.Settings.Save(); };

            CheckBox wifiChk = CreateCheckBox(p, "Tuile d'accès rapide Wi-Fi", 28, 280, controller.Settings.QuickToggleWifi);
            wifiChk.CheckedChanged += (s, e) => { controller.Settings.QuickToggleWifi = wifiChk.Checked; controller.Settings.Save(); };

            CheckBox btChk = CreateCheckBox(p, "Tuile d'accès rapide Bluetooth", 28, 315, controller.Settings.QuickToggleBluetooth);
            btChk.CheckedChanged += (s, e) => { controller.Settings.QuickToggleBluetooth = btChk.Checked; controller.Settings.Save(); };

            CheckBox nightChk = CreateCheckBox(p, "Tuile Éclairage Nocturne", 28, 350, controller.Settings.QuickToggleNightLight);
            nightChk.CheckedChanged += (s, e) => { controller.Settings.QuickToggleNightLight = nightChk.Checked; controller.Settings.Save(); };

            return p;
        }

        private Panel BuildPageCorners()
        {
            Panel p = CreatePage();
            AddHeading(p, "✦ Coins d'Écran Organiques & Liseré Spéculaire");

            CheckBox enChk = CreateCheckBox(p, "Activer les coins d'écran arrondis", 28, 65, controller.Settings.CornersEnabled);
            enChk.CheckedChanged += (s, e) => { controller.Settings.CornersEnabled = enChk.Checked; controller.Settings.Save(); };

            AddLabel(p, "Rayon de courbure (0 à 110 px) :", 28, 110);
            TrackBar radBar = CreateTrack(p, 28, 135, 360, 0, 110, controller.Settings.CornerRadius);
            radBar.ValueChanged += (s, e) => { controller.Settings.CornerRadius = radBar.Value; controller.Settings.Save(); };

            AddLabel(p, "Profil géométrique :", 28, 190);
            ComboBox geomCombo = CreateCombo(p, 28, 215, 360, new string[] { "Superellipse Hyper-Continue (n=4.2)", "Arc Circulaire Classique (n=2.0)" }, (int)controller.Settings.Curve);
            geomCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Curve = (CurveGeometry)geomCombo.SelectedIndex; controller.Settings.Save(); };

            CheckBox glowChk = CreateCheckBox(p, "Liseré spéculaire biseauté", 28, 275, controller.Settings.CornerBezelGlow);
            glowChk.CheckedChanged += (s, e) => { controller.Settings.CornerBezelGlow = glowChk.Checked; controller.Settings.Save(); };

            return p;
        }

        private Panel BuildPageBridge()
        {
            Panel p = CreatePage();
            AddHeading(p, "🌊 Passerelle Liquide Inter-Écrans");

            CheckBox enChk = CreateCheckBox(p, "Activer les transitions au passage du curseur", 28, 65, controller.Settings.BridgeEnabled);
            enChk.CheckedChanged += (s, e) => { controller.Settings.BridgeEnabled = enChk.Checked; controller.Settings.Save(); };

            CheckBox magChk = CreateCheckBox(p, "Halo magnétique caustique à l'approche de la bordure", 28, 100, controller.Settings.BridgeMagneticProximity);
            magChk.CheckedChanged += (s, e) => { controller.Settings.BridgeMagneticProximity = magChk.Checked; controller.Settings.Save(); };

            AddLabel(p, "Style visuel de la passerelle :", 28, 145);
            ComboBox stCombo = CreateCombo(p, 28, 170, 360, new string[] {
                "Fusion Glassmorphique (Pont + Halo + Étoiles)",
                "Pont Liquide en Verre Dépoli",
                "Onde Prismatique Atmosphérique",
                "Filaments Quantiques",
                "Nuage de Poussière Stellaire",
                "Résonance Concentrique",
                "Ligne Optique Horizon"
            }, (int)controller.Settings.BridgeStyle);
            stCombo.SelectedIndexChanged += (s, e) => { controller.Settings.BridgeStyle = (PortalTransitionStyle)stCombo.SelectedIndex; controller.Settings.Save(); };

            AddLabel(p, "Durée de la transition (ms) :", 28, 225);
            TrackBar durBar = CreateTrack(p, 28, 250, 360, 350, 2000, controller.Settings.BridgeDurationMs);
            durBar.ValueChanged += (s, e) => { controller.Settings.BridgeDurationMs = durBar.Value; controller.Settings.Save(); };

            return p;
        }

        private Panel BuildPageIsland()
        {
            Panel p = CreatePage();
            AddHeading(p, "💧 Dynamic Island en Verre Liquide");

            CheckBox enChk = CreateCheckBox(p, "Activer la capsule Dynamic Island flottante", 28, 65, controller.Settings.IslandEnabled);
            enChk.CheckedChanged += (s, e) => { controller.Settings.IslandEnabled = enChk.Checked; controller.Settings.Save(); };

            CheckBox hovChk = CreateCheckBox(p, "Déployer automatiquement au survol du curseur", 28, 100, controller.Settings.IslandExpandOnHover);
            hovChk.CheckedChanged += (s, e) => { controller.Settings.IslandExpandOnHover = hovChk.Checked; controller.Settings.Save(); };

            AddLabel(p, "Positionnement écran :", 28, 145);
            ComboBox posCombo = CreateCombo(p, 28, 170, 360, new string[] { "Suivre l'écran du curseur actif", "Toujours au sommet de l'écran principal" }, (int)controller.Settings.IslandPlacement);
            posCombo.SelectedIndexChanged += (s, e) => { controller.Settings.IslandPlacement = (CapsuleScreenPlacement)posCombo.SelectedIndex; controller.Settings.Save(); };

            AddLabel(p, "Échelle de la capsule :", 28, 225);
            TrackBar scBar = CreateTrack(p, 28, 250, 360, 80, 130, controller.Settings.IslandScalePercent);
            scBar.ValueChanged += (s, e) => { controller.Settings.IslandScalePercent = scBar.Value; controller.Settings.Save(); };

            return p;
        }

        private Panel BuildPageMascot()
        {
            Panel p = CreatePage();
            AddHeading(p, "✨ Mascottes Célestes de Bureau");

            CheckBox enChk = CreateCheckBox(p, "Afficher la mascotte céleste sur le bureau", 28, 65, controller.Settings.MascotEnabled);
            enChk.CheckedChanged += (s, e) => { controller.Settings.MascotEnabled = enChk.Checked; controller.Settings.Save(); };

            AddLabel(p, "Créature céleste :", 28, 110);
            ComboBox entCombo = CreateCombo(p, 28, 135, 360, new string[] {
                "Lumina (Orbe stellaire aux anneaux gyroscopiques)",
                "Kitsune (Renard boréal en soie céleste)",
                "Draco (Wyverne géométrique taillée en diamant)",
                "Nyx (Goutte d'opale stellaire)",
                "Kuro-Neko (Silhouettte féline en obsidienne)",
                "Axiom (Sphère saphir et titane)"
            }, (int)controller.Settings.MascotEntity);
            entCombo.SelectedIndexChanged += (s, e) => { controller.Settings.MascotEntity = (CompanionEntity)entCombo.SelectedIndex; controller.Settings.Save(); };

            CheckBox eyesChk = CreateCheckBox(p, "Les yeux suivent le curseur de la souris en temps réel", 28, 195, controller.Settings.MascotFollowCursorEyes);
            eyesChk.CheckedChanged += (s, e) => { controller.Settings.MascotFollowCursorEyes = eyesChk.Checked; controller.Settings.Save(); };

            CheckBox danceChk = CreateCheckBox(p, "Danse et réagit aux pulsations de la musique", 28, 230, controller.Settings.MascotDanceToMusic);
            danceChk.CheckedChanged += (s, e) => { controller.Settings.MascotDanceToMusic = danceChk.Checked; controller.Settings.Save(); };

            return p;
        }

        private Panel BuildPageTheme()
        {
            Panel p = CreatePage();
            AddHeading(p, "🎨 Palettes Chromatiques & Thèmes Verre");

            AddLabel(p, "Thème chromatique unifié pour toute la suite :", 28, 65);
            ComboBox thCombo = CreateCombo(p, 28, 90, 420, new string[] {
                "Prisme Éther (Cyan Glacé / Lavande / Rose)",
                "Verre Givre & Argent Pur",
                "Océan Polaire (Azur / Cyan / Indigo)",
                "Émeraude & Menthe Givrée",
                "Velours Rubis & Corail",
                "Ambre Solaire & Champagne",
                "Aurore Boréale Nocturne",
                "Brume Ultraviolette",
                "Fleur de Sakura Pastel",
                "Monochrome Obsidienne & Titane",
                "Spectre Chromatique Continu 360°",
                "Palette Personnalisée"
            }, (int)controller.Settings.Theme);
            thCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Theme = (GlassColorPalette)thCombo.SelectedIndex; controller.Settings.Save(); };

            Button saveBtn = new Button { Text = "Enregistrer la Configuration", Location = new Point(28, 180), Size = new Size(240, 40), BackColor = Color.FromArgb(40, 120, 230), FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold) };
            saveBtn.Click += (s, e) => { controller.Settings.Save(); MessageBox.Show("Configuration enregistrée avec succès !", "iSuite Studio", MessageBoxButtons.OK, MessageBoxIcon.Information); };
            p.Controls.Add(saveBtn);

            return p;
        }

        private void AddHeading(Panel p, string text)
        {
            Label l = new Label { Text = text, Location = new Point(24, 20), AutoSize = true, Font = new Font("Segoe UI Semibold", 13.5f, FontStyle.Bold), ForeColor = Color.White };
            p.Controls.Add(l);
        }

        private void AddLabel(Panel p, string text, int x, int y)
        {
            Label l = new Label { Text = text, Location = new Point(x, y), AutoSize = true, Font = new Font("Segoe UI", 9.2f), ForeColor = Color.FromArgb(180, 195, 220) };
            p.Controls.Add(l);
        }

        private CheckBox CreateCheckBox(Panel p, string text, int x, int y, bool isChecked)
        {
            CheckBox cb = new CheckBox { Text = text, Location = new Point(x, y), AutoSize = true, Checked = isChecked, Font = new Font("Segoe UI", 9.2f), ForeColor = Color.White };
            p.Controls.Add(cb);
            return cb;
        }

        private TrackBar CreateTrack(Panel p, int x, int y, int w, int min, int max, int val)
        {
            TrackBar tb = new TrackBar { Location = new Point(x, y), Size = new Size(w, 45), Minimum = min, Maximum = max, Value = Math.Max(min, Math.Min(max, val)) };
            p.Controls.Add(tb);
            return tb;
        }

        private ComboBox CreateCombo(Panel p, int x, int y, int w, string[] items, int selIdx)
        {
            ComboBox cb = new ComboBox { Location = new Point(x, y), Size = new Size(w, 30), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.2f) };
            cb.Items.AddRange(items);
            if (selIdx >= 0 && selIdx < cb.Items.Count) cb.SelectedIndex = selIdx;
            p.Controls.Add(cb);
            return cb;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) { e.Cancel = true; Hide(); return; }
            base.OnFormClosing(e);
        }
    }

    // =========================================================================
    // ENTRY POINT
    // =========================================================================
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            NativeMethods.EnablePerMonitorDpiAwareness();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (MasterOverlayController controller = new MasterOverlayController())
            {
                bool startInTray = (args != null && args.Length > 0 && args[0] == "--tray");
                if (!startInTray) controller.ShowStudioWindow();
                Application.Run();
            }
        }
    }
}
