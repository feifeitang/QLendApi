namespace QLendApi.Responses
{
    public class PreCreateResponse : BaseResponse
    {
        public class PreCreateDataStruct
        {
            public string NextApi { get; set; }
        }
        public PreCreateDataStruct Data { get; init; }
    }
}