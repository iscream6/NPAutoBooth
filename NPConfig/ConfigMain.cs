using FadeFox.UI;
using FadeFox.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPConfig
{
    public partial class ConfigMain : Form
    {
        private SortedList<string, Form> oMdlChildFrmList;
        private Form oCurrentFrm;

        #region Constructor

        public ConfigMain()
        {
            InitializeComponent();

            oMdlChildFrmList = new SortedList<string, Form>();
        }

        #endregion

        #region Form Load & Close & Initialize

        private void ConfigMain_Load(object sender, EventArgs e)
        {
            oftConfig.Text = Config.GetValue("NPPAYMENTCONFIG:CONFIG_FILE_PATH");
        }

        private void ConfigMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Config.SetValue("NPPAYMENTCONFIG:CONFIG_FILE_PATH", oftConfig.Text);
        }

        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((Form)sender).FormClosing -= ChildForm_FormClosing;
            
            //화면에 나타나 있는 폼을 보관하고 있는 객체에서 해당 폼을 제거한다...
            if(((Form)sender).Tag != null)
            {
                oMdlChildFrmList.Remove(((Form)sender).Tag.ToString());
            }
        }

        #endregion

        #region Setting form call Button evnet

        private void SettingButton_Click(object sender, EventArgs e)
        {
            if (oftConfig.Text == "")
            {
                MsgBox.Show("설정파일을 선택해 주세요.");
            }
            else
            {
                ChildDisplay(((RibbonButton)sender).Tag.ToString());
            }
        }

        #endregion

        #region Private Methods

        private void ChildDisplay(string sPrmTag)
        {
            //이미 해당 폼이 화면에 나타나 있는지의 여부를 확인
            if (oMdlChildFrmList.ContainsKey(sPrmTag))
            {
                oCurrentFrm = oMdlChildFrmList[sPrmTag];
                oCurrentFrm.Focus();
            }
            else //화면에 나타나 있지 않은 경우
            {
                if (File.Exists(oftConfig.Text))
                {
                    switch (sPrmTag)
                    {
                        case "SVR": //서버 설정
                            oCurrentFrm = new ServerInfo(oftConfig.Text);
                            break;
                        case "SRI": //시리얼포트 설정
                            oCurrentFrm = new SerialPortConnectionSetting(oftConfig.Text);
                            break;
                        case "RES": //레스트풀 설정
                            oCurrentFrm = new AdminHTTP(oftConfig.Text);
                            break;
                        case "DIV": //사용기능 설정
                            oCurrentFrm = new UsingSetting(oftConfig.Text);
                            break;
                        case "PRK": //주차기능 설정
                            oCurrentFrm = new AdminFetureSetting(oftConfig.Text);
                            break;
                        case "PAY": //일반 및 결제 설정
                            oCurrentFrm = new NormalInfo(oftConfig.Text);
                            break;
                        case "PWD": //관리 암호 설정
                            oCurrentFrm = new AdminPasswordSetting(oftConfig.Text);
                            break;
                    }

                    oCurrentFrm.MdiParent = this;
                    oCurrentFrm.Dock = DockStyle.Fill;
                    oCurrentFrm.FormBorderStyle = FormBorderStyle.None;
                    oCurrentFrm.FormClosing += ChildForm_FormClosing;
                    oCurrentFrm.Show();
                    oMdlChildFrmList.Add(sPrmTag, oCurrentFrm);
                }
                else
                {
                    MsgBox.Show("존재하지 않는 설정파일을 선택해 주세요.", MsgType.Warning);
                }
            }
        }

        #endregion
    }
}
