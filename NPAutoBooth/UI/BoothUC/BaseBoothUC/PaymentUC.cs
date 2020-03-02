using NPCommon;
using NPCommon.DTO;
using System;
using System.Drawing;
using System.Windows.Forms;
using static NPAutoBooth.UI.BoothUC.BoothCommonLib;

namespace NPAutoBooth.UI.BoothUC
{
    /// <summary>
    /// 화면 자체가 너무 복잡하고....
    /// 제어되는 컨트롤이 너무 많은 관계로
    /// 출력되는 모든 컨트롤을 Property로 제공하기로 한다. ㅠㅠ
    /// </summary>
    public class PaymentUC : UserControl, IConvertible
    {
        public enum ButtonEnableType
        {
            /// <summary>
            /// 요금결제폼 시작인경우
            /// </summary>
            PayFormStart,
            /// <summary>
            /// 요금결제폼 종료인경우
            /// </summary>
            PayFormEnd,
            /// <summary>
            /// 현금이 방출중일때
            /// </summary>
            CashCancle,
            /// <summary>
            /// 현금 방출이 종료됬을때
            /// </summary>
            CashCancleStop,
            /// <summary>
            /// 삼성페이 결제시
            /// </summary>
            SamsumPayStart,
            /// <summary>
            /// 상섬페이 결제처리 종료시
            /// </summary>
            SamsunPayEnd,
            /// <summary>
            /// 정기권연장인경우
            /// </summary>
            AddMonthStart,
            AddMonthEnd,
            InsertCoin,
        }


        public enum ENUM_Control
        {
            lbl_MSG_DISCOUNTINFO,
            lbl_MSG_PAYINFO,
            lbl_TXT_CARNO,
            lbl_CarNumber,
            lbl_TXT_INDATE,
            lblIndate,
            lbl_TXT_ELAPSEDTIME,
            lblElapsedTime,
            lbl_TXT_PARKINGFEE,
            lblParkingFee,
            lbl_UNIT_CASH1,
            lbl_TXT_DISCOUNTFEE,
            lbl_DIscountMoney,
            lbl_UNIT_CASH2,
            lbl_TXT_PREFEE,
            lbl_RecvMoney,
            lbl_UNIT_CASH3,
            lbl_TXT_AMOUNTFEE,
            lbl_Payment,
            lbl_UNIT_CASH4,
            lblCarnumber2,
            lbl_CarType,
            lblRegExpireInfo,
            lblDiscountCountName,
            lblDiscountInputCount,
            lblErrorMessage,
            pic_carimage,
            btn_TXT_CANCEL,
            btnCardApproval,
            btnSamsunPay,
            btnOneMonthAdd,
            btnTwoMonthAdd,
            btnThreeMonthAdd,
            btnFourMonthAdd,
            btnFiveMonthAdd,
            btnSixMonthAdd,
            btn_TXT_BACK,
            btn_TXT_HOME,
            btnEnglish,
            btnJapan,
        }

        public virtual T GetControl<T>(ENUM_Control control)
        {
            return default(T);
        }

        public virtual event Event_ConfigCall ConfigCall;
        public virtual event Event_LanguageChange LanguageChange_Click;
        public virtual event EventHandler PreForm_Click;
        public virtual event EventHandler Home_Click;
        public virtual event Event_SeasonCarAddMonth SeasonCarAddMonth;
        public virtual event EventHandler CashCancel_Click;
        public virtual event EventHandler SamsungPay_Click;

        public virtual void Initialize() { }

        public virtual void ForeignLanguageVisible(bool visible) { }

        public virtual void SetCarInfo(NormalCarInfo pNormalCarInfo) { }

        public virtual void Clear() { }

        public virtual void ButtonEnable(ButtonEnableType pEnableType) { }

        public virtual void SetLanguage(ConfigID.LanguageType languageType, NormalCarInfo normalCarInfo = null) { }

        public virtual void SetPageChangeButtonVisible(bool visibled) { }

        public virtual Image CarImage { get; set; }

        /// <summary>
        /// 결제요금
        /// </summary>
        public virtual string Payment { get; set; }

        /// <summary>
        /// 할인요금
        /// </summary>
        public virtual string DiscountMoney { get; set; }

        /// <summary>
        /// 화면 표시 메시지
        /// </summary>
        public virtual string ErrorMessage { get; set; }

        /// <summary>
        /// 주차요금
        /// </summary>
        public virtual string ParkingFee { get; set; }

        /// <summary>
        /// 사전정산금액
        /// </summary>
        public virtual string RecvMoney { get; set; }

        /// <summary>
        /// 경과시간
        /// </summary>
        public virtual string ElapsedTime { get; set; }

        /// <summary>
        /// 입수된할인권수량
        /// </summary>
        public virtual string DiscountInputCount { get; set; }

        /// <summary>
        /// 현금취소버튼 Visible
        /// </summary>
        public virtual bool CancelButtonVisible { get; set; }


        #region Implements IConvertible

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return true;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return this.AccessibleName;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return this;
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        #endregion

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }
    }
}
