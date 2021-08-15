using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public class NoticeRepository : INoticeRepository
    {
        private readonly QLendDBContext _context;
        public NoticeRepository(QLendDBContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Notice notice)
        {
            _context.Notices.Add(notice);
            await _context.SaveChangesAsync();
        }

        public async Task<Notice[]> GetListByForeignWorkerIdAsync(int Id)
        {
            return await _context.Notices.Where(el => el.ForeignWorkerId == Id).ToArrayAsync();
        }
    }

}