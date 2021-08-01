using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface ILoanRecordRepository
    {
        Task CreateLoanRecordAsync(LoanRecord loanRecord);
    }
}