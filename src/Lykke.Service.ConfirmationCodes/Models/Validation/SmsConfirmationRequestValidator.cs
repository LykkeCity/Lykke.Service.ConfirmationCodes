using JetBrains.Annotations;
using Lykke.Service.ConfirmationCodes.Client.Models.Request;

namespace Lykke.Service.ConfirmationCodes.Models.Validation
{
    [UsedImplicitly]
    public class SmsConfirmationRequestValidator : PhoneValidator<SmsConfirmationRequest>
    {
    }
}
