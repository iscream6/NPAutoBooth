using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class SeasonCar
    {
        public enum ID
        {
            /// <summary>
            /// id long
            /// </summary>
            id,
            /// <summary>
            /// 정가차량종류 int
            /// </summary>
            tkType,
            /// <summary>
            /// 시리얼번호 string
            /// </summary>
            tkNo,
            /// <summary>
            /// 고객 이름
            /// </summary>
            name,
            /// <summary>
            /// 연락처 string
            /// </summary>
            telNo,
            /// <summary>
            /// 차량번호 string
            /// </summary>
            carNo,
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
            /// 발급일 long
            /// </summary>
            issueDate,
            /// <summary>
            /// 발급금액 int
            /// </summary>
            issueAmt,
            /// <summary>
            /// 상태 int
            /// </summary>
            status,
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
            currAmt,
            /// <summary>
            /// int
            /// </summary>
            apb,
            /// <summary>
            /// 차량종류 int
            /// </summary>
            carType,
            /// <summary>
            /// int
            /// </summary>
            markNo,
            /// <summary>
            /// int
            /// </summary>
            hiType,
            /// <summary>
            /// long
            /// </summary>
            startDate,
            /// <summary>
            /// long
            /// </summary>
            endDate,
            /// <summary>
            /// 결제종류 int
            /// </summary>
            payType,
            /// <summary>
            /// object
            /// </summary>
            seasonTicketGroup
        }

        /// <summary>
        /// id long
        /// </summary>
        public long id
        {
            set; get;
        }
        /// <summary>
        /// 정가차량종류 int
        /// </summary>
        public int tkType
        {
            set; get;
        }

        /// <summary>
        /// 시리얼번호 string
        /// </summary>
        public string tkNo
        {
            set; get;
        }
        /// <summary>
        /// 고객 이름 string
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
        /// 차량번호 string
        /// </summary>
        public string carNo
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
        /// 발급일 long
        /// </summary>
        public long issueDate
        {
            set; get;
        }
        /// <summary>
        /// 발급금액 int
        /// </summary>
        public int issueAmt
        {
            set; get;
        }
        /// <summary>
        /// 상태 int
        /// </summary>
        public string status
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
        public long currAmt
        {
            set; get;
        }
        /// <summary>
        /// int
        /// </summary>
        public int apb
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

        /// <summary>
        /// int
        /// </summary>
        public int markNo
        {
            set; get;
        }

        /// <summary>
        /// int
        /// </summary>
        public int hiType
        {
            set; get;
        }

        /// <summary>
        /// time
        /// </summary>
        public long startDate
        {
            set; get;
        }
        /// <summary>
        /// time
        /// </summary>
        public long endDate
        {
            set; get;
        }
        /// <summary>
        /// 결제종류 int
        /// </summary>
        public int payType
        {
            set; get;
        }
        /// <summary>
        /// object
        /// </summary>
        public SeasonTicketGroup seasonTicketGroup
        {
            set; get;
        }
        public SeasonCarGroup seasonCarGroup
        {
            set; get;
        }
    }
}
