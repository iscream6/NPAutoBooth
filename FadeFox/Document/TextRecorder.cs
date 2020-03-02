/*
 * ==============================================================================
 *   Program ID     :
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   :
 *   Developer      :
 *   Create Date    : 2009-11-05
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FadeFox.Document
{
	public class TextRecorder
	{
		StreamWriter mWriter = null;
		string mFileName = Guid.NewGuid().ToString().ToUpper() + ".txt";
		string mFilePath = FadeFoxCore.StartupPath;

		public TextRecorder()
		{

		}

		public string FileName
		{
			get { return mFileName; }
			set { mFileName = value; }
		}

		public string FilePath
		{
			get { return mFilePath; }
			set { mFilePath = value; }
		}

		public string FileFullPath
		{
			get { return Path.Combine(mFilePath, mFileName); }
			set
			{
				mFilePath = Path.GetDirectoryName(value);
				mFileName = Path.GetFileName(value);
			}
		}

		public void Open()
		{
			Close();

			if (!Directory.Exists(mFilePath))
			{
				Directory.CreateDirectory(mFilePath);
			}

			mWriter = new StreamWriter(Path.Combine(mFilePath, mFileName), true, Encoding.Default);
		}

		public void Write(string pString)
		{
			if (mWriter != null)
			{
				mWriter.Write(pString);
			}
		}

		public void WriteLine(string pString)
		{
			if (mWriter != null)
			{
				mWriter.WriteLine(pString);
			}
		}

		public void Close()
		{
			if (mWriter != null)
			{
				mWriter.Flush();
				mWriter.Close();
				mWriter.Dispose();
				mWriter = null;
			}
		}
	}
}
