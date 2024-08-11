namespace Play.Inventory.Service.Dtos
{
  public record GrantItemDto(Guid Id, Guid CatalogItemId, int Quantity);
  public record InventoryItemDto(Guid CatalogItemId, string name, string description, int Quantity, DateTimeOffset AcquiredDate);
  public record CatalogItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);
}