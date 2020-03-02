using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NPAutoBooth.Common;
using FadeFox.Text;
using NPCommon;
using FadeFox.UI;

namespace NPAutoBooth.UI
{
    public partial class FormAdminMenu : Form
    {
        private NPSYS.FormType mPreFomrType = NPSYS.FormType.NONE;
        private NPSYS.FormType mCurrentFormType = NPSYS.FormType.MENU;
        public FormAdminMenu()
        {
            InitializeComponent();
        }

        public FormAdminMenu(NPSYS.FormType pPreFomrType)
        {
            InitializeComponent();
            SetLanguage(NPSYS.CurrentLanguageType);
            NPSYS.CurrentFormType = mCurrentFormType;
            mPreFomrType = pPreFomrType;
            this.Activate();
            timeSerialKeyFormAction.Enabled = true;
            timeSerialKeyFormAction.Start();
            timerExit.Enabled = true;
            timerExit.Start();
            
        }
        private void btn_home_Click(object sender, EventArgs e)
        {
 
            NPSYS.buttonSoundDingDong();
          
            this.Close();
        }


        private void btn_FeeSetup_Click(object sender, EventArgs e)
        {
        
                NPSYS.buttonSoundDingDong();
            
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminMenu|btn_FeeSetup_Click", "보우현금버튼누름");
            try
            {
                new FormAdminCashSetting().ShowDialog();
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR , "FormAdminMenu|btn_FeeSetup_Click" , "보우현금버튼누름시에러" + ex.ToString());
            }
        }

        private void btn_receiptSetup_Click(object sender, EventArgs e)
        {
  
                NPSYS.buttonSoundDingDong();
          
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminMenu|btn_receiptSetup_Click", "영수증설정버튼누름");

         
        }






        private void btn_DeviceTest_Click(object sender, EventArgs e)
        {
          
                NPSYS.buttonSoundDingDong();
            
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminMenu|btn_DeviceTest_Click", "장비테스트누름");
            new FormDeviceTest(mCurrentFormType).ShowDialog();
        }

        private void btn_Application_Exit(object sender, EventArgs e)
        {
            ProgramExit();
            
        }

        private void ProgramExit()
        {
  
                NPSYS.buttonSoundDingDong();
           
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminMenu|btn_Application_Exit", "프로그램종료버튼누름");
            string exitMsgData = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_TXT_CLOSE_PROGRAM.ToString());
            DialogResult exitDialogResult = new FormMessagePrePay( FormMessagePrePay.CarType.EXIT, exitMsgData).ShowDialog();
                if (exitDialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminMenu|btn_Application_Exit", "프로그램종료 선택");
                    Application.Exit();
                    return;

                }
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminMenu|btn_Application_Exit", "프로그램종료 취소");

        }

        private void btn_RePassWord_Click(object sender, EventArgs e)
        {

                NPSYS.buttonSoundDingDong();
          
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminMenu|btn_RePassWord_Click", "패스워드재설정누름:");
            new FormAdminPassword(mCurrentFormType).ShowDialog();
        }

        private void FormAdminMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            NPSYS.CurrentFormType = mPreFomrType; // 기존폼타입을 반환한다.
 
        }

        private void FormAdminMenu_Load(object sender, EventArgs e)
        {

        }

  
        bool isUseExit = false;
        private void timerExit_Tick(object sender, EventArgs e)
        {
            if (isUseExit)
            {
                timerExit.Enabled = false;
                timerExit.Stop();
                ProgramExit();
            }
        }

        #region 언어변환
        /// <summary>
        /// 언어변경
        /// </summary>
        private void SetLanguage(ConfigID.LanguageType pLanguageType)
        {

            NPSYS.Config.SetValue(ConfigID.FeatureSettingLanguage, pLanguageType.ToString()); // 메인폼에서 받아서 저장한다

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

        private void SetLanuageDynamic(ConfigID.LanguageType pLanguageType)
        {

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

        #endregion


    }
}
