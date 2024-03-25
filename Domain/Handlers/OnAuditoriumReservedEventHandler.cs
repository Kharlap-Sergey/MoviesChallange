using Domain.Abstractions;
using Domain.Entities;
using Domain.Entities.ShowTimes;
using Domain.Events;
using MediatR;

namespace Domain.Handlers;

public class OnAuditoriumReservedEventHandler
    : INotificationHandler<NewAuditoriumReservationDomainEvent<MovieEntity>>
{
    private readonly IShowTimeRepository _showTimeRepository;

    public OnAuditoriumReservedEventHandler(
        IShowTimeRepository showTimeRepository
        )
    {
        _showTimeRepository = showTimeRepository;
    }
    public Task Handle(
        NewAuditoriumReservationDomainEvent<MovieEntity> notification,
        CancellationToken cancellationToken)
    {
        var showTime = new ShowTimeEntity(
            notification.SessionDate,
            notification.Event.Id,
            notification.AuditoriumId
        );

        return _showTimeRepository.AddShowTimeAsync(showTime, cancellationToken);
    }
}
