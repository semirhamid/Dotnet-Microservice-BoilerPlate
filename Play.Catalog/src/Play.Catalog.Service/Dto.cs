namespace Play.Catalog.Service
{
  public record ItemDto
  {
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
  }
  public record CreateItemDto
  {
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
  }
  public record UpdateItemDto
  {
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
  }
}