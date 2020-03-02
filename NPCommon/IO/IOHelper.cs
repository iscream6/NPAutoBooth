using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace NPCommon.IO
{
    public static class IOHelper
    {
        public static T AsSerializable<T>(this byte[] value, int start = 0) where T : ISerializable, new()
        {
            using (MemoryStream ms = new MemoryStream(value, start, value.Length - start, false))
            using (BinaryReader reader = new BinaryReader(ms, Encoding.Default))
            {
                return reader.ReadSerializable<T>();
            }
        }

        public static ISerializable AsSerializable(this byte[] value, Type type)
        {
            if (!typeof(ISerializable).GetTypeInfo().IsAssignableFrom(type))
                throw new InvalidCastException();
            ISerializable serializable = (ISerializable)Activator.CreateInstance(type);
            using (MemoryStream ms = new MemoryStream(value, false))
            using (BinaryReader reader = new BinaryReader(ms, Encoding.Default))
            {
                serializable.Deserialize(reader);
            }
            return serializable;
        }

        /// <summary>
        /// 현재 스트림에서 length 만큼 읽고 스트림의 현재 위치를 해당 바이트 수 만큼 이동합니다.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ReadFixedString(this BinaryReader reader, int length)
        {
            byte[] data = reader.ReadBytes(length);
            return Encoding.Default.GetString(data.TakeWhile(p => p != 0).ToArray());
        }

        public static T ReadSerializable<T>(this BinaryReader reader) where T : ISerializable, new()
        {
            T obj = new T();
            obj.Deserialize(reader);
            return obj;
        }
        
        public static byte[] ToArray(this ISerializable value)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms, Encoding.Default))
            {
                value.Serialize(writer);
                writer.Flush();
                return ms.ToArray();
            }
        }

        public static void Write(this BinaryWriter writer, ISerializable value)
        {
            value.Serialize(writer);
        }

        /// <summary>
        /// 문자열의 마지막 끝에 char 배열을 추가한다.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string Insert(this string str, char[] c)
        {
            return str.Insert(str.Length, new String(c));
        }

        /// <summary>
        /// 문자열에서 byte 에 해당하는 Char를 모두 제거한다.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string RemoveVarChar(this string str, byte b)
        {
            char c = Convert.ToChar(b);
            var rtnStr = new String(str.Where(p => p != c).ToArray());
            return rtnStr;
        }

        public static string SafeSubstring(this string value, int startIndex, int length)
        {
            return new string((value ?? string.Empty).Skip(startIndex).Take(length).ToArray());
        }

        public static byte[] GetBytes(this string value)
        {
            return Encoding.Default.GetBytes(value);
        }
    }
}
