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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace FadeFox.UI
{
	/// <summary>
	/// Represents border color information associated with a user interface (UI) element.
	/// </summary>
	public class BorderColor
	{
		private Color _bottom = Color.Black;
		private Color _left = Color.Black;
		private Color _right = Color.Black;
		private Color _top = Color.Black;

		public BorderColor(Color all)
		{
			this._left = all;
			this._top = all;
			this._right = all;
			this._bottom = all;
		}

		public BorderColor(Color left, Color top, Color right, Color bottom)
		{
			this._left = left;
			this._top = top;
			this._right = right;
			this._bottom = bottom;
		}

		public Color Bottom
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

		public Color Left
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

		public Color Right
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

		public Color Top
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

		public override string ToString()
		{
			string[] textArray = new string[9] { "{Left=", this.Left.ToString(), ",Top=", this.Top.ToString(), ",Right=", this.Right.ToString(), ",Bottom=", this.Bottom.ToString(), "}" };
			return string.Concat(textArray);

		}

		public override int GetHashCode()
		{
			return (this.Left.GetHashCode() ^ this.Top.GetHashCode() ^ this.Right.GetHashCode() ^ this.Bottom.GetHashCode());
		}

		public override bool Equals(object obj)
		{
			if (obj is BorderColor)
			{
				BorderColor borderColor = (BorderColor)obj;

				if ((borderColor._top == this._top) && (borderColor._left == this._left) && (borderColor._bottom == this._bottom))
				{
					return (borderColor.Right == this._right);
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

		public static bool operator ==(BorderColor borderColor1, BorderColor borderColor2)
		{
			return borderColor1.Equals(borderColor2);
		}

		public static bool operator !=(BorderColor borderColor1, BorderColor borderColor2)
		{
			return !(borderColor1.Equals(borderColor2));
		}

		public BorderColor CreateInstance()
		{
			return new BorderColor(this._left, this._top, this._right, this._bottom);
		}
	}
}
