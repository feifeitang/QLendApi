using System;
using System.Reflection;
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
        public async Task<ActionResult<PaymentCreateResponse>> Create()
        {
            try
            {
                var createRes = await this.ecpayService.create(2000);
                if (!createRes)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 90310,
                        Message = $"payment ecpayService error"
                    });
                }
                return Ok(new PaymentCreateResponse
                {
                    StatusCode = 10000,
                    Message = "success",
                });
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
        public IActionResult ReceiveBarCode([FromForm] EcpayCreateOrderDto ecpayReceivePaymentInfoDto)
        {
            try
            {
                foreach (PropertyInfo prop in ecpayReceivePaymentInfoDto.GetType().GetProperties())
                {
                    Console.WriteLine($"{prop.Name}: {prop.GetValue(ecpayReceivePaymentInfoDto, null)}");
                }
                string json = JsonConvert.SerializeObject(ecpayReceivePaymentInfoDto);
                Console.WriteLine(json);

                return Ok("0|1");
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