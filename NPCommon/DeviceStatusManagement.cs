using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon
{
    public class DeviceStatusManagement
    {
        private string mDeviceCode = string.Empty;
        public string DeviceCode
        {
            set { mDeviceCode = value; }
            get { return mDeviceCode; }
        }
        private string mDeviceName = string.Empty;
        public string DeviceName
        {
            set { mDeviceName = value; }
            get { return mDeviceName; }
        }

        private int mDeviceStatus = 2; // 정상
        /// <summary>
        /// 1.장애 2 정상 3사용안함
        /// </summary>
        public int DeviceStatus
        {
            set { mDeviceStatus = value; }
            get { return mDeviceStatus; }
        }

        private string mErrorCode = string.Empty;
        public string ErrorCode
        {
            set { mErrorCode = value; }
            get { return mErrorCode; }
        }

        private bool mUpdateFlag = false;
        public bool UpdateFlag
        {
            set { mUpdateFlag = value; }
            get { return mUpdateFlag; }
        }

        private string mTkKey = string.Empty;
        public string Key
        {
            set { mTkKey = value; }
            get { return mTkKey; }
        }
    }
}
