/* 
 * ==============================================================================
 *   Program ID     : SQLite
 *   Program Name   : SQLite
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : fadefox
 *   Developer      : fadefox
 *   Create Date    : 2010-03-10
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
using System.Data.SQLite;

namespace FadeFox.Database.SQLite
{
	public class SQLite : IDatabase
	{
		private string mServer = string.Empty;				// 서버명, 사용안함
		private string mPort = string.Empty;				// 포트, 사용안함
		private string mDatabase = string.Empty;			// 데이터베이스명
		private string mUserID = string.Empty;				// 접속자 아이디, 사용안함
		private string mPassword = string.Empty;			// 접속 패스워드
		private string mConnectionString = string.Empty;	// 연결문자열

		private SQLiteConnection mConnection = null;           // 연결 객체

		// 트렌젝션을 사용하기 위함.
		private bool mBeginTrans = false;                   // 트렌잭션 중임.
		private SQLiteCommand mTransCmd = null;               // 명령 객체
		private SQLiteTransaction mTrans = null;               // 트렌잭션 객체

		private bool mIsBusy = false;						// 현재 해당 연결에 대하여 명령을 수행중인지 아닌지 표시

		private bool mWindowsAuthority = false;             // 윈도우즈 인증 사용, 사용안함

		private string mSchemaName = "";					// 스키마 명

		public string SchemaName
		{
			get { return mSchemaName; }
			set { mSchemaName = value; }
		}

		public DatabaseKind DatabaseType
		{
			get { return DatabaseKind.SQLite; }
		}

		public string EmptyString
		{
			get { return ""; }
		}

		/// <summary>
		/// 사용안함
		/// </summary>
		public string Server
		{
			get { return mServer; }
			set {}
		}

		/// <summary>
		/// SQLite는 사용안함
		/// </summary>
		public string Port
		{
			get	{ return mPort;	}
			set {}
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

		/// <summary>
		/// SQLite는 사용안함
		/// </summary>
		public string UserID
		{
			get	{ return mUserID; }
			set {}
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
		/// 윈도우즈 인증 사용여부, 사용안함.
		/// </summary>
		public bool WindowsAuthority
		{
			get { return mWindowsAuthority; }
			set {}
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

		/*
		public SqlConnection Connection
		{
			get { return mConnection; }
		}
		*/

		//
		// 기능 : 데이터 베이스 연결 문자열 설정
		// MSSQL
		//  => Data Source : 데이터베이스 파일 위치
		//  => Password : 암호
		//  => Pooling : (True Or False) 기본 False
		//  => FailIfMissing : 만약 데이터베이스 파일이 없으면 생성할지 오류를 낼 지 여부(True:오류발생 Or False:자동생성)
		//  => Default Timeout : {초단위} 기본은 30
		//  => Read Only : (True Or False) 기본 False
		//  => Default IsolationLevel : 트렌잭션 격리 레벨, 기본 Serializable
		private void BuildConnectionString()
		{
			if (mPassword == string.Empty)
			{
				//MultipleActiveResultSets=True
				mConnectionString = string.Format("Data Source={0}; FailIfMissing=False; Pooling=False;",
				   mDatabase);
			}
			else
			{
				mConnectionString = string.Format("Data Source={0}; Password={1}; FailIfMissing=False; Pooling=False;",
				   mDatabase, mPassword);
			}
		}

		public bool Connect()
		{
			Disconnect();

			mConnection = new SQLiteConnection(mConnectionString);

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
			catch (SQLiteException ex)
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
			catch (SQLiteException ex)
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
			catch (SQLiteException ex)
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

		private DataTable _SelectT(SQLiteCommand cmd, string sql)
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

			SQLiteDataAdapter dapt = new SQLiteDataAdapter(cmd);
			DataTable dt = new DataTable();

			try
			{
				dapt.Fill(dt);
			}
			catch (SQLiteException ex)
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

		public System.Data.DataTable SelectT(string pProcedureName, object[] pParameter, bool isProcedure)
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

		private DataTable _SelectT(SQLiteCommand cmd, string pProcedureName, object[] pParameter)
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

			SQLiteDataAdapter dapt = new SQLiteDataAdapter(cmd);
			DataTable dt = new DataTable();

			try
			{
				dapt.Fill(dt);
			}
			catch (SQLiteException ex)
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

		private DataSet _SelectS(SQLiteCommand cmd, string sql)
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

			SQLiteDataAdapter dapt = new SQLiteDataAdapter(cmd);
			DataSet ds = new DataSet();

			try
			{
				dapt.Fill(ds);
			}
			catch (SQLiteException ex)
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
		private int _Execute(SQLiteCommand cmd, string sql)
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
			catch (SQLiteException ex)
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
		private int _Execute(SQLiteCommand cmd, string pProcedureName, object[] pParameter)
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
			catch (SQLiteException ex)
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

			sql = "  SELECT NAME "
				+ "    FROM SQLITE_MASTER  "
				+ "   WHERE TYPE = 'table' AND NAME = '" + pTableName + "'";


			string result = SelectC(sql);

			if (result == string.Empty)
				return false;
			else
				return true;
		}

		public bool IsExistRoutine(string pRoutineName)
		{
			return false;
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
				+ "    FROM SQLITE_MASTER  "
				+ "   WHERE TYPE='index' AND TBL_NAME = '" + pTableName + "' AND NAME = '" + pIndexName + "'";

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

			sql = "PRAGMA TABLE_INFO(" + pTableName + ")";

			DataTable dt = this.SelectT(sql);

			if (dt != null)
			{
				foreach (DataRow r in dt.Rows)
				{
					if (r["name"].ToString() == pColumnName)
						return true;
				}
			}

			return false;
		}

        public void SetTableProperty(string pTableName, string pPropertyName, string pValue)
        {
			return;
        }

        public string GetTableProperty(string pTableName, string pPropertyName)
        {
			return "";
        }

		public void SetTableDescription(string pTableName, string pDescription)
		{
			return;
		}

		public string GetTableDescription(string pTableName)
		{
			return "";
		}

		public void SetColumnDescription(string pTableName, string pColumnName, string pDescription)
		{
			return;
		}

		public string GetColumnDescription(string pTableName, string pColumnName)
		{
			string sql = string.Empty;

			return "";
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
			return SelectC("SELECT DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME')");
		}

		[SQLiteFunction(Name = "ToUpper", Arguments = 1, FuncType = FunctionType.Scalar)]
		public class ToUpper : SQLiteFunction
		{
			public override object Invoke(object[] args)
			{
				return args[0].ToString().ToUpper();
			}
		}
		
		[SQLiteFunction(Name = "GetDate", Arguments = 0, FuncType = FunctionType.Scalar)]
		public class GetDate : SQLiteFunction
		{
			public override object Invoke(object[] args)
			{
				return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			}
		}
	}
}
