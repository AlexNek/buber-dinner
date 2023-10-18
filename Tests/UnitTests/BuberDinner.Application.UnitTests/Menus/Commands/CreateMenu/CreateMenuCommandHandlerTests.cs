using BuberDinner.Application.Common.Interfaces.Persistance;
using BuberDinner.Application.Menus.Commands.CreateMenu;
using BuberDinner.Application.UnitTests.Consts;
using BuberDinner.Domain.Menus;

using FluentAssertions;

using Moq;

namespace BuberDinner.Application.UnitTests.Menus.Commands.CreateMenu
{
    /// <summary>
    /// Class CreateMenuCommandHandlerTests.
    /// We test only valid menu as invalid value will be handled on upper level
    /// </summary>
    public class CreateMenuCommandHandlerTests
    {
        private readonly Mock<IMenuRepository> _mockMenuRepository;

        private readonly MockRepository _mockRepository;

        public CreateMenuCommandHandlerTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _mockMenuRepository = _mockRepository.Create<IMenuRepository>();
            _mockMenuRepository.Setup(m => m.AddAsync(It.IsAny<Menu>())).Returns(Task.CompletedTask);
        }

        [Theory]
        [MemberData(nameof(ValidCreateMenuCommands))]
        public async Task HandleCreateMenu_WhenMenuIsValid_ShouldCreateAndReturnMenu(CreateMenuCommand menuCommand)
        {
            // Arrange
            var createMenuCommandHandler = CreateCreateMenuCommandHandler();
            //CreateMenuCommand menuCommand = CreateMenuCommandUtils.CreateCommand();
            CancellationToken cancellationToken = default(CancellationToken);

            // Act
            var result = await createMenuCommandHandler.Handle(menuCommand, cancellationToken);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().NotBeNull();
            result.Value.ValidateCreatedFrom(menuCommand);

            _mockMenuRepository.Verify(m => m.AddAsync(result.Value), Times.Once());
            //_mockRepository.VerifyAll();
        }

        private CreateMenuCommandHandler CreateCreateMenuCommandHandler()
        {
            return new CreateMenuCommandHandler(_mockMenuRepository.Object);
        }

        public static IEnumerable<object[]> ValidCreateMenuCommands()
        {
            yield return new[] { CreateMenuCommandUtils.CreateCommand() };

            yield return new[]
                             {
                                 CreateMenuCommandUtils.CreateCommand(
                                     sections: CreateMenuCommandUtils.CreateSectionsCommand())
                             };
            yield return new[]
                             {
                                 CreateMenuCommandUtils.CreateCommand(
                                     sections: CreateMenuCommandUtils.CreateSectionsCommand(
                                         sectionCount:3,
                                         items:CreateMenuCommandUtils.CreateMenuItemsCommand(itemCount:3)
                                         ))
                             };
        }
    }
}