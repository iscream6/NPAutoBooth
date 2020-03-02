using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FadeFox.Document.ExcelLite.BinaryDrawingFormat
{
	public partial class MsofbtDggContainer : MsofbtContainer
	{
		public MsofbtDggContainer(EscherRecord record) : base(record) { }

		public MsofbtDggContainer()
		{
			this.Type = EscherRecordType.MsofbtDggContainer;
		}

	}
}
