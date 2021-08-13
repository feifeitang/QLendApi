using QLendApi.lib;

namespace QLendApi.Dtos
{
    public class NotFinishSignupResponse : BaseResponse
    {
        public class DataStruct
        {
            public int ForeignWorkerId { get; set; }
            public int NextStatus { get; set; }

        }
        public DataStruct Data { get; set; }
    }
}