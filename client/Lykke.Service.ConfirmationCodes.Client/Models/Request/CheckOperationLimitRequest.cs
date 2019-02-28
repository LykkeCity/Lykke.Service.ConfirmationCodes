using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.ConfirmationCodes.Client.Models.Request
{
    /// <summary>
    /// Operation calls limit check request
    /// </summary>
    public class CheckOperationLimitRequest
    {
        /// <summary>
        /// Client Id
        /// </summary>
        [Required]
        public string ClientId { get; set; }
        
        /// <summary>
        /// Operation name
        /// </summary>
        [Required]
        public string Operation { get; set; }

        /// <summary>
        /// Check for call timeout
        /// </summary>
        public bool RepeatCheck { get; set; }
    }
}
