using BuberDinner.Api.Controllers;
using BuberDinner.Api.UnitTests.Consts;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Application.Common.Interfaces.Persistance;
using BuberDinner.Contracts.Authentication;

using ErrorOr;

using FluentAssertions;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Moq;

namespace BuberDinner.Api.UnitTests.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IMapper> _mapper;

        private readonly Mock<ISender> _mediator;

        private readonly MockRepository _mockRepository;
        public AuthenticationControllerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mediator = _mockRepository.Create<ISender>();
            _mapper = _mockRepository.Create<IMapper>();
        }

        [Fact]
        public async Task AuthController_LoginWhenDataValid_ReturnTokenAndUser()
        {
            // Arrange
            var loginRequest = AuthUtils.CreateLoginRequest();

            _mapper.Setup(mock => mock.Map<LoginQuery>(It.IsAny<LoginRequest>()))
                .Returns((LoginRequest inp) => new LoginQuery(inp.Email, inp.Password));

            _mediator.Setup(mock => mock.Send(It.IsAny<LoginQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ErrorOr<AuthenticationResult>)new AuthenticationResult(AuthUtils.CreateUser(),Constants.Token.Jwt));

            _mapper.Setup(mock => mock.Map<AuthenticationResponse>(It.IsAny<AuthenticationResult>())).Returns(
                (AuthenticationResult inp) =>
                    new AuthenticationResponse(
                        inp.User.Id.Value,
                        inp.User.FirstName,
                        inp.User.LastName,
                        inp.User.Email,
                        inp.Token
                    ));

            var testClass = CreateController();

            // Act
            var result = await testClass.Login(loginRequest) as OkObjectResult;
            
            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            var response = result.Value as AuthenticationResponse;
            response.ValidateCreatedFrom(loginRequest, response.Token);

            response.FirstName.Should().Be(Constants.Auth.FirstName);
            response.LastName.Should().Be(Constants.Auth.LastName);

            
            //_mediator.Verify(mock => mock.Send(It.IsAny<IRequest<ErrorOr<AuthenticationResult>>>(), It.IsAny<CancellationToken>()));
            //_mapper.Verify(mock => mock.Map<LoginQuery>(It.IsAny<object>()));
            //_mapper.Verify(mock => mock.Map<AuthenticationResponse>(It.IsAny<object>()));
            
            _mockRepository.VerifyAll();
        }

        [Fact]
        public async Task AuthController_RegisterWhenDataValid_ReturnTokenAndUser()
        {
            // Arrange
            RegisterRequest request = AuthUtils.CreateRegisterRequest();

            _mapper.Setup(mock => mock.Map<RegisterCommand>(It.IsAny<RegisterRequest>())).Returns(
                (RegisterRequest inp) =>
                    new RegisterCommand(inp.FirstName, inp.LastName, inp.Email, inp.Password));

            _mediator.Setup(mock => mock.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ErrorOr<AuthenticationResult>)new AuthenticationResult(AuthUtils.CreateUser(), Constants.Token.Jwt));

            _mapper.Setup(mock => mock.Map<AuthenticationResponse>(It.IsAny<AuthenticationResult>())).Returns(
                (AuthenticationResult inp) =>
                    new AuthenticationResponse(
                        inp.User.Id.Value,
                        inp.User.FirstName,
                        inp.User.LastName,
                        inp.User.Email,
                        inp.Token
                    ));

            var testClass = CreateController();
            // Act
            var result = await testClass.Register(request) as OkObjectResult;

            // Assert
            var response = result.Value as AuthenticationResponse;
            response.ValidateCreatedFrom(request, response.Token);

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void AuthController_CanConstruct()
        {
            // Act
            var instance = CreateController();

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public async Task AuthController_CannotCallLoginWithNullRequest()
        {
            var testClass = CreateController();

            await FluentActions.Invoking(() => testClass.Login(default(LoginRequest))).Should().ThrowAsync<ArgumentNullException>()
                .WithParameterName("request");
        }

        [Fact]
        public async Task AuthController_CannotCallRegisterWithNullRequest()
        {
            var testClass = CreateController();

            await FluentActions.Invoking(() => testClass.Register(default(RegisterRequest))).Should().ThrowAsync<ArgumentNullException>()
                .WithParameterName("request");
        }

        [Fact]
        public void AuthController_CannotConstructWithNullMapper()
        {
            FluentActions.Invoking(() => new AuthenticationController(_mediator.Object, default(IMapper))).Should().Throw<ArgumentNullException>()
                .WithParameterName("mapper");
        }

        [Fact]
        public void AuthController_CannotConstructWithNullMediator()
        {
            FluentActions.Invoking(() => new AuthenticationController(default(ISender), _mapper.Object)).Should().Throw<ArgumentNullException>()
                .WithParameterName("mediator");
        }

        private AuthenticationController CreateController()
        {
            return new AuthenticationController(_mediator.Object, _mapper.Object);
        }
    }
}
