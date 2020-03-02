using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO
{
    [Serializable]
    public class DcDetail
    {
        public enum DIscountTicketType
        {
            NONE,
            /// <summary>
            /// 마그네틱 할인권
            /// </summary>
            MI,
            /// <summary>
            /// 바코드할인권
            /// </summary>
            BR,
            /// <summary>
            /// 바코드 삽입형 할인권
            /// </summary>
            BI

        }
        public DIscountTicketType currentDiscountTicketType = DIscountTicketType.NONE;

        private string mDcTkno = string.Empty;
        public string DcTkno
        {
            set { mDcTkno = value; }
            get { return mDcTkno; }
        }

        private bool mUseYn = false;
        public bool UseYn
        {
            set { mUseYn = value; }
            get { return mUseYn; }

        }
    }
}
