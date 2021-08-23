using System.Threading.Tasks;

namespace QLendApi.Services
{
    public interface IEcpayService : IPaymentService
    {
        Task<bool> ReceivePaymentResult();
        Task<bool> ReceivePaymentInfo();
    }
}