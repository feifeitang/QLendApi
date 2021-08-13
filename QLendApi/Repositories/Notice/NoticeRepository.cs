using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public class NoticeRepository : INoticeRepository
    {
        private readonly QLendDBContext _conetxt;
        public NoticeRepository(QLendDBContext context)
        {
            _conetxt = context;
        }
        public async Task<Notice[]> GetListByForeignWorkerIdAsync(int Id)
        {
            return await _conetxt.Notices.Where(el => el.ForeignWorkerId == Id).ToArrayAsync();
        }
    }

}