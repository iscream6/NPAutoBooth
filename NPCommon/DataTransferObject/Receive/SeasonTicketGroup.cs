using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class SeasonTicketGroup
    {
        public enum ID
        {
            id
        }
        public long id
        {
            set; get;
        }
    }
}
