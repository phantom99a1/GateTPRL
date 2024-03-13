using CommonLib;
using Disruptor;
using Disruptor.Dsl;

namespace LocalMemory
{
    internal class StoreOrderInfo : IDisposable
    {
        private FileHelper fStoreHelper;
        private Disruptor<OrdeInfoContainer> c_Disruptor;
        private RingBuffer<OrdeInfoContainer> c_RingBuffer;

        public StoreOrderInfo()
        {
            fStoreHelper = new FileHelper("StoreData", ConfigData.LogOrderPath + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMdd"));

            c_Disruptor = new Disruptor<OrdeInfoContainer>(() => new OrdeInfoContainer(),
                                                    2048, TaskScheduler.Default, ProducerType.Multi, ConfigData.StrategyMode);
            c_RingBuffer = c_Disruptor.RingBuffer;
            c_Disruptor.HandleEventsWith(new OrderInfoEvent(fStoreHelper));           
        }

        public long StoreData(OrderInfo p_Data)
        {
            long sequence = c_RingBuffer.Next();
            try
            {
                c_RingBuffer[sequence].Data = p_Data;
            }
            finally
            {
                c_RingBuffer.Publish(sequence);
            }
            return sequence;
        }

        public List<OrderInfo> RecoverData()
        {
            List<string> ListRaw = fStoreHelper.ReadAllData();
            List<OrderInfo> Results = new List<OrderInfo>();
            List<string> ListKeys = new List<string>();
            try
            {
                for (int i = ListRaw.Count - 1; i >= 0; i--)
                {
                    string[] Parameters = ListRaw[i].Split(',');
                    OrderInfo orderInfo = new OrderInfo()
                    {
                        Key = Parameters[0],
                        RefMsgType = Parameters[1],
                        OrderNo = Parameters[2],
                        ClOrdID = Parameters[3],
                        ExchangeID = Parameters[4],
                        RefExchangeID = Parameters[5],
                        SeqNum = Utils.ParseInt(Parameters[6]),
                        Symbol = Parameters[7],
                        Side = Parameters[8],
                        Price = Utils.ParseLongSpan(Parameters[9]),
                        OrderQty = Utils.ParseLongSpan(Parameters[10]),
                        CrossType = Parameters[11],
                        ClientID = Parameters[12],
                        ClientIDCounterFirm = Parameters[13],
                        MemberCounterFirm = Parameters[14],
                        OrderType = Parameters[15],
                        QuoteType = Utils.ParseInt(Parameters[16])
                    };
                    if (!ListKeys.Contains(orderInfo.Key))
                    {
                        Results.Add(orderInfo);
                        ListKeys.Add(orderInfo.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
            return Results;
        }

        public void StartLogging()
        {
            if (!c_Disruptor.HasStarted)
                c_Disruptor.Start();
        }

        public void StopLogging()
        {
            c_Disruptor.Shutdown();
            fStoreHelper.Dispose();
        }

        public void Dispose()
        {
            fStoreHelper.Dispose();
        }
    }

    public class OrdeInfoContainer
    {
        public OrderInfo Data;
    }

    internal class OrderInfoEvent : IEventHandler<OrdeInfoContainer>
    {
        private FileHelper fStoreHelper;

        public OrderInfoEvent(FileHelper p_Fhelper)
        {
            fStoreHelper = p_Fhelper;
        }

        public void OnEvent(OrdeInfoContainer p_data, long sequence, bool endOfBatch)
        {
            OrderInfo orderInfo = p_data.Data;
            //
            fStoreHelper.WriteData($"{orderInfo.Key},{orderInfo.RefMsgType},{orderInfo.OrderNo},{orderInfo.ClOrdID},{orderInfo.ExchangeID},{orderInfo.RefExchangeID},{orderInfo.SeqNum},{orderInfo.Symbol},{orderInfo.Side},{orderInfo.Price},{orderInfo.OrderQty},{orderInfo.CrossType},{orderInfo.ClientID},{orderInfo.ClientIDCounterFirm},{orderInfo.MemberCounterFirm},{orderInfo.OrderType},{orderInfo.QuoteType}");
        }
    }
}