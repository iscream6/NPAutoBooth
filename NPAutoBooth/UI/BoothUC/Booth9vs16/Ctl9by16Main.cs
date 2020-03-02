using FadeFox.Text;
using FadeFox.UI;
using NPCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace NPAutoBooth.UI.BoothUC
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class Ctl9by16Main : MainUC
    {
        public Ctl9by16Main()
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
            SetContainerLanguage(NPSYS.CurrentLanguageType, this, SetLanguageDynamic);
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
            SetLanguage(ConfigID.LanguageType.ENGLISH);
        }

        private void btnJapan_Click(object sender, EventArgs e)
        {
            NPSYS.CurrentLanguageType = ConfigID.LanguageType.ENGLISH;
            SetLanguage(ConfigID.LanguageType.JAPAN);
        }

        /// <summary>
        /// 언어변경
        /// </summary>
        public void SetLanguage(NPCommon.ConfigID.LanguageType pLanguageType)
        {
            Control[] currentControl = GetAllControlsUsingRecursive(this);
            foreach (Control controlItem in currentControl)
            {
                switch (controlItem)
                {
                    case Label labelType:
                        if (labelType.Tag != null && labelType.Tag.ToString().Trim().Length > 0)
                        {
                            labelType.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.transaction, labelType.Tag.ToString());
                        }
                        break;
                    case ImageButton imageButtonType:
                        if (imageButtonType.Tag != null && imageButtonType.Tag.ToString().Trim().Length > 0)
                        {
                            imageButtonType.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.transaction, imageButtonType.Tag.ToString());
                        }
                        break;
                    case TextBox textBoxType:
                        if (textBoxType.Tag != null && textBoxType.Tag.ToString().Trim().Length > 0)
                        {
                            textBoxType.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.transaction, textBoxType.Tag.ToString());
                        }
                        break;
                    case Button buttoType:
                        if (buttoType.Tag != null && buttoType.Tag.ToString().Trim().Length > 0)
                        {
                            buttoType.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.transaction, buttoType.Tag.ToString());
                        }
                        break;
                }
            }
            SetLanuageDynamic(pLanguageType);
        }

        private void SetLanuageDynamic(NPCommon.ConfigID.LanguageType pLanguageType)
        {
            //if (NPSYS.gIsAutoBooth)
            //{
            //    lbl_TITLE_MAINNAME.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MACHINNAME1.ToString());
            //}
            //else
            //{
            //    lbl_TITLE_MAINNAME.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MACHINNAME2.ToString());
            //}
        }

        private Control[] GetAllControlsUsingRecursive(Control containerControl)
        {
            List<Control> allControls = new List<Control>();

            foreach (Control control in containerControl.Controls)
            {
                //자식 컨트롤을 컬렉션에 추가한다
                allControls.Add(control);

                //만일 자식 컨트롤이 또 다른 자식 컨트롤을 가지고 있다면…
                if (control.Controls.Count > 0)
                {
                    //자신을 재귀적으로 호출한다
                    allControls.AddRange(GetAllControlsUsingRecursive(control));
                }
            }

            //모든 컨트롤을 반환한다
            return allControls.ToArray();
        }
    }
}
