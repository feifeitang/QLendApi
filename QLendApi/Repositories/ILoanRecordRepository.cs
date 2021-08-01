using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface ILoanRecordRepository
    {
       Task CreateLoanApplyAsync(LoanRecord loanRecord);

       
    }
}