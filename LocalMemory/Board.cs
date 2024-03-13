using System.Security.Cryptography;
using CommonLib;
using HNX.FIXMessage;
using Microsoft.AspNetCore.Http;
using static CommonLib.CommonData;

namespace LocalMemory
{
    /// <summary>
    /// Class lưu thông tin trái phiếu
    /// </summary>
    public class BOND
    {

        public string Name { get; set; }    //mã chứng khoán

        public string TradingSession { get; set; } //Mã phiên 

        public string TradeSesStatus { get; set; } = string.Empty;   //phiên của mã ck

        public int SecurityTradingStatus { get; set; } = 0;//trạng thái mã ck

        public void UpdateStatus(MessageSecurityStatus message)
        {
            SecurityTradingStatus = message.SecurityTradingStatus;
        }

        public void UpdateStatus(MessageTradingSessionStatus message)
        {
            TradeSesStatus = message.TradSesStatus;
            TradingSession = message.TradingSessionID;
        }
    }

    public class Board
    {
        public string BoardCode { get; set; } //Mã bảng
        public string TradingSession = string.Empty; //mã phiên
        public string TradeSesStatus = string.Empty;// trạng thái

        public Dictionary<string, BOND> ListBOND = new Dictionary<string, BOND>();

        /// <summary>
        /// Check xem có chấp nhận lệnh đặt ở đầu api
        /// </summary>
        /// <returns></returns>
        public bool CheckAcceptOrder(string Symbol, out string Text, out string Code)
        {
            if (TradeSesStatus == "1")
            {
                return CheckBondTradingSession(Symbol, out Text, out Code);
            }
            else
            {
                Text = CommonData.ORDER_RETURNMESSAGE.BOND_GATEWAY_HAS_NO_SESSION;
                Code = CommonData.ORDER_RETURNCODE.BOND_GATEWAY_HAS_NO_SESSION;
                return false;
            }

        }

        public bool CheckBondTradingSession(string Bond, out string Text, out string Code)
        {
            try
            {       
                if (ListBOND[Bond].TradeSesStatus == TradingSessionStatus.PauseSession)
                {
                    Text = ORDER_RETURNMESSAGE.MARKET_IN_BREAK_TIME;
                    Code = ORDER_RETURNCODE.MARKET_IN_BREAK_TIME;
                    return false;
                }    
                if (ListBOND[Bond].TradeSesStatus == TradingSessionStatus.WaitforOrder )
                {
                    Text = ORDER_RETURNMESSAGE.BOND_GATEWAY_HAS_NO_SESSION;
                    Code = ORDER_RETURNCODE.BOND_GATEWAY_HAS_NO_SESSION;
                    return false;
                }
                else if ( ListBOND[Bond].TradeSesStatus == TradingSessionStatus.EndOfTradingSession
                        || ListBOND[Bond].TradeSesStatus == TradingSessionStatus.MarketClose)
                {
                    Text = CommonData.ORDER_RETURNMESSAGE.MARKET_CLOSE;
                    Code = CommonData.ORDER_RETURNCODE.MARKET_CLOSE;
                    return false;
                }
                else if (ListBOND[Bond].SecurityTradingStatus == SecurityTradingSessionStatus.Stop_Trading ||
                         ListBOND[Bond].SecurityTradingStatus == SecurityTradingSessionStatus.Pause_Trading)
                {
                    Text = CommonData.ORDER_RETURNMESSAGE.SYMBOL_IS_SUSPENDED;
                    Code = CommonData.ORDER_RETURNCODE.SYMBOL_IS_SUSPENDED;
                    return false;
                }    
                else
                {
                    return CheckTradingSession(out Text, out Code);
                }

            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
                Text = CommonData.ORDER_RETURNMESSAGE.Application_Error;
                Code = CommonData.ORDER_RETURNCODE.Application_Error;
                return false;
            }
        }

        public bool CheckTradingSession(out string Text, out string Code)
        {
            if (TradeSesStatus == TradingSessionStatus.InTradingSession)
            {
                Text = CommonData.ORDER_RETURNMESSAGE.SUCCESS;
                Code = CommonData.ORDER_RETURNCODE.SUCCESS;
                return true;
            }
            else if (TradeSesStatus == TradingSessionStatus.BeforeTradingSession
                || TradeSesStatus == TradingSessionStatus.PauseSession)               
            {
                Text = CommonData.ORDER_RETURNMESSAGE.MARKET_IN_BREAK_TIME;
                Code = CommonData.ORDER_RETURNCODE.MARKET_IN_BREAK_TIME;
                return false;
            }
            else if (TradeSesStatus == TradingSessionStatus.WaitforOrder)
            {
                Text = CommonData.ORDER_RETURNMESSAGE.BOND_GATEWAY_HAS_NO_SESSION;
                Code = CommonData.ORDER_RETURNCODE.BOND_GATEWAY_HAS_NO_SESSION;
                return false;
            }
            else
            {
                Text = CommonData.ORDER_RETURNMESSAGE.MARKET_CLOSE;
                Code = CommonData.ORDER_RETURNCODE.MARKET_CLOSE;
                return false;
            }

        }
    }
}