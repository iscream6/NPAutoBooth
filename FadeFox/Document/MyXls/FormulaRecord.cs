using System;
using System.Collections.Generic;
using System.Text;
using FadeFox.Document.MyXls.ByteUtil;

namespace FadeFox.Document.MyXls
{
    internal class FormulaRecord : Record
    {
        internal Record StringRecord = null;

        internal FormulaRecord(Record formulaRecord, Record stringRecord)
            : base()
        {
            _rid = formulaRecord.RID;
            _data = formulaRecord.Data;
            _continues = formulaRecord.Continues;

            StringRecord = stringRecord;
        }
    }
}
