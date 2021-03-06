using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface ILoanRecordRepository
    {
       Task<LoanRecord[]> GetByForeignWorkerIdAsync(int id);

       Task<LoanRecord[]> GetByForeignWorkerIdAndStatusAsync(int id, int status);

       Task<LoanRecord> GetByIdAndStateAsync(int id, int state);

       Task<LoanRecord> GetByLoanNumber(string loanNumber);

       Task CreateAsync(LoanRecord loanRecord);

       Task UpdateAsync(LoanRecord loanRecord);
    }
}