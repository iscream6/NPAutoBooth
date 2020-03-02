using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NPAutoBooth.Common;
using FadeFox.Text;
using FadeFox.UI;
using NPCommon;

namespace NPAutoBooth.UI
{
    public partial class FormMessagePrePay : Form
    {
        private int inputtime = 10;
        public enum CarType
        {
            EXIT,
            ERROREXIT,
            FreeCar,
            PayCar
        }
        public FormMessagePrePay(CarType pCarType,string pMessage)
        {
            InitializeComponent();
            SetLanguage(NPSYS.CurrentLanguageType);
            SetLanuageDynamic(pCarType, pMessage);

            inputTimer.Enabled = true;
  
        }




        private void FormMessageBox_Load(object sender, EventArgs e)
        {
            ControlBackColorConvertImage();
        }

        private void FormMessageBox_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.Close();
                if (this != null)
                {
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMessageBox|FormMessageBox_FormClosed", "폼종료중 에러"+ex.ToString());
            }
        }

        private void ControlBackColorConvertImage()
        {


            btn_TXT_YES.Parent = PicBackGroud;
            btnTXT_BACK_TOMENU.Parent = PicBackGroud;
            lbl_Msg1.Parent = PicBackGroud;
            lbl_Msg2.Parent = PicBackGroud;
            lblTitle.Parent = PicBackGroud;

        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btn_cancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void inputTimer_Tick(object sender, EventArgs e)
        {
            inputtime -= 1;
            if (inputtime < 0)
            {
                inputTimer.Stop();
                this.DialogResult = DialogResult.Cancel;

                return;
            }
        }

        #region 언어변경
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



        }

        private void SetLanuageDynamic(CarType pCarType, string pMessage)
        {


                switch (pCarType)
                {
                    case CarType.PayCar:
                        lblTitle.Text = string.Empty;
                        lbl_Msg1.Text = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_Q_PAYMININFO1.ToString()).Replace("__MIN__",pMessage);
                        lbl_Msg2.Text = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_Q_PAYMININFO2.ToString()).Replace("__MIN__", pMessage);

                        break;

                    case CarType.FreeCar:
                        lblTitle.Text = string.Empty;
                        lbl_Msg1.Text = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_Q_FREECAR1.ToString()).Replace("__MIN__", pMessage);
                        lbl_Msg2.Text = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_Q_FREECAR2.ToString()).Replace("__MIN__", pMessage);
                        break;
                    case CarType.ERROREXIT:
                        lblTitle.Text = pMessage;
                        lbl_Msg1.Text = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_ERROREXIT_TOP.ToString());
                        lbl_Msg2.Text = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_ERROREXIT_BOTTOM.ToString());
                        break; ;
                    case CarType.EXIT:
                        lblTitle.Text = pMessage;
                        lbl_Msg1.Text = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_EXIT_TOP.ToString());
                        lbl_Msg2.Text = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_EXIT_BOTTOM.ToString());
                        break; ;



            }
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
