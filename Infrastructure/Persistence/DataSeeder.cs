using Domain.Entities.Auditorium;
using Domain.Entities.ShowTimes;
using Domain.Entities.Tickets;
using Domain.ValueObjects;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ApiApplication.Database
{
    public class SampleDataSeed
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
            context.Database.EnsureCreated();

            context.Auditoriums.Add(
                new AuditoriumEntity(
                    1,
                    GenerateSeats(1, 28, 22)
                )
            );

            var showtime = new ShowTimeEntity(
                    DateTime.UtcNow.AddDays(1),
                    "ID-test1",
                    1,
                    Guid.Parse("b80013f4-7ee5-4292-a397-82b577de6502")
                    );

            context.Showtimes.Add(
                    showtime
                );

            context.Tickets.Add(
                new TicketEntity(
                    Guid.Parse("b80013f4-7ee5-1111-a397-82b577de6502"),
                    new List<Seat> { new Seat(1, 1) },
                    showtime,
                    TicketStatus.Pending,
                    DateTime.UtcNow
                    )
                );

            context.SaveChanges();
        }

        private static List<Seat> GenerateSeats(int auditoriumId, short rows, short seatsPerRow)
        {
            var seats = new List<Seat>();
            for (short r = 1; r <= rows; r++)
                for (short s = 1; s <= seatsPerRow; s++)
                    seats.Add(new Seat(r, s));

            return seats;
        }
    }
}
