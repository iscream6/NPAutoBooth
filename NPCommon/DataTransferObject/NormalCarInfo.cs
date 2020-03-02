using FadeFox.Text;
using NPCommon.DTO.Receive;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NPCommon.DTO
{
    [Serializable]
    public enum PaymentType
    {
        None,
        Cash,
        CreditCard,
        TmoneyCard,
        CashTicket,
        Free,

        //카드실패전송
        DiscountCard,

        DiscountBarcode,
        DiscountRemote,

        /// <summary>
        /// 카드결제실패
        /// </summary>
        Fail_Card

        //카드실패전송완료
    }

    [Serializable]
    public class NormalCarInfo
    {
        #region 변수

        private Payment mCurrentPayment = null;
        private Car mCurrentCar = null;
        public List<SeasonCarAmounts> mCurrentSeasonCarAmount = null;
        /// <summary>
        /// 결제관련 전송클래스
        /// </summary>

        public Car CurrentCar
        {
            set
            {
                mCurrentCar = value;

                if (mCurrentCar != null)
                {
                    CarId = mCurrentCar.id;
                    TkNO = mCurrentCar.tkNo;
                    CarType = mCurrentCar.carType;
                    parkingType = mCurrentCar.parkingType;
                    if (mCurrentCar.inDate > 0)
                    {
                        InYmd = NPSYS.LongTypeToDateTime(mCurrentCar.inDate).ToString("yyyyMMdd");
                        InHms = NPSYS.LongTypeToDateTime(mCurrentCar.inDate).ToString("HHmmss");
                    }
                    InImage1 = mCurrentCar.inImage1;
                    InCarNo1 = mCurrentCar.inCarNo1;
                    InImage2 = mCurrentCar.inImage2;
                    InCarNo2 = mCurrentCar.inCarNo2;
                    if (mCurrentCar.outDate > 0)
                    {
                        OutYmd = NPSYS.LongTypeToDateTime(mCurrentCar.outDate).ToString("yyyyMMdd");
                        OutHms = NPSYS.LongTypeToDateTime(mCurrentCar.outDate).ToString("HHmmss");
                    }
                    else
                    {
                        OutYmd = DateTime.Now.ToString("yyyyMMdd");
                        OutHms = DateTime.Now.ToString("HHmmss");
                    }
                    if (NPSYS.gIsAutoBooth)
                    {
                        OutChk = mCurrentCar.outChk;
                        OutCarNo1 = mCurrentCar.outCarNo1;
                        OutImage1 = mCurrentCar.outImage1;
                        OutImage2 = mCurrentCar.outImage2;
                    }

                    if (mCurrentCar.seasonCar != null)
                    {
                        CurrStartYMD = NPSYS.LongTypeToDateTime(mCurrentCar.seasonCar.startDate).ToString("yyyyMMdd");
                        CurrStartHms = NPSYS.LongTypeToDateTime(mCurrentCar.seasonCar.startDate).ToString("HHmmss");
                        CurrExpireYmd = NPSYS.LongTypeToDateTime(mCurrentCar.seasonCar.endDate).ToString("yyyyMMdd");
                        CurrExpireHms = NPSYS.LongTypeToDateTime(mCurrentCar.seasonCar.endDate).ToString("HHmmss");

                        if (mCurrentCar.seasonCar.seasonCarGroup.seasonCarAmounts != null)
                        {
                            string lastExpired = DateTime.ParseExact(CurrExpireYmd, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddDays(-20).ToString("yyyyMMdd"); // 만료연장가능 시작일
                            if (Convert.ToInt32(lastExpired) >= Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"))) // 현재 연장가능기간이 아니면
                            {
                                mCurrentCar.seasonCar.seasonCarGroup.seasonCarAmounts = null;
                            }
                            else
                            {
                                mCurrentSeasonCarAmount = CommonFuction.Clone<List<SeasonCarAmounts>>(mCurrentCar.seasonCar.seasonCarGroup.seasonCarAmounts);

                                foreach (SeasonCarAmounts carAmountItem in mCurrentSeasonCarAmount)
                                {
                                    if (carAmountItem.amount > 0)
                                    {
                                        CurrMonth = Convert.ToInt32(carAmountItem.month);
                                        SetRegCurrMonthSetting(CurrMonth);
                                        CurrMonthId = Convert.ToInt32(carAmountItem.id);
                                        break;
                                    }
                                }
                                TotDc = 0;
                                RecvAmt = 0;
                            }
                        }
                    }

                    SetCurrentStatusNotMoney();
                }
            }
            get { return mCurrentCar; }
        }

        public Payment CurrentPayment
        {
            set { mCurrentPayment = value; }
            get { return mCurrentPayment; }
        }

        public enum PaymentSetType
        {
            /// <summary>
            /// 요금부과시
            /// </summary>
            NormalPaymnet,

            /// <summary>
            /// 원격할인취소
            /// </summary>
            RemoteCard,

            /// <summary>
            /// 원격영수증
            /// </summary>
            RemoteReceipt
        }

        public void SetCurrentPayment(PaymentSetType pPaymentSetType)
        {
            if (mCurrentPayment != null)
            {
                PaymentId = mCurrentPayment.id;
                CarId = mCurrentPayment.car.id;
                TkNO = mCurrentPayment.car.tkNo;
                CarType = mCurrentPayment.car.carType;
                if (mCurrentPayment.car.inDate > 0)
                {
                    InYmd = NPSYS.LongTypeToDateTime(mCurrentPayment.car.inDate).ToString("yyyyMMdd");
                    InHms = NPSYS.LongTypeToDateTime(mCurrentPayment.car.inDate).ToString("HHmmss");
                }
                InImage1 = mCurrentPayment.car.inImage1;
                InCarNo1 = mCurrentPayment.car.inCarNo1;
                InImage2 = mCurrentPayment.car.inImage2;
                InCarNo2 = mCurrentPayment.car.inCarNo2;
                if (mCurrentPayment.car.outDate > 0)
                {
                    OutYmd = NPSYS.LongTypeToDateTime(mCurrentPayment.car.outDate).ToString("yyyyMMdd");
                    OutHms = NPSYS.LongTypeToDateTime(mCurrentPayment.car.outDate).ToString("HHmmss");
                    OutCarNo1 = mCurrentPayment.car.outCarNo1;
                }
                else
                {
                    OutYmd = DateTime.Now.ToString("yyyyMMdd");
                    OutHms = DateTime.Now.ToString("HHmmss");
                }
                if (NPSYS.gIsAutoBooth)
                {
                    OutChk = mCurrentPayment.car.outChk;
                    OutImage1 = mCurrentPayment.car.outImage1;
                    OutImage2 = mCurrentPayment.car.outImage2;
                }

                ParkingMin = mCurrentPayment.parkingMin;

                ElpaseMinute();
                if (pPaymentSetType == PaymentSetType.NormalPaymnet)
                {
                    try
                    {
                        TotFee = Convert.ToInt32(mCurrentPayment.totFee);
                        TotDc = Convert.ToInt32(mCurrentPayment.totDc);
                        RecvAmt = Convert.ToInt32(mCurrentPayment.recvAmt);
                        Change = Convert.ToInt32(mCurrentPayment.change);
                        DcCnt = mCurrentPayment.dcCnt;
                        Unpaid = mCurrentPayment.unpaid;
                        RealFee = Convert.ToInt32(mCurrentPayment.realFee);

                        LastPayDate = mCurrentPayment.lastPayDate;
                        if (mCurrentPayment.feeGroup != null)
                        {
                            if (mCurrentPayment.feeGroup.feeItem != null && mCurrentPayment.feeGroup.feeItem.Count > 0)
                            {
                                mMaxFee = Convert.ToInt32(mCurrentPayment.feeGroup.feeItem[0].maxFee);
                            }
                            mFreeTimeAfterPay = Convert.ToInt32(mCurrentPayment.feeGroup.freeTimeAfterPay);

                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NormalCarInfo | SetCurrentPayment", "[최대요금]" + mMaxFee.ToString());
                        }
                        SetCurrentStatus();
                    }
                    catch (Exception normalPaymnetex)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NormalCarInfo | SetCurrentPayment", normalPaymnetex.ToString());
                    }
                }
                else if (pPaymentSetType == PaymentSetType.RemoteCard)
                {
                    try
                    {
                        if (mCurrentPayment.paymentDetail != null)
                        {
                            PaymentDetailId = mCurrentPayment.paymentDetail[0].id;
                            TotFee = Convert.ToInt32(mCurrentPayment.totFee);
                            TotDc = Convert.ToInt32(mCurrentPayment.totDc);
                            RecvAmt = Convert.ToInt32(mCurrentPayment.recvAmt) - Convert.ToInt32(mCurrentPayment.paymentDetail[0].payAmt);
                            RealFee = Convert.ToInt32(mCurrentPayment.paymentDetail[0].payAmt);
                            VanRegNo_Cancle = mCurrentPayment.paymentDetail[0].card.vanRegNo;
                            VanDate_Cancle = NPSYS.LongTypeToDateTime(mCurrentPayment.paymentDetail[0].card.vanDate).ToString("yyyy-MM-dd");
                            VanTime_Cancle = NPSYS.LongTypeToDateTime(mCurrentPayment.paymentDetail[0].card.vanDate).ToString("HH:mm:ss");
                            if (NPSYS.gIsAutoBooth)
                            {
                                CurrentCarPayStatus = CarPayStatus.RemoteCancleCard_OutCar;
                            }
                            else
                            {
                                CurrentCarPayStatus = CarPayStatus.RemoteCancleCard_PreCar;
                            }
                        }
                        else
                        {
                            TotFee = Convert.ToInt32(mCurrentPayment.totFee);
                            TotDc = Convert.ToInt32(mCurrentPayment.totDc);
                            RecvAmt = Convert.ToInt32(mCurrentPayment.recvAmt);
                            Change = Convert.ToInt32(mCurrentPayment.change);
                            DcCnt = mCurrentPayment.dcCnt;
                            Unpaid = mCurrentPayment.unpaid;
                            RealFee = Convert.ToInt32(mCurrentPayment.realFee);
                            LastPayDate = mCurrentPayment.lastPayDate;
                            if (NPSYS.gIsAutoBooth)
                            {
                                CurrentCarPayStatus = CarPayStatus.RemoteCancleCard_OutCar_Fail;
                            }
                            else
                            {
                                CurrentCarPayStatus = CarPayStatus.RemoteCancleCard_PreCar_Fail;
                            }
                        }
                    }
                    catch (Exception RemoteCardex)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NormalCarInfo | SetCurrentPayment", RemoteCardex.ToString());
                        if (NPSYS.gIsAutoBooth)
                        {
                            CurrentCarPayStatus = CarPayStatus.RemoteCancleCard_OutCar_Fail;
                        }
                        else
                        {
                            CurrentCarPayStatus = CarPayStatus.RemoteCancleCard_PreCar_Fail;
                        }
                    }
                }
                else if (pPaymentSetType == PaymentSetType.RemoteReceipt)
                {
                    try
                    {
                        if (mCurrentPayment.paymentDetail != null && mCurrentPayment.paymentDetail.Count > 0)
                        {
                            // 시티벨리 패취해줘야함
                            if (mCurrentPayment.paymentDetail[0].payType == 1)  // 현금이라면
                            {
                                TotFee = Convert.ToInt32(mCurrentPayment.totFee);
                                TotDc = Convert.ToInt32(mCurrentPayment.totDc);
                                InComeMoney = Convert.ToInt32(mCurrentPayment.paymentDetail[0].recvAmt);
                                OutComeMoney = Convert.ToInt32(mCurrentPayment.paymentDetail[0].retAmt);
                                CurrentCarPayStatus = CarPayStatus.RemoteReceipt;

                                //    InComeMoney = Convert.ToInt32(mCurrentPayment.recvAmt);
                            }
                            else if (mCurrentPayment.paymentDetail[0].payType == 2) // 카드결제가있다면
                            {
                                TotFee = Convert.ToInt32(mCurrentPayment.totFee);
                                TotDc = Convert.ToInt32(mCurrentPayment.totDc);
                                RecvAmt = Convert.ToInt32(mCurrentPayment.recvAmt) - Convert.ToInt32(mCurrentPayment.paymentDetail[0].payAmt);
                                RealFee = Convert.ToInt32(mCurrentPayment.paymentDetail[0].payAmt);

                                VanAmt = Convert.ToInt32(mCurrentPayment.paymentDetail[0].payAmt);
                                VanRegNo = mCurrentPayment.paymentDetail[0].card.vanRegNo;
                                VanCardApproveYmd = NPSYS.LongTypeToDateTime(mCurrentPayment.paymentDetail[0].card.vanDate).ToString("yyyy-MM-dd");
                                VanCardApproveHms = NPSYS.LongTypeToDateTime(mCurrentPayment.paymentDetail[0].card.vanDate).ToString("HH:mm:ss");
                                VanCardApprovalYmd = VanCardApproveYmd;
                                VanCardApprovalHms = VanCardApproveHms;
                                VanIssueName = mCurrentPayment.paymentDetail[0].card.issueName;
                                VanCardNumber = mCurrentPayment.paymentDetail[0].card.cardNo;
                                CurrentCarPayStatus = CarPayStatus.RemoteReceipt;
                            }
                            else
                            {
                                TotFee = Convert.ToInt32(mCurrentPayment.totFee);
                                TotDc = Convert.ToInt32(mCurrentPayment.totDc);
                                RecvAmt = Convert.ToInt32(mCurrentPayment.recvAmt) - Convert.ToInt32(mCurrentPayment.paymentDetail[0].payAmt);
                                RealFee = Convert.ToInt32(mCurrentPayment.paymentDetail[0].payAmt);
                                CurrentCarPayStatus = CarPayStatus.RemoteReceipt;
                            }
                        }
                        else
                        {
                            TotFee = Convert.ToInt32(mCurrentPayment.totFee);
                            TotDc = Convert.ToInt32(mCurrentPayment.totDc);
                            RecvAmt = Convert.ToInt32(mCurrentPayment.recvAmt);
                            Change = Convert.ToInt32(mCurrentPayment.change);
                            DcCnt = mCurrentPayment.dcCnt;
                            Unpaid = mCurrentPayment.unpaid;
                            RealFee = Convert.ToInt32(mCurrentPayment.realFee);
                            LastPayDate = mCurrentPayment.lastPayDate;
                            CurrentCarPayStatus = CarPayStatus.RemoteReceipt_Fail;
                        }
                    }
                    catch (Exception RemoteCardex)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NormalCarInfo | SetCurrentPayment", RemoteCardex.ToString());
                        CurrentCarPayStatus = CarPayStatus.RemoteReceipt_Fail;
                    }
                }
            }
        }

        #region 주차요금 처리

        private int mMaxFee = 0;

        public int MaxFee
        {
            set { mMaxFee = value; }
            get { return mMaxFee; }
        }

        private int mParkingType = 1;
        private string mTkNO = string.Empty;
        private int mCarType = 1;

        public long PaymentId
        {
            set; get;
        }

        public long CarId
        {
            set; get;
        }

        /// <summary>
        /// 0:일반,1:방문,2:정기,3:회차
        /// </summary>
        public int parkingType
        {
            set { mParkingType = value; }
            get { return mParkingType; }
        }

        /// <summary>
        /// 0:경차, 1: 소형,2:중형,3:대형
        /// </summary>
        public int CarType
        {
            set { mCarType = value; }
            get { return mCarType; }
        }

        public string TkNO
        {
            set { mTkNO = value; }
            get { return mTkNO; }
        }

        public long ParkingMin
        {
            set; get;
        }

        public int PaymentMoney
        {
            get
            {
                int paymentMoney = TotFee - TotDc - (RecvAmt - Change) - Current_Money - VanAmt - TMoneyPay; //시제설정누락처리

                if (paymentMoney < 0)
                    return 0;
                else
                    return paymentMoney;
            }
        }

        /// <summary>
        /// 할인하고 남은금액
        /// </summary>
        public int PaymentMoneyAfterDc // 할인하고 남은금액
        {
            get
            {
                int PaymentMoneyAfterDc = TotFee - TotDc - (RecvAmt - Change); //시제설정누락처리

                if (PaymentMoneyAfterDc < 0)
                    return 0;
                else
                    return PaymentMoneyAfterDc;
            }
        }

        /// <summary>
        /// 할인하기전 금액(기존받은금액 제외)
        /// </summary>
        public int PaymentMoneyBeforeDc
        {
            get
            {
                int PaymentMoneyAfterDc = TotFee - (RecvAmt - Change); //시제설정누락처리

                if (PaymentMoneyAfterDc < 0)
                    return 0;
                else
                    return PaymentMoneyAfterDc;
            }
        }

        /// <summary>
        /// 전체 주차금액
        /// </summary>
        public int TotFee
        {
            set; get;
        }

        /// <summary>
        /// 사전정산 받은금액
        /// </summary>
        public int RecvAmt
        {
            set; get;
        }

        /// <summary>
        /// 총할인금액
        /// </summary>
        public int TotDc
        {
            set; get;
        }

        private int mFreeTimeAfterPay = 0;

        public int FreeTimeAfterPay
        {
            set { mFreeTimeAfterPay = value; }
            get { return mFreeTimeAfterPay; }
        }

        /// <summary>
        /// 현재 남은 결제금액
        /// </summary>
        public int RealFee
        {
            set; get;
        }

        /// <summary>
        /// 사전정산기 거스름돈
        /// </summary>
        public int Change
        {
            set; get;
        }

        public string InCarNo2
        {
            set; get;
        }

        public string InCarNo1
        {
            set; get;
        }

        public string OutCarNo1
        {
            set; get;
        }

        public string OutCarNo2
        {
            set; get;
        }

        public string InImage1
        {
            set; get;
        }

        public string InImage2
        {
            set; get;
        }

        public string OutImage1
        {
            set; get;
        }

        public string OutImage2
        {
            set; get;
        }

        public string InYmd
        {
            set; get;
        }

        public string InHms
        {
            set; get;
        }

        public string OutYmd
        {
            set; get;
        }

        public string OutHms
        {
            set; get;
        }

        public int OutChk
        {
            set; get;
        }

        public long LastPayDate
        {
            set; get;
        }

        public int inRecog1
        {
            set; get;
        }

        public int inRecog2
        {
            set; get;
        }

        /// <summary>
        /// 할인개수
        /// </summary>
        public int DcCnt
        {
            set; get;
        }

        /// <summary>
        /// 미지불금액
        /// </summary>
        public long Unpaid
        {
            set; get;
        }

        #endregion 주차요금 처리

        #region 정산시점에서 차량의 정산상태 확인

        /// <summary>
        /// 정산시점에 차량의 정산상태
        /// </summary>
        public enum CarPayStatus
        {
            RemoteReceipt_Fail,
            RemoteReceipt,

            /// <summary>
            /// 출구무인 원격카드취소 전문오류
            /// </summary>
            RemoteCancleCard_OutCar_Fail,

            /// <summary>
            /// 사전무인 원격카드취소 전문오류
            /// </summary>
            RemoteCancleCard_PreCar_Fail,

            /// <summary>
            /// 출구무인 원격카드취소
            /// </summary>
            RemoteCancleCard_OutCar,

            /// <summary>
            /// 사전무인 원격카드취소
            /// </summary>
            RemoteCancleCard_PreCar,

            /// <summary>
            /// 출구무인 일반요금부과대상
            /// </summary>
            NotFree_OutCar_None,

            /// <summary>
            /// 사전무인 일반요금부과대상
            /// </summary>
            NotFree_PreCar_None,

            /// <summary>
            /// 출구무인 회차차량
            /// </summary>
            Free_OutCar_Hwacha_,

            /// <summary>
            /// 사전무인 회차차량
            /// </summary>
            Free_PreCar_Hwacha,

            /// <summary>
            /// 사전할인받아 0원인 사전정산차량
            /// </summary>
            Free_PreCar_PreDisocunt,

            /// <summary>
            /// 사전할인받아 0원인 출구무인차량
            /// </summary>
            Free_OutCar_PreDisocunt,

            /// <summary>
            /// 사전할인받아 요금이 있는 사전무인정산차량
            /// </summary>
            NotFree_PreCar_PreDiscount,

            /// <summary>
            /// 사전할인받아 요금이 있는 출구무인정산차량
            /// </summary>
            NotFree_OutCar_PreDiscount,

            /// <summary>
            /// 사전정산후 사전에서 재정산하려는 경우 요금이 없는경우
            /// </summary>
            Free_PreCar_PrePay_RePay,

            /// <summary>
            /// 사전정산후 사전에서 재정산하려는 경우 요금이 있는경우
            /// </summary>
            NotFree_PreCar_PrePay_RePay,

            /// <summary>
            /// 사전정산후 출차시 요금이 없는경우
            /// </summary>
            Free_OutCar_PrePay,

            /// <summary>
            /// 사전정산후 출차시 요금이 있는경우
            /// </summary>
            NotFree_OutCar_PrePay,

            /// <summary>
            /// 출차시 연장정기권인경우
            /// </summary>
            Reg_Outcar_Season,

            /// <summary>
            /// 출차시 비연장정기권인경우
            /// </summary>
            Reg_Outcar_NotSeason,

            /// <summary>
            /// 출차시 요금나오는 정기권인경우
            /// </summary>
            Reg_Outcar_Money,

            /// <summary>
            /// 만료 출차정산연장차량
            /// </summary>
            RegExtension_Outcar,

            /// <summary>
            /// 사전정산시 연장정기권인경우
            /// </summary>
            Reg_Precar_Season,

            /// <summary>
            /// 사전정산시 비연장정기권인경우
            /// </summary>
            Reg_Precar_NotSeason,

            /// <summary>
            /// 사전정산시 요금나오는 정기권인경우
            /// </summary>
            Reg_Precar_Money,

            /// <summary>
            /// 만료 사전정산연장차량
            /// </summary>
            RegExtension_Precar,

            NotSearch_Precar,

            /// <summary>
            /// 출차시 LPR에 찍혔을때 정체를 알수없는차량(오인식 미인식등)
            /// </summary>
            NotSearch_Outcar
        }

        private CarPayStatus mCurrentCarPayStatus = CarPayStatus.NotFree_OutCar_None;

        public CarPayStatus CurrentCarPayStatus
        {
            set { mCurrentCarPayStatus = value; }
            get { return mCurrentCarPayStatus; }
        }

        public void SetCurrentStatusNotMoney()
        {
            long monthRegMoney = 0;
            if (mCurrentSeasonCarAmount != null)
            {
                foreach (SeasonCarAmounts carAmountItem in mCurrentSeasonCarAmount)
                {
                    monthRegMoney += carAmountItem.amount;
                }
            }
            if (parkingType == 2) // 정기차량인경우
            {
                if (NPSYS.gIsAutoBooth)
                {
                    if (monthRegMoney > 0)
                    {
                        mCurrentCarPayStatus = CarPayStatus.Reg_Outcar_Season;
                    }
                    else
                    {
                        mCurrentCarPayStatus = CarPayStatus.Reg_Outcar_NotSeason;
                    }
                }
                //정기권 요금연장이 필요한 차량인경우
                //정기권 요금연장이 필요없는 경우
            }
            else  // 방문차량인경우
            {
                if (NPSYS.gIsAutoBooth)
                {
                    mCurrentCarPayStatus = CarPayStatus.NotSearch_Outcar;
                }
                else
                {
                    mCurrentCarPayStatus = CarPayStatus.NotSearch_Precar;
                }
                //입차조회 안되는 차량(오인식,미인식 등등의 사유)
            }
        }

        /// <summary>
        /// 요금이 있는 차량에 대한 상태정의
        /// </summary>
        public void SetCurrentStatus()
        {
            if (mCurrentPayment.lastPayDate > 0) // 기존에 정산을 한경우
            {
                if (NPSYS.gIsAutoBooth) // 출구인경우
                {
                    if (mCurrentPayment.realFee == 0) // 받을금액이 없는경우
                    {
                        CurrentCarPayStatus = CarPayStatus.Free_OutCar_PrePay;
                    }
                    else // 받을금액이 있는경우
                    {
                        CurrentCarPayStatus = CarPayStatus.NotFree_OutCar_PrePay;
                    }
                }
                else // 사전인경우
                {
                    if (mCurrentPayment.totFee - TotDc - RecvAmt <= 0) //받을금액이 없는경우
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NormalCarInfo | SetCurrentStatus", "[현재차량상태]" + "사전정산한 현재 무료차량");
                        CurrentCarPayStatus = CarPayStatus.Free_PreCar_PrePay_RePay;
                    }
                    else
                    {
                        CurrentCarPayStatus = CarPayStatus.NotFree_PreCar_PrePay_RePay;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NormalCarInfo | SetCurrentStatus", "[현재차량상태]" + "사전정산한 추가요금이 나온차량");
                    }
                }
            }
            else // 기존에 정산을 하지안않은경우
            {
                if (NPSYS.gIsAutoBooth) // 출구인경우
                {
                    if (mCurrentPayment.totDc > 0) // 할인받았을때
                    {
                        if (mCurrentPayment.realFee == 0) // 받을금액이 0원이라면
                        {
                            CurrentCarPayStatus = CarPayStatus.Free_OutCar_PreDisocunt;
                        }
                        else
                        {
                            CurrentCarPayStatus = CarPayStatus.NotFree_OutCar_PreDiscount;
                        }
                    }
                    else
                    {
                        if (mCurrentPayment.realFee == 0)
                        {
                            mCurrentCarPayStatus = CarPayStatus.Free_OutCar_Hwacha_;
                        }
                        else
                        {
                            mCurrentCarPayStatus = CarPayStatus.NotFree_OutCar_None;
                        }
                    }
                }
                else // 사전인경우
                {
                    if (mCurrentPayment.totDc > 0) // 할인받았을때
                    {
                        if (mCurrentPayment.realFee == 0) // 받을금액이 0원이라면
                        {
                            CurrentCarPayStatus = CarPayStatus.Free_PreCar_PreDisocunt;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NormalCarInfo | SetCurrentStatus", "[현재차량상태]" + "사전할인 받은 무료인 0원차량");
                        }
                        else
                        {
                            CurrentCarPayStatus = CarPayStatus.NotFree_PreCar_PreDiscount;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NormalCarInfo | SetCurrentStatus", "[현재차량상태]" + "사전할인 받은 추가로 주차요금이 남은차량");
                        }
                    }
                    else
                    {
                        if (mCurrentPayment.realFee == 0)
                        {
                            mCurrentCarPayStatus = CarPayStatus.Free_PreCar_Hwacha;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NormalCarInfo | SetCurrentStatus", "[현재차량상태]" + "현재 회차인 차량");
                        }
                        else
                        {
                            mCurrentCarPayStatus = CarPayStatus.NotFree_PreCar_None;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NormalCarInfo | SetCurrentStatus", "[현재차량상태]" + "사전무인 일반요금 부과되는 차량");
                        }
                    }
                }
            }
        }

        #endregion 정산시점에서 차량의 정산상태 확인

        #region 신용카드

        private long mPaymentDetailId = 0;

        public long PaymentDetailId
        {
            set { mPaymentDetailId = value; }
            get { return mPaymentDetailId; }
        }

        private int mVanCheck = 1;
        private int mVanAmt = 0;
        private string mVanRegNo = string.Empty;
        private string mVanDate = string.Empty;
        private string mVanTime = string.Empty;
        private string mVanRescode = string.Empty;
        private string mVanResMsg = string.Empty;
        private string mVanResMsg2 = string.Empty;
        private int mVanSupplyPay = 0;
        private int mVanTaxPay = 0;

        private string mVanCardName = string.Empty;
        private string mVanCardNumber = string.Empty;
        private string mVanCardFullNumber = string.Empty;
        private string mVandCardApproveYmd = string.Empty;
        private string mVanCardApproveHms = string.Empty;
        private string mVanCardApprovalYmd = string.Empty;
        private string mVanCardApprovalHms = string.Empty;

        private string mVanIssueCode = string.Empty;
        private string mVanCardMagneMentCode = string.Empty;
        private string mVanCardMemberCode = string.Empty;
        private string mVanIssueName = string.Empty;

        private int mVanBeforeCardPay = 0;
        private string mVanCardAcquirerCode = string.Empty;
        private string mVanCardAcquirerName = string.Empty;
        private string mVanCardOriginalNumber = string.Empty;

        /// <summary>
        /// 1: 결제성공 2:결제실패 3:카드취소
        /// </summary>
        public int VanCheck
        {
            set { mVanCheck = value; }
            get { return mVanCheck; }
        }

        /// <summary>
        /// 신용카드 금액
        /// </summary>
        public int VanAmt
        {
            set { mVanAmt = value; }
            get { return mVanAmt; }
        }

        /// <summary>
        /// 승인번호
        /// </summary>
        public string VanRegNo
        {
            set { mVanRegNo = value; }
            get { return mVanRegNo; }
        }

        public string VanDate_Cancle
        {
            set; get;
        }

        public string VanTime_Cancle
        {
            set; get;
        }

        public string VanRegNo_Cancle
        {
            set; get;
        }

        /// <summary>
        /// 승인일자
        /// </summary>
        public string VanDate
        {
            set { mVanDate = value; }
            get { return mVanDate; }
        }

        /// <summary>
        /// 승인일자
        /// </summary>
        public string VanTime
        {
            set { mVanTime = value; }
            get { return mVanTime; }
        }

        /// <summary>
        /// 카드 승인응답코드
        /// </summary>
        public string VanRescode
        {
            set { mVanRescode = value; }
            get { return mVanRescode; }
        }

        /// <summary>
        /// 카드 응답메시지
        /// </summary>
        public string VanResMsg
        {
            set { mVanResMsg = value; }
            get { return mVanResMsg; }
        }

        /// <summary>
        /// 카드 응답메시지
        /// </summary>
        public string VanResMsg2
        {
            set { mVanResMsg2 = value; }
            get { return mVanResMsg2; }
        }

        /// <summary>
        /// 공급가
        /// </summary>
        public int VanSupplyPay
        {
            set { mVanSupplyPay = value; }
            get { return mVanSupplyPay; }
        }

        public int VanTaxPay
        {
            set { mVanTaxPay = value; }
            get { return mVanTaxPay; }
        }

        public string VanCardName
        {
            set { mVanCardName = value; }
            get { return mVanCardName; }
        }

        public string VanCardNumber
        {
            set { mVanCardNumber = value; }
            get { return mVanCardNumber; }
        }

        public string VanCardFullNumber
        {
            set { mVanCardFullNumber = value; }
            get { return mVanCardFullNumber; }
        }

        public string VanCardApproveYmd
        {
            set { mVandCardApproveYmd = value; }
            get { return mVandCardApproveYmd; }
        }

        public string VanCardApproveHms
        {
            set { mVanCardApproveHms = value; }
            get { return mVanCardApproveHms; }
        }

        public string VanCardApprovalYmd
        {
            set { mVanCardApprovalYmd = value; }
            get { return mVanCardApprovalYmd; }
        }

        public string VanCardApprovalHms
        {
            set { mVanCardApprovalHms = value; }
            get { return mVanCardApprovalHms; }
        }

        public string VanCardOriginalNumber
        {
            set { mVanCardOriginalNumber = value; }
            get { return mVanCardOriginalNumber; }
        }

        /// <summary>
        /// 발급사코드
        /// </summary>
        public string VanIssueCode
        {
            set { mVanIssueCode = value; }
            get { return mVanIssueCode; }
        }

        /// <summary>
        /// 발급사명
        /// </summary>
        public string VanIssueName
        {
            set { mVanIssueName = value; }
            get { return mVanIssueName; }
        }

        public string VanCardMagneMentCode
        {
            set { mVanCardMagneMentCode = value; }
            get { return mVanCardMagneMentCode; }
        }

        /// <summary>
        /// 카드 가맹정코드
        /// </summary>
        public string VanCardMemberCode
        {
            set { mVanCardMemberCode = value; }
            get { return mVanCardMemberCode; }
        }

        public int VanBeforeCardPay
        {
            set { mVanBeforeCardPay = value; }
            get { return mVanBeforeCardPay; }
        }

        /// <summary>
        /// 매입사코드
        /// </summary>
        public string VanCardAcquirerCode
        {
            set { mVanCardAcquirerCode = value; }
            get { return mVanCardAcquirerCode; }
        }

        /// <summary>
        /// 매입사명
        /// </summary>
        public string VanCardAcquirerName
        {
            set { mVanCardAcquirerName = value; }
            get { return mVanCardAcquirerName; }
        }

        #endregion 신용카드

        #region 현금관련 변수

        private int mInCome50Qty = 0;
        private int mInCome100Qty = 0;
        private int mInCome500Qty = 0;
        private int mInCome1000Qty = 0;
        private int mInCome5000Qty = 0;
        private int mInCome10000Qty = 0;
        private int mInCome50000Qty = 0;

        private int mOutCome50Qty = 0;
        private int mOutCome100Qty = 0;
        private int mOutCome500Qty = 0;
        private int mOutCome1000Qty = 0;
        private int mOutCome5000Qty = 0;
        private int mOutCome10000Qty = 0;
        private int mOutCome50000Qty = 0;

        private int mCancle5000Qty = 0;
        private int mCancle1000Qty = 0;
        private int mCancle500Qty = 0;
        private int mCancle100Qty = 0;
        private int mCancle50Qty = 0;

        private int mCharge5000Qty = 0;
        private int mCharge1000Qty = 0;
        private int mCharge500Qty = 0;
        private int mCharge100Qty = 0;
        private int mCharge50Qty = 0;

        /// <summary>
        /// 방출한 시간
        /// </summary>
        public DateTime CurrentMoneyOutTime = DateTime.Now;

        public MoneyInOutType CurrentMoneyInOutType = MoneyInOutType.SuccessOut;

        public enum MoneyInOutType
        {
            /// <summary>
            /// 정상완료방출
            /// </summary>
            SuccessOut = 0,

            /// <summary>
            /// 정산완료미방출
            /// </summary>
            SuccessNotOut = 1,

            /// <summary>
            /// 취소완료방출
            /// </summary>
            CancelOut = 2,

            /// <summary>
            /// 취소완료미방출
            /// </summary>
            CancelNotOut = 3,

            /// <summary>
            /// 강제입금
            /// </summary>
            CommandOut = 4,

            /// <summary>
            /// 장애시 방출완료 재전송
            /// </summary>
            ErrorOut = 5,

            /// <summary>
            /// 장애시 방출미완료 재전송
            /// </summary>
            ErrorNotOut = 6
        }

        public bool isBillUse()
        {
            int countMoney = InCome10000Qty + InCome1000Qty + InCome100Qty + InCome50000Qty + InCome5000Qty + InCome500Qty + InCome50Qty
                           + OutCome10000Qty + OutCome1000Qty + OutCome100Qty + OutCome50000Qty + OutCome5000Qty + OutCome500Qty + OutCome50Qty;
            if (countMoney > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 방출한경우 입수권종의 수량과 방출권종의 수량이 다른경우 센터에 보내서 저장한다.
        /// </summary>
        /// <returns></returns>
        public bool isCancleOutchargeSave()
        {
            if (InCome50Qty != OutCome50Qty)
            {
                return true;
            }
            if (InCome100Qty != OutCome100Qty)
            {
                return true;
            }
            if (InCome500Qty != OutCome500Qty)
            {
                return true;
            }
            if (InCome1000Qty != OutCome1000Qty)
            {
                return true;
            }
            if (GetInComeMoney != GetOutComeMoney)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 현재 거스름돈
        /// </summary>
        public int BillCointChange
        {
            get
            {
                if (GetInComeMoney > PaymentMoneyAfterDc)
                {
                    return GetInComeMoney - PaymentMoneyAfterDc;
                }
                else
                {
                    return 0;
                }
            }
        }

        private int mInComeMoney = 0;

        public int InComeMoney
        {
            get { return mInComeMoney; }
            set { mInComeMoney = value; }
        }

        /// <summary>
        /// 현재 입수된 지폐 및 동전을 금액으로합산한다.
        /// </summary>
        /// <returns></returns>
        public int GetInComeMoney
        {
            get
            {
                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    return ((InCome50Qty * 50)
              + (InCome100Qty * 100)
              + (InCome500Qty * 500)
              + (InCome1000Qty * 1000)
              + (InCome5000Qty * 5000)
              + (InCome10000Qty * 10000)
              + (InCome50000Qty * 50000)
              );
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    return ((InCome50Qty * 1)
              + (InCome100Qty * 5)
              + (InCome500Qty * 10)
              + (InCome1000Qty * 20)
              + (InCome5000Qty * 50)
              + (InCome10000Qty * 100)
              + (InCome50000Qty * 200)
              );
                }
                else
                {
                    return 0;
                }
            }
        }

        public int InCome50Qty
        {
            get { return mInCome50Qty; }
            set
            {
                mInCome50Qty = value;
                CalculateCancleMoney();
                CalculateCharge();
            }
        }

        public int InCome100Qty
        {
            get { return mInCome100Qty; }
            set
            {
                mInCome100Qty = value;
                CalculateCancleMoney();
                CalculateCharge();
            }
        }

        public int InCome500Qty
        {
            get { return mInCome500Qty; }
            set
            {
                mInCome500Qty = value;
                CalculateCancleMoney();
                CalculateCharge();
            }
        }

        public int InCome1000Qty
        {
            get { return mInCome1000Qty; }
            set
            {
                mInCome1000Qty = value;
                CalculateCancleMoney();
                CalculateCharge();
            }
        }

        public int InCome5000Qty
        {
            get { return mInCome5000Qty; }
            set
            {
                mInCome5000Qty = value;
                CalculateCancleMoney();
                CalculateCharge();
            }
        }

        public int InCome10000Qty
        {
            get { return mInCome10000Qty; }
            set
            {
                mInCome10000Qty = value;
                CalculateCancleMoney();
                CalculateCharge();
            }
        }

        public int InCome50000Qty
        {
            get { return mInCome50000Qty; }
            set
            {
                mInCome50000Qty = value;
                CalculateCancleMoney();
                CalculateCharge();
            }
        }

        private int mOutComeMoney = 0;

        public int OutComeMoney
        {
            get { return mOutComeMoney; }
            set { mOutComeMoney = value; }
        }

        /// <summary>
        /// 현재 방출한 수량에 대해서 금액을 합산한다.
        /// </summary>
        public int GetOutComeMoney
        {
            get
            {
                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    return ((OutCome50Qty * 50)
                          + (OutCome100Qty * 100)
                          + (OutCome500Qty * 500)
                          + (OutCome1000Qty * 1000)
                          + (OutCome5000Qty * 5000)
                          + (OutCome10000Qty * 10000)
                          + (OutCome50000Qty * 50000)
                          );
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    return ((OutCome50Qty * 1)
                          + (OutCome100Qty * 5)
                          + (OutCome500Qty * 10)
                          + (OutCome1000Qty * 20)
                          + (OutCome5000Qty * 50)
                          + (OutCome10000Qty * 100)
                          + (OutCome50000Qty * 200)
                          );
                }
                else
                {
                    return 0;
                }
            }
        }

        public int OutCome50Qty
        {
            get { return mOutCome50Qty; }
            set { mOutCome50Qty = value; }
        }

        public int OutCome100Qty
        {
            get { return mOutCome100Qty; }
            set { mOutCome100Qty = value; }
        }

        public int OutCome500Qty
        {
            get { return mOutCome500Qty; }
            set { mOutCome500Qty = value; }
        }

        public int OutCome1000Qty
        {
            get { return mOutCome1000Qty; }
            set { mOutCome1000Qty = value; }
        }

        public int OutCome5000Qty
        {
            get { return mOutCome5000Qty; }
            set { mOutCome5000Qty = value; }
        }

        public int OutCome10000Qty
        {
            get { return mOutCome10000Qty; }
            set { mOutCome10000Qty = value; }
        }

        public int OutCome50000Qty
        {
            get { return mOutCome50000Qty; }
            set { mOutCome50000Qty = value; }
        }

        /// <summary>
        /// 현재 일자
        /// </summary>
        public string CurrentDate { get; set; }

        /// <summary>
        /// 현재 입수금 - 방출금
        /// </summary>
        public int Current_Money
        {
            get { return (GetInComeMoney - GetOutComeMoney); }
        }

        /// <summary>
        /// 현재 투입 금액 5000원권 수량
        /// </summary>
        public int Cancle5000Qty
        {
            get { return mCancle5000Qty; }
            set { mCancle5000Qty = value; }
        }

        /// <summary>
        /// 현재 투입 금액 1000원권 수량
        /// </summary>
        public int Cancle1000Qty
        {
            get { return mCancle1000Qty; }
            set { mCancle1000Qty = value; }
        }

        /// <summary>
        /// 현재 투입 금액 500원 수량
        /// </summary>
        public int Cancle500Qty
        {
            get { return mCancle500Qty; }
            set { mCancle500Qty = value; }
        }

        /// <summary>
        /// 현재 투입 금액 100원 수량
        /// </summary>
        public int Cancle100Qty
        {
            get { return mCancle100Qty; }
            set { mCancle100Qty = value; }
        }

        /// <summary>
        /// 현재 투입 금액 50원 수량
        /// </summary>
        public int Cancle50Qty
        {
            get { return mCancle50Qty; }
            set { mCancle50Qty = value; }
        }

        /// <summary>
        /// 현재 투입 금액 동전 금액
        /// </summary>
        public int CurrentCancleCoinMoney
        {
            get
            {
                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    return (Cancle500Qty * 500) + (Cancle100Qty * 100) + (Cancle50Qty * 50);
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    return (Cancle500Qty * 10) + (Cancle100Qty * 5) + (Cancle50Qty * 1);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 방출해야할 거스름돈 총액
        /// </summary>
        /// <returns></returns>
        public int GetChargeMoney
        {
            get
            {
                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    return ((Charge50Qty * 50)
              + (Charge100Qty * 100)
              + (Charge500Qty * 500)
              + (Charge1000Qty * 1000)
              + (Charge5000Qty * 5000)

              );
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    return ((Charge50Qty * 1)
              + (Charge100Qty * 5)
              + (Charge500Qty * 10)
              + (Charge1000Qty * 20)
              + (Charge5000Qty * 50)

              );
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 방출해야할 거스름 돈 5000원권 수량
        /// </summary>
        public int Charge5000Qty
        {
            get { return mCharge5000Qty; }
            set { mCharge5000Qty = value; }
        }

        /// <summary>
        /// 방출해야할 거스름 돈 1000원권 수량
        /// </summary>
        public int Charge1000Qty
        {
            get { return mCharge1000Qty; }
            set { mCharge1000Qty = value; }
        }

        /// <summary>
        /// 방출해야할 거스름 돈 500원 수량
        /// </summary>
        public int Charge500Qty
        {
            get { return mCharge500Qty; }
            set { mCharge500Qty = value; }
        }

        /// <summary>
        /// 방출해야할 거스름 돈 100원 수량
        /// </summary>
        public int Charge100Qty
        {
            get { return mCharge100Qty; }
            set { mCharge100Qty = value; }
        }

        /// <summary>
        /// 방출해야할 거스름 돈 50원 수량
        /// </summary>
        public int Charge50Qty
        {
            get { return mCharge50Qty; }
            set { mCharge50Qty = value; }
        }

        /// <summary>
        /// 거스름돈 동전 금액
        /// </summary>
        public int ChargeCoinMoney
        {
            get
            {
                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    return (Charge500Qty * 500) + (Charge100Qty * 100) + (Charge50Qty * 50);
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    return (Charge500Qty * 10) + (Charge100Qty * 5) + (Charge50Qty * 1);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 방출할려는 금액중에 방출안된 금액(보관증 발행시 사용 변수)
        /// </summary>
        public int GetNotDisChargeMoney
        {
            get
            {
                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    return
                    ((mNotdisChargeMoney50Qty * 50)
                  + (mNotdisChargeMoney100Qty * 100)
                  + (mNotdisChargeMoney500Qty * 500)
                  + (mNotdisChargeMoney1000Qty * 1000)
                  + (mNotdisChargeMoney5000Qty * 5000)
                  + (mNotdisChargeMoney10000Qty * 10000)
                  + (mNotdisChargeMoney50000Qty * 50000));
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    return
                    ((mNotdisChargeMoney50Qty * 1)
                  + (mNotdisChargeMoney100Qty * 5)
                  + (mNotdisChargeMoney500Qty * 10)
                  + (mNotdisChargeMoney1000Qty * 20)
                  + (mNotdisChargeMoney5000Qty * 50)
                  + (mNotdisChargeMoney10000Qty * 100)
                  + (mNotdisChargeMoney50000Qty * 200));
                }
                else
                {
                    return 0;
                }
            }
        }

        public void ClearDischargeMoney()
        {
            mNotdisChargeMoney50Qty = 0;
            mNotdisChargeMoney100Qty = 0;
            mNotdisChargeMoney500Qty = 0;
            mNotdisChargeMoney1000Qty = 0;
            mNotdisChargeMoney5000Qty = 0;
            mNotdisChargeMoney10000Qty = 0;
            mNotdisChargeMoney50000Qty = 0;
        }

        private int mNotdisChargeMoney50Qty = 0;

        public int NotdisChargeMoney50Qty
        {
            set { mNotdisChargeMoney50Qty = value; }
            get { return mNotdisChargeMoney50Qty; }
        }

        private int mNotdisChargeMoney100Qty = 0;

        public int NotdisChargeMoney100Qty
        {
            set { mNotdisChargeMoney100Qty = value; }
            get { return mNotdisChargeMoney100Qty; }
        }

        private int mNotdisChargeMoney500Qty = 0;

        public int NotdisChargeMoney500Qty
        {
            set { mNotdisChargeMoney500Qty = value; }
            get { return mNotdisChargeMoney500Qty; }
        }

        private int mNotdisChargeMoney1000Qty = 0;

        public int NotdisChargeMoney1000Qty
        {
            set { mNotdisChargeMoney1000Qty = value; }
            get { return mNotdisChargeMoney1000Qty; }
        }

        private int mNotdisChargeMoney5000Qty = 0;

        public int NotdisChargeMoney5000Qty
        {
            set { mNotdisChargeMoney5000Qty = value; }
            get { return mNotdisChargeMoney5000Qty; }
        }

        private int mNotdisChargeMoney10000Qty = 0;

        public int NotdisChargeMoney10000Qty
        {
            set { mNotdisChargeMoney10000Qty = value; }
            get { return mNotdisChargeMoney10000Qty; }
        }

        private int mNotdisChargeMoney50000Qty = 0;

        public int NotdisChargeMoney50000Qty
        {
            set { mNotdisChargeMoney50000Qty = value; }
            get { return mNotdisChargeMoney50000Qty; }
        }

        /// <summary>
        /// 거스름돈을 수량을 모두 0으로 변경한다.
        /// </summary>
        public void SetResetChargeMoney()
        {
            mCharge5000Qty = 0;
            mCharge1000Qty = 0;
            mCharge500Qty = 0;
            mCharge100Qty = 0;
            mCharge50Qty = 0;
        }

        /// <summary>
        /// 현재 금액 계산
        /// </summary>
        /// <returns></returns>
        public bool CalculateCancleMoney()
        {
            int temp = GetInComeMoney;
            if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
            {
                mCancle5000Qty = temp / 5000;

                temp = temp % 5000;

                mCancle1000Qty = temp / 1000;

                temp = temp % 1000;

                mCancle500Qty = temp / 500;

                temp = temp % 500;

                mCancle100Qty = temp / 100;

                temp = temp % 100;

                mCancle50Qty = temp / 50;
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                mCancle5000Qty = temp / 50;

                temp = temp % 50;

                mCancle1000Qty = temp / 20;

                temp = temp % 20;

                mCancle500Qty = temp / 10;

                temp = temp % 10;

                mCancle100Qty = temp / 5;

                temp = temp % 5;

                mCancle50Qty = temp / 1;
            }

            return true;
        }

        /// <summary>
        /// 거스름 돈 계산
        /// </summary>
        /// <returns></returns>
        public bool CalculateCharge()
        {
            mCharge50Qty = 0;
            mCharge100Qty = 0;
            mCharge500Qty = 0;
            mCharge1000Qty = 0;
            mCharge5000Qty = 0;
            int chargeMoney = 0;
            if (GetInComeMoney + TotDc + (RecvAmt - Change) > TotFee) //현재 투입금액+할인금액이이 주차요금보다 많을때 //시제설정누락처리
            {
                chargeMoney = GetInComeMoney + TotDc + (RecvAmt - Change) - TotFee; //시제설정누락처리
            }

            if (chargeMoney < 0)
            {
                chargeMoney = 0;
            }

            int temp = chargeMoney;
            if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
            {
                mCharge5000Qty = temp / 5000;

                temp = temp % 5000;

                mCharge1000Qty = temp / 1000;

                temp = temp % 1000;

                mCharge500Qty = temp / 500;

                temp = temp % 500;

                mCharge100Qty = temp / 100;

                temp = temp % 100;

                mCharge50Qty = temp / 50;
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                mCharge5000Qty = temp / 50;

                temp = temp % 50;

                mCharge1000Qty = temp / 20;

                temp = temp % 20;

                mCharge500Qty = temp / 10;

                temp = temp % 10;

                mCharge100Qty = temp / 5;

                temp = temp % 5;

                mCharge50Qty = temp / 1;
            }

            return true;
        }

        //시제설정누락처리
        public void CanCleClear() // 2016.06.23 동전방출 수정
        {
            CurrentMoneyOutTime = DateTime.Now;

            this.mPaymentMethod = PaymentType.Cash;

            mCharge50Qty = 0;
            mCharge100Qty = 0;
            mCharge500Qty = 0;
            mCharge1000Qty = 0;
            mCharge5000Qty = 0;

            mInComeMoney = 0;

            mInCome50Qty = 0;
            mInCome100Qty = 0;
            mInCome500Qty = 0;
            mInCome1000Qty = 0;
            mInCome5000Qty = 0;
            mInCome10000Qty = 0;
            mInCome50000Qty = 0;

            mOutComeMoney = 0;

            mOutCome50Qty = 0;
            mOutCome100Qty = 0;
            mOutCome500Qty = 0;
            mOutCome1000Qty = 0;
            mOutCome5000Qty = 0;
            mOutCome10000Qty = 0;
            mOutCome50000Qty = 0;

            mCancle50Qty = 0;
            mCancle100Qty = 0;
            mCancle500Qty = 0;
            mCancle1000Qty = 0;
            mCancle5000Qty = 0;

            mCharge50Qty = 0;
            mCharge50Qty = 0;
            mCharge100Qty = 0;
            mCharge500Qty = 0;
            mCharge1000Qty = 0;
            mCharge5000Qty = 0;

            mNotdisChargeMoney50Qty = 0;
            mNotdisChargeMoney100Qty = 0;
            mNotdisChargeMoney500Qty = 0;
            mNotdisChargeMoney1000Qty = 0;
            mNotdisChargeMoney5000Qty = 0;
            mNotdisChargeMoney10000Qty = 0;
            mNotdisChargeMoney50000Qty = 0;
        }

        //시제설정누락처리 처리완료

        #endregion 현금관련 변수

        #region 그룹별정기권 연장및금액처리

        private string mNextStartYmd = string.Empty;
        private string mNextExpiredYmd = string.Empty;
        private int mCurrMonth = 0;
        private long mCurrMonthId = 0;

        // 정기권 개월별 금액
        private int mAddMonth_FixRegPay = 0;

        private int mMonth1_RegPay = 0;
        private int mMonth2_RegPay = 0;
        private int mMonth3_RegPay = 0;
        private int mMonth4_RegPay = 0;
        private int mMonth5_RegPay = 0;
        private int mMonth6_RegPay = 0;
        private int mMonth7_RegPay = 0;
        private int mMonth8_RegPay = 0;
        private int mMonth9_RegPay = 0;
        private int mMonth10_RegPay = 0;
        private int mMonth11_RegPay = 0;
        private int mMonth12_RegPay = 0;

        private string mCurrStartYMD = string.Empty;
        private string mCurrExpireYmd = string.Empty;
        private string mCurrStartHMS = string.Empty;
        private string mCurrExpireHMS = string.Empty;

        /// <summary>
        /// 현재 정기권시작일
        /// </summary>
        public string CurrStartYMD
        {
            set { mCurrStartYMD = value; }
            get { return mCurrStartYMD; }
        }

        /// <summary>
        /// 현재 정기권 만료일
        /// </summary>
        public string CurrExpireYmd
        {
            set { mCurrExpireYmd = value; }
            get { return mCurrExpireYmd; }
        }

        /// <summary>
        /// 현재 정기권시작시간
        /// </summary>
        public string CurrStartHms
        {
            set { mCurrStartHMS = value; }
            get { return mCurrStartHMS; }
        }

        /// <summary>
        /// 현재 정기권 만료시간
        /// </summary>
        public string CurrExpireHms
        {
            set { mCurrExpireHMS = value; }
            get { return mCurrExpireHMS; }
        }

        /// <summary>
        /// 정기권 연장시작일
        /// </summary>
        public string NextStartYmd
        {
            set { mNextStartYmd = value; }
            get { return mNextStartYmd; }
        }

        /// <summary>
        /// 정기권 연장종료일
        /// </summary>
        public string NextExpiredYmd
        {
            set { mNextExpiredYmd = value; }
            get { return mNextExpiredYmd; }
        }

        public long CurrMonthId
        {
            set { mCurrMonthId = value; }
            get { return mCurrMonthId; }
        }

        public int CurrMonth
        {
            set { mCurrMonth = value; }
            get { return mCurrMonth; }
        }

        /// <summary>
        /// 1개월일때 정기권금액
        /// </summary>
        public int Month1_RegPay
        {
            set { mMonth1_RegPay = value; }
            get { return mMonth1_RegPay; }
        }

        /// <summary>
        /// 2개월일때 정기권금액
        /// </summary>
        public int Month2_RegPay
        {
            set { mMonth2_RegPay = value; }
            get { return mMonth2_RegPay; }
        }

        /// <summary>
        /// 3개월일때 정기권금액
        /// </summary>
        public int Month3_RegPay
        {
            set { mMonth3_RegPay = value; }
            get { return mMonth3_RegPay; }
        }

        /// <summary>
        /// 4개월일때 정기권금액
        /// </summary>
        public int Month4_RegPay
        {
            set { mMonth4_RegPay = value; }
            get { return mMonth4_RegPay; }
        }

        /// <summary>
        /// 5개월일때 정기권금액
        /// </summary>
        public int Month5_RegPay
        {
            set { mMonth5_RegPay = value; }
            get { return mMonth5_RegPay; }
        }

        /// <summary>
        /// 6개월일때 정기권금액
        /// </summary>
        public int Month6_RegPay
        {
            set { mMonth6_RegPay = value; }
            get { return mMonth6_RegPay; }
        }

        public int Month7_RegPay
        {
            set { mMonth7_RegPay = value; }
            get { return mMonth7_RegPay; }
        }

        public int Month8_RegPay
        {
            set { mMonth8_RegPay = value; }
            get { return mMonth8_RegPay; }
        }

        public int Month9_RegPay
        {
            set { mMonth9_RegPay = value; }
            get { return mMonth9_RegPay; }
        }

        public int Month10_RegPay
        {
            set { mMonth10_RegPay = value; }
            get { return mMonth10_RegPay; }
        }

        public int Month11_RegPay
        {
            set { mMonth11_RegPay = value; }
            get { return mMonth11_RegPay; }
        }

        public int Month12_RegPay
        {
            set { mMonth12_RegPay = value; }
            get { return mMonth12_RegPay; }
        }

        public void SetRegCurrMonthSetting(int pMonth)
        {
            SeasonCarAmounts setSeasonCarAmount = mCurrentSeasonCarAmount.Find(x => x.month == pMonth);
            this.TotDc = 0;
            this.RecvAmt = 0;
            this.TotFee = Convert.ToInt32(setSeasonCarAmount.amount);
            this.RealFee = Convert.ToInt32(setSeasonCarAmount.amount);
            mCurrMonth = pMonth;
            mCurrMonthId = setSeasonCarAmount.id;

            //정기권연장시 다음달 말일까지 안되는현상수정
            NextStartYmd = DateTime.ParseExact(CurrExpireYmd, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddDays(1).ToString("yyyyMMdd");
            string currentFinlDate = DateTime.ParseExact(CurrExpireYmd.Substring(0, 6) + "01", "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddMonths(+1).AddDays(-1).ToString("yyyyMMdd"); // 말일인지확인
            if (CurrExpireYmd == currentFinlDate) // 오늘이 월말이면
            {
                //1Month
                NextStartYmd = DateTime.ParseExact(CurrExpireYmd, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddDays(1).ToString("yyyyMMdd");
                DateTime Nextcurrent1DayDateTime = DateTime.ParseExact(NextStartYmd, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture); // 다음달1일이 정기권시작일
                NextExpiredYmd = new DateTime(Nextcurrent1DayDateTime.Year, Nextcurrent1DayDateTime.Month, 1).AddMonths(pMonth).AddDays(-1).ToString("yyyyMMdd"); ;
            }
            else
            {
                NextStartYmd = DateTime.ParseExact(CurrExpireYmd, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddDays(1).ToString("yyyyMMdd"); // 정기권시작일

                NextExpiredYmd = DateTime.ParseExact(CurrExpireYmd, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddMonths(pMonth).ToString("yyyyMMdd");
                currentFinlDate = DateTime.ParseExact(NextExpiredYmd.Substring(0, 6) + "01", "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddMonths(+pMonth).AddDays(-1).ToString("yyyyMMdd");
                if (currentFinlDate == NextExpiredYmd)
                {
                    NextExpiredYmd = DateTime.ParseExact(CurrExpireYmd, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddMonths(pMonth).AddDays(-1).ToString("yyyyMMdd");
                }
            }

            TextCore.INFO(TextCore.INFOS.CARSEARCH, "NormalCarInfo | SetRegExtensionCarPay", "[정기권연장시도가능] "
                                                                                + " 차량번호:" + OutCarNo1
                                                                                + " 연장요금:" + PaymentMoney.ToString()
                                                                                + " 연장기간:" + NPSYS.ConvetYears_Dash(NextStartYmd) + NPSYS.ConvetDay_Dash(CurrStartHms) + " ~ " + NPSYS.ConvetYears_Dash(NextExpiredYmd) + NPSYS.ConvetDay_Dash(CurrExpireHms)
                                                                                + " 출차요청시간:" + NPSYS.ConvetYears_Dash(this.OutYmd) + " " + NPSYS.ConvetDay_Dash(this.OutHms)
                                                                                + " 연장개월수:" + pMonth.ToString());
        }

        #endregion 그룹별정기권 연장및금액처리

        private PaymentType mPaymentMethod = PaymentType.Cash;
        private ManualType mManualType = ManualType.None;

        private string mCarKind = string.Empty;
        private string mNumType = string.Empty;

        private string mPreProcYmd = string.Empty;
        private string mPreProcHms = string.Empty;

        private string mInUnitNo = "0";

        private int mOutRecog1 = 1;

        public List<DcDetail> ListDcDetail = new List<DcDetail>();

        private PayType mCurrentPayType = PayType.MoneyOrFree;

        private string mCurrentOutName = string.Empty;

        private string mLastErrorMessage = string.Empty;

        private long mPreParktime = 0;
        private string mPreOutYmd = "";
        private string mPreOutHms = "";
        private string mPreInYmd = "";
        private string mPreInHms = "";
        private int mGuestDiscountTime = 0;

        private string mDiscountDetail = string.Empty;
        private string mReserve7 = string.Empty;

        /// <summary>
        /// 사전 할인
        /// </summary>
        private string mPreDiscountContent = string.Empty;

        private int mTMoneyPay = 0;

        //////////  현금관련  ////////////////////////
        /// <summary>
        /// 현금 승인번호
        /// </summary>
        private string mCashReciptNo = string.Empty;

        /// <summary>
        /// 현금 승인일자
        /// </summary>
        private string mCashReciptAuthDate = string.Empty;

        /// <summary>
        /// 현금 승인응답코드
        /// </summary>
        private string mCashReciptRescode = string.Empty;

        /// <summary>
        /// 현금 응답메시지
        /// </summary>
        private string mCashReciptResMsg = string.Empty;

        private string mCashReciptApproveYmd = string.Empty;
        private string mCashReciptApproveHms = string.Empty;
        private string mCashReciptApprovalYmd = string.Empty;
        private string mCashReciptApprovalHms = string.Empty;
        private int mCashReciptRequestYesNo = 0;
        /////////////////////////////////////////////////////

        private long mElapsedMinute = 0;
        private long mElapsedDay = 0;

        private string mCurrentTotalDiscount = string.Empty;

        //시간대정기권추가
        private string mCurrentDiscountImagePath = string.Empty;

        //시간대정기권추가 주석완료
        private bool mIsBarOpen = false;

        private string mOutLprChannelNum = "1";
        private string mOutLprUnitNo = string.Empty;
        private int mOutLprUnitPort = 2000;
        private string mOutLprUnitIp = string.Empty;

        //개방모드수정
        private bool mIsGabangCar = false;

        //개방모드수정완료

        // 정기권관련기능(만료요금부과/연장관련)

        //그룹별정기권 연장및금액처리 주석

        // private int mCurrMonthRegPay = 0;
        //그룹별정기권 연장및금액처리 완료
        //시간대정기권추가

        private int mPreDiscountMoney = 0;
        private int mPreParkMoney = 0;
        // 정기권관련기능(만료요금부과/연장관련)

        //할인권 제한 1일 어떤할인권들이 총 할인시간처리

        private int mSumPreTotalDisocuntTime = 0;
        //할인권 제한 1일 어떤할인권들이 총 할인시간처리 주석

        //나이스연동
        //요금할인권처리
        private string mFeeName = string.Empty;

        //LPR2대전방처리

        private string mOutCarPath2 = string.Empty;
        private bool mIsDualDetect = false;

        //LPR2대전방처리 주석처리
        //Kakao연동
        private bool mKakaoMember = false;

        //Kakao연동완료

        //GS POS할인
        private int mPosAmount = 0; // GS RETAIL 포스금액

        private int mPosUse = 0; // GS RETAIL 포스사용유무?
                                 //GS POS할인 완료

        //바코드 누적할인추가
        /// <summary>
        /// 바코드 누적을 위한 총 바코드 금액 기억
        /// </summary>
        private int mBarcodeMoney = 0;

        public List<string> mBarcodeListData = new List<string>();
        //바코드 누적할인추가완료

        private Car currentCarInfo = new Car();

        #endregion 변수

        public NormalCarInfo()
        {
        }

        public enum CarFreeType
        {
            /// <summary>
            /// 회차가아닌차량
            /// </summary>
            NONE,

            /// <summary>
            /// 회차차량
            /// </summary>
            FreeCar,

            /// <summary>
            /// 사전정산이후 인터벌이전에 나감
            /// </summary>
            PreFreeCar,

            /// <summary>
            /// 웹할인등으로 할인후 무료차량
            /// </summary>
            DiscountFreeCar
        }

        ///// <summary>
        ///// 결제종류
        ///// </summary>
        //public PaymentType PaymentMethod
        //{
        //    get { return mPaymentMethod; }
        //    set { mPaymentMethod = value; }
        //}

        public enum ManualType
        {
            None,

            InTime,
            Paymoney,
            Time,
        }

        /// <summary>
        /// 수동정산 타입인지 아닌지 입차주차권 파킹티켓인지등
        /// </summary>
        public ManualType ManualTypes
        {
            set { mManualType = value; }
            get { return mManualType; }
        }

        private int mSelectType = 0;

        /// <summary>
        /// 0이면 차량4자리로 1이면 시간으로 온거임
        /// </summary>
        public int SelectType
        {
            set { mSelectType = value; }
            get { return mSelectType; }
        }

        /// <summary>
        /// 입차 일자
        /// </summary>
        public string InDate { get { return InYmd + InHms; } }

        public void CanclePreCreditBooth()
        {
        }

        public bool IsDualDetect
        {
            set { mIsDualDetect = value; }
            get { return mIsDualDetect; }
        }

        //Kakao연동
        public bool KakaoMember
        {
            set { mKakaoMember = value; }
            get { return mKakaoMember; }
        }

        //Kakao연동완료

        //GS POS할인
        public int PosUse
        {
            set { mPosUse = value; }
            get { return mPosUse; }
        }

        public int PosAmount
        {
            set { mPosAmount = value; }
            get { return mPosAmount; }
        }

        //GS POS할인완료

        //바코드 누적할인추가
        public int BarcodeMoney
        {
            set { mBarcodeMoney = value; }
            get { return mBarcodeMoney; }
        }

        //바코드 누적할인추가완료

        public string CarKind
        {
            set { mCarKind = value; }
            get { return mCarKind; }
        }

        public string CurrentTotalDiscount
        {
            set { mCurrentTotalDiscount = value; }
            get { return mCurrentTotalDiscount; }
        }

        //시간대정기권추가
        public string CurrentDiscountImagePath
        {
            set { mCurrentDiscountImagePath = value; }
            get { return mCurrentDiscountImagePath; }
        }

        //시간대정기권추가완료

        public bool IsBarOpen
        {
            set { mIsBarOpen = value; }
            get { return mIsBarOpen; }
        }

        public string OutLprChannelNum
        {
            set { mOutLprChannelNum = value; }
            get { return mOutLprChannelNum; }
        }

        public string OutLprUnitNo
        {
            set { mOutLprUnitNo = value; }
            get { return mOutLprUnitNo; }
        }

        public int OutLprUnitPort
        {
            set { mOutLprUnitPort = value; }
            get { return mOutLprUnitPort; }
        }

        public string OutLprUnitIp
        {
            set { mOutLprUnitIp = value; }
            get { return mOutLprUnitIp; }
        }

        public string PreProcYmd
        {
            set { mPreProcYmd = value; }
            get { return mPreProcYmd; }
        }

        public string PreProcHms
        {
            set { mPreProcHms = value; }
            get { return mPreProcHms; }
        }

        /// <summary>
        /// 입차장비
        /// </summary>
        public string InUnitNo
        {
            set { mInUnitNo = value; }
            get { return mInUnitNo; }
        }

        /// <summary>
        /// 인식여부 1이면 정상인식 2이면 부분인식 3이면 미인식
        /// </summary>
        public int OutRecog1
        {
            set { mOutRecog1 = value; }
            get { return mOutRecog1; }
        }

        //개방모드수정
        public bool IsGabangCar
        {
            set { mIsGabangCar = value; }
            get { return mIsGabangCar; }
        }

        //개방모드수정완료

        /// <summary>
        /// 요금정산타입
        /// </summary>
        public enum PayType
        {
            /// <summary>
            /// 무료 아님 현금
            /// </summary>
            MoneyOrFree = 1,

            /// <summary>
            /// 신용카드
            /// </summary>
            Credit = 2,
        }

        /// <summary>
        /// 출차시 출차타입...민원출차,...할인출차등등
        /// </summary>
        public PayType CurrentPayType
        {
            set { mCurrentPayType = value; }
            get { return mCurrentPayType; }
        }

        //할인권 제한 1일 어떤할인권들이 총 할인시간처리
        public int SumPreTotalDisocuntTime
        {
            set { mSumPreTotalDisocuntTime = value; }
            get { return mSumPreTotalDisocuntTime; }
        }

        public string LastErrorMessage
        {
            set { mLastErrorMessage = value; }
            get { return mLastErrorMessage; }
        }

        public string NumType
        {
            set { mNumType = value; }
            get { return mNumType; }
        }

        // 정기권관련기능(만료요금부과/연장관련)주석종료

        /// <summary>
        /// 이전 입차  년월일
        /// </summary>
        public string PreInYmd
        {
            set { mPreInYmd = value; }
            get { return mPreInYmd; }
        }

        /// <summary>
        /// 이전 입차 시간
        /// </summary>
        public string PreInHms
        {
            set { mPreInHms = value; }
            get { return mPreInHms; }
        }

        /// <summary>
        /// 이전 출차  년월일
        /// </summary>
        public string PreOutYmd
        {
            set { mPreOutYmd = value; }
            get { return mPreOutYmd; }
        }

        /// <summary>
        /// 이전 출차 시간
        /// </summary>
        public string PreOutHms
        {
            set { mPreOutHms = value; }
            get { return mPreOutHms; }
        }

        /// <summary>
        /// 바로 이전 주차시간
        /// </summary>
        public long PreParktime
        {
            set { mPreParktime = value; }
            get { return mPreParktime; }
        }

        /// <summary>
        /// 방문자 할인시간
        /// </summary>
        public int GuestDiscountTime
        {
            set { mGuestDiscountTime = value; }
            get { return mGuestDiscountTime; }
        }

        //나이스연동완료

        /// <summary>
        /// 자동으로 할인금액에 따라 자동으로 받을금액이 생성된다.
        /// </summary>
        public int ReceiveMoney
        {
            get
            {
                int receveMoney = TotFee - TotDc - (RecvAmt - Change); //시제설정누락처리
                if (receveMoney < 0)
                {
                    receveMoney = 0;
                }

                return receveMoney;
            }
        }

        /// <summary>
        /// 할인권이 입수됨에 따라 "할인권:수량" 포맷을 만들어 주는 함수 ex) "22:2시간:1,23:3시간:3"   "코드:이름:수량" 으로 들어감
        /// </summary>
        /// <param name="p_CodeName"></param>
        public string DiscountDetail
        {
            get { return mDiscountDetail; }
            set { mDiscountDetail = value; }
        }

        /// <summary>
        /// 사전 할인 IONDATA의 RESERVE6을 사용
        /// </summary>
        public string PreDiscountContent
        {
            get { return mPreDiscountContent; }
            set { mPreDiscountContent = value; }
        }

        public string Reserve7
        {
            set { mReserve7 = value; }
            get { return mReserve7; }
        }

        /// <summary>
        /// 교통카드 금액
        /// </summary>
        public int TMoneyPay
        {
            set { mTMoneyPay = value; }
            get { return mTMoneyPay; }
        }

        /// <summary>
        /// 현금 승인번호
        /// </summary>
        public string CashReciptNo
        {
            set { mCashReciptNo = value; }
            get { return mCashReciptNo; }
        }

        /// <summary>
        /// 현금 승인일자
        /// </summary>
        public string CashReciptAuthDate
        {
            set { mCashReciptAuthDate = value; }
            get { return mCashReciptAuthDate; }
        }

        /// <summary>
        /// 현금 승인응답코드
        /// </summary>
        public string CashReciptRescode
        {
            set { mCashReciptRescode = value; }
            get { return mCashReciptRescode; }
        }

        /// <summary>
        /// 현금 응답메시지
        /// </summary>
        public string CashReciptResMsg
        {
            set { mCashReciptResMsg = value; }
            get { return mCashReciptResMsg; }
        }

        public string CashReciptApproveYmd
        {
            set { mCashReciptApproveYmd = value; }
            get { return mCashReciptApproveYmd; }
        }

        public string CashReciptApproveHms
        {
            set { mCashReciptApproveHms = value; }
            get { return mCashReciptApproveHms; }
        }

        public string CashReciptApprovalYmd
        {
            set { mCashReciptApprovalYmd = value; }
            get { return mCashReciptApprovalYmd; }
        }

        public string CashReciptApprovalHms
        {
            set { mCashReciptApprovalHms = value; }
            get { return mCashReciptApprovalHms; }
        }

        /// <summary>
        /// 청구여부.  0:안함,  1:청구함
        /// </summary>
        public int CashReciptRequestYesNo
        {
            set { mCashReciptRequestYesNo = value; }
            get { return mCashReciptRequestYesNo; }
        }

        /// <summary>
        /// 할인금액을 넣으면 총합을 구해서 DiscountMoney에 자동으로 집어넣고 자동으로 PaymentMoney 도 계산된다
        /// </summary>
        /// <param name="discountmoney"></param>
        public void TotalDiscountMoneyCalcureate(int discount_money)
        {
            TotDc += discount_money;
        }

        private long mTotalDiscountTIme = 0;

        /// <summary>
        /// 할인된 시간을 누적한다
        /// </summary>
        /// <param name="discountTime"></param>
        public void TotalDiscountTimeCalcureate(long discountTime)
        {
            mTotalDiscountTIme += discountTime;
        }

        /// <summary>
        /// 총할인시간
        /// </summary>
        public long TotalDiscountTime
        {
            set { mTotalDiscountTIme = value; }
            get { return mTotalDiscountTIme; }
        }

        /// <summary>
        /// 경과 분(화면 표시용)
        /// </summary>
        public long ElapsedMinute
        {
            set { ElapsedMinute = value; }
            get { return mElapsedMinute; }
        }

        private long mElapsedHour = 0;

        /// <summary>
        /// 경과 시간(화면 표시용)
        /// </summary>
        public long ElapsedHour
        {
            set { mElapsedHour = value; }
            get { return mElapsedHour; }
        }

        //Tmap연동
        /// <summary>
        /// 결제종류
        /// </summary>
        public PaymentType PaymentMethod
        {
            get { return mPaymentMethod; }
            set { mPaymentMethod = value; }
        }

        //Tmap연동 완료

        /// <summary>
        /// 경과 일(화면 표시용)
        /// </summary>
        public long ElapsedDay
        {
            set { mElapsedDay = value; }
            get { return mElapsedDay; }
        }

        public string DateMinute(DateTime pDatetime)
        {
            string pData = string.Empty;
            switch (NPSYS.CurrentLanguageType)
            {
                case ConfigID.LanguageType.ENGLISH:
                    pData = pDatetime.ToString("yyyy-MM-dd HH:mm").Replace("-", "/").ToString();

                    break;

                case ConfigID.LanguageType.JAPAN:
                    pData = pDatetime.ToString("yyyy") + "年" + pDatetime.ToString("MM") + "月" + pDatetime.ToString("dd") + "日" + pDatetime.ToString("HH:mm");

                    break;

                case ConfigID.LanguageType.KOREAN:

                    break;
            }
            return pData;
        }

        public void ElpaseMinute()
        {
            if (ParkingMin == 0)
            {
                ParkingMin = 1;
            }
            CalculateElapsedTime(Convert.ToInt64(ParkingMin.ToString("######")));
        }

        //public void ElpaseMinute(string pInYmd, string pInHms, string pOutYmd, string pOutHms)
        //{
        //    DateTime oldDate = DateTime.ParseExact(pInYmd + pInHms.Substring(0, 12), "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
        //    DateTime newDate = DateTime.ParseExact(pOutYmd + pOutHms.Substring(0, 12), "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
        //    TimeSpan ts = newDate - oldDate;
        //    int dirrednceInMinute = Convert.ToInt32(ts.TotalMinutes);
        //    if (dirrednceInMinute <= 0)
        //    {
        //        dirrednceInMinute = 1;
        //    }
        //    ParkingMin = Convert.ToInt64(dirrednceInMinute);

        //    CalculateElapsedTime(Convert.ToInt64(ParkingMin.ToString("######")));
        //}
        /// <summary>
        /// 분 총 경과시간을 일 시간 분으로 분리 시킴
        /// </summary>
        private void CalculateElapsedTime(long TotalMinute)
        {
            if (TotalMinute <= 1)
            {
                TotalMinute = 1;
                mElapsedMinute = 1;
                mElapsedHour = 0;
                mElapsedDay = 0;
                return;
            }
            try
            {
                long temp = TotalMinute;

                mElapsedMinute = temp % 60;

                temp -= mElapsedMinute;

                temp /= 60;

                mElapsedHour = temp % 24;

                temp -= mElapsedHour;

                temp /= 24;

                mElapsedDay = temp;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NormalCarInfo|CalculateElapsedTime", "분을 일시분으로 변경중 예외사항:" + ex.ToString());
            }
        }

        /// <summary>
        /// 입력받은 영수증 번호
        /// </summary>
        public string ScanReceiptNo { get; set; }

        /// <summary>
        /// 입력받은 영수증에 대한 기타 정보들
        /// </summary>
        public string ScanReceiptNoExtra { get; set; }

        /// <summary>
        /// 정산기에서 출력한 영수증 번호
        /// </summary>
        public string PrintReceiptNo { get; set; }

        /// <summary>
        /// 정산기에서 출력한 보관증 번호
        /// </summary>
        public string PrintCashTicketNo { get; set; }

        public int PreDiscountMoney
        {
            set { mPreDiscountMoney = value; }
            get { return mPreDiscountMoney; }
        }

        public int PreParkMoney
        {
            set { mPreParkMoney = value; }
            get { return mPreParkMoney; }
        }

        /// <summary>
        /// 주차시간에서 할인시간을 뺀 나머지
        /// </summary>
        public long GetRemainderParktime()
        {
            return (ParkingMin - mTotalDiscountTIme);
        }

        /// <summary>
        /// 요금에서 할인 금액등을 제외한 실제 결제할 금액
        /// </summary>

        private string getImagePath(string pinCarImagePath)
        {
            if (pinCarImagePath.Trim() == "")
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "CarInfo|getImagePath ", "차량이미지값이 없음");
                return "";
            }
            try
            {
                pinCarImagePath = pinCarImagePath.ToUpper().Replace(":9080", "").Replace("HTTP:", "").Replace((char)0x2F, (char)0x5c);
                return pinCarImagePath;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CarInfo|getImagePath", ex.ToString());
                return "";
            }
        }
    }
}