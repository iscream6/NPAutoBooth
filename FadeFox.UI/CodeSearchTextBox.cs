/*
 * ==============================================================================
 *   Program ID     :
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   :
 *   Developer      :
 *   Create Date    : 2009-08-24
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

namespace FadeFox.UI
{
	public delegate bool CodeSearchTextBoxBoolDelegate();

	[ToolboxBitmap(typeof(System.Windows.Forms.TextBox))]
	public partial class CodeSearchTextBox : UserControl
	{
		Form mSearchForm = null;
		ICodeSearchForm mCodeSearchForm = null;
		CodeChangeEventHandler mCodeChanged;
		CodeClearEventHandler mCodeCleared;
		Object mData = null;
		Dictionary<string, string> mCodeData = new Dictionary<string, string>();

		// 검색창을 띄울지 안띄울지  시작과 종료시 실행될 델리게이트 인수는 (true, false로 넘어옴)
		private CodeSearchTextBoxBoolDelegate mOpenSearchFormAction = null;
		[Browsable(false)]
		public CodeSearchTextBoxBoolDelegate OpenSearchFormAction
		{
			get { return mOpenSearchFormAction; }
			set { mOpenSearchFormAction = value; }
		}

		public event CodeChangeEventHandler CodeChanged
		{
			add
			{
				mCodeChanged = (CodeChangeEventHandler)System.Delegate.Combine(mCodeChanged, value);
			}
			remove
			{
				mCodeChanged = (CodeChangeEventHandler)System.Delegate.Remove(mCodeChanged, value);
			}
		}

		public event CodeChangeEventHandler CodeCleared
		{
			add
			{
				mCodeCleared = (CodeClearEventHandler)System.Delegate.Combine(mCodeCleared, value);
			}
			remove
			{
				mCodeCleared = (CodeClearEventHandler)System.Delegate.Remove(mCodeCleared, value);
			}
		}

		public Form SearchForm
		{
			set
			{
				if (mSearchForm != value)
				{
					mSearchForm = value as Form;
					mCodeSearchForm = mSearchForm as ICodeSearchForm;
				}
			}
		}

		public bool AllowInputCode
		{
			get { return !txtCode.ReadOnly; }
			set { txtCode.ReadOnly = !value; }
		}

		public int CodeMaxLength
		{
			get { return txtCode.MaxLength; }
			set { txtCode.MaxLength = value; }
		}

		public int CodeWidth
		{
			get { return txtCode.Width; }
			set 
			{
				if (value > this.Width - 40)
					return;

				if (txtCode.Width != value)
				{
					int oldCodeWidth = txtCode.Width;

					txtCode.Width = value;

					int changeWidth = oldCodeWidth - txtCode.Width;

					txtCodeName.Location = new Point(txtCodeName.Location.X - changeWidth, txtCodeName.Location.Y);
					boxSplit.Location = new Point(txtCodeName.Location.X - 5, boxSplit.Location.Y);
					txtCodeName.Size = new Size(txtCodeName.Size.Width + changeWidth, txtCodeName.Size.Height);
				}
			}
		}

		public HorizontalAlignment CodeAlign
		{
			get { return txtCode.TextAlign; }
			set { txtCode.TextAlign = value; }
		}

		public string Code
		{
			get { return txtCode.Text; }
			set 
			{
				if (txtCode.Text != value)
				{
					txtCode.Text = value;

					if (!this.DesignMode) // 디자인 모드가 아닐때에만 실행
					{
						if (mAutoCodeName)
						{
							if (mCodeSearchForm != null)
							{
								txtCodeName.Text = mCodeSearchForm.GetCodeName(value);
								mData = mCodeSearchForm.GetCodeInfo(value);
								//mCodeData = mCodeSearchForm.StoredRowData;
								CloneCodeData(mCodeSearchForm.StoredRowData, mCodeData);
							}
							else
							{
								txtCodeName.Text = string.Empty;
								mData = null;
								mCodeData = null;
							}
						}
					}

					if (mCodeChanged != null)
						mCodeChanged(this);
				}
			}
		}

		private void CloneCodeData(Dictionary<string, string> pSource, Dictionary<string, string> pTarget)
		{
			pTarget.Clear();

			if (pSource != null)
			{
				foreach (string key in pSource.Keys)
				{
					pTarget.Add(key, pSource[key]);
				}
			}
		}

		public string CodeName
		{
			get { return txtCodeName.Text; }
			set { txtCodeName.Text = value; }
		}

		private bool mAutoCodeName = false;
		public bool AutoCodeName
		{
			get { return mAutoCodeName; }
			set { mAutoCodeName = value; }
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
				if (txtCodeName.Size.Width < btnClear.Width - 2)
					return;

				if (btnClear.Visible != value)
				{
					if (value)
					{
						txtCodeName.Size = new Size(txtCodeName.Size.Width - btnClear.Width - 2, txtCodeName.Size.Height);
					}
					else
					{
						txtCodeName.Size = new Size(txtCodeName.Size.Width + btnClear.Width + 2, txtCodeName.Size.Height);
					}

					btnClear.Visible = value;
				}
			}
		}

		public bool CodeNameVisible
		{
			get { return txtCodeName.Visible; }
			set { txtCodeName.Visible = value; }
		}

		public CodeSearchTextBox()
		{
			InitializeComponent();
			Initialize();
		}

		public Object Data
		{
			get { return mData; }
		}

		public bool HasCodeData
		{
			get
			{
				if (mCodeData.Count > 0)
					return true;
				else
					return false;
			}
		}

		public Dictionary<string, string> CodeData
		{
			get { return mCodeData; }
		}

		private void Initialize()
		{
			Clear();
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			if (this.DesignMode)
				return;

			if (mOpenSearchFormAction != null)
			{
				if (mOpenSearchFormAction() == false)
					return;
			}

			if (mSearchForm != null)
			{
				if (mSearchForm.ShowDialog() == DialogResult.OK)
				{
					if (mCodeSearchForm != null)
					{
						txtCode.Text = mCodeSearchForm.SelectedCode;
						txtCodeName.Text = mCodeSearchForm.SelectedCodeName;


						mData = mCodeSearchForm.GetCodeInfo(mCodeSearchForm.SelectedCode);
						//mCodeData = mCodeSearchForm.StoredRowData;
						CloneCodeData(mCodeSearchForm.StoredRowData, mCodeData);

						mCanceled = false;

						if (mCodeChanged != null)
							mCodeChanged(this);
					}
					else
					{
						Clear();
						mCanceled = true;
					}
				}
				else
				{
					mCanceled = true;
				}
			}
			else
			{
				mCanceled = true;
			}
		}

		public void Clear()
		{
			txtCode.Text = string.Empty;
			txtCodeName.Text = string.Empty;
			mData = null;
			mCodeData.Clear();
		}

		public object GetCodeInfo(string pCode)
		{
			if (mCodeSearchForm == null)
				return null;
			else
				return mCodeSearchForm.GetCodeInfo(pCode);
		}

		public void SetCodeName()
		{
			if (this.DesignMode)
				return;

			if (mCodeSearchForm != null)
			{
				txtCodeName.Text = mCodeSearchForm.GetCodeName(txtCode.Text);
				CloneCodeData(mCodeSearchForm.StoredRowData, mCodeData);
			}
			else
			{
				Clear();
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			if (this.DesignMode)
				return;

			Clear();
			if (mCodeCleared != null)
				mCodeCleared(this);
		}
	}

	public delegate void CodeChangeEventHandler(CodeSearchTextBox sender);
	public delegate void CodeClearEventHandler(CodeSearchTextBox sender);
}
