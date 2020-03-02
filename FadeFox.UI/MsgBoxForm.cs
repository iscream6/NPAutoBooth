using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FadeFox.Utility;

namespace FadeFox.UI
{
	internal partial class MsgBoxForm : Form
	{
		public MsgBoxForm(string pMessage, string pTitle, MsgType pType)
		{
			InitializeComponent();

			this.Text = pTitle;

			txtMessage.Text = pMessage.Replace("\r\n", "\n").Replace("\n", "\r\n");
			txtMessage.Select(0, 0);

			switch (pType)
			{
				case MsgType.Information:
					btnYes.Visible = false;
					btnNo.Visible = false;
					btnOk.Visible = true;
					this.CancelButton = btnOk;
					this.picIcon.Image = global::FadeFox.UI.Properties.Resources.MsgBoxInformation;
					break;
				case MsgType.Warning:
					btnYes.Visible = false;
					btnNo.Visible = false;
					btnOk.Visible = true;
					this.CancelButton = btnOk;
					this.picIcon.Image = global::FadeFox.UI.Properties.Resources.MsgBoxWarning;
					break;
				case MsgType.Error:
					btnYes.Visible = false;
					btnNo.Visible = false;
					btnOk.Visible = true;
					this.CancelButton = btnOk;
					this.picIcon.Image = global::FadeFox.UI.Properties.Resources.MsgBoxError;
					break;
				case MsgType.Question:
					btnYes.Visible = true;
					btnNo.Visible = true;
					btnOk.Visible = false;
					this.CancelButton = btnNo;
					this.picIcon.Image = global::FadeFox.UI.Properties.Resources.MsgBoxQuestion;
					break;
				case MsgType.Normal:
					btnYes.Visible = false;
					btnNo.Visible = false;
					btnOk.Visible = true;
					this.CancelButton = btnOk;
					this.picIcon.Image = null;
					break;
			}
		}

		private void MsgBoxForm_Load(object sender, EventArgs e)
		{

		}
	}
}
