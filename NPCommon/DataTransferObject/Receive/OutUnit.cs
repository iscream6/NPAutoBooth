using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class OutUnit
    {
        public enum ID
        {
            id,
            fullcode
        }
        public long id
        {
            set; get;
        }
        public string fullCode
        {
            set; get;
        }
    }
}
