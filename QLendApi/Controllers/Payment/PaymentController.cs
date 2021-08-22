using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Models;
using QLendApi.Repositories;
using QLendApi.Responses;
using QLendApi.Services;

namespace QLendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase, IPaymentController
    {
        private readonly IEcpayService ecpayService;
        public PaymentController(IEcpayService ecpayService)
        {
            this.ecpayService = ecpayService;
        }

        [Authorize]
        // POST /api/payment/Create
        [Route("Create")]
        [HttpPost]
        public Task<ActionResult<PaymentCreateResponse>> Create()
        {
            throw new System.NotImplementedException();
        }

        [Authorize]
        // POST /api/payment/GetBarCode
        [Route("GetBarCode")]
        [HttpPost]
        public Task<ActionResult<PaymentGetBarCodeResponse>> GetBarCode()
        {
            throw new System.NotImplementedException();
        }

        // POST /api/payment/ReceiveBarCode
        [Route("ReceiveBarCode")]
        [HttpPost]
        public Task<ActionResult> ReceiveBarCode()
        {
            throw new System.NotImplementedException();
        }

        // POST /api/payment/CallBack
        [Route("CallBack")]
        [HttpPost]
        public Task<ActionResult> CallBack()
        {
            throw new System.NotImplementedException();
        }
    }
}