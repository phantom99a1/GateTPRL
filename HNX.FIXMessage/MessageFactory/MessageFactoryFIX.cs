/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: MessageFactoryFIX will define method to manipulate with message ex: Build msg to string, parse string to msg ...
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */

using System.Text;

namespace HNX.FIXMessage
{
    /// <summary>
    /// Summary description for MessageFactoryFIX.
    /// </summary>
    public class MessageFactoryFIX
    {
        #region Member
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private static string BODY_TAG = Common.SOH + "9=";
        private static string sMsgTypePrefix = Common.SOH + "35=";
        private static string sMsgSeqNumPrefix = Common.SOH + "34=";
        private static string sExecTypePrefix = Common.SOH + "150=";
        private List<Field> _fields = new List<Field>();

        // Contructor
        public MessageFactoryFIX() { }

        #endregion

        #region  MessageFactory Members

        /// <summary>
        /// Creates an instance of a specified message type.
        /// extType tao
        /// </summary> 
        public virtual FIXMessageBase CreateInstance(string msgType, string extType)
        {
            FIXMessageBase message;
            switch (msgType)
            {
                case MessageType.Logon:
                    message = new MessageLogon();
                    break;
                case MessageType.Logout:
                    message = new MessageLogout();
                    break;
                case MessageType.Heartbeat:
                    message = new MessageHeartbeat();
                    break;
                case MessageType.TestRequest:
                    message = new MessageTestRequest();
                    break;
                case MessageType.ResendRequest:
                    message = new MessageResendRequest();
                    break;
                case MessageType.SequenceReset:
                    message = new MessageSequenceReset();
                    break;
                case MessageType.Reject:
                    message = new MessageReject();
                    break;

                case MessageType.TradingSessionStatusRequest:
                    message = new MessageTradingSessionStatusRequest();
                    break;

                case MessageType.TradingSessionStatus:
                    message = new MessageTradingSessionStatus();
                    break;

                case MessageType.SecurityStatusRequest:
                    message = new MessageSecurityStatusRequest();
                    break;

                case MessageType.SecurityStatus:
                    message = new MessageSecurityStatus();
                    break;

                case MessageType.TopicTradingInfomation:
                    message = new MessageTopicTradingInfomation();
                    break;

                case MessageType.Quote:
                    message = new MessageQuote();
                    break;
                case MessageType.QuoteCancel:
                    message = new MessageQuoteCancel();
                    break;
                case MessageType.QuoteRequest:
                    message = new MessageQuoteRequest();
                    break;
                case MessageType.QuoteResponse:
                    message = new MessageQuoteResponse();
                    break;
                case MessageType.QuoteStatusReport:
                    message = new MessageQuoteSatusReport();
                    break;

                case MessageType.NewOrderCross:
                    message = new MessageNewOrderCross();
                    break;
                case MessageType.CrossOrderCancelRequest:
                    message = new CrossOrderCancelRequest();
                    break;
                case MessageType.CrossOrderCancelReplaceRequest:
                    message = new CrossOrderCancelReplaceRequest();
                    break;
                
                    // Repos
                case MessageType.ReposInquiry: // N01
                    message = new MessageReposInquiry();
                    break;

                case MessageType.ReposInquiryReport: // N02
                    message = new MessageReposInquiryReport();
                    break;

                case MessageType.ReposFirm: //N03
                    message = new MessageReposFirm();
                    break;

                case MessageType.ReposFirmReport: //N04
                    message = new MessageReposFirmReport();
                    break;

                case MessageType.ReposFirmAccept: // N05
                    message = new MessageReposFirmAccept();
                    break;
                
                case MessageType.ExecOrderRepos: // EE
                    message = new MessageExecOrderRepos();
                    break;

                case MessageType.ReposBCGDReport: // MR
                    message = new MessageReposBCGDReport();
                    break;

                case MessageType.ReposBCGD: // MA
                    message = new MessageReposBCGD();
                    break;

                case MessageType.ReposBCGDModify: // ME
                    message = new MessageReposBCGDModify();
                    break;

                case MessageType.NewOrder: // 35=D
                    message = new MessageNewOrder();
                    break;

                case MessageType.ReplaceOrder: // 35=G
                    message = new MessageReplaceOrder();
                    break;

                case MessageType.CancelOrder: // 35=F
                    message = new MessageCancelOrder();
                    break;

                case MessageType.UserRequest: // 35=BE
                    message = new MessageUserRequest();
                    break;

                case MessageType.UserResponse: // 35=BF
                    message = new MessageUserResponse();
                    break;



                case MessageType.ExecutionReport:
                    switch (extType[0])
                    {
                        case ExecutionReportType.ER_Order_0:
                            message = new MessageER_Order();
                            break;
                        case ExecutionReportType.ER_ExecOrder_3:
                            message = new MessageER_ExecOrder();
                            break;
                        case ExecutionReportType.ER_CancelOrder_4:
                            message = new MessageER_OrderCancel();
                            break;
                        case ExecutionReportType.ER_ReplaceOrder_5:
                            message = new MessageER_OrderReplace();
                            break;
                        case ExecutionReportType.ER_Rejected_8:
                            message = new MessageER_OrderReject();
                            break;
                        default:
                            message = new MessageExecutionReport();
                            break;
                    }
                    break;
                default:
                    message = new FIXMessageBase(msgType);
                    break;
            }
            foreach (Field field in _fields)
                message.Fields.Add(field);

            return message;
        }

        /// <summary>
        /// Abstract method to creat a MessageHelper instance used in the build/parse process.
        /// </summary>
        protected virtual MessageHelper CreateMessageHelper(FIXMessageBase message)
        {
            string msgType = message.MsgType;

            switch (msgType)
            {
                case MessageType.Logon:
                    return new MessageHelperLogon(message);
                case MessageType.Logout:
                    return new MessageHelperLogout(message);
                case MessageType.Heartbeat:
                    return new MessageHelperHeartbeat(message);
                case MessageType.TestRequest:
                    return new MessageHelperTestRequest(message);
                case MessageType.ResendRequest:
                    return new MessageHelperResendRequest(message);
                case MessageType.SequenceReset:
                    return new MessageHelperSequenceReset(message);
                case MessageType.Reject:
                    return new MessageHelperReject(message);

                case MessageType.TradingSessionStatusRequest:
                    return new MessageHelperTradingSessionStatusRequest(message);
                case MessageType.TradingSessionStatus:
                    return new MessageHelperTradingSessionStatus(message);
                case MessageType.SecurityStatusRequest:
                    return new MessageHelperSecurityStatusRequest(message);
                case MessageType.SecurityStatus:
                    return new MessageHelperSecurityStatus(message);
                case MessageType.TopicTradingInfomation:
                    return new MessageHelperTopicTradingInfomation(message);


                case MessageType.Quote:
                    return new MessageHelperQuote(message);
                case MessageType.QuoteCancel:
                    return new MessageHelperQuoteCancel(message);
                case MessageType.QuoteRequest:
                    return new MessageHelperQuoteRequest(message);
                case MessageType.QuoteResponse:
                    return new MessageHelperQuoteResponse(message);
                case MessageType.QuoteStatusReport:
                    return new MessageHelperQuoteStatusReport(message);



                case MessageType.NewOrderCross:
                    return new MessageHelperNewOrderCross(message);
                case MessageType.CrossOrderCancelRequest:
                    return new MessageHelperCrossOrderCancelRequest(message);
                case MessageType.CrossOrderCancelReplaceRequest:
                    return new MessageHelperCrossOrderCancelReplaceRequest(message);


                case MessageType.ReposInquiry: // N01
                    return new MessageHelperReposInquiry(message);
                case MessageType.ReposInquiryReport: // N02
                    return new MessageHelperReposInquiryReport(message); 
                case MessageType.ReposFirm: // N03
                    return new MessageHelperReposFirm(message);
                case MessageType.ReposFirmAccept: // N05
                    return new MessageHelperReposFirmAccept(message);
                case MessageType.ReposBCGD: // MA
                    return new MessageHelperReposBCGD(message);
                case MessageType.ReposBCGDModify: // ME
                    return new MessageHelperReposBCGDModify(message);
                case MessageType.ReposBCGDCancel: // MC
                    return new MessageHelperReposBCGDCancel(message);

                case MessageType.NewOrder: // 35=D
                    return new MessageHelperNewOrder(message);
                case MessageType.ReplaceOrder: // 35=G
                    return new MessageHelperReplaceOrder(message);
                case MessageType.CancelOrder: // 35=F
                    return new MessageHelperCancelOrder(message);

                case MessageType.UserRequest: // 35=BE
                    return new MessageHelperUserRequest(message);
                case MessageType.UserResponse: // 35=BF
                    return new MessageHelperUserResponse(message);

                case MessageType.ExecutionReport:
                    switch (message.ExecType)
                    {
                        case ExecutionReportType.ER_Order_0:
                            return new MessageHelperER_Order(message);
                        case ExecutionReportType.ER_ExecOrder_3:
                            return new MessageHelperER_ExecOrder(message);
                        case ExecutionReportType.ER_CancelOrder_4:
                            return new MessageHelperER_OrderCancel(message);
                        case ExecutionReportType.ER_ReplaceOrder_5:
                            if (message is MessageER_OrderCancel)
                            {
                                return new MessageHelperER_OrderCancel(message);
                            }
                            else
                                return new MessageHelperER_OrderReplace(message);
                        case ExecutionReportType.ER_Rejected_8:
                            return new MessageHelperER_OrderReject(message);
                        default:
                            return new MessageHelperExectutionReport(message);
                    }
                default:
                    return new MessageHelper(message);
            }
        }

        protected (FIXMessageBase, MessageHelper) GetInstance(string msgType, string extType = "")
        {
            FIXMessageBase message;
            MessageHelper helper;
            switch (msgType)
            {
                case MessageType.Logon:
                    message = new MessageLogon();
                    helper = new MessageHelperLogon(message);
                    break;
                case MessageType.Logout:
                    message = new MessageLogout();
                    helper = new MessageHelperLogout(message);
                    break;
                case MessageType.Heartbeat:
                    message = new MessageHeartbeat();
                    helper = new MessageHelperHeartbeat(message);
                    break;
                case MessageType.TestRequest:
                    message = new MessageTestRequest();
                    helper = new MessageHelperTestRequest(message);
                    break;
                case MessageType.ResendRequest:
                    message = new MessageResendRequest();
                    helper = new MessageHelperResendRequest(message);
                    break;
                case MessageType.SequenceReset:
                    message = new MessageSequenceReset();
                    helper = new MessageHelperSequenceReset(message);
                    break;
                case MessageType.Reject:
                    message = new MessageReject();
                    helper = new MessageHelperReject(message);
                    break;

                case MessageType.TradingSessionStatusRequest:
                    message = new MessageTradingSessionStatusRequest();
                    helper = new MessageHelperTradingSessionStatusRequest(message);
                    break;

                case MessageType.TradingSessionStatus:
                    message = new MessageTradingSessionStatus();
                    helper = new MessageHelperTradingSessionStatus(message);
                    break;

                case MessageType.SecurityStatusRequest:
                    message = new MessageSecurityStatusRequest();
                    helper = new MessageHelperSecurityStatusRequest(message);
                    break;

                case MessageType.SecurityStatus:
                    message = new MessageSecurityStatus();
                    helper = new MessageHelperSecurityStatus(message);
                    break;
                
                case MessageType.TopicTradingInfomation:
                    message = new MessageTopicTradingInfomation();
                    helper = new MessageHelperTopicTradingInfomation(message);
                    break;

                case MessageType.Quote:
                    message = new MessageQuote();
                    helper = new MessageHelperQuote(message);
                    break;
                case MessageType.QuoteCancel:
                    message = new MessageQuoteCancel();
                    helper = new MessageHelperQuoteCancel(message);
                    break;
                case MessageType.QuoteRequest:
                    message = new MessageQuoteRequest();
                    helper = new MessageHelperQuoteRequest(message);
                    break;
                case MessageType.QuoteResponse:
                    message = new MessageQuoteResponse();
                    helper = new MessageHelperQuoteResponse(message);
                    break;
                case MessageType.QuoteStatusReport:
                    message = new MessageQuoteSatusReport();
                    helper = new MessageHelperQuoteStatusReport(message);
                    break;

                case MessageType.NewOrderCross:
                    message = new MessageNewOrderCross();
                    helper = new MessageHelperNewOrderCross(message);
                    break;
                case MessageType.CrossOrderCancelRequest:
                    message = new CrossOrderCancelRequest();
                    helper = new MessageHelperCrossOrderCancelRequest(message); 
                    break;
                case MessageType.CrossOrderCancelReplaceRequest:
                    message = new CrossOrderCancelReplaceRequest();
                    helper  = new MessageHelperCrossOrderCancelReplaceRequest(message);
                    break;

                case MessageType.ReposInquiry: // N01
                    message = new MessageReposInquiry();
                    helper = new MessageHelperReposInquiry(message);
                    break;

                case MessageType.ReposInquiryReport: //N02
                    message = new MessageReposInquiryReport();
                    helper = new MessageHelperReposInquiryReport(message);
                    break;

                case MessageType.ReposFirm: // N03
                    message = new MessageReposFirm();
                    helper = new MessageHelperReposFirm(message);
                    MessageHelperReposFirm.islast = false;
                    break;


                case MessageType.ReposFirmReport: // N04
                    message = new MessageReposFirmReport();
                    helper = new MessageHelperReposFirmReport(message);
                    MessageHelperReposFirmReport.islast = false;
                    break;

                case MessageType.ReposFirmAccept: // N05
                    message = new MessageReposFirmAccept();
                    helper = new MessageHelperReposFirmAccept(message);
					MessageHelperReposFirmAccept.islast = false;
					break; 
                
                case MessageType.ExecOrderRepos: // EE
                    message = new MessageExecOrderRepos();
                    helper = new MessageHelperExecOrderRepos(message);
                    MessageHelperExecOrderRepos.islast = false;
                    break;

                case MessageType.ReposBCGDReport: // MR
                    message = new MessageReposBCGDReport();
                    helper = new MessageHelperReposBCGDReport(message);
                    MessageHelperReposBCGDReport.islast = false;
                    break;

                case MessageType.ReposBCGD: // MA
                    message = new MessageReposBCGD();
                    helper = new MessageHelperReposBCGD(message);
                    MessageHelperReposBCGD.islast = false;    
                    break;

                case MessageType.ReposBCGDModify: // ME
                    message = new MessageReposBCGDModify();
                    helper = new MessageHelperReposBCGDModify(message);
                    MessageHelperReposBCGDModify.islast = false;
                    break;

                case MessageType.ReposBCGDCancel: // MC
                    message = new MessageReposBCGDCancel();
                    helper = new MessageHelperReposBCGDCancel(message);
                    break;


                case MessageType.NewOrder: // 35=D
                    message = new MessageNewOrder();
                    helper = new MessageHelperNewOrder(message);
                    break;

                case MessageType.ReplaceOrder: // 35=G
                    message = new MessageReplaceOrder();
                    helper = new MessageHelperReplaceOrder(message);
                    break;

                case MessageType.CancelOrder: // 35=F
                    message = new MessageCancelOrder();
                    helper = new MessageHelperCancelOrder(message);
                    break;

                case MessageType.UserRequest: // 35=BE
                    message = new MessageUserRequest();
                    helper = new MessageHelperUserRequest(message);
                    break;
                case MessageType.UserResponse: // 35=BF
                    message = new MessageUserResponse();
                    helper = new MessageHelperUserResponse(message);
                    break;

                case MessageType.ExecutionReport:
                    switch (extType[0])
                    {
                        case ExecutionReportType.ER_Order_0:
                            message = new MessageER_Order();
                            helper = new MessageHelperER_Order(message);
                            break;
                        case ExecutionReportType.ER_ExecOrder_3:
                            message = new MessageER_ExecOrder();
                            helper = new MessageHelperER_ExecOrder(message);
                            break;
                        case ExecutionReportType.ER_CancelOrder_4:
                            message = new MessageER_OrderCancel();
                            helper = new MessageHelperER_OrderCancel(message);
                            break;
                        case ExecutionReportType.ER_ReplaceOrder_5:
                            message = new MessageER_OrderReplace();
                            helper = new MessageHelperER_OrderReplace(message);
                            break;
                        case ExecutionReportType.ER_Rejected_8:
                            message = new MessageER_OrderReject();
                            helper = new MessageHelperER_OrderReject(message);
                            break;
                        default:
                            message = new MessageExecutionReport();
                            helper = new MessageHelperExectutionReport(message);
                            break;
                    }
                    break;
                default:
                    message = new FIXMessageBase(msgType);
                    helper = new MessageHelper(message);
                    break;
            }      

            foreach (Field field in _fields)
                message.Fields.Add(field);

            return (message, helper);
        }

        /// <summary>
        /// Checks if the message supplied is a administrative message.  
        /// Administrative messages don't get re-trasmitted when a ResendRequest is received.
        /// </summary>
        public virtual bool IsAdminitrativeMessage(FIXMessageBase message)
        {
            string msgType = message.MsgType;
            switch (msgType)
            {
                case MessageType.Logon:
                    return true;
                case MessageType.Logout:
                    return true;
                case MessageType.Heartbeat:
                    return true;
                case MessageType.TestRequest:
                    return true;
                case MessageType.ResendRequest:
                    return true;
                case MessageType.SequenceReset:
                    return true;
                case MessageType.Reject:
                    return IsAdminitrativeMessage(((MessageReject)message).RefMsgType);
                default:
                    return false;
            }
        }

        public virtual bool IsAdminitrativeMessage(string msgType)
        {
            switch (msgType)
            {
                case MessageType.Logon:
                    return true;
                case MessageType.Logout:
                    return true;
                case MessageType.Heartbeat:
                    return true;
                case MessageType.TestRequest:
                    return true;
                case MessageType.ResendRequest:
                    return true;
                case MessageType.SequenceReset:
                    return true;
                default:
                    return false;
            }
        }

        protected virtual void BuildHeader(MessageHelper helper, StringBuilder sb)
        {
            helper.BuildHeader(sb);
        }

        /// <summary>
        /// Converts to a string representation of a FIX message for possible tranmission.
        /// </summary>
        public string Build(FIXMessageBase message)
        {
            StringBuilder sb = new StringBuilder();

            //Create MessageHelper
            MessageHelper helper = CreateMessageHelper(message);

            //Build Header
            BuildHeader(helper, sb);

            //Build Body
            helper.BuildBody(sb);

            //Other Tags
            if (message.Fields.Count > 0)
            {
                foreach (Field field in message.Fields)
                {
                    sb.Append(field.Tag);
                    sb.Append(Common.DELIMIT);
                    sb.Append(field.Value);
                    sb.Append(Common.SOH);
                }
            }

            //Header
            //This information must be INSERTED in the begining of the message.  Since we are inserting it must be done in REVERSE order.
            message.BodyLength = sb.Length;
            helper.InsertHeader(sb);

            //Trailer
            message.CheckSum = CalcCheckSum(sb);
            helper.AppendTrailer(sb);

            message.MessageRaw = sb.ToString().Replace((char)0, ' ');
            message.MessageRawByte = Encoding.ASCII.GetBytes(message.MessageRaw);
            return message.MessageRaw;
        }

        /// <summary>
        /// parse the message string to message object
        /// </summary>
        /// <param name="sMessage"></param>
        /// <returns></returns>
        /*public FIXMessageBase Parse(string sMessage)
        {
            try
            {
                FIXMessageBase message;
                List<Field> fields = new List<Field>();
                string sMsgType = null;
                string sExecType = "";
                Field field;

                //sMessage = sMessage.Replace( (char)0  , ' ');

                FieldReaderFIX reader = new FieldReaderFIX(sMessage);
                while ((field = reader.GetNextField()) != null)
                {
                    if (field.Tag == 0) //Sai chuan Fix
                    {
                        return null;
                    }
                    else
                    {
                        fields.Add(field);

                        if (field.Tag == FIXMessageBase.TAG_MsgType && field.Value != null && field.Value.Length > 0)
                            sMsgType = field.Value;
                        if (field.Tag == FIXMessageBase.TAG_ExecType && field.Value != null && field.Value.Length > 0)
                            sExecType = field.Value;
                    }
                }

                if (sMsgType == null || sMsgType.Length == 0)
                {
                    if (log.IsWarnEnabled)
                        log.Warn("Parse - Failed to parse MsgType from message / Message={0}", sMessage);
                    throw new Exception("Unable to parse MsgType");
                }
                message = CreateInstance(sMsgType, sExecType);

                MessageHelper helper = CreateMessageHelper(message);


                for (int i = 0; i < fields.Count; i++)
                {
                    field = fields[i];

                    if (helper.ParseField(field) == false)
                        return null;
                }


                message.MessageRaw = sMessage;
                return message;
            }
            catch (Exception ex)
            {
                log.Info("Msg: {0} | EX:{1}", sMessage, ex.ToString());
                return null;
            }
        }*/


        public FIXMessageBase Parse(string sMessage)
        {
            try
            {

                FIXMessageBase message;
                MessageHelper helper;               
                string sMsgType;
                string sExecType;
                (sMsgType, sExecType) = Parse_MsgType_ExecType(sMessage);
                if (sMsgType != string.Empty)
                {
                    List<Field> fields = new List<Field>();

                    (message, helper) = GetInstance(sMsgType, sExecType);
                    FieldReaderFIX reader = new FieldReaderFIX(sMessage);
                    Field field;

                    while ((field = reader.GetNextField()) != null)
                    {
                        if (field.Tag == 0) //Sai chuan Fix
                        {
                            return null;
                        }
                        else
                        {
                            fields.Add(field);
                            if (helper.ParseField(field) == false)
                            {
                                if (log.IsWarnEnabled)
                                    log.Warn("Parse - Failed to parse Field {0}  with tag {1} from message Message={2}",field.Tag, field.Value, sMessage);
                                return null;
                            }    
                               
                        }
                    }
                    switch (sMsgType)
                    {
                        case MessageType.ExecOrderRepos: // 35=EE
                            MessageHelperExecOrderRepos.islast = true;
                            helper.AddReposSide();
                            break;
                        case MessageType.ReposBCGD: // 35=MA
                            MessageHelperReposBCGD.islast = true;
                            helper.AddReposSide();
                            break;
                        case MessageType.ReposBCGDModify:// 35=ME
                            MessageHelperReposBCGDModify.islast = true;
                            helper.AddReposSide();
                            break;
                        case MessageType.ReposBCGDReport: // 35=MR
                            MessageHelperReposBCGDReport.islast = true;
                            helper.AddReposSide();
                            break;
                        case MessageType.ReposFirm: // 35=N03
                            MessageHelperReposFirm.islast = true;
                            helper.AddReposSide();
                            break;
                        case MessageType.ReposFirmReport: // 35=N04
                            MessageHelperReposFirmReport.islast = true;
                            helper.AddReposSide();
                            break;
						case MessageType.ReposFirmAccept: // 35=N05
							MessageHelperReposFirmAccept.islast = true;
							helper.AddReposSide();
							break;
					}
                    //                 
                }
                else
                {
                    if (log.IsWarnEnabled)
                        log.Warn("Parse - Failed to parse MsgType from message / Message={0}", sMessage);
                    throw new Exception("Unable to parse MsgType");
                }      
                message.MessageRaw = sMessage;
                //
                
                return message;
            }
            catch (Exception ex)
            {
                log.Info("Msg: {0} | EX:{1}", sMessage, ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Parses the MsgType value from a supplied FIX message.
        /// </summary>
        public (string,string) Parse_MsgType_ExecType(string sMessage)
        {
            
            int iStart = sMessage.IndexOf(sMsgTypePrefix);
            if (iStart == -1)
                return (string.Empty ,string.Empty);
            iStart += sMsgTypePrefix.Length - 1;

            int iEnd = sMessage.IndexOf(Common.SOH, iStart);
            if (iEnd == -1)
                return (string.Empty, string.Empty);

            string sMsgType = sMessage.Substring(iStart, iEnd - iStart);
            if (sMsgType != MessageType.ExecutionReport)
            {
                return (sMsgType, string.Empty);
            }
            else
            {
                iStart = sMessage.IndexOf(sExecTypePrefix);
                if (iStart == -1)
                    return (string.Empty, string.Empty);
                iStart += sExecTypePrefix.Length - 1;

                iEnd = sMessage.IndexOf(Common.SOH, iStart);
                if (iEnd == -1)
                    return (sMsgType, string.Empty);
                else
                    return (sMsgType, sMessage.Substring(iStart, iEnd - iStart));
            }    
           
        }
       
        /// <summary>
        /// Parses the MsgSeqNum value from a supplied FIX message.
        /// </summary>
        public int ParseMsgSeqNum(string sMessage)
        {
            int iMsgSeqNum = -1;

            int iStart = sMessage.IndexOf(sMsgSeqNumPrefix);
            if (iStart >= 0)
            {
                iStart += sMsgSeqNumPrefix.Length - 1;
                int iEnd = sMessage.IndexOf(Common.SOH, iStart);
                if (iEnd > iStart)
                {
                    try
                    {
                        bool _IsSuccess = false;
                        iMsgSeqNum = Utils.Convert.ParseInt(sMessage, iStart, iEnd - iStart, ref _IsSuccess);
                    }
                    catch { }
                }
            }

            return iMsgSeqNum;
        }

        #endregion

        #region Private support method

        /// <summary>
        /// Calculates the FIX checksum of a message in a StringBuilder.
        /// </summary>
        private byte CalcCheckSum(StringBuilder sb)
        {
            int iCheckSum = 0;
            int iLen = sb.Length;
            for (int i = 0; i < iLen; i++)
                iCheckSum += sb[i];

            return (byte)(iCheckSum - (int)(iCheckSum / 256) * 256);
        }

        /*/// <summary>
        /// Calculates the FIX checksum of a message in a String.
        /// </summary>
        private byte CalcCheckSum(string s, int iIndex, int iLen)
        {
            int iCheckSum = 0;
            int iEnd = iIndex + iLen;
            for (int i = iIndex; i < iEnd; i++)
                iCheckSum += s[i];

            return (byte)(iCheckSum - (int)(iCheckSum / 256) * 256);
        }*/

        #endregion
    }
}
