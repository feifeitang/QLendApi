namespace QLendApi.Responses
{
    public class OtpByPhoneNumberResponse : BaseResponse
    {
        public class DataStruct
        {
            public int Id { get; init; }

            public string UINo {get; init;}
        }

        public DataStruct Data { get; init; }
    }
}