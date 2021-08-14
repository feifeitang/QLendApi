namespace QLendApi.Responses
{
    public class LoginResponse : BaseResponse
    {
        public class DataStruct
        {
            public string Token { get; set; }
        }

        public DataStruct Data { get; init; }
    }
}