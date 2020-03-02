using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FadeFox.UI
{
    public interface INPControlProperty
    {
        PropertyListBAS.ENUM_FONTSELECT NPUseFontStyle { get; set; }

        float NPFontSize { get; set; }

        string NPLanguageCode { get; set; }

        void NPSetFontStyle(PropertyListBAS.ENUM_FONTSELECT peFontStyle);
    }
}
