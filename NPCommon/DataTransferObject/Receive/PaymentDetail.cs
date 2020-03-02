using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class PaymentDetail
    {
        public enum ID
        {
            /// <summary>
            /// 아이디 long
            /// </summary>
            id,
            /// <summary>
            /// 결제일자 long
            /// </summary>
            payDate,
            /// <summary> 
            /// 결제타입 int
            /// </summary>
            payType,
            /// <summary>
            /// 결제금액 double
            /// </summary>
            payAmt,
            /// <summary>
            /// 받은금액 double
            /// </summary>
            recvAmt,

            /// <summary>
            /// 받은금액 double
            /// </summary>
            retAmt,

            /// <summary>
            /// 받은금액 double
            /// </summary>
            notretAmt,

        }


        /// <summary>
        /// 아이디 long
        /// </summary>
        private long mid = 0;
        public long id
        {
            set { mid = value; }
            get { return mid; }

        }
        /// <summary>
        /// 결제일자 long
        /// </summary>
        private long mpayDate = 0;
        public long payDate
        {
            set { mpayDate = value; }
            get { return mpayDate; }
        }
        public int payType
        {
            set; get;

        }


        /// <summary>
        /// 결제금액
        /// </summary>
        public long payAmt
        {
            set; get;
        }


        /// <summary>
        /// 기존 투입금액
        /// </summary>
        public long recvAmt
        {
            set; get;
        }


        /// <summary>
        /// 기존 거스름돈
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
        public Send.Card card
        {
            set; get;
        }
    }
}
