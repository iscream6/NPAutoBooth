using FadeFox.Text;
using NPAutoBooth.Common;
using NPCommon;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace NPAutoBooth.UI.BoothUC
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class Ctl9by16SelectCar : SelectCarUC
    {
        private int GoConfigSequence = 0;
        private Color InitTextColor;

        public override event BoothCommonLib.Event_ConfigCall ConfigCall;
        public override event EventHandler PreForm_Click;
        public override event EventHandler Home_Click;
        public override event BoothCommonLib.Event_LanguageChange LanguageChange_Click;
        public override event EventHandler PrePage_Click;
        public override event EventHandler NextPage_Click;
        public override event BoothCommonLib.Event_CarSelected Car_Selected;

        public override string TotalPage { get => lblPageTotal.Text; set => lblPageTotal.Text = value; }
        public override string CurrentPage { get => lblPageCurrent.Text; set => lblPageCurrent.Text = value; }

        public override bool PrevPageEnable { get => btnPrevPage.Enabled; set => btnPrevPage.Enabled = value; }
        public override bool NextPageEnable { get => btnNextPage.Enabled; set => btnNextPage.Enabled = value; }

        public Ctl9by16SelectCar()
        {
            InitializeComponent();

            InitTextColor = lbl_OneCarnumber.ForeColor;
        }
        
        public override void SetCarInfo(ENUM_CarIndex carIndex, SelectCarInfo carInfo)
        {
            switch (carIndex)
            {
                case ENUM_CarIndex.One:
                    btn_OneCarimage.Image = carInfo.carImage;
                    lbl_OneCarnumber.Text = carInfo.carNumber;
                    lblOneIndate.Text = carInfo.carInDateTime;
                    break;
                case ENUM_CarIndex.Two:
                    btn_TwoCarimage.Image = carInfo.carImage;
                    lbl_TwoCarnumber.Text = carInfo.carNumber;
                    lblTwoIndate.Text = carInfo.carInDateTime;
                    break;
                case ENUM_CarIndex.Three:
                    btn_ThreeCarimage.Image = carInfo.carImage;
                    lbl_ThreeCarnumber.Text = carInfo.carNumber;
                    lblThreeIndate.Text = carInfo.carInDateTime;
                    break;
            }
        }

        public override void SetCarInfoEnable(ENUM_CarIndex carIndex, bool enabled)
        {
            switch (carIndex)
            {
                case ENUM_CarIndex.One:
                    btn_OneCarimage.Enabled = enabled;
                    break;
                case ENUM_CarIndex.Two:
                    btn_TwoCarimage.Enabled = enabled;
                    break;
                case ENUM_CarIndex.Three:
                    btn_ThreeCarimage.Enabled = enabled;
                    break;
            }
        }

        public override void SetCarInfoVisible(ENUM_CarIndex carIndex, bool visibled)
        {
            switch (carIndex)
            {
                case ENUM_CarIndex.One:
                    btn_OneCarimage.Visible = visibled;
                    lbl_OneCarnumber.Visible = visibled;
                    lblOneIndate.Visible = visibled;
                    break;
                case ENUM_CarIndex.Two:
                    btn_TwoCarimage.Visible = visibled;
                    lbl_TwoCarnumber.Visible = visibled;
                    lblTwoIndate.Visible = visibled;
                    break;
                case ENUM_CarIndex.Three:
                    btn_ThreeCarimage.Visible = visibled;
                    lbl_ThreeCarnumber.Visible = visibled;
                    lblThreeIndate.Visible = visibled;
                    break;
            }
        }

        public override void SetCarInfoColor(ENUM_CarIndex carIndex, Color color)
        {
            switch (carIndex)
            {
                case ENUM_CarIndex.One:
                    lbl_OneCarnumber.ForeColor = color;
                    lblOneIndate.ForeColor = color;
                    break;
                case ENUM_CarIndex.Two:
                    lbl_TwoCarnumber.ForeColor = color;
                    lblTwoIndate.ForeColor = color;
                    break;
                case ENUM_CarIndex.Three:
                    lbl_ThreeCarnumber.ForeColor = color;
                    lblThreeIndate.ForeColor = color;
                    break;
            }
        }

        public override void Clear()
        {
            //차량번호 초기화
            lbl_OneCarnumber.Text = string.Empty;
            lbl_TwoCarnumber.Text = string.Empty;
            lbl_ThreeCarnumber.Text = string.Empty;

            //입차일시 초기화
            lblOneIndate.Text = string.Empty;
            lblTwoIndate.Text = string.Empty;
            lblThreeIndate.Text = string.Empty;

            //이미지 초기화
            btn_OneCarimage.Image = null;
            btn_TwoCarimage.Image = null;
            btn_ThreeCarimage.Image = null;

            //Color 초기화
            InitializeTextColor();

            //선택차량번호 초기화
            SelectedCarNumber = "";
        }

        public override void Initialize()
        {
            if (NPSYS.gUseMultiLanguage)
            {
                btnEnglish.Visible = true;
                btnJapan.Visible = true;
            }
            else
            {
                btnEnglish.Visible = false;
                btnJapan.Visible = false;
            }

            if (NPSYS.gIsAutoBooth)
            {
                btn_TXT_BACK.Visible = false;
                btn_TXT_HOME.Visible = false;
            }

            SelectedCarNumber = "";
        }

        public void InitializeTextColor()
        {
            lbl_OneCarnumber.ForeColor = InitTextColor;
            lblOneIndate.ForeColor = InitTextColor;

            lbl_TwoCarnumber.ForeColor = InitTextColor;
            lblOneIndate.ForeColor = InitTextColor;

            lbl_ThreeCarnumber.ForeColor = InitTextColor;
            lblOneIndate.ForeColor = InitTextColor;
        }

        #region Event Handle 처리

        /// <summary>
        /// 영문
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnglish_Click(object sender, EventArgs e)
        {
            if (LanguageChange_Click != null) LanguageChange_Click(BoothCommonLib.LanguageType.ENGLISH);
        }

        /// <summary>
        /// 일본어
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJapan_Click(object sender, EventArgs e)
        {
            if (LanguageChange_Click != null) LanguageChange_Click(BoothCommonLib.LanguageType.JAPAN);
        }

        /// <summary>
        /// 차량정보를 선택할 때 발생하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarInfo_click(object sender, EventArgs e)
        {
            if (Car_Selected != null)
            {
                SelectedCarNumber = "";

                //어디서 눌린건지 판독을 하자. PictureBox 이거나 Label에서 눌렸을거다. 어차피 둘다 Control 이다.
                Control c = sender as Control;
                if (c != null)
                {
                    switch (c.Name)
                    {
                        case "btnOneCarConfrim":
                        case "btn_OneCarimage":
                        case "lbl_OneCarnumber":
                            //1번 차량 선택
                            SelectedCarNumber = lbl_OneCarnumber.Text;
                            Car_Selected(1);
                            break;
                        case "btnTwoCarConfrim":
                        case "btn_TwoCarimage":
                        case "lbl_TwoCarnumber":
                            //2번 차량 선택
                            SelectedCarNumber = lbl_TwoCarnumber.Text;
                            Car_Selected(2);
                            break;
                        case "btnThreeCarConfrim":
                        case "btn_ThreeCarimage":
                        case "lbl_ThreeCarnumber":
                            //3번 차량 선택
                            SelectedCarNumber = lbl_ThreeCarnumber.Text;
                            Car_Selected(3);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 이전 페이지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            InitializeTextColor();
            if (PrePage_Click != null) PrePage_Click(sender, e);
        }

        /// <summary>
        /// 다음 페이지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            InitializeTextColor();
            if (NextPage_Click != null) NextPage_Click(sender, e);
        }

        /// <summary>
        /// 이전 화면
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TXT_BACK_Click(object sender, EventArgs e)
        {
            if (PreForm_Click != null) PreForm_Click(sender, e);
        }

        /// <summary>
        /// 처음 화면
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TXT_HOME_Click(object sender, EventArgs e)
        {
            if (Home_Click != null) Home_Click(sender, e);
        }

        private void panel_ConfigClick1_Click(object sender, EventArgs e)
        {
            GoConfigSequence = 1;
            NPSYS.buttonSoundDingDong();
        }

        private void panel_ConfigClick2_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();

            if (GoConfigSequence == 1)
            {
                if (ConfigCall != null)
                {
                    ConfigCall();
                    TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu | panel_ConfigClick2_Click", "메인화면으로 강제로 이동시킴");
                }
                GoConfigSequence = 0;
            }
            else
            {
                GoConfigSequence = 0;
            }
        }

        #endregion
    }
}
