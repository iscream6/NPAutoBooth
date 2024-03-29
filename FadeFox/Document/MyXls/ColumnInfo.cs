using System;
using FadeFox.Document.MyXls.ByteUtil;

namespace FadeFox.Document.MyXls
{
    /// <summary>
    /// Describes a range of columns and properties to set on those columns (column width, etc.).
    /// </summary>
	public class ColumnInfo
	{
		private readonly XlsDocument _doc;
		private readonly Worksheet _worksheet;

		private ushort _colIdxStart = 0;
		private ushort _colIDxEnd = 0;
		private ushort _width = 2560; //Set default to 10-character column width
        private bool _hidden;
        private bool _collapsed;
        private byte _outlineLevel;

        /// <summary>
        /// Initializes a new instance of the ColumnInfo class for the given Doc
        /// and Worksheet.
        /// </summary>
        /// <param name="doc">The parent MyXls.Doc object for the new ColumnInfo object.</param>
        /// <param name="worksheet">The parent MyXls.Worksheet object for the new ColumnInfo object.</param>
		public ColumnInfo(XlsDocument doc, Worksheet worksheet)
		{
			_doc = doc;
			_worksheet = worksheet;
		}

		/// <summary>
		/// Gets or sets index to first column in the range.
		/// </summary>
		public ushort ColumnIndexStart
		{
			get { return _colIdxStart; }
			set
			{
				_colIdxStart = value;
				if (_colIDxEnd < _colIdxStart)
					_colIDxEnd = _colIdxStart;
			}
		}

		/// <summary>
		/// Gets or set index to last column in the range.
		/// </summary>
		public ushort ColumnIndexEnd
		{
			get { return _colIDxEnd; }
			set
			{
				_colIDxEnd = value;
				if (_colIdxStart > _colIDxEnd)
					_colIdxStart = _colIDxEnd;
			}
		}

		/// <summary>
		/// Gets or sets width of the columns in 1/256 of the width of the zero character, using default font (first FONT record in the file).
		/// </summary>
		public ushort Width
		{
			get { return _width; }
			set { _width = value; }
		}

        /// <summary>
        /// Gets or sets XF record (➜5.114) for default column formatting
        /// </summary>
        public XF ExtendedFormat
        {
            get { throw new NotSupportedException("ColumnInfo.get_ExtendedFormat"); }
            set { throw new NotSupportedException("ColumnInfo.set_ExtendedFormat"); }
        }

        /// <summary>
        /// Gets or sets whether the columns included in this ColumnInfo definition are hidden.
        /// </summary>
        public bool Hidden
        {
            get { return _hidden; }
            set { _hidden = value; }
        }

        /// <summary>
        /// Gets or sets whether the columns included in this ColumnInfo definition are collapsed.
        /// </summary>
        public bool Collapsed
        {
            get { return _collapsed; }
            set { _collapsed = value; }
        }

        /// <summary>
        /// Gets or sets the outline level of the columns (0 = no outline).
        /// </summary>
        public byte OutlineLevel
        {
            get { return _outlineLevel; }
            set { if (value > 0x07)
                throw new ArgumentException(string.Format("value {0} must be between 0 and 7", value)); _outlineLevel = value; }
        }

        internal Bytes Bytes
		{
			get
			{
				Bytes bytes = new Bytes();

				bytes.Append(BitConverter.GetBytes(_colIdxStart));
				bytes.Append(BitConverter.GetBytes(_colIDxEnd));
				bytes.Append(BitConverter.GetBytes(_width));
				bytes.Append(BitConverter.GetBytes((ushort)0)); //Index to XF record for default column formatting
				bytes.Append(COLINFO_OPTION_FLAGS());
				bytes.Append(new byte[0]); //Not used

			    return Record.GetBytes(RID.COLINFO, bytes);
			}
		}

		private Bytes COLINFO_OPTION_FLAGS()
		{
		    bool[] bits = new bool[16];
		    bits[0] = _hidden;
		    Bytes outlineBytes = new Bytes(_outlineLevel);
		    bool[] outlineLevelBits = outlineBytes.GetBits().Get(3).Values;
            outlineLevelBits.CopyTo(bits, 8);
		    bits[12] = _collapsed;

		    return new Bytes.Bits(bits).GetBytes();
		}
	}
}
