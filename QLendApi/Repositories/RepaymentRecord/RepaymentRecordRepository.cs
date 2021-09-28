using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public class RepaymentRecordRepository : IRepaymentRecordRepository
    {
        private readonly QLendDBContext _context;
        public RepaymentRecordRepository(QLendDBContext context)
        {
            _context = context;
        }
        public async Task<RepaymentRecord[]> GetByLoanNumberAsync(string loanNumber)
        {
            return await _context.RepaymentRecords.Where(el => el.LoanNumber == loanNumber).ToArrayAsync();
        }

        public async Task<RepaymentRecord> GetByRepaymentNumberAsync(string repaymentNumber)
        {
            return await _context.RepaymentRecords.FindAsync(repaymentNumber);
        }

        public async Task UpdateAsync(RepaymentRecord repaymentRecord)
        {
            _context.RepaymentRecords.Update(repaymentRecord);
            await _context.SaveChangesAsync();
        }

        // public async Task<bool> ReceiveRepaymentResult(string repaymentNumber)
        // {
        //     // bool b = await this.repaymentRecordRepository.GetByRepaymentNumberAsync(repaymentNumber);
        //     if (_context.RepaymentRecords.Where(el => el.RepaymentNumber == repaymentNumber).ToArrayAsync();)
        // }
    }
}