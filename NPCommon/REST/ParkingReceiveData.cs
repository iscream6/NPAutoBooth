using FadeFox.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPCommon.DTO.Receive;
using System;
using System.Collections.Generic;

namespace NPCommon.REST
{
    [Serializable]
    public class ParkingReceiveData
    {
        #region 받아오는 데이터
        /// <summary>
        /// 각 json의 해더정보
        /// </summary>
        public enum HeaderInfo
        {
            data,
            /// <summary>
            /// 상태값커맨드
            /// </summary>
            status,
            /// <summary>
            /// 날짜
            /// </summary>
            date,
            /// <summary>
            /// 지폐종류관련커맨드
            /// </summary>
            monetary,
            /// <summary>
            /// 주차장정보커맨드
            /// </summary>
            parkingLot,
            /// <summary>
            /// 국가커맨드
            /// </summary>
            nation,
            /// <summary>
            /// 차량정보커맨드
            /// </summary>
            car,
            /// <summary>
            /// 개국정보커맨드
            /// </summary>
            cert,
            /// <summary>
            /// 장비정보커맨드
            /// </summary>
            unit,
            payment,
            close,
            pettyCash
        }

        public Payment payment { get; set; }
        
        private string SetNullString(object pData)
        {
            if (pData == null)
            {
                return string.Empty;
            }
            return pData.ToString();
        }

        private string SetNullZeroString(object pData)
        {
            if (pData == null)
            {
                return "0";
            }
            return pData.ToString();
        }
        
        public object SetDataParsing(NPHttpControl.UrlCmdType pUrlCmdType, string pData)
        {
            try
            {
                if (pData == null || pData == string.Empty)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "RestClassData | SetDataParsing", "[받은전문 데이터가 없어서 처리안함]");
                    return null;
                }
                JObject objPaser = JObject.Parse(pData); // 원본 최초파싱
                switch (pUrlCmdType)
                {
                    case NPHttpControl.UrlCmdType.petty_cashes:
                        Status pettyStatus = new Status();
                        var StatuspettyObject = objPaser[HeaderInfo.status.ToString()];
                        pettyStatus = JsonConvert.DeserializeObject<Status>(StatuspettyObject.ToString());
                        if (pettyStatus.Success == true)
                        {
                            var pettyCashObject = objPaser[HeaderInfo.data.ToString()][HeaderInfo.pettyCash.ToString()].ToString(); // 'macInfo' : [ { 식 배열 파싱
                            PettyCash pettyCashData = JsonConvert.DeserializeObject<PettyCash>(pettyCashObject.ToString());
                            pettyCashData.status = pettyStatus;
                            return pettyCashData;
                        }
                        else
                        {
                            PettyCash pettyCashData = new PettyCash();
                            pettyCashData.status = pettyStatus;
                            return pettyCashData;
                        }
                    case NPHttpControl.UrlCmdType.certs:
                        Status certStatus = new Status();
                        var StatusDataObject = objPaser[HeaderInfo.status.ToString()];
                        certStatus = JsonConvert.DeserializeObject<Status>(StatusDataObject.ToString());

                        Certs sendCert = null;
                        if (certStatus.Success)
                        {
                            var certObject = objPaser[HeaderInfo.data.ToString()][HeaderInfo.cert.ToString()];
                            sendCert = JsonConvert.DeserializeObject<Certs>(certObject.ToString());
                            sendCert.status = certStatus;
                            return sendCert;
                        }
                        else
                        {
                            sendCert = new Certs();
                            sendCert.status = certStatus;
                            return sendCert;
                        }

                    case NPHttpControl.UrlCmdType.status_devices:
                        Status errorStatus = new Status();
                        var errorStatusDataObject = objPaser[HeaderInfo.status.ToString()];
                        errorStatus = JsonConvert.DeserializeObject<Status>(errorStatusDataObject.ToString());
                        return errorStatus;



                    case NPHttpControl.UrlCmdType.cars: // 차량조회
                    case NPHttpControl.UrlCmdType.TimeSearch:
                        var StatusDataCarObject = objPaser[HeaderInfo.status.ToString()];
                        Status carstatus = new Status();
                        carstatus = JsonConvert.DeserializeObject<Status>(StatusDataCarObject.ToString());
                        CarList sendCarList = null;
                        if (carstatus.Success == true)
                        {
                            JArray jArraycarNumDataObject = JArray.Parse(objPaser[HeaderInfo.data.ToString()][HeaderInfo.car.ToString()].ToString()); // 'macInfo' : [ { 식 배열 파싱
                            sendCarList = new CarList();
                            sendCarList.carDataList = jArraycarNumDataObject.ToObject<List<Car>>();
                            sendCarList.status = carstatus;
                            return sendCarList;
                        }
                        else
                        {
                            sendCarList = new CarList();
                            sendCarList.carDataList = new List<Car>();
                            sendCarList.status = carstatus;
                            return sendCarList;
                        }
                    case NPHttpControl.UrlCmdType.onecar:
                        var StatusDataOneCarObject = objPaser[HeaderInfo.status.ToString()];
                        Status onecarstatus = new Status();
                        onecarstatus = JsonConvert.DeserializeObject<Status>(StatusDataOneCarObject.ToString());
                        if (onecarstatus.Success == true)
                        {
                            var onecarObject = objPaser[HeaderInfo.data.ToString()][HeaderInfo.car.ToString()].ToString(); // 'macInfo' : [ { 식 배열 파싱
                            Car sendcar = JsonConvert.DeserializeObject<Car>(onecarObject.ToString());
                            sendcar.status = onecarstatus;
                            return sendcar;
                        }
                        else
                        {
                            Car sendNotcar = new Car();
                            sendNotcar.status = onecarstatus;
                            return sendNotcar;

                        }


                    case NPHttpControl.UrlCmdType.payments:
                    case NPHttpControl.UrlCmdType.payment_details:
                    case NPHttpControl.UrlCmdType.season_car_payment_details:
                    case NPHttpControl.UrlCmdType.discount_tickets:
                        var StatusPaymentObject = objPaser[HeaderInfo.status.ToString()];
                        Status paystatus = new Status();
                        paystatus = JsonConvert.DeserializeObject<Status>(StatusPaymentObject.ToString());
                        Payment sendPayment = null;
                        if (paystatus.Success == true)
                        {
                            var paymentObject = objPaser[HeaderInfo.data.ToString()][HeaderInfo.payment.ToString()]; // cert parsing
                            sendPayment = JsonConvert.DeserializeObject<Payment>(paymentObject.ToString());
                            sendPayment.status = paystatus;
                        }
                        else
                        {
                            sendPayment = new Payment();
                            sendPayment.status = paystatus;
                        }
                        return sendPayment;

                    case NPHttpControl.UrlCmdType.close:
                    case NPHttpControl.UrlCmdType.closeInfo:
                        Status closeStatus = new Status();
                        var StatusCloseObject = objPaser[HeaderInfo.status.ToString()];
                        closeStatus = JsonConvert.DeserializeObject<Status>(StatusCloseObject.ToString());

                        Close sendClose = null;
                        if (closeStatus.Success)
                        {
                            var closeObject = objPaser[HeaderInfo.data.ToString()][HeaderInfo.close.ToString()]; // cert parsing
                            sendClose = JsonConvert.DeserializeObject<Close>(closeObject.ToString());
                            sendClose.status = closeStatus;

                        }
                        else
                        {
                            sendClose = new Close();
                            sendClose.status = closeStatus;
                        }
                        return sendClose;
                    case NPHttpControl.UrlCmdType.car:
                        Car currentCar = new Car();
                        var currentcarObject = objPaser[HeaderInfo.car.ToString()];
                        currentCar = JsonConvert.DeserializeObject<Car>(currentcarObject.ToString());
                        Car sendCar = CommonFuction.Clone<Car>(currentCar);
                        currentCar = null;
                        return sendCar;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "RestClassData | SetDataParsing", ex.ToString());
                return null;
            }

        }

        #endregion 받아오는 데이터
    }
}
