using System;
using System.Data;

namespace NPCommon.Van
{
    public class CreditReaderStatusManageMent
    {
        public enum CREDITReaderStatusType
        {
            DeviceStatus = 3101,
            CommuniCationStatus = 3102,
            PortOpenStatus = 3103,
            OK = 3,

        }

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
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, statusCode);
                    }
                    else if (printerItem["ISSUCCESS"].ToString() == CommProtocol.DeviceStatus.Fail.ToString())
                    {
                        isSuccess = false;
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, statusCode);
                    }

                    switch ((CREDITReaderStatusType)statusCode)
                    {
                        case CREDITReaderStatusType.CommuniCationStatus:
                            mIsComuniCationError = !isSuccess;

                            break;
                        case CREDITReaderStatusType.DeviceStatus:
                            mIsDeviceError = !isSuccess;

                            break;

                        case CREDITReaderStatusType.PortOpenStatus:
                            mIsPortOpenError = !isSuccess;

                            break;



                    }
                }
                NPSYS.Device.gIsUseCreditCardDevice = GetCreditReaderDeveiceOpertationYn();
            }
            else
            {
                mIsComuniCationError = false;
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CDR, CommProtocol.DeviceStatus.Success, (int)CREDITReaderStatusType.CommuniCationStatus);

                mIsDeviceError = false;
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CDR, CommProtocol.DeviceStatus.Success, (int)CREDITReaderStatusType.DeviceStatus);



                mIsPortOpenError = false;
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CDR, CommProtocol.DeviceStatus.Success, (int)CREDITReaderStatusType.PortOpenStatus);




                NPSYS.Device.gIsUseCreditCardDevice = GetCreditReaderDeveiceOpertationYn();
            }
        }

        public void SendAllDeviveOk()
        {
            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CDR, CommProtocol.DeviceStatus.Success, (int)CREDITReaderStatusType.CommuniCationStatus);

            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CDR, CommProtocol.DeviceStatus.Success, (int)CREDITReaderStatusType.DeviceStatus);

            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CDR, CommProtocol.DeviceStatus.Success, (int)CREDITReaderStatusType.PortOpenStatus);

        }

        public void SetCreditReaderDeviceStatus(CREDITReaderStatusType pResultType, bool isSuccess)
        {
            switch (pResultType)
            {
                case CREDITReaderStatusType.CommuniCationStatus:
                    SetComuniCationOk(isSuccess);
                    break;
                case CREDITReaderStatusType.DeviceStatus:
                    SetDeviceOk(isSuccess);
                    break;

                case CREDITReaderStatusType.OK:
                    SetComuniCationOk(isSuccess);
                    SetDeviceOk(isSuccess);
                    SetPortOpenOk(isSuccess);
                    break;

                case CREDITReaderStatusType.PortOpenStatus:
                    SetPortOpenOk(isSuccess);
                    break;


            }

            NPSYS.Device.gIsUseCreditCardDevice = GetCreditReaderDeveiceOpertationYn();
        }

        private bool GetCreditReaderDeveiceOpertationYn()
        {
            if ((mIsComuniCationError
                || mIsDeviceError
                || mIsPortOpenError
                ) == true)
            {
                return false;
            }
            return true;
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
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CDR, CommProtocol.DeviceStatus.Success, (int)CREDITReaderStatusType.CommuniCationStatus);
                    mIsComuniCationError = !pIscommunication;

                }
            }
            else  // 기존에 정상일때
            {
                if (pIscommunication == false) // 현재 장비가 에러라면
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CDR, CommProtocol.DeviceStatus.Fail, (int)CREDITReaderStatusType.CommuniCationStatus);

                    mIsComuniCationError = !pIscommunication;

                }
            }

        }




        private bool mIsDeviceError = false;
        public bool IsDeviceError
        {
            get { return mIsDeviceError; }
        }


        private void SetDeviceOk(bool pIsDeviceOk)
        {
            if (mIsDeviceError) // 기존에 에러였다면
            {
                if (pIsDeviceOk) // 정상상태로 바뀌면
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CDR, CommProtocol.DeviceStatus.Success, (int)CREDITReaderStatusType.DeviceStatus);
                    mIsDeviceError = !pIsDeviceOk;

                }
            }
            else  // 기존에  정상일때
            {
                if (pIsDeviceOk == false) // 현재 장비가 에러라면
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CDR, CommProtocol.DeviceStatus.Fail, (int)CREDITReaderStatusType.DeviceStatus);

                    mIsDeviceError = !pIsDeviceOk;

                }
            }

        }

        private bool mIsDisableError = false;


        private bool mIsPortOpenError = false;
        public bool IsPortOpenError
        {
            get { return mIsPortOpenError; }
        }


        private void SetPortOpenOk(bool pIsPortOpenOk)
        {
            if (mIsPortOpenError) // 기존에 에러였다면
            {
                if (pIsPortOpenOk) // 정상상태로 바뀌면
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CDR, CommProtocol.DeviceStatus.Success, (int)CREDITReaderStatusType.PortOpenStatus);
                    mIsPortOpenError = !pIsPortOpenOk;

                }
            }
            else  // 기존에  정상일때
            {
                if (pIsPortOpenOk == false) // 현재 장비가 에러라면
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CDR, CommProtocol.DeviceStatus.Fail, (int)CREDITReaderStatusType.PortOpenStatus);

                    mIsPortOpenError = !pIsPortOpenOk;

                }
            }
        }



    }
}
