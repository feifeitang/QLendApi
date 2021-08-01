using QLendApi.Models;

namespace QLendApi.Dtos
{
    public class GetLoanListResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public LoanRecord[] LoanRecord { get; set; }
    }
}