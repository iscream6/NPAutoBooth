﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FadeFox.Document.ExcelLite.BinaryFileFormat
{
	/// <summary>
	/// This record contains the definition of all user-defined  colours  available for cell and object formatting.
	/// </summary>
	public partial class PALETTE : Record
	{
		public PALETTE(Record record) : base(record) { }

		public PALETTE()
		{
			this.Type = RecordType.PALETTE;
			this.Colors = new List<Int32>();
		}

		/// <summary>
		/// Number of following colours.
		/// </summary>
		public Int16 NumColors;

		/// <summary>
		/// List of RGB colours.
		/// </summary>
		public List<Int32> Colors;

		public void decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.NumColors = reader.ReadInt16();
			reader.ReadInt32();
		}

		public void encode()
		{
			MemoryStream stream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(stream);
			writer.Write(NumColors);
			foreach(Int32 int32Var in Colors)
			{
				writer.Write(int32Var);
			}
			this.Data = stream.ToArray();
			this.Size = (UInt16)Data.Length;
			base.Encode();
		}

	}
}
