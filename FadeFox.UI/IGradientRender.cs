/*
 * ==============================================================================
 *   Program ID     :
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   :
 *   Developer      :
 *   Create Date    : 2008-08-24
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
   /// Specifies the painting style applied to the background in a control.
   /// </summary>
   public enum RenderMode
   {
      /// <summary>
      /// Indicates the use of a FlatBackgroundRender to paint.
      /// </summary>
      Flat = 0,
      /// <summary>
      /// Indicates the use of a GradientBackgroundRender to paint.
      /// </summary>
      Gradient = 1,
      /// <summary>
      /// Indicates the use of a GlassBackgroundRender to paint.
      /// </summary>
      Glass = 2

   }

   /// <summary>
   /// Provides an interface for gradient rendering.
   /// </summary>
   public interface IGradientRender
   {
      /// <summary>
      /// Gets or sets the alpha (transparancy) level (0 - 255) of the fill of the control.
      /// </summary>
      int Alpha
      {
         get;
         set;
      }

      /// <summary>
      /// Gets or sets the border for the control.
      /// </summary>
      /// <remarks>For containers such as GradientPanel and GradientCaption, the Border property gets or sets their respective border widths inside the DisplayRectangle.</remarks>
      /// <value>
      /// 	<para>
      /// Far.Border. An object of type Border representing the control's border width characteristics.
      /// </para>
      /// 	<para>
      /// This property is read/write.
      /// </para>
      /// </value>
      Border Border
      {
         get;
         set;
      }

      /// <summary>
      /// Gets or sets the border color(s) for the control.
      /// </summary>
      /// <remarks>For containers such as GradientPanel and GradientCaption, the BorderColor property gets or sets their respective border colors inside the DisplayRectangle.</remarks>
      /// <value>
      /// 	<para>
      /// Far.BorderColor. An object of type Border representing the control's border color characteristics.
      /// </para>
      /// 	<para>
      /// This property is read/write.
      /// </para>
      /// </value>
      BorderColor BorderColor
      {
         get;
         set;
      }

      /// <summary>
      /// The display rectangle to render to.
      /// </summary>
      Rectangle DisplayRectangle
      {
         get;
         set;
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
      bool Enabled
      {
         get;
         set;
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
      System.Drawing.Color GradientHighColor
      {
         get;
         set;
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
      System.Drawing.Color GradientLowColor
      {
         get;
         set;
      }

      /// <summary>
      /// Specifies the direction of a linear gradient.
      /// </summary>
      /// <value>
      /// <para>
      /// System.Drawing.Drawing2D.LinearGradientMode . Specifies the direction of a linear gradient.
      /// </para>
      /// <para>
      /// This property is read/write.
      /// </para>
      /// </value>
      System.Drawing.Drawing2D.LinearGradientMode GradientMode
      {
         get;
         set;
      }

      /// <summary>
      /// Specifies the painting style applied to the control.
      /// </summary>
      /// <value>
      /// <para>
      /// bool
      /// </para>
      /// <para>
      /// This property is read/write.
      /// </para>
      /// </value>
      RenderMode RenderMode
      {
         get;
         set;
      }

      /// <summary>
      /// Renders the gradient control.
      /// </summary>
      /// <param name="e">Provides data for the Paint event.</param>
      void Render(System.Windows.Forms.PaintEventArgs e);

      /// <summary>
      /// Resets the colors associated with the control.
      /// </summary>
      void ResetColors();

   }

}
