using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace ReenbitMessenger.API.Services
{
    public static class KeyVault
    {
        public static void AddKeyVault(this ConfigurationManager config)
        {
            string keyVaultUrl = config["AzureKeyVault:AzureKeyVaultURL"];
            string tenantId = config["AzureKeyVault:TenantId"];
            string clientId = config["AzureKeyVault:ClientId"];
            string clientSecret = config["AzureKeyVault:ClientSecretId"];

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            var client = new SecretClient(new Uri(keyVaultUrl), credential);

            config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
        }
    }
}
