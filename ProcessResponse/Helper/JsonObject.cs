using CommonLib;
using Confluent.Kafka;
using HNX.FIXMessage;
using LocalMemory;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessProcessResponse
{
    // Object trả ra kafka khi nhận được msg
    // Khi nhận được lệnh 35=AI,35=3
    public class ResponseMessageKafka
    {
        public string MsgType { get; set; } = string.Empty;
        public string OrderPartyID { get; set; } = string.Empty;
        public string OrderNo { get; set; } = string.Empty;
        public string RefExchangeID { get; set; } = string.Empty;
        public string ExchangeID { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
        public string RefMsgType { get; set; } = string.Empty;
        public string OrdType { get; set; } = string.Empty;
        public int CrossType { get; set; } = 0;
        public string ClientID { get; set; } = string.Empty;
        public string ClientIDCounterFirm { get; set; } = string.Empty;
        public string MemberCounterFirm { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public long OrderQty { get; set; } = 0;
        public long Price { get; set; } = 0;
        public double SettleValue { get; set; } = 0;
        public string SettleDate { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public int SettleMethod { get; set; } = 0;
        public string RegistID { get; set; } = string.Empty;
        public string EffectiveTime { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string RejectReasonCode { get; set; } = string.Empty;
        public string RejectReason { get; set; } = string.Empty;
        public string SendingTime { get; set; } = string.Empty;
		public int RefSeqNum { get; set; } = 0;
	}

	// Object trả ra kafka khi nhận được msg
	// Khi nhận được lệnh Khớp Map từ 35=8
	public class ResponseOrderFilledToKafka
    {
        public string MsgType { get; set; } = string.Empty;
        public string SendingTime { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string OrderID { get; set; } = string.Empty;
        public string OrderNo { get; set; } = string.Empty;
        public string OrigClOrdID { get; set; } = string.Empty;
        public string SecondaryClOrdID { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public long LastQty { get; set; } = 0;
        public long LastPx { get; set; } = 0;
        public double SettleValue { get; set; } = 0;
        public string ExecID { get; set; } = string.Empty;
        public string MemberCounterFirm { get; set; } = string.Empty;
    }
    public class ResponseTradingSession
    {
        public string MsgType { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string TradSesReqID { get; set; } = string.Empty;
        public string TradingSessionID { get; set; } = string.Empty;
        public int TradeSesMode { get; set; } = 0;
        public int TradSesStatus { get; set; } = 0;
        public string TradSesStartTime { get; set; } = string.Empty;
        public string SendingTime { get; set; } = string.Empty;

        public ResponseTradingSession(MessageTradingSessionStatus message)
        {
            MsgType = "TS";
            Text = message.Text;
            TradSesReqID = message.TradSesReqID;
            TradeSesMode = message.TradSesMode;
            TradSesStatus = Utils.ParseInt(message.TradSesStatus);
            TradingSessionID = message.TradingSessionID;
            TradSesStartTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(message.TradSesStartTime);
            SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(message.GetSendingTime);

        }
    }

    public class ResponseTopicTradingInfomation
    {
        public string MsgType { get; set; } = "";
        public string Member { get; set; } = "";
        public string Symbol { get; set; } = "";
        public string SendingTime { get; set; } = "";
        public ResponseTopicTradingInfomation(MessageTopicTradingInfomation message)
        {
            MsgType = MessageType.TopicTradingInfomation;
            Member = message.InquiryMember;
            Symbol = message.Symbol;
            SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(message.GetSendingTime);
        }
    }

    public class ResponseSecurityStatus
    {
        public string MsgType { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string TradingSubID { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string SecurityType { get; set; } = string.Empty;
        public string MaturityDate { get; set; } = string.Empty;
        public string IssueDate { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public long HighPX { get; set; } = 0;
        public long LowPX { get; set; } = 0;
        public long HighPXOut { get; set; } = 0;
        public long LowPXOut { get; set; } = 0;
        public long HighPXRep { get; set; } = 0;
        public long LowPXRep { get; set; } = 0;
        public long LastPX { get; set; } = 0;
        public int SecurityTradingStatus { get; set; } = 0;
        public long BuyVolume { get; set; } = 0;
        public int DateNo { get; set; } = 0;
        public long TotalListingQty { get; set; } = 0;
        public int TypeRule { get; set; } = 0;
        public string TradingSubjectBuyAllowed { get; set; } = string.Empty;
        public string TradingSubjectSellAllowed { get; set; } = string.Empty;
        public string SendingTime { get; set; } = string.Empty;
        public ResponseSecurityStatus(MessageSecurityStatus message)
        {
            MsgType = "SS";
            Text = message.Text != null ? message.Text : string.Empty;
            TradingSubID = message.TradingSessionSubID;
            Symbol = message.Symbol;
            SecurityType = message.SecurityType;
            MaturityDate = HNX.FIXMessage.Utils.Convert.ToShortDate(message.MaturityDate);
            IssueDate = HNX.FIXMessage.Utils.Convert.ToShortDate(message.IssueDate);
            Issuer = message.Issuer;
            HighPX = message.HighPx;
            LowPX = message.LowPx;
            HighPXOut = message.HighPxOut;
            LowPXOut = message.LowPxOut;
            HighPXRep = message.HighPxRep;
            LowPXRep = message.LowPxRep;
            LastPX = message.LastPx;
            SecurityTradingStatus = message.SecurityTradingStatus;
            BuyVolume = message.BuyVolume;
            DateNo = Utils.ParseInt(message.DateNo);
            TotalListingQty = message.TotalListingQtty;
            TypeRule = message.TypeRule;
            TradingSubjectBuyAllowed = message.Allowed_Trading_Subject;
            TradingSubjectSellAllowed = message.Allowed_Trading_Subject_Sell;
            SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(message.GetSendingTime);

        }

    }
}