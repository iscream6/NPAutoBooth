using FadeFox.Text;
using NPCommon.DTO;
using NPCommon.DTO.Receive;
using NPCommon.IO;
using NPCommon.REST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon
{
    public class Print
    {
        public static int printLenghtHmc054(string pData, int pLen)
        {
            return pLen - Encoding.Default.GetByteCount(pData);
        }

        private static HttpProcess mHttpProcess = new HttpProcess();

        /// <summary>
        /// 보관증 출력 동전이 안될때
        /// </summary>
        /// <param name="pInfo"></param>
        /// <param name="pIsCharge">true면 정산후 나머지 금액에 대한 금액, false면 현재 투입된 금액에 대한 반환금액</param>
        /// <summary>
        /// 보관증 출력 동전이 안될때
        /// </summary>
        /// <param name="pInfo"></param>
        /// <param name="pIsCharge">true면 정산후 나머지 금액에 대한 금액, false면 현재 투입된 금액에 대한 반환금액</param>
        public static void CashTicketNotCoinPrint(NormalCarInfo _NormalCarInfo, bool pIsCharge, string pScreenName)
        {
            try
            {
                string dY_UNIT_PARKNAME = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_PARKNAME.ToString());
                string dY_UNIT_BUSINESSNUMBER = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_BUSINESSNUMBER.ToString());
                string dY_UNIT_ADDRESS = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_ADDRESS.ToString());
                string dY_UNIT_TELNO = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_TELNO.ToString());
                string dY_UNIT_UNITNAME = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_UNITNAME.ToString());
                string dY_UNIT_MACHINNAME1 = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_MACHINNAME1.ToString());
                string dY_UNIT_MACHINNAME2 = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_MACHINNAME2.ToString());
                string dY_UNIT_CARNO = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_CARNO.ToString());
                string dY_UNIT_INDATE = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_INDATE.ToString());
                string dY_UNIT_DAYS = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_DAYS.ToString());
                string dY_UNIT_HOURS = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_HOURS.ToString());
                string dY_UNIT_MINUTE = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_MINUTE.ToString());
                string dY_UNIT_PARKINGFEE = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_PARKINGFEE.ToString());
                string dY_UNIT_DISCOUNTFEE = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_DISCOUNTFEE.ToString());
                string dY_UNIT_COIN_PUT_IN = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_COIN_PUT_IN.ToString());
                string dY_PAYDATE = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_PAYDATE.ToString());
                string dY_PARKINGTIME = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_PARKINGTIME.ToString());
                string dY_UNIT_CASH = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_CASH.ToString());
                string dY_UNIT_CASHNAME = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_CASHNAME.ToString());
                string dY_UNIT_REMARK = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_REMARK.ToString());// 비고
                string dY_TXT_LACKOFCHANGE = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_TXT_LACKOFCHANGE.ToString());// 거스름돈이 부족합니다
                string dY_TXT_HELP_MANAGER = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_TXT_HELP_MANAGER.ToString());// 관리자에게 주세요
                string dY_UNIT_LACKOFCHANGE = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_LACKOFCHANGE.ToString());// 거스름돈 부족
                string dY_UNIT_CASH_DEPOSIT = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_CASH_DEPOSIT.ToString());// 현금 보관증
                string dY_UNIT_SHORT_CHANGE = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_SHORT_CHANGE.ToString());// 거스름돈 부족액
                string dY_UNIT_LACKOFCHANGE_AFTER_PAYMENT = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_LACKOFCHANGE_AFTER_PAYMENT.ToString());// 결제후 거스름돈 부족
                string dY_UNIT_RETURN_INPUT_AMOUNT = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_RETURN_INPUT_AMOUNT.ToString());// 투입금액 반환
                string dY_UNIT_CHANGE = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_CHANGE.ToString());
                string dY_UNIT_PREPAYD = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_PREPAYD.ToString());

                if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
                {
                    int valueSpace = 38;
                    NPSYS.Device.HMC60.FontSize(2, 2);
                    NPSYS.Device.HMC60.Print("     보  관  증\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "     보  관  증");
                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print("   주차장:" + NPSYS.Config.GetValue(ConfigID.ParkingLotName) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   주차장:" + NPSYS.Config.GetValue(ConfigID.ParkingLotName));
                    NPSYS.Device.HMC60.Print("   사업자:" + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   사업자:" + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo));
                    NPSYS.Device.HMC60.Print("   주  소:" + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   주  소:" + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress));
                    NPSYS.Device.HMC60.Print("   전화번호:" + NPSYS.Config.GetValue(ConfigID.ParkingLotTelNo) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   전화번호:" + NPSYS.Config.GetValue(ConfigID.ParkingLotTelNo));
                    NPSYS.Device.HMC60.Print("   정산기명:" + NPSYS.BoothName + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   정산기명:" + NPSYS.BoothName);
                    NPSYS.Device.HMC60.Print("   =================================================\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   =================================================");
                    NPSYS.Device.HMC60.FontSize(2, 1);
                    string lBoothName = (NPSYS.gIsAutoBooth == true ? " *출차일반(무인)*" : " *사전일반(무인)*");
                    NPSYS.Device.HMC60.Print(" *출차일반(무인)*\n\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " *출차일반(무인)*");
                    NPSYS.Device.HMC60.Print(" 차량 번호" + TextCore.ToRightAlignString(15, _NormalCarInfo.OutCarNo1) + "\n\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " 차량 번호" + TextCore.ToRightAlignString(15, _NormalCarInfo.OutCarNo1));
                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print("   입차일시" + TextCore.ToRightAlignString(valueSpace, NPSYS.ConvetYears_Dash(_NormalCarInfo.InYmd.Trim()) + " " + NPSYS.ConvetDay_Dash(_NormalCarInfo.InHms.Trim())) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   입차일시" + TextCore.ToRightAlignString(valueSpace, NPSYS.ConvetYears_Dash(_NormalCarInfo.InYmd.Trim()) + " " + NPSYS.ConvetDay_Dash(_NormalCarInfo.InHms.Trim())));
                    NPSYS.Device.HMC60.Print("   정산일시" + TextCore.ToRightAlignString(valueSpace, NPSYS.ConvetYears_Dash(_NormalCarInfo.OutYmd) + " " + NPSYS.ConvetDay_Dash(_NormalCarInfo.OutHms)) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   정산일시" + TextCore.ToRightAlignString(valueSpace, NPSYS.ConvetYears_Dash(_NormalCarInfo.OutYmd) + " " + NPSYS.ConvetDay_Dash(_NormalCarInfo.OutHms)));
                    NPSYS.Device.HMC60.Print("   주차시간" + TextCore.ToRightAlignString(valueSpace, string.Format("{0}일 {1}시간 {2}분", _NormalCarInfo.ElapsedDay, _NormalCarInfo.ElapsedHour, _NormalCarInfo.ElapsedMinute)) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   주차시간" + TextCore.ToRightAlignString(valueSpace, string.Format("{0}일 {1}시간 {2}분", _NormalCarInfo.ElapsedDay, _NormalCarInfo.ElapsedHour, _NormalCarInfo.ElapsedMinute)));
                    NPSYS.Device.HMC60.Print(" 총주차요금" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(_NormalCarInfo.TotFee) + "원") + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " 총주차요금" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(_NormalCarInfo.TotFee) + "원"));
                    NPSYS.Device.HMC60.Print(" 총할인요금" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(_NormalCarInfo.TotDc) + "원") + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " 총할인요금" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(_NormalCarInfo.TotDc) + "원"));

                    NPSYS.Device.HMC60.Print("   -------------------------------------------------\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   -------------------------------------------------");
                    NPSYS.Device.HMC60.FontSize(2, 1);

                    NPSYS.Device.HMC60.FontSize(1, 1);

                    NPSYS.Device.HMC60.Print("   투입금액" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(_NormalCarInfo.InComeMoney) + "원") + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   투입금액" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(_NormalCarInfo.InComeMoney) + "원"));

                    if (pIsCharge)
                    {
                        NPSYS.Device.HMC60.Print("   거스름돈 부족액" + TextCore.ToRightAlignString(valueSpace - 7, TextCore.ToCommaString(_NormalCarInfo.GetNotDisChargeMoney) + "원") + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   거스름돈 부족액" + TextCore.ToRightAlignString(valueSpace - 7, TextCore.ToCommaString(_NormalCarInfo.GetNotDisChargeMoney) + "원"));
                        NPSYS.Device.HMC60.Print("   비    고" + TextCore.ToRightAlignString(valueSpace, "정산후 거스름돈 부족") + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   비    고" + TextCore.ToRightAlignString(valueSpace, "정산후 거스름돈 부족"));
                    }
                    else
                    {
                        NPSYS.Device.HMC60.Print("   거스름돈 부족액" + TextCore.ToRightAlignString(valueSpace - 7, TextCore.ToCommaString(_NormalCarInfo.GetNotDisChargeMoney) + "원") + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   거스름돈 부족액" + TextCore.ToRightAlignString(valueSpace - 7, TextCore.ToCommaString(_NormalCarInfo.GetNotDisChargeMoney) + "원"));
                        NPSYS.Device.HMC60.Print("   비    고" + TextCore.ToRightAlignString(valueSpace, "투입금액 반환") + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   비    고" + TextCore.ToRightAlignString(valueSpace, "투입금액 반환"));
                    }

                    NPSYS.Device.HMC60.Print("   =================================================\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   =================================================");
                    NPSYS.Device.HMC60.Print("   " + DateTime.Now.ToString("yyyy-MM-dd") + TextCore.ToRightAlignString(valueSpace - 2, NPSYS.Config.GetValue(ConfigID.ParkingLotBoothID) + " 정산기") + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   " + DateTime.Now.ToString("yyyy-MM-dd") + TextCore.ToRightAlignString(valueSpace - 2, NPSYS.Config.GetValue(ConfigID.ParkingLotBoothID) + " 정산기"));
                    NPSYS.Device.HMC60.Print("   죄송합니다.\n거스름돈이 부족하오니 주차관리실로 가셔서\n거스름돈을 받으세요.");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   죄송합니다. 거스름돈이 부족하오니 주차관리실로 가셔서 거스름돈을 받으세요.");
                }
                else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
                {
                    int valueSpaceHmk054 = 38 - 13;
                    int defaultLent = 8;
                    NPSYS.Device.HMC60.FontSize(2, 2);
                    NPSYS.Device.HMC60.Print(dY_UNIT_CASH_DEPOSIT + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_CASH_DEPOSIT);
                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print(dY_UNIT_PARKNAME + ": " + NPSYS.Config.GetValue(ConfigID.ParkingLotName) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_PARKNAME + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotName));
                    NPSYS.Device.HMC60.Print(dY_UNIT_BUSINESSNUMBER + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_BUSINESSNUMBER + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo));
                    NPSYS.Device.HMC60.Print(dY_UNIT_ADDRESS + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_ADDRESS + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress));
                    NPSYS.Device.HMC60.Print(dY_UNIT_TELNO + ": " + NPSYS.Config.GetValue(ConfigID.ParkingLotTelNo) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_TELNO + ": " + NPSYS.Config.GetValue(ConfigID.ParkingLotTelNo));
                    NPSYS.Device.HMC60.Print(dY_UNIT_UNITNAME + ":" + NPSYS.BoothName + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_UNITNAME + ": " + NPSYS.BoothName);
                    NPSYS.Device.HMC60.Print("====================================\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   =================================================");
                    NPSYS.Device.HMC60.FontSize(2, 1);
                    string lBoothName = (NPSYS.gIsAutoBooth == true ? " *" + dY_UNIT_MACHINNAME1 + "*" : " *" + dY_UNIT_MACHINNAME2 + "*");
                    NPSYS.Device.HMC60.Print(lBoothName + "*\n\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " *" + lBoothName + "*");
                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print(dY_UNIT_CARNO + TextCore.ToRightAlignString(valueSpaceHmk054, _NormalCarInfo.OutCarNo1) + "\n\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_CARNO + TextCore.ToRightAlignString(15, _NormalCarInfo.OutCarNo1));
                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print(dY_UNIT_INDATE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_INDATE, defaultLent), NPSYS.ConvetYears_Dash(_NormalCarInfo.InYmd.Trim()) + " " + NPSYS.ConvetDay_Dash(_NormalCarInfo.InHms.Trim())) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_INDATE + TextCore.ToRightAlignString(valueSpaceHmk054, NPSYS.ConvetYears_Dash(_NormalCarInfo.InYmd.Trim()) + " " + NPSYS.ConvetDay_Dash(_NormalCarInfo.InHms.Trim())));

                    NPSYS.Device.HMC60.Print(dY_PAYDATE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_PAYDATE, defaultLent), NPSYS.ConvetYears_Dash(_NormalCarInfo.OutYmd.Trim()) + " " + NPSYS.ConvetDay_Dash(_NormalCarInfo.OutHms.Trim()).SafeSubstring(0, 5)) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_PAYDATE + TextCore.ToRightAlignString(valueSpaceHmk054, NPSYS.ConvetYears_Dash(_NormalCarInfo.OutYmd.Trim()) + " " + NPSYS.ConvetDay_Dash(_NormalCarInfo.OutHms.Trim()).SafeSubstring(0, 5)));

                    NPSYS.Device.HMC60.Print(dY_PARKINGTIME + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_PARKINGTIME, defaultLent), string.Format("{0}" + dY_UNIT_DAYS + " {1}" + dY_UNIT_HOURS + " {2}" + dY_UNIT_MINUTE, _NormalCarInfo.ElapsedDay, _NormalCarInfo.ElapsedHour, _NormalCarInfo.ElapsedMinute)) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_PARKINGTIME + TextCore.ToRightAlignString(valueSpaceHmk054, string.Format(" {0}" + dY_UNIT_DAYS + " {1}" + dY_UNIT_HOURS + " {2}" + dY_UNIT_MINUTE, _NormalCarInfo.ElapsedDay, _NormalCarInfo.ElapsedHour, _NormalCarInfo.ElapsedMinute)));

                    NPSYS.Device.HMC60.Print(dY_UNIT_PARKINGFEE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_PARKINGFEE, defaultLent), TextCore.ToCommaString(_NormalCarInfo.TotFee) + dY_UNIT_CASHNAME) + "\n");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_PARKINGFEE + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(_NormalCarInfo.TotFee) + dY_UNIT_CASHNAME));

                    NPSYS.Device.HMC60.Print(dY_UNIT_DISCOUNTFEE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_DISCOUNTFEE, defaultLent), TextCore.ToCommaString(_NormalCarInfo.TotDc) + dY_UNIT_CASHNAME) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_DISCOUNTFEE + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(_NormalCarInfo.TotDc) + dY_UNIT_CASHNAME));

                    NPSYS.Device.HMC60.Print(dY_UNIT_PREPAYD + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_PREPAYD, defaultLent), TextCore.ToCommaString(_NormalCarInfo.RecvAmt) + dY_UNIT_CASHNAME) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_PREPAYD + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(_NormalCarInfo.RecvAmt) + dY_UNIT_CASHNAME));

                    NPSYS.Device.HMC60.Print("------------------------------------\n");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   -------------------------------------------------");
                    NPSYS.Device.HMC60.FontSize(1, 1);

                    NPSYS.Device.HMC60.Print(dY_UNIT_COIN_PUT_IN + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_COIN_PUT_IN, defaultLent), TextCore.ToCommaString(_NormalCarInfo.GetInComeMoney) + dY_UNIT_CASHNAME) + "\n");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_COIN_PUT_IN + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(_NormalCarInfo.GetInComeMoney) + dY_UNIT_CASHNAME));

                    NPSYS.Device.HMC60.Print(dY_UNIT_CHANGE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_COIN_PUT_IN, defaultLent), TextCore.ToCommaString(_NormalCarInfo.GetOutComeMoney) + dY_UNIT_CASHNAME) + "\n");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_CHANGE + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(_NormalCarInfo.GetOutComeMoney) + dY_UNIT_CASHNAME));

                    if (pIsCharge)
                    {
                        NPSYS.Device.HMC60.Print(dY_UNIT_SHORT_CHANGE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_SHORT_CHANGE, defaultLent), TextCore.ToCommaString(_NormalCarInfo.GetNotDisChargeMoney) + dY_UNIT_CASHNAME) + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_SHORT_CHANGE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_SHORT_CHANGE, defaultLent), TextCore.ToCommaString(_NormalCarInfo.GetNotDisChargeMoney) + dY_UNIT_CASHNAME));
                        NPSYS.Device.HMC60.Print(dY_UNIT_REMARK + TextCore.ToRightAlignString(valueSpaceHmk054, dY_UNIT_LACKOFCHANGE_AFTER_PAYMENT) + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_REMARK + TextCore.ToRightAlignString(valueSpaceHmk054, dY_UNIT_LACKOFCHANGE_AFTER_PAYMENT));
                    }
                    else
                    {
                        NPSYS.Device.HMC60.Print(dY_UNIT_SHORT_CHANGE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_SHORT_CHANGE, defaultLent), TextCore.ToCommaString(_NormalCarInfo.GetNotDisChargeMoney) + dY_UNIT_CASHNAME) + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_SHORT_CHANGE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_SHORT_CHANGE, defaultLent), TextCore.ToCommaString(_NormalCarInfo.GetNotDisChargeMoney) + dY_UNIT_CASHNAME));
                        NPSYS.Device.HMC60.Print(dY_UNIT_REMARK + TextCore.ToRightAlignString(valueSpaceHmk054, dY_UNIT_RETURN_INPUT_AMOUNT) + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_REMARK + TextCore.ToRightAlignString(valueSpaceHmk054, dY_UNIT_RETURN_INPUT_AMOUNT));
                    }

                    NPSYS.Device.HMC60.Print("====================================\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   =================================================");
                    NPSYS.Device.HMC60.Print(DateTime.Now.ToString("yyyy-MM-dd") + TextCore.ToRightAlignString(valueSpaceHmk054 - 2, NPSYS.Config.GetValue(ConfigID.ParkingLotBoothID) + dY_UNIT_UNITNAME) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   " + DateTime.Now.ToString("yyyy-MM-dd") + TextCore.ToRightAlignString(valueSpaceHmk054 - 2, NPSYS.Config.GetValue(ConfigID.ParkingLotBoothID) + dY_UNIT_UNITNAME));
                    NPSYS.Device.HMC60.Print(dY_TXT_LACKOFCHANGE + "\n");
                    NPSYS.Device.HMC60.Print(dY_TXT_HELP_MANAGER + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_TXT_LACKOFCHANGE + dY_TXT_HELP_MANAGER);
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS|CashTicketNotCoinPrint", "보관중 출력중 예외사항" + ex.ToString());
            }
            finally
            {
                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
                {
                    NPSYS.Device.HMC60.Feeding(25);
                    System.Threading.Thread.Sleep(200);
                    if (NPSYS.g_UsePrintFullCuting)
                    {
                        NPSYS.Device.HMC60.FullCutting();
                    }
                    else
                    {
                        NPSYS.Device.HMC60.ParticalCutting();
                    }
                }
            }
        }

        public static void PrintMagam(bool P_Update_Yn, bool pIsManualMagam, Action<string, string> messageFormAction)
        {
            NPHttpControl mNPHttpControl = new NPHttpControl();
            ParkingReceiveData restClassParser = new ParkingReceiveData();
            Close currentClose = new Close();
            NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.closeInfo;
            mNPHttpControl.SetHeader(currnetUrl, string.Empty, string.Empty);

            currentClose = (Close)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMethodGet());

            if (currentClose.status.Success == false)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS | PrintMagam", "마감조회에 실패하였습니다. 원인:" + currentClose.status.message);
                if (pIsManualMagam == true)
                {
                    string infoErrorMsg = string.Empty;
                    if (currentClose.status.currentStatus == Status.BodyStatus.CloseInfo_NotFound)
                    {
                        infoErrorMsg = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_ERR_CLOSE_NOTFOUND.ToString());
                    }
                    else if (currentClose.status.currentStatus == Status.BodyStatus.CloseInfo_BadRequest)
                    {
                        infoErrorMsg = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_ERR_CLOSE_NOTDEVICE.ToString());
                    }
                    else
                    {
                        infoErrorMsg = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_TXT_UNKNOWNERR.ToString()) + "CODE:" + currentClose.status.code;
                    }

                    string infoErrorButton = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_TXT_ENTER.ToString());
                    messageFormAction(infoErrorMsg, infoErrorButton);
                    return;
                }
                return;
            }
            if (P_Update_Yn)
            {
                Close saveReturnCloseData = mHttpProcess.SaveClose(currentClose);
                if (saveReturnCloseData.status.Success == false)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS | PrintMagam", "마감저장에 실패하였습니다. 원인:" + saveReturnCloseData.status.message);
                    if (pIsManualMagam == true)
                    {
                        string infoErrorMsg = string.Empty;
                        if (currentClose.status.currentStatus == Status.BodyStatus.CloseSave_Duplicate)
                        {
                            infoErrorMsg = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_ERR_CLOSE_DUPLICATE.ToString());
                        }
                        else if (currentClose.status.currentStatus == Status.BodyStatus.CloseSave_BadRequest)
                        {
                            infoErrorMsg = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_ERR_CLOSE_NOTDEVICE.ToString());
                        }
                        else
                        {
                            infoErrorMsg = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_TXT_UNKNOWNERR.ToString()) + "CODE:" + currentClose.status.code;
                        }

                        string infoErrorButton = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_TXT_ENTER.ToString());
                        messageFormAction(infoErrorMsg, infoErrorButton);
                        return;
                    }
                    return;
                }
                else
                {
                    if (pIsManualMagam == true)
                    {
                        string infoSucessMsg = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_CLOSE_COMPLETE.ToString());
                        string infoSucessButton = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MSG_TXT_ENTER.ToString());
                        messageFormAction(infoSucessMsg, infoSucessButton);
                        string nowYetdate = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string savecloseDate = nowYetdate.SafeSubstring(0, 4) + "-" + nowYetdate.SafeSubstring(4, 2) + "-" + nowYetdate.SafeSubstring(6, 2) + " " + nowYetdate.SafeSubstring(8, 2) + ":" + nowYetdate.SafeSubstring(10, 2) + ":" + nowYetdate.SafeSubstring(12, 2);
                        NPSYS.Config.SetValue(ConfigID.MagameEndDate, savecloseDate);
                    }
                    else
                    {
                        string nowYetdate = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string savecloseDate = nowYetdate.SafeSubstring(0, 4) + "-" + nowYetdate.SafeSubstring(4, 2) + "-" + nowYetdate.SafeSubstring(6, 2) + " " + nowYetdate.SafeSubstring(8, 2) + ":" + nowYetdate.SafeSubstring(10, 2) + ":" + nowYetdate.SafeSubstring(12, 2);
                        NPSYS.Config.SetValue(ConfigID.MagameEndDate, savecloseDate);
                    }
                }
            }
            string olddate = string.Empty;
            string Nowdate = string.Empty;
            olddate = NPSYS.LongTypeToDateTime(currentClose.closeStartDt).ToString("yyyy-MM-dd HH:mm:ss");
            Nowdate = NPSYS.LongTypeToDateTime(currentClose.closeEndDt).ToString("yyyy-MM-dd HH:mm:ss");

            string MNO = TextCore.ToRightAlignString(2, NPSYS.Config.GetValue(ConfigID.ParkingLotBoothID), '0');

            string dY_UNIT_PARKNAME = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_PARKNAME.ToString());
            string dY_UNIT_BUSINESSNUMBER = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_BUSINESSNUMBER.ToString());
            string dY_UNIT_ADDRESS = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_ADDRESS.ToString());
            string dY_UNIT_TELNO = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_TELNO.ToString());
            string dY_UNIT_UNITNAME = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_UNITNAME.ToString());
            string dY_CLOSE_CLOSING_PREIOD = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_CLOSING_PREIOD.ToString());
            string dY_CLOSE_CLOSING = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_CLOSING.ToString());
            string dY_CLOSE_CLOSING_DETAIL = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_CLOSING_DETAIL.ToString());
            string dY_CLOSE_CLOSING_STATUS = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_CLOSING_STATUS.ToString());
            string dY_CLOSE_AUTOSTATION_DETAIL = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_AUTOSTATION_DETAIL.ToString());
            string dY_CLOSE_DETAUL_VEHICLEL = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_DETAUL_VEHICLEL.ToString());
            string dY_CLOSE_KIND = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_KIND.ToString());
            string dY_CLOSE_QUANTITY = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_QUANTITY.ToString());
            string dY_CLOSE_AMOUNT = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_AMOUNT.ToString());
            string dY_CLOSE_TOTAL = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_TOTAL.ToString());
            string dY_CLOSE_NORMALCAR = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_NORMALCAR.ToString());
            string dY_CLOSE_FREECAR = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_FREECAR.ToString());
            string dY_CLOSE_COMMUTERCAR = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_COMMUTERCAR.ToString());
            string dY_CLOSE_CASH = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_CASH.ToString());
            string dY_CLOSE_TMOEY = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_TMOEY.ToString());
            string dY_CLOSE_CREDITCARD = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_CREDITCARD.ToString());
            string dY_CLOSE_DISCOUNT = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_DISCOUNT.ToString());
            string dY_CLOSE_DESPOSIT = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_DESPOSIT.ToString());
            string dY_CLOSE_WITHROW = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_WITHROW.ToString());
            string dY_CLOSE_INCOME_DETAIL = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_INCOME_DETAIL.ToString());
            string dY_CLOSE_CURRENT_CASH_RESERVE = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_CURRENT_CASH_RESERVE.ToString());
            string dY_CLOSE_PRINTDATE = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_CLOSE_PRINTDATE.ToString());
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
                {
                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print("   " + dY_UNIT_PARKNAME + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotName) + "\n");
                }
                else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
                {
                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print(dY_UNIT_PARKNAME + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotName) + "\n");
                }
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", dY_UNIT_PARKNAME + NPSYS.Config.GetValue(ConfigID.ParkingLotName));
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
                {
                    NPSYS.Device.HMC60.Print("   " + dY_UNIT_BUSINESSNUMBER + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo) + "\n");
                }
                else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
                {
                    NPSYS.Device.HMC60.Print(dY_UNIT_BUSINESSNUMBER + ": " + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo) + "\n");
                }
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", dY_UNIT_BUSINESSNUMBER + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo));
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
                {
                    NPSYS.Device.HMC60.Print("   " + dY_UNIT_ADDRESS + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress) + "\n");
                }
                else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
                {
                    NPSYS.Device.HMC60.Print(dY_UNIT_ADDRESS + ": " + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress) + "\n");
                }
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   주  소:" + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress));
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
                {
                    NPSYS.Device.HMC60.Print("   " + dY_UNIT_UNITNAME + ":" + NPSYS.BoothName + "\n");
                }
                else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
                {
                    NPSYS.Device.HMC60.Print(dY_UNIT_UNITNAME + ": " + NPSYS.BoothName + "\n");
                }
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", dY_UNIT_UNITNAME + ": " + NPSYS.BoothName);
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
                {
                    NPSYS.Device.HMC60.Print("   " + dY_CLOSE_CLOSING_PREIOD + " :" + olddate + "\n");
                }
                else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
                {
                    NPSYS.Device.HMC60.Print(dY_CLOSE_CLOSING_PREIOD + ":" + olddate + "\n");
                }
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", dY_CLOSE_CLOSING_PREIOD + ":" + olddate);
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
                {
                    NPSYS.Device.HMC60.Print("             " + Nowdate + "\n");
                }
                else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
                {
                    NPSYS.Device.HMC60.Print("          " + Nowdate + "\n");
                }
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "             " + Nowdate);
            string l_IsMagam = (P_Update_Yn == true ? dY_CLOSE_CLOSING : dY_CLOSE_CLOSING_DETAIL);
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
                {
                    NPSYS.Device.HMC60.Print("   " + dY_CLOSE_CLOSING_STATUS + ": " + l_IsMagam + "\n");
                }
                else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
                {
                    NPSYS.Device.HMC60.Print(dY_CLOSE_CLOSING_STATUS + ": " + l_IsMagam + "\n");
                }
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", dY_CLOSE_CLOSING_STATUS + ": " + l_IsMagam);
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
                {
                    NPSYS.Device.HMC60.Print("   " + "===========================================\n\n");
                    NPSYS.Device.HMC60.FontSize(2, 1);
                    NPSYS.Device.HMC60.Print("  *" + MNO + " " + dY_CLOSE_AUTOSTATION_DETAIL + "*\n\n");
                    NPSYS.Device.HMC60.FontSize(1, 1);
                }
                else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
                {
                    NPSYS.Device.HMC60.Print("==============================\n\n");
                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print("  *" + MNO + " " + dY_CLOSE_AUTOSTATION_DETAIL + "*\n\n");
                    NPSYS.Device.HMC60.FontSize(1, 1);
                }
            }

            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", " ==============================");
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "  *" + MNO + " " + dY_CLOSE_AUTOSTATION_DETAIL + "*");

            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                NPSYS.Device.HMC60.Print("   ■ " + dY_CLOSE_DETAUL_VEHICLEL + " ■ \n");
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   ■ " + dY_CLOSE_DETAUL_VEHICLEL + " ■ ");
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrint(dY_CLOSE_KIND, dY_CLOSE_QUANTITY, dY_CLOSE_AMOUNT);
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, dY_CLOSE_KIND) + "     " + TextCore.ToRightAlignString(10, dY_CLOSE_QUANTITY) + TextCore.Space(7) + TextCore.ToRightAlignString(10, dY_CLOSE_AMOUNT));
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrintOnline();
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + "-------------------------------------------");

            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrint("<" + dY_CLOSE_TOTAL + ">", TextCore.ToCommaString(currentClose.closeCar.totalCnt), TextCore.ToCommaString(currentClose.closeCar.totalAmt));
                NPSYS.Device.HMC60.Print("\n");
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, "<" + dY_CLOSE_TOTAL + ">") + "     " + TextCore.ToRightAlignString(10, currentClose.closeCar.totalCnt.ToString()) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCar.totalAmt)));

            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrint(dY_CLOSE_NORMALCAR, TextCore.ToCommaString(currentClose.closeCar.generalCarCnt), TextCore.ToCommaString(currentClose.closeCar.generalCarAmt));
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, dY_CLOSE_NORMALCAR) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCar.generalCarCnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCar.generalCarAmt)));

            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrint(dY_CLOSE_FREECAR, TextCore.ToCommaString(currentClose.closeCar.freeCarCnt), TextCore.ToCommaString(currentClose.closeCar.freeCarAmt));
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, dY_CLOSE_FREECAR) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCar.freeCarCnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCar.freeCarAmt)));

            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrint(dY_CLOSE_COMMUTERCAR, TextCore.ToCommaString(currentClose.closeCar.seasonCarCnt), TextCore.ToCommaString(currentClose.closeCar.seasonCarAmt));
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, dY_CLOSE_COMMUTERCAR) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCar.seasonCarCnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCar.seasonCarAmt)));

            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrintTwoLine();
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + "===========================================");

            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                NPSYS.Device.HMC60.Print("   ■" + dY_CLOSE_INCOME_DETAIL + "■\n");
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   ■" + dY_CLOSE_INCOME_DETAIL + "■");

            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrint(dY_CLOSE_KIND, dY_CLOSE_QUANTITY, dY_CLOSE_AMOUNT);
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, "종       류") + "     " + TextCore.ToRightAlignString(10, "수    량") + TextCore.Space(7) + TextCore.ToRightAlignString(10, "금    액"));
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrintOnline();
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + "-------------------------------------------");

            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrint("<" + dY_CLOSE_TOTAL + ">", TextCore.ToCommaString(currentClose.closePayment.totalCnt), TextCore.ToCommaString(currentClose.closePayment.totalAmt));
                NPSYS.Device.HMC60.Print("\n");
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, "<" + dY_CLOSE_TOTAL + ">") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closePayment.totalCnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closePayment.totalAmt)));
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrint(dY_CLOSE_CASH, TextCore.ToCommaString(currentClose.closePayment.cashCnt), TextCore.ToCommaString(currentClose.closePayment.cashAmt));
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, dY_CLOSE_CASH) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closePayment.cashCnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closePayment.cashAmt)));
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrint(dY_CLOSE_TMOEY, TextCore.ToCommaString(currentClose.closePayment.transCnt), TextCore.ToCommaString(currentClose.closePayment.transAmt));
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, dY_CLOSE_TMOEY) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closePayment.transCnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closePayment.transAmt)));
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrint(dY_CLOSE_CREDITCARD, TextCore.ToCommaString(currentClose.closePayment.cardCnt), TextCore.ToCommaString(currentClose.closePayment.cardAmt));
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, dY_CLOSE_CREDITCARD) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closePayment.cardCnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closePayment.cardAmt)));
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrint(dY_CLOSE_DISCOUNT, TextCore.ToCommaString(currentClose.closePayment.discountCnt), TextCore.ToCommaString(currentClose.closePayment.discountAmt));
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, dY_CLOSE_DISCOUNT) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closePayment.discountCnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closePayment.discountAmt)));
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcPrintOnline();
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + "-------------------------------------------");

            if (NPSYS.Device.UsingSettingBill)
            {
                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    HmcPrint("[" + dY_CLOSE_DESPOSIT + "]", TextCore.ToCommaString(currentClose.closeCash.inCashTotalCnt), TextCore.ToCommaString(currentClose.closeCash.inCashTotalAmt));
                    NPSYS.Device.HMC60.Print("\n");
                }
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, "[" + dY_CLOSE_DESPOSIT + "]") + "      " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCashTotalCnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCashTotalAmt)));

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin1).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin1), TextCore.ToCommaString(currentClose.closeCash.inCoin1Cnt), TextCore.ToCommaString(currentClose.closeCash.inCoin1Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin1).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin1)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCoin1Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCoin1Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin2).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin2), TextCore.ToCommaString(currentClose.closeCash.inCoin2Cnt), TextCore.ToCommaString(currentClose.closeCash.inCoin2Amt));
                    }
                }
                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin2).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin2)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCoin2Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCoin2Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin3).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin3), TextCore.ToCommaString(currentClose.closeCash.inCoin3Cnt), TextCore.ToCommaString(currentClose.closeCash.inCoin3Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin3).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin3)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCoin3Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCoin3Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin4).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin4), TextCore.ToCommaString(currentClose.closeCash.inCoin4Cnt), TextCore.ToCommaString(currentClose.closeCash.inCoin4Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin4).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin4)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCoin4Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCoin4Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin5).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin5), TextCore.ToCommaString(currentClose.closeCash.inCoin5Cnt), TextCore.ToCommaString(currentClose.closeCash.inCoin5Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin5).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin5)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCoin5Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCoin5Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin6).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin6), TextCore.ToCommaString(currentClose.closeCash.inCoin6Cnt), TextCore.ToCommaString(currentClose.closeCash.inCoin6Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin6).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin6)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCoin6Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inCoin6Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill1).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill1), TextCore.ToCommaString(currentClose.closeCash.inBill1Cnt), TextCore.ToCommaString(currentClose.closeCash.inBill1Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill1).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill1)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inBill1Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inBill1Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill2).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill2), TextCore.ToCommaString(currentClose.closeCash.inBill2Cnt), TextCore.ToCommaString(currentClose.closeCash.inBill2Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill2).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill2)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inBill2Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inBill2Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill3).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill3), TextCore.ToCommaString(currentClose.closeCash.inBill3Cnt), TextCore.ToCommaString(currentClose.closeCash.inBill3Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill3).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill3)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inBill3Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inBill3Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill4).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill4), TextCore.ToCommaString(currentClose.closeCash.inBill4Cnt), TextCore.ToCommaString(currentClose.closeCash.inBill4Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill4).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill4)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inBill4Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inBill4Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill5).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill5), TextCore.ToCommaString(currentClose.closeCash.inBill5Cnt), TextCore.ToCommaString(currentClose.closeCash.inBill5Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill5).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill5)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inBill5Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inBill5Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill6).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill6), TextCore.ToCommaString(currentClose.closeCash.inBill6Cnt), TextCore.ToCommaString(currentClose.closeCash.inBill6Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill6).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill6)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inBill6Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.inBill6Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    HmcPrintOnline();
                }
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + "-------------------------------------------");
            }

            if (NPSYS.Device.UsingSettingBill)
            {
                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    HmcPrint("[" + dY_CLOSE_WITHROW + "]", TextCore.ToCommaString(currentClose.closeCash.outCashTotalCnt), TextCore.ToCommaString(currentClose.closeCash.outCashTotalAmt), 1, -1);
                    NPSYS.Device.HMC60.Print("\n");
                }
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, "[" + dY_CLOSE_WITHROW + "]") + "      " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCashTotalCnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCashTotalAmt)));

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin1).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin1), TextCore.ToCommaString(currentClose.closeCash.outCoin1Cnt), TextCore.ToCommaString(currentClose.closeCash.outCoin1Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin1).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin1)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCoin1Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCoin1Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin2).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin2), TextCore.ToCommaString(currentClose.closeCash.outCoin2Cnt), TextCore.ToCommaString(currentClose.closeCash.outCoin2Amt));
                    }
                }
                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin2).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin2)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCoin2Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCoin2Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin3).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin3), TextCore.ToCommaString(currentClose.closeCash.outCoin3Cnt), TextCore.ToCommaString(currentClose.closeCash.outCoin3Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin3).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin3)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCoin3Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCoin3Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin4).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin4), TextCore.ToCommaString(currentClose.closeCash.outCoin4Cnt), TextCore.ToCommaString(currentClose.closeCash.outCoin4Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin4).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin4)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCoin4Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCoin4Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin5).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin5), TextCore.ToCommaString(currentClose.closeCash.outCoin5Cnt), TextCore.ToCommaString(currentClose.closeCash.outCoin5Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin5).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin5)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCoin5Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCoin5Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin6).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin6), TextCore.ToCommaString(currentClose.closeCash.outCoin6Cnt), TextCore.ToCommaString(currentClose.closeCash.outCoin6Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin6).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin6)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCoin6Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outCoin6Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill1).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill1), TextCore.ToCommaString(currentClose.closeCash.outBill1Cnt), TextCore.ToCommaString(currentClose.closeCash.outBill1Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill1).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill1)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outBill1Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outBill1Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill2).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill2), TextCore.ToCommaString(currentClose.closeCash.outBill2Cnt), TextCore.ToCommaString(currentClose.closeCash.outBill2Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill2).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill2)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outBill2Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outBill2Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill3).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill3), TextCore.ToCommaString(currentClose.closeCash.outBill3Cnt), TextCore.ToCommaString(currentClose.closeCash.outBill3Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill3).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill3)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outBill3Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outBill3Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill4).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill4), TextCore.ToCommaString(currentClose.closeCash.outBill4Cnt), TextCore.ToCommaString(currentClose.closeCash.outBill4Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill4).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill4)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outBill4Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outBill4Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill5).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill5), TextCore.ToCommaString(currentClose.closeCash.outBill5Cnt), TextCore.ToCommaString(currentClose.closeCash.outBill5Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill5).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill5)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outBill5Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outBill5Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill6).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill6), TextCore.ToCommaString(currentClose.closeCash.outBill6Cnt), TextCore.ToCommaString(currentClose.closeCash.outBill6Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill6).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill6)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outBill6Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.outBill6Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    HmcPrintOnline();
                }
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + "-------------------------------------------");
            }

            if (currentClose.closeDiscount != null && currentClose.closeDiscount.Count > 0)
            {
                foreach (CloseDiscount itemCloseDiscount in currentClose.closeDiscount)
                {
                    GetDiscountType(itemCloseDiscount, pIsManualMagam);
                }
            }

            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                NPSYS.Device.HMC60.FontSize(1, 1);
            }

            if (NPSYS.Device.UsingSettingBill)
            {
                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    NPSYS.Device.HMC60.Print("   ■" + dY_CLOSE_CURRENT_CASH_RESERVE + "■ \n");
                }
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   ■" + dY_CLOSE_CURRENT_CASH_RESERVE + "■");

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    HmcPrint(dY_CLOSE_KIND, dY_CLOSE_QUANTITY, dY_CLOSE_AMOUNT);
                }
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, dY_CLOSE_KIND) + "     " + TextCore.ToRightAlignString(10, dY_CLOSE_QUANTITY) + TextCore.Space(7) + TextCore.ToRightAlignString(10, dY_CLOSE_AMOUNT));

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    HmcPrintOnline();
                }
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + "----------------------------------------------");

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    HmcPrint("<" + dY_CLOSE_TOTAL + ">", TextCore.ToCommaString(currentClose.closeCash.currCashTotalCnt), TextCore.ToCommaString(currentClose.closeCash.currCashTotalAmt));
                }
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, "<" + dY_CLOSE_TOTAL + ">") + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCashTotalCnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCashTotalAmt)));

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin1).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin1), TextCore.ToCommaString(currentClose.closeCash.currCoin1Cnt), TextCore.ToCommaString(currentClose.closeCash.currCoin1Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin1).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin1)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCoin1Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCoin1Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin2).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin2), TextCore.ToCommaString(currentClose.closeCash.currCoin2Cnt), TextCore.ToCommaString(currentClose.closeCash.currCoin2Amt));
                    }
                }
                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin2).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin2)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCoin2Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCoin2Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin3).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin3), TextCore.ToCommaString(currentClose.closeCash.currCoin3Cnt), TextCore.ToCommaString(currentClose.closeCash.currCoin3Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin3).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin3)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCoin3Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCoin3Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin4).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin4), TextCore.ToCommaString(currentClose.closeCash.currCoin4Cnt), TextCore.ToCommaString(currentClose.closeCash.currCoin4Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin4).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin4)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCoin4Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCoin4Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin5).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin5), TextCore.ToCommaString(currentClose.closeCash.currCoin5Cnt), TextCore.ToCommaString(currentClose.closeCash.currCoin5Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin5).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin5)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCoin5Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCoin5Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin6).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin6), TextCore.ToCommaString(currentClose.closeCash.currCoin6Cnt), TextCore.ToCommaString(currentClose.closeCash.currCoin6Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin6).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.coin6)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCoin6Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currCoin6Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill1).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill1), TextCore.ToCommaString(currentClose.closeCash.currBill1Cnt), TextCore.ToCommaString(currentClose.closeCash.currBill1Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill1).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill1)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currBill1Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currBill1Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill2).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill2), TextCore.ToCommaString(currentClose.closeCash.currBill2Cnt), TextCore.ToCommaString(currentClose.closeCash.currBill2Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill2).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill2)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currBill2Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currBill2Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill3).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill3), TextCore.ToCommaString(currentClose.closeCash.currBill3Cnt), TextCore.ToCommaString(currentClose.closeCash.currBill3Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill3).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill3)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currBill3Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currBill3Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill4).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill4), TextCore.ToCommaString(currentClose.closeCash.currBill4Cnt), TextCore.ToCommaString(currentClose.closeCash.currBill4Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill4).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill4)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currBill4Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currBill4Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill5).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill5), TextCore.ToCommaString(currentClose.closeCash.currBill5Cnt), TextCore.ToCommaString(currentClose.closeCash.currBill5Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill5).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill5)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currBill5Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currBill5Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill6).Trim() != string.Empty)
                    {
                        HmcPrint(NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill6), TextCore.ToCommaString(currentClose.closeCash.currBill6Cnt), TextCore.ToCommaString(currentClose.closeCash.currBill6Amt));
                    }
                }

                if (NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill6).Trim() != string.Empty)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(10, NPSYS.gGurrentMoneTary.GetBillName(NPSYS.gGurrentMoneTary.bill6)) + "     " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currBill6Cnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(currentClose.closeCash.currBill6Amt)));
                }

                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    HmcPrintTwoLine();
                }
                TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + "===========================================");
            }
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
                {
                    NPSYS.Device.HMC60.Print("   " + dY_CLOSE_PRINTDATE + ": " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n");
                }
                else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
                {
                    NPSYS.Device.HMC60.Print(dY_CLOSE_PRINTDATE + ": " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n");
                }
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + dY_CLOSE_PRINTDATE + ": " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                NPSYS.Device.HMC60.Feeding(25);
            }
            System.Threading.Thread.Sleep(200);
            if (NPSYS.g_UsePrintFullCuting)
            {
                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    NPSYS.Device.HMC60.FullCutting();
                }
            }
            else
            {
                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    NPSYS.Device.HMC60.ParticalCutting();
                }
            }
        }

        public static void HmcDiscountPrint(string pFirstData, string pSecondData, string pThreeData)
        {
            int hmc60FirstSpace = 3;
            int hmc60MiddleSpace = 2;
            int hmc60FInalSapce = 7;
            int hmc54FirstSpace = 0;
            int hmc54MiddleSapce = 0;
            int hmc54FInalSapce = 2;
            if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
            {
                NPSYS.Device.HMC60.Print(TextCore.Space(hmc60FirstSpace) + TextCore.ToLeftAlignString(14, pFirstData) + TextCore.Space(hmc60MiddleSpace) + TextCore.ToRightAlignString(10, pSecondData) + TextCore.Space(hmc60FInalSapce) + TextCore.ToRightAlignString(10, pThreeData) + "\n");
            }
            else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
            {
                NPSYS.Device.HMC60.Print(TextCore.Space(hmc54FirstSpace) + TextCore.ToLeftAlignString(14, pFirstData) + TextCore.Space(hmc54MiddleSapce) + TextCore.ToRightAlignString(10, pSecondData) + TextCore.Space(hmc54FInalSapce) + TextCore.ToRightAlignString(10, pThreeData) + "\n");
            }
        }

        public static void HmcPrint(string pFirstData, string pSecondData, string pThreeData, int pFrontAdd = 0, int pSecondAdd = 0)
        {
            int hmc60FirstSpace = 3;
            int hmc60MiddleSpace = 5;
            int hmc60FInalSapce = 7;
            int hmc54FirstSpace = 0;
            int hmc54MiddleSapce = 2;
            int hmc54FInalSapce = 3;
            if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
            {
                NPSYS.Device.HMC60.Print(TextCore.Space(hmc60FirstSpace) + TextCore.ToLeftAlignString(11 + pFrontAdd, pFirstData) + TextCore.Space(hmc60MiddleSpace + pSecondAdd) + TextCore.ToRightAlignString(10, pSecondData) + TextCore.Space(hmc60FInalSapce) + TextCore.ToRightAlignString(10, pThreeData) + "\n");
            }
            else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
            {
                NPSYS.Device.HMC60.Print(TextCore.Space(hmc54FirstSpace) + TextCore.ToLeftAlignString(11 + pFrontAdd, pFirstData) + TextCore.Space(hmc54MiddleSapce + pSecondAdd) + TextCore.ToRightAlignString(10, pSecondData) + TextCore.Space(hmc54FInalSapce) + TextCore.ToRightAlignString(10, pThreeData) + "\n");
            }
        }

        public static void HmcPrint(string pFirstData, string pSecondData)
        {
            int hmc60FirstSpace = 3;
            int hmc60MiddleSpace = 23;
            int hmc54FirstSpace = 0;
            int hmc54MiddleSapce = 16;
            if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
            {
                NPSYS.Device.HMC60.Print(TextCore.Space(hmc60FirstSpace) + TextCore.ToLeftAlignString(10, pFirstData) + TextCore.Space(hmc60MiddleSpace) + TextCore.ToRightAlignString(10, pSecondData) + "\n");
            }
            else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
            {
                NPSYS.Device.HMC60.Print(TextCore.Space(hmc54FirstSpace) + TextCore.ToLeftAlignString(10, pFirstData) + TextCore.Space(hmc54MiddleSapce) + TextCore.ToRightAlignString(10, pSecondData) + "\n");
            }
        }

        public static void HmcPrintOnline()
        {
            if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
            {
                NPSYS.Device.HMC60.Print("   -------------------------------------------\n");
            }
            else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
            {
                NPSYS.Device.HMC60.Print("------------------------------------\n");
            }
        }

        public static void HmcPrintTwoLine()
        {
            if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
            {
                NPSYS.Device.HMC60.Print("   ===========================================\n");
            }
            else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
            {
                NPSYS.Device.HMC60.Print("====================================\n");
            }
        }

        private static void GetDiscountType(CloseDiscount pCloseDiscount, bool pIsManualMagam)
        {
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                HmcDiscountPrint(pCloseDiscount.discountTypeName, TextCore.ToCommaString(pCloseDiscount.discountTypeCnt), TextCore.ToCommaString(pCloseDiscount.discountTypeAmt));
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(14, pCloseDiscount.discountTypeName.ToString()) + "  " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(pCloseDiscount.discountTypeCnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(pCloseDiscount.discountTypeAmt)));

            if (pCloseDiscount.closeDiscountItem != null && pCloseDiscount.closeDiscountItem.Count > 0)
            {
                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    foreach (CloseDiscountItem closediscountDetail in pCloseDiscount.closeDiscountItem)
                    {
                        HmcDiscountPrint(closediscountDetail.discountName, TextCore.ToCommaString(closediscountDetail.discountCnt.ToString()), TextCore.ToCommaString(closediscountDetail.discountAmt));
                    }
                }
                foreach (CloseDiscountItem closediscountDetail in pCloseDiscount.closeDiscountItem)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(14, closediscountDetail.discountName) + "  " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(closediscountDetail.discountCnt.ToString())) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(closediscountDetail.discountAmt)));
                }
                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    HmcPrintTwoLine();
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + "===========================================");
                }
            }
        }
    }
}
