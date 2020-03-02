using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace FadeFox.Display
{
    public class DisplayClass
    {
            public enum enumColor
            {
                Black = 0,
                Red,
                Green,
                Yellow,
                Violet,
                ThinRed,
                ThinGreen,
                White
            }

            public enum enumCmdType
            {
                /// <summary>
                /// 고정문구
                /// </summary>
                cmdFIX = 83,
                /// <summary>
                /// 일반문구
                /// </summary>
                cmdNORMAL = 84
            }

            public enum enumDisplayEffect
            {
                NormalEffect = 128,
                LeftScroll = 129,
                RightScroll = 130,
                UpScroll = 131,
                DownScroll = 132,
                DeleteNormalEffect = 0,
                DeleteLeftScroll = 1,
                DeleteRightScroll = 2,
                DeleteUpScroll = 3,
                DeleteDownScroll = 4,
                Reverse = 0x15,
                ReverseBlink = 0x16,
                Blink = 0x17

            }

            public enum enumDisplayPosition
            {
                /// <summary>
                /// 상단위치
                /// </summary>
                dpTop,
                /// <summary>
                /// 하단위치
                /// </summary>
                dpBottom,
                /// <summary>
                /// 전체
                /// </summary>
                dpAll
            }

            public enum enumSendType
            {
                stEach = 0,
                stAll
            }

            public enum enumMessageAlign
            {
                Left = 0,
                Right
            }
            /// <summary>
            /// 저장타입 Ram이면 1회용으로 전과판을 껐다키면 사라진다
            /// </summary>
            public enum SaveDevice
            {
                /// <summary>
                /// 1회용
                /// </summary>
                Ram = 1,
                /// <summary>
                /// 내부에저장
                /// </summary>
                Rom = 0
            }


            #region 내부 변수 선언

            private enumCmdType eCmdType;
            private enumDisplayEffect eDisplayEffect;
            private enumDisplayPosition eDisplayPosition;
            private int nDisplaySpeed;
            private int nDisplayTime;
            private int nRepeatCount;
            private enumSendType eSendType;
            private enumMessageAlign eTopMessageAlign;
            private enumMessageAlign eBottomMessageAlign;

            private enumColor eTopMessageColor;
            private enumColor eBottomMessageColor;
            private SaveDevice m_SaveType;
            #endregion




            #region 내부 Property 선언

            private enumColor TopMessageColor { set { eTopMessageColor = value; } get { return eTopMessageColor; } }
            private enumColor BottomMessageColor { set { eBottomMessageColor = value; } get { return eBottomMessageColor; } }

            private enumCmdType CmdType { set { eCmdType = value; } get { return eCmdType; } }
            private enumDisplayEffect DisplayEffect { set { eDisplayEffect = value; } get { return eDisplayEffect; } }
            private enumDisplayPosition DisplayPosition { set { eDisplayPosition = value; } get { return eDisplayPosition; } }
            private int DisplaySpeed { set { nDisplaySpeed = value; } get { return nDisplaySpeed; } }
            private int DisplayTime { set { nDisplayTime = value; } get { return nDisplayTime; } }
            private int RepeatCount { set { nRepeatCount = value; } get { return nRepeatCount; } }
            private enumSendType SendType { set { eSendType = value; } get { return eSendType; } }
            private enumMessageAlign TopMessageAlign { set { eTopMessageAlign = value; } get { return eTopMessageAlign; } }
            private enumMessageAlign BottomMessageAlign { set { eBottomMessageAlign = value; } get { return eBottomMessageAlign; } }
            private SaveDevice SaveType { set { m_SaveType = value; } get { return m_SaveType; } }

            #endregion





            #region 생성자

            public DisplayClass()
            {
                CmdType = enumCmdType.cmdNORMAL;
                DisplayEffect = enumDisplayEffect.LeftScroll;
                DisplayPosition = enumDisplayPosition.dpAll;
                DisplaySpeed = 40;
                DisplayTime = 12;
                RepeatCount = 1;
                SendType = enumSendType.stEach;
                TopMessageAlign = enumMessageAlign.Left;
                BottomMessageAlign = enumMessageAlign.Left;
                SaveType = SaveDevice.Ram;
                TopMessageColor = enumColor.Yellow;
                BottomMessageColor = enumColor.Red;

            }

            #endregion

            #region 일반문구관련

            /// <summary>
            /// 하단색상설정
            /// </summary>
            /// <param name="pTopColor"></param>
            public void SetTopColorDisplaySetting(string pTopColor)
            {
                TopMessageColor = (enumColor)Enum.Parse(typeof(enumColor), pTopColor);
            }
            /// <summary>
            /// 하단색상설정
            /// </summary>
            /// <param name="pTopColor"></param>
            public void SetTopColorDisplaySetting(enumColor pTopColor)
            {
                TopMessageColor = pTopColor;
            }

            /// <summary>
            /// 하단색상설정
            /// </summary>
            /// <param name="pTopColor"></param>
            public void SetBottomColorDisplaySetting(string pBottomColor)
            {
                BottomMessageColor = (enumColor)Enum.Parse(typeof(enumColor), pBottomColor);
            }
            /// <summary>
            /// 하단색상설정
            /// </summary>
            /// <param name="pTopColor"></param>
            public void SetBottomColorDisplaySetting(enumColor pBottomColor)
            {
                BottomMessageColor = pBottomColor;
            }

            public void SetDIsplaySpeedSetting(int pDisplaySpeed)
            {
                DisplaySpeed = pDisplaySpeed;
            }

            public void SetDIsplayTimeSetting(int pDisplayTime)
            {
                DisplayTime = pDisplayTime;
            }

            public void SetTopMessageAlign(enumMessageAlign pMessageAlign)
            {
                TopMessageAlign = pMessageAlign;
            }

            public void SetTopMessageAlign(string pMessageAlign)
            {
                TopMessageAlign = (enumMessageAlign)Enum.Parse(typeof(enumMessageAlign), pMessageAlign);
            }

            public void SetBottomMessageAlign(enumMessageAlign pMessageAlign)
            {
                BottomMessageAlign = pMessageAlign;
            }

            public void SetBottomMessageAlign(string pMessageAlign)
            {
                BottomMessageAlign = (enumMessageAlign)Enum.Parse(typeof(enumMessageAlign), pMessageAlign);
            }

            public void SetDisplayEffect(string pDIsplayEffect)
            {
                DisplayEffect = (enumDisplayEffect)Enum.Parse(typeof(enumDisplayEffect), pDIsplayEffect);
            }

            public void SetDisplayEffect(enumDisplayEffect pDIsplayEffect)
            {
                DisplayEffect = pDIsplayEffect;
            }

              /// <summary>
            /// 전광판 일반메세지 표출 상단 / 하단 문구
            /// </summary>
            /// <param name="pTopMsg"></param>
            /// <param name="pBottomMsg"></param>
            /// <returns></returns>
            public byte[] NormalDisplayMessage(string pTopMsg, string pBottomMsg)
            {
                CmdType = enumCmdType.cmdNORMAL;
                DisplayPosition = enumDisplayPosition.dpAll;
                RepeatCount = 1;
                SendType = enumSendType.stEach;
                SaveType = SaveDevice.Ram;
                return DisplayMessage(pTopMsg, pBottomMsg);
            }

            /// <summary>
            /// 전광판 일반메세지 표출 상단 / 하단 문구
            /// </summary>
            /// <param name="pTopMsg"></param>
            /// <param name="pBottomMsg"></param>
            /// <returns></returns>
            public byte[] NormalDisplayMessage(string pTopMsg, string pBottomMsg,int pDisplayTIme)
            {
                CmdType = enumCmdType.cmdNORMAL;
                DisplayPosition = enumDisplayPosition.dpAll;
                RepeatCount = 1;
                SendType = enumSendType.stEach;
                SaveType = SaveDevice.Ram;
                DisplayTime = pDisplayTIme;
                return DisplayMessage(pTopMsg, pBottomMsg);
            }
    
            #endregion

            #region 고정문구관련


            /// <summary>
            /// 전광판 고정메세지 표출 상단 / 하단 문구
            /// </summary>
            /// <param name="pTopMsg"></param>
            /// <param name="pBottomMsg"></param>
            /// <returns></returns>
            public byte[] PrefixDisplayMessage(string pTopMsg, string pBottomMsg)
            {
                CmdType = enumCmdType.cmdFIX;
                DisplayPosition = enumDisplayPosition.dpAll;
                RepeatCount = 1;
                SendType = enumSendType.stEach;
                SaveType = SaveDevice.Rom;
                return DisplayMessage(pTopMsg, pBottomMsg);
            }

            #endregion

           
            public byte[] DisplayMessage(string TopMsg, string BottomMsg)
            {
                try
                {
                    int i, nLen;
                    string sData = "";



                        switch (DisplayPosition)
                        {
                            case enumDisplayPosition.dpTop:
                                {
                                    nLen = Encoding.Default.GetByteCount(TopMsg);
                                    if (nLen > 12)
                                    {
                                        sData = TopMsg;
                                    }
                                    else
                                    {
                                        sData = TopMessageAlign == enumMessageAlign.Left ? SetLeftAlign(TopMsg, 12) : SetRightAlign(TopMsg, 12);
                                        sData += BottomMessageAlign == enumMessageAlign.Left ? SetLeftAlign(BottomMsg, 12) : SetRightAlign(BottomMsg, 12);
                                    }
                                }
                                break;

                            case enumDisplayPosition.dpBottom:
                                {
                                    nLen = Encoding.Default.GetByteCount(BottomMsg);
                                    if (nLen > 12)
                                    {
                                        sData = BottomMsg;
                                    }
                                    else
                                    {
                                        sData = TopMessageAlign == enumMessageAlign.Left ? SetLeftAlign(TopMsg, 12) : SetRightAlign(TopMsg, 12);
                                        sData += BottomMessageAlign == enumMessageAlign.Left ? SetLeftAlign(BottomMsg, 12) : SetRightAlign(BottomMsg, 12);
                                    }
                                }
                                break;

                            case enumDisplayPosition.dpAll:
                                {
                                    sData = TopMessageAlign == enumMessageAlign.Left ? SetLeftAlign(TopMsg, 12) : SetRightAlign(TopMsg, 12);
                                    sData += BottomMessageAlign == enumMessageAlign.Left ? SetLeftAlign(BottomMsg, 12) : SetRightAlign(BottomMsg, 12);
                                }
                                break;
                        }
                    //}
                    byte[] tmp = Encoding.Default.GetBytes(sData);
                    int nTextSize = tmp.Length * 2;

                    byte[] DData = new byte[3 + 2 + 12 + nTextSize + 2];
                    for (i = 0; i < DData.Length; i++)
                        DData[i] = 0x00;

                    DData[0] = 0x10; // DLE
                    DData[1] = 0x02; // STX
                    DData[2] = 0x00;
                    DData[3] = 0x00;
                    DData[4] = (byte)(12 + nTextSize + 2);
                    DData[5] = (byte)CmdType;
                    DData[6] = 0x00;
                    DData[7] = 0x01;  // 표시방법
                    DData[8] = (byte)m_SaveType;
                    DData[9] = (byte)(3 | 4 | 16);
                    if (SendType == enumSendType.stEach) DData[9] |= 64;
                    DData[10] = 0x00;
                    DData[11] = 0x00;
                    DData[12] = 0x80;
                    DData[13] = (byte)(DisplayEffect);
                    DData[14] = (byte)DisplaySpeed;
                    DData[15] = (byte)DisplayTime;
                    DData[16] = (byte)(DisplayPosition == enumDisplayPosition.dpBottom ? 1 : 0);

                    int nPos = 17;
                    nTextSize /= 2;
                    for (i = 0; i < nTextSize; i++)
                    {
                        switch (DisplayPosition)
                        {
                            case enumDisplayPosition.dpTop: DData[nPos++] = (byte)TopMessageColor; break;
                            case enumDisplayPosition.dpBottom: DData[nPos++] = (byte)BottomMessageColor; break;
                            case enumDisplayPosition.dpAll: DData[nPos++] = (byte)(i > 11 ? BottomMessageColor : TopMessageColor); break;
                        }
                    }

                    for (i = 0; i < nTextSize; i++)
                        DData[nPos++] = tmp[i];

                    DData[nPos++] = 0x10;
                    DData[nPos++] = 0x03;

                    nLen = 12 + (nTextSize * 2);
                    if (nLen > 255)
                    {
                        DData[3] = (byte)(nLen % 255);
                        DData[4] = 255;
                    }
                    else
                    {
                        DData[3] = 0;
                        DData[4] = (byte)nLen;
                    }
                    return DData;
                }
                catch
                {
                    return null;
                }
            }





            private string SetLeftAlign(string sMsg, int nSize)
            {
                string sResult, t = "";

                int nLen = Encoding.Default.GetByteCount(sMsg);
                if (nLen < nSize)
                {
                    for (int i = 0; i < nSize - nLen; i++)
                        t += " ";
                    sResult = sMsg + t;
                }
                else
                {
                    int nPos = nSize - 1;
                    sResult = sMsg;
                }

                return sResult;
            }

            private string SetRightAlign(string sMsg, int nSize)
            {
                string sResult, t = "";

                int nLen = Encoding.Default.GetByteCount(sMsg);
                if (nLen < nSize)
                {
                    for (int i = 0; i < nSize - nLen; i++)
                        t += " ";
                    sResult = t + sMsg;
                }
                else
                {
                    int nPos = nSize - 1;
                    sResult = sMsg;
                }

                return sResult;
            }


    }
}
