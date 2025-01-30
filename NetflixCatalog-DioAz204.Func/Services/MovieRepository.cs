using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using NetflixCatalog_DioAz204.Func.Model;
using System.Net;

namespace NetflixCatalog_DioAz204.Func.Services;

public class MovieRepository
{
    private readonly Container _container;

    public MovieRepository(IConfiguration configuration)
    {
        var connectionString = configuration["CosmosDb:ConnectionString"];
        var databaseName = configuration["CosmosDb:DatabaseName"];
        var containerName = configuration["CosmosDb:ContainerName"];

        CosmosClient client = new CosmosClient(connectionString);
        _container = client.GetContainer(databaseName, containerName);
    }

    public async Task<MovieModel> CreateAsync(MovieModel movie)
    {
        movie.Id = Guid.NewGuid();
        movie.CreatedAt = DateTime.UtcNow;
        ItemResponse<MovieModel> response = await _container.CreateItemAsync(movie, new PartitionKey(movie.Category));
        return response.Resource;
    }

    public async Task<MovieModel?> GetAsync(Guid id, string category)
    {
        try
        {
            ItemResponse<MovieModel> response = await _container.ReadItemAsync<MovieModel>(id.ToString(), new PartitionKey(category));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<IEnumerable<MovieModel>> GetAllAsync()
    {
        var query = _container.GetItemQueryIterator<MovieModel>("SELECT * FROM c");
        List<MovieModel> results = new List<MovieModel>();
        while (query.HasMoreResults)
        {
            FeedResponse<MovieModel> response = await query.ReadNextAsync();
            results.AddRange(response);
        }
        return results;
    }

    public async Task<MovieModel?> UpdateAsync(MovieModel movie)
    {
        try
        {
            ItemResponse<MovieModel> response = await _container.UpsertItemAsync(movie, new PartitionKey(movie.Category));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<bool> DeleteAsync(Guid id, string category)
    {
        try
        {
            await _container.DeleteItemAsync<MovieModel>(id.ToString(), new PartitionKey(category));
            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }
}
