using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Management;
using System.Net.Sockets;

namespace FadeFox.Network
{
	public class NetworkCore
	{
		/// <summary>
		/// 네트워크가 연결된 상태인지를 검사한다. 가장 빠르고 효과적임.
		/// </summary>
		/// <param name="url">테스트 대상 주소. null 이면 www.naver.com 을 사용</param>
		/// <returns></returns>
		[DllImport("Sensapi.dll")]
		private static extern int IsNetworkAlive(ref int dwFlags);
		private static int NETWORK_ALIVE_WAN = 0x02;	// WAN 관련 유무(랜 카드 신호유무까지 포괄)
		//private static int NETWORK_ALIVE_LAN = 0x01;  // 랜 카드 신호 유무
		//private static int NETWORK_ALIVE_AOL = 0x04;  // 랜 카드 AOL 신호 유무

		public static bool IsNetworkConnected()
		{
			int status = IsNetworkAlive(ref NETWORK_ALIVE_WAN);   // 0은 접속안됨, 1은 접속됨.

			if (status == 0)
				return false;
			else
				return true;
		}

		public static string GetIPAddress()
		{
			string ip = "";

			foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
			{
				if (address.AddressFamily == AddressFamily.InterNetwork)
				{
					ip = address.ToString();
					break;
				}
			}

			return ip;
		}

		/// <summary>
		/// IP Address를 얻음
		/// </summary>
		/// <returns></returns>
		public static string GetIPAddressUsingWMI()
		{
			try
			{
				ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled='TRUE'");
				ManagementObjectCollection queryCol = query.Get();
				string ipAddress = string.Empty;

				foreach (ManagementObject mo in queryCol)
				{
					string[] address = mo["IPAddress"] as string[];
					string[] subnets = mo["IPSubnet"] as string[];
					string[] defaultgateways = mo["DefaultIPGateway"] as string[];
					string networkCard = mo["Description"] as string;
					string macAddress = mo["MACAddress"] as string;

					foreach (string ip in address)
					{
						if (ip.IndexOf(":") < 0)
							ipAddress += "{" + ip + "}";
					}
				}

				return ipAddress;
				
				//IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
				//IPAddress addr = ipHost.AddressList[0];

				//return ipHost.AddressList[0].ToString();
			}
			catch
			{
				return string.Empty;
			}
		}

		// 공인아이피를 얻을때 사용함.
		public static string GetIPAddressUsingSite()
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://whatismyip.org");
				request.Timeout = 100000;
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					using (Stream stream = response.GetResponseStream())
					{
						using (StreamReader reader = new StreamReader(stream))
						{
							return reader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return string.Empty;
		}

		/// <summary>
		/// 웹상의 이미지 파일을 읽어 Bitmap 객체로 반환한다.
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static Bitmap LoadImageFromUrl(string url)
		{
			HttpWebRequest request;
			HttpWebResponse response = null;
			Stream stream = null;
			Bitmap bitmap = null;

			try
			{
				request = (HttpWebRequest)WebRequest.Create(url);
				request.AllowWriteStreamBuffering = true;

				response = (HttpWebResponse)request.GetResponse();

				if ((stream = response.GetResponseStream()) != null)
					bitmap = new Bitmap(stream);
			}
			catch
			{
				return null;
			}
			finally
			{
				if (stream != null)
					stream.Close();

				if (response != null)
					response.Close();
			}

			return (bitmap);
		}

		/// <summary>
		/// 웹상의 이미지 파일을 읽어 바이트 배열로 반환한다.
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static byte[] LoadImageBinaryFromUrl(string url)
		{
			Bitmap bitmap = LoadImageFromUrl(url);

			if (bitmap == null)
				return null;

			try
			{
				MemoryStream ms = new MemoryStream();
				bitmap.Save(ms, bitmap.RawFormat);
				ms.Close();

				return ms.ToArray();
			}
			catch
			{
				return null;
			}
		}

		// 유효한 URL인지를 체크한다.
		public static bool IsUrl(string text)
		{
			return Regex.IsMatch(text, @"^((ht|f)tp(s?)\:\/\/|~/|/)?([\w]+:\w+@)?([a-zA-Z]{1}([\w\-]+\.)+([\w]{2,5}))(:[\d]{1,5})?((/?\w+/)+|/?)(\w+\.[\w]{3,4})?((\?\w+=\w+)?(&\w+=\w+)*)?");
		}

		/// <summary>
		/// Gmail을 이용하여 웹메일을 발송한다.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public static bool SendMail(MailMessage message)
		{
			SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
			sc.Credentials = new NetworkCredential("somisul@gmail.com", "!tngusdl");
			sc.EnableSsl = true;

			try
			{
				sc.Send(message);

				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Gmail을 이용하여 웹메일을 발송한다.
		/// </summary>
		/// <param name="fromAddress"></param>
		/// <param name="toAddress"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <returns></returns>
		public static bool SendMail(string fromAddress, string toAddress, string subject, string body)
		{
			MailAddress from = new MailAddress(fromAddress);
			MailAddress to = new MailAddress(toAddress);

			MailMessage message = new MailMessage(from, to);
			message.Subject = subject;
			message.Body = body;

			return SendMail(message);
		}
	}
}
