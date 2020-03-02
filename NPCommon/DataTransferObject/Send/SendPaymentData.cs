using System;

namespace NPCommon.DTO.Send
{
    [Serializable]
    public class SendPaymentData
    {
        public enum ID
        {
            unit,

            /// <summary>
            /// 상태값커맨드
            /// </summary>
            payment,

            /// <summary>
            /// 날짜
            /// </summary>
            payDate,

            /// <summary>
            /// 지폐종류관련커맨드
            /// </summary>
            payType,

            /// <summary>
            /// 주차장정보커맨드
            /// </summary>
            payAmt,

            /// <summary>
            /// 국가커맨드
            /// </summary>
            card,

            /// <summary>
            /// 차량정보커맨드
            /// </summary>
            cash
        }

        private Unit mUnit = new Unit();

        public Unit unit
        {
            set { mUnit = value; }
            get { return mUnit; }
        }

        private Payment mpayment = new Payment();

        public Payment payment
        {
            set { mpayment = value; }
            get { return mpayment; }
        }

        public long payDate
        {
            set; get;
        }

        /// <summary>
        /// 결제방식 0.할인또는 무료1.현금/무료 2.신용 3:교통
        /// </summary>
        public int payType
        {
            set; get;
        }

        /// <summary>
        /// 실제 순수 무인정산기가 결제한 금액
        /// 예를들어 주차요금 2000원 투입금액 3000원 방출금액 1000원이면
        /// payamt 2000 , recvamt = 3000 retamt=1000
        /// 실제 결제한 금액 예를들어 주차요금 2000원 투입금액 3000원 오류로 방출금액 0원이면
        /// payamt 3000 , recvamt = 3000 retamt=0 , notretamt=1000
        /// 신용카드로 5000원 결제한 경우  payamt 5000 , recvamt = 5000 retamt=0 , notretamt=0
        /// </summary>
        public long payAmt
        {
            set; get;
        }

        /// <summary>
        /// 받은금액  카드나 현금은 투입금액
        /// </summary>
        public long recvAmt
        {
            set; get;
        }

        /// <summary>
        /// 방출금액
        /// </summary>
        public long retAmt
        {
            set; get;
        }

        /// <summary>
        /// 못받은금액
        /// </summary>
        public long notRetAmt
        {
            set; get;
        }

        /// <summary>
        /// 카드나 현금일때는 결제시  A 카드 취소시는 C 할인은 없음
        /// </summary>
        public string status
        {
            set; get;
        }

        private Card mcard = new Card();

        /// <summary>
        /// 카드클래스
        /// </summary>
        public Card card
        {
            set { mcard = value; }
            get { return mcard; }
        }

        private Cash mCash = new Cash();

        /// <summary>
        /// 현금관련클래스
        /// </summary>
        public Cash cash
        {
            set { mCash = value; }
            get { return mCash; }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="pCarInfo"></param>
        /// <param name="pDateTime"></param>
        public SendPaymentData(NormalCarInfo pCarInfo, DateTime pDateTime)
        {
            mUnit.fullCode = NPSYS.ParkCode + "-" + NPSYS.BoothID;
            pCarInfo.InComeMoney = pCarInfo.GetInComeMoney;
            pCarInfo.OutComeMoney = pCarInfo.GetOutComeMoney;
            payDate = NPSYS.DateTimeToLongType(pDateTime);
            recvAmt = 0;
            notRetAmt = 0;
            retAmt = 0;
            payAmt = 0; //

            if (pCarInfo.VanAmt == 0)  // 신용카드도 아니고
            {
                card = null;
                if (pCarInfo.isBillUse() == false)  // 현금도 아니면 강제입금도아니면
                {
                    payType = 0; //결제방식 : 할인 또는 무료
                }
            }
            else // 카드면
            {
                payType = 2;
                status = "A";
                payAmt = pCarInfo.VanAmt;
                recvAmt = pCarInfo.VanAmt;
                card.vanCheck = pCarInfo.VanCheck;
                card.cardNo = pCarInfo.VanCardNumber;
                card.vanAmt = pCarInfo.VanAmt;
                card.vanDate = NPSYS.DateTimeToLongType(
                    DateTime.ParseExact((pCarInfo.VanCardApproveYmd + pCarInfo.VanCardApproveHms).Replace("-", string.Empty)
                                                                                                 .Replace(":", string.Empty)
                                       , "yyyyMMddHHmmss"
                                       , System.Globalization.CultureInfo.CurrentCulture));
                payDate = card.vanDate;
                card.vanRegNo = pCarInfo.VanRegNo;
                card.cardType = pCarInfo.VanCardAcquirerName;
                card.vanStatus = pCarInfo.VanResMsg;
                card.issueName = pCarInfo.VanIssueName;
                card.cardFullNo = pCarInfo.VanCardFullNumber;
            }
            //TMAP연동
            if (pCarInfo.PaymentMethod == PaymentType.Fail_Card)
            {
                card = new Card();
                payType = 2;
                status = "A"; //F
                payAmt = pCarInfo.VanAmt;  //0
                recvAmt = pCarInfo.VanAmt; //0
                card.vanCheck = pCarInfo.VanCheck;   //2
                card.cardNo = pCarInfo.VanCardNumber; //
                card.vanAmt = pCarInfo.VanAmt;
                DateTime vanApprovalTIme = DateTime.ParseExact((pCarInfo.VanCardApproveYmd + pCarInfo.VanCardApproveHms).Replace("-", string.Empty).Replace(":", string.Empty), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                card.vanDate = NPSYS.DateTimeToLongType(vanApprovalTIme);
                payDate = card.vanDate;
                card.vanRegNo = pCarInfo.VanRegNo;
                card.cardType = pCarInfo.VanCardAcquirerName;
                card.vanStatus = pCarInfo.VanResMsg;
                card.vanCode = pCarInfo.VanRescode;
                card.issueName = pCarInfo.VanIssueName;
                card.cardFullNo = pCarInfo.VanCardFullNumber;
            }
            //TMAP연동완료
            if (pCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season ||
                 pCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
            {
                mpayment.car = null;
                mpayment.seasonCar.carNo = pCarInfo.OutCarNo1;

                DateTime currStartdatetime = DateTime.ParseExact(pCarInfo.CurrStartYMD + pCarInfo.CurrStartHms, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                DateTime currNextdatetime = DateTime.ParseExact(pCarInfo.NextExpiredYmd + pCarInfo.CurrExpireHms, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                mpayment.seasonCar.startDate = NPSYS.DateTimeToLongType(currStartdatetime);
                mpayment.seasonCar.endDate = NPSYS.DateTimeToLongType(currNextdatetime);
                mpayment.seasonCar.amount = pCarInfo.RealFee;
                mpayment.seasonCar.month = pCarInfo.CurrMonth;
                mpayment.seasonCar.updateDate = payDate;
            }
            else
            {
                mpayment.seasonCar = null;
                mpayment.car.tkNo = pCarInfo.TkNO;
            }

            if (pCarInfo.isBillUse() == false)
            {
                mCash = null;
            }
            else
            {
                payType = 1;
                status = "A";
                cash.sendTime = NPSYS.DateTimeToLongType(pCarInfo.CurrentMoneyOutTime);
                cash.inOutType = (int)pCarInfo.CurrentMoneyInOutType;
                SetCash(pCarInfo);
                recvAmt = pCarInfo.GetInComeMoney;
                notRetAmt = pCarInfo.GetNotDisChargeMoney;
                retAmt = pCarInfo.GetOutComeMoney;
                payAmt = recvAmt - retAmt - notRetAmt;
            }
        }

        /// <summary>
        /// 현재 설정에 따라 결제관련 전송문서 데이터 넣음
        /// </summary>
        /// <param name="pCarInfo"></param>
        /// <param name="pId"></param>
        private void SetCash(NormalCarInfo pCarInfo)
        {
            int cash50 = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty));
            int cash100 = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));
            int cash500 = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));
            int cash1000 = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty));
            int cash5000 = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty));

            mCash.currCoin1 = cash50;
            mCash.inCoin1 = pCarInfo.InCome50Qty;
            mCash.outCoin1 = pCarInfo.OutCome50Qty;
            mCash.notCoin1 = pCarInfo.NotdisChargeMoney50Qty;

            mCash.currCoin2 = cash100;
            mCash.inCoin2 = pCarInfo.InCome100Qty;
            mCash.outCoin2 = pCarInfo.OutCome100Qty;
            mCash.notCoin2 = pCarInfo.NotdisChargeMoney100Qty;

            mCash.currCoin3 = cash500;
            mCash.inCoin3 = pCarInfo.InCome500Qty;
            mCash.outCoin3 = pCarInfo.OutCome500Qty;
            mCash.notCoin3 = pCarInfo.NotdisChargeMoney500Qty;

            mCash.currCoin4 = 0;
            mCash.inCoin4 = 0;
            mCash.outCoin4 = 0;
            mCash.notCoin4 = 0;

            mCash.currBill1 = cash1000;
            mCash.inBill1 = pCarInfo.InCome1000Qty;
            mCash.outBill1 = pCarInfo.OutCome1000Qty;
            mCash.notBill1 = pCarInfo.NotdisChargeMoney1000Qty;

            mCash.currBill2 = cash5000;
            mCash.inBill2 = pCarInfo.InCome5000Qty;
            mCash.outBill2 = pCarInfo.OutCome5000Qty;
            mCash.notBill2 = pCarInfo.NotdisChargeMoney5000Qty;

            mCash.currBill3 = 0;
            mCash.inBill3 = pCarInfo.InCome10000Qty;
            mCash.outBill3 = pCarInfo.OutCome10000Qty;
            mCash.notBill3 = pCarInfo.NotdisChargeMoney10000Qty;

            mCash.currBill4 = 0;
            mCash.inBill4 = pCarInfo.InCome50000Qty;
            mCash.outBill4 = pCarInfo.OutCome50000Qty;
            mCash.notBill4 = pCarInfo.NotdisChargeMoney50000Qty;
        }
    }
}