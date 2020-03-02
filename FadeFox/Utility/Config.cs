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

namespace FadeFox.Utility
{
	public static class Config
	{
		private static string mFilePath = FadeFoxCore.StartupPath;
		private static string mFileName = "config.xml";
		private static string mFileFullPath = string.Empty;
		private static Rijndael mSecurity = new Rijndael();
		private static XmlDocument mXmlDoc = new XmlDocument();

		public static string FilePath
		{
			get { return mFilePath; }
			set
			{
				if (mFilePath != value)
				{
					mFilePath = value;
					Open();
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
					Open();
				}
			}
		}

		static Config()
		{
			Open();
		}

		private static void Open()
		{
			mFileFullPath = Path.Combine(mFilePath, mFileName);

			if (File.Exists(mFileFullPath))
			{
				mXmlDoc.Load(Path.Combine(mFilePath, mFileName));
			}
			else
			{
				string str = string.Empty;

				str = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
					+ "<configuration>"
					+ "<appSettings>"
					+ "</appSettings>"
					+ "</configuration>";

				mXmlDoc.LoadXml(str);
			}
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
			string value = string.Empty;

			XmlNode node = mXmlDoc.SelectSingleNode("configuration/appSettings/add[@key='" + pKeyName + "']");

			if (node != null)
			{
				value = node.Attributes["value"].Value;
			}
			else
			{
				SetValue(pKeyName, "");
				value = string.Empty;
			}

			return value;
		}

		public static void SetValueS(string pKeyName, string pValue)
		{
			string value = mSecurity.Encode(pValue);

			SetValue(pKeyName, value);
		}

		public static void SetValue(string pKeyName, string pValue)
		{
			XmlNode node = mXmlDoc.SelectSingleNode("configuration/appSettings/add[@key='" + pKeyName + "']");

			if (node != null)
			{
				node.Attributes["value"].Value = pValue;
			}
			else
			{
				XmlNode parentNode = mXmlDoc.SelectSingleNode("configuration/appSettings");

				if (parentNode != null)
				{
					XmlElement elem = mXmlDoc.CreateElement("add");
					XmlAttribute attKey = mXmlDoc.CreateAttribute("key");
					XmlAttribute attValue = mXmlDoc.CreateAttribute("value");
					attKey.Value = pKeyName;
					attValue.Value = pValue;

					elem.Attributes.Append(attKey);
					elem.Attributes.Append(attValue);
					parentNode.AppendChild(elem);
				}
			}

			XmlTextWriter w = new XmlTextWriter(mFileFullPath, Encoding.UTF8);
			w.Formatting = Formatting.Indented;
			w.Indentation = 1;
			w.IndentChar = '\t';
			mXmlDoc.Save(w);
			w.Flush();
			w.Close();
		}
	}
}
