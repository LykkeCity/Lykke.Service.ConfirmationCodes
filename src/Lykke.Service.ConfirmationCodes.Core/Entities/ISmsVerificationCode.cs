using System;

namespace Lykke.Service.ConfirmationCodes.Core.Entities
{
    public interface ISmsVerificationCode
    {
        string Id { get; }
        string Phone { get; }
        string Code { get; }
        DateTime CreationDateTime { get; }
    }
}
