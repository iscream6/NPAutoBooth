using FadeFox;
using FadeFox.Text;
using NPCommon;
using NPCommon.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Text;

namespace NPCommon.DEVICE
{
    //GS POS할인
    public class HMC60 : AbstractSerialPort<Result>
    {
        //영수증프린터자동상태변경기능
        ProtocolStep mStep = ProtocolStep.Ready;
        public PrinterStatusManageMent CurrentPrinterStatus = new PrinterStatusManageMent();

        public HmcStatus CurrentStatus = HmcStatus.Ok;

        private enum ProtocolStep
        {
            Ready,
            SendCommand,
            AutoStatusCommand,
        }

        #region 속성

        private string mReceiveData = null;

        public override void Initialize()
        {
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }

        public override Result Connect()
        {
            if (SerialPort.IsOpen)
                SerialPort.Close();

            SerialPort.ReadTimeout = 1000;
            SerialPort.WriteTimeout = 1000;
            SerialPort.DtrEnable = true;
            SerialPort.RtsEnable = true;


            SerialPort.DataBits = 8;
            SerialPort.StopBits = System.IO.Ports.StopBits.One;
            SerialPort.Parity = System.IO.Ports.Parity.None;
            SerialPort.Handshake = System.IO.Ports.Handshake.None;

            try
            {
                SerialPort.Open();
                Initialize();
                CurrentPrinterStatus.SetIsPortOpenOk(true);
                return new Result(true);
            }
            catch (Exception ex)
            {
                CurrentPrinterStatus.SetIsPortOpenOk(false);
                return new Result(ex);
            }
        }

        public override void Disconnect()
        {
            SerialPort.Close();
        }

        #endregion

        public HMC60()
        {
            SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            SerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialPort_ErrorReceived);
        }

        protected override void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            //DO nothing.
        }

        List<byte> mReadBuffer = new List<byte>();

        protected override void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int length = SerialPort.BytesToRead;
            for (int i = 0; i < length; i++)
            {
                mReadBuffer.Add((byte)SerialPort.ReadByte());
            }
            byte[] resRemain = mReadBuffer.ToArray();
            string data = string.Empty;
            for (int i = 0; i < resRemain.Length; i++)
            {
                data += i.ToString() + ":" + resRemain[i].ToString("X") + " ";
            }
            TextCore.ACTION(TextCore.ACTIONS.RECIPT, "HMC60 | mSerialPort_DataReceived", "RemainCoin:" + data);
            //if (mStep == ProtocolStep.SendCommand)
            //{
            for (int i = 0; i < resRemain.Length; i++)
            {
                data += i.ToString() + ":" + resRemain[i].ToString("X") + " ";
            }
            try
            {
                string signal1 = TextCore.lPad(Convert.ToString(resRemain[0], 2), 8, "0");
                TextCore.ACTION(TextCore.ACTIONS.RECIPT, "HMC60 | mSerialPort_DataReceived", "상태표시:" + signal1);
                //mReceiveData = signal1;
                string isPageExist = signal1.Substring(7, 1); // 0x00 / 0x01
                string isPrintHeader = signal1.Substring(6, 1); // 0x00 / 0x02
                                                                // 0x00 / 0x04 잼
                string isPageAmount = signal1.Substring(4, 1); // 0x00 / 0x08
                CurrentPrinterStatus.SetIsComuniCationOk(true);
                if (isPageExist == "1")
                {
                    TextCore.DeviceError(TextCore.DEVICE.RECIPT, "HMC60 | mSerialPort_DataReceived", "상채테크명령어응답:" + "용지없음에러");
                    CurrentStatus = HmcStatus.PageEmpty;
                    CurrentPrinterStatus.SetIsPageEmptyOk(false);


                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HMC60 | mSerialPort_DataReceived", "상채테크명령어응답:" + "용지없음정상");
                    CurrentPrinterStatus.SetIsPageEmptyOk(true);
                }

                if (isPageAmount == "1")
                {

                    TextCore.DeviceError(TextCore.DEVICE.RECIPT, "HMC60 | mSerialPort_DataReceived", "상태체크명령어응답:" + "용지부족에러");
                    CurrentStatus = HmcStatus.PageNearEnd;
                    CurrentPrinterStatus.SetIsPageNearOk(false);

                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HMC60 | mSerialPort_DataReceived", "상채테크명령어응답:" + "용지부족정상");
                    CurrentPrinterStatus.SetIsPageNearOk(true);

                }
                if (isPrintHeader == "1")
                {
                    TextCore.DeviceError(TextCore.DEVICE.RECIPT, "HMC60 | mSerialPort_DataReceived", "상태체크명령어응답:" + "커터업에러");
                    CurrentStatus = HmcStatus.CutterUp;
                    CurrentPrinterStatus.SetIsCutterUpOk(false);


                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HMC60 | mSerialPort_DataReceived", "상채테크명령어응답:" + "커터업정상");
                    CurrentStatus = HmcStatus.CutterUp;
                    CurrentPrinterStatus.SetIsCutterUpOk(true);

                }

                mReadBuffer.Clear();
                mStep = ProtocolStep.Ready;
                return;
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.RECIPT, "HMC60 | mSerialPort_DataReceived", ex.ToString());
                mReadBuffer.Clear();
                mStep = ProtocolStep.Ready;
                return;

            }

        }

        public void Test()
        {
            if (!SerialPort.IsOpen)
            {
                throw new Exception("포트가 열려있지 않습니다.");
            }

            mReadBuffer.Clear();

            List<byte> command = new List<byte>();

            command.Add(0x1B);					// ESC
            command.Add(Convert.ToByte('L'));	// PageMode : STANDARD모드에서 페이지 모드로 전환함.

            command.Add(0x1B);					// ESC
            command.Add(Convert.ToByte('T'));	// 페이지 모드의 인자방향 및 시점을 지정함
            command.Add(Convert.ToByte('3'));	// 0: Top, 1:Left, 2:Bottom, 3:Right

            command.Add(Convert.ToByte('A'));
            command.Add(Convert.ToByte('B'));
            command.Add(Convert.ToByte('C'));
            command.Add(Convert.ToByte('D'));
            command.Add(Convert.ToByte('E'));
            command.Add(Convert.ToByte('F'));
            command.Add(Convert.ToByte('G'));
            command.Add(Convert.ToByte('1'));
            command.Add(Convert.ToByte('2'));
            command.Add(Convert.ToByte('3'));

            command.Add(0x1B);					// ESC
            command.Add(Convert.ToByte('i'));

            Write(command.ToArray());
        }

        /// <summary>
        /// 용지를 완전히 커팅 함
        /// </summary>
        public void FullCutting()
        {
            // ESC + 'i'
            byte[] command = { 0x1B, 0x69 };
            Write(command);
        }

        public byte[] FullCuttingByte()
        {
            // ESC + 'i'
            byte[] command = { 0x1B, 0x69 };
            return command;
        }

        /// <summary>
        /// 용지가 서로 매달려 있을 수 있도록 약간 덜 커팅 함
        /// </summary>
        public void ParticalCutting()
        {
            // ESC + 'm'
            byte[] command = { 0x1B, 0x6D };
            Write(command);
        }

        public byte[] ParticalCuttingByte()
        {
            // ESC + 'm'
            byte[] command = { 0x1B, 0x6D };
            return command;
        }

        /// <summary>
        /// 밀리미터 단위로 용지 전진 이동 시킴
        /// </summary>
        /// <param name="pMillimeter">이동시킬 길이</param>
        public void Feeding(int pMillimeter)
        {
            int totalLen = pMillimeter * 8;

            int etcLen = totalLen % 240; // 240(4cm) 단위로 짤라서 남은 나머지 길이
            int unitCount = 0;  // 240(4cm) 단위로 짜름 

            int len = totalLen - etcLen;

            if (len > 0)
            {
                unitCount = len / 240;
            }

            for (int i = 0; i < unitCount; i++)
            {
                // ESC + 'J' + n
                // n * 0.125mm 이동
                byte[] command = { 0x1B, 0x4A, Convert.ToByte(240) };
                Write(command);
            }

            // ESC + 'J' + n
            // n * 0.125mm 이동
            byte[] command2 = { 0x1B, 0x4A, Convert.ToByte(etcLen) };
            Write(command2);
        }

        public byte[] FeedingByte(int pMillimeter)
        {
            List<byte> addByte = new List<byte>();
            int totalLen = pMillimeter * 8;

            int etcLen = totalLen % 240; // 240(4cm) 단위로 짤라서 남은 나머지 길이
            int unitCount = 0;  // 240(4cm) 단위로 짜름 

            int len = totalLen - etcLen;

            if (len > 0)
            {
                unitCount = len / 240;
            }

            for (int i = 0; i < unitCount; i++)
            {
                // ESC + 'J' + n
                // n * 0.125mm 이동
                byte[] command = { 0x1B, 0x4A, Convert.ToByte(240) };
            }

            // ESC + 'J' + n
            // n * 0.125mm 이동
            byte[] command2 = { 0x1B, 0x4A, Convert.ToByte(etcLen) };
            return command2;
        }

        /// <summary>
        /// 밀리미터 단위로 용지 후진 이동 시킴
        /// </summary>
        /// <param name="pMillimeter">이동시킬 길이</param>
        public void BackFeeding(int pMillimeter)
        {
            int totalLen = pMillimeter * 8;

            int etcLen = totalLen % 240; // 240(4cm) 단위로 짤라서 남은 나머지 길이
            int unitCount = 0;  // 240(4cm) 단위로 짜름 

            int len = totalLen - etcLen;

            if (len > 0)
            {
                unitCount = len / 240;
            }

            for (int i = 0; i < unitCount; i++)
            {
                // ESC + 'j' + n
                // n * 0.125mm 이동
                byte[] command = { 0x1B, 0x6A, Convert.ToByte(240) };
                Write(command);
            }

            // ESC + 'j' + n
            // n * 0.125mm 이동
            byte[] command2 = { 0x1B, 0x6A, Convert.ToByte(etcLen) };
            Write(command2);
        }

        /// <summary>
        /// 글꼴을 굵게 얇게 변경
        /// </summary>
        /// <param name="pIsBold">굵게인지</param>
        public void FontBold(bool pIsBold)
        {
            // ESC + 'E' + n
            // n:0 일반, n:1굵게
            if (pIsBold)
            {
                byte[] command = { 0x1B, 0x45, 0x01 };
                Write(command);
            }
            else
            {
                byte[] command = { 0x1B, 0x45, 0x00 };
                Write(command);
            }
        }


        /// <summary>
        /// 글꼴을 역상 표준으로 변경
        /// </summary>
        /// <param name="pIsBold">굵게인지</param>
        public void FontReverse(bool pIsReverse)
        {
            // GS + 'B' + n
            // n:0 일반, n:1역상
            if (pIsReverse)
            {
                byte[] command = { 0x1D, 0x42, 0x01 };
                Write(command);
            }
            else
            {
                byte[] command = { 0x1D, 0x42, 0x00 };
                Write(command);
            }
        }

        /// <summary>
        /// 폰트 크기 지정
        /// </summary>
        /// <param name="pWidthRate">넓이 비율 지정(1~8까지)</param>
        /// <param name="pHeightRate">높이 비율 지정(1~8까지)</param>
        public void FontSize(int pWidthRate, int pHeightRate)
        {
            byte width = 0;
            byte height = 0;

            switch (pWidthRate)
            {
                case 1:
                    width = 0x00;
                    break;
                case 2:
                    width = 0x10;
                    break;
                case 3:
                    width = 0x20;
                    break;
                case 4:
                    width = 0x30;
                    break;
                case 5:
                    width = 0x40;
                    break;
                case 6:
                    width = 0x50;
                    break;
                case 7:
                    width = 0x60;
                    break;
                case 8:
                    width = 0x70;
                    break;
            }

            switch (pHeightRate)
            {
                case 1:
                    height = 0x00;
                    break;
                case 2:
                    height = 0x01;
                    break;
                case 3:
                    height = 0x02;
                    break;
                case 4:
                    height = 0x03;
                    break;
                case 5:
                    height = 0x04;
                    break;
                case 6:
                    height = 0x05;
                    break;
                case 7:
                    height = 0x06;
                    break;
                case 8:
                    height = 0x07;
                    break;
            }

            // GS + '!' + n
            // 
            byte[] command = { 0x1D, 0x21, Convert.ToByte(width + height) };
            Write(command);
        }

        public byte[] FontSizeByte(int pWidthRate, int pHeightRate)
        {
            byte width = 0;
            byte height = 0;

            switch (pWidthRate)
            {
                case 1:
                    width = 0x00;
                    break;
                case 2:
                    width = 0x10;
                    break;
                case 3:
                    width = 0x20;
                    break;
                case 4:
                    width = 0x30;
                    break;
                case 5:
                    width = 0x40;
                    break;
                case 6:
                    width = 0x50;
                    break;
                case 7:
                    width = 0x60;
                    break;
                case 8:
                    width = 0x70;
                    break;
            }

            switch (pHeightRate)
            {
                case 1:
                    height = 0x00;
                    break;
                case 2:
                    height = 0x01;
                    break;
                case 3:
                    height = 0x02;
                    break;
                case 4:
                    height = 0x03;
                    break;
                case 5:
                    height = 0x04;
                    break;
                case 6:
                    height = 0x05;
                    break;
                case 7:
                    height = 0x06;
                    break;
                case 8:
                    height = 0x07;
                    break;
            }

            // GS + '!' + n
            // 
            byte[] command = { 0x1D, 0x21, Convert.ToByte(width + height) };
            return command;
        }

        /// <summary>
        /// 폰트 진하기
        /// </summary>
        public void FontDeep(int p_FoneDeep)
        {
            if (p_FoneDeep > 5)
            {
                p_FoneDeep = 1;
            }
            else if (p_FoneDeep < 0)
            {
                p_FoneDeep = 1;
            }
            byte deep = 0;
            switch (p_FoneDeep)
            {
                case 0:
                    deep = 0x00;
                    break;
                case 1:
                    deep = 0x01;
                    break;
                case 2:
                    deep = 0x02;
                    break;
                case 3:
                    deep = 0x03;
                    break;
                case 4:
                    deep = 0x04;
                    break;
                case 5:
                    deep = 0x05;
                    break;
            }


            //GS ( K pL pH fn m

            byte[] command = { 0x1D, 0x28, 0x4B, 0x02, 0x00, 0x49, deep };
            Write(command);
        }

        /// <summary>
        /// 문자열 인쇄
        /// </summary>
        /// <param name="pValue">인쇄할 문자열</param>
        public void Print(string pValue)
        {
            byte[] command = Encoding.Default.GetBytes(pValue);
            Write(command);
        }

        public byte[] PrintByte(string pValue)
        {
            byte[] command = Encoding.Default.GetBytes(pValue);
            return command;
        }


        private void Write(byte[] pCommend)
        {
            if (!SerialPort.IsOpen)
            {
                return;
            }

            SerialPort.Write(pCommend, 0, pCommend.Length);
        }

        private void Write(string pCommand)
        {
            if (!SerialPort.IsOpen)
            {
                throw new Exception("포트가 열려있지 않습니다.");
            }

            SerialPort.Write(pCommand);
        }
        private int mTimeOut = 2000;

        public enum HmcStatus
        {
            PageEmpty = 605,
            PageNearEnd = 606,
            CutterUp = 604,
            Ok = 4,
            Communcation = 602,
            PortOpen = 607

        }
        public HmcStatus HmcGetStatus()
        {
            if (!SerialPort.IsOpen)
            {
                throw new Exception("포트가 열려있지 않습니다.");
            }
            try
            {

                mStep = ProtocolStep.SendCommand;
                mReadBuffer.Clear();
                byte[] command = { 0x1D, 0x72, 0x01 };
                TextCore.ACTION(TextCore.ACTIONS.RECIPT, "HMC60|HmcGetStatus", "상채테크명령어보냄:");
                SerialPort.Write(command, 0, command.Length);

                DateTime startDate = DateTime.Now;

                while (mStep != ProtocolStep.Ready)
                {
                    TimeSpan diff = DateTime.Now - startDate;

                    if (Convert.ToInt32(diff.TotalMilliseconds) > mTimeOut)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.RECIPT, "HMC60|HmcGetStatus", "응답시간초과");
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        return HmcStatus.Communcation;
                    }
                }
                return HmcStatus.Ok;


            }
            catch (Exception ex)
            {

                TextCore.DeviceError(TextCore.DEVICE.RECIPT, "HMC60 | HmcStatus", "응답시간초과" + ex.ToString());
                return HmcStatus.Communcation;
            }

        }

        public void AutoStatus()
        {
            if (!SerialPort.IsOpen)
            {
                throw new Exception("포트가 열려있지 않습니다.");
            }
            try
            {

                mStep = ProtocolStep.AutoStatusCommand;
                mReadBuffer.Clear();
                byte[] command = { 0x1D, 0x61, 0x01 };
                TextCore.ACTION(TextCore.ACTIONS.RECIPT, "HMC60 | AutoStatus", "자동상채테크명령어보냄:");
                SerialPort.Write(command, 0, command.Length);

            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.RECIPT, "HMC60 | AutoStatus", "응답시간초과" + ex.ToString());
                //return HmcStatus.OK;

            }

        }

        public class PrinterStatusManageMent
        {
            public void SetDbErrorInfo(DataTable pPrinterData)
            {
                if (pPrinterData != null && pPrinterData.Rows.Count > 0)
                {
                    foreach (DataRow printerItem in pPrinterData.Rows)
                    {
                        int statusCode = Convert.ToInt32(printerItem["STATUSCODE"].ToString());
                        bool isSuccess = true;

                        if (printerItem["ISSUCCESS"].ToString() == CommProtocol.DeviceStatus.Success.ToString())
                        {
                            isSuccess = true;
                        }
                        else if (printerItem["ISSUCCESS"].ToString() == CommProtocol.DeviceStatus.Fail.ToString())
                        {
                            isSuccess = false;
                        }
                        switch ((HmcStatus)statusCode)
                        {
                            case HmcStatus.PortOpen:
                                mIsPortOpenError = !isSuccess;
                                if (mIsPortOpenError)
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Fail, statusCode);
                                }
                                else
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, statusCode);
                                }
                                break;

                            case HmcStatus.Communcation:
                                mIsComuniCationError = !isSuccess;
                                if (mIsComuniCationError)
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Fail, statusCode);
                                }
                                else
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, statusCode);
                                }
                                break;
                            case HmcStatus.CutterUp:
                                mIsCuterUpError = !isSuccess;
                                if (mIsCuterUpError)
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Fail, statusCode);
                                }
                                else
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, statusCode);
                                }
                                break;
                            case HmcStatus.PageNearEnd:
                                mIsPageNearError = !isSuccess;
                                if (mIsPageNearError)
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Fail, statusCode);
                                }
                                else
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, statusCode);
                                }
                                break;
                            case HmcStatus.PageEmpty:
                                mIsPageEmptyError = !isSuccess;
                                if (mIsPageEmptyError)
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Fail, statusCode);
                                }
                                else
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, statusCode);
                                }
                                break;
                        }
                    }
                }
                else
                {
                    mIsPortOpenError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.PortOpen);
                    mIsComuniCationError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.Communcation);
                    mIsCuterUpError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.CutterUp);
                    mIsPageNearError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.PageNearEnd);
                    mIsPageEmptyError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.PageEmpty);
                }
            }

            public void SendAllDeviveOk()
            {
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.PortOpen);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.Communcation);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.CutterUp);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.PageNearEnd);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.PageEmpty);

            }

            private bool mIsPortOpenError = false;
            public bool IsPortOpenError
            {
                get { return mIsPortOpenError; }
            }


            /// <summary>
            /// 커터업상태인지 true면    커터업 3
            /// </summary>
            private bool mIsCuterUpError = false;
            public bool IsCutterUpError
            {
                get { return mIsCuterUpError; }
            }
            /// <summary>
            /// 용지부족 상태인지 true면 용지부족 1
            /// </summary>
            private bool mIsPageNearError = false;
            public bool IsPageNearError
            {
                get { return mIsPageNearError; }
            }

            /// <summary>
            /// 용지없음 상태인지 true면 용지없음 2
            /// </summary>
            private bool mIsPageEmptyError = false;
            public bool IsPageEmptyError
            {
                get { return mIsPageEmptyError; }
            }

            /// <summary>
            /// 통신장애 true면
            /// </summary>
            private bool mIsComuniCationError = false;

            public bool IsComuniCationError
            {
                get { return mIsComuniCationError; }
            }



            public void SetIsPortOpenOk(bool pIsPortOpen)
            {
                if (mIsPortOpenError) // 기존에 통신에러였다면
                {
                    if (pIsPortOpen) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.Communcation);
                        mIsPortOpenError = !pIsPortOpen;
                    }
                }
                else  // 기존에 동전입수가기 정상일때
                {
                    if (pIsPortOpen == false) // 현재 동전입수기가 정상이라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Fail, (int)HmcStatus.Communcation);

                        mIsPortOpenError = !pIsPortOpen;

                    }
                }
                NPSYS.Device.gIsUseReceiptPrintDevice = GetHmc60DeveiceOpertationYn();

            }

            public void SetIsComuniCationOk(bool pIscommunication)
            {
                if (mIsComuniCationError) // 기존에 통신에러였다면
                {
                    if (pIscommunication) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.Communcation);
                        mIsComuniCationError = !pIscommunication;

                    }
                }
                else  // 기존에 동전입수가기 정상일때
                {
                    if (pIscommunication == false) // 현재 동전입수기가 정상이라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Fail, (int)HmcStatus.Communcation);

                        mIsComuniCationError = !pIscommunication;

                    }
                }
                NPSYS.Device.gIsUseReceiptPrintDevice = GetHmc60DeveiceOpertationYn();

            }

            public void SetIsCutterUpOk(bool pIsCutterUp)
            {
                if (mIsCuterUpError)
                {
                    if (pIsCutterUp)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.CutterUp);
                        mIsCuterUpError = !pIsCutterUp;

                    }
                }
                else
                {
                    if (pIsCutterUp == false) // 현재 동전입수기가 정상이라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Fail, (int)HmcStatus.CutterUp);

                        mIsCuterUpError = !pIsCutterUp;

                    }
                }
                NPSYS.Device.gIsUseReceiptPrintDevice = GetHmc60DeveiceOpertationYn();

            }

            public void SetIsPageNearOk(bool pIsPageNear)
            {
                if (mIsPageNearError)
                {
                    if (pIsPageNear)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.PageNearEnd);
                        mIsPageNearError = !pIsPageNear;

                    }
                }
                else  // 기존에 동전입수가기 정상일때
                {
                    if (pIsPageNear == false) // 현재 동전입수기가 정상이라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Fail, (int)HmcStatus.PageNearEnd);

                        mIsPageNearError = !pIsPageNear;

                    }
                }
                NPSYS.Device.gIsUseReceiptPrintDevice = GetHmc60DeveiceOpertationYn();

            }

            public void SetIsPageEmptyOk(bool pIsPageEmpy)
            {
                if (mIsPageEmptyError) // 기존에 통신에러였다면
                {
                    if (pIsPageEmpy) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Success, (int)HmcStatus.PageEmpty);
                        mIsPageEmptyError = !pIsPageEmpy;

                    }
                }
                else
                {
                    if (pIsPageEmpy == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, CommProtocol.DeviceStatus.Fail, (int)HmcStatus.PageEmpty);

                        mIsPageEmptyError = !pIsPageEmpy;
                    }
                }
                NPSYS.Device.gIsUseReceiptPrintDevice = GetHmc60DeveiceOpertationYn();

            }


            private bool GetHmc60DeveiceOpertationYn()
            {
                if ((mIsComuniCationError
                    || mIsPortOpenError
                    ) == true)
                {
                    return false;
                }
                return true;
            }

        }
    }

}