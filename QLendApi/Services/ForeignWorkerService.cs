using System.Threading.Tasks;
using QLendApi.lib;
using QLendApi.Models;
using QLendApi.Repositories;

namespace QLendApi.Services
{
    public interface IForeignWorkerService
    {
        Task<ForeignWorker> GetInfoByAuthOrNull(string uino, string password);
    }
    public class ForeignWorkerService : IForeignWorkerService
    {
        private readonly IForeignWorkerRepository foreignWorkerRepository;

        public ForeignWorkerService(IForeignWorkerRepository foreignWorkerRepository)
        {
            this.foreignWorkerRepository = foreignWorkerRepository;
        }

        public async Task<ForeignWorker> GetInfoByAuthOrNull(string uino, string password)
        {
            var foreignWorker = await foreignWorkerRepository.GetForeignWorkerByUINoAsync(uino);

            if (foreignWorker == null)
            {
                return null;
            }
            if (Crypt.VerifyHash(password, foreignWorker.Password))
            {
                return foreignWorker;
            }

            return null;
        }

    }
}