using TowerApi.Domain.Entities;
using TowerApi.Domain.Repositories.Interfaces;
using TowerApi.Infrastructure.Repositories;

namespace TowerApi.Application.Services;

public class TowerStatusAggregationService(
    ITowerRepository towerRepository,
    ITowerLiveStatusRepository liveStatusRepository)
{
    private readonly ITowerLiveStatusRepository _liveStatusRepository = liveStatusRepository;

    public async Task<IEnumerable<object>> GetAllWithLiveStatusAsync(CancellationToken cancellationToken = default)
    {
        var towers = await towerRepository.GetAllAsync(cancellationToken);
        var liveStatuses = await _liveStatusRepository.GetAllAsync(cancellationToken);

        var result = from t in towers
            join s in liveStatuses on t.Id equals s.TowerId into gj
            from status in gj.DefaultIfEmpty()
            select new
            {
                TowerId = t.Id,
                t.License,
                t.Address,
                t.City,
                t.State,
                t.StructureHeight,
                Status = status?.Status.ToString() ?? nameof(TowerStatus.Unknown),
                LastUpdate = status?.Timestamp
            };

        return result;
    }
}