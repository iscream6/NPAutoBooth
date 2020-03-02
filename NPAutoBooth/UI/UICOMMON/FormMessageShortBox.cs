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
    public partial class FormMessageShortBox : Form
    {
        public FormMessageShortBox(string p_TopMessage , string pButtonMessage)
        {
            InitializeComponent();
            lbl_message.Text = p_TopMessage;
            btn_Yes.Text = pButtonMessage;


        }

        public FormMessageShortBox(string p_TopMessage, string pButtonMessage,int pTimeOut)
        {
            InitializeComponent();
            lbl_message.Text = p_TopMessage;
            btn_Yes.Text = pButtonMessage;
            inputTimer = pTimeOut;

        }

        private int inputTimer = 6;




  


        private void btn_OK_Click(object sender, EventArgs e)
        {
            timerInput.Stop();
            timerInput.Enabled = false;
            this.DialogResult = DialogResult.OK;
        }

        private void FormMessageShortBox_Load(object sender, EventArgs e)
        {
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMessageShortBox | FormMessageShortBox_Load", "폼로딩");
            timerInput.Enabled = true;
            timerInput.Start();
            

        }

        private void FormMessageShortBox_FormClosed(object sender, FormClosedEventArgs e)
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
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMessageShortBox | FormMessageShortBox_FormClosed", "폼종료중 에러" + ex.ToString());
            }
        }

        private void timerInput_Tick(object sender, EventArgs e)
        {
            inputTimer -= 1;
            if (inputTimer <= 0)
            {
                timerInput.Stop();
                timerInput.Enabled = false;
                this.DialogResult = DialogResult.OK;
     
                return;
            }
        }
    }
}
