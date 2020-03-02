using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Send
{
    [Serializable]
    public class SeasonCar
    {
        public enum ID
        {
            carNo,
            startDate,
            endDate,
            updateDate,
            month,
            amount
        }
        /// <summary>
        /// 차량번호
        /// </summary>
        public string carNo
        {
            set; get;
        }
        /// <summary>
        /// 연장시작일
        /// </summary>
        public long startDate
        {
            set; get;
        }
        /// <summary>
        /// 연장종료일
        /// </summary>
        public long endDate
        {
            set; get;
        }
        /// <summary>
        /// 업데이트날짜
        /// </summary>
        public long updateDate
        {
            set; get;
        }

        /// <summary>
        /// 연장개월수
        /// </summary>
        public int month
        {
            set; get;
        }

        /// <summary>
        /// 연장금액
        /// </summary>
        public long amount
        {
            set; get;
        }
    }
}
