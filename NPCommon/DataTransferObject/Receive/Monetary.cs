using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class Monetary
    {
        public enum ID
        {
            id,
            language,
            unitName,
            unitSign,
            coin1,
            coin2,
            coin3,
            coin4,
            coin5,
            coin6,
            bill1,
            bill2,
            bill3,
            bill4,
            bill5,
            bill6
        }

        public long id
        {
            set; get;
        }

        public string language
        {
            set; get;
        }


        public string unitName
        {
            set; get;
        }

        public string unitSign
        {
            set; get;
        }
        public long coin1
        {
            set;
            get;
        }
        public long coin2
        {
            set;
            get;
        }


        public long coin3
        {
            set;
            get;
        }


        public long coin4
        {
            set;
            get;
        }


        public long coin5
        {
            set;
            get;
        }


        public long coin6
        {
            set;
            get;
        }

        public long bill1
        {
            set;
            get;
        }

        public long bill2
        {
            set;
            get;
        }

        public long bill3
        {
            set;
            get;
        }


        public long bill4
        {
            set;
            get;
        }


        public long bill5
        {
            set;
            get;
        }


        public long bill6
        {
            set;
            get;
        }

        public long GetIdValue(ID pId)
        {
            switch (pId)
            {
                case ID.bill1:
                    return bill1;
                case ID.bill2:
                    return bill2;
                case ID.bill3:
                    return bill3;
                case ID.bill4:
                    return bill4;
                case ID.bill5:
                    return bill5;
                case ID.bill6:
                    return bill6;
                case ID.coin1:
                    return coin1;
                case ID.coin2:
                    return coin2;
                case ID.coin3:
                    return coin3;
                case ID.coin4:
                    return coin4;
                case ID.coin5:
                    return coin5;
                case ID.coin6:
                    return coin6;

            }
            return 0;
        }

        public string GetBillName(long pBillType)
        {
            string moneyName = string.Empty;

            if (NPSYS.CurrentMoneyType == NPCommon.ConfigID.MoneyType.WON)
            {
                switch (pBillType)
                {

                    case 10:
                        moneyName = "10       원";
                        break;
                    case 50:
                        moneyName = "50       원";
                        break;
                    case 100:
                        moneyName = "100      원";
                        break;
                    case 500:
                        moneyName = "500      원";
                        break;
                    case 1000:
                        moneyName = "1000     원";
                        break;
                    case 5000:
                        moneyName = "5000     원";
                        break;
                    case 10000:
                        moneyName = "10000    원";
                        break;
                    case 50000:
                        moneyName = "50000    원";
                        break;

                }
            }

            if (NPSYS.CurrentMoneyType == NPCommon.ConfigID.MoneyType.PHP)
            {
                switch (pBillType)
                {

                    case 1:
                        moneyName = "1       PHP";
                        break;
                    case 5:
                        moneyName = "5       PHP";
                        break;
                    case 10:
                        moneyName = "10      PHP";
                        break;
                    case 20:
                        moneyName = "20      PHP";
                        break;
                    case 50:
                        moneyName = "50      PHP";
                        break;
                    case 100:
                        moneyName = "100     PHP";
                        break;

                }
            }

            return moneyName;
        }
    }
}
