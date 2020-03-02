using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FadeFox.LPR
{
    /// <summary>
    /// LPR데이터 파징
    /// </summary>
    public class LprParsing
    {
        #region NPSERVER 통신변수


        private enum EnumCarPicType
        {
            /// <summary>
            /// CH1#01나2985#N#2010\06\08\CH1_20100608154810_01나2985.jpg
            /// </summary>
            NORMAL,
            /// <summary>
            /// 후면촬영결과가 전면촬영결과와 다른 경우 새로운 차량으로 간주된 경우이다.
            /// (변경:전면촬영결과가 없는 상태에서 후면만 촬영된 경우로 범위를 한정함)
            /// “NW”라는 문구가 추가된다.
            /// 통신 내용 : NW#채널#차번#구분자#파일경로
            /// 통신 내용 : NW#CH1#01나2985#N#2010\06\08\CH1_20100608154810_01나2985.jpg
            /// </summary>
            NW,
            /// <summary>
            /// 후면촬영결과 오인식으로 판단된 경우이다.
            /// “UP”라는 문구가 추가되며, 전면촬영결과가 앞부분에 반복되어 표시된다.
            /// 통신 문자열상 CR 코드값은 들어가 있지 않는다.
            /// 전면이 “01나2985” 차량이고 후면이 “01나2988”인 경우이다.
            /// 통신 내용 : UP#전면채널#전면차번#구분자#전면파일경로#채널#차번#구분자#파일경로
            /// 통신 내용 : UP# CH1#01나2985#N#2010\06\08\CH1_20100608154810_01나2985.jpg
            /// #CH1#01나2988#N#2010\06\08\CH101_20100608154815_01나2988.jpg
            /// </summary>
            UP,
            /// <summary>
            /// 전면의 결과와 후면의 결과가 오인식이 아닌 다른 차로 인식된 경우.. 비교가 된 전면차량정보를 함께 전달한다.
            ///	“NP”라는 문구가 추가되며, 전면촬영결과가 앞부분에 반복되어 표시된다.
            ///	통신 문자열상 CR 코드 값은 들어가 있지 않는다.
            ///	전면이 “01나2985” 차량이고 후면이 “01나1234”인 경우이다.
            ///-----------------------------------------------------------------예) Send Type이 “1” 인 경우
            ///통신 규약 : NP#전면채널#전면차번#전면파일경로#채널#차번#파일경로
            ///통신 내용 : NP#CH1#01나2985#2010\06\08\CH1_20100608154810_01나2985.jpg
            ///#CH1#01나2988#2010\06\08\CH101_20100608154815_01나2988.jpg
            ///예) Send Type이 “2” 인 경우
            ///통신 내용 : NP#전면채널#전면차번#구분자#전면파일경로#채널#차번#구분자#파일경로
            ///통신 내용 : NP# CH1#01나2985#N#2010\06\08\CH1_20100608154810_01나2985.jpg
            ///#CH1#01나2988#N#2010\06\08\CH101_20100608154815_01나1234.jpg
            /// </summary>
            NP,
        }
        private enum EnumCarStatus
        {
            /// <summary>
            /// 정인식
            /// </summary>
            N,
            /// <summary>
            /// 정인식
            /// </summary>
            O,
            /// <summary>
            /// 오인식
            /// </summary>
            M,
            /// <summary>
            /// 부분인식
            /// </summary>
            P,
            /// <summary>
            /// 미인식
            /// </summary>
            X
        }

        private enum EnumNp1400Status
        {
            /// <summary>
            /// NP1400 죽음
            /// </summary>
            LPR_N,
            /// <summary>
            /// NP1400 살아있음
            /// </summary>
            LPR_R
        }

        private const int mFail = -1;
        public const string NPSERVER_NOPLATE = "00000";

        private const string m_NpserverStatusOk = "OK";
        private const string NPSERVER_STX = "CH";
        private const string NPSERVER_ETX = ".JPG";
        private const string NPSERVER_OK = "OK";
        private const string NPSERVER_SUB_DELIMITER = "_";
        private const string NPSERVER_DELIMITER = "#";

        #endregion



        private string m_NpserverData = string.Empty;

        public void ParsingLprData(string pLocalIp,string pServerIp, ref  LprParsing.LprData pLprData, byte[] pReceiveByte)
        {
            try
            {

                lock (this)
                {
                    string tempData;

                    tempData = Encoding.Default.GetString(pReceiveByte).ToUpper();
                    if (tempData.Trim().Length < 1)
                    {
                        return;
                    }

                  //  tempData = @LPR_NLPR_NOKOK"UP# CH1#01나2985#N#2010\06\08\CH1_20100608154810_01나2985.jpg#CH1#01나2988#N#2010\06\08\CH101_20100608154815_01나2988.jpgOKOK";
                    ////tempData = @"LPR_NLPR_NOKOKNW#CH1#01나2985#N#2010\06\08\CH1_20100608154810_01나2985.jpgOKOK";
                    int nPos = tempData.IndexOf(pLocalIp);
                    if (nPos != -1)
                    {
                        tempData = tempData.Substring(nPos + pLocalIp.Length);
                        if (tempData.Length.Equals(0)) return;
                    }

                    nPos = tempData.IndexOf(pLocalIp);
                    if (nPos != -1)
                    {
                        tempData = tempData.Substring(nPos + pLocalIp.Length);
                        if (tempData.Length.Equals(0)) return;
                    }


                    nPos = tempData.IndexOf(EnumNp1400Status.LPR_R.ToString());
                    if (nPos != mFail)
                    {
                        tempData = tempData.Substring(nPos + EnumNp1400Status.LPR_R.ToString().Length);
                        if (tempData.Length.Equals(0)) return;
                    }

                    nPos = tempData.IndexOf(EnumNp1400Status.LPR_R.ToString());
                    if (nPos != mFail)
                    {
                        tempData = tempData.Substring(nPos + EnumNp1400Status.LPR_R.ToString().Length);
                        if (tempData.Length.Equals(0)) return;
                    }


                    nPos = tempData.IndexOf(EnumNp1400Status.LPR_N.ToString());
                    if (nPos != mFail)
                    {
                        tempData = tempData.Substring(nPos + EnumNp1400Status.LPR_N.ToString().Length);
                        if (tempData.Length.Equals(0)) return;
                    }
                    nPos = tempData.IndexOf(EnumNp1400Status.LPR_N.ToString());
                    if (nPos != mFail)
                    {
                        tempData = tempData.Substring(nPos + EnumNp1400Status.LPR_N.ToString().Length);
                        if (tempData.Length.Equals(0)) return;
                    }

                    nPos = tempData.IndexOf(NPSERVER_OK);
                    if (nPos != mFail)
                    {

                        tempData = tempData.Substring(nPos + NPSERVER_OK.ToString().Length);
                        if (tempData.Length.Equals(0)) return;
                    }


                    nPos = tempData.IndexOf(NPSERVER_OK);
                    if (nPos != mFail)
                    {

                        tempData = tempData.Substring(nPos + NPSERVER_OK.ToString().Length);
                        if (tempData.Length.Equals(0)) return;
                    }

                    int startPosion = 0, endPositon = 0; // 시작점 및 끝점
                    const int OFFSET = 4;
                    m_NpserverData += tempData.ToUpper();
                    do
                    {
                        startPosion = m_NpserverData.IndexOf(NPSERVER_STX);
                        endPositon = m_NpserverData.IndexOf(NPSERVER_ETX);

                        if (endPositon.Equals(mFail)) break;
                        if (startPosion.Equals(mFail))
                        {
                            m_NpserverData = String.Empty;
                            break;
                        }

                        string currentProcessData = m_NpserverData.Substring(0, endPositon + OFFSET); // 현재 처리데이터 분리

                        

                        string TypeCheckData = currentProcessData.Substring(0, startPosion);

                        string NPType = EnumCarPicType.NORMAL.ToString();
                        if (TypeCheckData.Contains(EnumCarPicType.NW.ToString()))
                        {
                            NPType = EnumCarPicType.NW.ToString();
                        }
                        else if (TypeCheckData.Contains(EnumCarPicType.UP.ToString()))
                        {
                            NPType = EnumCarPicType.UP.ToString();
                        }
                        else if (TypeCheckData.Contains(EnumCarPicType.NP.ToString()))
                        {
                            NPType = EnumCarPicType.NP.ToString();
                        }

                        
                        pLprData.Clear();
                        pLprData.CarPicType = NPType;
                        string carNumberStatus = string.Empty; // 정인식 N 부분인식 P 미인식 X

                        string carYmd = string.Empty;
                        string carHms = string.Empty;
                        string carNumber = string.Empty;
                        string imagePath=string.Empty;
                        int channelNo=0;
                        string numType = string.Empty;
                        
                        switch ((EnumCarPicType)Enum.Parse(typeof(EnumCarPicType), NPType))
                        {
                            case EnumCarPicType.NORMAL:          // 후면 번호판(전면 번호판은 촬영못하고 후면만 촬영)
                            case EnumCarPicType.NW:        // 정상적인 자료(전면 또는 후면 번호판중 인식된 번호판 한개만 수신)
                                {

                                    string sWork1 = String.Empty;
                                    sWork1 = currentProcessData.Substring(startPosion, endPositon + OFFSET - startPosion);
                                    m_NpserverData = m_NpserverData.Substring(endPositon + OFFSET); // 다음처리할 데이터
                                    GetLPRData(sWork1, pServerIp, ref carYmd, ref carHms, ref carNumber, ref imagePath, ref channelNo, ref numType);
                                    pLprData.FrontYmd = carYmd;
                                    pLprData.FrontHms = carHms;
                                    pLprData.FrontCarnumber = carNumber;
                                    pLprData.FrontImagePath = imagePath;
                                    pLprData.FrontChannel = channelNo;
                                    pLprData.FrontNumType = numType;
                                    
                                }
                                break;

                            case EnumCarPicType.UP:     // 앞뒤 번호판이 다름(동일차량)
                            case EnumCarPicType.NP:      // 앞뒤 번호판이 다름(다른차량)
                                {
                                    string sWork1 = currentProcessData.Substring(startPosion, endPositon + OFFSET - startPosion);
                                    m_NpserverData = m_NpserverData.Substring(endPositon + OFFSET); // 다음처리할 데이터
                                    GetLPRData(sWork1, pServerIp,  ref carYmd, ref carHms, ref carNumber, ref imagePath, ref channelNo, ref numType);

                                    pLprData.FrontYmd = carYmd;
                                    pLprData.FrontHms = carHms;
                                    pLprData.FrontCarnumber = carNumber;
                                    pLprData.FrontImagePath = imagePath;
                                    pLprData.FrontChannel = channelNo;
                                    pLprData.FrontNumType = numType;
                                    string sWork2 = string.Empty;
                                    endPositon = m_NpserverData.IndexOf(NPSERVER_ETX);
                                    startPosion = m_NpserverData.IndexOf(NPSERVER_STX);
                                    sWork2 = m_NpserverData.Substring(startPosion, endPositon + OFFSET - startPosion);
                                    m_NpserverData = m_NpserverData.Substring(endPositon + OFFSET);
                                    GetLPRData(sWork2, pServerIp,  ref carYmd, ref carHms, ref carNumber, ref imagePath, ref channelNo, ref numType);
                                    pLprData.BackYmd = carYmd;
                                    pLprData.BackHms = carHms;
                                    pLprData.BackCarnumber = carNumber;
                                    pLprData.BackImagePath = imagePath;
                                    pLprData.BackChannel = channelNo;
                                    pLprData.BackNumType = numType;
                                }
                                break;

                            default:
                                m_NpserverData = string.Empty;
                                break;
                        }
                    } while (m_NpserverData.Length > 0);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void GetLPRData(string sData,string pServerIp, ref string pCarYmd,ref string pCarHms,ref string pCarNumber,ref string pImagePath,ref int pChannelNo,ref string pNumType)
        {
            String[] sItem = sData.Split('#');
            int nCPos = sItem.Length - 1;
            String[] sSub = sItem[nCPos].Split('_');

            pCarYmd = sSub[1].Substring(0, 4) + "-" + sSub[1].Substring(4, 2) + "-" + sSub[1].Substring(6, 2);
            pCarHms = sSub[1].Substring(8, 2) + ":" + sSub[1].Substring(10, 2) + ":" + sSub[1].Substring(12, 2);
            pCarNumber = sItem[1];

            int nPos = sItem[nCPos].ToUpper().IndexOf("MSIMAGE");
            if (nPos == mFail)
            {
                if (sItem[nCPos].Substring(0, 1) == @"\")
                {
                    pImagePath = @"\MSImage" + sItem[nCPos];
                }
                else
                {
                    pImagePath = @"\MSImage" + @"\" + sItem[nCPos];
                }
            }
            else if (nPos > 1)
            {
                pImagePath = @"\" + sItem[nCPos].Substring(nPos);
            }
            else if (nPos < 1)
            {
                pImagePath = @"\" + sItem[nCPos];
            }
            else
            {
                pImagePath = sItem[nCPos];
            }
            pImagePath= @"\\" + pServerIp+pImagePath;
            if (sItem[0].Substring(0, 2).Equals(NPSERVER_STX))
            {
                pChannelNo = int.Parse(sItem[0].Substring(2));
            }
            if (pCarNumber.Contains("X"))
            {
                pNumType = "X";
            }
            else if(pCarNumber.Contains("0000"))
            {
                pNumType = "P";
            }
            else
            {
                pNumType = "N";
            }
        }
        /// <summary>
        /// LPR데이터형
        /// </summary>
        public class LprData
        {
            string mCarPicType = string.Empty;
            public string CarPicType
            {
                set { mCarPicType = value; }
                get { return mCarPicType; }
            }

            string mFrontNumType = string.Empty;
            public string FrontNumType
            {
                set { mFrontNumType = value; }
                get { return mFrontNumType; }
            }

            string mFrontCarnumber = string.Empty;
            public string FrontCarnumber
            {
                set { mFrontCarnumber = value; }
                get { return mFrontCarnumber; }
            }

            string mFrontYmd = string.Empty;
            public string FrontYmd
            {
                set { mFrontYmd = value; }
                get { return mFrontYmd; }
            }

            string mFrontHms = string.Empty;
            public string FrontHms
            {
                set { mFrontHms = value; }
                get { return mFrontHms; }
            }

            int mFrontChannel = 1;
            public int FrontChannel
            {
                set { mFrontChannel = value; }
                get { return mFrontChannel; }
            }

            string mFrontImagePath = string.Empty;
            public string FrontImagePath
            {
                set { mFrontImagePath = value; }
                get { return mFrontImagePath; }
            }

            string mBackNumType = string.Empty;
            public string BackNumType
            {
                set { mBackNumType = value; }
                get { return mBackNumType; }
            }

            string mBackCarnumber = string.Empty;
            public string BackCarnumber
            {
                set { mBackCarnumber = value; }
                get { return mBackCarnumber; }
            }

            string mBackYmd = string.Empty;
            public string BackYmd
            {
                set { mBackYmd = value; }
                get { return mBackYmd; }
            }

            string mBackHms = string.Empty;
            public string BackHms
            {
                set { mBackHms = value; }
                get { return mBackHms; }
            }

            int mBackChannel = 1;
            public int BackChannel
            {
                set { mBackChannel = value; }
                get { return mBackChannel; }
            }

            string mBackImagePath = string.Empty;
            public string BackImagePath
            {
                set { mBackImagePath = value; }
                get { return mBackImagePath; }
            }

            public void Clear()
            {
                 mCarPicType = string.Empty;
                 mFrontNumType = string.Empty;
                 mFrontCarnumber = string.Empty;
                 mFrontYmd = string.Empty;
                 mFrontHms = string.Empty;
                 mFrontChannel = 1;
                 mFrontImagePath = string.Empty;
                 mBackNumType = string.Empty;
                 mBackCarnumber = string.Empty;
                 mBackYmd = string.Empty;
                 mBackHms = string.Empty;
                 mBackChannel = 1;
                 mBackImagePath = string.Empty;
            }

            public void Clonet(LprData pLprData)
            {
                mCarPicType = pLprData.mCarPicType;
                mFrontNumType = pLprData.mFrontNumType;
                mFrontCarnumber = pLprData.mFrontCarnumber;
                mFrontYmd = pLprData.mFrontYmd;
                mFrontHms = pLprData.mFrontHms;
                mFrontChannel = pLprData.mFrontChannel;
                mFrontImagePath = pLprData.mFrontImagePath;
                mBackNumType = pLprData.mBackNumType;
                mBackCarnumber = pLprData.mBackCarnumber;
                mBackYmd = pLprData.mBackYmd;
                mBackHms = pLprData.mBackHms;
                mBackChannel = pLprData.mBackChannel;
                mBackImagePath = pLprData.mBackImagePath;

            }
        }

    }
}
