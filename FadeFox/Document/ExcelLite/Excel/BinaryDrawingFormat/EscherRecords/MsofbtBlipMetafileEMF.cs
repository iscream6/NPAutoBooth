﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FadeFox.Document.ExcelLite.BinaryDrawingFormat
{
	public partial class MsofbtBlipMetafileEMF : MsofbtBlip
	{
		public MsofbtBlipMetafileEMF(EscherRecord record) : base(record) { }

		public MsofbtBlipMetafileEMF()
		{
			this.Type = EscherRecordType.MsofbtBlipMetafileEMF;
		}

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.UID = new Guid(reader.ReadBytes(16));
			this.Marker = reader.ReadByte();
			this.ImageData = reader.ReadBytes((int)(stream.Length - stream.Position));
		}

		public override void Encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(UID.ToByteArray());
			writer.Write(Marker);
			writer.Write(ImageData);
			this.Data = stream.ToArray();
			this.Size = (UInt32)Data.Length;
			base.Encode();
		}

	}
}
