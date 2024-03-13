/*
 * Project:
 * Author :
 * Summary: Lớp xử lý build data gửi sang kafka trước khi gửi sang sở
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 *
 */

using CommonLib;
using HNX.FIXMessage;
using KafkaInterface;
using LocalMemory;
using static CommonLib.CommonData;
using static CommonLib.CommonDataInCore;

namespace BusinessProcessResponse
{
    public partial class ResponseInterface : IResponseInterface
    {
        /// <summary>
        /// Phản hồi dữ liệu của API đặt vào gửi sang Kafka trước khi gửi sang Sở HNXExchange
        /// </summary>
        /// <param name="Msg"></param>
        public void ResponseApi2Kafka(FIXMessageBase Msg, int OrgSeq)
        {
            switch (Msg.GetMsgType)
            {
                case MessageType.Quote: // 35=S
                    ResponseApi2Kafka_35S_MessageQuote((MessageQuote)Msg, OrgSeq);
                    break;

                case MessageType.NewOrderCross: // 35=s
                    ResponseApi2Kafka_35s_MessageNewOrderCross((MessageNewOrderCross)Msg, OrgSeq);
                    break;

                case MessageType.QuoteCancel: // 35=Z
                    ResponseApi2Kafka_35Z_MessageQuoteCancel((MessageQuoteCancel)Msg, OrgSeq);
                    break;

                case MessageType.QuoteRequest: // 35=R
                    ResponseApi2Kafka_35R_MessageQuoteRequest((MessageQuoteRequest)Msg, OrgSeq);
                    break;

                case MessageType.QuoteResponse: // 35=AJ
                    ResponseApi2Kafka_35AJ_MessageQuoteResponse((MessageQuoteResponse)Msg, OrgSeq);
                    break;

                case MessageType.CrossOrderCancelReplaceRequest: // 35=t
                    ResponseApi2Kafka_35t_CrossOrderCancelReplaceRequest((CrossOrderCancelReplaceRequest)Msg, OrgSeq);
                    break;

                case MessageType.CrossOrderCancelRequest: // 35=u
                    ResponseApi2Kafka_35u_CrossOrderCancelRequest((CrossOrderCancelRequest)Msg, OrgSeq);
                    break;

                case MessageType.ReposInquiry: // 35=N01
                    ResponseApi2Kafka_35N01_NewInquiryRepos((MessageReposInquiry)Msg, OrgSeq);
                    break;

                case MessageType.ReposFirm: // 35=N03
                    ResponseApi2Kafka_35N03_MessageReposFirm((MessageReposFirm)Msg, OrgSeq);
                    break;

                case MessageType.ReposFirmAccept: // 35=N05
                    ResponseApi2Kafka_35N05_MessageReposFirmAccept((MessageReposFirmAccept)Msg, OrgSeq);
                    break;

                case MessageType.ReposBCGD: // 35=MA
                    ResponseApi2Kafka_35MA_MessageReposBCGD((MessageReposBCGD)Msg, OrgSeq);
                    break;

                case MessageType.ReposBCGDModify: // 35=ME
                    ResponseApi2Kafka_35ME_MessageReposBCGDModify((MessageReposBCGDModify)Msg, OrgSeq);
                    break;

                case MessageType.ReposBCGDCancel: // 35=MC
                    ResponseApi2Kafka_35MC_MessageReposBCGDCancel((MessageReposBCGDCancel)Msg, OrgSeq);
                    break;

                case MessageType.NewOrder: // 35=D
                    ResponseApi2Kafka_35D_MessageNewOrder((MessageNewOrder)Msg, OrgSeq);
                    break;

                case MessageType.ReplaceOrder: // 35=G
                    ResponseApi2Kafka_35G_MessageReplaceOrder((MessageReplaceOrder)Msg, OrgSeq);
                    break;

                case MessageType.CancelOrder: // 35=F
                    ResponseApi2Kafka_35F_MessageCancelOrder((MessageCancelOrder)Msg, OrgSeq);
                    break;
            }
        }

        // Xử lý send kafka trước khi sang sở với msg: 35=S
        private void ResponseApi2Kafka_35S_MessageQuote(MessageQuote p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35S_MessageQuote: start process before send exchange 35={MessageType.Quote} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, IsVisible={p_Message.IsVisible}");
                //
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderPartyID = "";
                _Response.OrderNo = p_Message.ClOrdID;
                _Response.RefExchangeID = "";
                _Response.ExchangeID = "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
                _Response.OrderPartyID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35S_MessageQuote: End process before send exchange message 35={MessageType.Quote} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, ClOrdID={p_Message.ClOrdID}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, IsVisible={p_Message.IsVisible}, SendingTime={_Response.SendingTime}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35S_MessageQuote: Error when  process before send exchange 35={MessageType.Quote} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, ClOrdID={p_Message.ClOrdID}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, IsVisible={p_Message.IsVisible}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi sang sở với msg: 35=s
        private void ResponseApi2Kafka_35s_MessageNewOrderCross(MessageNewOrderCross p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35s_MessageNewOrderCross: start process before send exchange 35={MessageType.NewOrderCross} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, CrossID={p_Message.CrossID}, EffectiveTime={p_Message.EffectiveTime}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}");
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
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35s_MessageNewOrderCross: End process before send exchange 35={MessageType.NewOrderCross} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, CrossID={p_Message.CrossID}, EffectiveTime={p_Message.EffectiveTime}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, SendingTime={_Response.SendingTime}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35s_MessageNewOrderCross: Error process before send exchange 35={MessageType.NewOrderCross} ID {p_Message.IDRequest} with OrderNo={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, CrossID={p_Message.CrossID}, EffectiveTime={p_Message.EffectiveTime}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, ClOrdID={p_Message.ClOrdID}, SettlMethod={p_Message.SettlMethod}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi sang sở với msg: 35=Z
        private void ResponseApi2Kafka_35Z_MessageQuoteCancel(MessageQuoteCancel p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35Z_MessageQuoteCancel: start process before send exchange 35={MessageType.QuoteCancel} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteID(171)={p_Message.QuoteID}, ClOrdID(11)={p_Message.ClOrdID}, QuoteCancelType={p_Message.QuoteCancelType}, Symbol={p_Message.Symbol}, OrdType={p_Message.OrdType}");
                //
                string p_OrderNo = "";
                // p_Message.QuoteID 171
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                else
                {
                    Logger.ResponseLog.Warn($"ResponseApi2Kafka_35Z_MessageQuoteCancel: Can't find order info on memory when received 35={MessageType.QuoteCancel} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteID(171)={p_Message.QuoteID}, ClOrdID(11)={p_Message.ClOrdID}, QuoteCancelType={p_Message.QuoteCancelType}, Symbol={p_Message.Symbol}, OrdType={p_Message.OrdType} before send exchange");
                }
                //
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderPartyID = "";
                _Response.OrderNo = p_OrderNo;
                _Response.RefExchangeID = !string.IsNullOrEmpty(p_Message.QuoteID) ? p_Message.QuoteID : "";
                _Response.ExchangeID = "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
                _Response.OrderPartyID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35Z_MessageQuoteCancel: End process before send exchange 35={MessageType.QuoteCancel} with ID {p_Message.IDRequest} MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteID(171)={p_Message.QuoteID}, ClOrdID(11)={p_Message.ClOrdID}, QuoteCancelType={p_Message.QuoteCancelType}, Symbol={p_Message.Symbol}, OrdType={p_Message.OrdType}, SendingTime={_Response.SendingTime}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35Z_MessageQuoteCancel: Error when process before send exchange 35={MessageType.QuoteCancel} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteID(171)={p_Message.QuoteID}, ClOrdID={p_Message.ClOrdID}, QuoteCancelType={p_Message.QuoteCancelType}, Symbol={p_Message.Symbol}, OrdType={p_Message.OrdType}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi sang sở với msg: 35=R
        private void ResponseApi2Kafka_35R_MessageQuoteRequest(MessageQuoteRequest p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35R_MessageQuoteRequest: start process before send exchange 35={MessageType.QuoteRequest} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID(644)={p_Message.RFQReqID}, OrderID(37)={p_Message.OrderID}, ClOrdID(11)={p_Message.ClOrdID}, OrdType={p_Message.OrdType}, Account={p_Message.Account},  Symbol={p_Message.Symbol}, TransactTime={p_Message.TransactTime}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}");
                //
                string p_OrderNo = "";
                //
                // p_Message.RFQReqID 644
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                else
                {
                    Logger.ResponseLog.Warn($"ResponseApi2Kafka_35R_MessageQuoteRequest: Can't find order info on memory when received 35={MessageType.QuoteRequest} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID(644)={p_Message.RFQReqID}, OrderID(37)={p_Message.OrderID}, ClOrdID(11)={p_Message.ClOrdID}, OrdType={p_Message.OrdType}, Account={p_Message.Account},  Symbol={p_Message.Symbol}, TransactTime={p_Message.TransactTime}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, before send exchange");
                }
                //
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderPartyID = "";
                _Response.OrderNo = p_OrderNo;
                _Response.RefExchangeID = p_Message.RFQReqID;
                _Response.ExchangeID = "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
                _Response.OrderPartyID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35R_MessageQuoteRequest: End process before send exchange 35={MessageType.QuoteRequest} with ID {p_Message.IDRequest} RFQReqID(644)={p_Message.RFQReqID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, OrderID={p_Message.OrderID}, Symbol={p_Message.Symbol}, TransactTime={p_Message.TransactTime}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, ClOrdID={p_Message.ClOrdID}, SendingTime={_Response.SendingTime}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35R_MessageQuoteRequest: Error when process before send exchange 35={MessageType.QuoteRequest} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID(644)={p_Message.RFQReqID}, OrderID(37)={p_Message.OrderID}, ClOrdID(11)={p_Message.ClOrdID}, OrdType={p_Message.OrdType}, Account={p_Message.Account},  Symbol={p_Message.Symbol}, TransactTime={p_Message.TransactTime}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}; Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi sang sở với msg: 35=AJ
        private void ResponseApi2Kafka_35AJ_MessageQuoteResponse(MessageQuoteResponse p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35AJ_MessageQuoteResponse: start process before send exchange 35={MessageType.QuoteResponse} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, QuoteRespType={p_Message.QuoteRespType}, QuoteRespID={p_Message.QuoteRespID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount(2)={p_Message.CoAccount}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}");
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
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
                _Response.OrderPartyID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35AJ_MessageQuoteResponse: End process before send exchange 35={MessageType.QuoteResponse} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, QuoteRespType={p_Message.QuoteRespType}, QuoteRespID={p_Message.QuoteRespID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, SendingTime={_Response.SendingTime}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35AJ_MessageQuoteResponse: Error when process before send exchange 35={MessageType.QuoteResponse} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, QuoteRespType={p_Message.QuoteRespType}, QuoteRespID={p_Message.QuoteRespID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}; Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi sang sở với msg: 35=t
        private void ResponseApi2Kafka_35t_CrossOrderCancelReplaceRequest(CrossOrderCancelReplaceRequest p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35t_CrossOrderCancelReplaceRequest: start process before send exchange 35={MessageType.CrossOrderCancelReplaceRequest} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID},  OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, OrderID={p_Message.OrderID}, EffectiveTime={p_Message.EffectiveTime}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}");
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
                    Logger.ResponseLog.Warn($"ResponseApi2Kafka_35t_CrossOrderCancelReplaceRequest: Error find order info when process 35={MessageType.CrossOrderCancelReplaceRequest} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID},  OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, OrderID={p_Message.OrderID}, EffectiveTime={p_Message.EffectiveTime}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, before send exchange");
                }
                //
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderPartyID = "";
                _Response.OrderNo = p_OrderNo;
                _Response.RefExchangeID = !string.IsNullOrEmpty(p_Message.OrgCrossID) ? p_Message.OrgCrossID : "";
                _Response.ExchangeID = "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
                _Response.OrderPartyID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";

                // send kafka

                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);//
                
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35t_CrossOrderCancelReplaceRequest: End process before send exchange 35=t ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID},  OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, OrderID={p_Message.OrderID}, EffectiveTime={p_Message.EffectiveTime}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, SendingTime={_Response.SendingTime}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35t_CrossOrderCancelReplaceRequest: Error when process before send exchange 35={MessageType.CrossOrderCancelReplaceRequest} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, OrderID={p_Message.OrderID}, EffectiveTime={p_Message.EffectiveTime}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi sang sở với msg: 35=u
        private void ResponseApi2Kafka_35u_CrossOrderCancelRequest(CrossOrderCancelRequest p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35u_CrossOrderCancelRequest: start process before send exchange 35={MessageType.CrossOrderCancelRequest} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, CrossType={p_Message.CrossType}, OrdType={p_Message.OrdType}, Symbol={p_Message.Symbol}, Side={p_Message.Side}");
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
                    Logger.ResponseLog.Warn($"ResponseApi2Kafka_35u_CrossOrderCancelRequest: Error find order info when process 35={MessageType.CrossOrderCancelRequest} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, CrossType={p_Message.CrossType}, OrdType={p_Message.OrdType}, Symbol={p_Message.Symbol}, Side={p_Message.Side} before send exchange");
                }
                //
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderPartyID = "";
                _Response.OrderNo = p_OrderNo;
                _Response.RefExchangeID = !string.IsNullOrEmpty(p_Message.OrgCrossID) ? p_Message.OrgCrossID : "";
                _Response.ExchangeID = "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(DateTime.Now);
                _Response.OrderPartyID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";

                // send kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35u_CrossOrderCancelRequest: End process before send exchange 35={MessageType.CrossOrderCancelRequest} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, CrossType={p_Message.CrossType}, OrdType={p_Message.OrdType}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, SendingTime={_Response.SendingTime}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35u_CrossOrderCancelRequest: Error when process before send exchange 35={MessageType.CrossOrderCancelRequest} ID {p_Message.IDRequest} with MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, CrossType={p_Message.CrossType}, OrdType={p_Message.OrdType}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= N01
        private void ResponseApi2Kafka_35N01_NewInquiryRepos(MessageReposInquiry p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35N01_NewInquiryRepos: start process before send exchange 35={MessageType.ReposInquiry} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, Symbol={p_Message.Symbol}, QuoteType={p_Message.QuoteType}, OrdType={p_Message.OrdType}, Side={p_Message.Side},  OrderQty={p_Message.OrderQty}, EffectiveTime={p_Message.EffectiveTime}, SettlMethod={p_Message.SettlMethod}, SettlDate={p_Message.SettlDate}, SettlDate2={p_Message.SettlDate2}, EndDate={p_Message.EndDate}, RepurchaseTerm={p_Message.RepurchaseTerm}, RegistID={p_Message.RegistID}, RFQReqID={p_Message.RFQReqID}");
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
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
                _Response.OrderPartyID = "";
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                else
                    _Response.Side = "";
                //
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
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35N01_NewInquiryRepos: End process before send exchange message 35={MessageType.ReposInquiry} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35N01_NewInquiryRepos: Error when  process before send exchange 35={MessageType.ReposInquiry} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, Symbol={p_Message.Symbol}, QuoteType={p_Message.QuoteType}, OrdType={p_Message.OrdType}, Side={p_Message.Side},  OrderQty={p_Message.OrderQty}, EffectiveTime={p_Message.EffectiveTime}, SettlMethod={p_Message.SettlMethod}, SettlDate={p_Message.SettlDate}, SettlDate2={p_Message.SettlDate2}, EndDate={p_Message.EndDate}, RepurchaseTerm={p_Message.RepurchaseTerm}, RegistID={p_Message.RegistID}, RFQReqID={p_Message.RFQReqID}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= N03
        private void ResponseApi2Kafka_35N03_MessageReposFirm(MessageReposFirm p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35N03_MessageReposFirm: start process before send exchange 35={MessageType.ReposFirm} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side},  Account(1)={p_Message.Account}, EffectiveTime(168)={p_Message.EffectiveTime}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, NoSide(552)={p_Message.NoSide}");

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
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35N03_MessageReposFirm: End process before send exchange message 35={MessageType.ReposFirm} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35N03_MessageReposFirm: Error process before send exchange 35={MessageType.ReposFirm} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side},  Account(1)={p_Message.Account}, EffectiveTime(168)={p_Message.EffectiveTime}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, NoSide(552)={p_Message.NoSide}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= N05
        private void ResponseApi2Kafka_35N05_MessageReposFirmAccept(MessageReposFirmAccept p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35N05_MessageReposFirmAccept: start process before send exchange 35={MessageType.ReposFirmAccept} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Account(1)={p_Message.Account}, CoAccount(2)={p_Message.CoAccount}");

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
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35N05_MessageReposFirmAccept: End process before send exchange message 35={MessageType.ReposFirmAccept} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35N05_MessageReposFirmAccept: Error process before send exchange 35={MessageType.ReposFirmAccept} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Account(1)={p_Message.Account}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= MA
        private void ResponseApi2Kafka_35MA_MessageReposBCGD(MessageReposBCGD p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35MA_MessageReposBCGD: start process before send exchange 35={MessageType.ReposBCGD} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgOrderID(198)={p_Message.OrgOrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side},  Account(1)={p_Message.Account},  CoAccount(2)={p_Message.CoAccount}, PartyID(448)={p_Message.PartyID}, CoPartyID(449)={p_Message.CoPartyID}, EffectiveTime(168)={p_Message.EffectiveTime}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, NoSide(552)={p_Message.NoSide}");

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
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35MA_MessageReposBCGD: End process before send exchange message 35={MessageType.ReposBCGD} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35MA_MessageReposBCGD: Error process before send exchange 35={MessageType.ReposBCGD} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgOrderID(198)={p_Message.OrgOrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side},  Account(1)={p_Message.Account},  CoAccount(2)={p_Message.CoAccount}, PartyID(448)={p_Message.PartyID}, CoPartyID(449)={p_Message.CoPartyID}, EffectiveTime(168)={p_Message.EffectiveTime}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, NoSide(552)={p_Message.NoSide}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= ME
        private void ResponseApi2Kafka_35ME_MessageReposBCGDModify(MessageReposBCGDModify p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35ME_MessageReposBCGDModify: start process before send exchange 35={MessageType.ReposBCGDModify} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgOrderID(198)={p_Message.OrgOrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side},  Account(1)={p_Message.Account},  CoAccount(2)={p_Message.CoAccount}, PartyID(448)={p_Message.PartyID}, CoPartyID(449)={p_Message.CoPartyID}, EffectiveTime(168)={p_Message.EffectiveTime}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, NoSide(552)={p_Message.NoSide}");

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
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35ME_MessageReposBCGDModify: End process before send exchange message 35={MessageType.ReposBCGD} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35ME_MessageReposBCGDModify: Error process before send exchange 35={MessageType.ReposBCGDModify} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgOrderID(198)={p_Message.OrgOrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side},  Account(1)={p_Message.Account},  CoAccount(2)={p_Message.CoAccount}, PartyID(448)={p_Message.PartyID}, CoPartyID(449)={p_Message.CoPartyID}, EffectiveTime(168)={p_Message.EffectiveTime}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(917)={p_Message.EndDate}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, NoSide(552)={p_Message.NoSide}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= MC
        private void ResponseApi2Kafka_35MC_MessageReposBCGDCancel(MessageReposBCGDCancel p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35MC_MessageReposBCGDCancel: start process before send exchange 35={MessageType.ReposBCGDCancel} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgOrderID(198)={p_Message.OrgOrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side}");

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
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35MC_MessageReposBCGDCancel: End process before send exchange message 35={MessageType.ReposBCGDCancel} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35MC_MessageReposBCGDCancel: Error process before send exchange 35={MessageType.ReposBCGDCancel} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrgOrderID(198)={p_Message.OrgOrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType}, Side(54)={p_Message.Side}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= D
        private void ResponseApi2Kafka_35D_MessageNewOrder(MessageNewOrder p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35D_MessageNewOrder: start process before send exchange 35={MessageType.NewOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, Account(1)={p_Message.Account}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, OrdType(40)={p_Message.Side}, OrderQty(38)={p_Message.OrderQty}, OrderQty2(192)={p_Message.OrderQty2}, Price(44)={p_Message.Price}, Price2(640)={p_Message.Price2}, Price2(640)={p_Message.Price2}, SpecialType(440)={p_Message.SpecialType}");

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
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35D_MessageNewOrder: End process before send exchange message 35={MessageType.NewOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35D_MessageNewOrder: Error process before send exchange 35={MessageType.NewOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, Account(1)={p_Message.Account}, Symbol(55)={p_Message.Symbol}, Side(54)={p_Message.Side}, OrdType(40)={p_Message.Side}, OrderQty(38)={p_Message.OrderQty}, OrderQty2(192)={p_Message.OrderQty2}, Price(44)={p_Message.Price}, Price2(640)={p_Message.Price2}, Price2(640)={p_Message.Price2}, SpecialType(440)={p_Message.SpecialType}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= G
        private void ResponseApi2Kafka_35G_MessageReplaceOrder(MessageReplaceOrder p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35G_MessageReplaceOrder: start process before send exchange 35={MessageType.ReplaceOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrigClOrdID(41)={p_Message.OrigClOrdID}, Account(1)={p_Message.Account}, Symbol(55)={p_Message.Symbol}, OrderQty(38)={p_Message.OrderQty}, OrgOrderQty(2238)={p_Message.OrgOrderQty}, Price2(640)={p_Message.Price2}");

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
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35G_MessageReplaceOrder: End process before send exchange message 35={MessageType.ReplaceOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35G_MessageReplaceOrder: Error process before send exchange 35={MessageType.ReplaceOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrigClOrdID(41)={p_Message.OrigClOrdID}, Account(1)={p_Message.Account}, Symbol(55)={p_Message.Symbol}, OrderQty(38)={p_Message.OrderQty}, OrgOrderQty(2238)={p_Message.OrgOrderQty}, Price2(640)={p_Message.Price2}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý send kafka trước khi gửi sang sở với msg : 35= F
        private void ResponseApi2Kafka_35F_MessageCancelOrder(MessageCancelOrder p_Message, int OrgSeq)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35F_MessageCancelOrder: start process before send exchange 35={MessageType.CancelOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrigClOrdID(41)={p_Message.OrigClOrdID}, Symbol(55)={p_Message.Symbol}");

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
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_QE;
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
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, OrgSeq, FlagSendKafka.ACCEPT_REQUEST);
                //
                Logger.ResponseLog.Info($"ResponseApi2Kafka_35F_MessageCancelOrder: End process before send exchange message 35={MessageType.CancelOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseApi2Kafka_35F_MessageCancelOrder: Error process before send exchange 35={MessageType.CancelOrder} ID {p_Message.IDRequest} with ClOrdID(11)={p_Message.ClOrdID}, MsgSeqNum(34)={p_Message.MsgSeqNum}, OrigClOrdID(41)={p_Message.OrigClOrdID}, Symbol(55)={p_Message.Symbol}, Exception: {ex?.ToString()}");
            }
        }
    }
}