using System;
using System.Collections.Generic;
using System.Text;
using FadeFox.Document.MyXls.ByteUtil;

namespace FadeFox.Document.MyXls
{
    /// <summary>
    /// Represents an RGB color value.  Use the values in Colors.  Custom colors are not yet supported.
    /// </summary>
    public class ColorEx : ICloneable
    {
        internal byte Red;
        internal byte Green;
        internal byte Blue;
        internal ushort? Id;

        internal ColorEx(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Id = null;
        }

        internal ColorEx(ushort id)
        {
            Red = 0x00;
            Green = 0x00;
            Blue = 0x00;
            Id = id;
        }

        /// <summary>
        /// Determines whether this Color is equal to the specifed Color.
        /// </summary>
        /// <param name="obj">The Color to compare to this Color.</param>
        /// <returns>true if this Color is Equal to that Color, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            ColorEx that = (ColorEx) obj;
            if (this.Id != null || that.Id != null)
                return this.Id == that.Id;
            else
                return (this.Red == that.Red &&
                        this.Green == that.Green &&
                        this.Blue == that.Blue);
        }

        /// <summary>
        /// Returns a new Color instance Equal to this Color.
        /// </summary>
        /// <returns>A new Color instance Equal to this Color.</returns>
        public object Clone()
        {
            ColorEx clone = new ColorEx(Red, Green, Blue);
            clone.Id = Id;
            return clone;
        }

		/// <summary>
		/// 단순 재정의
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
    }
}
