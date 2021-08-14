using QLendApi.Models;

namespace QLendApi.Responses
{
    public class LoanListResponse : BaseResponse
    {
        public class DataStruct
        {
            public LoanRecord[] LoanRecords { get; set; }
        }

        public DataStruct Data { get; init; }
    }
}