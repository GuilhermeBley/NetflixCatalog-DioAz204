﻿using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace NetflixCatalog_DioAz204.Func.Services;

public class ContainerStorageRepository
{
    private readonly BlobContainerClient _containerClient;

    public ContainerStorageRepository(IOptions<StorageAccountOptions> opt)
    {
        var connectionString = opt.Value.ConnectionString;
        var containerName = opt.Value.Container;

        _containerClient = new BlobContainerClient(connectionString, containerName);
        _containerClient.CreateIfNotExists(PublicAccessType.Blob);
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
