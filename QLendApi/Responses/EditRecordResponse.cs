using QLendApi.Models;

namespace QLendApi.Responses
{
    public class EditRecordResponse : BaseResponse
    {
        public class DataStruct
        {
            public LoanRecord LoanRecord { get; init; }
        }

        public DataStruct Data { get; init; }
    }
}