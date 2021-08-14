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
        public const int Init = 1;
        public const int InitArcFinish = 2;
        public const int PersonalInfoFinish = 3;
        public const int Finish = 4;
    }

    public class LoanState
    {
        public const int LoanSurveyInfoFinish = 3;
        public const int IncomeInfoFinish = 3;
        public const int ApplyFinish = 5;
    }

    public class NoticeStatus
    {
        public const int Success = 1;
    }
}