using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NPAutoBooth.Common;
using NPCommon;

namespace NPAutoBooth
{
    public partial class UserControl_SearchCar : UserControl
    {
        private const string defaultFont = "옥션고딕 B, 110F";

        public UserControl_SearchCar()
        {
            InitializeComponent();
            lblFiveCarNumber.Parent = pic_SelectArrow;
            lblFourCarNumber.Parent = pic_SelectArrow;
            lblThreeCarNumber.Parent = pic_SelectArrow;
            lblTwoCarNumber.Parent = pic_SelectArrow;
            lblOneCarNumber.Parent = pic_SelectArrow;

        }
        private void UserControl_SearchCar_Load(object sender, EventArgs e)
        {
            if (NPSYS.CurrentCarNumType == NPCommon.ConfigID.CarNumberType.Digit4SetAUTO
             || NPSYS.CurrentCarNumType == NPCommon.ConfigID.CarNumberType.Digit4SetEnt)
            {
                lblFiveCarNumber.Visible = false;
                lblOneCarNumber.Location = new Point(61, 9);
                lblTwoCarNumber.Location = new Point(226, 9);
                lblThreeCarNumber.Location = new Point(401, 9);
                lblFourCarNumber.Location = new Point(584, 9);
            }
            else if (NPSYS.CurrentCarNumType == NPCommon.ConfigID.CarNumberType.Digit5Free)
            {
                lblFiveCarNumber.Visible = true;
                lblOneCarNumber.Location = new Point(20, 9);
                lblTwoCarNumber.Location = new Point(170, 9);
                lblThreeCarNumber.Location = new Point(320, 9);
                lblFourCarNumber.Location = new Point(470, 9);
                lblFiveCarNumber.Location = new Point(620,9);
            }
        }

        public string SetOneText
        {
            set { lblOneCarNumber.Text = value; }
            get { return lblOneCarNumber.Text; }
        }
        public string SetTwoText
        {
            set { lblTwoCarNumber.Text = value; }
            get { return lblTwoCarNumber.Text; }
        }

        public string SetThreeText
        {
            set { lblThreeCarNumber.Text = value; }
            get { return lblThreeCarNumber.Text; }
        }

        public string SetFourText
        {
            set { lblFourCarNumber.Text = value; }
            get { return lblFourCarNumber.Text; }
        }

        public string SetFiveText
        {
            set { lblFiveCarNumber.Text = value; }
            get { return lblFiveCarNumber.Text; }
        }

        [DefaultValue(typeof(Font), defaultFont)]
        public Font NumberFont
        {
            get
            {
                return lblOneCarNumber.Font;
            }
            set
            {
                lblOneCarNumber.Font = value;
                lblTwoCarNumber.Font = value;
                lblThreeCarNumber.Font = value;
                lblFourCarNumber.Font = value;
                lblFiveCarNumber.Font = value;
                Invalidate();
            }
        }
    }
}
