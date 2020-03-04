using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FadeFox.Database.SQLite;
using FadeFox.Security;
using FadeFox.Utility;
using FadeFox.Text;
using FadeFox.UI;
using NPCommon;

namespace NPConfig
{
	public partial class AdminPasswordSetting : Form
	{
		string mConfigFilePath = "";

		public AdminPasswordSetting(string pConfigfilePath)
		{
			InitializeComponent();
			mConfigFilePath = pConfigfilePath;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
            ConfigDB3I config = new ConfigDB3I(mConfigFilePath);
			if (txtPassword1.Text.Trim() == "")
			{
				MsgBox.Show("암호를 설정해 주세요.", MsgType.Warning);
                return;
			}

			if (!TextCore.IsInt(txtPassword1.Text))
			{
				MsgBox.Show("암호는 숫자로만 가능합니다.", MsgType.Warning);
				return;
			}

			if (txtPassword1.Text != txtPassword2.Text)
			{
				MsgBox.Show("암호가 일치하지 않습니다.", MsgType.Warning);
				return;
			}
            config.SetValue(ConfigID.AdminPassword, txtPassword1.Text);
			//this.Close();
			MessageBox.Show("저장하였습니다.");
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

        private void AdminPasswordSetting_Load(object sender, EventArgs e)
        {
            ConfigDB3I config = new ConfigDB3I(mConfigFilePath);
            txtPassword1.Text = config.GetValue(ConfigID.AdminPassword);
        }
	}
}
