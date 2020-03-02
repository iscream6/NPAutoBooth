using FadeFox.Text;
using System;
using static NPAutoBooth.UI.BoothUC.BoothCommonLib;

namespace NPAutoBooth.UI.BoothUC
{
    public class FormFactory
    {
        private static FormFactory instance;
        private FormFactory() { }

        public static FormFactory GetInstance()
        {
            if (instance == null) instance = new FormFactory();
            return instance;
        }

        #region Factories

        private T ChangeType<T>(object o)
        {
            Type conversionType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            return (T)Convert.ChangeType(o, conversionType);
        }

        /// <summary>
        /// 화면 해상도에 맞는 Design Control을 반환한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rate"></param>
        /// <returns></returns>
        public T GetDesignControl<T>(ClientAreaRate rate)
        {
            T value;

            try
            {
                if (typeof(T) == typeof(MainUC))
                {
                    switch (rate)
                    {
                        case ClientAreaRate._4vs3:
                            value = (T)Convert.ChangeType(new Ctl4by3Main(), typeof(T));
                            break;
                        case ClientAreaRate._16vs9:
                            value = (T)Convert.ChangeType(new Ctl4by3Main(), typeof(T));
                            break;
                        case ClientAreaRate._9vs16:
                            value = (T)Convert.ChangeType(new Ctl9by16Main(), typeof(T));
                            break;
                        default:
                            value = (T)Convert.ChangeType(new Ctl4by3Main(), typeof(T));
                            break;
                    }
                }
                else if (typeof(T) == typeof(InformationUC))
                {
                    switch (rate)
                    {
                        case ClientAreaRate._4vs3:
                            value = (T)Convert.ChangeType(new Ctl4by3Information(), typeof(T));
                            break;
                        case ClientAreaRate._16vs9:
                            value = (T)Convert.ChangeType(new Ctl4by3Information(), typeof(T));
                            break;
                        case ClientAreaRate._9vs16:
                            value = (T)Convert.ChangeType(new Ctl9by16Information(), typeof(T));
                            break;
                        default:
                            value = (T)Convert.ChangeType(new Ctl4by3Information(), typeof(T));
                            break;
                    }
                }
                else if (typeof(T) == typeof(ReciptUC))
                {
                    switch (rate)
                    {
                        case ClientAreaRate._4vs3:
                            value = (T)Convert.ChangeType(new Ctl4by3Recipt(), typeof(T));
                            break;
                        case ClientAreaRate._16vs9:
                            value = (T)Convert.ChangeType(new Ctl4by3Recipt(), typeof(T));
                            break;
                        case ClientAreaRate._9vs16:
                            value = (T)Convert.ChangeType(new Ctl9by16Recipt(), typeof(T));
                            break;
                        default:
                            value = (T)Convert.ChangeType(new Ctl4by3Recipt(), typeof(T));
                            break;
                    }
                }
                else if (typeof(T) == typeof(SearchCarUC))
                {
                    switch (rate)
                    {
                        case ClientAreaRate._4vs3:
                            value = (T)Convert.ChangeType(new Ctl4by3SearchCar(), typeof(T));
                            break;
                        case ClientAreaRate._16vs9:
                            value = (T)Convert.ChangeType(new Ctl4by3SearchCar(), typeof(T));
                            break;
                        case ClientAreaRate._9vs16:
                            value = (T)Convert.ChangeType(new Ctl9by16SearchCar(), typeof(T));
                            break;
                        default:
                            value = (T)Convert.ChangeType(new Ctl4by3SearchCar(), typeof(T));
                            break;
                    }
                }
                else if (typeof(T) == typeof(SelectCarUC))
                {
                    switch (rate)
                    {
                        case ClientAreaRate._4vs3:
                            value = (T)Convert.ChangeType(new Ctl4by3SelectCar(), typeof(T));
                            break;
                        case ClientAreaRate._16vs9:
                            value = (T)Convert.ChangeType(new Ctl4by3SelectCar(), typeof(T));
                            break;
                        case ClientAreaRate._9vs16:
                            value = (T)Convert.ChangeType(new Ctl9by16SelectCar(), typeof(T));
                            break;
                        default:
                            value = (T)Convert.ChangeType(new Ctl4by3SelectCar(), typeof(T));
                            break;
                    }
                }
                else if(typeof(T) == typeof(PaymentUC))
                {
                    switch (rate)
                    {
                        case ClientAreaRate._4vs3:
                            value = (T)Convert.ChangeType(new Ctl4by3Payment(), typeof(T));
                            break;
                        case ClientAreaRate._16vs9:
                            value = (T)Convert.ChangeType(new Ctl4by3Payment(), typeof(T));
                            break;
                        case ClientAreaRate._9vs16:
                            value = (T)Convert.ChangeType(new Ctl9by16Payment(), typeof(T));
                            break;
                        default:
                            value = (T)Convert.ChangeType(new Ctl4by3Payment(), typeof(T));
                            break;
                    }
                }
                else
                {
                    return default(T);
                }

                return value;
            }
            catch(InvalidCastException ie)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "BoothCommon|FormFactory", $"타입 변환 오류 : {ie.Message}");
                return default(T);
            }
            catch (Exception e)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "BoothCommon|FormFactory", e.Message);
                return default(T);
            }
        }

        #endregion
    }
}
