using System;
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

        public async Task CreateAsync(LoanRecord loanRecord)
        {
            _context.LoanRecords.Add(loanRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<LoanRecord[]> GetByIdAndStatusAsync(int id, int status)
        {
            return await _context.LoanRecords.Where(el => el.Id == id && el.Status == status).ToArrayAsync();
        }
/*
        public async Task<LoanRecord> GetByIdAndStateAsync(int id, int state)
        {
            return await _context.LoanRecords.SingleOrDefaultAsync(e => e.Id == id && e.State == 0);
        }
       
        public async Task<LoanRecord> GetByIdAsync(int id, int State)
        {           
            return await _context.LoanRecords.SingleOrDefaultAsync(e => e.Id == id && e.State == State);
        }
 */       
        public async Task<LoanRecord> GetByLoanNumber(string loanNumber)
        {
            return await _context.LoanRecords.FindAsync(loanNumber);
        }

        public async Task UpdateAsync(LoanRecord loanRecord)
        {
            _context.LoanRecords.Update(loanRecord);
            await _context.SaveChangesAsync();
        }
    }
}