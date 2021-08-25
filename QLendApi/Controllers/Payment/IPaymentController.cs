using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Dtos;
using QLendApi.Responses;

namespace QLendApi.Controllers
{
    public interface IPaymentController
    {
        Task<ActionResult<PaymentCreateResponse>> Create();
        Task<ActionResult<PaymentGetBarCodeResponse>> GetBarCode();
        IActionResult ReceiveBarCode([FromForm] EcpayCreateOrderDto ecpayReceivePaymentInfoDto);
        Task<ActionResult> CallBack();
    }
}