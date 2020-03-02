using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FadeFox.UI
{
	public enum Corner
	{
		None = 0,
		TopLeft = 1,
		TopRight = 2,
		BottomLeft = 4,
		BottomRight = 8,
		All = TopLeft | TopRight | BottomLeft | BottomRight,
		AllTop = TopLeft | TopRight,
		AllLeft = TopLeft | BottomLeft,
		AllRight = TopRight | BottomRight,
		AllBottom = BottomLeft | BottomRight
	}

	public class Round
	{
		public static GraphicsPath RoundCorners(RectangleF rect)
		{
			return RoundCorners(rect, 5, Corner.All);
		}

		public static GraphicsPath RoundCorners(RectangleF rect, int radius, Corner corners)
		{
			GraphicsPath p = new GraphicsPath();
			Single x = rect.X;
			Single y = rect.Y;
			Single w = rect.Width;
			Single h = rect.Height;
			int r = radius;

			p.StartFigure();
			
			//top left arc
			if (Convert.ToBoolean(corners & Corner.TopLeft))
			{
				p.AddArc(new RectangleF(x, y, 2 * r, 2 * r), 180, 90);
			}
			else
			{
				p.AddLine(new PointF(x, y + r), new PointF(x, y));
				p.AddLine(new PointF(x, y), new PointF(x + r, y));
			}

			//top line
			p.AddLine(new PointF(x + r, y), new PointF(x + w - r, y));

			//top right arc
			if (Convert.ToBoolean(corners & Corner.TopRight))
			{
				p.AddArc(new RectangleF(x + w - 2 * r, y, 2 * r, 2 * r), 270, 90);
			}
			else
			{
				p.AddLine(new PointF(x + w - r, y), new PointF(x + w, y));
				p.AddLine(new PointF(x + w, y), new PointF(x + w, y + r));
			}

			//right line
			p.AddLine(new PointF(x + w, y + r), new PointF(x + w, y + h - r));

			//bottom right arc
			if (Convert.ToBoolean(corners & Corner.BottomRight))
			{
				p.AddArc(new RectangleF(x + w - 2 * r, y + h - 2 * r, 2 * r, 2 * r), 0, 90);
			}
			else
			{
				p.AddLine(new PointF(x + w, y + h - r), new PointF(x + w, y + h));
				p.AddLine(new PointF(x + w, y + h), new PointF(x + w - r, y + h));
			}

			//bottom line
			p.AddLine(new PointF(x + w - r, y + h), new PointF(x + r, y + h));

			//bottom left arc
			if (Convert.ToBoolean(corners & Corner.BottomLeft))
			{
				p.AddArc(new RectangleF(x, y + h - 2 * r, 2 * r, 2 * r), 90, 90);
			}
			else
			{
				p.AddLine(new PointF(x + r, y + h), new PointF(x, y + h));
				p.AddLine(new PointF(x, y + h), new PointF(x, y + h - r));
			}
 
			//left line
			p.AddLine(new PointF(x, y + h - r), new PointF(x, y + r));

			//close figure...
			p.CloseFigure();

			return p;
		}
	}
}
