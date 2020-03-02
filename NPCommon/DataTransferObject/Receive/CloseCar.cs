using System;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class CloseCar
    {
        public enum ID
        {
            /// <summary>
            /// 일반차량대수 int
            /// </summary>
            generalCarCnt,

            /// <summary>
            /// 일반차량 합계 double
            /// </summary>
            generalCarAmt,

            /// <summary>
            /// 정기차량대수 int
            /// </summary>
            seasonCarCnt,

            /// <summary>
            /// 정기차량합계 double
            /// </summary>
            seasonCarAmt,

            /// <summary>
            /// 무료차량대수 int
            /// </summary>
            freeCarCnt,

            /// <summary>
            /// 무료차량합계 double
            /// </summary>
            freeCarAmt,

            /// <summary>
            /// 전체대수 int
            /// </summary>
            totalCnt,

            /// <summary>
            /// 전체합계 double
            /// </summary>
            totalAmt,
        }

        /// <summary>
        /// 일반차량대수 int
        /// </summary>
        public int generalCarCnt
        {
            set; get;
        }

        /// <summary>
        /// 일반차량 합계 double
        /// </summary>
        public long generalCarAmt
        {
            set; get;
        }

        /// <summary>
        /// 정기차량대수 int
        /// </summary>
        public int seasonCarCnt
        {
            set; get;
        }

        /// <summary>
        /// 정기차량합계 double
        /// </summary>
        public long seasonCarAmt
        {
            set; get;
        }

        /// <summary>
        /// 무료차량대수 int
        /// </summary>
        public int freeCarCnt
        {
            set; get;
        }

        /// <summary>
        /// 무료차량합계 double
        /// </summary>
        public long freeCarAmt
        {
            set; get;
        }

        /// <summary>
        /// 전체대수 int
        /// </summary>
        public int totalCnt
        {
            set; get;
        }

        public long totalAmt
        {
            set; get;
        }
    }
}