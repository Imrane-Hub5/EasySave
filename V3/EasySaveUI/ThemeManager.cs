using System.Windows;
using System.Windows.Media;

namespace EasySaveUI
{
    public static class ThemeManager
    {
        public static bool IsDark { get; private set; } = false;

        public static void Toggle()
        {
            IsDark = !IsDark;
            Apply();
        }

        private static void Apply()
        {
            var r = Application.Current.Resources;

            if (IsDark)
            {
                r["PageBGBrush"]        = Brush(0x0F, 0x11, 0x17);
                r["SurfaceBrush"]       = Brush(0x1C, 0x20, 0x29);
                r["AltRowBrush"]        = Brush(0x18, 0x1D, 0x26);
                r["ColHeaderBGBrush"]   = Brush(0x13, 0x16, 0x1D);
                r["ToolbarBGBrush"]     = Brush(0x1C, 0x20, 0x29);
                r["DividerBrush"]       = Brush(0x2D, 0x37, 0x48);
                r["TextPrimBrush"]      = Brush(0xE2, 0xE8, 0xF0);
                r["TextMutedBrush"]     = Brush(0x94, 0xA3, 0xB8);
                r["RowHoverBrush"]      = Brush(0x25, 0x31, 0x48);
                r["RowSelectedBrush"]   = Brush(0x1A, 0x30, 0x28);
                r["BtnSecBGBrush"]      = Brush(0x2D, 0x37, 0x48);
                r["BtnSecFGBrush"]      = Brush(0xE2, 0xE8, 0xF0);
                r["BtnSecBorderBrush"]  = Brush(0x4A, 0x55, 0x68);
                r["InputBGBrush"]       = Brush(0x1C, 0x20, 0x29);
                r["AccentLightBrush"]   = Brush(0x1A, 0x30, 0x28);
                r["AccentBlueBrush"]    = Brush(0xA9, 0xE1, 0x4D);
                r["LimeAccentBrush"]    = Brush(0xA9, 0xE1, 0x4D);
            }
            else
            {
                r["PageBGBrush"]        = Brush(0xF4, 0xF6, 0xF9);
                r["SurfaceBrush"]       = Brushes.White;
                r["AltRowBrush"]        = Brush(0xFA, 0xFA, 0xFA);
                r["ColHeaderBGBrush"]   = Brush(0xF8, 0xFA, 0xFC);
                r["ToolbarBGBrush"]     = Brushes.White;
                r["DividerBrush"]       = Brush(0xE5, 0xE7, 0xEB);
                r["TextPrimBrush"]      = Brush(0x11, 0x18, 0x27);
                r["TextMutedBrush"]     = Brush(0x6B, 0x72, 0x80);
                r["RowHoverBrush"]      = Brush(0xF0, 0xF7, 0xFF);
                r["RowSelectedBrush"]   = Brush(0xDB, 0xEA, 0xFE);
                r["BtnSecBGBrush"]      = Brushes.White;
                r["BtnSecFGBrush"]      = Brush(0x37, 0x41, 0x51);
                r["BtnSecBorderBrush"]  = Brush(0xD1, 0xD5, 0xDB);
                r["InputBGBrush"]       = Brushes.White;
                r["AccentLightBrush"]   = Brush(0xE3, 0xF2, 0xFD);
                r["AccentBlueBrush"]    = Brush(0x19, 0x76, 0xD2);
                r["LimeAccentBrush"]    = Brush(0x19, 0x76, 0xD2);
                r["RowHoverBrush"]      = Brush(0xF0, 0xF7, 0xFF);
                r["RowSelectedBrush"]   = Brush(0xDB, 0xEA, 0xFE);
            }
        }

        private static SolidColorBrush Brush(byte r, byte g, byte b)
            => new SolidColorBrush(Color.FromRgb(r, g, b));
    }
}
