using QLendApi.Models;

namespace QLendApi.Dtos
{
    public class GetNoticeListResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Notice[] NoticeRecords { get; set; }
    }
}