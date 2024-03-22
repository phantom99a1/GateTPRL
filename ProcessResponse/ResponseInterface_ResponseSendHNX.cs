/*
 * Project:
 * Author :
 * Summary: Lớp xử lý dữ liệu để gửi sang Kafka khi send message sang Sở thành công
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 *
 */

using CommonLib;
using HNX.FIXMessage;
using KafkaInterface;
using LocalMemory;
using StorageProcess;
using static CommonLib.CommonData;
using static CommonLib.CommonDataInCore;

namespace BusinessProcessResponse
{
    public partial class ResponseInterface : IResponseInterface
    {
        /// <summary>
        /// Phản hồi từ khi nhận được msg từ Sở HNXExchange để build dữ liệu gửi sang Kafka
        /// </summary>
        /// <param name="fMsg"></param>
        public void ResponseGateSend2HNX(FIXMessageBase fMsg)
        {
            switch (fMsg.GetMsgType)
            {
                case MessageType.Quote: // 35=S
                    ResponseSend2HNX_35S_MessageQuote((MessageQuote)fMsg);
                    break;

                case MessageType.NewOrderCross: // 35=s
                    ResponseSend2HNX_35s_MessageNewOrderCross((MessageNewOrderCross)fMsg);
                    break;

                case MessageType.QuoteCancel: // 35=Z
                    ResponseSend2HNX_35Z_MessageQuoteCancel((MessageQuoteCancel)fMsg);
                    break;

                case MessageType.QuoteRequest: // 35=R
                    ResponseSend2HNX_35R_MessageQuoteRequest((MessageQuoteRequest)fMsg);
                    break;

                case MessageType.QuoteResponse: // 35=AJ
                    ResponseSend2HNX_35AJ_MessageQuoteResponse((MessageQuoteResponse)fMsg);
                    break;

                case MessageType.CrossOrderCancelReplaceRequest: // 35=t
                    ResponseSend2HNX_35t_CrossOrderCancelReplaceRequest((CrossOrderCancelReplaceRequest)fMsg);
                    break;

                case MessageType.CrossOrderCancelRequest: // 35=u
                    ResponseSend2HNX_35u_CrossOrderCancelRequest((CrossOrderCancelRequest)fMsg);
                    break;

                case MessageType.ReposInquiry: // 35=N01
                    ResponseSend2HNX_35N01_NewInquiryRepos((MessageReposInquiry)fMsg);
                    break;

                case MessageType.ReposFirm: // 35=N03
                    ResponseSend2HNX_35N03_MessageReposFirm((MessageReposFirm)fMsg);
                    break;

                case MessageType.ReposFirmAccept: // 35=N05
                    ResponseSend2HNX_35N05_MessageReposFirmAccept((MessageReposFirmAccept)fMsg);
                    break;

                case MessageType.ReposBCGD: // 35=MA
                    ResponseSend2HNX_35MA_MessageReposBCGD((MessageReposBCGD)fMsg);
                    break;

                case MessageType.ReposBCGDModify: // 35=ME
                    ResponseSend2HNX_35ME_MessageReposBCGDModify((MessageReposBCGDModify)fMsg);
                    break;

                case MessageType.ReposBCGDCancel: // 35=MC
                    ResponseSend2HNX_35MC_MessageReposBCGDCancel((MessageReposBCGDCancel)fMsg);
                    break;

                case MessageType.NewOrder: // 35=D
                    ResponseSend2HNX_35D_MessageNewOrder((MessageNewOrder)fMsg);
                    break;

                case MessageType.ReplaceOrder: // 35=G
                    ResponseSend2HNX_35G_MessageReplaceOrder((MessageReplaceOrder)fMsg);
                    break;

                case MessageType.CancelOrder: // 35=F
                    ResponseSend2HNX_35F_MessageCancelOrder((MessageCancelOrder)fMsg);
                    break;
            }
        }

        // Xử lý send kafka khi gửi sang Sở thành công với msg: 35=S
        private void ResponseSend2HNX_35S_MessageQuote(MessageQuote p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35S_MessageQuote -> start process after send exchange 35={MessageType.Quote} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, ClOrdID={p_Message.ClOrdID}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, IsVisible={p_Message.IsVisible}");
                //
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderPartyID = "";
                _Response.OrderNo = p_Message.ClOrdID;
                _Response.RefExchangeID = "";
                _Response.ExchangeID = "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
                _Response.OrderPartyID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.RefMsgType = MessageType.Quote;
                _Response.OrdType = p_Message.OrdType;
                _Response.CrossType = 0;
                _Response.ClientID = p_Message.Account;
                _Response.ClientIDCounterFirm = "";
                _Response.MemberCounterFirm = "";
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                else
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                _Response.OrderQty = p_Message.OrderQty;
                _Response.Price = p_Message.Price2;
                _Response.SettleValue = p_Message.SettlValue;
                _Response.SettleDate = p_Message.SettDate;
                _Response.Symbol = p_Message.Symbol;
                _Response.SettleMethod = p_Message.SettlMethod;
                _Response.RegistID = p_Message.RegistID;
                _Response.EffectiveTime = "";
                _Response.Text = p_Message.Text;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";

                //
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35S_MessageQuote -> End process after send exchange message 35=S ID {p_Message.IDRequest}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, ClOrdID={p_Message.ClOrdID}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, IsVisible={p_Message.IsVisible}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35S_MessageQuote -> Error process after send exchange 35=S ID {p_Message.IDRequest}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, ClOrdID={p_Message.ClOrdID}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, IsVisible={p_Message.IsVisible}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka khi gửi sang Sở thành công với msg: 35=s
        private void ResponseSend2HNX_35s_MessageNewOrderCross(MessageNewOrderCross p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35s_MessageNewOrderCross -> start process after send exchange 35=s ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, CrossID={p_Message.CrossID}, EffectiveTime={p_Message.EffectiveTime}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, ClOrdID={p_Message.ClOrdID}, SettlMethod={p_Message.SettlMethod}");
                //
                OrderInfo objOrder = OrderMemory.GetOrderBy(p_ClOrdID: p_Message.ClOrdID);
                string p_OrderNo = string.Empty;
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }

                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderPartyID = "";
                _Response.OrderNo = p_OrderNo;
                _Response.RefExchangeID = !string.IsNullOrEmpty(p_Message.CrossID) ? p_Message.CrossID : "";
                _Response.ExchangeID = "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
                _Response.OrderPartyID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.RefMsgType = MessageType.NewOrderCross;
                _Response.OrdType = p_Message.OrdType;
                _Response.CrossType = p_Message.CrossType;
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                {
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                    _Response.ClientID = p_Message.CoAccount;
                    _Response.ClientIDCounterFirm = p_Message.Account;
                    _Response.MemberCounterFirm = p_Message.PartyID;
                }
                else
                {
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                    _Response.ClientID = p_Message.Account;
                    _Response.ClientIDCounterFirm = p_Message.CoAccount;
                    _Response.MemberCounterFirm = p_Message.CoPartyID;
                }
                _Response.OrderQty = p_Message.OrderQty;
                _Response.Price = p_Message.Price2;
                _Response.SettleValue = p_Message.SettlValue;
                _Response.SettleDate = p_Message.SettDate;
                _Response.Symbol = p_Message.Symbol;
                _Response.SettleMethod = p_Message.SettlMethod;
                _Response.RegistID = "";
                _Response.EffectiveTime = p_Message.EffectiveTime;
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";

                //
                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35s_MessageNewOrderCross -> End process after send exchange 35=s ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, CrossID={p_Message.CrossID}, EffectiveTime={p_Message.EffectiveTime}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, ClOrdID={p_Message.ClOrdID}, SettlMethod={p_Message.SettlMethod}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35s_MessageNewOrderCross -> Error process after send exchange 35=s with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, CrossID={p_Message.CrossID}, EffectiveTime={p_Message.EffectiveTime}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, ClOrdID={p_Message.ClOrdID}, SettlMethod={p_Message.SettlMethod}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka khi gửi sang Sở thành công với msg: 35=Z
        private void ResponseSend2HNX_35Z_MessageQuoteCancel(MessageQuoteCancel p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35Z_MessageQuoteCancel -> start process after send exchange 35=Z ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteID={p_Message.QuoteID}, QuoteCancelType={p_Message.QuoteCancelType}, Symbol={p_Message.Symbol}, OrdType={p_Message.OrdType}, ClOrdID={p_Message.ClOrdID}");
                //
                string p_OrderNo = "";
                //
                // p_Message.QuoteID 171
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                else
                {
                    Logger.ResponseLog.Warn($"ResponseSend2HNX_35Z_MessageQuoteCancel -> Error find order info when received 35=Z ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteID={p_Message.QuoteID}, QuoteCancelType={p_Message.QuoteCancelType}, Symbol={p_Message.Symbol}, OrdType={p_Message.OrdType}, ClOrdID={p_Message.ClOrdID} after send exchange");
                }
                //
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderPartyID = "";
                _Response.OrderNo = p_OrderNo;
                _Response.RefExchangeID = p_Message.QuoteID;
                _Response.ExchangeID = "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                _Response.OrderPartyID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.RefMsgType = MessageType.QuoteCancel;
                _Response.OrdType = p_Message.OrdType;
                _Response.ClientID = "";
                _Response.ClientIDCounterFirm = "";
                _Response.MemberCounterFirm = "";
                _Response.CrossType = 0;
                _Response.Side = "";
                _Response.OrderQty = 0;
                _Response.Price = 0;
                _Response.SettleValue = 0;
                _Response.SettleDate = "";
                _Response.Symbol = p_Message.Symbol;
                _Response.SettleMethod = 0;
                _Response.RegistID = "";
                _Response.EffectiveTime = "";
                _Response.Text = p_Message.Text;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";

                //
                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35Z_MessageQuoteCancel-> End process after send exchange 35=Z ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteID={p_Message.QuoteID}, QuoteCancelType={p_Message.QuoteCancelType}, Symbol={p_Message.Symbol}, OrdType={p_Message.OrdType}, ClOrdID={p_Message.ClOrdID}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35Z_MessageQuoteCancel: Error process after send exchange 35=Z ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteID={p_Message.QuoteID}, QuoteCancelType={p_Message.QuoteCancelType}, Symbol={p_Message.Symbol}, OrdType={p_Message.OrdType}, ClOrdID={p_Message.ClOrdID}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka khi gửi sang Sở thành công với msg: 35=R
        private void ResponseSend2HNX_35R_MessageQuoteRequest(MessageQuoteRequest p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35R_MessageQuoteRequest: start process after send exchange 35=R ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID={p_Message.RFQReqID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, OrderID={p_Message.OrderID}, Symbol={p_Message.Symbol}, TransactTime={p_Message.TransactTime}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, ClOrdID={p_Message.ClOrdID}");
                //
                string p_OrderNo = "";
                //
                // p_Message.QuoteID 171
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                else
                {
                    Logger.ResponseLog.Warn($"ResponseSend2HNX_35R_MessageQuoteRequest: Error find order info when received 35=R ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID={p_Message.RFQReqID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, OrderID={p_Message.OrderID}, Symbol={p_Message.Symbol}, TransactTime={p_Message.TransactTime}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, ClOrdID={p_Message.ClOrdID} after send exchange");
                }
                //
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderPartyID = "";
                _Response.OrderNo = p_OrderNo;
                _Response.RefExchangeID = p_Message.RFQReqID;
                _Response.ExchangeID = "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                _Response.OrderPartyID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.RefMsgType = MessageType.QuoteRequest;
                _Response.OrdType = p_Message.OrdType;
                _Response.ClientID = p_Message.Account;
                _Response.ClientIDCounterFirm = "";
                _Response.MemberCounterFirm = "";
                _Response.CrossType = 0;
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                {
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                }
                else
                {
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                }
                _Response.OrderQty = p_Message.OrderQty;
                _Response.Price = p_Message.Price2;
                _Response.SettleValue = p_Message.SettlValue;
                _Response.SettleDate = p_Message.SettDate;
                _Response.Symbol = p_Message.Symbol;
                _Response.SettleMethod = p_Message.SettlMethod;
                _Response.RegistID = p_Message.RegistID;
                _Response.EffectiveTime = "";
                _Response.Text = p_Message.Text;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";

                //
                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                Logger.ResponseLog.Info($"ResponseSend2HNX_35R_MessageQuoteRequest: End process after send exchange 35=R ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID={p_Message.RFQReqID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, OrderID={p_Message.OrderID}, Symbol={p_Message.Symbol}, TransactTime={p_Message.TransactTime}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, ClOrdID={p_Message.ClOrdID}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35R_MessageQuoteRequest: Error process after send exchange 35=R ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID={p_Message.RFQReqID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, OrderID={p_Message.OrderID}, Symbol={p_Message.Symbol}, TransactTime={p_Message.TransactTime}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, ClOrdID={p_Message.ClOrdID}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka khi gửi sang Sở thành công với msg: 35=AJ
        private void ResponseSend2HNX_35AJ_MessageQuoteResponse(MessageQuoteResponse p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35AJ_MessageQuoteResponse: start process after send exchange 35=AJ ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteRespType={p_Message.QuoteRespType}, QuoteRespID={p_Message.QuoteRespID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount(2)={p_Message.CoAccount}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, ClOrdID={p_Message.ClOrdID}");
                //
                OrderInfo objOrder = OrderMemory.GetOrderBy(p_ClOrdID: p_Message.ClOrdID);
                string p_OrderNo = string.Empty;
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderPartyID = "";
                _Response.OrderNo = p_OrderNo;
                _Response.RefExchangeID = p_Message.QuoteRespID;
                _Response.ExchangeID = "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                _Response.OrderPartyID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.RefMsgType = MessageType.QuoteResponse;
                _Response.OrdType = p_Message.OrdType;
                _Response.ClientID = p_Message.Account;
                _Response.ClientIDCounterFirm = p_Message.CoAccount;
                _Response.MemberCounterFirm = "";
                _Response.CrossType = 0;
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                {
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                }
                else
                {
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                }
                _Response.OrderQty = p_Message.OrderQty;
                _Response.Price = p_Message.Price2;
                _Response.Symbol = p_Message.Symbol;
                _Response.SettleMethod = p_Message.SettlMethod;
                _Response.SettleDate = p_Message.SettDate;
                _Response.SettleValue = p_Message.SettlValue;
                _Response.RegistID = "";
                _Response.EffectiveTime = "";
                _Response.Text = p_Message.Text;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";

                //
                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35AJ_MessageQuoteResponse: End process after send exchange 35=AJ ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteRespType={p_Message.QuoteRespType}, QuoteRespID={p_Message.QuoteRespID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, ClOrdID={p_Message.ClOrdID}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35AJ_MessageQuoteResponse: Error process after send exchange 35=AJ ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteRespType={p_Message.QuoteRespType}, QuoteRespID={p_Message.QuoteRespID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, ClOrdID={p_Message.ClOrdID}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka khi gửi sang Sở thành công với msg: 35=t
        private void ResponseSend2HNX_35t_CrossOrderCancelReplaceRequest(CrossOrderCancelReplaceRequest p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35t_CrossOrderCancelReplaceRequest: start process after send exchange 35=t ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, OrgCrossID={p_Message.OrgCrossID}, OrderID={p_Message.OrderID}, EffectiveTime={p_Message.EffectiveTime}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, ClOrdID={p_Message.ClOrdID}");
                //
                string p_OrderNo = "";
                // p_Message.OrgCrossID 551
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                else
                {
                    Logger.ResponseLog.Warn($"ResponseSend2HNX_35t_CrossOrderCancelReplaceRequest: Error find order info when process 35=t ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, OrgCrossID={p_Message.OrgCrossID}, OrderID={p_Message.OrderID}, EffectiveTime={p_Message.EffectiveTime}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, ClOrdID={p_Message.ClOrdID} after send exchange");
                }
                //
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderPartyID = "";
                _Response.OrderNo = p_OrderNo;
                _Response.RefExchangeID = p_Message.OrgCrossID;
                _Response.ExchangeID = "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                _Response.OrderPartyID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.RefMsgType = MessageType.CrossOrderCancelReplaceRequest;
                _Response.OrdType = p_Message.OrdType;
                _Response.CrossType = p_Message.CrossType;

                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                {
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                    _Response.ClientID = p_Message.CoAccount;
                    _Response.ClientIDCounterFirm = p_Message.Account;
                    _Response.MemberCounterFirm = p_Message.PartyID;
                }
                else
                {
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                    _Response.ClientID = p_Message.Account;
                    _Response.ClientIDCounterFirm = p_Message.CoAccount;
                    _Response.MemberCounterFirm = p_Message.CoPartyID;
                }
                _Response.OrderQty = p_Message.OrderQty;
                _Response.Price = p_Message.Price2;
                _Response.Symbol = p_Message.Symbol;
                _Response.SettleMethod = p_Message.SettlMethod;
                _Response.SettleDate = p_Message.SettDate;
                _Response.SettleValue = p_Message.SettlValue;
                _Response.RegistID = "";
                _Response.EffectiveTime = p_Message.EffectiveTime;
                _Response.Text = p_Message.Text;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";

                //
                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35t_CrossOrderCancelReplaceRequest: End process after send exchange 35=t ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, OrgCrossID={p_Message.OrgCrossID}, OrderID={p_Message.OrderID}, EffectiveTime={p_Message.EffectiveTime}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, ClOrdID={p_Message.ClOrdID}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35t_CrossOrderCancelReplaceRequest: Error process after send exchange 35=t ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, OrgCrossID={p_Message.OrgCrossID}, OrderID={p_Message.OrderID}, EffectiveTime={p_Message.EffectiveTime}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, ClOrdID={p_Message.ClOrdID}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka khi gửi sang Sở thành công với msg: 35=u
        private void ResponseSend2HNX_35u_CrossOrderCancelRequest(CrossOrderCancelRequest p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35u_CrossOrderCancelRequest: start process after send exchange 35=u ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, CrossType={p_Message.CrossType}, OrdType={p_Message.OrdType}, OrgCrossID={p_Message.OrgCrossID}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, ClOrdID={p_Message.ClOrdID}, OrderID={p_Message.OrderID}");
                //
                string p_OrderNo = "";
                //
                // p_Message.OrgCrossID 551
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                else
                {
                    Logger.ResponseLog.Warn($"ResponseSend2HNX_35u_CrossOrderCancelRequest: Error find order info when process 35=u ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, CrossType={p_Message.CrossType}, OrdType={p_Message.OrdType}, OrgCrossID={p_Message.OrgCrossID}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, ClOrdID={p_Message.ClOrdID}, OrderID={p_Message.OrderID} after send exchange");
                }
                //
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderPartyID = "";
                _Response.OrderNo = p_OrderNo;
                _Response.RefExchangeID = p_Message.OrgCrossID;
                _Response.ExchangeID = "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                _Response.OrderPartyID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.RefMsgType = MessageType.CrossOrderCancelRequest;
                _Response.OrdType = p_Message.OrdType;
                _Response.CrossType = p_Message.CrossType;
                _Response.ClientID = "";
                _Response.ClientIDCounterFirm = "";
                _Response.MemberCounterFirm = "";
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                {
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                }
                else
                {
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                }
                _Response.OrderQty = 0;
                _Response.Price = 0;
                _Response.SettleValue = 0;
                _Response.Symbol = p_Message.Symbol;
                _Response.SettleMethod = 0;
                _Response.SettleDate = "";
                _Response.RegistID = "";
                _Response.EffectiveTime = "";
                _Response.Text = p_Message.Text;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";
                //
                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35u_CrossOrderCancelRequest: End process after send exchange 35=u ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, CrossType={p_Message.CrossType}, OrdType={p_Message.OrdType}, OrgCrossID={p_Message.OrgCrossID}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, ClOrdID={p_Message.ClOrdID}, OrderID={p_Message.OrderID}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35u_CrossOrderCancelRequest: Error process after send exchange 35=u ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, CrossType={p_Message.CrossType}, OrdType={p_Message.OrdType}, OrgCrossID={p_Message.OrgCrossID}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, ClOrdID={p_Message.ClOrdID}, OrderID={p_Message.OrderID}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka khi không vượt qua check rule trên memory
        public void ReportGateReject(FIXMessageBase fMsg, string p_Text, string p_Code)
        {
            try
            {
                Logger.ResponseLog.Info($"ReportGateReject -> Start process gate reject not match rule with MsgSeqNum = {fMsg.MsgSeqNum}");
                //
                if (fMsg.GetMsgType == MessageType.ReposInquiry) // 35 = N01
                {
                    InquiryObjectModel _Response = new InquiryObjectModel();
                    _Response.MsgType = CORE_MsgType.MsgIS;
                    _Response.OrderNo = fMsg.ApiOrderNo;
                    _Response.ExchangeID = "";
                    _Response.RefExchangeID = "";
                    _Response.QuoteType = "";
                    _Response.OrdType = "";
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_EJ;
                    _Response.OrderPartyID = "";
                    _Response.Side = "";
                    _Response.EffectiveTime = "";
                    _Response.RepurchaseTerm = 0;

                    _Response.SettleMethod = 0;
                    _Response.RegistID = "";
                    _Response.SettleDate1 = "";
                    _Response.SettleDate2 = "";
                    _Response.EndDate = "";
                    _Response.OrderValue = 0;
                    _Response.Symbol = "";
                    _Response.Text = "";
                    _Response.RejectReasonCode = MapErrorCode(p_Code);
                    _Response.RejectReason = p_Text;
                    _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
                    // send kafka
                    //
                    c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, fMsg.TimeInit, fMsg.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                    //
                    Logger.ResponseLog.Info($"ReportGateReject -> process reject not match rule with MsgSeqNum(34)={fMsg.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                }
                else if (fMsg.GetMsgType == MessageType.ReposFirm || fMsg.GetMsgType == MessageType.ReposBCGD) // 35 = N03 || 35 = MA
                {
                    //
                    FirmReposModel _Response = new FirmReposModel();
                    _Response.MsgType = CORE_MsgType.MsgRS;
                    _Response.OrderNo = fMsg.ApiOrderNo;
                    _Response.RefMsgType = fMsg.GetMsgType;
                    _Response.ExchangeID = "";
                    _Response.RefExchangeID = "";
                    _Response.QuoteType = "";
                    _Response.OrdType = "";
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_EJ;
                    _Response.OrderPartyID = "";
                    _Response.InquiryMember = "";
                    _Response.Side = "";
                    _Response.EffectiveTime = "";
                    _Response.RepurchaseTerm = 0;
                    _Response.RepurchaseRate = 0;
                    _Response.SettleDate1 = "";
                    _Response.SettleDate2 = "";
                    _Response.EndDate = "";
                    _Response.SettleMethod = 0;
                    _Response.ClientID = "";
                    _Response.ClientIDCounterFirm = "";
                    _Response.MemberCounterFirm = "";
                    _Response.NoSide = 0;
                    //
                    _Response.SymbolFirmInfo = null;
                    //
                    _Response.MatchReportType = 0;
                    _Response.RejectReasonCode = MapErrorCode(p_Code);
                    _Response.RejectReason = p_Text;
                    _Response.Text = "";
                    _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
                    //
                    c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, fMsg.TimeInit, fMsg.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                    //
					Logger.ResponseLog.Info($"ReportGateReject -> process reject not match rule with MsgSeqNum(34)={fMsg.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
					//
				}
				else if (fMsg.GetMsgType == MessageType.NewOrder || fMsg.GetMsgType == MessageType.ReplaceOrder || fMsg.GetMsgType == MessageType.CancelOrder)
                {
                    NormalObjectModel _Response = new NormalObjectModel();
                    _Response.MsgType = CORE_MsgType.MsgNS;
                    _Response.OrderNo = fMsg.ApiOrderNo;
                    _Response.ExchangeID = "";
                    _Response.RefExchangeID = "";
                    _Response.OrderType = "";
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_EJ;
                    _Response.Side = "";
                    //
                    _Response.Symbol = "";
                    _Response.OrderQty = 0;
                    _Response.OrgOrderQty = 0;
                    _Response.LeavesQty = 0;
                    _Response.LastQty = 0;
                    _Response.Price = 0;
                    _Response.ClientID = "";
                    _Response.SettleValue = 0.0;
                    _Response.OrderQtyMM2 = 0;
                    _Response.PriceMM2 = 0;
                    _Response.SpecialType = 0;
                    //
                    _Response.RejectReasonCode = MapErrorCode(p_Code);
                    _Response.RejectReason = p_Text;
                    _Response.Text = "";
                    _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);

                    c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, fMsg.TimeInit, fMsg.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
					//
					Logger.ResponseLog.Info($"ReportGateReject -> process reject not match rule with MsgSeqNum(34)={fMsg.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
					//
				}
				else // Các message khác
                {
                    ResponseMessageKafka _Response = new ResponseMessageKafka();
                    _Response.MsgType = CORE_MsgType.MsgOS;
                    _Response.OrderPartyID = "";
                    _Response.OrderNo = fMsg.ApiOrderNo;
                    _Response.RefExchangeID = "";
                    _Response.ExchangeID = "";
                    _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
                    _Response.OrderPartyID = "";
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_EJ;
                    _Response.RefMsgType = fMsg.GetMsgType;
                    _Response.OrdType = "";
                    _Response.CrossType = 0;
                    _Response.ClientID = "";
                    _Response.ClientIDCounterFirm = "";
                    _Response.MemberCounterFirm = "";
                    _Response.Side = "";
                    _Response.OrderQty = 0;
                    _Response.Price = 0;
                    _Response.SettleValue = 0;
                    _Response.Symbol = "";
                    _Response.SettleMethod = 0;
                    _Response.SettleDate = "";
                    _Response.RegistID = "";
                    _Response.EffectiveTime = "";
                    _Response.Text = "";
                    _Response.RejectReasonCode = MapErrorCode(p_Code);
                    _Response.RejectReason = p_Text;
                    //
                    //
                    c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, fMsg.TimeInit, fMsg.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                    //
                    Logger.ResponseLog.Info($"ReportGateReject -> process reject not match rule with MsgSeqNum(34)={fMsg.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");

                }
                
                //
                //BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                MessageReject messageReject = new MessageReject();
                messageReject.RefSeqNum = fMsg.MsgSeqNum;
                messageReject.Text = p_Text;
                messageReject.SessionRejectReason = Utils.ParseInt(MapErrorCode(p_Code));
                SharedStorageProcess.c_DataStorageProcess.EnqueueData(messageReject, Data_SoR.Recei);
                //
                Logger.ResponseLog.Info($"ReportGateReject -> End process gate reject not match rule with MsgSeqNum(34) = {fMsg.MsgSeqNum}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_ReportGateReject gate reject not match rule with MsgSeqNum(34) = {fMsg.MsgSeqNum}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka khi gửi sang Sở thành công với msg: 35=N01
        private void ResponseSend2HNX_35N01_NewInquiryRepos(MessageReposInquiry p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35N01_NewInquiryRepos -> start process after send exchange 35={MessageType.ReposInquiry} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, Symbol={p_Message.Symbol}, QuoteType={p_Message.QuoteType}, OrdType={p_Message.OrdType}, Side={p_Message.Side},  OrderQty={p_Message.OrderQty}, EffectiveTime={p_Message.EffectiveTime}, SettlMethod={p_Message.SettlMethod}, SettlDate={p_Message.SettlDate}, SettlDate2={p_Message.SettlDate2}, EndDate={p_Message.EndDate}, RepurchaseTerm={p_Message.RepurchaseTerm}, RegistID={p_Message.RegistID}, RFQReqID={p_Message.RFQReqID}");
                //
                string p_OrderNo = "";
                if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_1)
                {
                    p_OrderNo = p_Message.ClOrdID;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_2 || p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_3 || p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_4)
                {
                    OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                    if (objOrder != null)
                    {
                        p_OrderNo = objOrder.OrderNo;
                    }
                }
                //
                InquiryObjectModel _Response = new InquiryObjectModel();
                _Response.MsgType = CORE_MsgType.MsgIS;
                _Response.OrderNo = p_OrderNo;
                _Response.ExchangeID = "";
                _Response.RefExchangeID = !string.IsNullOrEmpty(p_Message.RFQReqID) ? p_Message.RFQReqID : "";
                _Response.QuoteType = p_Message.QuoteType.ToString();
                _Response.OrdType = p_Message.OrdType;
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.OrderPartyID = "";
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                else
                    _Response.Side = "";
                _Response.EffectiveTime = p_Message.EffectiveTime;
                _Response.RepurchaseTerm = p_Message.RepurchaseTerm;
                _Response.SettleMethod = p_Message.SettlMethod;
                _Response.RegistID = p_Message.RegistID;
                _Response.SettleDate1 = p_Message.SettlDate;
                _Response.SettleDate2 = p_Message.SettlDate2;
                _Response.EndDate = p_Message.EndDate;
                _Response.OrderValue = (long)p_Message.OrderQty;
                _Response.Symbol = p_Message.Symbol;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35N01_NewInquiryRepos -> End process after send exchange message 35={MessageType.ReposInquiry} ID {p_Message.IDRequest}; with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, Symbol={p_Message.Symbol}, QuoteType={p_Message.QuoteType}, OrdType={p_Message.OrdType}, Side={p_Message.Side},  OrderQty={p_Message.OrderQty}, EffectiveTime={p_Message.EffectiveTime}, SettlMethod={p_Message.SettlMethod}, SettlDate={p_Message.SettlDate}, SettlDate2={p_Message.SettlDate2}, EndDate={p_Message.EndDate}, RepurchaseTerm={p_Message.RepurchaseTerm}, RegistID={p_Message.RegistID}, RFQReqID={p_Message.RFQReqID}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35N01_NewInquiryRepos -> Error process after send exchange 35=S ID {p_Message.IDRequest}; with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, Symbol={p_Message.Symbol}, QuoteType={p_Message.QuoteType}, OrdType={p_Message.OrdType}, Side={p_Message.Side},  OrderQty={p_Message.OrderQty}, EffectiveTime={p_Message.EffectiveTime}, SettlMethod={p_Message.SettlMethod}, SettlDate={p_Message.SettlDate}, SettlDate2={p_Message.SettlDate2}, EndDate={p_Message.EndDate}, RepurchaseTerm={p_Message.RepurchaseTerm}, RegistID={p_Message.RegistID}, RFQReqID={p_Message.RFQReqID}, Exception: {ex?.ToString()}");
            }
        }

        private string MapErrorCode(string Code)
        {
            switch (Code)
            {
                case CommonData.ORDER_RETURNCODE.MARKET_CLOSE:
                    return CORE_RejectReasonCode.Code_50003;

                case CommonData.ORDER_RETURNCODE.MARKET_IN_BREAK_TIME:
                    return CORE_RejectReasonCode.Code_50006;

                default:
                    return Code;
            }
        }

        // Xử lý send kafka khi gửi sang Sở thành công với msg: 35=N03
        private void ResponseSend2HNX_35N03_MessageReposFirm(MessageReposFirm p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35N03_MessageReposFirm: start process before send exchange 35={MessageType.ReposFirm} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side},  Account(1)={p_Message.Account}, EffectiveTime(168)={p_Message.EffectiveTime}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, NoSide(552)={p_Message.NoSide}");

                //
                string p_OrderNo = "";
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                //
                FirmReposModel _Response = new FirmReposModel();
                _Response.MsgType = CORE_MsgType.MsgRS;
                _Response.OrderNo = p_OrderNo;
                _Response.RefMsgType = MessageType.ReposFirm;
                _Response.ExchangeID = "";
                _Response.RefExchangeID = p_Message.RFQReqID;
                _Response.QuoteType = p_Message.QuoteType.ToString();
                _Response.OrdType = p_Message.OrdType;
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.OrderPartyID = "";
                _Response.InquiryMember = "";
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                else
                    _Response.Side = "";
                //
                _Response.EffectiveTime = p_Message.EffectiveTime;
                _Response.RepurchaseTerm = p_Message.RepurchaseTerm;
                _Response.RepurchaseRate = p_Message.RepurchaseRate;
                _Response.SettleDate1 = p_Message.SettlDate;
                _Response.SettleDate2 = p_Message.SettlDate2;
                _Response.EndDate = p_Message.EndDate;
                _Response.SettleMethod = p_Message.SettlMethod;
                _Response.ClientID = p_Message.Account;
                _Response.ClientIDCounterFirm = "";
                _Response.MemberCounterFirm = "";
                _Response.NoSide = p_Message.NoSide;
                //
                // Duyệt gửi ra
                List<ReposSideListResponse> listSymbolFirmInfo = null;
                if (p_Message.RepoSideList != null && p_Message.RepoSideList.Count > 0)
                {
                    listSymbolFirmInfo = new List<ReposSideListResponse>();
                    ReposSide itemSite;
                    for (int i = 0; i < p_Message.RepoSideList.Count; i++)
                    {
                        itemSite = p_Message.RepoSideList[i];
                        //
                        ReposSideListResponse _ReposSideListResponse = new ReposSideListResponse();
                        _ReposSideListResponse.NumSide = itemSite.NumSide;
                        _ReposSideListResponse.Symbol = itemSite.Symbol;
                        _ReposSideListResponse.OrderQty = itemSite.OrderQty;
                        _ReposSideListResponse.ExecPrice = 0;
                        _ReposSideListResponse.MergePrice = itemSite.Price;
                        _ReposSideListResponse.ReposInterest = 0;
                        _ReposSideListResponse.HedgeRate = itemSite.HedgeRate;
                        _ReposSideListResponse.SettleValue1 = 0.0;
                        _ReposSideListResponse.SettleValue2 = 0.0;
                        //
                        listSymbolFirmInfo.Add(_ReposSideListResponse);
                    }
                }
                //
                _Response.SymbolFirmInfo = listSymbolFirmInfo;
                //
                _Response.MatchReportType = 0;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35N03_MessageReposFirm -> End process after send exchange message 35={MessageType.ReposFirm} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side},  Account(1)={p_Message.Account}, EffectiveTime(168)={p_Message.EffectiveTime}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, NoSide(552)={p_Message.NoSide}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35N03_MessageReposFirm -> Error process after send exchange 35={MessageType.ReposFirm} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side},  Account(1)={p_Message.Account}, EffectiveTime(168)={p_Message.EffectiveTime}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, NoSide(552)={p_Message.NoSide}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= N05
        private void ResponseSend2HNX_35N05_MessageReposFirmAccept(MessageReposFirmAccept p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35N05_MessageReposFirmAccept: start process before send exchange 35={MessageType.ReposFirmAccept} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Account(1)={p_Message.Account}, CoAccount(2)={p_Message.CoAccount}");

                //
                string p_OrderNo = "";
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                //
                FirmReposModel _Response = new FirmReposModel();
                _Response.MsgType = CORE_MsgType.MsgRS;
                _Response.OrderNo = p_OrderNo;
                _Response.RefMsgType = MessageType.ReposFirmAccept;
                _Response.ExchangeID = "";
                _Response.RefExchangeID = p_Message.RFQReqID;
                _Response.QuoteType = p_Message.QuoteType.ToString();
                _Response.OrdType = p_Message.OrdType;
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.OrderPartyID = "";
                _Response.InquiryMember = "";
                _Response.Side = "";
                //
                _Response.EffectiveTime = "";
                _Response.RepurchaseTerm = 0;
                _Response.RepurchaseRate = 0.0;
                _Response.SettleDate1 = "";
                _Response.SettleDate2 = "";
                _Response.EndDate = "";
                _Response.SettleMethod = 0;
                _Response.ClientID = p_Message.Account;
                _Response.ClientIDCounterFirm = p_Message.CoAccount;
                _Response.MemberCounterFirm = "";
				_Response.RepurchaseRate = p_Message.RepurchaseRate;
				_Response.NoSide = p_Message.NoSide;
				//
				// Duyệt gửi ra
				List<ReposSideListResponse> listSymbolFirmInfo = null;
				if (p_Message.RepoSideList != null && p_Message.RepoSideList.Count > 0)
				{
					listSymbolFirmInfo = new List<ReposSideListResponse>();
					ReposSide itemSite;
					for (int i = 0; i < p_Message.RepoSideList.Count; i++)
					{
						itemSite = p_Message.RepoSideList[i];
						//
						ReposSideListResponse _ReposSideListResponse = new ReposSideListResponse();
						_ReposSideListResponse.NumSide = itemSite.NumSide;
						_ReposSideListResponse.Symbol = itemSite.Symbol;
						_ReposSideListResponse.OrderQty = itemSite.OrderQty;
						_ReposSideListResponse.ExecPrice = 0;
						_ReposSideListResponse.MergePrice = itemSite.Price;
						_ReposSideListResponse.ReposInterest = itemSite.ReposInterest;
						_ReposSideListResponse.HedgeRate = itemSite.HedgeRate;
						_ReposSideListResponse.SettleValue1 = 0.0;
						_ReposSideListResponse.SettleValue2 = 0.0;
						//
						listSymbolFirmInfo.Add(_ReposSideListResponse);
					}
				}
				//
				_Response.SymbolFirmInfo = listSymbolFirmInfo;
				//
				_Response.MatchReportType = 0;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35N05_MessageReposFirmAccept: End process before send exchange message 35={MessageType.ReposFirmAccept} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35N05_MessageReposFirmAccept: Error process before send exchange 35={MessageType.ReposFirmAccept} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Account(1)={p_Message.Account}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka khi gửi sang Sở thành công với msg: 35=MA
        private void ResponseSend2HNX_35MA_MessageReposBCGD(MessageReposBCGD p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35MA_MessageReposBCGD: start process before send exchange 35={MessageType.ReposBCGD} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgOrderID(198)={p_Message.OrgOrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side},  Account(1)={p_Message.Account},  CoAccount(2)={p_Message.CoAccount}, PartyID(448)={p_Message.PartyID}, CoPartyID(449)={p_Message.CoPartyID}, EffectiveTime(168)={p_Message.EffectiveTime}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, NoSide(552)={p_Message.NoSide}");

                //
                string p_OrderNo = "";
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                //
                FirmReposModel _Response = new FirmReposModel();
                _Response.MsgType = CORE_MsgType.MsgRS;
                _Response.OrderNo = p_OrderNo;
                _Response.RefMsgType = MessageType.ReposBCGD;
                _Response.ExchangeID = "";
                if (p_Message.QuoteType == 1)
                    _Response.RefExchangeID = "";
                if (p_Message.QuoteType == 3)
                    _Response.RefExchangeID = p_Message.OrgOrderID;
                _Response.QuoteType = p_Message.QuoteType.ToString();
                _Response.OrdType = p_Message.OrdType;
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.OrderPartyID = "";
                _Response.InquiryMember = "";
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                //
                _Response.EffectiveTime = p_Message.EffectiveTime;
                _Response.RepurchaseTerm = p_Message.RepurchaseTerm;
                _Response.RepurchaseRate = p_Message.RepurchaseRate;
                _Response.SettleDate1 = p_Message.SettlDate;
                _Response.SettleDate2 = p_Message.SettlDate2;
                _Response.EndDate = p_Message.EndDate;
                _Response.SettleMethod = p_Message.SettlMethod;

                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                {
                    _Response.ClientID = p_Message.CoAccount;
                    _Response.ClientIDCounterFirm = p_Message.Account;
                    _Response.MemberCounterFirm = p_Message.PartyID;
                }
                if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                {
                    _Response.ClientID = p_Message.Account;
                    _Response.ClientIDCounterFirm = p_Message.CoAccount;
                    _Response.MemberCounterFirm = p_Message.CoPartyID;
                }
                _Response.NoSide = p_Message.NoSide;
                //
                // Duyệt gửi ra
                List<ReposSideListResponse> listSymbolFirmInfo = null;
                if (p_Message.RepoSideList != null && p_Message.RepoSideList.Count > 0)
                {
                    listSymbolFirmInfo = new List<ReposSideListResponse>();
                    ReposSide itemSite;
                    for (int i = 0; i < p_Message.RepoSideList.Count; i++)
                    {
                        itemSite = p_Message.RepoSideList[i];
                        //
                        ReposSideListResponse _ReposSideListResponse = new ReposSideListResponse();
                        _ReposSideListResponse.NumSide = itemSite.NumSide;
                        _ReposSideListResponse.Symbol = itemSite.Symbol;
                        _ReposSideListResponse.OrderQty = itemSite.OrderQty;
                        _ReposSideListResponse.ExecPrice = 0;
                        _ReposSideListResponse.MergePrice = itemSite.Price;
                        _ReposSideListResponse.ReposInterest = 0;
                        _ReposSideListResponse.HedgeRate = itemSite.HedgeRate;
                        _ReposSideListResponse.SettleValue1 = 0.0;
                        _ReposSideListResponse.SettleValue2 = 0.0;
                        //
                        listSymbolFirmInfo.Add(_ReposSideListResponse);
                    }
                }
                //
                _Response.SymbolFirmInfo = listSymbolFirmInfo;
                //
                _Response.MatchReportType = 0;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35MA_MessageReposBCGD -> End process after send exchange message 35={MessageType.ReposBCGD} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35MA_MessageReposBCGD -> Error process after send exchange 35={MessageType.ReposBCGD} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgOrderID(198)={p_Message.OrgOrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side},  Account(1)={p_Message.Account},  CoAccount(2)={p_Message.CoAccount}, PartyID(448)={p_Message.PartyID}, CoPartyID(449)={p_Message.CoPartyID}, EffectiveTime(168)={p_Message.EffectiveTime}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, NoSide(552)={p_Message.NoSide}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= ME
        private void ResponseSend2HNX_35ME_MessageReposBCGDModify(MessageReposBCGDModify p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35ME_MessageReposBCGDModify: start process before send exchange 35={MessageType.ReposBCGDModify} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgOrderID(198)={p_Message.OrgOrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side},  Account(1)={p_Message.Account},  CoAccount(2)={p_Message.CoAccount}, PartyID(448)={p_Message.PartyID}, CoPartyID(449)={p_Message.CoPartyID}, EffectiveTime(168)={p_Message.EffectiveTime}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, NoSide(552)={p_Message.NoSide}");

                //
                string p_OrderNo = "";
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                //
                FirmReposModel _Response = new FirmReposModel();
                _Response.MsgType = CORE_MsgType.MsgRS;
                _Response.OrderNo = p_OrderNo;
                _Response.RefMsgType = MessageType.ReposBCGDModify;
                _Response.ExchangeID = "";
                _Response.RefExchangeID = p_Message.OrgOrderID;
                _Response.QuoteType = p_Message.QuoteType.ToString();
                _Response.OrdType = p_Message.OrdType;
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.OrderPartyID = "";
                _Response.InquiryMember = "";
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                //
                _Response.EffectiveTime = p_Message.EffectiveTime;
                _Response.RepurchaseTerm = p_Message.RepurchaseTerm;
                _Response.RepurchaseRate = p_Message.RepurchaseRate;
                _Response.SettleDate1 = p_Message.SettlDate;
                _Response.SettleDate2 = p_Message.SettlDate2;
                _Response.EndDate = p_Message.EndDate;
                _Response.SettleMethod = p_Message.SettlMethod;

                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                {
                    _Response.ClientID = p_Message.CoAccount;
                    _Response.ClientIDCounterFirm = p_Message.Account;
                    _Response.MemberCounterFirm = p_Message.PartyID;
                }
                if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                {
                    _Response.ClientID = p_Message.Account;
                    _Response.ClientIDCounterFirm = p_Message.CoAccount;
                    _Response.MemberCounterFirm = p_Message.CoPartyID;
                }
                _Response.NoSide = p_Message.NoSide;
                //
                // Duyệt gửi ra
                List<ReposSideListResponse> listSymbolFirmInfo = null;
                if (p_Message.RepoSideList != null && p_Message.RepoSideList.Count > 0)
                {
                    listSymbolFirmInfo = new List<ReposSideListResponse>();
                    ReposSide itemSite;
                    for (int i = 0; i < p_Message.RepoSideList.Count; i++)
                    {
                        itemSite = p_Message.RepoSideList[i];
                        //
                        ReposSideListResponse _ReposSideListResponse = new ReposSideListResponse();
                        _ReposSideListResponse.NumSide = itemSite.NumSide;
                        _ReposSideListResponse.Symbol = itemSite.Symbol;
                        _ReposSideListResponse.OrderQty = itemSite.OrderQty;
                        _ReposSideListResponse.ExecPrice = 0;
                        _ReposSideListResponse.MergePrice = itemSite.Price;
                        _ReposSideListResponse.ReposInterest = 0;
                        _ReposSideListResponse.HedgeRate = itemSite.HedgeRate;
                        _ReposSideListResponse.SettleValue1 = 0.0;
                        _ReposSideListResponse.SettleValue2 = 0.0;
                        //
                        listSymbolFirmInfo.Add(_ReposSideListResponse);
                    }
                }
                //
                _Response.SymbolFirmInfo = listSymbolFirmInfo;
                //
                _Response.MatchReportType = 0;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35ME_MessageReposBCGDModify: End process before send exchange message 35={MessageType.ReposBCGD} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35ME_MessageReposBCGDModify: Error process before send exchange 35={MessageType.ReposBCGDModify} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgOrderID(198)={p_Message.OrgOrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side},  Account(1)={p_Message.Account},  CoAccount(2)={p_Message.CoAccount}, PartyID(448)={p_Message.PartyID}, CoPartyID(449)={p_Message.CoPartyID}, EffectiveTime(168)={p_Message.EffectiveTime}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, NoSide(552)={p_Message.NoSide}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= MC
        private void ResponseSend2HNX_35MC_MessageReposBCGDCancel(MessageReposBCGDCancel p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35MC_MessageReposBCGDCancel: start process before send exchange 35={MessageType.ReposBCGDCancel} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgOrderID(198)={p_Message.OrgOrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side}");

                //
                string p_OrderNo = "";
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                //
                FirmReposModel _Response = new FirmReposModel();
                _Response.MsgType = CORE_MsgType.MsgRS;
                _Response.OrderNo = p_OrderNo;
                _Response.RefMsgType = MessageType.ReposBCGDCancel;
                _Response.ExchangeID = "";
                _Response.RefExchangeID = p_Message.OrgOrderID;
                _Response.QuoteType = p_Message.QuoteType.ToString();
                _Response.OrdType = p_Message.OrdType;
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.OrderPartyID = "";
                _Response.InquiryMember = "";
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                //
                _Response.EffectiveTime = "";
                _Response.RepurchaseTerm = 0;
                _Response.RepurchaseRate = 0.0;
                _Response.SettleDate1 = "";
                _Response.SettleDate2 = "";
                _Response.EndDate = "";
                _Response.SettleMethod = 0;
                _Response.ClientID = "";
                _Response.ClientIDCounterFirm = "";
                _Response.MemberCounterFirm = "";
                _Response.NoSide = 0;
                //
                List<ReposSideListResponse> listSymbolFirmInfo = new List<ReposSideListResponse>();
                ReposSideListResponse _ReposSideListResponse = new ReposSideListResponse();
                _ReposSideListResponse.NumSide = 0;
                _ReposSideListResponse.Symbol = "";
                _ReposSideListResponse.OrderQty = 0;
                _ReposSideListResponse.ExecPrice = 0;
                _ReposSideListResponse.MergePrice = 0;
                _ReposSideListResponse.ReposInterest = 0;
                _ReposSideListResponse.HedgeRate = 0.0;
                _ReposSideListResponse.SettleValue1 = 0.0;
                _ReposSideListResponse.SettleValue2 = 0.0;
                //
                listSymbolFirmInfo.Add(_ReposSideListResponse);
                //
                _Response.SymbolFirmInfo = listSymbolFirmInfo;
                //
                _Response.MatchReportType = 0;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35MC_MessageReposBCGDCancel: End process before send exchange message 35={MessageType.ReposBCGDCancel} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35MC_MessageReposBCGDCancel: Error process before send exchange 35={MessageType.ReposBCGDCancel} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgOrderID(198)={p_Message.OrgOrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= D
        private void ResponseSend2HNX_35D_MessageNewOrder(MessageNewOrder p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35D_MessageNewOrder: start process before send exchange 35={MessageType.NewOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, Account(1)={p_Message.Account}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, OrdType(40)={p_Message.Side}, OrderQty(38)={p_Message.OrderQty}, OrderQty2(192)={p_Message.OrderQty2}, Price(44)={p_Message.Price}, Price2(640)={p_Message.Price2}, Price2(640)={p_Message.Price2}, SpecialType(440)={p_Message.SpecialType}");

                //
                string p_OrderNo = "";
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                //
                NormalObjectModel _Response = new NormalObjectModel();
                _Response.MsgType = CORE_MsgType.MsgNS;
				_Response.RefMsgType = CORE_RefMsgType.RefMsgType_D;
				_Response.OrderNo = p_OrderNo;
                _Response.ExchangeID = "";
                _Response.RefExchangeID = "";
                _Response.OrderType = p_Message.OrdType;
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                //
                _Response.Symbol = p_Message.Symbol;
                _Response.OrderQty = p_Message.OrderQty;
                _Response.OrgOrderQty = 0;
                _Response.LeavesQty = 0;
                _Response.LastQty = 0;
                _Response.Price = p_Message.Price2;
                _Response.ClientID = p_Message.Account;
                _Response.SettleValue = 0;
                _Response.OrderQtyMM2 = p_Message.OrderQty2;
                _Response.PriceMM2 = p_Message.Price;
                _Response.SpecialType = p_Message.SpecialType;
                //
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35D_MessageNewOrder: End process before send exchange message 35={MessageType.NewOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35D_MessageNewOrder: Error process before send exchange 35={MessageType.NewOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, Account(1)={p_Message.Account}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, OrdType(40)={p_Message.Side}, OrderQty(38)={p_Message.OrderQty}, OrderQty2(192)={p_Message.OrderQty2}, Price(44)={p_Message.Price}, Price2(640)={p_Message.Price2}, Price2(640)={p_Message.Price2}, SpecialType(440)={p_Message.SpecialType}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= G
        private void ResponseSend2HNX_35G_MessageReplaceOrder(MessageReplaceOrder p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35G_MessageReplaceOrder: start process before send exchange 35={MessageType.ReplaceOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrigClOrdID(41)={p_Message.OrigClOrdID}, Account(1)={p_Message.Account}, Symbol(55)={p_Message.Symbol}, OrderQty(38)={p_Message.OrderQty}, OrgOrderQty(2238)={p_Message.OrgOrderQty}, Price2(640)={p_Message.Price2}");

                //
                string p_OrderNo = "";
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                //
                NormalObjectModel _Response = new NormalObjectModel();
                _Response.MsgType = CORE_MsgType.MsgNS;
				_Response.RefMsgType = CORE_RefMsgType.RefMsgType_G;
				_Response.OrderNo = p_OrderNo;
                _Response.ExchangeID = "";
                _Response.RefExchangeID = p_Message.OrigClOrdID;
                _Response.OrderType = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.Side = "";
                //
                _Response.Symbol = p_Message.Symbol;
                _Response.OrderQty = p_Message.OrderQty;
                _Response.OrgOrderQty = p_Message.OrgOrderQty;
                _Response.LeavesQty = 0;
                _Response.LastQty = 0;
                _Response.Price = p_Message.Price2;
                _Response.ClientID = p_Message.Account;
                _Response.SettleValue = 0;
                _Response.OrderQtyMM2 = 0;
                _Response.PriceMM2 = 0;
                _Response.SpecialType = 0;
                //
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35G_MessageReplaceOrder: End process before send exchange message 35={MessageType.ReplaceOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35G_MessageReplaceOrder: Error process before send exchange 35={MessageType.ReplaceOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrigClOrdID(41)={p_Message.OrigClOrdID}, Account(1)={p_Message.Account}, Symbol(55)={p_Message.Symbol}, OrderQty(38)={p_Message.OrderQty}, OrgOrderQty(2238)={p_Message.OrgOrderQty}, Price2(640)={p_Message.Price2}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= F
        private void ResponseSend2HNX_35F_MessageCancelOrder(MessageCancelOrder p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseSend2HNX_35F_MessageCancelOrder: start process before send exchange 35={MessageType.CancelOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrigClOrdID(41)={p_Message.OrigClOrdID}, Symbol(55)={p_Message.Symbol}");

                //
                string p_OrderNo = "";
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                //
                NormalObjectModel _Response = new NormalObjectModel();
                _Response.MsgType = CORE_MsgType.MsgNS;
				_Response.RefMsgType = CORE_RefMsgType.RefMsgType_F;
				_Response.OrderNo = p_OrderNo;
                _Response.ExchangeID = "";
                _Response.RefExchangeID = p_Message.OrigClOrdID;
                _Response.OrderType = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_TX;
                _Response.Side = "";
                //
                _Response.Symbol = p_Message.Symbol;
                _Response.OrderQty = 0;
                _Response.OrgOrderQty = 0;
                _Response.LeavesQty = 0;
                _Response.LastQty = 0;
                _Response.Price = 0;
                _Response.ClientID = "";
                _Response.SettleValue = 0;
                _Response.OrderQtyMM2 = 0;
                _Response.PriceMM2 = 0;
                _Response.SpecialType = 0;
                //
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseSend2HNX_35F_MessageCancelOrder: End process before send exchange message 35={MessageType.CancelOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseSend2HNX_35F_MessageCancelOrder: Error process before send exchange 35={MessageType.CancelOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrigClOrdID(41)={p_Message.OrigClOrdID}, Symbol(55)={p_Message.Symbol}, Exception: {ex?.ToString()}");
            }
        }
    }
}