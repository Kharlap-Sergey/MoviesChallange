using Domain.Abstractions;
using Infrastructure.Persistence.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class PersistenceDIExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services
        )
    {
        services.AddScoped<IAuditoriumRepository, AuditoriumRepository>();

        services.AddScoped<IShowTimeRepository, ShowTimeRepository>();

        services.AddScoped<ITicketRepository, TicketRepository>();

        return services;
    }
}
