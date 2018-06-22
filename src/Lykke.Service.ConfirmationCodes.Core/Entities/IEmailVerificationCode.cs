using System;

namespace Lykke.Service.ConfirmationCodes.Core.Entities
{
    public interface IEmailVerificationCode
    {
        string Id { get; }
        string Code { get; }
        DateTime CreationDateTime { get; }
    }
}
