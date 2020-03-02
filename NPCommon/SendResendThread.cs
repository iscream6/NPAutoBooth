using FadeFox.Text;
using NPCommon.REST;
using System;
using System.Data;
using System.Threading;

namespace NPCommon
{
    /// <summary>
    /// 장비에러 상태 변화에 따라서 웹서버로 장비에러상태를 보낸다
    /// </summary>
    public class SendResendThread
    {
        private HttpProcess mHttpProcess = new HttpProcess();
        private bool bForever = true;
        public SendResendThread()
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
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "SendResendThread | StartDbThread", "재전송시스템 동작시킴");
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
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "SendResendThread | EndDbThread", "재전송시스템  동작중지시킴");
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SendResendThread | EndDbThread", ex.ToString());
            }
        }
        private bool ActionSendData()
        {
            try
            {
                if (NPSYS.CurrentBusyType == NPSYS.BusyType.None && NPSYS.CurrentFormType == NPSYS.FormType.Main)
                {
                    DataTable dtResendData = LPRDbSelect.GetReSendData();
                    if (dtResendData != null && dtResendData.Rows.Count > 0)
                    {
                        foreach (DataRow sendItemRow in dtResendData.Rows)
                        {
                            string id = sendItemRow["ID"].ToString();
                            string currentUrl = sendItemRow["CURRENTURL"].ToString();
                            string sendData = sendItemRow["SENDDATA"].ToString();
                            bool isSuccessResave = mHttpProcess.RePaySave(currentUrl, sendData);
                            if (isSuccessResave)
                            {
                                LPRDbSelect.UpdateReSendData(Convert.ToInt32(id));
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {

                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SendResendThread | ActionSendData", "예외사항:" + ex.ToString());
                return false;
            }
        }

        private void SendLoop()
        {
            while (bForever)
            {
                ActionSendData();
                Thread.Sleep(10000);
            }
        }


        #endregion
    }
}
