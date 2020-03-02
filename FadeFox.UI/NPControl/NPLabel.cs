using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Collections;
using static FadeFox.UI.PropertyListBAS;
using Microsoft.Scripting.Metadata;

namespace FadeFox.UI
{
    [
    ComVisible(true),
    ClassInterface(ClassInterfaceType.AutoDispatch),
    DefaultProperty("Text"),
    DefaultBindingProperty("Text"),
    ]
    public class NPLabel : Control, INPControlProperty
    {
        #region Fields

        private ENUM_FONTSELECT meUseFontStyle;
        private float fontSize = 9.0F;
        private ContentAlignment moTextAlign = ContentAlignment.MiddleCenter;

        protected event PropertyChangedEvent Event_PropertyChanged;

        #endregion

        #region Constructor

        public NPLabel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
            Event_PropertyChanged += OnPropertyChanged;
            MouseHover += OnRefreshed;
            base.BackColor = Color.FromArgb(0, 0, 0, 0);
            Event_PropertyChanged(false);
        }

        #endregion

        #region Properties

        [Category("NPAutoBooth Design"), Browsable(true), Description("Text Alignment 설정")]
        public ContentAlignment NPTextAlign
        {
            get => moTextAlign;
            set
            {
                moTextAlign = value;
                Event_PropertyChanged(true);
            }
        }

        [Category("NPAutoBooth Design"), Browsable(true), Description("Font style 설정(옥션고딕)")]
        public PropertyListBAS.ENUM_FONTSELECT NPUseFontStyle
        {
            get => meUseFontStyle;
            set
            {
                meUseFontStyle = value;
                NPSetFontStyle(meUseFontStyle);
                this.Invalidate();
            }
        }

        [Category("NPAutoBooth Design"), Browsable(true), Description("Font size 설정(9pt)")]
        public float NPFontSize
        {
            get => fontSize;
            set
            {
                fontSize = value;
                NPSetFontStyle(meUseFontStyle);
                Event_PropertyChanged(true);
            }
        }

        [Category("NPAutoBooth Design"), Browsable(true), Description("다국어 지원 코드 설정")]
        public string NPLanguageCode
        {
            get => this.Tag?.ToString();
            set => this.Tag = value;
        }

        [Category("NPAutoBooth Design"), Browsable(true), Description("Text 설정")]
        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                Event_PropertyChanged(true);
            }
        }

        [Category("NPAutoBooth Design"), Browsable(true), Description("배경색 설정")]
        public Color NPBackColor
        {
            get => base.BackColor; 
            set
            {
                base.BackColor = value;
                Event_PropertyChanged(true);
            }
        }

        #endregion

        #region Event handler

        protected override void OnPaint(PaintEventArgs e)
        {
            NPDrawControlText(e.Graphics, this.ClientRectangle);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (base.BackColor == null || base.BackColor == Color.Transparent || base.BackColor == Color.FromArgb(0, 0, 0, 0))
            {

            }
            else
            {
                base.OnPaintBackground(e);
            }
        }

        /// <summary>
        /// Invoked when property changed
        /// </summary>
        /// <param name="pRecreateHandle"></param>
        private void OnPropertyChanged(bool pRecreateHandle)
        {
            if (pRecreateHandle) this.RecreateHandle();
            this.Invalidate();
        }

        private void OnRefreshed(object sender, EventArgs e)
        {
            OnPropertyChanged(true);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            Event_PropertyChanged(true);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            Event_PropertyChanged(true);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                if (base.BackColor != null || base.BackColor == Color.Transparent || base.BackColor == Color.FromArgb(0, 0, 0, 0))
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x20; // Turn on WS_EX_TRANSPARENT
                    return cp;
                }
                else
                {
                    return base.CreateParams;
                }
            }
        }

        #endregion

        #region Methods

        public void NPSetFontStyle(PropertyListBAS.ENUM_FONTSELECT peFontStyle)
        {
            if (peFontStyle == ENUM_FONTSELECT.BOLD)
            {
                this.Font = NPControlHelper.IsFontInstalled(NP_BOLD_FONT) ? new Font(NP_BOLD_FONT, NPFontSize) : new Font("맑은 고딕", NPFontSize);
            }
            else if (peFontStyle == ENUM_FONTSELECT.M_FONT)
            {
                this.Font = NPControlHelper.IsFontInstalled(NP_M_FONT) ? new Font(NP_M_FONT, NPFontSize) : new Font("맑은 고딕", NPFontSize);
            }
            else if (peFontStyle == ENUM_FONTSELECT.L_FONT)
            {
                this.Font = NPControlHelper.IsFontInstalled(NP_L_FONT) ? new Font(NP_L_FONT, NPFontSize) : new Font("맑은 고딕", NPFontSize);
            }
            else
            {
                this.Font = null; //기본 Font로 설정 후
                this.Font = new Font(this.Font.Name, NPFontSize);
            }
        }
        protected void NPDrawControlText(Graphics g, Rectangle pClientRect)
        {
            Rectangle oTextRect = NPGetTextRectangle(this.Text, this.Font, ref g, pClientRect, this.NPTextAlign);
            g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), oTextRect);
        }

        protected Rectangle NPGetTextRectangle(string pText, Font pFont, ref Graphics g,
            Rectangle pClientRect, ContentAlignment pTextAlign)
        {
            SizeF oTextSize = g.MeasureString(pText, pFont);
            int iTextLeft;
            oTextSize.Width = oTextSize.Width + 4;

            switch (pTextAlign)
            {
                case ContentAlignment.TopLeft:
                    return pClientRect;
                case ContentAlignment.TopCenter:
                    Conversion.Fix(12);
                    iTextLeft = (pClientRect.Width / 2) - ((int)(Conversion.Fix(oTextSize.Width)) / 2);

                    return new Rectangle((pClientRect.Left + iTextLeft), pClientRect.Top, (int)(Conversion.Fix(oTextSize.Width)),
                                       (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.TopRight:
                    iTextLeft = (int)(pClientRect.Width - oTextSize.Width);

                    return new Rectangle((pClientRect.Left + iTextLeft), pClientRect.Top, (int)(Conversion.Fix(oTextSize.Width)),
                                       (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.MiddleLeft:
                    return new Rectangle(pClientRect.Left, (int)(Conversion.Fix(pClientRect.Height / 2)) - (int)(Conversion.Fix(oTextSize.Height / 2)),
                                   (int)(Conversion.Fix(oTextSize.Width)), (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.MiddleCenter:
                    iTextLeft = (pClientRect.Width / 2) - ((int)(Conversion.Fix(oTextSize.Width)) / 2);

                    return new Rectangle((pClientRect.Left + iTextLeft),
                                       (int)(Conversion.Fix(pClientRect.Height / 2)) - (int)(Conversion.Fix(oTextSize.Height / 2)),
                                       (int)(Conversion.Fix(oTextSize.Width)), (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.MiddleRight:
                    iTextLeft = (int)(pClientRect.Width - oTextSize.Width);

                    return new Rectangle((pClientRect.Left + iTextLeft),
                                       (int)(Conversion.Fix(pClientRect.Height / 2)) - (int)(Conversion.Fix(oTextSize.Height / 2)),
                                       (int)(Conversion.Fix(oTextSize.Width)), (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.BottomLeft:
                    return new Rectangle(pClientRect.Left, (int)(Conversion.Fix(pClientRect.Bottom - oTextSize.Height)),
                                   (int)(Conversion.Fix(oTextSize.Width)), (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.BottomCenter:
                    iTextLeft = (pClientRect.Width / 2) - ((int)(Conversion.Fix(oTextSize.Width)) / 2);

                    return new Rectangle((pClientRect.Left + iTextLeft), (int)(Conversion.Fix(pClientRect.Bottom - oTextSize.Height)),
                                       (int)(Conversion.Fix(oTextSize.Width)), (int)(Conversion.Fix(oTextSize.Height)));
                case ContentAlignment.BottomRight:
                    iTextLeft = (int)(pClientRect.Width - oTextSize.Width);
                    return new Rectangle((pClientRect.Left + iTextLeft), (int)(Conversion.Fix(pClientRect.Bottom - oTextSize.Height)),
                                       (int)(Conversion.Fix(oTextSize.Width)), (int)(Conversion.Fix(oTextSize.Height)));
                default:
                    return new Rectangle();
            }
        }

        #endregion

    }

    public class NPTransparentControlDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        protected override void PostFilterProperties(IDictionary properties)
        {
            properties.Remove("BackColor");
            properties.Remove("BackgroundImage");
            properties.Remove("BackgroundImageLayout");
            properties.Remove("RightToLeft");
            properties.Remove("TabStop");
            properties.Remove("TabIndex");
            properties.Remove("AutoSize");
            properties.Remove("Tag");

            base.PostFilterProperties(properties);
        }
    }
}
