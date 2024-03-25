using Domain.Abstractions;
using Infrastructure.MoviesProvider;
using ProtoDefinitions;

namespace Microsoft.Extensions.DependencyInjection;

public static class MoviesProviderDIExtensions
{
    public static IServiceCollection AddMoviesGrpcProvider(
        this IServiceCollection services,
        string serviceUrl,
        string serviceApiKey,
        Action<MoviesProviderCacheDurationOptions> configureCacheDurationOptions
        )
    {
        //already has retry policy enabled with 5 attempts
        services.AddGrpcClient<MoviesApi.MoviesApiClient>(
            configuration =>
            {
                configuration.Address = new Uri(serviceUrl);
            })
            .ConfigureChannel(
                configuration =>
                {
                    var httpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                    configuration.HttpHandler = httpHandler;
                })
            .AddCallCredentials(
                (context, metadata) =>
                {
                    metadata.Add("X-Apikey", serviceApiKey);

                    return Task.CompletedTask;
                });


        services.AddTransient<MoviesGrpcProvider, MoviesGrpcProvider>();
        services.Configure<MoviesProviderCacheDurationOptions>(
            configureCacheDurationOptions
            );
        services.AddTransient<IMoviesProvider, MoviesProviderCacheDecorator>(
            sf => ActivatorUtilities.CreateInstance<MoviesProviderCacheDecorator>(
                sf,
                sf.GetRequiredService<MoviesGrpcProvider>())
            );

        return services;
    }
}
