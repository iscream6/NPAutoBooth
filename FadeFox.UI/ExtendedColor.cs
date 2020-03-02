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
using System.Drawing;
using System.Text;

namespace FadeFox.UI
{
    /// <summary>
    /// ExtendedColor allows colors to be defined/manipulated using HSL
    /// color values, and provides an implicit conversion to <see cref="Color"/>
    /// to make its use mostly transparent
    /// </summary>
    /// <remarks>
    /// The .NET Framework exposes HSL components of <see cref="Color"/> values in
    /// a read-only manner. This class allows those values to be written.
    /// <para>This class is <see cref="SerializableAttribute"/> but does not implement
    /// <see cref="System.Runtime.Serialization.ISerializable"/></para>
    /// </remarks>
    [Serializable]
    public class ExtendedColor : ICloneable
    {
        /// <summary>
        /// Hues are generally described as a circle, ergo 360.0d
        /// </summary>
        public const double HueMaxValue = 360.0d;

        /// <summary>
        /// Saturation values are normalized to the range 0.0 -> 1.0
        /// </summary>
        public const double SaturationMaxValue = 1.0d;

        /// <summary>
        /// Brightness/Luminance values are normalized to the range 0.0 -> 1.0
        /// </summary>
        public const double BrightnessMaxValue = 1.0d;

        /// <summary>
        /// Maximum value for an RGB component (0 -> 255)
        /// </summary>
        public const int RgbMaxValue = Byte.MaxValue;

        /// <summary>
        /// The current <see cref="Color"/> value of the <c>ExtendedColor</c>
        /// </summary>
        private Color _color;

        /// <summary>
        /// Implicitly (without casting) converts an <c>ExtendedColor</c>
        /// to a <see cref="Color"/> value
        /// </summary>
        /// <param name="extendedColor">The <c>ExtendedColor</c> to convert</param>
        /// <returns>
        /// The <see cref="Color"/> value for the <c>ExtendedColor</c>
        /// </returns>
        public static implicit operator Color(ExtendedColor extendedColor)
        {
            return extendedColor._color;

        }

        /// <summary>
        /// Converts an intermediate Hue value to a normalized
        /// RGB value (0.0 -> 1.0)
        /// </summary>
        /// <param name="m1">Intermediate value 1</param>
        /// <param name="m2">Intermediate value 2</param>
        /// <param name="hue">The normalized hue value</param>
        /// <returns></returns>
        public static double HueToRgb(double m1, double m2, double hue)
        {
            if (hue < 0.0d) hue += 1.0d;
            if (hue > 1.0d) hue -= 1.0d;

            if ((6.0 * hue) < 1.0d)
                return (m1 + (m2 - m1) * hue * 6.0d);
            else if ((2.0 * hue) < 1.0d)
                return m2;
            if ((3.0d * hue) < 2.0d)
                return (m1 + (m2 - m1) * ((2.0d / 3.0d) - hue) * 6.0d);

            return m1;

        }

        /// <summary>
        /// Convert an HSL definition to its RGB equivalent
        /// </summary>
        /// <param name="hue">Normalized hue</param>
        /// <param name="saturation">Normalized saturation</param>
        /// <param name="luminance">Normalized luminance</param>
        /// <returns>
        /// The equivalent <see cref="Color"/> value for the specified HSL definition
        /// </returns>
        public static Color HslToRgb(double hue, double saturation, double luminance)
        {
            double red;
            double green;
            double blue;
            double m1, m2;

            if (saturation == 0)
            {
                red = green = blue = luminance;

            }
            else
            {
                if (luminance <= 0.5d)
                {
                    m2 = luminance * (1.0d + saturation);

                }
                else
                {
                    m2 = luminance + saturation - luminance * saturation;

                }

                m1 = (2.0d * luminance) - m2;

                red = HueToRgb(m1, m2, hue + (1.0d / 3.0d));
                green = HueToRgb(m1, m2, hue);
                blue = HueToRgb(m1, m2, hue - (1.0d / 3.0d));

            }

            // Create a Color value from the red, greren, and blue values by converting the normalized values
            // into the standard 'byte' value space (0-255)
            return Color.FromArgb(
                (int)Math.Round(red * (double)RgbMaxValue),
                (int)Math.Round(green * (double)RgbMaxValue),
                (int)Math.Round(blue * (double)RgbMaxValue)
                );

        }


        /// <summary>
        /// Create an <c>ExtendedColor</c> from the specified <see cref="Color"/> value
        /// </summary>
        /// <param name="color">The color</param>
        public ExtendedColor(Color color)
        {
            this._color = color;

        }

        /// <summary>
        /// Create an <c>ExtendedColor</c> from another <c>ExtendedColor</c>
        /// </summary>
        /// <param name="extendedColor">The other <c>ExtendedColor</c></param>
        public ExtendedColor(ExtendedColor extendedColor)
        {
            this._color = extendedColor._color;

        }

        /// <summary>
        /// Create an <c>ExtendedColor</c> from the specified <see cref="Color"/> value
        /// and specified saturation level
        /// </summary>
        /// <param name="color">The color</param>
        /// <param name="saturation">The new saturation value for the color</param>
        public ExtendedColor(Color color, double saturation)
        {
            this._color = color;
            Saturation = saturation;

        }

        /// <summary>
        /// Create an <c>ExtendedColor</c> from the specified RGB values
        /// </summary>
        /// <param name="red">The Red component</param>
        /// <param name="green">The Green component</param>
        /// <param name="blue">The Blue component</param>
        public ExtendedColor(byte red, byte green, byte blue)
        {
            this._color = Color.FromArgb(red, green, blue);

        }

        /// <summary>
        /// Create an <c>ExtendedColor</c> from the specified HSL values
        /// </summary>
        /// <param name="hue">The hue component</param>
        /// <param name="saturation">The saturation component</param>
        /// <param name="luminance">The luminance component</param>
        public ExtendedColor(double hue, double saturation, double luminance)
        {
            this._color = HslToRgb(hue, saturation, luminance);

        }

        #region IClonable Members
        /// <summary>
        /// Helper with correct return value
        /// </summary>
        /// <returns>
        /// A clone of this <c>ExtendedColor</c>
        /// </returns>
        public ExtendedColor CloneExtendedColor()
        {
            return new ExtendedColor(this);
        }

        /// <summary>
        /// Clone this <c>ExtendedColor</c>
        /// </summary>
        /// <returns>
        /// A clone of this <c>ExtendedColor</c>
        /// </returns>
        public Object Clone()
        {
            return CloneExtendedColor();

        }

        #endregion

        /// <summary>
        /// Get/Set the <see cref="Color"/> value of the <c>ExtendedColor</c>
        /// </summary>
        public Color Color
        {
            get
            {
                return _color;

            }

            set
            {
                _color = value;

            }
        }

        /// <summary>
        /// Get/Set the Red component of the <c>ExtendedColor</c>
        /// </summary>
        public byte Red
        {
            get
            {
                return _color.R;

            }

            set
            {
                _color = Color.FromArgb(value, _color.G, _color.B);

            }

        }

        /// <summary>
        /// Get/Set the Green component of the <c>ExtendedColor</c>
        /// </summary>
        public byte Green
        {
            get
            {
                return _color.G;

            }

            set
            {
                _color = Color.FromArgb(_color.R, value, _color.B);

            }

        }

        /// <summary>
        /// Get/Set the Blue component of the <c>ExtendedColor</c>
        /// </summary>
        public byte Blue
        {
            get
            {
                return _color.B;

            }

            set
            {
                _color = Color.FromArgb(_color.R, _color.G, value);

            }

        }

        /// <summary>
        /// Get/Set the Alpha component of the <c>ExtendedColor</c>
        /// </summary>
        public byte Alpha
        {
            get
            {
                return _color.A;

            }

            set
            {
                _color = Color.FromArgb(value, _color);

            }

        }

        /// <summary>
        /// Convert a normalized hue value to its common non-normalized form
        /// (0 -> 360)
        /// </summary>
        /// <param name="hue">The normalized hue value</param>
        /// <returns>
        /// The equivalent hue value in degrees
        /// </returns>
        public static int NonNormalizedHue(double hue)
        {
            return (int)(Math.Min(Math.Max(0.0d, hue), 1.0d) * HueMaxValue);

        }

        /// <summary>
        /// Get/Set the hue component of the <c>ExtendedColor</c>
        /// </summary>
        /// <remarks>
        /// This value is always specified using the normalized form (0.0 -> 1.0)
        /// </remarks>
        public double Hue
        {
            get
            {
                return _color.GetHue() / HueMaxValue;

            }

            set
            {
                _color = HslToRgb(value, Saturation, Luminance);

            }

        }

        /// <summary>
        /// Get/Set the hue component of the <c>ExtendedColor</c> in its
        /// non-normalized form
        /// </summary>
        public double HueNonNormalized
        {
            get
            {
                return Hue * HueMaxValue;

            }

            set
            {
                _color = HslToRgb(
                    value / HueMaxValue,
                    Saturation,
                    Luminance
                    );

            }

        }

        /// <summary>
        /// Get/Set the saturation component of the <c>ExtendedColor</c>
        /// </summary>
        public double Saturation
        {
            get
            {
                return (double)_color.GetSaturation();

            }

            set
            {
                _color = HslToRgb(
                    Hue,
                    value,
                    Luminance
                    );

            }

        }

        /// <summary>
        /// Get/Set the luminance component of the <c>ExtendedColor</c>
        /// </summary>
        /// <remarks>
        /// Internally the .NET Framework refers to this value as Brightness
        /// (<see cref="System.Drawing.Color.GetBrightness()"/>)
        /// </remarks>
        public double Luminance
        {
            get
            {
                return (double)_color.GetBrightness();

            }

            set
            {
                _color = HslToRgb(
                    Hue,
                    Saturation,
                    value
                    );

            }

        }

        /// <summary>
        /// <see langword="true"/> if the <c>ExtendedColor</c> is <see cref="System.Drawing.Color.Empty"/>
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _color.IsEmpty;

            }

        }

        /// <summary>
        /// <see langword="true"/> if the <c>ExtendedColor</c> is <see cref="System.Drawing.Color.Transparent"/>
        /// </summary>
        public bool IsTransparent
        {
            get
            {
                return _color == Color.Transparent;

            }

        }

        /// <summary>
        /// Modify the current hue component of the <c>ExtendedColor</c> by
        /// a percentage
        /// </summary>
        /// <param name="hueAdjustment">The percentage</param>
        public void AdjustHue(double hueAdjustment)
        {
            hueAdjustment = Math.Min(Math.Max(hueAdjustment, 0.0d), 1.0d);
            Hue = (Hue * hueAdjustment);

        }

        /// <summary>
        /// Modify the current saturation component of the <c>ExtendedColor</c> by
        /// a percentage
        /// </summary>
        /// <param name="saturationAdjustment">The percentage</param>
        public void AdjustSaturation(double saturationAdjustment)
        {
            saturationAdjustment = Math.Min(Math.Max(saturationAdjustment, 0.0d), 1.0d);
            Saturation = Saturation * saturationAdjustment;

        }

        /// <summary>
        /// Modify the current luminance component of the <c>ExtendedColor</c> by
        /// a percentage
        /// </summary>
        /// <param name="luminanceAdjustment">The percentage</param>
        public void AdjustLuminance(double luminanceAdjustment)
        {
            luminanceAdjustment = Math.Min(Math.Max(luminanceAdjustment, 0.0d), 1.0d);
            Luminance = Luminance * luminanceAdjustment;

        }

    }

}
