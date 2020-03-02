using FadeFox;
using FadeFox.Text;
using NPCommon;
using System;
using System.Data;
using System.Runtime.InteropServices;

namespace NPCommon.DEVICE
{
    /// <summary>
    /// 지폐방출기 아래부분 배출에 대한 정보
    /// </summary>
    public class LowerOut
    {

        /// <summary>
        /// 배출할 매수
        /// </summary>
        public int nCount { get; set; }

        /// <summary>
        /// CHK3,4센서에 감지된 매수
        /// </summary>
        public int pChkSensor { get; set; }

        /// <summary>
        /// EXIT센서에 감지된 매수
        /// </summary>
        public int pExitSensor { get; set; }

        /// <summary>
        /// Reject 트레이로 떨어진 건수 
        /// </summary>
        public int pRejected { get; set; }

        /// <summary>
        /// 0이면 지폐충분함, 1이면 지폐부족예보
        /// </summary>
        public int pNearEnd { get; set; }

        /// <summary>
        /// 상태값
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 성공 / 또는 실패
        /// </summary>
        public bool isSuccess { get; set; }

    }
    /// <summary>
    /// 지폐방출기 위부분에 대한정보
    /// </summary>
    public class UpperOut
    {

        /// <summary>
        /// 배출할 매수
        /// </summary>
        public int nCount { get; set; }

        /// <summary>
        /// CHK3,4센서에 감지된 매수
        /// </summary>
        public int pChkSensor { get; set; }

        /// <summary>
        /// EXIT센서에 감지된 매수
        /// </summary>
        public int pExitSensor { get; set; }

        /// <summary>
        /// Reject 트레이로 떨어진 건수 
        /// </summary>
        public int pRejected { get; set; }

        /// <summary>
        /// 0이면 지폐충분함, 1이면 지폐부족예보
        /// </summary>
        public int pNearEnd { get; set; }

        /// <summary>
        /// 상태값
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 성공 / 또는 실패
        /// </summary>
        public bool isSuccess { get; set; }

    }
    public class MoneyBillOutDeviice
    {
        // extern "C" __stdcall int LCDM2000_Init(int Port, char *path);
        [DllImport("LCDM2000.DLL", EntryPoint = "LCDM2000_Init")]  // 장비 초기화
        private static extern int LCDM2000_Init(int Port, [MarshalAs(UnmanagedType.LPStr)]string path);

        private static BillDischargerStatusType Init(int pPort, string pLogPath)
        {
            try
            {
                return ConvertResultType(LCDM2000_Init(pPort, pLogPath));
            }
            catch
            {
                return BillDischargerStatusType.PortOpenStatus;
            }
        }

        /// <summary>
        /// 지폐방출기 초기화
        /// </summary>
        /// <param name="pPort"></param>
        /// <param name="pLogPath"></param>
        /// <returns></returns>
        public static Result Init(string pPort, string pLogPath)
        {
            try
            {
                BillDischargerStatusType result = Init(Convert.ToInt32(pPort.ToUpper().Replace("COM", "")), pLogPath);

                if (result == BillDischargerStatusType.Success)
                {
                    MoneyBillOutDeviice.BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.PortOpenStatus, true);
                    return new Result(true, GetMessage(result));
                }
                else
                {
                    MoneyBillOutDeviice.BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.PortOpenStatus, false);
                    return new Result(false, GetMessage(result));
                }
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.BILLCHARGER, "MoneyBillOutDeviice|Init", "지폐방출기 초기화에러:" + ex.ToString());
                MoneyBillOutDeviice.BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.PortOpenStatus, false);
                return new Result(ex);
            }
        }

        [DllImport("LCDM2000.DLL", EntryPoint = "LCDM2000_End")]  // 장비종료
        private static extern int LCDM2000_End();
        /// <summary>
        /// 지폐 방출기 장비를 종료시킴
        /// </summary>
        /// <returns></returns>
        public static BillDischargerStatusType End()
        {
            return ConvertResultType(LCDM2000_End());
        }



        [DllImport("LCDM2000.DLL", EntryPoint = "LCDM2000_Status")]
        private static extern int LCDM2000_Status();

        private static BillDischargerStatusType Status()
        {
            return ConvertResultType(LCDM2000_Status());
        }

        private static BillDischargerStatusType Status2()
        {
            return ConvertResultType(LCDM2000_Status(), false);
        }



        /// <summary>
        /// 장비의 현재상태를 가져온다
        /// </summary>
        /// <returns></returns>
        public static Result MoneyBillStatus()
        {
            try
            {

                BillDischargerStatusType result = Status();

                //if (result == IMoneyBillResult.Success || result == IMoneyBillResult.Success_Before_PowerOn || result == IMoneyBillResult.StatusSuccess_Before_BillRejcet || result == IMoneyBillResult.LowerTraySamll || result == IMoneyBillResult.UpperTraySamll )
                if (result == BillDischargerStatusType.Success || result == BillDischargerStatusType.Success_Before_PowerOn || result == BillDischargerStatusType.StatusSuccess_Before_BillRejcet)
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|MoneyBillStatus", "지폐방출기 상태정상:" + GetMessage(result));
                    return new Result(true, GetMessage(result));
                }
                else
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|MoneyBillStatus", "지폐방출기 상태비정상:" + GetMessage(result));
                    return new Result(false, GetMessage(result));
                }
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.BILLCHARGER, "MoneyBillOutDeviice|MoneyBillStatus", "지폐방출기 상태확인중 에러:" + ex.ToString());
                return new Result(ex);
            }
        }



        //int nCount : 배출할 매수
        //int *pChkSensor : CHK1,2센서에 감지된 매수
        //int *pExitSensor : EXIT센서에 감지된 매수
        //int *pRejected : Reject 트레이로 떨어진 건수 
        //int *pNearEnd : 0이면 지폐충분함, 1이면 지폐부족예보
        [DllImport("LCDM2000.DLL", EntryPoint = "LCDM2000_UpperOut")]
        private static extern int LCDM2000_UpperOut(int nCount, out int pChkSensor, out int pExitSensor, out int pRejected, out int pNearEnd);
        /// <summary>
        /// 지폐방출기 위부분에서 수량만큼 지폐를 방출한다
        /// </summary>
        /// <param name="OutUpbillCount"></param>
        /// <returns></returns>
        public static UpperOut OutUpperbill(int OutUpbillCount)
        {
            int nCount = OutUpbillCount;
            int pChkSensor = 0;
            int pExitSensor = 0;
            int pRejected = 0;
            int pNearEnd = 0;
            int LCDM2000_UpperOutStatus = LCDM2000_UpperOut(nCount, out pChkSensor, out pExitSensor, out pRejected, out pNearEnd);
            BillDischargerStatusType IMoneyBillResultStatus = ConvertResultType(LCDM2000_UpperOutStatus);
            UpperOut _OpperOut = new UpperOut();

            _OpperOut.nCount = nCount;
            _OpperOut.pChkSensor = pChkSensor;

            _OpperOut.pRejected = pRejected;
            _OpperOut.pNearEnd = pNearEnd;
            _OpperOut.Status = LCDM2000_UpperOutStatus;
            if (IMoneyBillResultStatus != BillDischargerStatusType.Success || IMoneyBillResultStatus != BillDischargerStatusType.Success_Before_PowerOn || IMoneyBillResultStatus != BillDischargerStatusType.StatusSuccess_Before_BillRejcet || IMoneyBillResultStatus != BillDischargerStatusType.LowerTraySamll || IMoneyBillResultStatus != BillDischargerStatusType.UpperTraySamll)
            {
                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutUpperbill", "방출기상태확인:" + GetMessage(IMoneyBillResultStatus));
                _OpperOut.isSuccess = true;
                _OpperOut.pExitSensor = pExitSensor;

            }
            else if (IMoneyBillResultStatus == BillDischargerStatusType.PollingStatus) // 폴링에러일때는 방출수량을 알수없으므로 방출된걸로 보고 수량을 임의로 넣는다
            {
                _OpperOut.isSuccess = false;
                _OpperOut.pExitSensor = OutUpbillCount;
                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutUpperbill", "방출기상태확인:" + GetMessage(IMoneyBillResultStatus) + "폴링에러일때는 방출수량을 알수없으므로 방출된걸로 보고 수량을 임의로 넣는다");

            }
            else
            {
                _OpperOut.isSuccess = false;
                _OpperOut.pExitSensor = pExitSensor;
                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutUpperbill", "방출기상태확인:" + GetMessage(IMoneyBillResultStatus));


            }
            return _OpperOut;
        }

        //int nCount : 배출할 매수
        //int *pChkSensor : CHK3,4센서에 감지된 매수
        //int *pExitSensor : EXIT센서에 감지된 매수
        //int *pRejected : Reject 트레이로 떨어진 건수 
        //int *pNearEnd : 0이면 지폐충분함, 1이면 지폐부족예보


        [DllImport("LCDM2000.DLL", EntryPoint = "LCDM2000_LowerOut")]
        private static extern int LCDM2000_LowerOut(int nCount, out int pChkSensor, out int pExitSensor, out int pRejected, out int pNearEnd);
        /// <summary>
        /// 지폐방출기 아래부분에서 수량만큼 지폐를 방출한다
        /// </summary>
        /// <param name="OutLowerbillCount"></param>
        /// <returns></returns>
        public static LowerOut OutLowerbill(int OutLowerbillCount)
        {
            int nCount = OutLowerbillCount;
            int pChkSensor = 0;
            int pExitSensor = 0;
            int pRejected = 0;
            int pNearEnd = 0;
            int LCDM2000_LowerOutStatus = LCDM2000_LowerOut(nCount, out pChkSensor, out pExitSensor, out pRejected, out pNearEnd);

            BillDischargerStatusType IMoneyBillResultStatus = ConvertResultType(LCDM2000_LowerOutStatus);
            LowerOut _LowerOut = new LowerOut();
            _LowerOut.nCount = nCount;
            _LowerOut.pChkSensor = pChkSensor;
            _LowerOut.pRejected = pRejected;
            _LowerOut.pNearEnd = pNearEnd;
            _LowerOut.Status = LCDM2000_LowerOutStatus;

            if (IMoneyBillResultStatus != BillDischargerStatusType.Success || IMoneyBillResultStatus != BillDischargerStatusType.Success_Before_PowerOn || IMoneyBillResultStatus != BillDischargerStatusType.StatusSuccess_Before_BillRejcet || IMoneyBillResultStatus != BillDischargerStatusType.LowerTraySamll || IMoneyBillResultStatus != BillDischargerStatusType.UpperTraySamll)
            {
                _LowerOut.isSuccess = true;

                _LowerOut.pExitSensor = pExitSensor;
                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutLowerbill", "방출기상태확인:" + GetMessage(IMoneyBillResultStatus));
            }
            else if (IMoneyBillResultStatus == BillDischargerStatusType.PollingStatus) // 폴링에러일때는 방출수량을 알수없으므로 방출된걸로 보고 수량을 임의로 넣는다
            {
                _LowerOut.isSuccess = false;
                _LowerOut.pExitSensor = OutLowerbillCount;

                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutLowerbill", "방출기상태확인:" + GetMessage(IMoneyBillResultStatus) + "폴링에러일때는 방출수량을 알수없으므로 방출된걸로 보고 수량을 임의로 넣는다");
            }
            else
            {
                _LowerOut.isSuccess = false;
                _LowerOut.pExitSensor = pExitSensor;
                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutLowerbill", "방출기상태확인:" + GetMessage(IMoneyBillResultStatus));
            }


            return _LowerOut;
        }




        //int nUpperCount : 트레이 위. 배출할 매수
        //int *pUpperChkSensor : 트레이 위. CHK1,2센서에 감지된 매수
        //int *pUpperExitSensor : 트레이 위. EXIT센서에 감지된 매수
        //int *pUpperRejected : 트레이 위. Reject 트레이로 떨어진 건수 
        //int *pUpperNearEnd : 트레이 위. 0이면 지폐충분함, 1이면 지폐부족예보

        //int nLowerCount : 트레이 아래. 배출할 매수
        //int *pLowerChkSensor : 트레이 아래. CHK1,2센서에 감지된 매수
        //int *pLowerExitSensor : 트레이 아래. EXIT센서에 감지된 매수
        //int *pLowerRejected : 트레이 아래. Reject 트레이로 떨어진 건수 
        //int *pLowerNearEnd :
        //트레이 아래. 0이면 지폐충분함, 1이면 지폐부족예보

        [DllImport("LCDM2000.DLL", EntryPoint = "LCDM2000_UpperLowerOut")]
        private static extern int LCDM2000_UpperLowerOut(int nUpperCount, int nLowerCount,
                                                        out int pUpperChkSensor, out int pUpperExitSensor,
                                                        out int pUpperRejected, out int pUpperNearEnd,
                                                        out int pLowerChkSensor, out int pLowerExitSensor,
                                                        out int pLowerRejected, out int pLowerNearEnd);


        [DllImport("LCDM2000.DLL", EntryPoint = "LCDM2000_UpperTest")]
        private static extern int LCDM2000_UpperTest(out int pOutCount, out int pNearEnd);


        [DllImport("LCDM2000.DLL", EntryPoint = "LCDM2000_LowerTest")]
        private static extern int LCDM2000_LowerTest(out int pOutCount, out int pNearEnd);

        // 장치 초기화 예)젬현상등
        [DllImport("LCDM2000.DLL", EntryPoint = "LCDM2000_Purge")]
        private static extern int LCDM2000_Purge();


        /// <summary>
        /// 한번에 20개씩 뿐이 방출이 안되기에 내부적으로 20개이상일때 방출을 여러번한다.
        /// </summary>
        /// <param name="qty1000"></param>
        /// <returns></returns>
        public static Result OutPut1000Qty(ref int qty1000)
        {
            try
            {
                int Over20Qty = qty1000 / 20;    // 20개인 수량
                int Samlled20Qty = qty1000 % 20; // 20개 이하의 수량
                int qtyOut20 = 0;
                int FailQty = qty1000;   // 방출 실패한 수량 저장

                for (int i = 1; i <= Over20Qty; i++)
                {
                    qtyOut20 = 20;
                    Result _result = MoneyBillOutDeviice.OutPut1000(ref qtyOut20); // 20개씩 방출
                    if (_result.Success)
                    {
                        FailQty = FailQty - 20;
                        qty1000 = FailQty;
                    }
                    else
                    {


                        FailQty = FailQty - (20 - qtyOut20); // 아직 방출하지 않는 숫자
                        qty1000 = FailQty;
                        Result _BillpurgeJob = MoneyBillOutDeviice.BillPurgeJob();
                        TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut1000Qty", "리셋후 상태값:" + _BillpurgeJob.Message);
                        return new Result(false, _result.Message);
                    }
                }
                if (qty1000 == 0)
                {
                    return new Result(true, "성공");
                }
                int sammIntQty = 0; // 방출해야할 수량 저장
                sammIntQty = Samlled20Qty;
                Result _result20Smalled = MoneyBillOutDeviice.OutPut1000(ref Samlled20Qty); // 20미만 방출
                if (_result20Smalled.Success)
                {
                    qty1000 = 0;
                    return new Result(true, "성공");
                }
                else
                {
                    qty1000 = FailQty - (sammIntQty - Samlled20Qty);
                    Result _BillpurgeJob = MoneyBillOutDeviice.BillPurgeJob();
                    TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut1000Qty", "지폐방출기 이상으로 초기화작업후 상태값:" + _BillpurgeJob.Message);
                    return new Result(false, _result20Smalled.Message);

                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "MoneyBillOutDeviice|OutPut1000Qty", "지폐방출중 예외사항:" + ex.ToString());
                return new Result(false, ex.ToString());
            }

        }
        /// <summary>
        /// 한번에 20개씩 뿐이 방출이 안되기에 내부적으로 20개이상일때 방출을 여러번한다.
        /// </summary>
        /// <param name="qty5000">인풋으로는 방출수량 OUTPUT으로는 방출실패수량</param>
        /// <returns></returns>
        public static Result OutPut5000Qty(ref int qty5000)
        {
            try
            {
                int Over20Qty = qty5000 / 20;    // 20개인 수량
                int Samlled20Qty = qty5000 % 20; // 20개 이하의 수량
                int qtyOut20 = 0;
                int FailQty = qty5000;   // 방출 실패한 수량 저장

                for (int i = 1; i <= Over20Qty; i++)
                {
                    qtyOut20 = 20;
                    Result _result = MoneyBillOutDeviice.OutPut5000(ref qtyOut20); // 20개씩 방출
                    if (_result.Success)
                    {
                        FailQty = FailQty - 20;
                        qty5000 = FailQty;
                    }
                    if (_result.Success == false)
                    {
                        FailQty = FailQty - (20 - qtyOut20); // 아직 방출하지 않는 숫자
                        qty5000 = FailQty;
                        Result _BillpurgeJob = MoneyBillOutDeviice.BillPurgeJob();

                        TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut5000Qty", "지폐방출기 이상으로 초기화작업후 상태값:" + _BillpurgeJob.Message);
                        return new Result(false, _result.Message);
                    }
                }
                if (qty5000 == 0)
                {
                    return new Result(true, "성공");
                }
                int sammIntQty = 0; // 방출해야할 수량 저장
                sammIntQty = Samlled20Qty;
                Result _result20Smalled = MoneyBillOutDeviice.OutPut5000(ref Samlled20Qty); // 20미만 방출
                if (_result20Smalled.Success)
                {
                    qty5000 = 0;
                    return new Result(true, "성공");
                }
                else
                {
                    qty5000 = FailQty - (sammIntQty - Samlled20Qty);
                    Result _BillpurgeJob = MoneyBillOutDeviice.BillPurgeJob();
                    TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut5000Qty", "지폐방출기 이상으로 초기화작업후 상태값:" + _BillpurgeJob.Message);
                    return new Result(false, _result20Smalled.Message);

                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "MoneyBillOutDeviice|OutPut5000Qty", "지폐방출중 예외사항:" + ex.ToString());
                return new Result(false, ex.ToString());
            }

        }
        /// <summary>
        /// 방출전 상태체크후 상태이상이면 리셋을 하지않고 실패로 간주하게 변경하고 다시 방출을 하지않음 20120612 수정버젼
        /// </summary>
        /// <param name="qty1000"></param>
        /// <returns></returns>
        public static Result OutPut1000(ref int qty1000)
        {
            try
            {
                LowerOut _loweroutput = MoneyBillOutDeviice.OutLowerbill(qty1000);
                int exitSensorQty = 0;

                exitSensorQty = _loweroutput.pExitSensor;
                if (_loweroutput.pRejected > 0) // 리젝트된 개수
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut1000", "1000원권리젝걸린개수:" + _loweroutput.pRejected.ToString());

                }
                if (exitSensorQty > 0 && exitSensorQty != qty1000) // 방출은 됬으나 수량이 맞지 않은경우
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut1000", "1000원권 부분 정상배출개수:" + exitSensorQty.ToString());
                }

                if (exitSensorQty == qty1000 && _loweroutput.isSuccess) // 정상 방출
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut1000", "1000원권 정상배출개수:" + exitSensorQty.ToString());
                    qty1000 = 0;
                    return new Result(true, "성공");
                }
                else if (exitSensorQty == qty1000 && _loweroutput.isSuccess == false) // 정상 방출됬으나 상태는 정상이아님(폴링에러)
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut1000", "1000원권 정상배출개수:" + exitSensorQty.ToString() + "  상태:폴링에러");
                    qty1000 = 0;
                    return new Result(false, GetMessage(ConvertResultType(_loweroutput.Status)));
                }
                else
                {
                    int outputQty1000 = 0;
                    Result _result = MoneyBillStatus();
                    if (exitSensorQty > qty1000) // 과방출
                    {
                        qty1000 = 0;
                        TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut1000", "지폐방출기 1000원권 과방출:" + (exitSensorQty - qty1000).ToString());
                        return new Result(false, _result.Message + " 과방출:" + (exitSensorQty - qty1000).ToString());
                    }
                    else
                    {
                        outputQty1000 = qty1000 - exitSensorQty;     // 배출해야할 5000원권 수량 
                        qty1000 = outputQty1000;
                        TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut1000", "1000원권배출안된개수:" + outputQty1000.ToString());
                        MoneyBillOutDeviice.BillDischargerStatusType LowerResult = ConvertResultType(_loweroutput.Status);
                        TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut1000", "1000원권 시도후 방출안됨으로 장비이상처리:" + GetMessage(LowerResult));
                        if (_result.Success == true)
                        {
                            return new Result(false, "정상 배출실패-지폐걸림 확인요망");
                        }
                        else
                        {
                            return new Result(false, _result.Message);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "MoneyBillOutDeviice|OutPut1000", "1000원권 방출중 에러:" + ex.ToString());
                return new Result(false, "MoneyBillOutDeviice|OutPut1000" + ex.ToString(), 98);
            }
        }

        /// <summary>
        /// 방출전 상태체크후 상태이상이면 리셋을 하지않고 실패로 간주하게 변경하고 다시 방출을 하지않음 20120612 수정버젼
        /// </summary>
        /// <param name="qty5000"></param>
        /// <returns></returns>
        public static Result OutPut5000(ref int qty5000)
        {
            try
            {
                // 방출시도
                UpperOut _UpperOut = MoneyBillOutDeviice.OutUpperbill(qty5000);

                int exitSensorQty = 0;
                exitSensorQty = _UpperOut.pExitSensor;
                if (_UpperOut.pRejected > 0)
                {

                    TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut5000", "5000원권리젝걸린개수:" + _UpperOut.pRejected.ToString());
                }
                if (exitSensorQty > 0 && exitSensorQty != qty5000)
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut5000", "5000원권 부분배출개수:" + exitSensorQty.ToString());
                }
                if (exitSensorQty == qty5000 && _UpperOut.isSuccess) // 수량이 맞고 리젝트로 떨어진게 없다면
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut5000", "5000원권 정상배출개수:" + exitSensorQty.ToString());
                    qty5000 = qty5000 - exitSensorQty;
                    return new Result(true, "성공");
                }
                else if (exitSensorQty == qty5000 && _UpperOut.isSuccess == false) // 정상 방출됬으나 상태는 정상이아님
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut5000", "5000원권 정상배출개수:" + exitSensorQty.ToString() + " 폴링에러");
                    qty5000 = qty5000 - exitSensorQty;
                    return new Result(false, GetMessage(ConvertResultType(_UpperOut.Status)));
                }
                else
                {
                    int outputQty5000 = 0;
                    Result _result = MoneyBillStatus();
                    if (exitSensorQty > qty5000)
                    {
                        qty5000 = 0;
                        TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut5000", "5000원권 과방출:" + (exitSensorQty - qty5000).ToString());
                        return new Result(false, _result.Message + " 과방출:" + (exitSensorQty - qty5000).ToString());

                    }
                    else
                    {
                        outputQty5000 = qty5000 - exitSensorQty;     // 배출해야할 5000원권 수량 
                        TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut5000", "5000원권배출안된개수:" + outputQty5000.ToString());
                        qty5000 = outputQty5000;
                        TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "MoneyBillOutDeviice|OutPut5000", "5000원권 시도후 방출안됨으로 장비이상처리:" + _result.Message);
                        if (_result.Success == true)
                        {
                            return new Result(false, "정상 배출실패-지폐걸림 확인요망");
                        }
                        else
                        {
                            return new Result(false, _result.Message);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "MoneyBillOutDeviice|OutPut5000", "500원방출중에러:" + ex.ToString());
                return new Result(false, "방출OutPut5000에러" + ex.ToString(), 98);
            }
        }


        /// <summary>
        /// 장비가 잼등이 생겼을때 시도하는 작업
        /// </summary>
        /// <returns></returns>
        private static BillDischargerStatusType BillPurge()
        {
            return ConvertResultType(LCDM2000_Purge());
        }

        public static Result BillPurgeJob()
        {
            try
            {

                BillDischargerStatusType result = BillPurge();

                if (result == BillDischargerStatusType.Success || result == BillDischargerStatusType.Success_Before_PowerOn || result == BillDischargerStatusType.StatusSuccess_Before_BillRejcet || result == BillDischargerStatusType.LowerTraySamll || result == BillDischargerStatusType.UpperTraySamll)
                {
                    return new Result(true, GetMessage(result));
                }
                else
                {
                    return new Result(false, GetMessage(result));
                }
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }


        // ROM 버전 체크
        [DllImport("LCDM2000.DLL", EntryPoint = "LCDM2000_ROMVersion")]
        public static extern int LCDM2000_ROMVersion();

        [DllImport("LCDM2000.DLL", EntryPoint = "LCDM2000_Log")]
        public static extern int LCDM2000_Log([MarshalAs(UnmanagedType.LPStr)]string path);

        [DllImport("LCDM2000.DLL", EntryPoint = "LCDM2000_SetPath")]
        public static extern int LCDM2000_SetPath([MarshalAs(UnmanagedType.LPStr)]string path);

        public enum BillDischargerStatusType
        {
            /// <summary>
            /// 정상
            /// </summary>
            Success = 0,
            /// <summary>
            /// 트레이 아래 수량부족
            /// </summary>
            LowerTraySamll = 2257,                      //트레이 아래 수량부족
            /// <summary>
            /// 트레이 위 수량부족
            /// </summary>
            UpperTraySamll = 2256,                         //트레이 위 수량부족
            /// <summary>
            /// 처음 파워켰을 때 정상
            /// </summary>
            Success_Before_PowerOn = 3,          //처음 파워켰을 때 정상
            /// <summary>
            /// 지폐불출후 정상
            /// </summary>
            StatusSuccess_Before_BillRejcet = 4, //지폐불출 후 정상
            /// <summary>
            /// 장비상태
            /// </summary>
            DeviceStatus = 2201,
            /// <summary>
            /// 통신상태
            /// </summary>
            CommunicationStatus = 2202,

            MIN_1000Alarm = 2211,
            MIN_5000Alarm = 2212,
            NONE_1000Alarm = 2217,
            NONE_5000Alarm = 2218,
            MAX_1000Alarm = 2223,
            MAX_5000Alarm = 2224,

            /// <summary>
            /// 포트오픈상태
            /// </summary>
            PortOpenStatus = 2229,
            /// <summary>
            /// 폴링상태
            /// </summary>
            PollingStatus = 2230,
            /// <summary>
            /// 파라메터상태
            /// </summary>
            ParameterStatus = 2231,
            /// <summary>
            /// 강제로 돈을가져가는
            /// </summary>
            Pickup_Status = 2232,
            /// <summary>
            /// CHK_1_2_SensorJam
            /// </summary>
            CHK_1_2_SensorJamStatus = 2233,                    //센서잼
            /// <summary>
            /// 지폐과방출
            /// </summary>
            BillManyOutRejectStatus = 2234,                    //지폐과방출 
            /// <summary>
            /// XIT센서 또는 EJT센서 잼 
            /// </summary>
            ExitSenSorAndEJTSensor_JAMStatus = 2235,           //XIT센서 또는 EJT센서 잼 
            /// <summary>
            /// DiviSensor 잼
            /// </summary>
            DiviSensorJamStatus = 2236,
            /// <summary>
            /// 알수없는 에러
            /// </summary>
            UnknownErrorStatus = 2237,                             //알수없는에러
            /// <summary>
            /// 수량체크에러(CHK3,4센서 ~ DIV센서)
            /// </summary>
            CHK_3_or_4_Sensor_Error = 2238,                      //수량체크에러(CHK3,4센서 ~ DIV센서)
            /// <summary>
            /// 
            /// </summary>
            Note_request_Status = 2239,                    //Note request error
            DIV_Sensor_and_EJT_Sensor = 2240,                      //수량체크에러(EJT센서 ~ EXIT센서)
            EJT_Sensor_and_EXIT_Sensor = 2241,                      //수량체크에러(EJT센서 ~ EXIT센서)
            RejectTrayNotEnableStatus = 2242,                 //Reject 트레이 인지불가
            MotorStopStatus = 2243,                           //모터 멈춤
            DviSensorJamStatus = 2244,                        //DIV센서 잼
            TimeOutStatus = 2245,                             //시간초과(DIV센서 ~ EJ 센서로부터)
            OverRejectStatus = 2246,                            //Over Reject
            UpperTrayNotEnableStatus = 2247,                    // 트레이 위 인지불가
            LowwerTrayNotEnableStatus = 2248,                 //트레이 아래 인지불가
            BillRejectTimeOutStatus = 2249,                   //분출시간초과
            EJTSensorJamStatus = 2250,                        //EJT센서 잼
            solenoidErrorStatus = 2251,                       //Diverter solenoid or SOL센서 에러
            SolSensorErrorStatus = 2252,                      //SOL센서 에러
            CHK_3_4_SensorJamStatus = 2253,                       //CHK3,4센서 잼
            PurgeStatus = 2254,                           //Purge 에러(DIV센서 잼)
            BillNotDischarge = 2255                      //돈이 안나올때  새로추가

        }

        public static string GetMessage(BillDischargerStatusType pResult)
        {
            switch (pResult)
            {
                case BillDischargerStatusType.Success:
                    return "정상";
                case BillDischargerStatusType.PortOpenStatus:
                    return "포트오픈에러";
                case BillDischargerStatusType.CommunicationStatus:
                    return "통신에러";
                case BillDischargerStatusType.PollingStatus:
                    return "폴링에러";
                case BillDischargerStatusType.ParameterStatus:
                    return "파라메터 에러";
                case BillDischargerStatusType.Success_Before_PowerOn:
                    return "처음 파워켰을 때 정상";
                case BillDischargerStatusType.StatusSuccess_Before_BillRejcet:
                    return "지폐불출 후 정상";
                case BillDischargerStatusType.Pickup_Status:
                    return "Pickup_error";
                case BillDischargerStatusType.CHK_1_2_SensorJamStatus:
                    return "센서잼";

                case BillDischargerStatusType.BillManyOutRejectStatus:
                    return " 지폐과방출";

                case BillDischargerStatusType.ExitSenSorAndEJTSensor_JAMStatus:
                    return "XIT센서 또는 EJT센서 잼 ";

                case BillDischargerStatusType.DiviSensorJamStatus:
                    return "DiviSensorJam ";

                case BillDischargerStatusType.UnknownErrorStatus:
                    return " 알수없는에러";

                case BillDischargerStatusType.UpperTraySamll:
                    return " 트레이 위 수량부족";

                case BillDischargerStatusType.CHK_3_or_4_Sensor_Error:
                    return "수량체크에러(CHK3,4센서 ~ DIV센서 ";

                case BillDischargerStatusType.Note_request_Status:
                    return " Note request error";

                case BillDischargerStatusType.DIV_Sensor_and_EJT_Sensor:
                    return " 수량체크에러(EJT센서 ~ EXIT센서)";

                case BillDischargerStatusType.EJT_Sensor_and_EXIT_Sensor:
                    return " 수량체크에러(EJT센서 ~ EXIT센서)";

                case BillDischargerStatusType.RejectTrayNotEnableStatus:
                    return " Reject 트레이 인지불가";


                case BillDischargerStatusType.LowerTraySamll:
                    return "트레이 아래 수량부족";

                case BillDischargerStatusType.MotorStopStatus:
                    return "모터 멈춤 ";


                case BillDischargerStatusType.DviSensorJamStatus:
                    return "DIV센서 잼 ";

                case BillDischargerStatusType.TimeOutStatus:
                    return " 시간초과(DIV센서 ~ EJ 센서로부터)";

                case BillDischargerStatusType.OverRejectStatus:
                    return "Over Reject ";

                case BillDischargerStatusType.UpperTrayNotEnableStatus:
                    return "트레이 위 인지불가 ";


                case BillDischargerStatusType.LowwerTrayNotEnableStatus:
                    return "트레이 아래 인지불가 ";

                case BillDischargerStatusType.BillRejectTimeOutStatus:
                    return " 분출시간초과";


                case BillDischargerStatusType.EJTSensorJamStatus:
                    return " EJT센서 잼";


                case BillDischargerStatusType.solenoidErrorStatus:
                    return "Diverter solenoid or SOL센서 에러 ";

                case BillDischargerStatusType.CHK_3_4_SensorJamStatus:
                    return " CHK3,4센서 잼";

                case BillDischargerStatusType.PurgeStatus:
                    return " Purge 에러(DIV센서 잼)";

                case BillDischargerStatusType.BillNotDischarge:
                    return " 돈이배출되지 않음";

                default:
                    return "알수없음";

            }
        }

        private static BillDischargerStatusType ConvertResultType(int pCode, bool pSatusType = true)
        {
            switch (pCode)
            {
                case -999:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.PortOpenStatus, false);
                    return BillDischargerStatusType.PortOpenStatus;

                case -100:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.CommunicationStatus, false);
                    return BillDischargerStatusType.CommunicationStatus;


                case -1:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.PollingStatus, false);
                    return BillDischargerStatusType.PollingStatus;

                case 0:
                    if (pSatusType)
                    {
                        BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.Success, true);
                        return BillDischargerStatusType.Success;
                    }
                    else
                    {
                        BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.CommunicationStatus, false);
                        return BillDischargerStatusType.CommunicationStatus;
                    }


                case -10:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.ParameterStatus, false);
                    return BillDischargerStatusType.ParameterStatus;


                case 48:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.Success_Before_PowerOn, false);
                    return BillDischargerStatusType.Success_Before_PowerOn;


                case 49:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.StatusSuccess_Before_BillRejcet, false);
                    return BillDischargerStatusType.StatusSuccess_Before_BillRejcet;


                case 50:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.Pickup_Status, false);
                    return BillDischargerStatusType.Pickup_Status;


                case 51:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.CHK_1_2_SensorJamStatus, false);
                    return BillDischargerStatusType.CHK_1_2_SensorJamStatus;


                case 52:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.BillManyOutRejectStatus, false);
                    return BillDischargerStatusType.BillManyOutRejectStatus;



                case 53:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.ExitSenSorAndEJTSensor_JAMStatus, false);
                    return BillDischargerStatusType.ExitSenSorAndEJTSensor_JAMStatus;


                case 54:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.DiviSensorJamStatus, false);
                    return BillDischargerStatusType.DiviSensorJamStatus;


                case 55:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.UnknownErrorStatus, false);
                    return BillDischargerStatusType.UnknownErrorStatus;


                case 56:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.UpperTraySamll, false);
                    return BillDischargerStatusType.UpperTraySamll;


                case 58:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.CHK_3_or_4_Sensor_Error, false);
                    return BillDischargerStatusType.CHK_3_or_4_Sensor_Error;


                case 59:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.Note_request_Status, false);
                    return BillDischargerStatusType.Note_request_Status;


                case 60:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.DIV_Sensor_and_EJT_Sensor, false);
                    return BillDischargerStatusType.DIV_Sensor_and_EJT_Sensor;


                case 61:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.EJT_Sensor_and_EXIT_Sensor, false);
                    return BillDischargerStatusType.EJT_Sensor_and_EXIT_Sensor;


                case 63:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.RejectTrayNotEnableStatus, false);
                    return BillDischargerStatusType.RejectTrayNotEnableStatus;


                case 64:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.LowerTraySamll, false);
                    return BillDischargerStatusType.LowerTraySamll;


                case 65:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.MotorStopStatus, false);
                    return BillDischargerStatusType.MotorStopStatus;


                case 66:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.DviSensorJamStatus, false);
                    return BillDischargerStatusType.DviSensorJamStatus;


                case 67:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.TimeOutStatus, false);
                    return BillDischargerStatusType.TimeOutStatus;


                case 68:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.OverRejectStatus, false);
                    return BillDischargerStatusType.OverRejectStatus;


                case 69:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.UpperTrayNotEnableStatus, false);
                    return BillDischargerStatusType.UpperTrayNotEnableStatus;


                case 71:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.BillRejectTimeOutStatus, false);
                    return BillDischargerStatusType.BillRejectTimeOutStatus;


                case 72:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.EJTSensorJamStatus, false);
                    return BillDischargerStatusType.EJTSensorJamStatus;


                case 73:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.solenoidErrorStatus, false);
                    return BillDischargerStatusType.solenoidErrorStatus;


                case 74:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.SolSensorErrorStatus, false);
                    return BillDischargerStatusType.SolSensorErrorStatus;


                case 76:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.CHK_3_4_SensorJamStatus, false);
                    return BillDischargerStatusType.CHK_3_4_SensorJamStatus;


                case 78:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.PurgeStatus, false);
                    return BillDischargerStatusType.PurgeStatus;

                case 100:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.BillNotDischarge, false);
                    return BillDischargerStatusType.BillNotDischarge;

                default:
                    BillDischargeStatusManageMent.SetBillDischargerDeviceStatus(BillDischargerStatusType.Success, true);
                    return BillDischargerStatusType.Success;
            }

        }


        public enum BillAmount
        {
            billMany = 0,
            billSmall = 1
        }

        public class BillDischargeStatusManageMent
        {
            public static void SetDbErrorInfo(DataTable pPrinterData)
            {
                if (pPrinterData != null && pPrinterData.Rows.Count > 0)
                {
                    foreach (DataRow printerItem in pPrinterData.Rows)
                    {
                        int statusCode = Convert.ToInt32(printerItem["STATUSCODE"].ToString());
                        bool isSuccess = true;

                        if (printerItem["ISSUCCESS"].ToString() == CommProtocol.DeviceStatus.Success.ToString())
                        {
                            isSuccess = true;
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, statusCode);
                        }
                        else if (printerItem["ISSUCCESS"].ToString() == CommProtocol.DeviceStatus.Fail.ToString())
                        {
                            isSuccess = false;
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, statusCode);
                        }

                        switch ((BillDischargerStatusType)statusCode)
                        {
                            case BillDischargerStatusType.BillManyOutRejectStatus:
                                mIsBillManyOutRejectStatusError = !isSuccess;
                                break;
                            case BillDischargerStatusType.BillNotDischarge:
                                mIsBillNotDischargeStatusError = !isSuccess;
                                break;
                            case BillDischargerStatusType.BillRejectTimeOutStatus:
                                mIsBillRejectTimeOutStatusERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.CHK_1_2_SensorJamStatus:
                                mIsCHK_1_2_SensorJamError = !isSuccess;
                                break;
                            case BillDischargerStatusType.CHK_3_4_SensorJamStatus:
                                mIsCHK_3_4_SensorJamStatusError = !isSuccess;
                                break;
                            case BillDischargerStatusType.CHK_3_or_4_Sensor_Error:
                                mIsCHK_3_or_4_SensorERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.CommunicationStatus:
                                mIsComuniCationError = !isSuccess;
                                break;
                            case BillDischargerStatusType.DeviceStatus:
                                mIsDeviceStatusError = !isSuccess;
                                break;
                            case BillDischargerStatusType.DiviSensorJamStatus:
                                mIsDiviSensorJamError = !isSuccess;
                                break;
                            case BillDischargerStatusType.DIV_Sensor_and_EJT_Sensor:
                                mIsDIV_Sensor_and_EJT_SensorERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.DviSensorJamStatus:
                                mIsDviSensorJamStatusERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.EJTSensorJamStatus:
                                mIsEJTSensorJamStatusERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.EJT_Sensor_and_EXIT_Sensor:
                                mIsEJT_Sensor_and_EXIT_SensorERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.ExitSenSorAndEJTSensor_JAMStatus:
                                mIsExitSenSorAndEJTSensor_JAMError = !isSuccess;
                                break;
                            case BillDischargerStatusType.LowerTraySamll:
                                mIsLowerTraySamllAlarm = !isSuccess;
                                break;
                            case BillDischargerStatusType.LowwerTrayNotEnableStatus:
                                mIsLowwerTrayNotEnableStatusERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.MotorStopStatus:
                                mIsMotorStopStatusERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.Note_request_Status:
                                mIsNote_requestERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.OverRejectStatus:
                                mIsOverRejectStatusERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.ParameterStatus:
                                mIsParameterError = !isSuccess;
                                break;
                            case BillDischargerStatusType.Pickup_Status:
                                mIsPickUpError = !isSuccess;
                                break;
                            case BillDischargerStatusType.PollingStatus:
                                mIsPollingError = !isSuccess;
                                break;
                            case BillDischargerStatusType.PortOpenStatus:
                                mIsPortOpenError = !isSuccess;
                                break;
                            case BillDischargerStatusType.PurgeStatus:
                                mIsPurgeStatusError = !isSuccess;
                                break;
                            case BillDischargerStatusType.RejectTrayNotEnableStatus:
                                mIsRejectTrayNotEnableStatusERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.solenoidErrorStatus:
                                mIssolenoidStatusERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.SolSensorErrorStatus:
                                mIsSolSensorError = !isSuccess;
                                break;
                            case BillDischargerStatusType.StatusSuccess_Before_BillRejcet:
                                break;
                            case BillDischargerStatusType.Success:
                                break;
                            case BillDischargerStatusType.Success_Before_PowerOn:
                                break;
                            case BillDischargerStatusType.TimeOutStatus:
                                mIsTimeOutStatusERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.UnknownErrorStatus:
                                mIsUNKOWN_ERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.UpperTrayNotEnableStatus:
                                mIsUpperTrayNotEnableStatusERROR = !isSuccess;
                                break;
                            case BillDischargerStatusType.UpperTraySamll:
                                mIsUpperTraySamllAlarm = !isSuccess;
                                break;
                            case BillDischargerStatusType.MAX_1000Alarm:
                                mIsMax1000QtyError = !isSuccess;
                                break;
                            case BillDischargerStatusType.MAX_5000Alarm:
                                mIsMax5000QtyError = !isSuccess;
                                break;

                            case BillDischargerStatusType.MIN_1000Alarm:
                                mIsMin1000QtyError = !isSuccess;
                                break;

                            case BillDischargerStatusType.MIN_5000Alarm:
                                mIsMin5000QtyError = !isSuccess;
                                break;

                            case BillDischargerStatusType.NONE_1000Alarm:
                                mIsNone1000QtyError = !isSuccess;
                                break;

                            case BillDischargerStatusType.NONE_5000Alarm:
                                mIsNone5000QtyError = !isSuccess;
                                break;

                        }
                    }
                    NPSYS.Device.gIsUseDeviceBillDischargeDevice = GetBillDischargerDeveiceOpertationYn();
                }
                else
                {
                    mIsBillManyOutRejectStatusError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.BillManyOutRejectStatus);

                    mIsBillNotDischargeStatusError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.BillNotDischarge);

                    mIsBillRejectTimeOutStatusERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.BillRejectTimeOutStatus);

                    mIsCHK_1_2_SensorJamError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.CHK_1_2_SensorJamStatus);

                    mIsCHK_3_4_SensorJamStatusError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.CHK_3_4_SensorJamStatus);

                    mIsCHK_3_or_4_SensorERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.CHK_3_or_4_Sensor_Error);

                    mIsComuniCationError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.CommunicationStatus);

                    mIsDeviceStatusError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.DeviceStatus);

                    mIsDiviSensorJamError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.DiviSensorJamStatus);

                    mIsDIV_Sensor_and_EJT_SensorERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.DIV_Sensor_and_EJT_Sensor);

                    mIsDviSensorJamStatusERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.DviSensorJamStatus);

                    mIsEJTSensorJamStatusERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.EJTSensorJamStatus);


                    mIsEJT_Sensor_and_EXIT_SensorERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.EJT_Sensor_and_EXIT_Sensor);

                    mIsExitSenSorAndEJTSensor_JAMError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.ExitSenSorAndEJTSensor_JAMStatus);

                    mIsLowerTraySamllAlarm = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.LowerTraySamll);

                    mIsLowwerTrayNotEnableStatusERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.LowwerTrayNotEnableStatus);

                    mIsMotorStopStatusERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.MotorStopStatus);

                    mIsNote_requestERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.Note_request_Status);

                    mIsOverRejectStatusERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.OverRejectStatus);

                    mIsParameterError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.ParameterStatus);

                    mIsPickUpError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.Pickup_Status);

                    mIsPollingError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.PollingStatus);

                    mIsPortOpenError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.PortOpenStatus);

                    mIsPurgeStatusError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.PurgeStatus);

                    mIsRejectTrayNotEnableStatusERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.RejectTrayNotEnableStatus);

                    mIssolenoidStatusERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.solenoidErrorStatus);

                    mIsSolSensorError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.SolSensorErrorStatus);

                    mIsTimeOutStatusERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.TimeOutStatus);

                    mIsUNKOWN_ERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.UnknownErrorStatus);

                    mIsUpperTrayNotEnableStatusERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.UpperTrayNotEnableStatus);

                    mIsUpperTraySamllAlarm = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.UpperTraySamll);

                    mIsNone1000QtyError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.NONE_1000Alarm);

                    mIsNone5000QtyError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.NONE_5000Alarm);

                    mIsMax1000QtyError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.MAX_1000Alarm);

                    mIsMax5000QtyError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.MAX_5000Alarm);

                    mIsMin1000QtyError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.MIN_1000Alarm);

                    mIsMin5000QtyError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.MIN_5000Alarm);

                    NPSYS.Device.gIsUseDeviceBillDischargeDevice = GetBillDischargerDeveiceOpertationYn();
                }
            }

            public static void SendAllDeviveOk()
            {
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.BillManyOutRejectStatus);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.BillNotDischarge);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.BillRejectTimeOutStatus);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.CHK_1_2_SensorJamStatus);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.CHK_3_4_SensorJamStatus);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.CHK_3_or_4_Sensor_Error);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.CommunicationStatus);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.DeviceStatus);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.DiviSensorJamStatus);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.DIV_Sensor_and_EJT_Sensor);

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.DviSensorJamStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.EJTSensorJamStatus);



                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.EJT_Sensor_and_EXIT_Sensor);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.ExitSenSorAndEJTSensor_JAMStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.LowerTraySamll);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.LowwerTrayNotEnableStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.MotorStopStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.Note_request_Status);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.OverRejectStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.ParameterStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.Pickup_Status);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.PollingStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.PortOpenStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.PurgeStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.RejectTrayNotEnableStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.solenoidErrorStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.SolSensorErrorStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.TimeOutStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.UnknownErrorStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.UpperTrayNotEnableStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.UpperTraySamll);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.NONE_1000Alarm);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.NONE_5000Alarm);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.MAX_1000Alarm);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.MAX_5000Alarm);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.MIN_1000Alarm);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)BillDischargerStatusType.MIN_5000Alarm);


            }

            public static void SetBillDischargerDeviceStatus(MoneyBillOutDeviice.BillDischargerStatusType pResultType, bool isSuccess)
            {
                switch (pResultType)
                {

                    case BillDischargerStatusType.StatusSuccess_Before_BillRejcet:
                    case BillDischargerStatusType.Success:
                    case BillDischargerStatusType.Success_Before_PowerOn:
                        SetUpperTraySamllStatusOk(true);
                        SetLowerTraySamllStatusOk(true);
                        SetBillManyOutRejectStatus(true);
                        SetBillNotDischargeStatusOk(true);
                        SetCHK_1_2SensorJamStatusOk(true);
                        SetCHK_3_4_SensorJamStatusOk(true);
                        SetCHK_3_or_4_SensorOk(true);
                        SetComuniCationOk(true);
                        SetDeviceStatusOk(true);
                        SetDiviSensorJamStatus(true);
                        SetDIV_Sensor_and_EJT_SensorOk(true);
                        SetDviSensorJamStatusOk(true);
                        SetEJTSensorJamStatusOk(true);
                        SetEJT_Sensor_and_EXIT_SensorOk(true);
                        SetExitSenSorAndEJTSensorStatus(true);
                        SetLowwerTrayNotEnableStatusOk(true);
                        SetmBillRejectTimeOutStatusOk(true);
                        SetMotorStopStatusOk(true);
                        SetNote_requestOk(true);
                        SetOverRejectStatusOk(true);
                        SetParameterStatusOk(true);
                        SetPickUpStatusOk(true);
                        SetPollingOk(true);
                        SetPortOpenOk(true);
                        SetPurgeStatusOk(true);
                        SetRejectTrayNotEnableStatusOk(true);
                        SetsolenoidStatusOk(true);
                        SetSolSensorStatusOk(true);
                        SetTimeOutStatusOk(true);
                        SetUnKnownOk(true);
                        SetUpperTrayNotEnableStatusOk(true);
                        break;
                    case BillDischargerStatusType.UpperTraySamll:
                        SetUpperTraySamllStatusOk(isSuccess);

                        SetBillManyOutRejectStatus(true);
                        SetBillNotDischargeStatusOk(true);
                        SetCHK_1_2SensorJamStatusOk(true);
                        SetCHK_3_4_SensorJamStatusOk(true);
                        SetCHK_3_or_4_SensorOk(true);
                        SetComuniCationOk(true);
                        SetDeviceStatusOk(true);
                        SetDiviSensorJamStatus(true);
                        SetDIV_Sensor_and_EJT_SensorOk(true);
                        SetDviSensorJamStatusOk(true);
                        SetEJTSensorJamStatusOk(true);
                        SetEJT_Sensor_and_EXIT_SensorOk(true);
                        SetExitSenSorAndEJTSensorStatus(true);
                        SetLowwerTrayNotEnableStatusOk(true);
                        SetmBillRejectTimeOutStatusOk(true);
                        SetMotorStopStatusOk(true);
                        SetNote_requestOk(true);
                        SetOverRejectStatusOk(true);
                        SetParameterStatusOk(true);
                        SetPickUpStatusOk(true);
                        SetPollingOk(true);
                        SetPortOpenOk(true);
                        SetPurgeStatusOk(true);
                        SetRejectTrayNotEnableStatusOk(true);
                        SetsolenoidStatusOk(true);
                        SetSolSensorStatusOk(true);
                        SetTimeOutStatusOk(true);
                        SetUnKnownOk(true);
                        SetUpperTrayNotEnableStatusOk(true);
                        break;
                    case BillDischargerStatusType.LowerTraySamll:
                        SetLowerTraySamllStatusOk(isSuccess);

                        SetBillManyOutRejectStatus(true);
                        SetBillNotDischargeStatusOk(true);
                        SetCHK_1_2SensorJamStatusOk(true);
                        SetCHK_3_4_SensorJamStatusOk(true);
                        SetCHK_3_or_4_SensorOk(true);
                        SetComuniCationOk(true);
                        SetDeviceStatusOk(true);
                        SetDiviSensorJamStatus(true);
                        SetDIV_Sensor_and_EJT_SensorOk(true);
                        SetDviSensorJamStatusOk(true);
                        SetEJTSensorJamStatusOk(true);
                        SetEJT_Sensor_and_EXIT_SensorOk(true);
                        SetExitSenSorAndEJTSensorStatus(true);
                        SetLowwerTrayNotEnableStatusOk(true);
                        SetmBillRejectTimeOutStatusOk(true);
                        SetMotorStopStatusOk(true);
                        SetNote_requestOk(true);
                        SetOverRejectStatusOk(true);
                        SetParameterStatusOk(true);
                        SetPickUpStatusOk(true);
                        SetPollingOk(true);
                        SetPortOpenOk(true);
                        SetPurgeStatusOk(true);
                        SetRejectTrayNotEnableStatusOk(true);
                        SetsolenoidStatusOk(true);
                        SetSolSensorStatusOk(true);
                        SetTimeOutStatusOk(true);
                        SetUnKnownOk(true);
                        SetUpperTrayNotEnableStatusOk(true);
                        break;
                    case BillDischargerStatusType.BillManyOutRejectStatus:
                        SetBillManyOutRejectStatus(isSuccess);
                        break;
                    case BillDischargerStatusType.BillNotDischarge:
                        SetBillNotDischargeStatusOk(isSuccess);
                        break;
                    case BillDischargerStatusType.BillRejectTimeOutStatus:
                        SetmBillRejectTimeOutStatusOk(isSuccess);
                        break;
                    case BillDischargerStatusType.CHK_1_2_SensorJamStatus:
                        SetCHK_1_2SensorJamStatusOk(isSuccess);
                        break;
                    case BillDischargerStatusType.CHK_3_4_SensorJamStatus:
                        SetCHK_3_4_SensorJamStatusOk(isSuccess);
                        break;
                    case BillDischargerStatusType.CHK_3_or_4_Sensor_Error:
                        SetCHK_3_or_4_SensorOk(isSuccess);
                        break;

                    case BillDischargerStatusType.CommunicationStatus:
                        SetComuniCationOk(isSuccess);
                        break;

                    case BillDischargerStatusType.DeviceStatus:
                        SetDeviceStatusOk(isSuccess);
                        break;

                    case BillDischargerStatusType.DiviSensorJamStatus:
                        SetDiviSensorJamStatus(isSuccess);
                        break;

                    case BillDischargerStatusType.DIV_Sensor_and_EJT_Sensor:
                        SetDIV_Sensor_and_EJT_SensorOk(isSuccess);
                        break;

                    case BillDischargerStatusType.DviSensorJamStatus:
                        SetDviSensorJamStatusOk(isSuccess);
                        break;
                    case BillDischargerStatusType.EJTSensorJamStatus:
                        SetEJTSensorJamStatusOk(isSuccess);
                        break;
                    case BillDischargerStatusType.EJT_Sensor_and_EXIT_Sensor:
                        SetEJT_Sensor_and_EXIT_SensorOk(isSuccess);
                        break;
                    case BillDischargerStatusType.ExitSenSorAndEJTSensor_JAMStatus:
                        SetExitSenSorAndEJTSensorStatus(isSuccess);
                        break;


                    case BillDischargerStatusType.LowwerTrayNotEnableStatus:
                        SetLowwerTrayNotEnableStatusOk(isSuccess);
                        break;

                    case BillDischargerStatusType.MotorStopStatus:
                        SetMotorStopStatusOk(isSuccess);
                        break;

                    case BillDischargerStatusType.Note_request_Status:
                        SetNote_requestOk(isSuccess);
                        break;

                    case BillDischargerStatusType.OverRejectStatus:
                        SetOverRejectStatusOk(isSuccess);
                        break;

                    case BillDischargerStatusType.ParameterStatus:
                        SetParameterStatusOk(isSuccess);
                        break;

                    case BillDischargerStatusType.Pickup_Status:
                        SetPickUpStatusOk(isSuccess);
                        break;

                    case BillDischargerStatusType.PollingStatus:
                        SetPollingOk(isSuccess);
                        break;

                    case BillDischargerStatusType.PortOpenStatus:
                        SetPortOpenOk(isSuccess);
                        break;

                    case BillDischargerStatusType.PurgeStatus:
                        SetPurgeStatusOk(isSuccess);
                        break;

                    case BillDischargerStatusType.RejectTrayNotEnableStatus:
                        SetRejectTrayNotEnableStatusOk(isSuccess);
                        break;

                    case BillDischargerStatusType.solenoidErrorStatus:
                        SetsolenoidStatusOk(isSuccess);
                        break;

                    case BillDischargerStatusType.SolSensorErrorStatus:
                        SetSolSensorStatusOk(isSuccess);
                        break;



                    case BillDischargerStatusType.TimeOutStatus:
                        SetTimeOutStatusOk(isSuccess);
                        break;
                    case BillDischargerStatusType.UnknownErrorStatus:
                        SetUnKnownOk(isSuccess);
                        break;
                    case BillDischargerStatusType.UpperTrayNotEnableStatus:
                        SetUpperTrayNotEnableStatusOk(isSuccess);
                        break;

                }

                NPSYS.Device.gIsUseDeviceBillDischargeDevice = GetBillDischargerDeveiceOpertationYn();
            }

            private static bool GetBillDischargerDeveiceOpertationYn()
            {
                if ((mIsBillManyOutRejectStatusError
                    || mIsBillNotDischargeStatusError
                    || mIsBillRejectTimeOutStatusERROR
                    || mIsCHK_1_2_SensorJamError
                    || mIsCHK_3_4_SensorJamStatusError
                    || mIsCHK_3_or_4_SensorERROR
                    || mIsComuniCationError
                    || mIsDeviceStatusError
                    || mIsDiviSensorJamError
                    || mIsDIV_Sensor_and_EJT_SensorERROR
                    || mIsDviSensorJamStatusERROR
                    || mIsEJTSensorJamStatusERROR
                    || mIsEJT_Sensor_and_EXIT_SensorERROR
                    || mIsExitSenSorAndEJTSensor_JAMError
                    || mIsLowwerTrayNotEnableStatusERROR
                    || mIsMotorStopStatusERROR
                    || mIsNote_requestERROR
                    || mIsOverRejectStatusERROR
                    || mIsParameterError
                    || mIsPickUpError
                    || mIsPollingError
                    || mIsPortOpenError
                    || mIsPurgeStatusError
                    || mIsRejectTrayNotEnableStatusERROR
                    || mIsMotorStopStatusERROR
                    || mIsNote_requestERROR
                    || mIsOverRejectStatusERROR
                    || mIsParameterError
                    || mIsPickUpError
                    || mIsPollingError
                    || mIsPortOpenError
                    || mIsPurgeStatusError
                    || mIsRejectTrayNotEnableStatusERROR
                    || mIssolenoidStatusERROR
                    || mIsSolSensorError
                    || mIsTimeOutStatusERROR
                    || mIsUNKOWN_ERROR
                    || mIsUpperTrayNotEnableStatusERROR

                    ) == true)
                {
                    return false;
                }
                return true;
            }






            private static bool mIsUpperTraySamllAlarm = false;
            public static bool IsUpperTraySamllAlarm
            {
                get { return mIsUpperTraySamllAlarm; }
            }


            private static void SetUpperTraySamllStatusOk(bool pIsUpperTraySamllOk)
            {

                if (mIsUpperTraySamllAlarm) // 기존에 에러였다면
                {
                    if (pIsUpperTraySamllOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.UpperTraySamll);
                        mIsUpperTraySamllAlarm = !pIsUpperTraySamllOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsUpperTraySamllOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.UpperTraySamll);

                        mIsUpperTraySamllAlarm = !pIsUpperTraySamllOk;

                    }
                }

            }

            private static bool mIsLowerTraySamllAlarm = false;
            public static bool IsLowerTraySamllAlarm
            {
                get { return mIsLowerTraySamllAlarm; }
            }


            private static void SetLowerTraySamllStatusOk(bool pIsLowerTraySamllOk)
            {

                if (mIsLowerTraySamllAlarm) // 기존에 에러였다면
                {
                    if (pIsLowerTraySamllOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.LowerTraySamll);
                        mIsLowerTraySamllAlarm = !pIsLowerTraySamllOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsLowerTraySamllOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.LowerTraySamll);

                        mIsLowerTraySamllAlarm = !pIsLowerTraySamllOk;

                    }
                }

            }


            private static bool mIsDeviceStatusError = false;
            public static bool IsDeviceStatusError
            {
                get { return mIsDeviceStatusError; }
            }


            private static void SetDeviceStatusOk(bool pIsDeviceOk)
            {
                if (mIsDeviceStatusError) // 기존에 에러였다면
                {
                    if (pIsDeviceOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.DeviceStatus);
                        mIsDeviceStatusError = !pIsDeviceOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsDeviceOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.DeviceStatus);

                        mIsDeviceStatusError = !pIsDeviceOk;

                    }
                }

            }

            private static bool mIsComuniCationError = false;
            public static bool IsComuniCationError
            {
                get { return mIsComuniCationError; }
            }


            private static void SetComuniCationOk(bool pIscommunication)
            {
                if (mIsComuniCationError) // 기존에 통신에러였다면
                {
                    if (pIscommunication) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.CommunicationStatus);
                        mIsComuniCationError = !pIscommunication;

                    }
                }
                else  // 기존에 정상일때
                {
                    if (pIscommunication == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.CommunicationStatus);

                        mIsComuniCationError = !pIscommunication;

                    }
                }

            }


            private static bool mIsPortOpenError = false;
            public static bool IsPortOpenError
            {
                get { return mIsPortOpenError; }
            }


            private static void SetPortOpenOk(bool pIsPortOpenOk)
            {
                if (mIsPortOpenError) // 기존에 에러였다면
                {
                    if (pIsPortOpenOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.PortOpenStatus);
                        mIsPortOpenError = !pIsPortOpenOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsPortOpenOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.PortOpenStatus);
                        mIsPortOpenError = !pIsPortOpenOk;

                    }
                }
            }


            private static bool mIsPollingError = false;
            public static bool IsPollingError
            {
                get { return mIsPollingError; }
            }


            private static void SetPollingOk(bool pIsPollingOk)
            {
                if (mIsPollingError) // 기존에 에러였다면
                {
                    if (pIsPollingOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.PollingStatus);
                        mIsPollingError = !pIsPollingOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsPollingOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.PollingStatus);
                        mIsPollingError = !pIsPollingOk;

                    }
                }
            }




            private static bool mIsParameterError = false;
            public static bool IsParameterError
            {
                get { return mIsParameterError; }
            }


            private static void SetParameterStatusOk(bool pIsParameterStatusOk)
            {
                if (mIsParameterError) // 기존에 에러였다면
                {
                    if (pIsParameterStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.ParameterStatus);
                        mIsParameterError = !pIsParameterStatusOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsParameterStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.ParameterStatus);
                        mIsParameterError = !pIsParameterStatusOk;

                    }
                }
            }


            private static bool mIsPickUpError = false;
            public static bool IsPickUpError
            {
                get { return mIsPickUpError; }
            }


            private static void SetPickUpStatusOk(bool pIsPickUpStatusOk)
            {
                if (mIsPickUpError) // 기존에 에러였다면
                {
                    if (pIsPickUpStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.Pickup_Status);
                        mIsPickUpError = !pIsPickUpStatusOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsPickUpStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.Pickup_Status);
                        mIsPickUpError = !pIsPickUpStatusOk;

                    }
                }
            }


            private static bool mIsCHK_1_2_SensorJamError = false;
            public static bool IsCHK_1_2_SensorJamError
            {
                get { return mIsCHK_1_2_SensorJamError; }
            }


            private static void SetCHK_1_2SensorJamStatusOk(bool pIsCHK_1_2SensorJamOk)
            {
                if (mIsCHK_1_2_SensorJamError) // 기존에 에러였다면
                {
                    if (pIsCHK_1_2SensorJamOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.CHK_1_2_SensorJamStatus);
                        mIsCHK_1_2_SensorJamError = !pIsCHK_1_2SensorJamOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsCHK_1_2SensorJamOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.CHK_1_2_SensorJamStatus);
                        mIsCHK_1_2_SensorJamError = !pIsCHK_1_2SensorJamOk;

                    }
                }
            }



            private static bool mIsBillManyOutRejectStatusError = false;
            public static bool IsBillManyOutRejectStatusError
            {
                get { return mIsBillManyOutRejectStatusError; }
            }


            private static void SetBillManyOutRejectStatus(bool pIsBillManyOutRejectStatusOk)
            {
                if (mIsBillManyOutRejectStatusError) // 기존에 에러였다면
                {
                    if (pIsBillManyOutRejectStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.BillManyOutRejectStatus);
                        mIsBillManyOutRejectStatusError = !pIsBillManyOutRejectStatusOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsBillManyOutRejectStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.BillManyOutRejectStatus);
                        mIsBillManyOutRejectStatusError = !pIsBillManyOutRejectStatusOk;

                    }
                }
            }



            private static bool mIsExitSenSorAndEJTSensor_JAMError = false;
            public static bool IsExitSenSorAndEJTSensor_JAMError
            {
                get { return mIsExitSenSorAndEJTSensor_JAMError; }
            }


            private static void SetExitSenSorAndEJTSensorStatus(bool pIsExitSenSorAndEJTSensorStatusOk)
            {
                if (mIsExitSenSorAndEJTSensor_JAMError) // 기존에 에러였다면
                {
                    if (pIsExitSenSorAndEJTSensorStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.ExitSenSorAndEJTSensor_JAMStatus);
                        mIsExitSenSorAndEJTSensor_JAMError = !pIsExitSenSorAndEJTSensorStatusOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsExitSenSorAndEJTSensorStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.ExitSenSorAndEJTSensor_JAMStatus);
                        mIsExitSenSorAndEJTSensor_JAMError = !pIsExitSenSorAndEJTSensorStatusOk;

                    }
                }
            }


            private static bool mIsDiviSensorJamError = false;
            public static bool IsDiviSensorJamError
            {
                get { return mIsDiviSensorJamError; }
            }


            private static void SetDiviSensorJamStatus(bool pIsDiviSensorJamStatusOk)
            {
                if (mIsDiviSensorJamError) // 기존에 에러였다면
                {
                    if (pIsDiviSensorJamStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.DiviSensorJamStatus);
                        mIsDiviSensorJamError = !pIsDiviSensorJamStatusOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsDiviSensorJamStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.DiviSensorJamStatus);
                        mIsDiviSensorJamError = !pIsDiviSensorJamStatusOk;

                    }
                }
            }



            private static bool mIsUNKOWN_ERROR = false;
            public static bool IsUNKOWN_ERROR
            {
                get { return mIsUNKOWN_ERROR; }
            }

            private static void SetUnKnownOk(bool pIsUnknownStatusOk)
            {
                if (mIsUNKOWN_ERROR) // 기존에 에러였다면
                {
                    if (pIsUnknownStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.UnknownErrorStatus);
                        mIsUNKOWN_ERROR = !pIsUnknownStatusOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsUnknownStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.UnknownErrorStatus);
                        mIsUNKOWN_ERROR = !pIsUnknownStatusOk;

                    }
                }
            }


            private static bool mIsCHK_3_or_4_SensorERROR = false;
            public static bool IsCHK_3_or_4_SensorERROR
            {
                get { return mIsCHK_3_or_4_SensorERROR; }
            }

            private static void SetCHK_3_or_4_SensorOk(bool pIsCHK_3_or_4_SensorOk)
            {
                if (mIsCHK_3_or_4_SensorERROR) // 기존에 에러였다면
                {
                    if (pIsCHK_3_or_4_SensorOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.CHK_3_or_4_Sensor_Error);
                        mIsCHK_3_or_4_SensorERROR = !pIsCHK_3_or_4_SensorOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsCHK_3_or_4_SensorOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.CHK_3_or_4_Sensor_Error);
                        mIsCHK_3_or_4_SensorERROR = !pIsCHK_3_or_4_SensorOk;

                    }
                }
            }

            private static bool mIsNote_requestERROR = false;
            public static bool IsNote_requestERROR
            {
                get { return mIsNote_requestERROR; }
            }

            private static void SetNote_requestOk(bool pIsNote_requestOk)
            {
                if (mIsNote_requestERROR) // 기존에 에러였다면
                {
                    if (pIsNote_requestOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.Note_request_Status);
                        mIsNote_requestERROR = !pIsNote_requestOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsNote_requestOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.Note_request_Status);
                        mIsNote_requestERROR = !pIsNote_requestOk;
                    }
                }
            }

            private static bool mIsDIV_Sensor_and_EJT_SensorERROR = false;
            public static bool IsDIV_Sensor_and_EJT_SensorERROR
            {
                get { return mIsDIV_Sensor_and_EJT_SensorERROR; }
            }

            private static void SetDIV_Sensor_and_EJT_SensorOk(bool pIsDIV_Sensor_and_EJT_SensorOk)
            {
                if (mIsDIV_Sensor_and_EJT_SensorERROR) // 기존에 에러였다면
                {
                    if (pIsDIV_Sensor_and_EJT_SensorOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.DIV_Sensor_and_EJT_Sensor);
                        mIsDIV_Sensor_and_EJT_SensorERROR = !pIsDIV_Sensor_and_EJT_SensorOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsDIV_Sensor_and_EJT_SensorOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.DIV_Sensor_and_EJT_Sensor);
                        mIsDIV_Sensor_and_EJT_SensorERROR = !pIsDIV_Sensor_and_EJT_SensorOk;
                    }
                }
            }

            private static bool mIsEJT_Sensor_and_EXIT_SensorERROR = false;
            public static bool IsEJT_Sensor_and_EXIT_SensorERROR
            {
                get { return mIsEJT_Sensor_and_EXIT_SensorERROR; }
            }

            private static void SetEJT_Sensor_and_EXIT_SensorOk(bool pIsEJT_Sensor_and_EXIT_SensorOk)
            {
                if (mIsEJT_Sensor_and_EXIT_SensorERROR) // 기존에 에러였다면
                {
                    if (pIsEJT_Sensor_and_EXIT_SensorOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.EJT_Sensor_and_EXIT_Sensor);
                        mIsEJT_Sensor_and_EXIT_SensorERROR = !pIsEJT_Sensor_and_EXIT_SensorOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsEJT_Sensor_and_EXIT_SensorOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.EJT_Sensor_and_EXIT_Sensor);
                        mIsEJT_Sensor_and_EXIT_SensorERROR = !pIsEJT_Sensor_and_EXIT_SensorOk;
                    }
                }
            }

            private static bool mIsRejectTrayNotEnableStatusERROR = false;
            public static bool IsRejectTrayNotEnableStatusERROR
            {
                get { return mIsRejectTrayNotEnableStatusERROR; }
            }

            private static void SetRejectTrayNotEnableStatusOk(bool pIsRejectTrayNotEnableStatusOk)
            {
                if (mIsRejectTrayNotEnableStatusERROR) // 기존에 에러였다면
                {
                    if (pIsRejectTrayNotEnableStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.RejectTrayNotEnableStatus);
                        mIsRejectTrayNotEnableStatusERROR = !pIsRejectTrayNotEnableStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsRejectTrayNotEnableStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.RejectTrayNotEnableStatus);
                        mIsRejectTrayNotEnableStatusERROR = !pIsRejectTrayNotEnableStatusOk;
                    }
                }
            }


            private static bool mIsMotorStopStatusERROR = false;
            public static bool IsMotorStopStatusERROR
            {
                get { return mIsMotorStopStatusERROR; }
            }

            private static void SetMotorStopStatusOk(bool pIsMotorStopStatusOk)
            {
                if (mIsMotorStopStatusERROR) // 기존에 에러였다면
                {
                    if (pIsMotorStopStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.MotorStopStatus);
                        mIsMotorStopStatusERROR = !pIsMotorStopStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsMotorStopStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.MotorStopStatus);
                        mIsMotorStopStatusERROR = !pIsMotorStopStatusOk;
                    }
                }
            }


            private static bool mIsDviSensorJamStatusERROR = false;
            public static bool IsDviSensorJamStatusERROR
            {
                get { return mIsDviSensorJamStatusERROR; }
            }

            private static void SetDviSensorJamStatusOk(bool pIsDviSensorJamStatusOk)
            {
                if (mIsDviSensorJamStatusERROR) // 기존에 에러였다면
                {
                    if (pIsDviSensorJamStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.DviSensorJamStatus);
                        mIsDviSensorJamStatusERROR = !pIsDviSensorJamStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsDviSensorJamStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.DviSensorJamStatus);
                        mIsDviSensorJamStatusERROR = !pIsDviSensorJamStatusOk;
                    }
                }
            }


            private static bool mIsTimeOutStatusERROR = false;
            public static bool IsTimeOutStatusERROR
            {
                get { return mIsTimeOutStatusERROR; }
            }

            private static void SetTimeOutStatusOk(bool pIsTimeOutStatusOk)
            {
                if (mIsTimeOutStatusERROR) // 기존에 에러였다면
                {
                    if (pIsTimeOutStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.TimeOutStatus);
                        mIsTimeOutStatusERROR = !pIsTimeOutStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsTimeOutStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.TimeOutStatus);
                        mIsTimeOutStatusERROR = !pIsTimeOutStatusOk;
                    }
                }
            }

            private static bool mIsOverRejectStatusERROR = false;
            public static bool IsOverRejectStatusERROR
            {
                get { return mIsOverRejectStatusERROR; }
            }

            private static void SetOverRejectStatusOk(bool pIsOverRejectStatusOk)
            {
                if (mIsOverRejectStatusERROR) // 기존에 에러였다면
                {
                    if (pIsOverRejectStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.OverRejectStatus);
                        mIsOverRejectStatusERROR = !pIsOverRejectStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsOverRejectStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.OverRejectStatus);
                        mIsOverRejectStatusERROR = !pIsOverRejectStatusOk;
                    }
                }
            }


            private static bool mIsUpperTrayNotEnableStatusERROR = false;
            public static bool IsUpperTrayNotEnableStatusERROR
            {
                get { return mIsUpperTrayNotEnableStatusERROR; }
            }

            private static void SetUpperTrayNotEnableStatusOk(bool pIsUpperTrayNotEnableStatusOk)
            {
                if (mIsUpperTrayNotEnableStatusERROR) // 기존에 에러였다면
                {
                    if (pIsUpperTrayNotEnableStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.UpperTrayNotEnableStatus);
                        mIsUpperTrayNotEnableStatusERROR = !pIsUpperTrayNotEnableStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsUpperTrayNotEnableStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.UpperTrayNotEnableStatus);
                        mIsUpperTrayNotEnableStatusERROR = !pIsUpperTrayNotEnableStatusOk;
                    }
                }
            }


            private static bool mIsLowwerTrayNotEnableStatusERROR = false;
            public static bool IsLowwerTrayNotEnableStatusERROR
            {
                get { return mIsLowwerTrayNotEnableStatusERROR; }
            }

            private static void SetLowwerTrayNotEnableStatusOk(bool pIsLowwerTrayNotEnableStatusOk)
            {
                if (mIsLowwerTrayNotEnableStatusERROR) // 기존에 에러였다면
                {
                    if (pIsLowwerTrayNotEnableStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.LowwerTrayNotEnableStatus);
                        mIsLowwerTrayNotEnableStatusERROR = !pIsLowwerTrayNotEnableStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsLowwerTrayNotEnableStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.LowwerTrayNotEnableStatus);
                        mIsLowwerTrayNotEnableStatusERROR = !pIsLowwerTrayNotEnableStatusOk;
                    }
                }
            }


            private static bool mIsBillRejectTimeOutStatusERROR = false;
            public static bool IsBillRejectTimeOutStatusERROR
            {
                get { return mIsBillRejectTimeOutStatusERROR; }
            }

            private static void SetmBillRejectTimeOutStatusOk(bool pIsBillRejectTimeOutStatusOk)
            {
                if (mIsBillRejectTimeOutStatusERROR) // 기존에 에러였다면
                {
                    if (pIsBillRejectTimeOutStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.BillRejectTimeOutStatus);
                        mIsBillRejectTimeOutStatusERROR = !pIsBillRejectTimeOutStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsBillRejectTimeOutStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.BillRejectTimeOutStatus);
                        mIsBillRejectTimeOutStatusERROR = !pIsBillRejectTimeOutStatusOk;
                    }
                }
            }


            private static bool mIsEJTSensorJamStatusERROR = false;
            public static bool IsEJTSensorJamStatusERROR
            {
                get { return mIsEJTSensorJamStatusERROR; }
            }

            private static void SetEJTSensorJamStatusOk(bool pIsEJTSensorJamStatusOk)
            {
                if (mIsEJTSensorJamStatusERROR) // 기존에 에러였다면
                {
                    if (pIsEJTSensorJamStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.EJTSensorJamStatus);
                        mIsEJTSensorJamStatusERROR = !pIsEJTSensorJamStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsEJTSensorJamStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.EJTSensorJamStatus);
                        mIsEJTSensorJamStatusERROR = !pIsEJTSensorJamStatusOk;
                    }
                }
            }

            private static bool mIssolenoidStatusERROR = false;
            public static bool IssolenoidStatusERROR
            {
                get { return mIssolenoidStatusERROR; }
            }

            private static void SetsolenoidStatusOk(bool pIssolenoidStatusOk)
            {
                if (mIssolenoidStatusERROR) // 기존에 에러였다면
                {
                    if (pIssolenoidStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.solenoidErrorStatus);
                        mIssolenoidStatusERROR = !pIssolenoidStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIssolenoidStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.solenoidErrorStatus);
                        mIssolenoidStatusERROR = !pIssolenoidStatusOk;
                    }
                }
            }
            private static bool mIsSolSensorError = false;
            public static bool IsSolSensorError
            {
                get { return mIsSolSensorError; }
            }

            private static void SetSolSensorStatusOk(bool pIsSolSensorStatusOk)
            {
                if (mIsSolSensorError) // 기존에 에러였다면
                {
                    if (pIsSolSensorStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.SolSensorErrorStatus);
                        mIsSolSensorError = !pIsSolSensorStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsSolSensorStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.SolSensorErrorStatus);
                        mIsSolSensorError = !pIsSolSensorStatusOk;
                    }
                }
            }

            private static bool mIsCHK_3_4_SensorJamStatusError = false;
            public static bool IsCHK_3_4_SensorJamStatusError
            {
                get { return mIsCHK_3_4_SensorJamStatusError; }
            }

            private static void SetCHK_3_4_SensorJamStatusOk(bool pIsCHK_3_4_SensorJamStatusOk)
            {
                if (mIsCHK_3_4_SensorJamStatusError) // 기존에 에러였다면
                {
                    if (pIsCHK_3_4_SensorJamStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.CHK_3_4_SensorJamStatus);
                        mIsCHK_3_4_SensorJamStatusError = !pIsCHK_3_4_SensorJamStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsCHK_3_4_SensorJamStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.CHK_3_4_SensorJamStatus);
                        mIsCHK_3_4_SensorJamStatusError = !pIsCHK_3_4_SensorJamStatusOk;
                    }
                }
            }

            private static bool mIsPurgeStatusError = false;
            public static bool IsPurgeStatusError
            {
                get { return mIsPurgeStatusError; }
            }

            private static void SetPurgeStatusOk(bool pIsPurgeStatusOk)
            {
                if (mIsPurgeStatusError) // 기존에 에러였다면
                {
                    if (pIsPurgeStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.PurgeStatus);
                        mIsPurgeStatusError = !pIsPurgeStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsPurgeStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.PurgeStatus);
                        mIsPurgeStatusError = !pIsPurgeStatusOk;
                    }
                }
            }

            private static bool mIsBillNotDischargeStatusError = false;
            public static bool IsBillNotDischargeStatusError
            {
                get { return mIsBillNotDischargeStatusError; }
            }

            private static void SetBillNotDischargeStatusOk(bool pIsBillNotDischargeStatusOk)
            {
                if (mIsBillNotDischargeStatusError) // 기존에 에러였다면
                {
                    if (pIsBillNotDischargeStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, (int)MoneyBillOutDeviice.BillDischargerStatusType.BillNotDischarge);
                        mIsBillNotDischargeStatusError = !pIsBillNotDischargeStatusOk;
                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsBillNotDischargeStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, (int)MoneyBillOutDeviice.BillDischargerStatusType.BillNotDischarge);
                        mIsBillNotDischargeStatusError = !pIsBillNotDischargeStatusOk;
                    }
                }
            }
            private static bool mIsMax1000QtyError = false;
            public static bool IsMax1000QtyError
            {
                get { return mIsMax1000QtyError; }
            }
            private static bool mIsMax5000QtyError = false;
            public static bool IsMax5000QtyError
            {
                get { return mIsMax5000QtyError; }
            }

            private static bool mIsMin1000QtyError = false;
            public static bool IsMin1000QtyError
            {
                get { return mIsMin1000QtyError; }
            }

            private static bool mIsMin5000QtyError = false;
            public static bool IsMin5000QtyError
            {
                get { return mIsMin5000QtyError; }
            }

            private static bool mIsNone1000QtyError = false;
            public static bool IsNone1000QtyError
            {
                get { return mIsNone1000QtyError; }
            }

            private static bool mIsNone5000QtyError = false;
            public static bool IsNone5000QtyError
            {
                get { return mIsNone5000QtyError; }
            }


            public static void setCash(int cash5000SettingQty, int cash1000SettingQty, int cash500SettingQty, int cash100SettingQty, int cash50SettingQty, int cash50MinQqty, int cash100MinQqty, int cash500MinQqty, int cash1000MinQqty, int cash5000MinQqty, int cash50MaxQqty, int cash100MaxQqty, int cash500MaxQqty, int cash1000MaxQqty, int cash5000MaxQqty)
            {
                if (NPSYS.Device.UsingSettingBill)
                {

                    if (cash1000SettingQty <= 0)  // 동전이 없다면
                    {
                        if (mIsNone1000QtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, MoneyBillOutDeviice.BillDischargerStatusType.NONE_1000Alarm);
                            mIsNone1000QtyError = true;
                        }
                    }
                    else // 1000원이 있다면
                    {
                        if (mIsNone1000QtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, MoneyBillOutDeviice.BillDischargerStatusType.NONE_1000Alarm);
                            mIsNone1000QtyError = false;
                        }

                    }
                    if (cash1000SettingQty <= cash1000MinQqty) // 최소수량 부족이면
                    {

                        if (mIsMin1000QtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, MoneyBillOutDeviice.BillDischargerStatusType.MIN_1000Alarm);
                            mIsMin1000QtyError = true;
                        }
                    }
                    else // 1000원이 있다면
                    {
                        if (mIsMin1000QtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, MoneyBillOutDeviice.BillDischargerStatusType.MIN_1000Alarm);
                            mIsMin1000QtyError = false;
                        }

                    }
                    if (cash1000SettingQty >= cash1000MaxQqty) // 최대수량 경고이면
                    {

                        if (mIsMax1000QtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, MoneyBillOutDeviice.BillDischargerStatusType.MAX_1000Alarm);
                            mIsMax1000QtyError = true;
                        }
                    }
                    else // 1000원이 있다면
                    {
                        if (mIsMax1000QtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, MoneyBillOutDeviice.BillDischargerStatusType.MAX_1000Alarm);
                            mIsMax1000QtyError = false;
                        }

                    }

                    if (cash5000SettingQty <= 0)  // 동전이 없다면
                    {
                        if (mIsNone5000QtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, MoneyBillOutDeviice.BillDischargerStatusType.NONE_5000Alarm);
                            mIsNone5000QtyError = true;
                        }
                    }
                    else // 5000원이 있다면
                    {
                        if (mIsNone5000QtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, MoneyBillOutDeviice.BillDischargerStatusType.NONE_5000Alarm);
                            mIsNone5000QtyError = false;
                        }

                    }
                    if (cash5000SettingQty <= cash5000MinQqty) // 최소수량 부족이면
                    {

                        if (mIsMin5000QtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, MoneyBillOutDeviice.BillDischargerStatusType.MIN_5000Alarm);
                            mIsMin5000QtyError = true;
                        }
                    }
                    else // 5000원이 있다면
                    {
                        if (mIsMin5000QtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, MoneyBillOutDeviice.BillDischargerStatusType.MIN_5000Alarm);
                            mIsMin5000QtyError = false;
                        }

                    }
                    if (cash5000SettingQty >= cash5000MaxQqty) // 최대수량 경고이면
                    {

                        if (mIsMax5000QtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Fail, MoneyBillOutDeviice.BillDischargerStatusType.MAX_5000Alarm);
                            mIsMax5000QtyError = true;
                        }
                    }
                    else // 5000원이 있다면
                    {
                        if (mIsMax5000QtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, CommProtocol.DeviceStatus.Success, MoneyBillOutDeviice.BillDischargerStatusType.MAX_5000Alarm);
                            mIsMax5000QtyError = false;
                        }

                    }

                }

            }


        }

    }
}
