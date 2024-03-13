using System.Text;

namespace HNX.FIXMessage
{
    public class MessageHelperReposFirm : MessageHelper
    {
        private MessageReposFirm c_message;

        #region #Tag const

        private const int TAG_ClOrdID = 11;
        private const int TAG_RFQReqID = 644;
        private const int TAG_QuoteType = 537;
        private const int TAG_OrdType = 40;
        private const int TAG_Side = 54;
        private const int TAG_Account = 1;
        private const int TAG_EffectiveTime = 168;
        private const int TAG_SettlMethod = 6363;
        private const int TAG_SettlDate = 64;
        private const int TAG_SettlDate2 = 193;
        private const int TAG_EndDate = 917;
        private const int TAG_RepurchaseTerm = 226;
        private const int TAG_RepurchaseRate = 227;
        private const int TAG_NoSide = 552;
        //
        private const int TAG_NumSide = 5522;
        private const int TAG_Symbol = 55;
        private const int TAG_OrderQty = 38;
        private const int TAG_Price = 44;
        private const int TAG_HedgeRate = 2260;

        private const int TAG_ReposInterest = 2261;
        private const int TAG_SettlValue = 6464;
        private const int TAG_SettlValue2 = 6465;

        #endregion #Tag const

        public MessageHelperReposFirm(FIXMessageBase message)
            : base(message)
        {
            c_message = (MessageReposFirm)message;
        }

        public override void BuildBody(StringBuilder sb)
        {
            base.BuildBody(sb);

            sb.Append(TAG_ClOrdID).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
            sb.Append(TAG_RFQReqID.ToString()).Append(Common.DELIMIT).Append(c_message.RFQReqID).Append(Common.SOH);
            sb.Append(TAG_QuoteType.ToString()).Append(Common.DELIMIT).Append(c_message.QuoteType).Append(Common.SOH);

            sb.Append(TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);
            sb.Append(TAG_Side.ToString()).Append(Common.DELIMIT).Append(c_message.Side).Append(Common.SOH);
            sb.Append(TAG_Account.ToString()).Append(Common.DELIMIT).Append(c_message.Account).Append(Common.SOH);

            sb.Append(TAG_EffectiveTime.ToString()).Append(Common.DELIMIT).Append(c_message.EffectiveTime).Append(Common.SOH);
            sb.Append(TAG_SettlMethod.ToString()).Append(Common.DELIMIT).Append(c_message.SettlMethod).Append(Common.SOH);
            sb.Append(TAG_SettlDate.ToString()).Append(Common.DELIMIT).Append(c_message.SettlDate).Append(Common.SOH);
            //
            sb.Append(TAG_SettlDate2.ToString()).Append(Common.DELIMIT).Append(c_message.SettlDate2).Append(Common.SOH);
            sb.Append(TAG_EndDate.ToString()).Append(Common.DELIMIT).Append(c_message.EndDate).Append(Common.SOH);
            sb.Append(TAG_RepurchaseTerm.ToString()).Append(Common.DELIMIT).Append(c_message.RepurchaseTerm).Append(Common.SOH);
            //
            sb.Append(TAG_RepurchaseRate.ToString()).Append(Common.DELIMIT).Append(c_message.RepurchaseRate).Append(Common.SOH);
            sb.Append(TAG_NoSide.ToString()).Append(Common.DELIMIT).Append(c_message.NoSide).Append(Common.SOH);
            //
            ReposSide itemSite;
            for (int i = 0; i < c_message.RepoSideList.Count; i++)
            {
                itemSite = c_message.RepoSideList[i];

                sb.Append(TAG_NumSide.ToString()).Append(Common.DELIMIT).Append(itemSite.NumSide).Append(Common.SOH);
                sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(itemSite.Symbol).Append(Common.SOH);
                sb.Append(TAG_OrderQty.ToString()).Append(Common.DELIMIT).Append(itemSite.OrderQty).Append(Common.SOH);
                sb.Append(TAG_Price.ToString()).Append(Common.DELIMIT).Append(itemSite.Price).Append(Common.SOH);
                sb.Append(TAG_HedgeRate.ToString()).Append(Common.DELIMIT).Append(itemSite.HedgeRate).Append(Common.SOH);

                sb.Append(TAG_SettlValue.ToString()).Append(Common.DELIMIT).Append(itemSite.SettlValue).Append(Common.SOH);
                sb.Append(TAG_SettlValue2.ToString()).Append(Common.DELIMIT).Append(itemSite.SettlValue2).Append(Common.SOH);
                sb.Append(TAG_ReposInterest.ToString()).Append(Common.DELIMIT).Append(itemSite.ReposInterest).Append(Common.SOH);
            }
        }

        int _numSide = 0;

        ReposSide _ReposSide = null;
        ReposSideList _ReposList = new ReposSideList();
        public static bool islast = false;

        public override bool ParseField(Field field)
        {
            if (TAG_ClOrdID == field.Tag)
                c_message.ClOrdID = field.Value;
            else if (TAG_RFQReqID == field.Tag)
                c_message.RFQReqID = field.Value;
            else if (TAG_QuoteType == field.Tag)
            {
                bool _isSuccess = false;
                c_message.QuoteType = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            //
            else if (TAG_OrdType == field.Tag)
                c_message.OrdType = field.Value;
            else if (TAG_Side == field.Tag)
            {
                bool _isSuccess = false;
                c_message.Side = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_Account == field.Tag)
                c_message.Account = field.Value;
            //
            else if (TAG_EffectiveTime == field.Tag)
                c_message.EffectiveTime = field.Value;
            else if (TAG_SettlMethod == field.Tag)
            {
                bool _isSuccess = false;
                c_message.SettlMethod = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_SettlDate == field.Tag)
                c_message.SettlDate = field.Value;
            //
            else if (TAG_SettlDate2 == field.Tag)
                c_message.SettlDate2 = field.Value;
            else if (TAG_EndDate == field.Tag)
                c_message.EndDate = field.Value;
            else if (TAG_RepurchaseTerm == field.Tag)
            {
                bool _isSuccess = false;
                c_message.RepurchaseTerm = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            //
            else if (TAG_RepurchaseRate == field.Tag)
            {
                bool _isSuccess = false;
                c_message.RepurchaseRate = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_NoSide == field.Tag)
            {
                bool _isSuccess = false;
                c_message.NoSide = Utils.Convert.ParseLong(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            //
            else if (TAG_NumSide == field.Tag)
            {
                _numSide++;
                if (_ReposSide != null)
                    _ReposList.Add(_ReposSide);

                _ReposSide = new ReposSide();

                bool _isSuccess = false;
                _ReposSide.NumSide = Utils.Convert.ParseLong(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_Symbol == field.Tag)
            {
                _ReposSide.Symbol = field.Value;
            }
            else if (TAG_OrderQty == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.OrderQty = Utils.Convert.ParseLong(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_Price == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.Price = Utils.Convert.ParseLong(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_HedgeRate == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.HedgeRate = Utils.Convert.ParseDouble(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_ReposInterest == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.ReposInterest = Utils.Convert.ParseLong(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_SettlValue == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.SettlValue = Utils.Convert.ParseLong(field.Value, ref _isSuccess);
                if (_isSuccess == false) return false;
            }
            else if (TAG_SettlValue2 == field.Tag)
            {
                bool _isSuccess = false;
                _ReposSide.SettlValue2 = Utils.Convert.ParseLong(field.Value, ref _isSuccess);
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
                if (_ReposSide != null)
                    _ReposList.Add(_ReposSide);
                c_message.RepoSideList = _ReposList;
            }
        }
    }
}
