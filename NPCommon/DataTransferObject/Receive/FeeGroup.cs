using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class FeeGroup
    {
        public long id { set; get; }
        public string feeName { set; get; }
        public long feeMax { set; get; }
        public long freeTime { set; get; }
        public string isBasicGroup { set; get; }
        public string dayMaxCriteria { set; get; }
        public long dayMaxAmt { set; get; }
        public long freeTimeAfterPay { set; get; }
        public string discountCriteria { set; get; }
        public List<FeeItem> feeItem { set; get; }
    }
}
