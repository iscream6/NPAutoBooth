using System;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class ClosePayment
    {
        public enum ID
        {
            /// <summary>
            /// 현금건수 int
            /// </summary>
            cashCnt,

            /// <summary>
            /// 현금합계 double
            /// </summary>
            cashAmt,

            /// <summary>
            /// 카드건수 int
            /// </summary>
            cardCnt,

            /// <summary>
            /// 카드합계 double
            /// </summary>
            cardAmt,

            /// <summary>
            /// 무료차량대수 int
            /// </summary>
            transCnt,

            /// <summary>
            /// 무료차량합계 double
            /// </summary>
            transAmt,

            /// <summary>
            /// 할인건수 int
            /// </summary>
            discountCnt,

            /// <summary>
            /// 할인합계 double
            /// </summary>
            discountAmt,

            /// <summary>
            /// 전체건수 int
            /// </summary>
            totalCnt,

            /// <summary>
            /// 전체합계 double
            /// </summary>
            totalAmt,
        }

        /// <summary>
        /// 현금건수 int
        /// </summary>
        public int cashCnt
        {
            set; get;
        }

        /// <summary>
        /// 현금합계 int
        /// </summary>
        public long cashAmt
        {
            set; get;
        }

        /// <summary>
        /// 카드건수 int
        /// </summary>
        public int cardCnt
        {
            set; get;
        }

        /// <summary>
        /// 카드합계 long
        /// </summary>
        public long cardAmt
        {
            set; get;
        }

        /// <summary>
        /// 무료차량대수 int
        /// </summary>
        public int transCnt
        {
            set; get;
        }

        /// <summary>
        /// 무료차량합계 long
        /// </summary>
        public long transAmt
        {
            set; get;
        }

        /// <summary>
        /// 할인건수 int
        /// </summary>
        public int discountCnt
        {
            set; get;
        }

        /// <summary>
        /// 할인건수합계 long
        /// </summary>
        public long discountAmt
        {
            set; get;
        }

        /// <summary>
        /// 전체건수 int
        /// </summary>
        public int totalCnt
        {
            set; get;
        }

        /// <summary>
        /// 전체합계 long
        /// </summary>
        public long totalAmt
        {
            set; get;
        }
    }
}