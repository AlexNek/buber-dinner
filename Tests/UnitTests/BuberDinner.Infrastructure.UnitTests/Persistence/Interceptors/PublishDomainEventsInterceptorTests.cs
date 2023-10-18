using BuberDinner.Contracts.Menus;
using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.Menus;
using BuberDinner.Domain.Menus.Events;
using BuberDinner.Infrastructure.Persistence;
using BuberDinner.Infrastructure.Persistence.Interceptors;
using BuberDinner.Infrastructure.UnitTests.Consts;

using FluentAssertions;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

using Moq;

namespace BuberDinner.Infrastructure.UnitTests.Persistence.Interceptors;

public class PublishDomainEventsInterceptorTests
{
    private readonly Mock<IPublisher> _mediator;

    private readonly MockRepository _mockRepository;

    private readonly Mock<ILoggingOptions> _loggingOptions;

    public PublishDomainEventsInterceptorTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Strict);
        _mediator = _mockRepository.Create<IPublisher>();
        _loggingOptions = _mockRepository.Create<ILoggingOptions>();
       ;
    }
    
    [Fact]
    public void CanConstruct()
    {
        // Act
        var instance = new PublishDomainEventsInterceptor(_mediator.Object);

        // Assert
        instance.Should().NotBeNull();
    }

    [Fact]
    public async Task CannotCallSavingChangesAsyncWithNullEventData()
    {
        //Func<Task> act = async () =>
        //    {
        //        await _testClass.SavingChangesAsync(default(DbContextEventData), new InterceptionResult<int>(), CancellationToken.None);
        //    };

        //await act.Should().ThrowAsync<ArgumentNullException>().WithParameterName("eventData"); 
        var testClass = CreateInterceptor();
        await FluentActions
            .Invoking(
                async () => await testClass.SavingChangesAsync(
                                default(DbContextEventData),
                                new InterceptionResult<int>(),
                                CancellationToken.None))
            .Should().ThrowAsync<ArgumentNullException>().WithParameterName("eventData");
    }

    [Fact]
    public void CannotCallSavingChangesWithNullEventData()
    {
        var testClass = CreateInterceptor();
        FluentActions
            .Invoking(() => testClass.SavingChanges(default(DbContextEventData), new InterceptionResult<int>()))
            .Should().Throw<ArgumentNullException>().WithParameterName("eventData");
    }

    [Fact]
    public void CannotConstructWithNullMediator()
    {
        FluentActions
            .Invoking(() => new PublishDomainEventsInterceptor(default(IPublisher)))
            .Should().Throw<ArgumentNullException>().WithParameterName("mediator");
    }

    [Fact]
    public void WhenMenuAdded_DomaindEventMustBePublishedAfterSaveChanges()
    {
        // Arrange
        var testClass = CreateInterceptor();
        var dbContext = CreateDbContext(testClass);

        _mediator.Setup(mock => mock.Publish(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        MenuRequest menuRequest = MenuUtils.CreateMenuRequest();
        var hostId = new Guid("35d538f2-5bca-4668-bff0-8f57a059692f");
        Menu menu = MenuUtils.CreateMenu(menuRequest, hostId);

        // Act
        dbContext.Menus.Add(menu);
        dbContext.SaveChanges();

        // Assert
        _mediator.Verify(mock => mock.Publish(
                It.Is<IDomainEvent>(x => (x as MenuCreated).Menu.Name == Constants.Menu.Name), It.IsAny<CancellationToken>()),
            Times.Exactly(1));

        _mockRepository.VerifyAll();
    }

    private BuberDinnerDbContext CreateDbContext(PublishDomainEventsInterceptor testClass)
    {
        var dbContextOptions = new DbContextOptionsBuilder<BuberDinnerDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase") // Use in-memory database for testing
            .Options;
        return new BuberDinnerDbContext(dbContextOptions, testClass);
    }

    [Fact]
    public async Task WhenMenuAdded_DomaindEventMustBePublishedAfterSaveChangesAsync()
    {
        // Arrange
        var testClass = CreateInterceptor();
        var dbContext = CreateDbContext(testClass);

        _mediator.Setup(mock => mock.Publish(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        MenuRequest menuRequest = MenuUtils.CreateMenuRequest();
        var hostId = new Guid("35d538f2-5bca-4668-bff0-8f57a059692f");
        Menu menu = MenuUtils.CreateMenu(menuRequest, hostId);

        // Act
        dbContext.Menus.Add(menu);
        await dbContext.SaveChangesAsync();

        // Assert
        _mediator.Verify(mock => mock.Publish(
                It.Is<IDomainEvent>(x => (x as MenuCreated).Menu.Name == Constants.Menu.Name), It.IsAny<CancellationToken>()),
            Times.Exactly(1));

        _mockRepository.VerifyAll();
    }

    
    private PublishDomainEventsInterceptor CreateInterceptor()
    {
        return new PublishDomainEventsInterceptor(_mediator.Object);
    }
}