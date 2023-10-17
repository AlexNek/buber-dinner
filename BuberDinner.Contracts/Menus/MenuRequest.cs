namespace BuberDinner.Contracts.Menus;

public record MenuRequest(
    string Name,
    string Description,
    List<MenuSectionRequest> Sections);