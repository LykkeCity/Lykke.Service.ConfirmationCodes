using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.ConfirmationCodes.Client.Models.Request
{
    /// <summary>
    /// 
    /// </summary>
    public class SmsConfirmCodeRequest
    {
        /// <summary>
        /// Client Id
        /// </summary>
        public string ClientId { get; set; }
        
        /// <summary>
        /// Phone number
        /// </summary>
        [Required]
        [MinLength(8)]
        public string Phone { get; set; }
        
        /// <summary>
        /// Operation name
        /// </summary>
        public string Operation { get; set; }
    }
}
