using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace NPAutoBooth.UI.BoothUC
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class Ctl9by16Information : InformationUC
    {
        public override event EventHandler PreForm_Click;

        public Ctl9by16Information()
        {
            InitializeComponent();
        }

        public override string FirstMessage 
        {
            get => lbl_Msg1.Text;
            set
            {
                lbl_Msg1.Text = "";
                lbl_Msg1.Text = value;
            }
        }

        public override string SecondMessage 
        {
            get => lbl_Msg2.Text;
            set
            {
                lbl_Msg2.Text = "";
                lbl_Msg2.Text = value;
            }
        }

        public override bool FirstMessageVisible 
        {
            get => lbl_Msg1.Visible;
            set => lbl_Msg1.Visible = value;
        }

        public override bool SecondMessageVisible 
        {
            get => lbl_Msg2.Visible;
            set => lbl_Msg2.Visible = value;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void btnPreForm_Click(object sender, EventArgs e)
        {
            if (PreForm_Click != null) PreForm_Click(sender, e);
        }
    }
}
