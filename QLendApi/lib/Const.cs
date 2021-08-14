namespace QLendApi.lib
{
    public class ResponseStatusCode
    {
        public const int Success = 10000;
    }
    public class ForeignWorkState
    {
        public const int Approve = 1;
        public const int Pending = 0;
        public const int Failure = -1;
    }
    public class ForeignWorkStatus
    {
        public const int Finish = 5;
    }

    public class LoanState
    {
        public const int ApplyFinish = 5;
    }

    public class NoticeStatus
    {
        public const int Success = 1;
    }
}