using Play.Common.Entities;

namespace Play.Catalog.Service.Entities
{
  public class CatalogItem : IEntity
  {

    public Guid Id { get; init; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

  }
}