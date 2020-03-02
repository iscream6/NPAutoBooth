using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FadeFox.UI.DataGridViewColumnTypeAddIn
{
	public class DataGridViewNumericUpDownCell : DataGridViewTextBoxCell
	{
		public DataGridViewNumericUpDownCell()
			: base()
		{
			// Use the short date format.
		}

		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			// Set the value of the editing control to the current cell value.
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			DataGridViewNumericUpDownEditingControl ctl = DataGridView.EditingControl as DataGridViewNumericUpDownEditingControl;

			string format = dataGridViewCellStyle.Format;
			
			if (format == string.Empty)
			{
				ctl.DecimalPlaces = 0;
			}
			else
			{
				if (format.Length == 2)
				{
					if (format.Substring(0, 1) == "N")
					{
						string places = format.Substring(1, 1);

						int temp;

						if (int.TryParse(places, out temp))
						{
							ctl.DecimalPlaces = temp;
						}
						else
						{
							base.Style.Format = "N0";
							ctl.DecimalPlaces = 0;
						}
					}
				}
				else
				{
					base.Style.Format = "N0";
					ctl.DecimalPlaces = 0;
				}
			}

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
				ctl.Value = 0;
			else
				ctl.Value = Convert.ToDecimal(val);
		}

		public override Type EditType
		{
			get
			{
				// Return the type of the editing contol that CalendarCell uses.
				return typeof(DataGridViewNumericUpDownEditingControl);
			}
		}

		public override Type ValueType
		{
			get
			{
				// Return the type of the value that CalendarCell contains.
				return typeof(Decimal);
			}
		}

		public override object DefaultNewRowValue
		{
			get
			{
				// Use the current date and time as the default value.
				return 0;
			}
		}
	}
}
