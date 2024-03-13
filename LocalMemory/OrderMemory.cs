using CommonLib;
using HNX.FIXMessage;
using System.Collections.Concurrent;
using static CommonLib.CommonData;

namespace LocalMemory
{
    public class OrderMemory
    {
        /// <summary>
        /// Key là shl đặt vào, value là số hiệu lệnh gửi lên sở
        /// </summary>
        ///
        private static List<OrderInfo> c_ListOrder = new List<OrderInfo>();

        private static ConcurrentDictionary<string, OrderInfo> c_DicMapClOrdID = new ConcurrentDictionary<string, OrderInfo>();
        private static ConcurrentDictionary<int, OrderInfo> c_DictMapSeqNum = new ConcurrentDictionary<int, OrderInfo>();
        private static StoreOrderInfo c_StoreOrderInfo = new StoreOrderInfo();

        #region Updatedata

        public static void Add_NewOrder(OrderInfo p_OrderInfo)
        {
            //add vào hệ thống.
            c_ListOrder.Add(p_OrderInfo);
            c_DicMapClOrdID[p_OrderInfo.ClOrdID] = p_OrderInfo;
            //Chỉ thread nhận message từ sở mới được lưu log ở hàm này.Còn Thread gửi lên sở thì lưu log ở hàm  update Sequence
            c_StoreOrderInfo.StoreData(p_OrderInfo);
        }

        public static bool Update_Order(string p_ClodrID, int p_SeqNum)
        {
            Logger.log.Info($"OrderMemory Update_Order start process update | p_ClodrID: {p_ClodrID}, p_SeqNum:{p_SeqNum}");
            OrderInfo _OrderInfo = GetOrder_byClOrdID(p_ClodrID);
            if (_OrderInfo != null)
            {
                _OrderInfo.SeqNum = p_SeqNum;

                c_DictMapSeqNum.TryAdd(p_SeqNum, c_DicMapClOrdID[p_ClodrID]);

                Logger.log.Info($"OrderMemory update sequence success with p_SeqNum={p_SeqNum}, p_ClodrID: {p_ClodrID}");
                c_StoreOrderInfo.StoreData(_OrderInfo);
                return true;
            }
            Logger.log.Info($"OrderMemory Update_Order Can't find order info with p_ClodrID: {p_ClodrID}, p_SeqNum:{p_SeqNum}");
            return false;
        }

        public static bool UpdateMap_ClOrdID_Index(string p_ClOrdID)
        {
            for (int i = 0; i < c_ListOrder.Count; i++)
            {
                OrderInfo objOrder = c_ListOrder[i];
                if (objOrder.ClOrdID == p_ClOrdID)
                {
                    if (c_DicMapClOrdID.ContainsKey(p_ClOrdID))
                    {
                        c_DicMapClOrdID[p_ClOrdID] = objOrder;
                    }
                    else
                    {
                        c_DicMapClOrdID.TryAdd(p_ClOrdID, objOrder);
                    }
                    return true;
                }
            }
            return false;
        }

        public static bool UpdateToSeq(int oldSeq, int newSeq)
        {
            if (c_DictMapSeqNum.ContainsKey(oldSeq))
            {
                OrderInfo c_OldInfo;// = GetOrder_bySeqNum(oldSeq);
                c_DictMapSeqNum.TryRemove(oldSeq, out c_OldInfo);
                c_DictMapSeqNum.TryAdd(newSeq, c_OldInfo);
                Logger.log.Info("OrderMemory update Order from Seq {0} to Seq {1}", oldSeq, newSeq);
                c_StoreOrderInfo.StoreData(c_OldInfo);
                return true;
            }
            Logger.log.Warn("OrderMemory can't find order with Seq {0}", oldSeq);
            return false;
        }

        public static void UpdateOrderInfo(OrderInfo orderInfo, string p_RefMsgType = null, string p_OrderNo = null, string p_ClOrdID = null, string p_ExchangeID = null, string p_RefExchangeID = null,int p_SeqNum = 0, string p_Symbol = null, string p_Side = null, long p_Price = 0, long p_OrderQty = 0, string p_CrossType = null,string p_ClientID = null, string p_ClientIDCounterFirm = null, string p_MemberCounterFirm = null, string p_OrdType = null,int p_QuoteType=0)
        {
            if (p_RefMsgType != null)
                orderInfo.RefMsgType = p_RefMsgType;
            if (p_OrderNo != null)
                orderInfo.OrderNo = p_OrderNo;
            if (p_ClOrdID != null)
                orderInfo.ClOrdID = p_ClOrdID;
            if (p_ExchangeID != null)
                orderInfo.ExchangeID = p_ExchangeID;
            if (p_RefExchangeID != null)
                orderInfo.RefExchangeID = p_RefExchangeID;
            if (p_SeqNum != 0)
                orderInfo.SeqNum = p_SeqNum;
            if (p_Symbol != null)
                orderInfo.Symbol = p_Symbol;
            if (p_Side != null)
                orderInfo.Side = p_Side;

            if (p_Price != 0)
                orderInfo.Price = p_Price;
            if (p_OrderQty != 0)
                orderInfo.OrderQty = p_OrderQty;
            if (p_CrossType != null)
                orderInfo.CrossType = p_CrossType;
            if (p_ClientID != null)
                orderInfo.ClientID = p_ClientID;
            if (p_ClientIDCounterFirm != null)
                orderInfo.ClientIDCounterFirm = p_ClientIDCounterFirm;
            if (p_MemberCounterFirm != null)
                orderInfo.MemberCounterFirm = p_MemberCounterFirm;
            if (p_OrdType != null)
                orderInfo.OrderType = p_OrdType;
            if (p_QuoteType != 0)
                orderInfo.QuoteType = p_QuoteType;
            c_StoreOrderInfo.StoreData(orderInfo);
        }

        #endregion Updatedata

        public static bool CheckCldOrIDExist(string p_ClodrID)
        {
            if (c_DicMapClOrdID.ContainsKey(p_ClodrID))
            {
                return true;
            }

            return false;
        }

        #region GetData

        public static OrderInfo GetOrder_byClOrdID(string p_ClodrID)
        {
            if (c_DicMapClOrdID.ContainsKey(p_ClodrID))
            {
                return c_DicMapClOrdID[p_ClodrID];
            }
            return null;
        }

        public static bool IsExist_OrderNo(string OrderNo)
        {
            for (int i = 0; i < c_ListOrder.Count; i++)
            {
                OrderInfo objOrder = c_ListOrder[i];
                if (OrderNo == objOrder.OrderNo)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsExist_OrderNo_RefMsgType(string OrderNo, string RefExchangeID)
        {
            for (int i = 0; i < c_ListOrder.Count; i++)
            {
                OrderInfo objOrder = c_ListOrder[i];
                // Case 1: Nếu tìm được bản ghi có exchangeID = refExchangeID từ API gửi lên --> So sánh orderNo của API gửi lên với orderNo lấy được trong mem
                if (RefExchangeID == objOrder.ExchangeID)
                {
                    if (OrderNo == objOrder.OrderNo)
                    { 
                        return true; 
                    }
                    else
                    {
                        return false;  // ma loi 023
                    }
                }
            }
            // Case 2: Nếu không tìm được bản ghi exchangeID = refExchangeID từ API gửi lên --> API bỏ qua không check orderNo, lệnh xử lý tiếp như map cách lưu mem
            return true; 
        }

        public static List<OrderInfo> GetListOrder_byExchangeID(string p_RefExchangeID)
        {
            List<OrderInfo> listOrder = new List<OrderInfo>();
            for (int i = 0; i < c_ListOrder.Count; i++)
            {
                OrderInfo objOrder = c_ListOrder[i];
                if (objOrder.ExchangeID == p_RefExchangeID)
                {
                    listOrder.Add(objOrder);
                }
            }
            return listOrder;
        }

        public static OrderInfo GetOrder_ByOrderNo(string OrderNo)
        {
            for (int i = 0; i < c_ListOrder.Count; i++)
            {
                OrderInfo objOrder = c_ListOrder[i];
                if (OrderNo == objOrder.OrderNo)
                {
                    return objOrder;
                }
            }
            return null;
        }

        public static string GetOrigOrder_byClOrdID(string p_ClOrdID)
        {
            if (c_DicMapClOrdID.ContainsKey(p_ClOrdID))
            {
                return c_DicMapClOrdID[p_ClOrdID].OrderNo;
            }
            else
                return "";
        }

        public static OrderInfo GetOrder_bySeqNum(int p_SeqNum)
        {
            if (c_DictMapSeqNum.ContainsKey(p_SeqNum))
            {
                return c_DictMapSeqNum[p_SeqNum];
            }
            else
                return null;
        }

        public static OrderInfo GetOrder_byExchangeID(string p_ExchangeID)
        {
            for (int i = 0; i < c_ListOrder.Count; i++)
            {
                OrderInfo objOrder = c_ListOrder[i];
                if (p_ExchangeID == objOrder.ExchangeID)
                {
                    return objOrder;
                }
            }
            return null;
        }

        public static List<OrderInfo> GetListOrder_byRefMsgType_With_S_AJ(string p_RefExchangeID)
        {
            List<OrderInfo> listOrder = new List<OrderInfo>();
            for (int i = 0; i < c_ListOrder.Count; i++)
            {
                OrderInfo objOrder = c_ListOrder[i];
                if (objOrder.ExchangeID == p_RefExchangeID && (objOrder.RefMsgType == MessageType.Quote || objOrder.RefMsgType == MessageType.QuoteResponse))
                {
                    listOrder.Add(objOrder);
                }
            }
            return listOrder;
        }

        public static List<OrderInfo> GetListOrder_byExchangeID_RefMsgType(string p_RefExchangeID)
        {
            List<OrderInfo> listOrder = new List<OrderInfo>();
            for (int i = 0; i < c_ListOrder.Count; i++)
            {
                OrderInfo objOrder = c_ListOrder[i];
                if (objOrder.ExchangeID == p_RefExchangeID && (objOrder.RefMsgType == MessageType.ReposFirm || objOrder.RefMsgType == MessageType.ReposFirmAccept))
                {
                    listOrder.Add(objOrder);
                }
            }
            return listOrder;
        }

        public static OrderInfo GetOrder_ByRefExchangeID_ExChangeIDNull_RefMsgType(string p_RefExchangeID, string p_RefMsgType)
        {
            for (int i = 0; i < c_ListOrder.Count; i++)
            {
                OrderInfo objOrder = c_ListOrder[i];
                if (p_RefExchangeID == objOrder.RefExchangeID && string.IsNullOrEmpty(objOrder.ExchangeID) && objOrder.RefMsgType == p_RefMsgType)
                {
                    return objOrder;
                }
            }
            return null;
        }

        public static OrderInfo GetOrderBy(string p_RefMsgType = null, string p_OrderNo = null, string p_ClOrdID = null, string p_ExchangeID = null, string p_RefExchangeID = null,
                                            int p_SeqNum = 0, string p_Symbol = null, string p_Side = null, long p_Price = 0, long p_OrderQty = 0, string p_CrossType = null,
                                            string p_ClientID = null, string p_ClientIDCounterFirm = null, string p_MemberCounterFirm = null, string p_OrdType = null)

        {
            for (int i = 0; i < c_ListOrder.Count; i++)
            {
                OrderInfo objOrder = c_ListOrder[i];
                if (p_RefMsgType != null && objOrder.RefMsgType != p_RefMsgType)
                    continue;
                if (p_OrderNo != null && objOrder.OrderNo != p_OrderNo)
                    continue;
                if (p_ClOrdID != null && objOrder.ClOrdID != p_ClOrdID)
                    continue;
                if (p_ExchangeID != null && objOrder.ExchangeID != p_ExchangeID)
                    continue;
                if (p_RefExchangeID != null && objOrder.RefExchangeID != p_RefExchangeID)
                    continue;
                if (p_SeqNum != 0 && objOrder.SeqNum != p_SeqNum)
                    continue;
                if (p_Symbol != null && objOrder.Symbol != p_Symbol)
                    continue;
                if (p_Side != null && objOrder.Side != p_Side)
                    continue;
                if (p_Price != 0 && objOrder.Price != p_Price)
                    continue;
                if (p_OrderQty != 0 && objOrder.OrderQty != p_OrderQty)
                    continue;
                if (p_CrossType != null && objOrder.CrossType != p_CrossType)
                    continue;
                if (p_ClientID != null && objOrder.ClientID != p_ClientID)
                    continue;
                if (p_ClientIDCounterFirm != null && objOrder.ClientIDCounterFirm != p_ClientIDCounterFirm)
                    continue;
                if (p_MemberCounterFirm != null && objOrder.MemberCounterFirm != p_MemberCounterFirm)
                    continue;
                if (p_OrdType != null && objOrder.OrderType != p_OrdType)
                    continue;
                return objOrder;
            }
            return null;
        }

        public static bool CheckValidOrderNoForReplaceCancel(string OrderNo, string RefExchangeID)
        {
            OrderInfo objOrder = GetOrder_byExchangeID(RefExchangeID);
            if (objOrder == null)
            {
                return true;
            }
            else
            {
                objOrder = GetOrderBy(p_ExchangeID: RefExchangeID, p_OrderNo: OrderNo);
                if (objOrder != null)
                    return true;
                else
                    return false;
            }
        }

        #endregion GetData

        public static List<OrderInfo> GetListOrder()
        {
            return c_ListOrder;
        }

        #region Recover

        private static void RecoverData(List<OrderInfo> data)
        {
            try
            {
                c_ListOrder = data;
                for (int i = 0; i < c_ListOrder.Count; i++)
                {
                    OrderInfo objOrder = c_ListOrder[i];
                    if (!c_DicMapClOrdID.ContainsKey(objOrder.ClOrdID))
                    {
                        c_DicMapClOrdID.TryAdd(objOrder.ClOrdID, objOrder);
                    }
                    if (!c_DictMapSeqNum.ContainsKey(objOrder.SeqNum))
                    {
                        c_DictMapSeqNum.TryAdd(objOrder.SeqNum, objOrder);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }

        public static void RecoverAndStartStoring()
        {
            RecoverData(c_StoreOrderInfo.RecoverData());
            c_StoreOrderInfo.StartLogging();
        }

        #endregion Recover
    }

    public class OrderInfo
    {
        public string Key;
        public string RefMsgType { get; set; } = "";
        public string OrderNo { get; set; } = "";
        public string ClOrdID { get; set; } = "";
        public string ExchangeID { get; set; } = "";
        public string RefExchangeID { get; set; } = "";
        public int SeqNum { get; set; }
        public string Symbol { get; set; } = "";
        public string Side { get; set; } = "";
        public long Price { get; set; }
        public long PriceMM2 { get; set; }
        public long OrderQty { get; set; }
        public long OrderQtyMM2 { get; set; }
        public int SpecialType { get; set; }
        public string CrossType { get; set; } = "";
        public string ClientID { get; set; } = "";
        public string ClientIDCounterFirm { get; set; } = "";
        public string MemberCounterFirm { get; set; } = "";
        public string OrderType { get; set; } = "";
        public int QuoteType { get; set; }
        public int SettlMethod { get; set; }
        public string SettleDate1 { get; set; }
        public string SettleDate2 { get; set; }
        public string EndDate { get; set; }
        public int RepurchaseTerm { get; set; }
        public double RepurchaseRate { get; set; }
        public long NoSide { get; set; }

        public List<SymbolFirmObject> SymbolFirmInfo { get; set; }

        public OrderInfo()
        {
            Key = Guid.NewGuid().ToString();
        }
    }

    public class SymbolFirmObject
    {
        public long NumSide { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public long OrderQty { get; set; }
        public double HedgeRate { get; set; }
        public long MergePrice { get; set; }
	}
}