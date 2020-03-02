using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


namespace NPCommon.NICE
{
    //나이스연동

    public class NiceClassData
    {
        /// <summary>
        /// 나이스 헤더클래스
        /// </summary>
        public class HeaderInfo
        {
            public enum HeadInfo
            {
                headerInfo,
                messageSendDt,
                messageSendTm,
                requestSeqNo,
                responseSeqNo,
                messageTyCd,
                serviceTyCd,
                responseCd,
                companyCd,
                parkNo,
                niceMacNo

            }

            public string messageSendDt { set; get; }
            public string messageSendTm { set; get; }
            public string requestSeqNo { set; get; }
            public string responseSeqNo { set; get; }
            public string messageTyCd { set; get; }
            public string serviceTyCd { set; get; }
            public string responseCd { set; get; }
            public string companyCd { set; get; }
            public string parkNo { set; get; }
            public string niceMacNo { set; get; }

        }

        /// <summary>
        /// 나이스 원격제어중 주차요금제어 
        /// </summary>
        public class JunsanControl
        {
            public enum Type
            {
                /// <summary>
                /// 요금제어종류 1이면 신규 2이면 변경
                /// </summary>
                chargeCtrlKnd,
                /// <summary>
                /// 차량번호
                /// </summary>
                carNo,
                /// <summary>
                /// 주차권번호
                /// </summary>
                parkTkNo,
                /// <summary>
                /// 입차일시
                /// </summary>
                inCarDt,
                /// <summary>
                /// 입차시간
                /// </summary>
                inCarTm,
                /// <summary>
                /// 주차시간
                /// </summary>
                parkTm,
                /// <summary>
                /// 원주차요금
                /// </summary>
                originalParkChrg,
                /// <summary>
                /// 할인요금
                /// </summary>
                discountChrg,
                /// <summary>
                /// 주차요금
                /// </summary>
                parkChrg



            }
            private string mchargeCtrlKnd = string.Empty;
            /// <summary>
            /// 요금제어종류 1이면 신규 2이면 변경
            /// </summary>
            public string chargeCtrlKnd
            {
                set { mchargeCtrlKnd = Convert.ToInt32(value).ToString().PadLeft(1, '0').ToString(); }
                get { return mchargeCtrlKnd; }
            }

            private string mcarNo = string.Empty;
            /// <summary>
            /// 차량번호
            /// </summary>
            public string carNo
            {
                set { mcarNo = value; }
                get { return mcarNo; }
            }


            private string mparkTkNo = string.Empty;
            /// <summary>
            /// 주차권번호
            /// </summary>
            public string parkTkNo
            {
                set { mparkTkNo = value; }
                get { return mparkTkNo; }
            }

            private string minCarDt = string.Empty;
            /// <summary>
            /// 입차일자 yyyymmdd
            /// </summary>
            public string inCarDt
            {
                set { minCarDt = value; }
                get { return minCarDt; }
            }


            private string minCarTm = string.Empty;
            /// <summary>
            /// 입차시간 hhmmss
            /// </summary>
            public string inCarTm
            {
                set { minCarTm = value; }
                get { return minCarTm; }

            }

            private string mparkTm = string.Empty;
            /// <summary>
            /// /주차시간 
            /// </summary>
            public string parkTm
            {
                set { mparkTm = Convert.ToInt32(value).ToString().PadLeft(5, '0').ToString(); }
                get { return mparkTm; }

            }

            private string moriginalParkChrg = string.Empty;
            /// <summary>
            /// 원주차요금
            /// </summary>
            public string originalParkChrg
            {
                set { moriginalParkChrg = Convert.ToInt32(value).ToString().PadLeft(7, '0').ToString(); }
                get { return moriginalParkChrg; }
            }


            private string mdiscountChrg = string.Empty;
            /// <summary>
            /// 할인요금
            /// </summary>
            public string discountChrg
            {
                set { mdiscountChrg = Convert.ToInt32(value).ToString().PadLeft(7, '0').ToString(); }
                get { return mdiscountChrg; }
            }

            /// <summary>
            /// 받을요금
            /// </summary>
            private string mparkChrg = string.Empty;
            public string parkChrg
            {
                set { mparkChrg = Convert.ToInt32(value).ToString().PadLeft(12, '0').ToString(); }
                get { return mparkChrg; }
            }
        }

        /// <summary>
        /// 나이스 정산기 요금조회 응답전문        /// 
        /// </summary>
        public class JunanSearch
        {
            public enum Type
            {
                /// <summary>
                /// 정산기표시금액
                /// </summary>
                adjMacMrkAmt,
                /// <summary>
                /// 원주차요금
                /// </summary>
                originalParkChrg,
                /// <summary>
                /// 할인금액
                /// </summary>
                discountAmt,
                /// <summary>
                /// 입금액
                /// </summary>
                inAmt
            }
            private string madjMacMrkAmt = "000000000000";
            /// <summary>
            /// 정산기표시금액
            /// </summary>
            public string adjMacMrkAmt
            {
                set { madjMacMrkAmt = Convert.ToInt32(value).ToString().PadLeft(12, '0').ToString(); }
                get { return madjMacMrkAmt; }
            }
            private string moriginalParkChrg = "0000000";
            /// <summary>
            /// 원주차요금
            /// </summary>
            public string originalParkChrg
            {
                set { moriginalParkChrg = Convert.ToInt32(value).ToString().PadLeft(7, '0').ToString(); }
                get { return moriginalParkChrg; }
            }
            private string mdiscountAmt = "000000000000";
            /// <summary>
            /// 할인금액
            /// </summary>
            public string discountAmt
            {
                set { mdiscountAmt = Convert.ToInt32(value).ToString().PadLeft(12, '0').ToString(); }
                get { return mdiscountAmt; }
            }
            private string minAmt = "0000000";
            /// <summary>
            /// 입금액
            /// </summary>
            public string inAmt
            {
                set { minAmt = Convert.ToInt32(value).ToString().PadLeft(7, '0').ToString(); }
                get { return minAmt; }
            }
        }


        /// <summary>
        /// 나이스 영수증클래스
        /// </summary>
        public class ReceiptPrint
        {
            public enum Type
            {
                /// <summary>
                /// 차량번호
                /// </summary>
                carNo,
                /// <summary>
                /// 입차일련번호
                /// </summary>
                inCarSeqNo,
                /// <summary>
                /// 입차일자 yyyymmdd
                /// </summary>
                inCarDt,
                /// <summary>
                /// 입차시간 hhmmss
                /// </summary>
                inCarTm

            }
            private string mcarNo = string.Empty;
            /// <summary>
            /// 차량번호
            /// </summary>
            public string carNo
            {
                set { mcarNo = value; }
                get { return mcarNo; }
            }

            private string minCarSeqNo = string.Empty;
            /// <summary>
            /// 입차일련번호
            /// </summary>
            public string inCarSeqNo
            {
                set { minCarSeqNo = value; }
                get { return minCarSeqNo; }
            }

            private string minCarDt = string.Empty;
            /// <summary>
            /// 입차일자 yyyymmdd
            /// </summary>
            public string inCarDt
            {
                set { minCarDt = value; }
                get { return minCarDt; }
            }


            private string minCarTm = string.Empty;
            /// <summary>
            /// 입차시간 hhmmss
            /// </summary>
            public string inCarTm
            {
                set { minCarTm = value; }
                get { return minCarTm; }
            }


        }
        /// <summary>
        /// 나이스 차단기관련
        /// </summary>
        public class BarControl
        {
            public enum Type
            {
                gatebarCtrlKnd
            }
            private string mgatebarCtrlKnd = string.Empty;
            public string gatebarCtrlKnd
            {
                set { mgatebarCtrlKnd = value; }
                get { return mgatebarCtrlKnd; }
            }
        }
        /// <summary>
        /// 나이스 할인제어클래스
        /// </summary>
        public class DiscountSet
        {
            public enum Type
            {
                /// <summary>
                /// 할인권정보 어레이 정보
                /// </summary>
                discountTkInfo,
                /// <summary>
                /// 할인방식
                /// </summary>
                discountMtd,
                /// <summary>
                /// 할인권종류
                /// </summary>
                discountTkKnd,
                /// <summary>
                /// 할인권사용매수
                /// </summary>
                discountTkUseCnt,
                /// <summary>
                /// 오류메세지
                /// </summary>
                errorMsg,
                /// <summary>
                /// 내부용할인코드
                /// </summary>
                discountDCNo
            }
            /// <summary>
            /// 할인권정보 어레이 정보
            /// </summary>
            public List<discountTkInfo> mListDiscountData = new List<discountTkInfo>();
            public class discountTkInfo
            {
                private string mdiscountMtd = string.Empty;
                /// <summary>
                /// 할인방식
                /// </summary>
                public string discountMtd
                {
                    set { mdiscountMtd = value; }
                    get { return mdiscountMtd; }
                }

                private string mdiscountTkKnd = string.Empty;
                /// <summary>
                /// 할인권종류
                /// </summary>
                public string discountTkKnd
                {
                    set { mdiscountTkKnd = value; }
                    get { return mdiscountTkKnd; }
                }

                private string mdiscountTkUseCnt = string.Empty;
                /// <summary>
                ///  할인권사용매수
                /// </summary>
                public string discountTkUseCnt
                {
                    set { mdiscountTkUseCnt = value; }
                    get { return mdiscountTkUseCnt; }
                }

                private string merrorMsg = string.Empty;
                /// <summary>
                /// 에러메세지
                /// </summary>
                public string errorMsg
                {
                    set { merrorMsg = value; }
                    get { return merrorMsg; }
                }


                private string mdiscountDCNo = string.Empty;
                /// <summary>
                /// 내부용 할인코드
                /// </summary>
                public string discountDCNo
                {
                    set { mdiscountDCNo = value; }
                    get { return mdiscountDCNo; }
                }
            }

        }
        /// <summary>
        /// 나이스 장비초기화
        /// </summary>
        public class DeviceReset
        {
            public enum Type
            {
                /// <summary>
                /// Reset제어종류
                /// </summary>
                resetCtrlKnd,
                /// <summary>
                /// 모듈정보
                /// </summary>
                moduleInfo,
                /// <summary>
                /// 모듈코드
                /// </summary>
                moduleCd,
                /// <summary>
                /// 모듈상태
                /// </summary>
                moduleSts,
            }

            private string mresetCtrlKnd = string.Empty;
            public string resetCtrlKnd
            {
                set { mresetCtrlKnd = value; }
                get { return mresetCtrlKnd; }
            }

            public List<moduleInfo> mListmoduleInfo = new List<moduleInfo>();
            public class moduleInfo
            {
                private string mmoduleCd = string.Empty;
                public string moduleCd
                {
                    set { mmoduleCd = value; }
                    get { return mmoduleCd; }
                }

                private string mmoduleSts = string.Empty;
                public string moduleSts
                {
                    set { mmoduleSts = value; }
                    get { return mmoduleSts; }
                }

            }

        }

        public class VersionInfo
        {
            public enum Type
            {
                programTy,
                programNm,
                programVer

            }
            #region 각각의 클래스들

            public string programTy { set; get; }
            public string programNm { set; get; }
            public string programVer { set; get; }

        }

        public class RemoteExcute
        {
            public enum Type
            {
                /// <summary>
                /// 실행파일정보 1: WAV, 2: JPG, 3: MP4
                /// </summary>
                executeFileTy,
                /// <summary>
                /// 실행파일ID
                /// </summary>
                executeFileId,
                /// <summary>
                /// 오류메세지
                /// </summary>
                errorMsg,
            }
            public string executeFileTy { set; get; }
            public string executeFileId { set; get; }
            private string merrorMsg = string.Empty;
            public string errorMsg
            {
                set { merrorMsg = value; }
                get { return merrorMsg; }
            }
        }


    }

    public class NiceSetData
    {
        public enum Cmd
        {
            /// <summary>
            /// 요금조회
            /// </summary>
            SearchPayment,
            /// <summary>
            /// 신규입차 및 표출
            /// </summary>
            RemoteCarNew,
            /// <summary>
            /// 강제요금부과 이후 할인처리안됨
            /// </summary>
            RemoteCarConvert,
            /// <summary>
            /// 할인
            /// </summary>
            Discount,
            /// <summary>
            /// 차단기
            /// </summary>
            Bar,
            /// <summary>
            /// 영수증
            /// </summary>
            Receipt,
            /// <summary>
            /// 리셋
            /// </summary>
            Reset,
            /// <summary>
            /// 버젼요청
            /// </summary>
            VersionInfo,
            /// <summary>
            /// 다시 요금정보세팅요청
            /// </summary>
            ReSetting,
            /// <summary>
            /// 없음
            /// </summary>
            None,
            /// <summary>
            /// 원격파일실행
            /// </summary>
            RemoteExute
        }

        /// <summary>
        /// 응답시 성공코드 headinfo에 들어감
        /// </summary>
        public const string SuccessResponeCode = "00";
        public const string FailResponeCode = "99";
        public const string NotSerachPay = "22";
        public Cmd CurrentCmd = Cmd.None;

        private NiceClassData.HeaderInfo mHeaderInfo = null;
        public NiceClassData.HeaderInfo HeaderInfo
        {
            set { mHeaderInfo = value; }
            get { return mHeaderInfo; }
        }
        private NiceClassData.ReceiptPrint mReceiptPrint = null;
        public NiceClassData.ReceiptPrint ReceiptPrint
        {
            set { mReceiptPrint = value; }
            get { return mReceiptPrint; }
        }

        private NiceClassData.JunanSearch mJunanSearch = null;
        public NiceClassData.JunanSearch JunanSearch
        {
            set { mJunanSearch = value; }
            get { return mJunanSearch; }
        }

        private NiceClassData.JunsanControl mJunsanControl = null;
        public NiceClassData.JunsanControl JunsanControl
        {
            set { mJunsanControl = value; }
            get { return mJunsanControl; }
        }


        private NiceClassData.BarControl mBarControl = null;
        public NiceClassData.BarControl BarControl
        {
            set { mBarControl = value; }
            get { return mBarControl; }
        }


        private NiceClassData.DiscountSet mDiscountSet = null;
        public NiceClassData.DiscountSet DiscountSet
        {
            set { mDiscountSet = value; }
            get { return mDiscountSet; }
        }


        private NiceClassData.DeviceReset mDeviceReset = null;
        public NiceClassData.DeviceReset DeviceReset
        {
            set { mDeviceReset = value; }
            get { return mDeviceReset; }
        }

        private NiceClassData.VersionInfo mVersionInfo = null;
        public NiceClassData.VersionInfo VersionInfo
        {
            set { mVersionInfo = value; }
            get { return mVersionInfo; }
        }

        private NiceClassData.RemoteExcute mRemoteExcute = null;
        public NiceClassData.RemoteExcute RemoteExcute
        {
            set { mRemoteExcute = value; }
            get { return mRemoteExcute; }

        }




        public void SetDataParsing(string pData)
        {
            mHeaderInfo = null;
            mReceiptPrint = null;
            mJunanSearch = null;
            mJunsanControl = null;
            mBarControl = null;
            mDiscountSet = null;
            mDeviceReset = null;
            mRemoteExcute = null;
            CurrentCmd = Cmd.None; // 최초 명령어 초기화
            JObject objPaser = JObject.Parse(pData); // 원본 최초파싱
            var headerInfoString = objPaser[NiceClassData.HeaderInfo.HeadInfo.headerInfo.ToString()];
            mHeaderInfo = JsonConvert.DeserializeObject<NiceClassData.HeaderInfo>(headerInfoString.ToString()); // Header 정보 클래스로 변환
            if (mHeaderInfo.messageTyCd.Equals("0400"))   // 원격요금조회
            {
                if (mHeaderInfo.serviceTyCd.Equals("0300"))
                {
                    CurrentCmd = Cmd.SearchPayment;
                    mHeaderInfo.messageTyCd = "0410"; //응답값만듬
                    mHeaderInfo.serviceTyCd = "0300"; // 응답값만듬
                    mJunanSearch = new NiceClassData.JunanSearch();
                    mJunanSearch.adjMacMrkAmt = SetNullZeroString(objPaser[NiceClassData.JunanSearch.Type.adjMacMrkAmt.ToString()]);
                    mJunanSearch.discountAmt = SetNullZeroString(objPaser[NiceClassData.JunanSearch.Type.discountAmt.ToString()]);
                    mJunanSearch.inAmt = SetNullZeroString(objPaser[NiceClassData.JunanSearch.Type.inAmt.ToString()]);
                    mJunanSearch.originalParkChrg = SetNullZeroString(objPaser[NiceClassData.JunanSearch.Type.originalParkChrg.ToString()]);
                }
            }
            else if (mHeaderInfo.messageTyCd.Equals("0600")) // 주차요금제어
            {
                mHeaderInfo.messageTyCd = "0610";
                switch (mHeaderInfo.serviceTyCd)
                {
                    case "0100": // 주차요금제어
                        mJunsanControl = new NiceClassData.JunsanControl();
                        mJunsanControl.carNo = SetNullString(objPaser[NiceClassData.JunsanControl.Type.carNo.ToString()]);
                        mJunsanControl.chargeCtrlKnd = SetNullString(objPaser[NiceClassData.JunsanControl.Type.chargeCtrlKnd.ToString()]);
                        if (mJunsanControl.chargeCtrlKnd == "1")
                        {
                            CurrentCmd = Cmd.RemoteCarNew;
                        }
                        else if (mJunsanControl.chargeCtrlKnd == "2" || mJunsanControl.chargeCtrlKnd == "3")
                        {
                            CurrentCmd = Cmd.RemoteCarConvert;
                        }

                        mJunsanControl.discountChrg = SetNullZeroString(objPaser[NiceClassData.JunsanControl.Type.discountChrg.ToString()]);
                        mJunsanControl.inCarDt = SetNullString(objPaser[NiceClassData.JunsanControl.Type.inCarDt.ToString()]);
                        mJunsanControl.inCarTm = SetNullString(objPaser[NiceClassData.JunsanControl.Type.inCarTm.ToString()]);
                        mJunsanControl.parkTm = SetNullZeroString(objPaser[NiceClassData.JunsanControl.Type.parkTm.ToString()]);
                        mJunsanControl.originalParkChrg = SetNullZeroString(objPaser[NiceClassData.JunsanControl.Type.originalParkChrg.ToString()]);
                        mJunsanControl.parkChrg = SetNullZeroString(objPaser[NiceClassData.JunsanControl.Type.parkChrg.ToString()]);
                        mJunsanControl.parkTkNo = SetNullString(objPaser[NiceClassData.JunsanControl.Type.parkTkNo.ToString()]); // 주차일련번호를 생성하는지...모르겠다
                        break;
                    case "0200"://영수증발행
                        CurrentCmd = Cmd.Receipt;
                        mReceiptPrint = new NiceClassData.ReceiptPrint();
                        mReceiptPrint.carNo = SetNullString(objPaser[NiceClassData.ReceiptPrint.Type.carNo.ToString()]);
                        mReceiptPrint.inCarDt = SetNullString(objPaser[NiceClassData.ReceiptPrint.Type.inCarDt.ToString()]);
                        mReceiptPrint.inCarSeqNo = SetNullString(objPaser[NiceClassData.ReceiptPrint.Type.inCarSeqNo.ToString()]);
                        mReceiptPrint.inCarTm = SetNullString(objPaser[NiceClassData.ReceiptPrint.Type.inCarTm.ToString()]);
                        break;
                    case "0300"://차단기제어
                        CurrentCmd = Cmd.Bar;
                        mBarControl = new NiceClassData.BarControl();
                        mBarControl.gatebarCtrlKnd = SetNullString(objPaser[NiceClassData.BarControl.Type.gatebarCtrlKnd.ToString()]);
                        break;
                    case "0400":
                        CurrentCmd = Cmd.Reset;
                        mDeviceReset = new NiceClassData.DeviceReset();
                        string aa = SetNullString(objPaser["resetCtrlKnd"]);
                        mDeviceReset.resetCtrlKnd = SetNullString(objPaser[NiceClassData.DeviceReset.Type.resetCtrlKnd.ToString()]);
                        //JArray objModuleInfo = JArray.Parse(objPaser["moduleInfo"].ToString());
                        if (mDeviceReset.resetCtrlKnd == "2")
                        {

                            JArray objModuleInfo = JArray.Parse(objPaser[NiceClassData.DeviceReset.Type.moduleInfo.ToString()].ToString()); // 'macInfo' : [ { 식 배열 파싱
                            foreach (JObject item in objModuleInfo)
                            {
                                NiceClassData.DeviceReset.moduleInfo moduleInfo = new NiceClassData.DeviceReset.moduleInfo();
                                moduleInfo.moduleCd = SetNullString(item[NiceClassData.DeviceReset.Type.moduleCd.ToString()]);
                                moduleInfo.moduleSts = SetNullString(item[NiceClassData.DeviceReset.Type.moduleSts.ToString()]);
                                mDeviceReset.mListmoduleInfo.Add(moduleInfo);

                            }
                        }


                        break;
                    case "0500":
                        CurrentCmd = Cmd.RemoteExute;
                        mRemoteExcute = new NiceClassData.RemoteExcute();
                        mRemoteExcute.executeFileTy = SetNullString(objPaser[NiceClassData.RemoteExcute.Type.executeFileTy.ToString()]);
                        mRemoteExcute.executeFileId = SetNullString(objPaser[NiceClassData.RemoteExcute.Type.executeFileId.ToString()]);

                        break;
                    case "0600": // 할인권적용
                        CurrentCmd = Cmd.Discount;
                        JArray objDiscountTkInfo = JArray.Parse(objPaser[NiceClassData.DiscountSet.Type.discountTkInfo.ToString()].ToString()); // 'macInfo' : [ { 식 배열 파싱
                        mDiscountSet = new NiceClassData.DiscountSet();
                        foreach (JObject item in objDiscountTkInfo)
                        {
                            NiceClassData.DiscountSet.discountTkInfo discountData = new NiceClassData.DiscountSet.discountTkInfo();
                            discountData.discountDCNo = SetNullString(item[NiceClassData.DiscountSet.Type.discountDCNo.ToString()]);
                            discountData.discountMtd = SetNullString(item[NiceClassData.DiscountSet.Type.discountMtd.ToString()]);
                            discountData.discountTkKnd = SetNullString(item[NiceClassData.DiscountSet.Type.discountTkKnd.ToString()]);
                            discountData.discountTkUseCnt = SetNullString(item[NiceClassData.DiscountSet.Type.discountTkUseCnt.ToString()]);
                            discountData.errorMsg = SetNullString(item[NiceClassData.DiscountSet.Type.errorMsg.ToString()]);
                            mDiscountSet.mListDiscountData.Add(discountData);

                        }
                        break;
                }
            }
            else if (mHeaderInfo.messageTyCd.Equals("9900"))
            {
                mHeaderInfo.messageTyCd = "9910";
                if (mHeaderInfo.serviceTyCd == "0100") // 설정변경요청
                {
                    CurrentCmd = Cmd.ReSetting;
                }
                if (mHeaderInfo.serviceTyCd == "0300") // 버젼요청
                {
                    CurrentCmd = Cmd.VersionInfo;
                    mVersionInfo = new NiceClassData.VersionInfo();
                    mVersionInfo.programNm = SetNullString(objPaser[NiceClassData.VersionInfo.Type.programNm.ToString()]);
                    mVersionInfo.programTy = SetNullString(objPaser[NiceClassData.VersionInfo.Type.programTy.ToString()]);
                    mVersionInfo.programVer = SetNullString(objPaser[NiceClassData.VersionInfo.Type.programVer.ToString()]);

                }
            }
        }

        public string MakeSendData()
        {
            string sendMessage = string.Empty;
            switch (CurrentCmd)
            {
                case Cmd.Bar: // 차단기
                    var jsonBar = JObject.FromObject(mBarControl);
                    jsonBar.Add(NiceClassData.HeaderInfo.HeadInfo.headerInfo.ToString(), JObject.FromObject(HeaderInfo));
                    sendMessage = jsonBar.ToString();
                    break;
                case Cmd.Receipt: //영수증
                    var jsonReceipt = JObject.FromObject(mReceiptPrint);
                    jsonReceipt.Add(NiceClassData.HeaderInfo.HeadInfo.headerInfo.ToString(), JObject.FromObject(HeaderInfo));
                    sendMessage = jsonReceipt.ToString();
                    break;
                case Cmd.RemoteCarConvert: // 원격제어에 요금제어
                case Cmd.RemoteCarNew:
                    var joinRemete = JObject.FromObject(mJunsanControl);
                    joinRemete.Add(NiceClassData.HeaderInfo.HeadInfo.headerInfo.ToString(), JObject.FromObject(HeaderInfo));
                    sendMessage = joinRemete.ToString();
                    break;
                case Cmd.SearchPayment: // 원격조회에 정산기요금조회
                    var joinSearchPayment = JObject.FromObject(mJunanSearch);
                    joinSearchPayment.Add(NiceClassData.HeaderInfo.HeadInfo.headerInfo.ToString(), JObject.FromObject(HeaderInfo));
                    sendMessage = joinSearchPayment.ToString();
                    break;
                case Cmd.Discount:
                    JObject discountObject = new JObject();
                    discountObject.Add(NiceClassData.HeaderInfo.HeadInfo.headerInfo.ToString(), JObject.FromObject(HeaderInfo));
                    if (mDiscountSet.mListDiscountData.Count > 0)
                    {
                        JArray jarray = new JArray();
                        foreach (NiceClassData.DiscountSet.discountTkInfo item in mDiscountSet.mListDiscountData)
                        {
                            jarray.Add(JObject.FromObject(item));
                        }
                        discountObject.Add(NiceClassData.DiscountSet.Type.discountTkInfo.ToString(), jarray);
                    }
                    sendMessage = discountObject.ToString();
                    break;
                case Cmd.Reset:
                    JObject resetObject = new JObject();
                    resetObject.Add(NiceClassData.DeviceReset.Type.resetCtrlKnd.ToString(), mDeviceReset.resetCtrlKnd);
                    resetObject.Add(NiceClassData.HeaderInfo.HeadInfo.headerInfo.ToString(), JObject.FromObject(HeaderInfo));

                    if (mDeviceReset.mListmoduleInfo.Count > 0)
                    {
                        JArray jarray = new JArray();
                        foreach (NiceClassData.DeviceReset.moduleInfo item in mDeviceReset.mListmoduleInfo)
                        {
                            jarray.Add(JObject.FromObject(item));
                        }
                        resetObject.Add(NiceClassData.DeviceReset.Type.moduleInfo.ToString(), jarray);
                    }
                    sendMessage = resetObject.ToString();
                    break;
                case Cmd.ReSetting:
                    JObject reserttingData = new JObject();
                    reserttingData.Add(NiceClassData.HeaderInfo.HeadInfo.headerInfo.ToString(), JObject.FromObject(HeaderInfo));
                    sendMessage = reserttingData.ToString();
                    break;
                case Cmd.VersionInfo:
                    var versionData = JObject.FromObject(mVersionInfo);
                    versionData.Add(NiceClassData.HeaderInfo.HeadInfo.headerInfo.ToString(), JObject.FromObject(HeaderInfo));
                    sendMessage = versionData.ToString();

                    break;
                case Cmd.RemoteExute:
                    var jsonRemoteExute = JObject.FromObject(mRemoteExcute);
                    jsonRemoteExute.Add(NiceClassData.HeaderInfo.HeadInfo.headerInfo.ToString(), JObject.FromObject(HeaderInfo));
                    sendMessage = jsonRemoteExute.ToString();
                    break;

            }
            sendMessage = sendMessage.Replace("\r", "");
            sendMessage = sendMessage.Replace("\n", "");
            sendMessage = sendMessage + "\r" + "\n";
            return sendMessage;
        }



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

    }


    #endregion 
}
