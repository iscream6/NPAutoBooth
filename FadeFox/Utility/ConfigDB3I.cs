/*
 * ==============================================================================
 *   Program ID     :
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   :
 *   Developer      :
 *   Create Date    : 2011-05-12
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using FadeFox.Security;
using FadeFox.Database.SQLite;
using System.IO;

namespace FadeFox.Utility
{
	public class ConfigDB3I
	{
		private string mFilePath = FadeFoxCore.StartupPath;
		private string mFileName = "config.db3";
		private string mFileFullPath = string.Empty;
		private bool mIsKeepOpened = false;
		private Rijndael mSecurity = new Rijndael();
		private SQLite mConfig = new SQLite();

		public string FilePath
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

		public string FileName
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

		public ConfigDB3I()
		{
			InitializeEnvironment();
		}

		public ConfigDB3I(string pFileFullPath)
		{
			mFileName = Path.GetFileName(pFileFullPath);
			mFilePath = Path.GetDirectoryName(pFileFullPath);
			mFileFullPath = pFileFullPath;

			InitializeEnvironment();	
		}

		public void Open()
		{
			_Open();
			mIsKeepOpened = true;
		}

		public void Close()
		{
			mIsKeepOpened = false;
			_Close();
		}

		private void _Open()
		{
			if (!mIsKeepOpened)
			{
				mConfig.Database = mFileFullPath;
				mConfig.Connect();
			}
		}

		private void _Close()
		{
			if (!mIsKeepOpened)
			{
				mConfig.Disconnect();
			}
		}

		private void InitializeEnvironment()
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

		public string GetValueS(string pKeyName)
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

		public string GetValue(string pKeyName)
		{
			string configValue = "";

			string sql = "";

			sql = "SELECT CONFIG_VALUE FROM ST_CONFIG WHERE CONFIG_ID = '" + pKeyName + "'";

			_Open();

			configValue = mConfig.SelectC(sql);

			_Close();

			return configValue;
		}

		public void SetValueS(string pKeyName, string pValue)
		{
			string value = mSecurity.Encode(pValue);

			SetValue(pKeyName, value);
		}

		public void SetValue(string pKeyName, string pValue)
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
