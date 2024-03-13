using HNX.FIXMessage;
using LocalMemory;
using BusinessProcessResponse;

namespace HNXInterface
{
    public class ProcessRevHNX
    {
        public IResponseInterface c_ResponseInterface;


        public ProcessRevHNX(IResponseInterface p_ResponseInterface)
        {
            c_ResponseInterface = p_ResponseInterface;
        }
        public void ProcessHNXMessage(FIXMessageBase message)
        {
            switch (message.GetMsgType)
            {
                case MessageType.SecurityStatus: // sở gửi 35=f
                    ProcessSecurityStatus((MessageSecurityStatus)message);
                    break;

                case MessageType.TradingSessionStatus: // sở gửi 35=h
                    ProcessTradingSessionStatus((MessageTradingSessionStatus)message);
                    break;

                case MessageType.TopicTradingInfomation: // sở gửi 35=MN
                    ProcessTopicTradingInfomation((MessageTopicTradingInfomation)message);
                    break;

                case MessageType.QuoteStatusReport: // sở gửi 35=AI
                    ProcessQuoteStatusReport((MessageQuoteSatusReport)message);
                    break;

                case MessageType.ExecutionReport: // sở gửi 35=8
                    ProcessExecOrder((MessageExecutionReport)message);
                    break;

                case MessageType.NewOrderCross: // sở gửi 35=s
                    c_ResponseInterface.HNXSendOrderCross((MessageNewOrderCross)message);
                    break;
                case MessageType.CrossOrderCancelReplaceRequest: // sở gửi 35=t
                    c_ResponseInterface.HNXResponse_CrossOrderCancelReplace((CrossOrderCancelReplaceRequest)message);
                    break;
                case MessageType.CrossOrderCancelRequest: // sở gửi 35=u
                    c_ResponseInterface.HNXResponse_CrossOrderCancelRequest((CrossOrderCancelRequest)message);
                    break;

                case MessageType.Reject: // sở gửi 35=3
                    ProcessRejectQuote((MessageReject)message);
                    break;

                case MessageType.ReposInquiryReport: // sở gửi 35=N02
                    c_ResponseInterface.HNXResponse_InquiryReposReponse((MessageReposInquiryReport)message);
                    break;

                case MessageType.ReposFirmReport: // sở gửi 35=N04
                    c_ResponseInterface.HNXResponse_ReposFirmReport((MessageReposFirmReport)message);
                    break;

                case MessageType.ExecOrderRepos: // sở gửi 35=EE
                    c_ResponseInterface.HNXResponse_ExecOrderRepos((MessageExecOrderRepos)message);
                    break;

                case MessageType.ReposBCGDReport: // sở gửi 35=MR
                    c_ResponseInterface.HNXResponse_ReposBCGDReport((MessageReposBCGDReport)message);
                    break;

                case MessageType.UserResponse: // sở gửi 35=BF
                    c_ResponseInterface.HNXResponse_UserResponse((MessageUserResponse)message);
                    break;
            }
        }


        private void ProcessExecOrder(MessageExecutionReport message)
        {
            switch (message.ExecType)
            {
                case ExecutionReportType.ER_ExecOrder_3:  // 35=8; 150 = 3
                    c_ResponseInterface.HNXResponseExcQuote((MessageER_ExecOrder)message);
                    break;

                case ExecutionReportType.ER_CancelOrder_4:  // 35=8; 150 = 4
                    c_ResponseInterface.HNXResponse_EROrderCancel((MessageER_OrderCancel)message);
                    break;

                case ExecutionReportType.ER_ReplaceOrder_5: // 35=8; 150 = 5
                    c_ResponseInterface.HNXResponse_ExecReplace((MessageER_OrderReplace)message);
                    break;

                case ExecutionReportType.ER_Order_0: // 35=8; 150 = 0
                    c_ResponseInterface.HNXResponse_ExecOrder((MessageER_Order)message);
                    break;

                case ExecutionReportType.ER_Rejected_8: // 35=8; 150 = 8
                    c_ResponseInterface.HNXResponse_ExecOrderReject((MessageER_OrderReject)message);
                    break;
            }
        }

        private void ProcessQuoteStatusReport(MessageQuoteSatusReport Message)
        {
            c_ResponseInterface.HNXSendQuoteStatusReport(Message);
        }
        /// <summary>
        /// Xử lý cho message Reject có refmsgtype = S
        /// </summary>
        private void ProcessRejectQuote(MessageReject Message)
        {
            c_ResponseInterface.HNXSendReject((MessageReject)Message);
        }

        public void ProcessTradingSessionStatus(MessageTradingSessionStatus message)
        {
            TradingRuleData.ProcessTradingSession(message);
            c_ResponseInterface.ResponseHNXSendTradingSessionStatus(message);
        }

        public void ProcessSecurityStatus(MessageSecurityStatus message)
        {
            TradingRuleData.ProcessSecurityStatus(message);
            c_ResponseInterface.ResponseHNXSendSecurityStatus(message);
        }

        public void ProcessTopicTradingInfomation(MessageTopicTradingInfomation message)
        {
            c_ResponseInterface.ResponseHNXTopicTradingInfomation(message);
        }

    }
}
