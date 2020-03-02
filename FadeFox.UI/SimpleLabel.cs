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
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace FadeFox.UI
{
	[ToolboxBitmap(typeof(System.Windows.Forms.Label))]
	public partial class SimpleLabel : Label
	{
		private const int DEFAULT_BORDER_WIDTH = 1;
		private string mPasswordChar = "";
		public string PasswordChar
		{
			get { return mPasswordChar; }
			set 
			{
				mPasswordChar = value;
				ChangeDisplay();
			}
		}

		private string mText = "";

		private Color _borderColor = Color.DimGray;

		public SimpleLabel()
		{
			InitializeComponent();
		}

		[Description("선 색 변경"), Category("모양"), Browsable(true)]
		public Color BorderColor
		{
			get
			{
				return _borderColor;
			}
			set
			{
				_borderColor = value;
				Invalidate();
			}
		}

		private void DrawBorder(PaintEventArgs peArgs)
		{
			Pen pen = null;

			try
			{
				pen = new Pen(_borderColor);
				peArgs.Graphics.DrawRectangle(pen, 0, 0,
									Width - DEFAULT_BORDER_WIDTH,
									Height - DEFAULT_BORDER_WIDTH);
			}
			finally
			{
				if (pen != null)
				{
					pen.Dispose();
					pen = null;
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (Width == 0 || Height == 0)
				return;

			base.OnPaint(e);

			DrawBorder(e);
		}

		public new string Text
		{
			get { return mText; }
			set
			{
				mText = value;
				ChangeDisplay();
			}
		}

		private void ChangeDisplay()
		{
			if (mPasswordChar == "")
				base.Text = mText;
			else
			{
				string pc = "";

				for (int i = 0; i < mText.Length; i++)
				{
					pc += mPasswordChar;
				}

				base.Text = pc;
			}
		}

		public void PerformClick()
		{
			OnClick(new EventArgs());
		}
	}
}
