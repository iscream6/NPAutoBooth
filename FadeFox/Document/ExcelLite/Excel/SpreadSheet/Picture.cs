using System;
using System.Collections.Generic;
using System.Text;
using FadeFox.Document.ExcelLite.Library;

namespace FadeFox.Document.ExcelLite.SpreadSheet
{
    public class Picture
    {
        public CellAnchor TopLeftCorner;
        public CellAnchor BottomRightCorner;
        public Image Image;

        public Pair<int, int> CellPos
        {
            get
            {
                return new Pair<int, int>(TopLeftCorner.RowIndex, TopLeftCorner.ColIndex);
            }
        }
    }
}
