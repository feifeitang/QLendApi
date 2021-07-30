using QLendApi.Models;

namespace QLendApi.Dtos
{
    public class GetForeignWorkerInfoResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ForeignWorker Info { get; set; }
    }
}