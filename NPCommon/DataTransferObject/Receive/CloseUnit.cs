﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.DTO.Receive
{
    [Serializable]
    public class CloseUnit
    {
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
