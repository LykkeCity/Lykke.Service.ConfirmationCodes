using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.ConfirmationCodes.Client.Models.Request
{
    /// <summary>
    /// Basic request for sms confirmations
    /// </summary>
    public class SmsConfirmationRequest
    {
        /// <summary>
        /// Phone number
        /// </summary>
        [Required]
        [MinLength(8)]
        public string Phone { get; set; }
        /// <summary>
        /// Partner Id
        /// </summary>
        public string PartnerId { get; set; }
    }
}
