using Domain.Entities.Tickets;
using MediatR;

namespace Domain.Events;

public record TicketReservedDomainEvent(
    TicketEntity Ticket,
    TimeSpan ExpiresIn
    )
    : INotification;

