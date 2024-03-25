using Domain.Abstractions;
using Domain.Entities.Tickets;
using MediatR;

namespace Infrastructure.Persistence.Repositories;

public class TicketRepository : BaseRepository, ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(
        IMediator mediator,
        AppDbContext context
        )
        : base(mediator)
    {
        _context = context;
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var ticket = await GetById(id, cancellationToken);

        if (ticket == null)
            return;

        _context.Tickets.Remove(ticket);

        await _context.SaveChangesAsync(cancellationToken);

    }

    public async Task<TicketEntity?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tickets.FindAsync(id, cancellationToken);
    }

    public async Task Update(TicketEntity ticket, CancellationToken cancellationToken = default)
    {
        _context.Tickets.Update(ticket);

        await _context.SaveChangesAsync(cancellationToken);

        await DispatchDomainEventsAsync(ticket, cancellationToken);
    }
}
