using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QLendApi.Dtos;

namespace QLendApi.Services
{
    public class SmsService : ISmsService
    {
        readonly ILogger<SmsService> _logger;
        readonly string _userName;
        readonly string _password;

        public SmsService(IOptions<SmsServiceOptions> options, ILogger<SmsService> logger)
        {
            _logger = logger;
            _userName = options.Value.UserName;
            _password = options.Value.Password;
        }
        public bool Send(string PhoneNumber, string content)
        {
            try
            {
                string requestUrl = $"https://api.kotsms.com.tw/kotsmsapi-1.php?username={_userName}&password={_password}&smbody={content}&dstaddr={PhoneNumber}";

                WebRequest request = WebRequest.Create(requestUrl);

                WebResponse response = request.GetResponse();

                Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                response.Close();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error sending notification");
                return false;
            }
        }
    }
}