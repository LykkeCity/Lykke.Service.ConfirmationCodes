namespace Lykke.Service.ConfirmationCodes.Client.Models.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class SendSmsConfirmationRequest : SmsConfirmationRequest
    {
        /// <summary>
        /// Is priority code. Used for manual code resending from support.
        /// </summary>
        public bool IsPriority { get; set; }
        /// <summary>
        /// Length for requested code
        /// </summary>
        public int CodeLength { get; set; }
        /// <summary>
        /// Expiration interval in seconds
        /// </summary>
        public int ExpirationInterval { get; set; }
    }
}
