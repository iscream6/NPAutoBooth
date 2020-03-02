using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class FeeItem
    {
        public long id { set; get; }
        public long workingDay { set; get; }
        public string feeItemName { set; get; }
        public long carType { set; get; }
        public string startTime { set; get; }
        public string endTime { set; get; }
        public long basicTime { set; get; }
        public long basicFee { set; get; }
        public long freeTime { set; get; }
        public long perFee { set; get; }
        public long perTime { set; get; }
        public long maxFee { set; get; }
        public string status { set; get; }
    }
}
