using Azure.Identity;

namespace ReenbitMessenger.API.Services
{
    public static class KeyVault
    {
        public static void AddKeyVault(this ConfigurationManager config)
        {
            string keyVaultUrl = config["AzureKeyVault:AzureKeyVaultURL"];

            config.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
        }
    }
}
