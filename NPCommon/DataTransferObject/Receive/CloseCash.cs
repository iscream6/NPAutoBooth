using System;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class CloseCash
    {
        public enum ID
        {
            /// <summary>
            /// 입수동전개수1type int
            /// </summary>
            inCoin1Cnt,

            /// <summary>
            /// 입수동전개수2type int
            /// </summary>
            inCoin2Cnt,

            /// <summary>
            /// 입수동전개수3type int
            /// </summary>
            inCoin3Cnt,

            /// <summary>
            /// 입수동전개수4type int
            /// </summary>
            inCoin4Cnt,

            /// <summary>
            /// 입수동전개수5type int
            /// </summary>
            inCoin5Cnt,

            /// <summary>
            /// 입수동전개수6type int
            /// </summary>
            inCoin6Cnt,

            /// <summary>
            /// 입수지폐개수1type int
            /// </summary>
            inBill1Cnt,

            /// <summary>
            /// 입수지폐개수2type int
            /// </summary>
            inBill2Cnt,

            /// <summary>
            /// 입수지폐개수3type int
            /// </summary>
            inBill3Cnt,

            /// <summary>
            /// 입수지폐개수4type int
            /// </summary>
            inBill4Cnt,

            /// <summary>
            /// 입수지폐개수5type int
            /// </summary>
            inBill5Cnt,

            /// <summary>
            /// 입수지폐개수6type int
            /// </summary>
            inBill6Cnt,

            /// <summary>
            /// 입수동전합계1type long
            /// </summary>
            inCoin1Amt,

            /// <summary>
            /// 입수동전합계2type long
            /// </summary>
            inCoin2Amt,

            /// <summary>
            /// 입수동전합계3type long
            /// </summary>
            inCoin3Amt,

            /// <summary>
            /// 입수동전합계4type long
            /// </summary>
            inCoin4Amt,

            /// <summary>
            /// 입수동전합계5type long
            /// </summary>
            inCoin5Amt,

            /// <summary>
            /// 입수동전합계6type long
            /// </summary>
            inCoin6Amt,

            /// <summary>
            /// 입수지폐합계1type long
            /// </summary>
            inBill1Amt,

            /// <summary>
            /// 입수지폐합계2type long
            /// </summary>
            inBill2Amt,

            /// <summary>
            /// 입수지폐합계3type long
            /// </summary>
            inBill3Amt,

            /// <summary>
            /// 입수지폐합계4type long
            /// </summary>
            inBill4Amt,

            /// <summary>
            /// 입수지폐합계5type long
            /// </summary>
            inBill5Amt,

            /// <summary>
            /// 입수지폐합계6type long
            /// </summary>
            inBill6Amt,

            /// <summary>
            /// 입수현금개수 int
            /// </summary>
            inCashTotalCnt,

            /// <summary>
            /// 입수현금합계 long
            /// </summary>
            inCashTotalAmt,

            /// <summary>
            /// 방출동전개수1type int
            /// </summary>
            outCoin1Cnt,

            /// <summary>
            /// 방출동전개수1type int
            /// </summary>
            outCoin2Cnt,

            /// <summary>
            /// 방출동전개수1type int
            /// </summary>
            outCoin3Cnt,

            /// <summary>
            /// 방출동전개수1type int
            /// </summary>
            outCoin4Cnt,

            /// <summary>
            /// 방출동전개수1type int
            /// </summary>
            outCoin5Cnt,

            /// <summary>
            /// 방출동전개수1type int
            /// </summary>
            outCoin6Cnt,

            /// <summary>
            /// 방출지폐개수1type int
            /// </summary>
            outBill1Cnt,

            /// <summary>
            /// 방출지폐개수1type int
            /// </summary>
            outBill2Cnt,

            /// <summary>
            /// 방출지폐개수1type int
            /// </summary>
            outBill3Cnt,

            /// <summary>
            /// 방출지폐개수1type int
            /// </summary>
            outBill4Cnt,

            /// <summary>
            /// 방출지폐개수1type int
            /// </summary>
            outBill5Cnt,

            /// <summary>
            /// 방출지폐개수1type int
            /// </summary>
            outBill6Cnt,

            /// <summary>
            /// 방출동전합계 1type long
            /// </summary>
            outCoin1Amt,

            /// <summary>
            /// 방출동전합계 2type long
            /// </summary>
            outCoin2Amt,

            /// <summary>
            /// 방출동전합계 3type long
            /// </summary>
            outCoin3Amt,

            /// <summary>
            /// 방출동전합계 4type long
            /// </summary>
            outCoin4Amt,

            /// <summary>
            /// 방출동전합계 5type long
            /// </summary>
            outCoin5Amt,

            /// <summary>
            /// 방출동전합계 6type long
            /// </summary>
            outCoin6Amt,

            /// <summary>
            /// 방출현금개수 int
            /// </summary>
            outCashTotalCnt,

            /// <summary>
            /// 방출현금합계 long
            /// </summary>
            outCashTotalAmt,

            /// <summary>
            /// 현재동전개수 1type int
            /// </summary>
            currCoin1Cnt,

            /// <summary>
            /// 현재동전개수 2type int
            /// </summary>
            currCoin2Cnt,

            /// <summary>
            /// 현재동전개수 3type int
            /// </summary>
            currCoin3Cnt,

            /// <summary>
            /// 현재동전개수 4type int
            /// </summary>
            currCoin4Cnt,

            /// <summary>
            /// 현재동전개수 5type int
            /// </summary>
            currCoin5Cnt,

            /// <summary>
            /// 현재동전개수 6type int
            /// </summary>
            currCoin6Cnt,

            /// <summary>
            /// 현재지폐개수 1type int
            /// </summary>
            currBill11Cnt,

            /// <summary>
            /// 현재지폐개수 2type int
            /// </summary>
            currBill12Cnt,

            /// <summary>
            /// 현재지폐개수 3type int
            /// </summary>
            currBill13Cnt,

            /// <summary>
            /// 현재지폐개수 4type int
            /// </summary>
            currBill14Cnt,

            /// <summary>
            /// 현재지폐개수 5type int
            /// </summary>
            currBill15Cnt,

            /// <summary>
            /// 현재지폐개수 6type int
            /// </summary>
            currBill16Cnt,

            /// <summary>
            /// 현재동전합계 1type long
            /// </summary>
            currCoin1Amt,

            /// <summary>
            /// 현재동전합계 2type long
            /// </summary>
            currCoin2Amt,

            /// <summary>
            /// 현재동전합계 3type long
            /// </summary>
            currCoin3Amt,

            /// <summary>
            /// 현재동전합계 4type long
            /// </summary>
            currCoin4Amt,

            /// <summary>
            /// 현재동전합계 5type long
            /// </summary>
            currCoin5Amt,

            /// <summary>
            /// 현재동전합계 6type long
            /// </summary>
            currCoin6Amt,

            /// <summary>
            /// 현재지폐합계 1type long
            /// </summary>
            currBill11Amt,

            /// <summary>
            /// 현재지폐합계 2type long
            /// </summary>
            currBill12Amt,

            /// <summary>
            /// 현재지폐합계 3type long
            /// </summary>
            currBill13Amt,

            /// <summary>
            /// 현재지폐합계 4type long
            /// </summary>
            currBill14Amt,

            /// <summary>
            /// 현재지폐합계 5type long
            /// </summary>
            currBill15Amt,

            /// <summary>
            /// 현재지폐합계 6type long
            /// </summary>
            currBill16Amt,

            /// <summary>
            /// 현재현금개수 int
            /// </summary>
            currCashTotalCnt,

            /// <summary>
            /// 현재현금합계
            /// </summary>
            currCashTotalAmt,
        }

        /// <summary>
        /// 입수동전개수 int
        /// </summary>
        public int inCoin1Cnt
        {
            set; get;
        }

        /// <summary>
        /// 입수동전개수 int
        /// </summary>
        public int inCoin2Cnt
        {
            set; get;
        }

        /// <summary>
        /// 입수동전개수 int
        /// </summary>
        public int inCoin3Cnt
        {
            set; get;
        }

        /// <summary>
        /// 입수동전개수 int
        /// </summary>
        public int inCoin4Cnt
        {
            set; get;
        }

        /// <summary>
        /// 입수동전개수 int
        /// </summary>
        public int inCoin5Cnt
        {
            set; get;
        }

        /// <summary>
        /// 입수동전개수 int
        /// </summary>
        public int inCoin6Cnt
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐개수 int
        /// </summary>
        public int inBill1Cnt
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐개수 int
        /// </summary>
        public int inBill2Cnt
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐개수 int
        /// </summary>
        public int inBill3Cnt
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐개수 int
        /// </summary>
        public int inBill4Cnt
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐개수 int
        /// </summary>
        public int inBill5Cnt
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐개수 int
        /// </summary>
        public int inBill6Cnt
        {
            set; get;
        }

        /// <summary>
        /// 입수동전합계1type long
        /// </summary>
        public long inCoin1Amt
        {
            set; get;
        }

        /// <summary>
        /// 입수동전합계2type long
        /// </summary>
        public long inCoin2Amt
        {
            set; get;
        }

        /// <summary>
        /// 입수동전합계3type long
        /// </summary>
        public long inCoin3Amt
        {
            set; get;
        }

        /// <summary>
        /// 입수동전합계4type long
        /// </summary>
        public long inCoin4Amt
        {
            set; get;
        }

        /// <summary>
        /// 입수동전합계5type long
        /// </summary>
        public long inCoin5Amt
        {
            set; get;
        }

        /// <summary>
        /// 입수동전합계6type long
        /// </summary>
        public long inCoin6Amt
        {
            set; get;
        }

        public long inBill1Amt
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐합계2type long
        /// </summary>
        public long inBill2Amt
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐합계3type long
        /// </summary>
        public long inBill3Amt
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐합계4type long
        /// </summary>
        public long inBill4Amt
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐합계5type long
        /// </summary>
        public long inBill5Amt
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐합계6type long
        /// </summary>
        public long inBill6Amt
        {
            set; get;
        }

        /// <summary>
        /// 입수현금개수 int
        /// </summary>
        public int inCashTotalCnt
        {
            set; get;
        }

        /// <summary>
        /// 입수현금합계 long
        /// </summary>

        public long inCashTotalAmt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전개수
        /// </summary>
        public int outCoin1Cnt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전개수
        /// </summary>
        public int outCoin2Cnt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전개수
        /// </summary>
        public int outCoin3Cnt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전개수
        /// </summary>
        public int outCoin4Cnt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전개수
        /// </summary>
        public int outCoin5Cnt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전개수
        /// </summary>
        public int outCoin6Cnt
        {
            set; get;
        }

        /// <summary>
        /// 방출지폐개수
        /// </summary>
        public int outBill1Cnt
        {
            set; get;
        }

        /// <summary>
        /// 방출지폐개수
        /// </summary>
        public int outBill2Cnt
        {
            set; get;
        }

        /// <summary>
        /// 방출지폐개수
        /// </summary>
        public int outBill3Cnt
        {
            set; get;
        }

        /// <summary>
        /// 방출지폐개수
        /// </summary>
        public int outBill4Cnt
        {
            set; get;
        }

        /// <summary>
        /// 방출지폐개수
        /// </summary>
        public int outBill5Cnt
        {
            set; get;
        }

        /// <summary>
        /// 방출지폐개수
        /// </summary>
        public int outBill6Cnt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전합계
        /// </summary>
        public long outCoin1Amt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전합계
        /// </summary>
        public long outCoin2Amt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전합계
        /// </summary>
        public long outCoin3Amt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전합계
        /// </summary>
        public long outCoin4Amt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전합계
        /// </summary>
        public long outCoin5Amt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전합계
        /// </summary>
        public long outCoin6Amt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전합계
        /// </summary>
        public long outBill1Amt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전합계
        /// </summary>
        public long outBill2Amt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전합계
        /// </summary>
        public long outBill3Amt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전합계
        /// </summary>
        public long outBill4Amt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전합계
        /// </summary>
        public long outBill5Amt
        {
            set; get;
        }

        /// <summary>
        /// 방출동전합계
        /// </summary>
        public long outBill6Amt
        {
            set; get;
        }

        /// <summary>
        /// 방출현금개수
        /// </summary>
        public int outCashTotalCnt
        {
            set; get;
        }

        /// <summary>
        /// 방출현금 합계
        /// </summary>
        public long outCashTotalAmt
        {
            set; get;
        }

        /// <summary>
        /// 현재동전개수
        /// </summary>
        public int currCoin1Cnt
        {
            set; get;
        }

        /// <summary>
        /// 현재동전개수
        /// </summary>
        public int currCoin2Cnt
        {
            set; get;
        }

        /// <summary>
        /// 현재동전개수
        /// </summary>
        public int currCoin3Cnt
        {
            set; get;
        }

        /// <summary>
        /// 현재동전개수
        /// </summary>
        public int currCoin4Cnt
        {
            set; get;
        }

        /// <summary>
        /// 현재동전개수
        /// </summary>
        public int currCoin5Cnt
        {
            set; get;
        }

        /// <summary>
        /// 현재동전개수
        /// </summary>
        public int currCoin6Cnt
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐개수 1type int
        /// </summary>
        public int currBill1Cnt
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐개수 2type int
        /// </summary>
        public int currBill2Cnt
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐개수 3type int
        /// </summary>
        public int currBill3Cnt
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐개수 4type int
        /// </summary>
        public int currBill4Cnt
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐개수 5type int
        /// </summary>
        public int currBill5Cnt
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐개수 6type int
        /// </summary>
        public int currBill6Cnt
        {
            set; get;
        }

        /// <summary>
        /// 현재동전합계 1type long
        /// </summary>
        public long currCoin1Amt
        {
            set; get;
        }

        /// <summary>
        /// 현재동전합계 2type long
        /// </summary>
        public long currCoin2Amt
        {
            set; get;
        }

        /// <summary>
        /// 현재동전합계 3type long
        /// </summary>
        public long currCoin3Amt
        {
            set; get;
        }

        /// <summary>
        /// 현재동전합계 4type long
        /// </summary>
        public long currCoin4Amt
        {
            set; get;
        }

        /// <summary>
        /// 현재동전합계 5type long
        /// </summary>
        public long currCoin5Amt
        {
            set; get;
        }

        /// <summary>
        /// 현재동전합계 6type long
        /// </summary>
        public long currCoin6Amt
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐합계 1type long
        /// </summary>
        public long currBill1Amt
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐합계 2type long
        /// </summary>
        public long currBill2Amt
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐합계 3type long
        /// </summary>
        public long currBill3Amt
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐합계 4type long
        /// </summary>
        public long currBill4Amt
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐합계 5type long
        /// </summary>
        public long currBill5Amt
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐합계 6type long
        /// </summary>
        public long currBill6Amt
        {
            set; get;
        }

        public int currCashTotalCnt
        {
            set; get;
        }

        /// <summary>
        /// 현재현금합계
        /// </summary>
        public long currCashTotalAmt
        {
            set; get;
        }
    }
}