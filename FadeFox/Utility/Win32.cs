/*
 * ==============================================================================
 *   Program ID     : 
 *   Program Name   : 
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : 
 *   Developer      :
 *   Create Date    : 2010-11-26
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FadeFox.Utility
{
	public class Win32
	{
		public enum SystemMessage
		{
			SM_CXMIN = 28
		};

		public const int SM_CXMIN = 28;

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetSystemMetrics(int nIndex);
	}
}
