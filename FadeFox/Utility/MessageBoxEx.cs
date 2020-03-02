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
using System.Windows.Forms;

namespace FadeFox.Utility
{
	//
	// 메시지 종류 구분.
	//
	public enum MsgType
	{
		Normal,
		Information,
		Warning,
		Question,
		Error
	}

	/// <summary>
	/// Message Item
	/// </summary>
	public class MsgItem
	{
		private string mMessageID = string.Empty;
		public string MessageID
		{
			get { return mMessageID; }
			set { mMessageID = value; }
		}

		private MsgType mMessageType = MsgType.Normal;
		public MsgType MessageType
		{
			get { return mMessageType; }
			set { mMessageType = value; }
		}

		private string mMessageContent = string.Empty;
		public string MessageContent
		{
			get { return mMessageContent; }
			set { mMessageContent = value; }
		}

		private string mMessageComment = string.Empty;
		public string MessageComment
		{
			get { return mMessageComment; }
			set { mMessageComment = value; }
		}

		public MsgItem()
		{
		}

		public MsgItem(string pMessageID, MsgType pMessageType, string pMessageContent, string pMessageComment)
		{
			mMessageID = pMessageID;
			mMessageType = pMessageType;
			mMessageContent = pMessageContent;
			mMessageComment = pMessageComment;
		}
	}

	public class Msg
	{
		#region "    Field    "

		private List<MsgItem> mMsgList = new List<MsgItem>();

		#endregion

		#region "    Property    "

		public int Count
		{
			get
			{
				return mMsgList.Count;
			}
		}

		// 
		// 인덱서
		//
		public MsgItem this[string pMessageID]
		{
			get
			{
				if (mMsgList.Count > 0)
				{
					for (int i = 0; i < mMsgList.Count; i++)
					{
						if (mMsgList[i].MessageID == pMessageID)
							return mMsgList[i];
					}

					return new MsgItem();
				}
				else
				{
					return new MsgItem();
				}
			}
		}

		public MsgItem this[int pIndex]
		{
			get
			{
				if (mMsgList.Count > 0)
					return mMsgList[pIndex];
				else
					return new MsgItem();
			}
		}

		#endregion

		#region 생성자


		#endregion

		#region "    Method    "

		// 리스트 전부 삭제
		public void Clear()
		{
			mMsgList.Clear();
		}

		public void Add(MsgItem pMsgItem)
		{
			mMsgList.Add(pMsgItem);
		}

		public MsgItem Add(string pMessageID, MsgType pMessageType, string pMessageContent, string pMessageComment)
		{
			MsgItem mi = new MsgItem(pMessageID, pMessageType, pMessageContent, pMessageComment);

			mMsgList.Add(mi);

			return mi;
		}

		#endregion
	}

	public static class MessageBoxEx
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
					title = string.Empty;
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
			DialogResult result = DialogResult.OK;

			switch (pType)
			{
				case MsgType.Error:
					MessageBox.Show(pMessage + "\n\n관리자에게 문의하여 주세요.", pTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
					result = DialogResult.OK;
					break;
				case MsgType.Information:
					MessageBox.Show(pMessage, pTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
					result = DialogResult.OK;
					break;
				case MsgType.Warning:
					MessageBox.Show(pMessage, pTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					result = DialogResult.OK;
					break;
				case MsgType.Normal:
					MessageBox.Show(pMessage, pTitle, MessageBoxButtons.OK, MessageBoxIcon.None);
					result = DialogResult.OK;
					break;
				case MsgType.Question:
					result = MessageBox.Show(pMessage, pTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					break;
			}

			return result;
		}
	}
}
