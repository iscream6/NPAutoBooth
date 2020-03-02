/* 
 * ==============================================================================
 *   Program ID     : 
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : sayou.net
 *   Developer      : 
 *   Create Date    : 2007-05-23
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;

namespace FadeFox.Network.Tcp
{
	// 문자열 기반으로 데이터를 관리
	public class ClientData
	{
		private StringBuilder mClientData = new StringBuilder();
		private List<byte> mClientRawData = new List<byte>();
		private string mSeparator = "58";
		private int mAddLength = 0;
		private const int mBufferSize = 1024 * 32;  // 버퍼 크기
		public byte[] Buffer = new byte[mBufferSize]; // 버퍼

		// 데이터 기록
		/*
		public void Add(string pData)
		{
			mClientData.Append(pData);
			mAddLength = pData.Length;
		}
		*/

		// 버퍼로부터 바로 추가함.
		public void AddFromBuffer(int pLength)
		{
			mClientData.Append(Encoding.Default.GetString(this.Buffer, 0, pLength));

			for (int i = 0; i < pLength; i++)
			{
				mClientRawData.Add(this.Buffer[i]);
			}

			mAddLength = pLength;
		}

		// 데이터를 전부 클리어
		public void ClearAll()
		{
			mClientData.Remove(0, mClientData.Length);
			mClientRawData.Clear();
		}

		// 데이터를 문자열로 얻음
		public string GetString()
		{
			return mClientData.ToString();
		}

		public byte[] GetBytes()
		{
			return mClientRawData.ToArray();
		}

		// 구분 문자열
		public string Separator
		{
			get { return mSeparator; }
			set
			{
				mSeparator = value;
			}
		}

		// 버퍼 크기
		public int BufferSize
		{
			get { return mBufferSize; }
		}

		// 저장되어 있는 문자열에 구분문자열이 포함되어 있으면 
		// 한개의 데이터 셋을 전부 수신되었다고 봄, 만약
		// CheckSeparator가 설정되어 있지 않으면 항상 True 리턴
		public bool IsComplete
		{
			get
			{
				string content = mClientData.ToString();

				if (content.Length < mSeparator.Length)
					return false;
				else
				{
					string lastString = content.Substring(content.Length - mSeparator.Length, mSeparator.Length);

					if (lastString.Equals(mSeparator))
						return true;
					else
						return false;
				}
			}
		}

		// 데이터의 길이를 리턴
		public int Length
		{
			get
			{
				return mClientData.Length;
			}
		}

		// 새로 들어온 데이터의 길이
		public int AddLength
		{
			get { return mAddLength; }
		}
	}

	public delegate void ClientEventHandler(Client sender);
	public delegate void ServerEventHandler(TcpServer sender);

	public delegate void TcpClientEventHandler(TcpClient sender);

	// 클라이언트 정보.
	public class Client : TcpCore
	{
		#region 필드

		public Socket mConnection = null;  // 연결 객체
		private string mID = string.Empty;  // 구분 아이디
		public ClientData Data = new ClientData();
		private TcpClientStatus mStatus = TcpClientStatus.None;

		private ClientEventHandler mEventDataReceived;
		private ClientEventHandler mEventDataSend;
		private ClientEventHandler mEventConnected;
		private ClientEventHandler mEventDisconnected;
		private ClientEventHandler mEventError;

		private ManualResetEvent mConnectDone = new ManualResetEvent(false);
		private ManualResetEvent mSendDone = new ManualResetEvent(false);
		private ManualResetEvent mReceiveDone = new ManualResetEvent(false);

		#endregion

		#region 이벤트

		public event ClientEventHandler EventDataReceived
		{
			add
			{
				mEventDataReceived = (ClientEventHandler)System.Delegate.Combine(mEventDataReceived, value);
			}
			remove
			{
				mEventDataReceived = (ClientEventHandler)System.Delegate.Remove(mEventDataReceived, value);
			}
		}

		public event ClientEventHandler EventDataSend
		{
			add
			{
				mEventDataSend = (ClientEventHandler)System.Delegate.Combine(mEventDataSend, value);
			}
			remove
			{
				mEventDataSend = (ClientEventHandler)System.Delegate.Remove(mEventDataSend, value);
			}
		}

		public event ClientEventHandler EventConnected
		{
			add
			{
				mEventConnected = (ClientEventHandler)System.Delegate.Combine(mEventConnected, value);
			}
			remove
			{
				mEventConnected = (ClientEventHandler)System.Delegate.Remove(mEventConnected, value);
			}
		}

		public event ClientEventHandler EventDisconnected
		{
			add
			{
				mEventDisconnected = (ClientEventHandler)System.Delegate.Combine(mEventDisconnected, value);
			}
			remove
			{
				mEventDisconnected = (ClientEventHandler)System.Delegate.Remove(mEventDisconnected, value);
			}
		}

		public event ClientEventHandler EventError
		{
			add
			{
				mEventError = (ClientEventHandler)System.Delegate.Combine(mEventError, value);
			}
			remove
			{
				mEventError = (ClientEventHandler)System.Delegate.Remove(mEventError, value);
			}
		}

		#endregion

		#region 속성

		// 연결 객체
		public Socket Connection
		{
			get { return mConnection; }
			set
			{
				mConnection = value;
			}
		}

		/// <summary>
		/// 상태
		/// </summary>
		public TcpClientStatus Status
		{
			get { return mStatus; }
			set { mStatus = value; }
		}

		// 구분 아이디
		public string ID
		{
			get { return mID; }
			set
			{
				mID = value;
			}
		}

		#endregion

		#region 메소드

		// 접속 시작
		public void BeginConnect()
		{
			BeginConnect(mIPAddressString, mPort);
		}

		public void BeginConnect(string pIPAddressString, int pPort)
		{
			// Connect to the remote endpoint.

			if (mStatus == TcpClientStatus.Connected)
				return;

			IPAddress ipa = IPAddress.Parse(pIPAddressString);
			IPEndPoint ipEp = new IPEndPoint(ipa, pPort);

			mStatus = TcpClientStatus.Connecting;
			mMessage = "연결중입니다.";

			mConnection.BeginConnect(ipEp, new AsyncCallback(ConnectCallback), this);
			mConnectDone.WaitOne();
		}

		// 접속 완료
		private void ConnectCallback(IAsyncResult ar)
		{
			try
			{
				// Retrieve the socket from the state object.
				Client client = (Client)ar.AsyncState;

				// Complete the connection.
				client.Connection.EndConnect(ar);

				mStatus = TcpClientStatus.Connected;
				mMessage = "연결되었습니다.";

				if (mEventConnected != null)
					mEventConnected(client);

				// Signal that the connection has been made.
				mConnectDone.Set();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		public void Send(byte[] pContent)
		{
			if (mStatus != TcpClientStatus.Connected)
			{
				throw new Exception("서버와 연결되어 있지 않습니다.");
			}

			int size = mConnection.Send(pContent);

			mMessage = string.Format("{0}", size);
		}

		public void Send(string pContent)
		{
			if (mStatus != TcpClientStatus.Connected)
			{
				throw new Exception("서버와 연결되어 있지 않습니다.");
			}

			byte[] bytes = Encoding.Default.GetBytes(pContent);

			int size = mConnection.Send(bytes);

			mMessage = string.Format("{0}", size);
		}

		// 전송 시작
		public void BeginSend(String pData)
		{
			// Convert the string data to byte data using ASCII encoding.
			byte[] byteData = Encoding.ASCII.GetBytes(pData);

			// Begin sending the data to the remote device.
			mConnection.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(this.SendCallback), this);
		}

		// 전송 완료
		private void SendCallback(IAsyncResult ar)
		{
			try
			{
				// Retrieve the socket from the state object.
				Client client = (Client)ar.AsyncState;

				// Complete sending the data to the remote device.
				int bytesSent = client.Connection.EndSend(ar);
				mSendDateTime = DateTime.Now;

				if (mEventDataSend != null)
					mEventDataSend(client);
			}
			catch (Exception e)
			{
				MessageBox.Show("Error : " + e.Message, "TcpBase.SendCallback", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		// 수신 시작
		public void BeginReceive()
		{
			mConnection.BeginReceive(this.Data.Buffer, 0, this.Data.BufferSize, 0, new AsyncCallback(ReceiveCallback), this);
		}

		// 수신 완료시
		private void ReceiveCallback(IAsyncResult ar)
		{
			Client client = (Client)ar.AsyncState;

			try
			{
				// 클라이언트 소켓으로부터 읽음.
				int bytesRead = client.Connection.EndReceive(ar);
				client.ReceiveDateTime = DateTime.Now;

				if (bytesRead > 0)  // 전송받은 데이터가 있으면
				{
					client.Data.AddFromBuffer(bytesRead);

					// 이벤트 발생
					if (mEventDataReceived != null)
						mEventDataReceived(client);

					client.BeginReceive();
				}
				else
				{
					if (mEventDisconnected != null)
						mEventDisconnected(client);
				}
			}
			catch (ObjectDisposedException e)
			{
				mMessage = e.Message;
			}
			catch (Exception e)
			{
				mMessage = e.Message;
			}
		}

		// 연결 종료
		public void Disconnect()
		{
			mConnection.Shutdown(SocketShutdown.Both);
			mConnection.Close();
			mStatus = TcpClientStatus.Disconnected;
		}

		#endregion
	}

	// 클라이언트 정보 리스트
	public class ClientList
	{
		private Dictionary<string, Client> mClientList = new Dictionary<string, Client>();

		public Dictionary<string, Client> Clients
		{
			get 
			{ 
				return mClientList;
			}
		}

		// 전부 삭제
		public void Clear()
		{
			mClientList.Clear();
		}

		// 클라이언트 정보 입력
		public void Add(Client pClientInfo)
		{
			pClientInfo.ConnectDateTime = DateTime.Now;
			mClientList.Add(pClientInfo.ID, pClientInfo);
		}

		public Client Add(string pID, Socket pConnection)
		{
			Client clientInfo = new Client();

			clientInfo.Connection = pConnection;
			clientInfo.ID = pID;
			clientInfo.ConnectDateTime = DateTime.Now;

			mClientList.Add(clientInfo.ID, clientInfo);

			return clientInfo;
		}

		//
		// 기능 : 클라이언트 리스트에서 클라이언트 정보를 얻음.
		//
		public Client GetbyID(string pID)
		{
			Client clientInfo;

			if (mClientList.TryGetValue(pID, out clientInfo) == true)
			{
				return clientInfo;
			}
			else
			{
				return null;
			}
		}

		public void RemoveAll()
		{
			foreach (KeyValuePair<string, Client> kvp in mClientList)
			{
				kvp.Value.Disconnect();
			}

			mClientList.Clear();
		}

		public void Remove(string pKey)
		{
			mClientList.Remove(pKey);
		}
	}

	public enum TcpServerStatus
	{
		None,
		Error,
		Started,
		Waiting,
		Accepted,
		Disconnected
	}

	public enum TcpClientStatus
	{
		None,
		Error,
		Connecting,
		Connected,
		Disconnected
	}

	// TcpBase
	public class TcpCore
	{
		#region 필드

		protected string mIPAddressString = NetworkCore.GetIPAddressUsingWMI();
		protected int mPort = 1024;
		protected string mMessage = string.Empty;
		protected DateTime mConnectDateTime = DateTime.Parse("0001-01-01 00:00:00");   // 연결된 시간
		protected DateTime mDisconnectDateTime = DateTime.Parse("0001-01-01 00:00:00");  //연결 종료된 시간
		protected DateTime mReceiveDateTime = DateTime.Parse("0001-01-01 00:00:00");  // 마지막으로 수신받은 시간.
		protected DateTime mSendDateTime = DateTime.Parse("0001-01-01 00:00:00");  // 마지막으로 전송한 시간

		#endregion

		#region 속성

		public string IPAddressString
		{
			get { return mIPAddressString; }
			set
			{
				mIPAddressString = value;
			}
		}

		public int Port
		{
			get { return mPort; }
			set
			{
				mPort = value;
			}
		}

		public string Message
		{
			get { return mMessage; }
		}

		// 연결된 시간
		public DateTime ConnectDateTime
		{
			get { return mConnectDateTime; }
			set
			{
				mConnectDateTime = value;
			}
		}

		// 연결종료된 시간
		public DateTime DisconnectDateTime
		{
			get { return mDisconnectDateTime; }
			set
			{
				mDisconnectDateTime = value;
			}
		}

		// 수신 받은 시간
		public DateTime ReceiveDateTime
		{
			get { return mReceiveDateTime; }
			set
			{
				mReceiveDateTime = value;
			}
		}

		// 전송한 시간
		public DateTime SendDateTime
		{
			get { return mSendDateTime; }
			set
			{
				mSendDateTime = value;
			}
		}

		#endregion
	}
}
