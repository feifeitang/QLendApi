namespace QLendApi.Dtos
{
    public class GetLoanDataResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public int Amount {get; set;}
        public int Period {get; set;}
    }
}