﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FadeFox.Document.ExcelLite.BinaryFileFormat
{
	public partial class MSODRAWINGGROUP : MSOCONTAINER
	{
		public MSODRAWINGGROUP(Record record) : base(record) { }

		public MSODRAWINGGROUP()
		{
			this.Type = RecordType.MSODRAWINGGROUP;
		}

	}
}
