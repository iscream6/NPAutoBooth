using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Send
{
    [Serializable]
    public class Card
    {
        public enum ID
        {
            /// <summary>
            /// 
            /// </summary>
            vanCheck,
            /// <summary>
            /// 카드금액
            /// </summary>
            cardNo,
            /// <summary>
            /// 승인금액
            /// </summary>
            vanAmt,
            /// <summary>
            /// 승인일자
            /// </summary>
            vanDate,
            /// <summary>
            /// 승인번호
            /// </summary>
            vanRegNo,
            /// <summary>
            /// 매입사
            /// </summary>
            cardType,
            /// <summary>
            /// 승인상태
            /// </summary>
            vanStatus,
            /// <summary>
            /// 발급사
            /// </summary>
            issueName,
            /// <summary>
            /// 카드풀번호
            /// </summary>
            cardFullNo
        }
        /// <summary>
        /// 카드풀번호
        /// </summary>
        public string cardFullNo
        {
            set; get;
        }

        /// <summary>
        /// 카드번호
        /// </summary>
        public string cardNo
        {
            set; get;
        }

        /// <summary>
        /// 매입사
        /// </summary>
        public string cardType
        {
            set; get;
        }

        /// <summary>
        /// 발급사
        /// </summary>
        public string issueName
        {
            set; get;
        }

        /// <summary>
        /// 승인금액
        /// </summary>
        public long vanAmt
        {
            set; get;
        }

        /// <summary>
        /// 승인여부 1.정상승인 2.승인불가 3.승인취소
        /// </summary>
        public int vanCheck
        {
            set; get;
        }
        //Tmap연동
        public string vanCode
        {
            set; get;
        }
        /// <summary>
        /// 승인일자
        /// </summary>
        public long vanDate
        {
            set; get;
        }
        /// <summary>
        /// 승인번호
        /// </summary>
        public string vanRegNo
        {
            set; get;
        }
        //Tmap연동 완료

        /// <summary>
        /// 승인상태
        /// </summary>
        public string vanStatus
        {
            set;
            get;
        }
    }
}
