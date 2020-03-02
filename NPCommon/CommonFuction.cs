using FadeFox.Text;
using NPCommon.IO;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NPCommon
{
    public class CommonFuction
    {
        public struct SYSTEMTIME
        {
            [MarshalAs(UnmanagedType.U2)]
            public short Year;
            [MarshalAs(UnmanagedType.U2)]
            public short Month;
            [MarshalAs(UnmanagedType.U2)]
            public short DayOfWeek;
            [MarshalAs(UnmanagedType.U2)]
            public short Day;
            [MarshalAs(UnmanagedType.U2)]
            public short Hour;
            [MarshalAs(UnmanagedType.U2)]
            public short Minute;
            [MarshalAs(UnmanagedType.U2)]
            public short Second;
            [MarshalAs(UnmanagedType.U2)]
            public short Milliseconds;
        }

        public enum ElapsTypes
        {
            /// <summary>
            /// 일
            /// </summary>
            Days,
            /// <summary>
            /// 시간
            /// </summary>
            Hours,
            /// <summary>
            /// 분
            /// </summary>
            Minute,
            /// <summary>
            /// 입차24시간 기준 일수계산
            /// </summary>
            InDays
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetSystemTime(ref SYSTEMTIME time);

        /// <summary>
        /// system date와 time을 변경
        /// </summary>
        /// <param name="o_date"></param>
        /// <param name="o_time"></param>
        public static void SetSystemDateTime(string yyyyMMddHHmmss)
        {
            try
            {
                string o_datetime = yyyyMMddHHmmss;
                CultureInfo enUS = new CultureInfo("en-US");
                string l_datetime = DateTime.ParseExact(o_datetime, "yyyyMMddHHmmss", enUS).AddHours(-9).ToString("yyyyMMddHHmmss");
                SYSTEMTIME tmpTime = new SYSTEMTIME();

                tmpTime.Year = short.Parse(l_datetime.SafeSubstring(0, 4));
                tmpTime.Month = short.Parse(l_datetime.SafeSubstring(4, 2));
                tmpTime.Day = short.Parse(l_datetime.SafeSubstring(6, 2));
                tmpTime.Hour = short.Parse(l_datetime.SafeSubstring(8, 2));
                tmpTime.Minute = short.Parse(l_datetime.SafeSubstring(10, 2));
                tmpTime.Second = short.Parse(l_datetime.SafeSubstring(12, 2));
                tmpTime.Milliseconds = (short)100;
                SetSystemTime(ref tmpTime);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CommonFuction|SetSystemDateTime", "예외사항:" + ex.ToString());
                //조용히 처리
            }
        }
        /// <summary>
        /// db에서 시간을 읽어온다. yyyymmddHHmmss 형식으로
        /// </summary>
        /// <returns></returns>
        public static string returnSqlyyyymmddHHMMss()
        {
            string sql = "select replace(        replace(        replace(convert(varchar(19), getdate(), 126),        '-',''),        'T',''),        ':','') ";
            return sql;
        }

        /// <summary>
        /// 시간리턴
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="YYMMDD"></param>
        /// <param name="HHMMSS"></param>
        /// <returns></returns>
        public static long ElpaseMinute(string startDate, string YYMMDD, string HHMMSS)
        {
            string _startDate = startDate.Replace("-", "").Replace(":", "").Replace(" ", "").Trim();
            int Years = Convert.ToInt32(_startDate.SafeSubstring(0, 4));
            int month = Convert.ToInt32(_startDate.SafeSubstring(4, 2));
            int day = Convert.ToInt32(_startDate.SafeSubstring(6, 2));
            int hour = Convert.ToInt32(_startDate.SafeSubstring(8, 2));
            int minute = Convert.ToInt32(_startDate.SafeSubstring(10, 2));
            int second = 0;
            string _enddata = (YYMMDD + HHMMSS).Replace(":", "").Replace("-", "").Replace(" ", "").Trim();

            int EndYears = Convert.ToInt32(_enddata.SafeSubstring(0, 4));
            int Endmonth = Convert.ToInt32(_enddata.SafeSubstring(4, 2));
            int Endday = Convert.ToInt32(_enddata.SafeSubstring(6, 2));
            int Endhour = Convert.ToInt32(_enddata.SafeSubstring(8, 2));
            int Endminute = Convert.ToInt32(_enddata.SafeSubstring(10, 2));
            int Endsecond = 0;

            DateTime oldDate = new DateTime(Years, month, day, hour, minute, second);
            DateTime newDate = new DateTime(EndYears, Endmonth, Endday, Endhour, Endminute, Endsecond);
            TimeSpan ts = newDate - oldDate;
            int dirrednceInMinute = Convert.ToInt32(ts.TotalMinutes);
            if (dirrednceInMinute <= 0)
            {
                dirrednceInMinute = 1;
            }
            long _ParkTime = Convert.ToInt64(dirrednceInMinute);

            return _ParkTime;
        }
        
        public static int ElpaseType(ElapsTypes pElapsTypes, string startDate, string YYMMDD, string HHMMSS)
        {
            string _startDate = startDate.Replace("-", "").Replace(":", "").Replace(" ", "").Trim();
            int Years = Convert.ToInt32(_startDate.SafeSubstring(0, 4));
            int month = Convert.ToInt32(_startDate.SafeSubstring(4, 2));
            int day = Convert.ToInt32(_startDate.SafeSubstring(6, 2));
            int hour = Convert.ToInt32(_startDate.SafeSubstring(8, 2));
            int minute = Convert.ToInt32(_startDate.SafeSubstring(10, 2));
            int second = 0;
            string _enddata = (YYMMDD + HHMMSS).Replace(":", "").Replace("-", "").Replace(" ", "").Trim();

            int EndYears = Convert.ToInt32(_enddata.SafeSubstring(0, 4));
            int Endmonth = Convert.ToInt32(_enddata.SafeSubstring(4, 2));
            int Endday = Convert.ToInt32(_enddata.SafeSubstring(6, 2));
            int Endhour = Convert.ToInt32(_enddata.SafeSubstring(8, 2));
            int Endminute = Convert.ToInt32(_enddata.SafeSubstring(10, 2));
            int Endsecond = 0;

            DateTime oldDate = new DateTime(Years, month, day, hour, minute, second);
            DateTime newDate = new DateTime(EndYears, Endmonth, Endday, Endhour, Endminute, Endsecond);
            TimeSpan ts = newDate - oldDate;
            int returnTIme = 0;
            switch (pElapsTypes)
            {
                case ElapsTypes.Days:
                    returnTIme = Convert.ToInt32(ts.Days);
                    if (newDate.ToString("yyyyMMdd") != oldDate.ToString("yyyyMMdd"))
                    {
                        // 입차시간보다 출차시간이 시간상 숫자가 적으면
                        if (Convert.ToInt32(newDate.ToString("HHmmss")) < Convert.ToInt32(oldDate.ToString("HHmmss")))
                        {
                            returnTIme = returnTIme + 1;
                        }
                    }
                    break;
                case ElapsTypes.Hours:
                    returnTIme = Convert.ToInt32(ts.Hours);
                    break;
                case ElapsTypes.Minute:
                    returnTIme = Convert.ToInt32(ts.TotalMinutes);
                    break;
                case ElapsTypes.InDays:
                    returnTIme = Convert.ToInt32(ts.Days);
                    break;

            }
            if (returnTIme <= 0)
            {
                if (pElapsTypes == ElapsTypes.Minute)
                {
                    returnTIme = 1;
                }
            }

            return returnTIme;
        }

        public static int CalculateParktime(string p_inYmd, string p_inHMS, string p_OutYmd, string p_OutHms)
        {
            string InYmd = p_inYmd.Replace("-", "");
            string InHms = p_inHMS.Replace(":", "");
            string OutYmd = p_OutYmd.Replace("-", "");
            string OutHms = p_OutHms.Replace(":", "");

            return Convert.ToInt32(ElpaseMinute(InYmd + InHms, OutYmd, OutHms));
        }

        /// <summary>
        /// 날짜변환 yyyymmddHHmmss로 변환시킨다
        /// </summary>
        /// <param name="pDateString"></param>
        /// <returns></returns>
        private static string ConvertYYYMMDD(string pDateString)
        {
            return pDateString.Replace("-", "").Replace(" ", "").Replace(":", "");
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            //if (!typeof(T).IsSerializable)
            //{
            //    throw new ArgumentException("The type must be serializable.", "source");
            //}

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }


            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
