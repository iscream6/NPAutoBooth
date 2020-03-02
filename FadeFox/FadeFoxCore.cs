/* 
 * ==============================================================================
 *   Program ID     : 
 *   Program Name   : 
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : fadefox
 *   Developer      : fadefox
 *   Create Date    : 2009-04-17
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 *       웹에서 사용시 참고 : 만약 64bit로 동작하는 IIS에서 해당 dll을 사용한다면 IIS의 응용프로그램 풀의 DefaultAppPool
 *                    의 고급 설정에서 32비트 응용 프로그램 사용을 True로 설정해야 함. 현재 해당 dll은 32bit로 컴파일 되기
 *                    때문에 위와 같은 설정을 하지 않으면 에러 발생
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;

namespace FadeFox
{
	public class FadeFoxCore
	{
		private static string mStartupPath = string.Empty;

		public FadeFoxCore()
		{
		}

		static FadeFoxCore()
		{
			// 본 어셈블리의 시작 위치를 얻음
			//string executingCodeBase = Assembly.GetExecutingAssembly().GetName().CodeBase;
			//mStartupPath = Path.GetDirectoryName(executingCodeBase);

			mStartupPath = Application.StartupPath;
		}

		public static string StartupPath
		{
			get
			{
				return mStartupPath;
			}
		}

		/// <summary>
		/// 시스템 강제 종료
		/// </summary>
		public static void Shutdown()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo("shutdown.exe");

			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.Arguments = "-s -f -t 00";

			Process.Start(startInfo);
		}

		/// <summary>
		/// 시스템 재부팅
		/// </summary>
		public static void Reboot()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo("shutdown.exe");

			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.Arguments = "-r -t 00";

			Process.Start(startInfo);
		}

		/// <summary>
		/// 시스템 로그오프
		/// </summary>
		public static void Logoff()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo("shutdown.exe");

			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.Arguments = "-l";

			Process.Start(startInfo);
		}

		/// <summary>
		/// 입력된 프로그램이름으로 실행된 프로세스가 실행중인지 검사, (확장자는 포함하지 않음)
		/// </summary>
		/// <param name="pProgName">프로세스명</param>
		/// <returns></returns>
		public static bool IsRunningProcess(string pProgName)
		{
			Process[] proc = Process.GetProcessesByName(pProgName);

			if (proc.Length > 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// 프로세스 실행
		/// </summary>
		/// <param name="pProgPath"></param>
		/// <param name="pProgName"></param>
		public static void StartProcess(string pProgPath, string pProgName)
		{
			StartProcess(pProgPath, pProgName, string.Empty, ProcessWindowStyle.Normal);
		}


		public static void StartProcess(string pProgPath, string pProgName, string pArguments)
		{
			StartProcess(pProgPath, pProgName, pArguments, ProcessWindowStyle.Normal);
		}

		/// <summary>
		/// 프로세스 실행
		/// </summary>
		/// <param name="pProgPath"></param>
		/// <param name="pProgName"></param>
		/// <param name="pArguments"></param>
		public static void StartProcess(string pProgPath, string pProgName, string pArguments, ProcessWindowStyle pStyle)
		{
			if (!File.Exists(pProgPath + "\\" + pProgName))
				return;

			ProcessStartInfo startInfo = new ProcessStartInfo(pProgPath + "\\" + pProgName);

			startInfo.WorkingDirectory = pProgPath;
			startInfo.WindowStyle = ProcessWindowStyle.Normal;

			if (pArguments != string.Empty)
				startInfo.Arguments = pArguments;

			Process.Start(startInfo);
		}

		/// <summary>
		/// 컴퓨터 명을 얻음
		/// </summary>
		/// <returns></returns>
		public static string GetComputerName()
		{
			return System.Environment.MachineName;
		}

		/// <summary>
		/// 로컬의 파일 사이즈를 얻음.
		/// 만약 존재하지 않으면 음수값을 리턴.
		/// </summary>
		/// <param name="pFullPath"></param>
		/// <returns></returns>
		public static long GetFileSize(string pFullPath)
		{
			FileInfo fInfo = new FileInfo(pFullPath);

			if (fInfo.Exists) // 파일이 존재할 경우
			{
				return fInfo.Length;
			}
			else
			{
				return -1;
			}
		}

		/// <summary>
		/// 파일 복사, 존재하면 덮어씀
		/// </summary>
		/// <param name="pSrcFullPath"></param>
		/// <param name="pDestFullPath"></param>
		public static void CopyFile(string pSrcFullPath, string pDestFullPath)
		{
			if (File.Exists(pSrcFullPath))
			{
				File.Copy(pSrcFullPath, pDestFullPath, true);
			}
		}

		/// <summary>
		/// 파일 삭제.
		/// </summary>
		/// <param name="pFullPath"></param>
		public static void DeleteFile(string pFullPath)
		{
			if (File.Exists(pFullPath))
			{
				File.Delete(pFullPath);
			}
		}

		public static void CreateTextFile(string pFullPath, string pContents)
		{
			if (!File.Exists(pFullPath))
			{
				// Create a file to write to.
				using (StreamWriter sw = File.CreateText(pFullPath))
				{
					sw.Write(pContents);
				}
			}
		}
	}

    // 결과, 성공여부 및 메시지 표시. /
    public class Result
    {
        private bool mSuccess = true;
        private ReadingTypes mReadingType = ReadingTypes.None;
        private int mResultInt = -1;
        public enum ReadingTypes
        {
            DiscountTIcket,
            CreditCard,
            ParkingTIcket,
            None
        }
        public bool Success
        {
            get
            {
                return mSuccess;
            }
            set
            {
                if (mSuccess != value)
                    mSuccess = value;
            }
        }
        /// <summary>
        /// 티켓인지 여부 true면 티켓 false면 카드
        /// </summary>
        public ReadingTypes CurrentReadingType
        {
            get
            {
                return mReadingType;
            }
        }


        private string mMessage = string.Empty;
        public string Message
        {
            get
            {
                return mMessage;
            }
            set
            {
                if (mMessage != value)
                    mMessage = value;
            }
        }

        public int ReultIntMessage
        {
            get
            {
                return mResultInt;
            }
            set
            {
                mResultInt = value;
            }
        }

        public Result(bool pSuccess, string pMessage)
        {
            mSuccess = pSuccess;
            mMessage = pMessage;
        }

        public Result(bool pSuccess, ReadingTypes pReadingTypes, string pMessage)
        {
            mSuccess = pSuccess;
            mReadingType = pReadingTypes;
            mMessage = pMessage;
        }



        public Result(bool pSuccess, string pMessage, int pIntMessage)
        {
            mSuccess = pSuccess;
            mMessage = pMessage;
            mResultInt = pIntMessage;
        }

        public Result(bool pSuccess, int pIntMessage)
        {
            mSuccess = pSuccess;
            mResultInt = pIntMessage;
        }

        public Result(bool pSuccess)
        {
            mSuccess = pSuccess;

            if (mSuccess)
                mMessage = "성공";
            else
                mMessage = "실패";
        }

        public Result(Exception pEx)
        {
            mSuccess = false;
            mMessage = pEx.Message;
        }
    }
}
