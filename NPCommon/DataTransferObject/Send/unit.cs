using System;

namespace NPCommon.DTO.Send
{
    [Serializable]
    public class Unit
    {
        public enum ID
        {
            fullcode,
        }

        public string fullCode
        {
            set; get;
        }
    }
}