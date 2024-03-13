using HNX.FIXMessage;

namespace BusinessProcessResponse
{
    public interface IResponseInterface
    {
        //Phản hồi cho Api
        public void ResponseApi2Kafka(FIXMessageBase fMgsBase, int OrgSeq);

        //Thông báo lệnh được gửi đi
        public void ResponseGateSend2HNX(FIXMessageBase fMsgBase);

        //Trả Api khi HNX gửi phản hồi
        public void HNXSendQuoteStatusReport(MessageQuoteSatusReport Message);

        // 35=8; 150=3
        public void HNXResponseExcQuote(MessageER_ExecOrder Message);

        // 35=8; 150 = 4
        public void HNXResponse_EROrderCancel(MessageER_OrderCancel Message);

        // 35=8; 150 = 5
        public void HNXResponse_ExecReplace(MessageER_OrderReplace Message);

        // 35=8; 150 = 0
        public void HNXResponse_ExecOrder(MessageER_Order Message);

        // 35=8; 150 = 8
        public void HNXResponse_ExecOrderReject(MessageER_OrderReject Message);
        
        // 35= s
        public void HNXSendOrderCross(MessageNewOrderCross Message);

        // 35= t
        public void HNXResponse_CrossOrderCancelReplace(CrossOrderCancelReplaceRequest Message);

        // 35=u
        public void HNXResponse_CrossOrderCancelRequest(CrossOrderCancelRequest Message);

        public void HNXSendReject(MessageReject Message);

        public void ResponseHNXSendTradingSessionStatus(MessageTradingSessionStatus message);

        public void ResponseHNXSendSecurityStatus(MessageSecurityStatus message);
        public void ResponseHNXTopicTradingInfomation(MessageTopicTradingInfomation message);

        //Gate gửi lệnh fail hoặc từ chối gửi lệnh
        public void ReportGateReject(FIXMessageBase fMsg, string text, string p_Code);

        public int NumOfMsg();

        public void HNXResponse_InquiryReposReponse(MessageReposInquiryReport message);

        public void HNXResponse_ReposFirmReport(MessageReposFirmReport message); 

        public void HNXResponse_ExecOrderRepos(MessageExecOrderRepos message);

        public void HNXResponse_ReposBCGDReport(MessageReposBCGDReport message);
        public void HNXResponse_UserResponse(MessageUserResponse message);

        public int SequenceGateAccept { get; }

        public int SequenceGateSendHNX { get; }

        public int SequenceGateFwdFromHNX { get; }
    }
}