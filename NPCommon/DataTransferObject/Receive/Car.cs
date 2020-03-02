using System;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class Car
    {
        private int mSelectType = 0;

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
            id,

            /// <summary>
            /// 시리얼번호
            /// </summary>
            tkNo,

            /// <summary>
            /// 입차종류 int 1:방문,2:정기,3:회차
            /// </summary>
            parkingType,

            /// <summary>
            /// 차량종류  int 0:경차,1:일반,2:대형
            /// </summary>
            carType,

            /// <summary>
            /// 입차시간   long
            /// </summary>
            inDate,

            /// <summary>
            /// 입차이미지
            /// </summary>
            inImage1,

            /// <summary>
            /// 입차번호1
            /// </summary>
            inCarNo1,

            /// <summary>
            /// 입차이미지2
            /// </summary>
            inImage2,

            /// <summary>
            /// 입차번호2
            /// </summary>
            inCarNo2,

            /// <summary>
            /// 출차시간   long
            /// </summary>
            outdate,

            /// <summary>
            /// 주차상태    int
            /// </summary>
            outChk,

            /// <summary>
            /// 출차이미지1  string
            /// </summary>
            outImage1,

            /// <summary>
            /// 출차번호1    string
            /// </summary>
            outCarNo1,

            /// <summary>
            /// 출차이미지2   string
            /// </summary>
            outImage2,

            /// <summary>
            /// 출차번호2   string
            /// </summary>
            outCarNo2,

            /// <summary>
            /// 입차인식1   int
            /// </summary>
            inRecog1,

            /// <summary>
            /// 출차인식1   int
            /// </summary>
            outRecog1,

            /// <summary>
            /// 입차인식2   int
            /// </summary>
            inRecog2,

            /// <summary>
            /// 출차인식2   int
            /// </summary>
            outRecog2,

            /// <summary>
            /// 면번호
            /// </summary>
            spaceNo,

            /// <summary>
            /// 입차기기
            /// </summary>
            inUnit,

            /// <summary>
            /// 출차기기
            /// </summary>
            outUnit,

            /// <summary>
            /// 정기차량정보
            /// </summary>
            seasonTicket,

            payment,

            /// <summary>
            /// 강제로 만듬 0 차량선택으로 만듬 / 1이면 시간조회로
            /// </summary>
            selectType
        }

        /// <summary>
        /// 차량종류   0:경차,1:일반,2:대형
        /// </summary>
        public int carType
        {
            set; get;
        }

        public long id
        {
            set; get;
        }

        public string inCarNo1
        {
            set; get;
        }

        public string inCarNo2
        {
            set; get;
        }

        public long inDate
        {
            set;
            get;
        }

        public string inImage1
        {
            set; get;
        }

        public string inImage2
        {
            set; get;
        }

        /// <summary>
        /// 입차기기
        /// </summary>
        public InUnit inUnit
        {
            set; get;
        }

        public string outCarNo1
        {
            set; get;
        }

        public string outCarNo2
        {
            set; get;
        }

        public int outChk
        {
            set; get;
        }

        public long outDate
        {
            set; get;
        }

        public string outImage1
        {
            set; get;
        }

        public string outImage2
        {
            set; get;
        }

        /// <summary>
        /// 출차기기
        /// </summary>
        public OutUnit outUnit
        {
            set; get;
        }

        /// <summary>
        /// 입차종류  1:방문,2:정기,3:회차(실제 회차)
        /// </summary>
        public int parkingType
        {
            set; get;
        }

        public Payment payment
        {
            set; get;
        }

        /// <summary>
        /// 정기차량정보
        /// </summary>
        public SeasonCar seasonCar
        {
            set; get;
        }

        /// <summary>
        /// 0이면 차량선택 1이면 시간조회로
        /// </summary>
        public int selectType
        {
            set { mSelectType = value; }
            get { return mSelectType; }
        }

        public string spaceNo
        {
            set; get;
        }

        public Status status
        {
            set; get;
        }

        public string tkNo
        {
            set; get;
        }
    }
}