using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Extensions
{
    public static class FormFileExtension
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}