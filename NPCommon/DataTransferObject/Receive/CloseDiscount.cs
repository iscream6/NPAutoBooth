using System;
using System.Collections.Generic;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class CloseDiscount
    {
        public enum ID
        {
            /// <summary>
            ///할인코드
            /// </summary>
            discountTypeCode,

            /// <summary>
            /// 할인명
            /// </summary>
            discountTypeName,

            /// <summary>
            /// 할인개수
            /// </summary>
            discountTypeCnt,

            /// <summary>
            /// 할인합계
            /// </summary>
            discountTypeAmt,

            /// <summary>
            /// 할인항목
            /// </summary>
            closeDiscountItem
        }

        /// <summary>
        /// 할인코드
        /// </summary>
        public int discountTypeCode
        {
            set; get;
        }

        /// <summary>
        /// 할인명
        /// </summary>
        public string discountTypeName
        {
            set; get;
        }

        /// <summary>
        /// 할인개수
        /// </summary>
        public int discountTypeCnt
        {
            set; get;
        }

        /// <summary>
        /// 할인합계
        /// </summary>
        public long discountTypeAmt
        {
            set; get;
        }

        public List<CloseDiscountItem> closeDiscountItem
        {
            set; get;
        }
    }
}