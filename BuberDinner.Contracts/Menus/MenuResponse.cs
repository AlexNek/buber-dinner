namespace BuberDinner.Contracts.Menus;

public record MenuResponse(
    string Id,
    string Name,
    string Description,
    double? AverageRating,
    List<MenuSectionResponse> Sections,
    string HostId,
    List<string> DinnerIds,
    List<string> MenuReviewIds,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);