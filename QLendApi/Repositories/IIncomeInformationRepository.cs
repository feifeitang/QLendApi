using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface IIncomeInformationRepository
    {
        Task CreatePersonalInfo2Async(IncomeInformation incomeInformation);

        Task <IncomeInformation> GetSalarybookAsync(string uino);
        Task UpdateSalarybookAsync(IncomeInformation incomeInformation);
    }
}