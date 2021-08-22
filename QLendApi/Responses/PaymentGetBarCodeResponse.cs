using QLendApi.Models;

namespace QLendApi.Responses
{
    public class PaymentGetBarCodeResponse : BaseResponse
    {
        public class PaymentGetBarCodeDataStruct
        {
            public string BarCode1 { get; set; }
            public string BarCode2 { get; set; }
            public string BarCode3 { get; set; }
        }
        public PaymentGetBarCodeDataStruct Data { get; init; }
    }
}