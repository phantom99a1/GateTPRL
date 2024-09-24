using CommonLib;
using HNX.FIXMessage;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommonLib.CommonData;
using static CommonLib.CommonData.TradingSessionStatus;

namespace LocalMemory
{
    public class TradingRuleData
    {
        private static Dictionary<string, Board> Boards = new Dictionary<string, Board>();

        public static string GetTradingSessionNameofMainBoard()
        {
            string _MainBoard = ConfigData.MainBoard;
            string _TradingSession = "N/A";
            if (Boards.ContainsKey(_MainBoard))
            {
                switch (Boards[_MainBoard].TradeSesStatus)
                {
                    case TradingSessionStatus.InTradingSession:
                        _TradingSession = "Dang giao dich";
                        break;
                    case TradingSessionStatus.PauseSession:
                        _TradingSession = "Tam dung giao dich";
                        break;
                    case TradingSessionStatus.EndOfTradingSession:
                        _TradingSession = "Ket thuc nhan lenh";
                        break;
                    case TradingSessionStatus.WaitforOrder:
                        _TradingSession = "Cho nhan lenh";
                        break;
                    case TradingSessionStatus.MarketClose:
                        _TradingSession = "Dong cua thi truong";
                        break;
                    default:
                        break;
                }

            }
            return _TradingSession;
        }

        public static string GetTradeSesStatusofMainBoard()
        {
            string _MainBoard = ConfigData.MainBoard;
            if (Boards.ContainsKey(_MainBoard))
            {
                return Boards[_MainBoard].TradeSesStatus;
            }
            return TradingSessionStatus.BeforeTradingSession;

        }

        public static string GetTradingSessionCodeofMainBoard()
        {
			string _MainBoard = ConfigData.MainBoard;
			if (Boards.ContainsKey(_MainBoard))
			{
				return Boards[_MainBoard].TradingSession;
			}
			return "N/A";
		}

        public static bool IsHaveData()
        {
            if (Boards.Count == 0)
                return false;
            foreach (KeyValuePair<string, Board> _board in Boards)
                if (_board.Value.ListBOND.Count == 0)
                    return false;
            return true;
        }

        /// <summary>
        /// Check rule khi nhận api
        /// </summary>
        /// <param name="Symbol"></param>
        /// <param name="Text">Thông tin trả ra</param>
        /// <returns></returns>
        public static bool CheckTradingRule_Input(string Symbol, out string Text, out string Code)
        {
            foreach (KeyValuePair<string, Board> board in Boards)
            {
                if (board.Value.ListBOND.ContainsKey(Symbol))
                {
                    return board.Value.CheckBondTradingSession(Symbol, out Text, out Code);
                }
            }
            Text = ORDER_RETURNMESSAGE.SYMBOL_IS_NOT_FOUND;
            Code = ORDER_RETURNCODE.SYMBOL_IS_NOT_FOUND;
            return false;
        }
        /// <summary>
        /// Check 
        /// </summary>
        /// <param name="Symbol">Mã trái phiếu</param>
        /// <param name="Text">Thông tin trả ra</param>
        /// <returns></returns>
        public static bool CheckTradingRule_Output(string Symbol, out string Text, out string Code)
        {
            if (string.IsNullOrEmpty(Symbol))
            {
                return CheckTradingSessionWithSymbolNull(out Text, out Code);

            }    
            else
            {
                foreach (KeyValuePair<string, Board> board in Boards)
                {
                    if (board.Value.ListBOND.ContainsKey(Symbol))
                    {
                        return board.Value.CheckBondTradingSession(Symbol, out Text, out Code);
                    }
                }
                //Ra đến đây thì chắc là sai ở đâu rôid
                Logger.log.Error("Something went wrong with Symbol {0}", Symbol);
                Text = ORDER_RETURNMESSAGE.Application_Error;
                Code = ORDER_RETURNCODE.Application_Error;
                return false;
            }    
        }

        public static bool CheckTradingSessionWithSymbolNull(out string Text, out string Code)
        {
            string _MainBoard = ConfigData.MainBoard;
            foreach (Board _board in Boards.Values)
            {
                if (_board.CheckTradingSession(out Text, out Code))
                {
                    return true;
                }    
                    
            }    

            // Nếu bảng nào cũng bị dừng thì trả theo MainBoard
            if (Boards.ContainsKey(_MainBoard))
            {
                return Boards[_MainBoard].CheckTradingSession(out Text, out Code);
            }
            else
            {
                Logger.log.Error("Something went wrong with MainBoard {0}. Can't find data of {0} int memory, please check in file Config", _MainBoard);
                Text = ORDER_RETURNMESSAGE.Application_Error;
                Code = ORDER_RETURNCODE.Application_Error;
                return false;
            }
        }

        public static void ProcessTradingSession(MessageTradingSessionStatus message)
        {
            if (message.TradSesMode == TradingSessionMode.byTable)
            {
                //Message thông báo cho bảng
                if (Boards.ContainsKey(message.TradSesReqID))
                {
                    Boards[message.TradSesReqID].TradingSession = message.TradingSessionID;
                    Boards[message.TradSesReqID].TradeSesStatus = message.TradSesStatus;
                }
                else
                {
                    Board board = new Board()
                    {
                        BoardCode = message.TradSesReqID,
                        TradingSession = message.TradingSessionID,
                        TradeSesStatus = message.TradSesStatus,

                    };
                    Boards.Add(message.TradSesReqID, board);
                }
            }
            else if (message.TradSesMode == TradingSessionMode.bySymbol)
            {
                //Thông báo chuyển phiên cho trái phiếu
                foreach (KeyValuePair<string, Board> Data in Boards)
                {                    
                    if (Data.Value.BoardCode == message.TradingSessionID)
                    {
                        if (Data.Value.ListBOND.ContainsKey(message.TradSesReqID))
                            Data.Value.ListBOND[message.TradSesReqID].UpdateStatus(message);
                        else
                            Data.Value.ListBOND.Add(message.TradSesReqID, new BOND() { Name = message.TradSesReqID});
                        return;
                    }                  
                }
                Board board = new Board()
                {
                    BoardCode = message.TradingSessionID,

                };
                Boards.Add(message.TradSesReqID, board);
                board.ListBOND.Add(message.TradSesReqID, new BOND() { Name = message.TradSesReqID });

            }
        }

        public static void ProcessSecurityStatus(MessageSecurityStatus message)
        {
            BOND bond = new BOND()
            {
                Name = message.Symbol,
                SecurityTradingStatus = message.SecurityTradingStatus,
            };
            if (Boards.ContainsKey(message.TradingSessionSubID))
            {
                if (!Boards[message.TradingSessionSubID].ListBOND.ContainsKey(message.Symbol))
                    Boards[message.TradingSessionSubID].ListBOND.Add(message.Symbol, bond);               
            }
            else
            {
                Board board = new Board()
                {
                    BoardCode = message.TradingSessionSubID
                };
                Boards.Add(message.TradingSessionSubID, board);
                Boards[message.TradingSessionSubID].ListBOND.Add(message.Symbol, bond);                
            }
            Boards[message.TradingSessionSubID].ListBOND[message.Symbol].UpdateStatus(message);
        }
    }
}
