using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaInterface
{
    public interface IKafkaClient
    {
        public long Send2KafkaObject(string p_TopicName, object p_Object, long p_StartTimeProcess, int p_MessageSequence, char p_EventFlag);

        public int NumOfMsg();
        public (long, long) ItemsInQueue();
        public int SequenceGateAccept { get; }
        public int SequenceGateSendHNX { get; }
        public int SequenceGateFwdFromHNX { get; }
        public void RecoverSeq();
    }
}
