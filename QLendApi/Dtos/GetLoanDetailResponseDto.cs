using QLendApi.Models;

namespace QLendApi.Dtos
{
    public class GetLoanDetailResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public LoanRecord LoanRecord { get; set; }
        public RepaymentRecord[] RepaymentRecords { get; set; }
    }
}