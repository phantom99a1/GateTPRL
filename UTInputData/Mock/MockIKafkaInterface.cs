using BusinessProcessResponse;
using HNX.FIXMessage;

namespace UTInputData.Mock
{
    public class MockIKafkaInterface : IResponseInterface
    {
        public int Send2Kafka(string p_TranId, string p_KafkaTopic, string p_KafkaData, string pLogType)
        {
            return 1;
        }

        //Phản hồi cho Api
        public void ResponseApi2Kafka(FIXMessageBase fMgsBase, int Seq)
        { }

        //Thông báo lệnh được gửi đi
        public void ResponseGateSend2HNX(FIXMessageBase fMsgBase)
        { }

        //Trả Api khi HNX gửi phản hồi
        public void HNXSendQuoteStatusReport(MessageQuoteSatusReport Message)
        { }

        public void HNXResponseExcQuote(MessageER_ExecOrder Message)
        { }

        public void HNXResponse_EROrderCancel(MessageER_OrderCancel Message)
        { }

        public void HNXResponse_ExecReplace(MessageER_OrderReplace Message)
        { }

        public void HXNReponse_ExecCancel(MessageER_OrderCancel Message)
        { }

        // 35= s
        public void HNXSendOrderCross(MessageNewOrderCross Message)
        { }

        // 35= t
        public void HNXResponse_CrossOrderCancelReplace(CrossOrderCancelReplaceRequest Message)
        { }

        // 35=u
        public void HNXResponse_CrossOrderCancelRequest(CrossOrderCancelRequest Message)
        { }

        public void HNXSendReject(MessageReject Message)
        { }

        public void ResponseHNXSendTradingSessionStatus(MessageTradingSessionStatus message)
        { }

        public void ResponseHNXSendSecurityStatus(MessageSecurityStatus message)
        { }

        //Gate gửi lệnh fail hoặc từ chối gửi lệnh
        public void ReportGateReject(FIXMessageBase fMsg, string text, string code)
        { }

        public int NumOfMsg()
        { return 1; }

        public int Send2KafkaObject(object p_object)
        { return 1; }

        public void HNXResponse_InquiryReposReponse(MessageReposInquiryReport p_Message)
        { }

        public void HNXResponse_ReposFirmReport(MessageReposFirmReport p_Message)
        { }

        public void HNXResponse_ExecOrderRepos(MessageExecOrderRepos p_Message)
        { }

        public void HNXResponse_ReposBCGDReport(MessageReposBCGDReport message)
        {
        }

        public void HNXResponse_ExecOrder(MessageER_Order Message)
        {
        }

        public void HNXResponse_ExecOrderReject(MessageER_OrderReject Message)
        {
        }

        public void ResponseHNXTopicTradingInfomation(MessageTopicTradingInfomation message)
        {
        }

        public void HNXResponse_UserResponse(MessageUserResponse message)
        {
            
        }

        public int SequenceGateAccept { get { return 0; } }

        public int SequenceGateSendHNX { get { return 0; } }

        public int SequenceGateFwdFromHNX { get { return 0; } }
    }
}