using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace FadeFox.Database
{
	public class DataTablePaging
	{
		DataTable mTable = null;

		// 글 목록 개수
		int mRowCount = 10;
		public int RowCount
		{
			get { return mRowCount; }
			set { mRowCount = value; }
		}

		// 전체 레코드 수
		int mRecordCount = 0;
		public int RecordCount
		{
			get { return mRecordCount; }
		}

		// 전체 페이지 수
		int mPageCount = 0;
		public int PageCount
		{
			get { return mPageCount; }
		}

		// 현재 페이지 번호
		int mPageNumber = 1;
		public int PageNumber
		{
			get { return mPageNumber; }
			set { mPageNumber = value; }
		}



		public DataTablePaging(DataTable pTable)
		{
			mTable = pTable;
			mRecordCount = pTable.Rows.Count;
			mPageCount = Convert.ToInt32((Convert.ToDecimal(mRecordCount - 1) / Convert.ToDecimal(mRowCount)) + 1;
		}
	}
}
