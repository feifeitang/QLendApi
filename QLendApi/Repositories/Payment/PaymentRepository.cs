using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly QLendDBContext _context;
        public PaymentRepository(QLendDBContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Payment payment)
        {
            _context.Payment.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<Payment> GetByIdAsync(int Id)
        {
            return await _context.Payment.SingleOrDefaultAsync(el => el.Id == Id);
        }
        public async Task<Payment> GetByMerchantTradeNoAsync(string MerchantTradeNo)
        {
            return await _context.Payment.SingleOrDefaultAsync(el => el.MerchantTradeNo == MerchantTradeNo);
        }

        public async Task<Payment> GetByRepaymentNumberAsync(string RepaymentNumber)
        {
            return await _context.Payment.SingleOrDefaultAsync(el => el.RepaymentNumber == RepaymentNumber);
        }

        public async Task UpdateAsync(Payment payment)
        {
            _context.Payment.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}