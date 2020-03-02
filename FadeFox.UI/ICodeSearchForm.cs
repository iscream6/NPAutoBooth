/*
 * ==============================================================================
 *   Program ID     :
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   :
 *   Developer      :
 *   Create Date    : 2009-07-14
 * ------------------------------------------------------------------------------
 *   Update History
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace FadeFox.UI
{
	public interface ICodeSearchForm
	{
		// 테이블 명
		string TableName
		{
			get;
		}

		string CodeField
		{
			get;
		}

		string CodeNameField
		{
			get;
		}

		string SelectedCode
		{
			get;
		}

		string SelectedCodeName
		{
			get;
		}

		Dictionary<string, string> StoredRowData
		{
			get;
		}

		string GetCodeName(string pCode);

		object GetCodeInfo(string pCode);
	}
}
