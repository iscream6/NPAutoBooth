using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NPAutoBooth.Common;
using NPAutoBooth.UI;
using System.Threading;
using FadeFox.Text;
using FadeFox.Utility;
using NPCommon;
using System.Diagnostics;

namespace NPAutoBooth
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        /// 
        static bool CheckDuplicateProcess()
        {
            try
            {
                // 현재 프로그램과 동일한 프로세스 이름의 프로세스를 반환
                int l_DuplicateProcess = 0;
                string l_CurrentLocalProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                System.Diagnostics.Process[] localProcessByName = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process l_Process in localProcessByName)
                {

                    if (l_Process.ProcessName == l_CurrentLocalProcessName)
                    {

                        l_DuplicateProcess += 1;
                    }
                }
                if (l_DuplicateProcess < 2)
                {
                    return true;
                }
                else
                {

                    return false;
                }


            }

            catch
            {
                //LogClass.getWriteLogData("Program|CheckDuplicateProcess" + ex.ToString());
                return false;
            }

        }
        /// <summary>
        /// OCX 등록
        /// </summary>
        static void SetRegsvr32(String OcxName)
        {
            ProcessStartInfo processstartinfo = new ProcessStartInfo();

            processstartinfo.WindowStyle = ProcessWindowStyle.Hidden;

            processstartinfo.FileName = "regsvr32.exe";

            //#if DEBUG
            //            processstartinfo.Arguments = String.Format(" {0}.ocx", OcxName);
            //#else
            processstartinfo.Arguments = String.Format("/s {0}.ocx", OcxName);
            //#endif

            processstartinfo.UseShellExecute = false;


            Process process = new Process();

            process.StartInfo = processstartinfo;


            process.Start();
        }


        [STAThread]
        static void Main()
        {


            if (CheckDuplicateProcess() == true)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                SetRegsvr32("SmtSndRcvVCAT");
                SetRegsvr32("KisPosAgent");
                SetRegsvr32("exvmuvc");
                SetRegsvr32("Dreampos_CertificationOcx");
                SetRegsvr32("SmtSignOcx");
                SetRegsvr32("SmartroEncDongle");
                SetRegsvr32("mswinsck");
                SetRegsvr32("SMTCatAgentOCX");
                NPSYS.ConfigFilePath = Config.GetValue(ConfigID.ConfigFilePath);

                if (NPSYS.ConfigFilePath == "")
                {
                    MessageBox.Show("환경설정파일 경로를 설정해 주세요!");
                    return;
                }


                NPSYS.Config = new ConfigDB3I(NPSYS.ConfigFilePath);
                NPSYS.Config.Open();
                string BoothType = NPSYS.Config.GetValue(ConfigID.FeatureSettingAutoboorhSelect);
                NPSYS.CurrentBoothType = (ConfigID.BoothType)Enum.Parse(typeof(ConfigID.BoothType), BoothType);
                switch (NPSYS.CurrentBoothType)
                {
                    case ConfigID.BoothType.AB_1024:
                        NPSYS.gIsAutoBooth = true;
                        break;
                    case ConfigID.BoothType.PB_1024:
                        NPSYS.gIsAutoBooth = false;
                        break;
                    case ConfigID.BoothType.NOMIVIEWPB_1080:
                        NPSYS.gIsAutoBooth = false;
                        break;
                }

                Application.Run(new FormCreditMain());

            }
            else
            {
                TextCore.getWriteLogData("Program|Main" + "프로그램 중복실행으로 종료시킴");

                Application.Exit();
            }
        }
    }
}
