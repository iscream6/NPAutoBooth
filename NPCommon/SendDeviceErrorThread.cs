using FadeFox.Text;
using NPCommon.DTO.Receive;
using NPCommon.REST;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NPCommon
{
    /// <summary>
    /// 장비에러 상태 변화에 따라서 웹서버로 장비에러상태를 보낸다
    /// </summary>
    public class SendDeviceErrorThread
    {
        public static ManualResetEvent ErrorSendManualEvent = new ManualResetEvent(true);
        private HttpProcess mHttpProcess = new HttpProcess();
        private bool bForever = true;
        public SendDeviceErrorThread()
        {

        }

        #region DB접속관련
        /// <summary>
        /// DB재연결 Thread
        /// </summary>
        private Thread mSendThread = null;
        /// <summary>
        /// db 재연결 쓰래드를 생성한다
        /// </summary>
        public void StartSendThread()
        {
            mSendThread = new Thread(SendLoop);
            mSendThread.IsBackground = true;
            mSendThread.Start();
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "SendDeviceErrorThread | StartDbThread", "에러메세지 전송시스템 처리");
        }
        /// <summary>
        /// db 재연결 쓰래드를 종료한다
        /// </summary>
        public void EndSendThread()
        {
            bForever = false;
            try
            {
                mSendThread.Abort();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "SendDeviceErrorThread | EndDbThread", "에러메세지 전송시스템  동작중지");
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SendDeviceErrorThread | EndDbThread", ex.ToString());
            }
        }
        private bool ActionSendData()
        {
            try
            {
                ErrorSendManualEvent.WaitOne(50000);
                ErrorSendManualEvent.Reset();

                List<string> deleteMove = new List<string>();
                foreach (KeyValuePair<string, DeviceStatusManagement> pair in NPSYS.gDic_DeviceStatusManageMent)
                {
                    if (pair.Value.UpdateFlag == false)
                    {
                        Status receiveErrorStatus = mHttpProcess.SendErrorMessgae(pair.Value);
                        if (receiveErrorStatus.Success)
                        {
                            pair.Value.UpdateFlag = true;
                            deleteMove.Add(pair.Value.Key);
                        }
                        else
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SendDeviceErrorThread | ActionSendData", "에러데이터 전송에러:"
                                                                                                                 + " 장비코드:" + pair.Value.ErrorCode.ToString()
                                                                                                                 + " 정상여부:" + (pair.Value.DeviceStatus == 1 ? false : true).ToString());
                            break;
                        }
                    }
                }
                for (int i = 0; i < deleteMove.Count; i++)
                {
                    NPSYS.gDic_DeviceStatusManageMent.Remove(deleteMove[i]);
                }



                return true;
            }
            catch (Exception ex)
            {

                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SendDeviceErrorThread | ActionSendData", "예외사항:" + ex.ToString());
                return false;
            }
            finally
            {
                ErrorSendManualEvent.Set();
            }
        }
        private void SendLoop()
        {
            while (bForever)
            {
                ActionSendData();
                Thread.Sleep(30000);
            }
        }


        #endregion
    }
}
