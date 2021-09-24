namespace QLendApi.Services
{
    public interface ISmsService
    {
        void Send(string PhoneNumber, string content);
    }
}