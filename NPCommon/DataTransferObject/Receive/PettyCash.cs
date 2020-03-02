using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class PettyCash
    {
        public long id { set; get; }
        public long settingTime { set; get; }
        public int inOutType { set; get; }
        public int coin1 { set; get; }
        public int coin2 { set; get; }
        public int coin3 { set; get; }
        public int coin4 { set; get; }
        public int coin5 { set; get; }
        public int coin6 { set; get; }
        public int bill1 { set; get; }
        public int bill2 { set; get; }
        public int bill3 { set; get; }
        public int bill4 { set; get; }
        public int bill5 { set; get; }
        public int bill6 { set; get; }
        public Status status
        {
            set; get;
        }
    }
}
