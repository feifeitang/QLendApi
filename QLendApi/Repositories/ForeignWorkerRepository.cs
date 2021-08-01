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

        public async Task CreateForeignWorkerAsync(ForeignWorker foreignWorker)
        {
            _context.ForeignWorkers.Add(foreignWorker);
            await _context.SaveChangesAsync();
        }

        public async Task<ForeignWorker> GetForeignWorkerByIdAsync(int id)
        {
            return await _context.ForeignWorkers.FindAsync(id);
        }

        public async Task<ForeignWorker> GetForeignWorkerByUINoAsync(string uino)
        {
            return await _context.ForeignWorkers.FirstOrDefaultAsync(f => f.Uino == uino);
        }

        public async Task UpdateForeignWorkerAsync(ForeignWorker foreignWorker)
        {
            _context.ForeignWorkers.Update(foreignWorker);

            await _context.SaveChangesAsync();
        }

    }
}