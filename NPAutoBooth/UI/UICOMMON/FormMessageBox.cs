using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NPAutoBooth.Common;
using FadeFox.Text;

namespace NPAutoBooth.UI
{
    public partial class FormMessageBox : Form
    {
        public FormMessageBox(string p_TopMessage , string p_BottomMessage)
        {
            InitializeComponent();
            lbl_message.Text = p_TopMessage;
            lbl_message2.Text = p_BottomMessage;
            lbl_message.TextAlign = ContentAlignment.TopCenter;
            lbl_message2.TextAlign = ContentAlignment.TopCenter;
  
        }

        public FormMessageBox(string p_TopMessage, string p_BottomMessage,bool isLeftAlgin)
        {
            InitializeComponent();
            lbl_message.Text = p_TopMessage;
            lbl_message2.Text = p_BottomMessage;
            lbl_message.TextAlign = ContentAlignment.TopLeft;
            lbl_message2.TextAlign = ContentAlignment.TopLeft;
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
            lbltitle.Parent = PicBackGroud;
            lbl_message.Parent = PicBackGroud;
            lbl_message2.Parent = PicBackGroud;
            btn_OK.Parent = PicBackGroud;
            btn_cancle.Parent = PicBackGroud;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btn_cancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
