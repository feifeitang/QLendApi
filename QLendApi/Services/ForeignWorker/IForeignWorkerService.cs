using System.Threading;
using System.Threading.Tasks;
using QLendApi.Dtos;
using QLendApi.Models;
using QLendApi.Settings;

namespace QLendApi.Services
{
    public interface IForeignWorkerService
    {
        Task<ForeignWorker> GetInfoByAuthOrNull(string uino, string password);
    }
}