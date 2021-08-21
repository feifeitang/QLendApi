using System.Threading;
using System.Threading.Tasks;
using QLendApi.Dtos;
using QLendApi.Settings;

namespace QLendApi.Services
{
    public interface ISmsService
    {
        bool Send(string PhoneNumber, string content);
    }
}