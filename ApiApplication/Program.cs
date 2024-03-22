using System;
using System.Net.Http;
using System.Threading.Tasks;
using ApiApplication.Database;
using ApiApplication.Database.Repositories;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Domain.Movies.Abstractions;
using ApiApplication.Infrastructure;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProtoDefinitions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

builder.Services.AddGrpcClient<MoviesApi.MoviesApiClient>(
    configuration =>
        {
            configuration.Address = new Uri(builder.Configuration["MoviesService:BaseUrl"]);
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
            metadata.Add("X-Apikey", builder.Configuration["MoviesService:X-Apikey"]);

            return Task.CompletedTask;
        });

builder.Services.AddTransient<IMoviesService, MoviesService>();

builder.Services.AddTransient<IShowtimesRepository, ShowtimesRepository>();
builder.Services.AddTransient<ITicketsRepository, TicketsRepository>();
builder.Services.AddTransient<IAuditoriumsRepository, AuditoriumsRepository>();

builder.Services.AddDbContext<CinemaContext>(options =>
{
    options.UseInMemoryDatabase("CinemaDb")
        .EnableSensitiveDataLogging()
        .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
});
builder.Services.AddControllers();

builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

SampleData.Initialize(app);

app.Run();