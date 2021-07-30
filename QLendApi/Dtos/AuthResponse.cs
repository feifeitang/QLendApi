namespace QLendApi.Dtos
{
    public class LoginResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}