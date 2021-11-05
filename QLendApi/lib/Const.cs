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
        public const int ApplyInit = 1;
        public const int LoanSurveyInfoFinish = 2;
        public const int IncomeInfoFinish = 3;
        public const int LoanSurveyArcFinish = 4;
        public const int ApplyFinish = 5;
        public const int PermitLoan = 6;
        public const int ConfirmLoan = 7;
        public const int BankAccountFinish = 8;
        public const int RemittanceFinish = 9;
        public const int CancelLoan = 0;
        public const int RejectLoan = -1;
    }

    public class NoticeStatus
    {
        public const int Success = 1;
    }

    public class ImageUploadType
    {
        public const int FrontArc = 1;
        public const int BackArc = 2;
        public const int Passport = 3;
        public const int LocalIdCard = 4;

        public const int SelfieArc = 5;
    }
}