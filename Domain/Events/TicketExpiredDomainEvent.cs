using Domain.ValueObjects;
using MediatR;

namespace Domain.Events;

public record TicketExpiredDomainEvent(
    Guid TicketId,
    IEnumerable<Seat> Seats
    )
    : INotification;
