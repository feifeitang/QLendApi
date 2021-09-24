using System;
using System.IO;
using System.Net;
using System.Text;
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

                Console.WriteLine("requestUrl {0}", requestUrl);

                WebRequest request = WebRequest.Create(requestUrl);
                
                request.Proxy="34.80.51.122:3128";

                WebResponse response = request.GetResponse();

                Stream receiveStream = response.GetResponseStream();

                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                StreamReader readStream = new StreamReader(receiveStream, encode);

                string data = readStream.ReadToEnd();

                Console.WriteLine("\r\nResponse stream received.");

                Console.WriteLine(data);

                response.Close();

                receiveStream.Close();

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
