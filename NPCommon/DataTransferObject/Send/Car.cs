using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Send
{
    [Serializable]
    public class Car
    {
        public enum ID
        {
            tkno
        }
        public string tkNo
        {
            set; get;
        }
    }
}
