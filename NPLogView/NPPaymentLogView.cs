using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FadeFox.UI;
using System.IO;
using FadeFox.Utility;
using FadeFox.Database;
using FadeFox.Database.SQLite;

namespace NPLogView
{
	public partial class NPPaymentLogView : Form
	{
		public NPPaymentLogView()
		{
			InitializeComponent();

			oftConfig.Text = "nppayment_log.db3";
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			if (oftConfig.Text == "")
			{
				MsgBox.Show("설정파일을 선택해 주세요.");
			}
			else
			{
				if (File.Exists(oftConfig.Text))
				{
					Search(oftConfig.Text);
				}
				else
				{
					MsgBox.Show("존재하지 않는 설정파일을 선택해 주세요.", MsgType.Warning);
				}
			}
		}

		public void Search(string pPath)
		{
			string fileName = Path.GetFileName(pPath);

			grdList.Rows.Clear();

			//IDatabase
			if (fileName.ToLower() == "nppayment_log.db3")
			{
				SQLite db = new SQLite();
				string sql = "";

				db.Database = pPath;

				db.Connect();

				sql = "  SELECT LOG_DATE 로그일, "
					+ "         CAR_NUMBER 차량번호, "
					+ "         IN_DATE 입차일, "
					+ "         CURRENT_DATE 정산일,"
					+ "         PAYMENT_METHOD 정산방법, "
					+ "         ELAPSED_TOTAL_MINUTE 총주차시간_분,"
					+ "         BASE_MINUTE 기본주차시간_분,"
					+ "         BASE_FEE 기본주차요금_원,"
					+ "         UNIT_MINUTE 단위주차시간_분,"
					+ "         FEE_PER_UNIT_MINUTE 단위주차요금_원,"
					+ "         PARKING_FEE 주차요금_원,"
					+ "         CURRENT_MONEY 투입금액_원,"
					+ "         CHARGE_MONEY 거스름돈_원,"
					+ "         DISCOUNT_MINUTE 할인_분,"
					+ "         DISCOUNT_MONEY 할인_원,"
					+ "         PAYMENT_LOG_RESULT_TYPE 지불처리결과,"
					+ "         LATE_CHARGE_YN 추가정산여부,"
					+ "         BEFORE_PAYMENT_MONEY 이전결제금액_원,"
					+ "         COMMENT 비고"
					+ "    FROM PAYMENT_LOG"
					+ "   WHERE LOG_DATE >= '" + dtpFrom.Text + " 00:00:00' AND LOG_DATE <= '" + dtpTo.Text + " 23:59:59'"
					+ "   ORDER BY LOG_DATE";

				grdList.ImportT(db, sql);

				//db.Disconnect();
			}
		}
	}

	internal class TableInfo
	{
		/// <summary>
		/// 테이블 명
		/// </summary>
		public string TableName
		{
			get;
			set;
		}

		/// <summary>
		/// 쿼리
		/// </summary>
		public string Query
		{
			get;
			set;
		}

		/// <summary>
		/// 설명
		/// </summary>
		public string Description
		{
			get;
			set;
		}
	}
}
