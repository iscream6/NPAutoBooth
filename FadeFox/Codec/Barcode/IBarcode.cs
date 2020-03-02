using System;
using System.Collections.Generic;
using System.Text;

namespace FadeFox.Codec.Barcode
{
	interface IBarcode
	{
		string Encoded_Value
		{
			get;
		}//Encoded_Value

		string RawData
		{
			get;
		}//Raw_Data

		string FormattedData
		{
			get;
		}//FormattedData
	}//interface
}
