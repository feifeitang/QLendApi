using System.Threading.Tasks;

namespace QLendApi.Services
{
    public interface IPaymentService
    {
        Task<string> create(int amount);
    }
}