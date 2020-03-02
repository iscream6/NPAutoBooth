using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace FadeFox.IO
{
	public class IOCore
	{
		public const byte SOH = 0x01; // Start Of Heading
		public const byte STX = 0x02; // Indicate start of text
		public const byte ETX = 0x03; // Indicate end of text
		public const byte EOT = 0x04; // End Of Transmit
		public const byte ENQ = 0x05; // Request to send response
		public const byte ACK = 0x06; // Send positive response
		public const byte NAK = 0x15; // Send negative response
		public const byte CR = 0x0D;  // Carriage return
		public const byte DLE = 0x10; // Clear

		public static string PortErrorMessage(SerialError pCode)
		{
			string errMsg = "";

			switch (pCode)
			{
				case SerialError.Frame:
					errMsg = "하드웨어에서 프레이밍 오류가 발생하였습니다.";
					break;

				case SerialError.Overrun:
					errMsg = "문자 버퍼 오버런이 발생하였습니다. 다음문자가 손실됩니다.";
					break;

				case SerialError.RXOver:
					errMsg = "입력 버퍼 오버플로우가 발생하였습니다.";
					break;

				case SerialError.RXParity:
					errMsg = "하드웨어에서 패리티 오류가 발생하였습니다.";
					break;

				case SerialError.TXFull:
					errMsg = "출력 버퍼 오버플로우가 발생하였습니다.";
					break;

				default:
					errMsg = "알려지지 않은 오류가 발생하였습니다.";
					break;
			}

			return errMsg;
		}
	}
}
