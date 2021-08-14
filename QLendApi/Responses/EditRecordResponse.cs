using QLendApi.Models;

namespace QLendApi.Responses
{
    public class EditRecordResponse : BaseResponse
    {
        public class EditRecordDataStruct
        {
            public LoanRecord LoanRecord { get; init; }
        }

        public EditRecordDataStruct Data { get; init; }
    }
}