using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.UnitTests.Consts;
using BuberDinner.Domain.Users;

using FluentAssertions;

using Moq;

namespace BuberDinner.Application.UnitTests.Authentication.Queries.Login
{
    public class LoginQueryHandlerTests
    {
        private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;

        private readonly MockRepository _mockRepository;

        private readonly Mock<IUserRepository> _mockUserRepository;

        public LoginQueryHandlerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockJwtTokenGenerator = _mockRepository.Create<IJwtTokenGenerator>();
            _mockUserRepository = _mockRepository.Create<IUserRepository>();
        }

        [Fact]
        public async Task HandleLoginCommand_WhenDataValid_ReturnToken()
        {
            // Arrange
            var loginQueryHandler = CreateLoginQueryHandler();
            LoginQuery query = AuthUtils.CreateLoginQuery();
            CancellationToken cancellationToken = default(CancellationToken);
            _mockJwtTokenGenerator.Setup(m => m.GenerateToken(It.IsAny<User>())).Returns(Constants.Token.Jwt);
            _mockUserRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(AuthUtils.CreateUser()); // simulate user already exists
            // Act
            var result = await loginQueryHandler.Handle(query, cancellationToken);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.ValidateCreatedFrom(query, Constants.Token.Jwt);
            _mockRepository.VerifyAll();
        }

        private LoginQueryHandler CreateLoginQueryHandler()
        {
            return new LoginQueryHandler(_mockJwtTokenGenerator.Object, _mockUserRepository.Object);
        }
    }
}
