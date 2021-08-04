using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface ILoanRecordRepository
    {
       Task<LoanRecord[]> GetByIdAndStatusAsync(int id, int status);

       Task<LoanRecord> GetByLoanNumber(string loanNumber);

       Task CreateAsync(LoanRecord loanRecord);
    }
}