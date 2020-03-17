using FadeFox.Text;
using NPCommon.DEVICE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPAutoBooth.UI
{
    partial class FormCreditPaymentMenu
    {
        // 신분증인식기 적용
        private void SinbunProcess(SinBunReader.CardInfo info)
        {
            try
            {
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu | SinbunProcess", "카드정보 들어옴 " + SinBunReader.CardInfo.GetCardTypeText(info.CardType));
                if (info.DiscountType == SinBunReader.DiscountType.BoHun || info.DiscountType == SinBunReader.DiscountType.BokJi)
                {
                    // 보훈카드는 8자리, 10자리 보훈번호가 존재하여 체크
                    if (info.DiscountType == SinBunReader.DiscountType.BoHun && !(info.BohunNum.Length == 8 || info.BohunNum.Length == 10))
                    {
                        return;
                    }

                    string type = ((int)info.DiscountType).ToString();              // 할인타입
                    string grade = info.Grade;                                      // 장애/보훈 등급
                    //string dcNo = LPRDbSelect.GetReductionDiscount(type, grade);    // 할인코드
                    //string feeNo = mDIscountTicketOcs.GetChangeFeeNo(dcNo);
                    //DIscountTicketOcs.TIcketReadingResult l_TIcketReadingResult = DIscountTicketOcs.TIcketReadingResult.NotTicket;
                    //TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu | SinbunProcess", "감면대상 확인 감면할인코드 : " + dcNo + " 변경요금코드 : " + feeNo);

                    //if (!string.IsNullOrEmpty(dcNo))
                    //{
                    //    if (mNormalCarInfo.ListDcDetail.Count > 0)
                    //    {
                    //        List<DcDetail> SortDiscount = new List<DcDetail>(mNormalCarInfo.ListDcDetail);

                    //        // 요금변경할인 우선 처리를 위해 먼저 넣어줌 순서 요금변경 -> 시간 -> 퍼센트(나머지)
                    //        SortDiscount = mNormalCarInfo.ListDcDetail.FindAll(x => x.DCType == "0");
                    //        // 이후 시간할인권 처리를 위해 넣어줌
                    //        SortDiscount.AddRange(mNormalCarInfo.ListDcDetail.FindAll(x => x.DCType == "1"));
                    //        // 나머지 할인권을 넣는다.
                    //        SortDiscount.AddRange(mNormalCarInfo.ListDcDetail.FindAll(x => x.DCType != "0" && x.DCType != "1"));
                    //        // 들어온 감면할인이 요금변경일 경우
                    //        if (feeNo != "0")
                    //        {
                    //            string preDcNo = string.Empty;
                    //            string preFeeNo = string.Empty;

                    //            foreach (DcDetail dcInfo in SortDiscount)
                    //            {
                    //                preFeeNo = mDIscountTicketOcs.GetChangeFeeNo(dcInfo.DcNo);

                    //                if (preFeeNo != "0" && preFeeNo != feeNo)
                    //                {
                    //                    preDcNo = dcInfo.DcNo;

                    //                    int parkMoney = FeeAction.FeeCalcMoney(Convert.ToInt32(NPSYS.ParkCode), Convert.ToInt32(feeNo), mNormalCarInfo.InYMD, mNormalCarInfo.InHMS, mNormalCarInfo.OutYmd, mNormalCarInfo.OutHms);
                    //                    if (parkMoney < mNormalCarInfo.ParkMoney)
                    //                    {
                    //                        SortDiscount.Remove(SortDiscount.Find(x => x.DcNo == preDcNo));
                    //                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SinbunProcess", "이전 변경요금제보다 할인율이 높으므로 요금변경처리 이전 요금변경 할인코드 : " + preDcNo);
                    //                        break;
                    //                    }
                    //                    else
                    //                    {
                    //                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SinbunProcess", "이전 할인요금제보다 할인율이 낮으므로 처리안함");
                    //                        return;
                    //                    }
                    //                }
                    //            }
                    //        }

                    //        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SinbunProcess", "기존 할인권 정리 후 감면할인 적용");
                    //        //모든할인 취소 처리
                    //        //할인권 입수수량 표출
                    //        lblDiscountInputCount.Text = "0";
                    //        //할인권 입수수량 표출 주석완료

                    //        //모든 할인처리 리셋
                    //        mNormalCarInfo.DiscountMoney = 0;
                    //        mNormalCarInfo.TotalDiscountTime = 0;
                    //        mNormalCarInfo.SumPreTotalDisocuntTime = 0;
                    //        mNormalCarInfo.ListDcDetail.Clear();
                    //        SaleTicketUsePermissions.clear();

                    //        //감면할인 포함하여 할인 재적용
                    //        if (feeNo != "0")
                    //        {
                    //            l_TIcketReadingResult = mDIscountTicketOcs.DiscountTIcket(DIscountTicketOcs.DIscountTicketType.ReductionDiscount, dcNo, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);
                    //            foreach (DcDetail dcInfo in SortDiscount)
                    //            {
                    //                DIscountTicketOcs.DIscountTicketType currentTicketType = (DIscountTicketOcs.DIscountTicketType)Enum.Parse(typeof(DIscountTicketOcs.DIscountTicketType), dcInfo.Reserve4);
                    //                l_TIcketReadingResult = mDIscountTicketOcs.DiscountTIcket(currentTicketType, dcInfo.DcTkNO, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);

                    //                if (l_TIcketReadingResult == DIscountTicketOcs.TIcketReadingResult.Success)
                    //                {
                    //                    //할인권 입수수량 표출
                    //                    lblDiscountInputCount.Text = (Convert.ToInt32(lblDiscountInputCount.Text) + 1).ToString();
                    //                    //할인권 입수수량 표출 주석완료
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            foreach (DcDetail dcInfo in SortDiscount)
                    //            {
                    //                DIscountTicketOcs.DIscountTicketType currentTicketType = (DIscountTicketOcs.DIscountTicketType)Enum.Parse(typeof(DIscountTicketOcs.DIscountTicketType), dcInfo.Reserve4);
                    //                l_TIcketReadingResult = mDIscountTicketOcs.DiscountTIcket(currentTicketType, dcInfo.DcTkNO, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);

                    //                if (l_TIcketReadingResult == DIscountTicketOcs.TIcketReadingResult.Success)
                    //                {
                    //                    //할인권 입수수량 표출
                    //                    lblDiscountInputCount.Text = (Convert.ToInt32(lblDiscountInputCount.Text) + 1).ToString();
                    //                    //할인권 입수수량 표출 주석완료
                    //                }
                    //            }
                    //            l_TIcketReadingResult = mDIscountTicketOcs.DiscountTIcket(DIscountTicketOcs.DIscountTicketType.ReductionDiscount, dcNo, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);
                    //        }

                    //        //else
                    //        //{
                    //        //    l_TIcketReadingResult = mDIscountTicketOcs.DiscountTIcket(DIscountTicketOcs.DIscountTicketType.ReductionDiscount, dcNo, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);
                    //        //    if (l_TIcketReadingResult == DIscountTicketOcs.TIcketReadingResult.Success)
                    //        //    {
                    //        //        //할인권 입수수량 표출
                    //        //        lblDiscountInputCount.Text = (Convert.ToInt32(lblDiscountInputCount.Text) + 1).ToString();
                    //        //        //할인권 입수수량 표출 주석완료
                    //        //    }
                    //        //}
                    //    }
                    //    else
                    //    {
                    //        l_TIcketReadingResult = mDIscountTicketOcs.DiscountTIcket(DIscountTicketOcs.DIscountTicketType.ReductionDiscount, dcNo, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);
                    //        if (feeNo != "0")
                    //        {
                    //            if (l_TIcketReadingResult == DIscountTicketOcs.TIcketReadingResult.Success)
                    //            {
                    //                //할인권 입수수량 표출
                    //                lblDiscountInputCount.Text = (Convert.ToInt32(lblDiscountInputCount.Text) + 1).ToString();
                    //                //할인권 입수수량 표출 주석완료
                    //            }
                    //        }
                    //    }

                    //    if (l_TIcketReadingResult == DIscountTicketOcs.TIcketReadingResult.Success) // 정상적인 티켓이라면
                    //    {
                    //        TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu | SinbunProcess", "감면대상 할인처리 완료");

                    //        //요금할인권처리
                    //        paymentControl.ParkingFee = TextCore.ToCommaString(mNormalCarInfo.ParkMoney.ToString()) ;
                    //        //요금할인권처리 주석완료
                    //        paymentControl.Payment = TextCore.ToCommaString(mNormalCarInfo.PaymentMoney) ;
                    //        JungangPangDisplay();
                    //        if (mNormalCarInfo.PaymentMoney == 0)
                    //        {
                    //            Payment();
                    //            return;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SinbunProcess", "감면대상 할인코드가 없음");
                    //}
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormCreditPaymentMenu | SinbunProcess ", "예외상황 : " + ex.ToString());
            }
        }

        private void btnGamMyunTestDiscount_Click(object sender, EventArgs e)
        {
            SinBunReader.CardInfo info = new SinBunReader.CardInfo();

            switch (cbGamMyeonItem.SelectedIndex)
            {
                case 0:
                    info.DiscountType = SinBunReader.DiscountType.BoHun;
                    info.BohunNum = "12345678";
                    break;
                case 1:
                    info.DiscountType = SinBunReader.DiscountType.BokJi;
                    info.Grade = "1";
                    break;
                case 2:
                    info.DiscountType = SinBunReader.DiscountType.BokJi;
                    info.Grade = "5";
                    break;
            }
        }
        // 신분증인식기 적용완료
    }
}
