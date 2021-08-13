using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Services
{
    public interface IForeignWorkerService
    {
        Task<ForeignWorker> GetInfoByAuthOrNull(string uino, string password);
    }
}