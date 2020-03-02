using System;
using System.ComponentModel;

namespace NPAutoBooth.UI.BoothUC
{
    public static class BoothCommonLib
    {
        public delegate void Event_CarSelected(int index);

        public delegate void Event_ConfigCall();

        public delegate void Event_LanguageChange(LanguageType languageType);

        public delegate void Event_NumberClick(string number);

        public delegate void Event_SeasonCarAddMonth(int monthAmount);

        public static event ChangeView EventExitSerachForm;

        public enum ClientAreaRate
        {
            _4vs3,
            _16vs9,
            _9vs16
        }

        public enum LanguageType
        {
            KOREAN,
            ENGLISH,
            JAPAN
        }
    }

    public class AbstractControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
    {
        public AbstractControlDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
        {
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType == typeof(TAbstract))
                objectType = typeof(TBase);

            return base.CreateInstance(provider, objectType, argTypes, args);
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            if (objectType == typeof(TAbstract))
                return typeof(TBase);

            return base.GetReflectionType(objectType, instance);
        }
    }
}