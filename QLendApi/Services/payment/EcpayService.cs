using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ECPay.Payment.Integration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QLendApi.Dtos;
using QLendApi.Models;
using QLendApi.Repositories;
using restapi.Settings;

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

            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    /* 服務參數 */
                    oPayment.ServiceMethod = HttpMethod.HttpPOST;//介接服務時，呼叫 API 的方法
                    oPayment.ServiceURL = _ecpaySettings.OrderCreateUrl;
                    oPayment.HashKey = _HashKey;
                    oPayment.HashIV = _HashIV;
                    oPayment.MerchantID = _MerchantID;

                    /* 基本參數 */
                    oPayment.Send.ReturnURL = _ecpaySettings.ReceivePaymentResultUrl;//付款完成通知回傳的網址
                    // oPayment.Send.ClientBackURL = "http://www.ecpay.com.tw/";//瀏覽器端返回的廠商網址
                    // oPayment.Send.OrderResultURL = "http://localhost:52413/CheckOutFeedback.aspx";//瀏覽器端回傳付款結果網址
                    oPayment.Send.MerchantTradeNo = TradeNo;//廠商的交易編號
                    oPayment.Send.MerchantTradeDate = CurrentDate.ToString("yyyy/MM/dd HH:mm:ss");//廠商的交易時間
                    oPayment.Send.TotalAmount = amount;//交易總金額
                    oPayment.Send.TradeDesc = "交易描述";//交易描述
                    oPayment.Send.ChoosePayment = PaymentMethod.BARCODE;//使用的付款方式
                                                                        // oPayment.Send.Remark = "";//備註欄位
                                                                        // oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;//使用的付款子項目
                                                                        // oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.Yes;//是否需要額外的付款資訊
                                                                        // oPayment.Send.DeviceSource = null;//來源裝置
                                                                        // oPayment.Send.IgnorePayment = ""; //不顯示的付款方式
                                                                        // oPayment.Send.PlatformID = "";//特約合作平台商代號
                                                                        // oPayment.Send.CustomField1 = "";
                                                                        // oPayment.Send.CustomField2 = "";
                                                                        // oPayment.Send.CustomField3 = "";
                                                                        // oPayment.Send.CustomField4 = "";
                    oPayment.Send.EncryptType = 1;

                    //訂單的商品資料
                    oPayment.Send.Items.Add(new Item() { Name = "商品" });


                    // #region CVS 額外功能參數

                    oPayment.SendExtend.StoreExpireDate = 1; //超商繳費截止時間 CVS:以分鐘為單位 BARCODE:以天為單位
                                                             // oPayment.SendExtend.Desc_1 = "test1";//交易描述 1
                                                             // oPayment.SendExtend.Desc_2 = "test2";//交易描述 2
                                                             // oPayment.SendExtend.Desc_3 = "test3";//交易描述 3
                                                             // oPayment.SendExtend.Desc_4 = "";//交易描述 4
                    oPayment.SendExtend.PaymentInfoURL = _ecpaySettings.ReceivePaymentInfoUrl;//伺服器端回傳付款相關資訊
                                                                                              // oPayment.SendExtend.ClientRedirectURL = "";///Client 端回傳付款相關資訊

                    /* 產生訂單 */
                    oPayment.CheckOut();

                    var s = JsonConvert.SerializeObject(oPayment.Send);

                    Console.WriteLine(s);
                    Console.WriteLine(TradeNo);
                    // Console.WriteLine(str);
                }

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

        private WebResponse RequestToEcpay()
        {
            throw new System.NotImplementedException();
        }
    }
}