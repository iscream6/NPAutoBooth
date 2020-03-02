/*
 * ==============================================================================
 *   Program ID     : 
 *   Program Name   : 
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : 
 *   Developer      :
 *   Create Date    : 2011-08-04
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
using FadeFox.Database.SQLite;

namespace NPConfig
{
	public partial class ParkingLotFloor : DialogForm
	{
		DataProcType mProcType = DataProcType.None;
		SQLite mConfig = new SQLite();
		string mConfigFilePath = "";
		string mParkingLotNo = "";

		public ParkingLotFloor(string pConfigFilePath)
		{
			InitializeComponent();
			mConfigFilePath = pConfigFilePath;
		}

		private void ParkingLotFloor_Load(object sender, EventArgs e)
		{
		}

		private void ParkingLotFloor_Shown(object sender, EventArgs e)
		{
			mConfig.Database = mConfigFilePath;
			mConfig.Connect();

			Initialize();
		}

		private void ParkingLotFloor_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (grdList.IsBusy)
			{
				MsgBox.Show("작업중인 내용이 있습니다.");
				e.Cancel = true;
			}
		}

		private void ParkingLotFloor_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (grdList.IsBusy)
				grdList.CancelWork();
		}

		private void Initialize()
		{
			InsertAuth = true;
			UpdateAuth = true;
			DeleteAuth = true;

			grdList.RowSelected = RowSelected;
			grdList.RowsPreAdding = RowsPreAdding;
			grdList.RowsAdding = RowsAdding;

			// Initialize Screen
			grdList.Rows.Clear();

			btnUp.Visible = UpdateAuth;
			btnDown.Visible = UpdateAuth;

			btnInsert.Visible = InsertAuth;
			btnUpdate.Visible = UpdateAuth;
			btnDelete.Visible = DeleteAuth;

			btnInsert.Enabled = true;
			btnUpdate.Enabled = false;
			btnDelete.Enabled = false;

			btnUp.Enabled = false;
			btnDown.Enabled = false;
			
			SYS.SetLookupInfo(mConfig, cboParkingLot, "PARKINGLOT");

			if (cboParkingLot.Items.Count < 1)
			{
				btnInsert.Enabled = false;
				btnSearch.Enabled = false;
			}
			else
			{
				btnInsert.Enabled = true;
				btnSearch.Enabled = true;
			}

			chkContinueInsertMode.Visible = false;
			btnSave.Visible = false;
			btnCancel.Visible = false;

			if (cboParkingLot.Items.Count > 0)
			{
				cboParkingLot.SelectedIndex = 0;
			}
		}

		private void RowSelected()
		{
			if (mProcType == DataProcType.None)
			{
				int selectedIndex = grdList.SelectedRows[0].Index;
				grdList.StoreRowData(selectedIndex);

				txtFloorCode.Text = grdList.StoredRowData["FLOOR_CODE"];
				txtFloorName.Text = grdList.StoredRowData["FLOOR_NAME"];
				txtMapCode.Text = grdList.StoredRowData["MAP_CODE"];

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

		private void RowsPreAdding(DataTable pDT)
		{
		}

		private void RowsAdding(bool pRun)
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

					btnUp.Enabled = false;
					btnDown.Enabled = false;

					ClearInputScreen();
				}

				pnlControl.Enabled = true;

				lblRowCount.Text = TextCore.ToCommaString(grdList.Rows.Count);

				this.Cursor = Cursors.Default;
			}
		}

		private void ClearInputScreen()
		{
			txtFloorCode.Text = "";
			txtFloorName.Text = "";
			txtMapCode.Text = "";
		}

		private void cboKind_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboParkingLot.Items.Count < 1)
				return;

			if (cboParkingLot.SelectedIndex < 0)
				return;

			mParkingLotNo = SYS.GetLookupInfo(cboParkingLot).LookupIDValue;

			btnSearch.PerformClick();
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			if (grdList.IsBusy)
			{
				MsgBox.Show("현재 작업중인 내용이 있습니다.", MsgType.Warning);
				return;
			}

			Search();
		}

		private void Search()
		{
			string sql = string.Empty;
			sql = "  SELECT *"
				+ "    FROM PARKINGLOT_FLOOR"
				+ "   WHERE PARKINGLOT_NO = '" + mParkingLotNo + "'"
				+ "   ORDER BY ORDER_BY";

			grdList.ImportT(mConfig, sql);
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			CloseEnabled = true;
			this.Close();
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			try
			{
				mConfig.BeginTrans();

				string sql = "";

				sql = "  SELECT ORDER_BY"
					+ "    FROM PARKINGLOT_FLOOR"
					+ "   WHERE PARKINGLOT_NO = '" + grdList.StoredRowData["PARKINGLOT_NO"] + "'"
					+ "         AND FLOOR_CODE = '" + grdList.StoredRowData["FLOOR_CODE"] + "'";

				string orderBy = mConfig.SelectC(sql);

				sql = "  UPDATE PARKINGLOT_FLOOR SET"
					+ "         ORDER_BY = " + orderBy + ","
					+ "         UPDATE_USER_ID = '" + "_X_" + "',"
					+ "         UPDATE_DATE = GETDATE()"
					+ "   WHERE PARKINGLOT_NO = '" + grdList.StoredRowData["PARKINGLOT_NO"] + "'"
					+ "         AND ORDER_BY = " + orderBy + " - 1";

				mConfig.Execute(sql);

				sql = "  UPDATE PARKINGLOT_FLOOR SET"
					+ "         ORDER_BY = ORDER_BY - 1,"
					+ "         UPDATE_USER_ID = '" + "_X_" + "',"
					+ "         UPDATE_DATE = GETDATE()"
					+ "   WHERE PARKINGLOT_NO = '" + grdList.StoredRowData["PARKINGLOT_NO"] + "'"
					+ "         AND FLOOR_CODE = '" + grdList.StoredRowData["FLOOR_CODE"] + "'";

				mConfig.Execute(sql);

				mConfig.CommitTrans();
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
					+ "    FROM PARKINGLOT_FLOOR"
					+ "   WHERE PARKINGLOT_NO = '" + grdList.StoredRowData["PARKINGLOT_NO"] + "'"
					+ "         AND FLOOR_CODE = '" + grdList.StoredRowData["FLOOR_CODE"] + "'";

				string orderBy = mConfig.SelectC(sql);

				sql = "  UPDATE PARKINGLOT_FLOOR SET"
					+ "         ORDER_BY = " + orderBy + ","
					+ "         UPDATE_USER_ID = '" + "_X_" + "',"
					+ "         UPDATE_DATE = GETDATE()"
					+ "   WHERE PARKINGLOT_NO = '" + grdList.StoredRowData["PARKINGLOT_NO"] + "'"
					+ "         AND ORDER_BY = " + orderBy + " + 1";

				mConfig.Execute(sql);

				sql = "  UPDATE PARKINGLOT_FLOOR SET"
					+ "         ORDER_BY = ORDER_BY + 1,"
					+ "         UPDATE_USER_ID = '" + "_X_" + "',"
					+ "         UPDATE_DATE = GETDATE()"
					+ "   WHERE PARKINGLOT_NO = '" + grdList.StoredRowData["PARKINGLOT_NO"] + "'"
					+ "         AND FLOOR_CODE = '" + grdList.StoredRowData["FLOOR_CODE"] + "'";

				mConfig.Execute(sql);

				mConfig.CommitTrans();
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

		private void ActionScreen(DataProcType pProcType)
		{
			mProcType = pProcType;

			switch (pProcType)
			{
				case DataProcType.Insert:
					pnlCondition.Enabled = false;

					pnlInput.Enabled = true;
					txtFloorCode.Enabled = true;
					txtFloorCode.Focus();

					ClearInputScreen();

					chkContinueInsertMode.Visible = true;
					btnSave.Visible = true;
					btnCancel.Visible = true;

					btnUp.Visible = false;
					btnDown.Visible = false;
					btnInsert.Visible = false;
					btnUpdate.Visible = false;
					btnDelete.Visible = false;

					break;

				case DataProcType.Update:
					pnlCondition.Enabled = false;

					pnlInput.Enabled = true;
					txtFloorCode.Enabled = false;
					txtFloorName.Focus();

					chkContinueInsertMode.Visible = false;
					btnSave.Visible = true;
					btnCancel.Visible = true;

					btnUp.Visible = false;
					btnDown.Visible = false;
					btnInsert.Visible = false;
					btnUpdate.Visible = false;
					btnDelete.Visible = false;

					break;

				case DataProcType.Delete:
					pnlCondition.Enabled = false;

					pnlInput.Enabled = false;

					pnlControl.Enabled = false;

					break;

				case DataProcType.None:
					pnlCondition.Enabled = true;

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

					btnUp.Visible = UpdateAuth;
					btnDown.Visible = UpdateAuth;
					btnInsert.Visible = InsertAuth;
					btnUpdate.Visible = UpdateAuth;
					btnDelete.Visible = DeleteAuth;

					pnlControl.Enabled = true;

					break;
			}

			lblRowCount.Text = grdList.Rows.Count.ToString();
		}

		private bool CheckData(DataProcType pProcType)
		{
			string sql = string.Empty;
			string msg = string.Empty;

			if (txtFloorCode.Text.Trim() == string.Empty)
			{
				MsgBox.Show("코드를 입력해 주세요.", MsgType.Warning);
				txtFloorCode.Focus();
				return false;
			}

			/*
			if (txtLookupName.Text.Trim() == string.Empty)
			{
				MsgBox.Show("이름을 입력해 주세요.", MsgType.Warning);
				txtLookupName.Focus();
				return false;
			}
			*/

			switch (pProcType)
			{
				case DataProcType.Insert:
					sql = "  SELECT COUNT(*)"
						+ "    FROM PARKINGLOT_FLOOR"
						+ "   WHERE PARKINGLOT_NO = '" + mParkingLotNo + "'"
						+ "         AND FLOOR_CODE = '" + txtFloorCode.Text + "'";

					string count = mConfig.SelectC(sql);

					if (count != "0")
					{
						MsgBox.Show(txtFloorCode.Text + " 는 사용할 수 없는 코드입니다.", MsgType.Warning);
						txtFloorCode.Focus();
						return false;
					}
					break;

				case DataProcType.Update:
					sql = "  SELECT UPDATE_USER_ID, UPDATE_DATE"
						+ "    FROM PARKINGLOT_FLOOR"
						+ "   WHERE PARKINGLOT_NO = '" + mParkingLotNo + "'"
						+ "         AND FLOOR_CODE = '" + grdList.StoredRowData["FLOOR_CODE"] + "'";

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

			return true;
		}

		private bool SaveData(DataProcType pProcType)
		{
			if (mParkingLotNo == "")
				return false;
			
			string sql = string.Empty;
			int index = -1;

			try
			{
				switch (pProcType)
				{
					case DataProcType.Insert:
						sql = "  SELECT COALESCE(MAX(ORDER_BY), 0) + 1 AS NEXT_ORDER_BY"
							+ "    FROM PARKINGLOT_FLOOR"
							+ "   WHERE PARKINGLOT_NO = '" + mParkingLotNo + "'";

						string nextOrderBy = mConfig.SelectC(sql);

						if (nextOrderBy == string.Empty)
							nextOrderBy = "1";

						sql = "  INSERT INTO PARKINGLOT_FLOOR ("
							+ "         PARKINGLOT_NO, FLOOR_CODE, FLOOR_NAME, MAP_CODE, ORDER_BY,"
							+ "         INSERT_USER_ID, INSERT_DATE, UPDATE_USER_ID, UPDATE_DATE"
							+ "         )"
							+ "  VALUES ("
							+ "         '" + mParkingLotNo + "',"
							+ "         '" + txtFloorCode.Text + "',"
							+ "         '" + txtFloorName.Text + "',"
							+ "         '" + txtMapCode.Text + "',"
							+ "         '" + nextOrderBy + "',"
							+ "         '" + TextCore.ConvertChar("_X_") + "',"
							+ "         GETDATE(),"
							+ "         '" + TextCore.ConvertChar("_X_") + "',"
							+ "         GETDATE()"
							+ "         )";

						mConfig.Execute(sql);

						sql = "  SELECT *"
							+ "    FROM PARKINGLOT_FLOOR"
							+ "   WHERE PARKINGLOT_NO = '" + mParkingLotNo + "'"
							+ "         AND FLOOR_CODE = '" + txtFloorCode.Text + "'";

						index = grdList.ImportR(mConfig, sql, -1);
						grdList.SelectRow(index);
						break;

					case DataProcType.Update:

						sql = "  UPDATE PARKINGLOT_FLOOR SET"
							+ "         FLOOR_NAME = '" + TextCore.ConvertChar(txtFloorName.Text) + "',"
							+ "         MAP_CODE = '" + TextCore.ConvertChar(txtMapCode.Text) + "',"
							+ "         UPDATE_USER_ID = '" + TextCore.ConvertChar("_X_") + "',"
							+ "         UPDATE_DATE = GETDATE()"
							+ "   WHERE PARKINGLOT_NO = '" + grdList.StoredRowData["PARKINGLOT_NO"] + "'"
							+ "         AND FLOOR_CODE = '" + grdList.StoredRowData["FLOOR_CODE"] + "'";

						mConfig.Execute(sql);

						sql = "  SELECT *"
							+ "    FROM PARKINGLOT_FLOOR"
							+ "   WHERE PARKINGLOT_NO = '" + grdList.StoredRowData["PARKINGLOT_NO"] + "'"
							+ "         AND FLOOR_CODE = '" + grdList.StoredRowData["FLOOR_CODE"] + "'";

						index = grdList.ImportR(mConfig, sql, grdList.StoredRowIndex);
						grdList.SelectRow(index);
						break;

					case DataProcType.Delete:

						try
						{
							mConfig.BeginTrans();

							sql = "  SELECT ORDER_BY"
								+ "    FROM PARKINGLOT_FLOOR"
								+ "   WHERE PARKINGLOT_NO = '" + grdList.StoredRowData["PARKINGLOT_NO"] + "'"
								+ "         AND FLOOR_CODE = '" + grdList.StoredRowData["FLOOR_CODE"] + "'";

							string orderBy = mConfig.SelectC(sql);

							if (orderBy == "")
								orderBy = "1";

							sql = "  UPDATE PARKINGLOT_FLOOR SET"
								+ "         ORDER_BY = ORDER_BY - 1"
								+ "   WHERE PARKINGLOT_NO = '" + grdList.StoredRowData["PARKINGLOT_NO"] + "'"
								+ "         AND ORDER_BY > " + orderBy;

							mConfig.Execute(sql);

							sql = "  DELETE FROM PARKINGLOT_FLOOR"
								+ "   WHERE PARKINGLOT_NO = '" + grdList.StoredRowData["PARKINGLOT_NO"] + "'"
								+ "         AND FLOOR_CODE = '" + grdList.StoredRowData["FLOOR_CODE"] + "'";

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