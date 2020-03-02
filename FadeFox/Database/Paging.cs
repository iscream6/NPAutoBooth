using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace FadeFox.Database
{
	public class Paging
	{
		DataTable mDataSource = null;
		int mPageSize = 10;				// 페이지당 행 수
		int mPageFromRowIndex = -1;		// 페이지의 시작 행 인덱스
		int mPageToRowIndex = -1;		// 페이지의 끝 행 인덱스
		int mCurrentPage = 1;			// 현재 페이지
		int mRowCount = 0;				// 전체 레코드 수
		int mPageCount = 0;				// 전체 페이지 수
		int mBlockSize = 10;			// 페이지 탭 표시 수
		int[] mPageBlockList = null;	// 페이지 블럭 안에 있는 페이지 리스트
		bool mPrevBlockExist = false;	// 전 페이지가 존재하는지
		bool mNextBlockExist = false;	// 다음 페이지가 존재하는지

		public int PageSize
		{
			get { return mPageSize; }
			set 
			{
				if (value > 0)
				{
					mPageSize = value;
					Initialize();
				}
				else
				{
					throw new Exception("올바르지 않은 크기입니다.");
				}
			}
		}

		public int PageFromRowIndex
		{
			get { return mPageFromRowIndex; }
		}

		public int PageToRowIndex
		{
			get { return mPageToRowIndex; }
		}

		public int CurrentPage
		{
			get { return mCurrentPage; }
		}

		public int RowCount
		{
			get { return mRowCount; }
		}

		public int PageCount
		{
			get { return mPageCount; }
		}

		public int BlockSize
		{
			get { return mBlockSize; }
			set { mBlockSize = value; }
		}

		public int[] PageBlockList
		{
			get { return mPageBlockList; }
		}

		public bool PrevBlockExist
		{
			get { return mPrevBlockExist; }
		}

		public bool NextBlockExist
		{
			get { return mNextBlockExist; }
		}

		public DataTable DataSource
		{
			get { return mDataSource; }
			set
			{
				mDataSource = value;

				Initialize();
			}
		}

		public Paging()
		{
		}

		public Paging(int pPageSize, int pBlockSize)
		{
			mPageSize = pPageSize;
			mBlockSize = pBlockSize;
		}

		public Paging(DataTable pDT)
		{
			mDataSource = pDT;

			Initialize();
		}


		private void Initialize()
		{
			if (mDataSource == null)
				return;

			mRowCount = mDataSource.Rows.Count;

			// 전체 페이지 수
			mPageCount = Convert.ToInt32((mRowCount - 1) / mPageSize) + 1;

			GoPage(1);
		}

		public DataTable this[int pPage]
		{
			get
			{
				return GoPage(pPage);
			}
		}

		public DataTable GoPage(int pPage)
		{
			mCurrentPage = pPage;

			// 현재 페이지 > 전체 페이지 수
			if (mCurrentPage > mPageCount) mCurrentPage = mPageCount;

			// 이전 페이지 블록이 존재하는지
			if (Convert.ToInt32((mCurrentPage - 1) / mBlockSize) > 0)
				mPrevBlockExist = true;
			else
				mPrevBlockExist = false;

			// 다음 페이지 블록이 존재하는지
			if ((Convert.ToInt32((mCurrentPage - 1) / mBlockSize) + 1) * mBlockSize < mPageCount)
				mNextBlockExist = true;
			else
				mNextBlockExist = false;

			int fromBlock = 1;
			int toBlock = 0;

			if (mPageCount == 0)
				toBlock = 0;
			else if ((Convert.ToInt32((mCurrentPage - 1) / mBlockSize) + 1) * mBlockSize < mPageCount)
				toBlock = mBlockSize;
			else
				toBlock = mPageCount - (Convert.ToInt32((mPageCount - 1) / mBlockSize) * mBlockSize);

			List<int> blockSizeList = new List<int>();

			for (int i = fromBlock; i <= toBlock; i++)
			{
				blockSizeList.Add((Convert.ToInt32((mCurrentPage - 1) / mBlockSize) * mBlockSize) + i);
			}

			mPageBlockList = blockSizeList.ToArray();

			mPageFromRowIndex = (mCurrentPage - 1) * mPageSize;
			if (mPageFromRowIndex < 0)
				mPageFromRowIndex = 0;

			mPageToRowIndex = mCurrentPage * mPageSize - 1;
			if (mPageToRowIndex > mRowCount - 1)
				mPageToRowIndex = mRowCount - 1;

			DataTable dt = mDataSource.Clone();

			for (int i = mPageFromRowIndex; i <= mPageToRowIndex; i++)
			{
				DataRow row = dt.NewRow();
				

				for (int j = 0; j < dt.Columns.Count; j++)
				{
					row[j] = mDataSource.Rows[i][j];
				}

				dt.Rows.Add(row);
			}

			return dt;
		}

		public override string ToString()
		{
			DataTable page = this[mCurrentPage];

			StringBuilder sb = new StringBuilder();

			sb.Append(this.CurrentPage.ToString() + "/" + this.PageCount + "\n");

			int columnCount = page.Columns.Count;

			foreach (DataRow r in page.Rows)
			{
				for (int i = 0; i < columnCount; i++)
				{
					sb.Append(r[i].ToString() + "\t");
				}

				sb.Append("\n");
			}

			sb.Append("[" + this.PrevBlockExist.ToString() + "]");

			foreach (int px in this.PageBlockList)
			{
				sb.Append("[" + px.ToString() + "]");
			}

			sb.Append("[" + this.NextBlockExist.ToString() + "]");

			return sb.ToString();
		}
	}
}
