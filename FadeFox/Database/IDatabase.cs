/* 
 * ==============================================================================
 *   Program ID     : IDatabase
 *   Program Name   : IDatabase
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : fadefox
 *   Developer      : fadefox
 *   Create Date    : 2009-04-17
 * ------------------------------------------------------------------------------
 *   Update History
 *	     2010-04-02 : Procedure 실행을 위한 메소드 추가
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace FadeFox.Database
{
	public enum DatabaseKind
	{
		Access,
		MSSQL,
		MySQL,
		Oracle,
		SQLite,
		Excel
	}

	public interface IDatabase
	{
		/// <summary>
		/// 데이터베이스 종류
		/// </summary>
		DatabaseKind DatabaseType
		{
			get;
		}

		string EmptyString
		{
			get;
		}

		/// <summary>
		/// 서버 명
		/// </summary>
		string Server
		{
			get;
			set;
		}

		/// <summary>
		/// 포트
		/// </summary>
		string Port
		{
			get;
			set;
		}

		/// <summary>
		/// 데이터베이스 명
		/// </summary>
		string Database
		{
			get;
			set;
		}

		/// <summary>
		/// 접속 아이디
		/// </summary>
		string UserID
		{
			get;
			set;
		}

		/// <summary>
		/// 접속 암호
		/// </summary>
		string Password
		{
			get;
			set;
		}

		/// <summary>
		/// 윈도우 인증 사용 여부
		/// </summary>
		bool WindowsAuthority
		{
			get;
			set;
		}

		/// <summary>
		/// 스키마 명
		/// </summary>
		string SchemaName
		{
			get;
			set;
		}

		/// <summary>
		/// 연결문자열
		/// </summary>
		string ConnectionString
		{
			get;
		}

		/// <summary>
		/// 연결 여부
		/// </summary>
		bool IsConnect
		{
			get;
		}

		/// <summary>
		/// 해당 연결에 명령을 수행중인지 아닌지 검사.
		/// </summary>
		bool IsBusy
		{
			get;
		}

		/// <summary>
		/// 연결
		/// </summary>
		/// <returns></returns>
		bool Connect();

		/// <summary>
		/// 연결해제
		/// </summary>
		void Disconnect();

		/// <summary>
		/// 트랜젝션 시작
		/// </summary>
		void BeginTrans();

		/// <summary>
		/// 롤백
		/// </summary>
		void RollbackTrans();

		/// <summary>
		/// 커밋
		/// </summary>
		void CommitTrans();

		/// <summary>
		/// 첫번째 행의 첫번째 컬럼의 값을 가지고 옴
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		string SelectC(string sql);

		/// <summary>
		/// 쿼리에 대한 DataRow 리턴
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		DataRow SelectR(string sql);

		/// <summary>
		/// 쿼리에 대한 DataTable 리턴
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		DataTable SelectT(string sql);

		/// <summary>
		/// 프로시져를 통해 데이터를 가지고 옴.
		/// </summary>
		/// <param name="pProcedureName">프로지져 명</param>
		/// <param name="pParameter">프로시져 파라메터 배열</param>
		/// <returns></returns>
        DataTable SelectT(string pProcedureName, object[] pParameter, bool isProcedure);


		/// <summary>
		/// 쿼리에 대한 DataSet 리턴
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		DataSet SelectS(string sql);

		/// <summary>
		/// Select이외의 쿼리 실행
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		int Execute(string sql);


		/// <summary>
		/// 프로시져를 통해 Select이외의 쿼리 실행
		/// </summary>
		/// <param name="pProcedureName">프로시져 명</param>
		/// <param name="pParameter">프로시져 파라메터 배열</param>
		/// <returns></returns>
		int Execute(string pProcedureName, object[] pParameter, bool isProcedure);

		/// <summary>
		/// 테이블이 존재하는지 않하는지 검사
		/// </summary>
		/// <returns></returns>
		bool IsExistTable(string pTableName);

		/// <summary>
		/// 루틴이 존재하는지 않하는지 검사
		/// </summary>
		/// <param name="pRoutineName"></param>
		/// <returns></returns>
		bool IsExistRoutine(string pRoutineName);

		/// <summary>
		/// 인덱스 존재 여부 검사
		/// </summary>
		/// <param name="pTablename"></param>
		/// <param name="pIndexName"></param>
		/// <returns></returns>
		bool IsExistIndex(string pTableName, string pIndexName);

		/// <summary>
		/// 컬럼이 존재하는지 않하는지 검사
		/// </summary>
		/// <returns></returns>
		bool IsExistColumn(string pTableName, string pColumnName);

		/// <summary>
		/// 테이블 속성 설정
		/// </summary>
		/// <param name="pTableName"></param>
		/// <param name="pPropertyName"></param>
		/// <param name="pValue"></param>
		void SetTableProperty(string pTableName, string pPropertyName, string pValue);

		/// <summary>
		/// 테이블 속성 얻음
		/// </summary>
		/// <param name="pTableName"></param>
		/// <param name="pPropertyName"></param>
		/// <returns></returns>
		string GetTableProperty(string pTableName, string pPropertyName);

		/// <summary>
		/// 테이블 설명 설정
		/// </summary>
		/// <param name="pTableName"></param>
		/// <param name="pDescription"></param>
		void SetTableDescription(string pTableName, string pDescription);

		/// <summary>
		/// 테이블 설명 얻기
		/// </summary>
		/// <param name="pTableName"></param>
		/// <returns></returns>
		string GetTableDescription(string pTableName);

		/// <summary>
		/// 컬럼 설명 설정
		/// </summary>
		/// <param name="pTableName"></param>
		/// <param name="pColumnName"></param>
		/// <param name="pDescription"></param>
		void SetColumnDescription(string pTableName, string pColumnName, string pDescription);

		/// <summary>
		/// 컬럼 설명 얻기
		/// </summary>
		/// <param name="pTableName"></param>
		/// <param name="pColumnName"></param>
		/// <returns></returns>
		string GetColumnDescription(string pTableName, string pColumnName);


		/// <summary>
		/// 레코드 수량 얻기
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		int RecordCount(string sql);

		/// <summary>
		/// DB에서 날짜 얻어오기
		/// 형식은 yyyy-MM-dd hh:mm:ss 형태로
		/// </summary>
		/// <returns></returns>
		string GetCurrentTimeStamp();
	}
}
