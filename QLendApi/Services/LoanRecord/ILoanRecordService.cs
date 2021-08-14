using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Services
{
    public interface ILoanRecordService
    {
        Task<LoanRecord> GetEditRecordByForeignWorkerId(int id);
    }
}