using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace FadeFox.Database
{
	public class DatabaseCore
	{
		// 데이터베이스 공통 모듈임.
		public static IDatabase CreateDatabaseClass(string pDatabaseKind)
		{
			switch (pDatabaseKind.ToUpper())
			{
				case "DATABASE.MSSQL":
					return CreateDatabaseClass(DatabaseKind.MSSQL);
				case "DATABASE.MYSQL":
					return CreateDatabaseClass(DatabaseKind.MySQL);
				case "DATABASE.ORACLE":
					return CreateDatabaseClass(DatabaseKind.Oracle);
				case "DATABASE.SQLITE":
					return CreateDatabaseClass(DatabaseKind.SQLite);
				default:
					return null;
			}
		}

		public static IDatabase CreateDatabaseClass(DatabaseKind pKind)
		{
			switch (pKind)
			{
				case DatabaseKind.MSSQL:
					return new FadeFox.Database.MSSQL.MSSQL();
				case DatabaseKind.Oracle:
					return new FadeFox.Database.Oracle.Oracle();
				case DatabaseKind.SQLite:
					return new FadeFox.Database.SQLite.SQLite();
				default:
					return null;
			}
		}

		public static bool CopyTableData(IDatabase pSource, IDatabase pTarget, string pTableName)
		{
			if (pSource == null || pTarget == null)
				return false;

			try
			{
				string sql = "";
				string fieldList = "";
				string valueList = "";
				string updateList = "";

				sql = "  SELECT * FROM " + pTableName;

				DataTable dtSource = pSource.SelectT(sql);

				DataTable dtTarget = pTarget.SelectT(sql);

				if (dtSource.Columns.Count != dtTarget.Columns.Count)
				{
					return false;
				}

				for (int i = 0; i < dtSource.Columns.Count; i++)
				{
					if (dtSource.Columns[i].ColumnName != dtTarget.Columns[i].ColumnName)
					{
						return false;
					}

					if (fieldList != "")
						fieldList += ", ";

					fieldList += dtSource.Columns[i].ColumnName;

					if (updateList != "")
						updateList += ", ";

					updateList += dtSource.Columns[i].ColumnName + " = REPLACE(" + dtSource.Columns[i].ColumnName + ", '<|=-comma-=|>', ',')";
				}

				foreach (DataRow r in dtSource.Rows)
				{
					valueList = "";

					for (int i = 0; i < dtSource.Columns.Count; i++)
					{
						string val = r[i].ToString();

						if (valueList != "")
							valueList += ", ";

						val = val.Replace(",", "<|=-comma-=|>");

						if (dtSource.Columns[i].ColumnName == "INSERT_USER_ID")
						{
							val = "__COPY__";
						}
						else if (dtSource.Columns[i].ColumnName == "INSERT_DATE")
						{
							val = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						}
						else if (dtSource.Columns[i].ColumnName == "UPDATE_USER_ID")
						{
							val = "__COPY__";
						}
						else if (dtSource.Columns[i].ColumnName == "UPDATE_DATE")
						{
							val = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						}

						valueList += "'" + val + "'";
					}

					sql = "INSERT INTO " + pTableName + "(" + fieldList + ") VALUES (" + valueList + ")";

					pTarget.Execute(sql);
				}

				sql = "UPDATE " + pTableName + " SET " + updateList;

				pTarget.Execute(sql);

				return true;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
