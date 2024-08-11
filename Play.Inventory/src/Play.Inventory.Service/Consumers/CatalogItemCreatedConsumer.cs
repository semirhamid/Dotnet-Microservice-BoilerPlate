using MassTransit;
using Play.Catalog.Contracts;
using Play.Catalog.Service.Repository;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
  public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
  {
    private readonly IRepository<CatalogItem> repository;

    public CatalogItemCreatedConsumer(IRepository<CatalogItem> repository)
    {
      this.repository = repository;
    }

    public async Task Consume(ConsumeContext<CatalogItemCreated> context)
    {
      var message = context.Message;
      var existingItem = await repository.GetAsync(i => i.Name == message.name);
      if (existingItem != null)
      {
        return;
      }
      var item = new CatalogItem
      {
        Id = message.ItemId,
        Name = message.name,
        Description = message.description
      };

      await repository.CreateAsync(item);
    }
  }
}