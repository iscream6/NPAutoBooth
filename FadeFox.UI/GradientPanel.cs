using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel.Design;

namespace FadeFox.UI
{
	[ToolboxBitmap(typeof(System.Windows.Forms.Panel))]
	public partial class GradientPanel : System.Windows.Forms.ScrollableControl
	{
		private IGradientRender _gradientRender;
		private bool _disposed;

		/// <summary>
		/// Gets or sets the alpha (transparancy) level (0 - 255) of the fill of the control.
		/// </summary>
		[DefaultValue(255)]
		public int Alpha
		{
			get
			{
				return this._gradientRender.Alpha;

			}

			set
			{
				if (value == this._gradientRender.Alpha)
				{
					return;

				}

				this._gradientRender.Alpha = value;

				base.Invalidate();
			}
		}

		[DefaultValue(0)]
		public int BorderThickLeft
		{
			get
			{
				return this._gradientRender.Border.Left;
			}

			set
			{
				if (value == this._gradientRender.Border.Left)
				{
					return;
				}

				this._gradientRender.Border.Left = value;

				base.Invalidate();
			}
		}

		[DefaultValue(0)]
		public int BorderThickRight
		{
			get
			{
				return this._gradientRender.Border.Right;
			}

			set
			{
				if (value == this._gradientRender.Border.Right)
				{
					return;
				}

				this._gradientRender.Border.Right = value;

				base.Invalidate();
			}
		}

		[DefaultValue(0)]
		public int BorderThickTop
		{
			get
			{
				return this._gradientRender.Border.Top;
			}

			set
			{
				if (value == this._gradientRender.Border.Top)
				{
					return;
				}

				this._gradientRender.Border.Top = value;

				base.Invalidate();
			}
		}

		[DefaultValue(0)]
		public int BorderThickBottom
		{
			get
			{
				return this._gradientRender.Border.Bottom;
			}

			set
			{
				if (value == this._gradientRender.Border.Bottom)
				{
					return;
				}

				this._gradientRender.Border.Bottom = value;

				base.Invalidate();
			}
		}

		[DefaultValue(typeof(Color), "Color.Black")]
		public virtual Color BorderColorLeft
		{
			get
			{
				return this._gradientRender.BorderColor.Left;
			}

			set
			{
				if (value == this._gradientRender.BorderColor.Left)
				{
					return;
				}
				else
				{
					this._gradientRender.BorderColor.Left = value;
					this._gradientRender.ResetColors();

					base.Invalidate();
				}
			}
		}

		[DefaultValue(typeof(Color), "Color.Black")]
		public virtual Color BorderColorRight
		{
			get
			{
				return this._gradientRender.BorderColor.Right;
			}

			set
			{
				if (value == this._gradientRender.BorderColor.Right)
				{
					return;
				}
				else
				{
					this._gradientRender.BorderColor.Right = value;
					this._gradientRender.ResetColors();

					base.Invalidate();
				}
			}
		}

		[DefaultValue(typeof(Color), "Color.Black")]
		public virtual Color BorderColorTop
		{
			get
			{
				return this._gradientRender.BorderColor.Top;
			}

			set
			{
				if (value == this._gradientRender.BorderColor.Top)
				{
					return;
				}
				else
				{
					this._gradientRender.BorderColor.Top = value;
					this._gradientRender.ResetColors();

					base.Invalidate();
				}
			}
		}

		[DefaultValue(typeof(Color), "Color.Black")]
		public virtual Color BorderColorBottom
		{
			get
			{
				return this._gradientRender.BorderColor.Bottom;
			}

			set
			{
				if (value == this._gradientRender.BorderColor.Bottom)
				{
					return;
				}
				else
				{
					this._gradientRender.BorderColor.Bottom = value;
					this._gradientRender.ResetColors();

					base.Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets or sets the gradient high (lighter) color for the control.
		/// </summary>
		/// <value>
		/// <para>
		/// System.Drawing.Color . A Color that represents the gradient high color of the control.
		/// </para>
		/// <para>
		/// This property is read/write.
		/// </para>
		/// </value>
		[DefaultValue(typeof(Color), "Color.Blue")]
		public Color GradientHighColor
		{
			get
			{
				return this._gradientRender.GradientHighColor;

			}

			set
			{
				if (value == this._gradientRender.GradientHighColor)
				{
					return;
				}

				this._gradientRender.GradientHighColor = value;

				base.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the gradient low (darker) color for the control.
		/// </summary>
		/// <value>
		/// <para>
		/// System.Drawing.Color . A Color that represents the gradient low color of the control.
		/// </para>
		/// <para>
		/// This property is read/write.
		/// </para>
		/// </value>
		[DefaultValue(typeof(Color), "Color.Navy")]
		public Color GradientLowColor
		{
			get
			{
				return this._gradientRender.GradientLowColor;
			}

			set
			{
				if (value == this._gradientRender.GradientLowColor)
				{
					return;
				}

				this._gradientRender.GradientLowColor = value;

				base.Invalidate();
			}
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
		public LinearGradientMode GradientMode
		{
			get
			{
				return this._gradientRender.GradientMode;
			}

			set
			{
				if (value == this._gradientRender.GradientMode)
				{
					return;
				}

				this._gradientRender.GradientMode = value;

				base.Invalidate();
			}
		}

		/// <summary>
		/// The render class.
		/// </summary>
		protected IGradientRender GradientRender
		{
			get
			{
				return this._gradientRender;
			}

			set
			{
				this._gradientRender = value;
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
				return ((GradientBackgroundRender)this._gradientRender).HighColorLuminance;
			}

			set
			{
				if (value == ((GradientBackgroundRender)this._gradientRender).HighColorLuminance)
				{
					return;
				}

				((GradientBackgroundRender)this._gradientRender).HighColorLuminance = value;

				base.Invalidate();
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
				return ((GradientBackgroundRender)this._gradientRender).HighlightBorder;

			}

			set
			{
				if (value == ((GradientBackgroundRender)this._gradientRender).HighlightBorder)
				{
					return;

				}

				((GradientBackgroundRender)this._gradientRender).HighlightBorder = value;

				base.Invalidate();

			}

		}

		/// <summary>
		/// Gets or sets the highlight transition percent.
		/// </summary>
		/// <remarks>
		/// .45 by default.
		/// The location by percent of the control where the color will transition from high to low color.
		/// This value is only used in glass render mode.
		/// </remarks>
		public float HighlightTransitionPercent
		{
			get
			{
				return ((GradientBackgroundRender)this._gradientRender).HighlightTransitionPercent;

			}

			set
			{
				if (value == ((GradientBackgroundRender)this._gradientRender).HighlightTransitionPercent)
				{
					return;

				}

				((GradientBackgroundRender)this._gradientRender).HighlightTransitionPercent = value;

				base.Invalidate();

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
				return ((GradientBackgroundRender)this._gradientRender).LowColorLuminance;

			}

			set
			{
				if (value == ((GradientBackgroundRender)this._gradientRender).LowColorLuminance)
				{
					return;
				}

				((GradientBackgroundRender)this._gradientRender).LowColorLuminance = value;

				base.Invalidate();
			}

		}

		/// <summary>
		/// Specifies the painting style applied to the background in a control.
		/// </summary>
		public RenderMode RenderMode
		{
			get
			{
				return this._gradientRender.RenderMode;
			}

			set
			{
				if (value == this._gradientRender.RenderMode)
				{
					return;
				}

				this._gradientRender.RenderMode = value;

				base.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user can give the focus to this control using the TAB key.
		/// </summary>
		/// <value>
		/// true if the user can give the focus to the control using the TAB key; otherwise, false. The default is true.
		/// </value>
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}

			set
			{
				base.TabStop = value;
			}
		}

		/// <summary>
		/// Gets or sets the image used as a watermark.
		/// </summary>
		public Image WatermarkImage
		{
			get
			{
				return ((GradientBackgroundRender)this._gradientRender).WatermarkImage;
			}

			set
			{
				if (value == ((GradientBackgroundRender)_gradientRender).WatermarkImage)
				{
					return;
				}

				((GradientBackgroundRender)this._gradientRender).WatermarkImage = value;

				base.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the alpha (transparancy) level (0 - 255) of the watermark in the control.
		/// </summary>
		public int WatermarkAlpha
		{
			get
			{
				return ((GradientBackgroundRender)this._gradientRender).WatermarkAlpha;
			}

			set
			{
				if (value == ((GradientBackgroundRender)this._gradientRender).WatermarkAlpha)
				{
					return;
				}

				((GradientBackgroundRender)this._gradientRender).WatermarkAlpha = value;

				base.Invalidate();
			}
		}

		/// <summary>
		/// The ContentAlignment associated with this controls watermark image.
		/// </summary>
		/// <value>
		/// <para>
		/// System.Drawing.ContentAlignment . The ContentAlignment associated with this controls watermark image.
		/// </para>
		/// <para>
		/// This property is read/write.
		/// </para>
		/// </value>
		public ContentAlignment WatermarkImageAlign
		{
			get
			{
				return ((GradientBackgroundRender)this._gradientRender).WatermarkImageAlign;

			}

			set
			{
				if (value == ((GradientBackgroundRender)this._gradientRender).WatermarkImageAlign)
				{
					return;

				}

				((GradientBackgroundRender)this._gradientRender).WatermarkImageAlign = value;

				base.Invalidate(true);
			}
		}

		/// <summary>
		/// Gets or sets the display style of watermark image.
		/// </summary>
		public WrapMode WatermarkDisplayStyle
		{
			get
			{
				return ((GradientBackgroundRender)this._gradientRender).WatermarkDisplayStyle;

			}

			set
			{
				if (value == ((GradientBackgroundRender)this._gradientRender).WatermarkDisplayStyle)
				{
					return;

				}

				((GradientBackgroundRender)this._gradientRender).WatermarkDisplayStyle = value;

				base.Invalidate();
			}
		}

		public GradientPanel()
			:base()
		{
			InitializeComponent();

			this.TabStop = false;

			base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Selectable, true);
			base.BackColor = Color.Transparent;

			this.InitializeRender();
		}

		/// <summary>
		/// Initializes the class used for rendering.
		/// Rendering class must be based on IGradientReder.
		/// </summary>
		protected virtual void InitializeRender()
		{
			this._gradientRender = new GradientBackgroundRender();

		}

		/// <summary>
		/// Gets the default size of the control.
		/// </summary>
		/// <value>
		/// The default Size of the control.
		/// </value>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(151, 105);

			}

		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A System.Windows.Forms.PaintEventArgs that contains the event data.</param>
		/// <remarks>
		/// <para>
		/// Raising an event invokes the event handler through a delegate. For more information, Raising an Event.
		/// </para>
		/// <para>
		/// The OnPaint method also allows derived classes to handle the event without attaching a delegate. This is the preferred technique for handling the event in a derived class.
		/// </para>
		/// <para>
		/// Note for Inheritors:
		/// When overriding OnPaint in a derived class, be sure to call the base class's OnPaint method so that registered delegates receive the event.
		/// </para>
		/// </remarks>
		protected override void OnPaint(PaintEventArgs pe)
		{
			if (!this.Disposing)
			{
				base.OnPaint(pe);

				//Rectangle rectangle = new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
				this._gradientRender.DisplayRectangle = this.ClientRectangle;
				this._gradientRender.Render(pe);
			}
		}

		/// <summary>
		/// Raises the SizeChanged event.
		/// </summary>
		/// <param name="e">An System.EventArgs that contains the event data.</param>
		protected override void OnSizeChanged(System.EventArgs e)
		{
			base.OnSizeChanged(e);

			this.Invalidate();

		}


		/// <summary>
		/// Fires the event indicating that the gradient panel has been resized. Inheriting controls should use this in favour of actually listening to the event, but should not forget to call base.onResize() to ensure that the event is still fired for external listeners.
		/// </summary>
		/// <param name="e">An System.EventArgs that contains the event data.</param>
		protected override void OnResize(System.EventArgs e)
		{
			base.OnResize(e);

			base.Invalidate();

		}

		/// <summary>
		/// Fires the event indicating that control padding has changed. Inheriting controls should use this in favour of actually listening to the event, but should not forget to call base.OnPaddingChanged() to ensure that the event is still fired for external listeners.
		/// </summary>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		protected override void OnPaddingChanged(System.EventArgs e)
		{
			((GradientBackgroundRender)this._gradientRender).Padding = this.Padding;

			base.OnPaddingChanged(e);

		}


		/// <summary>
		/// Fired when the background is painted.
		/// </summary>
		/// <param name="pevent">Paint event arguments.</param>
		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);

		}

		/// <param name="e">An System.EventArgs that contains the event data.</param>
		protected override void OnEnabledChanged(System.EventArgs e)
		{
			base.OnEnabledChanged(e);

			this._gradientRender.Enabled = this.Enabled;

			base.Invalidate();

		}

		/// <summary>
		/// Raises the SystemColorsChanged event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnSystemColorsChanged(System.EventArgs e)
		{
			this._gradientRender.ResetColors();

			base.OnSystemColorsChanged(e);

			base.Invalidate();

		}

		/// <summary>
		/// Raises the RightToLeftChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnRightToLeftChanged(System.EventArgs e)
		{
			base.OnRightToLeftChanged(e);

			((GradientBackgroundRender)this._gradientRender).RightToLeft = (this.RightToLeft == RightToLeft.Yes);

		}
	}
}
