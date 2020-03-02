/* 
 * ==============================================================================
 *   Program ID     : MSSQL
 *   Program Name   : MSSQL
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : fadefox
 *   Developer      : fadefox
 *   Create Date    : 2009-04-17
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace FadeFox.Database.MSSQL
{
	public class MSSQL : IDatabase
	{
		private string mServer = string.Empty;				// 서버명
		private string mPort = "1433";						// 포트
		private string mDatabase = string.Empty;			// 데이터베이스명
		private string mUserID = string.Empty;				// 접속자 아이디
		private string mPassword = string.Empty;			// 접속 패스워드
		private string mConnectionString = string.Empty;	// 연결문자열

		private SqlConnection mConnection = null;           // 연결 객체

		// 트렌젝션을 사용하기 위함.
		private bool mBeginTrans = false;                   // 트렌잭션 중임.
		private SqlCommand mTransCmd = null;                // 트렌잭션용 명령 객체
		private SqlTransaction mTrans = null;               // 트렌잭션 객체

		private bool mIsBusy = false;						// 현재 해당 연결에 대하여 명령을 수행중인지 아닌지 표시

		private bool mWindowsAuthority = false;             // 윈도우즈 인증 사용

		private string mSchemaName = "dbo";					// 스키마 명

		public string SchemaName
		{
			get { return mSchemaName; }
			set { mSchemaName = value; }
		}

		public DatabaseKind DatabaseType
		{
			get { return DatabaseKind.MSSQL; }
		}

		public string EmptyString
		{
			get { return ""; }
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
			get	{ return mPort;	}
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
			get	{ return mPassword;	}
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
			get { return mWindowsAuthority; }
			set
			{
				if (mWindowsAuthority != value)
				{
					mWindowsAuthority = value;
					BuildConnectionString();
				}
			}
		}

		public string ConnectionString
		{
			get	{ return mConnectionString;	}
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

        
        public SqlConnection GetConnection
        {
            get { return mConnection; }
        }
        

		//
		// 기능 : 데이터 베이스 연결 문자열 설정
		// MSSQL
		//  => Data Source : 서버 위치
		//  => Initial Catalog : 사용할 데이터베이스명
		//  => User ID : 접속가능한 아이디
		//  => Password : 해당 아이디에 대한 암호
		//
		private void BuildConnectionString()
		{
			if (mWindowsAuthority == true)
			{
				//MultipleActiveResultSets=True
				mConnectionString = string.Format("Persist Security Info=False;Server={0},{1};Database={2};Integrated Security=SSPI;",
				   mServer, mPort, mDatabase);
			}
			else
			{
                mConnectionString = string.Format("Persist Security Info=False;Server={0},{1};Database={2};User ID={3};Password={4};Connection Timeout=5;",
				   mServer, mPort, mDatabase, mUserID, mPassword);
                
			}
		}

		public bool Connect()
		{
			Disconnect();

			mConnection = new SqlConnection(mConnectionString);

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
			catch (SqlException ex)
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
			catch (SqlException ex)
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
			catch (SqlException ex)
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

		public System.Data.DataRow SelectR(string sql)
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

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

		public System.Data.DataTable SelectT(string sql)
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


		private DataTable _SelectT(SqlCommand cmd, string sql)
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

			SqlDataAdapter dapt = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();

			try
			{
				dapt.Fill(dt);
			}
			catch (SqlException ex)
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

		public System.Data.DataTable SelectT(string pProcedureName, object[] pParameter,bool isStoreProcedure)
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}
                if (mBeginTrans == true)
                    return _SelectT(mTransCmd, pProcedureName, pParameter, isStoreProcedure);
                else
                    return _SelectT(mConnection.CreateCommand(), pProcedureName, pParameter, isStoreProcedure);
		}

        private DataTable _SelectT(SqlCommand cmd, string pProcedureName, object[] pParameter, bool isStoreProcedure)
		{
			while (mIsBusy)
			{
				System.Threading.Thread.Sleep(100);
			}

			if (pProcedureName == "")
				return null;

			mIsBusy = true;
            if (isStoreProcedure)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }
			cmd.CommandText = pProcedureName;
			cmd.Parameters.Clear();

			if (pParameter != null)
			{
				cmd.Parameters.AddRange(pParameter);
			}

			SqlDataAdapter dapt = new SqlDataAdapter(cmd);
			DataTable dt = new DataTable();

			try
			{
				dapt.Fill(dt);
			}
			catch (SqlException ex)
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

		private DataSet _SelectS(SqlCommand cmd, string sql)
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

			SqlDataAdapter dapt = new SqlDataAdapter(cmd);
			DataSet ds = new DataSet();

			try
			{
				dapt.Fill(ds);
			}
			catch (SqlException ex)
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
		private int _Execute(SqlCommand cmd, string sql)
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
			catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				mIsBusy = false;
			}

			return result;
		}

		public int Execute(string pProcedureName, object[] pParameter, bool isStoreProcedure)
		{
			if (this.IsConnect == false)
			{
				throw new Exception("DB와 연결되어 있지 않습니다.");
			}

			if (mBeginTrans == true)
				return _Execute(mTransCmd, pProcedureName, pParameter,isStoreProcedure);
			else
				return _Execute(mConnection.CreateCommand(), pProcedureName, pParameter,isStoreProcedure);
		}

		// 
		// 기능 : Select 이외의 프로시져 실행
		//
		private int _Execute(SqlCommand cmd, string pProcedureName, object[] pParameter ,bool isStoreProcedure)
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
                if (isStoreProcedure)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }
				cmd.CommandText = pProcedureName;
				cmd.Parameters.Clear();

				if (pParameter != null)
				{
					cmd.Parameters.AddRange(pParameter);
				}

				result = cmd.ExecuteNonQuery();
			}
			catch (SqlException ex)
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
				+ "    FROM INFORMATION_SCHEMA.TABLES "
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

			string sql = string.Empty;

			sql = "  SELECT ROUTINE_NAME "
				+ "    FROM INFORMATION_SCHEMA.ROUTINES "
				+ "   WHERE ROUTINE_NAME = '" + pRoutineName + "'";

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

			sql = "  SELECT NAME "
				 + "    FROM SYSINDEXES "
				 + "   WHERE ID = (SELECT ID FROM SYSOBJECTS WHERE NAME = '" + pTableName + "')"
				 + "         AND NAME = '" + pIndexName + "'";

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

			sql = "  SELECT NAME "
				+ "    FROM SYSCOLUMNS"
				+ "   WHERE ID  = (SELECT ID FROM SYSOBJECTS WHERE NAME = '" + pTableName + "' AND TYPE = 'U')"
				+ "         AND NAME = '" + pColumnName + "'";

			string result = SelectC(sql);

			if (result == string.Empty)
				return false;
			else
				return true;
		}

        public void SetTableProperty(string pTableName, string pPropertyName, string pValue)
        {
            if (!IsExistTable(pTableName))
                return;

            string sql = string.Empty;

            // 테이블 디스크립션 저장
            sql = "  SELECT COUNT(*)"
                + "    FROM fn_listextendedproperty("
                + "  		'" + pPropertyName + "',"
                + "  		'schema', 'dbo',"
                + "  		'table', default,"
                + "  		NULL, NULL"
                + "  		)"
                + "   WHERE OBJNAME = '" + pTableName + "'";

            bool isExistProerty = (SelectC(sql) == "0" ? false : true);

            if (isExistProerty)
            {
                sql = "  EXEC sp_updateextendedproperty"
                    + "      '" + pPropertyName + "', '" + pValue + "',"
                    + "      'schema', 'dbo',"
                    + "      'table', '" + pTableName + "'";
            }
            else
            {
                sql = "  EXEC sp_addextendedproperty"
                    + "      '" + pPropertyName + "', '" + pValue + "',"
                    + "      'schema', 'dbo',"
                    + "      'table', '" + pTableName + "'";
            }

            Execute(sql);
        }

        public string GetTableProperty(string pTableName, string pPropertyName)
        {
            string sql = string.Empty;

            sql = "   SELECT ISNULL(VALUE, '') VALUE"
                + "     FROM SYS.TABLES T"
                + "     LEFT JOIN ("
                + "          SELECT MAJOR_ID TABLE_ID, VALUE"
                + "            FROM SYS.EXTENDED_PROPERTIES"
                + "           WHERE MINOR_ID = 0 AND NAME = '" + pPropertyName + "'"
                + "          ) TD"
                + "       ON T.OBJECT_ID = TD.TABLE_ID"
                + "    WHERE OBJECTPROPERTY(T.OBJECT_ID, 'IsMsShipped') = 0"
                + "          AND NAME = '" + pTableName + "'";

            return SelectC(sql);
        }

		public void SetTableDescription(string pTableName, string pDescription)
		{
			if (!IsExistTable(pTableName))
				return;

			string sql = string.Empty;

			// 테이블 디스크립션 저장
			sql = "  SELECT COUNT(*)"
				+ "    FROM fn_listextendedproperty("
				+ "  		'MS_Description',"
				+ "  		'schema', 'dbo',"
				+ "  		'table', default,"
				+ "  		NULL, NULL"
				+ "  		)"
				+ "   WHERE OBJNAME = '" + pTableName + "'";

			bool isExistDescription = (SelectC(sql) == "0" ? false : true);

			if (isExistDescription)
			{
				sql = "  EXEC sp_updateextendedproperty"
					+ "      'MS_Description', '" + pDescription + "',"
					+ "      'schema', 'dbo',"
					+ "      'table', '" + pTableName + "'";
			}
			else
			{
				sql = "  EXEC sp_addextendedproperty"
					+ "      'MS_Description', '" + pDescription + "',"
					+ "      'schema', 'dbo',"
					+ "      'table', '" + pTableName + "'";
			}

			Execute(sql);
		}

		public string GetTableDescription(string pTableName)
		{
			string sql = string.Empty;

			sql = "   SELECT ISNULL(TABLE_DESCRIPTION, '') TABLE_DESCRIPTION"
				+ "     FROM SYS.TABLES T"
				+ "     LEFT JOIN ("
				+ "          SELECT MAJOR_ID TABLE_ID, VALUE TABLE_DESCRIPTION"
				+ "            FROM SYS.EXTENDED_PROPERTIES"
				+ "           WHERE MINOR_ID = 0 AND NAME = 'MS_Description'"
				+ "          ) TD"
				+ "       ON T.OBJECT_ID = TD.TABLE_ID"
				+ "    WHERE OBJECTPROPERTY(T.OBJECT_ID, 'IsMsShipped') = 0"
				+ "          AND NAME = '" + pTableName + "'";

			return SelectC(sql);
		}

		public void SetColumnDescription(string pTableName, string pColumnName, string pDescription)
		{
			if (!IsExistColumn(pTableName, pColumnName))
				return;

			string sql = string.Empty;

			// 컬럼 디스크립션 저장
			sql = "  SELECT COUNT(*)"
				+ "    FROM fn_listextendedproperty("
				+ "  		'MS_Description',"
				+ "  		'schema', 'dbo',"
				+ "  		'table', '" + pTableName + "',"
				+ "  		'column', default"
				+ "  		)"
				+ "   WHERE OBJNAME = '" + pColumnName + "'";

			bool isExistDescription = (SelectC(sql) == "0" ? false : true);

			if (isExistDescription)
			{
				sql = "  EXEC sp_updateextendedproperty"
					+ "      'MS_Description', '" + pDescription + "',"
					+ "      'schema', 'dbo',"
					+ "      'table', '" + pTableName + "',"
					+ "      'column', '" + pColumnName + "'";
			}
			else
			{
				sql = "  EXEC sp_addextendedproperty"
					+ "      'MS_Description', '" + pDescription + "',"
					+ "      'schema', 'dbo',"
					+ "      'table', '" + pTableName + "',"
					+ "      'column', '" + pColumnName + "'";
			}

			Execute(sql);
		}

		public string GetColumnDescription(string pTableName, string pColumnName)
		{
			string sql = string.Empty;

			sql = "  SELECT COLUMN_DESCRIPTION"
				+ "    FROM ("
				+ "         SELECT OBJECT_NAME(TC.TABLE_ID) TABLE_NAME,"
				+ "                TC.COLUMN_NAME, ISNULL(CD.COLUMN_DESCRIPTION, '') COLUMN_DESCRIPTION"
				+ "           FROM ("
				+ "                SELECT OBJECT_ID TABLE_ID, COLUMN_ID, NAME COLUMN_NAME"
				+ "                  FROM SYS.COLUMNS"
				+ "                 WHERE OBJECTPROPERTY(OBJECT_ID, 'IsMsShipped') = 0"
				+ "                ) TC"
				+ "           LEFT JOIN ("
				+ "                SELECT MAJOR_ID TABLE_ID, MINOR_ID COLUMN_ID, VALUE COLUMN_DESCRIPTION"
				+ "                  FROM SYS.EXTENDED_PROPERTIES"
				+ "                 WHERE NAME = 'MS_Description'"
				+ "                ) CD"
				+ "             ON TC.TABLE_ID = CD.TABLE_ID"
				+ "                AND TC.COLUMN_ID = CD.COLUMN_ID"
				+ "         ) A"
				+ "   WHERE TABLE_NAME = '" + pTableName + "' AND COLUMN_NAME = '" + pColumnName + "'";

			return SelectC(sql);
		}

		//
		// 기능 레코드 카운트
		//
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
			return SelectC("SELECT CONVERT(VARCHAR(20), GETDATE(), 120)");
		}
	}
}
