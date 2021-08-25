using System.Threading.Tasks;

namespace QLendApi.Services
{
    public interface IPaymentService
    {
        Task<bool> create(int amount);
    }
}