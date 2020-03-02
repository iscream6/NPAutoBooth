using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FadeFox.Document.ExcelLite.BinaryFileFormat
{
	public partial class End : SubRecord
	{
		public End(SubRecord record) : base(record) { }

		public End()
		{
			this.Type = SubRecordType.End;
		}

	}
}
