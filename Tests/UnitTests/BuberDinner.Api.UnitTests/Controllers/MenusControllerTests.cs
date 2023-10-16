using BuberDinner.Api.Controllers;
using BuberDinner.Application.Menus.Commands.CreateMenu;
using BuberDinner.Contracts.Menus;
using BuberDinner.Domain.Menus;

using ErrorOr;

using FluentAssertions;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Moq;

namespace BuberDinner.Api.UnitTests.Controllers
{
    public class MenusControllerTests
    {
        private readonly Mock<IMapper> _mapper;

        private readonly Mock<ISender> _mediator;

        private readonly MockRepository _mockRepository;

        private readonly MenusController _testClass;

        public MenusControllerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _mediator = _mockRepository.Create<ISender>();
            _mapper = _mockRepository.Create<IMapper>();
            _testClass = new MenusController(_mapper.Object, _mediator.Object);
        }

        [Fact]
        public async Task MenuController_CanCallCreateMenu()
        {
            // Arrange
            CreateMenuRequest request = MenuUtils.CreateRequest();
            var hostId = new Guid("35d538f2-5bca-4668-bff0-8f57a059692f");
            //var valueTuple = (CreateMenuRequest Request, Guid HostId);
            _mapper.Setup(mock => mock.Map<CreateMenuCommand>(It.IsAny<(CreateMenuRequest, Guid)>()))
                .Returns(((CreateMenuRequest, Guid) inp) =>
                    {
                        (CreateMenuRequest? menuRequest, Guid guid) = inp;
                        return MenuUtils.CreateMenuCommand(guid, menuRequest);
                    });
            _mapper.Setup(mock => mock.Map<MenuResponse>(It.IsAny<Menu>())).Returns(
                (Menu inp) => MenuUtils.CreateMenuResponse(inp));

            _mediator.Setup(mock => mock.Send(It.IsAny<CreateMenuCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ErrorOr<Menu>)MenuUtils.CreateMenu(request, hostId));
            // Act
            var result = await _testClass.CreateMenu(request, hostId) as OkObjectResult;

            // Assert
            MenuResponse menuResponse = result.Value as MenuResponse;
            MenuExtension.ValidateCreatedFrom(menuResponse, request);
         
            //_mapper.Verify(mock => mock.Map<CreateMenuCommand>(It.IsAny<object>()));
            //_mapper.Verify(mock => mock.Map<MenuResponse>(It.IsAny<object>()));
            //_mediator.Verify(mock => mock.Send(It.IsAny<IRequest<ErrorOr<Menu>>>(), It.IsAny<CancellationToken>()));

            _mockRepository.VerifyAll();
        }

        [Fact]
        public void MenuController_CanConstruct()
        {
            // Act
            var instance = new MenusController(_mapper.Object, _mediator.Object);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public async Task MenuController_CannotCallCreateMenuWithNullRequest()
        {
            await FluentActions.Invoking(() => _testClass.CreateMenu(default(CreateMenuRequest), new Guid("4020d580-2bc2-4976-974b-24390acb4963")))
                .Should().ThrowAsync<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void MenuController_CannotConstructWithNullMapper()
        {
            FluentActions.Invoking(() => new MenusController(default(IMapper), _mediator.Object)).Should().Throw<ArgumentNullException>()
                .WithParameterName("mapper");
        }

        [Fact]
        public void MenuController_CannotConstructWithNullMediator()
        {
            FluentActions.Invoking(() => new MenusController(_mapper.Object, default(ISender))).Should().Throw<ArgumentNullException>()
                .WithParameterName("mediator");
        }
    }
}
