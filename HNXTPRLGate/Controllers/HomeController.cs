﻿using APIMonitor.Helpers;
using APIMonitor.Models;
using APIMonitor.ObjectInfo;
using BusinessProcessResponse;
using CommonLib;
using HNXInterface;
using HNXTPRLGate.Helpers;
using LocalMemory;
using LogStation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using static CommonLib.CommonDataInCore;
using static CommonLib.ConfigData;

namespace HNXTPRLGate.Controllers
{
    public class HomeController : Controller
	{
		public iHNXClient c_iHNXClient;
		public IResponseInterface c_iKafkaInterface;

		public HomeController(iHNXClient _iHNXEntity, IResponseInterface _iKafkaInterface)
		{
			c_iHNXClient = _iHNXEntity;
			c_iKafkaInterface = _iKafkaInterface;
		}

		[AuthActionFilter]
		public IActionResult Index()
	{
			BoxConnectModel _boxConnect = new BoxConnectModel();
			try
			{
				HttpContext.Session.Remove("SearchModel");
				HttpContext.Session.Remove("SymbolID");
				var client = new RestClient(APIMonitorDomain + ":" + APIMonitorPort);
				var request = new RestRequest("api/ApiMonitor/get-boxconnect-info", Method.Get);				
				var response = client.Execute(request);                
                _boxConnect = JsonConvert.DeserializeObject<BoxConnectModel>(response?.Content ?? "");
                var clientLogError = new RestClient(APIMonitorDomain + ":" + APIMonitorPort);
                var requestLog = new RestRequest("api/ApiMonitor/GetApplicationError", Method.Get);
                var responseLog = clientLogError.Execute(requestLog);
				var applicationLog = JsonConvert.DeserializeObject<ApplicationErrorModel>(responseLog?.Content ?? "");				
				if (_boxConnect != null)
				{
					if(_boxConnect.DataMem != null)
					{
						var listAllMsgRejectOnMemory = _boxConnect?.DataMem?.ListAllMsgRejectOnMemory;
						var listSearchSecurities = _boxConnect?.DataMem?.ListSearchSecurities;

						int? countPageIndexMaxRejection = listAllMsgRejectOnMemory?.Count;
                        int? countPageIndexSecurities = listSearchSecurities?.Count;
                        _boxConnect.DataMem.PageIndexMaxRejection = (countPageIndexMaxRejection % RecordInPage == 0) ? countPageIndexMaxRejection / RecordInPage : countPageIndexMaxRejection / RecordInPage + 1;
                        _boxConnect.DataMem.PageIndexMaxSecurities = (countPageIndexSecurities % RecordInPage == 0)
                            ? countPageIndexSecurities / RecordInPage : countPageIndexSecurities / RecordInPage + 1;
                        _boxConnect.DataMem.PageIndexRejection = _boxConnect.DataMem.PageIndexMaxRejection >= 1 ? 1 : 0;
                        _boxConnect.DataMem.PageIndexSecurities = _boxConnect.DataMem.PageIndexMaxSecurities >= 1 ? 1 : 0;

						_boxConnect.DataMem.ListAllMsgRejectOnMemory = listAllMsgRejectOnMemory?.OrderByDescending(item => item.TimeRecv).ToList() ?? new();
						_boxConnect.DataMem.ListDisplayMsgRejectOnMemory = listAllMsgRejectOnMemory?.OrderByDescending(item => item.TimeRecv)
							.Skip(0).Take(RecordInPage).ToList() ?? new();
						_boxConnect.DataMem.ListSearchSecurities = listSearchSecurities?.OrderBy(item => item.Symbol).ToList() ?? new();
						_boxConnect.DataMem.ListDisplaySecurities = listSearchSecurities?.OrderBy(item => item.Symbol)
							.Skip(0).Take(RecordInPage).ToList() ?? new();
                    }
                    int? countPageIndexMaxError = applicationLog?.ListAllErrors?.Count;
					_boxConnect.ApplicationError = applicationLog ?? new();
                    _boxConnect.ApplicationError.ListAllErrors = applicationLog?.ListAllErrors ?? new();
					_boxConnect.ApplicationError.ListDisplayErrors = applicationLog?.ListAllErrors.Skip(0).Take(RecordInPage).ToList() ?? new();
					_boxConnect.ApplicationError.PageIndexMaxpplicationError = (countPageIndexMaxError % RecordInPage == 0) ? countPageIndexMaxError / RecordInPage : countPageIndexMaxError /RecordInPage + 1;
					_boxConnect.ApplicationError.PageIndexApplicationError = _boxConnect.ApplicationError.PageIndexMaxpplicationError >= 1 ? 1 : 0;
                }
            }
			catch (Exception ex)
			{
				Logger.log.Error($"Error call Index() in HomeController, Exception: {ex?.ToString()}");
			}
			return View(_boxConnect);
		}

		[AuthActionFilter]
		[HttpGet]
		public IActionResult _BoxConnect()
		{
			BoxConnectModel _boxConnect = new BoxConnectModel();
			try
			{
				var client = new RestClient(APIMonitorDomain + ":" + APIMonitorPort);
				var request = new RestRequest("api/ApiMonitor/get-boxconnect-info", Method.Get);				
				var response = client.Execute(request);
				_boxConnect = JsonConvert.DeserializeObject<BoxConnectModel>(response?.Content ?? "");    
            }
			catch (Exception ex)
			{
				Logger.log.Error($"Error call _BoxConnect() in HomeController  ,Exception: {ex?.ToString()}");
			}
			
			return PartialView(_boxConnect);
		}
		
        [HttpGet]
        [Route("/Home/RejectionPaging")]
        public IActionResult GetRejectionListByPaging(int pageIndex)
        {
            var _boxConnect = new BoxConnectModel();
            long t1 = DateTime.Now.Ticks;
            try
            {
                Logger.ApiLog.Info($"Start call GetRejectionListByPaging with pageIndex: {pageIndex} ");
                var client = new RestClient(APIMonitorDomain + ":" + APIMonitorPort);
                var request = new RestRequest("api/ApiMonitor/get-boxconnect-info", Method.Get);
                var response = client.Execute(request);
                _boxConnect = JsonConvert.DeserializeObject<BoxConnectModel>(response.Content ?? "");
				if (_boxConnect != null && _boxConnect.DataMem != null)
				{
					var listAllMsgRejectOnMemory = _boxConnect?.DataMem?.ListAllMsgRejectOnMemory;
					int? countPageIndexMaxRejection = listAllMsgRejectOnMemory?.Count;

					_boxConnect.DataMem.PageIndexMaxRejection = (countPageIndexMaxRejection % RecordInPage == 0) 
						? countPageIndexMaxRejection / RecordInPage : countPageIndexMaxRejection / RecordInPage + 1;
					_boxConnect.DataMem.PageIndexRejection = pageIndex;
					_boxConnect.DataMem.ListAllMsgRejectOnMemory = listAllMsgRejectOnMemory?.OrderByDescending(item => item.TimeRecv).ToList() ?? new();
					_boxConnect.DataMem.ListDisplayMsgRejectOnMemory = listAllMsgRejectOnMemory?.OrderByDescending(item => item.TimeRecv)
						.Skip((pageIndex - 1)*RecordInPage).Take(RecordInPage).ToList() ?? new();
				}
                Logger.ApiLog.Info($"End call GetRejectionListByPaging with pageIndex: {pageIndex}; Processed in {(DateTime.Now.Ticks - t1) * 10} us");
                LogStationFacade.RecordforPT("GetRejectionListByPaging", DateTime.Now.Ticks - t1, true, "HomeController");
            }

            catch (Exception ex)
            {
                Logger.log.Error($"Error call GetRejectionListByPage() in HomeController, Exception: {ex?.ToString()}");
            }
            return  PartialView("par_Rejection", _boxConnect?.DataMem);
        }

		[HttpGet]
		[Route("/Home/SearchListSecurities")]
		public IActionResult SearchListSecurities(string symbolID)
		{
			var _boxConnect = new BoxConnectModel();
            long t1 = DateTime.Now.Ticks;
            try
			{
                Logger.ApiLog.Info($"Start call SearchListSecurities with symbolID: {symbolID} ");
                var client = new RestClient(APIMonitorDomain + ":" + APIMonitorPort);
				var request = new RestRequest("api/ApiMonitor/get-boxconnect-info", Method.Get);
				var response = client.Execute(request);
				_boxConnect = JsonConvert.DeserializeObject<BoxConnectModel>(response.Content ?? "");
				var listSecuritiesSearch = !string.IsNullOrEmpty(symbolID) ? _boxConnect?.DataMem?.ListSearchSecurities
					.Where(item => item.Symbol.Contains(symbolID.ToUpper()))					
					: _boxConnect?.DataMem?.ListSearchSecurities;
					
				if (_boxConnect != null && _boxConnect.DataMem != null)
				{
					int? countPageIndexMaxSecurities = listSecuritiesSearch?.ToList().Count;

					_boxConnect.DataMem.PageIndexMaxSecurities = (countPageIndexMaxSecurities % RecordInPage == 0) ? countPageIndexMaxSecurities / RecordInPage : countPageIndexMaxSecurities / RecordInPage + 1;
					_boxConnect.DataMem.PageIndexSecurities = _boxConnect.DataMem.PageIndexMaxSecurities >= 1 ? 1 : 0;
					_boxConnect.DataMem.ListSearchSecurities = listSecuritiesSearch?.OrderBy(item => item.Symbol).ToList() ?? new();
					_boxConnect.DataMem.ListDisplaySecurities = listSecuritiesSearch?.OrderBy(item => item.Symbol)
						.Skip(0).Take(RecordInPage).ToList() ?? new();
					HttpContext.Session.SetString("SearchModel", JsonConvert.SerializeObject(_boxConnect));
					if (symbolID != null) {
						HttpContext.Session.SetString("SymbolID", symbolID);
					}
					else
					{
						HttpContext.Session.Remove("SymbolID");
                    }
				}
                Logger.ApiLog.Info($"End call SearchListSecurities with symbolID: {symbolID}; Processed in {(DateTime.Now.Ticks - t1) * 10} us");
                LogStationFacade.RecordforPT("SearchListSecurities", DateTime.Now.Ticks - t1, true, "HomeController");
            }
            catch (Exception ex)
			{
				Logger.log.Error($"Error call SearchListSecurities() in HomeController, Exception: {ex?.ToString()}");
			}
			return PartialView("par_SecuritiesInfo", _boxConnect?.DataMem);
		}
		[HttpGet]
		[Route("/Home/ReloadSearchListSecurities")]
		public IActionResult ReloadSearchListSecurities()
		{
            var _boxConnect = new BoxConnectModel();
            long t1 = DateTime.Now.Ticks;
			try
			{
                Logger.ApiLog.Info($"Start call ReloadSearchListSecurities");
				var symbolID = HttpContext.Session.GetString("SymbolID") ?? "";
                var client = new RestClient(APIMonitorDomain + ":" + APIMonitorPort);
                var request = new RestRequest("api/ApiMonitor/get-boxconnect-info", Method.Get);
                var response = client.Execute(request);
                _boxConnect = JsonConvert.DeserializeObject<BoxConnectModel>(response.Content ?? "");
                var listSecuritiesSearch = !string.IsNullOrEmpty(symbolID) ? _boxConnect?.DataMem?.ListSearchSecurities
                    .Where(item => item.Symbol.Contains(symbolID.ToUpper()))
                    : _boxConnect?.DataMem?.ListSearchSecurities;

                if (_boxConnect != null && _boxConnect.DataMem != null)
                {
                    int? countPageIndexMaxSecurities = listSecuritiesSearch?.ToList().Count;

                    _boxConnect.DataMem.PageIndexMaxSecurities = (countPageIndexMaxSecurities % RecordInPage == 0) ? countPageIndexMaxSecurities / RecordInPage : countPageIndexMaxSecurities / RecordInPage + 1;
                    _boxConnect.DataMem.PageIndexSecurities = _boxConnect.DataMem.PageIndexMaxSecurities >= 1 ? 1 : 0;
                    _boxConnect.DataMem.ListSearchSecurities = listSecuritiesSearch?.OrderBy(item => item.Symbol).ToList() ?? new();
                    _boxConnect.DataMem.ListDisplaySecurities = listSecuritiesSearch?.OrderBy(item => item.Symbol)
                        .Skip(0).Take(RecordInPage).ToList() ?? new();
                    HttpContext.Session.SetString("SearchModel", JsonConvert.SerializeObject(_boxConnect));
                }
                Logger.ApiLog.Info($"End call ReloadSearchListSecurities; Processed in {(DateTime.Now.Ticks - t1) * 10} us" + "|listSecuritiesSearch count:" + listSecuritiesSearch.Count().ToString());

                LogStationFacade.RecordforPT("ReloadSearchListSecurities", DateTime.Now.Ticks - t1, true, "HomeController");
            }
			catch (Exception ex)
			{
                Logger.log.Error($"Error call ReloadSearchListSecurities() in HomeController, Exception: {ex?.ToString()}");
            }
			return PartialView("par_SecuritiesInfo", _boxConnect?.DataMem);
        }

        [HttpGet]
		[Route("/Home/SecuritiesPaging")]
		public IActionResult GetListSecuritiesByPage(int pageIndex)
		{
			var _boxConnect = new BoxConnectModel();
            long t1 = DateTime.Now.Ticks;
            try
			{
                Logger.ApiLog.Info($"Start call GetListSecuritiesByPage with pageIndex: {pageIndex} ");
                var searchModelString = HttpContext.Session.GetString("SearchModel");
                _boxConnect = JsonConvert.DeserializeObject<BoxConnectModel>(searchModelString ?? "");
                if (_boxConnect == null)
				{
                    var client = new RestClient(APIMonitorDomain + ":" + APIMonitorPort);
                    var request = new RestRequest("api/ApiMonitor/get-boxconnect-info", Method.Get);
                    var response = client.Execute(request);
                    _boxConnect ??= JsonConvert.DeserializeObject<BoxConnectModel>(response.Content ?? "");
                }
					
				if (_boxConnect != null && _boxConnect.DataMem != null)
				{
					var listSearchSecurities = _boxConnect?.DataMem?.ListSearchSecurities;
					int? countPageIndexMaxSecurities = _boxConnect?.DataMem?.ListSearchSecurities.Count;

					_boxConnect.DataMem.PageIndexMaxSecurities = (countPageIndexMaxSecurities % RecordInPage == 0) ? countPageIndexMaxSecurities / RecordInPage : countPageIndexMaxSecurities / RecordInPage + 1;
					_boxConnect.DataMem.PageIndexSecurities = pageIndex;
					_boxConnect.DataMem.ListSearchSecurities = listSearchSecurities?.OrderBy(item => item.Symbol).ToList() ?? new();
					_boxConnect.DataMem.ListDisplaySecurities = listSearchSecurities?.OrderBy(item => item.Symbol)
						.Skip((pageIndex - 1) * RecordInPage).Take(RecordInPage).ToList() ?? new();
				}
                Logger.ApiLog.Info($"End call GetListSecuritiesByPage with pageIndex: {pageIndex}; Processed in {(DateTime.Now.Ticks - t1) * 10} us");
                LogStationFacade.RecordforPT("GetListSecuritiesByPage", DateTime.Now.Ticks - t1, true, "HomeController");
            }
			catch (Exception ex)
			{
				Logger.log.Error($"Error call GetListSecuritiesByPage() in HomeController, Exception: {ex?.ToString()}");
			}
			return PartialView("par_SecuritiesInfo", _boxConnect?.DataMem);
		}
		
		[HttpGet]
		[Route("/Home/ApplicationErrorPaging")]
		public IActionResult GetListApplicationErrorByPage(int pageIndex)
		{
            long t1 = DateTime.Now.Ticks;
            var _boxConnect = new BoxConnectModel();
			try
			{
                Logger.ApiLog.Info($"Start call GetListApplicationErrorByPage with pageIndex: {pageIndex} ");
                var clientLogError = new RestClient(APIMonitorDomain + ":" + APIMonitorPort);
                var requestLog = new RestRequest("api/ApiMonitor/GetApplicationError", Method.Get);
                var responseLog = clientLogError.Execute(requestLog);
                var applicationLog = JsonConvert.DeserializeObject<ApplicationErrorModel>(responseLog?.Content ?? "");
				if(_boxConnect != null)
				{
					var listAllErrors = applicationLog?.ListAllErrors;
					int ? countPageIndexMaxError = listAllErrors?.Count;
                    _boxConnect.ApplicationError = applicationLog ?? new();
                    _boxConnect.ApplicationError.ListAllErrors = listAllErrors ?? new();
                    _boxConnect.ApplicationError.ListDisplayErrors = listAllErrors?.Skip((pageIndex - 1) * RecordInPage).Take(RecordInPage).ToList() ?? new();
                    _boxConnect.ApplicationError.PageIndexMaxpplicationError = (countPageIndexMaxError % RecordInPage == 0) 
						? countPageIndexMaxError / RecordInPage : countPageIndexMaxError / RecordInPage + 1;
                    _boxConnect.ApplicationError.PageIndexApplicationError = countPageIndexMaxError > 0 ? pageIndex : 0;
                }
                Logger.ApiLog.Info($"End call GetListApplicationErrorByPage with pageIndex: {pageIndex}; Processed in {(DateTime.Now.Ticks - t1) * 10} us");
                LogStationFacade.RecordforPT("GetListApplicationErrorByPage", DateTime.Now.Ticks - t1, true, "HomeController");
            }
			catch (Exception ex)
			{
                Logger.log.Error($"Error call GetListApplicationErrorByPage with pageIndex: {pageIndex} in HomeController, Exception: {ex?.ToString()}");
            }
			return PartialView("par_ApplicationError", _boxConnect?.ApplicationError);
		}

		[HttpGet]
		[Route("/Home/GetDetailApplicationError")]
        public IActionResult GetDetailApplicationError(int index)
		{
			var result = new ApplicationError();
            long t1 = DateTime.Now.Ticks;
            try
			{
                Logger.ApiLog.Info($"Start call GetDetailApplicationError with index: {index} ");
                var clientLogError = new RestClient(APIMonitorDomain + ":" + APIMonitorPort);
                var requestLog = new RestRequest("api/ApiMonitor/GetApplicationError", Method.Get);
                var responseLog = clientLogError.Execute(requestLog);
                var applicationLog = JsonConvert.DeserializeObject<ApplicationErrorModel>(responseLog?.Content ?? "");
                result = applicationLog?.ListAllErrors[index];
                Logger.ApiLog.Info($"End call GetDetailApplicationError with index: {index}; Processed in {(DateTime.Now.Ticks - t1) * 10} us");
                LogStationFacade.RecordforPT("GetDetailApplicationError", DateTime.Now.Ticks - t1, true, "HomeController");
            }
			catch (Exception ex)
			{
                Logger.log.Error($"Error call GetDetailApplicationError with index = {index} in HomeController, Exception: {ex?.ToString()}");
            }
            
            return Ok(result);
        }

		[AuthActionFilter(Const_UserRole.Role_Full)]
		[HttpPost]
		[Route("change-gateway-sequence")]
		public IActionResult ChangeGatewaySequence(string sequence, string lastProcessSequence)
		{
			try
			{
                UserInfo userInfo = HttpContext.GetCurrentUser();
                Logger.log.Info($"User name: {userInfo?.Username} -> change Gateway Sequence Request - IP Source: {HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()} - action: {HttpContext.Request.Method}");
                //
                if (!string.IsNullOrWhiteSpace(sequence))
                {
                    _ = sequence.Trim();
                }
                if (!string.IsNullOrWhiteSpace(lastProcessSequence))
                {
                    _ = lastProcessSequence.Trim();
                }
                var client = new RestClient(APIMonitorDomain + ":" + APIMonitorPort);
                var request = new RestRequest("api/ApiMonitor/change-gateway-sequence", Method.Post);
                //
                request.AddParameter("sequence", sequence, ParameterType.QueryString);
                request.AddParameter("lastProcessSequence", lastProcessSequence, ParameterType.QueryString);
                var response = client.Execute(request);
                //
                var res = Utils.ParseInt(response?.Content ?? "");
                string userStatusText = ShareMemoryData.c_UserStatusText;
                if (res > 0)
                {
                    return Json(new { code = 1, message = "Change Gateway Sequence success!" });
                }
                else
                {
                    return Json(new { code = -1, message = "Error Change Gateway Sequence!" });
                }
            }
			catch (Exception ex)
			{
                Logger.log.Error($"Error call ChangeGatewaySequence in HomeController  ,Exception: {ex?.ToString()}");
                return Json(new { code = -1, message = "Error catch system." });
            }            
        }

        [AuthActionFilter(Const_UserRole.Role_Full)]
		[HttpPost]
		[Route("showformcontrol")]
		public async Task<ActionResult> PopupShowFormControl()
		{
			try
			{
				UserInfo userInfo = HttpContext.GetCurrentUser();

				Logger.log.Info($"User name: {userInfo?.Username} -> open form control - IP Source: {HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()} - action: {HttpContext.Request.Method}");
				//
				string viewContent = await this.RenderViewToStringAsync("_ControlPanel", new object());
				return Json(new { code = 1, message = viewContent });
			}
			catch (Exception ex)
			{
				Logger.log.Error($"Error call _BoxConnect() in HomeController  ,Exception: {ex?.ToString()}");
				return Json(new { code = -1, message = "Đã có lỗi xảy ra!" });
			}
		}

		[AuthActionFilter]
		[HttpGet]
		public IActionResult _SessionFlag()
		{
			BoxConnectModel _boxConnect = new BoxConnectModel();
			try
			{
				var client = new RestClient(ConfigData.APIMonitorDomain + ":" + ConfigData.APIMonitorPort);
				var request = new RestRequest("api/ApiMonitor/get-boxconnect-info", Method.Get);
				//
				//request.AddParameter("param", _value);
				//
				var response = client.Execute(request);
				_boxConnect = JsonConvert.DeserializeObject<BoxConnectModel>(response?.Content ?? "");
			}
			catch (Exception ex)
			{
				Logger.log.Error($"Error call _SessionFlag() in HomeController  ,Exception: {ex?.ToString()}");
			}
			return PartialView(_boxConnect);
		}

		//
		[AuthActionFilter(Const_UserRole.Role_Full)]
		[HttpPost]
		[Route("change-password")]
		public IActionResult ChangGatewayPassword(string oldpass, string newpass)
		{
			try
			{
				// Trim
				UserInfo userInfo = HttpContext.GetCurrentUser();
				Logger.log.Info($"User name: {userInfo?.Username} -> Change gateway password - IP Source: {HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()} - action: {HttpContext.Request.Method}");
				//
				if (!string.IsNullOrWhiteSpace(oldpass))
				{
					oldpass = oldpass.Trim();
				}
				if (!string.IsNullOrWhiteSpace(newpass))
				{
					newpass = newpass.Trim();
				}
				if (!ChangPassCheckValidate(newpass))
				{
					return Json(new { code = -1, message = $"Password must be 8-20 characters and contain three of four types (uppercase, lowercase, number, or special character)" });
				}
				//
				var client = new RestClient(ConfigData.APIMonitorDomain + ":" + ConfigData.APIMonitorPort);
				var request = new RestRequest("api/ApiMonitor/change-gateway-password", Method.Post);
				//
				request.AddParameter("p_oldpass", oldpass, ParameterType.QueryString);
				request.AddParameter("p_newpass", newpass, ParameterType.QueryString);
				var response = client.Execute(request);
				//
				var res = Utils.ParseInt(response?.Content ?? "");
				string userStatusText = ShareMemoryData.c_UserStatusText;
				//
				if (res > 0)
				{
					if (ConfigData.EnableVault)
					{
						// Thực hiện cập nhật lại pass trên Vault
						bool vaultResult =VaultHelper.WriteData(newpass);
						if (vaultResult)
						{
							return Json(new { code = res, message = userStatusText });
						}
						else
						{
							return Json(new { code = -1, message = $"Receive Exchange with UserStatus ={userStatusText}, Fail to change gateway password when write data to Vault." });
						}
					}
					else
					{
						return Json(new { code = res, message = userStatusText });
					}
				}
				else
				{
					return Json(new { code = -1, message = $"Fail to change gateway password when send to exchange UserStatus ={userStatusText}" });
				}
			}
			catch (Exception ex)
			{
				Logger.log.Error($"Error call ChangGatewayPassword in HomeController  ,Exception: {ex?.ToString()}");
				return Json(new { code = -1, message = "Error catch system." });
			}
		}

		private bool ChangPassCheckValidate(string pPassword)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(pPassword))
				{
					return false;
				}
				else if(pPassword.Contains(" "))
				{
					return false;
				}
				else if (pPassword.Length < 4 || pPassword.Length > 10)
				{
					return false;
				}
				else if(Utils.CheckTiengViet(pPassword))
				{
					return false;
				}
				//
				int _countCondition = 4;
				if (Utils.CheckPassword_LowChar(pPassword) == false)
				{
					_countCondition--;
				}
				else if (Utils.CheckPassword_UpperChar(pPassword) == false)
				{
					_countCondition--;
				}
				else if (Utils.CheckPassword_NumChar(pPassword) == false)
				{
					_countCondition--;
				}
				else if (Utils.CheckPassword_SpecialChar(pPassword) == false)
				{
					_countCondition--;
				}
				if(_countCondition>=3)
				{
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				Logger.log.Error(ex.ToString());
				return false;
			}
		}

		//private bool VaultWriteData(string passNew)
		//{
		//	try
		//	{
		//		// config
		//		VaultConfiguration config = new VaultConfiguration(ConfigData.VaultAddress);
		//		//
		//		VaultClient vaultClient = new VaultClient(config);
		//		//
		//		if (string.IsNullOrEmpty(VaultSetting.Token))
		//		{
		//			VaultResponse<Object> resp = vaultClient.Auth.UserpassLogin(username: ConfigData.VaultUsername, new UserpassLoginRequest(Password: ConfigData.VaultPassword));
		//			VaultSetting.Token = resp.ResponseAuth.ClientToken;
		//		}
		//		vaultClient.SetToken(token: VaultSetting.Token);

		//		//
		//		try
		//		{
		//			var secretData = new Dictionary<string, string> { { VaultSetting.DataKey, passNew } };
		//			// Write a secret
		//			var kvRequestData = new KvV2WriteRequest(secretData);
		//			vaultClient.Secrets.KvV2Write(ConfigData.VaultPath, kvRequestData, kvV2MountPath: "secrets");
		//			Thread.Sleep(1000); // ssi bao sau khi write thì slep lai 1s
		//								// Read a secret
		//			VaultResponse<KvV2ReadResponse> kvResp = vaultClient.Secrets.KvV2Read(ConfigData.VaultPath, kvV2MountPath: "secrets");
		//			//
		//			Logger.log.Debug($"VaultResponse when write data = {kvResp.Data.Data}");
		//			if (kvResp.Data.Data != null)
		//			{
		//				VaultDataResponse res = JsonHelper.Deserialize<VaultDataResponse>(kvResp.Data.Data.ToString());
		//				//
		//				Logger.log.Debug($"Response from Vault success when execute write data with password with data = {kvResp.Data.Data.ToString()}, VaultDataResponse= {JsonHelper.Serialize(res)}");
		//				//
		//				if (!string.IsNullOrEmpty(res.MemberPass))
		//				{
		//					ConfigData.Password = res.MemberPass;
		//					return true;
		//				}
		//				else
		//				{
		//					Logger.log.Error($"Response from Vault when execute write data with key={VaultSetting.DataKey}, respone MemberPass is null.");
		//					return false;
		//				}
		//			}
		//			else
		//			{
		//				Logger.log.Error($"Error response from Vault fail when execute write data with password new.");
		//				//
		//				return false;
		//			}
		//		}
		//		catch (VaultApiException e)
		//		{
		//			Logger.log.Error("Failed to read secret with message {0}", e.Message);
		//			return false;
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.log.Error($"Error call VaultWriteData in HomeController  ,Exception: {ex?.ToString()}");
		//		return false;
		//	}
		//}

		[AuthActionFilter(Const_UserRole.Role_Full)]
		[HttpPost]
		[Route("trading-session-req")]
		public IActionResult TradingSessionRequest(string tradingCode, string tradingName)
		{
			try
			{
				UserInfo userInfo = HttpContext.GetCurrentUser();
				Logger.log.Info($"User name: {userInfo?.Username} -> Trading session request - IP Source: {HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()} - action: {HttpContext.Request.Method}");
				//
				if (!string.IsNullOrWhiteSpace(tradingCode))
				{
					tradingCode = tradingCode.Trim();
				}
				if (!string.IsNullOrWhiteSpace(tradingName))
				{
					tradingName = tradingName.Trim();
				}
				//
				var client = new RestClient(ConfigData.APIMonitorDomain + ":" + ConfigData.APIMonitorPort);
				var request = new RestRequest("api/ApiMonitor/trading-session-request", Method.Post);
				//
				request.AddParameter("p_tradingCode", tradingCode, ParameterType.QueryString);
				request.AddParameter("p_tradingName", tradingName, ParameterType.QueryString);
				var response = client.Execute(request);
				//
				var res = Utils.ParseInt(response?.Content ?? "");
				//
				if (res > 0)
				{
					return Json(new { code = 1, message = "Send Trading session request success!" });
				}
				else
				{
					return Json(new { code = -1, message = "Error Send Trading session request!" });
				}
			}
			catch (Exception ex)
			{
				Logger.log.Error($"Error call TradingSessionRequest in HomeController  ,Exception: {ex?.ToString()}");
				return Json(new { code = -1, message = "Error catch system." });
			}
		}

		[AuthActionFilter(Const_UserRole.Role_Full)]
		[HttpPost]
		[Route("security-status-req")]
		public IActionResult SendSecurityStatusRequest(string tradingCode, string symbol)
		{
			try
			{
				UserInfo userInfo = HttpContext.GetCurrentUser();
				Logger.log.Info($"User name: {userInfo?.Username} -> send Security Status Request - IP Source: {HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()} - action: {HttpContext.Request.Method}");
				//
				if (!string.IsNullOrWhiteSpace(tradingCode))
				{
					tradingCode = tradingCode.Trim();
				}
				if (!string.IsNullOrWhiteSpace(symbol))
				{
					symbol = symbol.Trim();
				}
				//
				var client = new RestClient(ConfigData.APIMonitorDomain + ":" + ConfigData.APIMonitorPort);
				var request = new RestRequest("api/ApiMonitor/security-status-request", Method.Post);
				//
				request.AddParameter("p_tradingCode", tradingCode, ParameterType.QueryString);
				request.AddParameter("p_symbol", symbol, ParameterType.QueryString);
				//
				var response = client.Execute(request);
				var res = Utils.ParseInt(response?.Content ?? "");
				//
				if (res > 0)
				{
					return Json(new { code = 1, message = "Send security status request success!" });
				}
				else
				{
					return Json(new { code = -1, message = "Error send security status request!" });
				}
			}
			catch (Exception ex)
			{
				Logger.log.Error($"Error call SendSecurityStatusRequest({tradingCode},{symbol}) in HomeController  ,Exception: {ex?.ToString()}");
				return Json(new { code = -1, message = "Error catch system." });
			}
		}

		[AuthActionFilter(Const_UserRole.Role_Full)]
		[HttpGet]
		[Route("vault-query")]
		public IActionResult VaultQuery(string pPassword, string pNewPass)
		{
			try
			{
				if (ConfigData.EnableVault)
				{
					var client = new RestClient(ConfigData.APIMonitorDomain + ":" + ConfigData.APIMonitorPort);
					var request = new RestRequest("api/ApiMonitor/api-vault-query", Method.Post);
					//
					request.AddParameter("p_passold", pPassword, ParameterType.QueryString);
					//
					var response = client.Execute(request);
					var res = Utils.ParseInt(response?.Content ?? "");
					if (res == 1)
					{
						return Json(new { code = 1, message = "Response from Vault success." });
					}
					else
					{
						Logger.log.Warn($"Warning call VaultQuery fail.");
						//
						return Json(new { code = res, message = "Old password does not match the saved in Vault" });
					}
				}
				else
				{
					return Json(new { code = 1, message = "Ignore check vault." });
				}
			}
			catch (Exception ex)
			{
				Logger.log.Error($"Error call VaultQuery() in HomeController  ,Exception: {ex?.ToString()}");
				return Json(new { code = -1, message = "Exception response from Vault fail when excute query" });
			}
		}
	}
}