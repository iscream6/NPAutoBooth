/* 
 * ==============================================================================
 *   Program ID     : 
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description    : 다른 스레드에서 UI 업데이트를 위해 사용
 * ------------------------------------------------------------------------------
 *   Company Name   : sayou.net
 *   Developer      : 
 *   Create Date    : 2007-06-04
 * ------------------------------------------------------------------------------
 *   Update History
 *       2009-07-03 : 라이브러리 재 작성에 따른 변경
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FadeFox.UI
{
	public class InvokeC
	{
		private delegate string GetStringHandler();
		private delegate bool GetBoolHandler();
		private delegate DateTime GetDateTimeHandler();
		private delegate int GetIntHandler();
		private delegate long GetLongHandler();
		private delegate decimal GetDecimalHandler();

		#region ListBox

		public static void Add(ListBox pListBox, object pItem)
		{
			try
			{
				if (pListBox.InvokeRequired)
				{
					pListBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pListBox.Items.Add(pItem);
							}
						)
					);
				}
				else
				{
					pListBox.Items.Add(pItem);
				}
			}
			catch
			{
				return;
			}
		}

		public static void Insert(ListBox pListBox, object pItem)
		{
			try
			{
				if (pListBox.InvokeRequired)
				{
					pListBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pListBox.Items.Insert(0, pItem);
							}
						)
					);
				}
				else
				{
					pListBox.Items.Insert(0, pItem);
				}
			}
			catch
			{
				return;
			}
		}

		public static void DoScroll(ListBox pListBox)
		{
			try
			{
				if (pListBox.InvokeRequired)
				{
					pListBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pListBox.SelectedIndex = pListBox.Items.Count - 1;
							}
						)
					);
				}
				else
				{
					pListBox.SelectedIndex = pListBox.Items.Count - 1;
				}
			}
			catch
			{
				return;
			}
		}

		#endregion

		#region DataGridView

		public static void AddRow(DataGridView pGrid, DataGridViewRow pRow)
		{
			try
			{
				if (pGrid.InvokeRequired)
				{
					pGrid.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pGrid.Rows.Add(pRow);
							}
						)
					);
				}
				else
				{
					pGrid.Rows.Add(pRow);
				}
			}
			catch
			{
				return;
			}
		}

		public static void AddRow(DataGridView pGrid, string[] pRow)
		{
			try
			{
				if (pGrid.InvokeRequired)
				{
					pGrid.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pGrid.Rows.Add(pRow);
							}
						)
					);
				}
				else
				{
					pGrid.Rows.Add(pRow);
				}
			}
			catch
			{
				return;
			}
		}

		public static void AddRow(DataGridView pGrid, object[] pRow)
		{
			try
			{
				if (pGrid.InvokeRequired)
				{
					pGrid.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pGrid.Rows.Add(pRow);
							}
						)
					);
				}
				else
				{
					pGrid.Rows.Add(pRow);
				}
			}
			catch
			{
				return;
			}
		}
		public static void Clear(DataGridView pGrid)
		{
			try
			{
				if (pGrid.InvokeRequired)
				{
					pGrid.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pGrid.Rows.Clear();
							}
						)
					);
				}
				else
				{
					pGrid.Rows.Clear();
				}
			}
			catch
			{
				return;
			}
		}

		public static void SetVisible(DataGridView pGrid, bool pVisible)
		{
			try
			{
				if (pGrid.InvokeRequired)
				{
					pGrid.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pGrid.Visible = pVisible;
							}
						)
					);
				}
				else
				{
					pGrid.Visible = pVisible;
				}
			}
			catch
			{
				return;
			}
		}

		public static void SetValue(DataGridView pGrid, int pRowIndex, int pCellIndex, string pValue)
		{
			try
			{
				if (pGrid.InvokeRequired)
				{
					pGrid.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								if (pRowIndex < pGrid.Rows.Count - 1)
									return;

								if (pCellIndex < pGrid.Columns.Count - 1)
									return;

								pGrid.Rows[pRowIndex].Cells[pCellIndex].Value = pValue;
							}
						)
					);
				}
				else
				{
					if (pRowIndex < pGrid.Rows.Count - 1)
						return;

					if (pCellIndex < pGrid.Columns.Count - 1)
						return;

					pGrid.Rows[pRowIndex].Cells[pCellIndex].Value = pValue;
				}
			}
			catch
			{
				return;
			}
		}

		public static void SetValue(DataGridView pGrid, int pRowIndex, string pCellName, string pValue)
		{
			try
			{
				if (pGrid.InvokeRequired)
				{
					pGrid.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								if (pRowIndex < pGrid.Rows.Count - 1)
									return;

								pGrid.Rows[pRowIndex].Cells[pCellName].Value = pValue;
							}
						)
					);
				}
				else
				{
					if (pRowIndex < pGrid.Rows.Count - 1)
						return;

					pGrid.Rows[pRowIndex].Cells[pCellName].Value = pValue;
				}
			}
			catch
			{
				return;
			}
		}

		#endregion

		#region DatetimePicker

		public static void SetValue(DateTimePicker pDateTimePicker, DateTime pValue)
		{
			try
			{
				if (pDateTimePicker.InvokeRequired)
				{
					pDateTimePicker.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pDateTimePicker.Value = pValue;
							}
						)
					);
				}
				else
				{
					pDateTimePicker.Value = pValue;
				}
			}
			catch
			{
				return;
			}
		}

		public static DateTime GetValue(DateTimePicker pDateTimePicker)
		{
			try
			{
				if (pDateTimePicker.InvokeRequired)
				{
					return (DateTime)pDateTimePicker.Invoke
						(
							new GetDateTimeHandler(
									delegate()
									{
										return pDateTimePicker.Value;
									}
								)
						);
				}
				else
				{
					return pDateTimePicker.Value;
				}
			}
			catch
			{
				return DateTime.Parse("0001-01-01 00:00:00");
			}
		}

		public static string GetText(DateTimePicker pDateTimePicker)
		{
			try
			{
				if (pDateTimePicker.InvokeRequired)
				{
					return pDateTimePicker.Invoke
						(
							new GetStringHandler(
									delegate()
									{
										return pDateTimePicker.Text;
									}
								)
						) as string;
				}
				else
				{
					return pDateTimePicker.Text;
				}
			}
			catch
			{
				return string.Empty;
			}
		}

		#endregion

		#region NumericUpDown

		public static void SetValue(NumericUpDown pNumericUpDown, decimal pValue)
		{
			try
			{
				if (pNumericUpDown.InvokeRequired)
				{
					pNumericUpDown.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pNumericUpDown.Value = pValue;
							}
						)
					);
				}
				else
				{
					pNumericUpDown.Value = pValue;
				}
			}
			catch
			{
				return;
			}
		}

		public static decimal GetValue(NumericUpDown pNumericUpDown)
		{
			try
			{
				if (pNumericUpDown.InvokeRequired)
				{
					return (decimal)pNumericUpDown.Invoke
						(
							new GetDecimalHandler(
									delegate()
									{
										return pNumericUpDown.Value;
									}
								)
						);
				}
				else
				{
					return pNumericUpDown.Value;
				}
			}
			catch
			{
				return 0m;
			}
		}

		#endregion

		#region Label

		public static void SetText(Label pLabel, string pText)
		{
			try
			{
				if (pLabel.InvokeRequired)
				{
					pLabel.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pLabel.Text = pText;
							}
						)
					);
				}
				else
				{
					pLabel.Text = pText;
				}
			}
			catch
			{
				return;
			}
		}

		public static string GetText(Label pLabel)
		{
			try
			{
				if (pLabel.InvokeRequired)
				{
					return pLabel.Invoke
						(
							new GetStringHandler(
									delegate()
									{
										return pLabel.Text;
									}
								)
						) as string;
				}
				else
				{
					return pLabel.Text;
				}
			}
			catch
			{
				return string.Empty;
			}
		}

		#endregion

		#region ComboBox

		public static void SetText(ComboBox pComboBox, string pText)
		{
			try
			{
				if (pComboBox.InvokeRequired)
				{
					pComboBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pComboBox.Text = pText;
							}
						)
					);
				}
				else
				{
					pComboBox.Text = pText;
				}
			}
			catch
			{
				return;
			}
		}

		public static string GetText(ComboBox pComboBox)
		{
			try
			{
				if (pComboBox.InvokeRequired)
				{
					return pComboBox.Invoke
						(
							new GetStringHandler(
									delegate()
									{
										return pComboBox.Text;
									}
								)
						) as string;
				}
				else
				{
					return pComboBox.Text;
				}
			}
			catch
			{
				return string.Empty;
			}
		}

		public static int GetSelectedIndex(ComboBox pComboBox)
		{
			try
			{
				if (pComboBox.InvokeRequired)
				{
					return (int)pComboBox.Invoke
						(
							new GetIntHandler(
									delegate()
									{
										return pComboBox.SelectedIndex;
									}
								)
						);
				}
				else
				{
					return pComboBox.SelectedIndex;
				}
			}
			catch
			{
				return -1;
			}
		}

		#endregion

		#region TextBox

		public static void SetText(TextBox pTextBox, string pText)
		{
			try
			{
				if (pTextBox.InvokeRequired)
				{
					pTextBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pTextBox.Text = pText;
							}
						)
					);
				}
				else
				{
					pTextBox.Text = pText;
				}
			}
			catch
			{
				return;
			}
		}

		public static string GetText(TextBox pTextBox)
		{
			try
			{
				if (pTextBox.InvokeRequired)
				{
					return pTextBox.Invoke
						(
							new GetStringHandler(
									delegate()
									{
										return pTextBox.Text;
									}
								)
						) as string;
				}
				else
				{
					return pTextBox.Text;
				}
			}
			catch
			{
				return string.Empty;
			}
		}

		public static void Add(TextBox pControl, string pValue)
		{
			try
			{
				if (pControl.InvokeRequired)
				{
					pControl.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pControl.Text = pControl.Text + pValue;
							}
						)
					);
				}
				else
				{
					pControl.Text = pControl.Text + pValue;
				}
			}
			catch
			{
				return;
			}
		}

		public static void DoScroll(TextBox pControl)
		{
			try
			{
				if (pControl.InvokeRequired)
				{
					pControl.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pControl.Select(pControl.Text.Length, 0);
								pControl.ScrollToCaret();
							}
						)
					);
				}
				else
				{
					pControl.Select(pControl.Text.Length, 0);
					pControl.ScrollToCaret();
				}
			}
			catch
			{
				return;
			}
		}

		#endregion

		#region Panel

		public static void SetVisible(Panel pPanel, bool pVisible)
		{
			try
			{
				if (pPanel.InvokeRequired)
				{
					pPanel.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pPanel.Visible = pVisible;
							}
						)
					);
				}
				else
				{
					pPanel.Visible = pVisible;
				}
			}
			catch
			{
				return;
			}
		}

		#endregion

		#region Button

		public static void SetEnabled(Button pButton, bool pEnabled)
		{
			try
			{
				if (pButton.InvokeRequired)
				{
					pButton.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pButton.Enabled = pEnabled;
							}
						)
					);
				}
				else
				{
					pButton.Enabled = pEnabled;
				}
			}
			catch
			{
				return;
			}
		}

		#endregion

		#region SimpleListBox

		public static void Insert(SimpleListBox pListBox, string pItem)
		{
			try
			{
				if (pListBox.InvokeRequired)
				{
					pListBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pListBox.Insert(pItem);
							}
						)
					);
				}
				else
				{
					pListBox.Insert(pItem);
				}
			}
			catch
			{
				return;
			}
		}

		public static void Insert(SimpleListBox pListBox, SimpleListBoxItem pItem)
		{
			try
			{
				if (pListBox.InvokeRequired)
				{
					pListBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pListBox.Insert(pItem);
							}
						)
					);
				}
				else
				{
					pListBox.Insert(pItem);
				}
			}
			catch
			{
				return;
			}
		}

		public static void Add(SimpleListBox pListBox, string pItem)
		{
			try
			{
				if (pListBox.InvokeRequired)
				{
					pListBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pListBox.Add(pItem);
							}
						)
					);
				}
				else
				{
					pListBox.Add(pItem);
				}
			}
			catch
			{
				return;
			}
		}

		public static void Add(SimpleListBox pListBox, SimpleListBoxItem pItem)
		{
			try
			{
				if (pListBox.InvokeRequired)
				{
					pListBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pListBox.Add(pItem);
							}
						)
					);
				}
				else
				{
					pListBox.Add(pItem);
				}
			}
			catch
			{
				return;
			}
		}

		public static void DoScroll(SimpleListBox pListBox)
		{
			try
			{
				if (pListBox.InvokeRequired)
				{
					pListBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pListBox.DoScroll();
							}
						)
					);
				}
				else
				{
					pListBox.DoScroll();
				}
			}
			catch
			{
				return;
			}
		}

		public static void Remove(SimpleListBox pListBox)
		{
			try
			{
				if (pListBox.InvokeRequired)
				{
					pListBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pListBox.Remove();
							}
						)
					);
				}
				else
				{
					pListBox.Remove();
				}
			}
			catch
			{
				return;
			}
		}

		public static void AddLine(SimpleListBox pListBox)
		{
			try
			{
				if (pListBox.InvokeRequired)
				{
					pListBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pListBox.AddLine();
							}
						)
					);
				}
				else
				{
					pListBox.AddLine();
				}
			}
			catch
			{
				return;
			}
		}

		public static void AddLine(SimpleListBox pListBox, Color pLineColor)
		{
			try
			{
				if (pListBox.InvokeRequired)
				{
					pListBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pListBox.AddLine(pLineColor);
							}
						)
					);
				}
				else
				{
					pListBox.AddLine(pLineColor);
				}
			}
			catch
			{
				return;
			}
		}

		#endregion

		#region SimpleProgressBar

		public static void SetText(SimpleProgressBar pProgressBar, string pText)
		{
			try
			{
				if (pProgressBar.InvokeRequired)
				{
					pProgressBar.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pProgressBar.Text = pText;
							}
						)
					);
				}
				else
				{
					pProgressBar.Text = pText;
				}
			}
			catch
			{
				return;
			}
		}

		public static void SetMaximum(SimpleProgressBar pProgressBar, int pMaximum)
		{
			try
			{
				if (pProgressBar.InvokeRequired)
				{
					pProgressBar.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pProgressBar.Maximum = pMaximum;
							}
						)
					);
				}
				else
				{
					pProgressBar.Maximum = pMaximum;
				}
			}
			catch
			{
				return;
			}
		}

		public static void SetValue(SimpleProgressBar pProgressBar, int pValue)
		{
			try
			{
				if (pProgressBar.InvokeRequired)
				{
					pProgressBar.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pProgressBar.Value = pValue;
							}
						)
					);
				}
				else
				{
					pProgressBar.Value = pValue;
				}
			}
			catch
			{
				return;
			}
		}

		#endregion

		#region GlassButton

		public static void SetEnabled(GlassButton pButton, bool pEnabled)
		{
			try
			{
				if (pButton.InvokeRequired)
				{
					pButton.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pButton.Enabled = pEnabled;
							}
						)
					);
				}
				else
				{
					pButton.Enabled = pEnabled;
				}
			}
			catch
			{
				return;
			}
		}

		#endregion

		#region Text3DLabel

		public static void SetText(Text3DLabel pLabel, string pText)
		{
			try
			{
				if (pLabel.InvokeRequired)
				{
					pLabel.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pLabel.Text = pText;
							}
						)
					);
				}
				else
				{
					pLabel.Text = pText;
				}
			}
			catch
			{
				return;
			}
		}

		public static string GetText(Text3DLabel pLabel)
		{
			try
			{
				if (pLabel.InvokeRequired)
				{
					return pLabel.Invoke
						(
							new GetStringHandler(
									delegate()
									{
										return pLabel.Text;
									}
								)
						) as string;
				}
				else
				{
					return pLabel.Text;
				}
			}
			catch
			{
				return string.Empty;
			}
		}

		#endregion

		#region RadioButton

		public static void SetChecked(RadioButton pRadioButton, bool pChecked)
		{
			try
			{
				if (pRadioButton.InvokeRequired)
				{
					pRadioButton.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pRadioButton.Checked = pChecked;
							}
						)
					);
				}
				else
				{
					pRadioButton.Checked = pChecked;
				}
			}
			catch
			{
				return;
			}
		}

		public static bool GetChecked(RadioButton pRadioButton)
		{
			try
			{
				if (pRadioButton.InvokeRequired)
				{
					return (bool)pRadioButton.Invoke
						(
							new GetBoolHandler(
									delegate()
									{
										return pRadioButton.Checked;
									}
								)
						);
				}
				else
				{
					return pRadioButton.Checked;
				}
			}
			catch
			{
				return false;
			}
		}

		#endregion

		#region CheckBox

		public static void SetChecked(CheckBox pCheckBox, bool pChecked)
		{
			try
			{
				if (pCheckBox.InvokeRequired)
				{
					pCheckBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pCheckBox.Checked = pChecked;
							}
						)
					);
				}
				else
				{
					pCheckBox.Checked = pChecked;
				}
			}
			catch
			{
				return;
			}
		}

		public static bool GetChecked(CheckBox pCheckBox)
		{
			try
			{
				if (pCheckBox.InvokeRequired)
				{
					return (bool)pCheckBox.Invoke
						(
							new GetBoolHandler(
									delegate()
									{
										return pCheckBox.Checked;
									}
								)
						);
				}
				else
				{
					return pCheckBox.Checked;
				}
			}
			catch
			{
				return false;
			}
		}

		#endregion

		#region CodeSearchTextBox

		public static void SetCode(CodeSearchTextBox pTextBox, string pCode)
		{
			try
			{
				if (pTextBox.InvokeRequired)
				{
					pTextBox.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pTextBox.Code = pCode;
							}
						)
					);
				}
				else
				{
					pTextBox.Text = pCode;
				}
			}
			catch
			{
				return;
			}
		}

		public static string GetCode(CodeSearchTextBox pTextBox)
		{
			try
			{
				if (pTextBox.InvokeRequired)
				{
					return pTextBox.Invoke
						(
							new GetStringHandler(
									delegate()
									{
										return pTextBox.Code;
									}
								)
						) as string;
				}
				else
				{
					return pTextBox.Code;
				}
			}
			catch
			{
				return string.Empty;
			}
		}

		#endregion

		#region ImageButton

		public static void SetEnabled(ImageButton pButton, bool pEnabled)
		{
			try
			{
				if (pButton.InvokeRequired)
				{
					pButton.Invoke
					(
						new MethodInvoker
						(
							delegate()
							{
								pButton.Enabled = pEnabled;
							}
						)
					);
				}
				else
				{
					pButton.Enabled = pEnabled;
				}
			}
			catch
			{
				return;
			}
		}

		#endregion
	}
}
