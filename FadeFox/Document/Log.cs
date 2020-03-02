/*
 * ==============================================================================
 *   Program ID     :
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   :
 *   Developer      :
 *   Create Date    : 2009-08-06
 * ------------------------------------------------------------------------------
 *   Update History
 *       2011-07-05 : Static에서 일반 클래스로 변경
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FadeFox.Document
{
	public class Log
	{
		private string mFilePath = FadeFoxCore.StartupPath + "\\log";
		//
		// #date : 20070103
		// #time : 170000
		// #year : 2007
		// #month : 12
		// #day : 31
		// #hour : 11
		// #minute : 10
		// #second : 10
		// 나머지 그대로 출력
		//
		private string mFileNameFormat = "";
		private string mFileName = "";
		private StreamWriter mSw = null;

		public string FilePath
		{
			get { return mFilePath; }
			set
			{
				if (mFilePath != value)
				{
					mFilePath = value;
				}
			}
		}

		/// <summary>
		/// #date : 20070103
		/// #time : 170000
		/// #year : 2007
		/// #month : 12
		/// #day : 31
		/// #hour : 11
		/// #minute : 10
		/// #second : 10
		/// #millisecond : 333
		/// 나머지 그대로 출력
		/// </summary>
		public string FileNameFormat
		{
			get { return mFileNameFormat; }
			set
			{
				if (mFileNameFormat != value)
				{
					mFileNameFormat = value;
				}
			}
		}

		public string FileName
		{
			get { return mFileName; }
		}

		public void Open()
		{
			Close();

			if (!Directory.Exists(mFilePath))
			{
				Directory.CreateDirectory(mFilePath);
			}

			if (mFileNameFormat != string.Empty)
			{
				mFileName = BuildFileName(mFileNameFormat);
			}
			else
			{
				
				mFileName = BuildFileName("#date_#time_#millisecond.log");
			}

			mSw = new StreamWriter(mFilePath + "\\" + mFileName, true, Encoding.Default);
		}

		public void WriteHead(string pString)
		{
			if (mSw != null)
			{
				mSw.WriteLine("# " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + pString);
				mSw.Flush();
			}
		}

		public void WriteBody(string pString)
		{
			if (mSw != null)
			{
				mSw.WriteLine("  " + pString);
				mSw.Flush();
			}
		}

		public void WriteLine()
		{
			if (mSw != null)
			{
				mSw.WriteLine("********************************************************************************");
				mSw.Flush();
			}
		}

		public void Write(string pString)
		{
			if (mSw != null)
			{
				mSw.Write(pString);
				mSw.Flush();
			}
		}

		private string BuildFileName(string pFormat)
		{
			string fileName = pFormat;

			fileName = fileName.Replace("#date", DateTime.Now.ToString("yyyyMMdd"));
			fileName = fileName.Replace("#time", DateTime.Now.ToString("HHmmss"));
			fileName = fileName.Replace("#year", DateTime.Now.ToString("yyyy"));
			fileName = fileName.Replace("#month", DateTime.Now.ToString("MM"));
			fileName = fileName.Replace("#day", DateTime.Now.ToString("dd"));
			fileName = fileName.Replace("#hour", DateTime.Now.ToString("HH"));
			fileName = fileName.Replace("#minute", DateTime.Now.ToString("mm"));
			fileName = fileName.Replace("#second", DateTime.Now.ToString("ss"));
			fileName = fileName.Replace("#millisecond", DateTime.Now.ToString("fff"));

			return fileName;
		}

		public void Close()
		{
			if (mSw != null)
			{
				mSw.Flush();
				mSw.Close();
				mSw.Dispose();
				mSw = null;
			}
		}
	}
}
