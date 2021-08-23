using System.Threading.Tasks;
using QLendApi.Models;

namespace QLendApi.Repositories
{
    public interface IPaymentRepository
    {
        Task CreateAsync(Payment payment);
        Task UpdateAsync(Payment payment);
        Task<Payment> GetByMerchantTradeNoAsync(string MerchantTradeNo);
        Task<Payment> GetByIdAsync(int Id);
        Task<Payment> GetByRepaymentNumberAsync(string RepaymentNumber);
    }
}