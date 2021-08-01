using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public class LoanRecordRepository : ILoanRecordRepository
    {
        private readonly QLendDBContext _conetxt;
        public LoanRecordRepository(QLendDBContext context)
        {
            _conetxt = context;
        }
        
        public async Task CreateLoanApplyAsync(LoanRecord loanRecord)
        {
            _conetxt.LoanRecords.Add(loanRecord);
            await _conetxt.SaveChangesAsync();
        }

        public async Task<LoanRecord[]> GetLoanRecordByIdAndStatusAsync(int id, int status)
        {
            return await _conetxt.LoanRecords.Where(el => el.Id == id && el.Status == status).ToArrayAsync();
        }
    }
}