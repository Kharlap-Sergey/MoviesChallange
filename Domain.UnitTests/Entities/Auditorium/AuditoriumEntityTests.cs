using NUnit.Framework;
using Domain.Entities.Auditorium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ValueObjects;
using Domain.Exceptions;
using Domain.Events;

namespace Domain.Entities.Auditorium.UnitTests
{
    [TestFixture()]
    public class AuditoriumEntityTests
    {
        [Test()]
        public void Given_Request_In_Past_ArrangeNewShowTest__Then_Not_Arranged()
        {
            var auditorium = new AuditoriumEntity(
                1, new List<Seat>()
                );

            Assert.Throws<InvalidOperationDomainException>(() =>
            {
                auditorium.ArrangeNewShow(
                    DateTime.UtcNow.AddDays(-1),
                    new MovieEntity());
            });
        }

        [Test()]
        public void Given_Valid_Request_ArrangeNewShowTest__Then_Arranged()
        {
            var auditorium = new AuditoriumEntity(
                1,
                new List<Seat>());

            auditorium.ArrangeNewShow(
                DateTime.UtcNow.AddDays(1),
                new MovieEntity());

            Assert.That(auditorium.DomainEvents, Has.One.InstanceOf<NewAuditoriumReservationDomainEvent<MovieEntity>>());
        }

        [Test()]
        public void Given_Seats_Are_Not_Contiguous_IsSeatsContiguousTest__Then_False()
        {
            var auditorium = new AuditoriumEntity(
                1,
                new List<Seat>());

            var seats = new List<Seat>
            {
                new Seat(1, 1),
                new Seat(1, 3),
                new Seat(1, 4)
            };

            Assert.That(auditorium.IsSeatsContiguous(seats), Is.False);
        }

        [Test()]
        public void Given_Seats_Are_Contiguous_IsSeatsContiguousTest__Then_True()
        {
            var auditorium = new AuditoriumEntity(
                1,
                new List<Seat>());

            var seats = new List<Seat>
            {
                new Seat(1, 1),
                new Seat(1, 2),
                new Seat(1, 3)
            };

            Assert.That(auditorium.IsSeatsContiguous(seats), Is.True);
        }
    }
}