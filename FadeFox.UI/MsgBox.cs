/*
 * ==============================================================================
 *   Program ID     : 
 *   Program Name   : 
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : 
 *   Developer      :
 *   Create Date    : 2011-07-04
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using FadeFox.Utility;

namespace FadeFox.UI
{
	public class MsgBox
	{
		public static DialogResult Show(Result pResult)
		{
			MsgType msgType = MsgType.Normal;
			string title = string.Empty;

			if (pResult.Success)
			{
				title = "정보";
				msgType = MsgType.Information;
			}
			else
			{
				title = "오류";
				msgType = MsgType.Error;
			}

			return Show(pResult.Message, title, msgType);
		}

		public static DialogResult Show(string pMessage)
		{
			return Show(pMessage, MsgType.Normal);
		}

		public static DialogResult Show(MsgItem pMsgItem)
		{
			return Show(pMsgItem.MessageContent, pMsgItem.MessageType);
		}

		public static DialogResult Show(MsgItem pMsgItem, string pContentExtra)
		{
			return Show(pMsgItem.MessageContent + pContentExtra, pMsgItem.MessageType);
		}

		public static DialogResult Show(Exception pEx)
		{
			return Show(pEx.Message, MsgType.Error);
		}

		public static DialogResult Show(string pMessage, MsgType pType)
		{
			string title = string.Empty;

			switch (pType)
			{
				case MsgType.Error:
					title = "오류";
					break;
				case MsgType.Information:
					title = "정보";
					break;
				case MsgType.Normal:
					title = "메시지";
					break;
				case MsgType.Question:
					title = "질문";
					break;
				case MsgType.Warning:
					title = "주의";
					break;
			}

			return Show(pMessage, title, pType);
		}

		public static DialogResult Show(string pMessage, string pTitle, MsgType pType)
		{
			MsgBoxForm frm = new MsgBoxForm(pMessage, pTitle, pType);

			return frm.ShowDialog();
		}
	}
}
