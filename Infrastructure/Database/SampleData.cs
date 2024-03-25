using ApiApplication.Database.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ApiApplication.Database
{
    public class SampleData
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<CinemaContext>();
            context.Database.EnsureCreated();
            
            context.Auditoriums.Add(new AuditoriumModel
            {
                Id = 1,
                Showtimes = new List<ShowtimeEntity> 
                { 
                    new ShowtimeEntity
                    {
                        Id = 1,
                        SessionDate = DateTime.UtcNow.AddDays(1),
                        Movie = new MovieEntityData
                        {
                            Id = 1,
                            Title = "Inception",
                            ImdbId = "tt1375666",
                            ReleaseDate = new DateTime(2010, 01, 14),
                            Stars = "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe"                            
                        },
                        AuditoriumId = 1,
                    } 
                },
                Seats = GenerateSeats(1, 28, 22)
            });

            context.Auditoriums.Add(new AuditoriumModel
            {
                Id = 2,
                Seats = GenerateSeats(2, 21, 18)
            });

            context.Auditoriums.Add(new AuditoriumModel
            {
                Id = 3,
                Seats = GenerateSeats(3, 15, 21)
            });

            context.SaveChanges();
        }

        private static List<SeatEntity> GenerateSeats(int auditoriumId, short rows, short seatsPerRow)
        {
            var seats = new List<SeatEntity>();
            for (short r = 1; r <= rows; r++)
                for (short s = 1; s <= seatsPerRow; s++)
                    seats.Add(new SeatEntity { AuditoriumId = auditoriumId, Row = r, SeatNumber = s });

            return seats;
        }
    }
}
