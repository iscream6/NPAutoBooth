using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class Certs
    {
        /// <summary>
        /// cert클래스에 사용하는 각 객체또는 값의 이름
        /// </summary>
        public Status status
        {
            set; get;
        }
        public Nation nation
        {
            set; get;
        }
        public Date date
        {
            set; get;
        }
        public Monetary monetary
        {
            set; get;
        }
        public ParkingLot parkingLot
        {
            set; get;
        }
        public Unit unit
        {
            set; get;
        }
    }
}
