using System;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class CloseDiscountItem
    {
        public enum ID
        {
            /// <summary>
            ///할인명
            /// </summary>
            discountName,

            /// <summary>
            /// 할인개수
            /// </summary>
            discountCnt,

            /// <summary>
            /// 할인합계
            /// </summary>
            discountAmt
        }

        /// <summary>
        /// 할인명
        /// </summary>
        public string discountName
        {
            set; get;
        }

        /// <summary>
        /// 할인개수
        /// </summary>
        public int discountCnt
        {
            set; get;
        }

        /// <summary>
        /// 할인합계
        /// </summary>
        public long discountAmt
        {
            set; get;
        }
    }
}