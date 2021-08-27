using System.Threading.Tasks;

namespace QLendApi.Services
{
    public interface IEcpayService
    {
        Task<string> create(int amount);
        Task<bool> ReceivePaymentResult();
        Task<bool> ReceivePaymentInfo();
    }
}