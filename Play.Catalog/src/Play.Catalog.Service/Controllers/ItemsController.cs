using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repository;
using Play.Catalog.Contracts;
namespace Play.Catalog.Service.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ItemsController : ControllerBase
  {
    private readonly IRepository<CatalogItem> _repository;
    private readonly IPublishEndpoint _publishEndpoint;

    public ItemsController(IRepository<CatalogItem> repository, IPublishEndpoint publishEndpoint)
    {
      _repository = repository;
      _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems()
    {
      var items = await _repository.GetAllAsync();
      return Ok(ItemConverter.ConvertToDtoList(items));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItemById(Guid id)
    {
      var item = await _repository.GetAsync(id);
      if (item == null)
      {
        return NotFound();
      }
      return Ok(ItemConverter.ConvertToDto(item));
    }

    [HttpPost]
    public async Task<ActionResult<CatalogItem>> CreateItem(CreateItemDto item)
    {
      var newItem = ItemConverter.ConvertCreateItemToItem(item);
      await _repository.CreateAsync(newItem);
      await _publishEndpoint.Publish(new CatalogItemCreated(newItem.Id, newItem.Name, newItem.Description));
      return CreatedAtAction(nameof(GetItemById), new { id = newItem.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(Guid id, UpdateItemDto item)
    {
      var existingItem = await _repository.GetAsync(id);
      if (existingItem == null)
      {
        return NotFound();
      }
      var newItem = ItemConverter.ConvertUpdateItemToItem(id, item);
      await _repository.UpdateAsync(newItem);
      await _publishEndpoint.Publish(new CatalogItemUpdated(newItem.Id, newItem.Name, newItem.Description));
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
      var existingItem = await _repository.GetAsync(id);
      if (existingItem == null)
      {
        return NotFound();
      }
      await _repository.DeleteAsync(id);
      await _publishEndpoint.Publish(new CatalogItemDeleted(id));
      return NoContent();
    }
  }
}