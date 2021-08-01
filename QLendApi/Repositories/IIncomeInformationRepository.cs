using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface IIncomeInformationRepository
    {
        Task <IncomeInformation> GetIncomeInfoByIncomeNumberAsync(int incomeNumber);

        Task CreateIncomeInfoAsync(IncomeInformation incomeInformation);

        Task UpdateIncomeInfoAsync(IncomeInformation incomeInformation);
    }
}