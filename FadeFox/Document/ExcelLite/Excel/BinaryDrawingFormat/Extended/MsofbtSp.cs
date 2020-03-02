using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FadeFox.Document.ExcelLite.BinaryDrawingFormat
{
	public partial class MsofbtSp : EscherRecord
	{
        public ShapeType ShapeType
        {
            get
            {
                return (ShapeType)Instance;
            }
            set
            {
                Instance = (ushort)value;
            }
        }
	}
}
