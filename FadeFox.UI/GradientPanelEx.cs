/* 
 * ==============================================================================
 *   Program ID     : 
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : sayou.net
 *   Developer      : Hyosik-Bae
 *   Create Date    : 2010-08-18
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Diagnostics;


namespace FadeFox.UI
{
	public enum ShadowLocation
	{
		Top,
		Bottom
	}

	[ToolboxBitmap(typeof(System.Windows.Forms.Panel))]
	public partial class GradientPanelEx : Panel
	{
		private const int DEFAULT_BORDER_WIDTH = 1;
		private const int DEFAULT_SHADOW_WIDTH = 5;
		
		#region Member Variables

		private Color mclr_StartColor;
		private Color mclr_EndColor;
		private LinearGradientMode menm_GradientMode;

		//private ShadowLocation mShadowLocation = ShadowLocation.Bottom;

		private Color mclr_BorderColor;

		#endregion

		#region Constructor

		public GradientPanelEx() : base()
		{
			InitializeComponent();

			base.BorderStyle = 0;
			this.BorderStyle = BorderStyle.None;
			//this.mclr_BorderColor = Color.FromKnownColor(KnownColor.ActiveCaption);
			this.mclr_BorderColor = Color.FromArgb(165, 172, 181);

			this.BackColor = Color.FromKnownColor(KnownColor.Window);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);

			this.mclr_StartColor = Color.FromKnownColor(KnownColor.Window);
			this.mclr_EndColor = Color.FromKnownColor(KnownColor.Window);
			this.menm_GradientMode = LinearGradientMode.Vertical;
		}

		#endregion

		#region Properties

		[Description("Change the starting color of gradient background"), Category("Gradient"), Browsable(true)]
		public Color GradientStart
		{
			get
			{
				return mclr_StartColor;
			}
			set
			{
				mclr_StartColor = value;
				Invalidate();
			}
		}

		[Description("Change the end color of gradient background"), Category("Gradient"), Browsable(true)]
		public Color GradientEnd
		{
			get
			{
				return mclr_EndColor;
			}
			set
			{
				mclr_EndColor = value;
				Invalidate();
			}
		}

		[Description("Change the background gradient direction"), Category("Gradient"), Browsable(true)]
		public LinearGradientMode GradientDirection
		{
			get
			{
				return menm_GradientMode;
			}
			set
			{
				menm_GradientMode = value;
				Invalidate();
			}
		}

		[Description("Change color of the border"), Category("Border"), Browsable(true)]
		public Color BorderColor
		{
			get
			{
				return mclr_BorderColor;
			}
			set
			{
				mclr_BorderColor = value;
				Invalidate();
			}
		}


		#endregion

		#region Overrided Methods

		public override Rectangle DisplayRectangle
		{
			get
			{
				int shadowWidth = DEFAULT_SHADOW_WIDTH;
				int borderWidth = DEFAULT_BORDER_WIDTH;

				Rectangle rect = new Rectangle();

				// rect.X = borderWidth + shadowWidth - 1;
				// rect.Y = borderWidth + shadowWidth - 1;
				// rect.Width = this.Width - ((2 * borderWidth) + (2 * shadowWidth)) + 2;
				// rect.Height = this.Height - ((2 * borderWidth) + (2 * shadowWidth)) + 2;

				rect.X = 0;
				rect.Y = 0;
				rect.Width = this.Width;
				rect.Height = this.Height - ((borderWidth) + (shadowWidth));

				return rect;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			Invalidate();
			base.OnResize(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			DrawShadow(e);

			if (Width == 0 || Height == 0) return;

			if (BackgroundImage == null)
			{
				DrawBackground(e);
			}

			DrawBorder(e);
		}
		#endregion

		#region Helper Routines

		private void DrawShadow(PaintEventArgs e)
		{
			Pen pen = null;
			Color[] clrArr = null;

			int shadowWidth = DEFAULT_SHADOW_WIDTH;

			try
			{
				clrArr = new Color[] {Color.FromArgb(150, 209, 213, 217), 
                                    Color.FromArgb(150, 214, 218, 222),
                                    Color.FromArgb(150, 219, 223, 227), 
                                    Color.FromArgb(150, 224, 228, 232),
									Color.FromArgb(150, 229, 233, 237)};

				for (int cntr = 0; cntr < 5; cntr++)
				{
					pen = new Pen(clrArr[cntr], 2.0f);

					e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

					//GraphicsPath path = Round.RoundCorners(new RectangleF((shadowWidth - cntr), (shadowWidth - cntr), (this.Width - ((2 * shadowWidth) - (2 * cntr))), (this.Height - ((2 * shadowWidth) - (2 * cntr)))), 3, Corner.All);
					//e.Graphics.DrawPath(pen, path);
					e.Graphics.DrawLine(pen, 0, this.Height - (shadowWidth - cntr), this.Width, this.Height - (shadowWidth - cntr));
					

					e.Graphics.SmoothingMode = SmoothingMode.Default;

					pen.Dispose();
					pen = null;
				}
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
					pen = null;
				}
				clrArr = null;
			}
		}

		private void DrawBackground(PaintEventArgs peArgs)
		{
			//SolidBrush brsh = null;
			LinearGradientBrush brsh = null;

			try
			{
				int shadowWidth = DEFAULT_SHADOW_WIDTH;
				//Rectangle rect = new Rectangle(shadowWidth, shadowWidth, this.Width - (2 * shadowWidth), this.Height - (2 * shadowWidth));
				Rectangle rect = new Rectangle(0, 0, this.Width, this.Height - (shadowWidth));


				//brsh = new SolidBrush(this.BackColor);

				Point startPoint2 = new Point(0, 0);
				Point endPoint2 = new Point(0, this.Height);
				float[] myFactors = { 0.0f, 0.0f, 0.13f, 0.2f};
				float[] myPositions = { 0.0f, 0.4f, 0.9f, 1.0f};
				Blend myBlend = new Blend();
				myBlend.Factors = myFactors;
				myBlend.Positions = myPositions;


				//brsh = new LinearGradientBrush(rect, mclr_StartColor, mclr_EndColor, menm_GradientMode);

				brsh = new LinearGradientBrush(startPoint2,	endPoint2, Color.White,	Color.DimGray);

				brsh.Blend = myBlend;
	
				peArgs.Graphics.FillRectangle(brsh, rect);
			}
			finally
			{
				if (brsh != null)
				{
					brsh.Dispose();
					brsh = null;
				}
			}
		}

		private void DrawBorder(PaintEventArgs peArgs)
		{
			Pen penDark = null;
			Pen penLight = null;

			try
			{
				int shadowWidth = DEFAULT_SHADOW_WIDTH;

				penDark = new Pen(mclr_BorderColor);
				//peArgs.Graphics.DrawRectangle(penDark, shadowWidth, shadowWidth, this.Width - ((shadowWidth + 1) * 2) + 1, this.Height - ((shadowWidth + 1) * 2) + 1);
				peArgs.Graphics.DrawLine(penDark, 0, this.Height - (shadowWidth + 1), this.Width, this.Height - (shadowWidth + 1));

			}
			finally
			{
				if (penDark != null)
				{
					penDark.Dispose();
					penDark = null;
				}
				if (penLight != null)
				{
					penLight.Dispose();
					penLight = null;
				}
			}
		}

		#endregion
	}
}
