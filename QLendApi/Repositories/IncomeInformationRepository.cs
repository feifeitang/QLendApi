using System.Threading.Tasks;
using QLendApi.Models;
namespace QLendApi.Repositories
{
    public class IncomeInformationRepository : IIncomeInformationRepository
    {
        private readonly QLendDBContext _context;

        public IncomeInformationRepository(QLendDBContext context)
        {
            _context = context;
        }

        public async Task CreateIncomeInfoAsync(IncomeInformation incomeInformation)
        {
            _context.IncomeInformations.Add(incomeInformation);
            await _context.SaveChangesAsync();
        }

        public async Task<IncomeInformation> GetIncomeInfoByIncomeNumberAsync(int incomeNumber)
        {
            return await _context.IncomeInformations.FindAsync(incomeNumber);
        }

        public async Task UpdateIncomeInfoAsync(IncomeInformation incomeInformation)
        {
            _context.IncomeInformations.Update(incomeInformation);
            await _context.SaveChangesAsync();
        }
    }
}