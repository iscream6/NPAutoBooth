using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FadeFox.UI
{
	public enum FlatButtonStyle
	{
		Flat,
		Popup,
		Standard
	}

	[ToolboxBitmap(typeof(System.Windows.Forms.Button))]
	public partial class FlatButton : Button
	{
		private FlatButtonStyle mFlatButtonStyle = FlatButtonStyle.Flat;

		private Color mTextColor = Color.Black;
		private Color mTextHotColor = Color.Black;
		private Color mTextShadowColor = Color.FromArgb(180, 180, 180);

		private SolidBrush mTextBrush = null;
		private SolidBrush mTextHotBrush = null;
		private SolidBrush mTextDisableBrush = new SolidBrush(Color.FromArgb(128, 128, 128));
		private SolidBrush mTextShadowBrush = null;

		private StringAlignment mTextAlign = StringAlignment.Center;

		private bool mTextShadow = false;
		
		private bool mDisplayFocus = true;
		private bool mDisplayPressedText = false;

		private Color mPopupButtonHighlightColor = SystemColors.ButtonHighlight;
		private Color mPopupButtonShadowColor = SystemColors.ButtonShadow;
		private Color mPopupButtonPressedColor = Color.White;
		private Pen mPopupButtonHighlightPen = null;
		private Pen mPopupButtonShadowPen = null;

		private Pen mButtonFacePen = null;
		private Color mButtonFaceColor = SystemColors.ButtonFace;

		private Color mFlatButtonHotColor = Color.FromArgb(182, 193, 214);
		private Color mFlatButtonPressedColor = Color.FromArgb(210, 218, 232);
		private Color mFlatButtonBorderColor = Color.DarkGray;
		private Color mFlatButtonBorderHotColor = Color.Black;
		private Pen mFlatButtonBorderPen = null;
		private Pen mFlatButtonBorderHotPen = null;

		private bool mIsHovered;
		private bool mIsFocused;
		private bool mIsFocusedByKey;
		private bool mIsKeyDown;
		private bool mIsMouseDown;
		private bool mIsPressed { get { return mIsKeyDown || (mIsMouseDown && mIsHovered); } }

		public FlatButton()
			: base()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.Opaque, false);

			CreateGraphicObject();
		}

		private void CreateGraphicObject()
		{
			if (mFlatButtonBorderPen != null)
			{
				mFlatButtonBorderPen.Dispose();
				mFlatButtonBorderPen = null;
			}
			mFlatButtonBorderPen = new Pen(mFlatButtonBorderColor, 2);

			if (mFlatButtonBorderHotPen != null)
			{
				mFlatButtonBorderHotPen.Dispose();
				mFlatButtonBorderHotPen = null;
			}
			mFlatButtonBorderHotPen = new Pen(mFlatButtonBorderHotColor, 2);

			if (mButtonFacePen != null)
			{
				mButtonFacePen.Dispose();
				mButtonFacePen = null;
			}
			mButtonFacePen = new Pen(mButtonFaceColor, 2);

			if (mPopupButtonHighlightPen != null)
			{
				mPopupButtonHighlightPen.Dispose();
				mPopupButtonHighlightPen = null;
			}
			mPopupButtonHighlightPen = new Pen(mPopupButtonHighlightColor, 2);

			if (mPopupButtonShadowPen != null)
			{
				mPopupButtonShadowPen.Dispose();
				mPopupButtonShadowPen = null;
			}
			mPopupButtonShadowPen = new Pen(mPopupButtonShadowColor, 2);


			if (mTextBrush != null)
			{
				mTextBrush.Dispose();
				mTextBrush = null;
			}
			mTextBrush = new SolidBrush(mTextColor);

			if (mTextHotBrush != null)
			{
				mTextHotBrush.Dispose();
				mTextHotBrush = null;
			}
			mTextHotBrush = new SolidBrush(mTextHotColor);

			if (mTextShadowBrush != null)
			{
				mTextShadowBrush.Dispose();
				mTextShadowBrush = null;
			}
			mTextShadowBrush = new SolidBrush(mTextShadowColor);
		}

		#region "속성"

		public FlatButtonStyle FlatButtonStyle
		{
			get { return mFlatButtonStyle; }
			set
			{
				mFlatButtonStyle = value;
				this.Invalidate();
			}
		}

		public bool DisplayFocus
		{
			get { return mDisplayFocus; }
			set
			{
				mDisplayFocus = value;
				this.Invalidate();
			}
		}

		public new StringAlignment TextAlign
		{
			get { return mTextAlign; }
			set
			{
				mTextAlign = value;
				this.Invalidate();
			}
		}

		public Color TextColor
		{
			get { return mTextColor; }
			set
			{
				mTextColor = value;
				CreateGraphicObject();
				this.Invalidate();
			}
		}

		public Color TextHotColor
		{
			get { return mTextHotColor; }
			set
			{
				mTextHotColor = value;
				CreateGraphicObject();
				this.Invalidate();
			}
		}

		public Color TextShadowColor
		{
			get { return mTextShadowColor; }
			set
			{
				mTextShadowColor = value;
				CreateGraphicObject();
				this.Invalidate();
			}
		}

		public bool TextShadow
		{
			get { return mTextShadow; }
			set
			{
				mTextShadow = value;
				this.Invalidate();
			}
		}

		public Color PopupButtonHighlightColor
		{
			get { return mPopupButtonHighlightColor; }
			set
			{
				mPopupButtonHighlightColor = value;
				CreateGraphicObject();
				this.Invalidate();
			}
		}

		public Color PopupButtonShadowColor
		{
			get { return mPopupButtonShadowColor; }
			set
			{
				mPopupButtonShadowColor = value;
				CreateGraphicObject();
				this.Invalidate();
			}
		}

		public Color PopupButtonPressedColor
		{
			get { return mPopupButtonPressedColor; }
			set
			{
				mPopupButtonPressedColor = value;
				CreateGraphicObject();
				this.Invalidate();
			}
		}

		public Color ButtonFaceColor
		{
			get { return mButtonFaceColor; }
			set
			{
				mButtonFaceColor = value;
				CreateGraphicObject();
				this.Invalidate();
			}
		}

		public Color FlatButtonHotColor
		{
			get { return mFlatButtonHotColor; }
			set
			{
				mFlatButtonHotColor = value;
				this.Invalidate();
			}
		}

		public Color FlatButtonPressedColor
		{
			get { return mFlatButtonPressedColor; }
			set
			{
				mFlatButtonPressedColor = value;
				this.Invalidate();
			}
		}

		public Color FlatButtonBorderColor
		{
			get { return mFlatButtonBorderColor; }
			set
			{
				mFlatButtonBorderColor = value;
				CreateGraphicObject();
				this.Invalidate();
			}
		}

		public Color FlatButtonBorderHotColor
		{
			get { return mFlatButtonBorderHotColor; }
			set
			{
				mFlatButtonBorderHotColor = value;
				CreateGraphicObject();
				this.Invalidate();
			}
		}

		private Color GetPopupButtonFaceColor(bool hot, bool pressed)
		{
			if (pressed)
				return mPopupButtonPressedColor;
			else
				return mButtonFaceColor;
				
		}

		private Pen GetPopupButtonTopLeftPen(bool hot, bool pressed)
		{
			if (hot)
			{
				if (pressed)
					return mPopupButtonShadowPen;
				else
					return mPopupButtonHighlightPen;
			}
			else
				return mButtonFacePen;
		}

		private Pen GetPopupButtonBottomRightPen(bool hot, bool pressed)
		{
			if (hot)
			{
				if (pressed)
					return mPopupButtonHighlightPen;
				else
					return mPopupButtonShadowPen;
			}
			else
				return mButtonFacePen;
		}

		private Color GetFlatButtonFaceColor(bool hot, bool pressed)
		{
			if (hot)
			{
				if (pressed)
				{
					return mFlatButtonPressedColor;
				}
				else
				{
					return mFlatButtonHotColor;
				}
			}
			else
			{
				return mButtonFaceColor;
			}
		}

		private Pen GetFlatButtonBorderPen(bool hot, bool focused)
		{
			if (hot)
			{
				return mFlatButtonBorderHotPen;
			}
			else if (focused)
			{
				if (mDisplayFocus)
					return mFlatButtonBorderHotPen;
				else
					return mFlatButtonBorderPen;
			}
			else
			{
				return mFlatButtonBorderPen;
			}
		}

		private SolidBrush GetTextBrush(bool hot)
		{
			if (hot)
			{
				return mTextHotBrush;
			}
			else
			{
				return mTextBrush;
			}
		}

		#endregion

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
		protected override void OnClick(EventArgs e)
		{
			mIsKeyDown = mIsMouseDown = false;
			base.OnClick(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Enter" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnEnter(EventArgs e)
		{
			mIsFocused = mIsFocusedByKey = true;
			base.OnEnter(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Leave" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			mIsFocused = mIsFocusedByKey = mIsKeyDown = mIsMouseDown = false;
			Invalidate();
		}

		protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space)
			{
				mIsKeyDown = true;
				Invalidate();
			}
			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(System.Windows.Forms.KeyEventArgs e)
		{
			if (mIsKeyDown && e.KeyCode == Keys.Space)
			{
				mIsKeyDown = false;
				Invalidate();
			}
			base.OnKeyUp(e);
		}

		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if (!mIsMouseDown && e.Button == MouseButtons.Left)
			{
				mIsMouseDown = true;
				mIsFocusedByKey = false;
				Invalidate();
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			if (mIsMouseDown)
			{
				mIsMouseDown = false;
				Invalidate();
			}
			base.OnMouseUp(e);
		}

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.Control.OnMouseMove(System.Windows.Forms.MouseEventArgs)" /> event.
		/// </summary>
		/// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button != MouseButtons.None)
			{
				if (!ClientRectangle.Contains(e.X, e.Y))
				{
					if (mIsHovered)
					{
						mIsHovered = false;
						Invalidate();
					}
				}
				else if (!mIsHovered)
				{
					mIsHovered = true;
					Invalidate();
				}
			}
		}

		protected override void OnMouseEnter(System.EventArgs e)
		{
			mIsHovered = true;
			Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(System.EventArgs e)
		{
			mIsHovered = false;
			Invalidate();
			base.OnMouseLeave(e);
		}

		protected override void OnGotFocus(System.EventArgs e)
		{
			mIsFocused = mIsFocusedByKey = true;
			Invalidate();
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(System.EventArgs e)
		{
			mIsFocused = mIsFocusedByKey = false;
			Invalidate();
			base.OnLostFocus(e);
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			switch (mFlatButtonStyle)
			{
				case FlatButtonStyle.Standard:
					DrawStandardButton(e.Graphics);
					break;
				case FlatButtonStyle.Flat:
					DrawFlatButton(e.Graphics);
					break;
				case FlatButtonStyle.Popup:
					DrawPopupButton(e.Graphics);
					break;
				default:
					DrawStandardButton(e.Graphics);
					break;

			}
		}

		private void DrawStandardButton(Graphics g)
		{
			g.Clear(GetPopupButtonFaceColor(true, mIsPressed));
			g.DrawLine(GetPopupButtonTopLeftPen(true, mIsPressed), 0, 0, 0, this.ClientRectangle.Height);
			g.DrawLine(GetPopupButtonTopLeftPen(true, mIsPressed), 0, 0, this.ClientRectangle.Width, 0);

			g.DrawLine(GetPopupButtonBottomRightPen(true, mIsPressed), this.ClientRectangle.Width, this.ClientRectangle.Height, 0, this.ClientRectangle.Height);
			g.DrawLine(GetPopupButtonBottomRightPen(true, mIsPressed), this.ClientRectangle.Width, this.ClientRectangle.Height, this.ClientRectangle.Width, 0);

			StringFormat format = new StringFormat();
			format.LineAlignment = StringAlignment.Center;
			format.Alignment = mTextAlign;
			format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;

			//Rectangle rect = this.ClientRectangle;
			int paddingAll = 3;
			Rectangle rect = new Rectangle(this.ClientRectangle.X + paddingAll, this.ClientRectangle.Y + 1 + paddingAll, this.ClientRectangle.Width - (paddingAll * 2), this.ClientRectangle.Height - (paddingAll * 2));
			Rectangle rect2 = rect;
			rect2.Location = new Point(rect.Left + 1, rect.Top + 1);

			if (mIsPressed)
			{
				rect.Location = new Point(rect.Left + 1, rect.Top + 1);
				rect2.Location = new Point(rect2.Left + 1, rect2.Top + 1);
			}

			if (this.Enabled)
			{
				if (mTextShadow)
					g.DrawString(this.Text, this.Font, mTextShadowBrush, rect2, format);

				g.DrawString(this.Text, this.Font, GetTextBrush(mIsHovered), rect, format);
			}
			else
			{
				g.DrawString(this.Text, this.Font, mTextDisableBrush, rect, format);
			}
		}

		private void DrawPopupButton(Graphics g)
		{
			g.Clear(GetPopupButtonFaceColor(mIsHovered, mIsPressed));
			g.DrawLine(GetPopupButtonTopLeftPen(mIsHovered, mIsPressed), 0, 0, 0, this.ClientRectangle.Height);
			g.DrawLine(GetPopupButtonTopLeftPen(mIsHovered, mIsPressed), 0, 0, this.ClientRectangle.Width, 0);

			g.DrawLine(GetPopupButtonBottomRightPen(mIsHovered, mIsPressed), this.ClientRectangle.Width, this.ClientRectangle.Height, 0, this.ClientRectangle.Height);
			g.DrawLine(GetPopupButtonBottomRightPen(mIsHovered, mIsPressed), this.ClientRectangle.Width, this.ClientRectangle.Height, this.ClientRectangle.Width, 0);

			StringFormat format = new StringFormat();
			format.LineAlignment = StringAlignment.Center;
			format.Alignment = mTextAlign;
			format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;

			//Rectangle rect = this.ClientRectangle;
			int paddingAll = 3;
			Rectangle rect = new Rectangle(this.ClientRectangle.X + paddingAll, this.ClientRectangle.Y + 1 + paddingAll, this.ClientRectangle.Width - (paddingAll * 2), this.ClientRectangle.Height - (paddingAll * 2));
			Rectangle rect2 = rect;
			rect2.Location = new Point(rect.Left + 1, rect.Top + 1);

			if (mIsPressed)
			{
				rect.Location = new Point(rect.Left + 1, rect.Top + 1);
				rect2.Location = new Point(rect2.Left + 1, rect2.Top + 1);
			}

			if (this.Enabled)
			{
				if (mTextShadow)
					g.DrawString(this.Text, this.Font, mTextShadowBrush, rect2, format);

				g.DrawString(this.Text, this.Font, GetTextBrush(mIsHovered), rect, format);
			}
			else
			{
				g.DrawString(this.Text, this.Font, mTextDisableBrush, rect, format);
			}
		}

		private void DrawFlatButton(Graphics g)
		{
			g.Clear(GetFlatButtonFaceColor(mIsHovered, mIsPressed));

			if (this.Image != null)
			{
				//g.DrawImage(this.Image, rect);
				g.DrawImage(this.Image, 0, 0);
			}

			g.DrawRectangle(GetFlatButtonBorderPen(mIsHovered, mIsFocused), this.ClientRectangle);

			StringFormat format = new StringFormat();
			format.LineAlignment = StringAlignment.Center;
			format.Alignment = mTextAlign;
			format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;  // 

			//Rectangle rect = this.ClientRectangle;
			int paddingAll = 1;
			Rectangle rect = new Rectangle(this.ClientRectangle.X + paddingAll, this.ClientRectangle.Y + 1 + paddingAll, this.ClientRectangle.Width - (paddingAll * 2), this.ClientRectangle.Height - (paddingAll * 2));
			Rectangle rect2 = rect;
			rect2.Location = new Point(rect.Left + 1, rect.Top + 1);

			if (mIsPressed)
			{
				if (mDisplayPressedText == true)
				{
					rect.Location = new Point(rect.Left + 1, rect.Top + 1);
					rect2.Location = new Point(rect2.Left + 1, rect2.Top + 1);
				}
			}

			//g.DrawImage(this.Image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, disabledImageAttr);

			if (this.Enabled)
			{
				if (mTextShadow)
					g.DrawString(this.Text, this.Font, mTextShadowBrush, rect2, format);

				g.DrawString(this.Text, this.Font, GetTextBrush(mIsHovered), rect, format);
			}
			else
			{
				g.DrawString(this.Text, this.Font, mTextDisableBrush, rect, format);
			}
		}
	}
}
