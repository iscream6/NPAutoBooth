using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FadeFox.UI.DataGridViewColumnTypeAddIn
{
	[ToolboxBitmap(typeof(System.Windows.Forms.NumericUpDown))]
	public class DataGridViewNumericUpDownColumn : DataGridViewColumn
	{
		public DataGridViewNumericUpDownColumn()
			: base(new DataGridViewNumericUpDownCell())
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
				if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewNumericUpDownCell)))
				{
					throw new InvalidCastException("Must be a NumericUpDownCell");
				}
				base.CellTemplate = value;
			}
		}
	}
}
