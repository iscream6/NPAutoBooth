using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;

namespace FadeFox.Network.Tcp
{
	public class TcpServer : TcpCore
	{
		#region 필드

		private Socket mListenSocket = null;
		private TcpServerStatus mStatus = TcpServerStatus.None;
		private Thread mThread = null;
		private bool mCancel = false;
		private ClientList mClientList = new ClientList();
		private int mConnectionCount = 0;

		private ServerEventHandler mEventStarted;
		private ServerEventHandler mEventStoped;
		private ServerEventHandler mEventWaiting;
		private ServerEventHandler mEventError;
		private ClientEventHandler mEventAccepted;
		private ClientEventHandler mEventDataReceived;
		private ClientEventHandler mEventDataSend;
		private ClientEventHandler mEventDisconnected;

		// Thread signal.
		public ManualResetEvent acceptDone = new ManualResetEvent(false);

		#endregion

		#region 생성자

		public TcpServer()
		{
		}

		#endregion

		#region 이벤트

		public event ServerEventHandler EventStarted
		{
			add
			{
				mEventStarted = (ServerEventHandler)System.Delegate.Combine(mEventStarted, value);
			}
			remove
			{
				mEventStarted = (ServerEventHandler)System.Delegate.Remove(mEventStarted, value);
			}
		}

		public event ServerEventHandler EventStoped
		{
			add
			{
				mEventStoped = (ServerEventHandler)System.Delegate.Combine(mEventStoped, value);
			}
			remove
			{
				mEventStoped = (ServerEventHandler)System.Delegate.Remove(mEventStoped, value);
			}
		}

		public event ServerEventHandler EventWaiting
		{
			add
			{
				mEventWaiting = (ServerEventHandler)System.Delegate.Combine(mEventWaiting, value);
			}
			remove
			{
				mEventWaiting = (ServerEventHandler)System.Delegate.Remove(mEventWaiting, value);
			}
		}

		public event ServerEventHandler EventError
		{
			add
			{
				mEventError = (ServerEventHandler)System.Delegate.Combine(mEventError, value);
			}
			remove
			{
				mEventError = (ServerEventHandler)System.Delegate.Remove(mEventError, value);
			}
		}

		public event ClientEventHandler EventAccepted
		{
			add
			{
				mEventAccepted = (ClientEventHandler)System.Delegate.Combine(mEventAccepted, value);
			}
			remove
			{
				mEventAccepted = (ClientEventHandler)System.Delegate.Remove(mEventAccepted, value);
			}
		}

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

		#endregion

		#region 속성

		public TcpServerStatus Status
		{
			get { return mStatus; }
		}

		/// <summary>
		/// 현재 서버가 작동중인지 아닌지 여부 
		/// </summary>
		public bool IsBusy
		{
			get
			{
				if (mThread == null)
				{
					return false;
				}
				else
				{
					return mThread.IsAlive;
				}
			}
		}

		/// <summary>
		/// 연결 수
		/// </summary>
		public int ConnectionCount
		{
			get { return mConnectionCount; }
		}

		/// <summary>
		/// 연결된 클라이언트 들
		/// </summary>
		public Dictionary<string, Client> Clients
		{
			get { return mClientList.Clients; }
		}

		#endregion

		#region 메소드

		public void Start()
		{
			try
			{
				if (IsBusy == false)
				{
					mThread = new Thread(new ThreadStart(Run));
					mThread.IsBackground = true;
					mCancel = false;
					mThread.Start();

					if (mEventStarted != null)
						this.mEventStarted(this);
				}
				else
				{
					mStatus = TcpServerStatus.Error;
					mMessage = "이미 시작되었습니다.";

					if (mEventError != null)
						mEventError(this);
				}
			}
			catch (Exception ex)
			{
				mStatus = TcpServerStatus.Error;
				mMessage = ex.Message;

				if (mEventError != null)
					mEventError(this);
			}
		}

		private void Run()
		{
			mListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

			try
			{
				mListenSocket.Bind(new IPEndPoint(IPAddress.Parse(mIPAddressString), mPort));
				mListenSocket.Listen(100);

				while (true)
				{
					// Set the event to nonsignaled state.
					acceptDone.Reset();

					// Start an asynchronous socket to listen for connections.
					mStatus = TcpServerStatus.Waiting;
					mMessage = string.Format("클라이언트를 기다리는 중입니다. (포트:{0})", mPort);

					if (mEventWaiting != null)
						mEventWaiting(this);

					mListenSocket.BeginAccept(new AsyncCallback(AcceptCallback), mListenSocket);

					// Wait until a connection is made before continuing.
					acceptDone.WaitOne();

					if (mCancel == true)
					{
						mListenSocket.Close();

						mClientList.RemoveAll();

						if (mEventStoped != null)
							mEventStoped(this);

						break;
					}
				}
			}
			catch (Exception e)
			{
				mMessage = e.Message;

				if (mEventError != null)
					mEventError(this);

				if (mEventStoped != null)
					mEventStoped(this);
			}
		}


		// 서버 정지
		public void Stop()
		{
			mClientList.RemoveAll();
			mConnectionCount = 0;
			mCancel = true;
			acceptDone.Set();
		}


		// 클라이언트 접속시
		public void AcceptCallback(IAsyncResult ar)
		{
			// Signal the main thread to continue.
			acceptDone.Set();

			// Get the socket that handles the client request.
			Socket listener = (Socket)ar.AsyncState;

			try
			{
				Socket handler = listener.EndAccept(ar);

				mConnectionCount++;

				// 클라이언트 리스트에 등록
				Client client = mClientList.Add(mConnectionCount.ToString(), handler);
				
				client.EventDataSend += new ClientEventHandler(this.OnDataSend);
				client.EventDataReceived += new ClientEventHandler(this.OnDataReceived);
				client.EventDisconnected += new ClientEventHandler(this.OnDisconnected);
				client.Status = TcpClientStatus.Connected;

				mMessage = "연결되었습니다.";
				mStatus = TcpServerStatus.Accepted;

				if (mEventAccepted != null)
					mEventAccepted(client);

				client.BeginReceive();
			}
			catch (ObjectDisposedException e)  // 소켓이 닫힌 경우
			{
				mMessage = e.Message;
			}
			catch (Exception e)
			{
				mMessage = e.Message;

				if (mEventError != null)
					mEventError(this);
			}
		}

		// 클라이언트로 부터 데이터를 전송하면.
		private void OnDataSend(Client pClient)
		{
			if (mEventDataSend != null)
				mEventDataSend(pClient);
		}

		// 클라이언트로 부터 데이터를 다 받으면...
		private void OnDataReceived(Client pClient)
		{
			if (mEventDataReceived != null)
				mEventDataReceived(pClient);
		}

		// 클라이언트 종료
		private void OnDisconnected(Client pClient)
		{
			mConnectionCount--;

			pClient.Disconnect();

			if (mEventDisconnected != null)
				mEventDisconnected(pClient);

			mClientList.Remove(pClient.ID);
		}

		#endregion
	}
}
