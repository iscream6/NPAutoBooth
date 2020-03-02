using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FadeFox;
using FadeFox.Text;
using NPCommon;
using System.Threading;
using NPCommon.DTO;
using NPCommon.Van;

namespace NPAutoBooth.Common
{
    /// <summary>
    /// 카드 및 현금 영수증을 처리
    /// </summary>
    public class PayCardandCash
    {
        public Result CreditCardPayResult(string pCrditCardInfo, NormalCarInfo pNormalCarInfo)
        {

            TextCore.INFO(TextCore.INFOS.CARD, "FormPaymentMenu|CreditCardPayResult", "카드 결제시작");
            string terminalNo = NPSYS.Config.GetValue(ConfigID.FeatureSettingCreditCardTerminalNo).ToUpper().Trim();
            int Creditpaymoneys = pNormalCarInfo.PaymentMoney;

            //double tax = ((long)(Math.Floor((decimal)(Creditpaymoneys / 1.1))));
            int taxsResult = (int)(Creditpaymoneys / 11);
            int SupplyPay = Creditpaymoneys - Convert.ToInt32(taxsResult); //공급가


            try
            {
                //KOCSE 카드리더기 추가
                if (pCrditCardInfo.Trim().Length>0)
                { 
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|CreditCardPayResult", "카드 정보:" + pCrditCardInfo.Split('=')[0].Substring(0, 4) + "-" + pCrditCardInfo.Split('=')[0].Substring(4, 4) + "-****-****");
                }
                //KOCSE 카드리더기 추가 주석
                //if (!NPSYS.isBoothRealMode)
                //{
                //    string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //    pNormalCarInfo.PaymentMethod = NormalCarInfo.PaymentType.CreditCard;
                //    LPRDbSelect.LogMoney(logDate, pNormalCarInfo, MoneyType.CreditCard, pNormalCarInfo.PaymentMoney, 0, "");
                //    pNormalCarInfo.VanCheck = 1;
                //    pNormalCarInfo.VanCardNumber = "1234";
                //    pNormalCarInfo.VanRegNo = "1111";
                //    pNormalCarInfo.VanDate = "201404";
                //    pNormalCarInfo.VanRescode = "0000";
                //    pNormalCarInfo.VanResMsg = "성공";
                //    pNormalCarInfo.VanSupplyPay = Convert.ToInt32(SupplyPay);
                //    pNormalCarInfo.VanTaxPay = Convert.ToInt32(taxsResult);
                //    pNormalCarInfo.VanCardName = "하나SK카드";
                //    pNormalCarInfo.VanBeforeCardPay = pNormalCarInfo.PaymentMoney;
                //    pNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash("20" + "140404");
                //    pNormalCarInfo.VanCardApproveHms = DateTime.Now.ToString("HH:mm:ss");
                //    pNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash("20" + "140404");
                //    pNormalCarInfo.VanCardApprovalHms = DateTime.Now.ToString("HH:mm:ss");
                //    pNormalCarInfo.VanIssueCode = "01";
                //    pNormalCarInfo.VanIssueName = "하나은행";
                //    pNormalCarInfo.VanCardAcquirerCode = "11";
                //    pNormalCarInfo.VanCardAcquirerName = "성공";
                     
                //    LPRDbSelect.Creditcard_Log_INsert(pNormalCarInfo);

                //    //LPRDbSelect.SaveCardPay(pNormalCarInfo);
                //    // 카드결제후 무언가 완료된거를 보내야한다


                //    pNormalCarInfo.VanAmt = pNormalCarInfo.PaymentMoney;
                //    TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|CreditCardPayResult", "카드 결제성공");
                //    return new Result(true, "성공");
                //}
                if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.VAN_FIRSTDATA)
                {
                    Thread.Sleep(2000);
                    CreditAuthSimpleExData result = FirstData.CreditAuthSimpleEx(terminalNo, pCrditCardInfo, "S", pNormalCarInfo.PaymentMoney.ToString(), taxsResult.ToString());
                    if (result == null || result.ResCode.Trim() == "")
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                        TextCore.DeviceError(TextCore.DEVICE.TCPIP, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                        return new Result(false, "신용카드 서버 접속안됨");
                    }
                    string returnCode = result.ResCode;
                    if (returnCode == FirstData.Success)
                    {

                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        LPRDbSelect.LogMoney(PaymentType.CreditCard,logDate, pNormalCarInfo, MoneyType.CreditCard, pNormalCarInfo.PaymentMoney, 0, "");
                        string[] lCardNumData = pCrditCardInfo.Split('=');

                        pNormalCarInfo.VanCardNumber = lCardNumData[0].Substring(0, 4) + "-" + lCardNumData[0].Substring(4, 4) + "-****-" + lCardNumData[0].Substring(12);
                        pNormalCarInfo.VanRegNo = result.AuthNumber.Trim();
                        pNormalCarInfo.VanDate = result.AuthDate.Trim();
                        pNormalCarInfo.VanRescode = returnCode.Trim();
                        pNormalCarInfo.VanResMsg = result.ResMsg.Trim();
                        pNormalCarInfo.VanSupplyPay = Convert.ToInt32(SupplyPay);
                        pNormalCarInfo.VanTaxPay = Convert.ToInt32(taxsResult);
                        pNormalCarInfo.VanCardName = result.CardName.Trim();
                        pNormalCarInfo.VanBeforeCardPay = pNormalCarInfo.PaymentMoney;
                        pNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash("20" + result.AuthDate);
                        pNormalCarInfo.VanCardApproveHms = DateTime.Now.ToString("HH:mm:ss");
                        pNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash("20" + result.AuthDate);
                        pNormalCarInfo.VanCardApprovalHms = DateTime.Now.ToString("HH:mm:ss");
                        pNormalCarInfo.VanIssueCode = result.IssuerCode.Trim();
                        pNormalCarInfo.VanIssueName = result.IssuerName.Trim();
                        pNormalCarInfo.VanCardAcquirerCode = result.AcquirerCode;
                        pNormalCarInfo.VanCardAcquirerName = result.AcquirerName;

                        LPRDbSelect.Creditcard_Log_INsert(pNormalCarInfo);
                        //LPRDbSelect.SaveCardPay(pNormalCarInfo);
                        // 카드결제후 무언가 완료된거를 보내야한다
                        pNormalCarInfo.VanAmt = pNormalCarInfo.PaymentMoney;
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|CreditCardPayResult", "카드 결제성공");
                        return new Result(true, "성공");

                    }
                    else
                    {

                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        pNormalCarInfo.VanCardNumber = pCrditCardInfo;
                        pNormalCarInfo.VanRegNo = result.AuthNumber;
                        pNormalCarInfo.VanDate = result.AuthDate;
                        pNormalCarInfo.VanRescode = returnCode;
                        pNormalCarInfo.VanResMsg = result.ResMsg;
                        pNormalCarInfo.VanSupplyPay = 0;
                        pNormalCarInfo.VanTaxPay = 0;
                        pNormalCarInfo.VanBeforeCardPay = pNormalCarInfo.PaymentMoney;
                        pNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash("20" + result.AuthDate);
                        pNormalCarInfo.VanCardApproveHms = DateTime.Now.ToString("HH:mm:ss");
                        pNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash("20" + result.AuthDate);
                        pNormalCarInfo.VanCardApprovalHms = DateTime.Now.ToString("HH:mm:ss");
                        pNormalCarInfo.VanIssueCode = result.IssuerCode.Trim();
                        pNormalCarInfo.VanIssueName = result.IssuerName.Trim();
                        pNormalCarInfo.VanCardAcquirerCode = result.AcquirerCode;
                        pNormalCarInfo.VanCardAcquirerName = result.AcquirerName;
                        LPRDbSelect.Creditcard_Log_INsert(pNormalCarInfo);
                        //LPRDbSelect.SaveCardPay(pNormalCarInfo);
                        // 카드결제후 무언가 완료된거를 보내야한다
                        pNormalCarInfo.VanAmt = 0;
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "카드 결제실패:" + pNormalCarInfo.VanResMsg);
                        return new Result(false, pNormalCarInfo.VanRescode + ":" + pNormalCarInfo.VanResMsg);

                    }
                }
                //KOCSE 카드리더기 추가
                
                else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KOCES_TCM|| NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KOCES_PAYMGATE)
                {
                    KocesTcmMotor.CreditAuthSimpleExData result = null;

                    if (pNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                        || pNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                    {
                        //카드취소 CreditAuthSimpleExCancle
                        result = KocesTcmMotor.CreditAuthSimpleExCancle(Creditpaymoneys, taxsResult, pNormalCarInfo.VanRegNo_Cancle.ToString(), pNormalCarInfo.VanDate_Cancle.Replace("-", ""));
                    }
                    else
                    {
                        //카드승인 CreditAuthSimpleEx
                        result = KocesTcmMotor.CreditAuthSimpleEx(Creditpaymoneys, taxsResult);
                    }


                    //KocesTcmMotor.CreditAuthSimpleExData result = KocesTcmMotor.CreditAuthSimpleEx(Creditpaymoneys, taxsResult);
                    if (result == null || result.ResCode.Trim() == "")
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                        TextCore.DeviceError(TextCore.DEVICE.TCPIP, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                        return new Result(false, "신용카드 서버 접속안됨");
                    }
                    string returnCode = result.ResCode;
                    if (returnCode == "0000")
                    {
                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        
                        LPRDbSelect.LogMoney(PaymentType.CreditCard,logDate, pNormalCarInfo, MoneyType.CreditCard, pNormalCarInfo.PaymentMoney, 0, "");

                        pNormalCarInfo.VanCheck = 1;
                        pNormalCarInfo.VanRegNo = result.AuthNumber.Trim();
                        pNormalCarInfo.VanDate = result.AuthDate.Trim();
                        pNormalCarInfo.VanRescode = returnCode.Trim();
                        pNormalCarInfo.VanResMsg = result.ResMsg.Trim();
                        pNormalCarInfo.VanSupplyPay = Convert.ToInt32(SupplyPay);
                        pNormalCarInfo.VanTaxPay = Convert.ToInt32(taxsResult);
                        pNormalCarInfo.VanCardName = result.CardName.Trim();
                        pNormalCarInfo.VanBeforeCardPay = pNormalCarInfo.PaymentMoney;
                        pNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(result.AuthDate);
                        pNormalCarInfo.VanCardApproveHms = NPSYS.ConvetDay_Dash(result.AuthTime);
                        pNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(result.AuthTime);
                        pNormalCarInfo.VanCardApprovalHms = DateTime.Now.ToString("HH:mm:ss");
                        //pNormalCarInfo.CardDDDNumber = result.DdcNumber.Trim();
                        pNormalCarInfo.VanIssueCode = result.IssuerCode.Trim();
                        pNormalCarInfo.VanIssueName = result.IssuerName.Trim();
                        pNormalCarInfo.VanCardAcquirerCode = result.AcquirerCode;
                        pNormalCarInfo.VanCardAcquirerName = result.AcquirerName;
                        pNormalCarInfo.VanCardNumber = result.CardNo;
                        LPRDbSelect.Creditcard_Log_INsert(pNormalCarInfo);
                        //LPRDbSelect.SaveCardPay(pNormalCarInfo);
                        // 카드결제후 무언가 완료된거를 보내야한다
                        pNormalCarInfo.VanAmt = pNormalCarInfo.PaymentMoney;
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|CreditCardPayResult", "카드 결제성공");
                        return new Result(true, "성공");

                    }
                    else
                    {

                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        pNormalCarInfo.VanCheck = 2;
                        pNormalCarInfo.VanRescode = returnCode;
                        pNormalCarInfo.VanAmt = 0;
                        pNormalCarInfo.VanResMsg = result.ResMsg.Trim();
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "카드 결제실패:" + pNormalCarInfo.VanResMsg);
                        return new Result(false, pNormalCarInfo.VanRescode + ":" + pNormalCarInfo.VanResMsg);

                    }
                }
                //KOCSE 카드리더기 추가 주석
 

                //KICC DIP적용

                else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KICC_DIP_IFM)
                {
                    if (pNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                       || pNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                    {
                        NPSYS.Device.KICC_TIT.SendData_D4(Creditpaymoneys.ToString(),pNormalCarInfo.VanDate_Cancle.Replace("-","").Substring(2,6) ,pNormalCarInfo.VanRegNo_Cancle, "10");

                    }
                    else
                    {
                        NPSYS.Device.KICC_TIT.SendData_D1(Creditpaymoneys.ToString(), "0", "10");
                    }
                    KICC_TIT.KICC_TIT_RECV_SUCCESS result = NPSYS.Device.KICC_TIT.GetRecvData();
                    if (result == null || result.SUC.Trim() == "")
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                        TextCore.DeviceError(TextCore.DEVICE.TCPIP, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                        //TMAP연동
                        pNormalCarInfo.VanRescode = KICC_TIT.KICC_USER_CANCLECODE;
                        //TMAP연동 완료
                        return new Result(false, "신용카드 서버 접속안됨");
                    }
                    string returnCode = result.RS04;
                    if (result.SUC == "00" && returnCode == "0000")
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "PayCardandCash | CreditCardPayResult", "[카드결제성공]");

                       string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        LPRDbSelect.LogMoney(PaymentType.CreditCard,logDate, pNormalCarInfo, MoneyType.CreditCard, pNormalCarInfo.PaymentMoney, 0, "");
                        pNormalCarInfo.VanCheck = 1;
                        string[] lCardNumData = result.RQ04.Split('=');
                        if (lCardNumData[0].Length > 13)
                        {
                            pNormalCarInfo.VanCardNumber = lCardNumData[0].Substring(0, 4) + "-" + lCardNumData[0].Substring(4, 4) + "-" + lCardNumData[0].Substring(8, 4) + "-" + lCardNumData[0].Substring(12);
                        }
                        else
                        {
                            pNormalCarInfo.VanCardNumber = lCardNumData[0];
                        }
                        //pNormalCarInfo.CardNumber = result.CardNo;
                        pNormalCarInfo.VanRegNo = result.RS09.Trim();
                        pNormalCarInfo.VanDate = "20" + result.RS07.Substring(0, 6).Trim();
                        pNormalCarInfo.VanRescode = returnCode.Trim();
                        pNormalCarInfo.VanResMsg = result.RS16.Trim();
                        pNormalCarInfo.VanSupplyPay = Convert.ToInt32(SupplyPay);
                        pNormalCarInfo.VanTaxPay = Convert.ToInt32(taxsResult);
                        pNormalCarInfo.VanCardName = result.RS12.Trim();
                        pNormalCarInfo.VanBeforeCardPay = pNormalCarInfo.PaymentMoney;
                        pNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash("20" + result.RS07.Substring(0, 6));
                        pNormalCarInfo.VanCardApproveHms = NPSYS.ConvetDay_Dash(result.RS07.Substring(6, 6));
                        pNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash("20" + result.RS07.Substring(0, 6));
                        pNormalCarInfo.VanCardApprovalHms = NPSYS.ConvetDay_Dash(result.RS07.Substring(6, 6));
                        pNormalCarInfo.VanIssueCode = result.RS11.Trim();
                        pNormalCarInfo.VanIssueName = result.RS12.Trim();
                        pNormalCarInfo.VanCardAcquirerCode = result.RS05.Trim();
                        pNormalCarInfo.VanCardAcquirerName = result.RS14.Trim();

                        string cardActionName = string.Empty;
                        // 카드취소
                        if (pNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                        || pNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                        {
                            cardActionName = "카드취소성공";
                        }
                        else
                        {
                            cardActionName = "카드결제성공";
                        }
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "PayCardandCash | CreditCardPayResult", "[" + cardActionName + "]"
                                                              + " [카드번호]" + pNormalCarInfo.VanRegNo
                                                              + " [응답메세지]" + pNormalCarInfo.VanResMsg
                                                              + " [응답코드]" + pNormalCarInfo.VanRescode
                                                              + " [카드명]" + pNormalCarInfo.VanCardName
                                                              + " [승인번호]" + pNormalCarInfo.VanRegNo
                                                              + " [승인일자]" + pNormalCarInfo.VanCardApproveYmd
                                                              + " [승인시간]" + pNormalCarInfo.VanCardApproveHms);

                        LPRDbSelect.Creditcard_Log_INsert(pNormalCarInfo);

                        pNormalCarInfo.VanAmt = pNormalCarInfo.PaymentMoney;
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|CreditCardPayResult", "카드 결제성공");
                        return new Result(true, "성공");

                    }
                    else
                    {
                        pNormalCarInfo.VanCheck = 2;
                        // syc 01  USER CANCLE
                        // suc 00 rescode 8037
                        
                        if (returnCode != string.Empty)
                        {
                            //카드실패전송
                            string[] lCardNumData = result.RQ04.Split('=');
                            if (lCardNumData[0].Length > 13)
                            {
                                pNormalCarInfo.VanCardNumber = lCardNumData[0].Substring(0, 4) + "-" + lCardNumData[0].Substring(4, 4) + "-" + lCardNumData[0].Substring(8, 4) + "-" + lCardNumData[0].Substring(12);
                            }
                            else
                            {
                                pNormalCarInfo.VanCardNumber = lCardNumData[0];
                            }
                            pNormalCarInfo.VanRegNo = result.RS09.Trim();
                            pNormalCarInfo.VanDate = "20" + result.RS07.Substring(0, 6).Trim();
                            pNormalCarInfo.VanRescode = returnCode.Trim();
                            pNormalCarInfo.VanResMsg = result.RS16.Trim();
                            pNormalCarInfo.VanSupplyPay = Convert.ToInt32(SupplyPay);
                            pNormalCarInfo.VanTaxPay = Convert.ToInt32(taxsResult);
                            pNormalCarInfo.VanCardName = result.RS12.Trim();
                            //pNormalCarInfo.VanBeforeCardPay = pNormalCarInfo.PaymentMoney;
                            pNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash("20" + result.RS07.Substring(0, 6));
                            pNormalCarInfo.VanCardApproveHms = NPSYS.ConvetDay_Dash(result.RS07.Substring(6, 6));
                            pNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash("20" + result.RS07.Substring(0, 6));
                            pNormalCarInfo.VanCardApprovalHms = NPSYS.ConvetDay_Dash(result.RS07.Substring(6, 6));
                            pNormalCarInfo.VanIssueCode = result.RS11.Trim();
                            pNormalCarInfo.VanIssueName = result.RS12.Trim();
                            pNormalCarInfo.VanCardAcquirerCode = result.RS05.Trim();
                            pNormalCarInfo.VanCardAcquirerName = result.RS14.Trim();
                            
                            //카드실패전송 완료
                        }
                        else
                        {
                            pNormalCarInfo.VanRescode = KICC_TIT.KICC_USER_CANCLECODE;
                        }

                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "PayCardandCash | CreditCardPayResult", "[카드결제실패]");

                        

                        

                        pNormalCarInfo.VanAmt = 0;
                        if (result.RS16 == null)
                        {
                            result.RS16 = string.Empty;
                        }
                        if (result.MSG == null)
                        {
                            result.MSG = string.Empty;
                        }
                        pNormalCarInfo.VanResMsg = (result.RS16.Trim() == string.Empty ? result.MSG.Trim() : result.RS16.Trim());
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "PayCardandCash | CreditCardPayResult", "[카드결제실패]"
                                                                  + " [응답메세지]" + pNormalCarInfo.VanResMsg
                                                                  + " [메세지]" + result.MSG.Trim()
                                                                  + " [응답코드]" + pNormalCarInfo.VanRescode);
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "카드 결제실패:" + pNormalCarInfo.VanResMsg);
                        return new Result(false, pNormalCarInfo.VanRescode + ":" + pNormalCarInfo.VanResMsg);

                    }
                }
                //KICC DIP적용완료
                //FIRSTDATA처리 
                else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.FIRSTDATA_DIP)
                {
                    Thread.Sleep(500);
                    FirstDataDip.CreditAuthSimpleExData result = FirstDataDip.CreditAuthSimpleEx(terminalNo, pNormalCarInfo.PaymentMoney.ToString());
                    if (result == null || result.ResCode.Trim() == "")
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                        TextCore.DeviceError(TextCore.DEVICE.TCPIP, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                        return new Result(false, "신용카드 서버 접속안됨");
                    }
                    string returnCode = result.ResCode;
                    if (returnCode == FirstData.Success)
                    {

                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        LPRDbSelect.LogMoney(PaymentType.CreditCard,logDate, pNormalCarInfo, MoneyType.CreditCard, pNormalCarInfo.PaymentMoney, 0, "");
                        pNormalCarInfo.VanCheck = 1;

                        pNormalCarInfo.VanCardNumber = result.CardNumber.Substring(0, 4) + "-" + result.CardNumber.Substring(4, 4) + "-" + result.CardNumber.Substring(8, 4) + "-" + result.CardNumber.Substring(12);
                        pNormalCarInfo.VanRegNo = result.AuthNumber.Trim();
                        pNormalCarInfo.VanDate = "20" + result.AuthDate.Trim();
                        pNormalCarInfo.VanRescode = returnCode.Trim();
                        pNormalCarInfo.VanResMsg = result.ResMsg.Trim();
                        pNormalCarInfo.VanSupplyPay = Convert.ToInt32(SupplyPay);
                        pNormalCarInfo.VanTaxPay = Convert.ToInt32(taxsResult);
                        pNormalCarInfo.VanCardName = result.CardName.Trim();
                        pNormalCarInfo.VanBeforeCardPay = pNormalCarInfo.PaymentMoney;
                        pNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash("20" + result.AuthDate);
                        pNormalCarInfo.VanCardApproveHms = DateTime.Now.ToString("HH:mm:ss");
                        pNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash("20" + result.AuthDate);
                        pNormalCarInfo.VanCardApprovalHms = DateTime.Now.ToString("HH:mm:ss");
                        pNormalCarInfo.VanIssueCode = result.IssuerCode.Trim();
                        pNormalCarInfo.VanIssueName = result.IssuerName.Trim();
                        pNormalCarInfo.VanCardAcquirerCode = result.AcquirerCode;
                        pNormalCarInfo.VanCardAcquirerName = result.AcquirerName;

                        LPRDbSelect.Creditcard_Log_INsert(pNormalCarInfo);
                        //LPRDbSelect.SaveCardPay(pNormalCarInfo);
                        // 카드결제후 무언가 완료된거를 보내야한다
                        pNormalCarInfo.VanAmt = pNormalCarInfo.PaymentMoney;
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|CreditCardPayResult", "카드 결제성공");
                        return new Result(true, "성공");
                    }
                    else
                    {

                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "PayCardandCash | CreditCardPayResult", "[카드결제실패]");
                        if (returnCode == null)
                        {
                            returnCode = "9999";
                        }
                        pNormalCarInfo.VanCheck = 2;
                        pNormalCarInfo.VanRescode = returnCode;

                        pNormalCarInfo.VanAmt = 0;

                        pNormalCarInfo.VanResMsg = (result.ResMsg);
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "PayCardandCash | CreditCardPayResult", "[카드결제실패]"
                                                                  + " [응답메세지]" + pNormalCarInfo.VanResMsg
                                                                  + " [응답코드]" + pNormalCarInfo.VanRescode);
                        return new Result(false, pNormalCarInfo.VanRescode + ":" + pNormalCarInfo.VanResMsg);

                    }
                }

                //FIRSTDATA처리 주석완료
                else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KSNET)
                {
                    KSR01.CreditAuthSimpleExData result = KSR01.CreditAuthSimpleEx(NPSYS.gVanIp, Convert.ToInt32(NPSYS.gVanPort), terminalNo, pNormalCarInfo.PaymentMoney.ToString(), SupplyPay.ToString(), taxsResult.ToString());
                    if (result == null || result.ResCode.Trim() == "")
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                        TextCore.DeviceError(TextCore.DEVICE.TCPIP, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                        return new Result(false, "신용카드 서버 접속안됨");
                    }
                    string returnCode = result.ResCode;
                    if (returnCode == FirstData.Success)
                    {

                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        LPRDbSelect.LogMoney(PaymentType.CreditCard,logDate, pNormalCarInfo, MoneyType.CreditCard, pNormalCarInfo.PaymentMoney, 0, "");
                        pNormalCarInfo.VanCheck = 1;
                        string lCardNumData = result.CreditInfo;
                        pNormalCarInfo.VanCardNumber = lCardNumData.Substring(0, 4) + "-" + lCardNumData.Substring(4, 4) + "-****-" + lCardNumData.Substring(12);
                        pNormalCarInfo.VanRegNo = result.AuthNumber.Trim();
                        pNormalCarInfo.VanDate = result.AuthDate.Trim();
                        pNormalCarInfo.VanTime = result.AuthTime.Trim();
                        pNormalCarInfo.VanRescode = returnCode;
                        pNormalCarInfo.VanResMsg = result.ResMsg.Trim();
                        pNormalCarInfo.VanResMsg2 = result.ResMsg2.Trim();
                        pNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(result.AuthDate.Trim());
                        pNormalCarInfo.VanCardApproveHms = NPSYS.ConvetDay_Dash(result.AuthTime.Trim());
                        pNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(result.AuthDate.Trim());
                        pNormalCarInfo.VanCardApprovalHms = NPSYS.ConvetDay_Dash(result.AuthTime.Trim());
                        pNormalCarInfo.VanSupplyPay = Convert.ToInt32(SupplyPay);
                        pNormalCarInfo.VanTaxPay = Convert.ToInt32(taxsResult);
                        pNormalCarInfo.VanCardName = result.IssuerName.Trim();
                        pNormalCarInfo.VanBeforeCardPay = pNormalCarInfo.PaymentMoney;
                        pNormalCarInfo.VanIssueCode = result.IssuerCode.Trim();
                        pNormalCarInfo.VanIssueName = result.IssuerName.Trim();
                        pNormalCarInfo.VanCardMagneMentCode = result.MagneMentCode.Trim();
                        pNormalCarInfo.VanCardAcquirerCode = result.AcquirerCode;
                        pNormalCarInfo.VanCardAcquirerName = result.AcquirerName;
                        pNormalCarInfo.VanCardMemberCode = result.MemberNumber;



                        LPRDbSelect.Creditcard_Log_INsert(pNormalCarInfo);
                        //LPRDbSelect.SaveCardPay(pNormalCarInfo);
                        // 카드결제후 무언가 완료된거를 보내야한다
                        pNormalCarInfo.VanAmt = pNormalCarInfo.PaymentMoney;
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|CreditCardPayResult", "카드 결제성공");
                        return new Result(true, "성공");

                    }
                    else
                    {

                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        pNormalCarInfo.VanCheck = 2;
                        pNormalCarInfo.VanRescode = result.ResCode;
                        pNormalCarInfo.VanResMsg = result.ResMsg;
                        pNormalCarInfo.VanResMsg2 = result.ResMsg2;
                        pNormalCarInfo.VanAmt = 0;
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "카드 결제실패:" + pNormalCarInfo.VanResMsg + pNormalCarInfo.VanResMsg2);
                        return new Result(false, pNormalCarInfo.VanRescode + ":" + pNormalCarInfo.VanResMsg + pNormalCarInfo.VanResMsg2);

                    }
                }
                else
                {
                    SMATROCREDIT.CreditAuthSimpleExData result = SMATROCREDIT.CreditAuthSimpleEx(NPSYS.gVanIp, Convert.ToInt32(NPSYS.gVanPort), terminalNo, pCrditCardInfo, pNormalCarInfo.PaymentMoney.ToString(), taxsResult.ToString());
                    if (result == null || result.ResCode.Trim() == "")
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                        TextCore.DeviceError(TextCore.DEVICE.TCPIP, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                        return new Result(false, "신용카드 서버 접속안됨");
                    }
                    string returnCode = result.ResCode;
                    if (returnCode == SMATROCREDIT.Success)
                    {

                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    
                        LPRDbSelect.LogMoney(PaymentType.CreditCard, logDate, pNormalCarInfo, MoneyType.CreditCard, pNormalCarInfo.PaymentMoney, 0, "");
                        pNormalCarInfo.VanCheck = 1;
                        string[] lCardNumData = pCrditCardInfo.Split('=');
                        pNormalCarInfo.VanCardNumber = pCrditCardInfo.Split('=')[0].Substring(0, 4) + "-" + pCrditCardInfo.Split('=')[0].Substring(4, 4) + "-****-" + pCrditCardInfo.Split('=')[0].Substring(12);
                        pNormalCarInfo.VanRegNo = result.AuthNumber.Trim();
                        pNormalCarInfo.VanDate = result.AuthDate.Trim();
                        pNormalCarInfo.VanTime = result.AuthTime.Trim();
                        pNormalCarInfo.VanRescode = returnCode.Trim() + "00";
                        pNormalCarInfo.VanResMsg = result.ResMsg.Trim();
                        pNormalCarInfo.VanResMsg2 = result.ResMsg2.Trim();
                        pNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(result.AuthDate.Trim());
                        pNormalCarInfo.VanCardApproveHms = NPSYS.ConvetDay_Dash(result.AuthTime.Trim());
                        pNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(result.AuthDate.Trim());
                        pNormalCarInfo.VanCardApprovalHms = NPSYS.ConvetDay_Dash(result.AuthTime.Trim());
                        pNormalCarInfo.VanSupplyPay = Convert.ToInt32(SupplyPay);
                        pNormalCarInfo.VanTaxPay = Convert.ToInt32(taxsResult);
                        pNormalCarInfo.VanCardName = result.IssuerName.Trim();
                        pNormalCarInfo.VanBeforeCardPay = pNormalCarInfo.PaymentMoney;
                        pNormalCarInfo.VanIssueCode = result.IssuerCode.Trim();
                        pNormalCarInfo.VanIssueName = result.IssuerName.Trim();
                        pNormalCarInfo.VanCardMagneMentCode = result.MagneMentCode.Trim();
                        pNormalCarInfo.VanCardAcquirerCode = result.AcquirerCode;
                        pNormalCarInfo.VanCardAcquirerName = result.AcquirerName;
                        pNormalCarInfo.VanCardMemberCode = result.MemberNumber;

                        LPRDbSelect.Creditcard_Log_INsert(pNormalCarInfo);
                        //LPRDbSelect.SaveCardPay(pNormalCarInfo);
                        // 카드결제후 무언가 완료된거를 보내야한다
                        pNormalCarInfo.VanAmt = pNormalCarInfo.PaymentMoney;
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|CreditCardPayResult", "카드 결제성공");
                        return new Result(true, "성공");

                    }
                    else
                    {

                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        pNormalCarInfo.VanCheck = 2;

                        pNormalCarInfo.VanRescode = result.ResCode;
                        pNormalCarInfo.VanResMsg = result.ResMsg;
                        pNormalCarInfo.VanResMsg2 = result.ResMsg2;

                        pNormalCarInfo.VanAmt = 0;
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "카드 결제실패:" + pNormalCarInfo.VanResMsg + pNormalCarInfo.VanResMsg2);
                        return new Result(false, pNormalCarInfo.VanRescode + ":" + pNormalCarInfo.VanResMsg + pNormalCarInfo.VanResMsg2);

                    }
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|CreditCardPayResult", "카드 결제중 예외상황:" + ex.ToString());
                return new Result(false, ex.ToString());
            }
            finally
            {

            }

        }

        public enum CashSwipKey
        {
            S,
            K
        }
        public Result CashRecipt(NormalCarInfo pNormalCarInfo)
        {
            if (!NPSYS.Device.UsingUsingSettingCashReceipt)
            {
                return new Result(false, "현금영수증 사용안함");;
            }
            TextCore.INFO(TextCore.INFOS.CASHRECEIPT, "FormPaymentMenu|CashRecipt", "현금 영수증 승인시작");
            string terminalNo = NPSYS.gCashTerminalId;


            double CurrentMoney = pNormalCarInfo.GetInComeMoney - pNormalCarInfo.GetChargeMoney ;

            double tax = ((long)(Math.Floor((decimal)(CurrentMoney / 1.1))));
            double taxsResult = ((long)(Math.Floor((decimal)(CurrentMoney - tax))));
            double SupplyPay = CurrentMoney - Convert.ToInt32(taxsResult); //공급가

            //double tax = ((long)(Math.Floor((decimal)(CurrentMoney / 1.1))) / 10) * 10;
            //double taxsResult = CurrentMoney - tax;  // 세금
            //double SupplyPay = CurrentMoney - Convert.ToInt32(taxsResult); //공급가
           // Thread.Sleep(1000);

            try
            {
                if (NPSYS.GetCurrentCashVANType == ConfigID.CardReaderType.VAN_FIRSTDATA)
                {
                    CashReceiptAuthData result = FirstData.CashReceiptAuthEx(terminalNo, "0100001234", CashSwipKey.K.ToString(), CurrentMoney.ToString(), taxsResult.ToString());
                    if (result == null || result.ResCode.Trim() == "")
                    {
                        TextCore.INFO(TextCore.INFOS.CASHRECEIPT_ERRPR, "FormPaymentMenu|CashRecipt", "현금 영수증 서버 접속안됨");
                        TextCore.DeviceError(TextCore.DEVICE.TCPIP, "FormPaymentMenu|CashRecipt", "현금 영수증 서버 접속안됨");
                        return new Result(false, "현금 영수증 서버 접속안됨");
                    }
                    string returnCode = result.ResCode;
                    if (returnCode == FirstData.Success)
                    {
                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        pNormalCarInfo.CashReciptNo = result.AuthNumber.Trim();
                        pNormalCarInfo.CashReciptRescode = result.ResCode;
                        pNormalCarInfo.CashReciptResMsg = result.ResMsg;
                        pNormalCarInfo.CashReciptAuthDate = result.AuthDate;
                        pNormalCarInfo.CashReciptApproveYmd = NPSYS.ConvetYears_Dash("20" + result.AuthDate);
                        pNormalCarInfo.CashReciptApproveHms = DateTime.Now.ToString("HH:mm:ss");
                        pNormalCarInfo.CashReciptApprovalYmd = NPSYS.ConvetYears_Dash("20" + result.AuthDate);
                        pNormalCarInfo.CashReciptApprovalHms = DateTime.Now.ToString("HH:mm:ss");
                        pNormalCarInfo.CashReciptRequestYesNo = 1;
                        //                    LPRDbSelect.SaveCashPay(pNormalCarInfo);
                        TextCore.INFO(TextCore.INFOS.CASHRECEIPT_SUCCES, "FormPaymentMenu|CashRecipt", "현금 영수증 승인성공");
                        return new Result(true, "성공");


                    }
                    else
                    {
                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        pNormalCarInfo.CashReciptNo = result.AuthNumber.Trim();
                        pNormalCarInfo.CashReciptRescode = result.ResCode;
                        pNormalCarInfo.CashReciptResMsg = result.ResMsg;
                        pNormalCarInfo.CashReciptAuthDate = result.AuthDate;
                        pNormalCarInfo.CashReciptApproveYmd = NPSYS.ConvetYears_Dash("20" + result.AuthDate);
                        pNormalCarInfo.CashReciptApproveHms = DateTime.Now.ToString("HH:mm:ss");
                        pNormalCarInfo.CashReciptApprovalYmd = NPSYS.ConvetYears_Dash("20" + result.AuthDate);
                        pNormalCarInfo.CashReciptApprovalHms = DateTime.Now.ToString("HH:mm:ss");
                        pNormalCarInfo.CashReciptRequestYesNo = 0;

                        TextCore.INFO(TextCore.INFOS.CASHRECEIPT_ERRPR, "FormPaymentMenu|CashRecipt", "현금 영수증 승인실패:" + pNormalCarInfo.VanResMsg);
                        return new Result(false, pNormalCarInfo.VanRescode + ":" + pNormalCarInfo.VanResMsg);
                    }
                }
                else if (NPSYS.GetCurrentCashVANType == ConfigID.CardReaderType.VNA_SMATRO)
                {
                    SMATROCREDIT.CashReceiptAuthData result = SMATROCREDIT.CashReceiptAuthEx(NPSYS.gCashVanIp, Convert.ToInt32(NPSYS.gCashVanPort), terminalNo, CurrentMoney.ToString(), taxsResult.ToString());
                    if (result == null || result.ResCode.Trim() == "")
                    {
                        TextCore.INFO(TextCore.INFOS.CASHRECEIPT_ERRPR, "FormPaymentMenu|CashRecipt", "현금 영수증 서버 접속안됨");
                        TextCore.DeviceError(TextCore.DEVICE.TCPIP, "FormPaymentMenu|CashRecipt", "현금 영수증 서버 접속안됨");
                        return new Result(false, "현금 영수증 서버 접속안됨");
                    }
                    string returnCode = result.ResCode;
                    if (returnCode == SMATROCREDIT.Success)
                    {
                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        pNormalCarInfo.CashReciptNo = result.AuthNumber.Trim();
                        pNormalCarInfo.CashReciptRescode = result.ResCode;
                        pNormalCarInfo.CashReciptResMsg = result.ResMsg;
                        pNormalCarInfo.CashReciptAuthDate = result.AuthDate;
                        pNormalCarInfo.CashReciptApproveYmd = NPSYS.ConvetYears_Dash(result.AuthDate);
                        pNormalCarInfo.CashReciptApproveHms = NPSYS.ConvetDay_Dash(result.AuthTime);
                        pNormalCarInfo.CashReciptApprovalYmd = NPSYS.ConvetYears_Dash(result.AuthDate);
                        pNormalCarInfo.CashReciptApprovalHms = NPSYS.ConvetDay_Dash(result.AuthTime);
                        pNormalCarInfo.CashReciptRequestYesNo = 1;
                        //                    LPRDbSelect.SaveCashPay(pNormalCarInfo);
                        TextCore.INFO(TextCore.INFOS.CASHRECEIPT_SUCCES, "FormPaymentMenu | CashRecipt", "[현금 영수증 승인성공] 승인번호" + result.AuthNumber.Trim());
                        return new Result(true, "성공");


                    }
                    else
                    {
                        try
                        {
                            string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            pNormalCarInfo.CashReciptNo = result.AuthNumber.Trim();
                            pNormalCarInfo.CashReciptRescode = result.ResCode;
                            pNormalCarInfo.CashReciptResMsg = result.ResMsg;
                            pNormalCarInfo.CashReciptAuthDate = result.AuthDate;
                            pNormalCarInfo.CashReciptApproveYmd = NPSYS.ConvetYears_Dash(result.AuthDate);
                            pNormalCarInfo.CashReciptApproveHms = NPSYS.ConvetDay_Dash(result.AuthTime);
                            pNormalCarInfo.CashReciptApprovalYmd = NPSYS.ConvetYears_Dash(result.AuthDate);
                            pNormalCarInfo.CashReciptApprovalHms = NPSYS.ConvetDay_Dash(result.AuthTime);
                            pNormalCarInfo.CashReciptRequestYesNo = 0;

                            TextCore.INFO(TextCore.INFOS.CASHRECEIPT_ERRPR, "FormPaymentMenu|CashRecipt", "현금 영수증 승인실패:" + pNormalCarInfo.VanResMsg);
                            return new Result(false, pNormalCarInfo.VanRescode + ":" + pNormalCarInfo.VanResMsg);
                        }
                        catch(Exception ex)
                        {
                            return new Result(false, pNormalCarInfo.VanRescode + ":" + ex.ToString());
                        }
                    }
                }
                return new Result(false, string.Empty + ":" + "");

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.CASHRECEIPT_ERRPR, "FormPaymentMenu|CashRecipt", "현금 영수증 승인중 예외상황:" + ex.ToString());
                return new Result(false, ex.ToString());
            }
        }


    }
}
