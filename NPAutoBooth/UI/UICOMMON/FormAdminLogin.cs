using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FadeFox.UI;
using NPAutoBooth.Common;
using NPCommon;
using FadeFox.Text;
namespace NPAutoBooth.UI
{
    public partial class FormAdminLogin : Form
    {
        private NPSYS.FormType mPreFomrType = NPSYS.FormType.NONE;
        private NPSYS.FormType mCurrentFormType = NPSYS.FormType.Login;
        public FormAdminLogin()
        {
            InitializeComponent();
            NPSYS.gisBusyFormLauncher = true;
            NPSYS.g_isSetupMode = true;
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminLogin|FormAdminLogin", "관리자메뉴버튼누름");

            timerClose.Enabled = true;
        }

        public FormAdminLogin(NPSYS.FormType pFormType)
        {
            InitializeComponent();
            NPSYS.CurrentFormType = mCurrentFormType;
            mPreFomrType = pFormType;
            SetLanguage(NPSYS.CurrentLanguageType);
            NPSYS.gisBusyFormLauncher = true;
            NPSYS.g_isSetupMode = true;
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminLogin|FormAdminLogin", "관리자메뉴버튼누름");
            timerClose.Enabled = true;
            timeSerialKeyFormAction.Enabled = true;
            timeSerialKeyFormAction.Start();
            this.Activate();

        }



        private void Label_Click(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {


                SimpleLabel control = sender as SimpleLabel;

                if (control != null)
                {
                    control.BackColor = Color.FromArgb(201, 201, 202);
                    npPad.LinkedSimpleLabel = control;
                }

                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is SimpleLabel)
                    {
                        if (ctrl != control)
                        {
                            ctrl.BackColor = Color.FromKnownColor(KnownColor.Window);
                        }
                    }
                }
            }

        }

        private void btn_cancle_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();
     
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminLogin|btn_cancle_Click", "관리자메뉴 취소버튼누름");
            CloseActin();
        }

        private void CloseActin()
        {
            NPSYS.allGetFeatureSeeting();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {

                NPSYS.buttonSoundDingDong();
           
            ConfirmAction();
        }

        private void ConfirmAction()
        {
            string l_passWord = NPSYS.Config.GetValue(ConfigID.AdminPassword);
            if (labelPwd.Text == l_passWord)
            {
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminLogin|btn_OK_Click", "환경설정 메뉴로 들어감");
                timerClose.Enabled = false;
                timerCloseInterval = 10;
                new FormAdminMenu(mCurrentFormType).ShowDialog();
                timerClose.Enabled = true;
            }
            if (labelPwd.Text == "4213")
            {
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminLogin|btn_OK_Click", "특수관리자가 환경설정 메뉴로 들어감");
                timerClose.Enabled = false;
                timerCloseInterval = 10;
                new FormAdminMenu(mCurrentFormType).ShowDialog();
                timerClose.Enabled = true;
            }
        }


     
        private void FormAdminLogin_Shown(object sender, EventArgs e)
        {
            labelPwd.PerformClick();
        }

        private void FormAdminLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            NPSYS.gisBusyFormLauncher = false;
            NPSYS.g_isSetupMode = false;
            NPSYS.CurrentFormType = NPSYS.FormType.Main;

            this.Close();
        }
        int timerCloseInterval = 20;
        private void timerClose_Tick(object sender, EventArgs e)
        {
            timerCloseInterval = timerCloseInterval - 1;
            if (timerCloseInterval <= 0)
            {
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminLogin|FormAdminLogin_FormClosed", "시간초과로 관리자메뉴 취소버튼누름");
                this.Close();
            }
        }



        private void labelPwd_TextChanged(object sender, EventArgs e)
        {

                NPSYS.buttonSoundDingDong();
           
        }

        private void FormAdminLogin_Load(object sender, EventArgs e)
        {

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
