using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLendApi.Dtos;
using QLendApi.Models;
using QLendApi.Repositories;
using QLendApi.Responses;
using QLendApi.Services;

namespace QLendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IEcpayService ecpayService;
        private readonly IPaymentRepository paymentRepository;
        public PaymentController(IEcpayService ecpayService, IPaymentRepository paymentRepository)
        {
            this.ecpayService = ecpayService;
            this.paymentRepository = paymentRepository;
        }

        // [Authorize]
        // GET /api/payment/Create
        [Route("Create")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            try
            {
                var content = await this.ecpayService.create(2000);
                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = 200,
                    Content = content
                };
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);

                return new ContentResult
                {
                    StatusCode = 400,
                    Content = "<h1>Bad Request</h1>"
                };
            }
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
        [Consumes("application/x-www-form-urlencoded")]
        [HttpPost]
        public async Task<IActionResult> ReceiveBarCode([FromForm] EcpayReceivePaymentInfoDto ecpayReceivePaymentInfoDto)
        {
            try
            {
                foreach (PropertyInfo prop in ecpayReceivePaymentInfoDto.GetType().GetProperties())
                {
                    Console.WriteLine($"{prop.Name}: {prop.GetValue(ecpayReceivePaymentInfoDto, null)}");
                }
                string json = JsonConvert.SerializeObject(ecpayReceivePaymentInfoDto);
                Console.WriteLine("recevie ecpay barcode info {0}", json);

                var paymentRecord = await this.paymentRepository.GetByMerchantTradeNoAsync(ecpayReceivePaymentInfoDto.MerchantTradeNo);

                paymentRecord.BarCode1 = ecpayReceivePaymentInfoDto.Barcode1;
                paymentRecord.BarCode2 = ecpayReceivePaymentInfoDto.Barcode2;
                paymentRecord.BarCode3 = ecpayReceivePaymentInfoDto.Barcode3;

                return Ok("1|O");
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90300,
                    Message = $"payment create api error:{ex}"
                });
            }
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