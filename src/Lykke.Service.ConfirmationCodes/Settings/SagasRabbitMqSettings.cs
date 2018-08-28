using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.ConfirmationCodes.Settings
{
    public class SagasRabbitMqSettings
    {
        [AmqpCheck]
        public string RabbitConnectionString { get; set; }
    }
}
