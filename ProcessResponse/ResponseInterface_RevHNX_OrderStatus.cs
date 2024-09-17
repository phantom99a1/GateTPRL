/*
 * Project:
 * Author :
 * Summary: Lớp xử lý dữ liệu để gửi sang Kafka khi Nhận được message từ sở
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 *
 */

using CommonLib;
using Confluent.Kafka;
using HNX.FIXMessage;
using KafkaInterface;
using LocalMemory;
using StorageProcess;
using static CommonLib.CommonData;
using static CommonLib.CommonDataInCore;
using static HNX.FIXMessage.MessageReposBCGDReport;

namespace BusinessProcessResponse
{
    public partial class ResponseInterface : IResponseInterface
    {
        // sở gửi 35=AI
        public void HNXSendQuoteStatusReport(MessageQuoteSatusReport p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> Start process message when received exchange 35=AI|4488={p_Message.OrderPartyID}|537={p_Message.QuoteType}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, GetSendingTime= {p_Message.GetSendingTime}, QuoteReqID={p_Message.QuoteReqID}, OrdType={p_Message.OrdType}, QuoteID={p_Message.QuoteID}, Account={p_Message.Account}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price},  Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, , OrderPartyID={p_Message.OrderPartyID}");
                //
                if (p_Message.OrderPartyID == ConfigData.FirmID && p_Message.QuoteType == CORE_QuoteType.Bao_Co_Dien_Tu_Moi)
                {
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Co_Dien_Tu_Moi}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}");
                    // Sở gử 35=AI ( 4488= của mình , 537=1) => phản hồi của lệnh đặt do thành viên gửi lên
                    // Cập nhật exchangeID vào thông tin memory vừa tìm được. Trả thông tin cho kafka
                    OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                    if (objOrder != null)
                    {
                        // Cập nhật thông tin ExchangeID vào Memory
                        //objOrder.ExchangeID = p_Message.QuoteID;
                        OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.QuoteID);
                        // Build object để gửi kafka
                        ResponseMessageKafka _Response = new ResponseMessageKafka();
                        _Response.MsgType = CORE_MsgType.MsgOS;
                        _Response.OrderNo = objOrder.OrderNo;
                        _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                        _Response.OrderPartyID = p_Message.OrderPartyID;
                        _Response.RefExchangeID = "";
                        _Response.ExchangeID = p_Message.QuoteID;
                        _Response.OrderStatus = CORE_OrderStatus.OrderStatus_RE;
                        _Response.RefMsgType = CORE_RefMsgType.RefMsgType_S;
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

                        // Gọi hàm gửi kafka
                        c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                        //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                        p_Message.OrderNo = _Response.OrderNo;
                        SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                        //
                        Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process send kafka 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Co_Dien_Tu_Moi}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} when received exchange; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"Error can't find order info when received 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Co_Dien_Tu_Moi}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} when received exchange");
                        objOrder = new OrderInfo()
                        {
                            OrderNo = p_Message.ClOrdID,
                            RefMsgType = MessageType.Quote,
                            ExchangeID = p_Message.QuoteID,
                            Symbol = p_Message.Symbol,
                            Side = p_Message.Side == 1 ? "B" : "S",
                            Price = p_Message.Price,
                            OrderQty = p_Message.OrderQty,
                            ClientID = p_Message.Account,
                            OrderType = p_Message.OrdType,
                        };

                        OrderMemory.Add_NewOrder(objOrder);
                        ResponseMessageKafka _Response = new ResponseMessageKafka();
                        _Response.MsgType = CORE_MsgType.MsgOS;
                        _Response.OrderNo = objOrder.OrderNo;
                        _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                        _Response.OrderPartyID = p_Message.OrderPartyID;
                        _Response.RefExchangeID = "";
                        _Response.ExchangeID = p_Message.QuoteID;
                        _Response.OrderStatus = CORE_OrderStatus.OrderStatus_RE;
                        _Response.RefMsgType = CORE_RefMsgType.RefMsgType_S;
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
                        // Gọi hàm gửi kafka
                        c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                        //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                        p_Message.OrderNo = _Response.OrderNo;
                        SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);

                        //
                        Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process send kafka 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Co_Dien_Tu_Moi}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} when received exchange; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                    }
                    //
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> End process message 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Co_Dien_Tu_Moi}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} when received exchange");
                }
                else if (p_Message.OrderPartyID != ConfigData.FirmID && p_Message.QuoteType == CORE_QuoteType.Bao_Co_Dien_Tu_Moi)
                {
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Co_Dien_Tu_Moi}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} when received exchange");
                    // Sở gửi 35=AI ( 4488 # của mình , 537=1) => phản hồi của lệnh do thành viên gửi lên
                    OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                    if (objOrder != null)
                    {
                        // - nếu thấy-> lý thuyết khong thể xảy ra tình huống này. Cần ghi lỗi
                        Logger.ResponseLog.Warn($"HNXSendQuoteStatusReport -> Error find order info when received 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Co_Dien_Tu_Moi}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11) = {p_Message.ClOrdID}, QuoteType={p_Message.QuoteType} when received exchange");
                    }
                    else
                    {
                        // Add thông tin order vào memory
                        LocalMemory.OrderInfo OrderObjMem = new OrderInfo();
                        OrderObjMem.RefMsgType = MessageType.Quote;
                        OrderObjMem.OrderNo = "";
                        OrderObjMem.ClOrdID = "";
                        OrderObjMem.ExchangeID = p_Message.QuoteID;
                        OrderObjMem.RefExchangeID = "";
                        OrderObjMem.SeqNum = 0;
                        OrderObjMem.Symbol = p_Message.Symbol;
                        if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                            OrderObjMem.Side = ORDER_SIDE.SIDE_BUY;
                        else
                            OrderObjMem.Side = ORDER_SIDE.SIDE_SELL;

                        OrderObjMem.Price = p_Message.Price2;
                        OrderObjMem.OrderQty = p_Message.OrderQty;
                        OrderObjMem.CrossType = "0";
                        OrderObjMem.ClientID = "";
                        OrderObjMem.ClientIDCounterFirm = p_Message.Account;
                        OrderObjMem.MemberCounterFirm = p_Message.OrderPartyID;
                        //
                        OrderMemory.Add_NewOrder(OrderObjMem);
                        //
                        // Build object để gửi kafka
                        ResponseMessageKafka _Response = new ResponseMessageKafka();
                        _Response.MsgType = CORE_MsgType.MsgOS;
                        if (!string.IsNullOrEmpty(p_Message.ClOrdID))
                            _Response.OrderNo = OrderMemory.GetOrigOrder_byClOrdID(p_Message.ClOrdID);
                        _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                        _Response.OrderPartyID = p_Message.OrderPartyID;
                        _Response.RefExchangeID = "";
                        _Response.ExchangeID = p_Message.QuoteID;
                        _Response.OrderStatus = CORE_OrderStatus.OrderStatus_PA;
                        _Response.RefMsgType = CORE_RefMsgType.RefMsgType_S;
                        _Response.OrdType = p_Message.OrdType;
                        _Response.CrossType = 0;
                        _Response.ClientID = "";
                        _Response.ClientIDCounterFirm = p_Message.Account;
                        _Response.MemberCounterFirm = p_Message.OrderPartyID;
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
                        c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                        //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                        p_Message.OrderNo = _Response.OrderNo;
                        SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                        //
                        Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process send kafka 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Co_Dien_Tu_Moi}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} when received exchange;  sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                    }
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> End process message when received exchange 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Co_Dien_Tu_Moi}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}");
                }
                else if (p_Message.OrderPartyID != ConfigData.FirmID && p_Message.QuoteType == CORE_QuoteType.Bao_Dien_Tu_Da_Duoc_Chap_Nhan)
                {
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Dien_Tu_Da_Duoc_Chap_Nhan}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}");

                    // Sở gửi 35=AI ( 4488 # của mình , 537=4) => phản hồi của lệnh sửa do thành viên gửi lên

                    // "map trực tiếp từ mesage nhận được, gửi về kafka, không lưu mem
                    // Lấy tag 171 trong message 35 = AI map với thông tin exchangeID trong memory để tìm bản ghi lệnh đặt-- > lấy ra thông tin orderNo để trả ra KAFKA
                    // (về cơ bản case này chắc chắn ""orderNo"":"""" vì toàn là nhận về, chưa gửi cái gì lên nên k có số orderNo, dev xem xét cần lấy thông tin mem hay không vì nó như là hủy rồi)"
                    ResponseMessageKafka _Response = new ResponseMessageKafka();
                    _Response.MsgType = CORE_MsgType.MsgOS;
                    if (!string.IsNullOrEmpty(p_Message.ClOrdID))
                        _Response.OrderNo = OrderMemory.GetOrigOrder_byClOrdID(p_Message.ClOrdID);
                    else
                        _Response.OrderNo = "";
                    _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                    _Response.OrderPartyID = p_Message.OrderPartyID;
                    _Response.RefExchangeID = "";
                    _Response.ExchangeID = p_Message.QuoteID;
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_CL;
                    _Response.RefMsgType = CORE_RefMsgType.RefMsgType_S;
                    _Response.OrdType = p_Message.OrdType;
                    _Response.CrossType = 0;
                    _Response.ClientID = "";
                    _Response.ClientIDCounterFirm = p_Message.Account;
                    _Response.MemberCounterFirm = p_Message.OrderPartyID;
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
                    _Response.RejectReasonCode = CORE_RejectReasonCode.Code_50004;
                    _Response.RejectReason = CORE_RejectReason.RejectReason_50004;
                    //
                    c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                    //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                    p_Message.OrderNo = _Response.OrderNo;
                    SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                    //
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> End process message when received exchange 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Dien_Tu_Da_Duoc_Chap_Nhan}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                }
                // 35=AI 4488#003 & 537=5
                else if (p_Message.OrderPartyID != ConfigData.FirmID && p_Message.QuoteType == CORE_QuoteType.Bao_Huy_Lenh_Thoa_Thuan_Dien_Tu_Sua_Doi_Ung)
                {
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Huy_Lenh_Thoa_Thuan_Dien_Tu_Sua_Doi_Ung}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}");
                    // Sở gửi 35=AI ( 4488 # của mình , 537=5) => phản hồi của lệnh sửa do thành viên gửi lên

                    // "map trực tiếp từ mesage nhận được, gửi về kafka, không lưu mem
                    // Lấy tag 131 trong message 35 = AI map với thông tin exchangeID trong memory để tìm bản ghi lệnh đặt-- > lấy ra thông tin orderNo để trả ra KAFKA
                    // (về cơ bản case này chắc chắn ""orderNo"":"""" vì toàn là nhận về, chưa gửi cái gì lên nên k có số orderNo, dev dev xem xét cần lấy thông tin mem hay không vì nó như là hủy rồi)"
                    ResponseMessageKafka _Response = new ResponseMessageKafka();
                    _Response.MsgType = CORE_MsgType.MsgOS;
                    if (!string.IsNullOrEmpty(p_Message.ClOrdID))
                        _Response.OrderNo = OrderMemory.GetOrigOrder_byClOrdID(p_Message.ClOrdID);
                    else
                        _Response.OrderNo = "";
                    _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                    _Response.OrderPartyID = p_Message.OrderPartyID;
                    _Response.RefExchangeID = p_Message.QuoteReqID;
                    _Response.ExchangeID = p_Message.QuoteID;
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_CL;
                    _Response.RefMsgType = CORE_RefMsgType.RefMsgType_S;
                    _Response.OrdType = p_Message.OrdType;
                    _Response.CrossType = 0;
                    _Response.ClientID = "";
                    _Response.ClientIDCounterFirm = p_Message.Account;
                    _Response.MemberCounterFirm = p_Message.OrderPartyID;
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
                    _Response.RejectReasonCode = CORE_RejectReasonCode.Code_50005;
                    _Response.RejectReason = CORE_RejectReason.RejectReason_50005;
                    //
                    c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                    //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                    p_Message.OrderNo = _Response.OrderNo;
                    SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                    //
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Huy_Lenh_Thoa_Thuan_Dien_Tu_Sua_Doi_Ung}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                }
                else if (p_Message.OrderPartyID == ConfigData.FirmID && p_Message.QuoteType == CORE_QuoteType.Bao_Sua_Dien_Tu)
                {
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Sua_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}");
                    // Sở gử 35=AI ( 4488= của mình , 537=2) => phản hồi của lệnh sửa do thành viên gửi lên

                    // "lấy tag 131 trong message 35=AI map với thông tin exchangeID trong memory để tìm bản ghi đặt ban đầu.
                    // - nếu không thấy-> lý thuyết không thể xảy ra tình huống này. Cần ghi lỗi
                    // - nếu thấy, lấy thông tin orderno trong memory để tra ra kafka. Cập nhật symbol, side, price, orderqty, clientID vào thông tin memory vừa tìm được."

                    OrderInfo objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.QuoteReqID);
                    if (objOrder != null)
                    {
                        // Cập nhật thông tin ExchangeID vào Memory
                        /*objOrder.Symbol = p_Message.Symbol;*/
                        string _Side = "";
                        if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                            _Side = ORDER_SIDE.SIDE_BUY;
                        else
                            _Side = ORDER_SIDE.SIDE_SELL;
                        OrderMemory.UpdateOrderInfo(objOrder, p_Symbol: p_Message.Symbol, p_Side: _Side, p_Price: p_Message.Price2,
                                                    p_OrderQty: p_Message.OrderQty, p_ClientID: p_Message.Account);
                        //
                        ResponseMessageKafka _Response = new ResponseMessageKafka();
                        _Response.MsgType = CORE_MsgType.MsgOS;
                        _Response.OrderNo = objOrder.OrderNo;
                        _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                        _Response.OrderPartyID = p_Message.OrderPartyID;
                        _Response.RefExchangeID = p_Message.QuoteReqID;
                        _Response.ExchangeID = p_Message.QuoteID;
                        _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AM;
                        _Response.RefMsgType = CORE_RefMsgType.RefMsgType_R;
                        _Response.OrdType = p_Message.OrdType;
                        _Response.CrossType = 1;
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
                        // Gọi hàm gửi sang Kafka
                        c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                        //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                        p_Message.OrderNo = _Response.OrderNo;
                        SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                        //
                        Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Sua_Dien_Tu}, MsgSeqNum(34)=(34){p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID} ; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"Error can't find order info when received 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Co_Dien_Tu_Moi}, MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} when received exchange");
                        objOrder = new OrderInfo()
                        {
                            OrderNo = p_Message.ClOrdID,
                            RefMsgType = MessageType.QuoteRequest,
                            ExchangeID = p_Message.QuoteID,
                            RefExchangeID = p_Message.QuoteReqID,
                            Symbol = p_Message.Symbol,
                            Side = p_Message.Side == 1 ? "B" : "S",
                            Price = p_Message.Price,
                            OrderQty = p_Message.OrderQty,
                            ClientID = p_Message.Account,
                            OrderType = p_Message.OrdType,
                        };

                        OrderMemory.Add_NewOrder(objOrder);

                        ResponseMessageKafka _Response = new ResponseMessageKafka();
                        _Response.MsgType = CORE_MsgType.MsgOS;
                        _Response.OrderNo = objOrder.OrderNo;
                        _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                        _Response.OrderPartyID = p_Message.OrderPartyID;
                        _Response.RefExchangeID = p_Message.QuoteReqID;
                        _Response.ExchangeID = p_Message.QuoteID;
                        _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AM;
                        _Response.RefMsgType = CORE_RefMsgType.RefMsgType_R;
                        _Response.OrdType = p_Message.OrdType;
                        _Response.CrossType = 1;
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

                        c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                        //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                        p_Message.OrderNo = _Response.OrderNo;
                        SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                        //
                        Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Sua_Dien_Tu}, MsgSeqNum(34)=(34){p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID} ; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                    }
                    //
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> end process message when received exchange 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Sua_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}");
                }
                else if (p_Message.OrderPartyID != ConfigData.FirmID && p_Message.QuoteType == CORE_QuoteType.Bao_Sua_Dien_Tu)
                {
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange  35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Sua_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}");

                    // Sở gử 35=AI ( 4488# của mình , 537=2) => phản hồi của lệnh sửa do thành viên gửi lên

                    // "lấy tag 131 trong message 35=AI map với thông tin exchangeID trong memory để tìm bản ghi lệnh đặt.
                    // -nếu thấy thì update các thông tin: symbol, side, price, orderqty, clientID->map thông tin gửi về kafka, không lưu mem lệnh sửa này
                    // -nếu không thấy: lý thuyết không thể xảy ra tình huống này.Cần ghi lỗi"
                    OrderInfo objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.QuoteReqID);
                    if (objOrder != null)
                    {
                        ResponseMessageKafka _Response = new ResponseMessageKafka();
                        _Response.MsgType = CORE_MsgType.MsgOS;
                        _Response.OrderNo = objOrder.OrderNo;
                        _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                        _Response.OrderPartyID = p_Message.OrderPartyID;
                        _Response.RefExchangeID = p_Message.QuoteReqID;
                        _Response.ExchangeID = p_Message.QuoteID;
                        _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AM;
                        _Response.RefMsgType = CORE_RefMsgType.RefMsgType_R;
                        _Response.OrdType = p_Message.OrdType;
                        _Response.CrossType = 1;
                        _Response.ClientID = "";
                        _Response.ClientIDCounterFirm = p_Message.Account;
                        _Response.MemberCounterFirm = p_Message.OrderPartyID;
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
                        c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
                        //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                        p_Message.OrderNo = _Response.OrderNo;
                        SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                        //
                        Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Sua_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXSendQuoteStatusReport -> Error find order info when received 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Sua_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}, OrderPartyID = {p_Message.OrderPartyID}, QuoteType={p_Message.QuoteType} when received exchange");
                    }
                    //
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> end process message when received exchange 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Sua_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}");
                }
                else if (p_Message.OrderPartyID == ConfigData.FirmID && p_Message.QuoteType == CommonDataInCore.CORE_QuoteType.Bao_Huy_Dien_Tu)
                {
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Huy_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}");

                    // Sở gửi 35=AI ( 4488= của mình , 537=3) => phản hồi của lệnh hủy do thành viên gửi lên

                    //"lấy tag 131 trong message 35=AI map với thông tin exchangeID trong memory để tìm bản ghi đặt ban đầu.
                    //-nếu không thấy-> lý thuyết không thể xảy ra tình huống này.Cần ghi lỗi
                    //- nếu thấy, lấy thông tin orderno trong memory để tra ra kafka."
                    OrderInfo objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.QuoteReqID);
                    if (objOrder != null)
                    {
                        ResponseMessageKafka _Response = new ResponseMessageKafka();
                        _Response.MsgType = CORE_MsgType.MsgOS;
                        _Response.OrderNo = objOrder.OrderNo;
                        _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                        _Response.OrderPartyID = p_Message.OrderPartyID;
                        _Response.RefExchangeID = p_Message.QuoteReqID;
                        _Response.ExchangeID = p_Message.QuoteID;
                        _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AC;
                        _Response.RefMsgType = CORE_RefMsgType.RefMsgType_Z;
                        _Response.OrdType = p_Message.OrdType;
                        _Response.CrossType = 1;
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
                        c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                        //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                        p_Message.OrderNo = _Response.OrderNo;
                        SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                        //
                        Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Huy_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXSendQuoteStatusReport -> Error find order info when received 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Huy_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}, OrderPartyID = {p_Message.OrderPartyID}, QuoteType={p_Message.QuoteType} when received exchange");

                        objOrder = new OrderInfo()
                        {
                            OrderNo = p_Message.ClOrdID,
                            RefMsgType = MessageType.QuoteCancel,
                            ExchangeID = p_Message.QuoteID,
                            RefExchangeID = p_Message.QuoteReqID,
                            Symbol = p_Message.Symbol,
                            Side = p_Message.Side == 1 ? "B" : "S",
                            Price = p_Message.Price,
                            OrderQty = p_Message.OrderQty,
                            ClientID = p_Message.Account,
                            OrderType = p_Message.OrdType,
                        };
                        OrderMemory.Add_NewOrder(objOrder);

                        ResponseMessageKafka _Response = new ResponseMessageKafka();
                        _Response.MsgType = CORE_MsgType.MsgOS;
                        _Response.OrderNo = objOrder.OrderNo;
                        _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                        _Response.OrderPartyID = p_Message.OrderPartyID;
                        _Response.RefExchangeID = p_Message.QuoteReqID;
                        _Response.ExchangeID = p_Message.QuoteID;
                        _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AC;
                        _Response.RefMsgType = CORE_RefMsgType.RefMsgType_Z;
                        _Response.OrdType = p_Message.OrdType;
                        _Response.CrossType = 1;
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
                        c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                        //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                        p_Message.OrderNo = _Response.OrderNo;
                        SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                        //
                        Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Huy_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                    }
                    //
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> end process message when received exchange 35=AI|4488={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Huy_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}");
                }
                else if (p_Message.OrderPartyID != ConfigData.FirmID && p_Message.QuoteType == CommonDataInCore.CORE_QuoteType.Bao_Huy_Dien_Tu)
                {
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Huy_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}");

                    // Sở gử 35=AI ( 4488 # của mình , 537=3) => phản hồi của lệnh hủy do thành viên gửi lên

                    // "map trực tiếp từ mesage nhận được, gửi về kafka, không lưu mem
                    // Lấy tag 131 trong message 35 = AI map với thông tin exchangeID trong memory để tìm bản ghi lệnh đặt-- > lấy ra thông tin orderNo để trả ra KAFKA
                    // (về cơ bản case này chắc chắn ""orderNo"":"""" vì toàn là nhận về, chưa gửi cái gì lên nên k có số orderNo, dev dev xem xét cần lấy thông tin mem hay không vì nó hủy rồi)"
                    OrderInfo objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.QuoteReqID);
                    if (objOrder != null)
                    {
                        ResponseMessageKafka _Response = new ResponseMessageKafka();
                        _Response.MsgType = CORE_MsgType.MsgOS;
                        _Response.OrderNo = objOrder.OrderNo;
                        _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                        _Response.OrderPartyID = p_Message.OrderPartyID;
                        _Response.RefExchangeID = p_Message.QuoteReqID;
                        _Response.ExchangeID = p_Message.QuoteID;
                        _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AC;
                        _Response.RefMsgType = CORE_RefMsgType.RefMsgType_Z;
                        _Response.OrdType = p_Message.OrdType;
                        _Response.CrossType = 1;
                        _Response.ClientID = "";
                        _Response.ClientIDCounterFirm = p_Message.Account;
                        _Response.MemberCounterFirm = p_Message.OrderPartyID;
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
                        c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                        //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                        p_Message.OrderNo = _Response.OrderNo;
                        SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                        //
                        Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> process message when received exchange 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Huy_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXSendQuoteStatusReport -> Error find order info 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Huy_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}, OrderPartyID = {p_Message.OrderPartyID}, QuoteType={p_Message.QuoteType} when received exchange");
                    }
                    //
                    Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> end process message when received exchange 35=AI|4488!={ConfigData.FirmID}|537={CORE_QuoteType.Bao_Huy_Dien_Tu}, MsgSeqNum(34)={p_Message.MsgSeqNum}, QuoteReqID(131)={p_Message.QuoteReqID}");
                }

                //
                Logger.ResponseLog.Info($"HNXSendQuoteStatusReport -> End process message when received exchange 35=AI|4488={p_Message.OrderPartyID}|537={p_Message.QuoteType}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, GetSendingTime= {p_Message.GetSendingTime}, QuoteReqID={p_Message.QuoteReqID}, OrdType={p_Message.OrdType}, QuoteID={p_Message.QuoteID}, Account={p_Message.Account}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price},  Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, , OrderPartyID={p_Message.OrderPartyID}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"HNXSendQuoteStatusReport -> Error process when received exchange 35=AI|4488={p_Message.OrderPartyID}|537={p_Message.QuoteType}; with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, GetSendingTime= {p_Message.GetSendingTime}, QuoteReqID={p_Message.QuoteReqID}, OrdType={p_Message.OrdType}, QuoteID={p_Message.QuoteID}, Account={p_Message.Account}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price},  Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, RegistID={p_Message.RegistID}, , OrderPartyID={p_Message.OrderPartyID}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý khi nhận được 35=s
        public void HNXSendOrderCross(MessageNewOrderCross p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"HNXSendOrderCross -> start process when received exchange 35=s with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount},  TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, CrossID={p_Message.CrossID}, EffectiveTime={p_Message.EffectiveTime}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate},  SettlMethod={p_Message.SettlMethod}");
                //
                string _OrderNo = "";
                OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder == null)
                {
                    // nếu không thấy: insert bản ghi mới (có các thông tin refExchangeID,exchangeID, symbol, side, price, orderqty, crossType, clientid, clientIDCounterFirm, memberCounterFirm).
                    // Add thông tin order vào memory
                    LocalMemory.OrderInfo OrderObjMem = new OrderInfo();
                    OrderObjMem.RefMsgType = MessageType.NewOrderCross;
                    OrderObjMem.OrderNo = "";
                    OrderObjMem.ClOrdID = "";
                    OrderObjMem.ExchangeID = p_Message.CrossID;
                    OrderObjMem.RefExchangeID = "";
                    OrderObjMem.SeqNum = 0;
                    if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                    {
                        OrderObjMem.Side = ORDER_SIDE.SIDE_BUY;
                        OrderObjMem.ClientID = p_Message.CoAccount;
                        OrderObjMem.ClientIDCounterFirm = p_Message.Account;
                        OrderObjMem.MemberCounterFirm = p_Message.PartyID;
                    }
                    else
                    {
                        OrderObjMem.Side = ORDER_SIDE.SIDE_SELL;
                        OrderObjMem.ClientID = p_Message.Account;
                        OrderObjMem.ClientIDCounterFirm = p_Message.CoAccount;
                        OrderObjMem.MemberCounterFirm = p_Message.CoPartyID;
                    }
                    OrderObjMem.Symbol = p_Message.Symbol;
                    OrderObjMem.Price = p_Message.Price2;
                    OrderObjMem.OrderQty = p_Message.OrderQty;
                    OrderObjMem.CrossType = p_Message.CrossType.ToString();
                    OrderObjMem.OrderType = p_Message.OrdType;
                    //
                    OrderMemory.Add_NewOrder(OrderObjMem);
                }
                else
                {
                    _OrderNo = objOrder.OrderNo;

                    // Cập nhật ExchangeID vào mem
                    //objOrder.ExchangeID = p_Message.CrossID;
                    OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.CrossID);
                }
                //
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderNo = _OrderNo;
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                _Response.OrderPartyID = "";
                _Response.RefExchangeID = "";
                _Response.ExchangeID = "";
                _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AM;
                _Response.RefMsgType = CORE_RefMsgType.RefMsgType_R;
                _Response.OrdType = p_Message.OrdType;
                _Response.CrossType = 0;
                _Response.ClientID = p_Message.Account;
                _Response.ClientIDCounterFirm = "";
                _Response.MemberCounterFirm = "";
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                _Response.ExchangeID = p_Message.CrossID;
                if (p_Message.PartyID != p_Message.CoPartyID)
                {
                    _Response.RefMsgType = MessageType.NewOrderCross;
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_NP;
                }
                else
                {
                    _Response.RefMsgType = "";
                    _Response.OrderStatus = "";
                }
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
                _Response.Text = p_Message.Text;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";
                // Gọi hàm gửi sang Kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                p_Message.OrderNo = _Response.OrderNo;
                SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                //
                Logger.ResponseLog.Info($"HNXSendOrderCross -> End process when received exchange 35=s with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, SendingTime={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"HNXSendOrderCross -> Error process when received exchange 35=s with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount},  TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, Price={p_Message.Price}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID}, CrossType={p_Message.CrossType}, CrossID={p_Message.CrossID}, EffectiveTime={p_Message.EffectiveTime}, Yield={p_Message.Yield}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate},  SettlMethod={p_Message.SettlMethod}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý khi nhận được 35=t
        public void HNXResponse_CrossOrderCancelReplace(CrossOrderCancelReplaceRequest p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"HNXResponse_CrossOrderCancelReplace -> start process when received exchange 35=t with MsgSeqNum(34)={p_Message.MsgSeqNum}, CrossType(549)={p_Message.CrossType}, OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID},   OrderID={p_Message.OrderID}, EffectiveTime={p_Message.EffectiveTime}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}");
                //
                string _OrderNo = "";
                OrderInfo objOrder = null;
                //
                // 549=1
                if (p_Message.CrossType == CORE_CrossType.CrossType_1)
                {
                    /*
                     * *****Với 35=t, 549=1 -> "ordType"="R"
                    Lấy giá trị tag 551, so sánh với dữ liệu exchangeID trong danh sách lệnh lưu mem của gate:
                    + Nếu trùng với 1 exchangeID trong mem của gate --> Thông tin orderNo trả ra cho KAFKA orderNo = giá trị orderNo của lệnh trong mem
                    + Nếu không trùng với bất cứ exchangeID nào trong mem --> "orderNo": ""
                     */

                    /*lấy tag 551 trong message 35 = t, 549 = 1 map với thông tin exchangeID trong memory để tìm bản ghi đặt ban đầu
                    -nếu không thấy-> thêm mới vào memory(là case nhận được thông báo sửa từ bên khác)
                    -nếu thấy, lấy thông tin orderNo trong memory để tra ra.Cập nhật symbol, side, price, orderqty, crossType, clientID, clientIDCounterFirm, memberCounterFirm vào thông tin memory vừa tìm được
                    */

                    objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrgCrossID);
                    if (objOrder == null)
                    {
                        // - nếu không thấy-> thêm mới vào memory(là case nhận được thông báo sửa từ bên khác)
                        // Add thông tin order vào memory
                        LocalMemory.OrderInfo OrderObjMem = new OrderInfo();
                        OrderObjMem.RefMsgType = MessageType.CrossOrderCancelReplaceRequest;
                        OrderObjMem.OrderNo = p_Message.OrgCrossID;
                        OrderObjMem.ClOrdID = p_Message.ClOrdID;
                        OrderObjMem.ExchangeID = p_Message.OrderID;
                        OrderObjMem.RefExchangeID = p_Message.OrgCrossID;
                        OrderObjMem.SeqNum = 0;
                        OrderObjMem.Symbol = p_Message.Symbol;
                        OrderObjMem.Price = p_Message.Price2;
                        OrderObjMem.OrderQty = p_Message.OrderQty;
                        OrderObjMem.CrossType = p_Message.CrossType.ToString();

                        if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                        {
                            OrderObjMem.ClientID = p_Message.CoAccount;
                            OrderObjMem.ClientIDCounterFirm = p_Message.Account;
                            OrderObjMem.MemberCounterFirm = p_Message.PartyID;
                            OrderObjMem.Side = ORDER_SIDE.SIDE_BUY;
                        }
                        else
                        {
                            OrderObjMem.ClientID = p_Message.Account;
                            OrderObjMem.ClientIDCounterFirm = p_Message.CoAccount;
                            OrderObjMem.MemberCounterFirm = p_Message.CoPartyID;
                            OrderObjMem.Side = ORDER_SIDE.SIDE_SELL;
                        }
                        //
                        OrderMemory.Add_NewOrder(OrderObjMem);
                    }
                    else
                    {
                        //  -nếu thấy, lấy thông tin orderNo trong memory để tra ra.Cập nhật symbol, side, price, orderqty, crossType, clientID, clientIDCounterFirm, memberCounterFirm vào thông tin memory vừa tìm được
                        /*objOrder.Symbol = p_Message.Symbol;
                        objOrder.Price = p_Message.Price2;
                        objOrder.OrderQty = p_Message.OrderQty;*/

                        if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                        {
                            /*objOrder.ClientID = p_Message.CoAccount;
                            objOrder.ClientIDCounterFirm = p_Message.Account;
                            objOrder.MemberCounterFirm = p_Message.PartyID;
                            objOrder.Side = ORDER_SIDE.SIDE_BUY;*/
                            OrderMemory.UpdateOrderInfo(objOrder, p_ClientID: p_Message.CoAccount, p_ClientIDCounterFirm: p_Message.Account,
                                                        p_MemberCounterFirm: p_Message.PartyID, p_Side: ORDER_SIDE.SIDE_BUY);
                        }
                        else
                        {
                            /*objOrder.ClientID = p_Message.Account;
                            objOrder.ClientIDCounterFirm = p_Message.CoAccount;
                            objOrder.MemberCounterFirm = p_Message.CoPartyID;
                            objOrder.Side = ORDER_SIDE.SIDE_SELL;*/
                            OrderMemory.UpdateOrderInfo(objOrder, p_ClientID: p_Message.Account, p_ClientIDCounterFirm: p_Message.CoAccount,
                                                        p_MemberCounterFirm: p_Message.CoPartyID, p_Side: ORDER_SIDE.SIDE_BUY);
                        }
                        //objOrder.CrossType = p_Message.CrossType.ToString();
                        OrderMemory.UpdateOrderInfo(objOrder, p_Symbol: p_Message.Symbol, p_Price: p_Message.Price2,
                                                        p_OrderQty: p_Message.OrderQty, p_CrossType: p_Message.CrossType.ToString());
                    }
                }
                //549=2
                else if (p_Message.CrossType == CORE_CrossType.CrossType_2)
                {
                    /*
                     * "lấy tag 11 trong message 35=t, 549=2 map với thông tin clOrdID trong memory để tìm bản ghi.
                        - nếu không thấy: insert bản ghi mới. Lấy tag 551, so sánh với dữ liệu exchangeID trên mem để lấy ra orderNo trả thông tin cho kafka
                        - nếu thấy-> lấy thông tin trong memory để tra ra. Cập nhật exchangeID vào thông tin memory vừa tìm được. Trả thông tin cho kafka"
                     */
                    objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                    if (objOrder == null)
                    {
                        /*
                         * + nếu không thấy:
                              -  B1: insert bản ghi mới.
                              -  B2:  Lấy tag 551, so sánh với dữ liệu exchangeID trên mem để lấy ra orderNo trả thông tin cho kafka
                              -  B3:  Lấy orderNo của bản ghi ở B2, cập nhật vào orderNo của bản ghi ở B1
                         */
                        // B1. Add thông tin order vào memory
                        LocalMemory.OrderInfo OrderObjMem = new OrderInfo();
                        OrderObjMem.RefMsgType = MessageType.CrossOrderCancelReplaceRequest;
                        OrderObjMem.OrderNo = "";
                        OrderObjMem.ClOrdID = p_Message.ClOrdID;
                        OrderObjMem.ExchangeID = p_Message.OrderID;
                        OrderObjMem.RefExchangeID = p_Message.OrgCrossID;
                        OrderObjMem.SeqNum = 0;
                        OrderObjMem.Symbol = p_Message.Symbol;
                        OrderObjMem.Price = p_Message.Price2;
                        OrderObjMem.OrderQty = p_Message.OrderQty;
                        OrderObjMem.CrossType = p_Message.CrossType.ToString();
                        OrderObjMem.OrderType = p_Message.OrdType;
                        if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                        {
                            OrderObjMem.ClientID = p_Message.CoAccount;
                            OrderObjMem.ClientIDCounterFirm = p_Message.Account;
                            OrderObjMem.MemberCounterFirm = p_Message.PartyID;
                            OrderObjMem.Side = ORDER_SIDE.SIDE_BUY;
                        }
                        else
                        {
                            OrderObjMem.ClientID = p_Message.Account;
                            OrderObjMem.ClientIDCounterFirm = p_Message.CoAccount;
                            OrderObjMem.MemberCounterFirm = p_Message.CoPartyID;
                            OrderObjMem.Side = ORDER_SIDE.SIDE_SELL;
                        }
                        //
                        OrderMemory.Add_NewOrder(OrderObjMem);

                        // B2. Lấy tag 551, so sánh với dữ liệu exchangeID trên mem để lấy ra orderNo trả thông tin cho kafka
                        objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrgCrossID); // OrgCrossID=551

                        // B3:  Lấy orderNo của bản ghi ở B2, cập nhật vào orderNo của bản ghi ở B1
                        if (objOrder != null)
                        {
                            OrderInfo objOrderNewInsert = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                            if (objOrderNewInsert != null)
                            {
                                objOrderNewInsert.OrderNo = objOrder.OrderNo;
                            }
                        }
                    }
                    else
                    {
                        // + nếu thấy-> Cập nhật exchangeID vào thông tin memory vừa tìm được. Trả thông tin cho kafka
                        OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID, p_OrdType: p_Message.OrdType);
                    }
                }
                //
                if (objOrder != null)
                {
                    // Lấy thông tin OrderNo ra để gửi Kafka
                    _OrderNo = objOrder.OrderNo;
                }
                //
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                _Response.OrderPartyID = "";
                //
                _Response.OrderNo = _OrderNo;
                //
                _Response.ExchangeID = p_Message.OrderID;
                _Response.RefExchangeID = p_Message.OrgCrossID;
                if (p_Message.CrossType == CORE_CrossType.CrossType_1)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AM;
                    //_Response.OrdType = "R";
                }
                else if (p_Message.CrossType == CORE_CrossType.CrossType_2)
                {
                    /*if (objOrder != null)
                    {
                        _Response.OrdType = objOrder.OrderType;
                    }*/
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_NM;
                }
                _Response.RefMsgType = MessageType.CrossOrderCancelReplaceRequest;
                _Response.CrossType = p_Message.CrossType;
                //
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                {
                    _Response.ClientID = p_Message.CoAccount;
                    _Response.ClientIDCounterFirm = p_Message.Account;
                    _Response.MemberCounterFirm = p_Message.PartyID;
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                }
                else
                {
                    _Response.ClientID = p_Message.Account;
                    _Response.ClientIDCounterFirm = p_Message.CoAccount;
                    _Response.MemberCounterFirm = p_Message.CoPartyID;
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                }
                _Response.OrdType = p_Message.OrdType;
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
                // Gọi hàm gửi sang Kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                p_Message.OrderNo = _Response.OrderNo;
                SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                //
                Logger.ResponseLog.Info($"HNXResponse_CrossOrderCancelReplace -> End process when received exchange 35=t with MsgSeqNum(34)={p_Message.MsgSeqNum}, CrossType(449)={p_Message.CrossType}, OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID}, SendingTime={HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime)}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"HNXResponse_CrossOrderCancelReplace -> Error process when received exchange 35=t with MsgSeqNum(34)={p_Message.MsgSeqNum}, CrossType(549)={p_Message.CrossType}, OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID}, OrdType={p_Message.OrdType}, Account={p_Message.Account}, CoAccount={p_Message.CoAccount}, TransactTime={p_Message.TransactTime}, Symbol={p_Message.Symbol}, Side={p_Message.Side}, OrderQty={p_Message.OrderQty}, PartyID={p_Message.PartyID}, CoPartyID={p_Message.CoPartyID},   OrderID={p_Message.OrderID}, EffectiveTime={p_Message.EffectiveTime}, Price2={p_Message.Price2}, SettlValue={p_Message.SettlValue}, SettDate={p_Message.SettDate}, SettlMethod={p_Message.SettlMethod}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý khi nhận được 35=u
        public void HNXResponse_CrossOrderCancelRequest(CrossOrderCancelRequest p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"HNXResponse_CrossOrderCancelRequest -> start process when received exchange 35=u with MsgSeqNum(34)={p_Message.MsgSeqNum}, CrossType(549)={p_Message.CrossType},  OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, OrdType={p_Message.OrdType},  Symbol={p_Message.Symbol}, Side={p_Message.Side}");
                //
                string _OrderNo = "";
                OrderInfo objOrder = null;
                //
                if (p_Message.CrossType == CORE_CrossType.CrossType_1) // 549=1
                {
                    /*
                     * *****Với 35=u, 549=1
                     Lấy giá trị tag 551, so sánh với dữ liệu exchangeID trong danh sách lệnh lưu mem của gate:
                     + Nếu trùng với 1 exchangeID trong mem của gate --> Thông tin orderNo trả ra cho KAFKA orderNo = giá trị orderNo của lệnh trong mem
                     + Nếu không trùng với bất cứ exchangeID nào trong mem --> "orderNo": ""
                     */
                    objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrgCrossID);
                }
                else if (p_Message.CrossType == CORE_CrossType.CrossType_2) // 549=2
                {
                    /*
                     * lấy tag 11 trong message 35=u, 549=2 map với thông tin clOrdID trong memory để tìm bản ghi.
                        + nếu không thấy:
                           - B1: insert bản ghi mới.
                           - B2: Lấy tag 551, so sánh với dữ liệu exchangeID trên mem để lấy ra orderNo trả thông tin cho kafka
                           - B3:  Lấy orderNo của bản ghi ở B2, cập nhật vào orderNo của bản ghi ở B1
                        + nếu thấy-> Cập nhật exchangeID vào thông tin memory vừa tìm được. Trả thông tin cho kafka
                     */
                    objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                    if (objOrder == null)
                    {
                        // -B1: insert bản ghi mới.
                        LocalMemory.OrderInfo OrderObjMem = new OrderInfo();
                        OrderObjMem.RefMsgType = MessageType.CrossOrderCancelRequest;
                        OrderObjMem.OrderNo = "";
                        OrderObjMem.ClOrdID = p_Message.ClOrdID;
                        OrderObjMem.ExchangeID = p_Message.OrderID;
                        OrderObjMem.RefExchangeID = p_Message.OrgCrossID;
                        OrderObjMem.SeqNum = 0;
                        OrderObjMem.Symbol = p_Message.Symbol;
                        if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                            OrderObjMem.Side = ORDER_SIDE.SIDE_BUY;
                        else
                            OrderObjMem.Side = ORDER_SIDE.SIDE_SELL;

                        OrderObjMem.Price = 0;
                        OrderObjMem.OrderQty = 0;
                        OrderObjMem.CrossType = p_Message.CrossType.ToString();
                        OrderObjMem.ClientID = "";
                        OrderObjMem.ClientIDCounterFirm = "";
                        OrderObjMem.MemberCounterFirm = "";
                        OrderObjMem.OrderType = p_Message.OrdType;
                        //
                        OrderMemory.Add_NewOrder(OrderObjMem);
                        //
                        //    - B2: Lấy tag 551, so sánh với dữ liệu exchangeID trên mem để lấy ra orderNo trả thông tin cho kafka
                        objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrgCrossID);  // OrgCrossID=551
                        // B3:  Lấy orderNo của bản ghi ở B2, cập nhật vào orderNo của bản ghi ở B1
                        if (objOrder != null)
                        {
                            OrderInfo objOrderNewInsert = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                            if (objOrderNewInsert != null)
                            {
                                objOrderNewInsert.OrderNo = objOrder.OrderNo;
                            }
                        }
                    }
                    else
                    {
                        //Cập nhật exchangeID vào thông tin memory vừa tìm được
                        OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                    }
                }
                // - nếu thấy, lấy thông tin trong memory để tra ra. Cập nhật exchangeID vào thông tin memory vừa tìm được
                if (objOrder != null)
                {
                    _OrderNo = objOrder.OrderNo; // - nếu thấy, lấy thông tin trong memory để tra ra
                }
                //
                ResponseMessageKafka _Response = new ResponseMessageKafka();
                _Response.MsgType = CORE_MsgType.MsgOS;
                _Response.OrderNo = _OrderNo;
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                _Response.OrderPartyID = "";
                _Response.RefExchangeID = p_Message.OrgCrossID;
                _Response.ExchangeID = p_Message.OrderID;
                if (p_Message.CrossType == CORE_CrossType.CrossType_1)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AM;
                }
                else if (p_Message.CrossType == CORE_CrossType.CrossType_2)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_NC;
                }
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
                _Response.SettleDate = "";
                _Response.Symbol = p_Message.Symbol;
                _Response.SettleMethod = 0;
                _Response.RegistID = "";
                _Response.EffectiveTime = "";
                _Response.Text = p_Message.Text;
                _Response.RejectReasonCode = "";
                _Response.RejectReason = "";
                //
                // Gọi hàm gửi sang Kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                p_Message.OrderNo = _Response.OrderNo;
                SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                //
                Logger.ResponseLog.Info($"HNXResponse_CrossOrderCancelRequest -> End process when received exchange 35=u with MsgSeqNum(34)={p_Message.MsgSeqNum}, CrossType(549)={p_Message.CrossType},  OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID}, OrdType={p_Message.OrdType},  Symbol={p_Message.Symbol}, Side={p_Message.Side}; Send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"HNXResponse_CrossOrderCancelRequest -> Error process when received exchange 35=u with MsgSeqNum(34)={p_Message.MsgSeqNum}, CrossType(549)={p_Message.CrossType},  OrgCrossID(551)={p_Message.OrgCrossID}, ClOrdID(11)={p_Message.ClOrdID}, OrderID(37)={p_Message.OrderID},OrdType={p_Message.OrdType},  Symbol={p_Message.Symbol}, Side={p_Message.Side},  Exception: {ex?.ToString()}");
            }
        }

        //  Xử lý khi nhận được 35 =3
        public void HNXSendReject(MessageReject p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"HNXSendReject -> Start process message when received exchange 35=3 with MsgSeqNum(34)={p_Message.MsgSeqNum}, RefSeqNum(45)={p_Message.RefSeqNum}, SendingTime(52)={p_Message.GetSendingTime}, RefMsgType(372)={p_Message.RefMsgType}, SessionRejectReason(373)={p_Message.SessionRejectReason}, Text(58)={p_Message.Text}");
                //
                string _OrderNo = "";
                string _QuoteType = "";
                string _RefMsgType = "";
                string _RefExchangeID = "";

                OrderInfo objOrder = LocalMemory.OrderMemory.GetOrder_bySeqNum(p_Message.RefSeqNum);
                if (objOrder != null)
                {
                    _OrderNo = objOrder.OrderNo;
                    _QuoteType = objOrder.QuoteType > 0 ? objOrder.QuoteType.ToString() : "";
                    _RefMsgType = objOrder.RefMsgType;
                    _RefExchangeID = objOrder.RefExchangeID != null ? objOrder.RefExchangeID : "";
                }
                else
                {
                    Logger.ResponseLog.Warn($"HNXSendReject -> Error find order info when call GetOrder_bySeqNum({p_Message.RefSeqNum}) received MsgSeqNum(34)={p_Message.MsgSeqNum}, RefSeqNum(45)={p_Message.RefSeqNum}, SendingTime(52)={p_Message.GetSendingTime}, RefMsgType(372)={p_Message.RefMsgType}, SessionRejectReason(373)={p_Message.SessionRejectReason}, Text(58)={p_Message.Text} when received exchange");
                }
                //
                if (p_Message.RefMsgType == MessageType.ReposInquiry) // N01
                {
                    InquiryObjectModel _Response = new InquiryObjectModel();
                    _Response.MsgType = CORE_MsgType.MsgIS;
                    _Response.OrderNo = _OrderNo;
                    _Response.ExchangeID = "";
                    _Response.RefExchangeID = _RefExchangeID;
                    _Response.QuoteType = _QuoteType;
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
                    _Response.RejectReasonCode = p_Message.SessionRejectReason.ToString();
                    _Response.RejectReason = ConfigData.DictError_Code_Text.ContainsKey(_Response.RejectReasonCode) ? ConfigData.DictError_Code_Text[_Response.RejectReasonCode] : string.Empty;
                    _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                    _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                    //Bổ sung RefSeqNum
                    _Response.RefSeqNum = p_Message.RefSeqNum;
                    // send kafka
                    c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
                    //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                    p_Message.OrderNo = _Response.OrderNo;
                    SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                    //
                    Logger.ResponseLog.Info($"HNXSendReject -> Start process message when received exchange 35=3 with MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                }
                else if (p_Message.RefMsgType == MessageType.ReposFirm ||
                    p_Message.RefMsgType == MessageType.ReposFirmAccept ||
                    p_Message.RefMsgType == MessageType.ReposFirmDTHModify ||
                    p_Message.RefMsgType == MessageType.ReposFirmDTHCancel ||
                    p_Message.RefMsgType == MessageType.ReposBCGD ||
                    p_Message.RefMsgType == MessageType.ReposBCGDModify ||
                    p_Message.RefMsgType == MessageType.ReposBCGDCancel) // N03 || N05 || N06 || N07 || MA || ME || MC
                {
                    //
                    FirmReposModel _Response = new FirmReposModel();
                    _Response.MsgType = CORE_MsgType.MsgRS;
                    _Response.OrderNo = _OrderNo;
                    _Response.RefMsgType = _RefMsgType;
                    _Response.ExchangeID = "";
                    _Response.RefExchangeID = _RefExchangeID;
                    _Response.QuoteType = _QuoteType;
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
                    List<ReposSideListResponse> lstSymbolFirmInfoRes = new List<ReposSideListResponse>();
                    ReposSideListResponse _reposSideList = new ReposSideListResponse();
                    _reposSideList.NumSide = 0;
                    _reposSideList.Symbol = "";
                    _reposSideList.OrderQty = 0;
                    _reposSideList.ExecPrice = 0;
                    _reposSideList.MergePrice = 0;
                    _reposSideList.ReposInterest = 0;
                    _reposSideList.HedgeRate = 0.0;
                    _reposSideList.SettleValue1 = 0.0;
                    _reposSideList.SettleValue2 = 0.0;
                    lstSymbolFirmInfoRes.Add(_reposSideList);
                    _Response.SymbolFirmInfo = lstSymbolFirmInfoRes;
                    //
                    _Response.MatchReportType = 0;
                    _Response.RejectReasonCode = p_Message.SessionRejectReason.ToString();
                    _Response.RejectReason = ConfigData.DictError_Code_Text.ContainsKey(_Response.RejectReasonCode) ? ConfigData.DictError_Code_Text[_Response.RejectReasonCode] : string.Empty;
                    _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                    _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                    // send kafka
                    c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                    //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                    p_Message.OrderNo = _Response.OrderNo;
                    SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                    //
                    Logger.ResponseLog.Info($"HNXSendReject -> Start process message when received exchange 35=3 with MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                }
                else if (p_Message.RefMsgType == MessageType.NewOrder || p_Message.RefMsgType == MessageType.ReplaceOrder || p_Message.RefMsgType == MessageType.CancelOrder)
                {
                    NormalObjectModel _Response = new NormalObjectModel();
                    _Response.MsgType = CORE_MsgType.MsgNS;
                    _Response.RefMsgType = p_Message.RefMsgType;
                    _Response.OrderNo = _OrderNo;
                    _Response.ExchangeID = "";
                    _Response.RefExchangeID = _RefExchangeID;
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
                    _Response.RejectReasonCode = p_Message.SessionRejectReason.ToString();
                    _Response.RejectReason = ConfigData.DictError_Code_Text.ContainsKey(_Response.RejectReasonCode) ? ConfigData.DictError_Code_Text[_Response.RejectReasonCode] : string.Empty;
                    _Response.Text = "";
                    _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);

                    c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                    //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                    p_Message.OrderNo = _Response.OrderNo;
                    SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                    //
                    Logger.ResponseLog.Info($"HNXSendReject -> Start process message when received exchange 35=3 with MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                }
                else
                {
                    ResponseMessageKafka _Response = new ResponseMessageKafka();
                    _Response.MsgType = CORE_MsgType.MsgOS;
                    _Response.OrderNo = _OrderNo;
                    _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                    _Response.OrderPartyID = "";
                    _Response.RefExchangeID = _RefExchangeID;
                    _Response.ExchangeID = "";
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_EJ;
                    _Response.RefMsgType = p_Message.RefMsgType;
                    _Response.OrdType = "";
                    _Response.CrossType = 0;
                    _Response.ClientID = "";
                    _Response.ClientIDCounterFirm = "";
                    _Response.MemberCounterFirm = "";
                    _Response.Side = "";
                    _Response.OrderQty = 0;
                    _Response.Price = 0;
                    _Response.SettleValue = 0;
                    _Response.SettleDate = "";
                    _Response.Symbol = "";
                    _Response.SettleMethod = 0;
                    _Response.RegistID = "";
                    _Response.EffectiveTime = "";
                    _Response.Text = "";
                    _Response.RejectReasonCode = p_Message.SessionRejectReason.ToString();
                    _Response.RejectReason = ConfigData.DictError_Code_Text.ContainsKey(_Response.RejectReasonCode) ? ConfigData.DictError_Code_Text[_Response.RejectReasonCode] : string.Empty;

                    // send kafka
                    c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                    //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                    p_Message.OrderNo = _Response.OrderNo;
                    SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                    //
                    Logger.ResponseLog.Info($"HNXSendReject -> Start process message when received exchange 35=3 with MsgSeqNum(34)={p_Message.MsgSeqNum}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
                }
                //
                Logger.ResponseLog.Info($"HNXSendReject -> End process when received exchange message 35=3 with MsgSeqNum(34)={p_Message.MsgSeqNum}, RefSeqNum(45)={p_Message.RefSeqNum}, SendingTime(52)={p_Message.GetSendingTime}, RefMsgType(372)={p_Message.RefMsgType}, SessionRejectReason(373)={p_Message.SessionRejectReason}, Text(58)={p_Message.Text}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"HNXSendReject -> Error process when received exchange 35=3 with MsgSeqNum(34)={p_Message.MsgSeqNum}, RefSeqNum(45)={p_Message.RefSeqNum}, SendingTime(52)={p_Message.GetSendingTime}, RefMsgType(372)={p_Message.RefMsgType}, SessionRejectReason(373)={p_Message.SessionRejectReason}, Text(58)={p_Message.Text}, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý khi nhận đc 35=N02
        public void HNXResponse_InquiryReposReponse(MessageReposInquiryReport p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"HNXResponse_InquiryReposReponse -> start process when received exchange 35={MessageType.ReposInquiryReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID},  QuoteID(117)={p_Message.QuoteID}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType},  OrderPartyID(4488)={p_Message.OrderPartyID}, Side(54)={p_Message.Side}, EffectiveTime(168)={p_Message.EffectiveTime}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, SettlMethod(6363)={p_Message.SettlMethod}, RegistID(513)={p_Message.RegistID}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(54)={p_Message.EndDate}, OrderQty(38)={p_Message.OrderQty}, Symbol(55)={p_Message.Symbol}");
                // Map thông tin vào mem
                if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_1 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    // lấy tag 11 trong message 35=N02 map với thông tin clOrdID trong memory để tìm bản ghi.
                    // - nếu không thấy-> lý thuyết khong thể xảy ra tình huống này. Cần ghi lỗi
                    // - nếu thấy, lấy thông tin trong memory để tra ra. Cập nhật exchangeID vào thông tin memory vừa tìm được. Trả thông tin cho kafka
                    OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                    if (objOrder != null)
                    {
                        OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.QuoteID);
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_InquiryReposReponse ->  process when received exchange 35={MessageType.ReposInquiryReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}; Can't find GetOrder_byClOrdID({p_Message.ClOrdID}) on memory!");
                    }
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_1 && p_Message.OrderPartyID != ConfigData.FirmID)
                {
                    // lấy tag 11 trong message 35=N01 map với thông tin clOrdID trong memory để tìm bản ghi.
                    // - nếu không thấy: insert bản ghi mới  (có các thông tin refMsgType quoteType, exchangeID, symbol, side, orderValue,orderType).  Trả thông tin cho kafka
                    // - nếu thấy-> lý thuyết khong thể xảy ra tình huống này. Cần ghi lỗi
                    OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                    if (objOrder == null)
                    {
                        // Add thông tin order vào memory
                        LocalMemory.OrderInfo OrderObjMem = new OrderInfo();
                        OrderObjMem.RefMsgType = MessageType.ReposInquiry;
                        OrderObjMem.QuoteType = p_Message.QuoteType;
                        OrderObjMem.ExchangeID = p_Message.QuoteID;
                        OrderObjMem.Symbol = p_Message.Symbol;
                        if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                        {
                            OrderObjMem.Side = ORDER_SIDE.SIDE_BUY;
                        }
                        else
                        {
                            OrderObjMem.Side = ORDER_SIDE.SIDE_SELL;
                        }

                        OrderObjMem.OrderQty = (long)p_Message.OrderQty;
                        OrderObjMem.OrderType = p_Message.OrdType;
                        OrderObjMem.SeqNum = 0;
                        //
                        OrderMemory.Add_NewOrder(OrderObjMem);
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_InquiryReposReponse ->  process when received exchange 35={MessageType.ReposInquiryReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}; Can't find GetOrder_byClOrdID({p_Message.ClOrdID}) on memory!");
                    }
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_2 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    // Lấy tag 644 trong message 35=N02 map với thông tin exchangeID trong memory để tìm bản ghi đặt ban đầu.
                    // - nếu không thấy-> lý thuyết không thể xảy ra tình huống này.Cần ghi lỗi
                    // - nếu thấy, lấy thông tin orderno trong memory để tra ra kafka. Cập nhật symbol, side, price, orderqty, clientID vào thông tin memory vừa tìm được.
                    OrderInfo objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.RFQReqID);
                    if (objOrder != null)
                    {
                        string side;
                        if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                            side = ORDER_SIDE.SIDE_BUY;
                        else
                            side = ORDER_SIDE.SIDE_SELL;
                        OrderMemory.UpdateOrderInfo(objOrder, p_Side: side, p_Symbol: p_Message.Symbol, p_OrderQty: (long)p_Message.OrderQty);
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_InquiryReposReponse ->  process when received exchange 35={MessageType.ReposInquiryReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, QuoteType(537)={p_Message.QuoteType}; Warning find GetOrder_byExchangeID({p_Message.RFQReqID}) on memory!");
                    }
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_2 && p_Message.OrderPartyID != ConfigData.FirmID)
                {
                    // Lấy tag 644 trong message 35=N02  map với thông tin exchangeID trong memory để tìm bản ghi lệnh đặt.
                    // - nếu thấy thì update các thông tin: symbol, side, price, orderqty, clientID->map thông tin gửi về kafka, không lưu mem lệnh sửa này
                    // - nếu không thấy: lý thuyết không thể xảy ra tình huống này.Cần ghi lỗi
                    OrderInfo objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.RFQReqID);
                    if (objOrder != null)
                    {
                        string side;
                        if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                            side = ORDER_SIDE.SIDE_BUY;
                        else
                            side = ORDER_SIDE.SIDE_SELL;
                        OrderMemory.UpdateOrderInfo(objOrder, p_Side: side, p_Symbol: p_Message.Symbol, p_OrderQty: (long)p_Message.OrderQty);
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_InquiryReposReponse ->  process when received exchange 35={MessageType.ReposInquiryReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, QuoteType(537)={p_Message.QuoteType}; Warning find GetOrder_byExchangeID({p_Message.RFQReqID}) on memory!");
                    }
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_3 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    // lấy tag 644 trong message 35=N02 map với thông tin exchangeID trong memory để tìm bản ghi đặt ban đầu.
                    // - nếu không thấy-> lý thuyết không thể xảy ra tình huống này.Cần ghi lỗi
                    // - nếu thấy, lấy thông tin orderno trong memory để tra ra kafka.
                    OrderInfo objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.RFQReqID);
                    if (objOrder != null)
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_InquiryReposReponse ->  process when received exchange 35={MessageType.ReposInquiryReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, QuoteType(537)={p_Message.QuoteType}; Warning find GetOrder_byExchangeID({p_Message.RFQReqID}) on memory!");
                    }
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_3 && p_Message.OrderPartyID != ConfigData.FirmID)
                {
                    // map trực tiếp từ mesage nhận được, gửi về kafka, không lưu mem
                    // Lấy tag 644 trong message 35 = N02 map với thông tin exchangeID trong memory để tìm bản ghi lệnh đặt-- > lấy ra thông tin orderNo để trả ra KAFKA
                    // (về cơ bản case này chắc chắn "orderNo":"" vì toàn là nhận về, chưa gửi cái gì lên nên k có số orderNo, dev dev xem xét cần lấy thông tin mem hay không vì nó hủy rồi)
                    OrderInfo objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.RFQReqID);
                    if (objOrder != null)
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_InquiryReposReponse ->  process when received exchange 35={MessageType.ReposInquiryReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, QuoteType(537)={p_Message.QuoteType}; Warning find GetOrder_byExchangeID({p_Message.RFQReqID}) on memory!");
                    }
                }
                // Lấy thông tin OrderNo
                string p_OrderNo = "";
                if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_1)
                {
                    OrderInfo objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                    if (objOrder != null)
                    {
                        p_OrderNo = objOrder.OrderNo;
                    }
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_2 || p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_3 || p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_4)
                {
                    OrderInfo objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.RFQReqID);
                    if (objOrder != null)
                    {
                        p_OrderNo = objOrder.OrderNo;
                    }
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_5)
                {
                    OrderInfo objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.QuoteID);
                    if (objOrder != null)
                    {
                        p_OrderNo = objOrder.OrderNo;
                    }
                }
                InquiryObjectModel _Response = new InquiryObjectModel();
                _Response.MsgType = CORE_MsgType.MsgIS;
                _Response.OrderNo = p_OrderNo;
                _Response.ExchangeID = p_Message.QuoteID;
                _Response.RefExchangeID = !string.IsNullOrEmpty(p_Message.RFQReqID) ? p_Message.RFQReqID : "";
                _Response.QuoteType = p_Message.QuoteType.ToString();
                _Response.OrdType = p_Message.OrdType;
                //
                if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_1 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_IN;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_1 && p_Message.OrderPartyID != ConfigData.FirmID)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_NI;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_2)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AM;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_3)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AC;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_4 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_SC;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_4 && p_Message.OrderPartyID != ConfigData.FirmID)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_CL;
                }
                else if ((p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_5 || p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_6))
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_CL;
                }
                //
                _Response.OrderPartyID = p_Message.OrderPartyID;
                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                else
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                _Response.EffectiveTime = p_Message.EffectiveTime;
                _Response.RepurchaseTerm = p_Message.RepurchaseTerm;
                _Response.SettleMethod = p_Message.SettlMethod;
                _Response.RegistID = p_Message.RegistID;
                _Response.SettleDate1 = p_Message.SettlDate;
                _Response.SettleDate2 = p_Message.SettlDate2;
                _Response.EndDate = p_Message.EndDate;
                _Response.OrderValue = (long)p_Message.OrderQty;
                _Response.Symbol = p_Message.Symbol;
                //
                if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_4 && p_Message.OrderPartyID != ConfigData.FirmID)
                {
                    _Response.RejectReasonCode = CORE_RejectReasonCode.Code_50007;
                    _Response.RejectReason = CORE_RejectReason.RejectReason_50007;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_5)
                {
                    _Response.RejectReasonCode = CORE_RejectReasonCode.Code_50008;
                    _Response.RejectReason = CORE_RejectReason.RejectReason_50008;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeInquiry.QuoteType_6)
                {
                    _Response.RejectReasonCode = CORE_RejectReasonCode.Code_50005;
                    _Response.RejectReason = CORE_RejectReason.RejectReason_50005;
                }
                else
                {
                    _Response.RejectReasonCode = "";
                    _Response.RejectReason = "";
                }

                //
                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);
                //
                // Gọi hàm gửi sang Kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                p_Message.OrderNo = _Response.OrderNo;
                SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                //
                Logger.ResponseLog.Info($"HNXResponse_InquiryReposReponse -> End process when received exchange 35={MessageType.ReposInquiryReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}, Symbol(55)={p_Message.Symbol}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"HNXResponse_InquiryReposReponse -> Error process when received exchange 35={MessageType.ReposInquiryReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID},  QuoteID(117)={p_Message.QuoteID}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType},  OrderPartyID(4488)={p_Message.OrderPartyID}, Side(54)={p_Message.Side}, EffectiveTime(168)={p_Message.EffectiveTime}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, SettlMethod(6363)={p_Message.SettlMethod}, RegistID(513)={p_Message.RegistID}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(54)={p_Message.EndDate}, OrderQty(38)={p_Message.OrderQty}, Symbol(55)={p_Message.Symbol},  Exception: {ex?.ToString()}");
            }
        }

        // Xử lý khi nhận được 35=N04 từ sở
        public void HNXResponse_ReposFirmReport(MessageReposFirmReport p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"HNXResponse_ReposFirmReport -> start process when received exchange 35={MessageType.ReposFirmReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID},  QuoteID(117)={p_Message.QuoteID}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType},  OrderPartyID(4488)={p_Message.OrderPartyID}, InquiryMember(4499)={p_Message.InquiryMember}, Side(54)={p_Message.Side}, EffectiveTime(168)={p_Message.EffectiveTime}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate={p_Message.RepurchaseRate}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(54)={p_Message.EndDate}, Account(1)={p_Message.Account}, NoSide(552)={p_Message.NoSide}, MatchReportType(5632)={p_Message.MatchReportType}");
                // Map thông tin vào mem
                OrderInfo objOrder = null;
                string p_OrderNo = "";
                //Sở gửi 35=N04 ( 4488= của mình , 537=1) => phản hồi của lệnh đặt do thành viên gửi lên
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_1 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                    if (objOrder != null)
                    {
                        OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.QuoteID);
                        //objOrder.ExchangeID = p_Message.QuoteID;
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_ReposFirmReport -> Error can't find order info when call GetOrder_byClOrdID({p_Message.ClOrdID}) with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID},  QuoteID(117)={p_Message.QuoteID}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType},  OrderPartyID(4488)={p_Message.OrderPartyID} when received exchange");
                    }
                }
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_1 && p_Message.OrderPartyID != ConfigData.FirmID)
                {
                    /*
                     * lấy tag 11 trong message 35=N04 map với thông tin clOrdID trong memory để tìm bản ghi.
                        - nếu không thấy: insert bản ghi mới (có các thông tin orderNo,refMsgType, refExchangeID, quoteType,  side, clientID, clientIDCounterFirm, memberCounterFirm, settleDate1, settleDate2, endDate, repurchaseTerm, repurchaseRate, orderType, noSide, symbolFirminfo(numSide, symbol, orderQty, hedgeRate, mergePrice).  Trả thông tin cho kafka
                        - nếu thấy-> lý thuyết khong thể xảy ra tình huống này. Cần ghi lỗi
                     */
                    objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                    if (objOrder == null)
                    {
                        List<SymbolFirmObject> listSymbolFirmMem = null;
                        if (p_Message.RepoSideList != null && p_Message.RepoSideList.Count > 0)
                        {
                            listSymbolFirmMem = new List<SymbolFirmObject>();
                            ReposSide itemSite;
                            for (int i = 0; i < p_Message.RepoSideList.Count; i++)
                            {
                                itemSite = p_Message.RepoSideList[i];
                                //
                                SymbolFirmObject _ReposSideListResponse = new SymbolFirmObject();
                                _ReposSideListResponse.NumSide = itemSite.NumSide;
                                _ReposSideListResponse.Symbol = itemSite.Symbol;
                                _ReposSideListResponse.OrderQty = itemSite.OrderQty;
                                _ReposSideListResponse.HedgeRate = itemSite.HedgeRate;
                                _ReposSideListResponse.MergePrice = itemSite.Price;
                                //
                                listSymbolFirmMem.Add(_ReposSideListResponse);
                            }
                        }
                        //
                        string p_Side = "";
                        if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                            p_Side = ORDER_SIDE.SIDE_BUY;
                        else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                            p_Side = ORDER_SIDE.SIDE_SELL;
                        else
                            p_Side = "";
                        //
                        objOrder = new OrderInfo()
                        {
                            RefMsgType = MessageType.ReposFirm,
                            OrderNo = "",
                            ClOrdID = "",
                            ExchangeID = p_Message.QuoteID,
                            RefExchangeID = p_Message.RFQReqID,
                            SeqNum = 0,  // khi nào sở về mới update
                            Symbol = "",
                            Side = p_Side,
                            Price = 0,
                            OrderQty = 0,
                            QuoteType = CORE_QuoteTypeRepos.QuoteType_1,
                            CrossType = "",
                            ClientID = "",
                            ClientIDCounterFirm = p_Message.Account,
                            MemberCounterFirm = p_Message.OrderPartyID,
                            SettlMethod = p_Message.SettlMethod,
                            SettleDate1 = p_Message.SettlDate,
                            SettleDate2 = p_Message.SettlDate2,
                            EndDate = p_Message.EndDate,
                            RepurchaseTerm = p_Message.RepurchaseTerm,
                            RepurchaseRate = p_Message.RepurchaseRate,
                            OrderType = p_Message.OrdType,
                            NoSide = p_Message.NoSide,
                            //
                            SymbolFirmInfo = listSymbolFirmMem
                        };
                        OrderMemory.Add_NewOrder(objOrder);
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_ReposFirmReport -> Error find order info when received GetOrder_byClOrdID({p_Message.ClOrdID}) with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID},  QuoteID(117)={p_Message.QuoteID}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType},  OrderPartyID(4488)={p_Message.OrderPartyID} when received exchange");
                    }
                }
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_2 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    /*
                     * lấy tag 644 trong message 35=N04 map với thông tin exchangeID trong memory để tìm bản ghi đặt ban đầu.
                        - nếu không thấy-> lý thuyết không thể xảy ra tình huống này. Cần ghi lỗi
                        - nếu thấy, lấy thông tin orderno trong memory để tra ra kafka. Cập nhật mergePrice, repurchaseRate, hedgeRate, clientID vào thông tin memory vừa tìm được.
                     */
                    objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.RFQReqID);
                    if (objOrder != null)
                    {
                        p_OrderNo = objOrder.OrderNo;
                        //
                        objOrder.RepurchaseRate = p_Message.RepurchaseRate;
                        objOrder.ClientID = p_Message.Account;
                        // Xử lý cập nhật memory
                        List<SymbolFirmObject> listSymbolFirmMem = objOrder.SymbolFirmInfo;
                        if (listSymbolFirmMem != null)
                        {
                            for (int i = 0; i < listSymbolFirmMem.Count; i++)
                            {
                                SymbolFirmObject itemMem = listSymbolFirmMem[i];

                                // Duyệt và update lại Mem trong list
                                if (p_Message.RepoSideList != null && p_Message.RepoSideList.Count > 0)
                                {
                                    ReposSide itemSite;
                                    for (int j = 0; j < p_Message.RepoSideList.Count; j++)
                                    {
                                        itemSite = p_Message.RepoSideList[i];

                                        if (itemMem.Symbol == itemSite.Symbol)
                                        {
                                            itemMem.MergePrice = itemSite.Price;
                                            itemMem.HedgeRate = itemSite.HedgeRate;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_ReposFirmReport -> Error can't find order info when call GetOrder_byClOrdID({p_Message.ClOrdID}) with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID},  QuoteID(117)={p_Message.QuoteID}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType},  OrderPartyID(4488)={p_Message.OrderPartyID} when received exchange");
                    }
                }
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_2 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    /*
                     * lấy tag 644 trong message 35=N04  map với thông tin exchangeID trong memory để tìm bản ghi lệnh đặt.
                        - nếu thấy thì update các thông tin: symbol, side, orderqty -> map thông tin gửi về kafka, không lưu mem lệnh sửa này
                        - nếu không thấy: lý thuyết không thể xảy ra tình huống này. Cần ghi lỗi
                     */
                    objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.RFQReqID);
                    if (objOrder != null)
                    {
                        p_OrderNo = objOrder.OrderNo;
                        //
                        string p_Side = "";
                        if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                            p_Side = ORDER_SIDE.SIDE_BUY;
                        else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                            p_Side = ORDER_SIDE.SIDE_SELL;
                        else
                            p_Side = "";
                        objOrder.Side = p_Side;
                        // Xử lý cập nhật memory
                        List<SymbolFirmObject> listSymbolFirmMem = objOrder.SymbolFirmInfo;
                        if (listSymbolFirmMem != null)
                        {
                            for (int i = 0; i < listSymbolFirmMem.Count; i++)
                            {
                                SymbolFirmObject itemMem = listSymbolFirmMem[i];

                                // Duyệt và update lại Mem trong list
                                if (p_Message.RepoSideList != null && p_Message.RepoSideList.Count > 0)
                                {
                                    ReposSide itemSite;
                                    for (int j = 0; j < p_Message.RepoSideList.Count; j++)
                                    {
                                        itemSite = p_Message.RepoSideList[i];

                                        if (itemMem.Symbol == itemSite.Symbol)
                                        {
                                            itemMem.OrderQty = itemSite.OrderQty;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_ReposFirmReport -> Error can't find order info when call GetOrder_byExchangeID({p_Message.RFQReqID}) with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID},  QuoteID(117)={p_Message.QuoteID}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType},  OrderPartyID(4488)={p_Message.OrderPartyID} when received exchange");
                    }
                }
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_4 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    /* Map trực tiếp, không cần cập nhật mem
                      Lấy tag 11 trong message 35 = N04 map với thông tin clOrdID trong memory để tìm bản ghi.
                         - nếu không thấy-> lý thuyết khong thể xảy ra tình huống này.Cần ghi lỗi
                         - nếu thấy, lấy thông tin trong memory để trả ra.
                    */
                    objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                    if (objOrder == null)
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_ReposFirmReport -> Error can't find order info when call GetOrder_byClOrdID({p_Message.ClOrdID}) with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID},  QuoteID(117)={p_Message.QuoteID}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType},  OrderPartyID(4488)={p_Message.OrderPartyID} when received exchange");
                    }
                }
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_3 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    /*
                     * "lấy tag 644 trong message 35=N04 map với thông tin exchangeID trong memory để tìm bản ghi đặt ban đầu.
                        - nếu không thấy-> lý thuyết không thể xảy ra tình huống này. Cần ghi lỗi
                        - nếu thấy, lấy thông tin orderno trong memory để tra ra kafka."
                     */
                    objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.RFQReqID);
                    if (objOrder != null)
                    {
                        p_OrderNo = objOrder.OrderNo;
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_ReposFirmReport -> Error can't find order info when call GetOrder_byExchangeID({p_Message.RFQReqID}) with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID},  QuoteID(117)={p_Message.QuoteID}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType},  OrderPartyID(4488)={p_Message.OrderPartyID} when received exchange");
                    }
                }

                // Lấy thông tin từ memory để gửi ra kafka
                objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                if (objOrder != null)
                {
                    p_OrderNo = objOrder.OrderNo;
                }
                //
                FirmReposModel _Response = new FirmReposModel();
                _Response.MsgType = CORE_MsgType.MsgRS;//
                _Response.OrderNo = p_OrderNo;//
                _Response.RefMsgType = MessageType.ReposFirm;//
                _Response.ExchangeID = p_Message.QuoteID; //
                _Response.RefExchangeID = p_Message.RFQReqID;//

                int p_QuoteType = 0;
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_1 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_4)
                {
                    p_QuoteType = CORE_QuoteTypeRepos.QuoteType_1;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_2)
                {
                    p_QuoteType = CORE_QuoteTypeRepos.QuoteType_2;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_3)
                {
                    p_QuoteType = CORE_QuoteTypeRepos.QuoteType_3;
                }
                _Response.QuoteType = p_QuoteType.ToString(); //
                _Response.OrdType = p_Message.OrdType;//
                //
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_1 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_RE;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_1 && p_Message.OrderPartyID != ConfigData.FirmID)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_PA;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_2)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AM;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_3)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AC;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_4)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_CL;
                }
                //
                _Response.OrderPartyID = p_Message.OrderPartyID; //
                _Response.InquiryMember = !string.IsNullOrEmpty(p_Message.InquiryMember) ? p_Message.InquiryMember : "";
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
                if (p_Message.OrderPartyID == ConfigData.FirmID)
                    _Response.ClientID = p_Message.Account;
                else
                    _Response.ClientID = "";
                //
                if (p_Message.OrderPartyID != ConfigData.FirmID)
                {
                    _Response.ClientIDCounterFirm = p_Message.Account;
                    _Response.MemberCounterFirm = p_Message.OrderPartyID;
                }
                else
                {
                    _Response.ClientIDCounterFirm = "";
                    _Response.MemberCounterFirm = "";
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
                        _ReposSideListResponse.ExecPrice = itemSite.ExecPrice;
                        _ReposSideListResponse.MergePrice = itemSite.Price;
                        _ReposSideListResponse.ReposInterest = itemSite.ReposInterest;
                        _ReposSideListResponse.HedgeRate = itemSite.HedgeRate;
                        _ReposSideListResponse.SettleValue1 = itemSite.SettlValue;
                        _ReposSideListResponse.SettleValue2 = itemSite.SettlValue2;
                        //
                        listSymbolFirmInfo.Add(_ReposSideListResponse);
                    }
                }
                //
                _Response.SymbolFirmInfo = listSymbolFirmInfo;
                //
                _Response.MatchReportType = p_Message.MatchReportType;
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_1 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_2 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_3)
                {
                    _Response.RejectReasonCode = "";
                    _Response.RejectReason = "";
                }
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_4)
                {
                    _Response.RejectReasonCode = CORE_RejectReasonCode.Code_50009;
                    _Response.RejectReason = CORE_RejectReason.RejectReason_50009;
                }

                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);

                //
                // Gọi hàm gửi sang Kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);

                //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                p_Message.OrderNo = _Response.OrderNo;
                SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                //
                Logger.ResponseLog.Info($"HNXResponse_ReposFirmReport -> End process when received exchange 35={MessageType.ReposFirmReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"HNXResponse_ReposFirmReport -> Error process when received exchange 35={MessageType.ReposFirmReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID},  QuoteID(117)={p_Message.QuoteID}, RFQReqID(644)={p_Message.RFQReqID}, QuoteType(537)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType},  OrderPartyID(4488)={p_Message.OrderPartyID}, InquiryMember(4499)={p_Message.InquiryMember}, Side(54)={p_Message.Side}, EffectiveTime(168)={p_Message.EffectiveTime}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate={p_Message.RepurchaseRate}, SettlMethod(6363)={p_Message.SettlMethod}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(54)={p_Message.EndDate}, Account(1)={p_Message.Account}, NoSide(552)={p_Message.NoSide}, MatchReportType(5632)={p_Message.MatchReportType},  Exception: {ex?.ToString()}");
            }
        }

        // Xử lý khi nhận được 35=MR từ sở
        public void HNXResponse_ReposBCGDReport(MessageReposBCGDReport p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"HNXResponse_ReposBCGDReport -> start process when received exchange 35={MessageType.ReposBCGDReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID},  OrgOrderID(198)={p_Message.OrgOrderID}, OrderID(37)={p_Message.OrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType},  OrderPartyID(4488)={p_Message.OrderPartyID}, InquiryMember(4499)={p_Message.InquiryMember}, EffectiveTime(168)={p_Message.EffectiveTime}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(54)={p_Message.EndDate}, SettlMethod(6363)={p_Message.SettlMethod}, Account(1)={p_Message.Account}, CoAccount(2)={p_Message.CoAccount}, PartyID(448)={p_Message.PartyID}, CoPartyID(449)={p_Message.CoPartyID}, NoSide(552)={p_Message.NoSide}, MatchReportType(5362)={p_Message.MatchReportType}, RepoBCGDSideList(Count)={p_Message.RepoBCGDSideList}");

                //
                // Duyệt gửi ra
                List<ReposSideListResponse> listSymbolFirmInfo = new List<ReposSideListResponse>();
                List<SymbolFirmObject> listSymbolFirmObject = new List<SymbolFirmObject>();
                if (p_Message.RepoBCGDSideList != null && p_Message.RepoBCGDSideList.Count > 0)
                {
                    ReposSideReposBCGDReport itemSite;
                    for (int i = 0; i < p_Message.RepoBCGDSideList.Count; i++)
                    {
                        itemSite = p_Message.RepoBCGDSideList[i];
                        //
                        ReposSideListResponse _ReposSideListResponse = new ReposSideListResponse();
                        _ReposSideListResponse.NumSide = itemSite.NumSide;
                        _ReposSideListResponse.Symbol = itemSite.Symbol;
                        _ReposSideListResponse.OrderQty = itemSite.OrderQty;
                        _ReposSideListResponse.ExecPrice = itemSite.ExecPrice;
                        _ReposSideListResponse.MergePrice = itemSite.Price;
                        _ReposSideListResponse.ReposInterest = itemSite.ReposInterest;
                        _ReposSideListResponse.HedgeRate = itemSite.HedgeRate;
                        _ReposSideListResponse.SettleValue1 = itemSite.SettlValue;
                        _ReposSideListResponse.SettleValue2 = itemSite.SettlValue2;
                        listSymbolFirmInfo.Add(_ReposSideListResponse);
                        //
                        //
                        SymbolFirmObject symbolFirmObject = new SymbolFirmObject();
                        symbolFirmObject.NumSide = itemSite.NumSide;
                        symbolFirmObject.Symbol = itemSite.Symbol;
                        symbolFirmObject.OrderQty = itemSite.OrderQty;
                        symbolFirmObject.HedgeRate = itemSite.HedgeRate;
                        symbolFirmObject.MergePrice = itemSite.Price;
                        listSymbolFirmObject.Add(symbolFirmObject);
                    }
                    Logger.ResponseLog.Info($"HNXResponse_ReposBCGDReport -> process when received exchange 35={MessageType.ReposBCGDReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID},  OrgOrderID(198)={p_Message.OrgOrderID}, OrderID(37)={p_Message.OrderID}, listSymbolFirmInfo={System.Text.Json.JsonSerializer.Serialize(listSymbolFirmObject)}");
                }
                else
                {
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
                    listSymbolFirmInfo.Add(_ReposSideListResponse);

                    //
                    Logger.ResponseLog.Debug($"HNXResponse_ReposBCGDReport -> process when received exchange 35={MessageType.ReposBCGDReport} with MsgSeqNum(34)={p_Message.MsgSeqNum} with p_Message.RepoBCGDSideList is null or  p_Message.RepoBCGDSideList.Count = 0");
                }

                // Map thông tin vào mem
                OrderInfo objOrder = null;
                string p_OrderNo = "";
                string p_OrderType = p_Message.OrdType;
                string p_CrossType = "";
                int p_QuoteType = p_Message.QuoteType;
                //Sở gửi 35=MR
                if (p_Message.OrderPartyID == ConfigData.FirmID && p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_1)
                {
                    objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                    if (objOrder != null)
                    {
                        p_OrderNo = objOrder.OrderNo;
                        //
                        OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                        //objOrder.ExchangeID = p_Message.OrderID;
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_ReposBCGDReport -> process when received exchange 35={MessageType.ReposBCGDReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} => Can't find memory GetOrder_byClOrdID({p_Message.ClOrdID})");
                    }
                }
                if (p_Message.OrderPartyID != ConfigData.FirmID && p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_1)
                {
                    /*
                     * "lấy tag 11 trong message 35=MR map với thông tin clOrdID trong memory để tìm bản ghi.
                        - nếu không thấy: insert bản ghi mới (có các thông tin orderNo,refMsgType,exchangeID, refExchangeID, quoteType,  side, clientID, clientIDCounterFirm, memberCounterFirm, settleDate1, settleDate2, endDate, repurchaseTerm, repurchaseRate, orderType, noSide, symbolFirminfo(numSide, symbol, orderQty, hedgeRate, mergePrice).  Trả thông tin cho kafka
                        - nếu thấy-> lý thuyết khong thể xảy ra tình huống này. Cần ghi lỗi"

                     */
                    objOrder = OrderMemory.GetOrder_byClOrdID(p_Message.ClOrdID);
                    if (objOrder == null)
                    {
                        //
                        string p_Side = "";
                        string p_ClientID = "";
                        string p_ClientIDCounterFirm = "";
                        string p_MemberCounterFirm = "";

                        if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                        {
                            p_Side = ORDER_SIDE.SIDE_BUY;

                            p_ClientID = p_Message.CoAccount;
                            p_ClientIDCounterFirm = p_Message.Account;
                            p_MemberCounterFirm = p_Message.PartyID;
                        }
                        else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                        {
                            p_Side = ORDER_SIDE.SIDE_SELL;
                            p_ClientID = p_Message.Account;
                            p_ClientIDCounterFirm = p_Message.CoAccount;
                            p_MemberCounterFirm = p_Message.CoPartyID;
                        }

                        //
                        objOrder = new OrderInfo()
                        {
                            RefMsgType = MessageType.ReposBCGD,
                            OrderNo = "",
                            ClOrdID = "",
                            ExchangeID = p_Message.OrderID,
                            RefExchangeID = "",
                            SeqNum = 0,  // khi nào sở về mới update
                            Symbol = "",
                            Side = p_Side,
                            Price = 0,
                            OrderQty = 0,
                            QuoteType = CORE_QuoteTypeRepos.QuoteType_1,
                            CrossType = "",
                            ClientID = p_ClientID,
                            ClientIDCounterFirm = p_ClientIDCounterFirm,
                            MemberCounterFirm = p_MemberCounterFirm,
                            SettlMethod = p_Message.SettlMethod,
                            SettleDate1 = p_Message.SettlDate,
                            SettleDate2 = p_Message.SettlDate2,
                            EndDate = p_Message.EndDate,
                            RepurchaseTerm = p_Message.RepurchaseTerm,
                            RepurchaseRate = p_Message.RepurchaseRate,
                            OrderType = p_Message.OrdType,
                            NoSide = p_Message.NoSide,
                            //
                            SymbolFirmInfo = listSymbolFirmObject
                        };
                        OrderMemory.Add_NewOrder(objOrder);
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_ReposBCGDReport -> process when received exchange 35={MessageType.ReposBCGDReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} => find memory GetOrder_byClOrdID({p_Message.ClOrdID}) -> this can't happen");
                    }
                }
                if (p_Message.OrderPartyID == ConfigData.FirmID && p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_2)
                {
                    objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrgOrderID);
                    if (objOrder != null)
                    {
                        p_OrderNo = objOrder.OrderNo;
                        //
                        string p_ClientID = "";
                        string p_MemberCounterFirm = "";
                        if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                        {
                            p_ClientID = p_Message.CoAccount;
                            p_MemberCounterFirm = p_Message.PartyID;
                        }
                        else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                        {
                            p_ClientID = p_Message.Account;
                            p_MemberCounterFirm = p_Message.CoPartyID;
                        }

                        objOrder.RepurchaseTerm = p_Message.RepurchaseTerm;
                        objOrder.RepurchaseRate = p_Message.RepurchaseRate;
                        objOrder.SettlMethod = p_Message.SettlMethod;
                        objOrder.SettleDate2 = p_Message.SettlDate2;
                        objOrder.EndDate = p_Message.EndDate;
                        objOrder.ClientID = p_ClientID;
                        objOrder.MemberCounterFirm = p_MemberCounterFirm;
                        //
                        // Xử lý cập nhật memory
                        List<SymbolFirmObject> listSymbolFirmMem = objOrder.SymbolFirmInfo;
                        if (listSymbolFirmMem != null)
                        {
                            for (int i = 0; i < listSymbolFirmMem.Count; i++)
                            {
                                SymbolFirmObject itemMem = listSymbolFirmMem[i];

                                // Duyệt và update lại Mem trong list
                                if (p_Message.RepoBCGDSideList != null && p_Message.RepoBCGDSideList.Count > 0)
                                {
                                    ReposSideReposBCGDReport itemSite;
                                    for (int j = 0; j < p_Message.RepoBCGDSideList.Count; j++)
                                    {
                                        itemSite = p_Message.RepoBCGDSideList[i];

                                        if (itemMem.Symbol == itemSite.Symbol)
                                        {
                                            itemMem.MergePrice = itemSite.Price;
                                            itemMem.OrderQty = itemSite.OrderQty;
                                            itemMem.HedgeRate = itemSite.HedgeRate;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_ReposBCGDReport -> process when received exchange 35={MessageType.ReposBCGDReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} => Can't find memory GetOrder_byExchangeID({p_Message.OrgOrderID})");
                    }
                }
                if (p_Message.OrderPartyID != ConfigData.FirmID && p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_2)
                {
                    objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrgOrderID);
                    if (objOrder != null)
                    {
                        p_OrderNo = objOrder.OrderNo;
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_ReposBCGDReport -> process when received exchange 35={MessageType.ReposBCGDReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} => Can't find memory GetOrder_byExchangeID({p_Message.OrgOrderID})");
                    }
                }
                if (p_Message.OrderPartyID == ConfigData.FirmID && p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_3)
                {
                    // lấy tag 198 trong message 35=MR map với thông tin exchangeID trong memory để tìm bản ghi đặt ban đầu.
                    objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrgOrderID);
                    if (objOrder != null)
                    {
                        p_OrderNo = objOrder.OrderNo;
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_ReposBCGDReport -> process when received exchange 35={MessageType.ReposBCGDReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} => Can't find memory GetOrder_byExchangeID({p_Message.OrgOrderID})");
                    }
                }
                if (p_Message.OrderPartyID == ConfigData.FirmID && p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_3)
                {
                    // Lấy tag 198 trong message 35=MR map với thông tin exchangeID trong memory để tìm bản ghi lệnh đặt--> lấy ra thông tin orderNo để trả ra KAFKA
                    objOrder = OrderMemory.GetOrder_byExchangeID(p_Message.OrgOrderID);
                    if (objOrder != null)
                    {
                        p_OrderNo = objOrder.OrderNo;
                    }
                    else
                    {
                        Logger.ResponseLog.Warn($"HNXResponse_ReposBCGDReport -> process when received exchange 35={MessageType.ReposBCGDReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID} => Can't find memory GetOrder_byExchangeID({p_Message.OrgOrderID})");
                    }
                }
                //
                if (p_Message.MatchReportType == CORE_MatchReportType.MatchReportType_1)
                {
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_4)
                    {
                        /*
                         * "lấy tag 11 trong message 35=MR, 563=4, 5632=1 map với thông tin clOrdID trong memory để tìm bản ghi.
                            + nếu không thấy:
                                   - B1: insert bản ghi mới.
                                  -  B2: Lấy tag 198 , so sánh với dữ liệu exchangeID trên mem để lấy ra orderNo trả thông tin cho kafka
                                  -  B3:  Lấy orderNo của bản ghi ở B2, cập nhật vào orderNo của bản ghi ở B1
                            + nếu thấy-> Cập nhật exchangeID vào thông tin memory vừa tìm được. Trả thông tin cho kafka"
                         */

                        // lấy tag 11 trong message 35=MR, 563=4, 5632=1 map với thông tin clOrdID trong memory để tìm bản ghi.
                        objOrder = OrderMemory.GetOrderBy(p_ClOrdID: p_Message.ClOrdID);
                        if (objOrder == null)
                        {
                            //  - B1: insert bản ghi mới.
                            string p_Side = "";
                            string p_ClientID = "";
                            string p_ClientIDCounterFirm = "";
                            string p_MemberCounterFirm = "";

                            if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                            {
                                p_Side = ORDER_SIDE.SIDE_BUY;

                                p_ClientID = p_Message.CoAccount;
                                p_ClientIDCounterFirm = p_Message.Account;
                                p_MemberCounterFirm = p_Message.PartyID;
                            }
                            else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                            {
                                p_Side = ORDER_SIDE.SIDE_SELL;
                                p_ClientID = p_Message.Account;
                                p_ClientIDCounterFirm = p_Message.CoAccount;
                                p_MemberCounterFirm = p_Message.CoPartyID;
                            }
                            //
                            objOrder = new OrderInfo()
                            {
                                RefMsgType = MessageType.ReposBCGDModify,
                                OrderNo = "",
                                ClOrdID = "",
                                ExchangeID = p_Message.OrderID,
                                RefExchangeID = p_Message.OrgOrderID,
                                SeqNum = 0,  // khi nào sở về mới update
                                Symbol = "",
                                Side = p_Side,
                                Price = 0,
                                OrderQty = 0,
                                QuoteType = CORE_QuoteTypeRepos.QuoteType_2,
                                CrossType = "",
                                ClientID = p_ClientID,
                                ClientIDCounterFirm = p_ClientIDCounterFirm,
                                MemberCounterFirm = p_MemberCounterFirm,
                                SettlMethod = p_Message.SettlMethod,
                                SettleDate1 = p_Message.SettlDate,
                                SettleDate2 = p_Message.SettlDate2,
                                EndDate = p_Message.EndDate,
                                RepurchaseTerm = p_Message.RepurchaseTerm,
                                RepurchaseRate = p_Message.RepurchaseRate,
                                OrderType = p_Message.OrdType,
                                NoSide = p_Message.NoSide,
                                //
                                SymbolFirmInfo = listSymbolFirmObject
                            };
                            OrderMemory.Add_NewOrder(objOrder);

                            // B2: Lấy tag 198 , so sánh với dữ liệu exchangeID trên mem để lấy ra orderNo trả thông tin cho kafka
                            OrderInfo objOrder2 = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrgOrderID);
                            if (objOrder2 != null)
                            {
                                p_OrderNo = objOrder2.OrderNo;
                                p_OrderType = objOrder2.OrderType;
                                p_CrossType = objOrder2.CrossType;
                                //
                            }
                            //  -  B3:  Lấy orderNo của bản ghi ở B2, cập nhật vào orderNo của bản ghi ở B1
                            objOrder.OrderNo = p_OrderNo;
                            //
                        }
                        else
                        {
                            //  + nếu thấy-> Cập nhật exchangeID vào thông tin memory vừa tìm được. Trả thông tin cho kafka"
                            OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_OrderNo);
                            //objOrder.ExchangeID = p_Message.OrderID;
                            //
                            p_OrderNo = objOrder.OrderNo;
                            p_OrderType = objOrder.OrderType;
                            p_CrossType = objOrder.CrossType;
                        }
                    }
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_5)
                    {
                        /*
                         * "lấy tag 11 trong message 35=MR, 563=5, 5632=1 map với thông tin clOrdID trong memory để tìm bản ghi.
                            + nếu không thấy:
                                   - B1: insert bản ghi mới.
                                  -  B2: Lấy tag 198 , so sánh với dữ liệu exchangeID trên mem để lấy ra orderNo trả thông tin cho kafka
                                  -  B3:  Lấy orderNo của bản ghi ở B2, cập nhật vào orderNo của bản ghi ở B1
                            + nếu thấy-> Cập nhật exchangeID vào thông tin memory vừa tìm được. Trả thông tin cho kafka"
                         */
                        objOrder = OrderMemory.GetOrderBy(p_ClOrdID: p_Message.ClOrdID);
                        if (objOrder == null)
                        {
                            //  - B1: insert bản ghi mới.
                            string p_Side = "";
                            string p_ClientID = "";
                            string p_ClientIDCounterFirm = "";
                            string p_MemberCounterFirm = "";

                            if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                            {
                                p_Side = ORDER_SIDE.SIDE_BUY;

                                p_ClientID = p_Message.CoAccount;
                                p_ClientIDCounterFirm = p_Message.Account;
                                p_MemberCounterFirm = p_Message.PartyID;
                            }
                            else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                            {
                                p_Side = ORDER_SIDE.SIDE_SELL;
                                p_ClientID = p_Message.Account;
                                p_ClientIDCounterFirm = p_Message.CoAccount;
                                p_MemberCounterFirm = p_Message.CoPartyID;
                            }
                            //
                            objOrder = new OrderInfo()
                            {
                                RefMsgType = MessageType.ReposBCGDCancel,
                                OrderNo = "",
                                ClOrdID = "",
                                ExchangeID = p_Message.OrderID,
                                RefExchangeID = p_Message.OrgOrderID,
                                SeqNum = 0,  // khi nào sở về mới update
                                Symbol = "",
                                Side = p_Side,
                                Price = 0,
                                OrderQty = 0,
                                QuoteType = CORE_QuoteTypeRepos.QuoteType_2,
                                CrossType = "",
                                ClientID = p_ClientID,
                                ClientIDCounterFirm = p_ClientIDCounterFirm,
                                MemberCounterFirm = p_MemberCounterFirm,
                                SettlMethod = p_Message.SettlMethod,
                                SettleDate1 = p_Message.SettlDate,
                                SettleDate2 = p_Message.SettlDate2,
                                EndDate = p_Message.EndDate,
                                RepurchaseTerm = p_Message.RepurchaseTerm,
                                RepurchaseRate = p_Message.RepurchaseRate,
                                OrderType = p_Message.OrdType,
                                NoSide = p_Message.NoSide,
                                //
                                SymbolFirmInfo = listSymbolFirmObject
                            };
                            OrderMemory.Add_NewOrder(objOrder);

                            // B2: Lấy tag 198 , so sánh với dữ liệu exchangeID trên mem để lấy ra orderNo trả thông tin cho kafka
                            OrderInfo objOrder2 = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrgOrderID);
                            if (objOrder2 != null)
                            {
                                p_OrderNo = objOrder2.OrderNo;
                                p_OrderType = objOrder2.OrderType;
                                p_CrossType = objOrder2.CrossType;
                                //
                            }
                            //  -  B3:  Lấy orderNo của bản ghi ở B2, cập nhật vào orderNo của bản ghi ở B1
                            objOrder.OrderNo = p_OrderNo;
                            //
                        }
                        else
                        {
                            //  + nếu thấy-> Cập nhật exchangeID vào thông tin memory vừa tìm được. Trả thông tin cho kafka"
                            OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                            //objOrder.ExchangeID = p_Message.OrderID;
                            //
                            p_OrderNo = objOrder.OrderNo;
                            p_OrderType = objOrder.OrderType;
                            p_CrossType = objOrder.CrossType;
                        }
                    }
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_7)
                    {
                        /*
                         * "Lấy tag 37 trong message 35=MR, 563=7, 5632=1 map với thông tin exchangeID trong memory để tìm bản ghi
                            + Nếu thấy --> Từ thông tin bản ghi đó lấy ra orderNo để trả ra KAFKA
                            + Nếu không thấy --> Lấy tag 198 trong msg 35=MR, 563=7, 5632=1, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 198 và exchangeID chưa có và refMsgType =ME. Tìm được bản ghi -->  cập nhật exchangeID và lấy ra orderNo, orderType, crossType để trả ra KAFKA"
                         */
                        objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrderID);
                        if (objOrder != null)
                        {
                            p_OrderNo = objOrder.OrderNo;
                        }
                        else
                        {
                            objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrgOrderID, p_ExchangeID: "", p_RefMsgType: MessageType.ReposBCGDModify);
                            if (objOrder != null)
                            {
                                OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                                //objOrder.ExchangeID = p_Message.OrderID;
                                //
                                p_OrderNo = objOrder.OrderNo;
                                p_OrderType = objOrder.OrderType;
                                p_CrossType = objOrder.CrossType;
                            }
                        }
                    }
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_8)
                    {
                        /*
                         * "Lấy tag 37 trong message 35=MR, 563=8, 5632=1 map với thông tin exchangeID trong memory để tìm bản ghi
                            + Nếu thấy --> Từ thông tin bản ghi đó lấy ra orderNo để trả ra KAFKA
                            + Nếu không thấy --> Lấy tag 198 trong msg 35=MR, 563=8, 5632=1, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 198 và exchangeID chưa có và refMsgType =ME. Tìm được bản ghi -->  cập nhật exchangeID và lấy ra orderNo, orderType, crossType để trả ra KAFKA"
                         */
                        objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrderID);
                        if (objOrder != null)
                        {
                            p_OrderNo = objOrder.OrderNo;
                            p_OrderType = objOrder.OrderType;
                            p_CrossType = objOrder.CrossType;
                        }
                        else
                        {
                            objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrgOrderID, p_ExchangeID: "", p_RefMsgType: MessageType.ReposBCGDModify);
                            if (objOrder != null)
                            {
                                OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                                //objOrder.ExchangeID = p_Message.OrderID;
                                //
                                p_OrderNo = objOrder.OrderNo;
                                p_OrderType = objOrder.OrderType;
                                p_CrossType = objOrder.CrossType;
                            }
                        }
                    }
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_9)
                    {
                        /*
                         * "Lấy tag 37 trong message 35=MR, 563=9, 5632=1 map với thông tin exchangeID trong memory để tìm bản ghi
                            + Nếu thấy --> Từ thông tin bản ghi đó lấy ra orderNo để trả ra KAFKA
                            + Nếu không thấy --> Lấy tag 198 trong msg 35=MR, 563=9, 5632=1, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 198 và exchangeID chưa có và refMsgType =ME. Tìm được bản ghi -->  cập nhật exchangeID và lấy ra orderNo, orderType, crossType để trả ra KAFKA"
                         */
                        objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrderID);
                        if (objOrder != null)
                        {
                            //
                            p_OrderNo = objOrder.OrderNo;
                            p_OrderType = objOrder.OrderType;
                            p_CrossType = objOrder.CrossType;
                        }
                        else
                        {
                            objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrgOrderID, p_ExchangeID: "", p_RefMsgType: MessageType.ReposBCGDModify);
                            if (objOrder != null)
                            {
                                OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                                //objOrder.ExchangeID = p_Message.OrderID;
                                //
                                p_OrderNo = objOrder.OrderNo;
                                p_OrderType = objOrder.OrderType;
                                p_CrossType = objOrder.CrossType;
                            }
                        }
                    }
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_10)
                    {
                        /*
                         * "Lấy tag 37 trong message 35=MR, 563=10, 5632=1 map với thông tin exchangeID trong memory để tìm bản ghi
                            + Nếu thấy --> Từ thông tin bản ghi đó lấy ra orderNo để trả ra KAFKA
                            + Nếu không thấy --> Lấy tag 198 trong msg 35=MR, 563=10, 5632=1, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 198 và exchangeID chưa có và refMsgType =ME. Tìm được bản ghi -->  cập nhật exchangeID và lấy ra orderNo, orderType, crossType để trả ra KAFKA"
                         */
                        objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrderID);
                        if (objOrder != null)
                        {
                            p_OrderNo = objOrder.OrderNo;
                        }
                        else
                        {
                            objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrgOrderID, p_ExchangeID: "", p_RefMsgType: MessageType.ReposBCGDModify);
                            if (objOrder != null)
                            {
                                OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                                //objOrder.ExchangeID = p_Message.OrderID;
                                //
                                p_OrderNo = objOrder.OrderNo;
                                p_OrderType = objOrder.OrderType;
                                p_CrossType = objOrder.CrossType;
                            }
                        }
                    }
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_11)
                    {
                        /*
                         * "Lấy tag 37 trong message 35=MR, 563=11, 5632=1 map với thông tin exchangeID trong memory để tìm bản ghi
                            + Nếu thấy --> Từ thông tin bản ghi đó lấy ra orderNo để trả ra KAFKA
                            + Nếu không thấy --> Lấy tag 198 trong msg 35=MR, 563=11, 5632=1, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 198 và exchangeID chưa có và refMsgType =MC. Tìm được bản ghi -->  cập nhật exchangeID và lấy ra orderNo, orderType, crossType để trả ra KAFKA"
                         */
                        objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrderID);
                        if (objOrder != null)
                        {
                            //
                            p_OrderNo = objOrder.OrderNo;
                        }
                        else
                        {
                            objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrgOrderID, p_ExchangeID: "", p_RefMsgType: MessageType.ReposBCGDCancel);
                            if (objOrder != null)
                            {
                                OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                                //objOrder.ExchangeID = p_Message.OrderID;
                                //
                                p_OrderNo = objOrder.OrderNo;
                                p_OrderType = objOrder.OrderType;
                                p_CrossType = objOrder.CrossType;
                            }
                        }
                    }
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_12)
                    {
                        /*
                         * "Lấy tag 37 trong message 35=MR, 563=12, 5632=1 map với thông tin exchangeID trong memory để tìm bản ghi
                            + Nếu thấy --> Từ thông tin bản ghi đó lấy ra orderNo để trả ra KAFKA
                            + Nếu không thấy --> Lấy tag 198 trong msg 35=MR, 563=12, 5632=1, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 198 và exchangeID chưa có và refMsgType =MC. Tìm được bản ghi -->  cập nhật exchangeID và lấy ra orderNo, orderType, crossType để trả ra KAFKA"
                         */
                        objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrderID);
                        if (objOrder != null)
                        {
                            //
                            p_OrderNo = objOrder.OrderNo;
                        }
                        else
                        {
                            objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrgOrderID, p_ExchangeID: "", p_RefMsgType: MessageType.ReposBCGDCancel);
                            if (objOrder != null)
                            {
                                OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                                //objOrder.ExchangeID = p_Message.OrderID;
                                //
                                p_OrderNo = objOrder.OrderNo;
                                p_OrderType = objOrder.OrderType;
                                p_CrossType = objOrder.CrossType;
                            }
                        }
                    }
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_13)
                    {
                        /*
                         * "Lấy tag 37 trong message 35=MR, 563=13, 5632=1 map với thông tin exchangeID trong memory để tìm bản ghi
                             + Nếu thấy --> Từ thông tin bản ghi đó lấy ra orderNo để trả ra KAFKA
                             + Nếu không thấy --> Lấy tag 198 trong msg 35=MR, 563=13, 5632=1, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 198 và exchangeID chưa có và refMsgType =MC. Tìm được bản ghi -->  cập nhật exchangeID và lấy ra orderNo, orderType, quoteType để trả ra KAFKA
                             + Nếu sau cả 2 thao tác trên vẫn k thấy --> insert 1 bản ghi vào mem"
                         */
                        objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrderID);
                        if (objOrder != null)
                        {
                            //
                            p_OrderNo = objOrder.OrderNo;
                        }
                        else
                        {
                            objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrgOrderID, p_ExchangeID: "", p_RefMsgType: MessageType.ReposBCGDCancel);
                            if (objOrder != null)
                            {
                                OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                                //objOrder.ExchangeID = p_Message.OrderID;
                                //
                                p_OrderNo = objOrder.OrderNo;
                                p_OrderType = objOrder.OrderType;
                                p_QuoteType = objOrder.QuoteType;
                                p_CrossType = objOrder.CrossType;
                            }
                            else
                            {
                                string p_Side = "";

                                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                                {
                                    p_Side = ORDER_SIDE.SIDE_BUY;
                                }
                                else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                                {
                                    p_Side = ORDER_SIDE.SIDE_SELL;
                                }
                                objOrder = new OrderInfo()
                                {
                                    RefMsgType = MessageType.ReposBCGDCancel,
                                    OrderNo = p_OrderNo,
                                    ClOrdID = "",
                                    ExchangeID = p_Message.OrderID,
                                    RefExchangeID = "",
                                    SeqNum = 0,  // khi nào sở về mới update
                                    Symbol = "",
                                    Side = p_Side,
                                    Price = 0,
                                    OrderQty = 0,
                                    QuoteType = CORE_QuoteTypeRepos.QuoteType_2,
                                    CrossType = "",
                                    ClientID = "",
                                    ClientIDCounterFirm = "",
                                    MemberCounterFirm = "",
                                    SettlMethod = 0,
                                    SettleDate1 = "",
                                    SettleDate2 = "",
                                    EndDate = "",
                                    RepurchaseTerm = 0,
                                    RepurchaseRate = 0,
                                    OrderType = p_Message.OrdType,
                                    NoSide = 0
                                };
                                OrderMemory.Add_NewOrder(objOrder);
                            }
                        }
                    }
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_14)
                    {
                        /*
                         * "Lấy tag 37 trong message 35=MR, 563=14, 5632=1 map với thông tin exchangeID trong memory để tìm bản ghi
                            + Nếu thấy --> Từ thông tin bản ghi đó lấy ra orderNo để trả ra KAFKA
                            + Nếu không thấy --> Lấy tag 198 trong msg 35=MR, 563=14, 5632=1, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 198 và exchangeID chưa có và refMsgType =MC. Tìm được bản ghi -->  cập nhật exchangeID và lấy ra orderNo, orderType, crossType để trả ra KAFKA"
                         */
                        objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrderID);
                        if (objOrder != null)
                        {
                            p_OrderNo = objOrder.OrderNo;
                        }
                        else
                        {
                            objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrgOrderID, p_ExchangeID: "", p_RefMsgType: MessageType.ReposBCGDCancel);
                            if (objOrder != null)
                            {
                                OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                                //objOrder.ExchangeID = p_Message.OrderID;
                                //
                                p_OrderNo = objOrder.OrderNo;
                                p_OrderType = objOrder.OrderType;
                                p_CrossType = objOrder.CrossType;
                            }
                        }
                    }
                }
                if (p_Message.MatchReportType == CORE_MatchReportType.MatchReportType_2)
                {
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_4)
                    {
                        /*
                         * "lấy tag 11 trong message 35=MR, 563=4, 5632=2 map với thông tin clOrdID trong memory để tìm bản ghi.
                            + nếu không thấy:  insert bản ghi mới (exchangeid, refExchangeID, refMsgType, quoteType, side, clientid, clientidCounterFirm, memberCounterFirm, orderType,settleMethod, settleDate1, settleDate2, endDate, repurrchaseTerm, repurchaseRate, numSide, noSide, symbolFirmInfo(symbol, orderQty, price, mergePrice, hedgeRate )
                            + nếu thấy-> Cập nhật exchangeID vào thông tin memory vừa tìm được. Trả thông tin cho kafka"
                         */
                        objOrder = OrderMemory.GetOrderBy(p_ClOrdID: p_Message.ClOrdID);
                        if (objOrder == null)
                        {
                            //  - B1: insert bản ghi mới.
                            string p_Side = "";
                            string p_ClientID = "";
                            string p_ClientIDCounterFirm = "";
                            string p_MemberCounterFirm = "";

                            if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                            {
                                p_Side = ORDER_SIDE.SIDE_BUY;

                                p_ClientID = p_Message.CoAccount;
                                p_ClientIDCounterFirm = p_Message.Account;
                                p_MemberCounterFirm = p_Message.PartyID;
                            }
                            else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                            {
                                p_Side = ORDER_SIDE.SIDE_SELL;
                                p_ClientID = p_Message.Account;
                                p_ClientIDCounterFirm = p_Message.CoAccount;
                                p_MemberCounterFirm = p_Message.CoPartyID;
                            }

                            objOrder = new OrderInfo()
                            {
                                RefMsgType = MessageType.ReposBCGDModify,
                                OrderNo = p_OrderNo,
                                ClOrdID = "",
                                ExchangeID = p_Message.OrderID,
                                RefExchangeID = p_Message.OrgOrderID,
                                SeqNum = 0,  // khi nào sở về mới update
                                Symbol = "",
                                Side = p_Side,
                                Price = 0,
                                OrderQty = 0,
                                QuoteType = CORE_QuoteTypeRepos.QuoteType_7,
                                CrossType = "",
                                ClientID = p_ClientID,
                                ClientIDCounterFirm = p_ClientIDCounterFirm,
                                MemberCounterFirm = p_MemberCounterFirm,
                                SettlMethod = p_Message.SettlMethod,
                                SettleDate1 = p_Message.SettlDate,
                                SettleDate2 = p_Message.SettlDate2,
                                EndDate = p_Message.EndDate,
                                RepurchaseTerm = p_Message.RepurchaseTerm,
                                RepurchaseRate = p_Message.RepurchaseRate,
                                OrderType = p_Message.OrdType,
                                NoSide = p_Message.NoSide,
                                //
                                SymbolFirmInfo = listSymbolFirmObject
                            };
                            OrderMemory.Add_NewOrder(objOrder);
                        }
                        else
                        {
                            //  + nếu thấy-> Cập nhật exchangeID vào thông tin memory vừa tìm được. Trả thông tin cho kafka"
                            OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                            //
                            p_OrderNo = objOrder.OrderNo;
                            p_OrderType = objOrder.OrderType;
                            p_CrossType = objOrder.CrossType;
                        }
                    }
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_7)
                    {
                        /*
                         * "Lấy tag 37 trong message 35=MR, 563=7, 5632=2 map với thông tin exchangeID trong memory để tìm bản ghi
                            + Nếu thấy --> Từ thông tin bản ghi đó lấy ra orderNo để trả ra KAFKA
                            + Nếu không thấy --> Lấy tag 198 trong msg 35=MR, 563=7, 5632=2, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 198 và exchangeID chưa có và refMsgType =ME. Tìm được bản ghi -->  cập nhật exchangeID và lấy ra orderNo, orderType, crossType để trả ra KAFKA"
                         */
                        objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrderID);
                        if (objOrder != null)
                        {
                            //
                            p_OrderNo = objOrder.OrderNo;
                        }
                        else
                        {
                            objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrgOrderID, p_ExchangeID: "", p_RefMsgType: MessageType.ReposBCGDModify);
                            if (objOrder != null)
                            {
                                OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                                //objOrder.ExchangeID = p_Message.OrderID;
                                //
                                p_OrderNo = objOrder.OrderNo;
                                p_OrderType = objOrder.OrderType;
                                p_CrossType = objOrder.CrossType;
                            }
                        }
                    }
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_8)
                    {
                        /*
                         * "Lấy tag 37 trong message 35=MR, 563=8, 5632=2 map với thông tin exchangeID trong memory để tìm bản ghi
                            + Nếu thấy --> Từ thông tin bản ghi đó lấy ra orderNo để trả ra KAFKA
                            + Nếu không thấy --> Lấy tag 198 trong msg 35=MR, 563=8, 5632=2, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 198 và exchangeID chưa có và refMsgType =ME. Tìm được bản ghi -->  cập nhật exchangeID và lấy ra orderNo, orderType, crossType để trả ra KAFKA"

                         */
                        objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrderID);
                        if (objOrder != null)
                        {
                            p_OrderNo = objOrder.OrderNo;
                            p_OrderType = objOrder.OrderType;
                            p_CrossType = objOrder.CrossType;
                        }
                        else
                        {
                            objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrgOrderID, p_ExchangeID: "", p_RefMsgType: MessageType.ReposBCGDModify);
                            if (objOrder != null)
                            {
                                OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                                //
                                p_OrderNo = objOrder.OrderNo;
                                p_OrderType = objOrder.OrderType;
                                p_CrossType = objOrder.CrossType;
                            }
                        }
                    }
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_9)
                    {
                        /*
                         *  "Lấy tag 37 trong message 35=MR, 563=9, 5632=2 map với thông tin exchangeID trong memory để tìm bản ghi
                            + Nếu thấy --> Từ thông tin bản ghi đó lấy ra orderNo để trả ra KAFKA
                            + Nếu không thấy --> Lấy tag 198 trong msg 35=MR, 563=9, 5632=2, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 198 và exchangeID chưa có và refMsgType =ME. Tìm được bản ghi -->  cập nhật exchangeID và lấy ra orderNo, orderType, quoteType để trả ra KAFKA
                            + Nếu dùng cả tag 37 và 198 tìm vẫn k thấy --> insert 1 bản ghi vào mem"
                         */
                        objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrderID);
                        if (objOrder != null)
                        {
                            //
                            p_OrderNo = objOrder.OrderNo;
                        }
                        else
                        {
                            objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrgOrderID, p_ExchangeID: "", p_RefMsgType: MessageType.ReposBCGDModify);
                            if (objOrder != null)
                            {
                                OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                                //
                                p_OrderNo = objOrder.OrderNo;
                                p_OrderType = objOrder.OrderType;
                                p_CrossType = objOrder.CrossType;
                            }
                            else
                            {
                                string p_Side = "";
                                string p_ClientID = "";
                                string p_ClientIDCounterFirm = "";
                                string p_MemberCounterFirm = "";

                                if (p_Message.Side == CORE_OrderSide.SIDE_BUY)
                                {
                                    p_Side = ORDER_SIDE.SIDE_BUY;

                                    p_ClientID = p_Message.CoAccount;
                                    p_ClientIDCounterFirm = p_Message.Account;
                                    p_MemberCounterFirm = p_Message.PartyID;
                                }
                                else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                                {
                                    p_Side = ORDER_SIDE.SIDE_SELL;
                                    p_ClientID = p_Message.Account;
                                    p_ClientIDCounterFirm = p_Message.CoAccount;
                                    p_MemberCounterFirm = p_Message.CoPartyID;
                                }

                                objOrder = new OrderInfo()
                                {
                                    RefMsgType = MessageType.ReposBCGDModify,
                                    OrderNo = "",
                                    ClOrdID = "",
                                    ExchangeID = p_Message.OrderID,
                                    RefExchangeID = "",
                                    SeqNum = 0,  // khi nào sở về mới update
                                    Symbol = "",
                                    Side = p_Side,
                                    Price = 0,
                                    OrderQty = 0,
                                    QuoteType = CORE_QuoteTypeRepos.QuoteType_7,
                                    CrossType = "",
                                    ClientID = p_ClientID,
                                    ClientIDCounterFirm = p_ClientIDCounterFirm,
                                    MemberCounterFirm = p_MemberCounterFirm,
                                    SettlMethod = p_Message.SettlMethod,
                                    SettleDate1 = p_Message.SettlDate,
                                    SettleDate2 = p_Message.SettlDate2,
                                    EndDate = p_Message.EndDate,
                                    RepurchaseTerm = p_Message.RepurchaseTerm,
                                    RepurchaseRate = p_Message.RepurchaseRate,
                                    OrderType = p_Message.OrdType,
                                    NoSide = p_Message.NoSide,
                                    //
                                    SymbolFirmInfo = listSymbolFirmObject
                                };
                                OrderMemory.Add_NewOrder(objOrder);
                            }
                        }
                    }
                    if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_10)
                    {
                        /*
                         * "Lấy tag 37 trong message 35=MR, 563=10, 5632=2 map với thông tin exchangeID trong memory để tìm bản ghi
                            + Nếu thấy --> Từ thông tin bản ghi đó lấy ra orderNo để trả ra KAFKA
                            + Nếu không thấy --> Lấy tag 198 trong msg 35=MR, 563=10, 5632=2, tìm thông tin refExchangeID trong mem, lấy bản ghi có refExchangeID = giá trị tag 198 và exchangeID chưa có và refMsgType =ME. Tìm được bản ghi -->  cập nhật exchangeID và lấy ra orderNo, orderType, crossType để trả ra KAFKA"
                         */
                        objOrder = OrderMemory.GetOrderBy(p_ExchangeID: p_Message.OrderID);
                        if (objOrder != null)
                        {
                            //
                            p_OrderNo = objOrder.OrderNo;
                        }
                        else
                        {
                            objOrder = OrderMemory.GetOrderBy(p_RefExchangeID: p_Message.OrgOrderID, p_ExchangeID: "", p_RefMsgType: MessageType.ReposBCGDModify);
                            if (objOrder != null)
                            {
                                OrderMemory.UpdateOrderInfo(objOrder, p_ExchangeID: p_Message.OrderID);
                                //
                                p_OrderNo = objOrder.OrderNo;
                                p_OrderType = objOrder.OrderType;
                                p_CrossType = objOrder.CrossType;
                            }
                        }
                    }
                }
                //
                FirmReposModel _Response = new FirmReposModel();
                _Response.MsgType = CORE_MsgType.MsgRS;//
                _Response.OrderNo = p_OrderNo;//
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_1 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_18)
                {
                    _Response.RefMsgType = MessageType.ReposBCGD;// MA
                }
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_2 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_4 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_7 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_8 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_9 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_10)
                {
                    _Response.RefMsgType = MessageType.ReposBCGDModify;// ME
                }
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_3 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_5 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_11 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_12 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_13 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_14)
                {
                    _Response.RefMsgType = MessageType.ReposBCGDCancel;// MC
                }
                _Response.ExchangeID = p_Message.OrderID; //
                _Response.RefExchangeID = p_Message.OrgOrderID;//

                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_1 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_2 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_3 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_18)
                {
                    p_QuoteType = CORE_QuoteTypeRepos.QuoteType_1;
                }
                if (p_Message.MatchReportType == 1 && (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_4 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_5 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_7 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_8 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_9 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_10 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_11 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_12 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_13 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_14))
                {
                    p_QuoteType = CORE_QuoteTypeRepos.QuoteType_2;
                }
                if (p_Message.MatchReportType == 2 && (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_4 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_5 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_7 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_8 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_9 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_10 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_11 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_12 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_13 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_14))
                {
                    p_QuoteType = CORE_QuoteTypeRepos.QuoteType_7;
                }
                _Response.QuoteType = p_QuoteType.ToString();//
                _Response.OrdType = p_OrderType;

                //
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_1 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_RE;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_1 && p_Message.OrderPartyID != ConfigData.FirmID)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_PA;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_2 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_7)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AM;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_3 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_11)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_AC;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_18)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_CL;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_4 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_PM;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_4 && p_Message.OrderPartyID != ConfigData.FirmID)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_MR;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_5 && p_Message.OrderPartyID == ConfigData.FirmID)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_PC;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_5 && p_Message.OrderPartyID != ConfigData.FirmID)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_CR;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_9 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_13)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_EC;
                }
                else if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_8 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_10 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_12 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_14 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_14)
                {
                    _Response.OrderStatus = CORE_OrderStatus.OrderStatus_EJ;
                }
                //
                _Response.OrderPartyID = p_Message.OrderPartyID; //
                _Response.InquiryMember = !string.IsNullOrEmpty(p_Message.InquiryMember) ? p_Message.InquiryMember : ""; //

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
                    _Response.Side = ORDER_SIDE.SIDE_BUY;
                    _Response.ClientID = p_Message.CoAccount;
                    _Response.ClientIDCounterFirm = p_Message.Account;
                    _Response.MemberCounterFirm = p_Message.PartyID;
                }
                else if (p_Message.Side == CORE_OrderSide.SIDE_SELL)
                {
                    _Response.Side = ORDER_SIDE.SIDE_SELL;
                    _Response.ClientID = p_Message.Account;
                    _Response.ClientIDCounterFirm = p_Message.CoAccount;
                    _Response.MemberCounterFirm = p_Message.CoPartyID;
                }
                //
                _Response.NoSide = p_Message.NoSide;

                //
                _Response.SymbolFirmInfo = listSymbolFirmInfo;
                //
                _Response.MatchReportType = p_Message.MatchReportType;
                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_1 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_2 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_3 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_4 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_5 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_7 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_9 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_11 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_13)
                {
                    _Response.RejectReasonCode = "";
                    _Response.RejectReason = "";
                }

                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_8)
                {
                    _Response.RejectReasonCode = CORE_RejectReasonCode.Code_50001;
                    _Response.RejectReason = CORE_RejectReason.RejectReason_50001;
                }

                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_12)
                {
                    _Response.RejectReasonCode = CORE_RejectReasonCode.Code_50002;
                    _Response.RejectReason = CORE_RejectReason.RejectReason_50002;
                }

                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_10 || p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_14)
                {
                    _Response.RejectReasonCode = CORE_RejectReasonCode.Code_50000;
                    _Response.RejectReason = CORE_RejectReason.RejectReason_50000;
                }

                if (p_Message.QuoteType == CORE_QuoteTypeRepos.QuoteType_18)
                {
                    _Response.RejectReasonCode = CORE_RejectReasonCode.Code_50005;
                    _Response.RejectReason = CORE_RejectReason.RejectReason_50005;
                }

                _Response.Text = !string.IsNullOrEmpty(p_Message.Text) ? p_Message.Text : "";
                _Response.SendingTime = HNX.FIXMessage.Utils.Convert.ToFIXUTCTimestamp(p_Message.GetSendingTime);

                //
                // Gọi hàm gửi sang Kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.FORWARD_FROM_HNX);
                //
                //2024.05.22 BacND: bổ sung thêm ghi vào DB sau khi nhận về từ sở
                p_Message.OrderNo = _Response.OrderNo;
                SharedStorageProcess.c_DataStorageProcess.EnqueueData(p_Message, Data_SoR.Recei);
                //
                Logger.ResponseLog.Info($"HNXResponse_ReposBCGDReport -> End process when received exchange 35={MessageType.ReposBCGDReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID}; send queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"HNXResponse_ReposBCGDReport -> Error process when received exchange 35={MessageType.ReposBCGDReport} with MsgSeqNum(34)={p_Message.MsgSeqNum}, ClOrdID(11)={p_Message.ClOrdID},  OrgOrderID(198)={p_Message.OrgOrderID}, OrderID(37)={p_Message.OrderID}, QuoteType(563)={p_Message.QuoteType}, OrdType(40)={p_Message.OrdType},  OrderPartyID(4488)={p_Message.OrderPartyID}, InquiryMember(4499)={p_Message.InquiryMember}, EffectiveTime(168)={p_Message.EffectiveTime}, RepurchaseTerm(226)={p_Message.RepurchaseTerm}, RepurchaseRate(227)={p_Message.RepurchaseRate}, SettlDate(64)={p_Message.SettlDate}, SettlDate2(193)={p_Message.SettlDate2}, EndDate(54)={p_Message.EndDate}, SettlMethod(6363)={p_Message.SettlMethod}, Account(1)={p_Message.Account}, CoAccount(2)={p_Message.CoAccount}, PartyID(448)={p_Message.PartyID}, CoPartyID(449)={p_Message.CoPartyID}, NoSide(552)={p_Message.NoSide}, MatchReportType(5362)={p_Message.MatchReportType},  Exception: {ex?.ToString()}");
            }
        }

        public void HNXResponse_UserResponse(MessageUserResponse p_Message)
        {
            Logger.ResponseLog.Info($"HNXResponse_UserResponse -> start process when received exchange 35={MessageType.UserResponse} with MsgSeqNum(34)={p_Message.MsgSeqNum}, UserStatus(926)={p_Message.UserStatus}, UserStatusText(927)={p_Message.UserStatusText}");
            //
            ShareMemoryData.c_UserStatus = p_Message.UserStatus;
            ShareMemoryData.c_UserStatusText = p_Message.UserStatusText;
        }
    }
}