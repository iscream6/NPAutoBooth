using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class Nation
    {
        public enum ID
        {
            /// <summary>
            /// 시리얼번호
            /// </summary>
            code
        }
        public string code
        {
            set; get;
        }
    }
}
