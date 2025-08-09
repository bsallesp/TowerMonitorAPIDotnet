using Microsoft.Azure.Cosmos;
using TowerApi.Domain.Entities;
using TowerApi.Domain.Repositories.Interfaces;

namespace TowerApi.Domain.Repositories;

public class TowerLiveStatusRepository(CosmosClient client) : ITowerLiveStatusRepository
{
    private readonly Container _container = client.GetContainer("TowerDb", "TowerLiveStatus");

    public async Task UpdateAsync(TowerLiveStatus status, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(status.Id)) status.Id = Guid.NewGuid().ToString();
        await _container.UpsertItemAsync(status, new PartitionKey(status.TowerId), cancellationToken: ct);
    }

    public async Task<IReadOnlyList<TowerLiveStatus>> GetAllAsync(CancellationToken ct = default)
    {
        var query = _container.GetItemQueryIterator<TowerLiveStatus>("SELECT * FROM c");
        var results = new List<TowerLiveStatus>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync(ct);
            results.AddRange(response);
        }
        return results;
    }
}