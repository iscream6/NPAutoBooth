using System;
using System.Collections.Generic;
using System.Text;

namespace NPAutoBooth.Common
{
    /// <summary>
    /// 시리얼 장비들에 물리적인 이름을 지정하여 DB에서 가져올수 있게한다.
    /// </summary>
	public class SerialPortID
	{
        public const string TmoneyCardReader = "SMART_CARD_READER"; // 교통카드 리드
        public const string CoinReader = "COIN_READER"; // 동전 리더기
        public const string CoinCharger50 = "COIN_CHARGER50"; // 동전 거스름
        public const string CoinCharger100 = "COIN_CHARGER100"; // 동전 거스름
        public const string CoinCharger500 = "COIN_CHARGER500"; // 동전 거스름
        public const string BarcodeSerialReader = "BARCODE_SERIALREADER"; // 바코드 리드
        public const string CreditCardReader_1 = "CREDIT_CARD_READER_1"; // 신용카드 리더기1
        public const string CreditCardReader_2 = "CREDIT_CARD_READER_2"; // 신용카드 리더기2
        public const string ReceiptPrint = "RECEIPT_PRINT"; // 영수증 프린트
        public const string BillCharger = "BILL_CHARGER"; // 지폐 거스름
        public const string BillReader = "BILL_READER"; // 지폐 리드
        public const string Dido = "DIDO"; // 정산기 불빛 제어
        public const string KungChar = "KUNGCHA"; //경차인식기
        public const string SerialKeyboard = "SERIALKEYBOARD"; //시리얼키보드
        //견광등 적용
        public const string Kungandong = "KUNGANDONG"; // 견광등 
        //견광등 적용완료
        //전동어닝 제어 적용
        public const string ElecAwning = "ELEC_AWNING"; // 전동어닝 
        //전동어닝 제어 적용완료
    }
}
