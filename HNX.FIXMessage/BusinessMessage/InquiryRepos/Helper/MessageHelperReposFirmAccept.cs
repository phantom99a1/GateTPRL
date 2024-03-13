using System.Text;

namespace HNX.FIXMessage
{
	public class MessageHelperReposFirmAccept : MessageHelper
	{
		private MessageReposFirmAccept c_message;

		#region #Tag const

		private const int TAG_ClOrdID = 11;
		private const int TAG_RFQReqID = 644;
		private const int TAG_QuoteType = 537;
		private const int TAG_OrdType = 40;
		private const int TAG_Account = 1;
		private const int TAG_CoAccount = 2;

        private const int TAG_RepurchaseRate = 227;
		private const int TAG_NoSide = 552;

		// Thông tin phần lặp
		private const int TAG_NumSide = 5522;

		private const int TAG_Symbol = 55;
		private const int TAG_OrderQty = 38;
		private const int TAG_Price = 44;
		private const int TAG_HedgeRate = 2260;
		private const int TAG_ReposInterest = 2261;

		#endregion #Tag const

		public MessageHelperReposFirmAccept(FIXMessageBase message)
			: base(message)
		{
			c_message = (MessageReposFirmAccept)message;
		}

		public override void BuildBody(StringBuilder sb)
		{
			base.BuildBody(sb);

			sb.Append(TAG_ClOrdID.ToString()).Append(Common.DELIMIT).Append(c_message.ClOrdID).Append(Common.SOH);
			sb.Append(TAG_RFQReqID.ToString()).Append(Common.DELIMIT).Append(c_message.RFQReqID).Append(Common.SOH);
			sb.Append(TAG_QuoteType.ToString()).Append(Common.DELIMIT).Append(c_message.QuoteType).Append(Common.SOH);
			sb.Append(TAG_OrdType.ToString()).Append(Common.DELIMIT).Append(c_message.OrdType).Append(Common.SOH);
			sb.Append(TAG_Account.ToString()).Append(Common.DELIMIT).Append(c_message.Account).Append(Common.SOH);
			sb.Append(TAG_CoAccount.ToString()).Append(Common.DELIMIT).Append(c_message.CoAccount).Append(Common.SOH);
			sb.Append(TAG_RepurchaseRate.ToString()).Append(Common.DELIMIT).Append(c_message.RepurchaseRate).Append(Common.SOH);
			sb.Append(TAG_NoSide.ToString()).Append(Common.DELIMIT).Append(c_message.NoSide).Append(Common.SOH);

			ReposSide itemSite;
			for (int i = 0; i < c_message.RepoSideList.Count; i++)
			{
				itemSite = c_message.RepoSideList[i];
				//
				sb.Append(TAG_NumSide).Append(Common.DELIMIT).Append(itemSite.NumSide).Append(Common.SOH);
				sb.Append(TAG_Symbol.ToString()).Append(Common.DELIMIT).Append(itemSite.Symbol).Append(Common.SOH);
				sb.Append(TAG_OrderQty.ToString()).Append(Common.DELIMIT).Append(itemSite.OrderQty).Append(Common.SOH);
				sb.Append(TAG_Price.ToString()).Append(Common.DELIMIT).Append(itemSite.Price).Append(Common.SOH);
				sb.Append(TAG_HedgeRate.ToString()).Append(Common.DELIMIT).Append(itemSite.HedgeRate).Append(Common.SOH);
				sb.Append(TAG_ReposInterest.ToString()).Append(Common.DELIMIT).Append(itemSite.ReposInterest).Append(Common.SOH);
			}
		}

		private int _numSide = 0;

		private ReposSide _ReposSide = null;
		private ReposSideList _ReposList = new ReposSideList();
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
			else if (TAG_OrdType == field.Tag)
			{
				if (field.Value.Length > 0)
					c_message.OrdType = field.Value;
			}
			else if (TAG_Account == field.Tag)
				c_message.Account = field.Value;
            else if (TAG_CoAccount == field.Tag)
                c_message.CoAccount = field.Value;
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
				_ReposSide.NumSide = Utils.Convert.ParseInt(field.Value, ref _isSuccess);
				if (_isSuccess == false) return false;
			}
			else if (TAG_Symbol == field.Tag)
			{
				_ReposSide.Symbol = field.Value;
				//chõ này gắn cho Symbol trên base nữa để thằng InfoEngine dùng
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
			else if (base.ParseField(field) == false) return false;

			return true;
		}

		public override void AddReposSide()
		{
			base.AddReposSide();
			if (islast && _numSide == c_message.NoSide)
			{
				_ReposList.Add(_ReposSide);
				c_message.RepoSideList = _ReposList;
			}
		}
	}
}