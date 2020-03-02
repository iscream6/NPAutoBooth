using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class Payment
    {
        public enum HeaderCode
        {
            /// <summary>
            /// 성공
            /// </summary>
            OK = 200,
            /// <summary>
            /// 차량생성성공
            /// </summary>
            CREATED = 201,
            NOCONTENT = 204,
            /// <summary>
            /// 데이터 없음
            /// </summary>
            BADREQUEST = 400,
            Unauthorized = 401,
            Forbbiden = 403,
            NotFound = 404,
            Conflict = 409,
            /// <summary>
            /// 차량데이터 없음
            /// </summary>

            Error = 500

        }

        public enum ID
        {
            /// <summary>
            /// 아이디 long
            /// </summary>
            id,
            /// <summary>
            /// 주차시간 int
            /// </summary>
            parkingMin,
            /// <summary> 
            /// 총주차금액 double
            /// </summary>
            totFee,
            /// <summary>
            /// 총할인금액 double
            /// </summary>
            totDc,
            /// <summary>
            /// 실제결제금액(현재 주차해서 받을금액) double
            /// </summary>
            realFee,
            /// <summary>
            /// 기존에 받은금액 double
            /// </summary>
            recvAmt,
            /// <summary>
            /// 반환금액 double
            /// </summary>
            change,
            /// <summary>
            /// 할인갯수 int
            /// </summary>
            dcCnt,
            /// <summary>
            /// 미지불금액 double
            /// </summary>
            unpaid,
            type,
            feeGroup,
            ///차량정보 Object
            car,
            /// <summary>
            /// 할인정보 List of array
            /// </summary>
            discount,
            /// <summary>
            /// 결제정보
            /// </summary>
            paymentDetail,
            lastPayDate
        }


        public long id
        {
            set; get;
        }

        /// <summary>
        /// 주차시간
        /// </summary>
        public int parkingMin
        {
            set; get;
        }

        /// <summary>
        /// 총주차금액
        /// </summary>
        public long totFee
        {
            set; get;
        }



        /// <summary>
        /// 총할인금액
        /// </summary>
        public long totDc
        {
            set; get;
        }

        /// <summary>
        /// 실제결제금액
        /// </summary>
        public long realFee
        {
            set; get;
        }

        /// <summary>
        /// 받은금액
        /// </summary>
        public long recvAmt
        {
            set; get;
        }

        /// <summary>
        /// 반환금액
        /// </summary>
        public long change
        {
            set; get;
        }

        /// <summary>
        /// 할인개수
        /// </summary>
        public int dcCnt
        {
            set; get;
        }



        /// <summary>
        /// 미지불금액
        /// </summary>
        public long unpaid
        {
            set; get;
        }

        public string type { set; get; }
        /// <summary>
        /// 최종결제시간 없으면 0임
        /// </summary>
        public long lastPayDate
        {
            set; get;
        }
        public FeeGroup feeGroup
        {
            set; get;
        }


        /// <summary>
        /// 차량정보 Object
        /// </summary>
        public Car car
        {
            set; get;
        }
        public List<Discount> discount
        {
            set; get;
        }

        public List<PaymentDetail> paymentDetail
        {
            set; get;

        }
        public Status status
        {
            set; get;
        }
    }
}
