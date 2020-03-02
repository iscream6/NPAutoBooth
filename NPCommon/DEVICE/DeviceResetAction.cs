namespace NPCommon.DEVICE
{
    // 나이스리셋기능
    public class DeviceResetAction
    {

        //public static BillReaderResult BillReaderReset()
        //{
        //    BillReaderResult billReaderResult = null;
        //    try
        //    {
        //        if (!NPSYS.Device.UsingSettingBillReader)
        //        {
        //            billReaderResult = new BillReaderResult();
        //            billReaderResult.ResultType = BillStatusResultType.OkStatus;
        //            billReaderResult.Message = "사용안함";
        //            return billReaderResult;
        //        }
        //        NPSYS.Device.BillReader.Disconnect();
        //        bool resultBool = NPSYS.Device.BillReader.Connect();
        //        if (resultBool == true)
        //        {

        //            NPSYS.Device.BillReader.Reset();
        //            NPSYS.Device.isUseDeviceBillReaderDevice = true;
        //            NPSYS.Device.BillReader.BillDIsableAccept();
        //            return BillReaderGetStatus();

        //        }
        //        else
        //        {
        //            billReaderResult = new BillReaderResult();
        //            billReaderResult.Message = "연결실패";
        //            billReaderResult.ResultCode = (byte)154;
        //            billReaderResult.ResultType = BillStatusResultType.Error;
        //            NPSYS.Device.isUseDeviceBillReaderDevice = false;
        //            TextCore.DeviceError(TextCore.DEVICE.BILLREADER, "DeviceResetAction | BillReaderReset", "지폐리더기 상태:" + "연결실패");
        //            NPSYS.Device.BillReaderDeviceErrorMessage = billReaderResult.Message;
        //            return billReaderResult;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TextCore.DeviceError(TextCore.DEVICE.BILLREADER, "DeviceResetAction | BillReaderReset", "지폐리더기 상태:" + "연결실패"+ ex.ToString());
        //        return null;
        //    }
        //}



        // private static  BillReaderResult BillReaderGetStatus()
        //{
        //    BillReaderResult p_BillReaderResult = null;
        //    try
        //    {
        //        if (!NPSYS.Device.UsingSettingBillReader)
        //        {
        //            p_BillReaderResult = new BillReaderResult();
        //            p_BillReaderResult.ResultType = BillStatusResultType.OkStatus;
        //            p_BillReaderResult.Message = "사용안함";
        //            return p_BillReaderResult;
        //        }
        //        p_BillReaderResult = new BillReaderResult(NPSYS.Device.BillReader.CurrentStatus());
        //        if (p_BillReaderResult.ResultType == BillStatusResultType.OkStatus)
        //        {
        //            NPSYS.Device.isUseDeviceBillReaderDevice = true;
        //            TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "DeviceResetAction | BillReaderGetStatus", "지폐리더기 리셋상태:" + p_BillReaderResult.Message);
        //            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, (byte)0x88);
        //            return p_BillReaderResult;
        //        }
        //        else
        //        {
        //            NPSYS.Device.isUseDeviceBillReaderDevice = false;
        //            TextCore.DeviceError(TextCore.DEVICE.BILLREADER, "DeviceResetAction | BillReaderGetStatus", "지폐리더기 리셋상태:" + p_BillReaderResult.Message);
        //            NPSYS.Device.BillReaderDeviceErrorMessage = p_BillReaderResult.Message;
        //            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, p_BillReaderResult.ResultCode);
        //            return p_BillReaderResult;

        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}



        ///// <summary>
        ///// 카드리더기2 리셋 
        ///// </summary>
        //public static Result CardReader2Reset()
        //{
        //    Result result = null;
        //    if (NPSYS.Device.UsingSettingCardReadType2 == NPCommon.ConfigID.CardReaderType.TItMagnetincDiscount )
        //    {
        //        try
        //        {

        //            result = NPSYS.Device.CardDevice2.SoftResetCreditDevice();
        //            result = NPSYS.Device.CardDevice2.GetStatus();
        //            if (result.Success == true)
        //            {
        //                NPSYS.Device.gIsUseCreditCardDevice2 = true;
        //                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CA2, (int)88);
        //                TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "DeviceResetAction | CardReader2Reset", "카드리더기2 리셋결과:" + result.Message);
        //                return result;
        //            }
        //            else
        //            {
        //                NPSYS.Device.gIsUseCreditCardDevice2 = false;
        //                TextCore.DeviceError(TextCore.DEVICE.CARDREADER2, "DeviceResetAction | CardReader2Reset", "카드리더기2 리셋결과:" + result.Message);
        //                NPSYS.Device.CreditCardDeviceErrorMessage2 = result.Message;
        //                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CA2, result.ReultIntMessage);
        //                return result;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            NPSYS.Device.gIsUseCreditCardDevice2 = false;
        //            TextCore.DeviceError(TextCore.DEVICE.CARDREADER2, "DeviceResetAction | CardReader2Reset", "카드리더기2 리셋결과: " + ex.ToString());
        //            NPSYS.Device.CreditCardDeviceErrorMessage2 = ex.ToString();
        //            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CA2, result.ReultIntMessage);
        //            return new Result(false, ex.ToString(), (int)106);
        //        }
        //    }
        //    else
        //    {
        //        result = new Result(true, "사용안함", 11);
        //        return result;

        //    }

        //}


        //public static Result CardReader1Reset()
        //{
        //    Result result = null;
        //    if (NPSYS.Device.UsingSettingCardReadType1 == NPCommon.ConfigID.CardReaderType.TItMagnetincDiscount )
        //    {
        //        try
        //        {

        //            result = NPSYS.Device.CardDevice1.SoftResetCreditDevice();
        //            result = NPSYS.Device.CardDevice1.GetStatus();
        //            if (result.Success == true)
        //            {
        //                NPSYS.Device.gIsUseCreditCardDevice1 = true;
        //                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CA1, (int)88);
        //                TextCore.ACTION(TextCore.ACTIONS.CARDREADER1, "DeviceResetAction | CardReader1Reset", "카드리더기1 리셋결과:" + result.Message);
        //                return result;
        //            }
        //            else
        //            {
        //                NPSYS.Device.gIsUseCreditCardDevice1 = false;
        //                TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "DeviceResetAction | CardReader1Reset", "카드리더기1 리셋결과:" + result.Message);
        //                NPSYS.Device.CreditCardDeviceErrorMessage1 = result.Message;
        //                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CA1, result.ReultIntMessage);
        //                return result;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            NPSYS.Device.gIsUseCreditCardDevice1 = false;
        //            TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "DeviceResetAction | CardReader1Reset", "카드리더기1 리셋결과: " + ex.ToString());
        //            NPSYS.Device.CreditCardDeviceErrorMessage1 = ex.ToString();
        //            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CA1, result.ReultIntMessage);
        //            return new Result(false, ex.ToString(), (int)106);
        //        }
        //    }
        //    else
        //    {
        //        result = new Result(true, "사용안함", 11);
        //        return result;

        //    }

        //}
        //public static Result BillDischargerGetStatus()
        //{
        //    Result p_result = null;
        //    if (!NPSYS.Device.UsingSettingBill)
        //    {
        //        p_result = new Result(true, "사용안함", 11);
        //        return p_result;
        //    }
        //    p_result = MoneyBillOutDeviice.MoneyBillStatus2();
        //    if (p_result.Success)
        //    {
        //        NPSYS.Device.gIsUseDeviceBillDischargeDevice = true;
        //        TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "DeviceResetAction|Bil l BillDischargerGetStatus", "지폐방출기 상태" + p_result.Message);
        //        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, (int)88);
        //        return p_result;


        //    }
        //    else
        //    {
        //        TextCore.DeviceError(TextCore.DEVICE.BILLCHARGER, "DeviceResetAction | BillDischargerGetStatus", "지폐방출기 상태:" + p_result.Message);
        //        NPSYS.Device.gIsUseDeviceBillDischargeDevice = false;
        //        NPSYS.Device.BillDischargeDeviceErrorMessage = p_result.Message;
        //        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, (int)106);
        //        return p_result;
        //    }
        //}

        ///// <summary>
        ///// 지폐방출기 상태체크 및 리셋
        ///// </summary>
        //public static  Result BillDischargerReset()
        //{
        //    Result p_result = null;
        //    if (!NPSYS.Device.UsingSettingBill)
        //    {
        //        p_result = new Result(true, "사용안함", 11);
        //        return p_result;
        //    }
        //    p_result = MoneyBillOutDeviice.BillPurgeJob();
        //    if (p_result.Success == false)
        //    {
        //        TextCore.DeviceError(TextCore.DEVICE.BILLCHARGER, "DeviceReset|BillDischargerGetStatus", "지폐방출기 리셋실패 상태:" + p_result.Message);
        //        NPSYS.Device.gIsUseDeviceBillDischargeDevice = false;
        //        NPSYS.Device.BillDischargeDeviceErrorMessage = p_result.Message;
        //        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, (int)106);

        //        return p_result;
        //    }
        //    else
        //    {
        //        TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "DeviceReset|BillDischargerGetStatus", "지폐방출기 리셋상태:" + p_result.Message);
        //        return BillDischargerGetStatus();
        //    }


        //}
        //// 나이스리셋기능 주석완료

    }
}
