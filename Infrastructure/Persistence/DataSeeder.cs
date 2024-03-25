using Domain.Entities.Auditorium;
using Domain.Entities.ShowTimes;
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

            context.Showtimes.Add(
                new ShowTimeEntity(
                    DateTime.UtcNow.AddDays(1),
                    "2134",
                    //new MovieEntity
                    //{
                    //    Id  = "2134",
                    //    Title = "Inception",
                    //    //fill with random data fields 
                    //    //use MovieEntity for context
                    //    Crew = "Christopher Nolan",
                    //    FullTitle = "Inception",
                    //    ImDbRating = "8.8",
                    //    ImDbRatingCount = "2,000,000",
                    //    Image = "adfasdfa",
                    //    Rank = "1",
                    //    Year = "2010"
                    //},
                    1
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
