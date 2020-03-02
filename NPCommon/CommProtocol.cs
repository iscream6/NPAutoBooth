// PROGRAM ID		: CommProtocol
// PROGRAM NAME		: Protocol
// PROGRAM DETAIL   : 공통적인 통신 코드들을 정리
// CREATED BY		:
// CREATION DATE	: 2011.06.21
//*****************************************************
//*****************************************************
//*****************************************************
using FadeFox.Text;
using System;

namespace NPCommon
{
    /// <summary>
    /// 클라이언트와 알람등을 전송하는 프로토콜 정의
    /// </summary>
    public class CommProtocol
    {
        /// <summary>
        /// BRE 지폐리더기, CRE 동전리더기,TRE 교통카드리더기,BCH 지폐방출기,_CC1_동전방출기,CA1 카드리더기 , DID 도어신호 , REP 영수증프린터
        /// </summary>
        public enum device
        {
            /// <summary>
            /// 무인정산기
            /// </summary>
            Booth = 0,

            /// <summary>
            /// 지폐리더기
            /// </summary>
            BRE = 21,

            /// <summary>
            /// 동전리더기
            /// </summary>
            CRE = 23,

            TRE,

            /// <summary>
            /// 지폐방출기
            /// </summary>
            BCH = 22,

            /// <summary>
            /// 동전방출기1
            /// </summary>
            CC1 = 25,

            /// <summary>
            /// 동전방출기2
            /// </summary>
            CC2 = 26,

            /// <summary>
            /// 동전방출기3
            /// </summary>
            CC3 = 27,

            /// <summary>
            /// 마그네틱리더기
            /// </summary>
            MAR = 19,

            /// <summary>
            /// 신용카드리더기
            /// </summary>
            CDR = 31,

            /// <summary>
            /// 도어
            /// </summary>
            DID = 39,

            /// <summary>
            /// 영수증프린터
            /// </summary>
            REP = 6,

            /// <summary>
            /// 출구무인정산기 24시간 무가동 체크
            /// </summary>
            APS = 13
        }

        public static string RestErrorCode(device deviceName, object ErrorCode)
        {
            string returnErrorCode = TextCore.ToRightAlignString(4, ((int)ErrorCode).ToString(), '0');
            return returnErrorCode;
        }

        public enum DeviceStatus
        {
            Success = 2,
            Fail = 1,
            NotUse = 3
        }

        /// <summary>
        /// 장비 장애 정보를 Server 로 보내기 위한 Dictionary(gDic_DeviceStatusManageMent) 셋팅
        /// </summary>
        /// <param name="pDeviceCode"></param>
        /// <param name="pDeviceStatus"></param>
        /// <param name="pStatus"></param>
        public static void MakeDevice_RestfulStatus(device pDeviceCode, DeviceStatus pDeviceStatus, object pStatus)
        {
            try
            {
                SendDeviceErrorThread.ErrorSendManualEvent.WaitOne(10000);
                SendDeviceErrorThread.ErrorSendManualEvent.Reset();

                int isDeviceStatus = 0;

                string niceDeviceCode = string.Empty;
                string koreaDevicename = string.Empty;
                switch (pDeviceCode)
                {
                    case device.DID:
                        koreaDevicename = "도어";

                        break;

                    case device.BCH:
                        koreaDevicename = "지폐방출기";

                        break;

                    case device.BRE:
                        koreaDevicename = "지폐리더기";

                        break;

                    case device.MAR:
                        koreaDevicename = "마그네틱리더기";

                        break;

                    case device.CDR:
                        koreaDevicename = "신용카드리더기";
                        break;

                    case device.CC1:
                        koreaDevicename = "동전방출기50";

                        break;

                    case device.CC2:
                        koreaDevicename = "동전방출기100";
                        break;

                    case device.CC3:
                        koreaDevicename = "동전방출기500";
                        break;

                    case device.CRE:
                        koreaDevicename = "동전리더기";
                        break;

                    case device.TRE:
                        koreaDevicename = "교통카드리더기";
                        break;

                    case device.REP:
                        koreaDevicename = "영수증프린터";
                        break;

                    case device.Booth:
                        koreaDevicename = "무인정산기";
                        break;

                    case device.APS: 
                        koreaDevicename = "출구무인정산기";
                        break;
                }
                string key = DateTime.Now.ToString("yyyyMMddHHmmssfff") + RestErrorCode(pDeviceCode, pStatus);
                bool isExistStatus = LPRDbSelect.IsDeivceErrorInfoFromStatusCode(RestErrorCode(pDeviceCode, pStatus)); // 기존에 동일장애코드가 있는지 확인
                if (isExistStatus)  // 있다면 없데이트
                {
                    LPRDbSelect.UpdateErrorInfo(RestErrorCode(pDeviceCode, pStatus), pDeviceStatus.ToString());
                }
                else
                {
                    LPRDbSelect.InsertErrorInfo(pDeviceCode, RestErrorCode(pDeviceCode, pStatus), pDeviceStatus.ToString());
                }
                if (NPSYS.gDic_DeviceStatusManageMent != null)
                {
                    string lDeviceCode = string.Empty;
                    string lDeviceName = string.Empty;
                    string lDeviceStatus = string.Empty;
                    string lerrorcode = RestErrorCode(pDeviceCode, pStatus);

                    try
                    {
                        DeviceStatusManagement currentstatusManageMent = new DeviceStatusManagement();
                        currentstatusManageMent.DeviceCode = niceDeviceCode;
                        currentstatusManageMent.DeviceName = koreaDevicename;
                        currentstatusManageMent.DeviceStatus = (int)pDeviceStatus;
                        currentstatusManageMent.ErrorCode = RestErrorCode(pDeviceCode, pStatus);
                        currentstatusManageMent.UpdateFlag = false;
                        NPSYS.gDic_DeviceStatusManageMent.Add(key, currentstatusManageMent);
                    }
                    catch (Exception pException)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CommProtocol | MakeDevice_RestfulStatus", "오류내용:" + pException.ToString()
                            + " DeviceCode:" + lDeviceCode
                            + " DeviceName:" + lDeviceName
                            + " DeviceStatus:" + lDeviceStatus.ToString()
                            + " ErrorCode:" + lerrorcode);
                    }
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CommProtocol | MakeDevice_RestfulStatus", ex.ToString());
            }
            finally
            {
                SendDeviceErrorThread.ErrorSendManualEvent.Set();
            }
        }
    }
}