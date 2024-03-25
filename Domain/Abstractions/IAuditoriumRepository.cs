using Domain.Entities.Auditorium;

namespace Domain.Abstractions;

public interface IAuditoriumRepository
{
    public Task<AuditoriumEntity?> GetById(int id, CancellationToken token);
    public Task Update(AuditoriumEntity auditorium, CancellationToken token);
}
