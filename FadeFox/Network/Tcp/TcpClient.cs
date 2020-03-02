using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;

namespace FadeFox.Network.Tcp
{
	public class TcpClient : TcpCore
	{
		#region 필드

		private Socket mConnection = null;
		public ClientData Data = new ClientData();
		TcpClientStatus mStatus = TcpClientStatus.None;

		private TcpClientEventHandler mEventDataReceived;
		private TcpClientEventHandler mEventNotDataReceived;
		private TcpClientEventHandler mEventDataSend;
		private TcpClientEventHandler mEventConnected;
		private TcpClientEventHandler mEventNotConnected;
		private TcpClientEventHandler mEventDisconnected;
		private TcpClientEventHandler mEventError;

		private ManualResetEvent mConnectDone = new ManualResetEvent(false);
		private ManualResetEvent mSendDone = new ManualResetEvent(false);
		private ManualResetEvent mReceiveDone = new ManualResetEvent(false);

		#endregion

		#region 이벤트

		public event TcpClientEventHandler EventDataReceived
		{
			add
			{
				mEventDataReceived = (TcpClientEventHandler)System.Delegate.Combine(mEventDataReceived, value);
			}
			remove
			{
				mEventDataReceived = (TcpClientEventHandler)System.Delegate.Remove(mEventDataReceived, value);
			}
		}

		public event TcpClientEventHandler EventNotDataReceived
		{
			add
			{
				mEventNotDataReceived = (TcpClientEventHandler)System.Delegate.Combine(mEventNotDataReceived, value);
			}
			remove
			{
				mEventNotDataReceived = (TcpClientEventHandler)System.Delegate.Remove(mEventNotDataReceived, value);
			}
		}

		public event TcpClientEventHandler EventDataSend
		{
			add
			{
				mEventDataSend = (TcpClientEventHandler)System.Delegate.Combine(mEventDataSend, value);
			}
			remove
			{
				mEventDataSend = (TcpClientEventHandler)System.Delegate.Remove(mEventDataSend, value);
			}
		}

		public event TcpClientEventHandler EventConnected
		{
			add
			{
				mEventConnected = (TcpClientEventHandler)System.Delegate.Combine(mEventConnected, value);
			}
			remove
			{
				mEventConnected = (TcpClientEventHandler)System.Delegate.Remove(mEventConnected, value);
			}
		}

		public event TcpClientEventHandler EventNotConnected
		{
			add
			{
				mEventNotConnected = (TcpClientEventHandler)System.Delegate.Combine(mEventNotConnected, value);
			}
			remove
			{
				mEventNotConnected = (TcpClientEventHandler)System.Delegate.Remove(mEventNotConnected, value);
			}
		}

		public event TcpClientEventHandler EventDisconnected
		{
			add
			{
				mEventDisconnected = (TcpClientEventHandler)System.Delegate.Combine(mEventDisconnected, value);
			}
			remove
			{
				mEventDisconnected = (TcpClientEventHandler)System.Delegate.Remove(mEventDisconnected, value);
			}
		}

		public event TcpClientEventHandler EventError
		{
			add
			{
				mEventError = (TcpClientEventHandler)System.Delegate.Combine(mEventError, value);
			}
			remove
			{
				mEventError = (TcpClientEventHandler)System.Delegate.Remove(mEventError, value);
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

		public TcpClientStatus Status
		{
			get { return mStatus; }
		}

		#endregion

		public void Connect()
		{
			Connect(mIPAddressString, mPort);
		}

		public void Connect(string pIPAddressString, int pPort)
		{
			if (mStatus == TcpClientStatus.Connecting ||
				mStatus == TcpClientStatus.Connected)
				return;

			mStatus = TcpClientStatus.Connecting;
			mMessage = "연결중입니다.";

			// ClientSocket를 생성합니다.
			mConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
			// connection socket.

			IPAddress ipa = IPAddress.Parse(pIPAddressString);

			mConnection.Connect(new IPEndPoint(ipa, pPort));

			mStatus = TcpClientStatus.Connected;
			mMessage = "연결되었습니다.";

			this.BeginReceive();

			if (mEventConnected != null)
				mEventConnected(this);
		}

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
				TcpClient client = (TcpClient)ar.AsyncState;

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

		public void Disconnect()
		{
			if (mConnection != null)
			{
				if (mConnection.Connected == true)
				{
					mConnection.Shutdown(SocketShutdown.Both);
					mConnection.Close();
					mConnection = null;
					mStatus = TcpClientStatus.Disconnected;
					mMessage = "연결이 종료되었습니다.";

					if (mEventDisconnected != null)
						mEventDisconnected(this);
				}
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

		public void Receive()
		{
			try
			{
				if (mConnection != null)
				{
					// 클라이언트 소켓으로부터 읽음.
					int bytesRead = mConnection.Receive(this.Data.Buffer);
					this.ReceiveDateTime = DateTime.Now;

					if (bytesRead > 0)  // 전송받은 데이터가 있으면
					{
						this.Data.AddFromBuffer(bytesRead);

						// 이벤트 발생
						if (mEventDataReceived != null)
							mEventDataReceived(this);
					}
					else
					{
						if (mEventNotDataReceived != null)
							mEventNotDataReceived(this);
					}
				}
				else
				{
					if (mEventNotConnected!= null)
						mEventNotConnected(this);
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

		public void BeginSend(string pContent)
		{
			if (mStatus != TcpClientStatus.Connected)
			{
				throw new Exception("서버와 연결되어 있지 않습니다.");
			}

			byte[] bytes = Encoding.Default.GetBytes(pContent);

			mConnection.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(SendCallBack), bytes);
		}

		private void SendCallBack(IAsyncResult pAr)
		{
			byte[] bytes = (byte[])pAr.AsyncState;

			int size = mConnection.EndSend(pAr);

			//MessageBox.Show(string.Format("{0}", size));
			mMessage = string.Format("{0}", size);
		}

		// 수신 시작
		public void BeginReceive()
		{
			mConnection.BeginReceive(this.Data.Buffer, 0, this.Data.BufferSize, 0, new AsyncCallback(ReceiveCallback), this);
		}

		// 수신 완료시
		private void ReceiveCallback(IAsyncResult ar)
		{
			TcpClient client = (TcpClient)ar.AsyncState;

			try
			{
				if (client.Connection != null)
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
						if (mEventNotDataReceived!= null)
							mEventNotDataReceived(client);
					}
				}
				else
				{
					if (mEventNotConnected != null)
						mEventNotConnected(client);
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
	}
}
