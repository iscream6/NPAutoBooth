using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace FadeFox.UI
{
	public class UICore
	{
		public static void FullScreenCapture(string pFileName, ImageFormat pImageFormat)
		{
			int width = Screen.PrimaryScreen.Bounds.Width;
			int height = Screen.PrimaryScreen.Bounds.Height;

			Size size = new Size(width, height);
			Bitmap bitmap = new Bitmap(width, height);
			Graphics g = Graphics.FromImage(bitmap);
			g.CopyFromScreen(0, 0, 0, 0, size);

			// Save  
			bitmap.Save(pFileName, pImageFormat); 
		}
	}
}
