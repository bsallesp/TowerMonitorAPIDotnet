using Microsoft.Azure.Cosmos;
using TowerApi.Domain.Entities;
using TowerApi.Domain.Repositories;

namespace TowerApi.Infrastructure.Cosmos.Repositories;

public class TowerRepositoryCosmos(CosmosClient client, string databaseName, string containerName)
    : ITowerRepository
{
    private readonly Container _container = client.GetContainer(databaseName, containerName);

    public async Task<Tower?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _container.ReadItemAsync<Tower>(id.ToString(), new PartitionKey(id.ToString()), cancellationToken: cancellationToken);
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<IEnumerable<Tower>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var query = _container.GetItemQueryIterator<Tower>("SELECT * FROM c");
        var results = new List<Tower>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync(cancellationToken);
            results.AddRange(response);
        }

        return results;
    }

    public async Task AddAsync(Tower tower, CancellationToken cancellationToken = default)
    {
        await _container.CreateItemAsync(tower, new PartitionKey(tower.Id.ToString()), cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(Tower tower, CancellationToken cancellationToken = default)
    {
        await _container.UpsertItemAsync(tower, new PartitionKey(tower.Id.ToString()), cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _container.DeleteItemAsync<Tower>(id.ToString(), new PartitionKey(id.ToString()), cancellationToken: cancellationToken);
    }
}
