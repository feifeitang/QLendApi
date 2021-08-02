using QLendApi.Models;

namespace QLendApi.Dtos
{
    public class GetLoanApplyDataDto
    {
        public int StatusCode {get; set;}
        public string Message { get; set;}

        public ForeignWorker foreignWorkerInfo {get; set;}

        public LoanRecord loanRecordInfo {get; set;} 
    }
}