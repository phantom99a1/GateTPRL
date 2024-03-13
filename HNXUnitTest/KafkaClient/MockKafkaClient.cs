using KafkaInterface;

namespace HNXUnitTest
{
    public class MockKafkaClient : IKafkaClient
    {
        public (long, long) ItemsInQueue()
        {
            return new(0, 0);
        }

        public int NumOfMsg()
        {
            return 1;
        }

        public long Send2KafkaObject(string p_TopicName, object Object, long StartTimeProcess, int MessageSequence, char Flag)
        {
            return SendKafkaStatus.SEND_SUCCESS;
        }

        public int SequenceGateAccept { get { return 0; } }
        public int SequenceGateSendHNX { get { return 0; } }
        public int SequenceGateFwdFromHNX { get { return 0; } }
        public void RecoverSeq() { }

    }
}