using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface IForeignWorkerRepository
    {
        Task<ForeignWorker> GetByIdAsync(int id);

        Task<ForeignWorker> GetByUINoAsync(string uino);

        Task<ForeignWorker> GetByPhoneNumberAsync(string phoneNumber);

        Task CreateAsync(ForeignWorker foreignWorker);
        
        Task UpdateAsync(ForeignWorker user);

        Task CreatePersonalInfo1Async(ForeignWorker foreignWorker);

        Boolean CheckPhoneNumberExist(string phoneNumber);
    }
}