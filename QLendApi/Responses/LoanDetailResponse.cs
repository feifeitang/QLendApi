using QLendApi.Models;

namespace QLendApi.Responses
{
    public class LoanDetailResponse : BaseResponse
    {
        public class DataStruct
        {
            public LoanRecord LoanRecord { get; set; }
            public RepaymentRecord[] RepaymentRecords { get; set; }
        }

        public DataStruct Data { get; init; }
    }
}