using Domain.Entities.Tickets;
using MediatR;

namespace Domain.Events;

public record TicketPaymentConfirmedDomainEvent(
    TicketEntity ticket,
    Guid PaymentId
    )
    : INotification;
