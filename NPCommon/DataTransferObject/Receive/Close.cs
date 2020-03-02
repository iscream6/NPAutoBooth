using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class Close
    {
        public enum ID
        {
            closeCar,
            closePayment,
            closeCash,
            closeDiscount,
            closeStartDt,
            closeEndDt,
            unit
        }
        public CloseCar closeCar
        {
            set; get;
        }
        public ClosePayment closePayment
        {
            set; get;
        }
        public CloseCash closeCash
        {
            set; get;
        }
        public List<CloseDiscount> closeDiscount
        {
            set; get;
        }
        public long closeStartDt
        {
            set; get;
        }
        public long closeEndDt
        {
            set; get;
        }

        public CloseUnit unit
        {
            set; get;
        }
        public Status status
        {
            set; get;
        }
    }
}
