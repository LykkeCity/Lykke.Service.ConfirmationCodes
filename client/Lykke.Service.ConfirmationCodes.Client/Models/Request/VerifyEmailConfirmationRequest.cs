using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.ConfirmationCodes.Client.Models.Request
{
    public class VerifyEmailConfirmationRequest : EmailConfirmationRequest
    {
        [Required]
        public string Code { get; set; }
    }
}
