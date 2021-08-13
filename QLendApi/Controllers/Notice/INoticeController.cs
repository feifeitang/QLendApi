using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Dtos;
using QLendApi.Settings;

namespace QLendApi.Controllers
{
    public interface INoticeController
    {
        Task<ActionResult<GetNoticeListResponseDto>> List();
    }
}