using System;

namespace NPCommon.DTO.Send
{
    [Serializable]
    public class SendDiscountCancleData
    {
        public enum ID
        {
            tkNo,
            serialNo,
            ticketUse,
        }

        public string tkNo { set; get; }
        public string serialNo { set; get; }
        public string ticketUse { set; get; }

        public SendDiscountCancleData(NormalCarInfo pNormaInfo, DcDetail pDcdetail)
        {
            tkNo = pNormaInfo.TkNO;
            serialNo = pDcdetail.DcTkno;
            ticketUse = "N";
        }
    }
}