using BuberDinner.Application.Menus.Commands.CreateMenu;
using BuberDinner.Domain.Menus;
using BuberDinner.Domain.Menus.Entites;

using FluentAssertions;

namespace BuberDinner.Application.UnitTests.Menus
{
    internal static class MenuExtension
    {
        public static void ValidateCreatedFrom(this Menu menu, CreateMenuCommand command)
        {
            menu.Name.Should().Be(command.Name);
            menu.Description.Should().Be(command.Description);
            menu.Sections.Should().HaveSameCount(command.Sections);
            menu.Sections.Zip(command.Sections).ToList().ForEach(pair => ValidateSection(pair.First, pair.Second));
        }

        private static void ValidateItem(MenuItem item, MenuItemCommand command)
        {
            item.Id.Should().NotBeNull();
            item.Name.Should().Be(command.Name);
            item.Description.Should().Be(command.Description);
        }

        private static void ValidateSection(MenuSection section, MenuSectionCommand command)
        {
            section.Id.Should().NotBeNull();
            section.Name.Should().Be(command.Name);
            section.Description.Should().Be(command.Description);
            section.Items.Should().HaveSameCount(command.Items);
            section.Items.Zip(command.Items).ToList().ForEach(pair => ValidateItem(pair.First, pair.Second));
        }
    }
}
