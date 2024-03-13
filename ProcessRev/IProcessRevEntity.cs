using HNX.FIXMessage;

namespace BusinessProcessAPIReq
{
    internal interface IProcessRevEntity
    {
        public long EnqueueData(FIXMessageBase p_MsgData);

        public (long, long) ItemsInQueue();

        public void StopProducer();

        public void RecoverData();
    }
}