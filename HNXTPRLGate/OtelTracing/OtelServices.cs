using BusinessProcessAPIReq.RequestModels;
using CommonLib;
using Confluent.Kafka.Extensions.OpenTelemetry;
using Newtonsoft.Json;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using static CommonLib.CommonDataInCore;

namespace HNXTPRLGate.OtelTracing
{
    public static class OtelServices
    {
        private static bool SensitiveHeaderMatches(string header, IEnumerable<string> patterns)
        {
            return patterns.Any(pattern => IsWildcardMatch(header, pattern));
        }

        private static bool IsWildcardMatch(string text, string pattern)
        {
            // Convert wildcard pattern to regex pattern
            string regexPattern = "^" + Regex.Escape(pattern).Replace(@"\*", ".*") + "$";
            return Regex.IsMatch(text, regexPattern, RegexOptions.IgnoreCase);
        }

        private static HttpRequestHeaders CleanHeaders(HttpRequestHeaders headers)
        {
            string[] sensitiveHeaders = { "cookie", "authorization", "proxyAuthorization", "cookie", "x*", "setCookie*" };
            foreach (var header in headers.ToArray())
            {
                if (SensitiveHeaderMatches(header.Key, sensitiveHeaders))
                {
                    headers.Remove(header.Key);
                    Logger.log.Info($"Sensitive header '{header.Key}' removed successfully.");
                    //Console.WriteLine($"Sensitive header '{header.Key}' removed successfully.");
                }
            }
            return headers;
        }

        //private static List<KeyValuePair<string, IEnumerable<string>>> CleanHeaders(HttpRequestHeaders headers)
        //{
        //    List<KeyValuePair<string, IEnumerable<string>>> cleanHeaders = new();

        //    string[] sensitiveHeaders = { "cookie", "authorization", "proxyAuthorization", "cookie", "x*", "setCookie*" };
        //    foreach (var header in headers.ToArray())
        //    {
        //        if (!SensitiveHeaderMatches(header.Key, sensitiveHeaders))
        //        {
        //            cleanHeaders.Add(new(header.Key, header.Value));
        //        }
        //    }
        //    return cleanHeaders;
        //}

        private static string CleanURL(Uri? inputUri)
        {
            var result = "";
            if (inputUri != null)
            {
                return inputUri.Scheme + "://" + inputUri.Host + inputUri.AbsolutePath;
            }
            return result;
        }

        public static IServiceCollection RegisterOpentelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new OtelAppSettings();
            configuration.GetSection(options.SectionName).Bind(options);
            var serviceName = options.ServiceName;
            var otelEndpoint = options.OtelEndpoint;
            var otelProtocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;

            if (options.EnableOtel)
            {
                //Console.WriteLine($"Register Opentelemetry {serviceName} [{otelEndpoint}] protocol[{otelProtocol}]");
                Logger.log.Info($"Register Opentelemetry {serviceName} [{otelEndpoint}] protocol[{otelProtocol}]");
                services.AddOpenTelemetry()
                    .ConfigureResource(resource => resource
                        .AddService(serviceName: serviceName))
                    .WithTracing(tracing => tracing
                        .AddSource(options.ServiceName)
                        .SetSampler(new AlwaysOnSampler())
                        .AddEntityFrameworkCoreInstrumentation()
                        .AddQuartzInstrumentation()
                        .AddConfluentKafkaInstrumentation()
                        .AddHttpClientInstrumentation(c =>
                        {
                            c.RecordException = true;
                            c.EnrichWithHttpRequestMessage = (a, b) =>
                            {
                                //a.AddTag("http.client.request.headers", CleanHeaders(b.Headers));
                                a.SetTag("http.url", CleanURL(b.RequestUri));
                            };
                            c.EnrichWithHttpResponseMessage = (a, b) =>
                            {
                                a.AddTag("http.client.status", b.StatusCode);
                            };
                        })
                        .AddAspNetCoreInstrumentation(c =>
                        {
                            c.RecordException = true;
                            c.EnrichWithHttpRequest = (activity, request) =>
                            {
                                try
                                {
                                    if (request.Method.ToUpper() == "GET")
                                    {
                                        activity.AddTag("OrderNo", request.Query["OrderNo"].ToString() ?? "null");
                                    }
                                    if (request.Method.ToUpper() == "POST")
                                    {
                                        long t1 = DateTime.Now.Ticks;
                                        //
                                        string gOrderNo = GetRequestBodyData(request).Result;
                                        activity.AddTag("OrderNo", gOrderNo ?? "null");
                                        //
                                        Logger.log.Debug($"Opentelemetry process getUrl={request.Path}, getMethod={request.Method.ToUpper()} , getOrderNo={gOrderNo}, Process time: {DateTime.Now.Ticks - t1}");
                                    }
                                }
                                catch { }
                            };
                            c.EnrichWithHttpResponse = (activity, response) =>
                            {
                                activity.AddTag("response.length", response.ContentLength);
                                activity.AddTag("response.status", response.StatusCode);
                            };
                        })
                        .AddOtlpExporter(c =>
                        {
                            c.Endpoint = new Uri(otelEndpoint);
                            c.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                            c.ExportProcessorType = OpenTelemetry.ExportProcessorType.Batch;
                        })
                        )
                    .WithMetrics(c =>
                    {
                        c.AddMeter(options.ServiceName);
                        c.AddRuntimeInstrumentation();
                        c.AddProcessInstrumentation();
                        c.AddAspNetCoreInstrumentation();
                        c.AddHttpClientInstrumentation();
                        c.AddOtlpExporter(c =>
                        {
                            c.Endpoint = new Uri(otelEndpoint);
                            c.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                            c.ExportProcessorType = OpenTelemetry.ExportProcessorType.Batch;
                        });
                    });
            }
            else
            {
                //Console.WriteLine($"Opentelemetry Disabled");
                Logger.log.Info($"Opentelemetry Disabled");
            }
            return services;
        }

        public static async Task<string> GetRequestBodyData(HttpRequest request)
        {
            try
            {
                string getContentRequest = string.Empty;
                using (var reader = new StreamReader(request.Body, encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false))
                {
                    getContentRequest = await reader.ReadToEndAsync();
                    //
                    Stream bodyStream = new MemoryStream(Encoding.UTF8.GetBytes(getContentRequest));
                    request.Body = bodyStream;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_1, StringComparison.OrdinalIgnoreCase))
                {
                    API1NewElectronicPutThroughRequest objData = JsonConvert.DeserializeObject<API1NewElectronicPutThroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_2, StringComparison.OrdinalIgnoreCase))
                {
                    API2AcceptElectronicPutThroughRequest objData = JsonConvert.DeserializeObject<API2AcceptElectronicPutThroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_3, StringComparison.OrdinalIgnoreCase))
                {
                    API3ReplaceElectronicPutThroughRequest objData = JsonConvert.DeserializeObject<API3ReplaceElectronicPutThroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_4, StringComparison.OrdinalIgnoreCase))
                {
                    API4CancelElectronicPutThroughRequest objData = JsonConvert.DeserializeObject<API4CancelElectronicPutThroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_5, StringComparison.OrdinalIgnoreCase))
                {
                    API5NewCommonPutThroughRequest objData = JsonConvert.DeserializeObject<API5NewCommonPutThroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_6, StringComparison.OrdinalIgnoreCase))
                {
                    API6AcceptCommonPutThroughRequest objData = JsonConvert.DeserializeObject<API6AcceptCommonPutThroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_7, StringComparison.OrdinalIgnoreCase))
                {
                    API7ReplaceCommonPutThroughRequest objData = JsonConvert.DeserializeObject<API7ReplaceCommonPutThroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_8, StringComparison.OrdinalIgnoreCase))
                {
                    API8CancelCommonPutThroughRequest objData = JsonConvert.DeserializeObject<API8CancelCommonPutThroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_9, StringComparison.OrdinalIgnoreCase))
                {
                    API9ReplaceCommonPutThroughDealRequest objData = JsonConvert.DeserializeObject<API9ReplaceCommonPutThroughDealRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_10, StringComparison.OrdinalIgnoreCase))
                {
                    API10ResponseForReplacingCommonPutThroughDealRequest objData = JsonConvert.DeserializeObject<API10ResponseForReplacingCommonPutThroughDealRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_11, StringComparison.OrdinalIgnoreCase))
                {
                    API11CancelCommonPutThroughDealRequest objData = JsonConvert.DeserializeObject<API11CancelCommonPutThroughDealRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_12, StringComparison.OrdinalIgnoreCase))
                {
                    API12ResponseForCancelingCommonPutThroughDealRequest objData = JsonConvert.DeserializeObject<API12ResponseForCancelingCommonPutThroughDealRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_13, StringComparison.OrdinalIgnoreCase))
                {
                    API13NewInquiryReposRequest objData = JsonConvert.DeserializeObject<API13NewInquiryReposRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_14, StringComparison.OrdinalIgnoreCase))
                {
                    API14ReplaceInquiryReposRequest objData = JsonConvert.DeserializeObject<API14ReplaceInquiryReposRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_15, StringComparison.OrdinalIgnoreCase))
                {
                    API15CancelInquiryReposRequest objData = JsonConvert.DeserializeObject<API15CancelInquiryReposRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_16, StringComparison.OrdinalIgnoreCase))
                {
                    API16CloseInquiryReposRequest objData = JsonConvert.DeserializeObject<API16CloseInquiryReposRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_17, StringComparison.OrdinalIgnoreCase))
                {
                    API17OrderNewFirmReposRequest objData = JsonConvert.DeserializeObject<API17OrderNewFirmReposRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_18, StringComparison.OrdinalIgnoreCase))
                {
                    API18OrderReplaceFirmReposRequest objData = JsonConvert.DeserializeObject<API18OrderReplaceFirmReposRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_19, StringComparison.OrdinalIgnoreCase))
                {
                    API19OrderCancelFirmReposRequest objData = JsonConvert.DeserializeObject<API19OrderCancelFirmReposRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_20, StringComparison.OrdinalIgnoreCase))
                {
                    API20OrderConfirmFirmReposRequest objData = JsonConvert.DeserializeObject<API20OrderConfirmFirmReposRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_21, StringComparison.OrdinalIgnoreCase))
                {
                    API21OrderNewReposCommonPutthroughRequest objData = JsonConvert.DeserializeObject<API21OrderNewReposCommonPutthroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_22, StringComparison.OrdinalIgnoreCase))
                {
                    API22OrderConfirmReposCommonPutthroughRequest objData = JsonConvert.DeserializeObject<API22OrderConfirmReposCommonPutthroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_23, StringComparison.OrdinalIgnoreCase))
                {
                    API23OrderReplaceReposCommonPutthroughRequest objData = JsonConvert.DeserializeObject<API23OrderReplaceReposCommonPutthroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_24, StringComparison.OrdinalIgnoreCase))
                {
                    API24OrderCancelReposCommonPutthroughRequest objData = JsonConvert.DeserializeObject<API24OrderCancelReposCommonPutthroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_25, StringComparison.OrdinalIgnoreCase))
                {
                    API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest objData = JsonConvert.DeserializeObject<API25OrderReplaceDeal1stTransactionReposCommonPutthroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_26, StringComparison.OrdinalIgnoreCase))
                {
                    API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest objData = JsonConvert.DeserializeObject<API26OrderReplaceDeal1stTransactionReposCommonPutthroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_27, StringComparison.OrdinalIgnoreCase))
                {
                    API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest objData = JsonConvert.DeserializeObject<API27OrderCancelDeal1stTransactionReposCommonPutthroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_28, StringComparison.OrdinalIgnoreCase))
                {
                    API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest objData = JsonConvert.DeserializeObject<API28OrderConfirmCancelDeal1stTransactionReposCommonPutthroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_29, StringComparison.OrdinalIgnoreCase))
                {
                    API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest objData = JsonConvert.DeserializeObject<API29OrderReplaceDeal2ndTransactionReposCommonPutthroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_30, StringComparison.OrdinalIgnoreCase))
                {
                    API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest objData = JsonConvert.DeserializeObject<API30OrderConfirmReplaceDeal2ndTransactionReposCommonPutthroughRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_31, StringComparison.OrdinalIgnoreCase))
                {
                    API31OrderNewAutomaticOrderMatchingRequest objData = JsonConvert.DeserializeObject<API31OrderNewAutomaticOrderMatchingRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_32, StringComparison.OrdinalIgnoreCase))
                {
                    API32OrderReplaceAutomaticOrderMatchingRequest objData = JsonConvert.DeserializeObject<API32OrderReplaceAutomaticOrderMatchingRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                //
                if (request.Path.Equals(Const_APIURL.API_BASE + Const_APIURL.API_33, StringComparison.OrdinalIgnoreCase))
                {
                    API33OrderCancelAutomaticOrderMatchingRequest objData = JsonConvert.DeserializeObject<API33OrderCancelAutomaticOrderMatchingRequest>(getContentRequest);
                    return objData.OrderNo;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}