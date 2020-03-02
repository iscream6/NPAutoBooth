/* 
 * ==============================================================================
 *   Program ID     : 
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : sayou.net
 *   Developer      : 
 *   Create Date    : 2007-05-18
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
	[ToolboxBitmap(typeof(System.Windows.Forms.Panel))]
	public partial class SimplePanel : Panel
	{
		private int mBorder1WidthTop = 1;
		private int mBorder1WidthBottom = 1;
		private int mBorder1WidthLeft = 1;
		private int mBorder1WidthRight = 1;

		private int mBorder1OffsetTop = 0;
		private int mBorder1OffsetBottom = 1;
		private int mBorder1OffsetLeft = 0;
		private int mBorder1OffsetRight = 1;

		private Color mBorder1ColorTop = Color.Gray;
		private Color mBorder1ColorBottom = Color.Gray;
		private Color mBorder1ColorLeft = Color.Gray;
		private Color mBorder1ColorRight = Color.Gray;

		private Pen mBorder1PenTop = null;
		private Pen mBorder1PenBottom = null;
		private Pen mBorder1PenLeft = null;
		private Pen mBorder1PenRight = null;

		private int mBorder2WidthTop = 0;
		private int mBorder2WidthBottom = 0;
		private int mBorder2WidthLeft = 0;
		private int mBorder2WidthRight = 0;

		private int mBorder2OffsetTop = 0;
		private int mBorder2OffsetBottom = 0;
		private int mBorder2OffsetLeft = 0;
		private int mBorder2OffsetRight = 0;

		private Color mBorder2ColorTop = Color.Gray;
		private Color mBorder2ColorBottom = Color.Gray;
		private Color mBorder2ColorLeft = Color.Gray;
		private Color mBorder2ColorRight = Color.Gray;

		private Pen mBorder2PenTop = null;
		private Pen mBorder2PenBottom = null;
		private Pen mBorder2PenLeft = null;
		private Pen mBorder2PenRight = null;

		public SimplePanel()
			: base()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.Opaque, false);

			this.BackColor = Color.White;

			CreateGraphicObject();
		}

		#region *    Border 1    *

		public Color Border1ColorTop
		{
			get { return mBorder1ColorTop; }
			set
			{
				mBorder1ColorTop = value;

				CreateGraphicObject();

				Invalidate();
			}
		}

		public Color Border1ColorBottom
		{
			get { return mBorder1ColorBottom; }
			set
			{
				mBorder1ColorBottom = value;

				CreateGraphicObject();

				Invalidate();
			}
		}

		public Color Border1ColorLeft
		{
			get { return mBorder1ColorLeft; }
			set
			{
				mBorder1ColorLeft = value;

				CreateGraphicObject();

				Invalidate();
			}
		}

		public Color Border1ColorRight
		{
			get { return mBorder1ColorRight; }
			set
			{
				mBorder1ColorRight = value;

				CreateGraphicObject();

				Invalidate();
			}
		}

		public int Border1WidthTop
		{
			get { return mBorder1WidthTop; }
			set
			{
				mBorder1WidthTop = value;

				if (mBorder1WidthTop % 2 == 0)
					mBorder1OffsetTop = mBorder1WidthTop / 2;
				else
					mBorder1OffsetTop = (mBorder1WidthTop - 1) / 2;

				CreateGraphicObject();
				Invalidate();
			}
		}

		public int Border1WidthBottom
		{
			get { return mBorder1WidthBottom; }
			set
			{
				mBorder1WidthBottom = value;

				if (mBorder1WidthBottom % 2 == 0)
					mBorder1OffsetBottom = mBorder1WidthBottom / 2;
				else
					mBorder1OffsetBottom = (mBorder1WidthBottom + 1) / 2;

				CreateGraphicObject();
				Invalidate();
			}
		}

		public int Border1WidthLeft
		{
			get { return mBorder1WidthLeft; }
			set
			{
				mBorder1WidthLeft = value;

				if (mBorder1WidthLeft % 2 == 0)
					mBorder1OffsetLeft = mBorder1WidthLeft / 2;
				else
					mBorder1OffsetLeft = (mBorder1WidthLeft - 1) / 2;

				CreateGraphicObject();
				Invalidate();
			}
		}

		public int Border1WidthRight
		{
			get { return mBorder1WidthRight; }
			set
			{
				mBorder1WidthRight = value;

				if (mBorder1WidthRight % 2 == 0)
					mBorder1OffsetRight = mBorder1WidthRight / 2;
				else
					mBorder1OffsetRight = (mBorder1WidthRight + 1) / 2;

				CreateGraphicObject();
				Invalidate();
			}
		}

#endregion

		#region *    Border 2    *

		public Color Border2ColorTop
		{
			get { return mBorder2ColorTop; }
			set
			{
				mBorder2ColorTop = value;

				CreateGraphicObject();

				Invalidate();
			}
		}

		public Color Border2ColorBottom
		{
			get { return mBorder2ColorBottom; }
			set
			{
				mBorder2ColorBottom = value;

				CreateGraphicObject();

				Invalidate();
			}
		}

		public Color Border2ColorLeft
		{
			get { return mBorder2ColorLeft; }
			set
			{
				mBorder2ColorLeft = value;

				CreateGraphicObject();

				Invalidate();
			}
		}

		public Color Border2ColorRight
		{
			get { return mBorder2ColorRight; }
			set
			{
				mBorder2ColorRight = value;

				CreateGraphicObject();

				Invalidate();
			}
		}

		public int Border2WidthTop
		{
			get { return mBorder2WidthTop; }
			set
			{
				mBorder2WidthTop = value;

				if (mBorder2WidthTop % 2 == 0)
					mBorder2OffsetTop = mBorder2WidthTop / 2;
				else
					mBorder2OffsetTop = (mBorder2WidthTop - 1) / 2;

				CreateGraphicObject();
				Invalidate();
			}
		}

		public int Border2WidthBottom
		{
			get { return mBorder2WidthBottom; }
			set
			{
				mBorder2WidthBottom = value;

				if (mBorder2WidthBottom % 2 == 0)
					mBorder2OffsetBottom = mBorder2WidthBottom / 2;
				else
					mBorder2OffsetBottom = (mBorder2WidthBottom + 1) / 2;

				CreateGraphicObject();
				Invalidate();
			}
		}

		public int Border2WidthLeft
		{
			get { return mBorder2WidthLeft; }
			set
			{
				mBorder2WidthLeft = value;

				if (mBorder2WidthLeft % 2 == 0)
					mBorder2OffsetLeft = mBorder2WidthLeft / 2;
				else
					mBorder2OffsetLeft = (mBorder2WidthLeft - 1) / 2;

				CreateGraphicObject();
				Invalidate();
			}
		}

		public int Border2WidthRight
		{
			get { return mBorder2WidthRight; }
			set
			{
				mBorder2WidthRight = value;

				if (mBorder2WidthRight % 2 == 0)
					mBorder2OffsetRight = mBorder2WidthRight / 2;
				else
					mBorder2OffsetRight = (mBorder2WidthRight + 1) / 2;

				CreateGraphicObject();
				Invalidate();
			}
		}

		#endregion


		private void CreateGraphicObject()
		{
			#region *    Border 1    *
			if (mBorder1PenTop != null)
			{
				mBorder1PenTop.Dispose();
				mBorder1PenTop = null;
			}
			if (mBorder1WidthTop > 0)
				mBorder1PenTop = new Pen(mBorder1ColorTop, mBorder1WidthTop);

			if (mBorder1PenBottom != null)
			{
				mBorder1PenBottom.Dispose();
				mBorder1PenBottom = null;
			}
			if (mBorder1WidthBottom > 0)
				mBorder1PenBottom = new Pen(mBorder1ColorBottom, mBorder1WidthBottom);

			if (mBorder1PenLeft != null)
			{
				mBorder1PenLeft.Dispose();
				mBorder1PenLeft = null;
			}
			if (mBorder1WidthLeft > 0)
				mBorder1PenLeft = new Pen(mBorder1ColorLeft, mBorder1WidthLeft);

			if (mBorder1PenRight != null)
			{
				mBorder1PenRight.Dispose();
				mBorder1PenRight = null;
			}
			if (mBorder1WidthRight > 0)
				mBorder1PenRight = new Pen(mBorder1ColorRight, mBorder1WidthRight);

			#endregion

			#region *    Border 2    *

			if (mBorder2PenTop != null)
			{
				mBorder2PenTop.Dispose();
				mBorder2PenTop = null;
			}
			if (mBorder2WidthTop > 0)
				mBorder2PenTop = new Pen(mBorder2ColorTop, mBorder2WidthTop);

			if (mBorder2PenBottom != null)
			{
				mBorder2PenBottom.Dispose();
				mBorder2PenBottom = null;
			}
			if (mBorder2WidthBottom > 0)
				mBorder2PenBottom = new Pen(mBorder2ColorBottom, mBorder2WidthBottom);

			if (mBorder2PenLeft != null)
			{
				mBorder2PenLeft.Dispose();
				mBorder2PenLeft = null;
			}
			if (mBorder2WidthLeft > 0)
				mBorder2PenLeft = new Pen(mBorder2ColorLeft, mBorder2WidthLeft);

			if (mBorder2PenRight != null)
			{
				mBorder2PenRight.Dispose();
				mBorder2PenRight = null;
			}
			if (mBorder2WidthRight > 0)
				mBorder2PenRight = new Pen(mBorder2ColorRight, mBorder2WidthRight);

			#endregion
		}

		private void DrawBorder(PaintEventArgs peArgs)
		{
			#region *    Border 1    *
			// Top
			if (mBorder1WidthTop > 0)
				peArgs.Graphics.DrawLine(mBorder1PenTop, 0, mBorder1OffsetTop, this.Width, mBorder1OffsetTop);

			// Bottom
			if (mBorder1WidthBottom > 0)
				peArgs.Graphics.DrawLine(mBorder1PenBottom, 0, this.Height - mBorder1OffsetBottom, this.Width, this.Height - mBorder1OffsetBottom);

			// Left
			if (mBorder1WidthLeft > 0)
				peArgs.Graphics.DrawLine(mBorder1PenLeft, mBorder1OffsetLeft, 0, mBorder1OffsetLeft, this.Height);

			// Right
			if (mBorder1WidthRight > 0)
				peArgs.Graphics.DrawLine(mBorder1PenRight, this.Width - mBorder1OffsetRight, 0, this.Width - mBorder1OffsetRight, this.Height);

			#endregion

			#region *    Border 2    *
			// Top
			if (mBorder2WidthTop > 0)
				peArgs.Graphics.DrawLine(mBorder2PenTop, 0, mBorder2OffsetTop, this.Width, mBorder2OffsetTop);

			// Bottom
			if (mBorder2WidthBottom > 0)
				peArgs.Graphics.DrawLine(mBorder2PenBottom, 0, this.Height - mBorder2OffsetBottom, this.Width, this.Height - mBorder2OffsetBottom);

			// Left
			if (mBorder2WidthLeft > 0)
				peArgs.Graphics.DrawLine(mBorder2PenLeft, mBorder2OffsetLeft, 0, mBorder2OffsetLeft, this.Height);

			// Right
			if (mBorder2WidthRight > 0)
				peArgs.Graphics.DrawLine(mBorder2PenRight, this.Width - mBorder2OffsetRight, 0, this.Width - mBorder2OffsetRight, this.Height);

			#endregion


			//if (mBorderWidth1 > 0)
			//{
			//    // Top
			//    if (mBorder1WidthTop > 0)
			//    //peArgs.Graphics.DrawLine(mBorderPen1, 0, mLineTopOffset1, this.Width, mLineTopOffset1);

			//    // Bottom
			//    peArgs.Graphics.DrawLine(mBorderPen1, 0, this.Height - mLineTopOffset1, this.Width, this.Height - mLineTopOffset1);

			//    //peArgs.Graphics.DrawLine(mBorderPen1, mLineTopOffset1, mLineTopOffset1, mLineTopOffset1, this.Height);
			//    //peArgs.Graphics.DrawLine(mBorderPen1, this.Width - mBorderWidth1, mLineTopOffset1, this.Width - mBorderWidth1, this.Height);


			//    //peArgs.Graphics.DrawRectangle(mBorderPen1, mLineTopOffset1, mLineTopOffset1, this.Width - mBorderWidth1, this.Height - mBorderWidth1);
			//}

			//if (mBorderWidth2 > 0)
			//{
			//    peArgs.Graphics.DrawLine(mBorderPen2, mLineTopOffset2, mLineTopOffset2, this.Width, mLineTopOffset2);
			//    peArgs.Graphics.DrawLine(mBorderPen2, mLineTopOffset2, this.Height - mBorderWidth2, this.Width, this.Height - mBorderWidth2);
			//    peArgs.Graphics.DrawLine(mBorderPen2, mLineTopOffset2, mLineTopOffset2, mLineTopOffset2, this.Height);
			//    peArgs.Graphics.DrawLine(mBorderPen2, this.Width - mBorderWidth2, mLineTopOffset2, this.Width - mBorderWidth2, this.Height);
			//    //peArgs.Graphics.DrawRectangle(mBorderPen2, mBorderWidth1 + mLineTopOffset2, mBorderWidth1 + mLineTopOffset2, this.Width - (mBorderWidth1 * 2) - mBorderWidth2, this.Height - (mBorderWidth1 * 2) - mBorderWidth2);
			//}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (Width == 0 || Height == 0)
				return;

			base.OnPaint(e);

			DrawBorder(e);
		}

		protected override void OnResize(EventArgs eventargs)
		{
			base.OnResize(eventargs);
			Invalidate();
		}
	}
}
