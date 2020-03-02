using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FadeFox.Document.ExcelLite.BinaryDrawingFormat
{
	public partial class MsofbtAnchor : EscherRecord
	{
		public MsofbtAnchor(EscherRecord record) : base(record) { }

		public MsofbtAnchor()
		{
			this.Type = EscherRecordType.MsofbtAnchor;
		}

	}
}
