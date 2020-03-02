using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPAutoBooth.Common
{
    /// <summary>
    /// 확장
    /// </summary>
    public static class ExtCommon
    {
        #region Extends Panel

        /// <summary>
        /// 부모 컨테이너 내 위치한 Panel 사각형 영역을 반환한다.
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        public static Rectangle GetArea(this Panel panel)
        {
            Rectangle recArea = new Rectangle
            {
                X = panel.Location.X,
                Y = panel.Location.Y,
                Width = panel.Width,
                Height = panel.Height
            };
            return recArea;
        }

        #endregion

        #region Object

        /// <summary>
        /// Converts input to Type of typeparam T
        /// </summary>
        /// <typeparam name="T">typeparam is the type in which value will be returned, it could be any type eg. int, string, bool, decimal etc.</typeparam>
        /// <param name="input">Input that need to be converted to specified type</param>
        /// <returns>Input is converted in Type of default value or given as typeparam T and returned</returns>
        public static T To<T>(this object input)
        {
            return To(input, default(T));
        }

        /// <summary>
        /// Converts input to Type of default value or given as typeparam T
        /// </summary>
        /// <typeparam name="T">typeparam is the type in which value will be returned, it could be any type eg. int, string, bool, decimal etc.</typeparam>
        /// <param name="input">Input that need to be converted to specified type</param>
        /// <param name="defaultValue">defaultValue will be returned in case of value is null or any exception occures</param>
        /// <returns>Input is converted in Type of default value or given as typeparam T and returned</returns>
        private static T To<T>(object input, T defaultValue)
        {
            var result = defaultValue;
            try
            {
                if (input == null || input == DBNull.Value) return result;
                if (typeof(T).IsEnum)
                {
                    result = (T)Enum.ToObject(typeof(T), To(input, Convert.ToInt32(defaultValue)));
                }
                else
                {
                    result = (T)Convert.ChangeType(input, typeof(T));
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }

            return result;
        }
        #endregion

        #region String

        public static string SafeSubstring(this string value, int startIndex, int length)
        {
            return new string((value ?? string.Empty).Skip(startIndex).Take(length).ToArray());
        }

        #endregion

    }
}
