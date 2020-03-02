using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FadeFox.UI.DataGridViewColumnTypeAddIn
{
	public class DataGridViewDatePickerCell : DataGridViewTextBoxCell
	{
		public DataGridViewDatePickerCell()	: base()
		{
			// Use the short date format.
			this.Style.Format = "yyyy-MM-dd";
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			// Set the value of the editing control to the current cell value.
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			DataGridViewDatePickerEditingControl ctl = DataGridView.EditingControl as DataGridViewDatePickerEditingControl;

			// ++ vic 14-aug-2009
			object val = null;
			try
			{
				val = this.Value;
			}
			catch
			{
				// Argument ot of range (value doesn't exists in collection)
				return;
			}

			if (val == System.DBNull.Value || Convert.ToString(val) == string.Empty)
				ctl.Value = string.Empty;
			else
				ctl.Value = Convert.ToDateTime(val);
				
		}

		public override Type EditType
		{
			get
			{
				// Return the type of the editing contol that CalendarCell uses.
				return typeof(DataGridViewDatePickerEditingControl);
			}
		}

		public override Type ValueType
		{
			get
			{
				// Return the type of the value that CalendarCell contains.
				//return typeof(DateTime);
				return typeof(string);
			}
		}

		public override object DefaultNewRowValue
		{
			get
			{
				// Use the current date and time as the default value.
				return string.Empty;
			}
		}
	}
}
