using FadeFox;
using FadeFox.Text;
using NPCommon;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;

namespace NPCommon.DEVICE
{
    /// <summary>
    /// 카드리더기 왼쪽거 제어
    /// </summary>
    public class TicketCardDevice
    {
        public TicketCardDeviceStatusManageMent CurrentTicketCardDeviceStatusManageMent = new TicketCardDeviceStatusManageMent();
        public enum TicketAndCardResult
        {
            OK = 0,
            No_TICEKT = 1,
            Empy = 2,
            No_Return_Sensor_Ticket = 3,
            No_Ready_Ticket = 4,
            Read_and_Verify_Fail = 5,
            TicketAndCardEntry = 6,    // 카드가 들어가 있는상태
            TicketAndCardEntry1 = 208,    // 카드가 들어가 있는상태
            Device_not_found = 1801,
            Device_Timeout = 1802,
            Device_not_opened = 1803,
            Response_frame_size_error = 1804,
            Module_Alarm = 1805,
            Ticket_Jam = 1806,
            Ticket_Reject = 1807,
            Invalid_Write_Data = 1808,
            Ticket_Reject_Error = 1809,
            Invalid_handle = 1810,
            IO_error = 1811,
            Unknown_Error1 = 1812,
            Unknown_Error2 = 1813,
            Unknown_Error3 = 1814,
            Unknown_Error4 = 1815,
        }





        private TicketAndCardResult ConvertResultType(int pCode)
        {
            switch (pCode)
            {
                case 0:
                    return TicketAndCardResult.OK;
                case 4:
                    return TicketAndCardResult.Device_Timeout;
                case 5:
                    return TicketAndCardResult.Response_frame_size_error;
                case 11:
                    return TicketAndCardResult.Module_Alarm;
                case 12:
                    return TicketAndCardResult.Ticket_Jam;
                case 13:
                    return TicketAndCardResult.No_Return_Sensor_Ticket;
                case 14:
                    return TicketAndCardResult.No_Ready_Ticket;
                case 15:
                    return TicketAndCardResult.Read_and_Verify_Fail;
                case 16:
                    return TicketAndCardResult.Ticket_Reject;
                case 17:
                    return TicketAndCardResult.Invalid_Write_Data;
                case 18:
                    return TicketAndCardResult.Ticket_Reject_Error;
                case 1001:
                    return TicketAndCardResult.Invalid_handle;
                case 1002:
                    return TicketAndCardResult.Device_not_found;
                case 1003:
                    return TicketAndCardResult.Device_not_opened;
                case 1004:
                    return TicketAndCardResult.IO_error;
                case 1005:
                    return TicketAndCardResult.Unknown_Error1;
                case 1006:
                    return TicketAndCardResult.Unknown_Error2;
                case 1007:
                    return TicketAndCardResult.Unknown_Error3;
                case 1008:
                    return TicketAndCardResult.Unknown_Error4;
                default:
                    return TicketAndCardResult.Unknown_Error4;

            }

        }




        [DllImport("TIU_DLL.Dll", EntryPoint = "OpenDevice")]
        private static extern int OpenDevice(int iDeviceType, Byte[] pData, int portNum);

        [DllImport("TIU_DLL.Dll", EntryPoint = "CloseDevice")]
        //        private static extern int IDCCOIN_Init(int iDeviceType, [MarshalAs(UnmanagedType.LPStr)]string pData,int portNum );
        private static extern int CloseDevice(int iDeviceType);

        [DllImport("TIU_DLL.Dll", EntryPoint = "TicketDecode")]
        //int TicketDecode (BYTE *pSetData, int nSetLen, BYTE *pRspData, DWORD *pdwLen)
        private static extern int TicketDecode(Byte[] pSetData, int nSetLen, Byte[] pRspData, out UInt32 pdwLen);

        [DllImport("TIU_DLL.Dll", EntryPoint = "TicketExit")]
        private static extern int TicketExit();

        [DllImport("TIU_DLL.Dll", EntryPoint = "TicketCapture")]
        private static extern int TicketCapture();

        [DllImport("TIU_DLL.Dll", EntryPoint = "SoftReset")]
        private static extern int SoftReset();


        [DllImport("TIU_DLL.Dll", EntryPoint = "TiuGetStatus")]
        //int TiuGetStatus ( BYTE *pRspData, DWORD *pdwLen )
        private static extern int TiuGetStatus(Byte[] pRspData, out UInt32 pdwLen);

        public Result SoftResetCreditDevice()
        {
            TicketAndCardResult resultSoftReset = ConvertResultType(SoftReset());
            if (resultSoftReset == TicketAndCardResult.OK)
            {
                return new Result(true, resultSoftReset.ToString(), (int)resultSoftReset);
            }
            else
            {
                return new Result(false, resultSoftReset.ToString(), (int)resultSoftReset);
            }
        }

        /// <summary>
        /// 카드장비의 상태메시지를 읽고 에러메시지에 카드가 빈상태거나 읽지못하거나 센서에 아무것도 없다면 false 반환
        /// </summary>
        /// <param name="ticketAndCardMessage"></param>
        /// <returns></returns>
        public bool IsCreditCardSuccessStatus(int ticketAndCardMessage)
        {
            TicketAndCardResult _currentTicketAndCardResult = (TicketAndCardResult)ticketAndCardMessage;
            if (_currentTicketAndCardResult == TicketCardDevice.TicketAndCardResult.Empy || //20120202 수정
                _currentTicketAndCardResult == TicketCardDevice.TicketAndCardResult.Read_and_Verify_Fail ||
                _currentTicketAndCardResult == TicketCardDevice.TicketAndCardResult.No_Return_Sensor_Ticket)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Result GetStatus()
        {
            try
            {
                Byte[] pRspData = new Byte[12];
                uint pdwLen = Convert.ToUInt32(pRspData.Length);
                TicketAndCardResult _TicketAndCardResult = ConvertResultType(TiuGetStatus(pRspData, out pdwLen));
                if (_TicketAndCardResult == TicketAndCardResult.OK)
                {
                    return new Result(true, _TicketAndCardResult.ToString(), (int)_TicketAndCardResult);
                }
                else
                {
                    return new Result(false, _TicketAndCardResult.ToString(), (int)_TicketAndCardResult);

                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TicketCardDevice|GetStatus", "상태체크에러" + ex.ToString());
                return new Result(false, "상태체크중 에러", 98);
            }
        }
        public Boolean GetTIcketReadingReadyStatus()
        {
            try
            {
                Byte[] pRspData = new Byte[12];
                uint pdwLen = Convert.ToUInt32(pRspData.Length);
                TicketAndCardResult _TicketAndCardResult = ConvertResultType(TiuGetStatus(pRspData, out pdwLen));
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TicketCardDevice | GetTIcketReadingReadyStatus", _TicketAndCardResult.ToString());
                if (_TicketAndCardResult == TicketAndCardResult.OK)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TicketCardDevice | GetTIcketReadingReadyStatus", "length" + pRspData.Length.ToString());
                    if (pRspData.Length > 11)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TicketCardDevice | GetTIcketReadingReadyStatus", "entry" + Convert.ToInt32(pRspData[10]).ToString());
                        if (Convert.ToInt32(pRspData[10]) == (int)TicketAndCardResult.TicketAndCardEntry)  // 카드가 들어있는상태 확인
                        {
                            return true;
                        }
                        else if (Convert.ToInt32(pRspData[10]) == (int)TicketAndCardResult.TicketAndCardEntry1)  // 카드가 들어있는상태 확인
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TicketCardDevice|GetTIcketReadingReadyStatus", ex.ToString());
                return true;
            }

        }

        public Result TIcektCreditCardopenDevice(string portselect, string baurdrateselect)
        {
            int port = 0;
            port = Convert.ToInt32(portselect) - 1;
            int baurdrate = 0;
            switch (baurdrateselect)
            {
                case "4800":
                    baurdrate = 0;
                    break;
                case "9600":
                    baurdrate = 1;
                    break;
                case "14400":
                    baurdrate = 2;
                    break;
                case "19200":
                    baurdrate = 3;
                    break;
                case "38400":
                    baurdrate = 4;
                    break;
                case "56000":
                    baurdrate = 5;
                    break;
                case "57600":
                    baurdrate = 6;
                    break;
                case "115200":
                    baurdrate = 7;
                    break;
                default:
                    baurdrate = 4;
                    break;


            }

            Byte[] psetData = new Byte[5];
            psetData[0] = Convert.ToByte(port);
            psetData[1] = Convert.ToByte(baurdrate);
            psetData[2] = 3;
            psetData[3] = 0;
            psetData[4] = 0;


            try
            {
                TicketAndCardResult _TicketAndCardResult = ConvertResultType(OpenDevice(0, psetData, 0));
                if (_TicketAndCardResult == TicketAndCardResult.OK)
                {
                    CurrentTicketCardDeviceStatusManageMent.SetDeviceStatus(TicketAndCardResult.Device_not_opened, true);
                    return new Result(true);
                }
                else
                {
                    CurrentTicketCardDeviceStatusManageMent.SetDeviceStatus(TicketAndCardResult.Device_not_opened, false);
                    return new Result(false, _TicketAndCardResult.ToString(), (int)_TicketAndCardResult);
                }


            }
            catch (Exception ex)
            {
                CurrentTicketCardDeviceStatusManageMent.SetDeviceStatus(TicketAndCardResult.Device_not_opened, false);
                TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "TicketCardDevice|TIcektCreditCardopenDevice", "연결실패:" + ex.ToString());
                return new Result(ex);
            }
        }


        public int TIcektCreditCardCloseDevice()
        {
            int status = 0;
            status = CloseDevice(1);
            return status;
        }




        public Result readingTicketCardStart()
        {

            Byte[] pSetData = new Byte[5];

            pSetData[0] = 0x36; //2,3
            if (NPSYS.GetCurrentDiscountReadingFormat == DiscountReadingFormat.TRACK2ISO_TRACK3105)
            {
                pSetData[1] = 0x34; // 2 iso 3은 105bpi
            }
            if (NPSYS.GetCurrentDiscountReadingFormat == DiscountReadingFormat.TRACK2ISO_TRACK3210)
            {
                pSetData[1] = 0x33; // 2 iso 3은 210bpi
            }
            pSetData[2] = 0x00; //holidingTIme
            pSetData[3] = 0x30; //Read 후 Wait
            pSetData[4] = 0x30; // 0x30: Read Start
            Byte[] pRspData = new Byte[40];

            UInt32 Responsedatalength = Convert.ToUInt32(pRspData.Length);
            try
            {
                TicketAndCardResult _TicketAndCardResult = ConvertResultType(TicketDecode(pSetData, pSetData.Length, pRspData, out Responsedatalength));
                if (_TicketAndCardResult == TicketAndCardResult.OK)
                {
                    return new Result(true, _TicketAndCardResult.ToString());
                }
                return new Result(false, (int)_TicketAndCardResult);
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "TicketCardDevice|readingTicketCardStart", ex.ToString());
                return new Result(ex);
            }
        }

        public Result readingTicketCardEnd()
        {

            string ticketInfo = "";
            bool readingSuccess = false;
            try
            {

                Byte[] pSetData = new Byte[5];
                pSetData[0] = 0x36; //2,3
                if (NPSYS.GetCurrentDiscountReadingFormat == DiscountReadingFormat.TRACK2ISO_TRACK3105)
                {
                    pSetData[1] = 0x34; // 2 iso 3은 105bpi
                }
                if (NPSYS.GetCurrentDiscountReadingFormat == DiscountReadingFormat.TRACK2ISO_TRACK3210)
                {
                    pSetData[1] = 0x33; // 2 iso 3은 210bpi
                }
                pSetData[2] = 0x00; //holidingTIme
                pSetData[3] = 0x30; //Read 후 Wait
                pSetData[4] = 0x31; // 0x30: Read end
                Byte[] pRspData = new Byte[200];
                UInt32 Responsedatalength = Convert.ToUInt32(pRspData.Length);

                TicketAndCardResult _TicketAndCardResult = ConvertResultType(TicketDecode(pSetData, pSetData.Length, pRspData, out Responsedatalength));

                if (_TicketAndCardResult == TicketAndCardResult.OK)
                {
                    if (GetTIcketReadingReadyStatus())
                    {
                        readingSuccess = true;
                        TextCore.ACTION(TextCore.ACTIONS.USER, "TicketCardDevice|readingTicketCardEnd", "카드/티켓 넣음" + NPSYS.GetCurrentDiscountReadingFormat.ToString());
                        // 105bpi의 티켓일때 pRspData[1]=0x30은 ok상태이며 기본리턴값 다섯자리 제외 5자리가넘어야한다.아니면 에러 기존 카드에서  7자리로 나온다
                        // ticketInfo = Encoding.Default.GetString(pRspData, 4, pRspData.Length - 4).Replace(" ","").Trim();

                        int l_Track_2 = 0;   // 2트랙이 시작위치
                        int l_Track_3 = 0;   // 3트랙이 시작위치

                        ticketInfo = Encoding.Default.GetString(pRspData).Replace(" ", "").Trim();
                        string aa = "";
                        for (int i = 0; i < pRspData.Length; i++)
                        {
                            aa += " " + i.ToString() + ":" + pRspData[i].ToString("X");

                            if (pRspData[i] == 0x12)
                            {
                                l_Track_2 = i;
                            }
                            if (pRspData[i] == 0x13)
                            {
                                l_Track_3 = i;
                            }

                        }
                        TextCore.ACTION(TextCore.ACTIONS.CARDREADER1, "TicketCardDevice|readingTicketCardEnd", "전체데이터:" + aa.ToString());

                        string cardTrack = "";
                        string ticketTrack = "";
                        if (l_Track_2 + 38 == l_Track_3)   // 두번째 트랙이 정상적이면 최소 37데이터가 들어감 
                        {
                            cardTrack = Encoding.Default.GetString(pRspData, l_Track_2 + 1, 37);
                            TextCore.ACTION(TextCore.ACTIONS.CARDREADER1, "TicketCardDevice|readingTicketCardEnd", "카드읽음:**************");
                        }
                        else if (l_Track_3 < 10 && pRspData.Length > 40)
                        {
                            ticketTrack = Encoding.Default.GetString(pRspData, l_Track_2 + 2, 30);
                            TextCore.ACTION(TextCore.ACTIONS.CARDREADER1, "TicketCardDevice|readingTicketCardEnd", "티켓읽음:" + ticketTrack);

                        }

                        if (ticketTrack.Trim() != string.Empty)
                        {

                            if (ticketTrack.Substring(0, 2) == "20")
                            {
                                // return new Result(true, Result.ReadingTypes.DiscountTIcket, "20130225175208003NN01140110"); // 이노위해 수정
                                return new Result(true, Result.ReadingTypes.DiscountTIcket, ticketTrack.Replace("\0", string.Empty));

                            }
                            else
                            {
                                // return new Result(true, Result.ReadingTypes.DiscountTIcket, "20130225175208003NN01140110"); // 이노위해 수정
                                return new Result(false, (int)TicketAndCardResult.No_TICEKT);
                            }



                        }
                        if (cardTrack.Trim() != string.Empty)
                        {
                            if (cardTrack.IndexOf('=') > 0)
                            {
                                return new Result(true, Result.ReadingTypes.CreditCard, cardTrack);

                            }
                            else
                            {

                                return new Result(false, (int)TicketAndCardResult.No_TICEKT);
                            }

                        }

                        return new Result(false, (int)TicketAndCardResult.No_TICEKT);
                        // return new Result(true, Result.ReadingTypes.DiscountTIcket, "20130225175208003NN01140110"); // 이노위해 수정

                    }
                    else
                    {

                        return new Result(false, (int)TicketAndCardResult.Empy); //이노
                                                                                 //  return new Result(true, Result.ReadingTypes.DiscountTIcket, "20130225175208003NN01140110"); // 이노위해 수정

                    }


                }
                else
                {

                    if (_TicketAndCardResult == TicketAndCardResult.No_Return_Sensor_Ticket)
                    {

                        return new Result(false, (int)_TicketAndCardResult);  //test                                      
                    }
                    else if (_TicketAndCardResult == TicketAndCardResult.Read_and_Verify_Fail)
                    {

                        TextCore.ACTION(TextCore.ACTIONS.USER, "TicketCardDevice|readingTicketCardEnd", "카드/티켓 넣음");
                        return new Result(false, (int)TicketAndCardResult.Read_and_Verify_Fail);
                        //return new Result(true, Result.ReadingTypes.DiscountTIcket, "20130225175208003NN01140110"); // 이노위해 수정

                    }

                    return new Result(false, (int)_TicketAndCardResult);  //test     

                }

            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "TicketCardDevice|readingTicketCardEnd", "ticket정보:" + ticketInfo + " 에러메세지:" + ex.ToString());
                return new Result(false, (int)TicketAndCardResult.No_TICEKT);
            }
            finally
            {

                if (readingSuccess)
                {
                    TextCore.ACTION(TextCore.ACTIONS.CARDREADER1, "TicketCardDevice|readingTicketCardEnd", "카드/티켓 읽음 종료");
                }
            }
        }

        public byte[] Decode(byte[] packet)
        {
            var i = packet.Length - 1;
            while (packet[i] == 0)
            {
                --i;
            }
            var temp = new byte[i + 1];
            Array.Copy(packet, temp, i + 1);
            return temp;
        }

        public int TIcketFrontEject()
        {
            int status = TicketExit();
            return status;
        }


        public int TIcketBackEject()
        {
            int status = TicketCapture();
            return status;
        }

        public class TicketCardDeviceStatusManageMent
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
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, statusCode);
                            isSuccess = true;
                        }
                        else if (printerItem["ISSUCCESS"].ToString() == CommProtocol.DeviceStatus.Fail.ToString())
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, statusCode);
                            isSuccess = false;
                        }

                        switch ((TicketAndCardResult)statusCode)
                        {
                            case TicketAndCardResult.Device_not_found:
                                mIsDeviceError = !isSuccess;
                                break;

                            case TicketAndCardResult.Device_Timeout:
                                mIsComuniCationError = !isSuccess;
                                break;
                            case TicketAndCardResult.Device_not_opened:
                                mIsPortOpenError = !isSuccess;
                                break;

                            case TicketAndCardResult.Invalid_handle:
                                mIsInvalid_handleError = !isSuccess;
                                break;
                            case TicketAndCardResult.Invalid_Write_Data:
                                mIsInvalid_Write_DataError = !isSuccess;
                                break;
                            case TicketAndCardResult.IO_error:
                                mIsIO_Error = !isSuccess;
                                break;
                            case TicketAndCardResult.Module_Alarm:
                                mIsModule_AlarmError = !isSuccess;
                                break;
                            case TicketAndCardResult.Response_frame_size_error:
                                mIsResponseFrameSizeError = !isSuccess;
                                break;
                            case TicketAndCardResult.Ticket_Jam:
                                mIsTicket_JamError = !isSuccess;
                                break;
                            case TicketAndCardResult.Ticket_Reject:
                                mIsTicket_RejectError = !isSuccess;
                                break;
                            case TicketAndCardResult.Ticket_Reject_Error:
                                mIsTicket_RejectError_Error = !isSuccess;
                                break;
                            case TicketAndCardResult.Unknown_Error1:
                                mIsUnknown_Error1 = !isSuccess;
                                break;
                            case TicketAndCardResult.Unknown_Error2:
                                mIsUnknown_Error2 = !isSuccess;
                                break;
                            case TicketAndCardResult.Unknown_Error3:
                                mIsUnknown_Error3 = !isSuccess;
                                break;
                            case TicketAndCardResult.Unknown_Error4:
                                mIsUnknown_Error4 = !isSuccess;
                                break;
                        }
                    }
                    NPSYS.Device.gIsUseMagneticReaderDevice = GetTIcketreaderDeveiceOpertationYn();
                }
                else
                {

                    mIsPortOpenError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Device_not_opened);

                    mIsComuniCationError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Device_Timeout);

                    mIsDeviceError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Device_not_found);

                    mIsInvalid_handleError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Invalid_handle);

                    mIsInvalid_Write_DataError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Invalid_Write_Data);

                    mIsIO_Error = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.IO_error);

                    mIsModule_AlarmError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Module_Alarm);

                    mIsPortOpenError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Device_not_opened);

                    mIsResponseFrameSizeError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Response_frame_size_error);

                    mIsTicket_JamError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Ticket_Jam);

                    mIsTicket_RejectError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Ticket_Reject);

                    mIsTicket_RejectError_Error = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Ticket_Reject_Error);

                    mIsUnknown_Error1 = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Unknown_Error1);

                    mIsUnknown_Error2 = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Unknown_Error2);

                    mIsUnknown_Error3 = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Unknown_Error3);

                    mIsUnknown_Error4 = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Unknown_Error4);



                    NPSYS.Device.gIsUseMagneticReaderDevice = GetTIcketreaderDeveiceOpertationYn();
                }
            }


            public void SendAllDeviveOk()
            {
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Device_not_opened);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Device_Timeout);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Device_not_found);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Invalid_handle);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Invalid_Write_Data);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.IO_error);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Module_Alarm);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Device_not_opened);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Response_frame_size_error);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Ticket_Jam);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Ticket_Reject);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Ticket_Reject_Error);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Unknown_Error1);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Unknown_Error2);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Unknown_Error3);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Unknown_Error4);

            }

            public void SetDeviceStatus(TicketAndCardResult pResultType, bool isSuccess)
            {
                switch (pResultType)
                {
                    case TicketAndCardResult.OK:
                    case TicketAndCardResult.No_TICEKT:
                    case TicketAndCardResult.Empy:
                    case TicketAndCardResult.No_Return_Sensor_Ticket:
                    case TicketAndCardResult.No_Ready_Ticket:
                    case TicketAndCardResult.Read_and_Verify_Fail:
                    case TicketAndCardResult.TicketAndCardEntry:
                        SetComuniCationOk(true);
                        SetDeviceOk(true);
                        SetInvalid_handleStatusOk(true);
                        SetInvalid_Write_DataStatusOk(true);
                        SetIO_ErrorStatusOk(true);
                        SetModule_AlarmStatusOk(true);
                        SetPortOpenOk(true);
                        SetResponseFrameSizeStatusOk(true);
                        SetTicket_JamStatusOk(true);
                        SetTicket_RejectErrorStatusOk(true);
                        SetTicket_RejectStatusOk(true);
                        SetUnknown_Error1StatusOk(true);
                        SetUnknown_Error2StatusOk(true);
                        SetUnknown_Error3StatusOk(true);
                        SetUnknown_Error4StatusOk(true);
                        break;
                    case TicketAndCardResult.Device_not_found:
                        SetDeviceOk(isSuccess);
                        break;

                    case TicketAndCardResult.Device_not_opened:
                        SetPortOpenOk(isSuccess);
                        break;
                    case TicketAndCardResult.Device_Timeout:
                        SetComuniCationOk(isSuccess);
                        break;
                    case TicketAndCardResult.Invalid_handle:
                        SetInvalid_handleStatusOk(isSuccess);
                        break;
                    case TicketAndCardResult.Invalid_Write_Data:
                        SetInvalid_Write_DataStatusOk(isSuccess);
                        break;
                    case TicketAndCardResult.IO_error:
                        SetIO_ErrorStatusOk(isSuccess);
                        break;
                    case TicketAndCardResult.Module_Alarm:
                        SetModule_AlarmStatusOk(isSuccess);
                        break;
                    case TicketAndCardResult.Response_frame_size_error:
                        SetResponseFrameSizeStatusOk(isSuccess);
                        break;
                    case TicketAndCardResult.Ticket_Jam:
                        SetTicket_JamStatusOk(isSuccess);
                        break;
                    case TicketAndCardResult.Ticket_Reject:
                        SetTicket_RejectStatusOk(isSuccess);
                        break;
                    case TicketAndCardResult.Ticket_Reject_Error:
                        SetTicket_RejectErrorStatusOk(isSuccess);
                        break;
                    case TicketAndCardResult.Unknown_Error1:
                        SetUnknown_Error1StatusOk(isSuccess);
                        break;
                    case TicketAndCardResult.Unknown_Error2:
                        SetUnknown_Error2StatusOk(isSuccess);
                        break;
                    case TicketAndCardResult.Unknown_Error3:
                        SetUnknown_Error3StatusOk(isSuccess);
                        break;
                    case TicketAndCardResult.Unknown_Error4:
                        SetUnknown_Error4StatusOk(isSuccess);
                        break;
                }

                NPSYS.Device.gIsUseMagneticReaderDevice = GetTIcketreaderDeveiceOpertationYn();
            }

            private bool GetTIcketreaderDeveiceOpertationYn()
            {
                if ((mIsComuniCationError
                    || mIsDeviceError
                    || mIsInvalid_handleError
                    || mIsInvalid_Write_DataError
                    || mIsIO_Error
                    || mIsModule_AlarmError
                    || mIsPortOpenError
                    || mIsResponseFrameSizeError
                    || mIsTicket_JamError
                    || mIsTicket_RejectError
                    || mIsTicket_RejectError_Error
                    || mIsUnknown_Error1
                    || mIsUnknown_Error2
                    || mIsUnknown_Error3
                    || mIsUnknown_Error4
                    ) == true)
                {
                    return false;
                }
                return true;
            }

            private bool mIsDeviceError = false;
            public bool IsDeviceError
            {
                get { return mIsDeviceError; }
            }


            private void SetDeviceOk(bool pIsDeviceOk)
            {
                if (mIsDeviceError) // 기존에 통신에러였다면
                {
                    if (pIsDeviceOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Device_not_found);
                        mIsDeviceError = !pIsDeviceOk;

                    }
                }
                else  // 기존에 동전입수가기 정상일때
                {
                    if (pIsDeviceOk == false) // 현재 동전입수기가 정상이라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Device_not_found);

                        mIsDeviceError = !pIsDeviceOk;

                    }
                }

            }

            private bool mIsComuniCationError = false;
            public bool IsComuniCationError
            {
                get { return mIsComuniCationError; }
            }


            private void SetComuniCationOk(bool pIscommunication)
            {
                if (mIsComuniCationError) // 기존에 통신에러였다면
                {
                    if (pIscommunication) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Device_Timeout);
                        mIsComuniCationError = !pIscommunication;

                    }
                }
                else  // 기존에 동전입수가기 정상일때
                {
                    if (pIscommunication == false) // 현재 동전입수기가 정상이라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Device_Timeout);

                        mIsComuniCationError = !pIscommunication;

                    }
                }

            }


            private bool mIsPortOpenError = false;
            public bool IsPortOpenError
            {
                get { return mIsPortOpenError; }
            }


            private void SetPortOpenOk(bool pIsPortOpenOk)
            {
                if (mIsPortOpenError)
                {
                    if (pIsPortOpenOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Device_not_opened);
                        mIsPortOpenError = !pIsPortOpenOk;

                    }
                }
                else
                {
                    if (pIsPortOpenOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Device_not_opened);

                        mIsPortOpenError = !pIsPortOpenOk;

                    }
                }
            }

            private bool mIsResponseFrameSizeError = false;
            public bool IsResponseFrameSizeError
            {
                get { return mIsResponseFrameSizeError; }
            }


            private void SetResponseFrameSizeStatusOk(bool pIsResponseFrameSizeStatusOk)
            {
                if (mIsResponseFrameSizeError)
                {
                    if (pIsResponseFrameSizeStatusOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Response_frame_size_error);
                        mIsResponseFrameSizeError = !pIsResponseFrameSizeStatusOk;

                    }
                }
                else
                {
                    if (pIsResponseFrameSizeStatusOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Response_frame_size_error);

                        mIsResponseFrameSizeError = !pIsResponseFrameSizeStatusOk;

                    }
                }
            }

            private bool mIsModule_AlarmError = false;
            public bool IsModule_AlarmError
            {
                get { return mIsModule_AlarmError; }
            }


            private void SetModule_AlarmStatusOk(bool pIsModule_AlarmStatusOk)
            {
                if (mIsModule_AlarmError)
                {
                    if (pIsModule_AlarmStatusOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Module_Alarm);
                        mIsModule_AlarmError = !pIsModule_AlarmStatusOk;

                    }
                }
                else
                {
                    if (pIsModule_AlarmStatusOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Module_Alarm);

                        mIsModule_AlarmError = !pIsModule_AlarmStatusOk;

                    }
                }
            }


            private bool mIsTicket_JamError = false;
            public bool IsTicket_JamError
            {
                get { return mIsTicket_JamError; }
            }


            private void SetTicket_JamStatusOk(bool pIsTicket_JamStatusOk)
            {
                if (mIsModule_AlarmError)
                {
                    if (pIsTicket_JamStatusOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Ticket_Jam);
                        mIsModule_AlarmError = !pIsTicket_JamStatusOk;

                    }
                }
                else
                {
                    if (pIsTicket_JamStatusOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Ticket_Jam);

                        mIsModule_AlarmError = !pIsTicket_JamStatusOk;

                    }
                }
            }

            private bool mIsTicket_RejectError = false;
            public bool IsTicket_RejectError
            {
                get { return mIsTicket_RejectError; }
            }

            private void SetTicket_RejectStatusOk(bool pIsTicket_RejectStatusOk)
            {
                if (mIsTicket_RejectError)
                {
                    if (pIsTicket_RejectStatusOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Ticket_Reject);
                        mIsTicket_RejectError = !pIsTicket_RejectStatusOk;

                    }
                }
                else
                {
                    if (pIsTicket_RejectStatusOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Ticket_Reject);

                        mIsTicket_RejectError = !pIsTicket_RejectStatusOk;

                    }
                }
            }

            private bool mIsInvalid_Write_DataError = false;
            public bool IsInvalid_Write_DataError
            {
                get { return mIsInvalid_Write_DataError; }
            }



            private void SetInvalid_Write_DataStatusOk(bool pIsInvalid_Write_DataStatusOk)
            {
                if (mIsInvalid_Write_DataError)
                {
                    if (pIsInvalid_Write_DataStatusOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Invalid_Write_Data);
                        mIsInvalid_Write_DataError = !pIsInvalid_Write_DataStatusOk;

                    }
                }
                else
                {
                    if (pIsInvalid_Write_DataStatusOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Invalid_Write_Data);

                        mIsInvalid_Write_DataError = !pIsInvalid_Write_DataStatusOk;

                    }
                }
            }

            private bool mIsTicket_RejectError_Error = false;
            public bool IsTicket_RejectError_Error
            {
                get { return mIsTicket_RejectError_Error; }
            }



            private void SetTicket_RejectErrorStatusOk(bool pIsTicket_RejectStatusOk)
            {
                if (mIsTicket_RejectError_Error)
                {
                    if (pIsTicket_RejectStatusOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Ticket_Reject_Error);
                        mIsTicket_RejectError_Error = !pIsTicket_RejectStatusOk;

                    }
                }
                else
                {
                    if (pIsTicket_RejectStatusOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Ticket_Reject_Error);

                        mIsTicket_RejectError_Error = !pIsTicket_RejectStatusOk;

                    }
                }
            }

            private bool mIsInvalid_handleError = false;
            public bool IsInvalid_handleError
            {
                get { return mIsInvalid_handleError; }
            }


            private void SetInvalid_handleStatusOk(bool pIsInvalid_handleStatusOk)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TicketExit | SetInvalid_handleStatusOk", "현재handle상태:" + pIsInvalid_handleStatusOk.ToString() + " 기존핸들 오류여부:" + mIsInvalid_handleError.ToString());
                if (mIsInvalid_handleError)
                {
                    if (pIsInvalid_handleStatusOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Invalid_handle);
                        mIsInvalid_handleError = !pIsInvalid_handleStatusOk;

                    }
                }
                else
                {
                    if (pIsInvalid_handleStatusOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Invalid_handle);

                        mIsInvalid_handleError = !pIsInvalid_handleStatusOk;

                    }
                }
            }

            private bool mIsIO_Error = false;
            public bool IsIO_Error
            {
                get { return mIsIO_Error; }
            }

            private void SetIO_ErrorStatusOk(bool pIsIO_ErrorStatusOk)
            {
                if (mIsIO_Error)
                {
                    if (pIsIO_ErrorStatusOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.IO_error);
                        mIsIO_Error = !pIsIO_ErrorStatusOk;

                    }
                }
                else
                {
                    if (pIsIO_ErrorStatusOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.IO_error);

                        mIsIO_Error = !pIsIO_ErrorStatusOk;

                    }
                }
            }


            private bool mIsUnknown_Error1 = false;
            public bool IsUnknown_Error1
            {
                get { return mIsUnknown_Error1; }
            }

            private void SetUnknown_Error1StatusOk(bool pIsUnknown_Error1StatusOk)
            {
                if (mIsUnknown_Error1)
                {
                    if (pIsUnknown_Error1StatusOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Unknown_Error1);
                        mIsUnknown_Error1 = !pIsUnknown_Error1StatusOk;

                    }
                }
                else
                {
                    if (pIsUnknown_Error1StatusOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Unknown_Error1);

                        mIsUnknown_Error1 = !pIsUnknown_Error1StatusOk;

                    }
                }
            }


            private bool mIsUnknown_Error2 = false;
            public bool IsUnknown_Error2
            {
                get { return mIsUnknown_Error2; }
            }

            private void SetUnknown_Error2StatusOk(bool pIsUnknown_Error2StatusOk)
            {
                if (mIsUnknown_Error2)
                {
                    if (pIsUnknown_Error2StatusOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Unknown_Error2);
                        mIsUnknown_Error2 = !pIsUnknown_Error2StatusOk;

                    }
                }
                else
                {
                    if (pIsUnknown_Error2StatusOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Unknown_Error2);

                        mIsUnknown_Error2 = !pIsUnknown_Error2StatusOk;

                    }
                }
            }


            private bool mIsUnknown_Error3 = false;
            public bool IsUnknown_Error3
            {
                get { return mIsUnknown_Error3; }
            }


            private void SetUnknown_Error3StatusOk(bool pIsUnknown_Error3StatusOk)
            {
                if (mIsUnknown_Error3)
                {
                    if (pIsUnknown_Error3StatusOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Unknown_Error3);
                        mIsUnknown_Error3 = !pIsUnknown_Error3StatusOk;

                    }
                }
                else
                {
                    if (pIsUnknown_Error3StatusOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Unknown_Error3);

                        mIsUnknown_Error3 = !pIsUnknown_Error3StatusOk;

                    }
                }
            }

            private bool mIsUnknown_Error4 = false;
            public bool IsUnknown_Error4
            {
                get { return mIsUnknown_Error4; }
            }
            private void SetUnknown_Error4StatusOk(bool pIsUnknown_Error4StatusOk)
            {
                if (mIsUnknown_Error4)
                {
                    if (pIsUnknown_Error4StatusOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Success, (int)TicketAndCardResult.Unknown_Error4);
                        mIsUnknown_Error4 = !pIsUnknown_Error4StatusOk;

                    }
                }
                else
                {
                    if (pIsUnknown_Error4StatusOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.MAR, CommProtocol.DeviceStatus.Fail, (int)TicketAndCardResult.Unknown_Error4);

                        mIsUnknown_Error4 = !pIsUnknown_Error4StatusOk;

                    }
                }
            }



        }

    }
}
