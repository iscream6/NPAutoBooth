using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class Date
    {
        public enum ID
        {
            unixTime
        }
        public long unixTime
        {
            set; get;
        }
    }
}
