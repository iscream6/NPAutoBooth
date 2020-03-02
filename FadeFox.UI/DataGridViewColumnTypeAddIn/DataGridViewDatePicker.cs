using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Threading;

namespace FadeFox.UI.DataGridViewColumnTypeAddIn
{
	[ToolboxBitmap(typeof(System.Windows.Forms.DateTimePicker)), ToolboxItem(false)]
	public partial class DataGridViewDatePicker : System.Windows.Forms.DateTimePicker
	{
		private bool mIsEmpty = false;
		private DateTimePickerFormat mFormat = DateTimePickerFormat.Short;

		public DataGridViewDatePicker() : base()
		{
			InitializeComponent();
			base.Format = DateTimePickerFormat.Custom;
			Format = DateTimePickerFormat.Short;
		}

		[Browsable(true)]
		[DefaultValue(DateTimePickerFormat.Short), TypeConverter(typeof(Enum))]
		public new DateTimePickerFormat Format
		{
			get { return mFormat; }
			set
			{
				mFormat = value;
				if (!mIsEmpty)
					SetFormat();
				OnFormatChanged(EventArgs.Empty);
			}
		}

		[Bindable(true), Browsable(false)]
		public new object Value
		{
			get
			{
				if (mIsEmpty)
					return string.Empty;
				else
					return base.Value.ToString("yyyy-MM-dd");
			}
			set
			{
				if (value == null || value == DBNull.Value)
				{
					SetEmptyValue();
				}
				else
				{
					if (Convert.ToString(value) == string.Empty)
					{
						SetEmptyValue();
					}
					else
					{
						SetDateTimeValue();
						base.Value = Convert.ToDateTime(value);
					}
				}
			}
		}

		private string mCustomFormat;
		public new string CustomFormat
		{
			get { return mCustomFormat; }
			set { mCustomFormat = value; }
		}

		private string mFormatAsString;
		private string FormatAsString
		{
			get { return mFormatAsString; }
			set
			{
				mFormatAsString = value;
				base.CustomFormat = value;
			}
		}

		private void SetEmptyValue()
		{
			mIsEmpty = true;
			base.CustomFormat = " ";
		}

		private void SetDateTimeValue()
		{
			if (mIsEmpty)
			{
				mIsEmpty = false;
				SetFormat();

				if (Convert.ToString(this.Value) != string.Empty)
				{
					base.Value = Convert.ToDateTime(this.Value);
					this.OnValueChanged(EventArgs.Empty);
				}
			}
		}

		private void SetFormat()
		{
			CultureInfo ci = Thread.CurrentThread.CurrentCulture;
			DateTimeFormatInfo dtf = ci.DateTimeFormat;

			switch (mFormat)
			{
				case DateTimePickerFormat.Long:
					FormatAsString = dtf.LongDatePattern;
					break;
				case DateTimePickerFormat.Short:
					FormatAsString = dtf.ShortDatePattern;
					break;
				case DateTimePickerFormat.Time:
					FormatAsString = dtf.ShortTimePattern;
					break;
				case DateTimePickerFormat.Custom:
					FormatAsString = this.CustomFormat;
					break;
			}
		}

		
		protected override void WndProc(ref Message m)
		{
			if (mIsEmpty)
			{
				if (m.Msg == 0x4e) // WM_NOTIFY
				{
					NMHDR nm = (NMHDR)m.GetLParam(typeof(NMHDR));

					if (nm.Code == -746 || nm.Code == -722)  // DTN_CLOSEUP || DTN_?
						SetDateTimeValue();
				}
			}
			base.WndProc(ref m);
		}
		
		/*
		protected override void OnCloseUp(EventArgs eventargs)
		{
			if (mIsEmpty)
			{
				SetDateTimeValue();
			}

			base.OnCloseUp(eventargs);
		}
		 **/

		[StructLayout(LayoutKind.Sequential)]
		private struct NMHDR
		{
			public IntPtr HwndFrom;
			public int IdFrom;
			public int Code;
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				this.Value = null;
				OnValueChanged(EventArgs.Empty);
			}

			base.OnKeyUp(e);
		}

		protected override void OnValueChanged(EventArgs eventargs)
		{
			base.OnValueChanged(eventargs);
		}

		public string ToShortDateString()
		{
			if (mIsEmpty)
			{
				return String.Empty;
			}
			else
			{
				return Convert.ToDateTime(Value).ToString("yyyy-MM-dd");
			}
		}
	}
}
