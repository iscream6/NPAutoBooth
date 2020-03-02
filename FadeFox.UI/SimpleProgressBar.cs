using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace FadeFox.UI
{
	[ToolboxBitmap(typeof(System.Windows.Forms.ProgressBar))]
	public partial class SimpleProgressBar : Control
	{
		private Color mHighColor = Color.FromArgb(170, 240, 170);
		private Color mLowColor = Color.FromArgb(10, 150, 10);

		private Color mColorText = Color.Black;

		private Image mDobleBack = null;

		private int mMaximum = 100;
		private int mMinimum = 0;
		private int mValue = 50;

		private LinearGradientBrush mForeBrush;
		private LinearGradientBrush mBackBrush;

		private Pen mPenOut = new Pen(Color.FromArgb(104, 104, 104));

		private Rectangle mOutnnerRectangle;
		private Rectangle mDisplayRectangle;

		public SimpleProgressBar()
		{
			InitializeComponent();
		}

		[Description("The Border Color of the gradient in the Progress Bar")]
		public Color GradientHighColor
		{
			get { return mHighColor; }
			set
			{
				mHighColor = value;
				this.InvalidateBuffer(true);
			}
		}


		[Description("The Center Color of the gradient in the Progress Bar")]
		public Color GradientLowColor
		{
			get { return mLowColor; }
			set
			{
				mLowColor = value;
				this.InvalidateBuffer(true);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Description("Set to TRUE to reset all colors like the Windows XP Progress Bar")]
		[DefaultValue(false)]
		public bool ColorsXP
		{
			get { return false; }
			set
			{
				GradientHighColor = Color.FromArgb(170, 240, 170);
				GradientLowColor = Color.FromArgb(10, 150, 10);
			}
		}


		[Description("The Color of the text displayed in the Progress Bar")]
		public Color ColorText
		{
			get { return mColorText; }
			set
			{
				mColorText = value;

				if (this.Text != String.Empty)
				{
					this.Invalidate();
				}
			}
		}

		[RefreshProperties(RefreshProperties.Repaint)]
		[Description("The Current Position of the Progress Bar")]
		public int Value
		{
			get { return mValue; }
			set
			{
				if (value > mMaximum)
				{
					mValue = mMaximum;
				}
				else if (value < mMinimum)
				{
					mValue = mMinimum;
				}
				else
				{
					mValue = value;
				}
				this.Invalidate();
			}
		}

		[RefreshProperties(RefreshProperties.Repaint)]
		[Description("The Max Position of the Progress Bar")]
		public int Maximum
		{
			get { return mMaximum; }
			set
			{
				if (value > mMinimum)
				{
					mMaximum = value;

					if (mValue > mMaximum)
					{
						Value = mMaximum;
					}

					this.InvalidateBuffer(true);
				}
			}
		}

		[RefreshProperties(RefreshProperties.Repaint)]
		[Description("The Min Position of the Progress Bar")]
		public int Minimum
		{
			get { return mMinimum; }
			set
			{
				if (value < mMaximum)
				{
					mMinimum = value;

					if (mValue < mMinimum)
					{
						Value = mMinimum;
					}
					this.InvalidateBuffer(true);
				}
			}
		}


		[Description("The Text displayed in the Progress Bar")]
		[DefaultValue("")]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				if (base.Text != value)
				{
					base.Text = value;
					this.Invalidate();
				}
			}
		}

		private bool mTextShadow = true;

		[Description("Set the Text shadow in the Progress Bar")]
		[DefaultValue(true)]
		public bool TextShadow
		{
			get { return mTextShadow; }
			set
			{
				mTextShadow = value;
				this.Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (!this.IsDisposed)
			{
				if (mDobleBack == null)
				{
					mDobleBack = new Bitmap(this.Width, this.Height);

					Graphics g2 = Graphics.FromImage(mDobleBack);

					mOutnnerRectangle = new Rectangle(
					   this.ClientRectangle.X,
					   this.ClientRectangle.Y,
					   this.ClientRectangle.Width - 1,
					   this.ClientRectangle.Height - 1);

					mDisplayRectangle = new Rectangle(
					   this.ClientRectangle.X + 1,
					   this.ClientRectangle.Y + 1,
					   this.ClientRectangle.Width - 2,
					   this.ClientRectangle.Height - 2);

					if (mForeBrush != null)
					{
						mForeBrush.Dispose();
						mForeBrush = null;
					}

					if (mBackBrush != null)
					{
						mBackBrush.Dispose();
						mBackBrush = null;
					}

					mForeBrush = new LinearGradientBrush(this.ClientRectangle, mHighColor, mLowColor, LinearGradientMode.Vertical);
					mBackBrush = new LinearGradientBrush(this.ClientRectangle, Color.White, Color.Silver, LinearGradientMode.Vertical);

					g2.Clear(Color.White);
					g2.DrawRectangle(mPenOut, mOutnnerRectangle);
					g2.Dispose();
				}

				Image ima = new Bitmap(mDobleBack);

				Graphics gtemp = Graphics.FromImage(ima);

				double maxValue = mMaximum - mMinimum;
				double currValue = mValue - mMinimum;

				double rate = currValue / maxValue;
				int currPos = (int)((double)mDisplayRectangle.Width * rate);

				gtemp.FillRectangle(mBackBrush, mDisplayRectangle);
				gtemp.FillRectangle(mForeBrush, mDisplayRectangle.X, mDisplayRectangle.Y, currPos, mDisplayRectangle.Height);

				if (this.Text != String.Empty)
				{
					DrawCenterString(gtemp, this.ClientRectangle);
				}

				e.Graphics.DrawImage(ima, e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle, GraphicsUnit.Pixel);
				ima.Dispose();
				gtemp.Dispose();
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			if (!this.IsDisposed)
			{
				if (this.Height < 12)
				{
					this.Height = 12;
				}

				base.OnSizeChanged(e);
				this.InvalidateBuffer(true);
			}

		}

		protected override Size DefaultSize
		{
			get { return new Size(100, 29); }
		}

		private void InvalidateBuffer()
		{
			InvalidateBuffer(false);
		}

		private void InvalidateBuffer(bool InvalidateControl)
		{
			if (mDobleBack != null)
			{
				mDobleBack.Dispose();
				mDobleBack = null;
			}

			if (InvalidateControl)
			{
				this.Invalidate();
			}
		}

		private void DrawCenterString(Graphics gfx, Rectangle box)
		{
			SizeF ss = gfx.MeasureString(this.Text, this.Font);

			float left = box.X + (box.Width - ss.Width) / 2;
			float top = box.Y + (box.Height - ss.Height) / 2;

			if (mTextShadow)
			{
				SolidBrush mShadowBrush = new SolidBrush(Color.FromArgb(150, Color.Gray));
				gfx.DrawString(this.Text, this.Font, mShadowBrush, left + 1, top + 2);
				mShadowBrush.Dispose();
			}

			SolidBrush mTextBrush = new SolidBrush(mColorText);
			gfx.DrawString(this.Text, this.Font, mTextBrush, left, top + 1);
			mTextBrush.Dispose();
		}
	}
}
