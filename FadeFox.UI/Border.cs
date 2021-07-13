
/*
 * ==============================================================================
 *   Program ID     :
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   :
 *   Developer      :
 *   Create Date    : 2009-08-24
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 *      1. Ascend.NET
 * ==============================================================================
 */

/* **********************************************************************************
 *
 * Copyright (c) Ascend.NET Project. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Shared Source License
 * for Ascend. A copy of the license can be found in the License.html file
 * at the root of this distribution. If you can not locate the Shared Source License
 * for Ascend, please send an email to ascendlic@<TBD>.
 * By using this source code in any fashion, you are agreeing to be bound by
 * the terms of the Shared Source License for Ascend.NET.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace FadeFox.UI
{
	/// <summary>
	/// Represents border information associated with a user interface (UI) element.
	/// </summary>
	public class Border
	{
		private int _bottom = 0;
		private int _left = 0;
		private int _right = 0;
		private int _top = 0;

		public Border(int pAll)
		{
			this._bottom = pAll;
			this._left = pAll;
			this._right = pAll;
			this._top = pAll;
		}

		public Border(int left, int top, int right, int bottom)
		{
			this._left = left;
			this._top = top;
			this._right = right;
			this._bottom = bottom;
		}

		/// <summary>
		/// Gets or sets the border value for the bottom edge.
		/// </summary>
		/// <value>
		/// <para>
		/// System.Int32 . The border, in pixels, for the bottom edge.
		/// </para>
		/// <para>
		/// This property is read/write.
		/// </para>
		/// </value>
		/// <remarks>
		/// Setting this value can also alter the All property.
		/// </remarks>
		public int Bottom
		{
			get
			{
				return this._bottom;
			}

			set
			{
				this._bottom = value;
			}
		}

		
		
		/// <summary>
		/// Gets or sets the border value for the left edge.
		/// </summary>
		/// <value>
		/// <para>
		/// System.Int32 . The border, in pixels, for the left edge.
		/// </para>
		/// <para>
		/// This property is read/write.
		/// </para>
		/// </value>
		/// <remarks>
		/// Setting this value can also alter the All property.
		/// </remarks>
		public int Left
		{
			get
			{
				return this._left;
			}

			set
			{
				this._left = value;
			}
		}

		/// <summary>
		/// Gets or sets the border value for the right edge.
		/// </summary>
		/// <value>
		/// <para>
		/// System.Int32 . The border, in pixels, for the right edge.
		/// </para>
		/// <para>
		/// This property is read/write.
		/// </para>
		/// </value>
		/// <remarks>
		/// Setting this value can also alter the All property.
		/// </remarks>
		public int Right
		{
			get
			{
				return this._right;
			}

			set
			{
				this._right = value;
			}
		}

		/// <summary>
		/// Gets or sets the border value for the top edge.
		/// </summary>
		/// <value>
		/// <para>
		/// System.Int32 . The border, in pixels, for the top edge.
		/// </para>
		/// <para>
		/// This property is read/write.
		/// </para>
		/// </value>
		/// <remarks>
		/// Setting this value can also alter the All property.
		/// </remarks>
		public int Top
		{
			get
			{
				return this._top;
			}

			set
			{
				this._top = value;
			}
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current Object.
		/// </summary>
		/// <param name="obj">The Object to compare with the current Object.</param>
		/// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Border)
			{
				Border border = (Border)obj;

				if ((border._top == this._top) && (border._left == this._left) && (border._bottom == this._bottom))
				{
					return (border._right == this._right);
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Serves as a hash function for the border type, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>
		/// <para>
		/// System.Int32
		/// </para>
		/// <para>
		/// A hash code for the Border object.
		/// </para>
		/// </returns>
		public override int GetHashCode()
		{
			return (((this.Left ^ RotateLeft(this.Top, 8)) ^ RotateLeft(this.Right, 0x10)) ^ RotateLeft(this.Bottom, 0x18));
		}

		/// <summary>
		/// Returns a string that contains the border values for an instance of the Border class.
		/// </summary>
		/// <returns>
		/// <para>
		/// System.String .
		/// </para>
		/// <para>
		/// A String that represents the current Border.
		/// </para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method returns a string containing the labeled values of the border for all four edges.
		/// </para>
		/// <para>
		/// This method overrides ToString.
		/// </para>
		/// </remarks>
		public override string ToString()
		{
			string[] textArray = new string[9] { "{Left=", this.Left.ToString(CultureInfo.CurrentCulture.NumberFormat), ",Top=", this.Top.ToString(CultureInfo.CurrentCulture.NumberFormat), ",Right=", this.Right.ToString(CultureInfo.CurrentCulture.NumberFormat), ",Bottom=", this.Bottom.ToString(CultureInfo.CurrentCulture.NumberFormat), "}" };
			return string.Concat(textArray);
		}

		/// <summary>
		/// Performs vector addition on the two specified Border objects, resulting in a new Border.
		/// </summary>
		/// <param name="border1">Border. The first Border to add.</param>
		/// <param name="border2">Border. The second Border to add.</param>
		/// <returns>Border. A new Border that results from adding border1 and border2.</returns>
		public static Border Add(Border border1, Border border2)
		{
			return (border1 + border2);
		}

		/// <summary>
		/// Performs vector subtraction on the two specified Border objects, resulting in a new Border.
		/// </summary>
		/// <param name="border1">Border. The first Border to subtract.</param>
		/// <param name="border2">Border. The second Border to subtract.</param>
		/// <returns>Border. A new Border that results from subtracting border2 from border1.</returns>
		public static Border Subtract(Border border1, Border border2)
		{
			return (border1 - border2);
		}

		private static int RotateLeft(int value, int bits)
		{
			bits = bits % 0x20;
			return ((value << (bits & 0x1f)) | (value >> ((0x20 - bits) & 0x1f)));
		}

		/// <summary>
		/// Performs vector addition on the two specified Border objects, resulting in a new Border.
		/// </summary>
		/// <param name="border1">Border. The first Border to add.</param>
		/// <param name="border2">Border. The second Border to add.</param>
		/// <returns>Border. A new Border that results from adding border1 and border2.</returns>
		public static Border operator +(Border border1, Border border2)
		{
			return new Border((border1.Left + border2.Left), (border1.Top + border2.Top), (border1.Right + border2.Right), (border1.Bottom + border2.Bottom));
		}

		/// <summary>
		/// Performs vector subtraction on the two specified Border objects, resulting in a new Border.
		/// </summary>
		/// <param name="border1">Border. The first Border to subtract.</param>
		/// <param name="border2">Border. The second Border to subtract.</param>
		/// <returns>Border. A new Border that results from subtracting border2 from border1.</returns>
		public static Border operator -(Border border1, Border border2)
		{
			return new Border(border1.Left - border2.Left, border1.Top - border2.Top, border1.Right - border2.Right, border1.Bottom - border2.Bottom);
		}

		/// <summary>
		/// Tests whether two specified Border objects are not equivalent.
		/// </summary>
		/// <param name="border1">Border. A Border to test.</param>
		/// <param name="border2">Border. A Border to test.</param>
		/// <returns>System.Boolean . true if the two Border objects are different; otherwise, false.</returns>
		public static bool operator !=(Border border1, Border border2)
		{
			return !(border1.Equals(border2));

		}

		/// <summary>
		/// Tests whether two specified Border objects are equivalent.
		/// </summary>
		/// <param name="border1">Border. A Border to test.</param>
		/// <param name="border2">Border. A Border to test.</param>
		/// <returns>System.Boolean . true if the two Border objects are equivalent; otherwise, false.</returns>
		public static bool operator ==(Border border1, Border border2)
		{
			return border1.Equals(border2);
		}
	}
}
