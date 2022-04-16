using System;
using System.IO;
using System.Net;
using System.Text;
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
        public void Send(string PhoneNumber, string content)
        {
            string requestUrl = $"https://api.kotsms.com.tw/kotsmsapi-1.php?username={_userName}&apikey={_password}&smbody={content}&dstaddr={PhoneNumber}";

            Console.WriteLine("requestUrl {0}", requestUrl);

            WebRequest request = WebRequest.Create(requestUrl);

            WebResponse response = request.GetResponse();

            Stream receiveStream = response.GetResponseStream();

            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

            StreamReader readStream = new StreamReader(receiveStream, encode);

            string data = readStream.ReadToEnd();

            // TODO: check the data if is error code
            Console.WriteLine("\r\n Send SMS Response: {0}", data);

            response.Close();

            receiveStream.Close();
        }
    }
}
