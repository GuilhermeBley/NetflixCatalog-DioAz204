using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace NetflixCatalog_DioAz204.Func.Services;

public class ContainerStorageRepository
{
    private readonly BlobContainerClient _containerClient;

    public ContainerStorageRepository(IConfiguration configuration)
    {
        var connectionString = configuration["AzureStorage:ConnectionString"];
        var containerName = configuration["AzureStorage:ContainerName"];

        _containerClient = new BlobContainerClient(connectionString, containerName);
        _containerClient.CreateIfNotExists(PublicAccessType.None);
    }

    public async Task<string> UploadOrReplaceFileAsync(
        string fileName, 
        Stream fileStream, 
        string contentType, 
        CancellationToken cancellationToken = default)
    {
        BlobClient blobClient = _containerClient.GetBlobClient(fileName);
        var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

        await blobClient.UploadAsync(fileStream, new BlobUploadOptions { HttpHeaders = blobHttpHeaders }, cancellationToken);

        return _containerClient.Uri.AbsoluteUri.Trim('/') + $"/{fileName}";
    }
}
