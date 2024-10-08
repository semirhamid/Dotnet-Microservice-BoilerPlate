using Play.Common.Entities;

namespace Play.Inventory.Service.Entities
{
  public class CatalogItem : IEntity
  {

    public Guid Id { get; init; }

    public string Name { get; set; }

    public string Description { get; set; }

  }
}