using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FadeFox.Database;
using FadeFox.UI;
using FadeFox.Database.SQLite;
using FadeFox.Security;
using FadeFox.Utility;
using NPCommon;

namespace NPConfig
{
    public partial class AdminHTTP : Form
    {
        string mConfigFilePath = "";
        ConfigDB3I mConfig = null;
        SQLite mDB = new SQLite();



        public AdminHTTP(string pConfigfilePath)
        {
            InitializeComponent();
            mConfigFilePath = pConfigfilePath;
            mConfig = new ConfigDB3I(mConfigFilePath);


            mDB.Database = mConfigFilePath;
            mDB.Connect();
        }



        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveInfo();
            //this.Close();
            MessageBox.Show("저장하였습니다.");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AdminSaleTIcket_Load(object sender, EventArgs e)
        {
            SetLoad();
        }

        private void SaveInfo()
        {

            mConfig.SetValue(ConfigID.RESTfulLocalPort, txtRESTfulLocalPort.Text);
            mConfig.SetValue(ConfigID.RESTfulServerIp, txtRESTfulServerIp.Text);
            mConfig.SetValue(ConfigID.RESTfulServerPort, txtRESTfulServerPort.Text);
            mConfig.SetValue(ConfigID.RESTfulVersion, txtRESTfulVersion.Text == string.Empty ? "v2.0" : txtRESTfulVersion.Text);
        }
        private void SetLoad()
        {
           txtRESTfulLocalPort.Text = mConfig.GetValue(ConfigID.RESTfulLocalPort);
           txtRESTfulServerIp.Text = mConfig.GetValue(ConfigID.RESTfulServerIp);
           txtRESTfulServerPort.Text = mConfig.GetValue(ConfigID.RESTfulServerPort);
           txtRESTfulVersion.Text= mConfig.GetValue(ConfigID.RESTfulVersion);


        }
        private void AdminSaleTIcket_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDB.Disconnect();
        }




    

     

    }
}
