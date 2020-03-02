using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace FadeFox.UI.Wizard
{
	public partial class WizardPage : UserControl
	{
		public string Subject { get; set; }
		protected WizardSheet mSheet = null;
		protected PictureBox mBackgroundPictureBox = null;

		protected WizardPage()
		{
			InitializeComponent();
		}

		protected WizardPage(string pName, WizardSheet pSheet)
			: this()
		{
			this.Name = pName;
			this.mSheet = pSheet;
		}

		protected WizardPage(string pName, string pSubject, WizardSheet pSheet)
			: this(pName, pSheet)
		{
			this.Subject = pSubject;
		}

		public WizardSheet Sheet
		{
			get { return mSheet; }
			set { mSheet = value; }
		}

		/// <summary>
		/// 배경으로 사용하는 Picture Box
		/// </summary>
		public PictureBox BackgroundPictureBox
		{
			get { return mBackgroundPictureBox; }
			set { mBackgroundPictureBox = value; }
		}

		/// <summary>
		/// 화면 활성화
		/// </summary>
		public void Active()
		{
			mSheet.Active(this.Name);
		}

		[Category("Wizard")]
		public event CancelEventHandler Actived;

		public virtual void OnActived(CancelEventArgs e)
		{
			this.Focus();

			if (Actived != null)
				Actived(this, e);
		}

		[Category("Wizard")]
		public event CancelEventHandler Inactived;

		public virtual void OnInactived(CancelEventArgs e)
		{
			if (Inactived != null)
				Inactived(this, e);
		}

		[Category("Wizard")]
		public event WizardPageEventHandler WizardNext;

		public virtual void OnWizardNext(WizardPageEventArgs e)
		{
			if (WizardNext != null)
				WizardNext(this, e);
		}

		[Category("Wizard")]
		public event WizardPageEventHandler WizardBack;

		public virtual void OnWizardBack(WizardPageEventArgs e)
		{
			if (WizardBack != null)
				WizardBack(this, e);
		}

		[Category("Wizard")]
		public event CancelEventHandler WizardFinish;

		public virtual void OnWizardFinish(CancelEventArgs e)
		{
			if (WizardFinish != null)
				WizardFinish(this, e);
		}

		[Category("Wizard")]
		public event CancelEventHandler QueryCancel;

		public virtual void OnQueryCancel(CancelEventArgs e)
		{
			if (QueryCancel != null)
				QueryCancel(this, e);
		}

		public virtual void Clear()
		{

		}
	}

	public class WizardPageEventArgs : CancelEventArgs
	{
		public string NewPage { get; set; }
	}

	public delegate void WizardPageEventHandler(object sender, WizardPageEventArgs e);
}
