using Domain.Entities.Tickets;

namespace Domain.Abstractions;

public interface ITicketRepository
{
    public Task<TicketEntity?> GetById(
        Guid id,
        CancellationToken cancellationToken = default);

    public Task Update(
        TicketEntity ticket,
        CancellationToken cancellationToken = default);

    public Task Delete(
        Guid id,
        CancellationToken cancellationToken = default);
}
