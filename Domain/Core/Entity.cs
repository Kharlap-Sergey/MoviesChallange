using MediatR;

namespace Domain.Core;

public class Entity<T> : BaseEntity
{
    public T Id { get; set; }
}

public class BaseEntity
{
    private List<INotification> _domainEvents;
    public List<INotification> DomainEvents => _domainEvents;
    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents ??= new List<INotification>();
        _domainEvents.Add(eventItem);
    }
    public void RemoveDomainEvent(INotification eventItem)
        => _domainEvents?.Remove(eventItem);
}
