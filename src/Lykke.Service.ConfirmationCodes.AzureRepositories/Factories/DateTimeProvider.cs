using System;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Factories
{
    class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
