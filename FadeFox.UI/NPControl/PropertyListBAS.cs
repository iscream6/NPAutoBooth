using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FadeFox.UI
{
    public static class PropertyListBAS
    {
        public const string NP_BOLD_FONT = "옥션고딕 B";
        public const string NP_M_FONT = "옥션고딕 M";
        public const string NP_L_FONT = "옥션고딕 L";

        public delegate void PropertyChangedEvent(bool pRecreateHandle);

        [Flags]
        public enum ENUM_LABELTYPE
        {
            NONE,
            TITLE_LEFT,
            VALUE_LEFT,
            TITLE_RIGHT,
            VALUE_RIGHT,
            TITLE_MIDDLE,
            VALUE_MIDDLE
        }

        [Flags]
        public enum ENUM_LABELSTYLE
        {
            LABEL,
            MOVELABEL
        }

        [Flags]
        public enum ENUM_TRUEFALSE
        {
            NONE,
            TRUE,
            FALSE
        }

        [Flags]
        public enum ENUM_FONTSELECT
        {
            NONE,
            BOLD,
            M_FONT,
            L_FONT
        }

        [Flags]
        public enum ENUM_CORNER
        {
            NONE = 0,
            TopLeft = 1,
            TopRight = 2,
            BottomRight = 4,
            BottomLeft = 8
        }
    }
}
