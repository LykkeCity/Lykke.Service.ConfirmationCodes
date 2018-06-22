using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.ConfirmationCodes.Client.Models.Request
{
    /// <summary>
    /// Basic request for email confirmations
    /// </summary>
    public class EmailConfirmationRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// Partner Id
        /// </summary>
        public string PartnerId { get; set; }
    }
}
