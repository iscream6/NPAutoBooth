using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Drawing;

namespace FadeFox.UI.Wizard
{
	public delegate void WizardPageActivedEventHandler(WizardPage page);

	public class WizardSheet
	{
		private List<WizardPage> mPages = new List<WizardPage>();
		private WizardPage mActivePage = null;
		private WizardPage mMainPage = null;
		private Panel mPagePanel = null;

		public event WizardPageActivedEventHandler WizardPageActived;
		private void OnWizardPageActived(WizardPage page)
		{
			if (WizardPageActived != null)
			{
				WizardPageActived(page);
			}
		}

		public WizardSheet(Panel pPagePanel)
		{
			mPagePanel = pPagePanel;
		}

		public int Count
		{
			get { return mPages.Count; }
		}

		public void Clear()
		{
			mPages.Clear();
		}


		public WizardPage Add(WizardPage pPage, string pName)
		{
			this.Add(pPage, pName, "", "", null);

			return pPage;
		}

		public WizardPage Add(WizardPage pPage, string pName, string pBackgroundImagePath)
		{
			this.Add(pPage, pName, "", pBackgroundImagePath, null);

			return pPage;
		}

		public WizardPage Add(WizardPage pPage, string pName, string pBackgroundImagePath, Image pDefaultBackgroundImage)
		{
			this.Add(pPage, pName, "", pBackgroundImagePath, pDefaultBackgroundImage);

			return pPage;
		}

		public WizardPage Add(WizardPage pPage, string pName, string pSubject, string pBackgroundImagePath, Image pDefaultBackgroundImage)
		{
			pPage.Name = pName;
			pPage.Subject = pSubject;
			pPage.Sheet = this;

			if (pPage.BackgroundPictureBox != null && pBackgroundImagePath != "")
			{
				if (File.Exists(pBackgroundImagePath))
				{
					Image img = Image.FromFile(pBackgroundImagePath);
					pPage.BackgroundPictureBox.Image = img;
				}
				else
				{
					pPage.BackgroundPictureBox.Image = pDefaultBackgroundImage;
				}
			}

			mPages.Add(pPage);

			return pPage;
		}

		public bool IsContain(string pName)
		{
			WizardPage page = Find(pName);

			if (page == null)
				return false;
			else
				return true;
		}

		// 
		// 인덱서
		//
		public WizardPage this[string pName]
		{
			get
			{
				if (mPages.Count > 0)
				{
					for (int i = 0; i < mPages.Count; i++)
					{
						if (mPages[i].Name == pName)
							return mPages[i];
					}

					return null;
				}
				else
				{
					return null;
				}
			}
		}

		public WizardPage this[int pIndex]
		{
			get
			{
				if (mPages.Count > 0)
					return mPages[pIndex];
				else
					return null;
			}
		}

		public WizardPage ActivePage
		{
			get { return mActivePage; }
		}

		public WizardPage MainPage
		{
			get { return mMainPage; }
			set { mMainPage = value; }
		}

		private void ResizeToFit()
		{
			foreach (WizardPage page in mPages)
			{
				page.Size = mPagePanel.Size;
			}
		}

		public int ActivePageIndex
		{
			get 
			{
				for (int i = 0; i < mPages.Count; i++)
				{
					if (mActivePage == mPages[i])
						return i;
				}

				return -1;
			}
		}

		public WizardPage Active(int pPageIndex)
		{
			if (pPageIndex < 0 || ActivePageIndex >= mPages.Count)
				throw new ArgumentOutOfRangeException("pPageIndex");

			WizardPage page = mPages[pPageIndex];
			return Active(page);
		}

		public WizardPage Find(string pName)
		{
			foreach (WizardPage page in mPages)
			{
				if (page.Name == pName)
					return page;
			}

			return null;
		}

		public WizardPage Active(string pName)
		{
			WizardPage page = Find(pName);

			if (page == null)
				throw new Exception(string.Format("페이지를 찾을 수 없습니다. {0}", pName));

			return Active(page);
		}

		public WizardPage Active(WizardPage pPage)
		{
			if (pPage == null)
				throw new Exception(string.Format("페이지를 찾을 수 없습니다"));

			WizardPage prevActivePage = mActivePage;

			// 입력된 페이지가 존재하지 않은 페이지 이면
			// 컨트롤 콜렉션에 추가
			if (!mPagePanel.Controls.Contains(pPage))
				mPagePanel.Controls.Add(pPage);

			// 현 페이지 표시
			pPage.Visible = true;
			pPage.Dock = DockStyle.Fill;

			mActivePage = pPage;

			CancelEventArgs e = new CancelEventArgs();

			pPage.OnActived(e);

			this.OnWizardPageActived(mActivePage);

			if (e.Cancel)
			{
				pPage.Visible = false;
				mActivePage = prevActivePage;
			}

			// 다른 페이지 들은 전부 숨김
			foreach (WizardPage page in mPages)
			{
				if (page != mActivePage)
				{
					page.Visible = false;

					page.OnInactived(e);
				}
			}

			return pPage;

			//if (!String.IsNullOrEmpty(mActivePage.PageName))
		}

		private WizardPageEventArgs PreChangePage(int pDelta)
		{
			int activeIndex = ActivePageIndex;
			int nextIndex = activeIndex + pDelta;

			if (nextIndex < 0 || nextIndex >= mPages.Count)
				nextIndex = activeIndex;

			WizardPage newPage = mPages[nextIndex];

			WizardPageEventArgs e = new WizardPageEventArgs { NewPage = newPage.Name, Cancel = false };

			return e;
		}

		private void PostChangePage(WizardPageEventArgs e)
		{
			if (!e.Cancel)
				Active(e.NewPage);
		}

		public void GoNextPage()
		{
			WizardPageEventArgs wpea = PreChangePage(+1);
			mActivePage.OnWizardNext(wpea);
			PostChangePage(wpea);
		}

		public void GoBackPage()
		{
			WizardPageEventArgs wpea = PreChangePage(-1);
			mActivePage.OnWizardBack(wpea);
			PostChangePage(wpea);
		}

		public void GoFinish()
		{
			CancelEventArgs cea = new CancelEventArgs();
			mActivePage.OnWizardFinish(cea);
			if (cea.Cancel)
				return;
		}

		public void Initialize()
		{
			foreach (WizardPage page in mPages)
			{
				page.Clear();
			}

			if (mMainPage != null)
				mMainPage.Active();
			else
			{
				// 모든 페이지 들을 숨김
				foreach (WizardPage page in mPages)
				{
					page.Visible = false;
				}
			}
		}
	}
}
