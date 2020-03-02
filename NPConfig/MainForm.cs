using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FadeFox.Utility;
using System.IO;
using FadeFox.UI;
using FadeFox.Text;


namespace NPConfig
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

		}

		private void btnSettingSerialPort_Click(object sender, EventArgs e)
		{
			if (oftConfig.Text == "")
			{
				MsgBox.Show("설정파일을 선택해 주세요.");
			}
			else
			{
				if (File.Exists(oftConfig.Text))
				{
					SerialPortConnectionSetting setting = new SerialPortConnectionSetting(oftConfig.Text);

					setting.ShowDialog();
				}
				else
				{
					MsgBox.Show("존재하지 않는 설정파일을 선택해 주세요.", MsgType.Warning);
				}
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			oftConfig.Text = Config.GetValue("NPPAYMENTCONFIG:CONFIG_FILE_PATH");
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			
			Config.SetValue("NPPAYMENTCONFIG:CONFIG_FILE_PATH", oftConfig.Text);
		}

		private void btnSettingServer_Click(object sender, EventArgs e)
		{
			if (oftConfig.Text == "")
			{
				MsgBox.Show("설정파일을 선택해 주세요.");
			}
			else
			{
				if (File.Exists(oftConfig.Text))
				{
					ServerInfo setting = new ServerInfo(oftConfig.Text);

					setting.ShowDialog();
				}
				else
				{
					MsgBox.Show("존재하지 않는 설정파일을 선택해 주세요.", MsgType.Warning);
				}
			}
		}

		private void btnSettingAdminPassword_Click(object sender, EventArgs e)
		{
			if (oftConfig.Text == "")
			{
				MsgBox.Show("설정파일을 선택해 주세요.");
			}
			else
			{
				if (File.Exists(oftConfig.Text))
				{
                    AdminPasswordSetting frm = new AdminPasswordSetting(oftConfig.Text);

					frm.ShowDialog();
				}
				else
				{
					MsgBox.Show("존재하지 않는 설정파일을 선택해 주세요.", MsgType.Warning);
				}
			}
		}

		private void btnNPPaymentInfo_Click(object sender, EventArgs e)
		{
			if (oftConfig.Text == "")
			{
				MsgBox.Show("설정파일을 선택해 주세요.");
			}
			else
			{
				if (File.Exists(oftConfig.Text))
				{
					NormalInfo frm = new NormalInfo(oftConfig.Text);

					frm.ShowDialog();
				}
				else
				{
					MsgBox.Show("존재하지 않는 설정파일을 선택해 주세요.", MsgType.Warning);
				}
			}
		}

		private void btnLookup_Click(object sender, EventArgs e)
		{
			if (oftConfig.Text == "")
			{
				MsgBox.Show("설정파일을 선택해 주세요.");
			}
			else
			{
				if (File.Exists(oftConfig.Text))
				{
					LookupInfo setting = new LookupInfo(oftConfig.Text);

					setting.ShowDialog();
				}
				else
				{
					MsgBox.Show("존재하지 않는 설정파일을 선택해 주세요.", MsgType.Warning);
				}
			}
		}

		private void btnUsingSetting_Click(object sender, EventArgs e)
		{
			if (oftConfig.Text == "")
			{
				MsgBox.Show("설정파일을 선택해 주세요.");
			}
			else
			{
				if (File.Exists(oftConfig.Text))
				{
					UsingSetting frm = new UsingSetting(oftConfig.Text);

					frm.ShowDialog();
				}
				else
				{
					MsgBox.Show("존재하지 않는 설정파일을 선택해 주세요.", MsgType.Warning);
				}
			}
		}

		private void btnSettingFloor_Click(object sender, EventArgs e)
		{
			if (oftConfig.Text == "")
			{
				MsgBox.Show("설정파일을 선택해 주세요.");
			}
			else
			{
				if (File.Exists(oftConfig.Text))
				{
					ParkingLotFloor setting = new ParkingLotFloor(oftConfig.Text);

					setting.ShowDialog();
				}
				else
				{
					MsgBox.Show("존재하지 않는 설정파일을 선택해 주세요.", MsgType.Warning);
				}
			}
		}

        private void btnParkingSetting_Click(object sender, EventArgs e)
        {
            if (oftConfig.Text == "")
            {
                MsgBox.Show("설정파일을 선택해 주세요.");
            }
            else
            {
                if (File.Exists(oftConfig.Text))
                {
                    AdminFetureSetting FetureSetting = new AdminFetureSetting(oftConfig.Text);

                    FetureSetting.ShowDialog();
                }
                else
                {
                    MsgBox.Show("존재하지 않는 설정파일을 선택해 주세요.", MsgType.Warning);
                }
            }

        }



        private void btn_BackGroundSetting_Click(object sender, EventArgs e)
        {
            if (oftConfig.Text == "")
            {
                MsgBox.Show("설정파일을 선택해 주세요.");
            }
            else
            {
                if (File.Exists(oftConfig.Text))
                {
                    AdminBackGround lAdminBackGround = new AdminBackGround(oftConfig.Text);

                    lAdminBackGround.ShowDialog();
                }
                else
                {
                    MsgBox.Show("존재하지 않는 설정파일을 선택해 주세요.", MsgType.Warning);
                }
            }
        }

        private void btnHttpSetting_Click(object sender, EventArgs e)
        {
            if (oftConfig.Text == "")
            {
                MsgBox.Show("설정파일을 선택해 주세요.");
            }
            else
            {
                if (File.Exists(oftConfig.Text))
                {
                    AdminHTTP FetureSetting = new AdminHTTP(oftConfig.Text);

                    FetureSetting.ShowDialog();
                }
                else
                {
                    MsgBox.Show("존재하지 않는 설정파일을 선택해 주세요.", MsgType.Warning);
                }
            }
        }
    }
}
