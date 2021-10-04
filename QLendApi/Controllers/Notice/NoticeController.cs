using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Models;
using QLendApi.Repositories;
using QLendApi.Responses;

namespace QLendApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoticeController : ControllerBase, INoticeController
    {
        private readonly INoticeRepository noticeRepository;
        public NoticeController(INoticeRepository noticeRepository)
        {
            this.noticeRepository = noticeRepository;
        }

        //GET api/notice/list
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<NoticeListResponse>> List()
        {
            try
            {
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                var noticeRecords = await this.noticeRepository.GetListByForeignWorkerIdAsync(foreignWorker.Id);

                return Ok(new NoticeListResponse
                {
                    StatusCode = 10000,
                    Message = "success",
                    Data = new NoticeListResponse.NoticeListDataStruct
                    {
                        NoticeRecords = noticeRecords
                    }
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90800,
                    Message = $"notice list api error:{ex}"
                });
            }
        }
    }
}