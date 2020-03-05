using FadeFox.Text;
using NPCommon.IO;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NPCommon.Van
{
    /// <summary>
    /// Smartro_TL3500S 의 Partial Class File
    /// </summary>
    /// <remarks>
    /// 구조체를 정의한다.
    /// </remarks>
    public partial class Smartro_TL3500S
    {
        #region 설정 정보 셋팅 전문에 필요한 변수들...

        /// <summary>
        /// 이더넷 통신 IP
        /// </summary>
        private string mEthernetIP = string.Empty;
        /// <summary>
        /// 이더넷 통신 PORT
        /// </summary>
        private string mEthernetPort = string.Empty;
        /// <summary>
        /// 가상ID
        /// </summary>
        private string mMID = string.Empty;
        /// <summary>
        /// VAN 서버 IP
        /// </summary>
        private string mVanIP = "211.192.50.244";
        /// <summary>
        /// VAN 서버 PORT
        /// </summary>
        private string mVanPort = "5500";
        /// <summary>
        /// 단말기 ID
        /// </summary>
        private string mTID = string.Empty;
        /// <summary>
        /// 단말기 IP 방식 "0"[DHCP], "1"[STATIC]
        /// </summary>
        private string mDeviceIPType = "0";
        /// <summary>
        /// 단말기 IP
        /// </summary>
        private string mDeviceIP = string.Empty;
        /// <summary>
        /// 단말기 SUBNET
        /// </summary>
        private string mDeviceSubNet = string.Empty;
        /// <summary>
        /// 단말기 GATEWAY
        /// </summary>
        private string mDeviceGateWay = string.Empty;
        /// <summary>
        /// 장비 통신포트 "0"[COM], "1"[USB], "2"[이더넷]
        /// </summary>
        private string mDeviceType = "0";

        #endregion

    }

    #region DTO

    public class SmartroDTO
    {
        private Header headerData;
        private ISerializable bodyData;
        private Tail tailData;

        public ISerializable HeaderData { get => headerData; }
        public ISerializable BodyData { get => bodyData; }
        public ISerializable TailData { get => tailData; }

        public bool IsNull
        {
            get
            {
                if (this.headerData == null || this.tailData == null) return true;
                else return false;
            }
        }

        private SmartroDTO()
        {

        }

        public static SmartroDTO GetNewDTO(ISerializable pBody)
        {
            SmartroDTO smartroDTO = new SmartroDTO();

            try
            {
                smartroDTO.headerData = new Header();
                smartroDTO.tailData = new Tail();

                if (pBody != null)
                {
                    smartroDTO.bodyData = pBody;
                }
                else
                {
                    smartroDTO.bodyData = null;
                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "Smartro_TL3500S || GetNewDTO ", ex.Message);
                return null;
            }

            return smartroDTO;
        }

        public static SmartroDTO Parse(byte[] data)
        {
            SmartroDTO dto = new SmartroDTO();
            //Header 정보 가져오기
            dto.headerData = (Header)dto.ParseToHeader(data);
            //Tail 정보 가져오기
            dto.tailData = (Tail)dto.ParseToTail(data);
            //Body 정보 가져오기
            Type t;
            switch (dto.headerData.JobCode)
            {
                case "a": //장치체크 응답전문
                    t = typeof(ReceiveDeviceCheck);
                    break;
                case "b": //거래승인 응답전문
                case "c": //거래취소 응답전문
                    t = typeof(ReceiveApproval);
                    break;
                case "d": //카드조회 응답전문
                    t = typeof(ReceiveCardSearch);
                    break;
                case "e": //결제대기 응답전문
                    t = null;
                    break;
                case "f": //카드 UID 읽기 응답전문 응답전문
                    t = typeof(ReceiveCradUIDRead);
                    break;
                case "@": //이벤트 응답전문
                    t = typeof(ReceiveEvent);
                    break;
                case "g": //부가정보 추가 거래승인 응답전문
                    t = null;
                    break;
                case "i": //설정 정보 세팅 응답전문
                    t = null;
                    break;
                case "j": //설정 정보 응답전문
                    t = null;
                    break;
                case "k": //설정 정보 메모리 WRITING 응답전문
                    t = null;
                    break;
                case "l": //마지막 승인 응답 응답전문
                    t = null;
                    break;
                case "v": //버전 체크 응답전문
                    t = typeof(ReceiveVersionCheck);
                    break;
                case "s": //화면&음성 설정 응답전문
                    t = typeof(SendReceiveScreenNSoundSetting);
                    break;
                default:
                    t = null;
                    break;
            }

            if (t != null)
            {
                dto.bodyData = dto.ParseToBody(data, t);
            }

            return dto;
        }

        public ISerializable ParseToHeader(byte[] data)
        {
            try
            {
                //Header 는 35byte
                var arr = data.Where((value, idx) => idx < 35).ToArray();
                var header = arr.AsSerializable(typeof(Header));
                return header;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "Smartro_TL3500S || ParseToHeader ", ex.Message);
                return null;
            }
        }

        public ISerializable ParseToBody(byte[] data, Type t)
        {
            try
            {
                //Body는 앞 36byte, 뒤 2byte 를 제외한 나머지
                var arr = data.Where((value, idx) => idx > 34 && idx < data.Length - 2).ToArray();
                var obj = arr.AsSerializable(t);
                return obj;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "Smartro_TL3500S || ParseToBody ", ex.Message);
                return null;
            }
        }

        public ISerializable ParseToTail(byte[] data)
        {
            try
            {
                //Tail 은 2byte
                var arr = data.Where((value, idx) => idx >= data.Length - 2).ToArray();
                var tail = arr.AsSerializable(typeof(Tail));
                return tail;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "Smartro_TL3500S || ParseToTail ", ex.Message);
                return null;
            }
        }
    }

    #endregion

    #region 헤더

    /// <summary>
    /// 헤더 전문
    /// </summary>
    public class Header : ISerializable
    {
        private const byte NullToken = 0x00;

        /// <summary>
        /// Header 의 Byte 길이
        /// </summary>
        private const ushort length = 35;

        /// <summary>
        /// 통신프레임의 시작부호[0x02]
        /// </summary>
        public byte STX;
        /// <summary>
        /// 단말기 ID, 좌측정렬, 나머지 우측 0x00 채움 
        /// </summary>
        public string TerminalID;
        /// <summary>
        /// 전송일시, YYYYMMDDhhmmss
        /// </summary>
        public string DateTime;
        /// <summary>
        /// 업무 코드
        /// </summary>
        public string JobCode;
        /// <summary>
        /// 응답 코드, 요청 시 0x00
        /// </summary>
        public byte ResponseCode = 0x00;
        /// <summary>
        /// Body 데이터의 길이
        /// </summary>
        public ushort DataLength;

        public ushort Size => length;


        public void Serialize(BinaryWriter writer)
        {
            writer.Write(STX);
            writer.Write(TerminalID.PadRight(16, Convert.ToChar(NullToken)).GetBytes());
            writer.Write(DateTime.GetBytes());
            writer.Write(JobCode.GetBytes());
            writer.Write(ResponseCode);
            writer.Write(DataLength);
        }

        public void Deserialize(BinaryReader reader)
        {
            STX = reader.ReadByte();
            TerminalID = reader.ReadFixedString(16).TrimEnd(Convert.ToChar(NullToken));
            DateTime = reader.ReadFixedString(14);
            JobCode = reader.ReadFixedString(1);
            ResponseCode = reader.ReadByte();
            DataLength = reader.ReadUInt16();
        }

    }

    #endregion

    #region 테일

    /// <summary>
    /// 테일 전문
    /// </summary>
    public class Tail : ISerializable
    {
        private const ushort length = 2;

        /// <summary>
        /// 통신프레임의 시작부호[0x03]
        /// </summary>
        public byte ETX = 0x03;
        /// <summary>
        /// STX ~ ETX까지의 XOR값
        /// </summary>
        public byte BCC;

        public ushort Size => length;

        public void Deserialize(BinaryReader reader)
        {
            ETX = reader.ReadByte();
            BCC = reader.ReadByte();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(ETX);
            writer.Write(BCC);
        }

        /// <summary>
        /// STX ~ ETX까지의 XOR 값
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        public byte MakeBCC(byte[] vs)
        {
            var xor = vs.Aggregate((acc, value) => (byte)(acc ^ value));
            xor ^= ETX;
            return xor;
        }
    }

    #endregion

    #region 장치체크

    /// <summary>
    /// 장치체크 응답 전문
    /// </summary>
    public class ReceiveDeviceCheck : ISerializable
    {
        private const ushort length = 4;

        public string CardConnectStat;      //[01 Byte] 카드모듈 통신상태 "N"[미설치], "O"[정상], "X"[오류]
        public string RFConnectStat;        //[01 Byte] RF모듈 상태 "O"[정상], "X"[오류]
        public string VANConnectStat;       //[01 Byte] VAN서버 연결상태  "N"[미설치], "O"[정상], "X"[연결 디바이스 오류], "F"[서버연결 실패]
        public string ServerConnectStat;    //[01 Byte] 연동서버 연결상태  "N"[미설치], "O"[정상], "X"[연결 디바이스 오류], "F"[서버연결 실패]

        public ushort Size => length;

        public void Deserialize(BinaryReader reader)
        {
            CardConnectStat = reader.ReadFixedString(1);
            RFConnectStat = reader.ReadFixedString(1);
            VANConnectStat = reader.ReadFixedString(1);
            ServerConnectStat = reader.ReadFixedString(1);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(CardConnectStat.GetBytes());
            writer.Write(RFConnectStat.GetBytes());
            writer.Write(VANConnectStat.GetBytes());
            writer.Write(ServerConnectStat.GetBytes());
        }
    }

    #endregion

    #region 거래승인

    /// <summary>
    /// 거래승인 요청 전문
    /// </summary>
    public class SendApproval : ISerializable
    {
        private const ushort length = 30;

        public string TradCode;     //[01 Byte] 거래구분코드 "1"[승인], "2"[마지막 거래 취소 후 승인]
        public string PayMoney;     //[10 Byte] 승인요청금액(원거래+세금+봉사료)
        public string Vat;          //[08 Byte] 부가세 우측 정렬, 좌측 "0" 채움
        public string Service;      //[08 Byte] 봉사료 우측 정렬, 좌측 "0" 채움
        public string InstMonth;    //[02 Byte] 할부개월 우측 정렬, 좌측 "0" 채움
        public string SignYN;       //[01 Byte] 서명 여부 "1"[비서명], "2"[서명]

        public ushort Size => length;

        public void Deserialize(BinaryReader reader)
        {
            TradCode = reader.ReadFixedString(1);
            PayMoney = reader.ReadFixedString(10).TrimStart('0');
            Vat = reader.ReadFixedString(8).TrimStart('0');
            Service = reader.ReadFixedString(8).TrimStart('0');
            InstMonth = reader.ReadFixedString(2).TrimStart('0');
            SignYN = reader.ReadFixedString(1);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(TradCode.GetBytes());
            writer.Write(PayMoney.PadLeft(10, '0').GetBytes());
            writer.Write(Vat.PadLeft(8, '0').GetBytes());
            writer.Write(Service.PadLeft(8, '0').GetBytes());
            writer.Write(InstMonth.PadLeft(2, '0').GetBytes());
            writer.Write(SignYN.GetBytes());
        }
    }

    /// <summary>
    /// 거래승인/취소 응답 전문
    /// </summary>
    public class ReceiveApproval : ISerializable
    {
        private const ushort length = 157;

        public string TradCode;     //[01 Byte] 거래구분코드 "1"[승인], "3"[선불카드], "X"[거래거절]:거래매체~단말기번호 Space 채움, 취소시 "2"인 경우 태그한 카드가 이전 거래 내역이 없거나 이미 취소한 카드인 경우 거래취소 요청전문에 대한 응답전문은 카드조회 응답전문이 전송
        public string TradType;     //[01 Byte] 거래매체 "1"[IC], "2"[MS], "3"[RF]
        public string CardNo;       //[20 Byte] 카드번호 신용카드 앞 6자리외 나머지 마스킹 처리 우측 정렬, 좌측 "0" 채움
        public string PayMoney;     //[10 Byte] 승인요청금액(원거래+세금+봉사료) 우측 정렬, 좌측 "0" 채움
        public string Vat;          //[08 Byte] 부가세 선불카드의 경우 거래 전 잔액 우측 정렬, 좌측 "0" 채움
        public string Service;      //[08 Byte] 봉사료 우측 정렬, 좌측 "0" 채움
        public string InstMonth;    //[02 Byte] 할부개월 우측 정렬, 좌측 "0" 채움
        public string AcceptNo;     //[12 Byte] 선불카드 정보 카드종류[1B]+"0"[5B]+잔액[6B] 카드종류 "T"[티머니], "E"[캐시비], "M"[마이비], "U"[유페이], "H"[한페이], "K"[코레일]
        public string AcceptDate;   //[08 Byte] 매출발생 일자[YYYYMMDD]
        public string AcceptTime;   //[06 Byte] 매출발생 시간[hhmmss]
        public string TradNo;       //[12 Byte] 거래일련번호
        public string FranchiNo;    //[15 Byte] 가맹점번호 좌측정렬 Space[0x20] 채움
        public string TID;          //[14 Byte] 단말기번호 좌측정렬 Space[0x20] 채움
        public string IssuerCode;   //[04 Byte] *거래거절 응답메세지 발급사코드 거래구분코드 "X"[거래거절] 시 VAN사 응답 메세지 "-" + 응답코드[2B] + 응답메세지[37B]
        public string IssuerName;   //[16 Byte] *거래거절 응답메세지 발급사명
        public string PurchaseCode; //[04 Byte] *거래거절 응답메세지 매입사코드
        public string PurchaseName; //[16 Byte] *거래거절 응답메세지 매입사명

        public ushort Size => length;

        public void Deserialize(BinaryReader reader)
        {
            TradCode = reader.ReadFixedString(1);
            TradType = reader.ReadFixedString(1);
            CardNo = reader.ReadFixedString(20).TrimStart('0');
            PayMoney = reader.ReadFixedString(10).TrimStart('0');
            Vat = reader.ReadFixedString(8).TrimStart('0');
            Service = reader.ReadFixedString(8).TrimStart('0');
            InstMonth = reader.ReadFixedString(2).TrimStart('0');
            AcceptNo = reader.ReadFixedString(12).TrimEnd();
            AcceptDate = reader.ReadFixedString(8);
            AcceptTime = reader.ReadFixedString(6);
            TradNo = reader.ReadFixedString(12);
            FranchiNo = reader.ReadFixedString(15).TrimEnd();
            TID = reader.ReadFixedString(14).Trim();
            IssuerCode = reader.ReadFixedString(4);
            IssuerName = reader.ReadFixedString(16);
            PurchaseCode = reader.ReadFixedString(4);
            PurchaseName = reader.ReadFixedString(16);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(TradCode.GetBytes());
            writer.Write(TradType.GetBytes());
            writer.Write(CardNo.PadLeft(20, '0').GetBytes());
            writer.Write(PayMoney.PadLeft(10, '0').GetBytes());
            writer.Write(Vat.PadLeft(8, '0').GetBytes());
            writer.Write(Service.PadLeft(8, '0').GetBytes());
            writer.Write(InstMonth.PadLeft(2, '0').GetBytes());
            writer.Write(AcceptNo.PadRight(12).GetBytes());
            writer.Write(AcceptDate.GetBytes());
            writer.Write(AcceptTime.GetBytes());
            writer.Write(TradNo.GetBytes());
            writer.Write(FranchiNo.PadRight(15).GetBytes());
            writer.Write(TID.GetBytes());
            writer.Write(IssuerCode.GetBytes());
            writer.Write(IssuerName.GetBytes());
            writer.Write(PurchaseCode.GetBytes());
            writer.Write(PurchaseName.GetBytes());
        }
    }

    #endregion

    #region 거래취소

    /// <summary>
    /// 거래취소 요청 전문
    /// </summary>
    public class SendApprovalCancel : ISerializable
    {
        private const ushort length = 57;

        public string CancleCode;   //[01 Byte] "1"[요청전문 취소], "2"[결제기 마지막 거래 취소] 연동 장치에서 거래 승인데이터를 저장 안하는 경우 "2"를 이용하여 취소 요청, "1"인 경우 동일 가맹점에서만 가능하고 승인 시의 승인번호와 원거래일자, 원거래시간이 일치하여야 함
        public string TradCode;     //[01 Byte] "1"[신용승인]
        public string PayMoney;     //[10 Byte] 승인금액(원거래+세금+봉사료)
        public string Vat;          //[08 Byte] 부가세 우측 정렬, 좌측 "0" 채움
        public string Service;      //[08 Byte] 봉사료 우측 정렬, 좌측 "0" 채움
        public string InstMonth;    //[02 Byte] 할부개월 우측 정렬, 좌측 "0" 채움
        public string SignYN;       //[01 Byte] 서명 여부 "1"[비서명], "2"[서명]
        public string AcceptNo;     //[12 Byte] 승인번호, 좌측 정렬 Space 채움
        public string AcceptDate;   //[08 Byte] 원거래일자 일자[YYYYMMDD]
        public string AcceptTime;   //[06 Byte] 원거래시간 시간[hhmmss]

        public ushort Size => length;

        public void Deserialize(BinaryReader reader)
        {
            CancleCode = reader.ReadFixedString(1);
            TradCode = reader.ReadFixedString(1);
            PayMoney = reader.ReadFixedString(10);
            Vat = reader.ReadFixedString(8);
            Service = reader.ReadFixedString(8);
            InstMonth = reader.ReadFixedString(2);
            SignYN = reader.ReadFixedString(1);
            AcceptNo = reader.ReadFixedString(12);
            AcceptDate = reader.ReadFixedString(8);
            AcceptTime = reader.ReadFixedString(6);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(CancleCode.GetBytes());
            writer.Write(TradCode.GetBytes());
            writer.Write(PayMoney.PadLeft(10, '0').GetBytes());
            writer.Write(Vat.PadLeft(8, '0').GetBytes());
            writer.Write(Service.PadLeft(8, '0').GetBytes());
            writer.Write(InstMonth.PadLeft(2, '0').GetBytes());
            writer.Write(SignYN.GetBytes());
            writer.Write(AcceptNo.PadRight(12).GetBytes());
            writer.Write(AcceptDate.GetBytes());
            writer.Write(AcceptTime.GetBytes());
        }
    }

    #endregion

    #region 카드조회

    /// <summary>
    /// 카드조회 응답 전문
    /// </summary>
    public class ReceiveCardSearch : ISerializable
    {
        private const ushort length = 53;

        public string TradType;     //[01 Byte] 거래매체 "1"[IC], "2"[MS], "3"[RF]
        public string CardType;     //[01 Byte] 카드종료 "T"[티머니], "E"[캐시비], "M"[마이비], "U"[유페이], "H"[한페이], "K"[코레일]
        public string CardNo;       //[20 Byte] 카드번호 신용카드 앞 6자리외 나머지 마스킹 처리 우측 정렬, 좌측 "0" 채움
        public string AcceptDate;   //[14 Byte] 직전거래일시 [YYYYMMDDhhmmss] 결제기에 직전거래가 없는 경우 "0"으로 채움
        public string PayMoney;     //[08 Byte] 직전거래금액 우측 정렬, 좌측 "0" 채움. 결제기에 직전거래가 없는 경우 후불인 경우 "0"채움. 선불인 경우 카드의 직전 거래금액
        public string CardBalance;  //[08 Byte] 카드잔액, 좌측 "0" 채움. 후불인 경우 "0" 채움.
        public string TradeGubun;   //[01 Byte] *거래거절 응답메세지 매입사명

        public ushort Size => length;

        public void Deserialize(BinaryReader reader)
        {
            TradType = reader.ReadFixedString(1);
            CardType = reader.ReadFixedString(1);
            CardNo = reader.ReadFixedString(20).TrimStart('0');
            AcceptDate = reader.ReadFixedString(14);
            PayMoney = reader.ReadFixedString(8).TrimStart('0');
            CardBalance = reader.ReadFixedString(8).TrimStart('0');
            TradeGubun = reader.ReadFixedString(1);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(TradType.GetBytes());
            writer.Write(CardType.GetBytes());
            writer.Write(CardNo.PadLeft(20, '0').GetBytes());
            writer.Write(AcceptDate.GetBytes());
            writer.Write(PayMoney.PadLeft(8, '0').GetBytes());
            writer.Write(CardBalance.PadLeft(8, '0').GetBytes());
            writer.Write(TradeGubun.GetBytes());
        }
    }

    #endregion

    #region 카드 UID 읽기

    /// <summary>
    /// 가드 UID 읽기 응답 전문
    /// </summary>
    public class ReceiveCradUIDRead : ISerializable
    {
        private const ushort length = 10;
        /// <summary>
        /// 마이페어 카드 UID 4바이트 십진수 표시
        /// </summary>
        public string CardUID;

        public ushort Size => length;

        public void Deserialize(BinaryReader reader)
        {
            CardUID = reader.ReadFixedString(10);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(CardUID.GetBytes());
        }
    }

    #endregion

    #region 이벤트

    /// <summary>
    /// 이벤트 응답 전문
    /// </summary>
    public class ReceiveEvent : ISerializable
    {
        private const ushort length = 1;

        public string EventCode;

        public ushort Size => length;

        public void Deserialize(BinaryReader reader)
        {
            EventCode = reader.ReadFixedString(1);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(EventCode.GetBytes());
        }
    }

    #endregion

    #region 설정 정보 셋팅

    /// <summary>
    /// 설정 정보 셋팅 요청 전문
    /// </summary>
    public class SendSetting : ISerializable
    {
        private const byte NullToken = 0x00;
        private const ushort length = 150;

        public string TerminalID;       //[16 Byte] CATID 마지막 끝에 0x00를 붙여서 전송
        public string SAMSlot1;         //[01 Byte] SAM 슬롯1 "0"[후불], "1"[티머니], "2"[이비], "3"[한페이], "4"[유페이], "5"[마이비]
        public string SAMSlot2;         //[01 Byte] SAM 슬롯2 "0"[후불], "1"[티머니], "2"[이비], "3"[한페이], "4"[유페이], "5"[마이비]
        public string SAMSlot3;         //[01 Byte] SAM 슬롯3 "0"[후불], "1"[티머니], "2"[이비], "3"[한페이], "4"[유페이], "5"[마이비]
        public string SAMSlot4;         //[01 Byte] SAM 슬롯4 "0"[후불], "1"[티머니], "2"[이비], "3"[한페이], "4"[유페이], "5"[마이비]
        public string DeviceType;       //[01 Byte] 통신연결방식 "0"[COM], "1"[USB], "2"[이더넷] 통신포트 설정이 USB인 경우 USEType은 무조건 VCP "1"로 셋팅됨
        public string EthernetIP;       //[16 Byte] 이더넷 통신 IP 마지막 끝에 0x00를 붙여서 전송
        public string EthernetPort;     //[16 Byte] 이더넷 통신 IP 마지막 끝에 0x00를 붙여서 전송
        public string MID;              //[16 Byte] 가상ID 마지막 끝에 0x00를 붙여서 전송
        public string VanIP;            //[16 Byte] VAN서버 IP 마지막 끝에 0x00를 붙여서 전송
        public string VanPort;          //[16 Byte] VAN서버 PORT 마지막 끝에 0x00를 붙여서 전송
        public string DeviceIPType;     //[01 Byte] 단말기 IP 방식 "0"[DHCP], "1"[STATIC] NET_Mode는 무조건 이더넷 모드로 셋팅
        public string DeviceIP;         //[16 Byte] 단말기 IP 마지막 끝에 0x00를 붙여서 전송
        public string DeviceSubNet;     //[16 Byte] 단말기 SUBNET 마지막 끝에 0x00를 붙여서 전송
        public string DeviceGateWay;    //[16 Byte] 단말기 GATEWAY 마지막 끝에 0x00를 붙여서 전송

        public ushort Size => length;

        public void Deserialize(BinaryReader reader)
        {
            TerminalID = reader.ReadFixedString(16).RemoveVarChar(NullToken);
            SAMSlot1 = reader.ReadFixedString(1);
            SAMSlot2 = reader.ReadFixedString(1);
            SAMSlot3 = reader.ReadFixedString(1);
            SAMSlot4 = reader.ReadFixedString(1);
            DeviceType = reader.ReadFixedString(1);
            EthernetIP = reader.ReadFixedString(16).RemoveVarChar(NullToken);
            EthernetPort = reader.ReadFixedString(16).RemoveVarChar(NullToken);
            MID = reader.ReadFixedString(16).RemoveVarChar(NullToken);
            VanIP = reader.ReadFixedString(16).RemoveVarChar(NullToken);
            VanPort = reader.ReadFixedString(16).RemoveVarChar(NullToken);
            DeviceIPType = reader.ReadFixedString(1);
            DeviceIP = reader.ReadFixedString(16).RemoveVarChar(NullToken);
            DeviceSubNet = reader.ReadFixedString(16).RemoveVarChar(NullToken);
            DeviceGateWay = reader.ReadFixedString(16).RemoveVarChar(NullToken);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(TerminalID.PadRight(16, Convert.ToChar(NullToken)).GetBytes());
            writer.Write(SAMSlot1.GetBytes());
            writer.Write(SAMSlot2.GetBytes());
            writer.Write(SAMSlot3.GetBytes());
            writer.Write(SAMSlot4.GetBytes());
            writer.Write(DeviceType.GetBytes());
            writer.Write(EthernetIP.PadRight(16, Convert.ToChar(NullToken)).GetBytes());
            writer.Write(EthernetPort.PadRight(16, Convert.ToChar(NullToken)).GetBytes());
            writer.Write(MID.PadRight(16, Convert.ToChar(NullToken)).GetBytes());
            writer.Write(VanIP.PadRight(16, Convert.ToChar(NullToken)).GetBytes());
            writer.Write(VanPort.PadRight(16, Convert.ToChar(NullToken)).GetBytes());
            writer.Write(DeviceIPType.GetBytes());
            writer.Write(DeviceIP.PadRight(16, Convert.ToChar(NullToken)).GetBytes());
            writer.Write(DeviceSubNet.PadRight(16, Convert.ToChar(NullToken)).GetBytes());
            writer.Write(DeviceGateWay.PadRight(16, Convert.ToChar(NullToken)).GetBytes());
        }
    }

    #endregion

    #region 화면&음성 설정

    /// <summary>
    /// 화면&음성 설정 요청/응답 전문
    /// </summary>
    public class SendReceiveScreenNSoundSetting : ISerializable
    {
        private const ushort length = 3;

        public string ScreenBrightness; //[01 Byte] 화면밝기 "0~9" 이외 문자 전송시 현재 설정값 응답
        public string VoiceVolume;      //[01 Byte] 음성크기 "0~9" 이외 문자 전송시 현재 설정값 응답
        public string TouchVolume;      //[01 Byte] 터치음크기 "0~9" 이외 문자 전송시 현재 설정값 응답

        public ushort Size => length;

        public void Deserialize(BinaryReader reader)
        {
            ScreenBrightness = reader.ReadFixedString(1);
            VoiceVolume = reader.ReadFixedString(1);
            TouchVolume = reader.ReadFixedString(1);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(ScreenBrightness.GetBytes());
            writer.Write(VoiceVolume.GetBytes());
            writer.Write(TouchVolume.GetBytes());
        }
    }

    #endregion

    #region 버전 체크

    /// <summary>
    /// 버전 체크 응답 전문
    /// </summary>
    public class ReceiveVersionCheck : ISerializable
    {
        private const ushort length = 45;

        public string Boot0Version; //[15 Byte] BOOT0 버전(TL3500_17071801)
        public string Boot1Version; //[15 Byte] BOOT1 버전(TL3500_18043001)
        public string ApplVersion;  //[15 Byte] APPL 버전(TL3500_18041101)

        public ushort Size => length;

        public void Deserialize(BinaryReader reader)
        {
            Boot0Version = reader.ReadFixedString(15);
            Boot1Version = reader.ReadFixedString(15);
            ApplVersion = reader.ReadFixedString(15);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Boot0Version.GetBytes());
            writer.Write(Boot1Version.GetBytes());
            writer.Write(ApplVersion.GetBytes());
        }
    }

    #endregion
}
