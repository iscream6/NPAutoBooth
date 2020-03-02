using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Send
{
    [Serializable]
    public class Pettycashes
    {
        public Pettycashes(int pCoin1, int pCoin2, int pCoin3, int pCoin4, int pBill1, int pBill2, int pBill3, int pBill4)
        {
            settingTime = NPSYS.DateTimeToLongType(DateTime.Now);
            inOutType = 1;
            coin1 = pCoin1;
            coin2 = pCoin2;
            coin3 = pCoin3;
            coin4 = pCoin4;
            coin5 = 0;
            coin6 = 0;
            bill1 = pBill1;
            bill2 = pBill2;
            bill3 = pBill3;
            bill4 = pBill4;
            bill5 = 0;
            bill6 = 0;
            unit.fullCode = NPSYS.ParkCode + "-" + NPSYS.BoothID;
        }
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
        private Unit mUnit = new Unit();
        public Unit unit
        {
            set { mUnit = value; }
            get { return mUnit; }
        }
    }
}
