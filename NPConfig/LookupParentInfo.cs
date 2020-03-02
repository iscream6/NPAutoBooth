/*
 * ==============================================================================
 *   Program ID     : 
 *   Program Name   : 
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : 
 *   Developer      :
 *   Create Date    : 
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
using FadeFox.Database.SQLite;

namespace NPConfig
{
	public partial class LookupParentInfo : DialogForm
	{
		DataProcType mProcType = DataProcType.None;
		SQLite mConfig = new SQLite();
		string mConfigFilePath = "";

		bool mChanged = false;

		public LookupParentInfo(string pConfigFilePath)
		{
			InitializeComponent();
			mConfigFilePath = pConfigFilePath;
		}

		private void LookupParentInfo_Load(object sender, EventArgs e)
		{
		}

		private void LookupParentInfo_Shown(object sender, EventArgs e)
		{
			mConfig.Database = mConfigFilePath;
			mConfig.Connect();

			Initialize();

			string sql = "";

			sql = "  SELECT LOOKUP_ID, LOOKUP_NAME, LOOKUP_EXTRA, "
				+ "         LOOKUP_COMMENT, UPDATE_DATE "
				+ "    FROM ST_LOOKUP"
				+ "   WHERE PARENT_LOOKUP_ID = '" + mConfig.EmptyString + "'"
				+ "       AND LOOKUP_ID NOT IN ('ADDRESS', 'PHONE', 'MESSAGE', 'SERVER', 'DATABASE', 'SYSTEM')"
				+ "   ORDER BY ORDER_BY";

			grdList.ImportT(mConfig, sql);
		}

		private void LookupParentInfo_FormClosing(object sender, FormClosingEventArgs e)
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

			if (mChanged)
				this.DialogResult = DialogResult.Yes;
			else
				this.DialogResult = DialogResult.No;
		}

		private void LookupParentInfo_FormClosed(object sender, FormClosedEventArgs e)
		{
		}

		private void Initialize()
		{
			this.MenuName = "구분관리";

			grdList.RowSelected = RowSelected;
			grdList.RowsAdding = RowsAdding;

			ActionScreen(DataProcType.None);
			this.DialogResult = DialogResult.None;

			chkContinueInsertMode.Visible = false;
			btnSave.Visible = false;
			btnCancel.Visible = false;
		}

		private void RowSelected()
		{
			if (mProcType == DataProcType.None)
			{
				int selectedIndex = grdList.SelectedRows[0].Index;
				grdList.StoreRowData(selectedIndex);

				txtLookupID.Text = grdList.StoredRowData["LOOKUP_ID"];
				txtLookupName.Text = grdList.StoredRowData["LOOKUP_NAME"];
				txtLookupExtra.Text = grdList.StoredRowData["LOOKUP_EXTRA"];
				txtLookupComment.Text = grdList.StoredRowData["LOOKUP_COMMENT"];

				if (grdList.Rows.Count > 1)
				{
					if (selectedIndex == 0)
					{
						btnUp.Enabled = false;
						btnDown.Enabled = true;
					}
					else if (selectedIndex == grdList.Rows.Count - 1)
					{
						btnUp.Enabled = true;
						btnDown.Enabled = false;
					}
					else
					{
						btnUp.Enabled = true;
						btnDown.Enabled = true;
					}
				}
				else
				{
					btnUp.Enabled = false;
					btnDown.Enabled = false;
				}
			}
		}

		private void RowsAdding(bool pRun)
		{
			pnlInput.Enabled = false;

			if (pRun)
			{
				this.Cursor = Cursors.WaitCursor;

				lblRowCount.Text = "_";

				pnlControl.Enabled = false;
			}
			else
			{
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

					btnUp.Enabled = false;
					btnDown.Enabled = false;
				}

				pnlControl.Enabled = true;

				lblRowCount.Text = TextCore.ToCommaString(grdList.Rows.Count);

				this.Cursor = Cursors.Default;
			}
		}

		private void ClearInputScreen()
		{
			txtLookupID.Text = "";
			txtLookupName.Text = "";
			txtLookupExtra.Text = "";
			txtLookupComment.Text = "";
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			try
			{
				mConfig.BeginTrans();

				string sql = "";

				sql = "  SELECT ORDER_BY"
					+ "    FROM ST_LOOKUP"
					+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

				string orderBy = mConfig.SelectC(sql);

				sql = "  UPDATE ST_LOOKUP SET"
					+ "         ORDER_BY = " + orderBy + ","
					+ "         UPDATE_USER_ID = '" + "_X_" + "',"
					+ "         UPDATE_DATE = GETDATE()"
					+ "   WHERE PARENT_LOOKUP_ID = '" + mConfig.EmptyString + "'"
					+ "         AND ORDER_BY = " + orderBy + " - 1";

				mConfig.Execute(sql);

				sql = "  UPDATE ST_LOOKUP SET"
					+ "         ORDER_BY = ORDER_BY - 1,"
					+ "         UPDATE_USER_ID = '" + "_X_" + "',"
					+ "         UPDATE_DATE = GETDATE()"
					+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

				mConfig.Execute(sql);

				mConfig.CommitTrans();

				mChanged = true;
			}
			catch
			{
				mConfig.RollbackTrans();
			}

			grdList.MoveRow(grdList.CurrentRow.Index, -1);
		}

		private void btnDown_Click(object sender, EventArgs e)
		{
			try
			{
				mConfig.BeginTrans();

				string sql = "";

				sql = "  SELECT ORDER_BY"
					+ "    FROM ST_LOOKUP"
					+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

				string orderBy = mConfig.SelectC(sql);

				sql = "  UPDATE ST_LOOKUP SET"
					+ "         ORDER_BY = " + orderBy + ","
					+ "         UPDATE_USER_ID = '" + "_X_" + "',"
					+ "         UPDATE_DATE = GETDATE()"
					+ "   WHERE PARENT_LOOKUP_ID = '" + mConfig.EmptyString + "'"
					+ "         AND ORDER_BY = " + orderBy + " + 1";

				mConfig.Execute(sql);

				sql = "  UPDATE ST_LOOKUP SET"
					+ "         ORDER_BY = ORDER_BY + 1,"
					+ "         UPDATE_USER_ID = '" + "_X_" + "',"
					+ "         UPDATE_DATE = GETDATE()"
					+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

				mConfig.Execute(sql);

				mConfig.CommitTrans();

				mChanged = true;
			}
			catch
			{
				mConfig.RollbackTrans();
			}

			grdList.MoveRow(grdList.CurrentRow.Index, 1);
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

		private void btnClose_Click(object sender, EventArgs e)
		{
			CloseEnabled = true;
			this.Close();
		}

		private void ActionScreen(DataProcType pProcType)
		{
			mProcType = pProcType;

			switch (pProcType)
			{
				case DataProcType.Insert:
					lblSubject.Text = this.MenuName + " 추가";
					pnlInput.Enabled = true;
					txtLookupID.Enabled = true;
					txtLookupID.Focus();

					ClearInputScreen();

					chkContinueInsertMode.Visible = true;
					btnSave.Visible = true;
					btnCancel.Visible = true;

					btnUp.Visible = false;
					btnDown.Visible = false;
					btnInsert.Visible = false;
					btnUpdate.Visible = false;
					btnDelete.Visible = false;
					btnClose.Visible = false;

					break;

				case DataProcType.Update:
					lblSubject.Text = this.MenuName + " 수정";
					txtLookupID.Enabled = false;
					pnlInput.Enabled = true;
					txtLookupName.Focus();

					chkContinueInsertMode.Visible = false;
					btnSave.Visible = true;
					btnCancel.Visible = true;

					btnUp.Visible = false;
					btnDown.Visible = false;
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
						RowSelected();
					}
					else
					{
						btnUpdate.Enabled = false;
						btnDelete.Enabled = false;
						ClearInputScreen();

						btnUp.Enabled = false;
						btnDown.Enabled = false;
					}

					chkContinueInsertMode.Visible = false;
					btnSave.Visible = false;
					btnCancel.Visible = false;

					btnUp.Visible = true;
					btnDown.Visible = true;
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

			if (txtLookupID.Text.Trim() == string.Empty)
			{
				MsgBox.Show("코드를 입력해 주세요.", MsgType.Warning);
				txtLookupID.Focus();
				return false;
			}

			
			if (txtLookupName.Text.Trim() == string.Empty)
			{
				MsgBox.Show("이름을 입력해 주세요.", MsgType.Warning);
				txtLookupName.Focus();
				return false;
			}
			

			switch (pProcType)
			{
				case DataProcType.Insert:
					sql = "  SELECT COUNT(*)"
						+ "    FROM ST_LOOKUP"
						+ "   WHERE LOOKUP_ID = '" + txtLookupID.Text + "'";

					string count = mConfig.SelectC(sql);

					if (count != "0")
					{
						MsgBox.Show(txtLookupID.Text + " 는 사용할 수 없는 코드입니다.", MsgType.Warning);
						txtLookupID.Focus();
						return false;
					}
					break;

				case DataProcType.Update:
					sql = "  SELECT UPDATE_USER_ID, UPDATE_DATE"
						+ "    FROM ST_LOOKUP"
						+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

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
							+ "다시 검색한 후 수정해 주세요.";

						MsgBox.Show(msg, MsgType.Warning);
						return false;
					}

					break;

				case DataProcType.Delete:
					return true;
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

						sql = "  SELECT COALESCE(MAX(ORDER_BY), 0) + 1 AS NEXT_ORDER_BY"
							+ "    FROM ST_LOOKUP"
							+ "   WHERE PARENT_LOOKUP_ID = '" + mConfig.EmptyString + "'";

						string nextOrderBy = mConfig.SelectC(sql);

						if (nextOrderBy == string.Empty)
							nextOrderBy = "1";

						sql = "  INSERT INTO ST_LOOKUP ("
							+ "         LOOKUP_ID, PARENT_LOOKUP_ID, LOOKUP_NAME, LOOKUP_EXTRA, LOOKUP_COMMENT, ORDER_BY,"
							+ "         INSERT_USER_ID, INSERT_DATE, UPDATE_USER_ID, UPDATE_DATE"
							+ "         )"
							+ "  VALUES ("
							+ "         '" + TextCore.ConvertChar(txtLookupID.Text) + "',"
							+ "         '" + mConfig.EmptyString + "',"
							+ "         '" + TextCore.ConvertChar(txtLookupName.Text) + "',"
							+ "         '" + TextCore.ConvertChar(txtLookupExtra.Text) + "',"
							+ "         '" + TextCore.ConvertChar(txtLookupComment.Text) + "',"
							+ "         '" + nextOrderBy + "',"
							+ "         '" + TextCore.ConvertChar("_X_") + "',"
							+ "         GETDATE(),"
							+ "         '" + TextCore.ConvertChar("_X_") + "',"
							+ "         GETDATE()"
							+ "         )";

						mConfig.Execute(sql);

						sql = "  SELECT *"
							+ "    FROM ST_LOOKUP"
							+ "   WHERE LOOKUP_ID = '" + TextCore.ConvertChar(txtLookupID.Text) + "'";

						index = grdList.ImportR(mConfig, sql, -1);
						grdList.SelectRow(index);
						break;

					case DataProcType.Update:

						string extras = string.Empty;

						sql = "  UPDATE ST_LOOKUP SET"
							+ "         LOOKUP_NAME = '" + TextCore.ConvertChar(txtLookupName.Text) + "',"
							+ "         LOOKUP_EXTRA = '" + TextCore.ConvertChar(txtLookupExtra.Text) + "',"
							+ "         LOOKUP_COMMENT = '" + TextCore.ConvertChar(txtLookupComment.Text) + "',"
							+ "         UPDATE_USER_ID = '" + TextCore.ConvertChar("_X_") + "',"
							+ "         UPDATE_DATE = GETDATE()"
							+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

						mConfig.Execute(sql);

						sql = "  SELECT *"
							+ "    FROM ST_LOOKUP"
							+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

						index = grdList.ImportR(mConfig, sql, grdList.StoredRowIndex);
						grdList.SelectRow(index);
						break;

					case DataProcType.Delete:

						try
						{
							mConfig.BeginTrans();

							sql = "  SELECT ORDER_BY"
								+ "    FROM ST_LOOKUP"
								+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

							string orderBy = mConfig.SelectC(sql);

							if (orderBy == "")
								orderBy = "1";

							sql = "  UPDATE ST_LOOKUP SET"
								+ "         ORDER_BY = ORDER_BY - 1"
								+ "   WHERE PARENT_LOOKUP_ID = '" + mConfig.EmptyString + "'"
								+ "         AND ORDER_BY > " + orderBy;

							mConfig.Execute(sql);

							sql = "  DELETE FROM ST_LOOKUP"
								+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

							mConfig.Execute(sql);

							sql = "  DELETE FROM ST_LOOKUP"
								+ "   WHERE PARENT_LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

							mConfig.Execute(sql);

							mConfig.CommitTrans();
						}
						catch
						{
							mConfig.RollbackTrans();
						}

						grdList.RemoveR(grdList.StoredRowIndex);
						break;
				}

				mChanged = true;

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
