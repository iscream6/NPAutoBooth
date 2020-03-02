using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Send
{
    [Serializable]
    public class Status_devices
    {
        public Status_devices(DeviceStatusManagement pDeviceStatusManagement)
        {
            mUnit.fullCode = NPSYS.ParkCode + "-" + NPSYS.BoothID;
            errCode = pDeviceStatusManagement.ErrorCode;
            status = pDeviceStatusManagement.DeviceStatus;
            sendTime = NPSYS.DateTimeToLongType(DateTime.Now);
        }
        private Unit mUnit = new Unit();
        public Unit unit
        {
            set { mUnit = value; }
            get { return mUnit; }
        }
        public string errCode
        {
            set; get;
        }
        public int status
        {
            set; get;
        }
        public long sendTime
        {
            set; get;
        }
    }
}
