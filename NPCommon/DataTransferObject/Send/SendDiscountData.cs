using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Send
{
    [Serializable]
    public class SendDiscountData
    {
        public enum ID
        {
            tkNo,
            serialNo,
            unit,
        }

        public string tkNo { set; get; }
        public string serialNo { set; get; }
        public string ticketType { set; get; }
        private Unit mUnit = new Unit();
        public Unit unit
        {
            set { mUnit = value; }
            get { return mUnit; }
        }
        public SendDiscountData(NormalCarInfo pCarInfo, string pDiscountData, DcDetail.DIscountTicketType pDiscountType)
        {
            mUnit.fullCode = NPSYS.ParkCode + "-" + NPSYS.BoothID;
            tkNo = pCarInfo.TkNO;
            serialNo = pDiscountData;
            ticketType = pDiscountType.ToString();

        }
    }
}
