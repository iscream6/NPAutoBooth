/*
 * ==============================================================================
 *   Program ID     : 
 *   Program Name   : 시리얼포트 연결관리
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : (주)넥스트소프트
 *   Developer      : Hyosik-Bae
 *   Create Date    : 2011-05-09
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
using FadeFox.Text;
using FadeFox.Utility;
using Sayou.Core;
using FadeFox.Security;
using FadeFox.Database.SQLite;

namespace NPConfig
{
	public partial class SerialPortConnectionSetting : DialogForm
	{
		DataProcType mProcType = DataProcType.None;
		SQLite mConfig = new SQLite();
		string mConfigFilePath = "";

		public SerialPortConnectionSetting(string pConfigFilePath)
		{
			InitializeComponent();
			mConfigFilePath = pConfigFilePath;
		}

		private void SerialPortConnectionSetting_Load(object sender, EventArgs e)
		{
		}

		private void SerialPortConnectionSetting_Shown(object sender, EventArgs e)
		{
			Initialize();

			mConfig.Database = mConfigFilePath;
			mConfig.Connect();

			string sql = "";
			sql = "  SELECT *"
				+ "    FROM ST_SERIALPORT"
				+ "   WHERE 1 = 1 " + (SYS.IsAdminMode ? "" : "AND USE_YN = 'Y'")
				+ "   ORDER BY SERIALPORT_NAME";

			grdList.ImportT(mConfig, sql);
		}

		private void SerialPortConnectionSetting_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (grdList.IsBusy)
			{
				MsgBox.Show("작업중인 내용이 있습니다.");
				e.Cancel = true;
			}

			if (!btnClose.Enabled)
			{
				e.Cancel = true;
			}
		}

		private void SerialPortConnectionSetting_FormClosed(object sender, FormClosedEventArgs e)
		{
			mConfig.Disconnect();
		}

		private void Initialize()
		{
			this.MenuName = "시리얼포트 연결관리";
			grdList.RowSelected = SelectRowScreen;
			grdList.RowsPreAdding = ImportBeforeRowAdd;
			grdList.RowsAdding = ImportRunScreen;

			txtPortName.SearchForm = new SerialPortSearch();

			ActionScreen(DataProcType.None);

			//btnInsert.Visible = (SYS.IsAdminMode ? true : false);
			//btnDelete.Visible = (SYS.IsAdminMode ? true : false);
			btnInsert.Visible = true;
			btnDelete.Visible = true;
			chkContinueInsertMode.Visible = false;
			btnSave.Visible = false;
			btnCancel.Visible = false;

			ClearInputScreen();
			pnlInput.Enabled = false;

			grdList.Columns["USE_YN"].Visible = (SYS.IsAdminMode ? true : false);
			lblUseYN.Visible = (SYS.IsAdminMode ? true : false);
			chkUseYN.Visible = (SYS.IsAdminMode ? true : false);
		}

		private void SelectRowScreen()
		{
			if (mProcType == DataProcType.None)
			{
				grdList.StoreRowData(grdList.SelectedRows[0].Index);

				txtSerialPortID.Text = grdList.StoredRowData["SERIALPORT_ID"];
				txtSerialPortName.Text = grdList.StoredRowData["SERIALPORT_NAME"];
				txtSerialPortComment.Text = grdList.StoredRowData["SERIALPORT_COMMENT"];
				txtPortName.Code = grdList.StoredRowData["PORTNAME"];
				cboBaudRate.Text = grdList.StoredRowData["BAUDRATE"];
				cboParity.Text = grdList.StoredRowData["PARITY"];
				cboDataBits.Text = grdList.StoredRowData["DATABITS"];
				cboStopBits.Text = grdList.StoredRowData["STOPBITS"];
				cboHandshake.Text = grdList.StoredRowData["HANDSHAKE"];
				chkUseYN.Checked = (grdList.StoredRowData["USE_YN"] == "Y" ? true : false);
			}
		}

		private void ImportBeforeRowAdd(DataTable pDT)
		{
		}

		private void ImportRunScreen(bool pRun)
		{
			pnlInput.Enabled = false;

			if (pRun)
			{
				this.Cursor = Cursors.WaitCursor;

				pnlControl.Enabled = false;

				lblRowCount.Text = "_";
			}
			else
			{
				btnInsert.Enabled = true;
				btnClose.Enabled = true;

				if (grdList.Rows.Count > 0)
				{
					btnUpdate.Enabled = true;
					btnDelete.Enabled = true;

					grdList.SelectRow(0);

					lblRowCount.Text = grdList.Rows.Count.ToString();
				}
				else
				{
					btnUpdate.Enabled = false;
					btnDelete.Enabled = false;

					ClearInputScreen();

					lblRowCount.Text = "0";
				}

				pnlControl.Enabled = true;

				this.Cursor = Cursors.Default;
			}
		}

		private void ClearInputScreen()
		{
			txtSerialPortID.Text = "";
			txtSerialPortName.Text = "";
			txtSerialPortComment.Text = "";
			txtPortName.Clear();
			cboBaudRate.SelectedIndex = 1;
			cboParity.SelectedIndex = 0;
			cboDataBits.SelectedIndex = 0;
			cboStopBits.SelectedIndex = 0;
			cboHandshake.SelectedIndex = 1;
			chkUseYN.Checked = true;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			CloseEnabled = true;
			this.Close();
		}

		private void btnInsert_Click(object sender, EventArgs e)
		{
			ActionScreen(DataProcType.Insert);
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			ActionScreen(DataProcType.Update);
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			ActionScreen(DataProcType.Delete);

			if (MsgBox.Show("삭제하시겠습니까?", MsgType.Question) == DialogResult.Yes)
			{
				if (CheckData(DataProcType.Delete))
				{
					if (SaveData(DataProcType.Delete))
					{
						ActionScreen(DataProcType.None);
					}
				}
				else
				{
					ActionScreen(DataProcType.None);
				}
			}
			else
			{
				ActionScreen(DataProcType.None);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (CheckData(mProcType))
			{
				if (SaveData(mProcType))
				{
					if (mProcType == DataProcType.Insert && chkContinueInsertMode.Checked == true)
					{
						ActionScreen(DataProcType.Insert);
					}
					else
					{
						ActionScreen(DataProcType.None);
					}
				}
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			ActionScreen(DataProcType.None);
		}

		private void ActionScreen(DataProcType pProcType)
		{
			mProcType = pProcType;

			switch (pProcType)
			{
				case DataProcType.Insert:
					lblSubject.Text = this.MenuName + " 추가";
					pnlInput.Enabled = true;

					txtSerialPortID.Enabled = true;
					txtSerialPortID.Focus();

					ClearInputScreen();

					chkContinueInsertMode.Visible = true;
					btnSave.Visible = true;
					btnCancel.Visible = true;

					btnInsert.Visible = false;
					btnUpdate.Visible = false;
					btnDelete.Visible = false;
					btnClose.Visible = false;

					break;

				case DataProcType.Update:
					lblSubject.Text = this.MenuName + " 수정";
					pnlInput.Enabled = true;

					txtSerialPortID.Enabled = false;
					txtSerialPortName.Focus();

					chkContinueInsertMode.Visible = false;
					btnSave.Visible = true;
					btnCancel.Visible = true;

					btnInsert.Visible = false;
					btnUpdate.Visible = false;
					btnDelete.Visible = false;
					btnClose.Visible = false;

					break;

				case DataProcType.Delete:
					lblSubject.Text = this.MenuName + " 삭제";
					pnlInput.Enabled = false;

					pnlControl.Enabled = false;
					break;

				case DataProcType.None:
					lblSubject.Text = this.MenuName;
					pnlInput.Enabled = false;

					if (grdList.Rows.Count > 0)
					{
						btnUpdate.Enabled = true;
						btnDelete.Enabled = true;
						SelectRowScreen();
					}
					else
					{
						btnUpdate.Enabled = false;
						btnDelete.Enabled = false;
						ClearInputScreen();
					}

					chkContinueInsertMode.Visible = false;
					btnSave.Visible = false;
					btnCancel.Visible = false;

					btnInsert.Visible = true;
					btnUpdate.Visible = true;
					btnDelete.Visible = true;
					btnClose.Visible = true;

					pnlControl.Enabled = true;
					break;
			}

			lblRowCount.Text = grdList.Rows.Count.ToString();
		}

		private bool CheckData(DataProcType pProcType)
		{
			string sql = string.Empty;
			string msg = string.Empty;

			switch (pProcType)
			{
				case DataProcType.Insert:
					sql = "  SELECT COUNT(*)"
						+ "    FROM ST_SERIALPORT"
						+ "   WHERE SERIALPORT_ID = '" + txtSerialPortID.Text + "'";

					string count = mConfig.SelectC(sql);

					if (count != "0")
					{
						MsgBox.Show(txtSerialPortID.Text + " 는 사용할 수 없는 코드입니다.", MsgType.Warning);
						txtSerialPortID.Focus();
						return false;
					}
					break;

				case DataProcType.Update:
					break;

				case DataProcType.Delete:
					return true;
			}

			if (txtSerialPortID.Text.Trim() == string.Empty)
			{
				MsgBox.Show("시리얼포트 아이디를 입력해 주세요.", MsgType.Warning);
				txtSerialPortID.Focus();
				return false;
			}

			if (txtSerialPortName.Text.Trim() == string.Empty)
			{
				MsgBox.Show("시리얼포트 명을 입력해 주세요.", MsgType.Warning);
				txtSerialPortName.Focus();
				return false;
			}

			return true;
		}

		private bool SaveData(DataProcType pProcType)
		{
			string sql = string.Empty;
			int index = -1;

			try
			{
				switch (pProcType)
				{
					case DataProcType.Insert:

						sql = "  INSERT INTO ST_SERIALPORT ("
							+ "         SERIALPORT_ID, SERIALPORT_NAME, SERIALPORT_COMMENT, "
							+ "         PORTNAME, BAUDRATE, DATABITS, PARITY, STOPBITS,"
							+ "         HANDSHAKE, USE_YN"
							+ "         )"
							+ "  VALUES ("
							+ "         '" + txtSerialPortID.Text + "',"
							+ "         '" + txtSerialPortName.Text + "',"
							+ "         '" + txtSerialPortComment.Text + "',"
							+ "         '" + txtPortName.Code + "',"
							+ "         '" + cboBaudRate.Text + "',"
							+ "         '" + cboDataBits.Text + "',"
							+ "         '" + cboParity.Text + "',"
							+ "         '" + cboStopBits.Text + "',"
							+ "         '" + cboHandshake.Text + "',"
							+ "         '" + (chkUseYN.Checked ? "Y" : "N") + "'"
							+ "       )";

						mConfig.Execute(sql);

						sql = "  SELECT *"
							+ "    FROM ST_SERIALPORT"
							+ "   WHERE SERIALPORT_ID = '" + txtSerialPortID.Text + "'";

						index = grdList.ImportR(mConfig, sql, -1);
						grdList.SelectRow(index);
						break;

					case DataProcType.Update:

						string extras = string.Empty;

						sql = "  UPDATE ST_SERIALPORT SET"
							+ "         SERIALPORT_NAME = '" + txtSerialPortName.Text + "',"
							+ "         SERIALPORT_COMMENT = '" + txtSerialPortComment.Text + "',"
							+ "         PORTNAME = '" + txtPortName.Code + "',"
							+ "         BAUDRATE = '" + cboBaudRate.Text + "',"
							+ "         DATABITS = '" + cboDataBits.Text + "',"
							+ "         PARITY = '" + cboParity.Text + "',"
							+ "         STOPBITS = '" + cboStopBits.Text + "',"
							+ "         HANDSHAKE = '" + cboHandshake.Text + "',"
							+ "         USE_YN = '" + (chkUseYN.Checked ? "Y" : "N") + "'"
							+ "   WHERE SERIALPORT_ID = '" + grdList.StoredRowData["SERIALPORT_ID"] + "'";

						mConfig.Execute(sql);

						sql = "  SELECT *"
							+ "    FROM ST_SERIALPORT"
							+ "   WHERE SERIALPORT_ID = '" + grdList.StoredRowData["SERIALPORT_ID"] + "'";

						index = grdList.ImportR(mConfig, sql, grdList.StoredRowIndex);
						grdList.SelectRow(index);
						break;

					case DataProcType.Delete:
						sql = "  DELETE FROM ST_SERIALPORT"
							+ "   WHERE SERIALPORT_ID = '" + grdList.StoredRowData["SERIALPORT_ID"] + "'";

						mConfig.Execute(sql);

						grdList.RemoveR(grdList.StoredRowIndex);
						break;
				}
				return true;
			}
			catch (Exception ex)
			{
				MsgBox.Show("반영중 오류가 발생하였습니다.\n\n(" + ex.Message + ")", MsgType.Error);
				return false;
			}
		}
	}
}
