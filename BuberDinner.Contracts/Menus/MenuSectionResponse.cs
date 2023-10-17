namespace BuberDinner.Contracts.Menus;

public record MenuSectionResponse(
    string Id,
    string Name,
    string Description,
    List<MenuItemResponse> Items);