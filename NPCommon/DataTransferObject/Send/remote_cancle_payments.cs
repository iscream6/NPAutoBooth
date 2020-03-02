using System;

namespace NPCommon.DTO.Send
{
    [Serializable]
    public class Remote_cancle_payments
    {
        public Remote_cancle_payments(string pData)
        {
            status = pData;
        }

        public string status { set; get; }
    }
}