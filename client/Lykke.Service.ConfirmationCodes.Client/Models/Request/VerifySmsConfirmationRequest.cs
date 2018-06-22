namespace Lykke.Service.ConfirmationCodes.Client.Models.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class VerifySmsConfirmationRequest : SmsConfirmationRequest
    {
        /// <summary>
        /// Verification code
        /// </summary>
        public string Code { get; set; }
    }
}
