using FadeFox.Text;
using NPCommon;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace NPAutoBooth.UI.BoothUC
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class Ctl4by3SearchCar : SearchCarUC
    {
        private int GoConfigSequence = 0;

        public override event BoothCommonLib.Event_ConfigCall ConfigCall;
        public override event EventHandler PreForm_Click;
        public override event EventHandler Home_Click;
        public override event BoothCommonLib.Event_NumberClick Number_Click;
        public override event EventHandler Confirm_Click;
        public override event EventHandler BackNumber_Click;
        public override event BoothCommonLib.Event_LanguageChange LanguageChange_Click;

        public Ctl4by3SearchCar()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.DoubleBuffer |
                    ControlStyles.UserPaint |
                    ControlStyles.AllPaintingInWmPaint,
                    true);
            this.UpdateStyles();
        }

        public override UserControl_SearchCar SearchCarTextBar => uc_SearchCarTextBar;

        public override void Initialize()
        {
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

            if (NPSYS.gIsAutoBooth)
            {
                btn_TXT_BACK.Visible = false;
                btn_TXT_HOME.Visible = false;
            }

            if (NPSYS.CurrentCarNumType == NPCommon.ConfigID.CarNumberType.Digit4SetAUTO)
            {
                btnOk.Enabled = false;
            }
        }

        /// <summary>
        /// 영문
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnglish_Click(object sender, EventArgs e)
        {
            if(LanguageChange_Click != null) LanguageChange_Click(BoothCommonLib.LanguageType.ENGLISH);
        }

        /// <summary>
        /// 일본어
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJapan_Click(object sender, EventArgs e)
        {
            if (LanguageChange_Click != null) LanguageChange_Click(BoothCommonLib.LanguageType.JAPAN);
        }

        public override void OpenSetControl()
        {
            if (NPSYS.gIsAutoBooth)
            {
                btn_TXT_BACK.Visible = false;
                btn_TXT_HOME.Visible = false;
            }

            //if (NPSYS.CurrentBoothType != NPCommon.ConfigID.BoothType.NOMIVIEWPB_1080)
            //{
            //    lblPremoveTextMsg.Visible = false;
            //    lblHomeTextMsg.Visible = false;
            //}

            if (NPSYS.CurrentCarNumType == NPCommon.ConfigID.CarNumberType.Digit4SetAUTO)
            {
                btnOk.Enabled = false;
            }
        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            string number = ((FadeFox.UI.NPButton)sender).Text;
            if (Number_Click != null) Number_Click(number);
        }

        /// <summary>
        /// 지우기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackNumber_Click(object sender, EventArgs e)
        {
            if (BackNumber_Click != null) BackNumber_Click(sender, e);
        }

        /// <summary>
        /// 확인
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Confirm_Click != null) Confirm_Click(sender, e);
        }

        /// <summary>
        /// 처음화면
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TXT_HOME_Click(object sender, EventArgs e)
        {
            if (Home_Click != null) Home_Click(sender, e);
        }

        /// <summary>
        /// 이전화면
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TXT_BACK_Click(object sender, EventArgs e)
        {
            if (PreForm_Click != null) PreForm_Click(sender, e);
        }

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
                if (ConfigCall != null)
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
    }
}
