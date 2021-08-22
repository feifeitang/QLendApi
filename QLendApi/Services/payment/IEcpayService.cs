using System.Threading.Tasks;

namespace QLendApi.Services
{
    public interface IEcpayService : IPaymentService
    {
        bool ReceivePaymentResult();
        bool ReceivePaymentInfo();
    }
}