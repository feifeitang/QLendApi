using System.Threading.Tasks;
using QLendApi.Dtos;

namespace QLendApi.Services
{
    public interface IEcpayService
    {
        Task<string> create(int amount);
        Task<bool> ReceivePaymentResult(EcpayReceivePaymentResultDto ecpayReceivePaymentResultDto);
        Task<bool> ReceivePaymentInfo();
    }
}