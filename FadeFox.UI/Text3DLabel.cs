using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace FadeFox.UI
{
	[ToolboxBitmap(typeof(System.Windows.Forms.Label))]
	public partial class Text3DLabel : UserControl
	{
		protected HorizontalAlignment mTextAlignment = HorizontalAlignment.Center;
		private string mText = string.Empty;
		private Color mBorderColor = Color.DarkGray;
		private Color mBorderHotColor = Color.Green;
		private Color mText3DColor = Color.Gainsboro;
		private Color mTextShadowColor = Color.Transparent;
		private Color mTextColor = Color.Black;

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

		public Text3DLabel()
		{
			InitializeComponent();

			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.Selectable, false);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.UpdateStyles();
		}

		public Color TextColor
		{
			get { return mTextColor; }
			set
			{
				mTextColor = value;
				this.Invalidate();
			}
		}

		public Color Text3DColor
		{
			get { return mText3DColor; }
			set
			{
				mText3DColor = value;
				this.Invalidate();
			}
		}

		public Color TextShadowColor
		{
			get { return mTextShadowColor; }
			set
			{
				mTextShadowColor = value;
				this.Invalidate();
			}
		}


		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public override string Text
		{
			get
			{
				return mText;
			}
			set
			{
				mText = value;
				this.Invalidate();
			}
		}

		public HorizontalAlignment TextAlign
		{
			get { return mTextAlignment; }
			set
			{
				mTextAlignment = value;
				this.Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

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

			//e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			Rectangle sRect = new Rectangle(new Point(rect.X + 2, rect.Y + mTextTopMargin + 2), rect.Size);
			e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(mTextShadowColor), sRect, format);

			Rectangle dRect = new Rectangle(new Point(rect.X + 1, rect.Y + mTextTopMargin + 1), rect.Size);
			e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(mText3DColor), dRect, format);

			Rectangle oRect = new Rectangle(new Point(rect.X, rect.Y + mTextTopMargin), rect.Size);
			e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(mTextColor), oRect, format);

			//e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
		}
	}
}
