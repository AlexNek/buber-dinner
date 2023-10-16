using BuberDinner.Api.UnitTests.Consts;
using BuberDinner.Application.Menus.Commands.CreateMenu;
using BuberDinner.Contracts.Menus;
using BuberDinner.Domain.Dinners.ValueObjects;
using BuberDinner.Domain.Hosts.ValueObjects;
using BuberDinner.Domain.MenuReview.ValueObjects;
using BuberDinner.Domain.Menus;

namespace BuberDinner.Api.UnitTests;

public static class MenuUtils
{
    public static List<MenuSectionCommand> Convert(List<MenuSection> sections)
    {
        return Enumerable.Range(0, sections.Count).Select(
            index => new MenuSectionCommand(
                sections[index].Name,
                sections[index].Description,
                Convert(sections[index].Items)
            )).ToList();
    }

    public static List<MenuItemCommand> Convert(List<MenuItem> items)
    {
        IEnumerable<int> range = Enumerable.Range(0, items.Count);
        IEnumerable<MenuItemCommand> commands = range.Select(
            index => new MenuItemCommand(items[index].Name, items[index].Description)
        );
        return commands.ToList();
    }

    public static List<string> Convert(IReadOnlyList<DinnerId> dinnerIds)
    {
        IEnumerable<int> range = Enumerable.Range(0, dinnerIds.Count);
        IEnumerable<string> items = range.Select(
            index => dinnerIds[index].ToString()
        );
        return items.ToList();
    }

    public static List<string> Convert(IReadOnlyList<MenuReviewId> menuReviewIds)
    {
        IEnumerable<int> range = Enumerable.Range(0, menuReviewIds.Count);
        IEnumerable<string> items = range.Select(
            index => menuReviewIds[index].ToString()
        );
        return items.ToList();
    }

    public static List<MenuSectionResponse> Convert2Response(IReadOnlyList<Domain.Menus.Entites.MenuSection> sections)
    {
        IEnumerable<int> range = Enumerable.Range(0, sections.Count);
        IEnumerable<MenuSectionResponse> responses = range.Select(
            index => new MenuSectionResponse(
                index.ToString(),
                sections[index].Name,
                sections[index].Description,
                Convert2Response(sections[index].Items))
        );
        return responses.ToList();
    }

    public static List<MenuItemResponse> Convert2Response(IReadOnlyList<Domain.Menus.Entites.MenuItem> items)
    {
        IEnumerable<int> range = Enumerable.Range(0, items.Count);
        IEnumerable<MenuItemResponse> responses = range.Select(
            index => new MenuItemResponse(index.ToString(), items[index].Name, items[index].Description)
        );
        return responses.ToList();
    }

    public static Menu CreateMenu(CreateMenuRequest createMenuRequest, Guid hostId)
    {
        Menu menu = Menu.Create(HostId.Create(hostId), Constants.Menu.Name, Constants.Menu.Description, Convert2Entities(CreateSections()));

        return menu;
    }

    public static CreateMenuCommand CreateMenuCommand(Guid hostId, CreateMenuRequest inp)
    {
        return new CreateMenuCommand(
            hostId,
            inp.Name,
            inp.Description,
            Convert(inp.Sections));
    }

    public static List<MenuItem> CreateMenuItems(int itemCount = 1)
    {
        return Enumerable.Range(0, itemCount).Select(
            index => new MenuItem(Constants.Menu.ItemNameFromIndex(index), Constants.Menu.ItemDescriptionFromIndex(index))
        ).ToList();
    }

    public static MenuResponse CreateMenuResponse(Menu inp)
    {
        return new MenuResponse(
            inp.Id.ToString(),
            inp.Name,
            inp.Description,
            inp.AverageRating.Value,
            Convert2Response(inp.Sections),
            inp.HostId.ToString(),
            Convert(inp.DinnerIds),
            Convert(inp.MenuReviewIds),
            inp.CreatedDateTime,
            inp.UpdatedDateTime
        );
    }

    public static CreateMenuRequest CreateRequest(List<MenuSection>? sections = null)
    {
        return new CreateMenuRequest(
            Constants.Menu.Name,
            Constants.Menu.Description,
            sections ?? CreateSections()
        );
    }

    public static List<MenuSection> CreateSections(int sectionCount = 1, List<MenuItem>? items = null)
    {
        return Enumerable.Range(0, sectionCount).Select(
            index => new MenuSection(
                Constants.Menu.SectionNameFromIndex(index),
                Constants.Menu.SectionDescriptionFromIndex(index),
                items ?? CreateMenuItems()
            )).ToList();
    }

    private static List<Domain.Menus.Entites.MenuSection> Convert2Entities(List<MenuSection> sections)
    {
        IEnumerable<int> range = Enumerable.Range(0, sections.Count);
        IEnumerable<Domain.Menus.Entites.MenuSection> items = range.Select(
            index => Domain.Menus.Entites.MenuSection.Create(
                sections[index].Name,
                sections[index].Description,
                Convert2Entities(sections[index].Items))
        );
        return items.ToList();
    }

    private static List<Domain.Menus.Entites.MenuItem> Convert2Entities(List<MenuItem> items)
    {
        IEnumerable<int> range = Enumerable.Range(0, items.Count);
        IEnumerable<Domain.Menus.Entites.MenuItem> menuItems = range.Select(
            index => Domain.Menus.Entites.MenuItem.Create(items[index].Name, items[index].Description)
        );

        return menuItems.ToList();
    }
}
