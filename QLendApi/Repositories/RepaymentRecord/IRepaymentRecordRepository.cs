using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface IRepaymentRecordRepository
    {
        Task<RepaymentRecord[]> GetByLoanNumberAsync(string loanNumber);
    }
}