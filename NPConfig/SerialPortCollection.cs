using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using FadeFox.Text;

namespace NPConfig
{
	public class SerialPortEx : SerialPort
	{
		private string mSerialPortID = "";
		public string SerialPortID
		{
			get { return mSerialPortID; }
			set { mSerialPortID = value; }
		}

		private string mSerialPortName = "";
		public string SerialPortName
		{
			get { return mSerialPortName; }
			set { mSerialPortName = value; }
		}

		private string mSerialPortComment = "";
		public string SerialPortComment
		{
			get { return mSerialPortComment; }
			set { mSerialPortComment = value; }
		}
	}
	
	public class SerialPortCollection
	{
		private Dictionary<string, SerialPortEx> mSerialPortList = new Dictionary<string, SerialPortEx>();

		public int Count
		{
			get { return mSerialPortList.Count; }
		}

		public SerialPortEx this[string pSerialPortID]
		{
			get
			{
				if (mSerialPortList.ContainsKey(pSerialPortID))
					return mSerialPortList[pSerialPortID];
				else
					return null;
			}
		}

		public void Write(string pSerialPortID, string pText)
		{
			if (Open(pSerialPortID))
			{
				mSerialPortList[pSerialPortID].Write(pText);
			}
		}

		public void Write(string pSerialPortID, byte[] pBuffer, int pOffset, int pCount)
		{
			if (Open(pSerialPortID))
				mSerialPortList[pSerialPortID].Write(pBuffer, pOffset, pCount);
		}

		public bool Open(string pSerialPortID)
		{
			try
			{
				if (mSerialPortList.ContainsKey(pSerialPortID))
				{
					if (!mSerialPortList[pSerialPortID].IsOpen)
						mSerialPortList[pSerialPortID].Open();

					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				return false;
			}
		}

		public bool Close(string pSerialPortID)
		{
			try
			{
				if (mSerialPortList.ContainsKey(pSerialPortID))
				{
					if (mSerialPortList[pSerialPortID].IsOpen)
						mSerialPortList[pSerialPortID].Close();

					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				return false;
			}
		}

		public void Clear()
		{
			CloseAll();
			mSerialPortList.Clear();
		}

		public void OpenAll()
		{
			foreach (SerialPortEx port in mSerialPortList.Values)
			{
				try
				{
					if (!port.IsOpen)
						port.Open();
				}
				catch
				{
					;
				}
			}
		}

		public void CloseAll()
		{
			foreach (SerialPortEx port in mSerialPortList.Values)
			{
				try
				{
					if (port.IsOpen)
						port.Close();
				}
				catch
				{
					;
				}
			}
		}

		public void Add(
			string pSerialPortID,
			string pSerialPortName,
			string pSerialPortComment,
			string pPortName,
			string pBaudRate,
			string pDataBits,
			string pParity,
			string pStopBits,
			string pHandShake)
		{
			SerialPortEx port = new SerialPortEx();
			port.PortName = pPortName;

			if (TextCore.IsInt(pBaudRate))
				port.BaudRate = Convert.ToInt32(pBaudRate);

			if (TextCore.IsInt(pDataBits))
				port.DataBits = Convert.ToInt32(pDataBits);

			switch (pParity.ToUpper())
			{
				case "NONE":
					port.Parity = Parity.None;
					break;
				case "ODD":
					port.Parity = Parity.Odd;
					break;
				case "EVEN":
					port.Parity = Parity.Even;
					break;
				default:
					port.Parity = Parity.None;
					break;
			}

			switch (pStopBits.ToUpper())
			{
				case "1":
					port.StopBits = StopBits.One;
					break;
				case "2":
					port.StopBits = StopBits.Two;
					break;
				default:
					port.StopBits = StopBits.One;
					break;
			}

			switch (pHandShake.ToUpper())
			{
				case "NONE":
					port.Handshake = Handshake.None;
					break;
				case "XON/XOFF":
					port.Handshake = Handshake.XOnXOff;
					break;
				case "RTS":
					port.Handshake = Handshake.RequestToSend;
					break;
				case "BOTH":
					port.Handshake = Handshake.RequestToSendXOnXOff;
					break;
				default:
					port.Handshake = Handshake.XOnXOff;
					break;
			}

			port.SerialPortID = pSerialPortID;
			port.SerialPortName = pSerialPortName;
			port.SerialPortComment = pSerialPortComment;

			mSerialPortList.Add(pSerialPortID, port);
		}
	}
}