using System;
using System.Collections.Generic;
using System.Text;

namespace NPCommon
{
	public class Parkinglots
	{
		List<ParkinglotItem> mList = new List<ParkinglotItem>();

		/// <summary>
		/// 추가
		/// </summary>
		/// <param name="pItem"></param>
		public void Add(ParkinglotItem pItem)
		{
			mList.Add(pItem);
		}

		/// <summary>
		/// 추가
		/// </summary>
		/// <param name="pParkinglotNo"></param>
		/// <param name="pParkingLotName"></param>
		public void Add(
			string pParkinglotNo,
			string pParkingLotName
			)
		{
			this.Add(new ParkinglotItem(pParkinglotNo, pParkingLotName));
		}

		/// <summary>
		/// 갯수 리턴
		/// </summary>
		public int Count
		{
			get { return mList.Count; }
		}

		/// <summary>
		/// 초기화
		/// </summary>
		public void Clear()
		{
			mList.Clear();
		}

		public ParkinglotItem this[string pParkinglotNo]
		{
			get
			{
				foreach (ParkinglotItem item in mList)
				{
					if (item.ParkingLotNo == pParkinglotNo)
					{
						return item;
					}
				}

				return null;
			}
		}
	}

	public class ParkinglotItem
	{
		/// <summary>
		/// 주차장번호
		/// </summary>
		public string ParkingLotNo
		{
			get;
			set;
		}

		/// <summary>
		/// 주차장명
		/// </summary>
		public string ParkinglotName
		{
			get;
			set;
		}

		public ParkinglotItem(
			string pParkinglotNo,
			string pParkingLotName
			)
		{
			ParkingLotNo = pParkinglotNo;
			ParkinglotName = pParkingLotName;
		}
	}
}
