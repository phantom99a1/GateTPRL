using APIServer.Helpers;
using APIServer.Validation;
using BusinessProcessAPIReq;
using BusinessProcessAPIReq.RequestModels;
using BusinessProcessResponse;
using CommonLib;
using FluentValidation;
using HNXInterface;
using HNXTPRLGate.Workers;
using KafkaInterface;

namespace HNXTPRLGate.Startups
{
    public class Starting
    {
        public static void InitServices(IServiceCollection services)
        {
            try
            {
                CommonLib.Logger.log.Info("InitServices");
                //
                if (ConfigData.EnableVault)
                {
                    // Gọi sang Vault để lấy thông tin configPass logon to Exchange
                    VaultService.AutoCallVaultService();
                }
                else
                {
                    ConfigData.Password = ConfigData.PasswordInConfig;
                }

                // Cấu hình chạy kafka hay không
                services.AddSingleton<IKafkaClient, KafkaClient>();

                services.AddSingleton<IResponseInterface, ResponseInterface>();
                services.AddSingleton<iHNXClient, HNXTCPClient>();
                services.AddSingleton<IProcessRevBussiness, ProcessRevBusiness>();
                services.AddScoped<CustomActionFilter>();
                //services.AddSingleton<ICheckPendingQueue, CheckPendingQueue>();

                // Init Validator
                services.AddSingleton<IValidator<API1NewElectronicPutThroughRequest>, API1NewElectronicPutThroughValidator>();
                services.AddSingleton<IValidator<API2AcceptElectronicPutThroughRequest>, API2AcceptElectronicPutThroughValidator>();
                services.AddSingleton<IValidator<API3ReplaceElectronicPutThroughRequest>, API3ReplaceElectronicPutThroughValidator>();
                services.AddSingleton<IValidator<API4CancelElectronicPutThroughRequest>, API4CancelElectronicPutThroughValidator>();
                services.AddSingleton<IValidator<API5NewCommonPutThroughRequest>, API5NewCommonPutThroughValidator>();
                services.AddSingleton<IValidator<API6AcceptCommonPutThroughRequest>, API6AcceptCommonPutThroughValidator>();
                //
                services.AddSingleton<IValidator<API7ReplaceCommonPutThroughRequest>, API7ReplaceCommonPutThroughValidator>();
                services.AddSingleton<IValidator<API8CancelCommonPutThroughRequest>, API8CancelCommonPutThroughValidator>();
                services.AddSingleton<IValidator<API9ReplaceCommonPutThroughDealRequest>, API9ReplaceCommonPutThroughDealValidator>();
                services.AddSingleton<IValidator<API10ResponseForReplacingCommonPutThroughDealRequest>, API10ResponseForReplacingCommonPutThroughDealValidator>();
                services.AddSingleton<IValidator<API11CancelCommonPutThroughDealRequest>, API11CancelCommonPutThroughDealValidator>();
                services.AddSingleton<IValidator<API12ResponseForCancelingCommonPutThroughDealRequest>, API12ResponseForCancelingCommonPutThroughDealValidator>();
                // API 13- API 16
                services.AddSingleton<IValidator<API13NewInquiryReposRequest>, API13NewInquiryReposValidator>();
                services.AddSingleton<IValidator<API14ReplaceInquiryReposRequest>, API14ReplaceInquiryReposValidator>();
                services.AddSingleton<IValidator<API15CancelInquiryReposRequest>, API15CancelInquiryReposValidator>();
                services.AddSingleton<IValidator<API16CloseInquiryReposRequest>, API16CloseInquiryReposValidator>();
                services.AddSingleton<IValidator<API17OrderNewFirmReposRequest>, API17OrderNewFirmReposValidator>();
                services.AddSingleton<IValidator<API18OrderReplaceFirmReposRequest>, API18OrderReplaceFirmReposValidator>();
                services.AddSingleton<IValidator<API19OrderCancelFirmReposRequest>, API19OrderCancelFirmReposValidator>();
                services.AddSingleton<IValidator<API20OrderConfirmFirmReposRequest>, API20OrderConfirmFirmReposValidator>();
                services.AddSingleton<IValidator<API21OrderNewReposCommonPutthroughRequest>, API21OrderNewReposCommonPutthroughValidator>();
                services.AddSingleton<IValidator<API22OrderConfirmReposCommonPutthroughRequest>, API22OrderConfirmReposCommonPutthroughValidator>();
                services.AddSingleton<IValidator<API23OrderReplaceReposCommonPutthroughRequest>, API23OrderReplaceReposCommonPutthroughValidator>();
                services.AddSingleton<IValidator<API24OrderCancelReposCommonPutthroughRequest>, API24OrderCancelReposCommonPutthroughValidator>();
                services.AddSingleton<IValidator<API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest>, API25OrderReplaceDeal1stTransactionReposCommonPutthroughValidator>();
                services.AddSingleton<IValidator<API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest>, API26OrderReplaceDeal1stTransactionReposCommonPutthroughValidator>();
                services.AddSingleton<IValidator<API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest>, API27OrderCancelDeal1stTransactionReposCommonPutthroughValidator>();
                services.AddSingleton<IValidator<API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest>, API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughValidator>();
                services.AddSingleton<IValidator<API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest>, API29OrderReplaceDeal2ndTransactionReposCommonPutthroughValidator>();
                services.AddSingleton<IValidator<API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest>, API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughValidator>();
                // Normal 31-33
                services.AddSingleton<IValidator<API31OrderNewAutomaticOrderMatchingRequest>, API31OrderNewAutomaticOrderMatchingValidator>();
                services.AddSingleton<IValidator<API32OrderReplaceAutomaticOrderMatchingRequest>, API32OrderReplaceAutomaticOrderMatchingValidator>();
                services.AddSingleton<IValidator<API33OrderCancelAutomaticOrderMatchingRequest>, API33OrderCancelAutomaticOrderMatchingValidator>();

                //
                if (ConfigData.ConnectExchange)
                {
                    CommonLib.Logger.log.Info("Run mode ConnectExchange =true");
                    services.AddHostedService<HNXTCPService>();
                }
                else
                {
                    CommonLib.Logger.log.Info("Run mode ConnectExchange =false");
                }

                CommonLib.Logger.log.Info("InitServices Sucessfull.");
            }
            catch (Exception ex)
            {
                CommonLib.Logger.log.Error(ex);
            }
        }
    }
}