﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FadeFox.Document.ExcelLite.BinaryDrawingFormat
{
	public partial class MsofbtAlignRule : EscherRecord
	{
		public MsofbtAlignRule(EscherRecord record) : base(record) { }

		public MsofbtAlignRule()
		{
			this.Type = EscherRecordType.MsofbtAlignRule;
		}

	}
}
