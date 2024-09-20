//#2024.09.20 Revie code => thêm comment rõ mục đich
//Comment rõ mục đích để làm gì 
//Dùng để hiển thị các dữ liệu về số lượng message Exchange đã gửi, Exchange đã nhận và đang trong hàng đợi chưa gửi tới Exchange
namespace CommonLib
{
    public class GateTPRLMonitorExchange
    {
        public int ExchangeSendMessageNum { get; set; } = 0;

        public int ExchangeRevMessageNum { get; set; } = 0;

        public int ExchangeQueueMessageNum { get; set; } = 0;
    }
}
