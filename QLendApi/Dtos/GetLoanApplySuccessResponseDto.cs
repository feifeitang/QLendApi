namespace QLendApi.Dtos
{
    public class GetLoanApplySuccessDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int Amount {get; set;}
        public int Period {get; set;}
    }
}