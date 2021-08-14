namespace QLendApi.Responses
{
    public class LoanApplyResponse : BaseResponse
    {
        public class LoanApplyDataStruct
        {
            public string LoanNumber { get; init; }
        }

        public LoanApplyDataStruct Data { get; init; }
    }
}