using CommonLib;
using HNX.FIXMessage;
using KafkaInterface;

namespace BusinessProcessResponse
{
    public partial class ResponseInterface : IResponseInterface
    {
        //  Xử lý khi nhận được 35=h
        public void ResponseHNXSendTradingSessionStatus(MessageTradingSessionStatus p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseHNXSendTradingSessionStatus -> Start process when received exchange message 35=h; with MsgSeqNum(34)={p_Message.MsgSeqNum}, TradSesReqID={p_Message.TradSesReqID}, TradingSessionID={p_Message.TradingSessionID}, TradSesStatus={p_Message.TradSesStatus}, TradSesStartTime={p_Message.TradSesStartTime}, TradSesMode={p_Message.TradSesMode}");
                //
                ResponseTradingSession _Response = new ResponseTradingSession(p_Message);
                //Send Kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_TradingInfo, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseHNXSendTradingSessionStatus -> End process when received exchange 35=h; with MsgSeqNum={p_Message.MsgSeqNum}, TradSesReqID={p_Message.TradSesReqID}, TradingSessionID={p_Message.TradingSessionID}, TradSesStatus={p_Message.TradSesStatus}, TradSesStartTime={p_Message.TradSesStartTime}, TradSesMode={p_Message.TradSesMode}; sended queue kafka -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_TradingInfo}, MsgType={MessageType.TradingSessionStatus}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseHNXSendTradingSessionStatus -> Error process when received exchange build data TradingSession send kafka, Exception: {ex?.ToString()}");
            }
        }

        //  Xử lý khi nhận đượcg 35=f
        public void ResponseHNXSendSecurityStatus(MessageSecurityStatus p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseHNXSendSecurityStatus -> Start process when received exchange message 35=f; with MsgSeqNum(34)={p_Message.MsgSeqNum}, Symbol={p_Message.Symbol}, DateNo={p_Message.DateNo}, SecurityType={p_Message.SecurityType}, MaturityDate={p_Message.MaturityDate}, IssueDate={p_Message.IssueDate}, Issuer={p_Message.Issuer}, SecurityDesc={p_Message.SecurityDesc}, HighPx={p_Message.HighPx}, LowPx={p_Message.LowPx}, HighPxOut={p_Message.HighPxOut}, LowPxRep={p_Message.LowPxRep}, HighPxRep={p_Message.HighPxRep}, LowPxRep={p_Message.LowPxRep}, LastPx={p_Message.LastPx}, SecurityTradingStatus={p_Message.SecurityTradingStatus}, BuyVolume={p_Message.BuyVolume}, TotalListingQtty={p_Message.TotalListingQtty}, TradingSessionSubID={p_Message.TradingSessionSubID}, TypeRule={p_Message.TypeRule}");
                //
                ResponseSecurityStatus _Response = new ResponseSecurityStatus(p_Message);
                //Send Kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_TradingInfo, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseHNXSendSecurityStatus -> End process when exchanreceived exchange message 35=h; with  MsgSeqNum(34)={p_Message.MsgSeqNum}, Symbol={p_Message.Symbol}, DateNo={p_Message.DateNo}, SecurityType={p_Message.SecurityType}, MaturityDate={p_Message.MaturityDate}, IssueDate={p_Message.IssueDate}, Issuer={p_Message.Issuer}, SecurityDesc={p_Message.SecurityDesc}, HighPx={p_Message.HighPx}, LowPx={p_Message.LowPx}, HighPxOut={p_Message.HighPxOut}, LowPxRep={p_Message.LowPxRep}, HighPxRep={p_Message.HighPxRep}, LowPxRep={p_Message.LowPxRep}, LastPx={p_Message.LastPx}, SecurityTradingStatus={p_Message.SecurityTradingStatus}, BuyVolume={p_Message.BuyVolume}, TotalListingQtty={p_Message.TotalListingQtty}, TradingSessionSubID={p_Message.TradingSessionSubID}, TypeRule={p_Message.TypeRule}, ; Send kafka success -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_TradingInfo}, MsgType={_Response.MsgType}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseHNXSendSecurityStatus -> Error process when received exchange build data SecurityStatus send kafka, Exception: {ex?.ToString()}");
            }
        }

        // Xử lý khi nhận được 35=MN
        public void ResponseHNXTopicTradingInfomation(MessageTopicTradingInfomation p_Message)
        {
            try
            {
                Logger.ResponseLog.Info($"ResponseHNXTopicTradingInfomation -> Start process when received exchange message 35={MessageType.TopicTradingInfomation}; with MsgSeqNum(34)={p_Message.MsgSeqNum},InquiryMember={p_Message.InquiryMember}, Symbol={p_Message.Symbol}, SendingTime={p_Message.GetSendingTime}");
                //
                ResponseTopicTradingInfomation _Response = new ResponseTopicTradingInfomation(p_Message);
                //Send Kafka
                c_KafkaClient.Send2KafkaObject(ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_TradingInfo, _Response, p_Message.TimeInit, p_Message.MsgSeqNum, FlagSendKafka.REPORT_SEND_HNX);
                //
                Logger.ResponseLog.Info($"ResponseHNXTopicTradingInfomation -> End proces swhen received exchange message 35={MessageType.TopicTradingInfomation}; with MsgSeqNum(34)={p_Message.MsgSeqNum}; Send kafka success -> Topic={ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_TradingInfo}, MsgType={MessageType.TopicTradingInfomation}");
            }
            catch (Exception ex)
            {
                Logger.ResponseLog.Error($"ResponseHNXTopicTradingInfomation -> Error process when received exchange build data MessageTopicTradingInfomation send kafka, Exception: {ex?.ToString()}");
            }
        }
    }
}