namespace Lykke.Service.ConfirmationCodes.Services
{
    public interface IDeploymentSettings
    {
        bool IsProduction { get; set; }
    }
}
