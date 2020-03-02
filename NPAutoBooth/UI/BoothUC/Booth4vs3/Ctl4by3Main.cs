using FadeFox.Text;
using NPCommon;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace NPAutoBooth.UI.BoothUC
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class Ctl4by3Main : MainUC
    {
        public Ctl4by3Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 상단 귀퉁이 Panel 을 연속 Click 시 발생하는 Event
        /// </summary>
        public override event BoothCommonLib.Event_ConfigCall ConfigCall;
        /// <summary>
        /// SearchCar 버튼 Click 시 발생하는 Event
        /// </summary>
        public override event EventHandler SearchCar_Click;

        public override void Initialize()
        {
            lblStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            TextCore.BoothName = NPSYS.BoothName;
            lblDeviceName.Text = NPSYS.BoothName;
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain|FormMain_Load", "프로그램 시작됨");
            //2020.01.08 이재영 : 언어변환 방식을 변경함. 
            SetContainerLanguage(NPSYS.CurrentLanguageType, tblPnlMain, SetLanguageDynamic);
            SetControlVisible();

            lblVersion.Text = NPSYS.ProgramVersion();
        }

        private void SetControlVisible()
        {
            if (NPSYS.gIsAutoBooth)
            {
                btnSearchCar.Visible = false;
                lbl_MSG_TITLE_PAYSTART.Visible = false;
                lblStart.Visible = false;
            }

            lblStart.Visible = false;

            if (NPSYS.gUseMultiLanguage)
            {
                btnEnglish.Visible = true;
                btnJapan.Visible = true;
            }
            else
            {
                btnEnglish.Visible = false;
                btnJapan.Visible = false;
            }
        }

        private void SetLanguageDynamic(ConfigID.LanguageType pLanguageType)
        {
            if (NPSYS.gIsAutoBooth)
            {
                lbl_TITLE_MAINNAME.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MACHINNAME1.ToString());
            }
            else
            {
                lbl_TITLE_MAINNAME.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MACHINNAME2.ToString());
            }
        }

        private int GoConfigSequence = 0;

        private void panel_ConfigClick1_Click(object sender, EventArgs e)
        {
            GoConfigSequence = 1;
            NPSYS.buttonSoundDingDong();
        }

        private void panel_ConfigClick2_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();

            if (GoConfigSequence == 1)
            {
                if(ConfigCall != null)
                {
                    ConfigCall();
                    TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu | panel_ConfigClick2_Click", "메인화면으로 강제로 이동시킴");
                }
                GoConfigSequence = 0;
            }
            else
            {
                GoConfigSequence = 0;
            }
        }

        private void btnSearchCar_Click(object sender, EventArgs e)
        {
            if (SearchCar_Click != null) SearchCar_Click(sender, e);
        }

        private void btnEnglish_Click(object sender, EventArgs e)
        {
            NPSYS.CurrentLanguageType = ConfigID.LanguageType.ENGLISH;
            SetContainerLanguage(ConfigID.LanguageType.ENGLISH, tblPnlMain, SetLanguageDynamic);
        }

        private void btnJapan_Click(object sender, EventArgs e)
        {
            NPSYS.CurrentLanguageType = ConfigID.LanguageType.ENGLISH;
            SetContainerLanguage(ConfigID.LanguageType.JAPAN, tblPnlMain, SetLanguageDynamic);
        }
    }
}
