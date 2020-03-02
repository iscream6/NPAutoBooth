/*
 * ==============================================================================
 *   Program ID     :
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   :
 *   Developer      :
 *   Create Date    : 2009-10-18
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 *       http://sourceforge.net/projects/myxls/
 * ==============================================================================
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Drawing;

namespace FadeFox.Document
{
	#region Farthest side of the western ocean
	//A Gorgon class - For the love of Zeus don't look directly at it!
	//Hides reflection used to call Excel. Reflection is used so Excel doesn't have to be on the dev machine
	public class Excel2003
	{
		const string ASSEMBLY2003 = "Microsoft.Office.Interop.Excel, Version=11.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c";

		#region Medusa
		public class Worksheet
		{
			object mWorksheet;
			Type mType;
			object mCells;
			PropertyInfo mCellIndex;

			public Worksheet()
			{
				object excel = mAssemblyExcel.GetType("Microsoft.Office.Interop.Excel.ApplicationClass").GetConstructor(new Type[] { }).Invoke(new object[] { });
				object workbooks = excel.GetType().GetProperty("Workbooks").GetValue(excel, null);
				object workbook = mAssemblyExcel.GetType("Microsoft.Office.Interop.Excel.Workbooks").GetMethod("Add").Invoke(workbooks, new object[] { Type.Missing });
				object worksheets = workbook.GetType().GetProperty("Worksheets").GetValue(workbook, null);
				object worksheet = mAssemblyExcel.GetType("Microsoft.Office.Interop.Excel.Sheets").GetMethod("get_Item").Invoke(worksheets, new object[] { 1 });

				mWorksheet = worksheet;
				mType = Excel2003.mAssemblyExcel.GetType("Microsoft.Office.Interop.Excel._Worksheet");
				mCells = mType.GetProperty("Cells").GetValue(mWorksheet, null);
				mCellIndex = Excel2003.mAssemblyExcel.GetType("Microsoft.Office.Interop.Excel.Range").GetProperty("Item", new Type[] { typeof(int), typeof(int) });
			}

			public string Name
			{
				get { return (string)mType.GetProperty("Name").GetValue(mWorksheet, null); }
				set { mType.GetProperty("Name").SetValue(mWorksheet, value, null); }
			}

			public Cell this[object pRow, object pColumn]
			{
				get { return new Cell(mCellIndex.GetValue(mCells, new object[] { pRow, pColumn })); }
			}

			public bool IsApplicationVisible
			{
				get
				{
					object app = mType.GetProperty("Application").GetValue(mWorksheet, null);
					return (bool)mAssemblyExcel.GetType("Microsoft.Office.Interop.Excel._Application").GetProperty("Visible").GetValue(app, null);
				}
				set
				{
					object app = mType.GetProperty("Application").GetValue(mWorksheet, null);
					mAssemblyExcel.GetType("Microsoft.Office.Interop.Excel._Application").GetProperty("Visible").SetValue(app, value, null);
				}
			}

			public void Close()
			{
				object app = mType.GetProperty("Application").GetValue(mWorksheet, null);
				mAssemblyExcel.GetType("Microsoft.Office.Interop.Excel._Application").GetMethod("Quit").Invoke(app, null);
			}
		}
		#endregion

		#region Stheno
		public class Cell
		{
			static Type mType;
			static PropertyInfo mValue,
								mNumberFormat,
								mFont,
								mEntireColumn;
			static MethodInfo mAutoFit;
			object mRange;

			static Cell()
			{
				mType = mAssemblyExcel.GetType("Microsoft.Office.Interop.Excel.Range");

				mValue = mType.GetProperty("Value2");
				mNumberFormat = mType.GetProperty("NumberFormat");
				mFont = mType.GetProperty("Font");
				mEntireColumn = mType.GetProperty("EntireColumn");

				mAutoFit = mType.GetMethod("AutoFit");
			}

			public Cell(object pCell)
			{
				mRange = pCell;
			}

			public object Value
			{
				get { return mValue.GetValue(mRange, null); }
				set { mValue.SetValue(mRange, value, null); }
			}

			public object NumberFormat
			{
				get { return mNumberFormat.GetValue(mRange, null); }
				set { mNumberFormat.SetValue(mRange, value, null); }
			}

			public Font Font
			{
				get { return new Font(mFont.GetValue(mRange, null)); }
			}

			public void AutoFitEntireColumn()
			{
				object entireColumn = mEntireColumn.GetValue(mRange, null);
				mAutoFit.Invoke(entireColumn, null);
			}
		}
		#endregion

		#region Euryale
		public class Font
		{
			static Type mType;
			static PropertyInfo mBold;
			static PropertyInfo mForeColor;
			static PropertyInfo mSize;
			object mFont;

			static Font()
			{
				mType = mAssemblyExcel.GetType("Microsoft.Office.Interop.Excel.Font");
				mBold = mType.GetProperty("Bold");
				mForeColor = mType.GetProperty("Color");
				mSize = mType.GetProperty("Size");
			}

			public Font(object pFont)
			{
				mFont = pFont;
			}

			public bool Bold
			{
				get { return (bool)mBold.GetValue(mFont, null); }
				set { mBold.SetValue(mFont, value, null); }
			}

			public double ForeColor
			{
				get
				{
					double color = (double)mForeColor.GetValue(mFont, null);
					//object size = mSize.GetValue(mFont, null);
					//MsgBox.Show(size.GetType().FullName);
					return color;
				}
				//set { mForeColor.SetValue(mFont, value, null); }
			}

			public double Size
			{
				get { return (double)mSize.GetValue(mFont, null); }
				set { mSize.SetValue(mFont, value, null); }
			}
		}
		#endregion

		static bool? mIsInstalled;
		static Assembly mAssemblyExcel;
		public static bool IsInstalled
		{
			get
			{
				if (mIsInstalled == null)
					mIsInstalled = IsAssemblyInstalled(ASSEMBLY2003);

				return (bool)mIsInstalled;
			}
		}

		static bool IsAssemblyInstalled(string pAssembly)
		{
			try
			{
				mAssemblyExcel = Assembly.Load(pAssembly);
				
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
	#endregion
}
