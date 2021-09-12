using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QLendApi.Models;
using QLendApi.Repositories;

namespace QLendApi.Repositories
{
    public class ForeignWorkerRepository : IForeignWorkerRepository
    {
        private readonly QLendDBContext _context;

        public ForeignWorkerRepository(QLendDBContext context)
        {
            _context = context;
        }

        public bool CheckPhoneNumberExist(string phoneNumber)
        {
            return _context.ForeignWorkers.Any(e => e.PhoneNumber == phoneNumber);
        }

    
        public async Task CreateAsync(ForeignWorker foreignWorker)
        {
            _context.ForeignWorkers.Add(foreignWorker);
            await _context.SaveChangesAsync();
        }

        public async Task<ForeignWorker> GetByIdAsync(int id)
        {
            return await _context.ForeignWorkers.FindAsync(id);
        }

        public async Task<ForeignWorker> GetByUINoAsync(string uino)
        {
            return await _context.ForeignWorkers.FirstOrDefaultAsync(f => f.Uino == uino);
        }

        public async Task<ForeignWorker> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.ForeignWorkers.FirstOrDefaultAsync(f => f.PhoneNumber == phoneNumber);
        }

        public async Task UpdateAsync(ForeignWorker foreignWorker)
        {
            _context.ForeignWorkers.Update(foreignWorker);
            await _context.SaveChangesAsync();
        }

        public async Task CreatePersonalInfo1Async(ForeignWorker foreignWorker)
        {
            _context.ForeignWorkers.Add(foreignWorker);
            await _context.SaveChangesAsync();
        }
    }
}