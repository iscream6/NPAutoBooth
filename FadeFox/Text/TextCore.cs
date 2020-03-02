using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace FadeFox.Text
{
    public class TextCore
    {
        protected static Regex mRegStartTagBeforeBlank = new Regex("\\s+<");
        protected static Regex mRegEndTagBeforeBlank = new Regex(">\\s+");
        protected static Regex mRegScriptTag = new Regex("<(no)?script[^>]*>.*?</(no)?script>");
        protected static Regex mRegStyleTag = new Regex("<style[^>]*>.*?</style>");
        protected static Regex mRegCommentTag = new Regex("<!--.*?-->");
        protected static Regex mRegTag = new Regex("<(\"[^\"]*\"|\'[^\']*\'|[^\'\">])*>"); //<.*?>
        protected static Regex mRegWhiteSpace = new Regex("\\s\\s+");

        public static string RemoveNumberFromString(string pString)
        {
            return pString.Replace("0", "").Replace("1", "").Replace("2", "").Replace("3", "").Replace("4", "").Replace("5", "").Replace("6", "").Replace("7", "").Replace("8", "").Replace("9", "");
        }

        public static string RemoveWhiteSpaceFromHTML(string pHtml)
        {
            string result = pHtml.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("\t", " ").Replace("&nbsp;", " ");

            result = mRegStartTagBeforeBlank.Replace(result, "<");
            result = mRegEndTagBeforeBlank.Replace(result, ">");

            result = mRegWhiteSpace.Replace(result, " ");

            return result;
        }

        public static string RemoveTagFromHTML(string pHtml)
        {
            string result = mRegCommentTag.Replace(pHtml, "");
            result = mRegScriptTag.Replace(result, "");
            result = mRegStyleTag.Replace(result, "");
            return mRegTag.Replace(result, "");
        }

        public static string Left(string pValue, int pLength)
        {
            if (pValue.Length > pLength)
                return pValue.Substring(0, pLength);
            else
                return pValue;
        }

        public static string Right(string pValue, int pLength)
        {
            if (pValue.Length > pLength)
                return pValue.Substring(pValue.Length - pLength, pLength);
            else
                return pValue;
        }

        /// <summary>
        /// 일정공간 가져오기
        /// </summary>
        /// <param name="input">입력값</param>
        /// <param name="start">시작값</param>
        /// <param name="end">마지막값</param>
        /// <returns>걸러진 공간</returns>
        public static string SubString(string pInput, string pStart, string pEnd, bool pFront)
        {
            int start = 0;
            int end = 0;

            start = pInput.IndexOf(pStart);

            if (!pFront)
                end = pInput.LastIndexOf(pEnd);
            else
                end = pInput.IndexOf(pEnd);

            return pInput.Substring(start, end - start);
        }

        public static string SubString(string pInput, string pStart, string pEnd)
        {
            return SubString(pInput, pStart, pEnd, false);
        }

        /// <summary>
        /// 입력된 문자열이 숫자로만 되어 있는지 아닌지 검사. 소수점 포함. -포함
        /// </summary>
        /// <param name="pValue"></param>
        /// <returns></returns>
        /// 
        public static bool IsNumeric(object pValue)
        {
            return pValue is int || pValue is double || pValue is float || pValue is decimal;
        }

        public static bool IsNumeric(char ch)
        {
            if (0x30 <= ch && ch <= 0x39)
                return true;
            else
                return false;
        }

        public static bool IsNumeric(string pValue)
        {
            if (pValue == string.Empty)
                return false;

            bool usingDot = false;

            for (int i = 0; i < pValue.Length; i++)
            {
                if (char.IsDigit(pValue[i]))
                    continue;
                else if (pValue[i] == '-')
                {
                    if (i == 0)
                        continue;
                    else
                        return false;
                }
                else if (pValue[i] == '.')
                {
                    if (usingDot)
                        return false;
                    else
                    {
                        usingDot = true;
                        continue;
                    }
                }
                else
                {
                    return false;     // 문자일때 false
                }
            }

            return true;          //숫자일때 true;
        }

        public static bool IsInt(string pString)
        {
            int convertValue;
            return int.TryParse(pString, out convertValue);
        }

        public static bool IsLong(string pString)
        {
            long convertValue;
            return long.TryParse(pString, out convertValue);
        }

        public static bool IsFloat(string pString)
        {
            float convertValue;
            return float.TryParse(pString, out convertValue);
        }

        public static bool IsDouble(string pString)
        {
            double convertValue;
            return double.TryParse(pString, out convertValue);
        }

        public static bool IsDecimal(string pString)
        {
            decimal convertValue;
            return decimal.TryParse(pString, out convertValue);
        }

        public static bool IsDateTime(string pString)
        {
            DateTime convertValue;
            return DateTime.TryParse(pString, out convertValue);
        }

        public static bool IsByte(string pString)
        {
            byte convertValue;
            return byte.TryParse(pString, out convertValue);
        }

        public static bool IsChar(string pString)
        {
            char convertValue;
            return char.TryParse(pString, out convertValue);
        }

        public static bool IsSByte(string pString)
        {
            sbyte convertValue;
            return sbyte.TryParse(pString, out convertValue);
        }

        public static bool IsShort(string pString)
        {
            short convertValue;
            return short.TryParse(pString, out convertValue);
        }

        public static bool IsUShort(string pString)
        {
            ushort convertValue;
            return ushort.TryParse(pString, out convertValue);
        }

        public static bool IsUInt(string pString)
        {
            uint convertValue;
            return uint.TryParse(pString, out convertValue);
        }

        public static bool IsULong(string pString)
        {
            ulong convertValue;
            return ulong.TryParse(pString, out convertValue);
        }

        public static bool IsAlphaNumeric(string str)
        {
            Regex regex = new Regex("[^a-zA-Z0-9]");
            return !regex.IsMatch(str);
        }

        public static bool IsHangul(char ch)
        {
            //( 한글자 || 자음 , 모음 )
            if ((0xAC00 <= ch && ch <= 0xD7A3) || (0x3131 <= ch && ch <= 0x318E))
                return true;
            else
                return false;
        }

        public static bool IsHangul(string ch)
        {
            if (ch.Length < 1) return false;

            return IsHangul(ch[0]);
        }

        public static bool IsEnglish(char ch)
        {
            if ((0x61 <= ch && ch <= 0x7A) || (0x41 <= ch && ch <= 0x5A))
                return true;
            else
                return false;
        }

        public static bool IsEnglish(string ch)
        {
            if (ch.Length < 1) return false;

            return IsEnglish(ch[0]);
        }

        // byte배열의 값을 값 그대로 문자열로 변형하여 리턴
        public static string ToHexString(byte[] pValue)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("0x");

            foreach (Byte bt in pValue)
                sb.AppendFormat("{0:X2}", bt);

            return sb.ToString();
        }

        public static string ToHexString(byte pValue)
        {
            return string.Format("0x{0:X2}", pValue);
        }

        public static string ToHexString(Object pValue)
        {
            byte[] bt = pValue as byte[];

            if (bt == null)
                return string.Empty;
            else
                return ToHexString(bt);
        }

        public static string HexStringToDecodeASCIIString(string pValue)
        {
            string result = "";

            foreach (Match m in (new Regex(@"[0-9A-F]{2,2}", RegexOptions.IgnoreCase)).Matches(pValue))
                result += ((char)Convert.ToInt32(m.Value, 16)).ToString();

            return result;
        }

        public static string ToCommaString(int pValue)
        {
            return ToCommaStringNoChecking(pValue.ToString(), 0);
        }

        public static string ToCommaString(int pValue, int pPoint)
        {
            return ToCommaStringNoChecking(pValue.ToString(), pPoint);
        }

        public static string ToCommaString(long pValue)
        {
            return ToCommaStringNoChecking(pValue.ToString(), 0);
        }

        public static string ToCommaString(long pValue, int pPoint)
        {
            return ToCommaStringNoChecking(pValue.ToString(), pPoint);
        }

        public static string ToCommaString(decimal pValue)
        {
            return ToCommaStringNoChecking(pValue.ToString(), 0);
        }

        public static string ToCommaString(decimal pValue, int pPoint)
        {
            return ToCommaStringNoChecking(pValue.ToString(), pPoint);
        }

        public static string ToCommaString(float pValue)
        {
            return ToCommaStringNoChecking(pValue.ToString(), 0);
        }

        public static string ToCommaString(float pValue, int pPoint)
        {
            return ToCommaStringNoChecking(pValue.ToString(), pPoint);
        }

        public static string ToCommaString(double pValue)
        {
            return ToCommaStringNoChecking(pValue.ToString(), 0);
        }

        public static string ToCommaString(double pValue, int pPoint)
        {
            return ToCommaStringNoChecking(pValue.ToString(), pPoint);
        }

        public static string ToCommaString(string pValue)
        {
            return ToCommaString(pValue, 0);
        }

        public static string ToCommaString(string pValue, int pPoint)
        {
            if (IsNumeric(pValue))
            {
                decimal value = decimal.Parse(pValue);
                string format = string.Empty;

                if (pPoint == 0)
                    format = "{0:#,0}";
                else if (pPoint > 0)
                {
                    format = "{0:#,0.";

                    for (int i = 0; i < pPoint; i++)
                        format = format + "0";

                    format = format + "}";
                }
                else
                    format = "{0:#,0}";

                return string.Format(format, value);
            }
            else
                return pValue;
        }

        private static string ToCommaStringNoChecking(string pValue, int pPoint)
        {
            decimal value = decimal.Parse(pValue);
            string format = string.Empty;

            if (pPoint == 0)
                format = "{0:#,0}";
            else if (pPoint > 0)
            {
                format = "{0:#,0.";

                for (int i = 0; i < pPoint; i++)
                    format = format + "0";

                format = format + "}";
            }
            else
                format = "{0:#,0}";

            return string.Format(format, value);
        }

        public static string ToDateString(Object pValue)
        {
            return ToDateString(pValue, "-");
        }

        public static string ToDateString(Object pValue, string pDiv)
        {
            if (pValue is string)
            {
                string value = pValue as string;
                return ToDateString(value, pDiv);
            }

            if (pValue is DateTime)
            {
                DateTime value = Convert.ToDateTime(pValue);
                return ToDateString(value, pDiv);
            }

            return string.Empty;
        }

        public static string ToDateString(string pValue)
        {
            return ToDateString(pValue, "-");
        }

        public static string ToDateString(string pValue, string pDiv)
        {
            string result = pValue;
            DateTime currentDate = DateTime.Now;

            string currentYear = currentDate.ToString("yyyy");
            string currentMonth = currentDate.ToString("MM");

            switch (pValue.Length)
            {
                case 1: // 1
                    result = currentYear + pDiv + currentMonth + pDiv + "0" + pValue;
                    break;
                case 2:  // 01
                    result = currentYear + pDiv + currentMonth + pDiv + pValue;
                    break;
                case 4:  // 0301
                    result = currentYear + pDiv + pValue.Substring(0, 2) + pDiv + pValue.Substring(2, 2);
                    break;
                case 6: // 080301
                    result = "20" + pValue.Substring(0, 2) + pDiv + pValue.Substring(2, 2) + pDiv + pValue.Substring(4, 2);
                    break;
                case 8:  // 20080301
                    result = pValue.Substring(0, 4) + pDiv + pValue.Substring(4, 2) + pDiv + pValue.Substring(6, 2);
                    break;
                case 10: // 2008-03-01
                    result = pValue.Substring(0, 4) + pDiv + pValue.Substring(5, 2) + pDiv + pValue.Substring(8, 2);
                    break;
                case 14: // 20110620192910
                    result = pValue.Substring(0, 4) + pDiv + pValue.Substring(4, 2) + pDiv + pValue.Substring(6, 2) + " " + pValue.Substring(8, 2) + ":" + pValue.Substring(10, 2) + ":" + pValue.Substring(12, 2);
                    break;
            }

            return result;
        }

        public static string ToDateString(DateTime pValue)
        {
            return ToDateString(pValue, "-");
        }

        public static string ToDateString(DateTime pValue, string pDiv)
        {
            return pValue.ToString("yyyy" + pDiv + "MM" + pDiv + "dd");
        }

        public static string ByteToString(byte[] pValue)
        {
            return System.Text.Encoding.Default.GetString(pValue);
        }

        public static byte[] StringToByte(string pValue)
        {
            return System.Text.Encoding.Default.GetBytes(pValue);
        }

        public static string 음력으로변환(DateTime dt)
        {
            int 윤월;
            int 음력년, 음력월, 음력일;
            bool 윤달체크 = false;

            System.Globalization.KoreanLunisolarCalendar 음력 = new System.Globalization.KoreanLunisolarCalendar();

            음력년 = 음력.GetYear(dt);
            음력월 = 음력.GetMonth(dt);
            음력일 = 음력.GetDayOfMonth(dt);

            if (음력.GetMonthsInYear(음력년) > 12)             //1년이 12이상이면 윤달이 있음..
            {
                윤달체크 = 음력.IsLeapMonth(음력년, 음력월);     //윤월인지
                윤월 = 음력.GetLeapMonth(음력년);             //년도의 윤달이 몇월인지?
                if (음력월 >= 윤월) 음력월--;    //달이 윤월보다 같거나 크면 -1을 함 즉 윤8은->9 이기때문
            }

            string temp = 음력년.ToString() + "-" + (윤달체크 ? "*" : "");
            temp += 음력월.ToString() + "-" + 음력일.ToString();
            return temp;
        }

        public static string 음력으로변환(int 양력년, int 양력월, int 양력일)
        {
            DateTime Temp = new DateTime(양력년, 양력월, 양력일); //양력

            return 음력으로변환(Temp);
        }

        public static DateTime 양력으로변환(int 음력년, int 음력월, int 음력일, bool 달)
        {
            int 윤월;

            System.Globalization.KoreanLunisolarCalendar 음력 = new System.Globalization.KoreanLunisolarCalendar();

            if (음력.GetMonthsInYear(음력년) > 12)
            {
                윤월 = 음력.GetLeapMonth(음력년);

                if (달) 음력월++;

                if (음력월 > 윤월) 음력월++;
            }

            return 음력.ToDateTime(음력년, 음력월, 음력일, 0, 0, 0, 0);
        }


        public static string ConvertChar(string pStr)
        {
            return pStr.Replace("'", "''");
        }

        public static string ConvertStr(object pStr)
        {
            string str = pStr as string;

            return (str == null ? string.Empty : str);
        }

        public static string EncodeChar(string pStr)
        {
            return pStr.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&#39;").Replace("\"", "&quot;");
        }

        public static string DecodeChar(string pStr)
        {
            return pStr.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&#39;", "'").Replace("&quot;", "\"").Replace("&amp;", "&");
        }

        /// <summary>
        /// 입력된 문자열을 입력된 공백 안에서 오른쪽 정렬, 한글은 2자리로 계산
        /// </summary>
        /// <param name="pLength"></param>
        /// <param name="pString"></param>
        /// <returns></returns>
        /// 
        public static string ToRightAlignString(int pLength, string pString)
        {
            return ToRightAlignString(pLength, pString, ' ');
        }

        public static string ToRightAlignString(int pLength, string pString, char pChar)
        {
            int lengthB = LengthB(pString);

            if (pLength <= lengthB)
                return pString;
            else
                return Repeat(pLength - lengthB, pChar) + pString;
        }

        /// <summary>
        /// 입력된 문자열을 입력된 공간 안에서 왼쪽 정렬, 한글은 2자리로 계산
        /// </summary>
        /// <param name="pLength"></param>
        /// <param name="pString"></param>
        /// <returns></returns>
        /// 
        public static string ToLeftAlignString(int pLength, string pString)
        {
            return ToLeftAlignString(pLength, pString, ' ');
        }

        public static string ToLeftAlignString(int pLength, string pString, char pChar)
        {
            int lengthB = LengthB(pString);

            if (pLength <= lengthB)
                return pString;
            else
                return pString + Repeat(pLength - lengthB, pChar);
        }

        /// <summary>
        /// 입력된 길이 만큼 공백을 만들어 리턴
        /// </summary>
        /// <param name="pSpaceLength"></param>
        /// <returns></returns>
        public static string Space(int pSpaceLength)
        {
            return Repeat(pSpaceLength, ' ');
        }

        /// <summary>
        /// 입력된 문자를 반복해서 문자열로 만들어 리턴
        /// </summary>
        /// <param name="pCount">반복횟수</param>
        /// <param name="pString">반복할 문자</param>
        /// <returns></returns>
        public static string Repeat(int pCount, char pChar)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < pCount; i++)
            {
                sb.Append(pChar);
            }

            return sb.ToString();
        }

        public static int LengthB(string pString)
        {
            if (pString == null)
                return 0;

            if (pString.Length == 0)
                return 0;
            else
                return System.Text.Encoding.Default.GetByteCount(pString);
        }

        public static string rPad(string originString, int totalLength, string padString)
        {
            if (originString.Length > totalLength)
                return originString.Substring(0, totalLength);

            string resultString = originString;
            int padLength = totalLength - originString.Length;
            for (int i = 0; i < padLength; i++)
            {
                resultString = resultString + padString;
            }

            return resultString;
        }

        public static string rPad(string originString, int totalLength, byte padByte)
        {
            if (originString.Length > totalLength)
                return originString.Substring(0, totalLength);

            string resultString = originString;
            int padLength = totalLength - originString.Length;
            var padChar = Convert.ToChar(padByte);
            for (int i = 0; i < padLength; i++)
            {
                resultString += padChar;
            }
            return resultString;
        }

        public static string lPad(string originString, int totalLength, string padString)
        {
            if (originString.Length > totalLength)
                return originString.Substring(0, totalLength);

            string resultString = originString;
            int padLength = totalLength - originString.Length;
            for (int i = 0; i < padLength; i++)
            {
                resultString = padString + resultString;
            }

            return resultString;
        }

        public static string lPad(string originString, int totalLength, byte padByte)
        {
            if (originString.Length > totalLength)
                return originString.Substring(0, totalLength);

            string resultString = originString;
            int padLength = totalLength - originString.Length;
            var padChar = Convert.ToChar(padByte);
            for (int i = 0; i < padLength; i++)
            {
                resultString = padChar + resultString;
            }

            return resultString;
        }

        public static string StringToByte(byte[] data)
        {
            string returnString = "";
            for (int i = 0; i < data.Length; i++)
            {
                returnString += lPad((data[i].ToString("X")), 2, "0");

            }
            return returnString;
        }

        public static byte[] Dec2Hexa(int dValue)
        {
            String s = String.Empty;
            String[] sN = new String[4];

            s = String.Format("{0:X}", dValue).PadLeft(8, '0');
            byte[] b = new byte[4];
            int n = 0;
            for (int i = 0; i < s.Length; i += 2)
            {
                b[n++] = HH2D(s.Substring(i, 2));
            }

            return b;
        }

        private static byte HH2D(String s)
        {
            int n = 0;
            int r = 0;
            byte rr = 0;
            for (int i = 0; i < s.Length; i++)
            {
                switch (s.Substring(i, 1))
                {
                    case "0": r = 0; break;
                    case "1": r = 1; break;
                    case "2": r = 2; break;
                    case "3": r = 3; break;
                    case "4": r = 4; break;
                    case "5": r = 5; break;
                    case "6": r = 6; break;
                    case "7": r = 7; break;
                    case "8": r = 8; break;
                    case "9": r = 9; break;
                    case "A": r = 10; break;
                    case "B": r = 11; break;
                    case "C": r = 12; break;
                    case "D": r = 13; break;
                    case "E": r = 14; break;
                    case "F": r = 15; break;
                }
                if (i == 0)
                    n = r * 16;
                else
                    n += r;
            }

            rr = (byte)n;
            return rr;
        }


        public static int HexaToDecimal(byte[] b)
        {
            int value = 0;
            int length = b.Length;
            for (int i = 0; i < length; i++)
            {
                int shift = (length - 1 - i) * 8;
                value += (b[i] & 0x000000FF) << shift;
            }
            return value;
;
        }

        public static int BcdToInt(byte[] bcd)
        {
            return Convert.ToInt32(BcdToLong(bcd));
        }

        public static long BcdToLong(byte[] bcd)
        {
            long result = 0;

            foreach (byte b in bcd)
            {
                int digit1 = b >> 4;
                int digit2 = b & 0x0f;

                result = (result * 100) + (digit1 * 10) + digit2;
            }

            return result;
        }

        public static byte[] ToBCD(int num, int byteCount)
        {
            return ToBCD<int>(num, byteCount);
        }

        public static byte[] ToBCD(long num, int byteCount)
        {
            return ToBCD<long>(num, byteCount);
        }

        private static byte[] ToBCD<T>(T num, int byteCount) where T : struct, IConvertible
        {
            long val = Convert.ToInt64(num);

            byte[] bcdNumber = new byte[byteCount];
            for (int i = 1; i <= byteCount; i++)
            {
                long mod = val % 100;

                long digit2 = mod % 10;
                long digit1 = (mod - digit2) / 10;

                bcdNumber[byteCount - i] = Convert.ToByte((digit1 * 16) + digit2);

                val = (val - mod) / 100;
            }

            return bcdNumber;
        }

        /// <summary>
        /// 한글이 들어가있는지 확인. false 면 한글이 아닌게 있음
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Boolean GetHangle(string message)
        {
            string Data = message.Trim();
            string patternKorea = @"[가-힣]";
            for (int i = 0; i < message.Length; i++)
            {
                if (!Regex.IsMatch(Data.Substring(i,1), patternKorea))
                {
                    return false;
                }
               
            }
            return true;

        }

        /// <summary>
        /// 숫자가 아닌게 있는지 확인 false 면 숫자 아닌값있음
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Boolean GetInteger(string message)
        {
            string Data = message.Trim();
            string patternInteger = @"[0-9]";
            for (int i = 0; i < message.Length; i++)
            {
                if (!Regex.IsMatch(Data.Substring(i, 1), patternInteger))
                {
                    return false;
                }

            }
            return true;


        }

        private static string logPath = Application.StartupPath + "\\log\\" + DateTime.Now.ToString("yyyyMMdd").Substring(0, 4) + "\\" + DateTime.Now.ToString("yyyyMMdd").Substring(4, 2) + "\\";
        public static ManualResetEvent SendLogDataSet = new ManualResetEvent(true);
        public static Boolean getWriteLogData(string p_log_data)
        {


            try
            {

                SendLogDataSet.WaitOne(50);
                SendLogDataSet.Reset();
                StreamWriter sw = new StreamWriter(logPath + MakeLogFileName(), true);
                sw.WriteLine(MakeLogDocument(p_log_data).Trim());
                sw.Close();
                sw = null;
                return true;
            }
            catch (Exception ex)
            {

                try
                {
                    if (!Directory.Exists(logYearPath))
                    {
                        Directory.CreateDirectory(logYearPath);
                    }

                }
                catch
                {
                    //  MessageDlg.showErrorDlg(ex.ToString());

                }

                try
                {
                    if (!Directory.Exists(logPath))
                    {
                        Directory.CreateDirectory(logPath);
                    }

                }
                catch
                {
                    //  MessageDlg.showErrorDlg(ex.ToString());

                }
                //  MessageDlg.showErrorDlg(ex.ToString());
                INFO(INFOS.PROGRAM_ERROR, "LogClass|getWriteLogData", ex.ToString());
                return false;
            }
            finally
            {
                SendLogDataSet.Set();
            }
        }
        
        private static string MakeLogDocument(string p_Log_Data)
        {
            string nowdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
            string returnmessage = "";
            returnmessage = "[" + nowdate + "]" + " " + p_Log_Data;
            return returnmessage;
        }
        public static string BoothID = "";
        public static string BoothName = "";

        private static string MakeLogFileName()
        {
            string nowdate = DateTime.Now.ToString("yyyyMMdd");
            string returnmessage = "";
            returnmessage = nowdate + "_" + BoothName + ".txt";
            return returnmessage;
        }
        private static string logYearPath = Application.StartupPath + "\\log\\" + DateTime.Now.ToString("yyyyMMdd").Substring(0, 4) + "\\";
        private static string logDEVICEPath = Application.StartupPath + "\\DEVICELOG\\";
        public static void makeLogFileFolder()
        {
            try
            {
                if (!Directory.Exists(logYearPath))
                {
                    Directory.CreateDirectory(logYearPath);
                }

            }
            catch
            {
                //  MessageDlg.showErrorDlg(ex.ToString());

            }

            try
            {
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

            }
            catch
            {
                //  MessageDlg.showErrorDlg(ex.ToString());

            }
            try
            {
                if (!Directory.Exists(logDEVICEPath))
                {
                    Directory.CreateDirectory(logDEVICEPath);
                }

            }
            catch
            {
                //  MessageDlg.showErrorDlg(ex.ToString());

            }
        }


        public enum DB
        {
            Search,
            Insert,
            Update
        }
        public static string DB_ACTION(DB p_DBACTIONS, string p_message, string p_message2)
        {
            switch (p_DBACTIONS)
            {
                case DB.Search:
                    getWriteLogData("DB작업|검색|" + p_message + "|" + p_message2);
                    break;
                case DB.Insert:
                    getWriteLogData("DB작업|저장|" + p_message + "|" + p_message2);
                    break;
                case DB.Update:
                    getWriteLogData("DB작업|업데이트|" + p_message + "|" + p_message2);
                    break;
            }
            return "";
        }
        public enum ACTIONS
        {
            /// <summary>
            /// 출차LPR
            /// </summary>
            LPR,
            /// <summary>
            /// 전광판
            /// </summary>
            JUNGANPAN,
            /// <summary>
            /// 지폐리더기
            /// </summary>
            BILLREADER,
            /// <summary>
            /// 지폐방출기
            /// </summary>
            BILLCHARGER,
            /// <summary>
            /// 동전방출기
            /// </summary>
            COINCHARGER,
            /// <summary>
            /// 동전방출기50
            /// </summary>
            COIN50CHARGER,
            /// <summary>
            /// 동전방출기100
            /// </summary>
            COIN100CHARGER,
            /// <summary>
            /// 동전방출기500
            /// </summary>
            COIN500CHARGER,
            /// <summary>
            /// 카드리더기
            /// </summary>
            CARDREADER,
            /// <summary>
            /// 카드리더기1
            /// </summary>
            CARDREADER1,
            /// <summary>
            /// 카드리더기2
            /// </summary>
            CARDREADER2,
            /// <summary>
            /// 교통카드
            /// </summary>
            TMONEY,
            /// <summary>
            /// 바코드
            /// </summary>
            BACODE,
            /// <summary>
            /// 경차인식기
            /// </summary>
            KUNGCHA,
            /// <summary>
            /// 동전리더기
            /// </summary>
            COINREADER,
            /// <summary>
            /// 고객
            /// </summary>
            USER,
            /// <summary>
            /// 관리자
            /// </summary>
            MANAGER,
            /// <summary>
            /// 통신
            /// </summary>
            TCPIP,
            /// <summary>
            /// 데이터베이스
            /// </summary>
            DB,
            /// <summary>
            /// 영수증프린터
            /// </summary>
            RECIPT,
            /// <summary>
            /// LED
            /// </summary>
            DIDO,
            /// <summary>
            /// 이미지서버
            /// </summary>
            IMAGESERVER,
            TOTALCONTROL,
            BARCODE,
            /// <summary>
            /// 신호관제
            /// </summary>
            SINHOGHANJE,
            /// <summary>
            /// 신분증인식기
            /// </summary>
            SINBUNREADER
        }
        public static string ACTION(ACTIONS p_ACTIONS, string p_message, string p_message2)
        {
            switch (p_ACTIONS)
            {
                case ACTIONS.LPR:
                    getWriteLogData("ACTION|LPR|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.JUNGANPAN:
                    getWriteLogData("ACTION|전광판|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.BILLREADER:
                    getWriteLogData("ACTION|지폐리더기|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.BILLCHARGER:
                    getWriteLogData("ACTION|지폐방출기|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.COINCHARGER:
                    getWriteLogData("ACTION|동전방출기|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.COIN50CHARGER:
                    getWriteLogData("ACTION|동전방출기50|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.COIN100CHARGER:
                    getWriteLogData("ACTION|동전방출기100|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.COIN500CHARGER:
                    getWriteLogData("ACTION|동전방출기500|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.CARDREADER:
                    getWriteLogData("ACTION|카드리더기|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.CARDREADER1:
                    getWriteLogData("ACTION|카드리더기1|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.CARDREADER2:
                    getWriteLogData("ACTION|카드리더기2|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.TMONEY:
                    getWriteLogData("ACTION|교통카드|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.BACODE:
                    getWriteLogData("ACTION|바코드|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.COINREADER:
                    getWriteLogData("ACTION|동전리더기|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.USER:
                    getWriteLogData("ACTION|고객|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.MANAGER:
                    getWriteLogData("ACTION|관리자|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.TCPIP:
                    getWriteLogData("ACTION|통신|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.DB:
                    getWriteLogData("ACTION|DB|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.RECIPT:
                    getWriteLogData("ACTION|영수증프린터|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.DIDO:
                    getWriteLogData("ACTION|LED|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.IMAGESERVER:
                    getWriteLogData("ACTION|IMAGESERVER|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.TOTALCONTROL:
                    getWriteLogData("ACTION|통합관제|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.BARCODE:
                    getWriteLogData("ACTION | 바코드|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.KUNGCHA:
                    getWriteLogData("ACTION | 경차|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.SINHOGHANJE:
                    getWriteLogData("ACTION | 신호관제|" + p_message + "|" + p_message2);
                    break;
                case ACTIONS.SINBUNREADER:
                    getWriteLogData("ACTION | 신분증리더기|" + p_message + "|" + p_message2);
                    break;
            }
            return "";
        }
        public enum INFOS
        {
            /// <summary>
            /// 차량검색
            /// </summary>
            CARSEARCH,
            /// <summary>
            /// 차량정보
            /// </summary>
            CARINFO,
            /// <summary>
            /// 요금변동
            /// </summary>
            PAYINFO,
            /// <summary>
            /// 프로그램 에러
            /// </summary>
            PROGRAM_ERROR,
            /// <summary>
            /// 프로그램 정보
            /// </summary>
            PROGRAM_INFO,
            /// <summary>
            /// 메모리정보
            /// </summary>
            MEMORY,
            /// <summary>
            /// 어떤일을 진행
            /// </summary>
            PROGRAM_ING,
            /// <summary>
            /// 방출관련
            /// </summary>
            CHARGE,
            /// <summary>
            /// 할인권 / 카드관련
            /// </summary>
            CARD_TICKET,
            /// <summary>
            /// 할인권관련
            /// </summary>
            DISCOUNTTICEKT,
            /// <summary>
            /// 할인권에러
            /// </summary>
            DISCOUNTTICEKT_ERRPR,
            /// <summary>
            /// 할인권결재성공
            /// </summary>
            DISCOUNTTICEKT_SUCCESS,
            /// <summary>
            /// 카드관련
            /// </summary>
            CARD,
            /// <summary>
            /// 카드 결제성공
            /// </summary>
            CARD_SUCCESS,
            /// <summary>
            /// 카드 결제실패
            /// </summary>
            CARD_ERRPR,
            /// <summary>
            /// OCS할인권관련
            /// </summary>
            OCS,
            /// <summary>
            /// OCS결제에러
            /// </summary>
            OCS_ERRPR,
            /// <summary>
            /// OCS결제성공
            /// </summary>
            OCS_SUCCES,
            /// <summary>
            /// 현금영수증
            /// </summary>
            CASHRECEIPT,
            /// <summary>
            /// 현금영수증 결제에러
            /// </summary>
            CASHRECEIPT_ERRPR,
            /// <summary>
            /// 현금영수증 결제성공
            /// </summary>
            CASHRECEIPT_SUCCES,
            CARD_FAIL,
            PARKINGTICKET,
            /// <summary>
            /// 신분증 인식 성공
            /// </summary>
            SINBUN_SUCCES,
            /// <summary>
            /// 신분증 인식 실패
            /// </summary>
            SINBUN_FAIL

        }

        public static string INFO(INFOS p_INFO, string p_message, string p_message2)
        {
            switch (p_INFO)
            {
                case INFOS.CARSEARCH:
                    getWriteLogData("INFO|차량검색|" + p_message + "|" + p_message2);
                    break;
                case INFOS.CARINFO:
                    getWriteLogData("INFO|차량정보|" + p_message + "|" + p_message2);
                    break;
                case INFOS.PAYINFO:
                    getWriteLogData("INFO|요금|" + p_message + "|" + p_message2);
                    break;
                case INFOS.PROGRAM_ERROR:
                    getWriteLogData("INFO|프로그램에러|" + p_message + "|" + p_message2);
                    break;
                case INFOS.PROGRAM_INFO:
                    getWriteLogData("INFO|프로그램정보|" + p_message + "|" + p_message2);
                    break;
                case INFOS.MEMORY:
                    getWriteLogData("INFO|MEMORY|" + p_message + "|" + p_message2);
                    break;
                case INFOS.PROGRAM_ING:
                    getWriteLogData("INFO|프로그램진행처리|" + p_message + "|" + p_message2);
                    break;
                case INFOS.CHARGE:
                    getWriteLogData("INFO|요금방출|" + p_message + "|" + p_message2);
                    break;
                case INFOS.CARD_TICKET:
                    getWriteLogData("INFO|카드/할인권|" + p_message + "|" + p_message2);
                    break;
                case INFOS.DISCOUNTTICEKT:
                    getWriteLogData("INFO|할인권|" + p_message + "|" + p_message2);
                    break;
                case INFOS.DISCOUNTTICEKT_SUCCESS:
                    getWriteLogData("INFO|할인권결제|" + p_message + "|" + p_message2);
                    break;

                case INFOS.DISCOUNTTICEKT_ERRPR:
                    getWriteLogData("INFO|할인권에러|" + p_message + "|" + p_message2);
                    break;
                case INFOS.CARD:
                    getWriteLogData("INFO|카드|" + p_message + "|" + p_message2);
                    break;
                case INFOS.CARD_SUCCESS:
                    getWriteLogData("INFO|카드결제|" + p_message + "|" + p_message2);
                    break;
                case INFOS.CARD_ERRPR:
                    getWriteLogData("INFO|카드결제에러|" + p_message + "|" + p_message2);
                    break;
                case INFOS.OCS:
                    getWriteLogData("INFO|OCS|" + p_message + "|" + p_message2);
                    break;
                case INFOS.OCS_ERRPR:
                    getWriteLogData("INFO|OCS에러|" + p_message + "|" + p_message2);
                    break;
                case INFOS.OCS_SUCCES:
                    getWriteLogData("INFO|OCS결제|" + p_message + "|" + p_message2);
                    break;
                case INFOS.CASHRECEIPT:
                    getWriteLogData("INFO|현금영수증|" + p_message + "|" + p_message2);
                    break;
                case INFOS.CASHRECEIPT_ERRPR:
                    getWriteLogData("INFO|현금영수증에러|" + p_message + "|" + p_message2);
                    break;
                case INFOS.CASHRECEIPT_SUCCES:
                    getWriteLogData("INFO|현금영수증결제|" + p_message + "|" + p_message2);
                    break;
                case INFOS.CARD_FAIL:
                    getWriteLogData("INFO|신용카드결제실패|" + p_message + "|" + p_message2);
                    break;
                case INFOS.PARKINGTICKET:
                    getWriteLogData("INFO|주차권|" + p_message + "|" + p_message2);
                    break;
                case INFOS.SINBUN_SUCCES:
                    getWriteLogData("INFO|신분증인식성공|" + p_message + "|" + p_message2);
                    break;
                case INFOS.SINBUN_FAIL:
                    getWriteLogData("INFO|신분증인식실패|" + p_message + "|" + p_message2);
                    break;
            }
            return "";
        }

        public enum DEVICE
        {
            /// <summary>
            /// LPR
            /// </summary>
            LPR,
            /// <summary>
            /// 전광판
            /// </summary>
            JUNGANPAN,
            /// <summary>
            /// 지폐리더기
            /// </summary>
            BILLREADER,
            /// <summary>
            /// 지폐방출기5000원권 부분
            /// </summary>
            BILLCHARGER5000,
            /// <summary>
            /// 지폐방출기1000원권 부분
            /// </summary>
            BILLCHARGER1000,
            /// <summary>
            /// 지폐방출기
            /// </summary>
            BILLCHARGER,
            /// <summary>
            /// 동전방출기
            /// </summary>
            COINCHARGER,
            /// <summary>
            /// 동전방출기50
            /// </summary>
            COIN50CHARGER,
            /// <summary>
            /// 동전방출기100
            /// </summary>
            COIN100CHARGER,
            /// <summary>
            /// 동전방출기500
            /// </summary>
            COIN500CHARGER,
            /// <summary>
            /// 카드리더기1
            /// </summary>
            CARDREADER1,
            /// <summary>
            /// 카드리더기2
            /// </summary>
            CARDREADER2,
            /// <summary>
            /// 교통카드
            /// </summary>
            TMONEY,
            /// <summary>
            /// 바코드
            /// </summary>
            BACODE,
            /// <summary>
            /// 경차인식기
            /// </summary>
            KUNGCHA,
            /// <summary>
            /// 동전리더기
            /// </summary>
            COINREADER,
            /// <summary>
            /// 통신
            /// </summary>
            TCPIP,
            /// <summary>
            /// 데이터베이스
            /// </summary>
            DB,
            /// <summary>
            /// 영수증프린터
            /// </summary>
            RECIPT,
            /// <summary>
            /// LED
            /// </summary>
            DIDO,
            /// <summary>
            /// 이미지서버
            /// </summary>
            IMAGESERVER,
            /// <summary>
            /// 음성
            /// </summary>
            TTS,
            /// <summary>
            /// 신호관제
            /// </summary>
            SINHOGHANJE

        }
        /// <summary>
        /// 장비이상시 표현
        /// </summary>
        /// <param name="p_DeviceName">장비이름</param>
        /// <param name="p_message">발생한구간</param>
        /// <param name="p_message2">발생내용</param>
        public static void DeviceError(DEVICE p_DeviceName, string p_message, string p_message2)
        {
            switch (p_DeviceName)
            {
                case DEVICE.LPR:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:LPR:" + p_message2);
                    break;
                case DEVICE.JUNGANPAN:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:전광판:" + p_message2);
                    break;
                case DEVICE.BILLREADER:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:지폐리더기:" + p_message2);
                    break;
                case DEVICE.BILLCHARGER:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:지폐방출기:" + p_message2);
                    break;
                case DEVICE.BILLCHARGER5000:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:지폐방출기5000:" + p_message2);
                    break;
                case DEVICE.BILLCHARGER1000:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:지폐방출기1000:" + p_message2);
                    break;
                case DEVICE.COINCHARGER:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:동전방출기:" + p_message2);
                    break;
                case DEVICE.COIN50CHARGER:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:동전방출기50:" + p_message2);
                    break;
                case DEVICE.COIN100CHARGER:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:동전방출기100:" + p_message2);
                    break;
                case DEVICE.COIN500CHARGER:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:동전방출기500:" + p_message2);
                    break;
                case DEVICE.CARDREADER1:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:카드리더기1:" + p_message2);
                    break;
                case DEVICE.CARDREADER2:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:카드리더기2:" + p_message2);
                    break;
                case DEVICE.TMONEY:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:교통카드:" + p_message2);
                    break;
                case DEVICE.BACODE:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:바코드:" + p_message2);
                    break;
                case DEVICE.COINREADER:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:동전리더기:" + p_message2);
                    break;
                case DEVICE.TCPIP:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:통신:" + p_message2);
                    break;
                case DEVICE.DB:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:데이터베이스:" + p_message2);
                    break;
                case DEVICE.RECIPT:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:영수증프린터:" + p_message2);
                    break;
                case DEVICE.DIDO:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:LED:" + p_message2);
                    break;
                case DEVICE.IMAGESERVER:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:IMAGESERVER:" + p_message2);
                    break;
                case DEVICE.TTS:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:음성:" + p_message2);
                    break;
                case DEVICE.KUNGCHA:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:경차인식기:" + p_message2);
                    break;
                case DEVICE.SINHOGHANJE:
                    getWriteLogData("INFO|DEVICE_ERROR|" + p_message + "|" + "장비이상:신호관제:" + p_message2);
                    break;
            }
        }

      /// <summary>  날짜사이의시간간격의숫자를반환합니다.
      /// </summary>

      /// <param name="Interval">y-m-d h:n:s:ms</param>

      /// <param name="Date1"></param>

     /// <param name="Date2"></param>

      /// <returns></returns>

        public static double DateDiff(string Interval, DateTime Date1, DateTime Date2)
        {

            double diff = 0;

            TimeSpan ts = Date2 - Date1;



            switch (Interval.ToLower())
            {

                case "y":

                    ts = DateTime.Parse(Date2.ToString("yyyy-01-01")) - DateTime.Parse(Date1.ToString("yyyy-01-01"));

                    diff = Convert.ToDouble(ts.TotalDays / 365);

                    break;

                case "m":

                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-01")) - DateTime.Parse(Date1.ToString("yyyy-MM-01"));

                    diff = Convert.ToDouble((ts.TotalDays / 365) * 12);

                    break;

                case "d":

                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-dd")) - DateTime.Parse(Date1.ToString("yyyy-MM-dd"));

                    diff = ts.Days;

                    break;

                case "h":

                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-dd HH:00:00")) - DateTime.Parse(Date1.ToString("yyyy-MM-dd HH:00:00"));

                    diff = ts.TotalHours;

                    break;

                case "n":

                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-dd HH:mm:00")) - DateTime.Parse(Date1.ToString("yyyy-MM-dd HH:mm:00"));

                    diff = ts.TotalMinutes;

                    break;

                case "s":

                    ts = DateTime.Parse(Date2.ToString("yyyy-MM-dd HH:mm:ss")) - DateTime.Parse(Date1.ToString("yyyy-MM-dd HH:mm:ss"));

                    diff = ts.TotalSeconds;

                    break;

                case "ms":

                    diff = ts.TotalMilliseconds;

                    break;

            }



            return diff;


        }

        public static int ByteSearch(byte[] searchIn, byte[] searchBytes, int start = 0)
        {
            int found = -1;
            bool matched = false;
            //only look at this if we have a populated search array and search bytes with a sensible start    
            if (searchIn.Length > 0 && searchBytes.Length > 0 && start <= (searchIn.Length - searchBytes.Length) && searchIn.Length >= searchBytes.Length)
            {        //iterate through the array to be searched        
                for (int i = start; i <= searchIn.Length - searchBytes.Length; i++)
                {
                    //if the start bytes match we will start comparing all other bytes           
                    if (searchIn[i] == searchBytes[0])
                    {
                        if (searchIn.Length > 1)
                        {
                            //multiple bytes to be searched we have to compare byte by byte                   
                            matched = true;
                            for (int y = 1; y <= searchBytes.Length - 1; y++)
                            { if (searchIn[i + y] != searchBytes[y]) { matched = false; break; } }
                            //everything matched up    
                            if (matched) { found = i; break; }
                        }
                        else
                        {
                            //search byte is only one bit nothing else to do            
                            found = i; break;
                            //stop the loop      
                        }
                    }
                }
            }
            return found;
        }


    }
}
