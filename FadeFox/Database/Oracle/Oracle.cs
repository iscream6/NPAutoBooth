/* 
 * ==============================================================================
 *   Program ID     : Oracle
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description    : ODAC112021 설치 필요함.
 * ------------------------------------------------------------------------------
 *   Company Name   : fadefox
 *   Developer      : fadefox
 *   Create Date    : 2011-01-14
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 *       Orace 쿼리문 사용시 주의
 *       - 테이블에 별칭 줄 때 AS 키워드를 사용하면 안됨.
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;

namespace FadeFox.Database.Oracle
{
	public class Oracle : IDatabase
	{
		private string mServer = string.Empty;				// 서버명
		private string mPort = "1521";						// 포트
		private string mDatabase = string.Empty;			// 데이터베이스명
		private string mUserID = string.Empty;				// 접속자 아이디
		private string mPassword = string.Empty;			// 접속 패스워드
		private string mConnectionString = string.Empty;	// 연결문자열

		private OracleConnection mConnection = null;		// 연결 객체

		// 트렌젝션을 사용하기 위함.
		private bool mBeginTrans = false;                   // 트렌잭션 중임.
		private OracleCommand mTransCmd = null;				// 명령 객체
		private OracleTransaction mTrans = null;			// 트렌잭션 객체

		private bool mIsBusy = false;						// 해당 연결에 명령을 수행중인지 아닌지 검사.

		private string mSchemaName = "";					// 스키마 명

		public string SchemaName
		{
			get { return mSchemaName; }
			set { mSchemaName = value; }
		}

		public DatabaseKind DatabaseType
		{
			get { return DatabaseKind.Oracle; }
		}

		public string EmptyString
		{
			get { return "-"; }
		}

		public string Server
		{
			get { return mServer; }
			set
			{
				if (mServer != value)
				{
					mServer = value;
					BuildConnectionString();
				}
			}
		}

		public string Port
		{
			get	{ return mPort; }
			set
			{
				if (mPort != value && value != string.Empty)
				{
					mPort = value;
					BuildConnectionString();
				}
			}
		}

		public string Database
		{
			get	{ return mDatabase; }
			set
			{
				if (mDatabase != value)
				{
					mDatabase = value;
					BuildConnectionString();
				}
			}
		}

		public string UserID
		{
			get	{ return mUserID; }
			set
			{
				if (mUserID != value)
				{
					mUserID = value;
					BuildConnectionString();
				}
			}
		}

		public string Password
		{
			get { return mPassword;	}
			set
			{
				if (mPassword != value)
				{
					mPassword = value;
					BuildConnectionString();
				}
			}
		}

		/// <summary>
		/// 윈도우즈 인증 사용여부
		/// </summary>
		public bool WindowsAuthority
		{
			get { return false; }
			set { }
		}

		public string ConnectionString
		{
			get	{ return mConnectionString; }
		}

		public bool IsConnect
		{
			get
			{
				if (mConnection == null)
				{
					return false;
				}

				switch (mConnection.State)
				{
					case ConnectionState.Open:
						return true;
					case ConnectionState.Broken:
						return false;
					case ConnectionState.Closed:
						return false;
					case ConnectionState.Connecting:
						return false;
				}

				return false;
			}
		}

		public bool IsBusy
		{
			get { return mIsBusy; }
		}

		/*
		/// <summary>
		/// 연결개체
		/// </summary>
		public OracleConnection Connection
		{
			get { return mConnection; }
		}
		*/


		//
		// 기능 : 데이터 베이스 연결 문자열 설정
		// Oracle
		// "Data Source=(DESCRIPTION=
		//	 (ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=OTNSRVR)(PORT=1521)))
		//	 (CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCL)));
		//	 User Id=scott;Password=tiger;"
		//  => HOST : 서버 위치
		//  => SERVER_NAME : 전역 데이터베이스명
		//  => User Id : 접속가능한 아이디
		//  => Password : 해당 아이디에 대한 암호
		//
		private void BuildConnectionString()
		{
			mConnectionString = string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));User Id={3};Password={4};",
				mServer, mPort, mDatabase, mUserID, mPassword);
		}

		public bool Connect()
		{
			Disconnect();

			mConnection = new OracleConnection(mConnectionString);

			try
			{
				mConnection.Open();
			}
			catch (Exception ex)
			{
				Disconnect();
				throw new Exception(ex.Message);
			}

			return true;
		}

		public void Disconnect()
		{
			if (mConnection != null)
			{
				mConnection.Close();
				mConnection.Dispose();
				mConnection = null;
			}
		}

		public void BeginTrans()
		{
			while (mIsBusy)
			{
				System.Threading.Thread.Sleep(100);
			}

			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			if (mBeginTrans == true)
			{
				throw new Exception("이미 트랜잭션이 시작되었습니다.");
			}

			try
			{
				mTransCmd = mConnection.CreateCommand();
				mTrans = mConnection.BeginTransaction();
				mTransCmd.Connection = mConnection;
				mTransCmd.Transaction = mTrans;

				mBeginTrans = true;
			}
			catch (Exception ex)
			{
				mBeginTrans = false;
				throw new Exception(ex.Message);
			}
		}

		public void RollbackTrans()
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			if (mBeginTrans == false)
			{
				throw new Exception("트랜잭션이 존재하지 않습니다.");
			}

			try
			{
				mTrans.Rollback();
				mTrans.Dispose();
				mTrans = null;
				mTransCmd.Dispose();
				mTransCmd = null;
				mBeginTrans = false;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public void CommitTrans()
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			if (mBeginTrans == false)
			{
				throw new Exception("트랜잭션이 존재하지 않습니다.");
			}

			try
			{
				mTrans.Commit();
				mTrans.Dispose();
				mTrans = null;
				mTransCmd.Dispose();
				mTransCmd = null;
				mBeginTrans = false;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public string SelectC(string sql)
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			DataTable dt = this.SelectT(sql);

			if (dt == null)
				return string.Empty;

			if (dt.Rows.Count < 1)
				return string.Empty;

			string result = dt.Rows[0][0].ToString();
			dt.Dispose();
			dt = null;

			return result;
		}

		public DataRow SelectR(string sql)
		{
			DataTable dt = SelectT(sql);

			if (dt == null)
				return null;
			else
			{
				if (dt.Rows.Count < 1)
					return null;
				else
					return dt.Rows[0];
			}
		}

		public DataTable SelectT(string sql)
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			if (mBeginTrans == true)
				return _SelectT(mTransCmd, sql);
			else
				return _SelectT(mConnection.CreateCommand(), sql);
		}

		private DataTable _SelectT(OracleCommand cmd, string sql)
		{
			while (mIsBusy)
			{
				System.Threading.Thread.Sleep(100);
			}

			if (sql == "")
				return null;

			mIsBusy = true;

			cmd.CommandType = CommandType.Text;
			cmd.CommandText = sql;
			cmd.Parameters.Clear();

			OracleDataAdapter dapt = new OracleDataAdapter(cmd);
			DataTable dt = new DataTable();

			try
			{
				dapt.Fill(dt);
			}
			catch (Exception ex)
			{
				dt.Dispose();

				throw new Exception(ex.Message);
			}
			finally
			{
				dapt.Dispose();
				mIsBusy = false;
			}

			return dt;
		}

		public DataTable SelectT(string pProcedureName, object[] pParameter, bool isProcedure)
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			if (mBeginTrans == true)
				return _SelectT(mTransCmd, pProcedureName, pParameter);
			else
				return _SelectT(mConnection.CreateCommand(), pProcedureName, pParameter);
		}

		private DataTable _SelectT(OracleCommand cmd, string pProcedureName, object[] pParameter)
		{
			while (mIsBusy)
			{
				System.Threading.Thread.Sleep(100);
			}

			if (pProcedureName == "")
				return null;

			mIsBusy = true;

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = pProcedureName;
			cmd.Parameters.Clear();

			if (pParameter != null)
			{
				cmd.Parameters.AddRange(pParameter);
			}

			OracleDataAdapter dapt = new OracleDataAdapter(cmd);
			DataTable dt = new DataTable();

			try
			{
				dapt.Fill(dt);
			}
			catch (Exception ex)
			{
				dt.Dispose();

				throw new Exception(ex.Message);
			}
			finally
			{
				dapt.Dispose();
				mIsBusy = false;
			}

			return dt;
		}

		public DataSet SelectS(string sql)
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			if (mBeginTrans == true)
				return _SelectS(mTransCmd, sql);
			else
				return _SelectS(mConnection.CreateCommand(), sql);
		}

		private DataSet _SelectS(OracleCommand cmd, string sql)
		{
			while (mIsBusy)
			{
				System.Threading.Thread.Sleep(100);
			}

			if (sql == "")
				return null;

			mIsBusy = true;

			cmd.CommandType = CommandType.Text;
			cmd.CommandText = sql;
			cmd.Parameters.Clear();

			OracleDataAdapter dapt = new OracleDataAdapter(cmd);
			DataSet ds = new DataSet();

			try
			{
				dapt.Fill(ds);
			}
			catch (Exception ex)
			{
				ds.Dispose();

				throw new Exception(ex.Message);
			}
			finally
			{
				dapt.Dispose();
				mIsBusy = false;
			}

			return ds;
		}

		public int Execute(string sql)
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			if (mBeginTrans == true)
				return _Execute(mTransCmd, sql);
			else
				return _Execute(mConnection.CreateCommand(), sql);
		}

		// 
		// 기능 : Select 이외의 쿼리문 실행
		//
		private int _Execute(OracleCommand cmd, string sql)
		{
			while (mIsBusy)
			{
				System.Threading.Thread.Sleep(100);
			}

			if (sql == "")
				return -1;

			mIsBusy = true;

			int result = -1;

			try
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = sql;
				cmd.Parameters.Clear();

				result = cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				mIsBusy = false;
			}

			return result;
		}

        public int Execute(string pProcedureName, object[] pParameter, bool isProcedure)
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			if (mBeginTrans == true)
				return _Execute(mTransCmd, pProcedureName, pParameter);
			else
				return _Execute(mConnection.CreateCommand(), pProcedureName, pParameter);
		}

		// 
		// 기능 : Select 이외의 쿼리문 실행
		//
		private int _Execute(OracleCommand cmd, string pProcedureName, object[] pParameter)
		{
			while (mIsBusy)
			{
				System.Threading.Thread.Sleep(100);
			}

			if (pProcedureName == "")
				return -1;

			mIsBusy = true;

			int result = -1;

			try
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = pProcedureName;
				cmd.Parameters.Clear();

				if (pParameter != null)
				{
					cmd.Parameters.AddRange(pParameter);
				}

				result = cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				mIsBusy = false;
			}

			return result;
		}

		public bool IsExistTable(string pTableName)
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			string sql = string.Empty;


			sql = "  SELECT TABLE_NAME "
				+ "    FROM USER_TABLES"
				+ "   WHERE TABLE_NAME = '" + pTableName + "'";

			string result = SelectC(sql);

			if (result == string.Empty)
				return false;
			else
				return true;
		}

		public bool IsExistRoutine(string pRoutineName)
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			string sql = "";

			sql = "  SELECT ROUTINE_NAME, Y.TYPE, TRANSLATE(LTRIM(X.TEXT1, '/'), '/', ' ') ROUTINE_DEFINITION "
				+ "    FROM ("
				+ "         SELECT NAME ROUTINE_NAME, LEVEL LVL, SYS_CONNECT_BY_PATH(TEXT, '/') TEXT1"
				+ "           FROM USER_SOURCE"
				+ "         CONNECT BY LINE - 1 = PRIOR LINE AND NAME = PRIOR NAME) X,"
				+ "         ("
				+ "         SELECT NAME, TYPE, MAX(LINE) AS MAXLINE"
				+ "           FROM USER_SOURCE"
				+ "          GROUP BY NAME, TYPE"
				+ "         ) Y "
				+ "   WHERE X.ROUTINE_NAME = Y.NAME AND X.LVL = Y.MAXLINE"
				+ "         AND ROUTINE_NAME = '" + pRoutineName + "'";

			string result = SelectC(sql);

			if (result == string.Empty)
				return false;
			else
				return true;
		}

		public bool IsExistIndex(string pTableName, string pIndexName)
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			if (!IsExistTable(pTableName))
			{
				throw new Exception("테이블(" + pTableName + ")이 존재하지 않습니다.");
			}

			string sql = "";

			sql = "  SELECT INDEX_NAME"
				+ "    FROM USER_INDEXES"
				+ "   WHERE TABLE_NAME = '" + pTableName + "'"
				+ "         AND INDEX_NAME = '" + pIndexName + "'";

			string result = SelectC(sql);

			if (result == "")
				return false;
			else
				return true;
		}

		public bool IsExistColumn(string pTableName, string pColumnName)
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			if (!IsExistTable(pTableName))
			{
				throw new Exception("테이블(" + pTableName + ")이 존재하지 않습니다.");
			}

			string sql = string.Empty;

			sql = "  SELECT COLUMN_NAME"
				+ "    FROM USER_TAB_COLUMNS"
				+ "   WHERE TABLE_NAME = '" + pTableName + "'"
				+ "         AND COLUMN_NAME = '" + pColumnName + "'";

			string result = SelectC(sql);

			if (result == string.Empty)
				return false;
			else
				return true;
		}

		public void SetTableProperty(string pTableName, string pPropertyName, string pValue)
		{
		}

		public string GetTableProperty(string pTableName, string pPropertyName)
		{
			return string.Empty;
		}

		public void SetTableDescription(string pTableName, string pDescription)
		{
		}

		public string GetTableDescription(string pTableName)
		{
			return string.Empty;
		}

		public void SetColumnDescription(string pTableName, string pColumnName, string pDescription)
		{
		}

		public string GetColumnDescription(string pTableName, string pColumnName)
		{
			return string.Empty;
		}

		public int RecordCount(string sql)
		{
			DataTable dt = this.SelectT(sql);
			int recordCount = 0;

			if (dt == null)
				return -1;

			recordCount = dt.Rows.Count;

			dt.Dispose();
			dt = null;

			return recordCount;
		}

		public string GetCurrentTimeStamp()
		{
			return string.Empty;
		}
	}
}
