using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class Unit
    {
        public enum ID
        {
            id,
            fullcode,
            name
        }

        public long id
        {
            set; get;
        }
        public string fullCode
        {
            set; get;
        }
        public string name
        {
            set; get;
        }
    }
}
