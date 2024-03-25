using MediatR;

namespace Domain.Events;

public record NewAuditoriumReservationDomainEvent<T>(
    int AuditoriumId,
    DateTime SessionDate,
    T Event
    )
    : INotification;
