using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace FadeFox.UI
{
	public enum SimpleListBoxItemType
	{
		Text,
		Line
	}

	[ToolboxBitmap(typeof(System.Windows.Forms.ListBox))]
	public partial class SimpleListBox : UserControl
	{
		public SimpleListBox()
		{
			InitializeComponent();
		}

		private void lstMessage_DrawItem(object sender, DrawItemEventArgs e)
		{
			DrawItem((ListBox)sender, e.Graphics, e.Bounds, e.Index);
		}


		protected void DrawItem(ListBox pListBox, Graphics pGraphics, Rectangle pBounds, int pIndex)
		{
			if (pIndex == -1)
				return;

			if (pBounds.Top < 0)
				return;

			if (pBounds.Bottom > (pListBox.Bottom + pListBox.ItemHeight))
				return;

			SimpleListBoxItem item = pListBox.Items[pIndex] as SimpleListBoxItem;

			if (item.ItemType == SimpleListBoxItemType.Text)
			{
				using (Brush b = new SolidBrush(item.ForeColor))
				{
					pGraphics.DrawString(item.Text, pListBox.Font, b, pBounds.Left + 1, pBounds.Top + 3);
				}

				if (item.DrawLine == true)
				{
					using (Pen p = new Pen(item.LineColor))
					{
						pGraphics.DrawLine(p, pBounds.Left, pBounds.Bottom - 1, pBounds.Right, pBounds.Bottom - 1);
					}
				}
			}
			else
			{
				using (Pen p = new Pen(item.LineColor))
				{
					int offset = (lstMessage.ItemHeight % 2 == 0) ? lstMessage.ItemHeight / 2 : (lstMessage.ItemHeight - 1) / 2;
					pGraphics.DrawLine(p, pBounds.Left, pBounds.Bottom - 1 - offset, pBounds.Right, pBounds.Bottom - 1 - offset);
				}
			}
		}

		public int Count
		{
			get { return lstMessage.Items.Count; }
		}

		public Font ItemFont
		{
			get { return lstMessage.Font; }
			set { lstMessage.Font = value; }
		}

		public int ItemHeight
		{
			get { return lstMessage.ItemHeight; }
			set { lstMessage.ItemHeight = value; }
		}

		public void Clear()
		{
			lstMessage.Items.Clear();
		}

		public int Add(SimpleListBoxItem pItem)
		{
			return lstMessage.Items.Add(pItem);
		}

		public int Add(string pMessage)
		{
			return Add(pMessage, Color.Black, false);
		}

		public int Add(string pMessage, Color pForeColor)
		{
			return Add(pMessage, pForeColor, false);
		}

		public int Add(string pMessage, Color pForeColor, bool pDrawLine)
		{
			SimpleListBoxItem item = new SimpleListBoxItem(pMessage, pForeColor, pDrawLine);

			return lstMessage.Items.Add(item);
		}

		public int AddLine()
		{
			return AddLine(Color.Gray);
		}

		public int AddLine(Color pLineColor)
		{
			SimpleListBoxItem item = new SimpleListBoxItem(pLineColor);

			return lstMessage.Items.Add(item);
		}

		// 마지막 라인으로 스크롤 시킴
		public void DoScroll()
		{
			lstMessage.SelectedIndex = lstMessage.Items.Count - 1;
		}

		public void Insert(int pIndex, SimpleListBoxItem pItem)
		{
			lstMessage.Items.Insert(pIndex, pItem);
		}

		public void Insert(int pIndex, string pMessage)
		{
			Insert(pIndex, pMessage, Color.Black, false);
		}

		public void Insert(int pIndex, string pMessage, Color pForeColor)
		{
			Insert(pIndex, pMessage, pForeColor, false);
		}

		public void Insert(int pIndex, string pMessage, Color pForeColor, bool pDrawLine)
		{
			SimpleListBoxItem item = new SimpleListBoxItem(pMessage, pForeColor, pDrawLine);

			lstMessage.Items.Insert(pIndex, item);
		}

		public void Insert(SimpleListBoxItem pItem)
		{
			Insert(0, pItem);
		}

		public void Insert(string pMessage)
		{
			Insert(0, pMessage);
		}

		public void Insert(string pMessage, Color pForeColor)
		{
			Insert(0, pMessage, pForeColor);
		}

		public void Insert(string pMessage, Color pForeColor, bool pDrawLine)
		{
			Insert(0, pMessage, pForeColor, pDrawLine);
		}

		public void Remove()
		{
			lstMessage.Items.RemoveAt(0);
		}
		


	}

	public class SimpleListBoxItem
	{
		public string Text = string.Empty;
		public Color ForeColor = Color.Black;
		public Color LineColor = Color.Gray;
		public bool DrawLine = false;
		public SimpleListBoxItemType ItemType = SimpleListBoxItemType.Text;
		public object Tag = null;

		public SimpleListBoxItem(Color pLineColor)
		{
			this.ItemType = SimpleListBoxItemType.Line;
			this.LineColor = pLineColor;
		}

		public SimpleListBoxItem(string pText, Color pForeColor, bool pDrawLine)
		{
			this.Text = pText;
			this.ForeColor = pForeColor;
			this.DrawLine = pDrawLine;
		}

		public override string ToString()
		{
			try
			{
				return Text;
			}
			catch
			{
				return "NULL";
			}
		}
	}
}
