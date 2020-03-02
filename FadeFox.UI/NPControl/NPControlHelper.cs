using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FadeFox.UI
{
    public static class NPControlHelper
    {
        public static bool IsFontInstalled(string fontName)
        {
            InstalledFontCollection collection = new InstalledFontCollection();
            foreach (FontFamily fontFamily in collection.Families)
            {
                if (fontFamily.Name.Equals(fontName))
                {
                    return true;
                }
            }

            return false;
        }


        public static Rectangle NPGetTextRectangle
            (string pText,
             Font pFont,
             Graphics g,
             Rectangle pClientRect,
             ContentAlignment pTextAlign,
             bool multiLine = true
            )
        {
            int iTextLeft;
            //가로 사이즈
            SizeF oTextSize = g.MeasureString(pText, pFont);
            oTextSize.Width = oTextSize.Width + 4;
            //반환 할 사각형
            Rectangle recReturn = pClientRect;
            switch (pTextAlign)
            {
                case ContentAlignment.TopLeft: //상단 좌측
                    if (multiLine)
                    {
                        recReturn = new Rectangle();
                        if(oTextSize.Width >= pClientRect.Width)
                        {
                            int h = GetRectanbleHeight(pText, pClientRect.Width, oTextSize.Height, pFont, g);
                            recReturn.Height = h > pClientRect.Height ? h : pClientRect.Height;
                        }
                    }
                    return recReturn;
                case ContentAlignment.TopCenter: //상단 중앙
                    Conversion.Fix(12);
                    iTextLeft = (pClientRect.Width / 2) - ((int)(Conversion.Fix(oTextSize.Width)) / 2);

                    return new Rectangle((pClientRect.Left + iTextLeft), pClientRect.Top, (int)(Conversion.Fix(oTextSize.Width)),
                                       (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.TopRight: //상단 우측
                    iTextLeft = (int)(pClientRect.Width - oTextSize.Width);

                    return new Rectangle((pClientRect.Left + iTextLeft), pClientRect.Top, (int)(Conversion.Fix(oTextSize.Width)),
                                       (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.MiddleLeft: //중앙 좌측
                    return new Rectangle(pClientRect.Left, (int)(Conversion.Fix(pClientRect.Height / 2)) - (int)(Conversion.Fix(oTextSize.Height / 2)),
                                   (int)(Conversion.Fix(oTextSize.Width)), (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.MiddleCenter: //중앙 중앙
                    iTextLeft = (pClientRect.Width / 2) - ((int)(Conversion.Fix(oTextSize.Width)) / 2);

                    return new Rectangle((pClientRect.Left + iTextLeft), //X좌표
                                       (int)(Conversion.Fix(pClientRect.Height / 2)) - (int)(Conversion.Fix(oTextSize.Height / 2)), //Y좌표
                                       (int)(Conversion.Fix(oTextSize.Width)), (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.MiddleRight: //중앙 우측
                    iTextLeft = (int)(pClientRect.Width - oTextSize.Width);

                    return new Rectangle((pClientRect.Left + iTextLeft),
                                       (int)(Conversion.Fix(pClientRect.Height / 2)) - (int)(Conversion.Fix(oTextSize.Height / 2)),
                                       (int)(Conversion.Fix(oTextSize.Width)), (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.BottomLeft: //하단 좌측
                    return new Rectangle(pClientRect.Left, (int)(Conversion.Fix(pClientRect.Bottom - oTextSize.Height)),
                                   (int)(Conversion.Fix(oTextSize.Width)), (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.BottomCenter: //하단 중앙
                    iTextLeft = (pClientRect.Width / 2) - ((int)(Conversion.Fix(oTextSize.Width)) / 2);

                    return new Rectangle((pClientRect.Left + iTextLeft), (int)(Conversion.Fix(pClientRect.Bottom - oTextSize.Height)),
                                       (int)(Conversion.Fix(oTextSize.Width)), (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.BottomRight: //하단 우측
                    iTextLeft = (int)(pClientRect.Width - oTextSize.Width);
                    return new Rectangle((pClientRect.Left + iTextLeft), (int)(Conversion.Fix(pClientRect.Bottom - oTextSize.Height)),
                                       (int)(Conversion.Fix(oTextSize.Width)), (int)(Conversion.Fix(oTextSize.Height)));
                default:
                    return new Rectangle();
            }
        }

        private static int GetRectanbleHeight(string text, float width, float fontHeigh, Font font, Graphics g)
        {
            StringBuilder builder = new StringBuilder();
            float returnHeight = fontHeigh;
            foreach (var item in text.ToCharArray())
            {
                builder.Append(item);
                if(width <= g.MeasureString(builder.ToString(), font).Width + 4)
                {
                    returnHeight += (GetRectanbleHeight(text.Substring(builder.ToString().Length - 1), width, returnHeight, font, g) + 4) ;
                }
                else
                {
                    continue;
                }
                    
            }

            return (int)returnHeight;
        }
    }
}
