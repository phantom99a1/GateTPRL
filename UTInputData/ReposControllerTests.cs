using APIServer;
using APIServer.Validation;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessAPIReq.ResponseModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UTInputData.Mock;
using static CommonLib.CommonData;

namespace UTInputData
{
	[TestClass()]
	public class ReposControllerTests
	{
		//
		private API13NewInquiryReposController c_API13NewInquiryReposController;

		private API13NewInquiryReposController c_API13NewInquiryReposController_E;

		//
		private API14ReplaceInquiryReposController c_API14ReplaceInquiryReposController;

		private API14ReplaceInquiryReposController c_API14ReplaceInquiryReposController_E;

		//
		private API15CancelInquiryReposController c_API15CancelInquiryReposController;

		private API15CancelInquiryReposController c_API15CancelInquiryReposController_E;

		//
		private API16CloseInquiryReposController c_API16CloseInquiryReposController;

		private API16CloseInquiryReposController c_API16CloseInquiryReposController_E;

		//
		private API17OrderNewFirmReposController c_API17OrderNewFirmReposController;

		private API17OrderNewFirmReposController c_API17OrderNewFirmReposController_E;

		//
		private API18OrderReplaceFirmReposController c_API18OrderReplaceFirmReposController;

		private API18OrderReplaceFirmReposController c_API18OrderReplaceFirmReposController_E;

		//
		private API19OrderCancelFirmReposController c_API19OrderCancelFirmReposController;

		private API19OrderCancelFirmReposController c_API19OrderCancelFirmReposController_E;

		//
		private API20OrderConfirmFirmReposController c_API20OrderConfirmFirmReposController;

		private API20OrderConfirmFirmReposController c_API20OrderConfirmFirmReposController_E;

		//
		private API21OrderNewReposCommonPutthoughController c_API21OrderNewReposCommonPutthoughController;

		private API21OrderNewReposCommonPutthoughController c_API21OrderNewReposCommonPutthoughController_E;

		//
		private API22OrderConfirmReposCommonPutthoughController c_API22OrderConfirmReposCommonPutthoughController;

		private API22OrderConfirmReposCommonPutthoughController c_API22OrderConfirmReposCommonPutthoughController_E;

		//
		private API23OrderReplaceReposCommonPutthroughController c_API23OrderReplaceReposCommonPutthroughController;

		private API23OrderReplaceReposCommonPutthroughController c_API23OrderReplaceReposCommonPutthroughController_E;

		//
		private API24OrderCancelReposCommonPutthroughController c_API24OrderCancelReposCommonPutthroughController;

		private API24OrderCancelReposCommonPutthroughController c_API24OrderCancelReposCommonPutthroughController_E;

		//
		private API25OrderReplaceDeal1stTransactionReposCommonPutthroughController c_API25OrderReplaceDeal1stTransactionReposCommonPutthroughController;

		private API25OrderReplaceDeal1stTransactionReposCommonPutthroughController c_API25OrderReplaceDeal1stTransactionReposCommonPutthroughController_E;

		//
		private API26OrderReplaceDeal1stTransactionReposCommonPutthroughController c_API26OrderReplaceDeal1stTransactionReposCommonPutthroughController;

		private API26OrderReplaceDeal1stTransactionReposCommonPutthroughController c_API26OrderReplaceDeal1stTransactionReposCommonPutthroughController_E;

		//
		private API27OrderCancelDeal1stTransactionReposCommonPutthroughController c_API27OrderCancelDeal1stTransactionReposCommonPutthroughController;

		private API27OrderCancelDeal1stTransactionReposCommonPutthroughController c_API27OrderCancelDeal1stTransactionReposCommonPutthroughController_E;

		//
		private API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughController c_API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughController;

		private API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughController c_API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughController_E;

		//
		private API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController c_API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController;

		private API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController c_API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController_E;

		//
		private API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughController c_API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughController;

		private API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughController c_API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughController_E;

		[TestInitialize]
		public void Initialize()
		{
			MockIProcessRevBussiness _MockIProcessRevBussiness = new MockIProcessRevBussiness();
			//
			API13NewInquiryReposValidator _API13NewInquiryReposValidator_Validator = new API13NewInquiryReposValidator();
			c_API13NewInquiryReposController = new API13NewInquiryReposController(_MockIProcessRevBussiness, _API13NewInquiryReposValidator_Validator);
			c_API13NewInquiryReposController_E = new API13NewInquiryReposController(null, _API13NewInquiryReposValidator_Validator);

			//
			API14ReplaceInquiryReposValidator _API14ReplaceInquiryReposValidator_Validator = new API14ReplaceInquiryReposValidator();
			c_API14ReplaceInquiryReposController = new API14ReplaceInquiryReposController(_MockIProcessRevBussiness, _API14ReplaceInquiryReposValidator_Validator);
			c_API14ReplaceInquiryReposController_E = new API14ReplaceInquiryReposController(null, _API14ReplaceInquiryReposValidator_Validator);

			//
			API15CancelInquiryReposValidator _API15CancelInquiryReposValidator_Validator = new API15CancelInquiryReposValidator();
			c_API15CancelInquiryReposController = new API15CancelInquiryReposController(_MockIProcessRevBussiness, _API15CancelInquiryReposValidator_Validator);
			c_API15CancelInquiryReposController_E = new API15CancelInquiryReposController(null, _API15CancelInquiryReposValidator_Validator);

			//
			API16CloseInquiryReposValidator _API16CloseInquiryReposValidator_Validator = new API16CloseInquiryReposValidator();
			c_API16CloseInquiryReposController = new API16CloseInquiryReposController(_MockIProcessRevBussiness, _API16CloseInquiryReposValidator_Validator);
			c_API16CloseInquiryReposController_E = new API16CloseInquiryReposController(null, _API16CloseInquiryReposValidator_Validator);
			//
			API17OrderNewFirmReposValidator _API17OrderNewFirmReposValidator_Validator = new API17OrderNewFirmReposValidator();
			c_API17OrderNewFirmReposController = new API17OrderNewFirmReposController(_MockIProcessRevBussiness, _API17OrderNewFirmReposValidator_Validator);
			c_API17OrderNewFirmReposController_E = new API17OrderNewFirmReposController(null, _API17OrderNewFirmReposValidator_Validator);
			//
			API18OrderReplaceFirmReposValidator _API18OrderReplaceFirmReposValidator_Validator = new API18OrderReplaceFirmReposValidator();
			c_API18OrderReplaceFirmReposController = new API18OrderReplaceFirmReposController(_MockIProcessRevBussiness, _API18OrderReplaceFirmReposValidator_Validator);
			c_API18OrderReplaceFirmReposController_E = new API18OrderReplaceFirmReposController(null, _API18OrderReplaceFirmReposValidator_Validator);
			//
			API19OrderCancelFirmReposValidator _API19OrderCancelFirmReposValidator_Validator = new API19OrderCancelFirmReposValidator();
			c_API19OrderCancelFirmReposController = new API19OrderCancelFirmReposController(_MockIProcessRevBussiness, _API19OrderCancelFirmReposValidator_Validator);
			c_API19OrderCancelFirmReposController_E = new API19OrderCancelFirmReposController(null, _API19OrderCancelFirmReposValidator_Validator);
			//
			API20OrderConfirmFirmReposValidator _API20OrderConfirmFirmReposValidator = new API20OrderConfirmFirmReposValidator();
			c_API20OrderConfirmFirmReposController = new API20OrderConfirmFirmReposController(_MockIProcessRevBussiness, _API20OrderConfirmFirmReposValidator);
			c_API20OrderConfirmFirmReposController_E = new API20OrderConfirmFirmReposController(null, _API20OrderConfirmFirmReposValidator);
			//
			API21OrderNewReposCommonPutthroughValidator _API21OrderNewReposCommonPutthroughValidator = new API21OrderNewReposCommonPutthroughValidator();
			c_API21OrderNewReposCommonPutthoughController = new API21OrderNewReposCommonPutthoughController(_MockIProcessRevBussiness, _API21OrderNewReposCommonPutthroughValidator);
			c_API21OrderNewReposCommonPutthoughController_E = new API21OrderNewReposCommonPutthoughController(null, _API21OrderNewReposCommonPutthroughValidator);

			//
			API22OrderConfirmReposCommonPutthroughValidator _API22OrderConfirmReposCommonPutthroughValidator = new API22OrderConfirmReposCommonPutthroughValidator();
			c_API22OrderConfirmReposCommonPutthoughController = new API22OrderConfirmReposCommonPutthoughController(_MockIProcessRevBussiness, _API22OrderConfirmReposCommonPutthroughValidator);
			c_API22OrderConfirmReposCommonPutthoughController_E = new API22OrderConfirmReposCommonPutthoughController(null, _API22OrderConfirmReposCommonPutthroughValidator);

			//
			API23OrderReplaceReposCommonPutthroughValidator _API23OrderReplaceReposCommonPutthroughValidator = new API23OrderReplaceReposCommonPutthroughValidator();
			c_API23OrderReplaceReposCommonPutthroughController = new API23OrderReplaceReposCommonPutthroughController(_MockIProcessRevBussiness, _API23OrderReplaceReposCommonPutthroughValidator);
			c_API23OrderReplaceReposCommonPutthroughController_E = new API23OrderReplaceReposCommonPutthroughController(null, _API23OrderReplaceReposCommonPutthroughValidator);

			//
			API24OrderCancelReposCommonPutthroughValidator _API24OrderCancelReposCommonPutthroughValidator = new API24OrderCancelReposCommonPutthroughValidator();
			c_API24OrderCancelReposCommonPutthroughController = new API24OrderCancelReposCommonPutthroughController(_MockIProcessRevBussiness, _API24OrderCancelReposCommonPutthroughValidator);
			c_API24OrderCancelReposCommonPutthroughController_E = new API24OrderCancelReposCommonPutthroughController(null, _API24OrderCancelReposCommonPutthroughValidator);

			//
			API25OrderReplaceDeal1stTransactionReposCommonPutthroughValidator _API25OrderReplaceDeal1stTransactionReposCommonPutthroughValidator = new API25OrderReplaceDeal1stTransactionReposCommonPutthroughValidator();
			c_API25OrderReplaceDeal1stTransactionReposCommonPutthroughController = new API25OrderReplaceDeal1stTransactionReposCommonPutthroughController(_MockIProcessRevBussiness, _API25OrderReplaceDeal1stTransactionReposCommonPutthroughValidator);
			c_API25OrderReplaceDeal1stTransactionReposCommonPutthroughController_E = new API25OrderReplaceDeal1stTransactionReposCommonPutthroughController(null, _API25OrderReplaceDeal1stTransactionReposCommonPutthroughValidator);

			//
			API26OrderReplaceDeal1stTransactionReposCommonPutthroughValidator _API26OrderReplaceDeal1stTransactionReposCommonPutthroughValidator = new API26OrderReplaceDeal1stTransactionReposCommonPutthroughValidator();
			c_API26OrderReplaceDeal1stTransactionReposCommonPutthroughController = new API26OrderReplaceDeal1stTransactionReposCommonPutthroughController(_MockIProcessRevBussiness, _API26OrderReplaceDeal1stTransactionReposCommonPutthroughValidator);
			c_API26OrderReplaceDeal1stTransactionReposCommonPutthroughController_E = new API26OrderReplaceDeal1stTransactionReposCommonPutthroughController(null, _API26OrderReplaceDeal1stTransactionReposCommonPutthroughValidator);

			//
			API27OrderCancelDeal1stTransactionReposCommonPutthroughValidator _API27OrderCancelDeal1stTransactionReposCommonPutthroughValidator = new API27OrderCancelDeal1stTransactionReposCommonPutthroughValidator();
			c_API27OrderCancelDeal1stTransactionReposCommonPutthroughController = new API27OrderCancelDeal1stTransactionReposCommonPutthroughController(_MockIProcessRevBussiness, _API27OrderCancelDeal1stTransactionReposCommonPutthroughValidator);
			c_API27OrderCancelDeal1stTransactionReposCommonPutthroughController_E = new API27OrderCancelDeal1stTransactionReposCommonPutthroughController(null, _API27OrderCancelDeal1stTransactionReposCommonPutthroughValidator);

			//
			API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughValidator _API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughValidator = new API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughValidator();
			c_API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughController = new API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughController(_MockIProcessRevBussiness, _API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughValidator);
			c_API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughController_E = new API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughController(null, _API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughValidator);

			//
			API29OrderReplaceDeal2ndTransactionReposCommonPutthroughValidator _API29OrderReplaceDeal2ndTransactionReposCommonPutthroughValidator = new API29OrderReplaceDeal2ndTransactionReposCommonPutthroughValidator();
			c_API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController = new API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController(_MockIProcessRevBussiness, _API29OrderReplaceDeal2ndTransactionReposCommonPutthroughValidator);
			c_API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController_E = new API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController(null, _API29OrderReplaceDeal2ndTransactionReposCommonPutthroughValidator);

			//
			API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughValidator _API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughValidator = new API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughValidator();
			c_API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughController = new API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughController(_MockIProcessRevBussiness, _API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughValidator);
			c_API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughController_E = new API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughController(null, _API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughValidator);
		}

		[TestMethod()]
		public void KhoitaoRequestTest()
		{
			//
			API13NewInquiryReposRequest _API13NewInquiryReposRequest = CBOTool<API13NewInquiryReposRequest>.CreateObjectFromType();
			CBOTool<API13NewInquiryReposRequest>.TestGetProperty(_API13NewInquiryReposRequest);
			API13NewInquiryReposResponse _API13NewInquiryReposResponse = CBOTool<API13NewInquiryReposResponse>.CreateObjectFromType();
			CBOTool<API13NewInquiryReposResponse>.TestGetProperty(_API13NewInquiryReposResponse);

			//
			API14ReplaceInquiryReposRequest _API14ReplaceInquiryReposRequest = CBOTool<API14ReplaceInquiryReposRequest>.CreateObjectFromType();
			CBOTool<API14ReplaceInquiryReposRequest>.TestGetProperty(_API14ReplaceInquiryReposRequest);
			API14ReplaceInquiryReposResponse _API14ReplaceInquiryReposResponse = CBOTool<API14ReplaceInquiryReposResponse>.CreateObjectFromType();
			CBOTool<API14ReplaceInquiryReposResponse>.TestGetProperty(_API14ReplaceInquiryReposResponse);

			//
			API15CancelInquiryReposRequest _API15CancelInquiryReposRequest = CBOTool<API15CancelInquiryReposRequest>.CreateObjectFromType();
			CBOTool<API15CancelInquiryReposRequest>.TestGetProperty(_API15CancelInquiryReposRequest);
			API15CancelInquiryReposResponse _API15CancelInquiryReposResponse = CBOTool<API15CancelInquiryReposResponse>.CreateObjectFromType();
			CBOTool<API15CancelInquiryReposResponse>.TestGetProperty(_API15CancelInquiryReposResponse);

			//
			API16CloseInquiryReposRequest _API16CloseInquiryReposRequest = CBOTool<API16CloseInquiryReposRequest>.CreateObjectFromType();
			CBOTool<API16CloseInquiryReposRequest>.TestGetProperty(_API16CloseInquiryReposRequest);
			API16CloseInquiryReposResponse _API16CloseInquiryReposResponse = CBOTool<API16CloseInquiryReposResponse>.CreateObjectFromType();
			CBOTool<API16CloseInquiryReposResponse>.TestGetProperty(_API16CloseInquiryReposResponse);

			//
			API17OrderNewFirmReposRequest _API17OrderNewFirmReposRequest = CBOTool<API17OrderNewFirmReposRequest>.CreateObjectFromType();
			CBOTool<API17OrderNewFirmReposRequest>.TestGetProperty(_API17OrderNewFirmReposRequest);
			API17OrderNewFirmReposResponse _API17OrderNewFirmReposResponse = CBOTool<API17OrderNewFirmReposResponse>.CreateObjectFromType();
			CBOTool<API17OrderNewFirmReposResponse>.TestGetProperty(_API17OrderNewFirmReposResponse);

			//
			API18OrderReplaceFirmReposRequest _API18OrderReplaceFirmReposRequest = CBOTool<API18OrderReplaceFirmReposRequest>.CreateObjectFromType();
			CBOTool<API18OrderReplaceFirmReposRequest>.TestGetProperty(_API18OrderReplaceFirmReposRequest);
			API18OrderReplaceFirmReposResponse _API18OrderReplaceFirmReposResponse = CBOTool<API18OrderReplaceFirmReposResponse>.CreateObjectFromType();
			CBOTool<API18OrderReplaceFirmReposResponse>.TestGetProperty(_API18OrderReplaceFirmReposResponse);
			//
			API19OrderCancelFirmReposRequest _API19OrderCancelFirmReposRequest = CBOTool<API19OrderCancelFirmReposRequest>.CreateObjectFromType();
			CBOTool<API19OrderCancelFirmReposRequest>.TestGetProperty(_API19OrderCancelFirmReposRequest);
			API19OrderCancelFirmReposResponse _API19OrderCancelFirmReposResponse = CBOTool<API19OrderCancelFirmReposResponse>.CreateObjectFromType();
			CBOTool<API19OrderCancelFirmReposResponse>.TestGetProperty(_API19OrderCancelFirmReposResponse);
			//
			API20OrderConfirmFirmReposRequest _API20OrderConfirmFirmReposRequest = CBOTool<API20OrderConfirmFirmReposRequest>.CreateObjectFromType();
			CBOTool<API20OrderConfirmFirmReposRequest>.TestGetProperty(_API20OrderConfirmFirmReposRequest);
			API20OrderConfirmFirmReposResponse _API20OrderConfirmFirmReposResponse = CBOTool<API20OrderConfirmFirmReposResponse>.CreateObjectFromType();
			CBOTool<API20OrderConfirmFirmReposResponse>.TestGetProperty(_API20OrderConfirmFirmReposResponse);
			//
			API21OrderNewReposCommonPutthroughRequest _API21OrderNewReposCommonPutthoughRequest = CBOTool<API21OrderNewReposCommonPutthroughRequest>.CreateObjectFromType();
			CBOTool<API21OrderNewReposCommonPutthroughRequest>.TestGetProperty(_API21OrderNewReposCommonPutthoughRequest);
			API21OrderNewReposCommonPutthroughResponse _API21OrderNewReposCommonPutthroughResponse = CBOTool<API21OrderNewReposCommonPutthroughResponse>.CreateObjectFromType();
			CBOTool<API21OrderNewReposCommonPutthroughResponse>.TestGetProperty(_API21OrderNewReposCommonPutthroughResponse);
			//
			API22OrderConfirmReposCommonPutthroughRequest _API22OrderConfirmReposCommonPutthroughRequest = CBOTool<API22OrderConfirmReposCommonPutthroughRequest>.CreateObjectFromType();
			CBOTool<API22OrderConfirmReposCommonPutthroughRequest>.TestGetProperty(_API22OrderConfirmReposCommonPutthroughRequest);
			API22OrderConfirmReposCommonPutthroughResponse _API22OrderConfirmReposCommonPutthroughResponse = CBOTool<API22OrderConfirmReposCommonPutthroughResponse>.CreateObjectFromType();
			CBOTool<API22OrderConfirmReposCommonPutthroughResponse>.TestGetProperty(_API22OrderConfirmReposCommonPutthroughResponse);
			//
			API23OrderReplaceReposCommonPutthroughRequest _API23OrderReplaceReposCommonPutthroughRequest = CBOTool<API23OrderReplaceReposCommonPutthroughRequest>.CreateObjectFromType();
			CBOTool<API23OrderReplaceReposCommonPutthroughRequest>.TestGetProperty(_API23OrderReplaceReposCommonPutthroughRequest);
			API23OrderReplaceReposCommonPutthroughResponse _API23OrderReplaceReposCommonPutthroughResponse = CBOTool<API23OrderReplaceReposCommonPutthroughResponse>.CreateObjectFromType();
			CBOTool<API23OrderReplaceReposCommonPutthroughResponse>.TestGetProperty(_API23OrderReplaceReposCommonPutthroughResponse);
			//
			API24OrderCancelReposCommonPutthroughRequest _API24OrderCancelReposCommonPutthroughRequest = CBOTool<API24OrderCancelReposCommonPutthroughRequest>.CreateObjectFromType();
			CBOTool<API24OrderCancelReposCommonPutthroughRequest>.TestGetProperty(_API24OrderCancelReposCommonPutthroughRequest);
			API24OrderCancelReposCommonPutthroughResponse _API24OrderCancelReposCommonPutthroughResponse = CBOTool<API24OrderCancelReposCommonPutthroughResponse>.CreateObjectFromType();
			CBOTool<API24OrderCancelReposCommonPutthroughResponse>.TestGetProperty(_API24OrderCancelReposCommonPutthroughResponse);
			//
			API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest _API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest = CBOTool<API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest>.CreateObjectFromType();
			CBOTool<API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest>.TestGetProperty(_API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest);
			API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse _API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse = CBOTool<API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse>.CreateObjectFromType();
			CBOTool<API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse>.TestGetProperty(_API25OrderReplaceDeal1stTransactionReposCommonPutthroughResponse);

			//
			API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest _API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest = CBOTool<API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest>.CreateObjectFromType();
			CBOTool<API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest>.TestGetProperty(_API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest);
			API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse _API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse = CBOTool<API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse>.CreateObjectFromType();
			CBOTool<API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse>.TestGetProperty(_API26OrderReplaceDeal1stTransactionReposCommonPutthroughResponse);

			//
			API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest _API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest = CBOTool<API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest>.CreateObjectFromType();
			CBOTool<API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest>.TestGetProperty(_API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest);
			API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse _API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse = CBOTool<API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse>.CreateObjectFromType();
			CBOTool<API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse>.TestGetProperty(_API27OrderCancelDeal1stTransactionReposCommonPutthroughResponse);

			//
			API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest _API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest = CBOTool<API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest>.CreateObjectFromType();
			CBOTool<API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest>.TestGetProperty(_API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest);
			API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse _API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse = CBOTool<API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse>.CreateObjectFromType();
			CBOTool<API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse>.TestGetProperty(_API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughResponse);

			//
			API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest _API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest = CBOTool<API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest>.CreateObjectFromType();
			CBOTool<API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest>.TestGetProperty(_API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest);
			API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse _API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse = CBOTool<API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse>.CreateObjectFromType();
			CBOTool<API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse>.TestGetProperty(_API29OrderReplaceDeal2ndTransactionReposCommonPutthroughResponse);

			//
			API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest _API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest = CBOTool<API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest>.CreateObjectFromType();
			CBOTool<API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest>.TestGetProperty(_API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest);
			API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse _API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse = CBOTool<API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse>.CreateObjectFromType();
			CBOTool<API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse>.TestGetProperty(_API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughResponse);

			Assert.IsTrue(true);
		}

		#region API13NewInquiryRepos

		[TestMethod()]
		public void API13NewInquiryRepos_Test()
		{
			//Arrange
			var request = new API13NewInquiryReposRequest()
			{
				OrderNo = "16",
				Symbol = "XDCR12101",
				QuoteType = 1,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = "B",
				OrderValue = 5,
				EffectiveTime = "20231024",
				SettleMethod = 1,
				SettleDate1 = "20231024",
				SettleDate2 = "20231024",
				EndDate = "20231024",
				RepurchaseTerm = 1,
				RegistID = "0",
				Text = "DAT LENH INQUIRY REPOS"
			};

			//Act
			var _return = c_API13NewInquiryReposController.API13NewInquiryRepos(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API13NewInquiryRepos_Invalid()
		{
			//Arrange
			var request = new API13NewInquiryReposRequest()
			{
				OrderNo = "",
				Symbol = "",
				QuoteType = 1,
				OrderType = "S",
				Side = "B",
				OrderValue = 5,
				EffectiveTime = "20231024",
				SettleMethod = 1,
				SettleDate1 = "20231024",
				SettleDate2 = "20231024",
				EndDate = "20231024",
				RepurchaseTerm = 1,
				RegistID = "0",
				Text = "DAT LENH INQUIRY REPOS"
			};

			//Act
			var _return = c_API13NewInquiryReposController.API13NewInquiryRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API13NewInquiryRepos_Invalid2()
		{
			//Arrange
			var request = new API13NewInquiryReposRequest()
			{
				OrderNo = "",
				Symbol = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Side = "B",
				OrderValue = 5,
				EffectiveTime = "20231024",
				SettleMethod = 1,
				SettleDate1 = "20231024",
				SettleDate2 = "20231024",
				EndDate = "20231024",
				RepurchaseTerm = 1,
				RegistID = "0",
				Text = "DAT LENH INQUIRY REPOS"
			};

			//Act
			var _return = c_API13NewInquiryReposController.API13NewInquiryRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API13NewInquiryRepos_Exception()
		{
			//Arrange
			var request = new API13NewInquiryReposRequest()
			{
				OrderNo = "16",
				Symbol = null
			};

			//Act
			var _return = c_API13NewInquiryReposController_E.API13NewInquiryRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API13NewInquiryRepos

		#region API14ReplaceInquiryRepos

		[TestMethod()]
		public void API14ReplaceInquiryRepos_Test()
		{
			//Arrange
			var request = new API14ReplaceInquiryReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				Symbol = "XDCR12101",
				QuoteType = QuoteType.LENH_SUA_INQUIRY,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = "B",
				OrderValue = 5,
				EffectiveTime = "20231024",
				SettleMethod = 1,
				SettleDate1 = "20231024",
				SettleDate2 = "20231024",
				EndDate = "20231024",
				RepurchaseTerm = 1,
				RegistID = "0",
				Text = "SUA LENH INQUIRY REPOS"
			};

			//Act
			var _return = c_API14ReplaceInquiryReposController.API14ReplaceInquiryRepos(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API14ReplaceInquiryRepos_Invalid()
		{
			//Arrange
			var request = new API14ReplaceInquiryReposRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
				Symbol = "",
				QuoteType = 1,
				OrderType = "S",
				Side = "B",
				OrderValue = 5,
				EffectiveTime = "20231024",
				SettleMethod = 1,
				SettleDate1 = "20231024",
				SettleDate2 = "20231024",
				EndDate = "20231024",
				RepurchaseTerm = 1,
				RegistID = "0",
				Text = "SUA LENH INQUIRY REPOS"
			};

			//Act
			var _return = c_API14ReplaceInquiryReposController.API14ReplaceInquiryRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API14ReplaceInquiryRepos_Invalid2()
		{
			//Arrange
			var request = new API14ReplaceInquiryReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				Symbol = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Side = "B",
				OrderValue = 5,
				EffectiveTime = "20231024",
				SettleMethod = 1,
				SettleDate1 = "20231024",
				SettleDate2 = "20231024",
				EndDate = "20231024",
				RepurchaseTerm = 1,
				RegistID = "0",
				Text = "SUA LENH INQUIRY REPOS"
			};

			//Act
			var _return = c_API14ReplaceInquiryReposController.API14ReplaceInquiryRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API14ReplaceInquiryRepos_Exception()
		{
			//Arrange
			var request = new API14ReplaceInquiryReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API14ReplaceInquiryReposController_E.API14ReplaceInquiryRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API14ReplaceInquiryRepos

		#region API15CancelInquiryRepos

		[TestMethod()]
		public void API15CancelInquiryRepos_Test()
		{
			//Arrange
			var request = new API15CancelInquiryReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				Symbol = "XDCR12101",
				QuoteType = QuoteType.LENH_HUY_INQUIRY,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Text = "HUY LENH INQUIRY REPOS"
			};

			//Act
			var _return = c_API15CancelInquiryReposController.API15CancelInquiryRepos(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API15CancelInquiryRepos_Invalid()
		{
			//Arrange
			var request = new API15CancelInquiryReposRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
				Symbol = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "HUY LENH INQUIRY REPOS"
			};

			//Act
			var _return = c_API15CancelInquiryReposController.API15CancelInquiryRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API15CancelInquiryRepos_Invalid2()
		{
			//Arrange
			var request = new API15CancelInquiryReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				Symbol = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "HUY LENH INQUIRY REPOS"
			};

			//Act
			var _return = c_API15CancelInquiryReposController.API15CancelInquiryRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API15CancelInquiryRepos_Exception()
		{
			//Arrange
			var request = new API15CancelInquiryReposRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API15CancelInquiryReposController_E.API15CancelInquiryRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API15CancelInquiryRepos

		#region API16CloseInquiryRepos

		[TestMethod()]
		public void API16CloseInquiryRepos_Test()
		{
			//Arrange
			var request = new API16CloseInquiryReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				Symbol = "XDCR12101",
				QuoteType = QuoteType.LENH_DONG_INQUIRY,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Text = "HUY DONG INQUIRY REPOS"
			};

			//Act
			var _return = c_API16CloseInquiryReposController.API16CloseInquiryRepos(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API16CloseInquiryRepos_Invalid()
		{
			//Arrange
			var request = new API16CloseInquiryReposRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
				Symbol = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "HUY DONG INQUIRY REPOS"
			};

			//Act
			var _return = c_API16CloseInquiryReposController.API16CloseInquiryRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API16CloseInquiryRepos_Invalid2()
		{
			//Arrange
			var request = new API16CloseInquiryReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				Symbol = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "HUY DONG INQUIRY REPOS"
			};

			//Act
			var _return = c_API16CloseInquiryReposController.API16CloseInquiryRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API16CloseInquiryRepos_Exception()
		{
			//Arrange
			var request = new API16CloseInquiryReposRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API16CloseInquiryReposController_E.API16CloseInquiryRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API16CloseInquiryRepos

		#region API17OrderNewFirmRepos

		[TestMethod()]
		public void API17OrderNewFirmRepos_Test()
		{
			//Arrange
			var request = new API17OrderNewFirmReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteTypeRepos.LENH_DAT,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = ORDER_SIDE.SIDE_BUY,
				ClientID = "ClientID_API17",
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_NOW,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST DAT LENH FIRM REPOS"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "SYMBOL_API17";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//Act
			var _return = c_API17OrderNewFirmReposController.API17OrderNewFirmRepos(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API17OrderNewFirmRepos_Invalid()
		{
			//Arrange
			var request = new API17OrderNewFirmReposRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Text = "TEST DAT LENH FIRM REPOS"
			};

			//Act
			var _return = c_API17OrderNewFirmReposController.API17OrderNewFirmRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API17OrderNewFirmRepos_Invalid2()
		{
			//Arrange
			var request = new API17OrderNewFirmReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "HUY DONG INQUIRY REPOS"
			};

			//Act
			var _return = c_API17OrderNewFirmReposController.API17OrderNewFirmRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API17OrderNewFirmRepos_Exception()
		{
			//Arrange
			var request = new API17OrderNewFirmReposRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API17OrderNewFirmReposController_E.API17OrderNewFirmRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API17OrderNewFirmRepos

		#region API18OrderReplaceFirmRepos

		[TestMethod()]
		public void API18OrderReplaceFirmRepos_Test()
		{
			//Arrange
			var request = new API18OrderReplaceFirmReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteTypeRepos.LENH_SUA,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = ORDER_SIDE.SIDE_BUY,
				ClientID = "ClientID_API18",
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_NOW,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST DAT LENH FIRM REPOS"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "SYMBOL_API18";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//Act
			var _return = c_API18OrderReplaceFirmReposController.API18OrderReplaceFirmRepos(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API18OrderReplaceFirmRepos_Invalid()
		{
			//Arrange
			var request = new API18OrderReplaceFirmReposRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Text = "TEST SUA LENH FIRM REPOS"
			};

			//Act
			var _return = c_API18OrderReplaceFirmReposController.API18OrderReplaceFirmRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API18OrderReplaceFirmRepos_Invalid2()
		{
			//Arrange
			var request = new API18OrderReplaceFirmReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "HUY DONG INQUIRY REPOS"
			};

			//Act
			var _return = c_API18OrderReplaceFirmReposController.API18OrderReplaceFirmRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API18OrderReplaceFirmRepos_Exception()
		{
			//Arrange
			var request = new API18OrderReplaceFirmReposRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API18OrderReplaceFirmReposController_E.API18OrderReplaceFirmRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API18OrderReplaceFirmRepos

		#region API19OrderCancelFirmRepos

		[TestMethod()]
		public void API19OrderCancelFirmRepos_Test()
		{
			//Arrange
			var request = new API19OrderCancelFirmReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteTypeRepos.LENH_HUY,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Text = "TEST SUA LENH FIRM REPOS"
			};

			//Act
			var _return = c_API19OrderCancelFirmReposController.API19OrderCancelFirmRepos(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API19OrderCancelFirmRepos_Invalid()
		{
			//Arrange
			var request = new API19OrderCancelFirmReposRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Text = "TEST SUA LENH FIRM REPOS"
			};

			//Act
			var _return = c_API19OrderCancelFirmReposController.API19OrderCancelFirmRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API19OrderCancelFirmRepos_Invalid2()
		{
			//Arrange
			var request = new API19OrderCancelFirmReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "HUY HUY INQUIRY REPOS"
			};

			//Act
			var _return = c_API19OrderCancelFirmReposController.API19OrderCancelFirmRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API19OrderCancelFirmRepos_Exception()
		{
			//Arrange
			var request = new API19OrderCancelFirmReposRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API19OrderCancelFirmReposController_E.API19OrderCancelFirmRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API19OrderCancelFirmRepos

		#region API20OrderConfirmFirmRepos

		[TestMethod()]
		public void API20OrderConfirmFirmRepos_Test()
		{
			//Arrange
			var request = new API20OrderConfirmFirmReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteTypeRepos.LENH_DAT,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				ClientID = "ClientID API20",
				ClientIDCounterFirm = "ClientIDCounterFirm API20",
                RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST CONFIRM LENH FIRM REPOS"
			};

			//Act
			var _return = c_API20OrderConfirmFirmReposController.API20OrderConfirmFirmRepos(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API20OrderConfirmFirmRepos_Invalid()
		{
			//Arrange
			var request = new API20OrderConfirmFirmReposRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Text = "TEST CONFIRM LENH FIRM REPOS"
			};

			//Act
			var _return = c_API20OrderConfirmFirmReposController.API20OrderConfirmFirmRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API20OrderConfirmFirmRepos_Invalid2()
		{
			//Arrange
			var request = new API20OrderConfirmFirmReposRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "CONFIRM FIRM REPOS"
			};

			//Act
			var _return = c_API20OrderConfirmFirmReposController.API20OrderConfirmFirmRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API20OrderConfirmFirmRepos_Exception()
		{
			//Arrange
			var request = new API20OrderConfirmFirmReposRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API20OrderConfirmFirmReposController_E.API20OrderConfirmFirmRepos(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API20OrderConfirmFirmRepos

		#region API21OrderNewReposCommonPutthough

		[TestMethod()]
		public void API21OrderNewReposCommonPutthough_Test()
		{
			//Arrange
			var request = new API21OrderNewReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				QuoteType = QuoteTypeRepos.LENH_DAT,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Side = ORDER_SIDE.SIDE_BUY,
				ClientID = "ClientID_API21",
				MemberCounterFirm = "MemberCounterFirm_API21",
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_NOW,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 21"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "SYMBOL_API21";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//Act
			var _return = c_API21OrderNewReposCommonPutthoughController.API21OrderNewReposCommonPutthough(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API21OrderNewReposCommonPutthough_Invalid()
		{
			//Arrange
			var request = new API21OrderNewReposCommonPutthroughRequest()
			{
				OrderNo = "",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Text = "TEST BCGD_REPOS"
			};

			//Act
			var _return = c_API21OrderNewReposCommonPutthoughController.API21OrderNewReposCommonPutthough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API21OrderNewReposCommonPutthough_Invalid2()
		{
			//Arrange
			var request = new API21OrderNewReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				QuoteType = 1,
				OrderType = "S",
				Text = "CONFIRM FIRM REPOS"
			};

			//Act
			var _return = c_API21OrderNewReposCommonPutthoughController.API21OrderNewReposCommonPutthough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API21OrderNewReposCommonPutthough_Exception()
		{
			//Arrange
			var request = new API21OrderNewReposCommonPutthroughRequest()
			{
				OrderNo = ""
			};

			//Act
			var _return = c_API21OrderNewReposCommonPutthoughController_E.API21OrderNewReposCommonPutthough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API21OrderNewReposCommonPutthough

		#region API22OrderConfirmReposCommonPutthrough

		[TestMethod()]
		public void API22OrderConfirmReposCommonPutthrough_Test()
		{
			//Arrange
			var request = new API22OrderConfirmReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteType_BCGDRepos.LENH_XAC_NHAN_BCGD_REPOS,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Side = ORDER_SIDE.SIDE_BUY,
				ClientID = "ClientID_API22",
				ClientIDCounterFirm = "ClientIDCounterFirm_API22",
				MemberCounterFirm = "MemberCounterFirm_API22",
                EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_NOW,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 21"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "SYMBOL_API22";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//Act
			var _return = c_API22OrderConfirmReposCommonPutthoughController.API22OrderConfirmReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API22OrderConfirmReposCommonPutthrough_Invalid()
		{
			//Arrange
			var request = new API22OrderConfirmReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Text = "TEST BCGD_REPOS"
			};

			//Act
			var _return = c_API22OrderConfirmReposCommonPutthoughController.API22OrderConfirmReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API22OrderConfirmReposCommonPutthrough_Invalid2()
		{
			//Arrange
			var request = new API22OrderConfirmReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "CONFIRM FIRM REPOS"
			};

			//Act
			var _return = c_API22OrderConfirmReposCommonPutthoughController.API22OrderConfirmReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API22OrderConfirmReposCommonPutthrough_Exception()
		{
			//Arrange
			var request = new API22OrderConfirmReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API22OrderConfirmReposCommonPutthoughController_E.API22OrderConfirmReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API22OrderConfirmReposCommonPutthrough

		#region API23OrderReplaceReposCommonPutthrough

		[TestMethod()]
		public void API23OrderReplaceReposCommonPutthrough_Test()
		{
			//Arrange
			var request = new API23OrderReplaceReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteType_BCGDRepos.LENH_SUA_BCGD_REPOS,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Side = ORDER_SIDE.SIDE_BUY,
				ClientID = "ClientID_API23",
				MemberCounterFirm = "MemberCounterFirm_API23",
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_NOW,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 21"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "SYMBOL_API23";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//Act
			var _return = c_API23OrderReplaceReposCommonPutthroughController.API23OrderReplaceReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API23OrderReplaceReposCommonPutthrough_Invalid()
		{
			//Arrange
			var request = new API23OrderReplaceReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Text = "TEST BCGD_REPOS"
			};

			//Act
			var _return = c_API23OrderReplaceReposCommonPutthroughController.API23OrderReplaceReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API23OrderReplaceReposCommonPutthrough_Invalid2()
		{
			//Arrange
			var request = new API23OrderReplaceReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "CONFIRM FIRM REPOS"
			};

			//Act
			var _return = c_API23OrderReplaceReposCommonPutthroughController.API23OrderReplaceReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API23OrderReplaceReposCommonPutthrough_Exception()
		{
			//Arrange
			var request = new API23OrderReplaceReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API23OrderReplaceReposCommonPutthroughController_E.API23OrderReplaceReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API23OrderReplaceReposCommonPutthrough

		#region API24OrderCancelReposCommonPutthrough

		[TestMethod()]
		public void API24OrderCancelReposCommonPutthrough_Test()
		{
			//Arrange
			var request = new API24OrderCancelReposCommonPutthroughRequest()
			{
				OrderNo = "NO_24",
				RefExchangeID = "RefExchangeID_24",
				QuoteType = QuoteTypeRepos.LENH_DAT,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Side = ORDER_SIDE.SIDE_BUY
			};

			//Act
			var _return = c_API24OrderCancelReposCommonPutthroughController.API24OrderCancelReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API24OrderCancelReposCommonPutthrough_Invalid()
		{
			//Arrange
			var request = new API24OrderCancelReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Text = "TEST BCGD_REPOS"
			};

			//Act
			var _return = c_API24OrderCancelReposCommonPutthroughController.API24OrderCancelReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API24OrderCancelReposCommonPutthrough_Invalid2()
		{
			//Arrange
			var request = new API24OrderCancelReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "CONFIRM FIRM REPOS"
			};

			//Act
			var _return = c_API24OrderCancelReposCommonPutthroughController.API24OrderCancelReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API24OrderCancelReposCommonPutthrough_Exception()
		{
			//Arrange
			var request = new API24OrderCancelReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API24OrderCancelReposCommonPutthroughController_E.API24OrderCancelReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API24OrderCancelReposCommonPutthrough

		#region API25OrderReplaceDeal1stTransactionReposCommonPutthrough

		[TestMethod()]
		public void API25OrderReplaceDeal1stTransactionReposCommonPutthrough_Test()
		{
			//Arrange
			var request = new API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteType_BCGDRepos.LENH_SUA_THOATHUAN_DA_THUC_HIEN_REPOS_TRONG_NGAY,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Side = ORDER_SIDE.SIDE_BUY,
				ClientID = "ClientID_API25",
				MemberCounterFirm = "MemberCounterFirm_API25",
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_LASTDATE,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 25"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "SYMBOL_API25";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//Act
			var _return = c_API25OrderReplaceDeal1stTransactionReposCommonPutthroughController.API25OrderReplaceDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API25OrderReplaceDeal1stTransactionReposCommonPutthrough_Invalid()
		{
			//Arrange
			var request = new API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Text = "TEST BCGD_REPOS"
			};

			//Act
			var _return = c_API25OrderReplaceDeal1stTransactionReposCommonPutthroughController.API25OrderReplaceDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API25OrderReplaceDeal1stTransactionReposCommonPutthrough_Invalid2()
		{
			//Arrange
			var request = new API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "CONFIRM FIRM REPOS"
			};

			//Act
			var _return = c_API25OrderReplaceDeal1stTransactionReposCommonPutthroughController.API25OrderReplaceDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API25OrderReplaceDeal1stTransactionReposCommonPutthrough_Exception()
		{
			//Arrange
			var request = new API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API25OrderReplaceDeal1stTransactionReposCommonPutthroughController_E.API25OrderReplaceDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API25OrderReplaceDeal1stTransactionReposCommonPutthrough

		#region API26OrderReplaceDeal1stTransactionReposCommonPutthrough

		[TestMethod()]
		public void API26OrderReplaceDeal1stTransactionReposCommonPutthrough_Test()
		{
			//Arrange
			var request = new API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteType_BCGDRepos.LENH_XAC_NHAN_BCGD_REPOS,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Side = ORDER_SIDE.SIDE_BUY,
				ClientID = "ClientID_API26",
				MemberCounterFirm = "MemberCounterFirm_API26",
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_LASTDATE,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 26"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "SYMBOL_API26";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//Act
			var _return = c_API26OrderReplaceDeal1stTransactionReposCommonPutthroughController.API26OrderReplaceDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API26OrderReplaceDeal1stTransactionReposCommonPutthrough_Invalid()
		{
			//Arrange
			var request = new API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Text = "TEST BCGD_REPOS"
			};

			//Act
			var _return = c_API26OrderReplaceDeal1stTransactionReposCommonPutthroughController.API26OrderReplaceDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API26OrderReplaceDeal1stTransactionReposCommonPutthrough_Invalid2()
		{
			//Arrange
			var request = new API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "CONFIRM FIRM REPOS"
			};

			//Act
			var _return = c_API26OrderReplaceDeal1stTransactionReposCommonPutthroughController.API26OrderReplaceDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API26OrderReplaceDeal1stTransactionReposCommonPutthrough_Exception()
		{
			//Arrange
			var request = new API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API26OrderReplaceDeal1stTransactionReposCommonPutthroughController_E.API26OrderReplaceDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API26OrderReplaceDeal1stTransactionReposCommonPutthrough

		#region API27OrderCancelDeal1stTransactionReposCommonPutthrough

		[TestMethod()]
		public void API27OrderCancelDeal1stTransactionReposCommonPutthrough_Test()
		{
			//Arrange
			var request = new API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "N0_27",
				RefExchangeID = "RefExchangeID_27",
				QuoteType = QuoteTypeRepos.LENH_SUA,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = ORDER_SIDE.SIDE_BUY
			};

			//Act
			var _return = c_API27OrderCancelDeal1stTransactionReposCommonPutthroughController.API27OrderCancelDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API27OrderCancelDeal1stTransactionReposCommonPutthrough_Invalid()
		{
			//Arrange
			var request = new API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Text = "TEST BCGD_REPOS"
			};

			//Act
			var _return = c_API27OrderCancelDeal1stTransactionReposCommonPutthroughController.API27OrderCancelDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API27OrderCancelDeal1stTransactionReposCommonPutthrough_Invalid2()
		{
			//Arrange
			var request = new API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "CONFIRM FIRM REPOS"
			};

			//Act
			var _return = c_API27OrderCancelDeal1stTransactionReposCommonPutthroughController.API27OrderCancelDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API27OrderCancelDeal1stTransactionReposCommonPutthrough_Exception()
		{
			//Arrange
			var request = new API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API27OrderCancelDeal1stTransactionReposCommonPutthroughController_E.API27OrderCancelDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API27OrderCancelDeal1stTransactionReposCommonPutthrough

		#region API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough

		[TestMethod()]
		public void API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough_Test()
		{
			//Arrange
			var request = new API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "NO_28",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteTypeRepos.LENH_HUY,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = ORDER_SIDE.SIDE_BUY
			};

			//Act
			var _return = c_API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughController.API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough_Invalid()
		{
			//Arrange
			var request = new API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Text = "TEST BCGD_REPOS"
			};

			//Act
			var _return = c_API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughController.API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough_Invalid2()
		{
			//Arrange
			var request = new API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "CONFIRM FIRM REPOS"
			};

			//Act
			var _return = c_API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughController.API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough_Exception()
		{
			//Arrange
			var request = new API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughController_E.API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API28OrderConfirmCancelDeal1stTransactionReposCommonPutthrough

		#region API29OrderReplaceDeal2ndTransactionReposCommonPutthrough

		[TestMethod()]
		public void API29OrderReplaceDeal2ndTransactionReposCommonPutthrough_Test()
		{
			//Arrange
			var request = new API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteType_BCGDRepos.LENH_SUA_THOATHUAN_DA_THUC_HIEN_REPOS,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = ORDER_SIDE.SIDE_BUY,
				ClientID = "ClientID_API29",
				MemberCounterFirm = "MemberCounterFirm_API26",
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_LASTDATE,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 29"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "SYMBOL_API29";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//Act
			var _return = c_API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController.API29OrderReplaceDeal2ndTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API29OrderReplaceDeal2ndTransactionReposCommonPutthrough_Invalid()
		{
			//Arrange
			var request = new API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Text = "TEST BCGD_REPOS"
			};

			//Act
			var _return = c_API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController.API29OrderReplaceDeal2ndTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API29OrderReplaceDeal2ndTransactionReposCommonPutthrough_Invalid2()
		{
			//Arrange
			var request = new API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "CONFIRM FIRM REPOS"
			};

			//Act
			var _return = c_API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController.API29OrderReplaceDeal2ndTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API29OrderReplaceDeal2ndTransactionReposCommonPutthrough_Exception()
		{
			//Arrange
			var request = new API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API29OrderReplaceDeal2ndTransactionReposCommonPutthroughController_E.API29OrderReplaceDeal2ndTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API29OrderReplaceDeal2ndTransactionReposCommonPutthrough

		#region API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough

		[TestMethod()]
		public void API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough_Test()
		{
			//Arrange
			var request = new API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = QuoteType_BCGDRepos.CHAP_NHAN_SUA_THOATHUAN_DA_THUC_HIEN_REPOS,
				OrderType = ORDER_ORDERTYPE.DIEN_TU_TUY_CHON_REPO,
				Side = ORDER_SIDE.SIDE_BUY,
				ClientID = "ClientID_API30",
				MemberCounterFirm = "MemberCounterFirm_API30",
				EffectiveTime = DateTime.Now.ToString("yyyyMMdd"),
				SettleMethod = ORDER_SETTLMETHOD.PAYMENT_LASTDATE,
				SettleDate1 = DateTime.Now.ToString("yyyyMMdd"),
				SettleDate2 = DateTime.Now.ToString("yyyyMMdd"),
				EndDate = DateTime.Now.ToString("yyyyMMdd"),
				RepurchaseTerm = 1,
				RepurchaseRate = 1,
				NoSide = 1,
				Text = "TEST API 30"
			};

			//
			APIReposSideList _APIReposSideList = new APIReposSideList();
			_APIReposSideList.NumSide = 1;
			_APIReposSideList.Symbol = "SYMBOL_API30";
			_APIReposSideList.OrderQty = 1000;
			_APIReposSideList.MergePrice = 1;
			_APIReposSideList.HedgeRate = 1;
			//
			var _SymbolFirmInfo = new List<APIReposSideList>();
			_SymbolFirmInfo.Add(_APIReposSideList);
			request.SymbolFirmInfo = _SymbolFirmInfo;
			//Act
			var _return = c_API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughController.API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough_Invalid()
		{
			//Arrange
			var request = new API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "XDCR12101",
				QuoteType = 6,
				OrderType = ORDER_ORDERTYPE.BCGD_REPOS,
				Text = "TEST BCGD_REPOS"
			};

			//Act
			var _return = c_API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughController.API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough_Invalid2()
		{
			//Arrange
			var request = new API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "16",
				RefExchangeID = "XDCR12101",
				QuoteType = 1,
				OrderType = "S",
				Text = "CONFIRM FIRM REPOS"
			};

			//Act
			var _return = c_API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughController.API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		[TestMethod()]
		public void API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough_Exception()
		{
			//Arrange
			var request = new API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest()
			{
				OrderNo = "",
				RefExchangeID = "",
			};

			//Act
			var _return = c_API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughController_E.API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough(request).Result;

			//Assert
			Assert.AreNotEqual("000", _return.ReturnCode);
		}

		#endregion API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthrough
	}
}