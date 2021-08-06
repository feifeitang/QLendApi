using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface ILoanRecordRepository
    {
       Task<LoanRecord[]> GetByIdAndStatusAsync(int id, int status);
    //   Task<LoanRecord> GetByIdAndStateAsync(int id, int state);

       Task<LoanRecord> GetByLoanNumber(string loanNumber);
      // Task<LoanRecord> GetByIdAsync(int id, int State);

       Task CreateAsync(LoanRecord loanRecord);
       Task UpdateAsync(LoanRecord loanRecord);
    }
}