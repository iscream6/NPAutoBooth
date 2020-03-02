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
using NPCommon;
using Sayou.Core;
using FadeFox.Database;
using FadeFox.UI;

namespace NPConfig
{
	public partial class NormalInfo : Form
	{
        // KIS_DIP KISAGNET는 127.0.0.1 에 PORT 1515사용
		string mConfigFilePath = "";
		ConfigDB3I mConfig = null;
		SQLite mDB = new SQLite();

		public NormalInfo(string pConfigfilePath)
		{
			InitializeComponent();
			mConfigFilePath = pConfigfilePath;
			mConfig = new ConfigDB3I(mConfigFilePath);


			mDB.Database = mConfigFilePath;
			mDB.Connect();


		//	LoadFloor();
		}

        /// <summary>
        /// DB에 설정정보 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnOk_Click(object sender, EventArgs e)
		{
            mConfig.SetValue(ConfigID.ParkingLotNo, txtParkingLotNo.Text); // 주차장 명칭
			mConfig.SetValue(ConfigID.ParkingLotName, txtParkingLotName.Text); // 주차장 명칭
			mConfig.SetValue(ConfigID.ParkingLotAddress, txtParkingLotAddress.Text); // 주차장 주소
			mConfig.SetValue(ConfigID.ParkingLotTelNo, txtParkingLotTelNo.Text); // 주차장 전화번호
			mConfig.SetValue(ConfigID.ParkingLotBusinessNo, txtParkingLotBusinessNo.Text); // 주차장 등록번호
			mConfig.SetValue(ConfigID.ParkingLotDaepyo, txtParkingLotDaepyo.Text); // 대표자
			mConfig.SetValue(ConfigID.ParkingLotBoothID, txtBoothID.Text); // 정산기 번호

			mConfig.SetValue(ConfigID.TmoneyGaID, txtTmoneyGaID.Text); // Tmoney 가맹점 ID
			mConfig.SetValue(ConfigID.TmoneyDanID, txtTmoneyDanID.Text); // Tmoney 단말기 ID
            mConfig.SetValue(ConfigID.TmoneySamId, txtSamId.Text); // Tmoney Sam ID
			mConfig.SetValue(ConfigID.TmoneyServiceType, cboTmoneyService.Text); // Tmoney 서비스 종류
            mConfig.SetValue(ConfigID.TmoneyVanIp, txt_TmoneyVanIp.Text); // Tmoney VAN IP
            mConfig.SetValue(ConfigID.TmoneyVanPort, txt_TmoneyVanPort.Text); // Tmoney VAN IP
            mConfig.SetValue(ConfigID.FeatureSettingCreditCardTerminalNo, txt_TerminalId.Text); // 신용카드 단말기 번호    
            mConfig.SetValue(ConfigID.CardVanIp, txt_TerminalIp.Text); // 신용카드 단말기 번호  
            mConfig.SetValue(ConfigID.CardVanPort, txt_TerminalPort.Text); // 신용카드 단말기 번호 
            mConfig.SetValue(ConfigID.FeatureSettingCreditCardSaupNo, txt_VanSaupNumber.Text); // 신용카드 사업자번호
            mConfig.SetValue(ConfigID.FeatureSettingSelectVan, cbxSelectVan.Text);

            mConfig.SetValue(ConfigID.CashTerminalId, txt_CashTerminalId.Text); // 신용카드 단말기 번호   
            mConfig.SetValue(ConfigID.CashVanIp, txt_Cash_TerminalIp.Text); // 신용카드 단말기 번호  
            mConfig.SetValue(ConfigID.CashVanPort, txt_CashTerminalPort.Text); // 신용카드 단말기 번호 
            mConfig.SetValue(ConfigID.CashSaupNo, txt_CashSaupNumber.Text); // 신용카드 사업자번호
            mConfig.SetValue(ConfigID.FeatureSettingCashSelectVan, cbxCashSelectVan.Text);

            if (chkVcatFailRestart.Checked)
            {
                mConfig.SetValue(ConfigID.FeatureSettingVCatFailRestart, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingVCatFailRestart, "N");
            }

            if (chkVcatUseVoice.Checked)
            {
                mConfig.SetValue(ConfigID.FeatureSettingVcatUseVoice, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingVcatUseVoice, "N");
            }

            //KOCSE 카드리더기 
            mConfig.SetValue(ConfigID.FeatureSettingVanSerialVersion, txtSerialVersion.Text);
            mConfig.SetValue(ConfigID.FeatureSettingVanSoftWareVersion, txt_VanSoftWareVersion.Text);
            //KOCSE 카드리더기 주석완료

            //----- TMoney Smartro 적용
            //CATID
            mConfig.SetValue(ConfigID.TmoneyTermID, txtTmnTermID.Text);
            //통신포트
            mConfig.SetValue(ConfigID.TmoneyDeviceType, (string)cmbTmnDiEVICEType.SelectedValue);
            //이더넷 통신 IP
            mConfig.SetValue(ConfigID.TmoneyDevIP, txtTmnDEVIp.Text);
            //이더넷 통신 Port
            mConfig.SetValue(ConfigID.TmoneyDevPort, txtTmnDEVPort.Text);
            //가상ID[MID]
            mConfig.SetValue(ConfigID.TmoneyCatID, txtTmnCatID.Text);
            //VAN 서버 IP
            mConfig.SetValue(ConfigID.TmoneyVanIp, txtTmnServerIP.Text);
            //VAN 서버 PORT
            mConfig.SetValue(ConfigID.TmoneyVanPort, txtTmnServerPort.Text);
            //단말기 IP 방식
            mConfig.SetValue(ConfigID.TmoneyEntDhcp, (string)cmbTmnENTDhcp.SelectedValue);
            //단말기 IP
            mConfig.SetValue(ConfigID.TmoneyEntDeviceIp, txtTmnENTDeviceip.Text);
            //단말기 SUBNET
            mConfig.SetValue(ConfigID.TmoneyEntSubnet, txtTmnENTSubnet.Text);
            //단말기 GATEWAY
            mConfig.SetValue(ConfigID.TmoneyEntGateway, txtTmnENTGateway.Text);
            //SAM 슬롯
            mConfig.SetValue(ConfigID.TmoneyTerminalSamSlot, ConfigID.SAMSLOT.NONE.ToString()); //초기화

            var strTmoneySlot = (string)cmbSAMSlot1.SelectedValue;
            if(strTmoneySlot == "1") mConfig.SetValue(ConfigID.TmoneyTerminalSamSlot, ConfigID.SAMSLOT.SLOT1.ToString());
            mConfig.SetValue(ConfigID.TmoneySamSlot1, (string)cmbSAMSlot1.SelectedValue);

            strTmoneySlot = (string)cmbSAMSlot2.SelectedValue;
            if (strTmoneySlot == "1") mConfig.SetValue(ConfigID.TmoneyTerminalSamSlot, ConfigID.SAMSLOT.SLOT2.ToString());
            mConfig.SetValue(ConfigID.TmoneySamSlot2, (string)cmbSAMSlot2.SelectedValue);

            strTmoneySlot = (string)cmbSAMSlot3.SelectedValue;
            if (strTmoneySlot == "1") mConfig.SetValue(ConfigID.TmoneyTerminalSamSlot, ConfigID.SAMSLOT.SLOT3.ToString());
            mConfig.SetValue(ConfigID.TmoneySamSlot3, (string)cmbSAMSlot3.SelectedValue);

            strTmoneySlot = (string)cmbSAMSlot4.SelectedValue;
            if (strTmoneySlot == "1") mConfig.SetValue(ConfigID.TmoneyTerminalSamSlot, ConfigID.SAMSLOT.SLOT4.ToString());
            mConfig.SetValue(ConfigID.TmoneySamSlot4, (string)cmbSAMSlot4.SelectedValue);
            //----- TMoney Smartro 적용완료

            this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void NPPaymentInfo_Load(object sender, EventArgs e)
		{
			string tmp = "";
  
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.None.ToString());
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.KIS_TIT_DIP_IFM.ToString());
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.KICC_DIP_IFM.ToString());
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.SmartroVCat.ToString());
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.FIRSTDATA_DIP.ToString());
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.KOCES_TCM.ToString());
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.KOCES_PAYMGATE.ToString());
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.KICC_TS141.ToString());
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.KSNET.ToString());
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.VNA_SMATRO.ToString());
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.VAN_FIRSTDATA.ToString());
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.VAN_NICE.ToString());
            //스마트로 TIT_DIP EV-CAT 적용
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.SMATRO_TIT_DIP.ToString());
            //TMoney Smartro 적용
            cbxSelectVan.Items.Add(ConfigID.CardReaderType.SMATRO_TL3500S.ToString());
            //Samslot 1,2,3,4 ComboBox DataSetting
            InitSamslotComboBox();
            //통신포트 ComboBox DataSetting
            InitDeviceTypeComboBox();
            //단말기 IP 방식 ComboBox DataSetting
            InitENTDhpcComboBox();
            //TMoney Smartro 적용완료

            cbxCashSelectVan.Items.Add(ConfigID.CardReaderType.None.ToString());
            cbxCashSelectVan.Items.Add(ConfigID.CardReaderType.VNA_SMATRO.ToString());
            cbxCashSelectVan.Items.Add(ConfigID.CardReaderType.VAN_FIRSTDATA.ToString());
            //KICC DIP적용완료
            //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑<ComboBox 초기화 완료>↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

            //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓<저장된 데이터 셋팅>↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            txtParkingLotName.Text = mConfig.GetValue(ConfigID.ParkingLotName); // 주차장 명칭
			txtParkingLotAddress.Text = mConfig.GetValue(ConfigID.ParkingLotAddress); // 주차장 주소
			
			txtParkingLotTelNo.Text = mConfig.GetValue(ConfigID.ParkingLotTelNo); // 주차장 전화번호
			txtParkingLotBusinessNo.Text = mConfig.GetValue(ConfigID.ParkingLotBusinessNo); // 주차장 등록번호
			txtBoothID.Text = mConfig.GetValue(ConfigID.ParkingLotBoothID); // 정산기 번호

			txtParkingLotDaepyo.Text = mConfig.GetValue(ConfigID.ParkingLotDaepyo); // 대표자
			txtTmoneyGaID.Text = mConfig.GetValue(ConfigID.TmoneyGaID); // Tmoney 가맹점 ID
			txtTmoneyDanID.Text = mConfig.GetValue(ConfigID.TmoneyDanID); // Tmoney 단말기 ID
            txtSamId.Text = mConfig.GetValue(ConfigID.TmoneySamId); // Tmoney Sam ID
            txt_TmoneyVanIp.Text = mConfig.GetValue(ConfigID.TmoneyVanIp); // Tmoney VAN IP
            txt_TmoneyVanPort.Text = mConfig.GetValue(ConfigID.TmoneyVanPort); // Tmoney VAN IP
            txt_TerminalId.Text = mConfig.GetValue(ConfigID.FeatureSettingCreditCardTerminalNo); // 단말기 번호 
            txt_TerminalIp.Text = mConfig.GetValue(ConfigID.CardVanIp); // 단말기 번호 
            txt_TerminalPort.Text = mConfig.GetValue(ConfigID.CardVanPort); // 단말기 번호 
            txt_VanSaupNumber.Text = mConfig.GetValue(ConfigID.FeatureSettingCreditCardSaupNo); // 신용카드 사업자번호
            cbxSelectVan.Text = mConfig.GetValue(ConfigID.FeatureSettingSelectVan); // 신용카드 VAN정보
            
            if(cbxSelectVan.Text == ConfigID.CardReaderType.SMATRO_TL3500S.ToString())
            {
                pnlASIS.Visible = false;
                pnlSMT3500S.Visible = true;
            }
            else
            {
                pnlASIS.Visible = true;
                pnlSMT3500S.Visible = false;
            }

            txt_CashTerminalId.Text = mConfig.GetValue(ConfigID.CashTerminalId); // 신용카드 단말기 번호     추가
            txt_Cash_TerminalIp.Text = mConfig.GetValue(ConfigID.CashVanIp); // 신용카드 단말기 번호   추가
            txt_CashTerminalPort.Text = mConfig.GetValue(ConfigID.CashVanPort); // 신용카드 단말기 번호  추가
            txt_CashSaupNumber.Text = mConfig.GetValue(ConfigID.CashSaupNo); // 신용카드 사업자번호



            cbxCashSelectVan.Text = (mConfig.GetValue(ConfigID.FeatureSettingCashSelectVan).Trim()==string.Empty? ConfigID.CardReaderType.None.ToString() : mConfig.GetValue(ConfigID.FeatureSettingCashSelectVan).Trim());


            if (mConfig.GetValue(ConfigID.FeatureSettingVCatFailRestart).ToUpper() == "N")
            {
                chkVcatFailRestart.Checked = false;
            }
            else
            {
                chkVcatFailRestart.Checked = true;
            }

            if (mConfig.GetValue(ConfigID.FeatureSettingVcatUseVoice).ToUpper() == "N")
            {
                chkVcatUseVoice.Checked = false;
            }
            else
            {
                chkVcatUseVoice.Checked = true;
            }



            if (cbxSelectVan.Text == string.Empty)
            {
                cbxSelectVan.Text = "FIRSTDATA";
            }

            tmp = mConfig.GetValue(ConfigID.TmoneyServiceType); // Tmoney 서비스 종류
			if (tmp == "")
				cboTmoneyService.SelectedIndex = 0;
			else
				cboTmoneyService.Text = tmp;

            txtParkingLotNo.Text = mConfig.GetValue(ConfigID.ParkingLotNo); // 주차장 번호

            //KOCSE 카드리더기 추가
            txt_VanSoftWareVersion.Text = mConfig.GetValue(ConfigID.FeatureSettingVanSoftWareVersion);
            txtSerialVersion.Text = mConfig.GetValue(ConfigID.FeatureSettingVanSerialVersion);
            //KOCSE 카드리더기 추가 주석

            //----- TMoney Smartro 적용
            //CATID
            txtTmnTermID.Text = mConfig.GetValue(ConfigID.TmoneyTermID);
            //통신포트
            var str = mConfig.GetValue(ConfigID.TmoneyDeviceType);
            cmbTmnDiEVICEType.SelectedValue = str == String.Empty? "-1" : str;
            //이더넷 통신 IP
            txtTmnDEVIp.Text = mConfig.GetValue(ConfigID.TmoneyDevIP);
            //이더넷 통신 Port
            txtTmnDEVPort.Text = mConfig.GetValue(ConfigID.TmoneyDevPort);
            //가상ID[MID]
            txtTmnCatID.Text = mConfig.GetValue(ConfigID.TmoneyCatID);
            //VAN 서버 IP
            txtTmnServerIP.Text = mConfig.GetValue(ConfigID.TmoneyVanIp);
            //VAN 서버 PORT
            txtTmnServerPort.Text = mConfig.GetValue(ConfigID.TmoneyVanPort);
            //단말기 IP 방식
            str = mConfig.GetValue(ConfigID.TmoneyEntDhcp);
            cmbTmnENTDhcp.SelectedValue = str == String.Empty? "-1" : str ;
            //단말기 IP
            txtTmnENTDeviceip.Text = mConfig.GetValue(ConfigID.TmoneyEntDeviceIp);
            //단말기 SUBNET
            txtTmnENTSubnet.Text = mConfig.GetValue(ConfigID.TmoneyEntSubnet);
            //단말기 GATEWAY
            txtTmnENTGateway.Text = mConfig.GetValue(ConfigID.TmoneyEntGateway);
            //SAM 슬롯
            NPSYS.TmoneyTerminalSamSlot = ConfigID.SAMSLOT.NONE;
            str = mConfig.GetValue(ConfigID.TmoneySamSlot1);
            cmbSAMSlot1.SelectedValue = str == String.Empty ? "-1" : str;
            if (str == "1") NPSYS.TmoneyTerminalSamSlot = ConfigID.SAMSLOT.SLOT1;

            str = mConfig.GetValue(ConfigID.TmoneySamSlot2);
            cmbSAMSlot2.SelectedValue = str == String.Empty ? "-1" : str;
            if (str == "1") NPSYS.TmoneyTerminalSamSlot = ConfigID.SAMSLOT.SLOT1;
            
            str = mConfig.GetValue(ConfigID.TmoneySamSlot3);
            cmbSAMSlot3.SelectedValue = str == String.Empty ? "-1" : str;
            if (str == "1") NPSYS.TmoneyTerminalSamSlot = ConfigID.SAMSLOT.SLOT1;

            str = mConfig.GetValue(ConfigID.TmoneySamSlot4);
            cmbSAMSlot4.SelectedValue = str == String.Empty ? "-1" : str;
            if (str == "1") NPSYS.TmoneyTerminalSamSlot = ConfigID.SAMSLOT.SLOT1;
            //----- TMoney Smartro 적용 완료
        }


        

        private void cbxSelectVan_SelectedIndexChanged(object sender, EventArgs e)
        {
            //KOCSE 카드리더기 추가
            if (cbxSelectVan.Text == ConfigID.CardReaderType.None.ToString())
            {
                pnlASIS.Visible = true;
                pnlSMT3500S.Visible = false;

                label23.Visible = false;
                label24.Visible = false;
                txt_TerminalId.Visible = false;
                txt_VanSaupNumber.Visible = false;
                label21.Visible = false;
                label22.Visible = false;
                txt_TerminalIp.Visible = false;
                txt_TerminalPort.Visible = false;
                chkVcatFailRestart.Visible = false;
                label15.Visible = false;
                label16.Visible = false;
                chkVcatUseVoice.Visible = false;
                txt_VanSoftWareVersion.Visible = false;
                txtSerialVersion.Visible = false;
            }
            if (cbxSelectVan.Text == ConfigID.CardReaderType.VAN_FIRSTDATA.ToString())
            {
                pnlASIS.Visible = true;
                pnlSMT3500S.Visible = false;
                
                label23.Visible = true;
                label24.Visible = true;
                txt_TerminalId.Visible = true;
                txt_VanSaupNumber.Visible = true;
                label21.Visible = false;
                label22.Visible = false;
                txt_TerminalIp.Visible = false;
                txt_TerminalPort.Visible = false;
                chkVcatFailRestart.Visible = false;
                label15.Visible = false;
                label16.Visible = false;
                chkVcatUseVoice.Visible = false;
                txt_VanSoftWareVersion.Visible = false;
                txtSerialVersion.Visible = false;

            }
            if (cbxSelectVan.Text == ConfigID.CardReaderType.VNA_SMATRO.ToString())
            {
                pnlASIS.Visible = true;
                pnlSMT3500S.Visible = false;

                label21.Visible = true;
                label22.Visible = true;
                txt_TerminalIp.Visible = true;
                txt_TerminalPort.Visible = true;
                chkVcatFailRestart.Visible = false;
                label15.Visible = false;
                label16.Visible = false;
                chkVcatUseVoice.Visible = false;
                txt_VanSoftWareVersion.Visible = false;
                txtSerialVersion.Visible = false;


            }
            if (cbxSelectVan.Text == ConfigID.CardReaderType.VAN_NICE.ToString())
            {
                pnlASIS.Visible = true;
                pnlSMT3500S.Visible = false; 
                
                label23.Visible = true;
                label24.Visible = true;
                txt_TerminalId.Visible = true;
                txt_VanSaupNumber.Visible = true;
                label21.Visible = true;
                label22.Visible = true;
                txt_TerminalIp.Visible = true;
                txt_TerminalPort.Visible = true;
                chkVcatFailRestart.Visible = false;
                label15.Visible = false;
                label16.Visible = false;
                chkVcatUseVoice.Visible = false;
                txt_VanSoftWareVersion.Visible = false;
                txtSerialVersion.Visible = false;


            }
            if (cbxSelectVan.Text == ConfigID.CardReaderType.SmartroVCat.ToString())
            {
                pnlASIS.Visible = true;
                pnlSMT3500S.Visible = false;

                label23.Visible = false;
                label24.Visible  = false;
                txt_TerminalId.Visible = false;
                txt_VanSaupNumber.Visible = false;
                label21.Visible = true;
                label22.Visible = true;
                txt_TerminalIp.Visible = true;
                txt_TerminalPort.Visible = true;
                chkVcatFailRestart.Visible = true;
                label15.Visible = false;
                label16.Visible = false;
                chkVcatUseVoice.Visible = true;
                txt_VanSoftWareVersion.Visible = false;
                txtSerialVersion.Visible = false;


            }
            if (cbxSelectVan.Text == NPCommon.ConfigID.CardReaderType.KIS_TIT_DIP_IFM.ToString()) //127.0.0.1에 1515포트사용
            {
                pnlASIS.Visible = true;
                pnlSMT3500S.Visible = false;

                label23.Visible = true;
                label24.Visible = false;
                txt_TerminalId.Visible = true;
                txt_VanSaupNumber.Visible = false;
                label21.Visible = true;
                label22.Visible = true;
                txt_TerminalIp.Visible = true;
                txt_TerminalPort.Visible = true;
                chkVcatFailRestart.Visible = true;
                label15.Visible = false;
                label16.Visible = false;
                chkVcatUseVoice.Visible = true;
                txt_VanSoftWareVersion.Visible = false;
                txtSerialVersion.Visible = false;

            }
            //KOCES PAYMGATE 적용
            if (cbxSelectVan.Text == NPCommon.ConfigID.CardReaderType.KOCES_TCM.ToString() || cbxSelectVan.Text == NPCommon.ConfigID.CardReaderType.KOCES_PAYMGATE.ToString()) //127.0.0.1에 1515포트사용
            {
                pnlASIS.Visible = true;
                pnlSMT3500S.Visible = false;

                label23.Visible = true;
                label24.Visible = true;
                txt_TerminalId.Visible = true;
                txt_VanSaupNumber.Visible = true;
                label21.Visible = false;
                label22.Visible = false;
                txt_TerminalIp.Visible = false;
                txt_TerminalPort.Visible = false;
                chkVcatFailRestart.Visible = true;
                label15.Visible = true;
                label16.Visible = true;
                chkVcatUseVoice.Visible = true;
                txt_VanSoftWareVersion.Visible = true;
                txtSerialVersion.Visible = true;

            }
            //KOCES PAYMGATE 적용완료
            //KOCSE 카드리더기 추가 주석
            //KICC DIP적용
            if (cbxSelectVan.Text == NPCommon.ConfigID.CardReaderType.KICC_DIP_IFM.ToString())
            {
                pnlASIS.Visible = true;
                pnlSMT3500S.Visible = false;

                txt_TerminalId.Visible = true;
                label23.Visible = true;
                label24.Visible = false;
                txt_VanSaupNumber.Visible = false;
                label21.Visible = false;
                label22.Visible = false;
                txt_TerminalIp.Visible = false;
                txt_TerminalPort.Visible = false;
                chkVcatFailRestart.Visible = true;
                label15.Visible = false;
                label16.Visible = false;
                chkVcatUseVoice.Visible = true;
                txt_VanSoftWareVersion.Visible = false;
                txtSerialVersion.Visible = false;
                label26.Visible = false;
                label27.Visible = false;
            }
            //KICC DIP적용완료

            //KICC TS141적용
            if (cbxSelectVan.Text == NPCommon.ConfigID.CardReaderType.KICC_TS141.ToString())
            {
                pnlASIS.Visible = true;
                pnlSMT3500S.Visible = false;

                txt_TerminalId.Visible = false;
                label23.Visible = false;
                label24.Visible = false;
                txt_VanSaupNumber.Visible = false;
                label21.Visible = false;
                label22.Visible = false;
                txt_TerminalIp.Visible = false;
                txt_TerminalPort.Visible = false;
                chkVcatFailRestart.Visible = false;
                label15.Visible = false;
                label16.Visible = false;
                chkVcatUseVoice.Visible = false;
                txt_VanSoftWareVersion.Visible = false;
                txtSerialVersion.Visible = false;
                label26.Visible = false;
                label27.Visible = false;


            }
            //KICC TS141적용완료

            //FIRSTDATA처리 
            if (cbxSelectVan.Text == NPCommon.ConfigID.CardReaderType.FIRSTDATA_DIP.ToString())
            {
                pnlASIS.Visible = true;
                pnlSMT3500S.Visible = false;

                label23.Visible = true;
                txt_TerminalId.Visible = true;
                label24.Visible = true;
                txt_VanSaupNumber.Visible = true;
                label21.Visible = true;
                txt_TerminalIp.Visible = true;
                label22.Visible = true;
                txt_TerminalPort.Visible = true;
                chkVcatFailRestart.Visible = false;
                label15.Visible = false;
                label16.Visible = false;
                chkVcatUseVoice.Visible = false;
                txt_VanSoftWareVersion.Visible = false;
                txtSerialVersion.Visible = false;
                label26.Visible = false;
                label27.Visible = false;
            }
            //FIRSTDATA처리 주석완료

            //KSNET 적용
            if (cbxSelectVan.Text == NPCommon.ConfigID.CardReaderType.KSNET.ToString())
            {
                pnlASIS.Visible = true;
                pnlSMT3500S.Visible = false;

                txt_TerminalId.Visible = true;
                label23.Visible = true;
                label21.Visible = true;
                label22.Visible = true;
                txt_TerminalIp.Visible = true;
                txt_TerminalPort.Visible = true;

                label24.Visible = false;
                txt_VanSaupNumber.Visible = false;
                chkVcatFailRestart.Visible = false;
                label15.Visible = false;
                label16.Visible = false;
                chkVcatUseVoice.Visible = false;
                txt_VanSoftWareVersion.Visible = false;
                txtSerialVersion.Visible = false;
                label26.Visible = false;
                label27.Visible = false;
            }
            //KSNET 적용완료

            //스마트로 TIT_DIP EV-CAT 적용
            if (cbxSelectVan.Text == ConfigID.CardReaderType.SMATRO_TIT_DIP.ToString())
            {
                pnlASIS.Visible = true;
                pnlSMT3500S.Visible = false;

                txt_TerminalId.Visible = false;
                label23.Visible = false;
                label24.Visible = false;
                txt_VanSaupNumber.Visible = false;
                label22.Visible = false;
                label21.Visible = true;
                txt_TerminalIp.Visible = true;
                txt_TerminalPort.Visible = false;
                chkVcatFailRestart.Visible = false;
                label15.Visible = false;
                label16.Visible = false;
                chkVcatUseVoice.Visible = false;
                txt_VanSoftWareVersion.Visible = false;
                txtSerialVersion.Visible = false;
                label26.Visible = false;
                label27.Visible = false;
            }
            //스마트로 TIT_DIP EV-CAT 적용완료

            //TMoney Smartro(SMATRO TL3500S) 적용
            if (cbxSelectVan.Text == ConfigID.CardReaderType.SMATRO_TL3500S.ToString())
            {
                pnlASIS.Visible = false;
                pnlSMT3500S.Visible = true;
            }
        }

        private void NormalInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDB.Disconnect();
        }

        private void InitSamslotComboBox()
        {
            var dicData = new Dictionary<string, string>();
            dicData.Add("NONE", "-1");
            dicData.Add("후불", "0");
            dicData.Add("티머니", "1");
            dicData.Add("이비", "2");
            dicData.Add("한페이", "3");
            dicData.Add("유페이", "4");
            dicData.Add("마이비", "5");
            cmbSAMSlot1.DataSource = new BindingSource(dicData, null);
            cmbSAMSlot1.DisplayMember = "Key";
            cmbSAMSlot1.ValueMember = "Value";
            cmbSAMSlot1.SelectedValue = '\0';

            cmbSAMSlot2.DataSource = new BindingSource(dicData, null);
            cmbSAMSlot2.DisplayMember = "Key";
            cmbSAMSlot2.ValueMember = "Value";
            cmbSAMSlot2.SelectedValue = '\0';

            cmbSAMSlot3.DataSource = new BindingSource(dicData, null);
            cmbSAMSlot3.DisplayMember = "Key";
            cmbSAMSlot3.ValueMember = "Value";
            cmbSAMSlot3.SelectedValue = '\0';

            cmbSAMSlot4.DataSource = new BindingSource(dicData, null);
            cmbSAMSlot4.DisplayMember = "Key";
            cmbSAMSlot4.ValueMember = "Value";
            cmbSAMSlot4.SelectedValue = '\0';
        }

        private void InitDeviceTypeComboBox()
        {
            var dicData = new Dictionary<string, string>();
            dicData.Add("NONE", "-1");
            dicData.Add("COM", "0");
            dicData.Add("USB", "1");
            dicData.Add("이더넷", "2");
            cmbTmnDiEVICEType.DataSource = new BindingSource(dicData, null);
            cmbTmnDiEVICEType.DisplayMember = "Key";
            cmbTmnDiEVICEType.ValueMember = "Value";
            cmbTmnDiEVICEType.SelectedValue = '\0';
        }

        private void InitENTDhpcComboBox()
        {
            var dicData = new Dictionary<string, string>();
            dicData.Add("NONE", "-1");
            dicData.Add("DHCP", "0");
            dicData.Add("STATIC", "1");
            cmbTmnENTDhcp.DataSource = new BindingSource(dicData, null);
            cmbTmnENTDhcp.DisplayMember = "Key";
            cmbTmnENTDhcp.ValueMember = "Value";
            cmbTmnENTDhcp.SelectedValue = '\0';
        }

        private void cmbTmnDiEVICEType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pnlSMT3500S.Visible)
            {
                switch ((string)cmbTmnDiEVICEType.SelectedValue)
                {
                    case "0":
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    default:
                        txtTmnDEVIp.Enabled = false;
                        txtTmnDEVPort.Enabled = false;
                        break;
                }
            }
        }
    }
}
