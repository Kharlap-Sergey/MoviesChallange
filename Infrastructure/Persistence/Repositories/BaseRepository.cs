using Domain.Core;
using MediatR;

namespace Infrastructure.Persistence.Repositories;

public class BaseRepository
{
    protected readonly IMediator _mediator;

    public BaseRepository(
        IMediator mediator
        )
    {
        _mediator = mediator;
    }

    protected Task DispatchDomainEventsAsync<T>(
        T entity,
        CancellationToken cancellationToken)
        where T : BaseEntity
    {
        if (entity.DomainEvents == null || !entity.DomainEvents.Any())
        {
            return Task.CompletedTask;
        }

        var dispatching = entity.DomainEvents
            .Select(@event => _mediator.Publish(@event, cancellationToken));

        return Task.WhenAll(dispatching);
    }
}
