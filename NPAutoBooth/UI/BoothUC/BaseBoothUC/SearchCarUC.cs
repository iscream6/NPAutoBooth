using System;
using System.Windows.Forms;
using static NPAutoBooth.UI.BoothUC.BoothCommonLib;

namespace NPAutoBooth.UI.BoothUC
{
    public class SearchCarUC : UserControl, IConvertible
    {
        public virtual event Event_ConfigCall ConfigCall;
        public virtual event Event_NumberClick Number_Click;
        public virtual event EventHandler PreForm_Click;
        public virtual event EventHandler Home_Click;
        public virtual event EventHandler Confirm_Click;
        public virtual event EventHandler BackNumber_Click;
        public virtual event Event_LanguageChange LanguageChange_Click;

        public virtual UserControl_SearchCar SearchCarTextBar { get; }

        public virtual void Initialize()
        {

        }

        public virtual void OpenSetControl()
        {

        }

        #region Implements IConvertible

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return true;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return this.AccessibleName;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return this;
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        #endregion

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }
    }
}
