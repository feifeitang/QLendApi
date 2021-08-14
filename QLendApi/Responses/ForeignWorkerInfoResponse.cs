using QLendApi.Models;

namespace QLendApi.Responses
{
    public class ForeignWorkerInfoResponse : BaseResponse
    {
        public class DataStruct
        {
            public ForeignWorker Info { get; set; }
        }

        public DataStruct Data { get; init; }
    }
}