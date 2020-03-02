/* 
 * ==============================================================================
 *   Program ID     : 
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : sayou.net
 *   Developer      : 
 *   Create Date    : 2011-05-11
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using FadeFox.Text;
using System.ComponentModel;

namespace FadeFox.IO
{
	public enum SerialPortDataType
	{
		None,
		Error,
		SendData,
		ReceivedData
	}

	public class SerialPortDataItem
	{
		private DateTime mDate = DateTime.Now;
		public DateTime Date
		{
			get { return mDate; }
			set { mDate = value; }
		}

		private SerialPortDataType mDataType = SerialPortDataType.None;
		public SerialPortDataType DataType
		{
			get { return mDataType; }
			set { mDataType = value; }
		}

		private string mData = "";
		public string Data
		{
			get { return mData; }
			set { mData = value; }
		}

		public SerialPortDataItem(SerialPortDataType pDataType, string pData)
		{
			mDataType = pDataType;
			mData = pData;
			mDate = DateTime.Now;
		}
	}

	public class SerialPortEx
	{
		private Queue<SerialPortDataItem> mDataQueue = new Queue<SerialPortDataItem>();
		private SerialPort mSerialPort = new SerialPort();

		public SerialPortEx()
		{
			mSerialPort.NewLine = "\r";
			mSerialPort.DataReceived += new SerialDataReceivedEventHandler(mSerialPort_DataReceived);
			mSerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(mSerialPort_ErrorReceived);
		}

		private void mSerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
		{
			SerialPortDataItem data = new SerialPortDataItem(SerialPortDataType.Error, e.EventType.ToString());
			mDataQueue.Enqueue(data);

			SerialPortExEventArgs args = new SerialPortExEventArgs { Data =  data };
			OnErrorReceived(args);
		}

		private void mSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			SerialPortDataItem data = new SerialPortDataItem(SerialPortDataType.ReceivedData, mSerialPort.ReadExisting());
			mDataQueue.Enqueue(data);

			SerialPortExEventArgs args = new SerialPortExEventArgs { Data = data };
			OnDataReceived(args);
		}

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

		public bool IsConnect
		{
			get { return mSerialPort.IsOpen; }
		}

		private string mPortNameString = "";
		public string PortNameString
		{
			get { return mPortNameString; }
			set 
			{
				mPortNameString = value;

				if (mPortNameString != "")
					mSerialPort.PortName = mPortNameString;
			}
		}

		public string PortName
		{
			get { return mSerialPort.PortName; }
			set 
			{ 
				mSerialPort.PortName = value;
				mPortNameString = value;
			}
		}

		private string mBaudRateString = "";
		public string BaudRateString
		{
			get { return mBaudRateString; }
			set 
			{
				mBaudRateString = value;

				if (TextCore.IsInt(mBaudRateString))
					mSerialPort.BaudRate = Convert.ToInt32(mBaudRateString);
				else
					mSerialPort.BaudRate = 9600;
			}
		}

		public int BoudRate
		{
			get { return mSerialPort.BaudRate; }
			set 
			{ 
				mSerialPort.BaudRate = value;
				mBaudRateString = value.ToString();
			}
		}

		private string mParityString = "";
		public string ParityString
		{
			get { return mParityString; }
			set 
			{ 
				mParityString = value;

				switch (mParityString.ToUpper())
				{
					case "NONE":
						mSerialPort.Parity = Parity.None;
						break;

					case "EVEN":
						mSerialPort.Parity = Parity.Even;
						break;

					case "ODD":
						mSerialPort.Parity = Parity.Odd;
						break;

					default:
						mSerialPort.Parity = Parity.None;
						break;
				}
			}
		}

		public Parity Parity
		{
			get { return mSerialPort.Parity; }
			set 
			{ 
				mSerialPort.Parity = value;
				mParityString = value.ToString();
			}
		}

		private string mDatabitsString = "";
		public string DatabitsString
		{
			get { return mDatabitsString; }
			set 
			{ 
				mDatabitsString = value;

				if (TextCore.IsInt(mDatabitsString))
				{
					mSerialPort.DataBits = Convert.ToInt32(mDatabitsString);
				}
				else
				{
					mSerialPort.DataBits = 8;
				}
			}
		}

		public int DataBits
		{
			get { return mSerialPort.DataBits; }
			set 
			{ 
				mSerialPort.DataBits = value;
				mDatabitsString = value.ToString();
			}
		}

		private string mStopbitsString = "";
		public string StopbitsString
		{
			get { return mStopbitsString; }
			set 
			{ 
				mStopbitsString = value;

				switch (mStopbitsString)
				{
					case "1":
						mSerialPort.StopBits = StopBits.One;
						break;

					case "2":
						mSerialPort.StopBits = StopBits.Two;
						break;

					default:
						mSerialPort.StopBits = StopBits.One;
						break;
				}
			}
		}

		public StopBits StopBits
		{
			get { return mSerialPort.StopBits; }
			set 
			{
				mSerialPort.StopBits = value;
				mStopbitsString = value.ToString();
			}
		}

		private string mHandshakeString = "";
		public string HandshakeString
		{
			get { return mHandshakeString; }
			set 
			{ 
				mHandshakeString = value;

				switch (mHandshakeString.ToUpper())
				{
					case "NONE":
						mSerialPort.Handshake = Handshake.None;
						break;

					case "RTS":
						mSerialPort.Handshake = Handshake.RequestToSend;
						break;

					case "XON/XOFF":
						mSerialPort.Handshake = Handshake.XOnXOff;
						break;

					case "XONXOFF":
						mSerialPort.Handshake = Handshake.XOnXOff;
						break;

					case "BOTH":
						mSerialPort.Handshake = Handshake.RequestToSendXOnXOff;
						break;
				}
			}
		}

		public Handshake Handshake
		{
			get { return mSerialPort.Handshake; }
			set 
			{
				mSerialPort.Handshake = value;
				mHandshakeString = value.ToString();
			}
		}

		public bool Connect()
		{
			mDataQueue.Clear();

			if (mSerialPort.IsOpen)
				mSerialPort.Close();

			//         serialPort.ReadTimeout = 500;
			mSerialPort.DtrEnable = true;
			mSerialPort.RtsEnable = true;

			try
			{
				mSerialPort.Open();
			}
			catch
			{
				return false;
			}

			return true;
		}

		public string[] ReceivedAllData()
		{
			List<string> list = new List<string>();

			foreach (SerialPortDataItem item in mDataQueue)
			{
				if (item.DataType == SerialPortDataType.ReceivedData)
					list.Add(item.Data);
			}

			return list.ToArray();
		}

		public string[] SendAllData()
		{
			List<string> list = new List<string>();

			foreach (SerialPortDataItem item in mDataQueue)
			{
				if (item.DataType == SerialPortDataType.SendData)
					list.Add(item.Data);
			}

			return list.ToArray();
		}

		public string[] ErrorAllData()
		{
			List<string> list = new List<string>();

			foreach (SerialPortDataItem item in mDataQueue)
			{
				if (item.DataType == SerialPortDataType.Error)
					list.Add(item.Data);
			}

			return list.ToArray();
		}

		public void Write(string pText)
		{
			if (mSerialPort.IsOpen == false)
			{
				throw new Exception("포트가 열려있지 않습니다.");
			}

			mSerialPort.Write(pText);
			//mSerialPort.Write(mSerialPort.NewLine);
			mDataQueue.Enqueue(new SerialPortDataItem(SerialPortDataType.SendData, pText));
		}

		public void Write(byte[] pBuffer, int pOffset, int pCount)
		{
			if (mSerialPort.IsOpen == false)
			{
				throw new Exception("포트가 열려있지 않습니다.");
			}

			mSerialPort.Write(pBuffer, pOffset, pCount);
		}

		public void Initialize()
		{
			mSerialPort.DiscardInBuffer();
			mSerialPort.DiscardOutBuffer();
		}

		public void Disconnect()
		{
			mSerialPort.Close();
		}

		[Category("Ex")]
		public event SerialPortExEventHandler DataReceived;

		public virtual void OnDataReceived(SerialPortExEventArgs e)
		{
			if (DataReceived != null)
				DataReceived(this, e);
		}

		[Category("Ex")]
		public event SerialPortExEventHandler ErrorReceived;

		public virtual void OnErrorReceived(SerialPortExEventArgs e)
		{
			if (ErrorReceived != null)
				ErrorReceived(this, e);
		}
	}
	
	public class SerialPortExEventArgs : EventArgs
	{
		public SerialPortDataItem Data { get; set; }
	}

	public delegate void SerialPortExEventHandler(object sender, SerialPortExEventArgs e);

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

		public bool Connect(string pSerialPortID)
		{
			try
			{
				if (mSerialPortList.ContainsKey(pSerialPortID))
				{
					if (!mSerialPortList[pSerialPortID].IsConnect)
						mSerialPortList[pSerialPortID].Connect();

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

		public bool Disconnect(string pSerialPortID)
		{
			try
			{
				if (mSerialPortList.ContainsKey(pSerialPortID))
				{
					if (mSerialPortList[pSerialPortID].IsConnect)
						mSerialPortList[pSerialPortID].Disconnect();

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
			DisconnectAll();
			mSerialPortList.Clear();
		}

		public void ConnectAll()
		{
			foreach (SerialPortEx port in mSerialPortList.Values)
			{
				try
				{
					if (!port.IsConnect)
						port.Connect();
				}
				catch
				{
					;
				}
			}
		}

		public void DisconnectAll()
		{
			foreach (SerialPortEx port in mSerialPortList.Values)
			{
				try
				{
					if (port.IsConnect)
						port.Disconnect();
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

			port.PortNameString = pPortName;

			port.BaudRateString = pBaudRate;

			port.DatabitsString = pDataBits;

			port.ParityString = pParity;

			port.StopbitsString = pStopBits;

			port.HandshakeString = pHandShake;

			port.SerialPortID = pSerialPortID;
			port.SerialPortName = pSerialPortName;
			port.SerialPortComment = pSerialPortComment;

			mSerialPortList.Add(pSerialPortID, port);
		}
	}
}
