using System.Threading.Tasks;
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

        public async Task CreateLoanRecordAsync(LoanRecord loanRecord)
        {
            _conetxt.LoanRecords.Add(loanRecord);
            await _conetxt.SaveChangesAsync();
        }
    }
}