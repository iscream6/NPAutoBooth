namespace NPCommon
{
    public class dynamictype
    {
        public enum HEADER
        {
            DY_TXT_MONEY_2TYPENAME,
            DY_TXT_MONEY_3TYPENAME,
            DY_TXT_MONEY_4TYPENAME,
            DY_TXT_MONEY_5TYPENAME,
            DY_TXT_MONEY_6TYPENAME,
            DY_MSG_TXT_NO,
            DY_MSG_CLOSE_COMPLETE,
            DY_MSG_TXT_UNKNOWNERR,
            DY_MSG_TXT_ENTER,
            DY_MSG_ERR_CLOSE_DUPLICATE,
            DY_MSG_ERR_CLOSE_NOTDEVICE,
            DY_MSG_ERR_CLOSE_NOTFOUND,
            DY_MSG_EXIT_TOP,
            DY_MSG_EXIT_BOTTOM,
            DY_MSG_ERROREXIT_TOP,
            DY_MSG_ERROREXIT_BOTTOM,
            DY_MACHINNAME1,
            DY_MACHINNAME2,
            DY_FARE_COMPLETCAR,
            DY_FARE_NOTCARSEARCH,
            DY_FARE_SERVICECAR,
            DY_PAY_COMMUTER,
            DY_FARE_KAKAOMEMBER,
            DY_FARE_NOREGEXTENSCAR,
            DY_FARE_NOREGEXTENSDISCOUNT,
            DY_FARE_ADDPAYMENT1,
            DY_FARE_ADDPAYMENT2,
            DY_FARE_NOTENOGHFMONEY1,
            DY_FARE_NOTENOGHFMONEY2,
            DY_FARE_CORRECTCARD,
            DY_FARE_NOTOUTCARD,
            DY_FARE_NOTCARDPAY,
            DY_FARE_NODISCOUNTTICEKT,
            DY_FARE_DUPLICATEDISCOUNTTICKET,
            DY_FARE_NOADDDISCOUNTTICKET,
            DY_FARE_NOBARCODE,
            DY_FARE_DUPLICATEBARCODE,
            DY_FARE_EXITBOOTH, // 필피핀언어추가필요
            DY_FARE_GCASH_FAIL,
            DY_FARE_GCASH_SUCCESS,
            DY_FARE_TOUCHK_RFREADER,
            DY_Q_PAYMININFO1,
            DY_Q_PAYMININFO2,
            DY_Q_FREECAR1,
            DY_Q_FREECAR2,
            DY_Q_CLOSING,
            DY_Q_CURRENTSET,



            DY_PAY_BARCODE,
            DY_PAY_RECEIPT,
            DY_PAY_MAGNETIC,
            DY_PAY_RECEIPT_MAGNETIC,
            DY_PAY_BARCODE_MAGNETIC,
            DY_PAY_CREDIT,
            DY_PAY_MONEY_CREDIT,
            DY_PAY_MONEY_TMONEY,
            DY_PAY_MONEY_CREDIT_TMONEY,
            DY_PAY_CREDIT_TMONEY,
            DY_PAY_TMONEY,
            DY_PAY_MONEY,

            DY_PAY_GCASH_INFO,
            DY_PAY_MONEY_GCASH,
            /// <summary>
            /// 주차요금
            /// </summary>
            DY_UNIT_PARKINGFEE,

            ///결제요금
            DY_UNIT_AMOUNTFEE,
            /// <summary>
            /// 입차일자
            /// </summary>
            DY_UNIT_INDATE,
            /// <summary>
            /// 경과시간
            /// </summary>
            DY_UNIT_ELAPSEDTIME,
            /// <summary>
            /// 카드취소요금
            /// </summary>
            DY_UNIT_CANCELFEE,
            /// <summary>
            /// 기존정산시간
            /// </summary>
            DY_UNIT_PREPAYTIME,
            /// <summary>
            /// 연장요금
            /// </summary>
            DY_UNIT_TERMFEE,
            /// <summary>
            /// 현재만료일
            /// </summary>
            DY_UNIT_CURTERMDATE,
            /// <summary>
            /// 연장가능일
            /// </summary>
            DY_UNIT_NEXTTERMDATE,
            /// <summary>
            /// 총할인요금
            /// </summary>
            DY_UNIT_DISCOUNTFEE,
            /// <summary>
            /// 투입금액
            /// </summary>
            DY_UNIT_COIN_PUT_IN,
            /// <summary>
            /// 사전정산
            /// </summary>
            DY_UNIT_PREPAYD,
            /// <summary>
            /// 주차장명
            /// </summary>
            DY_UNIT_PARKNAME,
            /// <summary>
            /// 조소
            /// </summary>
            DY_UNIT_ADDRESS,
            /// <summary>
            /// 전화번호
            /// </summary>
            DY_UNIT_TELNO,
            /// <summary>
            /// 지불방식
            /// </summary>
            DY_UNIT_PAYTYPE,
            /// <summary>
            /// 결제일시
            /// </summary>
            DY_UNIT_PAYDATE,
            /// <summary>
            /// 이용해 주셔서 감사합니다
            /// </summary>
            DY_UNIT_THANKYOU_PAY,
            /// <summary>
            /// 주차요금 영수증
            /// </summary>
            DY_UNIT_PARKING_FEE_RECEIPT,
            /// <summary>
            /// 정산기명
            /// </summary>
            DY_UNIT_UNITNAME,
            /// <summary>
            /// 차량번호
            /// </summary>
            DY_UNIT_CARNO,
            /// <summary>
            /// 출구(일반)
            /// </summary>
            DY_UNIT_MACHINNAME1,
            /// <summary>
            /// 사전(일반)
            /// </summary>
            DY_UNIT_MACHINNAME2,
            /// <summary>
            /// 현금
            /// </summary>
            DY_UNIT_CASH,
            /// <summary>
            /// 사업자
            /// </summary>
            DY_UNIT_BUSINESSNUMBER,
            /// <summary>
            /// 주차시간
            /// </summary>
            DY_UNIT_PARKINGTIME,
            /// <summary>
            /// 정기권연장 영수증
            /// </summary>
            DY_UNIT_COMMUTER_RECEIPT,
            /// <summary>
            /// 신용카드취소 영수증
            /// </summary>
            DY_UNIT_CARDCANCLE_RECEIPT,
            /// <summary>
            /// Days
            /// </summary>
	        DY_UNIT_DAYS,
            /// <summary>
            /// Hours
            /// </summary>
            DY_UNIT_HOURS,
            /// <summary>
            /// Min
            /// </summary>
            DY_UNIT_MINUTE,
            /// <summary>
            /// 원
            /// </summary>
            DY_UNIT_CASHNAME,
            /// <summary>
            /// 취소일시
            /// </summary>
            DY_UNIT_CANCLE_DATE,
            DY_UNIT_CANCLE_AMOUNT,
            /// <summary>
            /// 연장기간
            /// </summary>
            DY_UNIT_TERM_DATE,
            /// <summary>
            /// 신용카드
            /// </summary>
            DY_UNIT_CREDITCARD,
            /// <summary>
            /// 교통카드
            /// </summary>
            DY_UNIT_TMONEY,
            /// <summary>
            /// 거스름돈
            /// </summary>
            DY_UNIT_CHANGE,
            /// <summary>
            /// 비고
            /// </summary>
            DY_UNIT_REMARK,
            /// <summary>
            /// 거스름돈이 부족합니다.
            /// </summary>
            DY_TXT_LACKOFCHANGE,
            /// <summary>
            /// 관리자에게 문의해주세요.
            /// </summary>
            DY_TXT_HELP_MANAGER,
            /// <summary>
            /// 거스름돈 부족
            /// </summary>
            DY_UNIT_LACKOFCHANGE,
            /// <summary>
            /// 현금 보관증
            /// </summary>
            DY_UNIT_CASH_DEPOSIT,
            /// <summary>
            /// 거스름돈 부족액
            /// </summary>
            DY_UNIT_SHORT_CHANGE,
            /// <summary>
            /// 결제후 거스름돈 부족
            /// </summary>
            DY_UNIT_LACKOFCHANGE_AFTER_PAYMENT,
            /// <summary>
            /// 투입금액 반환
            /// </summary>
            DY_UNIT_RETURN_INPUT_AMOUNT,

            /// <summary>
            /// 마감기간
            /// </summary>
            DY_CLOSE_CLOSING_PREIOD,
            /// <summary>
            ///  마감
            /// </summary>
            DY_CLOSE_CLOSING,
            /// <summary>
            /// 마감내역확인
            /// </summary>
            DY_CLOSE_CLOSING_DETAIL,
            /// <summary>
            /// 마감유무
            /// </summary>
            DY_CLOSE_CLOSING_STATUS,
            /// <summary>
            /// 무인정산기 내역
            /// </summary>
            DY_CLOSE_AUTOSTATION_DETAIL,
            /// <summary>
            /// 차종류별 출차내역
            /// </summary>
            DY_CLOSE_DETAUL_VEHICLEL,
            /// <summary>
            /// 구       분
            /// </summary>
            DY_CLOSE_KIND,
            /// <summary>
            /// 수    량
            /// </summary>
            DY_CLOSE_QUANTITY,
            ///금    액
            DY_CLOSE_AMOUNT,
            // 합계
            DY_CLOSE_TOTAL,
            ///일 반 차 량
            DY_CLOSE_NORMALCAR,
            /// <summary>
            /// 회 차 차 량
            /// </summary>
            DY_CLOSE_FREECAR,
            /// <summary>
            /// 정 기 차 량
            /// </summary>
            DY_CLOSE_COMMUTERCAR,
            /// <summary>
            /// 현       금
            /// </summary>
            DY_CLOSE_CASH,
            /// <summary>
            /// 교 통 카 드
            /// </summary>
            DY_CLOSE_TMOEY,
            /// <summary>
            /// 신 용 카 드
            /// </summary>
            DY_CLOSE_CREDITCARD,
            /// <summary>
            /// 할       인
            /// </summary>
            DY_CLOSE_DISCOUNT,
            /// <summary>
            /// 현금입금
            /// </summary>
            DY_CLOSE_DESPOSIT,
            /// <summary>
            /// 현금출금
            /// </summary>
            DY_CLOSE_WITHROW,
            /// <summary>
            /// 수입금 내역
            /// </summary>
            DY_CLOSE_INCOME_DETAIL,
            /// <summary>
            /// 현 보유현금애녁
            /// </summary>
            DY_CLOSE_CURRENT_CASH_RESERVE,
            DY_CLOSE_PRINTDATE,


            DY_TXT_CLOSE_PROGRAM,
            DY_TXT_WEBFAIL_OPERATION,
            DY_TXT_DEVICE_NONE,
            DY_TXT_DEVICE_ERROR,
            DY_TXT_DEVICE_OK,

            DY_WAV_SearchCarNumber,
            DY_WAV_SelectCarNumber,
            DY_WAV_ReceitPrintInfo,
            DY_WAV_Discount_Cash_Card_Tmoney,
            DY_WAV_Cash_Card_Tmoney,
            DY_WAV_Discount_Card_Tmoney,
            DY_WAV_Card_Tmoney,
            DY_WAV_Discount_Cash_Card,
            DY_WAV_Cash_Card,
            DY_WAV_Discount_Cash,
            DY_WAV_Cash,
            DY_WAV_Card,
            DY_WAV_Discount_Card,
            DY_WAV_Discount_Tmoney,
            DY_WAV_Tmoney,
            DY_WAV_Discount,
            DY_WAV_DiscountReduce,
            DY_WAV_CommuterExtendedPeriod,
            DY_WAV_CardCancle,
            DY_WAV_DiscountBarcode_Card,
            DY_WAV_DBarcode_Discount_Card_Cash_Tmoney,
            DY_WAV_DBarcode_Card_Cash_Tmoney,
            DY_WAV_DBarcode_Discount_Cash_Tmoney,
            DY_WAV_DBarcode_Cash_Tmoney,
            DY_WAV_DBarcode_Discount_Card_Cash,
            DY_WAV_DBarcode_Card_Cash,
            DY_WAV_DBarcode_Discount_Cash,
            DY_WAV_DBarcode_Cash,
            DY_WAV_DBarcode_Discount_Card_Tmoney,
            DY_WAV_DBarcode_Card_Tmoney,
            DY_WAV_DBarcode_Discount_Tmoney,
            DY_WAV_DBarcode_Discount_Card,
            DY_WAV_DBarcode_Card,
            DY_WAV_DBarcode_Tmoney,
            DY_WAV_NotCarSearch,
            DY_WAV_SeviceCar,
            DY_WAV_Commuter,
            DY_WAV_AddPayment,
            DY_WAV_NotEnoghfMoney,
            DY_WAV_CorrectCard,
            DY_WAV_NotOutCard,
            DY_WAV_NotCardPay,
            DY_WAV_NoDiscountTicket,
            DY_WAV_DuplicateDiscountTicket,
            DY_WAV_NoAddDiscountTIcket,
            DY_WAV_NoBarcode,
            DY_WAV_DuplicateBarcode,
            DY_WAV_AdVERTISE,
        }


        public string DY_TXT_MONEY_2TYPENAME { set; get; }
        public string DY_TXT_MONEY_3TYPENAME { set; get; }
        public string DY_TXT_MONEY_4TYPENAME { set; get; }
        public string DY_TXT_MONEY_5TYPENAME { set; get; }
        public string DY_TXT_MONEY_6TYPENAME { set; get; }
        public string DY_MSG_TXT_NO { set; get; }
        public string DY_MSG_CLOSE_COMPLETE { set; get; }
        public string DY_MSG_ERR_CLOSE_DUPLICATE { set; get; }
        public string DY_MSG_TXT_UNKNOWNERR { set; get; }
        public string DY_MSG_TXT_ENTER { set; get; }
        public string DY_MSG_ERR_CLOSE_NOTDEVICE { set; get; }
        public string DY_MSG_ERR_CLOSE_NOTFOUND { set; get; }
        public string DY_MSG_EXIT_TOP { set; get; }
        public string DY_MSG_EXIT_BOTTOM { set; get; }
        public string DY_MSG_ERROREXIT_TOP { set; get; }
        public string DY_MSG_ERROREXIT_BOTTOM { set; get; }
        public string DY_MACHINNAME1 { set; get; }
        public string DY_MACHINNAME2 { set; get; }

        /// <summary>
        /// 정산완료차량 안내
        /// </summary>
        public string DY_FARE_COMPLETCAR { set; get; }
        /// <summary>
        /// 차량조회 불가 안내
        /// </summary>
        public string DY_FARE_NOTCARSEARCH { set; get; }
        /// <summary>
        /// 서비스차량 안내
        /// </summary>
        public string DY_FARE_SERVICECAR { set; get; }
        /// <summary>
        /// 정기권차량 안내
        /// </summary>
        public string DY_FARE_COMMUTER { set; get; }
        /// <summary>
        /// 정기권 연장불가차량 안내
        /// </summary>
        public string DY_FARE_NOREGEXTENSCAR { set; get; }
        /// <summary>
        /// 할인권 등으로 정기권 연장불가안내
        /// </summary>
        public string DY_FARE_NOREGEXTENSDISCOUNT { set; get; }
        /// <summary>
        /// 추가요금안내1
        /// </summary>
        public string DY_FARE_ADDPAYMENT1 { set; get; }
        /// <summary>
        /// 추가요금안내2
        /// </summary>
        public string DY_FARE_ADDPAYMENT2 { set; get; }
        /// <summary>
        /// 보관증안내1
        /// </summary>
        public string DY_FARE_NOTENOGHFMONEY1 { set; get; }
        /// <summary>
        /// 보관증 안내2
        /// </summary>
        public string DY_FARE_NOTENOGHFMONEY2 { set; get; }
        /// <summary>
        /// 카드 투입방향 인식불가 안내
        /// </summary>
        public string DY_FARE_CORRECTCARD { set; get; }
        /// <summary>
        /// 카드 방출불가 안내
        /// </summary>
        public string DY_FARE_NOTOUTCARD { set; get; }
        /// <summary>
        /// 카드결제실패안내
        /// </summary>
        public string DY_FARE_NOTCARDPAY { set; get; }
        /// <summary>
        /// 유효하지않은 할인권 안내
        /// </summary>
        public string DY_FARE_NODISCOUNTTICEKT { set; get; }
        /// <summary>
        /// 중복불가 할인권 안내
        /// </summary>
        public string DY_FARE_DUPLICATEDISCOUNTTICKET { set; get; }
        /// <summary>
        /// 중복불가 할인권 안내
        /// </summary>
        public string DY_FARE_NOADDDISCOUNTTICKET { set; get; }
        /// <summary>
        /// 유효하지않은 바코드 안내
        /// </summary>
        public string DY_FARE_NOBARCODE { set; get; }
        /// <summary>
        /// 중복불가 바코드안내
        /// </summary>
        public string DY_FARE_DUPLICATEBARCODE { set; get; }
        public string DY_FARE_EXITBOOTH { set; get; }

        public string DY_FARE_GCASH_FAIL { set; get; }
        public string DY_FARE_GCASH_SUCCESS { set; get; }

        public string DY_FARE_TOUCHK_RFREADER { set; get; }
        /// <summary>
        /// 정산후 몇분이내 출차안내1
        /// </summary>
        public string DY_Q_PAYMININFO1 { set; get; }
        /// <summary>
        /// 정산후 몇분이내 출차안내2
        /// </summary>
        public string DY_Q_PAYMININFO2 { set; get; }
        /// <summary>
        /// 무료차량 출차안내1
        /// </summary>
        public string DY_Q_FREECAR1 { set; get; }
        /// <summary>
        /// 무료차량 출차안내2
        /// </summary>
        public string DY_Q_FREECAR2 { set; get; }


        public string DY_Q_CLOSING { set; get; }
        public string DY_Q_CURRENTSET { set; get; }


        public string DY_PAY_COMMUTER { set; get; }
        public string DY_PAY_BARCODE { set; get; }
        public string DY_PAY_RECEIPT { set; get; }
        public string DY_PAY_MAGNETIC { set; get; }
        public string DY_PAY_RECEIPT_MAGNETIC { set; get; }
        public string DY_PAY_BARCODE_MAGNETIC { set; get; }
        public string DY_PAY_CREDIT { set; get; }
        public string DY_PAY_MONEY_CREDIT { set; get; }
        public string DY_PAY_MONEY_TMONEY { set; get; }
        public string DY_PAY_MONEY_CREDIT_TMONEY { set; get; }
        public string DY_PAY_CREDIT_TMONEY { set; get; }
        public string DY_PAY_TMONEY { set; get; }
        public string DY_PAY_MONEY { set; get; }
        public string DY_PAY_GCASH_INFO { set; get; }
        public string DY_PAY_MONEY_GCASH { set; get; }
        /// <summary>
        /// 주차요금
        /// </summary>
        public string DY_UNIT_PARKINGFEE { set; get; }

        public string DY_UNIT_AMOUNTFEE { set; get; }
        /// <summary>
        /// 입차시간
        /// </summary>
        public string DY_UNIT_INDATE { set; get; }
        /// <summary>
        /// 경과시간
        /// </summary>
        public string DY_UNIT_ELAPSEDTIME { set; get; }

        /// <summary>
        /// 취소요금
        /// </summary>
        public string DY_UNIT_CANCELFEE { set; get; }
        /// <summary>
        /// 기존정산시간
        /// </summary>
        public string DY_UNIT_PREPAYTIME { set; get; }
        /// <summary>
        /// 연장요금
        /// </summary>
        public string DY_UNIT_TERMFEE { set; get; }
        /// <summary>
        /// 현재만료일
        /// </summary>
        public string DY_UNIT_CURTERMDATE { set; get; }
        /// <summary>
        /// 연장가능일
        /// </summary>
        public string DY_UNIT_NEXTTERMDATE { set; get; }

        /// <summary>
        /// 총할인요금
        /// </summary>
        public string DY_UNIT_DISCOUNTFEE { set; get; }
        /// <summary>
        /// 투입금액
        /// </summary>
        public string DY_UNIT_COIN_PUT_IN { set; get; }
        /// <summary>
        /// 사전정산
        /// </summary>
        public string DY_UNIT_PREPAYD { set; get; }
        /// <summary>
        /// 주차장명
        /// </summary>
        public string DY_UNIT_PARKNAME { set; get; }
        /// <summary>
        /// 조소
        /// </summary>
        public string DY_UNIT_ADDRESS { set; get; }
        /// <summary>
        /// 전화번호
        /// </summary>
        public string DY_UNIT_TELNO { set; get; }
        /// <summary>
        /// 지불방식
        /// </summary>
        public string DY_UNIT_PAYTYPE { set; get; }
        /// <summary>
        /// 결제일시
        /// </summary>
        public string DY_UNIT_PAYDATE { set; get; }
        /// <summary>
        /// 이용해 주셔서 감사합니다
        /// </summary>
        public string DY_UNIT_THANKYOU_PAY { set; get; }
        /// <summary>
        /// 주차요금 영수증
        /// </summary>
        public string DY_UNIT_PARKING_FEE_RECEIPT { set; get; }
        /// <summary>
        /// 정산기명
        /// </summary>
        public string DY_UNIT_UNITNAME { set; get; }
        /// <summary>
        /// 차량번호
        /// </summary>
        public string DY_UNIT_CARNO { set; get; }
        /// <summary>
        /// 출구(일반)
        /// </summary>
        public string DY_UNIT_MACHINNAME1 { set; get; }
        /// <summary>
        /// 사전(일반)
        /// </summary>
        public string DY_UNIT_MACHINNAME2 { set; get; }
        /// <summary>
        /// 현금
        /// </summary>
        public string DY_UNIT_CASH { set; get; }
        /// <summary>
        /// 사업자
        /// </summary>
        public string DY_UNIT_BUSINESSNUMBER { set; get; }
        /// <summary>
        /// 주차시간
        /// </summary>
        public string DY_UNIT_PARKINGTIME { set; get; }
        /// <summary>
        /// 정기권연장 영수증
        /// </summary>
        public string DY_UNIT_COMMUTER_RECEIPT { set; get; }
        /// <summary>
        /// 신용카드취소 영수증
        /// </summary>
        public string DY_UNIT_CARDCANCLE_RECEIPT { set; get; }
        /// <summary>
        /// Days
        /// </summary>
        public string DY_UNIT_DAYS { set; get; }
        /// <summary>
        /// Hours
        /// </summary>
        public string DY_UNIT_HOURS { set; get; }
        /// <summary>
        /// Min
        /// </summary>
        public string DY_UNIT_MINUTE { set; get; }
        /// <summary>
        /// 원
        /// </summary>
        public string DY_UNIT_CASHNAME { set; get; }
        /// <summary>
        /// 취소일자
        /// </summary>
        public string DY_UNIT_CANCLE_DATE { set; get; }
        /// <summary>
        /// 취소요금
        /// </summary>
        public string DY_UNIT_CANCLE_AMOUNT { set; get; }
        /// <summary>
        /// 연장기간
        /// </summary>
        public string DY_UNIT_TERM_DATE { set; get; }

        /// <summary>
        /// 신용카드
        /// </summary>
        public string DY_UNIT_CREDITCARD { set; get; }
        /// <summary>
        /// 교통카드
        /// </summary>
        public string DY_UNIT_TMONEY { set; get; }
        /// <summary>
        /// 거스름돈
        /// </summary>
        public string DY_UNIT_CHANGE { set; get; }


        /// <summary>
        /// 비고
        /// </summary>
        public string DY_UNIT_REMARK { set; get; }
        /// <summary>
        /// 거스름돈이 부족합니다.
        /// </summary>
        public string DY_TXT_LACKOFCHANGE { set; get; }
        /// <summary>
        /// 관리자에게 문의해주세요.
        /// </summary>
        public string DY_TXT_HELP_MANAGER { set; get; }
        /// <summary>
        /// 거스름돈 부족
        /// </summary>
        public string DY_UNIT_LACKOFCHANGE { set; get; }
        /// <summary>
        /// 현금 보관증
        /// </summary>
        public string DY_UNIT_CASH_DEPOSIT { set; get; }
        /// <summary>
        /// 거스름돈 부족액
        /// </summary>
        public string DY_UNIT_SHORT_CHANGE { set; get; }
        /// <summary>
        /// 결제후 거스름돈 부족
        /// </summary>
        public string DY_UNIT_LACKOFCHANGE_AFTER_PAYMENT { set; get; }
        /// <summary>
        /// 투입금액 반환
        /// </summary>
        public string DY_UNIT_RETURN_INPUT_AMOUNT { set; get; }

        /// <summary>
        /// 마감기간
        /// </summary>
        public string DY_CLOSE_CLOSING_PREIOD { set; get; }
        /// <summary>
        ///  마감
        /// </summary>
        public string DY_CLOSE_CLOSING { set; get; }
        /// <summary>
        /// 마감내역확인
        /// </summary>
        public string DY_CLOSE_CLOSING_DETAIL { set; get; }
        /// <summary>
        /// 마감유무
        /// </summary>
        public string DY_CLOSE_CLOSING_STATUS { set; get; }
        /// <summary>
        /// 무인정산기 내역
        /// </summary>
        public string DY_CLOSE_AUTOSTATION_DETAIL { set; get; }
        /// <summary>
        /// 차종류별 출차내역
        /// </summary>
        public string DY_CLOSE_DETAUL_VEHICLEL { set; get; }
        /// <summary>
        /// 구       분
        /// </summary>
        public string DY_CLOSE_KIND { set; get; }
        /// <summary>
        /// 수    량
        /// </summary>
        public string DY_CLOSE_QUANTITY { set; get; }
        ///금    액
        public string DY_CLOSE_AMOUNT { set; get; }
        // 합계
        public string DY_CLOSE_TOTAL { set; get; }
        ///일 반 차 량
        public string DY_CLOSE_NORMALCAR { set; get; }
        /// <summary>
        /// 회 차 차 량
        /// </summary>
        public string DY_CLOSE_FREECAR { set; get; }
        /// <summary>
        /// 정 기 차 량
        /// </summary>
        public string DY_CLOSE_COMMUTERCAR { set; get; }
        /// <summary>
        /// 현       금
        /// </summary>
        public string DY_CLOSE_CASH { set; get; }
        /// <summary>
        /// 교 통 카 드
        /// </summary>
        public string DY_CLOSE_TMOEY { set; get; }
        /// <summary>
        /// 신 용 카 드
        /// </summary>
        public string DY_CLOSE_CREDITCARD { set; get; }
        /// <summary>
        /// 할       인
        /// </summary>
        public string DY_CLOSE_DISCOUNT { set; get; }
        /// <summary>
        /// 현금입금
        /// </summary>
        public string DY_CLOSE_DESPOSIT { set; get; }
        /// <summary>
        /// 현금출금
        /// </summary>
        public string DY_CLOSE_WITHROW { set; get; }
        /// <summary>
        /// 수입금 내역
        /// </summary>
        public string DY_CLOSE_INCOME_DETAIL { set; get; }
        /// <summary>
        /// 현 보유현금 내역
        /// </summary>
        public string DY_CLOSE_CURRENT_CASH_RESERVE { set; get; }
        /// <summary>
        /// 출력일시
        /// </summary>
        public string DY_CLOSE_PRINTDATE { set; get; }
        public string DY_TXT_WEBFAIL_OPERATION { set; get; }
        public string DY_TXT_CLOSE_PROGRAM { set; get; }

        public string DY_TXT_DEVICE_NONE { set; get; }

        public string DY_TXT_DEVICE_ERROR { set; get; }

        public string DY_TXT_DEVICE_OK { set; get; }


        public string DY_WAV_SearchCarNumber { set; get; }
        public string DY_WAV_SelectCarNumber { set; get; }
        public string DY_WAV_ReceitPrintInfo { set; get; }
        public string DY_WAV_Discount_Cash_Card_Tmoney { set; get; }
        public string DY_WAV_Cash_Card_Tmoney { set; get; }
        public string DY_WAV_Discount_Card_Tmoney { set; get; }
        public string DY_WAV_Card_Tmoney { set; get; }
        public string DY_WAV_Discount_Cash_Card { set; get; }
        public string DY_WAV_Cash_Card { set; get; }
        public string DY_WAV_Discount_Cash { set; get; }
        public string DY_WAV_Cash { set; get; }
        public string DY_WAV_Card { set; get; }
        public string DY_WAV_Discount_Card { set; get; }
        public string DY_WAV_Discount_Tmoney { set; get; }
        public string DY_WAV_Tmoney { set; get; }
        public string DY_WAV_Discount { set; get; }
        public string DY_WAV_DiscountReduce { set; get; }
        public string DY_WAV_CommuterExtendedPeriod { set; get; }
        public string DY_WAV_CardCancle { set; get; }
        public string DY_WAV_DiscountBarcode_Card { set; get; }
        public string DY_WAV_DBarcode_Discount_Card_Cash_Tmoney { set; get; }
        public string DY_WAV_DBarcode_Card_Cash_Tmoney { set; get; }
        public string DY_WAV_DBarcode_Discount_Cash_Tmoney { set; get; }
        public string DY_WAV_DBarcode_Cash_Tmoney { set; get; }
        public string DY_WAV_DBarcode_Discount_Card_Cash { set; get; }
        public string DY_WAV_DBarcode_Card_Cash { set; get; }
        public string DY_WAV_DBarcode_Discount_Cash { set; get; }
        public string DY_WAV_DBarcode_Cash { set; get; }
        public string DY_WAV_DBarcode_Discount_Card_Tmoney { set; get; }
        public string DY_WAV_DBarcode_Card_Tmoney { set; get; }
        public string DY_WAV_DBarcode_Discount_Tmoney { set; get; }
        public string DY_WAV_DBarcode_Discount_Card { set; get; }
        public string DY_WAV_DBarcode_Card { set; get; }
        public string DY_WAV_DBarcode_Tmoney { set; get; }
        public string DY_WAV_NotCarSearch { set; get; }
        public string DY_WAV_SeviceCar { set; get; }
        public string DY_WAV_Commuter { set; get; }
        public string DY_WAV_AddPayment { set; get; }
        public string DY_WAV_NotEnoghfMoney { set; get; }
        public string DY_WAV_CorrectCard { set; get; }
        public string DY_WAV_NotOutCard { set; get; }
        public string DY_WAV_NotCardPay { set; get; }
        public string DY_WAV_NoDiscountTicket { set; get; }
        public string DY_WAV_DuplicateDiscountTicket { set; get; }
        public string DY_WAV_NoAddDiscountTIcket { set; get; }
        public string DY_WAV_NoBarcode { set; get; }
        public string DY_WAV_DuplicateBarcode { set; get; }
        public string DY_WAV_AdVERTISE { set; get; }



    }
}
