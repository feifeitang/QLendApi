using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface IForeignWorkerRepository
    {
        Task<ForeignWorker> GetForeignWorkerByIdAsync(int id);

        Task<ForeignWorker> GetForeignWorkerByUINoAsync(string uino);

        Task CreateForeignWorkerAsync(ForeignWorker foreignWorker);
        
        Task UpdateForeignWorkerAsync(ForeignWorker user);

      //  Task GetForeignWorkerAsync(int id);

        Boolean CheckPhoneNumberExist(string phoneNumber);
    }
}