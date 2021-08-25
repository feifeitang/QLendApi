using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using ECPay.Payment.Integration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QLendApi.Dtos;
using QLendApi.Models;
using QLendApi.Repositories;
using restapi.Settings;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Http;
using System.Net.Http.Headers;

namespace QLendApi.Services
{
    public class EcpayService : IEcpayService
    {
        readonly private string _HashIV;
        readonly private string _HashKey;
        readonly private string _MerchantID;
        readonly ILogger<EcpayService> _logger;
        private readonly EcpaySettings _ecpaySettings;
        private readonly IPaymentRepository paymentRepository;
        public EcpayService(IOptions<EcpayServiceOptions> options, ILogger<EcpayService> logger,
        IOptions<EcpaySettings> ecpaySettings, IPaymentRepository paymentRepository)
        {
            this._logger = logger;

            this._HashIV = options.Value.HashIV;

            this._HashKey = options.Value.HashKey;

            this._MerchantID = options.Value.MerchantID;

            this._ecpaySettings = ecpaySettings.Value;

            this.paymentRepository = paymentRepository;
        }

        public async Task<bool> create(int amount)
        {
            List<string> enErrors = new List<string>();

            var CurrentDate = DateTime.Now;

            var TradeNo = "QLend" + CurrentDate.ToString("yyyyMMddHHmmss");

            EcpayCreateOrderDto ecpayCreateOrderDto = new EcpayCreateOrderDto();

            try
            {
                /* 服務參數 */
                ecpayCreateOrderDto.MerchantID = _MerchantID;
                ecpayCreateOrderDto.ReturnURL = _ecpaySettings.ReceivePaymentResultUrl;
                ecpayCreateOrderDto.MerchantTradeNo = TradeNo;//廠商的交易編號
                ecpayCreateOrderDto.MerchantTradeDate = CurrentDate.ToString("yyyy/MM/dd HH:mm:ss");//廠商的交易時間
                ecpayCreateOrderDto.TotalAmount = amount;//交易總金額
                ecpayCreateOrderDto.TradeDesc = "交易描述";//交易描述
                ecpayCreateOrderDto.ChoosePayment = "BARCODE";
                ecpayCreateOrderDto.PaymentType = "aio";

                ecpayCreateOrderDto.EncryptType = 1;

                //訂單的商品資料
                ecpayCreateOrderDto.ItemName = "商品";

                ecpayCreateOrderDto.StoreExpireDate = 1;

                ecpayCreateOrderDto.PaymentInfoURL = _ecpaySettings.ReceivePaymentInfoUrl;

                var _CheckMacValue = GenCheckMacValue(ecpayCreateOrderDto);

                Console.WriteLine("CheckMacValue {0}", _CheckMacValue);

                ecpayCreateOrderDto.CheckMacValue = _CheckMacValue;

                Console.WriteLine("TradeNo {0}", TradeNo);

                Payment paymentData = new()
                {
                    MerchantTradeNo = TradeNo,
                    Amount = amount,
                    ReturnURL = _ecpaySettings.ReceivePaymentResultUrl,
                    PaymentInfoURL = _ecpaySettings.ReceivePaymentInfoUrl,
                    Status = 0,
                    CreateTime = CurrentDate,
                    RepaymentNumber = "tmp"
                    // need to change to real RepaymentNumber
                };

                var postResult = await FormDataPostAsync(ecpayCreateOrderDto);

                Console.WriteLine("postResult {0}", postResult);

                await this.paymentRepository.CreateAsync(paymentData);
            }
            catch (Exception e)
            {
                enErrors.Add(e.Message);
                _logger.LogError(e, "Unexpected error create ecpay order");
                return false;
            }
            finally
            {
                // 顯示錯誤訊息。
                if (enErrors.Count() > 0)
                {
                    string szErrorMessage = String.Join("\\r\\n", enErrors);
                    Console.WriteLine(szErrorMessage);
                }
            }
            return true;
        }

        public async Task<bool> ReceivePaymentInfo()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> ReceivePaymentResult()
        {
            throw new System.NotImplementedException();
        }

        private async void RequestToEcpay(EcpayCreateOrderDto data)
        {
            WebRequest request = WebRequest.Create(_ecpaySettings.OrderCreateUrl);

            request.Method = "POST";

            request.ContentType = "application/x-www-form-urlencoded";

            byte[] byteArray = ObjectToByteArray(data);

            request.ContentLength = byteArray.Length;

            Stream dataStream = await request.GetRequestStreamAsync();

            await dataStream.WriteAsync(byteArray, 0, byteArray.Length);

            dataStream.Close();

            Console.WriteLine("dataStream {0}", dataStream);

            WebResponse response = await request.GetResponseAsync();

            Stream receiveStream = response.GetResponseStream();

            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

            StreamReader readStream = new StreamReader(receiveStream, encode);

            string resData = await readStream.ReadToEndAsync();

            Console.WriteLine("\r\nResponse stream received.");

            Console.WriteLine("resData {0}", resData);

            response.Close();
        }
        private string GenCheckMacValue(EcpayCreateOrderDto ecpayCreateOrderDto)
        {
            List<string> Arr = new List<string>();

            foreach (PropertyInfo prop in ecpayCreateOrderDto.GetType().GetProperties())
            {
                if (prop.Name != "CheckMacValue")
                {
                    Arr.Add($"{prop.Name}={prop.GetValue(ecpayCreateOrderDto, null)}");
                }

            }

            Console.WriteLine("init arr {0}", Arr);

            Arr.Sort();

            Console.WriteLine("after arr {0}", Arr);

            var s = String.Join("&", Arr);

            var newStr = $"HashKey={this._HashKey}&{s}&HashIV={this._HashIV}";

            Console.WriteLine("newStr {0}", newStr);

            var str3 = newStr.ToLower();

            Console.WriteLine("str3 {0}", str3);

            var encode = System.Web.HttpUtility.UrlEncode(str3);

            Console.WriteLine("encode {0}", encode);

            SHA256 mySHA256 = SHA256.Create();

            byte[] bytesString = Encoding.UTF8.GetBytes(str3);

            byte[] hashValue = mySHA256.ComputeHash(bytesString);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashValue.Length; i++)
            {
                builder.Append(hashValue[i].ToString("x2"));
            }
            return builder.ToString().ToUpper();
        }
        private async Task<bool> FormDataPostAsync(EcpayCreateOrderDto apiData)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        HttpResponseMessage response = null;

                        // Accept 用於宣告客戶端要求服務端回應的文件型態 (底下兩種方法皆可任選其一來使用)
                        //client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Content-Type 用於宣告遞送給對方的文件型態
                        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

                        //// 方法一： 使用字串名稱用法
                        var formData = new FormUrlEncodedContent(new[] {
                           new KeyValuePair<string, string>("MerchantID", apiData.MerchantID),
                           new KeyValuePair<string, string>("TotalAmount", apiData.TotalAmount.ToString()),
                           new KeyValuePair<string, string>("MerchantTradeNo", apiData.MerchantTradeNo),
                           new KeyValuePair<string, string>("MerchantTradeDate", apiData.MerchantTradeDate),
                           new KeyValuePair<string, string>("PaymentType", apiData.PaymentType),
                           new KeyValuePair<string, string>("StoreExpireDate", apiData.StoreExpireDate.ToString()),
                           new KeyValuePair<string, string>("TradeDesc", apiData.TradeDesc),
                           new KeyValuePair<string, string>("ItemName", apiData.ItemName),
                           new KeyValuePair<string, string>("ReturnURL", apiData.ReturnURL),
                           new KeyValuePair<string, string>("ChoosePayment", apiData.ChoosePayment),
                           new KeyValuePair<string, string>("CheckMacValue", apiData.CheckMacValue),
                           new KeyValuePair<string, string>("EncryptType", apiData.EncryptType.ToString()),
                           new KeyValuePair<string, string>("PaymentInfoURL", apiData.PaymentInfoURL),
                        });

                        string json = JsonConvert.SerializeObject(apiData);
                        Console.WriteLine("json {0}", json);

                        // 方法二： 強型別用法
                        // https://docs.microsoft.com/zh-tw/dotnet/csharp/language-reference/keywords/nameof
                        //         Dictionary<string, string> formDataDictionary = new Dictionary<string, string>()
                        // {
                        //     {nameof(APIData.Ma), apiData.Id.ToString() },
                        //     {nameof(APIData.Name), apiData.Name },
                        //     {nameof(APIData.Filename), apiData.Filename }
                        // };

                        // https://msdn.microsoft.com/zh-tw/library/system.net.http.formurlencodedcontent(v=vs.110).aspx
                        // var formData = new FormUrlEncodedContent(formDataDictionary);

                        response = await client.PostAsync(this._ecpaySettings.OrderCreateUrl, formData);

                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode == true)
                            {
                                // 取得呼叫完成 API 後的回報內容
                                String strResult = await response.Content.ReadAsStringAsync();
                                Console.WriteLine("strResult {0}", strResult);

                                return true;
                            }
                            else
                            {
                                return false;
                                // fooAPIResult = new APIResult
                                // {
                                //     Success = false,
                                //     Message = string.Format("Error Code:{0}, Error Message:{1}", response.StatusCode, response.RequestMessage),
                                //     Payload = null,
                                // };
                            }
                        }
                        else
                        {
                            return false;
                            // fooAPIResult = new APIResult
                            // {
                            //     Success = false,
                            //     Message = "應用程式呼叫 API 發生異常",
                            //     Payload = null,
                            // };
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ex {0}", ex);
                    }
                }
            }

            return true;
        }
        public static byte[] ObjectToByteArray(Object obj)
        {
            // proper way to serialize object
            var objToString = System.Text.Json.JsonSerializer.Serialize(obj);
            // convert that that to string with ascii you can chose what ever encoding want
            return System.Text.Encoding.UTF8.GetBytes(objToString);
        }
    }
}