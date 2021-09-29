using System;

namespace QLendApi.Responses
{
    public class GetBarCodeCreateTimeResponse : BaseResponse
    {
        public class GetBarCodeCreateTimeDataStruct
        {
            public DateTime? BarCodeCreateTime { get; set; }
        }
        public GetBarCodeCreateTimeDataStruct Data { get; init; }
    }
}