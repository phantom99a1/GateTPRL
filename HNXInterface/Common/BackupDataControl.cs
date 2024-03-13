using CommonLib;
using HNX.FIXMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNXInterface
{
    internal class BackupDataControl
    {
        private DataBackup[] c_BakDataforResend;
        private int CAPACITY = ConfigData.SafeWindowSize *5;
        private MessageFactoryFIX c_MsgFactory = new MessageFactoryFIX();
        private int c_Index;
        public BackupDataControl(int Capacity)
        {
            CAPACITY = Capacity;
            c_BakDataforResend = new DataBackup[CAPACITY];
            c_Index = -1;
        }

        public void AddtoBackup(int p_Seq, FIXMessageBase p_fMsg)
        {
            c_Index++;
            if (c_Index >= CAPACITY) c_Index = 0;

            c_BakDataforResend[c_Index].Seq = p_Seq;
            c_BakDataforResend[c_Index].FixMsg = p_fMsg;
        }

        public List<FIXMessageBase> GetMsgfromBackup(int fromSeq, int toSeq)
        {
            FIXMessageBase[] Result = new FIXMessageBase[toSeq - fromSeq +1];
            for (int i = 0; i < CAPACITY; i++)
            {
                if (c_BakDataforResend[i].Seq <= toSeq && c_BakDataforResend[i].Seq >= fromSeq)
                {
                    if (c_BakDataforResend[i].FixMsg != null && c_BakDataforResend[i].FixMsg.GetMsgType != null
                        && c_BakDataforResend[i].FixMsg.GetMsgType != String.Empty)
                    {
                        Result[c_BakDataforResend[i].Seq - fromSeq] = c_BakDataforResend[i].FixMsg;
                        Result[c_BakDataforResend[i].Seq - fromSeq].PossDupFlag = true;
                        c_MsgFactory.Build(Result[c_BakDataforResend[i].Seq - fromSeq]);
                    }    
                    else
                    {
                        Result[c_BakDataforResend[i].Seq - fromSeq] = MakeLostDTMsg(c_BakDataforResend[i].Seq);

                        c_MsgFactory.Build(Result[c_BakDataforResend[i].Seq - fromSeq]);
                    }    
                }
            }
            for (int i = 0; i< Result.Length; i++)
            {
                if (Result[i] == null)
                {
                    Result[i] = MakeLostDTMsg(i + fromSeq);
                    c_MsgFactory.Build(Result[i]);
                }    
                    
            }    
            
            return Result.ToList();
            //
        }


        private FIXMessageBase MakeLostDTMsg(int p_Seq)
        {
            //Thay bằng message xin phiên
            MessageTradingSessionStatusRequest mkLost = new MessageTradingSessionStatusRequest();
            mkLost.TradSesReqID = "0"; //lấy tất các bảng, cho nó đơn giản
            mkLost.SubscriptionRequestType = '2';
            mkLost.TradSesMode = 1;//lấy thông tin phiên theo bảng
            mkLost.PossDupFlag = true;
            mkLost.MsgSeqNum = p_Seq;
            mkLost.SenderCompID = ConfigData.TraderID;
            mkLost.TargetCompID = ConfigData.TargetCompID;
            mkLost.TargetSubID = ConfigData.TargetSubID;
            mkLost.LastMsgSeqNumProcessed = 0;
            return mkLost;//Tạm cứ để thế này đã

        }


        public struct DataBackup
        {
            public int Seq;
            public FIXMessageBase FixMsg;
            public DataBackup(int p_SeqMessage, FIXMessageBase fMsg)
            {
                Seq = p_SeqMessage;
                FixMsg = new FIXMessageBase();
                
            }

        
        }

    }

}
