using MassTransit;
using Play.Catalog.Contracts;
using Play.Catalog.Service.Repository;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
  public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
  {
    private readonly IRepository<CatalogItem> repository;

    public CatalogItemDeletedConsumer(IRepository<CatalogItem> repository)
    {
      this.repository = repository;
    }

    public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
    {
      var message = context.Message;
      var existingItem = await repository.GetAsync(i => i.Id == message.ItemId);
      if (existingItem == null)
      {
        return;
      }
      await repository.DeleteAsync(existingItem.Id);
    }
  }
}