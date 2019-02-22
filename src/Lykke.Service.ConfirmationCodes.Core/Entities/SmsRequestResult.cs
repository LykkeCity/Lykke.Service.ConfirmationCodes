namespace Lykke.Service.ConfirmationCodes.Core.Entities
{
    public class SmsRequestResult
    {
        public CallLimitStatus Status { get; set; }
        public string Code { get; set; }

        public static SmsRequestResult SuccessResult(string code)
        {
            return new SmsRequestResult
            {
                Status = CallLimitStatus.Allowed, 
                Code = code
            };
        }
        
        public static SmsRequestResult FailedResult(CallLimitStatus status)
        {
            return new SmsRequestResult
            {
                Status = status 
            };
        }
    }
}
