using FadeFox.Text;
using NPCommon.IO;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace NPCommon.DEVICE
{
    public class BarcodeSerial : AbstractSerialPort<bool>
    {
        public List<byte> mReadBuffer = new List<byte>();

        private byte mCr = (byte)0x0D;
        private byte[] mCrLineFeed = { 0x0D, 0x0A };
        public delegate void BarcodeEvent(object sender, string pBarcodeData);
        public event BarcodeEvent EventBarcode;

        public BarcodeSerial()
        {
            SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            SerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialPort_ErrorReceived);
        }

        public override void Initialize()
        {
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }

        public override bool Connect()
        {
            try
            {
                if (SerialPort.IsOpen)
                    SerialPort.Close();

                SerialPort.ReadTimeout = 1000;
                SerialPort.WriteTimeout = 1000;
                SerialPort.DtrEnable = true;
                SerialPort.RtsEnable = true;

                SerialPort.DataBits = 8;
                SerialPort.StopBits = System.IO.Ports.StopBits.One;
                SerialPort.Handshake = System.IO.Ports.Handshake.None;


                SerialPort.Open();
                Initialize();

                return true;
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.DIDO, "BarcodeSerial | Connect", "연결실패:" + ex.ToString());
                return false;
            }
        }

        public override void Disconnect()
        {
            try
            {
                SerialPort.Close();
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.DIDO, "BarcodeSerial | Disconnect", "연결종료실패:" + ex.ToString());
            }
        }

        protected override void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            //Do nothing.
        }

        protected override void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(10);
                int length = SerialPort.BytesToRead;


                for (int i = 0; i < length; i++)
                {
                    mReadBuffer.Add((byte)SerialPort.ReadByte());
                }

                byte[] res = mReadBuffer.ToArray();

                string data = "";
                for (int i = 0; i < res.Length; i++)
                {
                    data += i.ToString() + ":" + res[i].ToString("X2") + " ";
                }
                TextCore.ACTION(TextCore.ACTIONS.BARCODE, "BarcodeSerial | mSerialPort_DataReceived", "데이터수신:" + data.ToString());

                if (mReadBuffer.Contains(mCr)) // 바코드는 끝에 0x0D,0x0A 또는 0x0D가온다
                {
                    //바코드패턴 CGV메가박스

                    int l_etxIndex = 0; // 종료점
                    l_etxIndex = TextCore.ByteSearch(res, mCrLineFeed, 0);
                    if (l_etxIndex >= 0)
                    {
                        mReadBuffer.RemoveRange(l_etxIndex, 2);
                    }
                    string barcodeData = Encoding.Default.GetString(mReadBuffer.ToArray());
                    barcodeData = barcodeData.Replace("\r", "");
                    barcodeData = barcodeData.Replace("?", "");
                    TextCore.ACTION(TextCore.ACTIONS.BARCODE, "BarcodeSerial l mSerialPort_DataReceived", "[받은바코드데이터 이벤트전송] " + barcodeData.Length.ToString() + " " + barcodeData);
                    mReadBuffer.Clear();
                    //바코드패턴 CGV메가박스 주석완료

                    if (this != null)
                    {
                        EventBarcode(this, barcodeData);
                    }
                    return;
                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BarcodeSerial | mSerialPort_DataReceived", "자료수신중 예외상황:" + ex.ToString());
            }
        }
    }
}
