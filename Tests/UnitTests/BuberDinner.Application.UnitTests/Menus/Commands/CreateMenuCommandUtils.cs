using BuberDinner.Application.Menus.Commands.CreateMenu;
using BuberDinner.Application.UnitTests.Consts;

namespace BuberDinner.Application.UnitTests.Menus.Commands;

public static class CreateMenuCommandUtils
{
    public static CreateMenuCommand CreateCommand(List<MenuSectionCommand>? sections = null)
    {
        return new CreateMenuCommand(
            Constants.Host.Id.Value,
            Constants.Menu.Name,
            Constants.Menu.Description,
            sections ?? CreateSectionsCommand(1)
        );
    }

    public static List<MenuSectionCommand> CreateSectionsCommand(int sectionCount = 1, List<MenuItemCommand>? items = null)
    {
        return Enumerable.Range(0, sectionCount).Select(
            index => new MenuSectionCommand(
                Constants.Menu.SectionNameFromIndex(index),
                Constants.Menu.SectionDescriptionFromIndex(index),
                items ?? CreateMenuItemsCommand(1)
            )).ToList();
    }

    public static List<MenuItemCommand> CreateMenuItemsCommand(int itemCount = 1)
    {
        return Enumerable.Range(0, itemCount).Select(
            index => new MenuItemCommand(Constants.Menu.ItemNameFromIndex(index), Constants.Menu.ItemDescriptionFromIndex(index))
        ).ToList();
    }
}
