using NPCommon;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NPAutoBooth.Common
{
    public class GabigeCollectDelete
    {
        public static void GcCollecetDelete(object state)
        {
            try
            {
                GC.Collect();

            }
            catch 
            {

            }

        }
    }
    public class CenterAliveTimer
    {
        public static void CenterAliveTimerAction(object state)
        {
            try
            {
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.Booth, CommProtocol.DeviceStatus.Success, 0);

            }
            catch
            {

            }

        }
    }
}
