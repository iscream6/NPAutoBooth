using FadeFox.Text;
using NPCommon.DTO;
using NPCommon.DTO.Receive;
using System;

namespace NPCommon.REST
{
    public class HttpProcess
    {
        NPHttpControl mNPHttpControl = new NPHttpControl();
        Payment currentCar = new Payment();
        ParkingReceiveData restClassParser = new ParkingReceiveData();
        CarList currentListCar = new CarList();
        Status ErrorStatus = new Status();

        /// <summary>
        /// 원격할인 데이터 받아올때
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public Payment GetRemoteDiscount(string pData)
        {
            Payment remotePayment = (Payment)restClassParser.SetDataParsing(NPHttpControl.UrlCmdType.payments, pData);
            return remotePayment;
        }
        /// <summary>
        /// 원격에서 오는 요금 payment 클래스로 변환
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public Payment GetRemotePayment(string pData)
        {
            Payment remotePayment = (Payment)restClassParser.SetDataParsing(NPHttpControl.UrlCmdType.payments, pData);
            return remotePayment;
        }
        /// <summary>
        /// 일반차량 요금 받으때
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public Payment GetPayment(string pData)
        {
            Payment remotePayment = (Payment)restClassParser.SetDataParsing(NPHttpControl.UrlCmdType.payments, pData);
            return remotePayment;
        }
        /// <summary>
        /// 영수증 데이터 받을때
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public Payment GetReceiptPayment(string pData)
        {
            Payment remotePayment = (Payment)restClassParser.SetDataParsing(NPHttpControl.UrlCmdType.payments, pData);
            return remotePayment;
        }
        /// <summary>
        /// 카드취소 데이터 받을떄
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public Payment GetRemoteCardPayment(string pData)
        {
            Payment remotePayment = (Payment)restClassParser.SetDataParsing(NPHttpControl.UrlCmdType.payments, pData);
            return remotePayment;
        }


        /// <summary>
        /// 마감저장
        /// </summary>
        /// <param name="pCloseData"></param>
        /// <returns></returns>
        public Close SaveClose(Close pCloseData)
        {

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(pCloseData);
            NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.close;
            mNPHttpControl.SetHeader(currnetUrl, string.Empty);
            Close currentClose = new Close();
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | SaveClose", "[마감저장처리요청]" + data);
            currentClose = (Close)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMessagePost(string.Empty, data));
            return currentClose;

        }

        /// <summary>
        /// 카드결제취소
        /// </summary>
        /// <param name="pCarInfo"></param>
        public void PayCancle(NormalCarInfo pCarInfo)
        {
            if (pCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
               || pCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
            {
                DTO.Send.Remote_cancle_payments canclePayment = new DTO.Send.Remote_cancle_payments("C");
                string cancleData = Newtonsoft.Json.JsonConvert.SerializeObject(canclePayment);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PayCancle", "[카드취소관련 보내는 전문]" + cancleData);
                NPHttpControl.UrlCmdType cancleUrl = NPHttpControl.UrlCmdType.remote_cacnel_payments;
                mNPHttpControl.SetHeader(cancleUrl, string.Empty, string.Empty, pCarInfo.PaymentDetailId);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PayCancle", "[일반차량 카드취소요청]"
                                                                                    + " 차량번호:" + pCarInfo.OutCarNo1
                                                                                    + " 총주차요금:" + pCarInfo.VanAmt.ToString()
                                                                                    + " 이전 정산시간:" + pCarInfo.VanDate_Cancle + pCarInfo.VanTime_Cancle);
                currentCar = new Payment();
                mNPHttpControl.SendMessagePut(string.Empty, cancleData);
                return;

            }

        }

        public PettyCash SavepettyCash(int pCoin1, int pCoin2, int pCoin3, int pCoin4, int pBill1, int pBill2, int pBill3, int pBill4) // 시제설정누락처리
        {
            DTO.Send.Pettycashes Sendpettycashes = new DTO.Send.Pettycashes(pCoin1, pCoin2, pCoin3, pCoin4, pBill1, pBill2, pBill3, pBill4);
            string pettyCashData = Newtonsoft.Json.JsonConvert.SerializeObject(Sendpettycashes);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | SavepettyCash", "[시제설정 보내는 전문]" + pettyCashData);
            NPHttpControl.UrlCmdType pettyCashUrl = NPHttpControl.UrlCmdType.petty_cashes;
            mNPHttpControl.SetHeader(pettyCashUrl, string.Empty, string.Empty, 0);
            PettyCash pettyCashClass = new PettyCash();
            pettyCashClass = (PettyCash)restClassParser.SetDataParsing(pettyCashUrl, mNPHttpControl.SendMessagePost(string.Empty, pettyCashData));
            return pettyCashClass;
        }
        /// <summary>
        /// 결제저장
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public Payment PaySave(NormalCarInfo pCarInfo, DateTime pDateTime)
        {
            if (pCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
               || pCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
            {
                DTO.Send.Remote_cancle_payments canclePayment = new DTO.Send.Remote_cancle_payments("C");
                string cancleData = Newtonsoft.Json.JsonConvert.SerializeObject(canclePayment);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[카드취소관련 보내는 전문]" + cancleData);
                NPHttpControl.UrlCmdType cancleUrl = NPHttpControl.UrlCmdType.remote_cacnel_payments;
                mNPHttpControl.SetHeader(cancleUrl, string.Empty, string.Empty, pCarInfo.PaymentDetailId);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[일반차량 카드취소요청]"
                                                                                    + " 차량번호:" + pCarInfo.OutCarNo1
                                                                                    + " 총주차요금:" + pCarInfo.VanAmt.ToString());
                currentCar = new Payment();
                mNPHttpControl.SendMessagePut(string.Empty, cancleData);
                currentCar.status = new Status(); // 응답이없어서 이렇게 처리
                currentCar.status.Success = true;

                //Tmap연동
                //TODO : Normal Mode에서도 24시간 결제 체크를 요구할 시 If 조건을 없앨것.
                if (NPSYS.gUseTmap)
                {
                    string p_IO_TYPE = "";
                    if (pCarInfo.Current_Money > 0 && pCarInfo.GetNotDisChargeMoney == 0) p_IO_TYPE = "CASH";
                    else if (pCarInfo.Current_Money > 0 && pCarInfo.GetNotDisChargeMoney > 0) p_IO_TYPE = "CASH";
                    else if (pCarInfo.TMoneyPay > 0) p_IO_TYPE = MoneyType.TmoneyCard.ToString();
                    else if (pCarInfo.VanAmt > 0) p_IO_TYPE = MoneyType.CreditCard.ToString();
                    else p_IO_TYPE = "CASH";

                    LPRDbSelect.LastestPayment_LOG_Insert(pCarInfo.OutCarNo1, pCarInfo.ReceiveMoney, p_IO_TYPE, "PaymentCancel");
                }
                //Tmap연동완료

                return currentCar;

            }

            DTO.Send.SendPaymentData senddata = new DTO.Send.SendPaymentData(pCarInfo, pDateTime);
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(senddata);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[결제관련 보내는 전문]" + data);
            NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.None;

            //연장 정기권의 경우
            if (pCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season
               || pCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season)
            {
                currnetUrl = NPHttpControl.UrlCmdType.season_car_payment_details;
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[정기권차량 연장요금저장요청]"
                                                                                    + " 차량번호:" + pCarInfo.OutCarNo1
                                                                                    + " 총주차요금:" + pCarInfo.TotFee.ToString()
                                                                                    + " 총할인요금:" + pCarInfo.TotDc.ToString()
                                                                                    + " 실제결제금액" + pCarInfo.ReceiveMoney.ToString()
                                                                                    + " 연장시작일" + NPSYS.ConvetYears_Dash(pCarInfo.NextStartYmd) + NPSYS.ConvetDay_Dash(pCarInfo.CurrStartHms)
                                                                                    + " 연장종료일" + NPSYS.ConvetYears_Dash(pCarInfo.NextExpiredYmd) + NPSYS.ConvetDay_Dash(pCarInfo.CurrExpireHms)
                                                                                    );
                mNPHttpControl.SetHeader(currnetUrl, string.Empty);
                currentCar = new Payment();
                currentCar = (Payment)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMessagePost(string.Empty, data));
                if (currentCar.status.Success == false) // 결제가 실패시
                {
                    if (currentCar.status.currentStatus == Status.BodyStatus.PaymentDetail_Duplicate) // 중복결제면
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[기존 중복결제 차량이라 성공결제로 봄]");
                        currentCar.status.Success = true;
                        return currentCar;
                    }
                    else if (currentCar.status.currentStatus == Status.BodyStatus.ReturnFailDataError)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[등록된 정보에 이상이 있어서 성공결제로 봄]");
                        currentCar.status.Success = true;
                        return currentCar;

                    }
                    // 재전송 처리
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[재전송시도 1]");
                    currentCar = (Payment)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMessagePost(string.Empty, data));
                    if (currentCar.status.Success == false) // 결제가 실패
                    {
                        if (currentCar.status.currentStatus == Status.BodyStatus.PaymentDetail_Duplicate) // 중복결제면
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[기존 중복결제 차량이라 성공결제로 봄]");
                            currentCar.status.Success = true;
                            return currentCar;
                        }
                        else if (currentCar.status.currentStatus == Status.BodyStatus.ReturnFailDataError)
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[등록된 정보에 이상이 있어서 성공결제로 봄]");
                            currentCar.status.Success = true;
                            return currentCar;

                        }


                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[재전송시도 2]");
                        currentCar = (Payment)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMessagePost(string.Empty, data));
                        if (currentCar.status.Success == false)
                        {
                            if (currentCar.status.currentStatus == Status.BodyStatus.PaymentDetail_Duplicate) // 중복결제면
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[기존 중복결제 차량이라 성공결제로 봄]");
                                currentCar.status.Success = true;
                                return currentCar;
                            }
                            else if (currentCar.status.currentStatus == Status.BodyStatus.ReturnFailDataError)
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[등록된 정보에 이상이 있어서 성공결제로 봄]");
                                currentCar.status.Success = true;
                                return currentCar;

                            }

                            else
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[정기차량정기권연장관련 재전송을 위해 DB에저장]");
                                LPRDbSelect.ReSendData_INsert(currnetUrl.ToString(), data);
                            }
                        }
                    }
                }
                else //결제 성공 시
                {
                    //Tmap연동
                    //TODO : Normal Mode에서도 24시간 결제 체크를 요구할 시 If 조건을 없앨것.
                    if (NPSYS.gUseTmap)
                    {
                        string p_IO_TYPE = "";
                        if (pCarInfo.Current_Money > 0 && pCarInfo.GetNotDisChargeMoney == 0) p_IO_TYPE = "CASH";
                        else if (pCarInfo.Current_Money > 0 && pCarInfo.GetNotDisChargeMoney > 0) p_IO_TYPE = "CASH";
                        else if (pCarInfo.TMoneyPay > 0) p_IO_TYPE = MoneyType.TmoneyCard.ToString();
                        else if (pCarInfo.VanAmt > 0) p_IO_TYPE = MoneyType.CreditCard.ToString();
                        else p_IO_TYPE = "CASH";

                        LPRDbSelect.LastestPayment_LOG_Insert(pCarInfo.OutCarNo1, pCarInfo.ReceiveMoney, p_IO_TYPE, "SeasonPayment");
                    }
                    //Tmap연동완료
                }

                return currentCar;
            }
            else
            {
                currnetUrl = NPHttpControl.UrlCmdType.payment_details;
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[일반챠량 요금저장요청]"
                                                                                    + " 차량번호:" + pCarInfo.OutCarNo1
                                                                                    + " 총주차요금:" + pCarInfo.TotFee.ToString()
                                                                                    + " 총할인요금:" + pCarInfo.TotDc.ToString()
                                                                                    + " 실제결제금액" + pCarInfo.ReceiveMoney.ToString());
                mNPHttpControl.SetHeader(currnetUrl, string.Empty);
                currentCar = new Payment();
                currentCar = (Payment)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMessagePost(string.Empty, data));
                if (currentCar.status.Success == false) // 결제가 실패시
                {
                    if (currentCar.status.currentStatus == Status.BodyStatus.PaymentDetail_Duplicate) // 중복결제면
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[기존 중복결제 차량이라 성공결제로 봄]");
                        currentCar.status.Success = true;
                        return currentCar;
                    }
                    else if (currentCar.status.currentStatus == Status.BodyStatus.ReturnFailDataError)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[등록된 정보에 이상이 있어서 성공결제로 봄]");
                        currentCar.status.Success = true;
                        return currentCar;

                    }

                    // 재전송 처리
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[재전송시도 1]");
                    currentCar = (Payment)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMessagePost(string.Empty, data));
                    if (currentCar.status.Success == false) // 결제가 실패
                    {
                        if (currentCar.status.currentStatus == Status.BodyStatus.PaymentDetail_Duplicate) // 중복결제면
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[기존 중복결제 차량이라 성공결제로 봄]");
                            currentCar.status.Success = true;
                            return currentCar;
                        }
                        else if (currentCar.status.currentStatus == Status.BodyStatus.ReturnFailDataError) // 중복결제면
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[기존 중복결제 차량이라 성공결제로 봄]");
                            currentCar.status.Success = true;
                            return currentCar;
                        }

                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[재전송시도 2]");
                        currentCar = (Payment)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMessagePost(string.Empty, data));
                        if (currentCar.status.Success == false)
                        {
                            if (currentCar.status.currentStatus == Status.BodyStatus.PaymentDetail_Duplicate) // 중복결제면
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[기존 중복결제 차량이라 성공결제로 봄]");
                                currentCar.status.Success = true;
                                return currentCar;
                            }
                            else if (currentCar.status.currentStatus == Status.BodyStatus.ReturnFailDataError) // 중복결제면
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[기존 중복결제 차량이라 성공결제로 봄]");
                                currentCar.status.Success = true;
                                return currentCar;
                            }
                            else
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | PaySave", "[차량요금결제 재전송을 위해 DB에저장]");
                                LPRDbSelect.ReSendData_INsert(currnetUrl.ToString(), data);
                            }
                        }
                    }
                }
                else //결제 성공 시
                {
                    //Tmap연동
                    //TODO : Normal Mode에서도 24시간 결제 체크를 요구할 시 If 조건을 없앨것.
                    if (NPSYS.gUseTmap)
                    {
                        string p_IO_TYPE = "";
                        if (pCarInfo.Current_Money > 0 && pCarInfo.GetNotDisChargeMoney == 0) p_IO_TYPE = "CASH";
                        else if (pCarInfo.Current_Money > 0 && pCarInfo.GetNotDisChargeMoney > 0) p_IO_TYPE = "CASH";
                        else if (pCarInfo.TMoneyPay > 0) p_IO_TYPE = MoneyType.TmoneyCard.ToString();
                        else if (pCarInfo.VanAmt > 0) p_IO_TYPE = MoneyType.CreditCard.ToString();
                        else p_IO_TYPE = "CASH";

                        LPRDbSelect.LastestPayment_LOG_Insert(pCarInfo.OutCarNo1, pCarInfo.ReceiveMoney, p_IO_TYPE, "NormalPayment");
                    }
                    //Tmap연동완료
                }

                return currentCar;
            }
        }

        /// <summary>
        /// 저장로직 다시보냄
        /// </summary>
        /// <param name="pUrl"></param>
        /// <param name="pData"></param>
        /// <returns>true면 재전송 성공 false면 재전송안됨</returns>
        public bool RePaySave(string pUrl, string pData)
        {
            NPHttpControl.UrlCmdType currentSendUrl = (NPHttpControl.UrlCmdType)Enum.Parse(typeof(NPHttpControl.UrlCmdType), pUrl);
            switch (currentSendUrl)
            {

                case NPHttpControl.UrlCmdType.season_car_payment_details:
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | RePaySave", "[정기권 연장결제 로컬DB 재전송]"
                                                                    + " 보내는데이터:" + pData);
                    mNPHttpControl.SetHeader(currentSendUrl, string.Empty);
                    currentCar = new Payment();
                    currentCar = (Payment)restClassParser.SetDataParsing(currentSendUrl, mNPHttpControl.SendMessagePost(string.Empty, pData));
                    if (currentCar.status.Success == false) // 결제가 실패시
                    {
                        if (currentCar.status.currentStatus == Status.BodyStatus.PaymentDetail_Duplicate) // 중복결제면
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | RePaySave", "[일반차량 결제 로컬DB에서 재전송결과]"
                                                + " 중복결제 요청응답이라 정상으로 처리");
                            return true;
                        }
                        else if (currentCar.status.currentStatus == Status.BodyStatus.ReturnFailDataError)
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | RePaySave", "[일반차량 결제 로컬DB에서 재전송결과]"
                                                + " 관련데이터 오류로 정상으로 처리");
                            return true;

                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | RePaySave", "[일반차량 결제 로컬DB에서 재전송결과]"
                    + " 정상으로 처리");
                        return true;
                    }
                case NPHttpControl.UrlCmdType.payment_details:
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | RePaySave", "[일반차량 결제 로컬DB에서 재전송]"
                                                                    + " 보내는데이터:" + pData);

                    mNPHttpControl.SetHeader(currentSendUrl, string.Empty);
                    currentCar = new Payment();
                    currentCar = (Payment)restClassParser.SetDataParsing(currentSendUrl, mNPHttpControl.SendMessagePost(string.Empty, pData));
                    if (currentCar.status.Success == false) // 결제가 실패시
                    {
                        if (currentCar.status.currentStatus == Status.BodyStatus.PaymentDetail_Duplicate) // 중복결제면
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | RePaySave", "[일반차량 결제 로컬DB에서 재전송결과]"
                                                                            + " 중복결제 요청응답이라 정상으로 처리");

                            return true;
                        }
                        else if (currentCar.status.currentStatus == Status.BodyStatus.ReturnFailDataError)
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | RePaySave", "[일반차량 결제 로컬DB에서 재전송결과]"
                                                                            + " 관련데이터 오류로 정상으로 처리");


                            return true;

                        }

                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | RePaySave", "[일반차량 결제 로컬DB에서 재전송결과]"
+ " 정상으로 처리");
                        return true;
                    }
                default: return true;
            }
        }

        /// <summary>
        /// 할인권요청
        /// </summary>
        /// <param name="pCarInfo"></param>
        /// <returns></returns>
        public Payment Discount(NormalCarInfo pCarInfo, DcDetail.DIscountTicketType pDiscointTicketType, string pDiscountData)
        {
            DTO.Send.SendDiscountData senddata = new DTO.Send.SendDiscountData(pCarInfo, pDiscountData, pDiscointTicketType);
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(senddata);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | Discount", "[할인요청]" + data);
            NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.discount_tickets;
            mNPHttpControl.SetHeader(currnetUrl, string.Empty);
            currentCar = new Payment();
            currentCar = (Payment)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMessagePost(string.Empty, data));
            return currentCar;
        }

        public void DiscountCancle(NormalCarInfo pCarInfo, DcDetail pDcdetail)
        {


            DTO.Send.SendDiscountCancleData cancleDiscountTIcket = new DTO.Send.SendDiscountCancleData(pCarInfo, pDcdetail);
            string cancleData = Newtonsoft.Json.JsonConvert.SerializeObject(cancleDiscountTIcket);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | DiscointCancle", "[할인취소 전문요청]" + cancleData);
            NPHttpControl.UrlCmdType cancleUrl = NPHttpControl.UrlCmdType.discount_ticketsCANCLE;
            mNPHttpControl.SetHeader(cancleUrl, pDcdetail.DcTkno, string.Empty, 0);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | DiscointCancle", "[할인취소요청]"
                                                                                + " 할인권번호:" + pDcdetail.DcTkno);
            currentCar = new Payment();
            mNPHttpControl.SendMessagePut(string.Empty, cancleData);
            return;
        }

        public Status SendErrorMessgae(DeviceStatusManagement pDeviceStatusManagement)
        {
            DTO.Send.Status_devices senddata = new DTO.Send.Status_devices(pDeviceStatusManagement);
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(senddata);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | SendErrorMessgae", "[장비상태전송]" + data);
            NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.status_devices;
            mNPHttpControl.SetHeader(currnetUrl, string.Empty);
            ErrorStatus = new Status();
            ErrorStatus = (Status)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMessagePost(string.Empty, data));
            return ErrorStatus;
        }


        /// <summary>
        /// 네자리 차량조회 전문생성 및 처리
        /// </summary>
        /// <param name="pCarNumber"></param>
        /// <returns></returns>

        public CarList GetSearch4Number(string pCarNumber)
        {
            NPHttpControl mNPHttpControl = new NPHttpControl();
            ParkingReceiveData restClassParser = new ParkingReceiveData();
            NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.cars;
            mNPHttpControl.SetHeader(currnetUrl, pCarNumber);
            CarList currentListCar = (CarList)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMethodGet());
            return currentListCar;


        }

        //Tmap연동
        public CarList GetCarListTmap(string pData)
        {
            CarList currentListCar = (CarList)restClassParser.SetDataParsing(NPHttpControl.UrlCmdType.cars, pData);
            return currentListCar;
        }

        public CarList GetSearch4NumberTmap(string pCarNumber)
        {
            NPHttpControl mNPHttpControl = new NPHttpControl();
            ParkingReceiveData restClassParser = new ParkingReceiveData();
            NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.interworking_cars;
            mNPHttpControl.SetHeader(currnetUrl, pCarNumber);
            CarList currentStauts = (CarList)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMethodGet());
            return currentStauts;
        }

        public Payment GetPaymentTmapFromTkno(Car pCar)
        {

            NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.interworking_payments
;
            mNPHttpControl.SetHeader(currnetUrl, string.Empty, pCar.tkNo);
            Payment currentpayment = (Payment)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMethodGet());
            return currentpayment;
        }

        public Payment GetPaymentTmap(string pData)
        {
            Payment currentPaymentCar = (Payment)restClassParser.SetDataParsing(NPHttpControl.UrlCmdType.payments, pData);
            return currentPaymentCar;
        }
        //Tmap연동 완료

        public CarList GetTimeSearch(long pIntTIme, long pOutTime)
        {
            NPHttpControl mNPHttpControl = new NPHttpControl();
            ParkingReceiveData restClassParser = new ParkingReceiveData();
            NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.TimeSearch;

            mNPHttpControl.SetHeader(currnetUrl, string.Empty, string.Empty, 0, pIntTIme, pOutTime);
            CarList currentListCar = (CarList)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMethodGet());
            return currentListCar;
        }

        public Payment GetPaymentFromTkno(Car pCar)
        {

            NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.payments;
            mNPHttpControl.SetHeader(currnetUrl, string.Empty, pCar.tkNo);
            Payment currentPayment = (Payment)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMethodGet());
            return currentPayment;
        }


        /// <summary>
        /// 출차시 미인식 오인식에 의해서 입차가 없거나 정기권(요금없는)인경우 에 자료 가져오는경우
        /// </summary>
        /// <param name="pCarNumber"></param>
        /// <returns></returns>
        public Car ExitCarInfo(string pData)
        {
            NPHttpControl mNPHttpControl = new NPHttpControl();
            ParkingReceiveData restClassParser = new ParkingReceiveData();
            NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.onecar;
            Car currentCar = (Car)restClassParser.SetDataParsing(currnetUrl, pData);
            return currentCar;
        }

        public string GetReturnData(NPHttpServer.CmdType pCmdType, string pMessage, string pDescription, bool pIsSueecess)
        {
            Status currentReturnStatus = new Status();
            currentReturnStatus.description = pDescription;
            currentReturnStatus.message = pMessage;
            currentReturnStatus.Success = pIsSueecess;
            string cmdData = string.Empty;
            string logData = string.Empty;
            switch (pCmdType)
            {
                case NPHttpServer.CmdType.none:
                    cmdData = "000";
                    cmdData += (pIsSueecess == true ? "200" : "500");
                    logData += "[통신포맷오류] 명령어:" + pCmdType.ToString() + " 정상응답여부:" + pIsSueecess.ToString() + " 메세지:" + pMessage + " 설명:" + pDescription;
                    break;
                case NPHttpServer.CmdType.cars:
                    cmdData = "011";
                    cmdData += (pIsSueecess == true ? "200" : "500");
                    logData += "[정기권 또는 미인식차량 출차요청응답] 명령어:" + pCmdType.ToString() + " 정상응답여부:" + pIsSueecess.ToString() + " 메세지:" + pMessage + " 설명:" + pDescription;
                    break;
                case NPHttpServer.CmdType.remote_cars:
                    cmdData = "021";
                    cmdData += (pIsSueecess == true ? "200" : "500");
                    logData += "[원격요금조회응답] 명령어:" + pCmdType.ToString() + " 정상응답여부:" + pIsSueecess.ToString() + " 메세지:" + pMessage + " 설명:" + pDescription;
                    break;
                case NPHttpServer.CmdType.remote_discounts:
                    cmdData = "031";
                    cmdData += (pIsSueecess == true ? "200" : "500");
                    logData += "[원격할인응답] 명령어:" + pCmdType.ToString() + " 정상응답여부:" + pIsSueecess.ToString() + " 메세지:" + pMessage + " 설명:" + pDescription;
                    break;
                case NPHttpServer.CmdType.payments:
                    cmdData = "041";
                    cmdData += (pIsSueecess == true ? "200" : "500");
                    logData += "[일반차량 출차요청응답] 명령어:" + pCmdType.ToString() + " 정상응답여부:" + pIsSueecess.ToString() + " 메세지:" + pMessage + " 설명:" + pDescription;
                    break;
                case NPHttpServer.CmdType.remote_payments:
                    cmdData = "051";
                    cmdData += (pIsSueecess == true ? "200" : "500");
                    logData += "[원격요금조회응답] 명령어:" + pCmdType.ToString() + " 정상응답여부:" + pIsSueecess.ToString() + " 메세지:" + pMessage + " 설명:" + pDescription;
                    break;
                case NPHttpServer.CmdType.remote_cacnel_payments:
                    cmdData = "061";
                    cmdData += (pIsSueecess == true ? "200" : "500");
                    logData += "[원격 카드취소응답] 명령어:" + pCmdType.ToString() + " 정상응답여부:" + pIsSueecess.ToString() + " 메세지:" + pMessage + " 설명:" + pDescription;
                    break;
                case NPHttpServer.CmdType.remote_print_payments:
                    cmdData = "071";
                    cmdData += (pIsSueecess == true ? "200" : "500");
                    logData += "[원격 영수증발급] 명령어:" + pCmdType.ToString() + " 정상응답여부:" + pIsSueecess.ToString() + " 메세지:" + pMessage + " 설명:" + pDescription;
                    break;
                case NPHttpServer.CmdType.remote_closes:
                    cmdData = "081";
                    cmdData += (pIsSueecess == true ? "200" : "500");
                    logData += "[원격 마감요청] 명령어:" + pCmdType.ToString() + " 정상응답여부:" + pIsSueecess.ToString() + " 메세지:" + pMessage + " 설명:" + pDescription;
                    break;


            }

            currentReturnStatus.code = cmdData;
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(currentReturnStatus);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "HttpProcess | GetReturnData", logData + " 응답전문:" + logData);
            return data;
        }



    }
}
