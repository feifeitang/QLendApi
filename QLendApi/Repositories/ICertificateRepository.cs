using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface ICertificateRepository
    {
        Task<Certificate> GetCertificateAsync(string uino);
        Task CreateCertificateAsync(Certificate certificate);
        Task UpdateCertificateAsync(Certificate certificate);

        Boolean CheckUINoExist(string uino);
    }
}