using System;
using System.Collections.Generic;
using System.Text;

namespace FadeFox.Document.MyXls
{
    ///<summary>
    /// Provides named references to all colors in the standard (BIFF8) Excel
    /// color palette.
    ///</summary>
    public static class Colors
    {
        #region (System?) EGA Colors (don't use - they seem to break Excel's Format dialog)

        /// <summary>EGA Black (use Black instead)</summary>
        public static readonly ColorEx EgaBlack = new ColorEx(0x00, 0x00, 0x00);

        /// <summary>EGA White (use White instead)</summary>
        public static readonly ColorEx EgaWhite = new ColorEx(0xFF, 0xFF, 0xFF);
        
        /// <summary>EGA Red (use Red instead)</summary>
        public static readonly ColorEx EgaRed = new ColorEx(0xFF, 0x00, 0x00);
        
        /// <summary>EGA Green (use Green instead)</summary>
        public static readonly ColorEx EgaGreen = new ColorEx(0x00, 0xFF, 0x00);
        
        /// <summary>EGA Blue (use Blue instead)</summary>
        public static readonly ColorEx EgaBlue = new ColorEx(0x00, 0x00, 0xFF);
        
        /// <summary>EGA Yellow (use Yellow instead)</summary>
        public static readonly ColorEx EgaYellow = new ColorEx(0xFF, 0xFF, 0x00);
        
        /// <summary>EGA Magenta (use Magenta instead)</summary>
        public static readonly ColorEx EgaMagenta = new ColorEx(0xFF, 0x00, 0xFF);
        
        /// <summary>EGA Cyan (use Cyan instead)</summary>
        public static readonly ColorEx EgaCyan = new ColorEx(0x00, 0xFF, 0xFF);

        #endregion

        #region Default Palette Colors

        /// <summary>Default Palette Color Index 0x08 (#000000)</summary>
        public static readonly ColorEx Default08 = new ColorEx(0x00, 0x00, 0x00);
        
        /// <summary>Default Palette Color Index 0x09 (#FFFFFF)</summary>
        public static readonly ColorEx Default09 = new ColorEx(0xFF, 0xFF, 0xFF);
        
        /// <summary>Default Palette Color Index 0x0A (#FF0000)</summary>
        public static readonly ColorEx Default0A = new ColorEx(0xFF, 0x00, 0x00);
        
        /// <summary>Default Palette Color Index 0x0B (#00FF00)</summary>
        public static readonly ColorEx Default0B = new ColorEx(0x00, 0xFF, 0x00);
        
        /// <summary>Default Palette Color Index 0x0C (#0000FF)</summary>
        public static readonly ColorEx Default0C = new ColorEx(0x00, 0x00, 0xFF);
        
        /// <summary>Default Palette Color Index 0x0D (#FFFF00)</summary>
        public static readonly ColorEx Default0D = new ColorEx(0xFF, 0xFF, 0x00);
        
        /// <summary>Default Palette Color Index 0x0E (#FF00FF)</summary>
        public static readonly ColorEx Default0E = new ColorEx(0xFF, 0x00, 0xFF);
        
        /// <summary>Default Palette Color Index 0x0F (#00FFFF)</summary>
        public static readonly ColorEx Default0F = new ColorEx(0x00, 0xFF, 0xFF);
        
        /// <summary>Default Palette Color Index 0x10 (#800000)</summary>
        public static readonly ColorEx Default10 = new ColorEx(0x80, 0x00, 0x00);
        
        /// <summary>Default Palette Color Index 0x11 (#008000)</summary>
        public static readonly ColorEx Default11 = new ColorEx(0x00, 0x80, 0x00);
        
        /// <summary>Default Palette Color Index 0x12 (#000080)</summary>
        public static readonly ColorEx Default12 = new ColorEx(0x00, 0x00, 0x80);
        
        /// <summary>Default Palette Color Index 0x13 (#808000)</summary>
        public static readonly ColorEx Default13 = new ColorEx(0x80, 0x80, 0x00);
        
        /// <summary>Default Palette Color Index 0x14 (#800080)</summary>
        public static readonly ColorEx Default14 = new ColorEx(0x80, 0x00, 0x80);
        
        /// <summary>Default Palette Color Index 0x15 (#008080)</summary>
        public static readonly ColorEx Default15 = new ColorEx(0x00, 0x80, 0x80);
        
        /// <summary>Default Palette Color Index 0x16 (#C0C0C0)</summary>
        public static readonly ColorEx Default16 = new ColorEx(0xC0, 0xC0, 0xC0);
        
        /// <summary>Default Palette Color Index 0x17 (#808080)</summary>
        public static readonly ColorEx Default17 = new ColorEx(0x80, 0x80, 0x80);
        
        /// <summary>Default Palette Color Index 0x18 (#9999FF)</summary>
        public static readonly ColorEx Default18 = new ColorEx(0x99, 0x99, 0xFF);
        
        /// <summary>Default Palette Color Index 0x19 (#993366)</summary>
        public static readonly ColorEx Default19 = new ColorEx(0x99, 0x33, 0x66);
        
        /// <summary>Default Palette Color Index 0x1A (#FFFFCC)</summary>
        public static readonly ColorEx Default1A = new ColorEx(0xFF, 0xFF, 0xCC);
        
        /// <summary>Default Palette Color Index 0x1B (#CCFFFF)</summary>
        public static readonly ColorEx Default1B = new ColorEx(0xCC, 0xFF, 0xFF);
        
        /// <summary>Default Palette Color Index 0x1C (#660066)</summary>
        public static readonly ColorEx Default1C = new ColorEx(0x66, 0x00, 0x66);
        
        /// <summary>Default Palette Color Index 0x1D (#FF8080)</summary>
        public static readonly ColorEx Default1D = new ColorEx(0xFF, 0x80, 0x80);
        
        /// <summary>Default Palette Color Index 0x1E (#0066CC)</summary>
        public static readonly ColorEx Default1E = new ColorEx(0x00, 0x66, 0xCC);
        
        /// <summary>Default Palette Color Index 0x1F (#CCCCFF)</summary>
        public static readonly ColorEx Default1F = new ColorEx(0xCC, 0xCC, 0xFF);
        
        /// <summary>Default Palette Color Index 0x20 (#000080)</summary>
        public static readonly ColorEx Default20 = new ColorEx(0x00, 0x00, 0x80);
        
        /// <summary>Default Palette Color Index 0x21 (#FF00FF)</summary>
        public static readonly ColorEx Default21 = new ColorEx(0xFF, 0x00, 0xFF);
        
        /// <summary>Default Palette Color Index 0x22 (#FFFF00)</summary>
        public static readonly ColorEx Default22 = new ColorEx(0xFF, 0xFF, 0x00);
        
        /// <summary>Default Palette Color Index 0x23 (#00FFFF)</summary>
        public static readonly ColorEx Default23 = new ColorEx(0x00, 0xFF, 0xFF);
        
        /// <summary>Default Palette Color Index 0x24 (#800080)</summary>
        public static readonly ColorEx Default24 = new ColorEx(0x80, 0x00, 0x80);
        
        /// <summary>Default Palette Color Index 0x25 (#800000)</summary>
        public static readonly ColorEx Default25 = new ColorEx(0x80, 0x00, 0x00);
        
        /// <summary>Default Palette Color Index 0x26 (#008080)</summary>
        public static readonly ColorEx Default26 = new ColorEx(0x00, 0x80, 0x80);
        
        /// <summary>Default Palette Color Index 0x27 (#0000FF)</summary>
        public static readonly ColorEx Default27 = new ColorEx(0x00, 0x00, 0xFF);
        
        /// <summary>Default Palette Color Index 0x28 (#00CCFF)</summary>
        public static readonly ColorEx Default28 = new ColorEx(0x00, 0xCC, 0xFF);
        
        /// <summary>Default Palette Color Index 0x29 (#CCFFFF)</summary>
        public static readonly ColorEx Default29 = new ColorEx(0xCC, 0xFF, 0xFF);
        
        /// <summary>Default Palette Color Index 0x2A (#CCFFCC)</summary>
        public static readonly ColorEx Default2A = new ColorEx(0xCC, 0xFF, 0xCC);
        
        /// <summary>Default Palette Color Index 0x2B (#FFFF99)</summary>
        public static readonly ColorEx Default2B = new ColorEx(0xFF, 0xFF, 0x99);
        
        /// <summary>Default Palette Color Index 0x2C (#99CCFF)</summary>
        public static readonly ColorEx Default2C = new ColorEx(0x99, 0xCC, 0xFF);
        
        /// <summary>Default Palette Color Index 0x2D (#FF99CC)</summary>
        public static readonly ColorEx Default2D = new ColorEx(0xFF, 0x99, 0xCC);
        
        /// <summary>Default Palette Color Index 0x2E (#CC99FF)</summary>
        public static readonly ColorEx Default2E = new ColorEx(0xCC, 0x99, 0xFF);
        
        /// <summary>Default Palette Color Index 0x2F (#FFCC99)</summary>
        public static readonly ColorEx Default2F = new ColorEx(0xFF, 0xCC, 0x99);
        
        /// <summary>Default Palette Color Index 0x30 (#3366FF)</summary>
        public static readonly ColorEx Default30 = new ColorEx(0x33, 0x66, 0xFF);
        
        /// <summary>Default Palette Color Index 0x31 (#33CCCC)</summary>
        public static readonly ColorEx Default31 = new ColorEx(0x33, 0xCC, 0xCC);
        
        /// <summary>Default Palette Color Index 0x32 (#99CC00)</summary>
        public static readonly ColorEx Default32 = new ColorEx(0x99, 0xCC, 0x00);
        
        /// <summary>Default Palette Color Index 0x33 (#FFCC00)</summary>
        public static readonly ColorEx Default33 = new ColorEx(0xFF, 0xCC, 0x00);
        
        /// <summary>Default Palette Color Index 0x34 (#FF9900)</summary>
        public static readonly ColorEx Default34 = new ColorEx(0xFF, 0x99, 0x00);
        
        /// <summary>Default Palette Color Index 0x35 (#FF6600)</summary>
        public static readonly ColorEx Default35 = new ColorEx(0xFF, 0x66, 0x00);
        
        /// <summary>Default Palette Color Index 0x36 (#666699)</summary>
        public static readonly ColorEx Default36 = new ColorEx(0x66, 0x66, 0x99);
        
        /// <summary>Default Palette Color Index 0x37 (#969696)</summary>
        public static readonly ColorEx Default37 = new ColorEx(0x96, 0x96, 0x96);
        
        /// <summary>Default Palette Color Index 0x38 (#003366)</summary>
        public static readonly ColorEx Default38 = new ColorEx(0x00, 0x33, 0x66);
        
        /// <summary>Default Palette Color Index 0x39 (#339966)</summary>
        public static readonly ColorEx Default39 = new ColorEx(0x33, 0x99, 0x66);
        
        /// <summary>Default Palette Color Index 0x3A (#003300)</summary>
        public static readonly ColorEx Default3A = new ColorEx(0x00, 0x33, 0x00);
        
        /// <summary>Default Palette Color Index 0x3B (#333300)</summary>
        public static readonly ColorEx Default3B = new ColorEx(0x33, 0x33, 0x00);
        
        /// <summary>Default Palette Color Index 0x3C (#993300)</summary>
        public static readonly ColorEx Default3C = new ColorEx(0x99, 0x33, 0x00);
        
        /// <summary>Default Palette Color Index 0x3D (#993366)</summary>
        public static readonly ColorEx Default3D = new ColorEx(0x99, 0x33, 0x66);
        
        /// <summary>Default Palette Color Index 0x3E (#333399)</summary>
        public static readonly ColorEx Default3E = new ColorEx(0x33, 0x33, 0x99);
        
        /// <summary>Default Palette Color Index 0x3F (#333333)</summary>
        public static readonly ColorEx Default3F = new ColorEx(0x33, 0x33, 0x33);

        #endregion

        /// <summary>Black - Alias for Default08</summary>
        public static readonly ColorEx Black = Default08;
        
        /// <summary>White - Alias for Default09</summary>
        public static readonly ColorEx White = Default09;
        
        /// <summary>Red - Alias for Default0A</summary>
        public static readonly ColorEx Red = Default0A;
        
        /// <summary>Green - Alias for Default0B</summary>
        public static readonly ColorEx Green = Default0B;
        
        /// <summary>Blue - Alias for Default0C</summary>
        public static readonly ColorEx Blue = Default0C;
        
        /// <summary>Yellow - Alias for Default0D</summary>
        public static readonly ColorEx Yellow = Default0D;
        
        /// <summary>Magenta - Alias for Default0E</summary>
        public static readonly ColorEx Magenta = Default0E;
        
        /// <summary>Cyan - Alias for Default0F</summary>
        public static readonly ColorEx Cyan = Default0F;
        
        /// <summary>DarkRed - Alias for Default10</summary>
        public static readonly ColorEx DarkRed = Default10;
        
        /// <summary>DarkGreen - Alias for Default11</summary>
        public static readonly ColorEx DarkGreen = Default11;
        
        /// <summary>DarkBlue - Alias for Default12</summary>
        public static readonly ColorEx DarkBlue = Default12;
        
        /// <summary>Olive - Alias for Default13</summary>
        public static readonly ColorEx Olive = Default13;
        
        /// <summary>Purple - Alias for Default14</summary>
        public static readonly ColorEx Purple = Default14;
        
        /// <summary>Teal - Alias for Default15</summary>
        public static readonly ColorEx Teal = Default15;
        
        /// <summary>Silver - Alias for Default16</summary>
        public static readonly ColorEx Silver = Default16;
        
        /// <summary>Grey - Alias for Default17</summary>
        public static readonly ColorEx Grey = Default17;

        /// <summary>System window text colour for border lines (used in records XF, CF, and WINDOW2 (BIFF8 only))</summary>
        public static readonly ColorEx SystemWindowTextColorForBorderLines = new ColorEx(64);

        /// <summary>System window background colour for pattern background (used in records XF, and CF)</summary>
        public static readonly ColorEx SystemWindowBackgroundColorForPatternBackground = new ColorEx(65);

        /// <summary>System face colour (dialogue background colour)</summary>
        public static readonly ColorEx SystemFaceColor = new ColorEx(67);

        /// <summary>System window text colour for chart border lines</summary>
        public static readonly ColorEx SystemWindowTextColorForChartBorderLines = new ColorEx(77);

        /// <summary>System window background colour for chart areas</summary>
        public static readonly ColorEx SystemWindowBackgroundColorForChartAreas = new ColorEx(78);

        /// <summary>Automatic colour for chart border lines (seems to be always Black)</summary>
        public static readonly ColorEx SystemAutomaticColorForChartBorderLines = new ColorEx(79);

        /// <summary>System tool tip background colour (used in note objects)</summary>
        public static readonly ColorEx SystemToolTipBackgroundColor = new ColorEx(80);

        /// <summary>System tool tip text colour (used in note objects)</summary>
        public static readonly ColorEx SystemToolTipTextColor = new ColorEx(81);
        
        //TODO: Figure out the SystemWindowTextColorForFonts value
        /// <summary>System window text colour for fonts (used in records FONT, EFONT, and CF)</summary>
        //public static readonly Color SystemWindowTextColorForFonts = new Color(??);

        internal static readonly ColorEx DefaultLineColor = Black;
        internal static readonly ColorEx DefaultPatternColor = new ColorEx(64);
        internal static readonly ColorEx DefaultPatternBackgroundColor = SystemWindowBackgroundColorForPatternBackground;
    }
}
