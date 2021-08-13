using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Models;
using QLendApi.Repositories;

namespace QLendApi.Services
{
    public class LoanRecordService : ILoanRecordService
    {
        private readonly ILoanRecordRepository loanRecordRepository;

        private readonly double _expireHours;

        public LoanRecordService(ILoanRecordRepository loanRecordRepository)
        {
            this.loanRecordRepository = loanRecordRepository;
            this._expireHours = 3;
        }

        public async Task<LoanRecord> GetEditRecordByForeignWorkerId(int id)
        {
            var loanRecords = await loanRecordRepository.GetByForeignWorkerIdAsync(id);

            if (loanRecords == null)
            {
                return null;
            }
            else
            {
                return loanRecords.Where(e => e.State != 5 && CheckAppiyTimeIsVaild(e.CreateTime)).Single();
            }
        }

        private bool CheckAppiyTimeIsVaild(DateTime createTime)
        {
            // createTime need add _expirehours
            var expireTime = createTime.AddHours(this._expireHours);
            DateTime currentTime = DateTime.UtcNow;
            int result = DateTime.Compare(expireTime, currentTime);

            bool res = true;

            if (result < 0)
                res = false;

            return res;
        }
    }
}