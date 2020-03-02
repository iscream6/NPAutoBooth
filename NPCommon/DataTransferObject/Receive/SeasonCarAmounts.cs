using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class SeasonCarAmounts
    {
        public long id
        {
            set; get;
        }

        public long month
        {
            set; get;
        }
        public long amount
        {
            set; get;

        }
    }
}
