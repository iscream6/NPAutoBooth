using FadeFox.Text;
using NPCommon.DTO;
using System;
using System.Data;
using System.Text;

namespace NPCommon
{
    public class LPRDbSelect
    {


        #region 조회관련


        #endregion



        #region 로컬DB저장관련

        /// <summary>
        /// 최종 결제 정보를 저장한다.
        /// </summary>
        /// <param name="pCarNumber"></param>
        /// <param name="pReceiveMoney"></param>
        /// <param name="p_car_type"></param>
        /// <param name="p_IOTYPE"></param>
        public static void LastestPayment_LOG_Insert(string pCarNumber, int pReceiveMoney, string p_IOTYPE, string message)
        {
            try
            {
                string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                StringBuilder sql = new StringBuilder();

                //Insert 시 기존에 있던 모든 자료를 지우고 다시 Insert를 해야한다. 따라서 Transaction 처리로 하자.
                NPSYS.NPPaymentLog.BeginTrans();

                //Delete
                sql.AppendLine("  DELETE FROM LastestPayment_LOG");
                NPSYS.NPPaymentLog.Execute(sql.ToString());

                //string 변수 NVL
                pCarNumber = pCarNumber == null ? "" : pCarNumber;
                p_IOTYPE = p_IOTYPE == null ? "" : p_IOTYPE;

                //Insert
                sql.Clear();
                sql.Append("INSERT INTO LastestPayment_LOG(CAR_NUMBER, RECEIVEMONEY, LOG_DATE, IO_TYPE) ");
                sql.Append("VALUES('" + pCarNumber + "'," + pReceiveMoney + ",'" + logDate + "','" + p_IOTYPE + "','" + message + "')");

                NPSYS.NPPaymentLog.Execute(sql.ToString());

                //커밋~!
                NPSYS.NPPaymentLog.CommitTrans();
            }
            catch (Exception ex)
            {
                //DB처리 오류로 인해 Rollback을 한다.
                NPSYS.NPPaymentLog.RollbackTrans();
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect|Car_Log_INsert", ex.Message);
            }
        }

        public static DataTable GetLastestPaymentLog()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("  SELECT CAR_NUMBER, RECEIVEMONEY, IO_TYPE, LOG_DATE, LOG_DESC ");
                sql.AppendLine("     FROM LastestPayment_LOG");
                DataTable dt = NPSYS.NPPaymentLog.SelectT(sql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect | GetLastestPaymentLog", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 일반차량인지 정기차량인지 프리타임차량인지 구분후 로그에 기록
        /// </summary>
        /// <param name="p_carnumber"></param>
        /// <param name="p_receivemoney"></param>
        /// <param name="p_car_type"></param>
        public static void Car_Log_INsert(string p_carnumber, int p_receivemoney, Car_Type p_car_type, string p_IOTYPE)
        {
            //  수정중
            try
            {
                string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                StringBuilder sql = new StringBuilder();


                sql.Append("INSERT INTO CAR_LOG(CAR_NUMBER,RECEIVEMONEY,CAR_TYPE,LOG_DATE,IO_TYPE,UPDATE_YN)");
                sql.Append("                  VALUES('" + p_carnumber + "'," + p_receivemoney + ",'" + p_car_type.ToString() + "','" + logDate + "','" + p_IOTYPE + "','N')");

                NPSYS.NPPaymentLog.Execute(sql.ToString());
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect|Car_Log_INsert", ex.ToString());

            }
        }

        public static void LogMoney(PaymentType pType, string pLogDate, NormalCarInfo Carinfo, MoneyType pMoneyType, int pInMoney, int pOutMoney, string pComment)
        {
            string sql = "";

            sql = "  INSERT INTO MONEY_LOG ( "
                + "         NORKEY,LOG_DATE, CAR_NUMBER, IN_DATE, PAYMENT_METHOD, IO_TYPE,"
                + "  	    IN_MONEY, OUT_MONEY, COMMENT"
                + "  	    )"
                + "  VALUES ("
                + "         '" + Carinfo.TkNO + "',"
                + "         '" + pLogDate + "',"
                + "         '" + Carinfo.OutCarNo1 + "',"
                + "         '" + Carinfo.OutYmd + Carinfo.OutHms + "',"
                + "         '" + pType + "',"
                + "         '" + pMoneyType + "',"
                + "         '" + pInMoney + "',"
                + "         '" + pOutMoney + "',"
                + "         '" + pComment + "'"
                + "         )";
            try
            {
                NPSYS.NPPaymentLog.Execute(sql);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect|LogMoney", ex.ToString());
            }
        }

        public static void LogChargeMoney(PaymentType pType, string pLogDate, MoneyType pMoneyType, int pInMoney, int pOutMoney, string pComment)  //통합관제 추가
        {
            string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "";

            sql = "  INSERT INTO MONEY_LOG ( "
                + "         NORKEY,LOG_DATE, CAR_NUMBER, IN_DATE, PAYMENT_METHOD, IO_TYPE,"
                + "  	    IN_MONEY, OUT_MONEY, COMMENT"
                + "  	    )"
                + "  VALUES ("
                + "         '" + "11" + "',"
                 + "         '" + pLogDate + "',"
                + "         '" + " " + "',"
                + "         '" + " " + "',"
                + "         '" + pType + "',"
                 + "         '" + pMoneyType + "',"
                 + "         '" + pInMoney + "',"
                + "         '" + pOutMoney + "',"
                + "         '" + "Charge" + "'"
                + "         )";
            try
            {
                NPSYS.NPPaymentLog.Execute(sql);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect|LogChargeMoney", ex.ToString());
            }

        }


        public static void Creditcard_Log_INsert(NormalCarInfo _p_Info)
        {
            //  수정중

            //+"  	NORKEY VARCHAR(20) NULL,"
            //+ "  	RESCODE VARCHAR(50),"
            //+ "  	AUTH_NUMBER VARCHAR(20),"
            //+ "     AUTH_DATE VARCHAR(20) NOT NULL,"
            //+ "  	IN_DATE VARCHAR(20) NOT NULL,"
            //+ "  	OUT_DATE VARCHAR(20) NOT NULL,"
            //+ "  	PARKTIME VARCHAR(20),"
            //+ "  	CAR_NUMBER VARCHAR(10) NOT NULL,"
            //+ "  	CREDIT_PAY  VARCHAR(15),"
            //+ "  	LOG_DATE VARCHAR(20) NOT NULL,"
            //+ "  	UPDATE_YN VARCHAR(1))";
            try
            {
                string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO CreditCard_LOG(NORKEY,RESCODE,RESMSG,AUTH_NUMBER,AUTH_DATE,IN_DATE, OUT_DATE ,PARKTIME , CAR_NUMBER , CREDIT_PAY ,CREDIT_TAX,CREDIT_SUPPLY, LOG_DATE , UPDATE_YN )");
                sql.Append("                   VALUES('" + _p_Info.TkNO + "','" + _p_Info.VanRescode + "','" + _p_Info.VanResMsg + "','" + _p_Info.VanRegNo + "','" + _p_Info.VanDate + "' , ");
                sql.Append("                          '" + _p_Info.InDate + _p_Info.InHms + " ','" + _p_Info.OutYmd + _p_Info.OutHms + "', '" + _p_Info.ParkingMin.ToString() + "', ");
                sql.Append("                          '" + _p_Info.OutCarNo1 + " '," + _p_Info.VanBeforeCardPay + "," + _p_Info.VanTaxPay + "," + _p_Info.VanSupplyPay + ",'" + logDate + "', 'N' )");


                NPSYS.NPPaymentLog.Execute(sql.ToString());
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect|Creditcard_Log_INsert", ex.ToString());
            }
        }

        #endregion



        public static DataTable GetDeivceErrorInfoFromDeviceCode(CommProtocol.device pDeviceCode)
        {
            try
            {
                string deviceCode = TextCore.ToRightAlignString(4, ((int)pDeviceCode).ToString(), '0');
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("  SELECT LOG_DATE,DEVICECODE, STATUSCODE, ISSUCCESS,RESERVE1,RESERVE2,RESERVE3,RESERVE4,RESERVE5");
                sql.AppendLine("     FROM ERRORDEVICE_LOG WHERE DEVICECODE='" + deviceCode + "'");
                DataTable dt = NPSYS.NPPaymentLog.SelectT(sql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect | GetDeivceErrorInfo", ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 장비의 상태코드가 등록되어있느지 확인
        /// </summary>
        /// <param name="pStatusCode"></param>
        /// <returns></returns>
        public static bool IsDeivceErrorInfoFromStatusCode(string pStatusCode)
        {
            try
            {
                pStatusCode = TextCore.ToRightAlignString(4, pStatusCode, '0');
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("  SELECT LOG_DATE,DEVICECODE, STATUSCODE, ISSUCCESS,RESERVE1,RESERVE2,RESERVE3,RESERVE4,RESERVE5");
                sql.AppendLine("     FROM ERRORDEVICE_LOG WHERE STATUSCODE='" + pStatusCode + "'");
                DataTable dt = NPSYS.NPPaymentLog.SelectT(sql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect | IsDeivceErrorInfoFromStatusCode", ex.ToString());
                return false; ;
            }
        }

        public static void UpdateErrorInfo(string pStatusCode, string pResultData)
        {


            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("  UPDATE ERRORDEVICE_LOG set ISSUCCESS='" + pResultData + "' Where STATUSCODE=" + pStatusCode.ToString() + "");
                sql.AppendLine("     ");
                NPSYS.NPPaymentLog.Execute(sql.ToString());

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect | UpdateErrorInfo", ex.ToString());
            }
        }

        public static void InsertErrorInfo(CommProtocol.device pDeviceCode, string pStatusCode, string pResultData)
        {


            try
            {
                string deviceCode = TextCore.ToRightAlignString(4, ((int)pDeviceCode).ToString(), '0');
                string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO ERRORDEVICE_LOG(LOG_DATE,DEVICECODE,STATUSCODE,ISSUCCESS,RESERVE1)");
                sql.Append("                   VALUES('" + logDate + "','" + deviceCode + "','" + pStatusCode + "','" + pResultData + "', ' ')");
                NPSYS.NPPaymentLog.Execute(sql.ToString()); ;

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect | InsertErrorInfo", ex.ToString());
            }
        }

        /// <summary>
        /// 결제 전송에 실패한 차량을 저장한다.
        /// </summary>
        /// <param name="pUrl"></param>
        /// <param name="pSendData"></param>
        public static void ReSendData_INsert(string pUrl, string pSendData)
        {


            try
            {
                string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO RESENDDATA_LOG(LOG_DATE,CURRENTURL,SENDDATA,FAILCOUNT,UPDATE_YN)");
                sql.Append("                   VALUES('" + logDate + "','" + pUrl + "','" + pSendData + "',0,'N')");
                NPSYS.NPPaymentLog.Execute(sql.ToString());
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect | SendData_INsert", ex.ToString());
            }
        }

        /// <summary>
        /// 재정송할 데이터를 불러온다
        /// </summary>
        /// <returns></returns>
        public static DataTable GetReSendData()
        {


            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("  SELECT LOG_DATE,ID, CURRENTURL, SENDDATA,FAILCOUNT");
                sql.AppendLine("     FROM RESENDDATA_LOG WHERE UPDATE_YN='N' ORDER BY LOG_DATE LIMIT 5");
                DataTable dt = NPSYS.NPPaymentLog.SelectT(sql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect | GetReSendData", ex.ToString());
                return null;
            }
        }

        public static void UpdateReSendData(int pId)
        {


            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("  UPDATE RESENDDATA_LOG set UPDATE_YN='Y' Where ID=" + pId.ToString() + "");
                sql.AppendLine("     ");
                NPSYS.NPPaymentLog.Execute(sql.ToString());

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect | UpdateReSendData", ex.ToString());
            }
        }

        public static void DeleteSuccessReSendData()
        {


            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("  DELETE RESENDDATA_LOG");
                sql.AppendLine("     WHERE UPDATE_YN='N' ");
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "LPRDbSelect | DeleteReSendData", "[재전송 성공데이터 삭제 데이터 삭제]");
                NPSYS.NPPaymentLog.Execute(sql.ToString());
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect | DeleteReSendData", ex.ToString());
            }
        }

        #region 기타


        #endregion


    }
}
