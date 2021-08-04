using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface ICertificateRepository
    {
        Task<Certificate> GetByUINoAsync(string uino);
        Task CreateAsync(Certificate certificate);
        Task UpdateAsync(Certificate certificate);

        Boolean CheckUINoExist(string uino);
    }
}