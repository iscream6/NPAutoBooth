using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Runtime.InteropServices;
using static FadeFox.UI.PropertyListBAS;

namespace FadeFox.UI
{
    [ComVisible(false), Designer(typeof(NPButtonControlDesigner))]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [DefaultProperty("Text"), DefaultEvent("Click")]
    public class NPButton : Button, INPControlProperty
    {
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private bool mHover = false;
        private bool mDown = false;
        private ENUM_FONTSELECT meUseFontStyle;
        private float fontSize = 9.0F;
        private bool holdingSpace = false;

        private Color defaultBackColor;
        private Color defaultForeColor;
        private Color normalBackColor;
        private Color normalForeColor;
        private Color disableBackColor;
        private Color disableForeColor;
        private Color downBackColor;
        private Color downForeColor;
        private Color hoverBackColor;
        private Color hoverForeColor;

        public NPButton()
        {
            this.FlatAppearance.BorderColor = Color.LightGray; //포커스 사각형을 제거 하기위해 다른 Color를 대입함.
            this.FlatStyle = FlatStyle.Flat;
            NPNormalBackColor = Color.White;
            NPNormalForeColor = Color.Green;
            meUseFontStyle = ENUM_FONTSELECT.NONE;
        }

        #region Properties

        [Category("NPAutoBooth Design"), Browsable(true), Description("Font style 설정(옥션고딕)")]
        public ENUM_FONTSELECT NPUseFontStyle
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
                this.Invalidate();
            }
        }

        [Category("NPAutoBooth Design"), Browsable(true), Description("다국어 지원 코드 설정")]
        public string NPLanguageCode 
        { 
            get => this.Tag?.ToString(); 
            set => this.Tag = value; 
        }

        [Browsable(false)]
        public override Font Font
        {
            get => base.Font;
            set => base.Font = value;
        }

        [Category("NPAutoBooth Design"), Browsable(true), Description("표시안함")]
        public Color NPDefaultBackColor
        {
            get => defaultBackColor;
            set
            {
                defaultBackColor = value;
                this.BackColor = defaultBackColor;
                Invalidate();
            }
        }

        [Category("NPAutoBooth Design"), Browsable(true), Description("표시안함")]
        public Color NPDefaultForeColor
        {
            get => defaultForeColor;
            set
            {
                defaultForeColor = value;
                this.ForeColor = defaultForeColor;
                Invalidate();
            }
        }

        [Category("NPAutoBooth Design"), Browsable(true), Description("기본 배경 컬러 설정")]
        public Color NPNormalBackColor
        {
            get => normalBackColor;
            set
            {
                normalBackColor = value;
                NPDefaultBackColor = normalBackColor;
            }
        }

        [Category("NPAutoBooth Design"), Browsable(true), Description("기본 폰트 컬러 설정")]
        public Color NPNormalForeColor
        {
            get => normalForeColor;
            set
            {
                normalForeColor = value;
                NPDefaultForeColor = normalForeColor;
            }
        }

        [Category("NPAutoBooth Design"), Browsable(true), Description("비활성화 배경 컬러 설정")]
        public Color NPDisableBackColor { get => disableBackColor; set => disableBackColor = value; }

        [Category("NPAutoBooth Design"), Browsable(true), Description("비활성화 폰트 컬러 설정")]
        public Color NPDisableForeColor { get => disableForeColor; set => disableForeColor = value; }

        [Category("NPAutoBooth Design"), Browsable(true), Description("눌림 배경 컬러 설정")]
        public Color NPDownBackColor { get => downBackColor; set => downBackColor = value; }

        [Category("NPAutoBooth Design"), Browsable(true), Description("눌림 폰트 컬러 설정")]
        public Color NPDownForeColor { get => downForeColor; set => downForeColor = value; }

        [Category("NPAutoBooth Design"), Browsable(true), Description("호버 배경 컬러 설정")]
        public Color NPHoverBackColor { get => hoverBackColor; set => hoverBackColor = value; }

        [Category("NPAutoBooth Design"), Browsable(true), Description("호버 폰트 컬러 설정")]
        public Color NPHoverForeColor { get => hoverForeColor; set => hoverForeColor = value; }


        #endregion

        #region Events

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
            {
                mHover = false;
                mDown = false;

                NPDefaultBackColor = NPNormalBackColor;
                NPDefaultForeColor = NPNormalForeColor;
            }
            else
            {
                NPDefaultBackColor = NPDisableBackColor;
                NPDefaultForeColor = NPDisableForeColor;
            }

            base.OnEnabledChanged(e);
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            if (Enabled)
            {
                mHover = true;

                if (mDown)
                {
                    if (NPDownBackColor != null) NPDefaultBackColor = NPDownBackColor;
                    if (NPDownForeColor != null) NPDefaultForeColor = NPDownForeColor;
                }
                else
                {
                    if (NPHoverBackColor != null) NPDefaultBackColor = NPHoverBackColor;
                    if (NPHoverForeColor != null) NPDefaultForeColor = NPHoverForeColor;

                    if (NPHoverBackColor == null && NPHoverForeColor == null)
                    {
                        NPDefaultBackColor = NPNormalBackColor;
                        NPDefaultForeColor = NPNormalForeColor;
                    }
                }
            }
            else
            {
                if (NPDisableBackColor != null) NPDefaultForeColor = NPDisableBackColor;
                if (NPDisableForeColor != null) NPDefaultForeColor = NPDisableForeColor;
            }

            if (mevent != null) base.OnMouseMove(mevent);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (Enabled)
            {
                base.Focus();
                OnMouseUp(null);
                mDown = true;

                if (NPDownBackColor != null) NPDefaultBackColor = NPDownBackColor;
                if (NPDownForeColor != null) NPDefaultForeColor = NPDownForeColor;
            }
            else
            {
                if (NPDisableBackColor != null) NPDefaultForeColor = NPDisableBackColor;
                if (NPDisableForeColor != null) NPDefaultForeColor = NPDisableForeColor;
            }

            if (mevent != null) base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (Enabled)
            {
                mDown = false;

                if (mHover)
                {
                    if (NPHoverBackColor != null) NPDefaultForeColor = NPHoverBackColor;
                    if (NPHoverForeColor != null) NPDefaultForeColor = NPHoverForeColor;
                }
                else
                {
                    NPDefaultBackColor = NPNormalBackColor;
                    NPDefaultForeColor = NPNormalForeColor;
                }
            }
            else
            {
                if (NPDisableBackColor != null) NPDefaultForeColor = NPDisableBackColor;
                if (NPDisableForeColor != null) NPDefaultForeColor = NPDisableForeColor;
            }

            if(mevent != null) base.OnMouseUp(mevent);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (Enabled)
            {
                mHover = false;

                NPDefaultBackColor = NPNormalBackColor;
                NPDefaultForeColor = NPNormalForeColor;
            }
            else
            {
                if (NPDisableBackColor != null) NPDefaultForeColor = NPDisableBackColor;
                if (NPDisableForeColor != null) NPDefaultForeColor = NPDisableForeColor;
            }

            base.OnMouseLeave(e);
        }



        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle Rect = new Rectangle(0, 0, this.Width, this.Height);
            using (GraphicsPath GraphPath = RoundControlCLS.RoundedRectangle(Rect, 9, ENUM_CORNER.NONE))
            {
                using (Pen pen = new Pen(Color.LightGray, 1.75f))
                {
                    pen.Alignment = PenAlignment.Inset;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(pen, GraphPath);
                    this.Region = new Region(GraphPath);
                }
            }
        }

        public override bool PreProcessMessage(ref Message msg)
        {
            if (msg.Msg == WM_KEYUP)
            {
                if (holdingSpace)
                {
                    if ((int)msg.WParam == (int)Keys.Space)
                    {
                        OnMouseUp(null);
                        PerformClick();
                    }
                    else if ((int)msg.WParam == (int)Keys.Escape
                        || (int)msg.WParam == (int)Keys.Tab)
                    {
                        holdingSpace = false;
                        OnMouseUp(null);
                    }
                }
                return true;
            }
            else if (msg.Msg == WM_KEYDOWN)
            {
                if ((int)msg.WParam == (int)Keys.Space)
                {
                    holdingSpace = true;
                    OnMouseDown(null);
                }
                else if ((int)msg.WParam == (int)Keys.Enter)
                {
                    PerformClick();
                }
                return true;
            }
            else
            {
                return base.PreProcessMessage(ref msg);
            }
        }

        #endregion

        #region Functions

        public void NPSetFontStyle(PropertyListBAS.ENUM_FONTSELECT peFontStyle)
        {
            if (peFontStyle == ENUM_FONTSELECT.BOLD)
            {
                this.Font = NPControlHelper.IsFontInstalled(NP_BOLD_FONT)? new Font(NP_BOLD_FONT, NPFontSize) : new Font("맑은 고딕", NPFontSize);
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

        public class NPButtonControlDesigner : System.Windows.Forms.Design.ControlDesigner
        {
            protected override void PostFilterProperties(IDictionary properties)
            {
                properties.Remove("BackColor");
                properties.Remove("BackgroundImage");
                properties.Remove("BackgroundImageLayout");
                properties.Remove("RightToLeft");
                properties.Remove("FlatAppearance");
                properties.Remove("FlatStyle");
                properties.Remove("ForeColor");
                properties.Remove("Image");
                properties.Remove("ImageAlign");
                properties.Remove("ImageIndex");
                properties.Remove("ImageKey");
                properties.Remove("ImageList");
                properties.Remove("Tag");

                base.PostFilterProperties(properties);
            }
        }

        #endregion
    }
}
