using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FadeFox.UI.DataGridViewColumnTypeAddIn
{
	[ToolboxBitmap(typeof(System.Windows.Forms.DateTimePicker))]
	public class DataGridViewDatePickerColumn : DataGridViewColumn
	{
		public DataGridViewDatePickerColumn()
			: base(new DataGridViewDatePickerCell())
		{
		}

		public override DataGridViewCell CellTemplate
		{
			get
			{
				return base.CellTemplate;
			}
			set
			{
				// Ensure that the cell used for the template is a CalendarCell.
				if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewDatePickerCell)))
				{
					throw new InvalidCastException("Must be a CalendarCell");
				}

				base.CellTemplate = value;
			}
		}
	}
}
