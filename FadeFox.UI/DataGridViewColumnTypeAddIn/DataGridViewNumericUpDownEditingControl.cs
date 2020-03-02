using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace FadeFox.UI.DataGridViewColumnTypeAddIn
{
	[ToolboxItem(false)]
	public class DataGridViewNumericUpDownEditingControl : NumericUpDown, IDataGridViewEditingControl
	{
		private DataGridView mDataGridView;
		private bool mValueChanged = false;
		private int rowIndex;

		public DataGridViewNumericUpDownEditingControl()
		{
			this.Minimum = decimal.MinValue;
			this.Maximum = decimal.MaxValue;
			this.DecimalPlaces = 0;
		}

		// Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
		// property.
		public object EditingControlFormattedValue
		{
			get
			{
				// return this.Value.ToShortDateString();
				return this.Value.ToString();
			}
			set
			{
				if (value is String)
				{
					this.Value = Convert.ToDecimal(value);
				}
			}
		}

		// Implements the 
		// IDataGridViewEditingControl.GetEditingControlFormattedValue method.
		public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			return EditingControlFormattedValue;
		}

		// Implements the 
		// IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
		public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
		{
			this.Font = dataGridViewCellStyle.Font;
			this.ForeColor = dataGridViewCellStyle.ForeColor;
			this.BackColor = dataGridViewCellStyle.BackColor;
		}

		// Implements the IDataGridViewEditingControl.EditingControlRowIndex 
		// property.
		public int EditingControlRowIndex
		{
			get
			{
				return rowIndex;
			}
			set
			{
				rowIndex = value;
			}
		}

		// Implements the IDataGridViewEditingControl.EditingControlWantsInputKey 
		// method.
		public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
		{
			/*
			// Let the DateTimePicker handle the keys listed.
			switch (key & Keys.KeyCode)
			{
				case Keys.Left:
				case Keys.Up:
				case Keys.Down:
				case Keys.Right:
				case Keys.Home:
				case Keys.End:
				case Keys.PageDown:
				case Keys.PageUp:
					return true;
				default:
					return false;
			}
			 */

			return true;
		}

		// Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit 
		// method.
		public void PrepareEditingControlForEdit(bool selectAll)
		{
			// No preparation needs to be done.
		}

		// Implements the IDataGridViewEditingControl
		// .RepositionEditingControlOnValueChange property.
		public bool RepositionEditingControlOnValueChange
		{
			get
			{
				return false;
			}
		}

		// Implements the IDataGridViewEditingControl
		// .EditingControlDataGridView property.
		public DataGridView EditingControlDataGridView
		{
			get
			{
				return mDataGridView;
			}
			set
			{
				mDataGridView = value;
			}
		}

		// Implements the IDataGridViewEditingControl
		// .EditingControlValueChanged property.
		public bool EditingControlValueChanged
		{
			get
			{
				return mValueChanged;
			}
			set
			{
				mValueChanged = value;
			}
		}

		// Implements the IDataGridViewEditingControl
		// .EditingPanelCursor property.
		public Cursor EditingPanelCursor
		{
			get
			{
				return base.Cursor;
			}
		}

		protected override void OnValueChanged(EventArgs eventargs)
		{
			// Notify the DataGridView that the contents of the cell
			// have changed.
			mValueChanged = true;
			this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
			base.OnValueChanged(eventargs);
		}
	}
}
