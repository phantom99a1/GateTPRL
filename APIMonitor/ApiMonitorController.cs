﻿using APIMonitor.Helpers;
using APIMonitor.Models;
using APIMonitor.ObjectInfo;
using BusinessProcessResponse;
using CommonLib;
using HNX.FIXMessage;
using HNXInterface;
using LocalMemory;
using LogStation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ObjectInfo;

namespace APIMonitor
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomActionFilter]
    public class ApiMonitorController : ControllerBase
    {
        public iHNXClient c_iHNXClient;  //để gửi yêu cầu phiên, thị trường
        public IResponseInterface c_IResponseInterface; //để lấy thông tin của  IResponseInterface

        public ApiMonitorController(iHNXClient p_iHNXClient, IResponseInterface p_IResponseInterface)
        {
            c_iHNXClient = p_iHNXClient;
            c_IResponseInterface = p_IResponseInterface;
        }

        [HttpGet]
        [Route("get-boxconnect-info")]
        public BoxConnectModel _BoxConnect()
        {
            BoxConnectModel _boxConnect = GetBoxConnectModel();
            return _boxConnect;
        }

        [HttpGet]
        [Route("GetApplicationError")]
        public ApplicationErrorModel ApplicationError()
        {
            var applicationErrorModel = new ApplicationErrorModel
            {
                ListAllErrors = GetLogApplicationErrorList()
            };
            return applicationErrorModel;
        }

        [HttpGet]
        [Route("LogApplicationError")]
        public List<ApplicationError> GetLogApplicationErrorList()
        {
            int numberFormatMonthDay = 10;
            List<ApplicationError> lines = new();
			long t1 = DateTime.Now.Ticks;
			try
            {
				Logger.ApiLog.Info($"Start call GetLogApplicationErrorList");
				var time = DateTime.Now;
                string monthSring = time.Month < numberFormatMonthDay ? $"0{time.Month}" : time.Month.ToString();
                string dayString = time.Day < numberFormatMonthDay ? $"0{time.Day}" : time.Day.ToString();
                string fileLogError = ConfigData.LogApplicationError;
                string HNXTPRLTCPErrorFilePath = ConfigData.HNXTPRLTCPErrorFilePath;
                string HNXTPRLGateErrorFilePath = ConfigData.HNXTPRLGateErrorFilePath;

                var timeString = $"{time.Year}-{monthSring}-{dayString}";
                string logFilePathTCPError = $"{fileLogError}/{timeString}/{HNXTPRLTCPErrorFilePath}";
                string logFilePathGateError = $"{fileLogError}/{timeString}/{HNXTPRLGateErrorFilePath}";
				var fileStreamOptions = new FileStreamOptions
                {
                    Share = FileShare.ReadWrite
                };
                
                int lineCount = 0;

                if (System.IO.File.Exists(logFilePathTCPError))
                {
                    using var reader = new StreamReader(logFilePathTCPError, fileStreamOptions);
                    string line;
                    while ((line = reader.ReadLine()) != null && lineCount < ConfigData.MaxLinesReader)
                    {
                        var applicationError = JsonConvert.DeserializeObject<ApplicationError>(line) ?? new ApplicationError();
                        lines.Add(applicationError);
                        lineCount++;
                    }
                }
                if (System.IO.File.Exists(logFilePathGateError))
                {
                    using var reader =  new StreamReader(logFilePathGateError, fileStreamOptions);
                    string line;
                    while ((line = reader.ReadLine()) != null && lineCount < ConfigData.MaxLinesReader)
                    {
                        var applicationError = JsonConvert.DeserializeObject<ApplicationError>(line) ?? new ApplicationError();
                        lines.Add(applicationError);
                        lineCount++;
                    }
                }
				Logger.ApiLog.Info($"End call GetLogApplicationErrorList; Processed in {(DateTime.Now.Ticks - t1) * 10} us");
				LogStationFacade.RecordforPT("GetLogApplicationErrorList", DateTime.Now.Ticks - t1, true, "ApiMonitorController");
			}
            catch (Exception ex)
            {
                Logger.log.Error($"Error call GetLogApplicationErrorList() in ApiMonitorController, Exception: {ex?.ToString()}");
            }
            return lines.OrderByDescending(item => item.Time).ToList();
        }
        [HttpGet]
        [Route("GateTPRLMonitor")]
        public GateTPRLMonitorModel GetGateTPRLMonitor()
        {
            var gateTPRLMonitor = new GateTPRLMonitorModel();
			long t1 = DateTime.Now.Ticks;
			try
            {
				Logger.ApiLog.Info($"Start call GetGateTPRLMonitor");
				gateTPRLMonitor.ExchangeRevMessageNum = DataMem.gateTPRLMonitorExchange.ExchangeRevMessageNum;
                gateTPRLMonitor.ExchangeSendMessageNum = DataMem.gateTPRLMonitorExchange.ExchangeSendMessageNum;
                gateTPRLMonitor.ExchangeQueueMessageNum = DataMem.gateTPRLMonitorExchange.ExchangeQueueMessageNum;
                gateTPRLMonitor.TradingSession = TradingRuleData.GetTradingSessionCodeofMainBoard();
                gateTPRLMonitor.TradingStatus = TradingRuleData.GetTradeSesStatusofMainBoard();
                if (c_iHNXClient.ClientStatus() != enumClientStatus.DATA_TRANSFER
                    && c_iHNXClient.ClientStatus() != enumClientStatus.PROCESS_RESEND
                    && c_iHNXClient.ClientStatus() != enumClientStatus.RESEND_REQUEST)
                {
                    gateTPRLMonitor.LoginStatus = "0";
                }
                else
                {
                    gateTPRLMonitor.LoginStatus = "1";
                }
				Logger.ApiLog.Info($"End call GetGateTPRLMonitor; Processed in {(DateTime.Now.Ticks - t1) * 10} us");
				LogStationFacade.RecordforPT("GetGateTPRLMonitor", DateTime.Now.Ticks - t1, true, "ApiMonitorController");
			}
            catch (Exception ex)
            {
                Logger.log.Error($"Error call GetGateTPRLMonitor() in ApiMonitorController, Exception: {ex?.ToString()}");
            }
            return gateTPRLMonitor;
        }

        [HttpGet]
        [Route("GateTPRLWarningThreshold")]
        public GateTPRLWarningThreshold GetGateTPRLWarningThreshold()
        {
            var gateTPRLWarningThreshold = new GateTPRLWarningThreshold();
            long t1 = DateTime.Now.Ticks;
            try
            {
                Logger.ApiLog.Info($"Start call GetGateTPRLWarningThreshold");
                lock (DataMem.lockObj)
                {
                    gateTPRLWarningThreshold = DataMem.warningThreshold;
                }
                Logger.ApiLog.Info($"End call GetGateTPRLWarningThreshold; Processed in {(DateTime.Now.Ticks - t1) * 10} us");
                LogStationFacade.RecordforPT("GetGateTPRLWarningThreshold", DateTime.Now.Ticks - t1, true, "ApiMonitorController");
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call GetGateTPRLWarningThreshold() in ApiMonitorController, Exception: {ex?.ToString()}");
            }
            return gateTPRLWarningThreshold ?? new();
        }

        [HttpPost]
        [Route("change-gateway-sequence")]
        public int ChangeGatewaySequence([FromQuery] string sequence, [FromQuery] string lastProcessSequence)
        {
			long t1 = DateTime.Now.Ticks;
			try
            {
				Logger.ApiLog.Info($"Start call ChangeGatewaySequence with sequence: {sequence}, lastProcessSequence: {lastProcessSequence} ");
				int cliSeq = c_iHNXClient.ChangeSeq(int.Parse(sequence));
                int lastSeq = c_iHNXClient.ChangeLastSeqProcess(int.Parse(lastProcessSequence));
                var result = new { cliSeq, lastSeq };
                if(result != null)
                {
                    return 1;
                }
				Logger.ApiLog.Info($"End call ChangeGatewaySequence with sequence: {sequence}, lastProcessSequence: {lastProcessSequence}; Processed in {(DateTime.Now.Ticks - t1) * 10} us");
				LogStationFacade.RecordforPT("ChangeGatewaySequence", DateTime.Now.Ticks - t1, true, "ApiMonitorController");
				return 0;
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call ChangeGatewaySequence() in ApiMonitorController, Exception: {ex?.ToString()}");
                return -1;
            }
        }

        [HttpPost]
        [Route("change-gateway-password")]
        public int ChangGatewayPassword([FromQuery] string? p_oldpass, [FromQuery] string? p_newpass)
        {
            try
            {
                bool _result = c_iHNXClient.SendUserRequest(ConfigData.Username, p_oldpass, p_newpass);
                //
                long _Timeout = DateTime.Now.AddSeconds(30).Ticks;
                while (ShareMemoryData.c_UserStatus == -1 && DateTime.Now.Ticks < _Timeout)
                {
                    Thread.Sleep(500);
                }
                return ShareMemoryData.c_UserStatus;
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call ChangGatewayPassword() in ApiMonitorController  ,Exception: {ex?.ToString()}");
                return -1;
            }
        }

        [HttpPost]
        [Route("trading-session-request")]
        public int TradingSessionRequest([FromQuery] string? p_tradingCode, [FromQuery] string? p_tradingName)
        {
            try
            {
                bool _result = c_iHNXClient.SendTradingSessionRequest(p_tradingCode ?? string.Empty, p_tradingName ?? string.Empty);
                if (_result)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call TradingSessionRequest(p_tradingCode={p_tradingCode},p_tradingName={p_tradingName}) in ApiMonitorController  ,Exception: {ex?.ToString()}");
                return -1;
            }
        }

        [HttpPost]
        [Route("security-status-request")]
        public int SecurityStatusRequest([FromQuery] string? p_tradingCode, [FromQuery] string? p_symbol)
        {
            try
            {
                bool _result = c_iHNXClient.SendSecurityStatusRequest(p_tradingCode ?? string.Empty, p_symbol ?? string.Empty);
                if (_result)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call _SendSecurityStatusRequest() in ApiMonitorController  ,Exception: {ex?.ToString()}");
                return -1;
            }
        }

        [HttpPost]
        [Route("api-vault-query")]
        public int VaultQuery([FromQuery] string p_passold)
        {
            try
            {
                string _kvRespData = VaultHelper.ReadData();
                VaultDataResponse res = JsonHelper.Deserialize<VaultDataResponse>(_kvRespData);
                if (!string.IsNullOrEmpty(res.MemberPass) && res.MemberPass == p_passold)
                {
                    return 1;
                }
                else
                {
                    Logger.log.Error($"Response from Vault when read data with key={VaultSetting.DataKey}, respone MemberPass is null.");
                    return -1;
                }

                //            // config
                //            VaultConfiguration config = new VaultConfiguration(ConfigData.VaultAddress);
                ////
                //VaultClient vaultClient = new VaultClient(config);
                //if (string.IsNullOrEmpty(VaultSetting.Token))
                //{
                //	VaultResponse<Object> resp = vaultClient.Auth.UserpassLogin(username: ConfigData.VaultUsername, new UserpassLoginRequest(Password: ConfigData.VaultPassword));
                //	VaultSetting.Token = resp.ResponseAuth.ClientToken;
                //}
                //vaultClient.SetToken(token: VaultSetting.Token);
                ////
                //try
                //{
                //	//
                //	VaultResponse<KvV2ReadResponse> kvResp = vaultClient.Secrets.KvV2Read(ConfigData.VaultPath, kvV2MountPath: "secrets");
                //	//
                //	Logger.log.Debug($"VaultResponse when write data = {kvResp.Data.Data} when execute query");
                //	if (kvResp.Data.Data != null)
                //	{
                //		Logger.log.Debug($"Response from Vault success when execute query.");

                //		//VaultDataResponse res = (VaultDataResponse)kvResp.Data.Data;
                //		VaultDataResponse res = JsonHelper.Deserialize<VaultDataResponse>(kvResp.Data.Data.ToString());

                //		//
                //		Logger.log.Debug($"Response from Vault success when execute ExecuteAsync auto. | ConfigData.Password ={ConfigData.Password}, VaultDataResponse={JsonHelper.Serialize(res)}");

                //		//
                //		if (!string.IsNullOrEmpty(res.MemberPass) && res.MemberPass == p_passold)
                //		{
                //			return 1;
                //		}
                //		else
                //		{
                //			Logger.log.Error($"Response from Vault when read data with key={VaultSetting.DataKey}, respone MemberPass is null.");
                //			return -1;
                //		}
                //	}
                //	else
                //	{
                //		Logger.log.Error($"Error responsee from vault when execute query with kvResp={kvResp.Data.Data}.");
                //		//
                //		return 0;
                //	}
                //}
                //catch (VaultApiException e)
                //{
                //	Logger.log.Error("Failed to read secret with message {0}", e.Message);
                //	return -2;
                //}
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call VaultQuery() in ApiMonitorController  ,Exception: {ex?.ToString()}");
                return -1;
            }
        }

        [HttpGet]
        [Route("api-vault-read")]
        public string ReadQuery()
        {
            return VaultHelper.ReadData();
        }

        [HttpPost]
        [Route("api-vault-update")]
        public int VaultUpdatePass([FromQuery] string p_passnew)
        {
            try
            {
                bool result = VaultHelper.WriteData(p_passnew);
                if (result)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call VaultQuery() in ApiMonitorController  ,Exception: {ex?.ToString()}");
                return -1;
            }
        }

        [HttpGet, Route("get-config")]
        public ActionResult<object> ApiGetConfig()
        {
            return "username= " + ConfigData.Username + "   |   " + "pass=" + ConfigData.Password;
        }

        private BoxConnectModel GetBoxConnectModel()
        {
            BoxConnectModel _boxConnect = new BoxConnectModel();
            try
            {
                HNXSystemConnectModel _hnxSystem = new HNXSystemConnectModel();
                _hnxSystem.IP = ConfigData.IPServer;
                _hnxSystem.Port = ConfigData.PortServer;
                _hnxSystem.Status = c_iHNXClient.ClientStatus().ToString();
                //
                //DATA_TRANFER, PROCESS_RESEND, RESEND_REQUEST

                if (c_iHNXClient.ClientStatus() != enumClientStatus.DATA_TRANSFER
                    && c_iHNXClient.ClientStatus() != enumClientStatus.PROCESS_RESEND
                    && c_iHNXClient.ClientStatus() != enumClientStatus.RESEND_REQUEST)
                {
                    _hnxSystem.LoginStatus = "Not Login";
                }
                else
                {
                    _hnxSystem.LoginStatus = ShareMemoryData.c_LoginStatus;
                }

                _hnxSystem.Sequence = c_iHNXClient.Seq();
                _hnxSystem.LastSeqProcess = c_iHNXClient.LastSeqProcess();
                //
                KafkaSystemConnectModel _KafkaSystem = new KafkaSystemConnectModel();
                _KafkaSystem.IP = ConfigData.KafkaConfig.KafkaIp;
                _KafkaSystem.Port = ConfigData.KafkaConfig.KafkaPort;
                _KafkaSystem.TopicName_OrderStatus = ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderStatus;
                _KafkaSystem.TopicName_OrderExecution = ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_OrderExecution;
                _KafkaSystem.TopicName_TradingInfomation = ConfigData.KafkaConfig.KafkaTopic_HNXTPRL_TradingInfo;
                _KafkaSystem.NumberMessageSend = c_IResponseInterface.NumOfMsg();
                _KafkaSystem.EnableKafka = ConfigData.KafkaConfig.EnableKafka == true ? "On" : "Off";
                //
                _boxConnect.Session = TradingRuleData.GetTradingSessionNameofMainBoard();
                _boxConnect.TradingSession = TradingRuleData.GetTradingSessionCodeofMainBoard();
                _boxConnect._HNXSystemConnect = _hnxSystem;
                _boxConnect._KafkaSystemConnect = _KafkaSystem;

                //Thêm model cho phần MessageReject và SecurityInformation
                var listAllSecurities = DataMem.lstAllSecurities.GroupBy(item => item.Symbol)
                    .Select(item => item.OrderByDescending(m => m.IssueDate).FirstOrDefault()).ToList() as List<MessageSecurityStatus>;

				var _DataMem = new DataMemModel
                {
                    ListAllMsgRejectOnMemory = DataMem.lstAllMsgRejectOnMemory,
                    ListSearchSecurities = listAllSecurities
				};
                //
                _boxConnect.DataMem = _DataMem;
                //Thêm phần giám sát số lượng Message
                var _GateTPRLMonitor = new GateTPRLMonitorModel()
                {
                    ExchangeRevMessageNum = DataMem.gateTPRLMonitorExchange.ExchangeRevMessageNum,
                    ExchangeSendMessageNum = DataMem.gateTPRLMonitorExchange.ExchangeSendMessageNum,
                    ExchangeQueueMessageNum = 0,
                };
                _boxConnect.GateTPRLMonitor = _GateTPRLMonitor;
            }
            catch (Exception ex)
            {
                Logger.log.Error($"Error call GetBoxConnectModel() in ApiMonitorController  ,Exception: {ex?.ToString()}");
            }
            return _boxConnect;
        }

        [HttpGet, Route("get-by-clorderid")]
        public ActionResult<object> OrderGetByClOrderID([FromQuery] string p_ClodrID)
        {
            OrderInfo _OrderInfo = LocalMemory.OrderMemory.GetOrder_byClOrdID(p_ClodrID);
            if (_OrderInfo != null)
            {
                return _OrderInfo;
            }
            return "Khong tim thay du lieu tren memory";
        }

        [HttpGet, Route("get-by-orderno")]
        public ActionResult<object> OrderGetByOrderNo([FromQuery] string p_OrderNO)
        {
            OrderInfo _OrderInfo = LocalMemory.OrderMemory.GetOrder_ByOrderNo(p_OrderNO);
            if (_OrderInfo != null)
            {
                return _OrderInfo;
            }
            return "Khong tim thay du lieu tren memory";
        }

        [HttpGet, Route("get-org-by-clorderid")]
        public ActionResult<string> GetOrigOrder_byClOrdID([FromQuery] string p_ClOrderID)
        {
            return LocalMemory.OrderMemory.GetOrigOrder_byClOrdID(p_ClOrderID);
        }

        [HttpGet, Route("get-by-seq")]
        public ActionResult<object> OrderGetBySeq([FromQuery] int p_SeqNum)
        {
            OrderInfo _OrderInfo = LocalMemory.OrderMemory.GetOrder_bySeqNum(p_SeqNum);
            if (_OrderInfo != null)
            {
                return _OrderInfo;
            }
            return "Khong tim thay du lieu tren memory";
        }

        [HttpGet, Route("get-list-order")]
        public ActionResult<List<OrderInfo>> GetListOrder()
        {
            return OrderMemory.GetListOrder();
        }

        // API hỗ trợ check memory data
        [HttpGet, Route("test-get-mem")]
        public ActionResult<object> TestGetMemory([FromQuery] string RefExchangeID, [FromQuery] string RefMsgType)
        {
            try
            {
                OrderInfo objOrderMem = OrderMemory.GetOrderBy(p_RefExchangeID: RefExchangeID, p_RefMsgType: RefMsgType);
                if (objOrderMem != null)
                {
                    return objOrderMem;
                }
                return "Khong tim thay du lieu tren memory";
            }
            catch (Exception ex)
            {
                return "Error = " + ex?.ToString();
            }
        }

        [HttpGet]
        [Route("gen-pass")]
        public ActionResult<string> GenPassword(string p_username, string p_pass)
        {
            string _pass = GeneratePassWord(p_username, p_pass);
            return _pass;
        }

        private static string GeneratePassWord(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                return BCrypt.Net.BCrypt.HashPassword(string.Format("{0};#{1}", password, userName.ToLower()));
            }
            return "";
        }

        //// API hỗ trợ check memory data
        //[HttpGet, Route("test-get-update-mem")]
        //public ActionResult<object> TestGetUpdateMemory([FromQuery] string p_ClodrID)
        //{
        //    try
        //    {
        //        List<SymbolFirmObject> listSymbolFirmInfo = new List<SymbolFirmObject>();
        //        LocalMemory.OrderInfo objOrder = new OrderInfo()
        //        {
        //            RefMsgType = "S",
        //            OrderNo = "ACB",
        //            ClOrdID = "123456",
        //            ExchangeID = "", // ?
        //            RefExchangeID = "", // ?
        //            SeqNum = 0,  // khi nào sở về mới update
        //            Side = "B",
        //            Price = 10000,
        //            OrderQty = 1000,
        //            CrossType = "", // ?
        //            ClientID = "",
        //            ClientIDCounterFirm = "", // ?
        //            MemberCounterFirm = "" // ?
        //        };
        //        for (int i = 0; i < 2; i++)
        //        {
        //            SymbolFirmObject symbolFirm = new SymbolFirmObject();
        //            symbolFirm.NumSide = i;
        //            symbolFirm.Symbol = "SSI";
        //            symbolFirm.OrderQty = 100;
        //            symbolFirm.MergePrice = 1000;
        //            symbolFirm.HedgeRate = 1000;
        //            //
        //            listSymbolFirmInfo.Add(symbolFirm);
        //        }

        //        objOrder.SymbolFirmInfo = listSymbolFirmInfo;

        //        OrderMemory.Add_NewOrder(objOrder);

        //        OrderInfo _OrderInfo = LocalMemory.OrderMemory.GetOrder_byClOrdID(p_ClodrID);
        //        if (_OrderInfo != null)
        //        {
        //            _OrderInfo.ClientIDCounterFirm = "6789";
        //            //
        //            List<SymbolFirmObject> listSymbolFirmMem = _OrderInfo.SymbolFirmInfo;
        //            if (listSymbolFirmMem != null)
        //            {
        //                for (int i = 0; i < listSymbolFirmMem.Count; i++)
        //                {
        //                    SymbolFirmObject itemMem = listSymbolFirmMem[i];
    }
}