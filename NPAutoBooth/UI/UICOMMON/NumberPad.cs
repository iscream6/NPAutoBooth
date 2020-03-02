using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FadeFox.UI;

namespace NPAutoBooth.UI
{
	public partial class NumberPad : UserControl
	{
		private SimpleLabel mLinkedSimpleLabel = null;

		public SimpleLabel LinkedSimpleLabel
		{
			get { return mLinkedSimpleLabel; }
			set
			{
				mLinkedSimpleLabel = value;

				if (mLinkedSimpleLabel != null)
					mValue = mLinkedSimpleLabel.Text;

				ActionScreen();
			}
		}

		private string mValue = "";
		public string Value
		{
			get { return mValue; }
			set { mValue = value; }
		}

		private int mLimitLength = 10;
		public int LimitLength
		{
			get { return mLimitLength; }
			set 
			{
				if (mLimitLength < 1)
					mLimitLength = 1;
				else
					mLimitLength = value;
			}
		}

		private bool mIsNumber = true;
		public bool IsNumber
		{
			get { return mIsNumber; }
			set { mIsNumber = value; }
		}

		public NumberPad()
		{
			InitializeComponent();
		}

		public void Clear()
		{
			mValue = "";

			ActionScreen();

			OnCleared(this, new EventArgs());
			OnValueChanged(this, new EventArgs());
		}

		private void ActionScreen()
		{
			if (string.IsNullOrEmpty(mValue))
			{
				btnB.Enabled = false;
				btnC.Enabled = false;
			}
			else
			{
				btnB.Enabled = true;
				btnC.Enabled = true;
			}

			if (mValue.Length < mLimitLength)
			{
				btn0.Enabled = true;
				btn1.Enabled = true;
				btn2.Enabled = true;
				btn3.Enabled = true;
				btn4.Enabled = true;
				btn5.Enabled = true;
				btn6.Enabled = true;
				btn7.Enabled = true;
				btn8.Enabled = true;
				btn9.Enabled = true;
			}
			else
			{
				btn0.Enabled = false;
				btn1.Enabled = false;
				btn2.Enabled = false;
				btn3.Enabled = false;
				btn4.Enabled = false;
				btn5.Enabled = false;
				btn6.Enabled = false;
				btn7.Enabled = false;
				btn8.Enabled = false;
				btn9.Enabled = false;
			}
		}

		private void Button_Click(object sender, EventArgs e)
		{
			ImageButton btn = sender as ImageButton;

			if (btn != null)
			{
				string pressChar = btn.Name.Substring(3, 1);
				switch (pressChar)
				{
					case "B": // Back
						if (!string.IsNullOrEmpty(mValue))
						{
							mValue = mValue.Substring(0, mValue.Length - 1);

							OnValueChanged(this, new EventArgs());

							ActionScreen();
						}
						break;
					case "C": // Clear
                        this.Clear();

                        break;
					default:
						if (IsNumber)
						{
							if (pressChar == "0") // 숫자 0을 눌렀을 경우
							{
								if (mValue == "" || mValue == "0")
								{
									mValue = "0";
								}
								else
								{
									mValue += pressChar;
								}
							}
							else // 0이외의 숫자를 눌렀을 경우
							{
								if (mValue == "0") // 현재 입력되어 있는 값이 0이면 0은 없애 버리고 입력된 숫자를 입력
								{
									mValue = pressChar;
								}
								else
								{
									mValue += pressChar;
								}
							}
						}
						else
						{
							mValue += pressChar;
						}

						ActionScreen();

						OnValueChanged(this, new EventArgs());
						break;
				}
			}
		}

		private event EventHandler mValueChanged = null;
		public event EventHandler ValueChanged
		{
			add { mValueChanged += value; }
			remove { mValueChanged -= value; }
		}
		protected virtual void OnValueChanged(object sender, EventArgs e)
		{
			if (mLinkedSimpleLabel != null)
			{
				mLinkedSimpleLabel.Text = mValue;
			}

			if (mValueChanged != null)
				mValueChanged(sender, e);
		}

		private event EventHandler mCleared = null;
		public event EventHandler Cleared
		{
			add { mCleared += value; }
			remove { mCleared -= value; }
		}
		protected virtual void OnCleared(object sender, EventArgs e)
		{
			if (mCleared != null)
				mCleared(sender, e);
		}

  
    }
}
