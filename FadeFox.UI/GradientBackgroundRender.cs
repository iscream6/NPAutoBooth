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
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace FadeFox.UI
{
   /// <summary>
   /// Renders a gradient background based on properties.
   /// </summary>
   public class GradientBackgroundRender : IDisposable, IGradientRender
   {
      private LinearGradientMode _gradientMode = LinearGradientMode.ForwardDiagonal;
      private Color _gradientHighColor = Color.Blue;
      private Color _gradientLowColor = Color.Navy;
      private BorderColor _borderColor = new BorderColor(Color.Black);
      private Border _border = new Border(0);
      private LinearGradientBrush _backBrush;
      private SolidBrush _backBrushFlat;
      private int _alpha = 255;
      private int _watermarkAlpha = 255;
      private Color _adjustedLowColor;
      private Color _adjustedHighColor;
      private Color _adjustedBorderColorTop;
      private Color _adjustedBorderColorRight;
      private Color _adjustedBorderColorLeft;
      private Color _adjustedBorderColorBottom;
      private Rectangle _displayRectangle;
      private bool _enabled = true;
      private bool _disposed;
      private Image _watermarkImage;
      private ContentAlignment _watermarkImageAlign;
      private WrapMode _watermarkDisplayStyle;
      private int _posOffset = 2;
      private Padding _padding;
      private RenderMode _renderMode;
      private float _highlightTransitionPercent;
      private bool _highlightBorder;
      private float _highColorLuminance;
      private float _lowColorLuminance;
      private bool _rightToLeft;

      /// <summary>
      /// Gets or sets the alpha (transparancy) level (0 - 255) of the fill of the control.
      /// </summary>
      public int Alpha
      {
         get
         {
            return this._alpha;
         }

         set
         {
            if (value == this._alpha)
            {
               return;
            }

            if (value < 0) value = 0;
            if (value > 255) value = 255;

            this._alpha = value;

            this.CreateColors();
            this.CreateBackBrush();
         }
      }

      /// <summary>
      /// Gets or sets the linear gradient background brush of the fill of the control.
      /// </summary>
      public LinearGradientBrush BackBrush
      {
         get
         {
            return this._backBrush;
         }

         set
         {
            if (value == this._backBrush)
            {
               return;
            }

            this._backBrush = value;
         }
      }

      /// <summary>
      /// Specifies the direction of a linear gradient.
      /// </summary>
      /// <value>
      /// 	<para>
      /// System.Drawing.Drawing2D.LinearGradientMode . Specifies the direction of a linear gradient.
      /// </para>
      /// 	<para>
      /// This property is read/write.
      /// </para>
      /// </value>
      public LinearGradientMode GradientMode
      {
         get
         {
            return this._gradientMode;
         }

         set
         {
            if (value == this._gradientMode)
            {
               return;
            }

            this._gradientMode = value;

            this.CreateBackBrush();
         }
      }

      /// <summary>
      /// Gets or sets the gradient low (darker) color for the control.
      /// </summary>
      /// <value>
      /// 	<para>
      /// System.Drawing.Color . A Color that represents the gradient low color of the control.
      /// </para>
      /// 	<para>
      /// This property is read/write.
      /// </para>
      /// </value>
      public Color GradientLowColor
      {
         get
         {
            return this._gradientLowColor;
         }

         set
         {
            if (value == this._gradientLowColor)
            {
               return;
            }

            this._gradientLowColor = value;

            this.CreateColors();
            this.CreateBackBrush();
         }
      }

      /// <summary>
      /// Gets or sets the gradient high (lighter) color for the control.
      /// </summary>
      /// <value>
      /// 	<para>
      /// System.Drawing.Color . A Color that represents the gradient high color of the control.
      /// </para>
      /// 	<para>
      /// This property is read/write.
      /// </para>
      /// </value>
      public Color GradientHighColor
      {
         get
         {
            return this._gradientHighColor;
         }

         set
         {
            if (value == this._gradientHighColor)
            {
               return;
            }

            this._gradientHighColor = value;

            this.CreateColors();
            this.CreateBackBrush();
         }
      }

      /// <summary>
      /// Gets or sets the border color(s) for the control.
      /// </summary>
      /// <value>
      /// 	<para>
      /// BorderColor. An object of type Border representing the control's border color characteristics.
      /// </para>
      /// 	<para>
      /// This property is read/write.
      /// </para>
      /// </value>
      /// <remarks>
      /// For containers such as GradientPanel and GradientCaption, the BorderColor property gets or sets their respective border colors inside the DisplayRectangle.
      /// </remarks>
      public BorderColor BorderColor
      {
         get
         {
            return this._borderColor;
         }

         set
         {
            if (value == this._borderColor)
            {
               return;

            }

            this._borderColor = value;

            this.CreateColors();
            this.CreateBackBrush();

         }

      }

      /// <summary>
      /// Gets or sets the border for the control.
      /// </summary>
      /// <value>
      /// 	<para>
      /// Border. An object of type Border representing the control's border width characteristics.
      /// </para>
      /// 	<para>
      /// This property is read/write.
      /// </para>
      /// </value>
      /// <remarks>
      /// For containers such as GradientPanel and GradientCaption, the Border property gets or sets their respective border widths inside the DisplayRectangle.
      /// </remarks>
      public Border Border
      {
         get
         {
            return this._border;

         }

         set
         {
            if (value == this.Border)
            {
               return;

            }

            this._border = value;

         }

      }

      /// <summary>
      /// Gets or sets a value indicating whether the control can respond to user interaction.
      /// </summary>
      /// <value>
      /// 	<para>
      /// System.Boolean . true if the control can respond to user interaction; otherwise, false. The default is true.
      /// </para>
      /// 	<para>
      /// This property is read/write.
      /// </para>
      /// </value>
      public bool Enabled
      {
         get
         {
            return this._enabled;

         }

         set
         {
            if (value == this._enabled)
            {
               return;

            }

            this._enabled = value;

            this.CreateColors();
            this.CreateBackBrush();

         }

      }

      /// <summary>
      /// The display rectangle to render to.
      /// </summary>
      public Rectangle DisplayRectangle
      {
         get
         {
            return this._displayRectangle;

         }

         set
         {
            this._displayRectangle = value;

         }

      }

      /// <summary>
      /// Gets or sets how bright the high color will be starting at the highlight transition point.
      /// </summary>
      /// <remarks>
      /// 1.15 by default.
      /// </remarks>
      public float HighColorLuminance
      {
         get
         {
            return this._highColorLuminance;

         }

         set
         {
            this._highColorLuminance = value;

         }

      }

      /// <summary>
      /// Gets or sets if the inside edge of the border should be highlighted.
      /// </summary>
      /// <remarks>
      /// False by default.
      /// </remarks>
      /// <value>
      /// true to highlight; otherwise false
      /// </value>
      public bool HighlightBorder
      {
         get
         {
            return this._highlightBorder;

         }

         set
         {
            this._highlightBorder = value;

         }

      }

      /// <summary>
      /// Gets or sets the highlight transition percent.
      /// </summary>
      /// <remarks>
      /// .45 by default.
      /// .01 to .99.
      /// The location by percent of the control where the color will transition from high to low color.
      /// This value is only used in glass render mode.
      /// </remarks>
      public float HighlightTransitionPercent
      {
         get
         {
            return this._highlightTransitionPercent;

         }

         set
         {
            if (value > .99f)
            {
               value = .99f;

            }
            else if (value < .01f)
            {
               value = .01f;

            }

            this._highlightTransitionPercent = value;

         }

      }

      /// <summary>
      /// Gets or sets how bright the low color will be furthest from the highlight transition point.
      /// </summary>
      /// <remarks>
      /// 1.2 by default.
      /// </remarks>
      public float LowColorLuminance
      {
         get
         {
            return this._lowColorLuminance;

         }

         set
         {
            this._lowColorLuminance = value;

         }

      }

      /// <summary>
      /// Gets or sets the padding associated with this control.
      /// </summary>
      /// <value>
      /// 	<para>
      /// System.Windows.Forms.Padding . The padding associated with this control.
      /// </para>
      /// 	<para>
      /// This property is read/write.
      /// </para>
      /// </value>
      public Padding Padding
      {
         get
         {
            return this._padding;

         }

         set
         {
            if (value == this._padding)
            {
               return;

            }

            this._padding = value;

         }

      }

      /// <summary>
      /// Specifies the painting style applied to the background in a control.
      /// </summary>
      public RenderMode RenderMode
      {
         get
         {
            return this._renderMode;

         }

         set
         {
            if (value == this._renderMode)
            {
               return;

            }

            this._renderMode = value;

         }

      }

      /// <summary>
      /// Gets or sets if the render is right to left.
      /// </summary>
      public bool RightToLeft
      {
         get
         {
            return this._rightToLeft;

         }

         set
         {
            this._rightToLeft = value;

         }

      }

      /// <summary>
      /// Gets or sets the alpha (transparancy) level (0 - 255) of the watermark in the control.
      /// </summary>
      public int WatermarkAlpha
      {
         get
         {
            return this._watermarkAlpha;

         }

         set
         {
            if (value == this._watermarkAlpha)
            {
               return;

            }

            if (value < 0) value = 0;
            if (value > 255) value = 255;

            this._watermarkAlpha = value;

         }

      }

      /// <summary>
      /// Gets or sets the image used as a watermark.
      /// </summary>
      public Image WatermarkImage
      {
         get
         {
            return this._watermarkImage;

         }

         set
         {
            this._watermarkImage = value;

         }

      }

      /// <summary>
      /// The ContentAlignment associated with this controls watermark image.
      /// </summary>
      /// <value>
      /// 	<para>
      /// System.Drawing.ContentAlignment . The ContentAlignment associated with this controls watermark image.
      /// </para>
      /// 	<para>
      /// This property is read/write.
      /// </para>
      /// </value>
      public ContentAlignment WatermarkImageAlign
      {
         get
         {
            return this._watermarkImageAlign;

         }

         set
         {
            if (value == this._watermarkImageAlign)
            {
               return;

            }

            this._watermarkImageAlign = value;

         }

      }

      /// <summary>
      /// Gets or sets the display style of watermark image.
      /// </summary>
      public WrapMode WatermarkDisplayStyle
      {
         get
         {
            return this._watermarkDisplayStyle;

         }

         set
         {
            if (value == this._watermarkDisplayStyle)
            {
               return;

            }

            this._watermarkDisplayStyle = value;

         }

      }

      /// <summary>
      /// Creates the gradient brush needed to draw the background.
      /// </summary>
      protected virtual void CreateBackBrush()
      {
         this.CreateBackBrush(this._adjustedLowColor, this._adjustedHighColor);

      }

      /// <summary>
      /// Creates the gradient brush needed to draw the background.
      /// </summary>
      /// <param name="lowColor">The gradient low (darker) color for the control.</param>
      /// <param name="highColor">The gradient high (lighter) color for the control.</param>
      protected virtual void CreateBackBrush(Color lowColor, Color highColor)
      {
         if (this.DisplayRectangle.Width > 0 && this.DisplayRectangle.Height > 0)
         {
            if (this._renderMode == RenderMode.Glass)
            {
               Color[] colorArray = new Color[] { highColor, GetColorLuminanceAdjusted(lowColor, this._highColorLuminance), lowColor, GetColorLuminanceAdjusted(lowColor, this._lowColorLuminance) };
               float[] positionArray = new float[] { 0.0f, this._highlightTransitionPercent, this._highlightTransitionPercent, 1.0f };

               ColorBlend blend = new ColorBlend();
               blend.Colors = colorArray;
               blend.Positions = positionArray;

               this._backBrush = new LinearGradientBrush(this.DisplayRectangle, Color.Transparent, Color.Transparent, this._gradientMode);
               this._backBrush.InterpolationColors = blend;

            }
            else if (this._renderMode == RenderMode.Gradient)
            {
               if (this._rightToLeft && (this._gradientMode == LinearGradientMode.Horizontal))
               {
                  this._backBrush = new LinearGradientBrush(this.DisplayRectangle, lowColor, highColor, this._gradientMode);

               }
               else
               {
                  this._backBrush = new LinearGradientBrush(this.DisplayRectangle, highColor, lowColor, this._gradientMode);

               }

            }
            else
            {
               this._backBrushFlat = new SolidBrush(lowColor);

            }

         }

      }

      /// <summary>
      /// Creates the colors used to draw the control.
      /// </summary>
      /// <remarks>
      /// Takes the alpha and enabled into account.
      /// </remarks>
      protected void CreateColors()
      {
         if (this.Enabled)
         {
            this._adjustedHighColor = Color.FromArgb(this._alpha, this._gradientHighColor);
            this._adjustedLowColor = Color.FromArgb(this._alpha, this._gradientLowColor);

            this._adjustedBorderColorLeft = Color.FromArgb(this._alpha, this._borderColor.Left);
            this._adjustedBorderColorTop = Color.FromArgb(this._alpha, this._borderColor.Top);
            this._adjustedBorderColorRight = Color.FromArgb(this._alpha, this._borderColor.Right);
            this._adjustedBorderColorBottom = Color.FromArgb(this._alpha, this._borderColor.Bottom);
         }
         else
         {
            ExtendedColor grayscaleHigh = new ExtendedColor(this._gradientHighColor, 0.0d);
            ExtendedColor grayscaleLow = new ExtendedColor(this._gradientLowColor, 0.0d);
            grayscaleHigh.Alpha = (byte)this._alpha;
            grayscaleLow.Alpha = (byte)this._alpha;

            this._adjustedHighColor = grayscaleHigh;
            this._adjustedLowColor = grayscaleLow;

            ExtendedColor grayscaleBorderLeft = new ExtendedColor(this._borderColor.Left, 0.0d);
            grayscaleBorderLeft.Alpha = (byte)this._alpha;
            this._adjustedBorderColorLeft = grayscaleBorderLeft;

            ExtendedColor grayscaleBorderTop = new ExtendedColor(this._borderColor.Top, 0.0d);
            grayscaleBorderTop.Alpha = (byte)this._alpha;
            this._adjustedBorderColorTop = grayscaleBorderTop;

            ExtendedColor grayscaleBorderRight = new ExtendedColor(this._borderColor.Right, 0.0d);
            grayscaleBorderRight.Alpha = (byte)this._alpha;
            this._adjustedBorderColorRight = grayscaleBorderRight;

            ExtendedColor grayscaleBorderBottom = new ExtendedColor(this._borderColor.Bottom, 0.0d);
            grayscaleBorderBottom.Alpha = (byte)this._alpha;
            this._adjustedBorderColorBottom = grayscaleBorderBottom;
         }
      }

      /// <summary>
      /// Initializes a new instance of the GradientBackground class.
      /// </summary>
      public GradientBackgroundRender()
         : base()
      {
         this._watermarkImageAlign = ContentAlignment.BottomRight;

         this._renderMode = RenderMode.Gradient;
         this._highlightTransitionPercent = .45f;
         this._highColorLuminance = 1.15f;
         this._lowColorLuminance = 1.2f;

         this.CreateColors();

      }

      #region IDisposable Members

      /// <summary>
      /// Releases all resources used.
      /// </summary>
      public void Dispose()
      {
         this.Dispose(true);

         GC.SuppressFinalize(this);

         return;

      }

      /// <summary>
      /// Releases all resources used.
      /// </summary>
      /// <param name="disposing">true or false</param>
      protected virtual void Dispose(bool disposing)
      {
         if (!this._disposed)
         {
            if (disposing)
            {
               if (this._backBrush != null) this._backBrush.Dispose();
               if (this._backBrushFlat != null) this._backBrushFlat.Dispose();
               if (this._watermarkImage != null) this._watermarkImage = null;

            }

         }

         this._disposed = true;

      }

      #endregion


      #region IRender Members

      /// <summary>
      /// Renders the gradient background.
      /// </summary>
      /// <param name="e">Provides data for the Paint event.</param>
      public virtual void Render(PaintEventArgs e)
      {
         if (this.DisplayRectangle != null)
         {
            if (this.DisplayRectangle.Width > 0 && this.DisplayRectangle.Height > 0)
            {
               this.SetRenderingHints(e);
               this.CreateBackBrush();
               this.DrawSquareBackgound(e);
            }
         }
      }

      #endregion

      /// <summary>
      /// Sets the rendering hints based on the AntiAlias property.
      /// </summary>
      /// <param name="e">A System.Windows.Forms.PaintEventArgs that contains the event data.</param>
      /// <remarks>
      /// This method must be called before any drawing methods are called.
      /// </remarks>
      protected virtual void SetRenderingHints(System.Windows.Forms.PaintEventArgs e)
      {

         e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
         e.Graphics.SmoothingMode = SmoothingMode.Default;

         if (this._renderMode == RenderMode.Glass)
         {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
         }
      }

      /// <summary>
      /// Fiils the passed graphics path with the appropriate background.
      /// </summary>
      /// <param name="e">Provides data for the Paint event.</param>
      /// <param name="graphicsPath">Represents a series of connected lines and curves that is the shape of the background.</param>
      protected virtual void FillBackgroundPath(PaintEventArgs e, GraphicsPath graphicsPath)
      {
         if (this._renderMode == RenderMode.Flat)
         {
            e.Graphics.FillPath(this._backBrushFlat, graphicsPath);

         }
         else
         {
            e.Graphics.FillPath(this._backBrush, graphicsPath);
         }

         //if (this._highlightBorder)
         //{
         //   if (this._renderMode == RenderMode.Glass)
         //   {
         //      int adjustedAlpha = (70 - (255 - this._alpha));
         //      if (adjustedAlpha < 0) adjustedAlpha = 0;

         //      if (adjustedAlpha > 0)
         //      {
         //         Color highColor = Color.FromArgb(adjustedAlpha, this._adjustedHighColor);
         //         Pen borderPen = new Pen(highColor, 2f);
         //         try
         //         {
         //            borderPen.Alignment = PenAlignment.Inset;

         //            e.Graphics.DrawPath(borderPen, graphicsPath);

         //         }
         //         catch
         //         {
         //            throw;

         //         }
         //         finally
         //         {
         //            borderPen.Dispose();

         //         }

         //      }

         //   }

         //}

      }

      private GraphicsPath CreateSquareBackgroundGraphicsPath()
      {
         GraphicsPath backgroundPath = new GraphicsPath();

         backgroundPath.StartFigure();

         backgroundPath.AddRectangle(this.DisplayRectangle);

         //int rightOffset = 0;
         //int bottomOffset = 0;
         //int topOffset = 0;
         //int leftOffset = 0;

         //PointF leftTopPoint = new PointF();
         //PointF rightTopPoint = new PointF();
         //PointF rightBottomPoint = new PointF();
         //PointF leftBottomPoint = new PointF();

         //leftTopPoint.X = this.DisplayRectangle.Left;
         //leftTopPoint.Y = this.DisplayRectangle.Top;

         //rightTopPoint.X = this.DisplayRectangle.Right;
         //rightTopPoint.Y = this.DisplayRectangle.Top;

         //backgroundPath.AddLine(leftTopPoint, rightTopPoint);

         //rightBottomPoint.X = this.DisplayRectangle.Right;
         //rightBottomPoint.Y = this.DisplayRectangle.Bottom;

         //backgroundPath.AddLine(rightTopPoint, rightBottomPoint);

         //leftBottomPoint.X = this.DisplayRectangle.Left;
         //leftBottomPoint.Y = this.DisplayRectangle.Bottom;

         //backgroundPath.AddLine(rightBottomPoint, leftBottomPoint);

         //backgroundPath.AddLine(leftBottomPoint, leftTopPoint);

         backgroundPath.CloseAllFigures();

         return backgroundPath;

      }

      /// <summary>
      /// Draws a square background.
      /// </summary>
      /// <param name="e">Provides data for the Paint event.</param>
      private void DrawSquareBackgound(PaintEventArgs e)
      {
         this.FillBackgroundPath(e, this.CreateSquareBackgroundGraphicsPath());

         if (this._border.Top > 0)
         {
            float width = (float)this._border.Top;

            Pen topBorderPen = new Pen(this._adjustedBorderColorTop, width);

            try
            {
               float offset = 0;

               if (this._border.Top > 1)
               {
                  if (this._border.Top % 2 == 1) //�汝鶺� 唳辦
                  {
                     offset = (float)((this._border.Top - 1) / 2.0);
                  }
                  else
                  {
                     offset = (float)(this._border.Top / 2.0);
                  }
               }

               e.Graphics.DrawLine(topBorderPen, new PointF(this.DisplayRectangle.Left, (this.DisplayRectangle.Top + offset)), new PointF((this.DisplayRectangle.Right - this._border.Right), (this.DisplayRectangle.Top + offset)));

            }
            catch
            {
               throw;

            }
            finally
            {
               topBorderPen.Dispose();
            }
         }

         if (this._border.Right > 0)
         {
            float width = (float)this._border.Right;

            Pen rightBorderPen = new Pen(this._adjustedBorderColorRight, width);

            try
            {
               float offset = 0;

               if (this._border.Right % 2 == 1) //�汝鶺� 唳辦
               {
                  offset = (float)((this._border.Right + 1) / 2.0);
               }
               else
               {
                  offset = (float)(this._border.Right / 2.0);
               }

               e.Graphics.DrawLine(rightBorderPen, new PointF((this.DisplayRectangle.Right - offset), this.DisplayRectangle.Top), new PointF((this.DisplayRectangle.Right - offset), (this.DisplayRectangle.Bottom - this._border.Bottom)));
            }
            catch
            {
               throw;
            }
            finally
            {
               rightBorderPen.Dispose();
            }
         }

         if (this._border.Bottom > 0)
         {
            float width = (float)this._border.Bottom;

            Pen botttomBorderPen = new Pen(this._adjustedBorderColorBottom, width);

            try
            {
               float offset = 0;

               if (this._border.Bottom % 2 == 1) //�汝鶺� 唳辦
               {
                  offset = (float)((this._border.Bottom + 1) / 2.0);
               }
               else
               {
                  offset = (float)(this._border.Bottom / 2.0);
               }

               e.Graphics.DrawLine(botttomBorderPen, new PointF((this.DisplayRectangle.Left + this._border.Left), (this.DisplayRectangle.Bottom - offset)), new PointF((this.DisplayRectangle.Right), (this.DisplayRectangle.Bottom - offset)));
            }
            catch
            {
               throw;

            }
            finally
            {
               botttomBorderPen.Dispose();

            }

         }

         if (this._border.Left > 0)
         {
            float width = (float)this._border.Left;

            Pen leftBorderPen = new Pen(this._adjustedBorderColorLeft, width);

            try
            {
               float offset = 0;

               if (this._border.Left > 1)
               {
                  if (this._border.Left % 2 == 1) //�汝鶺� 唳辦
                  {
                     offset = (float)((this._border.Left - 1) / 2.0);
                  }
                  else
                  {
                     offset = (float)(this._border.Left / 2.0);
                  }
               }

               e.Graphics.DrawLine(leftBorderPen, new PointF((this.DisplayRectangle.Left + offset), (this.DisplayRectangle.Top + this._border.Top)), new PointF((this.DisplayRectangle.Left + offset), this.DisplayRectangle.Bottom));
            }
            catch
            {
               throw;
            }
            finally
            {
               leftBorderPen.Dispose();
            }
         }

         this.DrawWatermark(e);
      }

      private void DrawWatermark(PaintEventArgs e)
      {
         float topOffset = (this._posOffset + this._padding.Top + this.Border.Top);
         float leftPosition = (this.DisplayRectangle.Left + this._padding.Left + this._posOffset);
         float topPosition = (this.DisplayRectangle.Top + this._posOffset + this._padding.Top + this.Border.Top);
         float bottomOffset = (this._posOffset + this._padding.Bottom + this.Border.Bottom);

         if ((this._watermarkImage != null) && (this._watermarkImage.PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined) && (this._watermarkImage.Height > 0))
         {
            ColorMatrix colorMatrix = new ColorMatrix();

            float adjustedAlpha = ((this._watermarkAlpha - (255 - this._alpha)) / 255.0f);
            if (adjustedAlpha < 0) adjustedAlpha = 0;

            colorMatrix.Matrix33 = adjustedAlpha; //33 is alpha

            ImageAttributes imageAttributes = new ImageAttributes();
            try
            {
               imageAttributes.SetColorMatrix(colorMatrix);

               if (this._watermarkDisplayStyle == WrapMode.Clamp)
               {
                  if (this._watermarkImageAlign == ContentAlignment.MiddleLeft)
                  {
                     Rectangle imageRectangle = new Rectangle((int)leftPosition, (this.DisplayRectangle.Top + ((this.DisplayRectangle.Height / 2) - (this._watermarkImage.Height / 2))), this._watermarkImage.Width, this._watermarkImage.Height);

                     if (this.Enabled)
                     {
                        e.Graphics.DrawImage(this._watermarkImage, imageRectangle, 0, 0, this._watermarkImage.Width, this._watermarkImage.Height, GraphicsUnit.Pixel, imageAttributes);

                     }
                     else
                     {
                        ControlPaint.DrawImageDisabled(e.Graphics, this._watermarkImage, (int)leftPosition, (int)(this.DisplayRectangle.Top + ((this.DisplayRectangle.Height / 2.0f) - (this._watermarkImage.Height / 2.0f))), Color.Transparent);

                     }

                  }
                  else if (this._watermarkImageAlign == ContentAlignment.TopLeft)
                  {
                     Rectangle imageRectangle = new Rectangle((int)leftPosition, (int)topPosition, this._watermarkImage.Width, this._watermarkImage.Height);

                     if (this.Enabled)
                     {
                        e.Graphics.DrawImage(this._watermarkImage, imageRectangle, 0, 0, this._watermarkImage.Width, this._watermarkImage.Height, GraphicsUnit.Pixel, imageAttributes);

                     }
                     else
                     {
                        ControlPaint.DrawImageDisabled(e.Graphics, this._watermarkImage, (int)leftPosition, (int)topPosition, Color.Transparent);

                     }

                  }
                  else if (this._watermarkImageAlign == ContentAlignment.BottomLeft)
                  {
                     Rectangle imageRectangle = new Rectangle((int)leftPosition, (int)(this.DisplayRectangle.Height - (bottomOffset + this._watermarkImage.Height)), this._watermarkImage.Width, this._watermarkImage.Height);

                     if (this.Enabled)
                     {
                        e.Graphics.DrawImage(this._watermarkImage, imageRectangle, 0, 0, this._watermarkImage.Width, this._watermarkImage.Height, GraphicsUnit.Pixel, imageAttributes);

                     }
                     else
                     {
                        ControlPaint.DrawImageDisabled(e.Graphics, this._watermarkImage, (int)leftPosition, (int)(this.DisplayRectangle.Height - (bottomOffset + this._watermarkImage.Height)), Color.Transparent);

                     }

                  }
                  else if (this._watermarkImageAlign == ContentAlignment.MiddleRight)
                  {
                     Rectangle imageRectangle = new Rectangle((int)(this.DisplayRectangle.Right - this._posOffset - this._padding.Right - this._watermarkImage.Width), (int)(this.DisplayRectangle.Top + (this.DisplayRectangle.Height / 2.0f) - (this._watermarkImage.Height / 2.0f)), this._watermarkImage.Width, this._watermarkImage.Height);

                     if (this.Enabled)
                     {
                        e.Graphics.DrawImage(this._watermarkImage, imageRectangle, 0, 0, this._watermarkImage.Width, this._watermarkImage.Height, GraphicsUnit.Pixel, imageAttributes);

                     }
                     else
                     {
                        ControlPaint.DrawImageDisabled(e.Graphics, this._watermarkImage, (int)(this.DisplayRectangle.Right - this._posOffset - this._padding.Right - this._watermarkImage.Width), (int)(this.DisplayRectangle.Top + (this.DisplayRectangle.Height / 2.0f) - (this._watermarkImage.Height / 2.0f)), Color.Transparent);

                     }

                  }
                  else if (this._watermarkImageAlign == ContentAlignment.TopRight)
                  {
                     Rectangle imageRectangle = new Rectangle((int)(this.DisplayRectangle.Right - this._posOffset - this._watermarkImage.Width - this._padding.Right), (int)(this.DisplayRectangle.Top + this._posOffset + this._padding.Top), this._watermarkImage.Width, this._watermarkImage.Height);

                     if (this.Enabled)
                     {
                        e.Graphics.DrawImage(this._watermarkImage, imageRectangle, 0, 0, this._watermarkImage.Width, this._watermarkImage.Height, GraphicsUnit.Pixel, imageAttributes);

                     }
                     else
                     {
                        ControlPaint.DrawImageDisabled(e.Graphics, this._watermarkImage, (int)(this.DisplayRectangle.Right - this._posOffset - this._watermarkImage.Width - this._padding.Right), (int)(this.DisplayRectangle.Top + this._posOffset + this._padding.Top), Color.Transparent);

                     }

                  }
                  else if (this._watermarkImageAlign == ContentAlignment.BottomRight)
                  {
                     Rectangle imageRectangle = new Rectangle((int)(this.DisplayRectangle.Right - this._posOffset - this._watermarkImage.Width - this._padding.Right), (int)((this.DisplayRectangle.Top + this.DisplayRectangle.Height) - this._posOffset - this._watermarkImage.Height - this._padding.Bottom), this._watermarkImage.Width, this._watermarkImage.Height);

                     if (this.Enabled)
                     {
                        e.Graphics.DrawImage(this._watermarkImage, imageRectangle, 0, 0, this._watermarkImage.Width, this._watermarkImage.Height, GraphicsUnit.Pixel, imageAttributes);

                     }
                     else
                     {
                        ControlPaint.DrawImageDisabled(e.Graphics, this._watermarkImage, (int)(this.DisplayRectangle.Right - this._posOffset - this._watermarkImage.Width - this._padding.Right), (int)((this.DisplayRectangle.Top + this.DisplayRectangle.Height) - this._posOffset - this._watermarkImage.Height - this._padding.Bottom), Color.Transparent);

                     }

                  }
                  else if (this._watermarkImageAlign == ContentAlignment.TopCenter)
                  {
                     Rectangle imageRectangle = new Rectangle((int)(((this.DisplayRectangle.Width / 2.0f) + this.DisplayRectangle.Left) - (this._watermarkImage.Width / 2.0f)), (int)(this.DisplayRectangle.Top + this._posOffset + this._padding.Top), this._watermarkImage.Width, this._watermarkImage.Height);

                     if (this.Enabled)
                     {
                        e.Graphics.DrawImage(this._watermarkImage, imageRectangle, 0, 0, this._watermarkImage.Width, this._watermarkImage.Height, GraphicsUnit.Pixel, imageAttributes);

                     }
                     else
                     {
                        ControlPaint.DrawImageDisabled(e.Graphics, this._watermarkImage, (int)(((this.DisplayRectangle.Width / 2.0f) + this.DisplayRectangle.Left) - (this._watermarkImage.Width / 2.0f)), (int)(this.DisplayRectangle.Top + this._posOffset + this._padding.Top), Color.Transparent);

                     }

                  }
                  else if (this._watermarkImageAlign == ContentAlignment.BottomCenter)
                  {
                     Rectangle imageRectangle = new Rectangle((int)(((this.DisplayRectangle.Width / 2.0f) + this.DisplayRectangle.Left) - (this._watermarkImage.Width / 2.0f)), (int)((this.DisplayRectangle.Top + this.DisplayRectangle.Height) - this._posOffset - this._watermarkImage.Height - this._padding.Bottom), this._watermarkImage.Width, this._watermarkImage.Height);

                     if (this.Enabled)
                     {
                        e.Graphics.DrawImage(this._watermarkImage, imageRectangle, 0, 0, this._watermarkImage.Width, this._watermarkImage.Height, GraphicsUnit.Pixel, imageAttributes);

                     }
                     else
                     {
                        ControlPaint.DrawImageDisabled(e.Graphics, this._watermarkImage, (int)(((this.DisplayRectangle.Width / 2.0f) + this.DisplayRectangle.Left) - (this._watermarkImage.Width / 2.0f)), (int)((this.DisplayRectangle.Top + this.DisplayRectangle.Height) - this._posOffset - this._watermarkImage.Height - this._padding.Bottom), Color.Transparent);

                     }

                  }
                  else //centered
                  {
                     Rectangle imageRectangle = new Rectangle((int)(((this.DisplayRectangle.Width / 2.0f) + this.DisplayRectangle.Left) - (this._watermarkImage.Width / 2.0f)), (int)(((this.DisplayRectangle.Height / 2.0f) + this.DisplayRectangle.Top) - (this._watermarkImage.Height / 2.0f)), this._watermarkImage.Width, this._watermarkImage.Height);

                     if (this.Enabled)
                     {
                        e.Graphics.DrawImage(this._watermarkImage, imageRectangle, 0, 0, this._watermarkImage.Width, this._watermarkImage.Height, GraphicsUnit.Pixel, imageAttributes);

                     }
                     else
                     {
                        ControlPaint.DrawImageDisabled(e.Graphics, this._watermarkImage, (int)(((this.DisplayRectangle.Width / 2.0f) + this.DisplayRectangle.Left) - (this._watermarkImage.Width / 2.0f)), (int)(((this.DisplayRectangle.Height / 2.0f) + this.DisplayRectangle.Top) - (this._watermarkImage.Height / 2.0f)), Color.Transparent);

                     }

                  }
               }
               else
               {
                  imageAttributes.SetWrapMode(_watermarkDisplayStyle);

                  if (this.Enabled)
                  {
                     e.Graphics.DrawImage(this._watermarkImage, DisplayRectangle, 0, 0, DisplayRectangle.Width, DisplayRectangle.Height, GraphicsUnit.Pixel, imageAttributes);
                  }
                  else
                  {
                     ControlPaint.DrawImageDisabled(e.Graphics, this._watermarkImage, (int)(((this.DisplayRectangle.Width / 2.0f) + this.DisplayRectangle.Left) - (this._watermarkImage.Width / 2.0f)), (int)(((this.DisplayRectangle.Height / 2.0f) + this.DisplayRectangle.Top) - (this._watermarkImage.Height / 2.0f)), Color.Transparent);

                  }
               }

            }
            catch
            {
               throw;

            }
            finally
            {
               imageAttributes.Dispose();

            }

         }

      }

      /// <summary>
      /// Gets or sets the adjusted high color for the control.
      /// </summary>
      /// <value>
      /// <para>
      /// System.Drawing.Color.
      /// </para>
      /// 	<para>
      /// This property is read/write.
      /// </para>
      /// </value>
      public Color AdjustedHighColor
      {
         get
         {
            return this._adjustedHighColor;

         }

         set
         {
            if (value == this._adjustedHighColor)
            {
               return;

            }

            this._adjustedHighColor = value;

         }

      }

      /// <summary>
      /// Gets or sets the adjusted low color for the control.
      /// </summary>
      /// <value>
      /// <para>
      /// System.Drawing.Color.
      /// </para>
      /// 	<para>
      /// This property is read/write.
      /// </para>
      /// </value>
      public Color AdjustedLowColor
      {
         get
         {
            return this._adjustedLowColor;

         }

         set
         {
            if (value == this._adjustedLowColor)
            {
               return;

            }

            this._adjustedLowColor = value;

         }

      }

      /// <summary>
      /// Resets the colors associated with the control.
      /// </summary>
      public virtual void ResetColors()
      {
         this.CreateColors();
         this.CreateBackBrush();

      }

      /// <summary>
      /// Adjusts the passed color's luminance to the passed luminance level.
      /// </summary>
      /// <param name="baseColor">The starting color.</param>
      /// <param name="luminance">The luminace level to apply to the starting color.</param>
      /// <returns>The new color created.</returns>
      protected static Color GetColorLuminanceAdjusted(Color baseColor, double luminance)
      {
         Color color = ExtendedColor.HslToRgb((baseColor.GetHue() / 360), baseColor.GetSaturation(), baseColor.GetBrightness());

         double red = (double)color.R * luminance;
         double green = (double)color.G * luminance;
         double blue = (double)color.B * luminance;

         if (red > 255.0) red = 255.0;
         if (green > 255.0) green = 255.0;
         if (blue > 255.0) blue = 255.0;

         return Color.FromArgb((int)red, (int)green, (int)blue);

      }


   }

}
