using QLendApi.lib;
using QLendApi.Models;

namespace QLendApi.Dtos
{
    public class GetLoanEditRecordResponse : BaseResponse
    {
        public class DataStruct
        {
            public LoanRecord LoanRecord { get; init; }
        }

        public DataStruct Data { get; init; }
    }
}