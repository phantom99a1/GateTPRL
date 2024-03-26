/*
 * Project:
 * Author :
 * Summary: Lớp xử lý gửi dữ liệu sang Sở HNX và Gửi thông tin sang Kafka
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 *
 */

using BusinessProcessResponse;
using CommonLib;
using Confluent.Kafka;
using Disruptor;
using Disruptor.Dsl;
using HNX.FIXMessage;
using HNXInterface;
using LocalMemory;
using StorageProcess;

namespace BusinessProcessAPIReq
{
    internal class ProcessRevEntity : IProcessRevEntity
    {
        private readonly iHNXClient c_iHNXEntity;

        // Size of the ring buffer, must be power of 2.
        private int bufferSize = ConfigData.QueueSize;

        // Create the disruptor
        private Disruptor<RevDataEvent> c_disruptor;

        private RingBuffer<RevDataEvent> c_ringBuffer;
        private readonly IResponseInterface c_iProcessResponse;

        private MessageFactoryFIX c_MsgFactory;

        public void StopProducer()
        {
            this.c_disruptor.Shutdown();
        }

        public ProcessRevEntity(HNXInterface.iHNXClient p_iHNXClient, IResponseInterface p_ProcessResponse)

        {
            CommonLib.Logger.log.Info("Create Instance ProcessRevEntity input p_iHNXEntity:{0}", p_iHNXClient.GetHashCode());
            c_MsgFactory = new MessageFactoryFIX();
            c_iHNXEntity = p_iHNXClient;
            c_iProcessResponse = p_ProcessResponse;
            c_disruptor = new Disruptor<RevDataEvent>(() => new RevDataEvent(), bufferSize, TaskScheduler.Default, ProducerType.Multi, ConfigData.StrategyMode);
            c_disruptor.HandleEventsWith(new ProcessRevDisruptor(p_iHNXClient, c_iProcessResponse));

            // Start the disruptor (start the consumer threads)
            c_disruptor.Start();

            c_ringBuffer = c_disruptor.RingBuffer;
            CommonLib.Logger.log.Info("Created  Instance c_iHNXEntity:{0}", c_iHNXEntity.GetHashCode());
        }

        public long EnqueueData(FIXMessageBase p_MsgData)
        {
            //Đã Enqueue thì chắc chắn là accept Message rồi

            // (1) Claim the next sequence
            long sequence;
            if (!c_ringBuffer.TryNext(out sequence))
            {
                return -1;
            }
            int MsgSeq = (int)sequence;
            p_MsgData.MsgSeqNum = MsgSeq;
            //

            ShareMemoryData.c_FileStore.StoreRecoverMsg_GateSendHNX(p_MsgData.GetMsgType, c_MsgFactory.Build(p_MsgData), p_MsgData.MsgSeqNum);
            c_iProcessResponse.ResponseApi2Kafka(p_MsgData, MsgSeq);
            Logger.log.Info("Send Data Message {0} with api ID {1} to Kafka", p_MsgData.GetMsgType, p_MsgData.IDRequest);
            try
            {
                // (2) Get and configure the event for the sequence
                Logger.log.Info("Enqueue Data Message {0} ID {1}", p_MsgData.GetMsgType, p_MsgData.IDRequest);
                c_ringBuffer[sequence].FixMsgValue = p_MsgData;
            }
            finally
            {
                // (3) Publish the event
                c_ringBuffer.Publish(sequence);
                Logger.log.Info("Enqueue Data Message {0} ID {1} Complete", c_ringBuffer[sequence].FixMsgValue.GetMsgType, c_ringBuffer[sequence].FixMsgValue.IDRequest);
            }

            return sequence;
        }

        public void RecoverData()
        {
            if (TradingRuleData.GetTradingSessionNameofMainBoard() != "N/A")
            {
                Logger.log.Info("Recover Data from file RECOVER_STORE, Current Session {0}", TradingRuleData.GetTradingSessionNameofMainBoard());
                List<String> ListMSGRecover = ShareMemoryData.c_FileStore.ReadAllRECOVERFile();
                int LastIndexSendedHNX;

                #region Tìm lại và gửi lại message Gate chấp nhận nhưng chưa được gửi HNX

                LastIndexSendedHNX = ListMSGRecover.Count;
                for (int i = ListMSGRecover.Count - 1; i >= 0; i--)
                {
                    if (ListMSGRecover[i][0] == MessageFlag.RECOVER_FWD_FLAG)
                    {
                        int LastSeqProcessed = (int)Utils.ParseLongSpan(ListMSGRecover[i].AsSpan().Slice(2, 10));
                        if (LastSeqProcessed > c_iHNXEntity.LastMapSeqProcess)
                        {
                            LastIndexSendedHNX = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (LastIndexSendedHNX < ListMSGRecover.Count)
                {
                    for (int i = LastIndexSendedHNX; i < ListMSGRecover.Count; i++)
                    {
                        Logger.log.Info("Xu ly lai sau Fail over message {0} Content {1}", i, ListMSGRecover[i]);
                        long Sequence;
                        c_ringBuffer.TryNext(out Sequence);
                        try
                        {
                            string s = ListMSGRecover[i].Substring(13);
                            c_ringBuffer[Sequence].FixMsgValue = c_MsgFactory.Parse(s);
                        }
                        finally
                        {
                            c_ringBuffer.Publish(Sequence);
                        }
                    }
                }

                #endregion Tìm lại và gửi lại message Gate chấp nhận nhưng chưa được gửi HNX
            }
            else
            {
                //Không thấy thông tin phiên hiện tại. coi như là đầu ngày.
                Logger.log.Info("Khong tim thay thong tin phien hien tai. Khong thuc hien Recover");
            }

            Logger.log.Info("Recover Complete");
        }

        public (long, long) ItemsInQueue()
        {
            return (bufferSize - c_ringBuffer.GetRemainingCapacity(), bufferSize);
        }
    }

    internal class ProcessRevDisruptor : IEventHandler<RevDataEvent>
    {
        private readonly HNXInterface.iHNXClient c_iHNXClient;
        private readonly IResponseInterface c_ResponseInterface;
        private string outCode = string.Empty;
        private string outText = string.Empty;

        public ProcessRevDisruptor(HNXInterface.iHNXClient p_iHNXEntity, IResponseInterface p_ResponseInterface)
        {
            c_iHNXClient = p_iHNXEntity;
            c_ResponseInterface = p_ResponseInterface;
        }

        public void OnEvent(RevDataEvent p_data, long sequence, bool endOfBatch)
        {
            //  c_iHNXEntity gửi sang HNX
            Send2HNX(p_data.FixMsgValue);
        }

        public void Send2HNX(FIXMessageBase fMsgBase)
        {
            try
            {
                if (TradingRuleData.GetTradeSesStatusofMainBoard() != CommonData.TradingSessionStatus.DefaultStatus)
                {
                    string ClOrdID = string.Empty;
                    string Symbol = string.Empty;
                    //
                    bool isListSymbol = false;
                    ReposSideList reposSideList = null;
                    //Lấy lại thông tin
                    switch (fMsgBase.GetMsgType)
                    {
                        case MessageType.Quote: // 35=S
                            MessageQuote msgQuote = (MessageQuote)fMsgBase;
                            ClOrdID = msgQuote.ClOrdID;
                            Symbol = msgQuote.Symbol;
                            break;

                        case MessageType.NewOrderCross: // 35=s
                            MessageNewOrderCross NewOrderCross = (MessageNewOrderCross)fMsgBase;
                            ClOrdID = NewOrderCross.ClOrdID;
                            Symbol = NewOrderCross.Symbol;
                            break;

                        case MessageType.CrossOrderCancelReplaceRequest: // 35=t
                            CrossOrderCancelReplaceRequest OrderReplace = (CrossOrderCancelReplaceRequest)fMsgBase;
                            ClOrdID = OrderReplace.ClOrdID;
                            Symbol = OrderReplace.Symbol;
                            break;

                        case MessageType.CrossOrderCancelRequest: // 35=u
                            CrossOrderCancelRequest OrderCancel = (CrossOrderCancelRequest)fMsgBase;
                            ClOrdID = OrderCancel.ClOrdID;
                            Symbol = OrderCancel.Symbol;
                            break;

                        case MessageType.QuoteRequest: // 35=R
                            MessageQuoteRequest QuoteRequest = (MessageQuoteRequest)fMsgBase;
                            ClOrdID = QuoteRequest.ClOrdID;
                            Symbol = QuoteRequest.Symbol;
                            break;

                        case MessageType.QuoteCancel: // 35=Z
                            MessageQuoteCancel QuoteCancel = (MessageQuoteCancel)fMsgBase;
                            ClOrdID = QuoteCancel.ClOrdID;
                            Symbol = QuoteCancel.Symbol;
                            break;

                        case MessageType.QuoteResponse: // 35=AJ
                            MessageQuoteResponse QuoteResponse = (MessageQuoteResponse)fMsgBase;
                            ClOrdID = QuoteResponse.ClOrdID;
                            Symbol = QuoteResponse.Symbol;
                            break;

                        case MessageType.ReposInquiry: // 35=N01
                            MessageReposInquiry NewInquiryRepos = (MessageReposInquiry)fMsgBase;
                            ClOrdID = NewInquiryRepos.ClOrdID;
                            Symbol = NewInquiryRepos.Symbol;
                            break;

                        case MessageType.ReposFirm: // 35=N03
                            MessageReposFirm NewFirmRepos = (MessageReposFirm)fMsgBase;
                            ClOrdID = NewFirmRepos.ClOrdID;
                            isListSymbol = true;
                            reposSideList = NewFirmRepos.RepoSideList;
                            break;

                        case MessageType.ReposFirmAccept: // 35=N05
                            MessageReposFirmAccept _ReposFirmAccept = (MessageReposFirmAccept)fMsgBase;
                            ClOrdID = _ReposFirmAccept.ClOrdID;
                            reposSideList = _ReposFirmAccept.RepoSideList;
                            break;

                        case MessageType.ReposBCGDModify: // 35=ME
                            MessageReposBCGDModify _ReposBCGDModify = (MessageReposBCGDModify)fMsgBase;
                            ClOrdID = _ReposBCGDModify.ClOrdID;
                            isListSymbol = true;
                            reposSideList = _ReposBCGDModify.RepoSideList;
                            break;

                        case MessageType.ReposBCGD: // 35=MA
                            MessageReposBCGD _ReposBCGD = (MessageReposBCGD)fMsgBase;
                            ClOrdID = _ReposBCGD.ClOrdID;
                            isListSymbol = true;
                            reposSideList = _ReposBCGD.RepoSideList;
                            break;

                        case MessageType.ReposBCGDCancel: // 35=MC
                            MessageReposBCGDCancel _ReposBCGDCancel = (MessageReposBCGDCancel)fMsgBase;
                            ClOrdID = _ReposBCGDCancel.ClOrdID;
                            break;

                        case MessageType.NewOrder: // 35=D
                            MessageNewOrder _NewOrder = (MessageNewOrder)fMsgBase;
                            ClOrdID = _NewOrder.ClOrdID;
                            Symbol = _NewOrder.Symbol;
                            break;

                        case MessageType.ReplaceOrder: // 35=G
                            MessageReplaceOrder _ReplaceOrder = (MessageReplaceOrder)fMsgBase;
                            ClOrdID = _ReplaceOrder.ClOrdID;
                            Symbol = _ReplaceOrder.Symbol;
                            break;

                        case MessageType.CancelOrder: // 35=F
                            MessageCancelOrder _CancelOrder = (MessageCancelOrder)fMsgBase;
                            ClOrdID = _CancelOrder.ClOrdID;
                            Symbol = _CancelOrder.Symbol;
                            break;
                    }
                    bool preCheck = true;
                    if (!isListSymbol)
                    {
                        preCheck = TradingRuleData.CheckTradingRule_Output(Symbol, out outText, out outCode);
                    }
                    else
                    {
                        if (reposSideList != null)
                        {
                            ReposSide itemSite;
                            for (int i = 0; i < reposSideList.Count; i++)
                            {
                                itemSite = reposSideList[i];
                                preCheck = TradingRuleData.CheckTradingRule_Output(itemSite.Symbol, out outText, out outCode);
                                if (!preCheck)
                                    break;
                            }
                        }
                    }

                    if (!preCheck)
                    {
                        c_ResponseInterface.ReportGateReject(fMsgBase, outText, outCode);
                    }

                    bool isSend = RetrySendHNX(fMsgBase, ConfigData.RetryQ.Enable);
                    
                    //Update
                    if (isSend)
                    {
                        if (fMsgBase.ApiSeqNum != -1)
                            OrderMemory.UpdateToSeq(fMsgBase.ApiSeqNum, fMsgBase.MsgSeqNum);
                        else
                            OrderMemory.Update_Order(ClOrdID, fMsgBase.MsgSeqNum);
                        //Phản hồi về Kafka sau khi gửi sang gate
                        c_ResponseInterface.ResponseGateSend2HNX(fMsgBase);
                    }
                    else
                    {
                        c_ResponseInterface.ReportGateReject(fMsgBase, CommonDataInCore.CORE_RejectReason.RejectReason_50010, CommonDataInCore.CORE_RejectReasonCode.Code_50010);
                    }
                }
                else
                {
                    //Gửi Reject cho Kafka lỗi chưa vào phiên giao dịch
                    c_ResponseInterface.ReportGateReject(fMsgBase, CommonDataInCore.CORE_RejectReason.RejectReason_50006, CommonDataInCore.CORE_RejectReasonCode.Code_50006);
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error Send2HNX when process OnEvent, Exception: {ex?.ToString()}");
            }
        }

        private bool RetrySendHNX(FIXMessageBase fMsg, bool IsRetry)
        {
            if (IsRetry)
            {
                int retryTimes = 0;
                bool isSend = false;

                while (!isSend)
                {
                    isSend = c_iHNXClient.Send2HNX(fMsg);
                    retryTimes++;

                    if (isSend)
                    {
                        Logger.log.Warn("Retry times {0} Success", retryTimes);
                        break;
                    }
                    Logger.log.Warn("Retry fail {0} times", retryTimes);
                    if (retryTimes >= ConfigData.RetryQ.MaxTimes)
                    {
                        break;
                    }
                    Thread.Sleep(ConfigData.RetryQ.Interval);
                }
                return isSend;
            }
            else
            {
                return c_iHNXClient.Send2HNX(fMsg);
            }
        }
    }

    public class RevDataEvent
    {
        public FIXMessageBase FixMsgValue;
    }
}