using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLendApi.Dtos;
using QLendApi.lib;
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
        private readonly IRepaymentRecordRepository repaymentRecordRepository;
        public PaymentController(
            IEcpayService ecpayService,
            IPaymentRepository paymentRepository,
            IRepaymentRecordRepository repaymentRecordRepository)
        {
            this.ecpayService = ecpayService;
            this.paymentRepository = paymentRepository;
            this.repaymentRecordRepository = repaymentRecordRepository;
        }
/*
        [Authorize]
        // Get /api/payment/getBarCodeCreateTime/{repaymentNumber}
        [Route("/api/payment/getBarCodeCreateTime/{repaymentNumber}")]
        [HttpGet]
        public async Task<ActionResult> GetBarCodeCreateTime(string repaymentNumber)
        {
            try
            {
                var repaymentRecord = await this.repaymentRecordRepository.GetByRepaymentNumberAsync(repaymentNumber);

                if (repaymentRecord.BarCodeCreateTime != null)
                {
                    return Ok(new GetBarCodeCreateTimeResponse
                    {
                        StatusCode = ResponseStatusCode.Success,
                        Message = "success",
                        Data = new GetBarCodeCreateTimeResponse.GetBarCodeCreateTimeDataStruct
                        {
                            BarCodeCreateTime = repaymentRecord.BarCodeCreateTime
                        }
                    });
                }
                else
                {
                    return Ok(new GetBarCodeCreateTimeResponse
                    {
                        StatusCode = ResponseStatusCode.Success,
                        Message = "success",
                        Data = new GetBarCodeCreateTimeResponse.GetBarCodeCreateTimeDataStruct
                        {
                            BarCodeCreateTime = null
                        }
                    });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90100,
                    Message = $"getBarCodeCreateTime api error:{ex}"
                });
            }
        }
*/

        // [Authorize]
        // GET /api/payment/Create/{repaymentNumber}
        [Route("Create/{repaymentNumber}")]
        [HttpGet]
        public async Task<ActionResult> Create(string repaymentNumber)
        {
            try
            {
                var repaymentRecord = await this.repaymentRecordRepository.GetByRepaymentNumberAsync(repaymentNumber);

                if (repaymentRecord == null)
                {
                    return new ContentResult
                    {
                        StatusCode = 400,
                        Content = "<h1>Bad Request</h1>"
                    };
                }

                var content = await this.ecpayService.create(repaymentNumber);
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
        public async Task<ActionResult<PaymentGetBarCodeResponse>> GetBarCode(string repaymentNumber)
        {
            try
            {
                var payment = await this.paymentRepository.GetByRepaymentNumberAsync(repaymentNumber);

                return Ok(new PaymentGetBarCodeResponse
                {
                    StatusCode = ResponseStatusCode.Success,
                    Message = "success",
                    Data = new PaymentGetBarCodeResponse.PaymentGetBarCodeDataStruct
                    {
                        BarCode1 = payment.BarCode1,
                        BarCode2 = payment.BarCode2,
                        BarCode3 = payment.BarCode3
                    }
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90200,
                    Message = $"getBarCode api error:{ex}"
                });
            }
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
                var repaymentRecord = await this.repaymentRecordRepository.GetByRepaymentNumberAsync(paymentRecord.RepaymentNumber);

                paymentRecord.BarCode1 = ecpayReceivePaymentInfoDto.Barcode1;
                paymentRecord.BarCode2 = ecpayReceivePaymentInfoDto.Barcode2;
                paymentRecord.BarCode3 = ecpayReceivePaymentInfoDto.Barcode3;
                paymentRecord.TradeNo = ecpayReceivePaymentInfoDto.TradeNo;
                paymentRecord.UpdateTime = DateTime.Now;

                repaymentRecord.BarCode1 = ecpayReceivePaymentInfoDto.Barcode1;
                repaymentRecord.BarCode2 = ecpayReceivePaymentInfoDto.Barcode2;
                repaymentRecord.BarCode3 = ecpayReceivePaymentInfoDto.Barcode3;
                repaymentRecord.BarCodeCreateTime = DateTime.Now;

                await this.paymentRepository.UpdateAsync(paymentRecord);
                await this.repaymentRecordRepository.UpdateAsync(repaymentRecord);

                return Ok("1|OK");
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90300,
                    Message = $"receiveBarCode api error:{ex}"
                });
            }
        }

        // POST /api/payment/CallBack
        [Route("CallBack")]
        [Consumes("application/x-www-form-urlencoded")]
        [HttpPost]
        public async Task<ActionResult> CallBack(EcpayReceivePaymentResultDto ecpayReceivePaymentResultDto)
        {
            try
            {
                bool b = await this.ecpayService.ReceivePaymentResult(ecpayReceivePaymentResultDto);

                if (b != true)
                {
                    return Ok("0|checkMacValue not correct");
                }

                var payment = await this.paymentRepository.GetByMerchantTradeNoAsync(ecpayReceivePaymentResultDto.MerchantTradeNo);

                var repaymentRecord = await this.repaymentRecordRepository.GetByRepaymentNumberAsync(payment.RepaymentNumber);

                repaymentRecord.ActualRepaymentDate = DateTime.ParseExact(ecpayReceivePaymentResultDto.PaymentDate, "yyyy/MM/dd", null);
                repaymentRecord.ActualRepaymentAmount = ecpayReceivePaymentResultDto.TradeAmt;
                repaymentRecord.State = 1;

                await this.repaymentRecordRepository.UpdateAsync(repaymentRecord);

                return Ok("1|OK");
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90400,
                    Message = $"payment callBack api error:{ex}"
                });
            }
        }
    }
}