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
using FadeFox.Utility;
using FadeFox.Text;
using Sayou.Core;
using FadeFox.Database.SQLite;

namespace NPConfig
{
	public partial class LookupInfo : DialogForm
	{
		DataProcType mProcType = DataProcType.None;
		ST_LOOKUP mSelectedParentLookup = null;
		SQLite mConfig = new SQLite();
		string mConfigFilePath = "";

		public LookupInfo(string pConfigFilePath)
		{
			InitializeComponent();
			mConfigFilePath = pConfigFilePath;
		}

		private void LookupInfo_Load(object sender, EventArgs e)
		{
		}

		private void LookupInfo_Shown(object sender, EventArgs e)
		{
			mConfig.Database = mConfigFilePath;
			mConfig.Connect();

			Initialize();
		}

		private void LookupInfo_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (grdList.IsBusy)
			{
				MsgBox.Show("작업중인 내용이 있습니다.");
				e.Cancel = true;
			}
		}

		private void LookupInfo_FormClosed(object sender, FormClosedEventArgs e)
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

			mSelectedParentLookup = null;

			// Initialize Screen
			grdList.Rows.Clear();

			btnUp.Visible = UpdateAuth;
			btnDown.Visible = UpdateAuth;

			btnInsert.Visible = InsertAuth;
			btnUpdate.Visible = UpdateAuth;
			btnDelete.Visible = DeleteAuth;

			btnParentLookupEdit.Enabled = true;
			btnInsert.Enabled = true;
			btnUpdate.Enabled = false;
			btnDelete.Enabled = false;

			btnUp.Enabled = false;
			btnDown.Enabled = false;
			
			cboKind.DisplayMember = "LOOKUP_NAME";
			cboKind.ValueMember = "LOOKUP_ID";

			if (!ReadLookupKind())
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

			if (cboKind.Items.Count > 0)
				cboKind.SelectedIndex = 0;
		}

		private bool ReadLookupKind()
		{
			cboKind.Items.Clear();

			string sql = string.Empty;

			sql = "  SELECT *"
				+ "    FROM ST_LOOKUP"
				+ "   WHERE PARENT_LOOKUP_ID = '" + mConfig.EmptyString + "'"
				+ "         AND LOOKUP_ID NOT IN ('ADDRESS', 'PHONE', 'MESSAGE', 'SERVER', 'DATABASE', 'SYSTEM')"
				+ "   ORDER BY ORDER_BY";

			DataTable dt = mConfig.SelectT(sql);



			if (dt != null)
			{
				if (dt.Rows.Count > 0)
				{
					foreach (DataRow dr in dt.Rows)
					{
						cboKind.Items.Add(new ST_LOOKUP(dr));
					}

					return true;
				}
				else
				{
					return false;
				}
			}

			return false;
		}

		private void RowSelected()
		{
			if (mProcType == DataProcType.None)
			{
				int selectedIndex = grdList.SelectedRows[0].Index;
				grdList.StoreRowData(selectedIndex);

				txtLookupID.Text = grdList.StoredRowData["LOOKUP_ID_VALUE"];
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

		private void RowsPreAdding(DataTable pDT)
		{
			foreach (DataRow r in pDT.Rows)
			{
				//r["SERVER_ID"] = r["SERVER_ID"].ToString().Replace(mPrefixID + ".", "");

				string[] lookupIDs = r["LOOKUP_ID"].ToString().Split('.');
				string lookupIDValue = lookupIDs[1];

				for (int i = 2; i < lookupIDs.Length; i++)
				{
					lookupIDValue += "." + lookupIDs[i];
				}

				r["LOOKUP_ID_VALUE"] = lookupIDValue;
			}
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
			txtLookupID.Text = "";
			txtLookupName.Text = "";
			txtLookupExtra.Text = "";
			txtLookupComment.Text = "";
		}

		private void btnParentLookupEdit_Click(object sender, EventArgs e)
		{
			Form frm = new LookupParentInfo(mConfigFilePath);

			if (frm.ShowDialog() == DialogResult.Yes)
			{
				Initialize();
			}

			frm.Dispose();
		}

		private void cboKind_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboKind.Items.Count < 1)
				return;

			if (cboKind.SelectedIndex < 0)
				return;

			mSelectedParentLookup = cboKind.SelectedItem as ST_LOOKUP;

			if (mSelectedParentLookup != null)
			{
				lblSubject.Text = mSelectedParentLookup.LOOKUP_NAME;
				btnSearch.PerformClick();
			}
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			if (mSelectedParentLookup == null)
			{
				MsgBox.Show("선택한 정보가 올바르지 않습니다.", MsgType.Error);
				return;
			}

			if (grdList.IsBusy)
			{
				MsgBox.Show("현재 작업중인 내용이 있습니다.", MsgType.Warning);
				return;
			}

			string sql = string.Empty;
			sql = "  SELECT *, '' LOOKUP_ID_VALUE"
				+ "    FROM ST_LOOKUP"
				+ "   WHERE PARENT_LOOKUP_ID = '" + mSelectedParentLookup.LOOKUP_ID + "'"
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
					+ "    FROM ST_LOOKUP"
					+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

				string orderBy = mConfig.SelectC(sql);

				sql = "  UPDATE ST_LOOKUP SET"
					+ "         ORDER_BY = " + orderBy + ","
					+ "         UPDATE_USER_ID = '" + "_X_" + "',"
					+ "         UPDATE_DATE = GETDATE()"
					+ "   WHERE PARENT_LOOKUP_ID = '" + grdList.StoredRowData["PARENT_LOOKUP_ID"] + "'"
					+ "         AND ORDER_BY = " + orderBy + " - 1";

				mConfig.Execute(sql);

				sql = "  UPDATE ST_LOOKUP SET"
					+ "         ORDER_BY = ORDER_BY - 1,"
					+ "         UPDATE_USER_ID = '" + "_X_" + "',"
					+ "         UPDATE_DATE = GETDATE()"
					+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

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
					+ "    FROM ST_LOOKUP"
					+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

				string orderBy = mConfig.SelectC(sql);

				sql = "  UPDATE ST_LOOKUP SET"
					+ "         ORDER_BY = " + orderBy + ","
					+ "         UPDATE_USER_ID = '" + "_X_" + "',"
					+ "         UPDATE_DATE = GETDATE()"
					+ "   WHERE PARENT_LOOKUP_ID = '" + grdList.StoredRowData["PARENT_LOOKUP_ID"] + "'"
					+ "         AND ORDER_BY = " + orderBy + " + 1";

				mConfig.Execute(sql);

				sql = "  UPDATE ST_LOOKUP SET"
					+ "         ORDER_BY = ORDER_BY + 1,"
					+ "         UPDATE_USER_ID = '" + "_X_" + "',"
					+ "         UPDATE_DATE = GETDATE()"
					+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

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
					lblSubject.Text = mSelectedParentLookup.LOOKUP_NAME + " 추가";
					pnlCondition.Enabled = false;

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

					break;

				case DataProcType.Update:
					lblSubject.Text = mSelectedParentLookup.LOOKUP_NAME + " 수정";
					pnlCondition.Enabled = false;

					pnlInput.Enabled = true;
					txtLookupID.Enabled = false;
					txtLookupName.Focus();

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
					lblSubject.Text = mSelectedParentLookup.LOOKUP_NAME + " 삭제";
					pnlCondition.Enabled = false;

					pnlInput.Enabled = false;

					pnlControl.Enabled = false;

					break;

				case DataProcType.None:
					lblSubject.Text = mSelectedParentLookup.LOOKUP_NAME;
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

			if (txtLookupID.Text.Trim() == string.Empty)
			{
				MsgBox.Show("코드를 입력해 주세요.", MsgType.Warning);
				txtLookupID.Focus();
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
						+ "    FROM ST_LOOKUP"
						+ "   WHERE LOOKUP_ID = '" + mSelectedParentLookup.LOOKUP_ID + "." + txtLookupID.Text + "'";

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
			if (mSelectedParentLookup == null)
				return false;
			
			string sql = string.Empty;
			int index = -1;

			try
			{
				switch (pProcType)
				{
					case DataProcType.Insert:
						sql = "  SELECT COALESCE(MAX(ORDER_BY), 0) + 1 AS NEXT_ORDER_BY"
							+ "    FROM ST_LOOKUP"
							+ "   WHERE PARENT_LOOKUP_ID = '" + mSelectedParentLookup.LOOKUP_ID + "'";

						string nextOrderBy = mConfig.SelectC(sql);

						if (nextOrderBy == string.Empty)
							nextOrderBy = "1";

						sql = "  INSERT INTO ST_LOOKUP ("
							+ "         LOOKUP_ID, PARENT_LOOKUP_ID, LOOKUP_NAME, LOOKUP_EXTRA, LOOKUP_COMMENT, ORDER_BY,"
							+ "         INSERT_USER_ID, INSERT_DATE, UPDATE_USER_ID, UPDATE_DATE"
							+ "         )"
							+ "  VALUES ("
							+ "         '" + mSelectedParentLookup.LOOKUP_ID + "." + TextCore.ConvertChar(txtLookupID.Text) + "',"
							+ "         '" + mSelectedParentLookup.LOOKUP_ID + "',"
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

						sql = "  SELECT *, '' LOOKUP_ID_VALUE"
							+ "    FROM ST_LOOKUP"
							+ "   WHERE LOOKUP_ID = '" + mSelectedParentLookup.LOOKUP_ID + "." + txtLookupID.Text + "'";

						index = grdList.ImportR(mConfig, sql, -1);
						grdList.SelectRow(index);
						break;

					case DataProcType.Update:

						sql = "  UPDATE ST_LOOKUP SET"
							+ "         LOOKUP_NAME = '" + TextCore.ConvertChar(txtLookupName.Text) + "',"
							+ "         LOOKUP_EXTRA = '" + TextCore.ConvertChar(txtLookupExtra.Text) + "',"
							+ "         LOOKUP_COMMENT = '" + TextCore.ConvertChar(txtLookupComment.Text) + "',"
							+ "         UPDATE_USER_ID = '" + TextCore.ConvertChar("_X_") + "',"
							+ "         UPDATE_DATE = GETDATE()"
							+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

						mConfig.Execute(sql);

						sql = "  SELECT *, '' LOOKUP_ID_VALUE"
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
								+ "   WHERE PARENT_LOOKUP_ID = '" + grdList.StoredRowData["PARENT_LOOKUP_ID"] + "'"
								+ "         AND ORDER_BY > " + orderBy;

							mConfig.Execute(sql);

							sql = "  DELETE FROM ST_LOOKUP"
								+ "   WHERE LOOKUP_ID = '" + grdList.StoredRowData["LOOKUP_ID"] + "'";

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

	#region 데이터 클래스

	/// <summary>
	/// 
	/// </summary>
	public class ST_LOOKUP
	{
		/// <summary>
		/// 
		/// </summary>
		private string mLOOKUP_ID = string.Empty;
		public string LOOKUP_ID
		{
			get { return mLOOKUP_ID; }
			set { mLOOKUP_ID = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		private string mPARENT_LOOKUP_ID = string.Empty;
		public string PARENT_LOOKUP_ID
		{
			get { return mPARENT_LOOKUP_ID; }
			set { mPARENT_LOOKUP_ID = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		private string mLOOKUP_NAME = string.Empty;
		public string LOOKUP_NAME
		{
			get { return mLOOKUP_NAME; }
			set { mLOOKUP_NAME = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		private string mLOOKUP_EXTRA = string.Empty;
		public string LOOKUP_EXTRA
		{
			get { return mLOOKUP_EXTRA; }
			set { mLOOKUP_EXTRA = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		private string mLOOKUP_COMMENT = string.Empty;
		public string LOOKUP_COMMENT
		{
			get { return mLOOKUP_COMMENT; }
			set { mLOOKUP_COMMENT = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		private string mORDER_BY = string.Empty;
		public string ORDER_BY
		{
			get { return mORDER_BY; }
			set { mORDER_BY = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		private string mINSERT_USER_ID = string.Empty;
		public string INSERT_USER_ID
		{
			get { return mINSERT_USER_ID; }
			set { mINSERT_USER_ID = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		private string mINSERT_DATE = string.Empty;
		public string INSERT_DATE
		{
			get { return mINSERT_DATE; }
			set { mINSERT_DATE = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		private string mUPDATE_USER_ID = string.Empty;
		public string UPDATE_USER_ID
		{
			get { return mUPDATE_USER_ID; }
			set { mUPDATE_USER_ID = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		private string mUPDATE_DATE = string.Empty;
		public string UPDATE_DATE
		{
			get { return mUPDATE_DATE; }
			set { mUPDATE_DATE = value; }
		}

		/// <summary>
		/// 생성자
		/// </summary>
		public ST_LOOKUP(DataRow pR)
		{
			SetValue(pR);
		}

		public ST_LOOKUP()
		{
			Clear();
		}

		/// <summary>
		/// 값 설정
		/// </summary>
		public void SetValue(DataRow pR)
		{
			mLOOKUP_ID = pR["LOOKUP_ID"].ToString();
			mPARENT_LOOKUP_ID = pR["PARENT_LOOKUP_ID"].ToString();
			mLOOKUP_NAME = pR["LOOKUP_NAME"].ToString();
			mLOOKUP_EXTRA = pR["LOOKUP_EXTRA"].ToString();
			mLOOKUP_COMMENT = pR["LOOKUP_COMMENT"].ToString();
			mORDER_BY = pR["ORDER_BY"].ToString();
			mINSERT_USER_ID = pR["INSERT_USER_ID"].ToString();
			mINSERT_DATE = pR["INSERT_DATE"].ToString();
			mUPDATE_USER_ID = pR["UPDATE_USER_ID"].ToString();
			mUPDATE_DATE = pR["UPDATE_DATE"].ToString();
		}

		/// <summary>
		/// 초기화
		/// </summary>
		public void Clear()
		{
			mLOOKUP_ID = string.Empty;		//
			mPARENT_LOOKUP_ID = string.Empty;		//
			mLOOKUP_NAME = string.Empty;		//
			mLOOKUP_EXTRA = string.Empty;		//
			mLOOKUP_COMMENT = string.Empty;		//
			mORDER_BY = string.Empty;		//
			mINSERT_USER_ID = string.Empty;		//
			mINSERT_DATE = string.Empty;		//
			mUPDATE_USER_ID = string.Empty;		//
			mUPDATE_DATE = string.Empty;		//
		}
	}

	#endregion 데이터 클래스 끝
}