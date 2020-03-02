/*
 * ==============================================================================
 *   Program ID     :
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   :
 *   Developer      :
 *   Create Date    : 2009-09-08
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FadeFox.UI
{
	[ToolboxBitmap(typeof(System.Windows.Forms.TextBox))]
	public partial class OpenFileTextBox : UserControl
	{
		OpenFileTextChangeEventHandler mOpenFileTextChanged;
		OpenFileTextClearEventHandler mOpenFileTextCleared;

		public event OpenFileTextChangeEventHandler OpenFileTextChanged
		{
			add
			{
				mOpenFileTextChanged = (OpenFileTextChangeEventHandler)System.Delegate.Combine(mOpenFileTextChanged, value);
			}
			remove
			{
				mOpenFileTextChanged = (OpenFileTextChangeEventHandler)System.Delegate.Remove(mOpenFileTextChanged, value);
			}
		}

		public event OpenFileTextClearEventHandler OpenFileTextCleared
		{
			add
			{
				mOpenFileTextCleared = (OpenFileTextClearEventHandler)System.Delegate.Combine(mOpenFileTextCleared, value);
			}
			remove
			{
				mOpenFileTextCleared = (OpenFileTextClearEventHandler)System.Delegate.Remove(mOpenFileTextCleared, value);
			}
		}

		public HorizontalAlignment TextAlign
		{
			get { return txtOpenFileText.TextAlign; }
			set { txtOpenFileText.TextAlign = value; }
		}

		private bool mAllowFileCheck = false;
		public bool AllowFileCheck
		{
			get { return mAllowFileCheck; }
			set { mAllowFileCheck = value; }
		}

		private string mFilter = "모든 파일 (*.*)|*.*";
		public string Filter
		{
			get { return mFilter; }
			set { mFilter = value; }
		}

		public new string Text
		{
			get { return txtOpenFileText.Text; }
			set
			{
				if (value != string.Empty)
				{
					if (mAllowFileCheck)
					{
						if (File.Exists(value))
						{
							txtOpenFileText.Text = value;
						}
						else
						{
							MessageBox.Show("존재하지 않는 파일입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
					else
					{
						txtOpenFileText.Text = value;
					}
				}
				else
				{
					txtOpenFileText.Text = string.Empty;
				}
			}
		}

		public string GetFileName()
		{
			try
			{
				return Path.GetFileName(txtOpenFileText.Text);
			}
			catch
			{
				return string.Empty;
			}
		}

		public string GetDirectoryName()
		{
			try
			{
				return Path.GetDirectoryName(txtOpenFileText.Text);
			}
			catch
			{
				return string.Empty;
			}
		}

		public bool Exists()
		{
			if (txtOpenFileText.Text == string.Empty)
				return false;
			else
			{
				if (File.Exists(txtOpenFileText.Text))
					return true;
				else
					return false;
			}
		}

		private bool mCanceled = true;
		public bool Canceled
		{
			get { return mCanceled; }
		}

		public bool ClearButtonVisible
		{
			get { return btnClear.Visible; }
			set
			{
				if (txtOpenFileText.Size.Width < btnClear.Width - 2)
					return;

				if (btnClear.Visible != value)
				{
					if (value)
					{
						txtOpenFileText.Size = new Size(txtOpenFileText.Size.Width - btnClear.Width - 2, txtOpenFileText.Size.Height);
					}
					else
					{
						txtOpenFileText.Size = new Size(txtOpenFileText.Size.Width + btnClear.Width + 2, txtOpenFileText.Size.Height);
					}

					btnClear.Visible = value;
				}
			}
		}

		public OpenFileTextBox()
		{
			InitializeComponent();
			Initialize();
		}

		private void Initialize()
		{
			Clear();
		}

		public OpenFileTextBox(string pDateTimeText)
			: this()
		{
			this.Text = pDateTimeText;
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			ofOpen.Filter = mFilter;
			if (ofOpen.ShowDialog() == DialogResult.OK)
			{
				txtOpenFileText.Text = ofOpen.FileName;
				mCanceled = false;

				if (mOpenFileTextChanged != null)
					mOpenFileTextChanged(this);
			}
			else
			{
				mCanceled = true;
			}
		}

		public void Clear()
		{
			txtOpenFileText.Text = string.Empty;
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			Clear();
			if (mOpenFileTextCleared != null)
				mOpenFileTextCleared(this);
		}
	}

	public delegate void OpenFileTextChangeEventHandler(OpenFileTextBox sender);
	public delegate void OpenFileTextClearEventHandler(OpenFileTextBox sender);
}
