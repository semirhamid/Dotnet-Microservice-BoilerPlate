using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Repository;
using Play.Inventory.Service.Client;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class InventoryController(IRepository<InventoryItem> inventoryItemsRepository, IRepository<CatalogItem> catalogItemsRepository) : ControllerBase
{


  [HttpGet("{userId}")]
  public async Task<ActionResult<IEnumerable<InventoryItemDto>>> Get(Guid userId)
  {
    if (userId == Guid.Empty)
    {
      return Array.Empty<InventoryItemDto>();
    }
    var inventoryItems = await inventoryItemsRepository.GetAllAsync(item => item.UserId == userId);
    var inventoryCatalogItemIds = inventoryItems.Select(item => item.CatalogItemId);
    var catalogItems = await catalogItemsRepository.GetAllAsync(item => inventoryCatalogItemIds.Contains(item.Id));
    Console.WriteLine("Inventory Items: " + inventoryItems.Count());
    Console.WriteLine("Catalog Items: " + catalogItems.Count());
    var items = inventoryItems.Select(inventory =>
    {
      var catalogItem = catalogItems.Single(catalog => catalog.Id == inventory.CatalogItemId);
      return inventory.AsDto(catalogItem?.Name, catalogItem?.Description);
    });
    return Ok(items);
  }

  [HttpPost]
  public async Task<ActionResult> Post(GrantItemDto grantItemDto)
  {
    var item = await inventoryItemsRepository.GetAsync(i => i.UserId == grantItemDto.Id && i.CatalogItemId == grantItemDto.CatalogItemId);
    if (item is null)
    {
      item = new InventoryItem
      {
        UserId = grantItemDto.Id,
        CatalogItemId = grantItemDto.CatalogItemId,
        Quantity = grantItemDto.Quantity,
        AcquiredDate = DateTimeOffset.UtcNow
      };
      await inventoryItemsRepository.CreateAsync(item);
    }
    else
    {
      item.Quantity += grantItemDto.Quantity;
      await inventoryItemsRepository.UpdateAsync(item);
    }

    Console.WriteLine("Item added to inventory: " + item.Id);
    return Ok();
  }

  [HttpPut]
  public async Task<ActionResult> Put(GrantItemDto grantItemDto)
  {
    var item = await inventoryItemsRepository.GetAsync(i => i.UserId == grantItemDto.Id && i.CatalogItemId == grantItemDto.CatalogItemId);
    if (item is null)
    {
      return NotFound();
    }

    item.Quantity += grantItemDto.Quantity;
    await inventoryItemsRepository.UpdateAsync(item);
    return Ok();
  }

  [HttpDelete("{userId}")]
  public async Task<ActionResult> Delete(Guid userId)
  {
    var items = await inventoryItemsRepository.GetAllAsync(item => item.UserId == userId);
    foreach (var item in items)
    {
      await inventoryItemsRepository.DeleteAsync(item.Id);
    }

    return Ok();
  }

  [HttpDelete("{userId}/{itemId}")]
  public async Task<ActionResult> Delete(Guid userId, Guid itemId)
  {
    var item = await inventoryItemsRepository.GetAsync(i => i.UserId == userId && i.CatalogItemId == itemId);
    if (item is null)
    {
      return NotFound();
    }

    await inventoryItemsRepository.DeleteAsync(item.Id);
    return Ok();
  }
}