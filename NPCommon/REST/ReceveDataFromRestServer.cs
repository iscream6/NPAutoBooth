using System;

namespace NPCommon.REST
{
    [Serializable]
    public class ReceveDataFromRestServer
    {
        public HttpServer.RequestEventArgs Event
        {
            set; get;
        }
        public NPHttpServer.CmdType CmdType
        {
            set; get;
        }
        public string Method
        {
            set; get;
        }
        public string Data
        {
            set; get;
        }
    }
}
