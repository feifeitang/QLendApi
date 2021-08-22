using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QLendApi.Dtos;
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
        public EcpayService(IOptions<EcpayServiceOptions> options, ILogger<EcpayService> logger,
        IOptions<EcpaySettings> ecpaySettings)
        {
            _logger = logger;

            _HashIV = options.Value.HashIV;

            _HashKey = options.Value.HashKey;

            _MerchantID = options.Value.MerchantID;

            this._ecpaySettings = ecpaySettings.Value;
        }

        public bool create(int amount)
        {
            throw new System.NotImplementedException();
        }

        public bool ReceivePaymentInfo()
        {
            throw new System.NotImplementedException();
        }

        public bool ReceivePaymentResult()
        {
            throw new System.NotImplementedException();
        }

        private WebResponse RequestToEcpay()
        {
            throw new System.NotImplementedException();
        }
    }
}