using System;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Factories
{
    public interface IDateTimeProvider
    {
        DateTime GetDateTime();
    }
}
