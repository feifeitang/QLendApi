using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Dtos;
using QLendApi.Responses;

namespace QLendApi.Controllers
{
    public interface IPaymentController
    {
        Task<ActionResult<string>> Create();
        Task<ActionResult<PaymentGetBarCodeResponse>> GetBarCode();
        IActionResult ReceiveBarCode([FromForm] EcpayReceivePaymentInfoDto ecpayReceivePaymentInfoDto);
        Task<ActionResult> CallBack();
    }
}