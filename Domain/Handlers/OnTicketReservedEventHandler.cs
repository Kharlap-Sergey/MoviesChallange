using Domain.Abstractions;
using Domain.Events;
using MediatR;

namespace Domain.Handlers;

public class OnTicketReservedEventHandler
    : INotificationHandler<TicketReservedDomainEvent>
{
    private readonly ITicketRepository _ticketRepository;

    public OnTicketReservedEventHandler(
        ITicketRepository ticketRepository
        )
    {
        _ticketRepository = ticketRepository;
    }

    public async Task Handle(
        TicketReservedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var ticket = notification.Ticket;

        //originally it should've been a integration event sent here 
        //with derived ValidateTicketCommand sent with delay
        //for example we are using a delay with NServiceBus
        // https://docs.particular.net/nservicebus/messaging/delayed-delivery
        
        await Task.Delay(notification.ExpiresIn, cancellationToken);

        var fresh = await _ticketRepository.GetById(ticket.Id, cancellationToken);

        if (fresh != null && fresh.IsExpired())
        {
            await _ticketRepository.Delete(ticket.Id, cancellationToken);
        }
    }
}
