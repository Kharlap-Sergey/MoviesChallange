using Microsoft.EntityFrameworkCore;
using Domain.Entities.ShowTimes;
using Domain.Entities.Auditorium;
using Domain.Entities.Tickets;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<AuditoriumEntity> Auditoriums { get; set; }

    public DbSet<ShowTimeEntity> Showtimes { get; set; }

    public DbSet<TicketEntity> Tickets{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShowTimeEntity>(build =>
        {
            build.HasKey(entry => entry.Id);
            build.HasMany(entry => entry.Tickets)
                .WithOne(entry => entry.Showtime);
            build.HasOne<AuditoriumEntity>()
                .WithMany()
                .HasForeignKey(entry => entry.AuditoriumId);

            build.Property(entry => entry.Tickets)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            //var navigation 
            //    = build.Metadata.FindNavigation(nameof(ShowTimeEntity.Tickets));
            //navigation!.SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<TicketEntity>(build =>
        {
            build.HasKey(entry => entry.Id);

            build.OwnsOne(entry => entry.Status);
            build.OwnsMany(entity => entity.Seats);
        });
        
        modelBuilder.Entity<AuditoriumEntity>(build =>
        {
            build.HasKey(entry => entry.Id);
            build.Property(entry => entry.Id).ValueGeneratedOnAdd();
            build.OwnsMany(entry => entry.Seats);
        });
    }
}