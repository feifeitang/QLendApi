using QLendApi.Models;

namespace QLendApi.Responses
{
    public class LimitRecordResponse : BaseResponse
    {
        public class LimitRecordDataStruct
        {
            public LoanRecord LoanRecord { get; init; }
        }

        public LimitRecordDataStruct Data { get; init; }
    }
}