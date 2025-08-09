using Microsoft.Azure.Cosmos;
using TowerApi.Domain.Entities;

namespace TowerApi.Infrastructure.Cosmos.Repositories;

public class TowerLiveStatusRepositoryCosmos(CosmosClient client, string databaseName, string containerName)
{
    private readonly Container _container = client.GetContainer(databaseName, containerName);

    public async Task<TowerLiveStatus?> GetByTowerIdAsync(Guid towerId, CancellationToken cancellationToken = default)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.towerId = @towerId")
            .WithParameter("@towerId", towerId.ToString());

        var iterator = _container.GetItemQueryIterator<TowerLiveStatus>(query);
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync(cancellationToken);
            return response.FirstOrDefault();
        }

        return null;
    }

    public async Task UpsertStatusAsync(TowerLiveStatus status, CancellationToken cancellationToken = default)
    {
        await _container.UpsertItemAsync(status, new PartitionKey(status.TowerId.ToString()), cancellationToken: cancellationToken);
    }

    public async Task DeleteStatusAsync(Guid towerId, CancellationToken cancellationToken = default)
    {
        await _container.DeleteItemAsync<TowerLiveStatus>(towerId.ToString(), new PartitionKey(towerId.ToString()), cancellationToken: cancellationToken);
    }
}