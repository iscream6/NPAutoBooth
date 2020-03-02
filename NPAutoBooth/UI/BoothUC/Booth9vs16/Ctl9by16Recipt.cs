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
    public partial class Ctl9by16Recipt : ReciptUC
    {
        public Ctl9by16Recipt()
        {
            InitializeComponent();
        }

        public override event EventHandler Recipt_Click;
        public override event EventHandler Cancle_Click;

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
        }

        /// <summary>
        /// 영문
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnglish_Click(object sender, EventArgs e)
        {
            NPSYS.CurrentLanguageType = ConfigID.LanguageType.ENGLISH;
            SetLanguage(ConfigID.LanguageType.ENGLISH);
        }

        /// <summary>
        /// 일본어
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJapan_Click(object sender, EventArgs e)
        {
            NPSYS.CurrentLanguageType = ConfigID.LanguageType.ENGLISH;
            SetLanguage(ConfigID.LanguageType.JAPAN);
        }

        /// <summary>
        /// 영수증 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Recipt_Click(object sender, EventArgs e)
        {
            if (Recipt_Click != null) Recipt_Click(sender, e);
        }

        /// <summary>
        /// 취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            if (Cancle_Click != null) Cancle_Click(sender, e);
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
