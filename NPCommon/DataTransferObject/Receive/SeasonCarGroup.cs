using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class SeasonCarGroup
    {
        public enum ID
        {
            id
        }

        public long id
        {
            set; get;
        }

        public string groupName
        {
            set; get;
        }
        public string carType
        {
            set; get;
        }

        public string startTime
        {
            set; get;
        }

        public string endTime
        {
            set; get;
        }

        public List<SeasonCarAmounts> seasonCarAmounts
        {
            set; get;
        }
    }
}
