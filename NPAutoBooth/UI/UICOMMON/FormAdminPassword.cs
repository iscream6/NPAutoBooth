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
    public partial class FormAdminPassword : Form
    {
        private NPSYS.FormType mPreFomrType = NPSYS.FormType.NONE;

        private NPSYS.FormType mCurrentFormType = NPSYS.FormType.PASSWORD;
        private Color mLabelSelectColor = Color.Salmon;
        private enum FocusingType
        {
            CurrentPassword,
            NewPassword1,
            NewPassword2,
            ConFirm,
            Cancle

        }
        public FormAdminPassword(NPSYS.FormType pPreFomrType)
        {
            InitializeComponent();

            lbl_TXT_RESET_PWD.Parent = picBackground;
            lbl1.Parent = picBackground;
            lbl2.Parent = picBackground;
            lbl3.Parent = picBackground;
            NPSYS.CurrentFormType = mCurrentFormType;
            mPreFomrType = pPreFomrType;
            this.Activate();
            SetLanguage(NPSYS.CurrentLanguageType);
        }

        private void Label_Click(object sender, EventArgs e)
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

        private  void Clear()
        {
            lblCurrentPassword.Text = "";
            lblNewPassword1.Text = "";
            lblNewPassword2.Text = "";
            lblCurrentPassword.PerformClick();
        }

        private void FormAdminPassword_FormClosed(object sender, FormClosedEventArgs e)
        {

            NPSYS.CurrentFormType = mPreFomrType;

        }

        private void FormAdminPassword_Load(object sender, EventArgs e)
        {
            Clear();


   
        }




        private void btnOk_Click(object sender, EventArgs e)
        {

            SaveSetting();
        }

        private void SaveSetting()
        {
            string currentPassword = NPSYS.Config.GetValue(ConfigID.AdminPassword);

            if (currentPassword != lblCurrentPassword.Text)
            {
                MessageBox.Show(new Form { TopMost = true }, "현재 암호가 올바르지 않습니다.");
                return;
            }
            if (lblNewPassword1.Text.Trim() == string.Empty || lblNewPassword2.Text.Trim() == string.Empty)
            {
                MessageBox.Show(new Form { TopMost = true }, "새로운 암호는 입력값이 들어가야 합니다.");
                return;

            }
            if (lblNewPassword1.Text != lblNewPassword2.Text)
            {
                MessageBox.Show(new Form { TopMost = true }, "새로운 암호가 일치하지 않습니다.");
                return;
            }

            NPSYS.Config.SetValue(ConfigID.AdminPassword, lblNewPassword1.Text);

            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminPassword|btnOk_Click", "암호재설정:" + lblNewPassword1.Text);
            this.Clear();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textChangedEvent(object sender, EventArgs e)
        {

                NPSYS.buttonSoundDingDong();
       
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
