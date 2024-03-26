using Domain.Core;

namespace Domain.Entities.Tickets;

public record TicketStatus : Enumeration
{
    public static TicketStatus Pending = new(1, "Pending");

    public static TicketStatus Paid = new(2, "Paid");

    public static TicketStatus Expired = new(3, "Expired");

    public TicketStatus(int Id, string Name) : base(Id, Name)
    {
    }
}
