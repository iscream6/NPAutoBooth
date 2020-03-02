using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class ParkingLot
    {
        public enum ID
        {
            id,
            code,
            name,
            address,
            businessNo,
            ceoName,
            phone
        }

        public long id
        {
            set; get;
        }
        public string code
        {
            set; get;
        }

        public string name
        {
            set; get;
        }
        public string address
        {
            set; get;
        }
        public string businessNo
        {
            set; get;
        }
        public string ceoName
        {
            set; get;
        }
        public string phone
        {
            set; get;
        }
    }
}
