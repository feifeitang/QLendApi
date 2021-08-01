using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface ILoanRecordRepository
    {
       Task<LoanRecord[]> GetLoanRecordByIdAndStatusAsync(int id, int status);

        Task CreateLoanRecordAsync(LoanRecord loanRecord);

    }
}