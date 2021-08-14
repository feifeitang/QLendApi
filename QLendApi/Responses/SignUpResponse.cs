namespace QLendApi.Responses
{
    public class SignUpResponse : BaseResponse
    {
        public class DataStruct
        {
            public int Id { get; init; }
        }

        public DataStruct Data { get; init; }
    }
}