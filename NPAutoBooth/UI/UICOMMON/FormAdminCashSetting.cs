using FadeFox;
using FadeFox.Text;
using FadeFox.UI;
using NPCommon;
using NPCommon.DEVICE;
using NPCommon.DTO.Receive;
using NPCommon.REST;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NPAutoBooth.UI
{
    public partial class FormAdminCashSetting : Form
    {
        private NPSYS.FormType mPreFomrType = NPSYS.FormType.NONE;
        private Color mLabelSelectColor = Color.Salmon;
        private HttpProcess mHttpProcess = new HttpProcess(); // 시제설정누락처리
        private enum FocusingType
        {
            Qty50Settig,
            Qty50SettingMin,
            Qty100Settig,
            Qty100SettingMin,
            Qty500Settig,
            Qty500SettingMin,
            Qty1000Settig,
            Qty1000SettingMin,
            Qty5000Settig,
            Qty5000SettingMin,
            MagamDataInfo,
            Magam,
            ConFirm,
            Cancle

        }

        private void KeyBoardColorClear()
        {
            lbl50SettingQty.BackColor = Color.FromKnownColor(KnownColor.Window);
            lbl50MinQty.BackColor = Color.FromKnownColor(KnownColor.Window);
            lbl100SettingQty.BackColor = Color.FromKnownColor(KnownColor.Window);
            lbl100MinQty.BackColor = Color.FromKnownColor(KnownColor.Window);
            lbl500SettingQty.BackColor = Color.FromKnownColor(KnownColor.Window);
            lbl500MinQty.BackColor = Color.FromKnownColor(KnownColor.Window);
            lbl1000SettingQty.BackColor = Color.FromKnownColor(KnownColor.Window);
            lbl1000MinQty.BackColor = Color.FromKnownColor(KnownColor.Window);
            lbl5000SettingQty.BackColor = Color.FromKnownColor(KnownColor.Window);
            lbl5000MinQty.BackColor = Color.FromKnownColor(KnownColor.Window);
            picMagamDataInfo.BackColor = Color.Transparent;
            picMagamSave.BackColor = Color.Transparent;
        }
        public FormAdminCashSetting()
        {

            InitializeComponent();
        }


        public void SetControl()
        {
        }

        public FormAdminCashSetting(NPSYS.FormType pPreFomrType)
        {

            InitializeComponent();
            NPSYS.CurrentFormType = NPSYS.FormType.Magam;
            mPreFomrType = pPreFomrType;
            this.Activate();

        }


        private void FromAdminCashSetting_load(object sender, EventArgs e)
        {

            SetLanguage(NPSYS.CurrentLanguageType);
            ControlSet();
            Clear();
            ReadData();

            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormAdminCashSetting|FromAdminCashSetting_load", "보유현금및마감설정화면 로드됨");
        }

        private void Clear()
        {
            lbl50SettingQty.Text = "0";
            lbl100SettingQty.Text = "0";
            lbl500SettingQty.Text = "0";
            lbl1000SettingQty.Text = "0";
            lbl5000SettingQty.Text = "0";

            lbl50MinQty.Text = "0";
            lbl100MinQty.Text = "0";
            lbl500MinQty.Text = "0";
            lbl1000MinQty.Text = "0";
            lbl5000MinQty.Text = "0";

            lbl50MaxQty.Text = "0";
            lbl100MaxQty.Text = "0";
            lbl500MaxQty.Text = "0";
            lbl1000MaxQty.Text = "0";
            lbl5000MaxQty.Text = "0";


        }

        private void ControlSet()
        {
            if (!NPSYS.Device.UsingSettingBill )
            {
                lbl1000MinQty.Visible = false;
                lbl1000SettingQty.Visible = false;
                lbl100MinQty.Visible = false;
                lbl100SettingQty.Visible = false;
                lbl5000MinQty.Visible = false;
                lbl5000SettingQty.Visible = false;
                lbl500MinQty.Visible = false;
                lbl500SettingQty.Visible = false;
                lbl50MinQty.Visible = false;
                lbl50SettingQty.Visible = false;
                lblMoney_2TypeName.Visible = false;
                lblMoney_3TypeName.Visible = false;
                lblMoney_4TypeName.Visible = false;
                lblMoney_5TypeName.Visible = false;
                lblMoney_6TypeName.Visible = false;

                lbMoney_2Type.Visible = false;
                lbMoney_3Type.Visible = false;
                lbMoney_4Type.Visible = false;
                lbMoney_5Type.Visible = false;
                lbMoney_6Type.Visible = false;

                btn_out100.Visible = false;
                btn_out1000.Visible = false;
                btn_out50.Visible = false;
                btn_out500.Visible = false;
                btn_out5000.Visible = false;

                lbl_TXT_MAX_AMOUNT.Visible = false;
                lbl_TXT_CUR_AMOUNT.Visible = false;
                lbl_TXT_MIN_AMOUNT.Visible = false;
                lbl50MaxQty.Visible = false;
                lbl100MaxQty.Visible = false;
                lbl500MaxQty.Visible = false;
                lbl1000MaxQty.Visible = false;
                lbl5000MaxQty.Visible = false;
                btnOk_TXT_YES.Visible = false;


                npPad.Visible = false;
            }

        }

        private void ReadData()
        {
            // 동전에 설정되어 있는 예약 수량을 가져옴.


            lbl50SettingQty.Text = NPSYS.Config.GetValue(ConfigID.Cash50SettingQty);
            lbl100SettingQty.Text = NPSYS.Config.GetValue(ConfigID.Cash100SettingQty);
            lbl500SettingQty.Text = NPSYS.Config.GetValue(ConfigID.Cash500SettingQty);


            lbl1000SettingQty.Text = NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty);
            lbl5000SettingQty.Text = NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty);

            lbl50MinQty.Text = NPSYS.Config.GetValue(ConfigID.Cash50MinQty);
            lbl100MinQty.Text = NPSYS.Config.GetValue(ConfigID.Cash100MinQty);
            lbl500MinQty.Text = NPSYS.Config.GetValue(ConfigID.Cash500MinQty);

            lbl1000MinQty.Text = NPSYS.Config.GetValue(ConfigID.Cash1000MinQty);
            lbl5000MinQty.Text = NPSYS.Config.GetValue(ConfigID.Cash5000MinQty);


            lbl50MaxQty.Text = NPSYS.Config.GetValue(ConfigID.Cash50MaxQty);
            lbl100MaxQty.Text = NPSYS.Config.GetValue(ConfigID.Cash100MaxQty);
            lbl500MaxQty.Text = NPSYS.Config.GetValue(ConfigID.Cash500MaxQty);

            lbl1000MaxQty.Text = NPSYS.Config.GetValue(ConfigID.Cash1000MaxQty);
            lbl5000MaxQty.Text = NPSYS.Config.GetValue(ConfigID.Cash5000MaxQty);



        }



        private void btnCancel_Click(object sender, EventArgs e)
        {

                NPSYS.buttonSoundDingDong();
          
            FadeFox.Text.TextCore.ACTION(FadeFox.Text.TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnCancel_Click", "시제금보충버튼 누르지 않고 취소키로 종료함");
            this.Clear();
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            string infoMsg = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_Q_CURRENTSET.ToString());
            string infoYes = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_TXT_ENTER.ToString());
            string infono = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_TXT_NO.ToString());

            DialogResult result = new FormMessageShortBoxYESNO(infoMsg, infoYes, infono).ShowDialog();

            if (result == DialogResult.OK)
            {
                SaveSetting();
                NPSYS.NoCheckCargeMoneyOut();
            }


        }

        private void SaveSetting()
        {
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "시제금보충버튼누름");
            try
            {
                this.Enabled = false;
                if (NPSYS.Device.UsingSettingBill)
                {
                    string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    if (lbl50SettingQty.Text == "")
                        lbl50SettingQty.Text = "0";

                    if (lbl100SettingQty.Text == "")
                        lbl100SettingQty.Text = "0";

                    if (lbl500SettingQty.Text == "")
                        lbl500SettingQty.Text = "0";

                    if (lbl1000SettingQty.Text == "")
                        lbl1000SettingQty.Text = "0";

                    if (lbl5000SettingQty.Text == "")
                        lbl5000SettingQty.Text = "0";

                    if (lbl50MinQty.Text == "")
                        lbl50MinQty.Text = "0";

                    if (lbl100MinQty.Text == "")
                        lbl100MinQty.Text = "0";

                    if (lbl500MinQty.Text == "")
                        lbl500MinQty.Text = "0";

                    if (lbl1000MinQty.Text == "")
                        lbl1000MinQty.Text = "0";

                    if (lbl5000MinQty.Text == "")
                        lbl5000MinQty.Text = "0";

                    //if (lblBillSettingQty.Text == "")
                    //    lblBillSettingQty.Text = "0";

                    //if (lblBillMaxQty.Text == "")
                    //    lblBillMaxQty.Text = "0";



                    string Cahs1000 = NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty);
                    string Cahs5000 = NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty);


                    string Cahs50 = NPSYS.Config.GetValue(ConfigID.Cash50SettingQty);
                    string Cahs100 = NPSYS.Config.GetValue(ConfigID.Cash100SettingQty);
                    string Cahs500 = NPSYS.Config.GetValue(ConfigID.Cash500SettingQty);

                    //시제설정누락처리
                    int petty50Qty = Convert.ToInt32(lbl50SettingQty.Text);
                    int petty100Qty = Convert.ToInt32(lbl100SettingQty.Text);
                    int petty500Qty = Convert.ToInt32(lbl500SettingQty.Text);
                    int petty1000Qty = Convert.ToInt32(lbl1000SettingQty.Text);
                    int petty5000Qty = Convert.ToInt32(lbl5000SettingQty.Text);

                    PettyCash currentpettyCash = mHttpProcess.SavepettyCash(petty50Qty, petty100Qty, petty500Qty, 0, petty1000Qty, petty5000Qty,0,0);
                    if (currentpettyCash.status.Success == false)
                    {
                        string infoErrorMsg = "ERROR CODE:" + currentpettyCash.status.code;
                        string infoErrorButton = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_TXT_ENTER.ToString());
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormAdminCashSetting | SaveSetting", "시제금보충오류 원인:" + infoErrorMsg);
                        FormMessageShortBox mFormMessageShortBox = new FormMessageShortBox(infoErrorMsg, infoErrorButton);
                        mFormMessageShortBox.ShowDialog();
                        return;
                    }
                    //시제설정누락처리완료
                    int l_PrePossessionTotalCount = 0;
                         int l_PrePossessionTotalMoney = 0;
                    int l_NowPossessionTotalCount = 0;
                    int l_NowPossessionTotalMoney = 0;
                    if (NPSYS.CurrentMoneyType == NPCommon.ConfigID.MoneyType.WON)
                    {
                        l_PrePossessionTotalCount = Convert.ToInt32(Cahs50) + Convert.ToInt32(Cahs100) + Convert.ToInt32(Cahs500) + Convert.ToInt32(Cahs1000) + Convert.ToInt32(Cahs5000);
                        l_PrePossessionTotalMoney = (Convert.ToInt32(Cahs50) * 50) + (Convert.ToInt32(Cahs100) * 100) + (Convert.ToInt32(Cahs500) * 500) + (Convert.ToInt32(Cahs1000) * 1000) + (Convert.ToInt32(Cahs5000) * 5000);

                        l_NowPossessionTotalCount = Convert.ToInt32(lbl50SettingQty.Text) + Convert.ToInt32(lbl100SettingQty.Text) + Convert.ToInt32(lbl500SettingQty.Text) + Convert.ToInt32(lbl1000SettingQty.Text) + Convert.ToInt32(lbl5000SettingQty.Text);
                        l_NowPossessionTotalMoney = (Convert.ToInt32(lbl50SettingQty.Text) * 50) + (Convert.ToInt32(lbl100SettingQty.Text) * 100) + (Convert.ToInt32(lbl500SettingQty.Text) * 500) + (Convert.ToInt32(lbl1000SettingQty.Text) * 1000) + (Convert.ToInt32(lbl5000SettingQty.Text) * 5000);
                    }
                    else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                    {
                        l_PrePossessionTotalCount = Convert.ToInt32(Cahs50) + Convert.ToInt32(Cahs100) + Convert.ToInt32(Cahs500) + Convert.ToInt32(Cahs1000) + Convert.ToInt32(Cahs5000);
                        l_PrePossessionTotalMoney = (Convert.ToInt32(Cahs50) * 1) + (Convert.ToInt32(Cahs100) * 5) + (Convert.ToInt32(Cahs500) * 10) + (Convert.ToInt32(Cahs1000) * 20) + (Convert.ToInt32(Cahs5000) * 50);

                        l_NowPossessionTotalCount = Convert.ToInt32(lbl50SettingQty.Text) + Convert.ToInt32(lbl100SettingQty.Text) + Convert.ToInt32(lbl500SettingQty.Text) + Convert.ToInt32(lbl1000SettingQty.Text) + Convert.ToInt32(lbl5000SettingQty.Text);
                        l_NowPossessionTotalMoney = (Convert.ToInt32(lbl50SettingQty.Text) * 1) + (Convert.ToInt32(lbl100SettingQty.Text) * 5) + (Convert.ToInt32(lbl500SettingQty.Text) * 10) + (Convert.ToInt32(lbl1000SettingQty.Text) * 20) + (Convert.ToInt32(lbl5000SettingQty.Text) * 50);

                    }
                    // 영수증프린터알람해제처리 및 영수증 출력변경
                    //if (NPSYS.Device.gIsUseReceiptPrintDevice)
                    if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
                    // 영수증프린터알람해제처리 및 영수증 출력변경 주석완료
                    {
                        if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
                        {
                            NPSYS.Device.HMC60.FontSize(2, 1);
                            NPSYS.Device.HMC60.Print("   *" + "   " + " 시제금 내역*\n\n");
                            NPSYS.Device.HMC60.FontSize(1, 1);
                            NPSYS.Device.HMC60.Print("   이전시제:" + "\n");
                            NPSYS.Device.HMC60.Print("   " + "----------------------------------------------\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "종       류") + "     " + TextCore.ToRightAlignString(10, "수    량") + TextCore.Space(7) + TextCore.ToRightAlignString(10, "금    액") + "\n");
                            NPSYS.Device.HMC60.Print("   " + "----------------------------------------------\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "<합     계>") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(l_PrePossessionTotalCount)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(l_PrePossessionTotalMoney)) + "\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "50       원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Cahs50)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs50) * 50)) + "\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "100      원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Cahs100)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs100) * 100)) + "\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "500      원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Cahs500)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs500) * 500)) + "\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "1000     원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Cahs1000)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs1000) * 1000)) + "\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "5000     원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Cahs5000)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs5000) * 5000)) + "\n");
                            NPSYS.Device.HMC60.Print("   " + "----------------------------------------------\n");
                            NPSYS.Device.HMC60.Print("   현재 시제:" + "\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "종       류") + "     " + TextCore.ToRightAlignString(10, "수    량") + TextCore.Space(7) + TextCore.ToRightAlignString(10, "금    액") + "\n");
                            NPSYS.Device.HMC60.Print("   " + "----------------------------------------------\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "<합     계>") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(l_NowPossessionTotalCount)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(l_NowPossessionTotalMoney)) + "\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "50       원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(lbl50SettingQty.Text)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl50SettingQty.Text) * 50)) + "\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "100      원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(lbl100SettingQty.Text)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl100SettingQty.Text) * 100)) + "\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "500      원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(lbl500SettingQty.Text)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl500SettingQty.Text) * 500)) + "\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "1000     원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(lbl1000SettingQty.Text)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl1000SettingQty.Text) * 1000)) + "\n");
                            NPSYS.Device.HMC60.Print("   " + TextCore.ToLeftAlignString(10, "5000     원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(lbl5000SettingQty.Text)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl5000SettingQty.Text) * 5000)) + "\n");
                            NPSYS.Device.HMC60.Print("   " + "----------------------------------------------\n");
                            NPSYS.Device.HMC60.Print("   " + "출력일시: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n");
                        }
                        else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
                        {
                            if (NPSYS.CurrentMoneyType == NPCommon.ConfigID.MoneyType.WON)
                            {
                                int valueSpace = 4;
                                NPSYS.Device.HMC60.FontSize(2, 1);
                                NPSYS.Device.HMC60.Print("*시제금 내역*\n\n");
                                NPSYS.Device.HMC60.FontSize(1, 1);
                                NPSYS.Device.HMC60.Print("이전시제:" + "\n");
                                NPSYS.Device.HMC60.Print("------------------------------------\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "종     류") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, "수    량") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, "금    액") + "\n");
                                NPSYS.Device.HMC60.Print("------------------------------------\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "<합   계>") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(l_PrePossessionTotalCount)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(l_PrePossessionTotalMoney)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "50     원") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(Cahs50)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs50) * 50)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "100    원") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(Cahs100)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs100) * 100)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "500    원") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(Cahs500)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs500) * 500)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "1000   원") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(Cahs1000)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs1000) * 1000)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "5000   원") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(Cahs5000)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs5000) * 5000)) + "\n");
                                NPSYS.Device.HMC60.Print("------------------------------------\n");
                                NPSYS.Device.HMC60.Print("현재 시제:" + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "종     류") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, "수    량") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, "금    액") + "\n");
                                NPSYS.Device.HMC60.Print("------------------------------------\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "<합   계>") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(l_NowPossessionTotalCount)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(l_NowPossessionTotalMoney)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "50     원") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(lbl50SettingQty.Text)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl50SettingQty.Text) * 50)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "100    원") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(lbl100SettingQty.Text)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl100SettingQty.Text) * 100)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "500    원") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(lbl500SettingQty.Text)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl500SettingQty.Text) * 500)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "1000   원") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(lbl1000SettingQty.Text)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl1000SettingQty.Text) * 1000)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "5000   원") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(lbl5000SettingQty.Text)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl5000SettingQty.Text) * 5000)) + "\n");
                                NPSYS.Device.HMC60.Print("------------------------------------\n");
                                NPSYS.Device.HMC60.Print("출력일시: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n");
                            }
                            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                            {
                                int valueSpace = 4;
                                NPSYS.Device.HMC60.FontSize(2, 1);
                                NPSYS.Device.HMC60.Print("*Cash reserves*\n\n");
                                NPSYS.Device.HMC60.FontSize(1, 1);
                                NPSYS.Device.HMC60.Print("Previos Cash reserves:" + "\n");
                                NPSYS.Device.HMC60.Print("------------------------------------\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "TYPE") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, "AMOUNT") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, "FEE") + "\n");
                                NPSYS.Device.HMC60.Print("------------------------------------\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "<TOTAL>") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(l_PrePossessionTotalCount)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(l_PrePossessionTotalMoney)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "1    PHP") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(Cahs50)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs50) * 1)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "5    PHP") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(Cahs100)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs100) * 5)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "10   PHP") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(Cahs500)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs500) * 10)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "20   PHP") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(Cahs1000)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs1000) * 20)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "50   PHP") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(Cahs5000)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs5000) * 50)) + "\n");
                                NPSYS.Device.HMC60.Print("------------------------------------\n");
                                NPSYS.Device.HMC60.Print("Current Cash reserves:" + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "TYPE") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, "AMOUNT") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, "FEE") + "\n");
                                NPSYS.Device.HMC60.Print("------------------------------------\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "<TOTAL>") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(l_NowPossessionTotalCount)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(l_NowPossessionTotalMoney)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "1    PHP") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(lbl50SettingQty.Text)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl50SettingQty.Text) * 1)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "5    PHP") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(lbl100SettingQty.Text)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl100SettingQty.Text) * 5)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "10   PHP") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(lbl500SettingQty.Text)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl500SettingQty.Text) * 10)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "20   PHP") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(lbl1000SettingQty.Text)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl1000SettingQty.Text) * 20)) + "\n");
                                NPSYS.Device.HMC60.Print(TextCore.ToLeftAlignString(8, "50   PHP") + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(8, TextCore.ToCommaString(lbl5000SettingQty.Text)) + TextCore.Space(valueSpace) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl5000SettingQty.Text) * 50)) + "\n");
                                NPSYS.Device.HMC60.Print("------------------------------------\n");
                                NPSYS.Device.HMC60.Print("Print Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n");
                            }
                        }
                        NPSYS.Device.HMC60.Feeding(25);
                        System.Threading.Thread.Sleep(200);
                        if (NPSYS.g_UsePrintFullCuting)
                        {
                            NPSYS.Device.HMC60.FullCutting();
                        }
                        else
                        {
                            NPSYS.Device.HMC60.ParticalCutting();
                        }

                    }
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   *" + "   " + " 시제금 내역*");
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   이전시제내역:");
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + "----------------------------------------------");
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + TextCore.ToLeftAlignString(10, "50       원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Cahs50)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs50) * 50)));
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + TextCore.ToLeftAlignString(10, "100      원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Cahs100)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs100) * 100)));
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + TextCore.ToLeftAlignString(10, "500      원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Cahs500)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs500) * 500)));
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + TextCore.ToLeftAlignString(10, "1000     원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Cahs1000)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs1000) * 1000)));
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + TextCore.ToLeftAlignString(10, "5000     원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Cahs5000)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(Cahs5000) * 5000)));
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "---------------------------------------------------");
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   현재 시제내역:");
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + TextCore.ToLeftAlignString(10, "종       류") + "     " + TextCore.ToRightAlignString(10, "수    량") + TextCore.Space(7) + TextCore.ToRightAlignString(10, "금    액"));
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + "----------------------------------------------");
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + TextCore.ToLeftAlignString(10, "50       원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(lbl50SettingQty.Text)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl50SettingQty.Text) * 50)));
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + TextCore.ToLeftAlignString(10, "100      원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(lbl100SettingQty.Text)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl100SettingQty.Text) * 100)));
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + TextCore.ToLeftAlignString(10, "500      원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(lbl500SettingQty.Text)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl500SettingQty.Text) * 500)));
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + TextCore.ToLeftAlignString(10, "1000     원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(lbl1000SettingQty.Text)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl1000SettingQty.Text) * 1000)));
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + TextCore.ToLeftAlignString(10, "5000     원") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(lbl5000SettingQty.Text)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(Convert.ToInt32(lbl5000SettingQty.Text) * 5000)));
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "---------------------------------------------------");
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "   " + "출력일시: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    NPSYS.Config.SetValue(ConfigID.Cash50SettingQty, lbl50SettingQty.Text);


                    NPSYS.Config.SetValue(ConfigID.Cash100SettingQty, lbl100SettingQty.Text);
                    NPSYS.Config.SetValue(ConfigID.Cash500SettingQty, lbl500SettingQty.Text);

                    NPSYS.Config.SetValue(ConfigID.Cash1000SettingQty, lbl1000SettingQty.Text);
                    NPSYS.Config.SetValue(ConfigID.Cash5000SettingQty, lbl5000SettingQty.Text);

                    int l_Held10 = 0;
                    int l_Held50 = Convert.ToInt32(lbl50SettingQty.Text);
                    int l_Held100 = Convert.ToInt32(lbl100SettingQty.Text);
                    int l_Held500 = Convert.ToInt32(lbl500SettingQty.Text);
                    int l_Held1000 = Convert.ToInt32(lbl1000SettingQty.Text);
                    int l_Held5000 = Convert.ToInt32(lbl5000SettingQty.Text);
                    int l_Held10000 = 0;
                    int l_Held50000 = 0;

                    //LPRDbSelect.Insert_PossesionMoney(l_Held10, l_Held50, l_Held100, l_Held500, l_Held1000, l_Held5000, l_Held10000, l_Held50000);
                    // 보유금을 어딘가로 보내야한다

                    NPSYS.Config.SetValue(ConfigID.Cash50MinQty, lbl50MinQty.Text);
                    NPSYS.Config.SetValue(ConfigID.Cash100MinQty, lbl100MinQty.Text);
                    NPSYS.Config.SetValue(ConfigID.Cash500MinQty, lbl500MinQty.Text);

                    NPSYS.Config.SetValue(ConfigID.Cash1000MinQty, lbl1000MinQty.Text);
                    NPSYS.Config.SetValue(ConfigID.Cash5000MinQty, lbl5000MinQty.Text);


                    NPSYS.Config.SetValue(ConfigID.Cash50MaxQty, lbl50MaxQty.Text);
                    NPSYS.Config.SetValue(ConfigID.Cash100MaxQty, lbl100MaxQty.Text);
                    NPSYS.Config.SetValue(ConfigID.Cash500MaxQty, lbl500MaxQty.Text);

                    NPSYS.Config.SetValue(ConfigID.Cash1000MaxQty, lbl1000MaxQty.Text);
                    NPSYS.Config.SetValue(ConfigID.Cash5000MaxQty, lbl5000MaxQty.Text);

                    NPSYS.LogAdminCash(
                        logDate, AdminCashLogType.Setting,
                        Convert.ToInt32(lbl50SettingQty.Text),
                        Convert.ToInt32(lbl100SettingQty.Text),
                        Convert.ToInt32(lbl500SettingQty.Text),
                        Convert.ToInt32(lbl1000SettingQty.Text),
                        Convert.ToInt32(lbl5000SettingQty.Text),
                        0,
                        0,
                        0,
                        0,
                        0,
                        ""
                        );

                        
                        if (NPSYS.Device.gIsUseCoinDischarger500Device)
                        {
                            NPSYS.Device.CoinDispensor500.reset();                          // 20120601  추가
                            applicationDoevent(30);
                            NPSYS.Device.CoinDispensor500.BaReadinessSignal();
                            applicationDoevent(10);
                        }
                        if (NPSYS.Device.gIsUseCoinDischarger100Device)
                        {

                            NPSYS.Device.CoinDispensor100.reset();
                            applicationDoevent(30);
                            NPSYS.Device.CoinDispensor100.BaReadinessSignal();
                            applicationDoevent(10);
                        }

                        if (NPSYS.Device.gIsUseCoinDischarger50Device)
                        {
                            NPSYS.Device.CoinDispensor50.reset();
                            applicationDoevent(30);
                            NPSYS.Device.CoinDispensor50.BaReadinessSignal();
                            applicationDoevent(10);
                        }
                        //3.1.74 동전방출기 50원100원500원설정기능 주석완료

                     
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOk_Click", "시제금보충버튼누름완료");
                }
                this.Close();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.Enabled = true;
            }
        }

    
        /// <summary>
        /// p_temrm은 1에 0.1초이며 p_term이 되면 작업을 마침
        /// </summary>
        /// <param name="p_term"></param>
        private void applicationDoevent(int p_term)
        {
            int i = 0;
            while (true)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
                i++;
                if (i > p_term)
                {
                    break;
                }

            }

        }

        private void AdminCashSetting_Actived(object sender, CancelEventArgs e)
        {
            Clear();

            ReadData();
            this.Enabled = true;

        }

        private void AdminCashSetting_Inactived(object sender, CancelEventArgs e)
        {
            tmrHome.Enabled = false;
        }

        private void btnOut50_Click(object sender, EventArgs e)
        {
            if (NPSYS.CurrentMoneyOutputType == ConfigID.MoneyOutputType.NotOutPut)
            {
                MessageBox.Show(new Form { TopMost = true }, "설정에서 방출불가로 방출안됨");
                return;
            }
            if (NPSYS.CurrentMoneyOutputType == ConfigID.MoneyOutputType.Question)
            {
                DialogResult outMessage = new FormMessageBox("50원 방출 :" + lbl50SettingQty.Text, "방출하시겠습니까?").ShowDialog();
                if (outMessage != DialogResult.OK)
                {
                    return;
                }
            }
            try
            {
                tmrHome.Enabled = false;

                this.Enabled = false;

                int outCoin = 0;
                int outchargeResult = 0;
                outCoin = (lbl50SettingQty.Text.Trim() == "" ? 1 : Convert.ToInt32(lbl50SettingQty.Text));
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOut50_Click", "50원불출누름| 숫자:" + outCoin.ToString());

                CoinDispensor.CoinDispensorStatusType C_CoinDispensortResult = NPSYS.OutChargeCoin(NPSYS.Device.CoinDispensor50, outCoin, ref outchargeResult);
                if (C_CoinDispensortResult ==  CoinDispensor.CoinDispensorStatusType.OK)
                {
                    TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "FormAdminCashSetting|btnOut50_Click", "50원불출| 숫자:" + TextCore.ToCommaString(outchargeResult));

                }
                else
                {
                    MessageBox.Show(new Form { TopMost = true }, "FormAdminCashSetting|btnOut50_Click:에러발생:" + C_CoinDispensortResult.ToString()+ " 실제불출숫자:" + outchargeResult.ToString());
                    NPSYS.Device.CoinDispensor50.reset();
                    applicationDoevent(40);
                    NPSYS.Device.CoinDispensor50.BaReadinessSignal();
                    applicationDoevent(10);

                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormAdminCashSetting|btnOut50_Click", ex.ToString());

            }
            finally
            {
                tmrHome.Enabled = true;
                this.Enabled = true;
            }

        }

        private void btnOut100_Click(object sender, EventArgs e)
        {
            if (NPSYS.CurrentMoneyOutputType == ConfigID.MoneyOutputType.NotOutPut)
            {
                MessageBox.Show(new Form { TopMost = true }, "설정에서 방출불가로 방출안됨");
                return;
            }
            if (NPSYS.CurrentMoneyOutputType == ConfigID.MoneyOutputType.Question)
            {
                DialogResult outMessage = new FormMessageBox("100원 방출 :" + lbl100SettingQty.Text, "방출하시겠습니까?").ShowDialog();
                if (outMessage != DialogResult.OK)
                {
                    return;
                }
            }
            try
            {

                tmrHome.Enabled = false;

                this.Enabled = false;

                int outCoin = 0;
                int outchargeResult = 0;
                outCoin = (lbl100SettingQty.Text.Trim() == "" ? 1 : Convert.ToInt32(lbl100SettingQty.Text));
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOut100_Click", "100원불출누름| 숫자:" + outCoin.ToString());
                CoinDispensor.CoinDispensorStatusType C_CoinDispensortResult = NPSYS.OutChargeCoin(NPSYS.Device.CoinDispensor100, outCoin, ref outchargeResult);

                if (C_CoinDispensortResult == CoinDispensor.CoinDispensorStatusType.OK)
                {
                    TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "FormAdminCashSetting|btnOut100_Click", "100원불출| 숫자" + TextCore.ToCommaString(outchargeResult));

                }
                else
                {
                    MessageBox.Show(new Form { TopMost = true }, "FormAdminCashSetting|btnOut100_Click:에러발생:" + C_CoinDispensortResult.ToString() +" 실제불출숫자:" + outchargeResult.ToString());
                    NPSYS.Device.CoinDispensor100.reset();
                    applicationDoevent(40);
                    NPSYS.Device.CoinDispensor100.BaReadinessSignal();
                    applicationDoevent(10);

                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormAdminCashSetting|btnOut100_Click", ex.ToString());
            }
            finally
            {

                tmrHome.Enabled = true;
                this.Enabled = true;

            }
        }

        private void btnOut500_Click(object sender, EventArgs e)
        {
            if (NPSYS.CurrentMoneyOutputType == ConfigID.MoneyOutputType.NotOutPut)
            {
                MessageBox.Show(new Form { TopMost = true }, "설정에서 방출불가로 방출안됨");
                return;
            }
            if (NPSYS.CurrentMoneyOutputType == ConfigID.MoneyOutputType.Question)
            {
                DialogResult outMessage = new FormMessageBox("500원 방출 :" + lbl500SettingQty.Text, "방출하시겠습니까?").ShowDialog();
                if (outMessage != DialogResult.OK)
                {
                    return;
                }
            }
            try
            {
                tmrHome.Enabled = false;

                this.Enabled = false;

                int outCoin = 0;
                int outchargeResult = 0;
                outCoin = (lbl500SettingQty.Text.Trim() == "" ? 1 : Convert.ToInt32(lbl500SettingQty.Text));
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOut500_Click", "500원불출누름| 숫자:" + outCoin.ToString());
                NPSYS.Device.CoinDispensor500.BaReadinessSignal();
                CoinDispensor.CoinDispensorStatusType C_CoinDispensortResult = NPSYS.OutChargeCoin(NPSYS.Device.CoinDispensor500, outCoin, ref outchargeResult);
                if (C_CoinDispensortResult== CoinDispensor.CoinDispensorStatusType.OK)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOut500_Click", "500원불출숫자:" + TextCore.ToCommaString(outchargeResult));
                    TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "FormAdminCashSetting|btnOut500_Click", "500원불출| 숫자:" + outCoin.ToString());


                }
                else
                {
                    MessageBox.Show(new Form { TopMost = true }, "FormAdminCashSetting|btnOut500_Click:에러발생:" + C_CoinDispensortResult.ToString() + "실제불출숫자:" + outchargeResult.ToString());
                    NPSYS.Device.CoinDispensor500.reset();
                    applicationDoevent(40);
                    NPSYS.Device.CoinDispensor500.BaReadinessSignal();
                    applicationDoevent(10);
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormAdminCashSetting|btnOut500_Click", ex.ToString());
            }
            finally
            {
                tmrHome.Enabled = true;
                this.Enabled = true;

            }
        }




        private void btnOutTest1000_Click(object sender, EventArgs e)
        {
            if (NPSYS.CurrentMoneyOutputType == ConfigID.MoneyOutputType.NotOutPut)
            {
                MessageBox.Show(new Form { TopMost = true }, "설정에서 방출불가로 방출안됨");
                return;
            }
            if (NPSYS.CurrentMoneyOutputType == ConfigID.MoneyOutputType.Question)
            {
                DialogResult outMessage = new FormMessageBox("1000원 방출 :" + lbl1000SettingQty.Text, "방출하시겠습니까?").ShowDialog();
                if (outMessage != DialogResult.OK)
                {
                    return;
                }
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOut1000_Click", "1000원불출누름:" + lbl1000SettingQty.Text);
            int bill1000qty = 1;
            try
            {

                bill1000qty = Convert.ToInt32(lbl1000SettingQty.Text);
            }
            catch
            {
                MessageBox.Show(new Form { TopMost = true }, "현재보유수량은 숫자만 가능합니다.");
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormAdminCashSetting|btnOutTest1000_Click", "불출실패:" + "현재보유수량은 숫자만 가능합니다.");
                return;
            }

            Result _result = MoneyBillOutDeviice.OutPut1000Qty(ref bill1000qty);
            if (_result.Success == false)
            {
                MessageBox.Show(new Form { TopMost = true }, "FormAdminCashSetting|btnOutTest1000_Click:불출실패:" + _result.Message + " 불출안된숫자:" + bill1000qty.ToString());
                TextCore.DeviceError(TextCore.DEVICE.BILLCHARGER, "FormAdminCashSetting|btnOutTest1000_Click", "불출실패:" + _result.Message + " 불출안된숫자:" + bill1000qty.ToString());
                NPSYS.Device.BillDischargeDeviceErrorMessage = _result.Message;
                return;

            }
            TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormAdminCashSetting|btnOutTest1000_Click", "1000원불출|숫자:" + bill1000qty.ToString());
        }


        private void btnOutTest5000_Click(object sender, EventArgs e)
        {
            if (NPSYS.CurrentMoneyOutputType == ConfigID.MoneyOutputType.NotOutPut)
            {
                MessageBox.Show(new Form { TopMost = true }, "설정에서 방출불가로 방출안됨");
                return;
            }
            if (NPSYS.CurrentMoneyOutputType == ConfigID.MoneyOutputType.Question)
            {
                DialogResult outMessage = new FormMessageBox("5000원 방출 :" + lbl5000SettingQty.Text, "방출하시겠습니까?").ShowDialog();
                if (outMessage != DialogResult.OK)
                {
                    return;
                }
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnOutTest5000_Click", "5000원불출누름:" + lbl5000SettingQty.Text);
            int bill5000qty = 1;
            try
            {

                bill5000qty = Convert.ToInt32(lbl5000SettingQty.Text);
            }
            catch
            {
                MessageBox.Show(new Form { TopMost = true }, "현재보유수량은 숫자만 가능합니다.");
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormAdminCashSetting|btnOutTest5000_Click", "불출실패:" + "현재보유수량은 숫자만 가능합니다.");
                return;
            }

            Result _result = MoneyBillOutDeviice.OutPut5000Qty(ref bill5000qty);
            if (_result.Success == false)
            {
                MessageBox.Show(new Form { TopMost = true }, "FormAdminCashSetting|btnOutTest5000_Click:불출실패:" + _result.Message + " 불출안된숫자:" + bill5000qty.ToString());
                TextCore.DeviceError(TextCore.DEVICE.BILLCHARGER, "FormAdminCashSetting|btnOutTest5000_Click", "불출실패:" + _result.Message + " 불출안된숫자:" + bill5000qty.ToString());
                NPSYS.Device.BillDischargeDeviceErrorMessage = _result.Message;
            }
            TextCore.DeviceError(TextCore.DEVICE.BILLCHARGER, "FormAdminCashSetting|btnOutTest5000_Click", "5000원불출숫자:" + lbl5000SettingQty.Text);
        }






        private void FormAdminCashSetting_Activated(object sender, EventArgs e)
        {
            Clear();
            lbl50SettingQty.PerformClick();
            ReadData();
            this.Enabled = true;

        }

        private void Label_Click(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                tmrHome.Enabled = false;
                tmrHome.Enabled = true;

                SimpleLabel control = sender as SimpleLabel;

                if (control != null)
                {
                    control.BackColor = Color.FromArgb(201, 201, 202);
                    npPad.LinkedSimpleLabel = control;
                }

                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is SimpleLabel)
                    {
                        if (ctrl != control)
                        {
                            ctrl.BackColor = Color.FromKnownColor(KnownColor.Window);
                        }
                    }
                }
            }
        }

        private void tmrHome_Tick(object sender, EventArgs e)
        {

        }






        private void button1_Click(object sender, EventArgs e)
        {
            NPSYS.Device.HMC60.FontSize(1, 1);
            NPSYS.Device.HMC60.Print("테스트\nABC\n123\n");
            NPSYS.Device.HMC60.FontBold(true);
            NPSYS.Device.HMC60.Print("굵게\nABC\n123\n");
            NPSYS.Device.HMC60.FontBold(false);
            NPSYS.Device.HMC60.Print("얇게\nABC\n123\n");
            NPSYS.Device.HMC60.FontSize(2, 1);
            NPSYS.Device.HMC60.Print("테스트\nABC\nabc\n123\n");
            NPSYS.Device.HMC60.FontSize(1, 2);
            NPSYS.Device.HMC60.Print("테스트\nABC\nabc\n123\n");
            NPSYS.Device.HMC60.FontSize(2, 2);
            NPSYS.Device.HMC60.Print("테스트\nABC\nabc\n123\n");
            NPSYS.Device.HMC60.FontSize(1, 1);
            NPSYS.Device.HMC60.FontReverse(true);
            NPSYS.Device.HMC60.Print("테스트\nABC\nabc\n123\n");
            NPSYS.Device.HMC60.FontReverse(false);
            NPSYS.Device.HMC60.Print("테스트\nABC\nabc\n123\n");
            NPSYS.Device.HMC60.Feeding(30);
            NPSYS.Device.HMC60.FullCutting();
        }

        private void picPrint2_Click(object sender, EventArgs e)
        {
  
            NPSYS.buttonSoundDingDong();
            string infoMsg = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_Q_CLOSING.ToString());
            string infoYes = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_TXT_ENTER.ToString());
            string infono = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_TXT_NO.ToString());
            DialogResult result = new FormMessageShortBoxYESNO(infoMsg, infoYes, infono).ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }

            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnprint_Click", "마감 버튼 누름");
            picMagamSave.BackColor = Color.Silver;
            try
            {
                this.Enabled = false;
                Print.PrintMagam(true, true, (string info, string err) =>
                {
                    FormMessageShortBox mFormMessageShortBox = new FormMessageShortBox(info, err);
                    mFormMessageShortBox.ShowDialog();
                });
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormAdminCashSetting|PrintMagam", "마감중오류" + ex.ToString());

            }
            finally
            {

                this.Enabled = true;
            }
            picMagamSave.BackColor = Color.Transparent;

        }

        private void picPrint_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();
            
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|btnprint_Click", "마감 내용 확인 버튼 누름");
            picMagamDataInfo.BackColor = Color.Silver;
            try
            {
                this.Enabled = false;
                Print.PrintMagam(false, true, (string info, string err) =>
                {
                    FormMessageShortBox mFormMessageShortBox = new FormMessageShortBox(info, err);
                    mFormMessageShortBox.ShowDialog();
                });
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormAdminCashSetting|PrintMagam", "마감중오류" + ex.ToString());

            }
            finally
            {

                this.Enabled = true;
            }
            picMagamDataInfo.BackColor = Color.Transparent;


        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Result _result = MoneyBillOutDeviice.MoneyBillStatus();
            MessageBox.Show(new Form { TopMost = true }, _result.Success.ToString() + _result.Message);
        }



        private void FormAdminCashSetting_FormClosing(object sender, FormClosingEventArgs e)
        {


        }

        private void TextChangedEvent(object sender, EventArgs e)
        {
                NPSYS.buttonSoundDingDong();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            Print.PrintMagam(true, false, (string info, string err) =>
            {
                FormMessageShortBox mFormMessageShortBox = new FormMessageShortBox(info, err);
                mFormMessageShortBox.ShowDialog();
            });
        }

        private void FormAdminCashSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            NPSYS.CurrentFormType = mPreFomrType;
        }




        #region 언어변환
        /// <summary>
        /// 언어변경
        /// </summary>
        private void SetLanguage(ConfigID.LanguageType pLanguageType)
        {

            NPSYS.Config.SetValue(ConfigID.FeatureSettingLanguage, pLanguageType.ToString()); // 메인폼에서 받아서 저장한다

            Control[] currentControl = GetAllControlsUsingRecursive(this);
            foreach (Control controlItem in currentControl)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditMain | SetLanguage", controlItem.Name.ToString());
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
            SetLanuageDynamic(pLanguageType);

        }

        private void SetLanuageDynamic(ConfigID.LanguageType pLanguageType)
        {
            lblMoney_2TypeName.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_TXT_MONEY_2TYPENAME.ToString());
            lblMoney_3TypeName.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_TXT_MONEY_3TYPENAME.ToString());
            lblMoney_4TypeName.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_TXT_MONEY_4TYPENAME.ToString());
            lblMoney_5TypeName.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_TXT_MONEY_5TYPENAME.ToString());
            lblMoney_6TypeName.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_TXT_MONEY_6TYPENAME.ToString());
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

