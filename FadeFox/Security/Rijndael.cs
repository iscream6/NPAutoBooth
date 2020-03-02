/* 
 * ==============================================================================
 *   Program ID     : 
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : sayou.net
 *   Developer      : 
 *   Create Date    : 2007-05-18
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace FadeFox.Security
{
	public class Rijndael
	{
		private string mKeyString = string.Empty;
		private string mIVString = string.Empty;
		private byte[] mKey;
		private byte[] mIV;
		private static RijndaelManaged mRij = new RijndaelManaged();
		Encoding mEc = Encoding.GetEncoding(949);

		public Rijndael(string pKey, string pIV)
		{
			mKeyString = pKey;
			mIVString = pIV;

			mKey = ConvertBytes2(mKeyString);
			mIV = ConvertBytes2(mIVString);
		}

		public Rijndael()
		{
			mKeyString = "012068032143079149192067173112188112246004169148048126133168044128120127094001028120198080089237";
			// nexpa mKeyString = "215140159063212123044080228212245106158168136026026101169103127017188061214055010205143193140051";
			mIVString = "185084245127096028149236132217250123123092192254";
			// nexpa mIVString = "155250134104247108049233022037021225245095220082";

			mKey = ConvertBytes2(mKeyString);
			mIV = ConvertBytes2(mIVString);
		}

		/// <summary>
		/// 입력된 문자열에대하여 인코딩을 하여 결과를 리턴
		/// </summary>
		/// <param name="pSourceString">인코딩을 하고자 하는 문자열</param>
		/// <returns>
		/// 인코딩한 결과 
		/// 형태는 "숫자 구분문자 숫자 구분문자 숫자 ... "의 형태의 문자열임
		/// 숫자는 0~255사이의 크기를 가지고 있음
		/// </returns>
		public string Encode(string pSourceString)
		{
			byte[] sourceBytes = mEc.GetBytes(pSourceString);
			byte[] encodedBytes = EncodeBytes(sourceBytes);

			return ConvertString2(encodedBytes);
		}

		/// <summary>
		/// 입력된 인코딩된 데이터를 디코딩하여 결과를 리턴
		/// </summary>
		/// <param name="pSourceString">
		/// 인코딩된 문자열
		/// 형태는 "숫자 구분문자 숫자 구분문자 숫자 ... "의 형태의 문자열임
		/// 숫자는 0~255사이의 크기를 가지고 있음
		/// </param>
		/// <returns>디코딩된 결과의 문자열</returns>
		public string Decode(string pSourceString)
		{
			if (pSourceString.Trim() == string.Empty)
				return string.Empty;

			byte[] convertBytes = ConvertBytes2(pSourceString);

			if (convertBytes == null)
				return string.Empty;

			byte[] decodedBytes = DecodeBytes(convertBytes);
			byte[] trimBytes = TrimArray(decodedBytes);

			return mEc.GetString(trimBytes, 0, trimBytes.Length);
		}

		public void Clear()
		{
			mRij.Clear();
		}

		public static string CreateKey()
		{
			mRij.GenerateKey();
			byte[] key = mRij.Key;

			return ConvertString2(key);
		}

		public static string CreateIV()
		{
			mRij.GenerateIV();
			byte[] IV = mRij.IV;

			return ConvertString2(IV);
		}

		/// <summary>
		/// 분리문자로 구분된 숫자형태의 문자열을 바이트배열로 변환
		/// </summary>
		/// <param name="pSource">
		/// 형태는 "숫자 구분문자 숫자 구분문자 숫자 ... "의 형태의 문자열임
		/// 숫자는 0~255사이의 크기를 가지고 있음
		/// </param>
		/// 숫자:숫자... ,  0 <= 숫자 <= 255
		/// <param name="pSplitChar">어떤 문자열이 와도 상관이 없음.</param>
		/// <returns></returns>
		private byte[] ConvertBytes(string pSourceString, char pSplitChar)
		{
			byte[] convertedBytes;

			if (pSourceString.Length < 1)
				return null;

			string[] splitedString = pSourceString.Split(pSplitChar);

			convertedBytes = new byte[splitedString.Length];

			for (int i = 0; i < splitedString.Length; i++)
			{
				convertedBytes[i] = Convert.ToByte(splitedString[i]);
			}

			return convertedBytes;
		}

		/// <summary>
		/// 바이트값의 문자형태로 구성된 문자열을 바이트배열로 변환
		/// </summary>
		/// <param name="pSource">
		/// 형태는 "바이트값바이트값바이트값 ... "의 형태의 문자열임
		/// 바이트값는 000~255사이의 크기를 가지고 있음
		/// </param>
		/// <returns>바이트배열</returns>
		private byte[] ConvertBytes2(string pSourceString)
		{
			if (pSourceString.Length < 1)
				return null;

			if (pSourceString.Length % 3 != 0)
				return null;

			string[] splitedString = SplitString(pSourceString, 3);
			byte[] convertedBytes = new byte[splitedString.Length];

			for (int i = 0; i < splitedString.Length; i++)
			{
				convertedBytes[i] = Convert.ToByte(splitedString[i]);
			}

			return convertedBytes;
		}

		/// <summary>
		/// 입력된 크기만큼 문자열을 잘라 문자열 배열을 구성후 리턴
		/// </summary>
		/// <param name="pSourceString"></param>
		/// <param name="pSize"></param>
		/// <returns></returns>
		private string[] SplitString(string pSourceString, int pSize)
		{
			if (pSourceString.Length < 1)
				return null;

			if (pSize < 1)
				return null;

			int arraySize = pSourceString.Length / pSize;

			if (pSourceString.Length % pSize != 0)
				arraySize++;

			string[] splitedString = new string[arraySize];

			for (int i = 0; i < arraySize; i++)
			{
				if (i * pSize + pSize < pSourceString.Length)
					splitedString[i] = pSourceString.Substring(i * pSize, pSize);
				else
					splitedString[i] = pSourceString.Substring(i * pSize);
			}

			return splitedString;
		}

		/// <summary>
		/// 바이트로 구성된 배열을 구분문자로 구분하여 문자열 형태로 변환
		/// </summary>
		/// <param name="pSourceBytes"></param>
		/// <param name="pSplitChar"></param>
		/// <returns></returns>
		private static string ConvertString(byte[] pSourceBytes, char pSplitChar)
		{
			if (pSourceBytes.Length < 1)
				return string.Empty;

			StringBuilder sb = new StringBuilder(Convert.ToString(pSourceBytes[0]));

			for (int i = 1; i < pSourceBytes.Length; i++)
			{
				sb.Append(pSplitChar);
				sb.Append(Convert.ToString(pSourceBytes[i]));
			}

			return sb.ToString();
		}

		/// <summary>
		/// 바이트로 구성된 배열을 문자열 형태로 변환, 000~255 사이의 문자열 형태의 숫자로 구성된 문자열
		/// </summary>
		/// <param name="pSourceBytes"></param>
		/// <param name="pSplitChar"></param>
		/// <returns></returns>
		private static string ConvertString2(byte[] pSourceBytes)
		{
			if (pSourceBytes.Length < 1)
				return string.Empty;

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < pSourceBytes.Length; i++)
			{
				sb.Append(string.Format("{0:00#}", pSourceBytes[i]));
			}

			return sb.ToString();
		}

		private byte[] EncodeBytes(byte[] pSourceBytes)
		{
			ICryptoTransform encryptor = mRij.CreateEncryptor(mKey, mIV);

			//Encrypt the data.
			MemoryStream msEncrypt = new MemoryStream();
			CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

			//Write all data to the crypto stream and flush it.
			csEncrypt.Write(pSourceBytes, 0, pSourceBytes.Length);
			csEncrypt.FlushFinalBlock();

			return msEncrypt.ToArray();
		}

		private byte[] DecodeBytes(byte[] pSourceBytes)
		{
			byte[] decodedBytes;

			ICryptoTransform decryptor = mRij.CreateDecryptor(mKey, mIV);

			MemoryStream msDecrypt = new MemoryStream(pSourceBytes);
			CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

			decodedBytes = new byte[pSourceBytes.Length];

			//Read the data out of the crypto stream.
			csDecrypt.Read(decodedBytes, 0, decodedBytes.Length);

			return decodedBytes;
		}

		// Resize the dimensions of the array to a size that contains only valid
		// bytes.
		private byte[] TrimArray(byte[] pSourceBytes)
		{
			IEnumerator enum1 = pSourceBytes.GetEnumerator();
			int i = 0;

			while (enum1.MoveNext())
			{
				if (enum1.Current.ToString().Equals("0"))
				{
					break;
				}
				i++;
			}

			// Create a new array with the number of valid bytes.
			byte[] returnedArray = new byte[i];

			for (int j = 0; j < i; j++)
			{
				returnedArray[j] = pSourceBytes[j];
			}

			return returnedArray;
		}

	}
}
