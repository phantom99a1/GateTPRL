using System.Text;
using static HNX.FIXMessage.MessageExecOrderRepos;

namespace HNX.FIXMessage
{
    public class MessageHelperExecOrderRepos : MessageHelper
    {
        private MessageExecOrderRepos c_message;

        #region #Tag const

        private const int TAG_ClOrdID = 11;
        private const int TAG_PartyID = 448;// Mã thành viên bán
        private const int TAG_CoPartyID = 449;// Mã thành viên mua
        private const int TAG_OrderID = 37;// Số hiệu lệnh khớp.
        private const int TAG_BuyOrderID = 41;// Số hiệu lệnh gốc bên mua
        private const int TAG_SellOrderID = 526;// Số hiệu lệnh gốc bên bán
        private const int TAG_RepurchaseTerm = 226;
        private const int TAG_RepurchaseRate = 227;
        private const int TAG_SettDate = 64; // 64 ngày thanh toán.
        private const int TAG_SetDate2 = 193;
        private const int TAG_EndDate = 917;
        private const int TAG_MatchReportType = 5632;       //Tag 5632 thông báo là khớp repos trong ngày hay thông tin repos leg2
        private const int TAG_NoSide = 552; //Số mã ck có trong msg khớp
        private const int TAG_SettMethod = 6363; // 6363.

        //---------------------------------------------------
        public const int TAG_NumSide = 5522;//Số thứ tự của mức giá

        private const int TAG_Symbol = 55;//55 mã chứng khoán
        private const int TAG_ExecQty = 32;// Khối lượng khớp
        private const int TAG_ExecPx = 31;// Giá khớp
        private const int TAG_Price = 44; //giá gộp lãi
        private const int TAG_ReposInterest = 2261;
        private const int TAG_HedgeRate = 2260;
        private const int TAG_SettlValue = 6464; //6464 giá trị thanh toán
        private const int TAG_SettlValue2 = 6465; //ngay thanh toan lan 2

        #endregion #Tag const

        public MessageHelperExecOrderRepos(FIXMessageBase message)
            : base(message)
        {
            try
            {
                c_message = (MessageExecOrderRepos)message;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            sb.Append(TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            sb.Append(TAG_PartyID.ToString()).Append(Common.DELIMIT).Append(c_message.PartyID).Append(Common.SOH);
            sb.Append(TAG_CoPartyID.ToString()).Append(Common.DELIMIT).Append(c_message.CoPartyID).Append(Common.SOH);
            sb.Append(TAG_OrderID.ToString()).Append(Common.DELIMIT).Append(c_message.OrderID).Append(Common.SOH);
            sb.Append(TAG_BuyOrderID.ToString()).Append(Common.DELIMIT).Append(c_message.BuyOrderID).Append(Common.SOH);
            sb.Append(TAG_SellOrderID.ToString()).Append(Common.DELIMIT).Append(c_message.SellOrderID).Append(Common.SOH);
            sb.Append(TAG_RepurchaseTerm.ToString()).Append(Common.DELIMIT).Append(c_message.RepurchaseTerm).Append(Common.SOH);
            sb.Append(TAG_RepurchaseRate.ToString()).Append(Common.DELIMIT).Append(c_message.RepurchaseRate).Append(Common.SOH);
            sb.Append(TAG_SettDate.ToString()).Append(Common.DELIMIT).Append(c_message.SettDate).Append(Common.SOH);
            sb.Append(TAG_SetDate2.ToString()).Append(Common.DELIMIT).Append(c_message.SettlDate2).Append(Common.SOH);
            sb.Append(TAG_EndDate.ToString()).Append(Common.DELIMIT).Append(c_message.EndDate).Append(Common.SOH);
            sb.Append(TAG_MatchReportType.ToString()).Append(Common.DELIMIT).Append(c_message.MatchReportType).Append(Common.SOH);
            sb.Append(TAG_NoSide).Append(Common.DELIMIT).Append(c_message.NoSide).Append(Common.SOH);
            sb.Append(TAG_SettMethod.ToString()).Append(Common.DELIMIT).Append(c_message.SettlMethod).Append(Common.SOH);

            ReposSideExecOrder itemSite;
            for (int i = 0; i < c_message.ReposSideList.Count; i++)
            {
                itemSite = c_message.ReposSideList[i];
                //
                sb.Append(TAG_NumSide).Append(Common.DELIMIT).Append(itemSite.NumSide).Append(Common.SOH);
                sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(itemSite.Symbol).Append(Common.SOH);
                sb.Append(TAG_ExecQty.ToString()).Append(Common.DELIMIT).Append(itemSite.ExecQty).Append(Common.SOH);
                sb.Append(TAG_ExecPx.ToString()).Append(Common.DELIMIT).Append(itemSite.ExecPx).Append(Common.SOH);
                sb.Append(TAG_Price.ToString()).Append(Common.DELIMIT).Append(itemSite.Price).Append(Common.SOH);
                sb.Append(TAG_ReposInterest.ToString()).Append(Common.DELIMIT).Append(itemSite.ReposInterest).Append(Common.SOH);
                sb.Append(TAG_HedgeRate.ToString()).Append(Common.DELIMIT).Append(itemSite.HedgeRate).Append(Common.SOH);
                sb.Append(TAG_SettlValue.ToString()).Append(Common.DELIMIT).Append(itemSite.SettlValue).Append(Common.SOH);
                sb.Append(TAG_SettlValue2.ToString()).Append(Common.DELIMIT).Append(itemSite.SettlValue2).Append(Common.SOH);
            }
        }

        private int _numSide = 0;

        private ReposSideExecOrder _ReposSide = null;
        private ReposSideListExecOrder _ReposList = new ReposSideListExecOrder();
        public static bool islast = false;

        public override bool ParseField(Field field)
        {
            if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (TAG_PartyID == field.Tag)
                c_message.PartyID = field.Value;
            else if (TAG_CoPartyID == field.Tag)
                c_message.CoPartyID = field.Value;
            else if (TAG_OrderID == field.Tag)
                c_message.OrderID = field.Value;
            else if (TAG_BuyOrderID == field.Tag)
                c_message.BuyOrderID = field.Value;
            else if (TAG_SellOrderID == field.Tag)
                c_message.SellOrderID = field.Value;
            //
            else if (TAG_RepurchaseTerm == field.Tag)
            {
                bool _isSuccess = false;
                c_message.RepurchaseTerm = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_RepurchaseRate == field.Tag)
            {
                bool _isSuccess = false;
                c_message.RepurchaseRate = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_SettDate == field.Tag)
                c_message.SettDate = field.Value;
            else if (TAG_SetDate2 == field.Tag)
                c_message.SettlDate2 = field.Value;
            else if (TAG_EndDate == field.Tag)
                c_message.EndDate = field.Value;
            else if (TAG_MatchReportType == field.Tag)
            {
                bool _isSuccess = false;
                c_message.MatchReportType = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_NoSide == field.Tag)
            {
                bool _isSuccess = false;
                c_message.NoSide = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_SettMethod == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SettlMethod = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_NumSide == field.Tag)
            {
                _numSide++;
                if (_ReposSide != null)
                    _ReposList.Add(_ReposSide);

                _ReposSide = new ReposSideExecOrder();
                bool _isSuccess = false;
                _ReposSide.NumSide = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_Symbol == field.Tag)
            {
                _ReposSide.Symbol = field.Value;
            }
            else if (TAG_ExecQty == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.ExecQty = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_ExecPx == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.ExecPx = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_Price == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.Price = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_ReposInterest == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.ReposInterest = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_HedgeRate == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.HedgeRate = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_SettlValue == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.SettlValue = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_SettlValue2 == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.SettlValue2 = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else
                if (base.ParseField(field) == false) return false;

            return true;
        }

        public override void AddReposSide()
        {
            base.AddReposSide();
            if (islast && _numSide == c_message.NoSide)
            {
                if (_ReposSide != null)
                    _ReposList.Add(_ReposSide);
                c_message.ReposSideList = _ReposList;
            }
        }
    }
}