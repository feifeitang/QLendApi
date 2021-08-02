using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public class RepaymentRecordRepository : IRepaymentRecordRepository
    {
        private readonly QLendDBContext _conetxt;
        public RepaymentRecordRepository(QLendDBContext context)
        {
            _conetxt = context;
        }
        public async Task<RepaymentRecord[]> GetRepaymentRecordsByLoanNumberAsync(string loanNumber)
        {
            return await _conetxt.RepaymentRecords.Where(el => el.LoanNumber == loanNumber).ToArrayAsync();
        }
    }
}