using BuberDinner.Contracts.Menus;
using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.Menus;
using BuberDinner.Infrastructure.Persistence;
using BuberDinner.Infrastructure.Persistence.Interceptors;
using BuberDinner.Infrastructure.Persistence.Repositories;

using FluentAssertions;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

using Xunit.Abstractions;

namespace BuberDinner.Infrastructure.UnitTests.Persistence.Repositories
{
    public class MenuRepositoryTests
    {
        private readonly ILogger<MenuRepositoryTests> _logger;

        private readonly Mock<IPublisher> _mediator;

        private readonly MockRepository _mockRepository;

        private readonly ITestOutputHelper _output;

        public MenuRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mediator = _mockRepository.Create<IPublisher>();
            _logger = output.BuildLoggerFor<MenuRepositoryTests>();
        }

        //public void ClearDatabase()
        //{
        //    _dbContext.Dispose();
        //    _dbContext = null;

        //    CreateDatabase();
        //}

        [Fact]
        public void CanConstruct()
        {
            // Act
            var dbContext = CreateDatabase();
            var instance = CreateMenuRepository(dbContext);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CannotCallAddWithNullMenu()
        {
            var dbContext = CreateDatabase();
            var testClass = CreateMenuRepository(dbContext);

            FluentActions
                .Invoking(async () => await testClass.AddAsync(default(Menu)))
                .Should().ThrowAsync<ArgumentNullException>().WithParameterName("menu");
        }

        [Fact]
        public void CannotConstructWithNullDbContext()
        {
            FluentActions
                .Invoking(() => new MenuRepository(default(BuberDinnerDbContext)))
                .Should().Throw<ArgumentNullException>().WithParameterName("dbContext");
        }

        [Fact]
        public async Task WhenMenuIsValid_DbMustContainMenuRecord()
        {
            var services = new ServiceCollection()
                .AddLogging((builder) => builder.AddXUnit(_output).SetMinimumLevel(LogLevel.Debug)); 

            var calculator = services
                .BuildServiceProvider();
                
            // Arrange
            MenuRequest menuRequest = MenuUtils.CreateMenuRequest();
            var hostId = new Guid("35d538f2-5bca-4668-bff0-8f57a059692f");
            var menu = MenuUtils.CreateMenu(menuRequest, hostId);
            var dbContext = CreateDatabase();
            var testClass = CreateMenuRepository(dbContext);

            _mediator.Setup(mock => mock.Publish(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var menusBefore = await testClass.GetAllAsync();
            menusBefore.Should().HaveCount(0);

            // Act
            await testClass.AddAsync(menu);
            //_logger.LogDebug($"--Add menu before {dbContext.Menus.Count()}");
            //await dbContext.Menus.AddAsync(menu);
            //await dbContext.SaveChangesAsync();
            //_logger.LogDebug($"--Add menu after {dbContext.Menus.Count()}");

            // Assert
            var menusAfter = await testClass.GetAllAsync();
            menusAfter.Should().HaveCount(1);
            _mockRepository.VerifyAll();
        }

        private BuberDinnerDbContext CreateDatabase()
        {
            //important to use different database name for run all tests
            var dbContextOptions = new DbContextOptionsBuilder<BuberDinnerDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase2") // Use in-memory database for testing
                .Options;
            var dbContext = new BuberDinnerDbContext(
                dbContextOptions,
                new PublishDomainEventsInterceptor(_mediator.Object));
            return dbContext;
        }

        private MenuRepository CreateMenuRepository(BuberDinnerDbContext dbContext)
        {
            return new MenuRepository(dbContext);
        }

        //private async Task ClearTablesAsync()
        //{
        //    //Menus
        //    _dbContext.Menus.RemoveRange(_dbContext.Menus);
        //    await _dbContext.SaveChangesAsync();
        //}
    }
}
