using QLendApi.Models;

namespace QLendApi.Responses
{
    public class NoticeListResponse : BaseResponse
    {
        public class NoticeListDataStruct
        {
            public Notice[] NoticeRecords { get; set; }
        }
        
        public NoticeListDataStruct Data { get; init; }
    }
}