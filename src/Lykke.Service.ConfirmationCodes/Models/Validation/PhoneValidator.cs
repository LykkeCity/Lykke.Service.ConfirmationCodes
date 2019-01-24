using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.ConfirmationCodes.Client.Models.Request;

namespace Lykke.Service.ConfirmationCodes.Models.Validation
{
    [UsedImplicitly]
    public class PhoneValidator<T> : AbstractValidator<T>
        where T : SmsConfirmationRequest
    {
        public PhoneValidator()
        {
            RuleFor(x => x.Phone)
                .Must(x => !string.IsNullOrWhiteSpace(x) && x.Length >= 8)
                .WithMessage("Phone can't be empty");
        }
    }
}
