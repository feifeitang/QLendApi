using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface ILoanRecordRepository
    {
       Task<LoanRecord[]> GetLoanRecordsByIdAndStatusAsync(int id, int status);

       Task<LoanRecord> GetLoanRecordByLoanNumber(string loanNumber);

       Task CreateLoanRecordAsync(LoanRecord loanRecord);
    }
}