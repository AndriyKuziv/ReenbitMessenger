using Azure.Core.Extensions;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;

internal static class BlobStorage
{
    public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(
        this AzureClientFactoryBuilder builder, ConfigurationManager config, bool preferMsi)
    {
        if (preferMsi && Uri.TryCreate(config.GetSection("BlobStorageConnection").Value, UriKind.Absolute, out Uri? serviceUri))
        {
            return builder.AddBlobServiceClient(serviceUri);
        }
        else
        {
            return builder.AddBlobServiceClient(config.GetSection("BlobStorageConnection").Value);
        }
    }
}
