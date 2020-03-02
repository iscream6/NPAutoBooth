/*
 * ==============================================================================
 *   Program ID     : 
 *   Program Name   : 시리얼포트 선택
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : sayou.net
 *   Developer      : Hyosik-Bae
 *   Create Date    : 2011-02-17
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FadeFox.UI;
using FadeFox.Utility;
using FadeFox.Text;
using Sayou.Core;
using System.IO.Ports;

namespace NPConfig
{
	public partial class SerialPortSearch : DialogForm, ICodeSearchForm
	{

		public SerialPortSearch()
		{
			InitializeComponent();
		}

		private void SerialPortSearch_Load(object sender, EventArgs e)
		{
		}

		private void SerialPortSearch_Shown(object sender, EventArgs e)
		{
			Initialize();
		}

		private void SerialPortSearch_FormClosed(object sender, FormClosedEventArgs e)
		{
		}

		private void Initialize()
		{
			cboPortName.Items.Clear();

			cboPortName.Items.Add("직접입력");

			string[] ports = SerialPort.GetPortNames();

			foreach (string port in ports)
			{
				cboPortName.Items.Add(port);
			}

			cboPortName.SelectedIndex = 0;
			txtPortName.Text = "";
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (cboPortName.Text == "직접입력")
			{
				mSelectedCode = txtPortName.Text;
				mSelectedCodeName = txtPortName.Text;
			}
			else
			{
				mSelectedCode = cboPortName.Text;
				mSelectedCodeName = cboPortName.Text;
			}
		}

		private void cboPortName_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboPortName.Text == "직접입력")
				txtPortName.Enabled = true;
			else
			{
				txtPortName.Enabled = false;
				txtPortName.Text = "";
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			CloseEnabled = true;
			this.Close();
		}

		#region ICodeSearchForm 멤버

		public string TableName
		{
			get { return ""; }
		}

		public string CodeField
		{
			get { return ""; }
		}

		public string CodeNameField
		{
			get { return ""; }
		}

		string mSelectedCode = string.Empty;
		public string SelectedCode
		{
			get { return mSelectedCode; }
		}

		string mSelectedCodeName = string.Empty;
		public string SelectedCodeName
		{
			get { return mSelectedCodeName; }
		}

		public string GetCodeName(string pCode)
		{
			return pCode;
		}

		public object GetCodeInfo(string pCode)
		{
			return null;
		}

		public Dictionary<string, string> StoredRowData
		{
			get
			{
				return null;
			}
		}

		#endregion
	}
}