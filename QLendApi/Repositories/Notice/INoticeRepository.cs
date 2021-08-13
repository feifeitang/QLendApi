using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface INoticeRepository
    {
        Task<Notice[]> GetListByForeignWorkerIdAsync(int Id);
    }
}