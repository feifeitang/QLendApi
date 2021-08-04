using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface IIncomeInformationRepository
    {
        Task <IncomeInformation> GetByIncomeNumberAsync(int incomeNumber);

        Task CreateAsync(IncomeInformation incomeInformation);

        Task UpdateAsync(IncomeInformation incomeInformation);
    }
}