using System.Text;
using static HNX.FIXMessage.MessageReposBCGDReport;

namespace HNX.FIXMessage
{
    public class MessageHelperReposBCGDReport : MessageHelper
    {
        private MessageReposBCGDReport c_message;

        #region #Tag const

        private const int TAG_ClOrdID = 11;
        private const int TAG_OrgOrderID = 198;
        private const int TAG_OrderID = 37;
        private const int TAG_QuoteType = 563;
        private const int TAG_OrdType = 40;
        private const int TAG_OrderPartyID = 4488;
        private const int TAG_InquiryMember = 4499;
        private const int TAG_Side = 54;
        private const int TAG_EffectiveTime = 168;
        private const int TAG_RepurchaseTerm = 226;
        private const int TAG_RepurchaseRate = 227;
        private const int TAG_SetDate = 64;
        private const int TAG_SetDate2 = 193;
        private const int TAG_EndDate = 917;
        private const int TAG_SetMethod = 6363;
        private const int TAG_Account = 1;
        private const int TAG_CoAccount = 2;
        private const int TAG_PartyID = 448; //TV ban
        private const int TAG_CoPartyID = 449; //TV mua
        private const int TAG_NoSide = 552; //Số mã ck có trong msg khớp
        private const int TAG_MatchReportType = 5632;       //Tag 5632 thông báo là khớp repos trong ngày hay thông tin repos leg2

        //
        public const int TAG_NumSide = 5522;//Số thứ tự của mức giá

        private const int TAG_Symbol = 55; //ma chung khoan
        private const int TAG_OrderQty = 38; //khoi luong dat lenh
        private const int TAG_Price = 44;                  // gia yết tương ứng với giá khớp
        private const int TAG_ExecPrice = 640; //gia thực hiện giá bẩn
        private const int TAG_ReposInterest = 2261;
        private const int TAG_HedgeRate = 2260;
        private const int TAG_SettlValue = 6464; //6464 giá trị thanh toán
        private const int TAG_SettlValue2 = 6465; //ngay thanh toan lan 2

        #endregion #Tag const

        public MessageHelperReposBCGDReport(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageReposBCGDReport)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            sb.Append(TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            sb.Append(TAG_OrderID.ToString()).Append(Common.DELIMIT).Append(c_message.OrderID).Append(Common.SOH);
            sb.Append(TAG_OrgOrderID.ToString()).Append(Common.DELIMIT).Append(c_message.OrgOrderID).Append(Common.SOH);
            sb.Append(TAG_QuoteType.ToString()).Append(Common.DELIMIT).Append(c_message.QuoteType).Append(Common.SOH);
            sb.Append(TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);
            sb.Append(TAG_OrderPartyID.ToString()).Append(Common.DELIMIT).Append(c_message.OrderPartyID).Append(Common.SOH);
            sb.Append(TAG_InquiryMember.ToString()).Append(Common.DELIMIT).Append(c_message.InquiryMember).Append(Common.SOH);
            sb.Append(TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
            sb.Append(TAG_EffectiveTime.ToString()).Append(Common.DELIMIT).Append(c_message.EffectiveTime).Append(Common.SOH);
            sb.Append(TAG_RepurchaseRate.ToString()).Append(Common.DELIMIT).Append(c_message.RepurchaseRate).Append(Common.SOH);
            sb.Append(TAG_RepurchaseTerm.ToString()).Append(Common.DELIMIT).Append(c_message.RepurchaseTerm).Append(Common.SOH);
            sb.Append(TAG_SetDate.ToString()).Append(Common.DELIMIT).Append(c_message.SettlDate).Append(Common.SOH);
            sb.Append(TAG_SetDate2.ToString()).Append(Common.DELIMIT).Append(c_message.SettlDate2).Append(Common.SOH);
            sb.Append(TAG_EndDate.ToString()).Append(Common.DELIMIT).Append(c_message.EndDate).Append(Common.SOH);
            sb.Append(TAG_SetMethod.ToString()).Append(Common.DELIMIT).Append(c_message.SettlMethod).Append(Common.SOH);
            sb.Append(TAG_Account.ToString()).Append(Common.DELIMIT).Append(c_message.Account).Append(Common.SOH);
            sb.Append(TAG_CoAccount.ToString()).Append(Common.DELIMIT).Append(c_message.CoAccount).Append(Common.SOH);
            sb.Append(TAG_PartyID.ToString()).Append(Common.DELIMIT).Append(c_message.PartyID).Append(Common.SOH);
            sb.Append(TAG_CoPartyID.ToString()).Append(Common.DELIMIT).Append(c_message.CoPartyID).Append(Common.SOH);
            sb.Append(TAG_NoSide).Append(Common.DELIMIT).Append(c_message.NoSide).Append(Common.SOH);
            sb.Append(TAG_MatchReportType.ToString()).Append(Common.DELIMIT).Append(c_message.MatchReportType).Append(Common.SOH);

            ReposSideReposBCGDReport itemSite;
            for (int i = 0; i < c_message.RepoBCGDSideList.Count; i++)
            {
                itemSite = c_message.RepoBCGDSideList[i];
                //
                sb.Append(TAG_NumSide).Append(Common.DELIMIT).Append(itemSite.NumSide).Append(Common.SOH);
                sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(itemSite.Symbol).Append(Common.SOH);
                sb.Append(TAG_OrderQty.ToString()).Append(Common.DELIMIT).Append(itemSite.OrderQty).Append(Common.SOH);
                sb.Append(TAG_Price.ToString()).Append(Common.DELIMIT).Append(itemSite.Price).Append(Common.SOH);
                sb.Append(TAG_ExecPrice.ToString()).Append(Common.DELIMIT).Append(itemSite.ExecPrice).Append(Common.SOH);
                sb.Append(TAG_ReposInterest.ToString()).Append(Common.DELIMIT).Append(itemSite.ReposInterest).Append(Common.SOH);
                sb.Append(TAG_HedgeRate.ToString()).Append(Common.DELIMIT).Append(itemSite.HedgeRate).Append(Common.SOH);
                sb.Append(TAG_SettlValue.ToString()).Append(Common.DELIMIT).Append(itemSite.SettlValue).Append(Common.SOH);
                sb.Append(TAG_SettlValue2.ToString()).Append(Common.DELIMIT).Append(itemSite.SettlValue2).Append(Common.SOH);
            }
        }

        private int _numSide = 0;

        private ReposSideReposBCGDReport _ReposSide = null;
        private ReposSideReposBCGDReportList _ReposList = new ReposSideReposBCGDReportList();
        public static bool islast = false;

        public override bool ParseField(Field field)
        {
            if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if(TAG_OrgOrderID == field.Tag)
                c_message.OrgOrderID = field.Value;
            else if (TAG_OrderID == field.Tag)
                c_message.OrderID = field.Value;
            else if (TAG_QuoteType == field.Tag)
            {
                bool _isSuccess = false;
                c_message.QuoteType = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_OrdType == field.Tag)
                c_message.OrdType = field.Value;
            else if (TAG_OrderPartyID == field.Tag)
                c_message.OrderPartyID = field.Value;
            else if (TAG_InquiryMember == field.Tag)
                c_message.InquiryMember = field.Value;
            else if (TAG_Side == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Side = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_EffectiveTime == field.Tag)
                c_message.EffectiveTime = field.Value;
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
            else if (TAG_SetDate == field.Tag)
                c_message.SettlDate = field.Value;
            else if (TAG_SetDate2 == field.Tag)
                c_message.SettlDate2 = field.Value;
            else if (TAG_EndDate == field.Tag)
                c_message.EndDate = field.Value;
            else if (TAG_SetMethod == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SettlMethod = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_Account == field.Tag)
                c_message.Account = field.Value;
            else if (TAG_CoAccount == field.Tag)
                c_message.CoAccount = field.Value;
            else if (TAG_PartyID == field.Tag)
                c_message.PartyID = field.Value;
            else if (TAG_CoPartyID == field.Tag)
                c_message.CoPartyID = field.Value;
            else if (TAG_NoSide == field.Tag)
            {
                bool _isSuccess = false;
                c_message.NoSide = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_MatchReportType == field.Tag)
            {
                bool _isSuccess = false;
                c_message.MatchReportType = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            //
            else if (TAG_NumSide == field.Tag)
            {
                _numSide++;
                if (_ReposSide != null)
                    _ReposList.Add(_ReposSide);

                _ReposSide = new ReposSideReposBCGDReport();
                bool _isSuccess = false;
                _ReposSide.NumSide = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_Symbol == field.Tag)
            {
                _ReposSide.Symbol = field.Value;
            }
            else if (TAG_OrderQty == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.OrderQty = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_Price == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.Price = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_ExecPrice == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.ExecPrice = Utils.Convert.ParseInt64(field.Value, ref _isSuccess);
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
            else if (base.ParseField(field) == false) return false;
            
            return true;
        }

        public override void AddReposSide()
        {
            base.AddReposSide();
            if (islast && _numSide == c_message.NoSide)
            {
                _ReposList.Add(_ReposSide);
                c_message.RepoBCGDSideList = _ReposList;
            }

        }
    }
}