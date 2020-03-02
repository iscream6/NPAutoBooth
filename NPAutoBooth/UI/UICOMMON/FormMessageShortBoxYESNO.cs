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
    public partial class FormMessageShortBoxYESNO : Form
    {
        public FormMessageShortBoxYESNO(string p_TopMessage , string pButtonMessage,string pButtonMessage2)
        {
            InitializeComponent();
            lbl_message.Text = p_TopMessage;
            btn_Yes.Text = pButtonMessage;
            Btn_No.Text = pButtonMessage2;

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
                this.DialogResult = DialogResult.No;
                timerInput.Stop();
                timerInput.Enabled = false;
                return;
            }
        }

        private void Btn_No_Click(object sender, EventArgs e)
        {
            timerInput.Stop();
            timerInput.Enabled = false;
            this.DialogResult = DialogResult.No;
            return;
        }
    }
}
