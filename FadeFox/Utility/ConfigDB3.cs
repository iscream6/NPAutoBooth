/*
 * ==============================================================================
 *   Program ID     :
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   :
 *   Developer      :
 *   Create Date    : 2011-01-13
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using FadeFox.Security;
using FadeFox.Database.SQLite;

namespace FadeFox.Utility
{
	public static class ConfigDB3
	{
		private static string mFilePath = FadeFoxCore.StartupPath;
		private static string mFileName = "config.db3";
		private static string mFileFullPath = string.Empty;
		private static bool mIsKeepOpened = false;
		private static Rijndael mSecurity = new Rijndael();
		private static SQLite mConfig = new SQLite();

		public static string FilePath
		{
			get { return mFilePath; }
			set
			{
				if (mFilePath != value)
				{
					mFilePath = value;
					mFileFullPath = Path.Combine(mFilePath, mFileName);
				}
			}
		}

		public static string FileName
		{
			get { return mFileName; }
			set
			{
				if (mFileName != value)
				{
					mFileName = value;
					mFileFullPath = Path.Combine(mFilePath, mFileName);
				}
			}
		}

		static ConfigDB3()
		{
			InitializeEnvironment();
		}

		public static void Open()
		{
			_Open();
			mIsKeepOpened = true;
		}

		public static void Close()
		{
			mIsKeepOpened = false;
			_Close();
		}

		private static void _Open()
		{
			if (!mIsKeepOpened)
			{
				mConfig.Database = mFileFullPath;
				mConfig.Connect();
			}
		}

		private static void _Close()
		{
			if (!mIsKeepOpened)
			{
				mConfig.Disconnect();
			}
		}

		private static void InitializeEnvironment()
		{
			mFileFullPath = Path.Combine(mFilePath, mFileName);

			_Open();

			string sql = "";

			if (!mConfig.IsExistTable("ST_CONFIG"))
			{
				sql = "  CREATE TABLE ST_CONFIG ("
					+ "  	CONFIG_ID NVARCHAR(50) NOT NULL, "
					+ "  	CONFIG_VALUE NNVARCHAR(500) DEFAULT '',"
					+ "  	CONSTRAINT PK_ST_CONFIG PRIMARY KEY (CONFIG_ID)"
					+ "  );";

				mConfig.Execute(sql);
			}

			_Close();
		}

		public static string GetValueS(string pKeyName)
		{
			string value = GetValue(pKeyName);

			if (value == string.Empty)
			{
				return string.Empty;
			}
			else
			{
				try
				{
					return mSecurity.Decode(value);
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public static string GetValue(string pKeyName)
		{
			string configValue = "";

			string sql = "";

			sql = "SELECT CONFIG_VALUE FROM ST_CONFIG WHERE CONFIG_ID = '" + pKeyName + "'";

			_Open();

			configValue = mConfig.SelectC(sql);

			_Close();

			return configValue;
		}

		public static void SetValueS(string pKeyName, string pValue)
		{
			string value = mSecurity.Encode(pValue);

			SetValue(pKeyName, value);
		}

		public static void SetValue(string pKeyName, string pValue)
		{
			_Open();

			string sql = "";

			sql = "SELECT COUNT(*) FROM ST_CONFIG WHERE CONFIG_ID = '" + pKeyName + "'";

			string count = mConfig.SelectC(sql);

			if (count == "0")
			{
				sql = "  INSERT INTO ST_CONFIG(CONFIG_ID, CONFIG_VALUE)"
					+ "  VALUES ('" + pKeyName + "', '" + pValue + "')";
			}
			else
			{
				sql = "  UPDATE ST_CONFIG SET"
					+ "         CONFIG_VALUE = '" + pValue + "'"
					+ "   WHERE CONFIG_ID = '" + pKeyName + "'";
			}

			mConfig.Execute(sql);

			_Close();
		}
	}
}
