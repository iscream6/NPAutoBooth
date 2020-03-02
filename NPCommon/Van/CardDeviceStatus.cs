namespace NPCommon.Van
{
    public class CardDeviceStatus
    {
        public enum CardReaderStatus
        {
            /// <summary>
            /// 아무작업없음(결제요청전이나 결제취소일떄)
            /// </summary>
            None,
            /// <summary>
            /// 결제대기
            /// </summary>
            CardReady,
            MsReady,
            //KOCSE 카드리더기 추가
            /// <summary>
            /// 카드가 정상적으로 들어간 상태
            /// </summary>
            CARDINSERTED,
            //KOCSE 카드리더기 추가 주석
            /// <summary>
            /// 카드를 고객이 넣음 성공이든 실패든...OnreceiveState 'CK'
            /// </summary>
            CardReadyEnd,
            /// <summary>
            /// 카드결제 성공
            /// </summary>
            CardPaySuccess,
            /// <summary>
            /// 카드결제실패
            /// </summary>
            CardPayFail,
            /// <summary>
            /// 카드결제취소
            /// </summary>
            CardPayCancle,
            CardStop,
            //KIS 할인처리시 처리문제
            /// <summary>
            /// 할인등으로 카드장비에 요금을 취소시 정상적으로 처리됐을때 플레그
            /// </summary>
            CardInitailizeSuccess,
            //KIS 할인처리시 처리문제주석완료
            CardSoundPlay,
            //스마트로 TIT_DIP EV-CAT 적용
            // 스마트로 tit dip type에서 요금취소 및 배출처리했을시 상태값
            CardReset
            //스마트로 TIT_DIP EV-CAT 적용
            ,
            //KIS 삼성페이 결제 적용
            CardPowerCheck,
            CardLockEject,
            CardFullBack,
            CardApproval,
            CardStatusCheckFinish,
            CardLockEjectFinish
            //KIS 삼성페이 결제 적용완료
        }
        public CardReaderStatus currentCardReaderStatus = CardReaderStatus.None;
    }
}
