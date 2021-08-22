using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Responses;

namespace QLendApi.Controllers
{
    public interface IPaymentController
    {
        Task<ActionResult<PaymentCreateResponse>> Create();
        Task<ActionResult<PaymentGetBarCodeResponse>> GetBarCode();
        Task<ActionResult> ReceiveBarCode();
        Task<ActionResult> CallBack();
    }
}