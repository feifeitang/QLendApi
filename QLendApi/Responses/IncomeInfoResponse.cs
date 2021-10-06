namespace QLendApi.Responses
{
    public class IncomeInfoResponse : BaseResponse
    {
        public class DataStruct
        {
            public int PayDay { get; init; }
        }

        public DataStruct Data { get; init; }
    }
}