using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.ConfirmationCodes.Client.Models.Request
{
    /// <summary>
    /// Sms check request
    /// </summary>
    public class CheckSmsCodeRequest
    {
        /// <summary>
        /// Verification code
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Phone number
        /// </summary>
        [Required]
        [MinLength(8)]
        public string Phone { get; set; }

        /// <summary>
        /// Client Id
        /// </summary>
        public string ClientId { get; set; }
    }
}
