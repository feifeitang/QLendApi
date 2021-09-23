using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QLendApi.Dtos;
using QLendApi.Models;
using QLendApi.Repositories;
using restapi.Settings;
using System.Security.Cryptography;
using System.Text;

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
        private readonly IRepaymentRecordRepository repaymentRecordRepository;
        public EcpayService(
            IOptions<EcpayServiceOptions> options,
            ILogger<EcpayService> logger,
            IOptions<EcpaySettings> ecpaySettings,
            IPaymentRepository paymentRepository,
            IRepaymentRecordRepository repaymentRecordRepository
        )
        {
            this._logger = logger;

            this._HashIV = options.Value.HashIV;

            this._HashKey = options.Value.HashKey;

            this._MerchantID = options.Value.MerchantID;

            this._ecpaySettings = ecpaySettings.Value;

            this.paymentRepository = paymentRepository;

            this.repaymentRecordRepository = repaymentRecordRepository;
        }

        public async Task<string> create(string repaymentNumber)
        {
            var CurrentDate = DateTime.Now;

            var TradeNo = "QLend" + CurrentDate.ToString("yyyyMMddHHmmss");

            var repaymentRecord = await this.repaymentRecordRepository.GetByRepaymentNumberAsync(repaymentNumber);

            string htmlPage = null;

            EcpayCreateOrderDto ecpayCreateOrderDto = new EcpayCreateOrderDto();

            try
            {
                // 服務參數
                ecpayCreateOrderDto.MerchantID = _MerchantID;
                ecpayCreateOrderDto.ReturnURL = _ecpaySettings.ReceivePaymentResultUrl;
                ecpayCreateOrderDto.MerchantTradeNo = TradeNo;  // 廠商的交易編號
                ecpayCreateOrderDto.MerchantTradeDate = CurrentDate.ToString("yyyy/MM/dd HH:mm:ss");//廠商的交易時間
                ecpayCreateOrderDto.TotalAmount = repaymentRecord.RepaymentAmount;  // 交易總金額
                ecpayCreateOrderDto.TradeDesc = "desc";  // 交易描述
                ecpayCreateOrderDto.ChoosePayment = "BARCODE";
                ecpayCreateOrderDto.PaymentType = "aio";

                ecpayCreateOrderDto.EncryptType = 1;

                // 訂單的商品資料
                ecpayCreateOrderDto.ItemName = "item";

                ecpayCreateOrderDto.StoreExpireDate = 1;

                ecpayCreateOrderDto.PaymentInfoURL = _ecpaySettings.ReceivePaymentInfoUrl;

                var _CheckMacValue = GenCheckMacValueForCreate(ecpayCreateOrderDto);

                Console.WriteLine("CheckMacValue {0}", _CheckMacValue);

                ecpayCreateOrderDto.CheckMacValue = _CheckMacValue;

                Console.WriteLine("TradeNo {0}", TradeNo);

                Payment paymentData = new()
                {
                    MerchantTradeNo = TradeNo,
                    Amount = repaymentRecord.RepaymentAmount,
                    ReturnURL = _ecpaySettings.ReceivePaymentResultUrl,
                    PaymentInfoURL = _ecpaySettings.ReceivePaymentInfoUrl,
                    Status = 0,
                    CreateTime = CurrentDate,
                    RepaymentNumber = repaymentRecord.RepaymentNumber  // need to change to real RepaymentNumber
                };

                htmlPage = GenHtmlPage(ecpayCreateOrderDto);

                await this.paymentRepository.CreateAsync(paymentData);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error create ecpay order");
                return null;
            }
            return htmlPage;
        }

        public async Task<bool> ReceivePaymentInfo()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> ReceivePaymentResult(EcpayReceivePaymentResultDto ecpayReceivePaymentResultDto)
        {
            var _CheckMacValue = GenCheckMacValueForResult(ecpayReceivePaymentResultDto);
            Console.WriteLine("CheckMacValue {0}", _CheckMacValue);
            if (_CheckMacValue != ecpayReceivePaymentResultDto.CheckMacValue)
            {
                return false;
            }

            var payment = await this.paymentRepository.GetByMerchantTradeNoAsync(ecpayReceivePaymentResultDto.MerchantTradeNo);
            payment.Status = 1;
            await this.paymentRepository.UpdateAsync(payment);

            return true;
        }

        private string GenCheckMacValueForCreate(EcpayCreateOrderDto ecpayCreateOrderDto)
        {
            List<string> Arr = new List<string>();

            foreach (PropertyInfo prop in ecpayCreateOrderDto.GetType().GetProperties())
            {
                if (prop.Name != "CheckMacValue")
                {
                    Arr.Add($"{prop.Name}={prop.GetValue(ecpayCreateOrderDto, null)}");
                }

            }

            Arr.Sort();

            var s = String.Join("&", Arr);

            var newStr = $"HashKey={this._HashKey}&{s}&HashIV={this._HashIV}";

            var urlEncode = System.Web.HttpUtility.UrlEncode(newStr).ToLower();

            SHA256 mySHA256 = SHA256.Create();

            byte[] bytesString = Encoding.UTF8.GetBytes(urlEncode);

            byte[] hashValue = mySHA256.ComputeHash(bytesString);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashValue.Length; i++)
            {
                builder.Append(hashValue[i].ToString("x2"));
            }
            return builder.ToString().ToUpper();
        }

        private string GenCheckMacValueForResult(EcpayReceivePaymentResultDto ecpayReceivePaymentResultDto)
        {
            List<string> Arr = new List<string>();

            foreach (PropertyInfo prop in ecpayReceivePaymentResultDto.GetType().GetProperties())
            {
                if (prop.Name != "CheckMacValue")
                {
                    Arr.Add($"{prop.Name}={prop.GetValue(ecpayReceivePaymentResultDto, null)}");
                }

            }

            Arr.Sort();

            var s = String.Join("&", Arr);

            var newStr = $"HashKey={this._HashKey}&{s}&HashIV={this._HashIV}";

            var urlEncode = System.Web.HttpUtility.UrlEncode(newStr).ToLower();

            SHA256 mySHA256 = SHA256.Create();

            byte[] bytesString = Encoding.UTF8.GetBytes(urlEncode);

            byte[] hashValue = mySHA256.ComputeHash(bytesString);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashValue.Length; i++)
            {
                builder.Append(hashValue[i].ToString("x2"));
            }
            return builder.ToString().ToUpper();
        }
        private string GenHtmlPage(EcpayCreateOrderDto apiData)
        {
            try
            {
                var form = "";

                foreach (PropertyInfo prop in apiData.GetType().GetProperties())
                {
                    form += $"<input name='{prop.Name}' type='hidden' value='{prop.GetValue(apiData, null)}' />";
                }
                form += "<button type='hidden' id='submit'>submit</button>";

                var js = "(function () {document.getElementById('submit').click();})()";
                var html = $"<html xmlns='http://www.w3.org/1999/xhtml'><body><form action='{this._ecpaySettings.OrderCreateUrl}' method='post' ref='form'>{form}</form><script language='javascript' type='text/javascript'>{js}</script></body></html>";

                return html;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex {0}", ex);
                return null;
            }
        }
    }
}