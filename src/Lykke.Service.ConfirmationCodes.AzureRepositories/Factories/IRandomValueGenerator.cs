namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Factories
{
    public interface IRandomValueGenerator
    {
        int GetInt(int min, int max);
        string GetCode(int length);
    }
}
