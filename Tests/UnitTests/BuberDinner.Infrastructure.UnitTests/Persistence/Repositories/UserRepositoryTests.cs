using BuberDinner.Domain.Users;
using BuberDinner.Infrastructure.Persistence;
using BuberDinner.Infrastructure.Persistence.Interceptors;
using BuberDinner.Infrastructure.Persistence.Repositories;

using FluentAssertions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Moq;

namespace BuberDinner.Infrastructure.UnitTests.Persistence.Repositories
{
    public class UserRepositoryTests
    {
        private readonly BuberDinnerDbContext _dbContext;

        private readonly Mock<IPublisher> _mediator;

        private readonly MockRepository _mockRepository;

        public UserRepositoryTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mediator = _mockRepository.Create<IPublisher>();

            var dbContextOptions = new DbContextOptionsBuilder<BuberDinnerDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") // Use in-memory database for testing
                .Options;
            _dbContext = new BuberDinnerDbContext(
                dbContextOptions,
                new PublishDomainEventsInterceptor(_mediator.Object));
        }

        [Fact]
        public async Task WhenUserValid_DbMustContainUserRecord()
        {
            // Arrange
            await ClearUserTableAsync();
            var user = AuthUtils.CreateUser();

            var testClass = CreateUserRepository();

            var usersBefore = await testClass.GetAllAsync();
            usersBefore.Should().HaveCount(0);
            // Act
            testClass.Add(user);

            // Assert
            var usersAfter = await testClass.GetAllAsync();
            usersAfter.Should().HaveCount(1);
        }

        [Fact]
        public async Task WhenUserWithEmailExists_UserMustBeReturned()
        {
            // Arrange
            await ClearUserTableAsync();
            var user = AuthUtils.CreateUser();
            var email = user.Email;
            var testClass = CreateUserRepository();

            var usersBefore = await testClass.GetAllAsync();
            usersBefore.Should().HaveCount(0);

            // Act
            testClass.Add(user);
            var result = testClass.GetUserByEmail(email);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = CreateUserRepository();
            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CannotCallAddWithNullUser()
        {
            var testClass = CreateUserRepository();

            FluentActions
                .Invoking(() => testClass.Add(default(User)))
                .Should().Throw<ArgumentNullException>().WithParameterName("user");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CannotCallGetUserByEmailWithInvalidEmail(string value)
        {
            await ClearUserTableAsync();
            var testClass = CreateUserRepository();

            FluentActions
                .Invoking(() => testClass.GetUserByEmail(value))
                .Should().Throw<ArgumentNullException>().WithParameterName("email");
        }

        [Fact]
        public void CannotConstructWithNullDbContext()
        {
            FluentActions
                .Invoking(() => new UserRepository(default(BuberDinnerDbContext)))
                .Should().Throw<ArgumentNullException>().WithParameterName("dbContext");
        }

        private UserRepository CreateUserRepository()
        {
            return new UserRepository(_dbContext);
        }

        private async Task ClearUserTableAsync()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);
            await _dbContext.SaveChangesAsync();
        }

    }
}
