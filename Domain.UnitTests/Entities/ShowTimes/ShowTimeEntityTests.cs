using Domain.Entities.Auditorium;
using Domain.Entities.Tickets;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;
using NSubstitute;
using NUnit.Framework;

namespace Domain.Entities.ShowTimes.UnitTests
{
    [TestFixture()]
    public class ShowTimeEntityTests
    {
        [Test()]
        public void Given_Already_Reserved_Seats_ReserveTest_Then_Not_Reserve()
        {
            var auditorium = Substitute.ForPartsOf<AuditoriumEntity>(1, new List<Seat>());
                auditorium.IsSeatsContiguous(Arg.Any<IEnumerable<Seat>>()).Returns(true);

            var showTime = new ShowTimeEntity(
                    DateTime.UtcNow.AddDays(1),
                    "movieId",
                    1,
                    new Guid(),
                    new List<TicketEntity>
                    {
                        new TicketEntity(new List<Seat> {new Seat(1, 1)}, new ShowTimeEntity())
                    }
                );
            
            Assert.Throws<InvalidOperationDomainException>(
                () => showTime.Reserve(new List<Seat> { new Seat(1, 1)}, auditorium)
                );
        }

        [Test()]
        public void Given_Free_Seats_ReserveTest_Then_Reserve()
        {
            var auditorium = Substitute.ForPartsOf<AuditoriumEntity>(1, new List<Seat>());
                auditorium.IsSeatsContiguous(Arg.Any<IEnumerable<Seat>>()).Returns(true);

            var showTime = new ShowTimeEntity(
                    DateTime.UtcNow.AddDays(1),
                    "movieId",
                    1,
                    new Guid(),
                    new List<TicketEntity>
                    {
                        new TicketEntity(new List<Seat> {new Seat(1, 1)}, new ShowTimeEntity())
                    }
                );

            showTime.Reserve(new List<Seat> { new Seat(1, 2)}, auditorium);

            Assert.That(showTime.DomainEvents, Has.One.InstanceOf<TicketReservedDomainEvent>());
            Assert.That(showTime.Tickets, Has.Count.EqualTo(2));
        }

        [Test()]
        public void Given_Reserved_Seats_But_Reservation_Expired_ReserveTest_Then_Reserve()
        {
            var auditorium = Substitute.ForPartsOf<AuditoriumEntity>(1, new List<Seat>());
            auditorium.IsSeatsContiguous(Arg.Any<IEnumerable<Seat>>()).Returns(true);

            var showTime = new ShowTimeEntity(
                    DateTime.UtcNow.AddDays(1),
                    "movieId",
                    1,
                    new Guid(),
                    new List<TicketEntity>
                    {
                        new TicketEntity(
                            createdTime: DateTime.UtcNow.AddDays(-1),
                            status: TicketStatus.Pending,
                            seats: new List<Seat> {new Seat(1, 1)},
                            showtime: new ShowTimeEntity()
                            )
                    }
                ) ;

            showTime.Reserve(new List<Seat> { new Seat(1, 1) }, auditorium);

            Assert.That(showTime.DomainEvents, Has.One.InstanceOf<TicketReservedDomainEvent>());
            Assert.That(showTime.Tickets, Has.Count.EqualTo(2));
        }
    }
}