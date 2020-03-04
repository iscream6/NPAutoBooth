/*
 * ==============================================================================
 *   Program ID     :
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   :
 *   Developer      :
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
using FadeFox.Utility;
using FadeFox.Text;
using Sayou.Core;
using FadeFox.Security;
using FadeFox.Database.SQLite;

namespace NPConfig
{
	public partial class ServerInfo : Form//DialogForm
	{
		DataProcType mProcType = DataProcType.None;
		Rijndael mSecurity = new Rijndael();
		SQLite mConfig = new SQLite();
		string mConfigFilePath = "";

		public ServerInfo(string pConfigFilePath)
		{
			InitializeComponent();
			mConfigFilePath = pConfigFilePath;
		}

		private void ServerInfo_Load(object sender, EventArgs e)
		{
		}

		private void ServerInfo_Shown(object sender, EventArgs e)
		{
			mConfig.Database = mConfigFilePath;
			mConfig.Connect();

			Initialize();

			btnSearch.PerformClick();

			txtSearchName.Focus();
		}

		private void ServerInfo_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (grdList.IsBusy)
			{
				MsgBox.Show("작업중인 내용이 있습니다.");
				e.Cancel = true;
			}
		}

		private void ServerInfo_FormClosed(object sender, FormClosedEventArgs e)
		{
			mConfig.Disconnect();
		}

		private void Initialize()
		{
			//this.MenuName = "서버 정보";
			//this.InsertAuth = true;
			//this.UpdateAuth = true;
			//this.DeleteAuth = true;

			SYS.SetLookupInfo(mConfig, cboSearchServerLID, "SERVER", true);

			SYS.SetLookupInfo(mConfig, cboServerLID, "SERVER", false);
			SYS.SetLookupInfo(mConfig, cboDatabaseLID, "DATABASE", false);

			grdList.RowSelected = SelectRowScreen;
			grdList.RowsPreAdding = ImportBeforeRowAdd;
			grdList.RowsAdding = ImportRunScreen;

			// Initialize Screen
			grdList.Rows.Clear();

			chkContinueInsertMode.Visible = false;
			btnSave.Visible = false;
			btnCancel.Visible = false;

			btnInsert.Visible = true; //nsertAuth;
			btnUpdate.Visible = true; //pdateAuth;
			btnDelete.Visible = true; //DeleteAuth;

			pnlControl.Enabled = true;
			btnUpdate.Enabled = false;
			btnDelete.Enabled = false;

			lblSubject.Text = "서버 정보"; //this.MenuName;

			ClearInputScreen();
			pnlInput.Enabled = false;
		}

		private void SelectRowScreen()
		{
			if (mProcType == DataProcType.None)
			{
				grdList.StoreRowData(grdList.SelectedRows[0].Index);

				txtServerID.Text = grdList.StoredRowData["SERVER_ID"];
				txtServerName.Text = grdList.StoredRowData["SERVER_NAME"];
				SYS.SelectLookupInfo(cboServerLID, grdList.StoredRowData["SERVER_LID"]);
				SYS.SelectLookupInfo(cboDatabaseLID, grdList.StoredRowData["DATABASE_LID"]);
				txtServerAddress.Text = grdList.StoredRowData["SERVER_ADDRESS"];
				txtServerPort.Text = grdList.StoredRowData["SERVER_PORT"];
				txtServerDatabase.Text = grdList.StoredRowData["SERVER_DATABASE"];

				chkWindowsAuth.Checked = (grdList.StoredRowData["WINDOWS_AUTH_YN"] == "Y" ? true : false);
				if (chkWindowsAuth.Checked)
				{
					txtServerUserID.Text = "";
					txtServerUserID.Enabled = false;
					txtServerUserPassword.Text = "";
					txtServerUserPassword.Enabled = false;
				}
				else
				{
					txtServerUserID.Text = grdList.StoredRowData["SERVER_USER_ID"];
					txtServerUserID.Enabled = true;
					txtServerUserPassword.Text = grdList.StoredRowData["SERVER_USER_PASSWORD"];
					txtServerUserPassword.Enabled = true;
				}
				
				txtServerComment.Text = grdList.StoredRowData["SERVER_COMMENT"];
			}
		}

		private void ImportBeforeRowAdd(DataTable pDT)
		{
			foreach (DataRow r in pDT.Rows)
			{
				//r["SERVER_ID"] = r["SERVER_ID"].ToString().Replace(mPrefixID + ".", "");
				r["SERVER_USER_PASSWORD"] = mSecurity.Decode(r["SERVER_USER_PASSWORD"].ToString());
			}
		}

		private void ImportRunScreen(bool pRun)
		{
			pnlInput.Enabled = false;

			if (pRun)
			{
				this.Cursor = Cursors.WaitCursor;

				pnlCondition.Enabled = false;
				pnlControl.Enabled = false;

				lblRowCount.Text = "_";
			}
			else
			{
				pnlCondition.Enabled = true;

				if (grdList.Rows.Count > 0)
				{
					btnUpdate.Enabled = true;
					btnDelete.Enabled = true;

					grdList.SelectRow(0);
				}
				else
				{
					btnUpdate.Enabled = false;
					btnDelete.Enabled = false;

					ClearInputScreen();
				}
				pnlControl.Enabled = true;

				lblRowCount.Text = TextCore.ToCommaString(grdList.Rows.Count);

				this.Cursor = Cursors.Default;
			}
		}

		private void ClearInputScreen()
		{
			txtServerID.Text = "";
			txtServerName.Text = "";
			txtServerAddress.Text = "";
			txtServerPort.Text = "";
			txtServerDatabase.Text = "";
			chkWindowsAuth.Checked = false;
			txtServerUserID.Text = "";
			txtServerUserPassword.Text = "";
			txtServerComment.Text = "";

			SYS.SelectLookupInfo(cboServerLID, "SERVER.DATABASE");
			SYS.SelectLookupInfo(cboDatabaseLID, "DATABASE.MSSQL");
		}

		private void txtSearchName_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				e.Handled = true;
				btnSearch.PerformClick();
			}
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			if (grdList.IsBusy)
			{
				MsgBox.Show("현재 작업중인 내용이 있습니다.", MsgType.Warning);
				return;
			}

			string where = "";

			if (SYS.GetLookupInfo(cboSearchServerLID).LookupID != "")
				where += "  AND SERVER_LID = '" + SYS.GetLookupInfo(cboSearchServerLID).LookupID + "'";

			if (txtSearchName.Text.Trim() != "")
				where += "  AND SERVER_NAME LIKE '%" + txtSearchName.Text + "%'";
			
			string sql = "";
			sql = "  SELECT *,"
				+ "         COALESCE((SELECT LOOKUP_NAME FROM ST_LOOKUP WHERE LOOKUP_ID = ST_SERVER.SERVER_LID), '') AS SERVER_LID_NAME,"
				+ "         COALESCE((SELECT LOOKUP_NAME FROM ST_LOOKUP WHERE LOOKUP_ID = ST_SERVER.DATABASE_LID), '') AS DATABASE_LID_NAME"
				+ "    FROM ST_SERVER"
				+ "   WHERE SERVER_ID <> '" + mConfig.EmptyString + "'" + where
				+ "   ORDER BY SERVER_NAME";

			grdList.ImportT(mConfig, sql);
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			//CloseEnabled = true;
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
					lblSubject.Text = /*this.MenuName*/ "서버 정보" + " 추가";
					pnlCondition.Enabled = false;

					txtServerID.Enabled = true;
					pnlInput.Enabled = true;

					ClearInputScreen();

					txtServerID.Focus();

					chkContinueInsertMode.Visible = true;
					btnSave.Visible = true;
					btnCancel.Visible = true;

					btnInsert.Visible = false;
					btnUpdate.Visible = false;
					btnDelete.Visible = false;
					btnClose.Visible = false;

					break;

				case DataProcType.Update:
					lblSubject.Text = /*this.MenuName*/ "서버 정보" + " 수정";
					pnlCondition.Enabled = false;

					txtServerID.Enabled = false;
					pnlInput.Enabled = true;

					txtServerName.Focus();

					chkContinueInsertMode.Visible = false;
					btnSave.Visible = true;
					btnCancel.Visible = true;

					btnInsert.Visible = false;
					btnUpdate.Visible = false;
					btnDelete.Visible = false;
					btnClose.Visible = false;

					break;

				case DataProcType.Delete:
					lblSubject.Text = /*this.MenuName*/ "서버 정보" + " 삭제";
					pnlCondition.Enabled = false;

					pnlInput.Enabled = false;

					pnlControl.Enabled = false;

					break;

				case DataProcType.None:
					lblSubject.Text = /*this.MenuName*/ "서버 정보";
					pnlCondition.Enabled = true;

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

					btnInsert.Visible = true; //InsertAuth;
					btnUpdate.Visible = true; //UpdateAuth;
					btnDelete.Visible = true; //DeleteAuth;
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
						+ "    FROM ST_SERVER"
						+ "   WHERE SERVER_ID = '" + txtServerID.Text + "'";

					string count = mConfig.SelectC(sql);

					if (count != "0")
					{
						MsgBox.Show(txtServerID.Text + " 는 사용할 수 없는 코드입니다.", MsgType.Warning);
						txtServerID.Focus();
						return false;
					}
					break;

				case DataProcType.Update:
					sql = "  SELECT UPDATE_USER_ID, UPDATE_DATE"
						+ "    FROM ST_SERVER"
						+ "   WHERE SERVER_ID = '" + grdList.StoredRowData["SERVER_ID"] + "'";

					DataRow dr = mConfig.SelectR(sql);

					if (dr == null)
					{
						MsgBox.Show("존재하지 않는 데이터입니다.", MsgType.Warning);
						return false;
					}

					if (grdList.StoredRowData["UPDATE_DATE"] != dr["UPDATE_DATE"].ToString())
					{
						msg = "사용자 [" + dr["UPDATE_USER_ID"].ToString() + "]에 의해\n"
							+ "[" + dr["UPDATE_DATE"].ToString() + "]에 변경된 데이터입니다.\n\n"
							+ "다시 검색해서 내용을 확인한 후 수정해 주세요.";

						MsgBox.Show(msg, MsgType.Warning);
						return false;
					}
					break;

				case DataProcType.Delete:
					return true;
			}

			if (txtServerID.Text.Trim() == string.Empty)
			{
				MsgBox.Show("서버 아이디를 입력해 주세요.", MsgType.Warning);
				txtServerID.Focus();
				return false;
			}

			if (txtServerName.Text.Trim() == string.Empty)
			{
				MsgBox.Show("서버 명을 입력해 주세요.", MsgType.Warning);
				txtServerName.Focus();
				return false;
			}

			switch (SYS.GetLookupInfo(cboDatabaseLID).LookupID)
			{
				case "DATABASE.MSSQL":
					if (txtServerAddress.Text.Trim() == string.Empty)
					{
						MsgBox.Show("서버 주소를 입력해 주세요.", MsgType.Warning);
						txtServerAddress.Focus();
						return false;
					}

					if (txtServerPort.Text.Trim() == string.Empty)
					{
						MsgBox.Show("포트를 입력해 주세요.", MsgType.Warning);
						txtServerPort.Focus();
						return false;
					}

					if (!chkWindowsAuth.Checked)
					{
						if (txtServerUserID.Text.Trim() == string.Empty)
						{
							MsgBox.Show("사용자 아이디를 입력해 주세요.", MsgType.Warning);
							txtServerUserID.Focus();
							return false;
						}

						if (txtServerUserPassword.Text.Trim() == string.Empty)
						{
							MsgBox.Show("사용자 암호를 입력해 주세요.", MsgType.Warning);
							txtServerUserPassword.Focus();
							return false;
						}
					}

					if (txtServerDatabase.Text.Trim() == string.Empty)
					{
						MsgBox.Show("데이터베이스 명을 입력해 주세요.", MsgType.Warning);
						txtServerDatabase.Focus();
						return false;
					}
					break;
				case "DATABASE.ORACLE":
					if (txtServerAddress.Text.Trim() == string.Empty)
					{
						MsgBox.Show("서버 주소를 입력해 주세요.", MsgType.Warning);
						txtServerAddress.Focus();
						return false;
					}

					if (txtServerPort.Text.Trim() == string.Empty)
					{
						MsgBox.Show("포트를 입력해 주세요.", MsgType.Warning);
						txtServerPort.Focus();
						return false;
					}

					if (chkWindowsAuth.Checked)
					{
						if (txtServerUserID.Text.Trim() == string.Empty)
						{
							MsgBox.Show("사용자 아이디를 입력해 주세요.", MsgType.Warning);
							txtServerUserID.Focus();
							return false;
						}

						if (txtServerUserPassword.Text.Trim() == string.Empty)
						{
							MsgBox.Show("사용자 암호를 입력해 주세요.", MsgType.Warning);
							txtServerUserPassword.Focus();
							return false;
						}
					}

					if (txtServerDatabase.Text.Trim() == string.Empty)
					{
						MsgBox.Show("데이터베이스 명을 입력해 주세요.", MsgType.Warning);
						txtServerDatabase.Focus();
						return false;
					}
					break;

				case "DATABASE.ACCESS":
					if (txtServerDatabase.Text.Trim() == string.Empty)
					{
						MsgBox.Show("데이터베이스 명을 입력해 주세요.", MsgType.Warning);
						txtServerDatabase.Focus();
						return false;
					}
					break;

				case "DATABASE.SQLITE":
					if (txtServerDatabase.Text.Trim() == string.Empty)
					{
						MsgBox.Show("데이터베이스 명을 입력해 주세요.", MsgType.Warning);
						txtServerDatabase.Focus();
						return false;
					}

					break;

				default:
					if (txtServerAddress.Text.Trim() == string.Empty)
					{
						MsgBox.Show("서버 주소를 입력해 주세요.", MsgType.Warning);
						txtServerAddress.Focus();
						return false;
					}

					break;
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

						sql = "  INSERT INTO ST_SERVER ("
							+ "         SERVER_ID, SERVER_NAME, SERVER_COMMENT, SERVER_LID,"
							+ "         SERVER_ADDRESS, SERVER_USER_ID, SERVER_USER_PASSWORD, SERVER_DATABASE, DATABASE_LID,"
							+ "         SERVER_PORT, WINDOWS_AUTH_YN, INSERT_USER_ID, INSERT_DATE, UPDATE_USER_ID, UPDATE_DATE"
							+ "         )"
							+ "  VALUES ("
							+ "         '" + txtServerID.Text + "',"
							+ "         '" + txtServerName.Text + "',"
							+ "         '" + txtServerComment.Text + "',"
							+ "         '" + SYS.GetLookupInfo(cboServerLID).LookupID + "',"
							+ "         '" + txtServerAddress.Text + "',"
							+ "         '" + txtServerUserID.Text + "',"
							+ "         '" + mSecurity.Encode(txtServerUserPassword.Text) + "',"
							+ "         '" + txtServerDatabase.Text + "',"
							+ "         '" + SYS.GetLookupInfo(cboDatabaseLID).LookupID + "',"
							+ "         '" + txtServerPort.Text + "',"
							+ "         '" + (chkWindowsAuth.Checked ? "Y" : "N") + "',"
							+ "         '" + SYS.UserID + "',"
							+ "         GETDATE(),"
							+ "         '" + SYS.UserID + "',"
							+ "         GETDATE()"
							+ "       )";

						mConfig.Execute(sql);

						sql = "  SELECT *,"
							+ "         COALESCE((SELECT LOOKUP_NAME FROM ST_LOOKUP WHERE LOOKUP_ID = ST_SERVER.SERVER_LID), '') AS SERVER_LID_NAME,"
							+ "         COALESCE((SELECT LOOKUP_NAME FROM ST_LOOKUP WHERE LOOKUP_ID = ST_SERVER.DATABASE_LID), '') AS DATABASE_LID_NAME"
							+ "    FROM ST_SERVER"
							+ "   WHERE SERVER_ID = '" + txtServerID.Text + "'";

						index = grdList.ImportR(mConfig, sql, -1);
						grdList.SelectRow(index);
						break;

					case DataProcType.Update:

						string extras = string.Empty;

						sql = "  UPDATE ST_SERVER SET"
							+ "         SERVER_NAME = '" + txtServerName.Text + "',"
							+ "         SERVER_COMMENT = '" + txtServerComment.Text + "',"
							+ "         SERVER_LID = '" + SYS.GetLookupInfo(cboServerLID).LookupID + "',"
							+ "         SERVER_ADDRESS = '" + txtServerAddress.Text + "',"
							+ "         SERVER_USER_ID = '" + txtServerUserID.Text + "',"
							+ "         SERVER_USER_PASSWORD = '" + mSecurity.Encode(txtServerUserPassword.Text) + "',"
							+ "         SERVER_DATABASE = '" + txtServerDatabase.Text + "',"
							+ "         SERVER_PORT = '" + txtServerPort.Text + "',"
							+ "         DATABASE_LID = '" + SYS.GetLookupInfo(cboDatabaseLID).LookupID + "',"
							+ "         WINDOWS_AUTH_YN = '" + (chkWindowsAuth.Checked ? "Y" : "N") + "',"
							+ "         UPDATE_USER_ID = '" + SYS.UserID + "',"
							+ "         UPDATE_DATE = GETDATE()"
							+ "   WHERE SERVER_ID = '" + grdList.StoredRowData["SERVER_ID"] + "'";

						mConfig.Execute(sql);

						sql = "  SELECT *,"
							+ "         COALESCE((SELECT LOOKUP_NAME FROM ST_LOOKUP WHERE LOOKUP_ID = ST_SERVER.SERVER_LID), '') AS SERVER_LID_NAME,"
							+ "         COALESCE((SELECT LOOKUP_NAME FROM ST_LOOKUP WHERE LOOKUP_ID = ST_SERVER.DATABASE_LID), '') AS DATABASE_LID_NAME"
							+ "    FROM ST_SERVER"
							+ "   WHERE SERVER_ID = '" + grdList.StoredRowData["SERVER_ID"] + "'";

						index = grdList.ImportR(mConfig, sql, grdList.StoredRowIndex);
						grdList.SelectRow(index);
						break;

					case DataProcType.Delete:
						sql = "  DELETE FROM ST_SERVER"
							+ "   WHERE SERVER_ID = '" + grdList.StoredRowData["SERVER_ID"] + "'";

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

		private void cboServerLID_SelectedIndexChanged(object sender, EventArgs e)
		{
			string serverLID = SYS.GetLookupInfo(cboServerLID).LookupID;

			switch (serverLID)
			{
				case "SERVER.DATABASE":
					cboDatabaseLID.Enabled = true;
					SYS.SelectLookupInfo(cboDatabaseLID, "DATABASE.MSSQL");

					txtServerAddress.Text = "";
					txtServerAddress.Enabled = true;
					txtServerPort.Text = "1433";
					txtServerPort.Enabled = true;
					txtServerDatabase.Text = "";
					txtServerDatabase.Enabled = true;
					btnFindFile.Enabled = false;
					chkWindowsAuth.Checked = false;
					chkWindowsAuth.Enabled = true;
					txtServerUserID.Text = "";
					txtServerUserID.Enabled = true;
					txtServerUserPassword.Text = "";
					txtServerUserPassword.Enabled = true;
					break;
				case "SERVER.FTP":
					cboDatabaseLID.Enabled = false;
					SYS.SelectLookupInfo(cboDatabaseLID, "DATABASE.NONE");

					txtServerAddress.Text = "";
					txtServerAddress.Enabled = true;
					txtServerPort.Text = "21";
					txtServerPort.Enabled = true;
					txtServerDatabase.Text = "";
					txtServerDatabase.Enabled = false;
					btnFindFile.Enabled = false;
					chkWindowsAuth.Checked = false;
					chkWindowsAuth.Enabled = false;
					txtServerUserID.Text = "";
					txtServerUserID.Enabled = true;
					txtServerUserPassword.Text = "";
					txtServerUserPassword.Enabled = true;

					break;
				case "SERVER.HTTP":
					cboDatabaseLID.Enabled = false;
					SYS.SelectLookupInfo(cboDatabaseLID, "DATABASE.NONE");

					txtServerAddress.Text = "";
					txtServerAddress.Enabled = true;
					txtServerPort.Text = "80";
					txtServerPort.Enabled = true;
					txtServerDatabase.Text = "";
					txtServerDatabase.Enabled = false;
					btnFindFile.Enabled = false;
					chkWindowsAuth.Checked = false;
					chkWindowsAuth.Enabled = false;
					txtServerUserID.Text = "";
					txtServerUserID.Enabled = true;
					txtServerUserPassword.Text = "";
					txtServerUserPassword.Enabled = true;
					break;
				case "SERVER.TCPIP":
					cboDatabaseLID.Enabled = false;
					SYS.SelectLookupInfo(cboDatabaseLID, "DATABASE.NONE");

					txtServerAddress.Text = "";
					txtServerAddress.Enabled = true;
					txtServerPort.Text = "";
					txtServerPort.Enabled = true;
					txtServerDatabase.Text = "";
					txtServerDatabase.Enabled = false;
					btnFindFile.Enabled = false;
					chkWindowsAuth.Checked = false;
					chkWindowsAuth.Enabled = false;
					txtServerUserID.Text = "";
					txtServerUserID.Enabled = true;
					txtServerUserPassword.Text = "";
					txtServerUserPassword.Enabled = true;
					break;
			}

			txtServerComment.Text = "";
		}

		private void cboDatabaseKind_SelectedIndexChanged(object sender, EventArgs e)
		{
			string databaseKind = SYS.GetLookupInfo(cboDatabaseLID).LookupID;

			switch (databaseKind)
			{
				case "DATABASE.MSSQL":
					txtServerAddress.Text = "";
					txtServerAddress.Enabled = true;
					txtServerPort.Text = "1433";
					txtServerPort.Enabled = true;
					txtServerDatabase.Text = "";
					txtServerDatabase.Enabled = true;
					btnFindFile.Enabled = false;
					chkWindowsAuth.Checked = false;
					chkWindowsAuth.Enabled = true;
					txtServerUserID.Text = "";
					txtServerUserID.Enabled = true;
					txtServerUserPassword.Text = "";
					txtServerUserPassword.Enabled = true;
					break;
				case "DATABASE.ORACLE":
					txtServerAddress.Text = "";
					txtServerAddress.Enabled = true;
					txtServerPort.Enabled = true;
					txtServerPort.Text = "1521";
					txtServerDatabase.Text = "";
					txtServerDatabase.Enabled = true;
					btnFindFile.Enabled = false;
					chkWindowsAuth.Checked = false;
					chkWindowsAuth.Enabled = false;
					txtServerUserID.Text = "";
					txtServerUserID.Enabled = true;
					txtServerUserPassword.Text = "";
					txtServerUserPassword.Enabled = true;
					break;
				case "DATABASE.ACCESS":
					txtServerAddress.Text = "";
					txtServerAddress.Enabled = false;
					txtServerPort.Text = "";
					txtServerPort.Enabled = false;
					txtServerDatabase.Text = "";
					txtServerDatabase.Enabled = true;
					btnFindFile.Enabled = true;
					ofFindFile.Filter = "mdb files (*.mdb) | *.mdb";
					chkWindowsAuth.Checked = false;
					chkWindowsAuth.Enabled = false;
					txtServerUserID.Text = "";
					txtServerUserID.Enabled = false;
					txtServerUserPassword.Text = "";
					txtServerUserPassword.Enabled = true;
					break;
				case "DATABASE.SQLITE":
					txtServerAddress.Text = "";
					txtServerAddress.Enabled = false;
					txtServerPort.Text = "";
					txtServerPort.Enabled = false;
					txtServerDatabase.Text = "";
					txtServerDatabase.Enabled = true;
					btnFindFile.Enabled = true;
					ofFindFile.Filter = "db3 files (*.db3) | *.db3";
					chkWindowsAuth.Checked = false;
					chkWindowsAuth.Enabled = false;
					txtServerUserID.Text = "";
					txtServerUserID.Enabled = false;
					txtServerUserPassword.Text = "";
					txtServerUserPassword.Enabled = true;
					break;
				default:
					txtServerAddress.Text = "";
					txtServerAddress.Enabled = true;
					txtServerPort.Enabled = true;
					txtServerPort.Text = "";
					txtServerDatabase.Text = "";
					txtServerDatabase.Enabled = false;
					btnFindFile.Enabled = false;
					chkWindowsAuth.Checked = false;
					chkWindowsAuth.Enabled = false;
					txtServerUserID.Text = "";
					txtServerUserID.Enabled = true;
					txtServerUserPassword.Text = "";
					txtServerUserPassword.Enabled = true;
					break;
			}

			txtServerComment.Text = "";
		}

		private void btnFindFile_Click(object sender, EventArgs e)
		{
			if (ofFindFile.ShowDialog() == DialogResult.OK)
			{
				txtServerDatabase.Text = ofFindFile.FileName;
			}
		}

		private void chkWindowsAuth_CheckedChanged(object sender, EventArgs e)
		{
			txtServerUserID.Enabled = !chkWindowsAuth.Checked;
			txtServerUserPassword.Enabled = !chkWindowsAuth.Checked;
		}
	}
}