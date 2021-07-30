using System.Linq;
using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly QLendDBContext _context;

        public CertificateRepository(QLendDBContext context)
        {
            _context = context;
        }

        public bool CheckUINoExist(string uino)
        {
            return _context.Certificates.Any(e => e.Uino == uino);
        }

        public async Task CreateCertificateAsync(Certificate certificate)
        {
            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();
        }

        public async Task<Certificate> GetCertificateAsync(string uino)
        {
            return await _context.Certificates.FindAsync(uino);
        }

        public async Task UpdateCertificateAsync(Certificate certificate)
        {
            _context.Certificates.Update(certificate);

            await _context.SaveChangesAsync();
        }
    }
}