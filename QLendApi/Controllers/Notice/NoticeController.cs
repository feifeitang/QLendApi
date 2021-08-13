using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Dtos;
using QLendApi.Models;
using QLendApi.Repositories;

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
        public async Task<ActionResult<GetNoticeListResponseDto>> List()
        {

            var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

            var noticeRecords = await this.noticeRepository.GetListByForeignWorkerIdAsync(foreignWorker.Id);

            return Ok(new GetNoticeListResponseDto
            {
                StatusCode = 10000,
                Message = "success",
                NoticeRecords = noticeRecords
            });

        }
    }
}