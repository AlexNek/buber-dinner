namespace BuberDinner.Contracts.Menus;

public record MenuSectionRequest(
    string Name,
    string Description,
    List<MenuItemRequest> Items);