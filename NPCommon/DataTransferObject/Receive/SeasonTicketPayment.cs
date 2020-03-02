using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class SeasonTicketPayment
    {
        public enum ID
        {
            /// <summary>
            /// id long
            /// </summary>
            id,
            /// <summary>
            /// 주차장번호 long
            /// </summary>
            parkNo,
            /// <summary>
            /// 기기번호 long
            /// </summary>
            unitNo,
            /// <summary>
            /// 처리일자 long
            /// </summary>
            issueDate,
            /// <summary>
            /// 시리얼번호 string
            /// </summary>
            tkNo,
            /// <summary>
            /// 차량번호 string
            /// </summary>
            carNo,
            /// <summary>
            /// 그룹번호 string
            /// </summary>
            groupNo,
            /// <summary>
            /// 그룹명 stirng
            /// </summary>
            groupName,
            /// <summary>
            /// 이름 string
            /// </summary>
            name,
            /// <summary>
            /// 연락처 string
            /// </summary>
            telNo,
            /// <summary>
            /// 회사명 string
            /// </summary>
            compName,
            /// <summary>
            /// 부서명 string
            /// </summary>
            deptName,
            /// <summary>
            /// 주소 string
            /// </summary>
            address,
            /// <summary>
            /// 근무자 long
            /// </summary>
            mNo,
            /// <summary>
            /// 티켓종류 int
            /// </summary>
            tkType,
            /// <summary>
            /// 차량종류 int
            /// </summary>
            carType,
            /// <summary>
            /// 발급종류  int
            /// </summary>
            issueType,
            /// <summary>
            /// 발급기기
            /// </summary>
            issueUnit,
            /// <summary> 
            /// 시작일 long
            /// </summary>
            expDateF,
            /// <summary>
            /// 종료일 long
            /// </summary>
            expDateT,
            /// <summary>
            /// int
            /// </summary>
            fValue,
            /// <summary>
            /// int
            /// </summary>
            chkClosing,
            /// <summary>
            /// string
            /// </summary>
            cardIssueName,
            /// <summary>
            /// 카드번호 string
            /// </summary>
            cardNo,
            /// <summary>
            /// string
            /// </summary>
            vanDate,
            /// <summary>
            /// string
            /// </summary>
            purchaseName,
            /// <summary>
            /// string
            /// </summary>
            cardRegNo,
            /// <summary>
            /// int
            /// </summary>
            currMonth,
            /// <summary>
            /// int
            /// </summary>
            payType,
            /// <summary>
            /// 정기권 그룹 object
            /// </summary>
            seasonTicketGroup,
        }
        private long mid = 0;
        /// <summary>
        /// id
        /// </summary>
        public long id
        {
            set { mid = value; }
            get { return mid; }
        }
        /// <summary>
        /// 주차장번호
        /// </summary>
        public long parkNo
        {
            set; get;
        }
        /// <summary>
        /// 기기번호
        /// </summary>
        public long unitNo
        {
            set; get;
        }
        /// <summary>
        /// 처리일자
        /// </summary>
        public long issueDate
        {
            set; get;
        }
        /// <summary>
        /// 시리얼번호
        /// </summary>
        public string tkNo
        {
            set; get;
        }
        /// <summary>
        /// 차량번호
        /// </summary>
        public string carNo
        {
            set; get;
        }
        /// <summary>
        /// 그룹번호
        /// </summary>
        public string groupNo
        {
            set; get;
        }

        /// <summary>
        /// 그룹명
        /// </summary>
        public string groupName
        {
            set; get;
        }
        /// <summary>
        /// 이름
        /// </summary>
        public string name
        {
            set; get;
        }
        /// <summary>
        /// 연락처 string
        /// </summary>
        public string telNo
        {
            set; get;
        }
        /// <summary>
        /// 회사명 string
        /// </summary>
        public string compName
        {
            set; get;
        }
        /// <summary>
        /// 부서명 string
        /// </summary>
        public string deptName
        {
            set; get;
        }
        /// <summary>
        /// 주소 string
        /// </summary>
        public string address
        {
            set; get;
        }
        /// <summary>
        /// 근무자 long
        /// </summary>
        public long mNo
        {
            set; get;
        }

        /// <summary>
        /// 티켓종류 int
        /// </summary>
        public int tkType
        {
            set; get;
        }
        /// <summary>
        /// 차량종류 int
        /// </summary>
        public int carType
        {
            set; get;
        }

        public int issueType
        {
            set; get;
        }


        /// <summary>
        /// 발급기기 long
        /// </summary>
        public long issueUnit
        {
            set; get;
        }

        /// <summary> 
        /// 시작일 long
        /// </summary>
        public long expDateF
        {
            set; get;
        }
        /// <summary>
        /// 종료일 long
        /// </summary>
        public long expDateT
        {
            set; get;
        }
        /// <summary>
        /// int
        /// </summary>
        public int fValue
        {
            set; get;
        }
        /// <summary>
        /// int
        /// </summary>
        public int chkClosing
        {
            set; get;
        }
        /// <summary>
        /// string
        /// </summary>
        public string cardIssueName
        {
            set; get;
        }

        /// <summary>
        /// 카드번호 string
        /// </summary>
        public string cardNo
        {
            set; get;
        }
        /// <summary>
        /// string
        /// </summary>
        public string vanDate
        {
            set; get;
        }
        /// <summary>
        /// string
        /// </summary>
        public string purchaseName
        {
            set; get;
        }
        /// <summary>
        /// string
        /// </summary>
        public string cardRegNo
        {
            set; get;
        }
        /// <summary>
        /// int
        /// </summary>
        public int currMonth
        {
            set; get;
        }
        /// <summary>
        /// int
        /// </summary>
        public int payType
        {
            set; get;
        }
        /// <summary>
        /// 정기권 그룹 object
        /// </summary>
        public SeasonTicketGroup seasonTicketGroup
        {
            set; get;
        }
    }
}
