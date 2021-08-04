using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public class LoanRecordRepository : ILoanRecordRepository
    {
        private readonly QLendDBContext _context;
        public LoanRecordRepository(QLendDBContext context)
        {
            _context = context;
        }

        public async Task CreateLoanRecordAsync(LoanRecord loanRecord)
        {
            _context.LoanRecords.Add(loanRecord);
            await _context.SaveChangesAsync();
        }


        public async Task<LoanRecord[]> GetLoanRecordsByIdAndStatusAsync(int id, int status)
        {
            return await _context.LoanRecords.Where(el => el.Id == id && el.Status == status).ToArrayAsync();
        }
        
        public async Task<LoanRecord> GetLoanRecordByLoanNumber(string loanNumber)
        {
            return await _context.LoanRecords.FindAsync(loanNumber);
        }
    }
}