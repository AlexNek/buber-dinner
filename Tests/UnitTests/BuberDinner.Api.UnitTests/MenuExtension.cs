using BuberDinner.Contracts.Menus;

using FluentAssertions;

namespace BuberDinner.Api.UnitTests
{
    internal static class MenuExtension
    {
        public static void ValidateCreatedFrom(this MenuResponse menuResponse, MenuRequest request)
        {
            menuResponse.Name.Should().Be(request.Name);
            menuResponse.Description.Should().Be(request.Description);
            menuResponse.Sections.Should().HaveSameCount(request.Sections);
            menuResponse.Sections.Zip(request.Sections).ToList().ForEach(pair => ValidateSection(pair.First, pair.Second));
        }

        private static void ValidateItem(MenuItemResponse item, MenuItemRequest command)
        {
            item.Id.Should().NotBeNull();
            item.Name.Should().Be(command.Name);
            item.Description.Should().Be(command.Description);
        }

        private static void ValidateSection(MenuSectionResponse section, MenuSectionRequest command)
        {
            section.Id.Should().NotBeNull();
            section.Name.Should().Be(command.Name);
            section.Description.Should().Be(command.Description);
            section.Items.Should().HaveSameCount(command.Items);
            section.Items.Zip(command.Items).ToList().ForEach(pair => ValidateItem(pair.First, pair.Second));
        }
    }
}
