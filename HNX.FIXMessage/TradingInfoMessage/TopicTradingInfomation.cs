namespace HNX.FIXMessage
{
    [Serializable]
    public class MessageTopicTradingInfomation : FIXMessageBase
    {
        public string InquiryMember; // Tag 4499
        public string Symbol; // Tag 55

        public MessageTopicTradingInfomation()
            : base()
        {
            MsgType = MessageType.TopicTradingInfomation;
        }
    }
}