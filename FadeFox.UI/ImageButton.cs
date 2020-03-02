using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace FadeFox.UI
{
	[ToolboxBitmap(typeof(System.Windows.Forms.Button))]
	public partial class ImageButton : PictureBox, IButtonControl
	{
		private Color mTextShadowColor = Color.FromArgb(9, 9, 9);
		private Color mDisabledTextColor = Color.FromArgb(170, 179, 179);
		private Color mDownTextColor = Color.White;
		private Color mDownTextShadowColor = Color.FromArgb(9, 9, 9);

		protected HorizontalAlignment mTextAlignment = HorizontalAlignment.Center;

		private int mTextTopMargin = 2;
		public int TextTopMargin
		{
			get { return mTextTopMargin; }
			set
			{
				mTextTopMargin = value;
				this.Refresh();
			}
		}

		public Color TextShadowColor
		{
			get { return mTextShadowColor; }
			set
			{
				mTextShadowColor = value;
				this.Refresh();
			}
		}

		public Color DisabledTextColor
		{
			get { return mDisabledTextColor; }
			set
			{
				mDisabledTextColor = value;
				this.Refresh();
			}
		}

		public Color DownTextColor
		{
			get { return mDownTextColor; }
			set 
			{ 
				mDownTextColor = value;
				this.Refresh();
			}
		}

		public Color DownTextShadowColor
		{
			get { return mDownTextShadowColor; }
			set 
			{
				mDownTextShadowColor = value;
				this.Refresh();
			}
		}

		private bool mUsingTextShadow = true;
		public bool UsingTextShadow
		{
			get { return mUsingTextShadow; }
			set 
			{ 
				mUsingTextShadow = value;
				this.Refresh();
			}
		}

		private bool mUsingTextAlignInImage = true;
		public bool UsingTextAlignInImage
		{
			get { return mUsingTextAlignInImage; }
			set 
			{
				mUsingTextAlignInImage = value;
				this.Refresh();
			}
		}

		public HorizontalAlignment TextAlign
		{
			get { return mTextAlignment; }
			set
			{
				mTextAlignment = value;
				this.Refresh();
			}
		}

		public ImageButton()
		{
			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.Opaque, false);
			NormalImage = GetResourceImage("FadeFox.UI.ImageButtonDefaultNormal.png");
			DownImage = GetResourceImage("FadeFox.UI.ImageButtonDefaultDown.png");
			HoverImage = GetResourceImage("FadeFox.UI.ImageButtonDefaultHover.png");
			DisabledImage = GetResourceImage("FadeFox.UI.ImageButtonDefaultNormal.png");
			this.ForeColor = Color.White;
		}

		#region IButtonControl Members

		private DialogResult mDialogResult;
		public DialogResult DialogResult
		{
			get
			{
				return mDialogResult;
			}
			set
			{
				mDialogResult = value;
			}
		}

		public void NotifyDefault(bool value)
		{
			isDefault = value;
		}

		public void PerformClick()
		{
			base.OnClick(EventArgs.Empty);
		}

		#endregion

		#region HoverImage

		private Image mHoverImage;

		[Category("Appearance")]
		[Description("Image to show when the button is hovered over.")]
		public Image HoverImage
		{
			get { return mHoverImage; }
			set 
			{ 
				mHoverImage = value;

				if (Enabled)
				{
					if (mHover)
					{
						Image = value;
					}
				}
			}
		}
		#endregion

		#region DownImage
		private Image mDownImage;

		[Category("Appearance")]
		[Description("Image to show when the button is depressed.")]
		public Image DownImage
		{
			get { return mDownImage; }
			set 
			{ 
				mDownImage = value;

				if (Enabled)
				{
					if (mDown)
					{
						Image = value;
					}
				}
			}
		}
		#endregion

		#region NormalImage
		private Image mNormalImage;

		[Category("Appearance")]
		[Description("Image to show when the button is not in any other state.")]
		public Image NormalImage
		{
			get { return mNormalImage; }
			set 
			{ 
				mNormalImage = value;

				if (Enabled)
				{
					if (!(mHover || mDown))
					{
						Image = value;
					}
				}
			}
		}
		#endregion

		#region DisabledImage
		private Image mDisabledImage;

		[Category("Appearance")]
		[Description("Image to show when the button is disabled.")]
		public Image DisabledImage
		{
			get { return mDisabledImage; }
			set
			{
				mDisabledImage = value;

				if (!Enabled)
				{
					Image = value;
				}
			}
		}
		#endregion

		private const int WM_KEYDOWN = 0x0100;
		private const int WM_KEYUP = 0x0101;
		private bool mHover = false;
		private bool mDown = false;
		private bool isDefault = false;

		#region Overrides

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Category("Appearance")]
		[Description("The text associated with the control.")]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Category("Appearance")]
		[Description("전경색.")]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Category("Appearance")]
		[Description("The font used to display text in the control.")]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		#endregion

		#region Description Changes
		[Description("Controls how the ImageButton will handle image placement and control sizing.")]
		public new PictureBoxSizeMode SizeMode 
		{ 
			get 
			{ 
				return base.SizeMode; 
			} 

			set 
			{
				base.SizeMode = value;
				this.Refresh();
			} 
		}

		[Description("Controls what type of border the ImageButton should have.")]
		public new BorderStyle BorderStyle { get { return base.BorderStyle; } set { base.BorderStyle = value; } }
		#endregion

		#region Hiding

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image Image { get { return base.Image; } set { base.Image = value; } }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new String ImageLocation { get { return base.ImageLocation; } set { base.ImageLocation = value; } }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image ErrorImage { get { return base.ErrorImage; } set { base.ErrorImage = value; } }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image InitialImage { get { return base.InitialImage; } set { base.InitialImage = value; } }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool WaitOnLoad { get { return base.WaitOnLoad; } set { base.WaitOnLoad = value; } }


		public new bool Enabled
		{
			get
			{
				return base.Enabled;
			}

			set
			{
				base.Enabled = value;

				if (value)
				{
					mHover = false;
					mDown = false;

					if (Image != mNormalImage)
					{
						Image = mNormalImage;
					}
				}
				else
				{
					if ((mDisabledImage != null) && (Image != mDisabledImage))
					{
						Image = mDisabledImage;
					}
				}
			}
		}

		#endregion

		#region Events

		protected override void OnEnabledChanged(EventArgs e)
		{
			if (Enabled)
			{
				mHover = false;
				mDown = false;

				if (Image != mNormalImage)
				{
					Image = mNormalImage;
				}
			}
			else
			{
				if ((mDisabledImage != null) && (Image != mDisabledImage))
				{
					Image = mDisabledImage;
				}
			}

			base.OnEnabledChanged(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (Enabled)
			{
				mHover = true;

				if (mDown)
				{
					if ((mDownImage != null) && (Image != mDownImage))
					{
						Image = mDownImage;
					}
				}
				else
				{
					if (mHoverImage != null)
					{
						Image = mHoverImage;
					}
					else
					{
						if (Image != mNormalImage)
						{
							Image = mNormalImage;
						}
					}
				}
			}
			else
			{
				if ((mDisabledImage != null) && (Image != mDisabledImage))
				{
					Image = mDisabledImage;
				}
			}

			base.OnMouseMove(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			if (Enabled)
			{
				mHover = false;

				if (Image != mNormalImage)
				{
					Image = mNormalImage;
				}
			}
			else
			{
				if ((mDisabledImage != null) && (Image != mDisabledImage))
				{
					Image = mDisabledImage;
				}
			}

			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (Enabled)
			{
				base.Focus();
				OnMouseUp(null);
				mDown = true;

				if ((mDownImage != null) && (Image != mDownImage))
				{
					Image = mDownImage;
				}
			}
			else
			{
				if ((mDisabledImage != null) && (Image != mDisabledImage))
				{
					Image = mDisabledImage;
				}
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (Enabled)
			{
				mDown = false;

				if (mHover)
				{
					if ((mHoverImage != null) && (Image != mHoverImage))
					{
						Image = mHoverImage;
					}
				}
				else
				{
					if (Image != mNormalImage)
					{
						Image = mNormalImage;
					}
				}
			}
			else
			{
				if ((mDisabledImage != null) && (Image != mDisabledImage))
				{
					Image = mDisabledImage;
				}
			}

			base.OnMouseUp(e);
		}

		private bool holdingSpace = false;

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

		protected override void OnLostFocus(EventArgs e)
		{
			holdingSpace = false;
			OnMouseUp(null);
			base.OnLostFocus(e);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);

			if ((!string.IsNullOrEmpty(Text)) && (pe != null) && (base.Font != null))
			{
				StringFormat format = new StringFormat();
				
				format.LineAlignment = StringAlignment.Center;

				switch (mTextAlignment)
				{
					case HorizontalAlignment.Left:
						format.Alignment = StringAlignment.Near;
						break;
					case HorizontalAlignment.Center:
						format.Alignment = StringAlignment.Center;
						break;
					case HorizontalAlignment.Right:
						format.Alignment = StringAlignment.Far;
						break;
				}


				Rectangle rect = this.ClientRectangle;

				if ((Image != null) && mUsingTextAlignInImage == true && SizeMode == PictureBoxSizeMode.Normal)
				{
					rect = new Rectangle(new Point(0, mTextTopMargin), Image.Size);
				}
				else
				{
					rect = new Rectangle(new Point(0, mTextTopMargin), this.ClientRectangle.Size);
				}

				if (mUsingTextShadow)
				{
					Rectangle sRect = new Rectangle(new Point(rect.X, rect.Y + 1), rect.Size);

					if (!mDown)
						pe.Graphics.DrawString(this.Text, this.Font, new SolidBrush(mTextShadowColor), sRect, format);
					else
						pe.Graphics.DrawString(this.Text, this.Font, new SolidBrush(mDownTextShadowColor), sRect, format);
				}

				if (Enabled)
				{
					if (!mDown)
						pe.Graphics.DrawString(this.Text, this.Font, new SolidBrush(base.ForeColor), rect, format);
					else
						pe.Graphics.DrawString(this.Text, this.Font, new SolidBrush(mDownTextColor), rect, format);
				}
				else
				{
					pe.Graphics.DrawString(this.Text, this.Font, new SolidBrush(mDisabledTextColor), rect, format);
				}


				/*
				SolidBrush drawBrush = new SolidBrush(base.ForeColor);
				SizeF drawStringSize = pe.Graphics.MeasureString(base.Text, base.Font);
				PointF drawPoint;

				if (base.Image != null)
				{
					drawPoint = new PointF(base.Image.Width / 2 - drawStringSize.Width / 2, base.Image.Height / 2 - drawStringSize.Height / 2);
				}
				else
				{
					drawPoint = new PointF(base.Width / 2 - drawStringSize.Width / 2, base.Height / 2 - drawStringSize.Height / 2);
				}

				pe.Graphics.DrawString(base.Text, base.Font, drawBrush, drawPoint);
				 * */
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			Refresh();
			base.OnTextChanged(e);
		}

		Image GetResourceImage(string name)
		{
			Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			System.IO.Stream s = assembly.GetManifestResourceStream(name);
			return Image.FromStream(s);
		}

		#endregion
	}
}
