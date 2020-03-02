using System;

namespace NPCommon.DTO.Send
{
    [Serializable]
    public class Cash
    {
        public long sendTime
        {
            set; get;
        }

        /// <summary>
        /// 방출타입
        /// 0:정산완료방출, 1:정산완료미방출, 2: 취소완료방출, 3:취소완료미방출,  4:강제입금(출구무인에서 1000원 입수후 다음차량올시등)
        /// </summary>
        public int inOutType
        {
            set; get;
        }

        /// <summary>
        /// 입수동전개수 int
        /// </summary>
        public int inCoin1
        {
            set; get;
        }

        /// <summary>
        /// 입수동전개수 int
        /// </summary>
        public int inCoin2
        {
            set; get;
        }

        /// <summary>
        /// 입수동전개수 int
        /// </summary>
        public int inCoin3
        {
            set; get;
        }

        /// <summary>
        /// 입수동전개수 int
        /// </summary>
        public int inCoin4
        {
            set; get;
        }

        /// <summary>
        /// 입수동전개수 int
        /// </summary>
        public int inCoin5
        {
            set; get;
        }

        /// <summary>
        /// 입수동전개수 int
        /// </summary>
        public int inCoin6
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐개수 int
        /// </summary>
        public int inBill1
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐개수 int
        /// </summary>
        public int inBill2
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐개수 int
        /// </summary>
        public int inBill3
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐개수 int
        /// </summary>
        public int inBill4
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐개수 int
        /// </summary>
        public int inBill5
        {
            set; get;
        }

        /// <summary>
        /// 입수지폐개수 int
        /// </summary>
        public int inBill6
        {
            set; get;
        }

        /// <summary>
        /// 방출동전개수
        /// </summary>
        public int outCoin1
        {
            set; get;
        }

        /// <summary>
        /// 방출동전개수
        /// </summary>
        public int outCoin2
        {
            set; get;
        }

        /// <summary>
        /// 방출동전개수
        /// </summary>
        public int outCoin3
        {
            set; get;
        }

        /// <summary>
        /// 방출동전개수
        /// </summary>
        public int outCoin4
        {
            set; get;
        }

        /// <summary>
        /// 방출동전개수
        /// </summary>
        public int outCoin5
        {
            set; get;
        }

        /// <summary>
        /// 방출동전개수
        /// </summary>
        public int outCoin6
        {
            set; get;
        }

        /// <summary>
        /// 방출지폐개수
        /// </summary>
        public int outBill1
        {
            set; get;
        }

        /// <summary>
        /// 방출지폐개수
        /// </summary>
        public int outBill2
        {
            set; get;
        }

        /// <summary>
        /// 방출지폐개수
        /// </summary>
        public int outBill3
        {
            set; get;
        }

        /// <summary>
        /// 방출지폐개수
        /// </summary>
        public int outBill4
        {
            set; get;
        }

        /// <summary>
        /// 방출지폐개수
        /// </summary>
        public int outBill5
        {
            set; get;
        }

        /// <summary>
        /// 방출지폐개수
        /// </summary>
        public int outBill6
        {
            set; get;
        }

        /// <summary>
        /// 현재동전개수
        /// </summary>
        public int currCoin1
        {
            set; get;
        }

        /// <summary>
        /// 현재동전개수
        /// </summary>
        public int currCoin2
        {
            set; get;
        }

        /// <summary>
        /// 현재동전개수
        /// </summary>
        public int currCoin3
        {
            set; get;
        }

        /// <summary>
        /// 현재동전개수
        /// </summary>
        public int currCoin4
        {
            set; get;
        }

        /// <summary>
        /// 현재동전개수
        /// </summary>
        public int currCoin5
        {
            set; get;
        }

        /// <summary>
        /// 현재동전개수
        /// </summary>
        public int currCoin6
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐개수 1type int
        /// </summary>
        public int currBill1
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐개수 2type int
        /// </summary>
        public int currBill2
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐개수 3type int
        /// </summary>
        public int currBill3
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐개수 4type int
        /// </summary>
        public int currBill4
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐개수 5type int
        /// </summary>
        public int currBill5
        {
            set; get;
        }

        /// <summary>
        /// 현재지폐개수 6type int
        /// </summary>
        public int currBill6
        {
            set; get;
        }

        /// <summary>
        /// 미방출동전개수
        /// </summary>
        public int notCoin1
        {
            set; get;
        }

        /// <summary>
        /// 미방출동전개수
        /// </summary>
        public int notCoin2
        {
            set; get;
        }

        /// <summary>
        /// 미방출동전개수
        /// </summary>
        public int notCoin3
        {
            set; get;
        }

        /// <summary>
        /// 미방출동전개수
        /// </summary>
        public int notCoin4
        {
            set; get;
        }

        /// <summary>
        /// 미방출동전개수
        /// </summary>
        public int notCoin5
        {
            set; get;
        }

        /// <summary>
        /// 미방출동전개수
        /// </summary>
        public int notCoin6
        {
            set; get;
        }

        /// <summary>
        /// 미방출지폐개수 1type int
        /// </summary>
        public int notBill1
        {
            set; get;
        }

        /// <summary>
        /// 미방출지폐개수 2type int
        /// </summary>
        public int notBill2
        {
            set; get;
        }

        /// <summary>
        /// 미방출지폐개수 3type int
        /// </summary>
        public int notBill3
        {
            set; get;
        }

        /// <summary>
        /// 미방출지폐개수 4type int
        /// </summary>
        public int notBill4
        {
            set; get;
        }

        /// <summary>
        /// 미방출지폐개수 5type int
        /// </summary>
        public int notBill5
        {
            set; get;
        }

        /// <summary>
        /// 미방출지폐개수 6type int
        /// </summary>
        public int notBill6
        {
            set; get;
        }
    }
}