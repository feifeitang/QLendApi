using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Dtos;

namespace QLendApi.Controllers
{
    public interface INoticeController
    {
        Task<ActionResult<GetNoticeListResponseDto>> List();
    }
}