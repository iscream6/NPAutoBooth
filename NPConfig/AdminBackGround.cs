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
    public partial class AdminBackGround : Form
    {
        string mConfigFilePath = "";
        ConfigDB3I mConfig = null;
        SQLite mDB = new SQLite();

        public enum SearchType
        {
            MainImage,
            MainFlash,
            MainBtnSearchRoad,
            MainBtnSearchPay,
            MainLogo,
            MiddleImage
        }

        public AdminBackGround(string pConfigfilePath)
        {
            InitializeComponent();
            mConfigFilePath = pConfigfilePath;
            mConfig = new ConfigDB3I(mConfigFilePath);


            mDB.Database = mConfigFilePath;
            mDB.Connect();
        }



        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveBackGround();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AdminSaleTIcket_Load(object sender, EventArgs e)
        {
            SetLoad();
        }

        private void SaveBackGround()
        {


        }
        private void SetLoad()
        {



        }
        private void AdminSaleTIcket_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDB.Disconnect();
        }



        private void btn_locationMainImageSearch_Click(object sender, EventArgs e)
        {
            SearchImageLocation(SearchType.MainImage);
        }

        private void SearchImageLocation(SearchType pSeachtype)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (pSeachtype == SearchType.MainFlash)
            {
                openFileDialog.Filter = "(.swf)|*.swf";
            }
            else
            {
                openFileDialog.Filter = "(.jpg)|*.jpg|(.png)|*.png|(.gif)|*.gif";
            }


            openFileDialog.FilterIndex = 1;     // FilterIndex는 1부터 시작 (여기서는 *.txt) 

            openFileDialog.RestoreDirectory = true;

            try
            {

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string lFileName =openFileDialog.FileName;
                    switch (pSeachtype)
                    {
                        case SearchType.MainImage:
                            lblMainImageLoaction.Text = lFileName;    // Label에 파일경로 출력 
                            break;
                        case SearchType.MainFlash:
                            lblMainFlashLocation.Text = lFileName;
                            break;
                        case SearchType.MainBtnSearchPay:
                            lbl_MainSearchPaymentLocation.Text = lFileName;
                            break;
                        case SearchType.MainBtnSearchRoad:
                            lbl_MainSearchRoadLocation.Text = lFileName;
                            break;
                        case SearchType.MainLogo:
                            lblMainLogoLoaction.Text = lFileName;
                            break;
                        case SearchType.MiddleImage:
                            lblMiddleImageLoaction.Text = lFileName;
                            break;


                    }
                    
                }

            }

            catch (Exception ex)
            {

                MessageBox.Show("예외사항:" + ex.ToString());

            }
        }

        private void btn_MainLogoLoaction_Click(object sender, EventArgs e)
        {
            SearchImageLocation(SearchType.MainLogo);
        }

        private void btn_MainFlashLocation_Click(object sender, EventArgs e)
        {
            SearchImageLocation(SearchType.MainFlash);
        }

        private void btn_MainSearchRoadLocation_Click(object sender, EventArgs e)
        {
            SearchImageLocation(SearchType.MainBtnSearchRoad);
        }

        private void btn_MainSearchPaymentLocation_Click(object sender, EventArgs e)
        {
            SearchImageLocation(SearchType.MainBtnSearchPay);
        }

        private void btn_MiddleImageLoaction_Click(object sender, EventArgs e)
        {
            SearchImageLocation(SearchType.MiddleImage);
        }

    }
}
