using QLendApi.Models;

namespace QLendApi.Responses
{
    public class LoanDetailResponse : BaseResponse
    {
        public class LoanDetailDataStruct
        {
            public LoanRecord LoanRecords { get; set; }
            public RepaymentRecord[] RepaymentRecords { get; set; }
            
        }

        public LoanDetailDataStruct Data { get; init; }
    }
}