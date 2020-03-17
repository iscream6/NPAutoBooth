using FadeFox.Text;
using NPCommon;
using NPCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPAutoBooth.UI
{
    partial class FormCreditPaymentMenu
    {
        #region 각종 장비들및 결재 방식에 따른 동영상및 문구

        private string m_DiscountAndPayAndCreditAndTMoneyMovie = "할인권_현금_신용카드_교통카드.avi";
        private string m_PayAndCreditAndTMoneyMovie = "현금_신용카드_교통카드.avi";
        private string m_DiscountAndCreditAndTMoneyMovie = "할인권_신용카드_교통카드.avi";
        private string m_CreditAndTmoneyMovie = "신용카드_교통카드.avi";
        private string m_DiscountAndPayAndCreditMovie = "할인권_현금_신용카드.avi";
        private string m_PayAndCreditMovie = "현금_신용카드.avi";
        private string m_DiscountAndPayMovie = "할인권_현금.avi";
        private string m_PayMovie = "현금.avi";
        private string m_CreditMovie = "신용카드.avi";
        private string m_DiscountAndCreditMovie = "할인권_신용카드.avi";
        private string m_DiscountAndTmoneyMovie = "할인권_교통카드.avi";
        private string m_TmoneyMovie = "교통카드.avi";
        private string m_DiscountMovie = "할인권.avi";
        private string m_JuminDIscountMovie = "감면혜택.wav";
        private string m_Junggi = "정기권연장.wav";
        private string m_CancleCard = "카드취소.wav";
        private string m_DiscountBarcodeCreditMovie = "할인권바코드_신용카드.wav";
        //영수증할인문구 적용
        private string m_BarAndDiscountAndCreditAndPayAndTMoneyMovie = "바코드_할인권_신용카드_현금_교통카드.wav";
        private string m_BarAndCreditAndPayAndTMoneyMovie = "바코드_신용카드_현금_교통카드.wav";
        private string m_BarAndDiscountAndPayAndTMoneyMovie = "바코드_할인권_현금_교통카드.wav";
        private string m_BarAndPayAndTMoneyMovie = "바코드_현금_교통카드.wav";
        private string m_BarAndDiscountAndCreditAndPayMovie = "바코드_할인권_신용카드_현금.wav";
        private string m_BarAndCreditAndPayMovie = "바코드_신용카드_현금.wav";
        private string m_BarAndDiscountAndPayMovie = "바코드_할인권_현금.wav";
        private string m_BarAndPayMovie = "바코드_현금.wav";
        private string m_BarAndDiscountAndCreditAndTMoneyMovie = "바코드_할인권_신용카드_교통카드.wav";
        private string m_BarAndCreditAndTMoneyMovie = "바코드_신용카드_교통카드.wav";
        private string m_BarAndDiscountAndTMoneyMovie = "바코드_할인권_교통카드.wav";
        private string m_BarAndDiscountAndCreditMovie = "바코드_할인권_신용카드.wav";
        private string m_BarAndCreditMovie = "바코드_신용카드.wav";
        private string m_BarAndTMoneyMovie = "바코드_교통카드.wav";
        //영수증할인문구 적용완료

        private void MovieTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (IsNextFormPlaying == false)
                {
                    MovieStopPlay -= 1000;
                }
                if (MovieStopPlay == 0 && IsNextFormPlaying == false)
                {
                    //if (mIsPlayerOkStatus == true)
                    //{
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    //}
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|MovieTimer_Tick", "예외사항:" + ex.ToString());
            }
        }

        #region 동영상 관련 이벤트처리

        void Player_MediaError(object sender, AxWMPLib._WMPOCXEvents_MediaErrorEvent e)
        {
            try
            {
                mIsPlayerOkStatus = false;
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|Player_MediaError", "플레이어오류");
            }
            catch (Exception ex)
            {
                mIsPlayerOkStatus = false;
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|Player_MediaError", ex.ToString());
            }
        }

        void player_ErrorEvent(object sender, System.EventArgs e)
        {
            try
            {
                mIsPlayerOkStatus = false;
                string errDesc = axWindowsMediaPlayer1.Error.get_Item(0).errorDescription;

                // Display the error description.
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|player_ErrorEvent", "에러내용" + errDesc);
            }
            catch (Exception ex)
            {
                mIsPlayerOkStatus = false;
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|player_ErrorEvent", ex.ToString());
            }
        }

        void Player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            try
            {
                if ((WMPLib.WMPPlayState)e.newState == WMPLib.WMPPlayState.wmppsStopped)
                {
                    MovieStopPlay = NPSYS.SettingMoviePlayTimeValue;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|Player_PlayStateChange", ex.ToString());
            }
        }
        #endregion

        //영수증바코드문구 적용
        public void Action_DeviceEnableMovie()
        {
            if (mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                || mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
            {
                playVideo(m_Junggi);
                return;
            }
            else if (mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                   || mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
            {
                playVideo(m_CancleCard);
                return;
            }

            if (NPSYS.IsUsedCashPay() == true && (NPSYS.Device.gIsUseCreditCardDevice || NPSYS.Device.gIsUseMagneticReaderDevice) == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == true) // 현금,카드리더기,교통카드 장비 정상
            {
                //635, 419
                if (NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None)  // 할인권, 현금, 신용카드 , 교통카드
                {

                    playVideo(m_DiscountAndPayAndCreditAndTMoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 할인권, 현금 , 교통카드(동영상없음)
                {

                    playVideo(m_DiscountAndPayMovie);

                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) //현금, 신용카드 , 교통카드
                {
                    playVideo(m_PayAndCreditAndTMoneyMovie);
                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 현금 , 교통카드(동영상 없음)
                {
                    playVideo(m_PayMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // [현금, 교통카드], 바코드, 할인권, 신용카드
                {
                    //바코드, 할인권, 신용카드, 현금, 교통카드
                    playVideo(m_BarAndDiscountAndCreditAndPayAndTMoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && !NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // [현금, 교통카드], 바코드, 신용카드
                {
                    // 바코드, 신용카드, 현금, 교통카드
                    playVideo(m_BarAndCreditAndPayAndTMoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard) // [현금, 교통카드], 바코드, 할인권
                {
                    // 바코드, 할인권, 현금, 교통카드
                    playVideo(m_BarAndDiscountAndPayAndTMoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && !NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard) // [현금, 교통카드], 바코드
                {
                    // 바코드, 현금, 교통카드
                    playVideo(m_BarAndPayAndTMoneyMovie);
                }
            }
            if (NPSYS.IsUsedCashPay() == true && (NPSYS.Device.gIsUseCreditCardDevice || NPSYS.Device.gIsUseMagneticReaderDevice) == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == false) // 현금,카드리더기 정상
            {
                //635, 419
                if (NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None)// 할인권 , 현금, 신용카드 
                {

                    playVideo(m_DiscountAndPayAndCreditMovie);

                }
                else if (NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 할인권 ,현금
                {

                    playVideo(m_DiscountAndPayMovie);

                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 현금 , 신용카드
                {
                    playVideo(m_PayAndCreditMovie);
                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 현금
                {
                    playVideo(m_PayMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // [현금] 바코드, 할인권, 신용카드
                {
                    // 바코드, 할인권, 신용카드, 현금
                    playVideo(m_BarAndDiscountAndCreditAndPayMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && !NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // [현금] 바코드, 신용카드
                {
                    // 바코드, 신용카드, 현금
                    playVideo(m_BarAndCreditAndPayMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard) // [현금] 바코드, 할인권
                {
                    // 바코드, 할인권, 현금
                    playVideo(m_BarAndDiscountAndPayMovie);
                }
            }
            if (NPSYS.IsUsedCashPay() == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == false && (NPSYS.Device.gIsUseCreditCardDevice || NPSYS.Device.gIsUseMagneticReaderDevice) == false) // 현금 장비 정상
            {
                if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None)
                {
                    playVideo(m_PayMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None)
                {
                    // 바코드, 현금
                    playVideo(m_BarAndPayMovie);
                }
            }
            if (NPSYS.IsUsedCashPay() == false && (NPSYS.Device.gIsUseCreditCardDevice || NPSYS.Device.gIsUseMagneticReaderDevice) == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == true) // 카드리더기 , 교통카드리더기 정상
            {
                if (NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) //  할인권, 신용카드, 교통카드 
                {

                    playVideo(m_DiscountAndCreditAndTMoneyMovie);

                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 신용카드, 교통카드  
                {
                    playVideo(m_CreditAndTmoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 할인권 , 교통카드
                {

                    playVideo(m_DiscountAndTmoneyMovie);

                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 교통카드
                {
                    playVideo(m_TmoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // [교통카드] 바코드, 할인권, 신용카드
                {
                    // 바코드, 할인권, 신용카드, 교통카드
                    playVideo(m_BarAndDiscountAndCreditAndTMoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && !NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // [교통카드] 바코드, 신용카드
                {
                    // 바코드, 신용카드, 교통카드
                    playVideo(m_BarAndCreditAndTMoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard) // [교통카드] 바코드, 할인권
                {
                    // 바코드, 할인권, 교통카드
                    playVideo(m_BarAndDiscountAndTMoneyMovie);
                }
            }
            if (NPSYS.IsUsedCashPay() == false && (NPSYS.Device.gIsUseCreditCardDevice || NPSYS.Device.gIsUseMagneticReaderDevice) == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == false) // 카드리더기 정상
            {
                //635, 456
                if (NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 할인권, 신용카드
                {

                    playVideo(m_DiscountAndCreditMovie);

                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 신용카드
                {
                    playVideo(m_CreditMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 할인권
                {
                    playVideo(m_DiscountMovie);
                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 암것도 안됨
                {
                    playVideo(m_CreditMovie);
                }
                //할인권 타입설정 추가
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingBarcodeDCTicket) // 바코드할인권, 신용카드
                {
                    // 바코드할인권, 신용카드
                    playVideo(m_DiscountBarcodeCreditMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // 바코드, 할인권, 신용카드
                {
                    // 바코드, 할인권, 신용카드
                    playVideo(m_BarAndDiscountAndCreditMovie);
                }
                //할인권 타입설정 추가완료
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && !NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // 바코드, 신용카드
                {
                    // 바코드, 신용카드
                    playVideo(m_BarAndCreditMovie);
                }
            }
            if (NPSYS.IsUsedCashPay() == false && (NPSYS.Device.gIsUseCreditCardDevice || NPSYS.Device.gIsUseMagneticReaderDevice) == false && NPSYS.Device.isUseDeviceTMoneyReaderDevice == true)
            {
                //635, 456
                if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None)
                {
                    playVideo(m_TmoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None) // 바코드, 교통카드
                {
                    // 바코드, 교통카드
                    playVideo(m_BarAndTMoneyMovie);
                }
            }
        }
        //영수증바코드문구 적용완료
        //바코드모터드리블 사용완료
        #endregion

        private void stopVideo()
        {
            try
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | playVideo", "예외사항:" + ex.ToString());
            }
        }

        private void PausePlayVideo()
        {
            try
            {
                //if (mIsPlayerOkStatus == false)
                //{
                //    return;
                //}

                inputTimer.Enabled = false;
                inputtime = paymentInputTimer;
                MovieTimer.Enabled = false;
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                IsNextFormPlaying = true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|StartPlayVideo", "예외사항:" + ex.ToString());
            }
        }

        private void StartPlayVideo()
        {
            try
            {
                //if (mIsPlayerOkStatus == true)
                //{
                axWindowsMediaPlayer1.Ctlcontrols.play();
                //}

                inputTimer.Enabled = true;
                MovieTimer.Enabled = true;
                IsNextFormPlaying = false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|StartPlayVideo", "예외사항:" + ex.ToString());
            }
        }
    }
}
