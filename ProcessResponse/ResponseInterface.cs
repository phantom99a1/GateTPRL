/*
 * Project:
 * Author :
 * Summary: Lớp Interface các đầu hàm xử lý của Kafka
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 *
 */

using CommonLib;
using HNX.FIXMessage;
using KafkaInterface;
namespace BusinessProcessResponse
{
    public partial class ResponseInterface : IResponseInterface
    {
        private IKafkaClient c_KafkaClient;
        public ResponseInterface(IKafkaClient p_KafkaClient)
        {
            c_KafkaClient = p_KafkaClient;
        }

        public int NumOfMsg()
        {
            return c_KafkaClient.NumOfMsg();
        }

        public int SequenceGateAccept
        {
            get
            {
                return c_KafkaClient.SequenceGateAccept;
            }
        }

        public int SequenceGateSendHNX
        {
            get
            {
                return c_KafkaClient.SequenceGateSendHNX;
            }
        }

        public int SequenceGateFwdFromHNX
        {
            get
            {
                return c_KafkaClient.SequenceGateFwdFromHNX;
            }
        }

    }
}