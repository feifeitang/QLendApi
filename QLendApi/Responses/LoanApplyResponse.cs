namespace QLendApi.Responses
{
    public class LoanApplyResponse : BaseResponse
    {
        public class DataStruct
        {
            public string LoanNumber { get; init; }
        }

        public DataStruct Data { get; init; }
    }
}