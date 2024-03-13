namespace CommonLib
{
    public class CommonData
    {
        public class ORDER_RETURNCODE
        {
            public const string SUCCESS = "000";
            public const string Application_Error = "-999";
            public const string MARKET_CLOSE = "017";
            public const string BOND_GATEWAY_HAS_NO_SESSION = "018";
            public const string OrderNo_Duplicated = "019";
            public const string MARKET_IN_BREAK_TIME = "020";
            public const string SYMBOL_IS_NOT_FOUND = "021";
            public const string SYMBOL_IS_SUSPENDED = "022";
            public const string INVALID_ORIGIN_ORDERNO_REFEXCHANGEID = "023";
            public const string BOND_DMA_DISCONNECTED = "029";
            public const string QUEUE_FULL = "039";
        }

        public class ORDER_RETURNMESSAGE
        {
            public const string SUCCESS = "Thanh cong";
            public const string Application_Error = "ERROR_APPLICATION";
            public const string OrderNo_Duplicated = "'OrderNo' had been duplicated";
            public const string OrderNoAPI_IsValid = "OrderNo of the order replacement, order cancellation, order closing must be equal to orderNo of the original order";

            public const string BOND_GATEWAY_HAS_NO_SESSION = "Bond DMA Gateway has no session";
            public const string MARKET_CLOSE = "Market close";
            public const string MARKET_IN_BREAK_TIME = "Market in intermission";
            public const string SYMBOL_IS_NOT_FOUND = "Symbol does not exist";
            public const string SYMBOL_IS_SUSPENDED = "Trading halt for this symbol";
            public const string INVALID_ORIGIN_ORDERNO_REFEXCHANGEID = "OrderNo of the order replacement, order cancellation, order closing must be equal to orderNo of the original order";
            public const string BOND_DMA_DISCONNECTED = "Order was rejected when Bond DMA Gateway disconnected from HNX FIX Gateway";
            public const string QUEUE_FULL = "Order queue is full";
        }

        public class ORDER_SIDE
        {
            public const string SIDE_BUY = "B";
            public const string SIDE_SELL = "S";
        }

        public class ORDER_ORDERTYPE
        {
            public const string TTDTO = "S";
            public const string BCGDO = "R";

            public const string DIEN_TU_TUY_CHON_REPO = "U";
            public const string BCGD_REPOS = "T";
        }

        public class NORMAL_ORDERTYPE
        {
            public const string LO = "2";
            public const string MTL = "3";
            public const string MAS = "4";
            public const string ATC = "5";
            public const string ATO = "6";
            public const string MAK = "A";
            public const string MOK = "K";
            public const string MM = "M";
        }

        public class ORDER_SPECIALTYPE
        {
            public const int LENH_MM_YET_GIA_1_CHIEU = 1;
            public const int LENH_MM_YET_GIA_2_CHIEU = 2;
            public const int LENH_MM_2_CHIEU_THAY_THE = 3;
        }

        public class ORDER_SETTLMETHOD
        {
            public const int PAYMENT_NOW = 1;
            public const int PAYMENT_LASTDATE = 2;
        }

        public class ORDER_ISVISIBLE
        {
            public const int ISVISIBLE = 1;
            public const int NOT_ISVISIBLE = 0;
        }

        public class ORDER_CrossType
        {
            public const int Order_BCGDO = 1;
            public const int Accept_BCGDO = 3;

            public const int SUA_LENH_THOA_THUAN_BCGD_CHUA_THUC_HIEN = 1;
            public const int HUY_LENH_THOA_THUAN_CHUA_THUC_HIEN = 1;
            public const int HUY_LENH_THOA_THUAN_DA_THUC_HIEN = 2;
            public const int SUA_LENH_THOA_THUAN_BCGD_DA_THUC_HIEN = 2;

            //
            public const int CHAP_NHAN_YC_SUA_LENH_THOA_THUAN_BCGD_DA_THUC_HIEN = 3;

            public const int TU_CHOI_YC_SUA_LENH_THOA_THUAN_BCGD_DA_THUC_HIEN = 4;

            //
            public const int CHAP_NHAN_HUY_LENH_THOA_THUAN = 3;

            public const int TU_CHOI_HUY_LENH_THOA_THUAN = 4;
        }

        public class TradingSessionStatus
        {
            public const string BeforeTradingSession = "0";  //Chưa bắt đầu.
            public const string InTradingSession = "1"; // đang giao dịch bình thường
            public const string PauseSession = "2";  //Tạm dừng
            public const string EndOfTradingSession = "13"; //Kết thúc nhận lệnh của ngày giao dịch hiện tại
            public const string WaitforOrder = "90";  //Thị trường đang ở trạng thái chờ nhận lệnh
            public const string MarketClose = "97"; //Thị trường đã đóng cửa
            public const string DefaultStatus = ""; // Trạng thái này khi gate chưa mới bật lên và chưa lấy được phiên
        }

        public class SecurityTradingSessionStatus
        {
            public const int Normal = 0;
            public const int Not_Trading = 1;
            public const int Stop_Trading = 2;
            public const int Delisting = 6;
            public const int Wait_Trading = 9;
            public const int Pause_Trading = 10;
            public const int Transaction_Restrictions = 11;
            public const int Special_Bond = 25;
        }

        public class TradingSessionMode
        {
            public const int byTable = 1;  //= 1 lấy thông tin phiên theo bảng
            public const int bySymbol = 2;  //= 2 lấy thông tin phiên theo CK
        }

        public class ORDER_API
        {
            public const string API_1 = "API1";
            public const string API_2 = "API2";
            public const string API_3 = "API3";
            public const string API_4 = "API4";
            public const string API_5 = "API5";
            public const string API_6 = "API6";
            public const string API_7 = "API7";
            public const string API_8 = "API8";
            public const string API_9 = "API9";
            public const string API_10 = "API10";
            public const string API_11 = "API11";
            public const string API_12 = "API12";
            public const string API_13 = "API13";
            public const string API_14 = "API14";
            public const string API_15 = "API15";
            public const string API_16 = "API16";

            public const string API_17 = "API17";
            public const string API_18 = "API18";
            public const string API_19 = "API19";
            public const string API_20 = "API20";

            public const string API_21 = "API21";
            public const string API_22 = "API22";
            public const string API_23 = "API23";
            public const string API_24 = "API24";
            public const string API_25 = "API25";
            public const string API_26 = "API26";
            public const string API_27 = "API27";
            public const string API_28 = "API28";
            public const string API_29 = "API29";
            public const string API_30 = "API30";
            public const string API_31 = "API31";
            public const string API_32 = "API32";
            public const string API_33 = "API33";
        }

        public class QuoteType
        {
            public const int LENH_DAT_INQUIRY = 1;
            public const int LENH_SUA_INQUIRY = 2;
            public const int LENH_HUY_INQUIRY = 3;
            public const int LENH_DONG_INQUIRY = 4;
        }

        public class QuoteTypeRepos
        {
            public const int LENH_DAT = 1;
            public const int LENH_SUA = 2;
            public const int LENH_HUY = 3;
            public const int LENH_TU_CHOI = 4;
        }

        public class QuoteType_BCGDRepos
        {
            public const int LENH_SUA_BCGD_REPOS = 1;
            public const int LENH_SUA_THOATHUAN_DA_THUC_HIEN_REPOS_TRONG_NGAY = 2;
            public const int LENH_XAC_NHAN_BCGD_REPOS = 3;
            public const int LENH_TUCHOI_BCGD_REPOS = 4;
            public const int LENH_SUA_THOATHUAN_DA_THUC_HIEN_REPOS = 7;

            public const int CHAP_NHAN_SUA_THOATHUAN_DA_THUC_HIEN_REPOS = 8;
            public const int TU_CHOI_SUA_THOATHUAN_DA_THUC_HIEN_REPOS = 9;
        }
    }

    public class CommonDataInCore
    {
        public class CORE_MsgType
        {
            public const string MsgOS = "OS";
            public const string MsgOE = "OE";
            public const string MsgIS = "IS";
            public const string MsgRS = "RS";
            public const string MsgRE = "RE";
            public const string MsgNS = "NS";
            public const string MsgNE = "NE";
        }

        public class CORE_OrderStatus
        {
            public const string OrderStatus_RE = "RE";
            public const string OrderStatus_PA = "PA";
            public const string OrderStatus_AM = "AM";
            public const string OrderStatus_NM = "NM";
            public const string OrderStatus_AC = "AC";
            public const string OrderStatus_CL = "CL";
            public const string OrderStatus_EJ = "EJ";
            public const string OrderStatus_NP = "NP";
            public const string OrderStatus_EC = "EC";
            public const string OrderStatus_TX = "TX";
            public const string OrderStatus_QE = "QE";
            public const string OrderStatus_NC = "NC";

            public const string OrderStatus_IN = "IN";
            public const string OrderStatus_NI = "NI";
            public const string OrderStatus_SC = "SC";
            public const string OrderStatus_PM = "PM";
            public const string OrderStatus_MR = "MR";
            public const string OrderStatus_PC = "PC";
            public const string OrderStatus_CR = "CR";
        }

        public class CORE_RefMsgType
        {
            public const string RefMsgType_S = "S";
            public const string RefMsgType_R = "R";
            public const string RefMsgType_Z = "Z";
            public const string RefMsgType_D = "D";
            public const string RefMsgType_G = "G";
            public const string RefMsgType_F = "F";
        }

        public class CORE_OrderSide
        {
            public const int SIDE_BUY = 1;
            public const int SIDE_SELL = 2;
        }

        public class CORE_OrderType
        {
            public const string TTDTO = "S";
            public const string BCGDO = "R";
        }

        /// <summary>
        /// 537 QuoteType
        /// </summary>
        public class CORE_QuoteType
        {
            public const int Bao_Co_Dien_Tu_Moi = 1;
            public const int Bao_Sua_Dien_Tu = 2;
            public const int Bao_Huy_Dien_Tu = 3;
            public const int Bao_Dien_Tu_Da_Duoc_Chap_Nhan = 4;
            public const int Bao_Huy_Lenh_Thoa_Thuan_Dien_Tu_Sua_Doi_Ung = 5;
        }

        public class CORE_QuoteTypeInquiry
        {
            public const int QuoteType_1 = 1;
            public const int QuoteType_2 = 2;
            public const int QuoteType_3 = 3;
            public const int QuoteType_4 = 4;
            public const int QuoteType_5 = 5;
            public const int QuoteType_6 = 6;
        }

        public class CORE_QuoteTypeRepos
        {
            public const int QuoteType_1 = 1;
            public const int QuoteType_2 = 2;
            public const int QuoteType_3 = 3;
            public const int QuoteType_4 = 4;
            public const int QuoteType_5 = 5;
            public const int QuoteType_6 = 6;
            public const int QuoteType_7 = 7;
            public const int QuoteType_8 = 8;
            public const int QuoteType_9 = 9;
            public const int QuoteType_10 = 10;
            public const int QuoteType_11 = 11;
            public const int QuoteType_12 = 12;
            public const int QuoteType_13 = 13;
            public const int QuoteType_14 = 14;
            public const int QuoteType_18 = 18;
        }

        public class CORE_RejectReasonCode
        {
            public const string Code_50000 = "50000";
            public const string Code_50001 = "50001";
            public const string Code_50002 = "50002";
            public const string Code_50003 = "50003";
            public const string Code_50004 = "50004";
            public const string Code_50005 = "50005";
            public const string Code_50006 = "50006";
            public const string Code_50007 = "50007";
            public const string Code_50008 = "50008";
            public const string Code_50009 = "50009";
            public const string Code_50010 = "50010";
            public const string Code_50011 = "50011";
            public const string Code_50012 = "50012";
            public const string Code_50013 = "50013";
            public const string Code_50014 = "50014";
            public const string Code_50015 = "50015";
            public const string Code_50017 = "50017";
            public const string Code_GateNotReady = "-1";
        }

        public class CORE_RejectReason
        {
            public const string RejectReason_50000 = "Exchange did not accept";
            public const string RejectReason_50001 = "Partner did not accept deal replacement";
            public const string RejectReason_50002 = "Partner did not accept deal cancellation";
            public const string RejectReason_50003 = "Market close";
            public const string RejectReason_50004 = "This order has been accepted by another firm";
            public const string RejectReason_50005 = "Counter Firm has been changed, this order was closed";
            public const string RejectReason_50006 = "Market in intermission";
            public const string RejectReason_50007 = "Inquiry Member has closed this order";
            public const string RejectReason_50008 = "Inquiry order was expired";
            public const string RejectReason_50009 = "Firm order was expired";
            public const string RejectReason_50010 = "Order was rejected when Bond DMA Gateway disconnected from HNX FIX Gateway";
            public const string RejectReason_50011 = "No available foreign room";
            public const string RejectReason_50012 = "Reject due to order’s attributes (FAK, FOK, KOS)";
            public const string RejectReason_50013 = "Reject due to session changing";
            public const string RejectReason_50014 = "Reject before market closing";
            public const string RejectReason_50015 = "Reject due to unidentified reason";
            public const string RejectReason_50017 = "Reject due to two-sided market maker order's attributes";

            //
        }

        public class CORE_CrossType
        {
            public const int CrossType_1 = 1;
            public const int CrossType_2 = 2;
        }

        public class CORE_OrdStatus
        {
            public const string OrdStatus_A = "A";
            public const string OrdStatus_M = "M";
            public const string OrdStatus_2 = "2";
            public const string OrdStatus_3 = "3";
            public const string OrdStatus_4 = "4";
            public const string OrdStatus_5 = "5";
            public const string OrdStatus_8 = "8";
            public const string OrdStatus_9 = "9";
            public const string OrdStatus_10 = "10";
            public const string OrdStatus_11 = "11";
        }

        public class CORE_OrdType
        {
            public const string OrdType_U = "U";
            public const string OrdType_R = "R";
            public const string OrdType_T = "T";
            public const string OrdType_S = "S";
        }

        public class CORE_OrdRejReason
        {
            public const int OrdRejReason_1 = 1;
            public const int OrdRejReason_2 = 2;
            public const int OrdRejReason_3 = 3;
            public const int OrdRejReason_4 = 4;
            public const int OrdRejReason_5 = 5;
            public const int OrdRejReason_9 = 9;
        }

        public class CORE_MatchReportType
        {
            public const int MatchReportType_1 = 1;
            public const int MatchReportType_2 = 2;
        }

        public class Const_UserRole
        {
            public const string Role_Full = "FULL";
            public const string Role_View = "VIEW";
        }

        public class Const_APIURL
        {
            public const string API_BASE = "/api/hnxtprl/v1/";

            public const string API_1 = "order/new/electronic-put-through";
            public const string API_2 = "order/confirm/electronic-put-through";
            public const string API_3 = "order/replace/electronic-put-through";
            public const string API_4 = "order/cancel/electronic-put-through";
            public const string API_5 = "order/new/outright-common-put-through";
            public const string API_6 = "order/confirm/outright-common-put-through";
            public const string API_7 = "order/replace/outright-common-put-through";
            public const string API_8 = "order/cancel/outright-common-put-through";
            public const string API_9 = "order/replace-deal/outright-common-put-through";
            public const string API_10 = "order/confirm-replace-deal/outright-common-put-through";

            public const string API_11 = "order/cancel-deal/outright-common-put-through";
            public const string API_12 = "order/confirm-cancel-deal/outright-common-put-through";
            public const string API_13 = "order/new/inquiry-repos";
            public const string API_14 = "order/replace/inquiry-repos";
            public const string API_15 = "order/cancel/inquiry-repos";
            public const string API_16 = "order/close/inquiry-repos";
            public const string API_17 = "order/new/firm-repos";
            public const string API_18 = "order/replace/firm-repos";
            public const string API_19 = "order/cancel/firm-repos";
            public const string API_20 = "order/confirm/firm-repos";

            public const string API_21 = "order/new/repos-common-put-through";
            public const string API_22 = "order/confirm/repos-common-put-through";
            public const string API_23 = "order/replace/repos-common-put-through";
            public const string API_24 = "order/cancel/repos-common-put-through";
            public const string API_25 = "order/replace-deal/sale/repos-common-put-through";
            public const string API_26 = "order/confirm-replace-deal/sale/repos-common-put-through";
            public const string API_27 = "order/cancel-deal/sale/repos-common-put-through";
            public const string API_28 = "order/confirm-cancel-deal/sale/repos-common-put-through";
            public const string API_29 = "order/replace-deal/repurchase/repos-common-put-through";
            public const string API_30 = "order/confirm-replace-deal/repurchase/repos-common-put-through";

            public const string API_31 = "order/new/automatic-order-matching";
            public const string API_32 = "order/replace/automatic-order-matching";
            public const string API_33 = "order/cancel/automatic-order-matching";
        }
    }

    public static class TradingCode
    {
        public const string AllSymbol = "0";
        public const string AllTable = "1";
        public const string WithTable = "2";
        public const string WithSymbol = "3";
    }
    public static class VaultSetting
    {
        public static string Token = "";
        public static string DataKey = "memberpass";
	}

	public class VaultDataResponse
	{
        public string MemberPass { get; set; } = string.Empty;
	}
}