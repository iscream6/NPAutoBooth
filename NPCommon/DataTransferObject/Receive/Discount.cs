using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class Discount
    {
        public enum ID
        {
            /// <summary>
            /// id int
            /// </summary>
            id,
            /// <summary>
            /// 할인기기 object
            /// </summary>
            unit,
            /// <summary>
            /// 할인시간 long
            /// </summary>
            dcdate,
            /// <summary>
            /// 할인순서 int
            /// </summary>
            dcSeq,
            /// <summary>
            /// 시리얼번호 
            /// </summary>
            tkNo,
            /// <summary>
            /// 차량번호
            /// </summary>
            carNo,
            /// <summary>
            /// 할인번호
            /// </summary>
            dcNo,
            /// <summary>
            /// 할인값 double
            /// </summary>
            dcAmt,
            /// <summary>
            /// 실제할인값 실제할인값
            /// </summary>
            realDcAmt,
            /// <summary>
            /// 할인일  int
            /// </summary>
            realDcDay,
            /// <summary>
            /// 할인종류 int
            /// </summary>
            dcType,
            /// <summary>
            /// 할인사용자 object
            /// </summary>
            user,
            /// <summary>
            /// 할인항목 object
            /// </summary>
            discountItem

        }
        /// <summary>
        /// id int
        /// </summary>
        private int mid = 0;
        public int id
        {
            set { mid = value; }
            get { return mid; }
        }
        private Unit mUnit = null;
        /// <summary>
        /// 할인기기 object
        /// </summary>
        public Unit Unit
        {
            set { mUnit = value; }
            get { return mUnit; }
        }
        private long mdcdate = 0;
        /// <summary>
        /// 할인시간 long
        /// </summary>
        public long dcdate
        {
            set { mdcdate = value; }
            get { return mdcdate; }
        }

        private int mdcSeq = 0;
        /// <summary>
        /// 할인순서 int
        /// </summary>
        public int dcSeq
        {
            set { mdcSeq = value; }
            get { return mdcSeq; }
        }

        private string mtkNo = string.Empty;
        /// <summary>
        /// 시리얼번호
        /// </summary>
        public string tkNo
        {
            set { mtkNo = value; }
            get { return mtkNo; }
        }

        private string mcarNo = string.Empty;
        /// <summary>
        /// 차량번호
        /// </summary>
        public string carNo
        {
            set { mcarNo = value; }
            get { return mcarNo; }
        }


        private string mdcNo = string.Empty;
        /// <summary>
        /// 할인번호
        /// </summary>
        public string dcNo
        {
            set { mdcNo = value; }
            get { return mdcNo; }
        }


        private long mdcAmt = 0;
        /// <summary>
        /// 할인값
        /// </summary>
        public long dcAmt
        {
            set { mdcAmt = value; }
            get { return mdcAmt; }
        }

        private long mrealDcAmt = 0;
        /// <summary>
        /// 실제할인값
        /// </summary>
        public long realDcAmt
        {
            set { mrealDcAmt = value; }
            get { return mrealDcAmt; }
        }


        private int mrealDcDay = 0;
        /// <summary>
        /// 할인일
        /// </summary>
        public int realDcDay
        {
            set { mrealDcDay = value; }
            get { return mrealDcDay; }
        }


        private int mdcType = 0;
        /// <summary>
        /// 할인일
        /// </summary>
        public int dcType
        {
            set { mdcType = value; }
            get { return mdcType; }
        }

        private User muser = null;
        /// <summary>
        /// 사용자
        /// </summary>
        public User user
        {
            set { muser = value; }
            get { return muser; }

        }


        /// <summary>
        /// 할인항목 object
        /// </summary>


        private DiscountItem mdiscountItem = null;
        /// <summary>
        /// 사용자
        /// </summary>
        public DiscountItem discountItem
        {
            set { mdiscountItem = value; }
            get { return mdiscountItem; }

        }
    }
}
