using Domain.Abstractions;
using Domain.Events;
using MediatR;

namespace Domain.Handlers;

public class OnTicketExpiredEventHandler
    : INotificationHandler<TicketExpiredDomainEvent>
{
    private readonly ITicketRepository _ticketRepository;

    public OnTicketExpiredEventHandler(
        ITicketRepository ticketRepository
        )
    {
        _ticketRepository = ticketRepository;
    }

    public async Task Handle(
        TicketExpiredDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _ticketRepository.Delete(notification.TicketId, cancellationToken);
    }
}
