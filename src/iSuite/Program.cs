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
        public bool ReplaceWindowsTaskbar = true; // Hides native Windows Shell_TrayWnd
        public TaskbarStyle BarStyle = TaskbarStyle.FloatingDock;
        public int TaskbarHeight = 58;
        public int TaskbarCornerRadius = 20;
        public int TaskbarOpacity = 242;
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
                sb.AppendLine(string.Format("  \"ReplaceWindowsTaskbar\": {0},", ReplaceWindowsTaskbar ? "true" : "false"));
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
                s.ReplaceWindowsTaskbar = ReadBool(json, "ReplaceWindowsTaskbar", s.ReplaceWindowsTaskbar);
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
    // GLASS COLOR ENGINE & LIQUID SHADERS
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

        // Hyper-realistic Liquid Glass renderer with refraction, caustic rim highlight, and inner glow
        public static void DrawFrostedGlassSurface(Graphics g, RectangleF rect, float cornerRadius, Color baseDarkTint, Color accentTint, int glassOpacity = 238, float borderAlphaRatio = 1.0f)
        {
            if (rect.Width <= 2f || rect.Height <= 2f) return;
            using (GraphicsPath path = CreateRoundedRect(rect, cornerRadius))
            {
                // 1. Deep frosted glass tint
                using (SolidBrush baseBrush = new SolidBrush(Color.FromArgb(glassOpacity, baseDarkTint)))
                    g.FillPath(baseBrush, path);

                // 2. Liquid Glass caustic light refraction sheen
                using (LinearGradientBrush sheen = new LinearGradientBrush(rect, Color.FromArgb((int)(34 * borderAlphaRatio), 255, 255, 255), Color.FromArgb((int)(14 * borderAlphaRatio), accentTint), 45f))
                    g.FillPath(sheen, path);

                // 3. Specular top refraction line
                RectangleF topRefract = new RectangleF(rect.X + 2, rect.Y + 1, rect.Width - 4, rect.Height * 0.45f);
                if (topRefract.Width > 4 && topRefract.Height > 2)
                {
                    using (LinearGradientBrush topGrad = new LinearGradientBrush(topRefract, Color.FromArgb((int)(40 * borderAlphaRatio), 255, 255, 255), Color.FromArgb(0, 255, 255, 255), LinearGradientMode.Vertical))
                    using (GraphicsPath topPath = CreateRoundedRect(topRefract, cornerRadius * 0.8f))
                    {
                        g.FillPath(topGrad, topPath);
                    }
                }

                // 4. Dynamic glowing chromatic glass rim border
                using (LinearGradientBrush rimGrad = new LinearGradientBrush(rect, Color.FromArgb((int)(95 * borderAlphaRatio), Interpolate(Color.White, accentTint, 0.4f)), Color.FromArgb((int)(32 * borderAlphaRatio), accentTint), 60f))
                using (Pen rimPen = new Pen(rimGrad, 1.25f))
                {
                    g.DrawPath(rimPen, path);
                }
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
    // NATIVE WIN32 INTEROP & REAL TASKBAR REPLACEMENT
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
        public const int WS_EX_APPWINDOW = 0x00040000;
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const uint WS_VISIBLE = 0x10000000;
        public const uint WM_CLOSE = 0x0010;

        public const int SW_HIDE = 0;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_MAXIMIZE = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;
        public const int SW_MINIMIZE = 6;
        public const int SW_RESTORE = 9;

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

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8) return GetWindowLongPtr64(hWnd, nIndex);
            return GetWindowLongPtr32(hWnd, nIndex);
        }

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public static void SetWindowsTaskbarVisible(bool visible)
        {
            try
            {
                IntPtr primary = FindWindow("Shell_TrayWnd", null);
                if (primary != IntPtr.Zero)
                    ShowWindow(primary, visible ? SW_SHOW : SW_HIDE);

                IntPtr secondary = IntPtr.Zero;
                while ((secondary = FindWindowEx(IntPtr.Zero, secondary, "Shell_SecondaryTrayWnd", null)) != IntPtr.Zero)
                {
                    ShowWindow(secondary, visible ? SW_SHOW : SW_HIDE);
                }
            }
            catch { }
        }

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
    // WINDOWS 12 CONCEPT GLASS TASKBAR (REPLACES NATIVE WINDOWS TASKBAR)
    // =========================================================================
    public class GlassTaskbarWidget : IDisposable
    {
        private LayeredOverlayForm taskbarForm;
        private Rectangle barBounds = Rectangle.Empty;

        public event Action OnStartButtonClicked;
        public event Action OnActionCenterClicked;
        public event Action OnSettingsClicked;

        public class PinnedIcon
        {
            public string Glyph;
            public string Tooltip;
            public Action OnClick;
            public Rectangle Rect;
        }

        public class RunningAppInfo
        {
            public IntPtr Hwnd;
            public string Title;
            public bool IsActive;
            public bool IsIconic;
            public Rectangle Rect;
        }

        private List<PinnedIcon> pinnedIcons = new List<PinnedIcon>();
        private List<RunningAppInfo> runningApps = new List<RunningAppInfo>();
        private Rectangle clockRect = Rectangle.Empty;
        private Rectangle actionCenterRect = Rectangle.Empty;

        private int hoveredPinnedIdx = -1;
        private int hoveredRunningIdx = -1;
        private bool isHoverClock = false;
        private bool isHoverActionCenter = false;

        private long lastEnumTicks = 0;

        public GlassTaskbarWidget()
        {
            taskbarForm = new LayeredOverlayForm(false);

            // Pinned Glass Icons
            pinnedIcons.Add(new PinnedIcon { Glyph = "❖", Tooltip = "Menu Démarrer Glass (Windows 12)", OnClick = () => { if (OnStartButtonClicked != null) OnStartButtonClicked(); } });
            pinnedIcons.Add(new PinnedIcon { Glyph = "📁", Tooltip = "Explorateur de fichiers", OnClick = () => { try { Process.Start("explorer.exe"); } catch { } } });
            pinnedIcons.Add(new PinnedIcon { Glyph = "🌐", Tooltip = "Navigateur Web", OnClick = () => { try { Process.Start("https://google.com"); } catch { } } });
            pinnedIcons.Add(new PinnedIcon { Glyph = "💻", Tooltip = "Terminal PowerShell", OnClick = () => { try { Process.Start("powershell.exe"); } catch { } } });
            pinnedIcons.Add(new PinnedIcon { Glyph = "⚙️", Tooltip = "iSuite Master Studio", OnClick = () => { if (OnSettingsClicked != null) OnSettingsClicked(); } });

            taskbarForm.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (hoveredPinnedIdx >= 0 && hoveredPinnedIdx < pinnedIcons.Count)
                    {
                        pinnedIcons[hoveredPinnedIdx].OnClick();
                        return;
                    }
                    if (isHoverActionCenter)
                    {
                        if (OnActionCenterClicked != null) OnActionCenterClicked();
                        return;
                    }
                    if (hoveredRunningIdx >= 0 && hoveredRunningIdx < runningApps.Count)
                    {
                        RunningAppInfo app = runningApps[hoveredRunningIdx];
                        if (app.IsActive && !app.IsIconic)
                        {
                            NativeMethods.ShowWindow(app.Hwnd, NativeMethods.SW_MINIMIZE);
                        }
                        else
                        {
                            NativeMethods.ShowWindow(app.Hwnd, NativeMethods.SW_RESTORE);
                            NativeMethods.SetForegroundWindow(app.Hwnd);
                        }
                        return;
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (hoveredRunningIdx >= 0 && hoveredRunningIdx < runningApps.Count)
                    {
                        RunningAppInfo app = runningApps[hoveredRunningIdx];
                        ShowAppContextMenu(app);
                    }
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    if (hoveredRunningIdx >= 0 && hoveredRunningIdx < runningApps.Count)
                    {
                        RunningAppInfo app = runningApps[hoveredRunningIdx];
                        NativeMethods.PostMessage(app.Hwnd, NativeMethods.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    }
                }
            };

            taskbarForm.Show();
        }

        private void ShowAppContextMenu(RunningAppInfo app)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem itemTitle = new ToolStripMenuItem(app.Title);
            itemTitle.Enabled = false;
            itemTitle.Font = new Font(itemTitle.Font, FontStyle.Bold);

            ToolStripMenuItem switchItem = new ToolStripMenuItem("Basculer vers cette fenêtre");
            switchItem.Click += (s, e) => { NativeMethods.ShowWindow(app.Hwnd, NativeMethods.SW_RESTORE); NativeMethods.SetForegroundWindow(app.Hwnd); };

            ToolStripMenuItem minItem = new ToolStripMenuItem("Réduire");
            minItem.Click += (s, e) => NativeMethods.ShowWindow(app.Hwnd, NativeMethods.SW_MINIMIZE);

            ToolStripMenuItem maxItem = new ToolStripMenuItem("Agrandir");
            maxItem.Click += (s, e) => NativeMethods.ShowWindow(app.Hwnd, NativeMethods.SW_MAXIMIZE);

            ToolStripMenuItem closeItem = new ToolStripMenuItem("Fermer la fenêtre");
            closeItem.ForeColor = Color.Crimson;
            closeItem.Click += (s, e) => NativeMethods.PostMessage(app.Hwnd, NativeMethods.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

            menu.Items.Add(itemTitle);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(switchItem);
            menu.Items.Add(minItem);
            menu.Items.Add(maxItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(closeItem);

            menu.Show(Cursor.Position);
        }

        private void RefreshRunningApps()
        {
            long now = DateTime.UtcNow.Ticks;
            if (now - lastEnumTicks < 4000000) return; // refresh every 400ms
            lastEnumTicks = now;

            List<RunningAppInfo> list = new List<RunningAppInfo>();
            IntPtr foreground = NativeMethods.GetForegroundWindow();

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

                // Exclude system shells, wallpapers, and iSuite overlays
                if (cls == "Progman" || cls == "WorkerW" || cls == "Shell_TrayWnd" || cls == "Shell_SecondaryTrayWnd" || cls == "Windows.UI.Core.CoreWindow" || cls.Contains("LayeredOverlayForm") || cls == "ComboLBox") return true;

                long exStyle = NativeMethods.GetWindowLongPtr(hWnd, NativeMethods.GWL_EXSTYLE).ToInt64();
                if ((exStyle & NativeMethods.WS_EX_TOOLWINDOW) != 0) return true;

                IntPtr owner = NativeMethods.GetWindow(hWnd, 4 /* GW_OWNER */);
                if (owner != IntPtr.Zero && (exStyle & NativeMethods.WS_EX_APPWINDOW) == 0) return true;

                list.Add(new RunningAppInfo
                {
                    Hwnd = hWnd,
                    Title = title,
                    IsActive = (hWnd == foreground),
                    IsIconic = NativeMethods.IsIconic(hWnd)
                });

                return true;
            }, IntPtr.Zero);

            runningApps = list;
        }

        public void UpdateAndRender(SuiteSettings settings, Rectangle screenBounds, Point cursorPt, Color[] palette, float globalTime)
        {
            if (!settings.TaskbarEnabled) { taskbarForm.Hide(); return; }

            RefreshRunningApps();

            int barH = settings.TaskbarHeight;
            int pinnedWidth = pinnedIcons.Count * 46;
            int runningWidth = Math.Min(600, runningApps.Count * 115);
            int trayWidth = 140;
            int totalContentWidth = pinnedWidth + 24 + runningWidth + trayWidth + 40;

            int barW = (settings.BarStyle == TaskbarStyle.FloatingDock) 
                ? Math.Max(560, Math.Min(screenBounds.Width - 60, totalContentWidth)) 
                : (screenBounds.Width - 32);

            int barX = screenBounds.Left + (screenBounds.Width - barW) / 2;
            int barY = screenBounds.Bottom - barH - (settings.BarStyle == TaskbarStyle.FloatingDock ? 14 : 0);

            barBounds = new Rectangle(barX, barY, barW, barH);

            Point relMouse = new Point(cursorPt.X - barX, cursorPt.Y - barY);
            bool isInside = barBounds.Contains(cursorPt);

            hoveredPinnedIdx = -1;
            hoveredRunningIdx = -1;
            isHoverClock = false;
            isHoverActionCenter = false;

            using (Bitmap bmp = new Bitmap(barW, barH, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.Clear(Color.Transparent);

                RectangleF pill = new RectangleF(0, 0, barW, barH);
                Color accent = palette[0];
                GlassThemeEngine.DrawFrostedGlassSurface(g, pill, settings.TaskbarCornerRadius, Color.FromArgb(10, 12, 18), accent, settings.TaskbarOpacity, 0.95f);

                // --- 1. PINNED ICONS ---
                int curX = 14;
                for (int i = 0; i < pinnedIcons.Count; i++)
                {
                    Rectangle ir = new Rectangle(curX, (barH - 40) / 2, 40, 40);
                    pinnedIcons[i].Rect = ir;
                    bool hov = isInside && ir.Contains(relMouse);
                    if (hov) hoveredPinnedIdx = i;

                    if (hov)
                    {
                        using (GraphicsPath ip = GlassThemeEngine.CreateRoundedRect(ir, 10f))
                        using (SolidBrush ib = new SolidBrush(Color.FromArgb(42, 255, 255, 255)))
                        using (Pen ipen = new Pen(Color.FromArgb(90, 255, 255, 255), 1.0f))
                        {
                            g.FillPath(ib, ip);
                            g.DrawPath(ipen, ip);
                        }
                    }

                    using (Font f = new Font("Segoe UI Semibold", 13.5f, FontStyle.Bold))
                    using (SolidBrush b = new SolidBrush(hov ? Color.White : Color.FromArgb(215, 225, 245)))
                    {
                        SizeF sz = g.MeasureString(pinnedIcons[i].Glyph, f);
                        g.DrawString(pinnedIcons[i].Glyph, f, b, ir.X + (ir.Width - sz.Width) / 2f, ir.Y + (ir.Height - sz.Height) / 2f);
                    }

                    // Windows 12 Start button distinctive cyan neon aura
                    if (i == 0)
                    {
                        using (SolidBrush dot = new SolidBrush(accent))
                            g.FillEllipse(dot, ir.X + ir.Width / 2f - 2f, ir.Bottom - 3f, 4f, 4f);
                    }

                    curX += 44;
                }

                // Glass Divider Line
                using (Pen divPen = new Pen(Color.FromArgb(40, 255, 255, 255), 1.2f))
                {
                    g.DrawLine(divPen, curX + 6, barH * 0.22f, curX + 6, barH * 0.78f);
                }
                curX += 16;

                // --- 2. RUNNING APPLICATIONS CHIPS ---
                if (settings.TaskbarShowRunningApps && runningApps.Count > 0)
                {
                    int appChipWidth = 110;
                    for (int i = 0; i < runningApps.Count; i++)
                    {
                        if (curX + appChipWidth > barW - trayWidth - 10) break; // Don't overflow tray

                        RunningAppInfo app = runningApps[i];
                        Rectangle ar = new Rectangle(curX, (barH - 36) / 2, appChipWidth, 36);
                        app.Rect = ar;
                        bool hov = isInside && ar.Contains(relMouse);
                        if (hov) hoveredRunningIdx = i;

                        // Chip background
                        using (GraphicsPath ap = GlassThemeEngine.CreateRoundedRect(ar, 8f))
                        {
                            Color chipBg = app.IsActive 
                                ? Color.FromArgb(60, accent) 
                                : (hov ? Color.FromArgb(45, 255, 255, 255) : Color.FromArgb(18, 255, 255, 255));
                            using (SolidBrush ab = new SolidBrush(chipBg))
                                g.FillPath(ab, ap);

                            if (app.IsActive)
                            {
                                using (Pen apen = new Pen(Color.FromArgb(140, accent), 1.2f))
                                    g.DrawPath(apen, ap);
                            }
                        }

                        // App Initial Badge / Icon
                        Rectangle badgeRect = new Rectangle(ar.X + 6, ar.Y + 7, 22, 22);
                        using (GraphicsPath bp = GlassThemeEngine.CreateRoundedRect(badgeRect, 6f))
                        using (SolidBrush bb = new SolidBrush(Color.FromArgb(80, 20, 26, 40)))
                        {
                            g.FillPath(bb, bp);
                        }

                        string initial = string.IsNullOrEmpty(app.Title) ? "?" : app.Title.Substring(0, 1).ToUpper();
                        using (Font bf = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold))
                        using (SolidBrush txtB = new SolidBrush(app.IsActive ? accent : Color.White))
                        {
                            SizeF bsz = g.MeasureString(initial, bf);
                            g.DrawString(initial, bf, txtB, badgeRect.X + (badgeRect.Width - bsz.Width) / 2f, badgeRect.Y + (badgeRect.Height - bsz.Height) / 2f);
                        }

                        // App Title (Truncated)
                        string dispTitle = app.Title;
                        if (dispTitle.Length > 9) dispTitle = dispTitle.Substring(0, 8) + "…";
                        using (Font tf = new Font("Segoe UI", 8.5f))
                        using (SolidBrush tb = new SolidBrush(app.IsActive ? Color.White : Color.FromArgb(200, 215, 235)))
                        {
                            g.DrawString(dispTitle, tf, tb, ar.X + 32, ar.Y + 9);
                        }

                        // Active Indicator Bar underneath
                        if (app.IsActive)
                        {
                            using (SolidBrush indBrush = new SolidBrush(accent))
                                g.FillRoundedRectangle(indBrush, ar.X + 28, ar.Bottom - 3, 34, 2, 1);
                        }
                        else
                        {
                            using (SolidBrush dotBrush = new SolidBrush(Color.FromArgb(120, 255, 255, 255)))
                                g.FillEllipse(dotBrush, ar.X + ar.Width / 2f - 2f, ar.Bottom - 3f, 4f, 4f);
                        }

                        curX += appChipWidth + 6;
                    }
                }

                // --- 3. SYSTEM TRAY & ACTION CENTER & CLOCK ---
                int rightPos = barW - 14;

                // Clock
                if (settings.TaskbarShowClock)
                {
                    string timeStr = DateTime.Now.ToString("HH:mm");
                    string dateStr = DateTime.Now.ToString("dd/MM");
                    clockRect = new Rectangle(rightPos - 60, (barH - 36) / 2, 60, 36);
                    isHoverClock = isInside && clockRect.Contains(relMouse);

                    using (Font tf = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold))
                    using (Font df = new Font("Segoe UI", 7.8f))
                    using (SolidBrush tb = new SolidBrush(Color.White))
                    using (SolidBrush db = new SolidBrush(Color.FromArgb(160, 175, 205)))
                    {
                        g.DrawString(timeStr, tf, tb, clockRect.X + 8, clockRect.Y + 2);
                        g.DrawString(dateStr, df, db, clockRect.X + 11, clockRect.Y + 18);
                    }

                    rightPos -= 68;
                }

                // Action Center Bell
                if (settings.TaskbarShowActionCenterBtn)
                {
                    actionCenterRect = new Rectangle(rightPos - 36, (barH - 36) / 2, 36, 36);
                    isHoverActionCenter = isInside && actionCenterRect.Contains(relMouse);

                    if (isHoverActionCenter)
                    {
                        using (GraphicsPath ap = GlassThemeEngine.CreateRoundedRect(actionCenterRect, 8f))
                        using (SolidBrush ab = new SolidBrush(Color.FromArgb(42, 255, 255, 255)))
                            g.FillPath(ab, ap);
                    }

                    using (Font af = new Font("Segoe UI Semibold", 11.5f, FontStyle.Bold))
                    using (SolidBrush ab = new SolidBrush(isHoverActionCenter ? Color.White : Color.FromArgb(215, 225, 245)))
                    {
                        SizeF asz = g.MeasureString("🔔", af);
                        g.DrawString("🔔", af, ab, actionCenterRect.X + (actionCenterRect.Width - asz.Width) / 2f, actionCenterRect.Y + (actionCenterRect.Height - asz.Height) / 2f);
                    }

                    rightPos -= 42;
                }

                // Status Icons: WiFi + Volume
                using (Font sf = new Font("Segoe UI", 9.5f))
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(190, 205, 230)))
                {
                    g.DrawString("📶", sf, sb, rightPos - 22, (barH - 20) / 2);
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

    // Extension method helper for rounded rectangle fill
    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, float x, float y, float w, float h, float r)
        {
            using (GraphicsPath p = GlassThemeEngine.CreateRoundedRect(new RectangleF(x, y, w, h), r))
                g.FillPath(brush, p);
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
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(160, 175, 205)))
            {
                g.DrawString("🔍  Rechercher des applications, fichiers, paramètres...", sf, sb, 42, 34);
            }

            // Pinned Section Header
            using (Font hf = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold))
            using (SolidBrush hb = new SolidBrush(Color.White))
            {
                g.DrawString("Applications Épinglées (Windows 12 Glass)", hf, hb, 30, 86);
            }

            // App Grid (6 pinned modern glass cards)
            string[] appNames = new string[] { "iSuite Studio", "Explorateur", "Terminal", "Paramètres", "iStore", "Apple Music" };
            string[] appIcons = new string[] { "⚙️", "📁", "💻", "🛠️", "🛍️", "🎵" };

            int startX = 30;
            int startY = 118;
            int cardW = 145;
            int cardH = 78;

            for (int i = 0; i < 6; i++)
            {
                int col = i % 3;
                int row = i / 3;
                RectangleF cardRect = new RectangleF(startX + col * (cardW + 20), startY + row * (cardH + 16), cardW, cardH);

                using (GraphicsPath cp = GlassThemeEngine.CreateRoundedRect(cardRect, 12f))
                using (SolidBrush cb = new SolidBrush(Color.FromArgb(16, 255, 255, 255)))
                using (Pen cpen = new Pen(Color.FromArgb(32, 255, 255, 255), 1.0f))
                {
                    g.FillPath(cb, cp);
                    g.DrawPath(cpen, cp);
                }

                using (Font icf = new Font("Segoe UI", 16f))
                using (SolidBrush icb = new SolidBrush(Color.White))
                {
                    g.DrawString(appIcons[i], icf, icb, cardRect.X + 16, cardRect.Y + 12);
                }

                using (Font nmf = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold))
                using (SolidBrush nmb = new SolidBrush(Color.FromArgb(220, 230, 248)))
                {
                    g.DrawString(appNames[i], nmf, nmb, cardRect.X + 16, cardRect.Y + 48);
                }
            }

            // Recommended Section Header
            using (Font hf = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold))
            using (SolidBrush hb = new SolidBrush(Color.White))
            {
                g.DrawString("Recommandés & Écosystème Connecté", hf, hb, 30, 310);
            }

            string[] recents = new string[] {
                "iVolume • HUD Audio & Lecteur Liquid Glass",
                "iAltTab • Mission Control & Switcher de Fenêtres",
                "iWidgets • Tableau de bord widgets flottants"
            };

            for (int i = 0; i < recents.Length; i++)
            {
                RectangleF rr = new RectangleF(30, 342 + i * 44, Width - 60, 36);
                using (GraphicsPath rp = GlassThemeEngine.CreateRoundedRect(rr, 8f))
                using (SolidBrush rb = new SolidBrush(Color.FromArgb(12, 255, 255, 255)))
                    g.FillPath(rb, rp);

                using (Font rf = new Font("Segoe UI", 8.8f))
                using (SolidBrush rtxtb = new SolidBrush(Color.FromArgb(200, 215, 240)))
                {
                    g.DrawString(recents[i], rf, rtxtb, rr.X + 14, rr.Y + 9);
                }
            }

            // Bottom Profile / Power Bar
            RectangleF bottomBar = new RectangleF(0, Height - 62, Width, 62);
            using (SolidBrush bbb = new SolidBrush(Color.FromArgb(30, 8, 10, 16)))
                g.FillRectangle(bbb, bottomBar);

            using (Font pf = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold))
            using (SolidBrush pb = new SolidBrush(Color.White))
            {
                g.DrawString("👤 Utilisateur Windows 12", pf, pb, 30, Height - 40);
                g.DrawString("⏻ Arrêter", pf, pb, Width - 100, Height - 40);
            }
        }
    }

    // =========================================================================
    // WINDOWS 12 CONCEPT GLASS ACTION & NOTIFICATION CENTER
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
            Size = new Size(settings.ActionCenterWidth, 600);
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
                "iSuite • La barre Windows native est remplacée",
                "Spotify • En cours de lecture sur vos enceintes",
                "Système • Mode Windows 12 Concept Glass 60 FPS"
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
                using (SolidBrush ntb = new SolidBrush(Color.FromArgb(210, 225, 245)))
                {
                    g.DrawString(notifs[i], nf, ntb, nr.X + 14, nr.Y + 14);
                }
            }
        }

        private void DrawTrackSlider(Graphics g, float x, float y, float w, float ratio, Color accent)
        {
            RectangleF track = new RectangleF(x, y, w, 18);
            using (GraphicsPath tp = GlassThemeEngine.CreateRoundedRect(track, 9f))
            using (SolidBrush tb = new SolidBrush(Color.FromArgb(25, 255, 255, 255)))
            {
                g.FillPath(tb, tp);
            }

            float fillW = Math.Max(18f, w * ratio);
            RectangleF fillRect = new RectangleF(x, y, fillW, 18);
            using (GraphicsPath fp = GlassThemeEngine.CreateRoundedRect(fillRect, 9f))
            using (SolidBrush fb = new SolidBrush(accent))
            {
                g.FillPath(fb, fp);
            }

            // Knob
            using (SolidBrush kb = new SolidBrush(Color.White))
                g.FillEllipse(kb, x + fillW - 16, y + 1, 16, 16);
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
        private NotifyIcon trayIcon;
        private MasterStudioForm studioForm;
        private long startTicks = DateTime.UtcNow.Ticks;

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

            // Safe replacement of Windows native taskbar
            ApplyWindowsTaskbarReplacement();

            // Always restore native taskbars on app exit or crash
            Application.ApplicationExit += (s, e) => NativeMethods.SetWindowsTaskbarVisible(true);
            AppDomain.CurrentDomain.ProcessExit += (s, e) => NativeMethods.SetWindowsTaskbarVisible(true);

            SetupTray();

            tickTimer = new System.Windows.Forms.Timer { Interval = 16 };
            tickTimer.Tick += OnTick;
            tickTimer.Start();
        }

        public void ApplyWindowsTaskbarReplacement()
        {
            if (Settings.TaskbarEnabled && Settings.ReplaceWindowsTaskbar)
            {
                NativeMethods.SetWindowsTaskbarVisible(false);
            }
            else
            {
                NativeMethods.SetWindowsTaskbarVisible(true);
            }
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
            taskbarToggle.Click += (s, e) => 
            { 
                Settings.TaskbarEnabled = !Settings.TaskbarEnabled; 
                taskbarToggle.Checked = Settings.TaskbarEnabled; 
                ApplyWindowsTaskbarReplacement();
                Settings.Save(); 
            };

            ToolStripMenuItem replaceWinToggle = new ToolStripMenuItem("Remplacer la barre Windows native");
            replaceWinToggle.Checked = Settings.ReplaceWindowsTaskbar;
            replaceWinToggle.Click += (s, e) =>
            {
                Settings.ReplaceWindowsTaskbar = !Settings.ReplaceWindowsTaskbar;
                replaceWinToggle.Checked = Settings.ReplaceWindowsTaskbar;
                ApplyWindowsTaskbarReplacement();
                Settings.Save();
            };

            ToolStripMenuItem restoreWinItem = new ToolStripMenuItem("Restaurer la barre des tâches Windows (Failsafe)");
            restoreWinItem.Click += (s, e) => NativeMethods.SetWindowsTaskbarVisible(true);

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Quitter iSuite");
            exitItem.Click += (s, e) => 
            { 
                NativeMethods.SetWindowsTaskbarVisible(true);
                trayIcon.Visible = false; 
                Application.Exit(); 
            };

            menu.Items.Add(studioItem);
            menu.Items.Add(taskbarToggle);
            menu.Items.Add(replaceWinToggle);
            menu.Items.Add(restoreWinItem);
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
            NativeMethods.SetWindowsTaskbarVisible(true);
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
                using (SolidBrush tb = new SolidBrush(Color.White))
                {
                    g.DrawString("iSuite", tf, tb, 26, 12);
                }

                using (Font sf = new Font("Segoe UI", 9.5f))
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(160, 175, 205)))
                {
                    g.DrawString("Écosystème Unifié de Personnalisation Windows 12 Glass", sf, sb, 28, 44);
                }

                // Decorative chromatic glass emblem badge
                RectangleF badge = new RectangleF(Width - 180, 20, 140, 34);
                GlassThemeEngine.DrawFrostedGlassSurface(g, badge, 10f, Color.FromArgb(20, 24, 38), pal[0], 220, 1f);
                using (Font bf = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold))
                using (SolidBrush bb = new SolidBrush(Color.White))
                {
                    g.DrawString("Liquid Glass v1.0.0", bf, bb, badge.X + 16, badge.Y + 8);
                }
            };
            Controls.Add(header);

            // Left Navigation Sidebar
            Panel sidebar = new Panel { Dock = DockStyle.Left, Width = 230, BackColor = Color.FromArgb(13, 15, 23) };
            Controls.Add(sidebar);

            // Main Content Area
            Panel contentContainer = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(11, 13, 20) };
            Controls.Add(contentContainer);

            string[] tabs = new string[] {
                "❖ Barre & Shell",
                "🔲 Arrondis d'Écran",
                "🌉 Passerelle Écrans",
                "💊 Capsule Dynamique",
                "✨ Mascottes Célestes",
                "🎨 Thèmes & Verre",
                "🛍️ Modules & Store"
            };

            for (int i = 0; i < tabs.Length; i++)
            {
                int idx = i;
                Button btn = new Button
                {
                    Text = "  " + tabs[i],
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Top,
                    Height = 48,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = (i == 0) ? Color.White : Color.FromArgb(170, 185, 215),
                    BackColor = (i == 0) ? Color.FromArgb(28, 36, 56) : Color.Transparent,
                    Font = new Font("Segoe UI Semibold", 9.5f, (i == 0) ? FontStyle.Bold : FontStyle.Regular),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (s, e) => SwitchTab(idx);
                sidebar.Controls.Add(btn);
                navButtons[i] = btn;
            }

            // Create Tab Pages
            pages[0] = BuildPageShell();
            pages[1] = BuildPageCorners();
            pages[2] = BuildPageBridge();
            pages[3] = BuildPageIsland();
            pages[4] = BuildPageMascot();
            pages[5] = BuildPageTheme();
            pages[6] = BuildPageStore();

            for (int i = 0; i < 7; i++)
            {
                pages[i].Dock = DockStyle.Fill;
                pages[i].Visible = (i == 0);
                contentContainer.Controls.Add(pages[i]);
            }
        }

        private void SwitchTab(int idx)
        {
            activePage = idx;
            for (int i = 0; i < 7; i++)
            {
                pages[i].Visible = (i == idx);
                navButtons[i].BackColor = (i == idx) ? Color.FromArgb(28, 36, 56) : Color.Transparent;
                navButtons[i].ForeColor = (i == idx) ? Color.White : Color.FromArgb(170, 185, 215);
                navButtons[i].Font = new Font("Segoe UI Semibold", 9.5f, (i == idx) ? FontStyle.Bold : FontStyle.Regular);
            }
        }

        private Panel CreatePage()
        {
            return new Panel { AutoScroll = true, BackColor = Color.FromArgb(11, 13, 20), Padding = new Padding(28) };
        }

        private Panel BuildPageShell()
        {
            Panel p = CreatePage();
            AddHeading(p, "❖ Barre des Tâches & Shell Windows 12 Glass");

            CheckBox enChk = CreateCheckBox(p, "Activer la barre des tâches flottante Liquid Glass", 28, 65, controller.Settings.TaskbarEnabled);
            enChk.CheckedChanged += (s, e) => 
            { 
                controller.Settings.TaskbarEnabled = enChk.Checked; 
                controller.ApplyWindowsTaskbarReplacement();
                controller.Settings.Save(); 
            };

            CheckBox repChk = CreateCheckBox(p, "Remplacer complètement la barre des tâches native de Windows (Shell_TrayWnd)", 28, 100, controller.Settings.ReplaceWindowsTaskbar);
            repChk.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            repChk.ForeColor = Color.FromArgb(110, 215, 255);
            repChk.CheckedChanged += (s, e) =>
            {
                controller.Settings.ReplaceWindowsTaskbar = repChk.Checked;
                controller.ApplyWindowsTaskbarReplacement();
                controller.Settings.Save();
            };

            Button restoreBtn = new Button { Text = "Restaurer la barre native Windows (Failsafe)", Location = new Point(28, 135), Size = new Size(320, 32), BackColor = Color.FromArgb(35, 45, 65), FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9f) };
            restoreBtn.Click += (s, e) => { NativeMethods.SetWindowsTaskbarVisible(true); MessageBox.Show("Barre des tâches native de Windows restaurée !", "iSuite Failsafe", MessageBoxButtons.OK, MessageBoxIcon.Information); };
            p.Controls.Add(restoreBtn);

            CheckBox appsChk = CreateCheckBox(p, "Afficher les applications ouvertes en direct avec bascule et miniature", 28, 180, controller.Settings.TaskbarShowRunningApps);
            appsChk.CheckedChanged += (s, e) => { controller.Settings.TaskbarShowRunningApps = appsChk.Checked; controller.Settings.Save(); };

            CheckBox clkChk = CreateCheckBox(p, "Afficher l'horloge et la date liquide", 28, 215, controller.Settings.TaskbarShowClock);
            clkChk.CheckedChanged += (s, e) => { controller.Settings.TaskbarShowClock = clkChk.Checked; controller.Settings.Save(); };

            AddLabel(p, "Hauteur de la barre :", 28, 260);
            TrackBar hBar = CreateTrack(p, 28, 285, 360, 44, 76, controller.Settings.TaskbarHeight);
            hBar.ValueChanged += (s, e) => { controller.Settings.TaskbarHeight = hBar.Value; controller.Settings.Save(); };

            AddLabel(p, "Rayon des coins arrondis (Glass Squircle) :", 28, 335);
            TrackBar rBar = CreateTrack(p, 28, 360, 360, 10, 32, controller.Settings.TaskbarCornerRadius);
            rBar.ValueChanged += (s, e) => { controller.Settings.TaskbarCornerRadius = rBar.Value; controller.Settings.Save(); };

            AddLabel(p, "Opacité du verre acrylique :", 28, 410);
            TrackBar opBar = CreateTrack(p, 28, 435, 360, 160, 255, controller.Settings.TaskbarOpacity);
            opBar.ValueChanged += (s, e) => { controller.Settings.TaskbarOpacity = opBar.Value; controller.Settings.Save(); };

            return p;
        }

        private Panel BuildPageCorners()
        {
            Panel p = CreatePage();
            AddHeading(p, "🔲 Arrondis d'Écran Matériel Liquid Glass");

            CheckBox enChk = CreateCheckBox(p, "Activer les coins d'écran ultra-doux façon macOS / Pro Display", 28, 65, controller.Settings.CornersEnabled);
            enChk.CheckedChanged += (s, e) => { controller.Settings.CornersEnabled = enChk.Checked; controller.Settings.Save(); };

            AddLabel(p, "Rayon de courbure des coins d'écran :", 28, 110);
            TrackBar rBar = CreateTrack(p, 28, 135, 360, 12, 64, controller.Settings.CornerRadius);
            rBar.ValueChanged += (s, e) => { controller.Settings.CornerRadius = rBar.Value; controller.Settings.Save(); };

            AddLabel(p, "Géométrie de courbe :", 28, 185);
            ComboBox geomCombo = CreateCombo(p, 28, 210, 360, new string[] { "Squircle Hyperbolique iOS (Courbure continue G2)", "Arc Circulaire Standard" }, (int)controller.Settings.Curve);
            geomCombo.SelectedIndexChanged += (s, e) => { controller.Settings.Curve = (CurveGeometry)geomCombo.SelectedIndex; controller.Settings.Save(); };

            CheckBox glowChk = CreateCheckBox(p, "Halo chromatique liquide de bordure d'écran (Bezel Glow)", 28, 260, controller.Settings.CornerBezelGlow);
            glowChk.CheckedChanged += (s, e) => { controller.Settings.CornerBezelGlow = glowChk.Checked; controller.Settings.Save(); };

            return p;
        }

        private Panel BuildPageBridge()
        {
            Panel p = CreatePage();
            AddHeading(p, "🌉 Passerelle Visuelle Fluide Multi-Écrans");

            CheckBox enChk = CreateCheckBox(p, "Activer la passerelle visuelle inter-écrans", 28, 65, controller.Settings.BridgeEnabled);
            enChk.CheckedChanged += (s, e) => { controller.Settings.BridgeEnabled = enChk.Checked; controller.Settings.Save(); };

            AddLabel(p, "Effet de transition du curseur :", 28, 110);
            ComboBox stCombo = CreateCombo(p, 28, 135, 360, new string[] {
                "Morphing Verre Fluide (Goutte de Mercure / Apple)",
                "Pont Liquid Glass Luminescent",
                "Onde Prismatique Boréale",
                "Portail Arc Quantique",
                "Nuée de Poussière Stellaire",
                "Résonance Sonore & Ondes Liquides",
                "Ligne d'Horizon Laser Néon"
            }, (int)controller.Settings.BridgeStyle);
            stCombo.SelectedIndexChanged += (s, e) => { controller.Settings.BridgeStyle = (PortalTransitionStyle)stCombo.SelectedIndex; controller.Settings.Save(); };

            return p;
        }

        private Panel BuildPageIsland()
        {
            Panel p = CreatePage();
            AddHeading(p, "💊 Dynamic Island / Capsule Multimédia");

            CheckBox enChk = CreateCheckBox(p, "Activer la capsule Dynamic Island", 28, 65, controller.Settings.IslandEnabled);
            enChk.CheckedChanged += (s, e) => { controller.Settings.IslandEnabled = enChk.Checked; controller.Settings.Save(); };

            CheckBox expChk = CreateCheckBox(p, "Déployer en carte multimédia au survol de la souris", 28, 105, controller.Settings.IslandExpandOnHover);
            expChk.CheckedChanged += (s, e) => { controller.Settings.IslandExpandOnHover = expChk.Checked; controller.Settings.Save(); };

            AddLabel(p, "Positionnement écran :", 28, 145);
            ComboBox posCombo = CreateCombo(p, 28, 170, 360, new string[] { "Suivre l'écran du curseur actif", "Toujours au sommet de l'écran principal" }, (int)controller.Settings.IslandPlacement);
            posCombo.SelectedIndexChanged += (s, e) => { controller.Settings.IslandPlacement = (CapsuleScreenPlacement)posCombo.SelectedIndex; controller.Settings.Save(); };

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

        private Panel BuildPageStore()
        {
            Panel p = CreatePage();
            AddHeading(p, "🛍️ iStore • Catalogue des Outils de Personnalisation Windows");

            string[] apps = new string[] {
                "iMaker|Studio de création pour concevoir vos propres applications et skins|iMaker.exe",
                "iWidgets|Studio de Widgets Rainmeter Liquid Glass & Gestionnaire de skins|iWidgets.exe",
                "iVolume|HUD Audio & Lecteur Liquid Glass flottant|iVolume.exe",
                "iAltTab|Mission Control & Switcher de fenêtres plein écran|iAltTab.exe",
                "iCorner|Coins d'écran arrondis ultra-doux & Aura luminescente|iCorner.exe",
                "iBridge|Passerelle visuelle et portail fluide multi-écrans|iBridge.exe",
                "iIsland|Capsule multimédia Dynamic Island façon macOS / iOS|iIsland.exe",
                "iHub|Lanceur rapide flottant et switcher d'écosystème|iHub.exe"
            };

            int startY = 65;
            for (int i = 0; i < apps.Length; i++)
            {
                string[] parts = apps[i].Split('|');
                string title = parts[0];
                string desc = parts[1];
                string exe = parts[2];

                Panel item = new Panel { Location = new Point(28, startY + i * 72), Size = new Size(720, 62), BackColor = Color.FromArgb(18, 22, 34) };
                item.Paint += (s, e) =>
                {
                    Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                    RectangleF r = new RectangleF(1, 1, item.Width - 2, item.Height - 2);
                    GlassThemeEngine.DrawFrostedGlassSurface(g, r, 10f, Color.FromArgb(16, 20, 30), Color.FromArgb(70, 180, 255), 230, 0.8f);

                    using (Font tf = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold))
                    using (SolidBrush tb = new SolidBrush(Color.White))
                        g.DrawString(title, tf, tb, 18, 10);

                    using (Font df = new Font("Segoe UI", 8.8f))
                    using (SolidBrush db = new SolidBrush(Color.FromArgb(160, 175, 205)))
                        g.DrawString(desc, df, db, 18, 34);
                };

                Button runBtn = new Button { Text = "Lancer", Location = new Point(610, 14), Size = new Size(95, 34), BackColor = Color.FromArgb(40, 120, 230), FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold) };
                runBtn.Click += (s, e) =>
                {
                    string dir = Path.GetDirectoryName(Application.ExecutablePath);
                    string target = Path.Combine(dir, exe);
                    if (File.Exists(target)) Process.Start(target);
                    else MessageBox.Show("Application disponible dans le dossier dist : " + exe, "iStore", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };
                item.Controls.Add(runBtn);

                p.Controls.Add(item);
            }

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
