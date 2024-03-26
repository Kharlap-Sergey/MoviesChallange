using Domain.Abstractions;
using Domain.Entities;
using Domain.Entities.Auditorium;
using Domain.Entities.Auditorium.Commands;
using Domain.Events;
using Domain.ValueObjects;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Integration;

public class Tests
{
    private AppDbContext _context;
    private IMediator _mediator;
    private AuditoriumRepository _auditoriumRepo;
    private IMoviesProvider _movieProvider;
    private ArrangeNewShowHandler _handler;

    [OneTimeSetUp]
    public void Setup()
    {
        var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestCatalog")
            .Options;
        _context = new AppDbContext(dbOptions);

        _context.Auditoriums.Add(
            new AuditoriumEntity(
                1, 
                new List<Seat>()
                )
            );
        _context.SaveChanges();

        _mediator = Substitute.For<IMediator>();

        _auditoriumRepo = new AuditoriumRepository(
            _mediator,
            _context
            );

        _movieProvider = Substitute.For<IMoviesProvider>();
        _movieProvider.GetById(Arg.Is("ID-test1"), Arg.Any<CancellationToken>()).Returns(Task.FromResult(
            new MovieEntity
            {
                Id = "ID-test1",
                Title = "Test Movie",
            }));
        _handler = new ArrangeNewShowHandler(_auditoriumRepo, _movieProvider);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task Test1()
    {
        var arrangeNewShowCommand = new ArrangeNewShowCommand(
            "ID-test1",
            DateTime.UtcNow.AddDays(2),
            1);

        await _handler.Handle(arrangeNewShowCommand, CancellationToken.None);
        
        await _mediator.Received(1).Publish(Arg.Any<NewAuditoriumReservationDomainEvent<MovieEntity>>());
    }
}