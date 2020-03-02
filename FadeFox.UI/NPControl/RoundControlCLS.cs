using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Drawing;

namespace FadeFox.UI
{
    public sealed class RoundControlCLS
    {
        #region Methods

        [DebuggerHidden()]
        private RoundControlCLS() { }

        [DebuggerHidden()]
        public static GraphicsPath RoundedRectangle(Rectangle rect, Single radius, PropertyListBAS.ENUM_CORNER exclude)
        {
            if (radius < 0.0F)
            {
                throw new ArgumentException("Invalid argument. The value cannot be negativ.", "radius");
            }

            if (rect.IsEmpty)
            {
                throw new ArgumentException("Invalid argument. The rectangle cannot be empty.", "rect");
            }

            //Return-rounded rectangle path.
            return RoundControlCLS.GetPath(rect.X, rect.Y, rect.Width, rect.Height, radius, exclude);
        }

        [DebuggerHidden()]
        public static GraphicsPath RoundedRectangle(Rectangle rect, int radius, PropertyListBAS.ENUM_CORNER exclude)
        {
            //if(radius is less than zero, 
            //throw an ArgumentException.
            if (radius < 0.0F)
            {
                throw new ArgumentException("Invalid argument. The value cannot be negativ.", "radius");
            }

            //if(the rectangle object is empty, 
            //than throw an ArgumentException.
            if (rect.IsEmpty)
            {
                throw new ArgumentException("Invalid argument. The rectangle cannot be empty.", "rect");
            }

            //Return-rounded rectangle path.
            return RoundControlCLS.GetPath((Single)rect.X, (Single)rect.Y, (Single)rect.Width, (Single)rect.Height, (Single)radius, exclude);
        }

        /// <summary>
        /// Creats and returns a rounded corner region.
        /// </summary>
        /// <param name="rSize">The size of the region.</param>
        /// <param name="radius">The radius of the rounded corners. This value should be 
        /// bigger than or equal to 0 and less or equal to the half of the smallest value 
        /// of the region뭩 side.</param>
        /// <param name="exclude">A value that specifies the corners that should be excluded from rounding. This value 
        /// can be one or combination of the Shape.Corner enumeration value combined by "OR".</param>
        [DebuggerHidden()]
        public static Region RoundedRegion(SizeF rSize, Single radius, PropertyListBAS.ENUM_CORNER exclude)
        {
            //if(radius is less than zero, 
            //throw an ArgumentException.
            if (radius < 0.0F)
            {
                throw new ArgumentException("Invalid argument. The value cannot be negativ.", "radius");
            }

            //if(the rectangle object is empty, 
            //than throw an ArgumentException.
            if (rSize.IsEmpty)
            {
                throw new ArgumentException("Invalid argument. The rectangle cannot be empty.", "rSize");
            }

            //Return-rounded rectangle region.
            return RoundControlCLS.GetRegion(0.0F, 0.0F, rSize.Width, rSize.Height, radius, exclude);
        }

        /// <summary>
        /// Creats and returns a rounded corner region.
        /// </summary>
        /// <param name="rSize">The size of the region.</param>
        /// <param name="radius">The radius of the rounded corners. This value should be 
        /// bigger than or equal to 0 and less or equal to the half of the smallest value 
        /// of the region뭩 side.</param>
        /// <param name="exclude">A value that specifies the corners that should be excluded from rounding. This value 
        /// can be one or combination of the Shape.Corner enumeration value combined by "OR".</param>
        [DebuggerHidden()]
        public static Region RoundedRegion(Size rSize, int radius, PropertyListBAS.ENUM_CORNER exclude)
        {
            //if(radius is less than zero, 
            //throw an ArgumentException.
            if (radius < 0.0F)
            {
                throw new ArgumentException("Invalid argument. The value cannot be negativ.", "radius");
            }

            //if(the rectangle object is empty, 
            //than throw an ArgumentException.
            if (rSize.IsEmpty)
            {
                throw new ArgumentException("Invalid argument. The rectangle cannot be empty.", "rSize");
            }

            //Return-rounded rectangle region.
            return RoundControlCLS.GetRegion(0.0F, 0.0F, (Single)rSize.Width, (Single)rSize.Height, (Single)radius, exclude);
        }

        ///<summary>
        ///Creats and returns a rounded corner region.
        ///</summary>
        ///<param name="rect">The rectangle of the region.</param>
        ///<param name="radius">The radius of the rounded corners. This value should be 
        ///bigger than or equal to 0 and less or equal to the half of the smallest value 
        ///of the region뭩 side.</param>
        ///<param name="exclude">A value that specifies the corners that should be excluded from rounding. This value 
        ///can be one or combination of the Shape.Corner enumeration value combined by "OR".</param>
        [DebuggerHidden()]
        public static Region RoundedRegion(RectangleF rect, Single radius, PropertyListBAS.ENUM_CORNER exclude)
        {
            //if(radius is less than zero, 
            //throw an ArgumentException.
            if (radius < 0.0F)
            {
                throw new ArgumentException("Invalid argument. The value cannot be negativ.", "radius");
            }

            //if(the rectangle object is empty, 
            //than throw an ArgumentException.
            if (rect.IsEmpty)
            {
                throw new ArgumentException("Invalid argument. The rectangle cannot be empty.", "rect");
            }

            //Return-rounded rectangle region.
            return RoundControlCLS.GetRegion(rect.X, rect.Y, rect.Width, rect.Height, radius, exclude);
        }


        /// <summary>
        /// Creats and returns a rounded corner region.
        /// </summary>
        /// <param name="rect">The rectangle of the region.</param>
        /// <param name="radius">The radius of the rounded corners. This value should be 
        /// bigger than or equal to 0 and less or equal to the half of the smallest value 
        /// of the region뭩 side.</param>
        /// <param name="exclude">A value that specifies the corners that should be excluded from rounding. This value 
        /// can be one or combination of the Shape.Corner enumeration value combined by "OR".</param>
        [DebuggerHidden()]
        public static Region RoundedRegion(Rectangle rect, int radius, PropertyListBAS.ENUM_CORNER exclude)
        {
            //if(radius is less than zero, 
            //throw an ArgumentException.
            if (radius < 0.0F)
            {
                throw new ArgumentException("Invalid argument. The value cannot be negativ.", "radius");
            }

            //if(the rectangle object is empty, 
            //than throw an ArgumentException.
            if (rect.IsEmpty)
            {
                throw new ArgumentException("Invalid argument. The rectangle cannot be empty.", "rect");
            }

            //Return-rounded rectangle region.
            return RoundControlCLS.GetRegion((Single)rect.X, (Single)rect.Y, (Single)rect.Width
                , (Single)rect.Height, (Single)radius, exclude);
        }



        [DebuggerHidden()]
        private static GraphicsPath GetPath(Single x,
            Single y,
            Single width,
            Single height,
            Single r,
            PropertyListBAS.ENUM_CORNER exclude)
        {
            var path = new GraphicsPath();
            //if(radius is equal to zero, 
            //than return a simple rectangle path.
            if (r == 0.0F)
            {
                path.AddRectangle(new RectangleF(x, y, width, height));
                return path;
            }

            //Small corner square-rectangles width
            Single w = r + r;
            //if('w' is bigger than the smallest value of the whidth/height, 
            //than assign the smallest value of the whidth/height to 'w'.
            if (height < width)
            {
                if (w > height)
                {
                    w = height;
                }
            }
            else
            {
                if (w > width)
                {
                    w = width;
                }
            }

            path.StartFigure();
            //Set top-left corner.
            if ((exclude & PropertyListBAS.ENUM_CORNER.TopLeft) != PropertyListBAS.ENUM_CORNER.TopLeft)
            {
                path.AddLine(x, y + r, x, y + r);
                path.AddArc(x, y, w, w, 180.0F, 90.0F);
                path.AddLine(x + r, y, x + r, y);
            }
            else
            {
                path.AddLine(x, y, x, y);
            }
            //Set top-right corner.
            if ((exclude & PropertyListBAS.ENUM_CORNER.TopRight) != PropertyListBAS.ENUM_CORNER.TopRight)
            {
                path.AddLine(x + width - r - 1, y, x + width - r, y);
                path.AddArc(x + width - w - 1, y, w, w, 270.0F, 90.0F);
                path.AddLine(x + width - 1, y + r, x + width, y + r);
            }
            else
            {
                path.AddLine(x + width, y, x + width, y);
            }
            //Set bottom-right corner.
            if ((exclude & PropertyListBAS.ENUM_CORNER.BottomRight) != PropertyListBAS.ENUM_CORNER.BottomRight)
            {
                path.AddLine(x + width - 1, y + height - r, x + width, y + height - r);
                path.AddArc(x + width - w - 1, y + height - w, w, w, 0.0F, 90.0F);
                path.AddLine(x + width - r - 1, y + height, x + width - r, y + height);
            }
            else
            {
                path.AddLine(x + width, y + height, x + width, y + height);
            }
            //Set bottom-left corner.
            if ((exclude & PropertyListBAS.ENUM_CORNER.BottomLeft) != PropertyListBAS.ENUM_CORNER.BottomLeft)
            {
                path.AddLine(x + r, y + height, x + r, y + height);
                path.AddArc(x, y + height - w, w, w, 90.0F, 90.0F);
                path.AddLine(x, y + height - r, x, y + height - r);
            }
            else
            {
                path.AddLine(x, y + height, x, y + height);
            }
            path.CloseAllFigures();
            return path;
        }



        [DebuggerHidden()]
        private static Region GetRegion(
        Single x,
        Single y,
        Single width,
        Single height,
        Single r,
        PropertyListBAS.ENUM_CORNER exclude)
        {
            var path = new GraphicsPath();
            //if(radius is equal to zero, 
            //than return a simple rectangle region.
            if (r == 0.0F)
            {
                path.AddRectangle(new RectangleF(x, y, width, height));
                return new Region(path);
            }

            //Small corner square-rectangles width
            Single w = r + r;
            //if('w' is bigger than the smallest value of the whidth/height, 
            //than assign the smallest value of the whidth/height to 'w'.
            if (height < width)
            {
                if (w > height)
                {
                    w = height;
                }
            }
            else
            {
                if (w > width)
                {
                    w = width;
                }
            }
            path.StartFigure();
            //Set top-left corner.
            if ((exclude & PropertyListBAS.ENUM_CORNER.TopLeft) != PropertyListBAS.ENUM_CORNER.TopLeft)
            {
                path.AddLine(x, y + r, x, y + r);
                path.AddArc(x, y, w - 1.0F, w - 1.0F, 180.0F, 90.0F);
                path.AddLine(x + r, y, x + r, y);
            }
            else
            {
                path.AddLine(x, y, x, y);
            }
            //Set top-right corner.
            if ((exclude & PropertyListBAS.ENUM_CORNER.TopRight) != PropertyListBAS.ENUM_CORNER.TopRight)
            {
                path.AddLine(x + width - r, y, x + width - r, y);
                path.AddArc(x + width - w, y, w - 1.0F, w - 1.0F, 270.0F, 90.0F);
                path.AddLine(x + width, y + r, x + width, y + r);
            }
            else
            {
                path.AddLine(x + width, y, x + width, y);
            }
            //Set bottom-right corner.
            if ((exclude & PropertyListBAS.ENUM_CORNER.BottomRight) != PropertyListBAS.ENUM_CORNER.BottomRight)
            {
                path.AddLine(x + width, y + height - r - 1.0F, x + width, y + height - r - 1.0F);
                path.AddArc(x + width - w, y + height - w, w - 1.0F, w - 1.0F, 0.0F, 90.0F);
                path.AddLine(x + width - r - 1.0F, y + height, x + width - r - 1.0F, y + height);
            }
            else
            {
                path.AddLine(x + width, y + height, x + width, y + height);
            }
            //Set bottom-left corner.
            if ((exclude & PropertyListBAS.ENUM_CORNER.BottomLeft) != PropertyListBAS.ENUM_CORNER.BottomLeft)
            {
                path.AddLine(x + r + 1.0F, y + height, x + r + 1.0F, y + height);
                path.AddArc(x, y + height - w, w - 1.0F, w - 1.0F, 90.0F, 90.0F);
                path.AddLine(x, y + height - r - 1.0F, x, y + height - r - 1.0F);
            }
            else
            {
                path.AddLine(x, y + height, x, y + height);
            }
            path.CloseAllFigures();
            return new Region(path);
        }


        #endregion
    }
}
