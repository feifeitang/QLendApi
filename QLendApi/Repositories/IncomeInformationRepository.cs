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

        public async Task CreatePersonalInfo2Async(IncomeInformation incomeInformation)
        {
            _context.IncomeInformations.Add(incomeInformation);
            await _context.SaveChangesAsync();
        }

        public async Task<IncomeInformation> GetSalarybookAsync(string uino)
        {
            return await _context.IncomeInformations.FindAsync(uino);
        }

        public async Task UpdateSalarybookAsync(IncomeInformation incomeInformation)
        {
            _context.IncomeInformations.Update(incomeInformation);
            await _context.SaveChangesAsync();
        }
    }
}