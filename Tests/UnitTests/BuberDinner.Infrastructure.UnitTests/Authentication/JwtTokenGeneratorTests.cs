using BuberDinner.Application.Common.Interfaces.Services;
using BuberDinner.Domain.Users;
using BuberDinner.Infrastructure.Authentication;

using FluentAssertions;

using Microsoft.Extensions.Options;

using Moq;

namespace BuberDinner.Infrastructure.UnitTests.Authentication
{
    public class JwtTokenGeneratorTests
    {
        private readonly MockRepository _mockRepository;

        private readonly Mock<IDateTimeProvider> _dateTimeProvider;

        private readonly Mock<IOptions<JwtSettings>> _jwtOptions;

        public JwtTokenGeneratorTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _jwtOptions = _mockRepository.Create<IOptions<JwtSettings>>();
            _dateTimeProvider = _mockRepository.Create<IDateTimeProvider>();

            JwtSettings jwtSettings = new JwtSettings() { Secret = "0123456789012345" };
            _jwtOptions.SetupGet(x => x.Value).Returns(jwtSettings);
        }

        [Fact]
        public void ValidUser_ReturnToken()
        {
            // Arrange
            var user = AuthUtils.CreateUser();

            _dateTimeProvider.Setup(mock => mock.UtcNow).Returns(DateTime.UtcNow);
          
           
            var testClass = CreateTokenGenerator();

            // Act
            var result = testClass.GenerateToken(user);

            // Assert
            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJCdWJlckRpbm5lci5Eb21haW4uVXNlcnMuVmFsdWVPYmplY3RzLlVzZXJJZCIsImdpdmVuX25hbWUiOiJGaXJzdCBOYW1lIiwiZmFtaWx5X25hbWUiOiJMYXN0IE5hbWUiLCJqdGkiOiI2NDgzMzY1MC1jNmZmLTQyNTctYWYwNy05MmRmY2E1MmI3NGMiLCJleHAiOjE2OTc1NDUzNjB9.ezJLP-H3aOjBCYurN3DvxawNuhaUi8una29yA-qW4Jw
            result.Should().NotBeNull();
            result.Length.Should().Be(305);
            _mockRepository.VerifyAll();
            //throw new NotImplementedException("Create or modify test");
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = CreateTokenGenerator();

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CannotCallGenerateTokenWithNullUser()
        {
            var testClass = CreateTokenGenerator();

            FluentActions.Invoking(() => testClass.GenerateToken(default(User))).Should().Throw<ArgumentNullException>().WithParameterName("user");
        }

        [Fact]
        public void CannotConstructWithNullDateTimeProvider()
        {
            FluentActions.Invoking(() => new JwtTokenGenerator(default(IDateTimeProvider), _jwtOptions.Object)).Should()
                .Throw<ArgumentNullException>().WithParameterName("dateTimeProvider");
        }

        [Fact]
        public void CannotConstructWithNullJwtOptions()
        {
            FluentActions.Invoking(() => new JwtTokenGenerator(_dateTimeProvider.Object, default(IOptions<JwtSettings>))).Should()
                .Throw<ArgumentNullException>().WithParameterName("jwtOptions");
        }

        private JwtTokenGenerator CreateTokenGenerator()
        {
            return new JwtTokenGenerator(_dateTimeProvider.Object, _jwtOptions.Object);
        }
    }
}
