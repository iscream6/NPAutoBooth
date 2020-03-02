using System;
using System.Collections.Generic;
using System.Text;
using FadeFox.Security;
using System.Xml;
using System.IO;

namespace FadeFox.Utility
{
	public class ConfigI
	{
		private string mFilePath = FadeFoxCore.StartupPath;
		private string mFileName = "config.xml";
		private string mFileFullPath = string.Empty;
		private Rijndael mSecurity = new Rijndael();
		private XmlDocument mXmlDoc = new XmlDocument();

		public string FilePath
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

		public string FileName
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

		public ConfigI()
		{
			Open();
		}

		public ConfigI(string pFileFullPath)
		{
			mFileName = Path.GetFileName(pFileFullPath);
			mFilePath = Path.GetDirectoryName(pFileFullPath);
			mFileFullPath = pFileFullPath;

			Open();
		}

		private void Open()
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

		public void SetValueS(string pKeyName, string pValue)
		{
			string value = mSecurity.Encode(pValue);

			SetValue(pKeyName, value);
		}

		public void SetValue(string pKeyName, string pValue)
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
