using FadeFox.Text;
using HttpServer;
using HttpServer.Headers;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using HttpListener = HttpServer.HttpListener;

namespace NPCommon.REST
{
    public class NPHttpServer
    {
        public static string REQ_GET_XVR_LIST = "/v1.0/xvr/list";
        public static string REQ_POST_EVT_BACKUP_STAT = "/v1.0/xvr/evt/video/backup/stat";

        public delegate void ReceiveData(RequestEventArgs pEvent, CmdType pCmdType, string strMethod, string strData);
        public event ReceiveData EventOnReceiveData;
        private HttpListener m_listener = null;

        public enum CmdType
        {
            none,
            /// <summary>
            /// LPR로 찍었을시요금부과안되는 입출차 미매칭차량 및 정기권차량
            /// </summary>
            cars,
            remote_cars,
            /// <summary>
            /// LPR로 찍얼을시 요금부과되는 모든차량(시간대정기권,일반차량)
            /// </summary>
            payments,
            remote_payments,
            remote_cacnel_payments,
            remote_discounts,
            remote_print_payments,
            remote_closes,
            //TMAP연동
            interworking_cars,
            interworking_payments,
            //TMAP연동완료
        }


        public void Start(int pPort)
        {
            m_listener = HttpListener.Create(IPAddress.Any, Convert.ToInt32(NPSYS.gRestFulLocalPort));

            m_listener.Start(10);
            m_listener.SocketAccepted += onAccept;
            m_listener.RequestReceived += OnRequestReceive;     // 웹서버에서 데이터가 전달 되면 호출됨
            m_listener.ErrorPageRequested += OnErrorPageRequested;
            m_listener.ContinueResponseRequested += OnContinueResponseRequested;
        }

        public void Stop()
        {
            m_listener.Stop();
            m_listener.RequestReceived -= OnRequestReceive;     // 웹서버에서 데이터가 전달 되면 호출됨
        }


        private void OnContinueResponseRequested(object sender, RequestEventArgs e)
        {
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPHttpServer | OnErrorPageRequested", "[OnContinueResponseRequested]");
        }

        private void OnErrorPageRequested(object sender, ErrorPageEventArgs e)
        {
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPHttpServer | OnErrorPageRequested", "[OnErrorPageRequested]");
        }
        private void onAccept(object sender, SocketFilterEventArgs e)
        {
            IPEndPoint remoteIpEndPoint = e.Socket.RemoteEndPoint as IPEndPoint;

            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPHttpServer | onAccept", "[서버에 클라이언트 접속]" + remoteIpEndPoint.Address);
        }
        private void OnRequestReceive(object sender, RequestEventArgs e)
        {
            try
            {
                StreamReader reader = new StreamReader(e.Context.Request.Body);
                string readData = reader.ReadToEnd();

                JObject obj = JObject.Parse(readData);

                if (obj == null)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPHttpServer | OnRequestReceive", "[123]");
                }
                else
                {
                    string urlData = e.Request.Uri.PathAndQuery;
                    e.Response.Connection.Type = ConnectionType.Close;
                    e.Response.ContentType = new ContentTypeHeader("application/json;charset=UTF-8");
                    e.Response.Reason = "OK";


                    urlData = urlData.Replace("-", "_").Replace("/", "");
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPHttpServer | OnRequestReceive", "[서버에서 수신받은 데이터]"
                                  + " URL:" + urlData + " 받은데이터:" + readData);

                    CmdType currentCMd = CmdType.none;
                    currentCMd = (CmdType)Enum.Parse(typeof(CmdType), urlData);

                    EventOnReceiveData(e, currentCMd, e.Request.Method, readData);
                }
            }
            catch (Exception ex)
            {
                HttpProcess mHttpProcess = new HttpProcess();
                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(CmdType.none, "전문오류", "현재 해당전문 없음", false));
                e.Response.Body.Write(sendData, 0, sendData.Length);
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPHttpServer | OnRequestReceive", ex.ToString());
            }
        }

    }
}
