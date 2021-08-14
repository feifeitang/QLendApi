using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Responses;

namespace QLendApi.Controllers
{
    public interface INoticeController
    {
        Task<ActionResult<NoticeListResponse>> List();
    }
}