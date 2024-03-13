using CommonLib;
using HNX.FIXMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalMemory;
using KafkaInterface;
using Microsoft.AspNetCore.WebUtilities;

namespace HNXInterface
{
    public partial class HNXTCPClient : iHNXClient, IDisposable
    {
        public int LastMapSeqProcess
        {
            get
            {
                return lastMapSeqProcess;
            }
        }

        private int lastMapSeqProcess = 0;

        public void Recovery()
        {
            string messageRaw2 = "";
            try
            {
                string StartOfMessage = "8=FIX.4.4";
                OrderMemory.RecoverAndStartStoring();
                List<string> DataBackUpSend = ShareMemoryData.c_FileStore.ReadAllSENDFile();

                //Lấy lại Last Sequence gửi 
                for (int i = DataBackUpSend.Count - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrEmpty(DataBackUpSend[i]) && DataBackUpSend[i][0] != MessageFlag.RESEND_FLAG)
                    {
                        //Không phải Resend. lấy lại Seq cuối cùng xử lý nhận từ Disruptor
                        ReadOnlySpan<char> MsgSpan = DataBackUpSend[i].AsSpan();
                        lastMapSeqProcess = (int)Utils.ParseLongSpan(MsgSpan.Slice(67, 10));
                        break;
                    }
                }

                List<string> ListMsgRecv = ShareMemoryData.c_FileStore.ReadAllREVFile();

                if (DataBackUpSend.Count > 0 && ListMsgRecv.Count > 0)
                {
                    RecoverFromFile(DataBackUpSend.Last(), ListMsgRecv.Last());
                }

                int startIndex = (0 > DataBackUpSend.Count - 100) ? 0 : DataBackUpSend.Count - 100;
                for (int i = startIndex; i < DataBackUpSend.Count; i++)
                {
                    string messageRaw = DataBackUpSend[i].AsSpan().Slice(DataBackUpSend[i].IndexOf(StartOfMessage)).ToString();
                    FIXMessageBase fMsg = c_MsgFactoryFix.Parse(messageRaw);
                    if (fMsg != null && !c_MsgFactoryFix.IsAdminitrativeMessage(fMsg))
                    {
                        c_BackupData.AddtoBackup(fMsg.MsgSeqNum, fMsg);

                        #region Tìm và gửi lại nhưng message đã gửi sở nhưng chưa báo về kafka
                        if (fMsg.MsgSeqNum > c_ProcessRevHNX.c_ResponseInterface.SequenceGateSendHNX)
                        {
                            c_ProcessRevHNX.c_ResponseInterface.ResponseGateSend2HNX(fMsg);
                        }    
                    #endregion
                    }


                }


                for (int i = 0; i < ListMsgRecv.Count; i++)
                {
                    messageRaw2 = ListMsgRecv[i];
                    if (messageRaw2.Contains(StartOfMessage))
                    {
                        string messageRaw = ListMsgRecv[i].AsSpan().Slice(ListMsgRecv[i].IndexOf(StartOfMessage)).ToString();
                        FIXMessageBase fMsg = c_MsgFactoryFix.Parse(messageRaw);
                        if (fMsg.GetMsgType == MessageType.TradingSessionStatus)
                        {
                            TradingRuleData.ProcessTradingSession((MessageTradingSessionStatus)fMsg);
                        }
                        else if (fMsg.GetMsgType == MessageType.SecurityStatus)
                        {
                            TradingRuleData.ProcessSecurityStatus((MessageSecurityStatus)fMsg);
                        }
                        if (!c_MsgFactoryFix.IsAdminitrativeMessage(fMsg) && fMsg.MsgSeqNum > c_ProcessRevHNX.c_ResponseInterface.SequenceGateFwdFromHNX)
                        {
                            //Tức là chưa gửi cho Kafka
                            Logger.log.Info("Xu ly lai sau Fail over message {0}", messageRaw);
                            c_ProcessRevHNX.ProcessHNXMessage(fMsg);
                        }
                    }
                      
                }
            }
            catch (Exception e)
            {
                Logger.log.Error($"messageRaw2={messageRaw2},exceptions: {e.Message}");
            }
        }

        private void RecoverFromFile(string LineDataSend, string LineDataRecv)
        {
            //Thông tin ghi file đọc theo dòng, 68 kí tự đầu là flag, thời gian, sequence Gate
            if (LineDataSend.Length > 68)
            {
                int gateSeq = Utils.ParseInt(LineDataSend.Substring(20, 10));
                GateSeqInfo.Set_CliSeq(gateSeq);
            }
            if (LineDataRecv.Length > 66)
            {
                int hnxSeq = Utils.ParseInt(LineDataRecv.Substring(18, 10));
                int hnxLastSeqProcess = Utils.ParseInt(LineDataRecv.Substring(29, 10));
                GateSeqInfo.Set_SerSeq(hnxSeq);
                GateSeqInfo.Set_LastCliProcess(hnxSeq);
                GateSeqInfo.Set_LastSerProcess(hnxLastSeqProcess);
            }
        }
    }
}
