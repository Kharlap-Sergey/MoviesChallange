using Domain.Entities.ShowTimes;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;
using NUnit.Framework;

namespace Domain.Entities.Tickets.UnitTests
{
    [TestFixture()]
    public class TicketEntityTests
    {
        [Test()]
        public void Given_Paid_With_Old_Date_IsExpired_Returns_False()
        {
            var ticket = new TicketEntity(
                id: Guid.NewGuid(),
                seats: new List<Seat> { new Seat(1, 1) },
                showtime: new ShowTimeEntity(
                    date: DateTime.UtcNow.AddDays(-1),
                    movieId: "1",
                    auditoriumId: 1),
                status: TicketStatus.Paid,
                createdTime: DateTime.UtcNow.AddDays(-1),
                paymentId: Guid.NewGuid()
                );

            Assert.That(ticket.IsExpired(), Is.False);
        }

        [Test()]
        public void Given_Pending_With_Old_Date_IsExpired_Returns_False()
        {
            var ticket = new TicketEntity(
                id: Guid.NewGuid(),
                seats: new List<Seat> { new Seat(1, 1) },
                showtime: new ShowTimeEntity(
                    date: DateTime.UtcNow.AddDays(-1),
                    movieId: "1",
                    auditoriumId: 1),
                status: TicketStatus.Pending,
                createdTime: DateTime.UtcNow.AddDays(-1),
                paymentId: Guid.NewGuid()
                );

            Assert.That(ticket.IsExpired(), Is.True);
        }

        [Test()]
        public void Given_Paid_Expire_Throws_InvalidOperationDomainException()
        {
            var ticket = new TicketEntity(
                id: Guid.NewGuid(),
                seats: new List<Seat> { new Seat(1, 1) },
                showtime: new ShowTimeEntity(
                    date: DateTime.UtcNow.AddDays(-1),
                    movieId: "1",
                    auditoriumId: 1),
                status: TicketStatus.Paid,
                createdTime: DateTime.UtcNow.AddDays(-1),
                paymentId: Guid.NewGuid()
                );

            Assert.That(() => ticket.Expire(), Throws.Exception.InstanceOf<InvalidOperationDomainException>());
        }

        [Test()]
        public void Given_Paid_Expired__Throws_InvalidOperationDomainException()
        {
            var ticket = new TicketEntity(
                id: Guid.NewGuid(),
                seats: new List<Seat> { new Seat(1, 1) },
                showtime: new ShowTimeEntity(
                    date: DateTime.UtcNow.AddDays(-1),
                    movieId: "1",
                    auditoriumId: 1),
                status: TicketStatus.Expired,
                createdTime: DateTime.UtcNow.AddDays(-1),
                paymentId: Guid.NewGuid()
                );

            Assert.That(() => ticket.Expire(), Throws.Exception.InstanceOf<InvalidOperationDomainException>());
        }

        [Test()]
        public void Given_Pending_Expire_Expire_Ticket()
        {
            var ticket = new TicketEntity(
                id: Guid.NewGuid(),
                seats: new List<Seat> { new Seat(1, 1) },
                showtime: new ShowTimeEntity(
                    date: DateTime.UtcNow.AddDays(-1),
                    movieId: "1",
                    auditoriumId: 1),
                status: TicketStatus.Pending,
                createdTime: DateTime.UtcNow.AddDays(-1),
                paymentId: Guid.NewGuid()
                );

            ticket.Expire();

            Assert.That(ticket.Status, Is.EqualTo(TicketStatus.Expired));
            Assert.That(ticket.DomainEvents, Has.One.InstanceOf<TicketExpiredDomainEvent>());
        }

    }
}