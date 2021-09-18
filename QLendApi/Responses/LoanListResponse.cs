using QLendApi.Models;

namespace QLendApi.Responses
{
    public class LoanListResponse : BaseResponse
    {
         
       
        public class LoanListDataStruct
        {
           public LoanRecord[] LoanRecords { get; set; }     
        }
        
       public LoanListDataStruct Data { get; init; }
       
       
    }
}