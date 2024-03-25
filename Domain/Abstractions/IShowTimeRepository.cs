using Domain.Entities.ShowTimes;

namespace Domain.Abstractions;

public interface IShowTimeRepository
{
    Task<ShowTimeEntity> GetShowTimeByIdAsync(Guid showTimeId, CancellationToken cancellationToken);
    
    Task<IEnumerable<ShowTimeEntity>> GetShowTimesAsync(
        DateTime? from = null,
        DateTime? to = null,
        int? auditoriumId = null,
        CancellationToken cancellationToken = default
        );

    Task AddShowTimeAsync(ShowTimeEntity ShowTimeEntity, CancellationToken cancellationToken);
    Task UpdateShowTimeAsync(ShowTimeEntity ShowTimeEntity, CancellationToken cancellationToken);
    Task DeleteShowTimeAsync(ShowTimeEntity ShowTimeEntity, CancellationToken cancellationToken);
}
