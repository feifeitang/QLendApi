using QLendApi.Models;

namespace QLendApi.Responses
{
    public class NoticeListResponse : BaseResponse
    {
        public class DataStruct
        {
            public Notice[] NoticeRecords { get; set; }
        }
        
        public DataStruct Data { get; init; }
    }
}