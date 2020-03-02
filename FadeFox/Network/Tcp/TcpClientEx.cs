/*
 * ==============================================================================
 *   Program ID     :
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 *       메시지 요청후 요청 도달 까지 딜레이 시키는 방식으로 마치 함수 호출하는 것처럼
 *       원격지 tcp에 접속해서 데이터를 가지고 오는 방식으로 처리
 * ------------------------------------------------------------------------------
 *   Company Name   :
 *   Developer      :
 *   Create Date    : 2011-07-21
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace FadeFox.Network.Tcp
{
	public enum TcpClientExReturnType
	{
		None,
		String,
		Byte,
		StringAndByte
	}

	public enum TcpClientExStatus
	{
		None,
		Success,
		TimeOut,
		Error,
		NotConnected
	}

	public class TcpClientExResult
	{
		/// <summary>
		/// 상태
		/// </summary>
		public TcpClientExStatus Status
		{
			get;
			set;
		}

		/// <summary>
		/// 메시지
		/// </summary>
		public string Message
		{
			get;
			set;
		}

		/// <summary>
		/// 리턴타입
		/// </summary>
		public TcpClientExReturnType ReturnType
		{
			get;
			set;
		}

		/// <summary>
		/// 문자열 데이터
		/// </summary>
		public string StringData
		{
			get;
			set;
		}

		/// <summary>
		/// 바이트데이터
		/// </summary>
		public byte[] ByteData
		{
			get;
			set;
		}

		/// <summary>
		/// 초기화
		/// </summary>
		public void Clear()
		{
			StringData = "";
			ByteData = null;
			ReturnType = TcpClientExReturnType.None;
			Status = TcpClientExStatus.None;
			Message = "";
		}

		public TcpClientExResult()
		{
			Clear();
		}
	}

	public class TcpClientEx
	{
		TcpClient mTcpClient = new TcpClient();
		TcpClientExStatus mStatus = TcpClientExStatus.None;
		string mMessage = "";
		byte[] mReceiveData = null;

		public TcpClientEx()
		{
			mTcpClient.EventDataReceived += new TcpClientEventHandler(TcpClient_DataReceived);
			mTcpClient.EventError += new TcpClientEventHandler(TcpClient_EventError);
		}

		public TcpClientEx(string pIP, int pPort) : this()
		{
			mTcpClient.IPAddressString = pIP;
			mTcpClient.Port = pPort;
			UsingSTXETX = false;
		}

		private int mTimeOut = 10000;  // 10초 디폴트, 응답 데이터 가져 오는데 시간 오래 걸림
		public int TimeOut
		{
			get { return mTimeOut; }
			set { mTimeOut = value; }
		}

		/// <summary>
		/// 시작, 종료 문자 사용여부
		/// </summary>
		public bool UsingSTXETX
		{
			get;
			set;
		}

		/// <summary>
		/// IP
		/// </summary>
		public string IPAddress
		{
			get { return mTcpClient.IPAddressString; }
			set { mTcpClient.IPAddressString = value; }
		}

		/// <summary>
		/// 포트
		/// </summary>
		public int Port
		{
			get { return mTcpClient.Port; }
			set { mTcpClient.Port = value; }
		}

		/// <summary>
		/// 연결
		/// </summary>
		public void Connect()
		{
			mTcpClient.Connect();
		}

		/// <summary>
		/// 연결해제
		/// </summary>
		public void Disconnect()
		{
			mTcpClient.Disconnect();
		}

		private enum ProtocolStep
		{
			Ready,
			SendCommand,
			ReceiveData
		}

		ProtocolStep mStep = ProtocolStep.Ready;

		public TcpClientExResult Send(string pData, TcpClientExReturnType pReturnType)
		{
			return Send(ConvertToByte(pData, UsingSTXETX), pReturnType);
		}

		public TcpClientExResult Send(byte[] pData, TcpClientExReturnType pReturnType)
		{
			TcpClientExResult result = new TcpClientExResult();
			result.ReturnType = pReturnType;

			if (mTcpClient.Status != TcpClientStatus.Connected)
			{
				result.Message = "연결되어 있지 않습니다..";
				result.Status = TcpClientExStatus.NotConnected;

				return result;
			}

			while (mStep != ProtocolStep.Ready)
			{
				;
			}

			mStep = ProtocolStep.SendCommand;

			mMessage = "";
			mStatus = TcpClientExStatus.None;
			mReceiveData = null;

			mTcpClient.Send(pData);

			// 에러가 발생할 경우
			if (mStatus == TcpClientExStatus.Error)
			{
				mStep = ProtocolStep.Ready;

				result.Status = TcpClientExStatus.Error;
				result.Message = mMessage;

				return result;
			}

			// 결과가 필요 없을시
			if (pReturnType == TcpClientExReturnType.None)
			{
				mStep = ProtocolStep.Ready;

				result.Status = TcpClientExStatus.Success;
				result.Message = "성공";

				return result;
			}

			DateTime startDate = DateTime.Now;

			while (mStep != ProtocolStep.Ready)
			{
				TimeSpan diff = DateTime.Now - startDate;

				if (Convert.ToInt32(diff.TotalMilliseconds) > mTimeOut)
				{
					mStep = ProtocolStep.Ready;

					result.Status = TcpClientExStatus.TimeOut;
					result.Message = "시간만료";

					return result;
				}
			}

			result.Status = TcpClientExStatus.Success;
			result.Message = "성공";

			switch (pReturnType)
			{
				case TcpClientExReturnType.Byte:
					result.ByteData = mReceiveData;
					break;
				case TcpClientExReturnType.String:
					result.StringData = ConvertToString(mReceiveData);
					break;
				case TcpClientExReturnType.StringAndByte:
					result.ByteData = mReceiveData;
					result.StringData = ConvertToString(mReceiveData);
					break;
			}

			return result;
		}

		void TcpClient_EventError(TcpClient sender)
		{
			mStatus = TcpClientExStatus.Error;
			mMessage = sender.Message;
			mStep = ProtocolStep.Ready;
		}

		private void TcpClient_DataReceived(TcpClient sender)
		{
			mStep = ProtocolStep.ReceiveData;

			mReceiveData = sender.Data.GetBytes();

			sender.Data.ClearAll();

			mStatus = TcpClientExStatus.Success;
			mMessage = "성공";

			mStep = ProtocolStep.Ready;
		}

		private string ConvertToString(byte[] pSource)
		{
			List<byte> result = new List<byte>();

			foreach (byte b in pSource)
			{
				if (b == 0x02 || b == 0x03) // STX, ETX
					continue;

				result.Add(b);
			}

			byte[] r2 = result.ToArray();

			return Encoding.Default.GetString(r2);
		}

		private byte[] ConvertToByte(string pString, bool pUsingSTXETX)
		{
			List<byte> result = new List<byte>();
			byte[] source = Encoding.Default.GetBytes(pString);

			result.Add(0x02);

			foreach (byte b in source)
			{
				result.Add(b);
			}

			result.Add(0x03);

			return result.ToArray();
		}
	}
}
