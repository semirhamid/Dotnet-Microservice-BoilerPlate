using Play.Catalog.Service;
using Play.Catalog.Service.Entities;

public static class ItemConverter
{
  public static ItemDto ConvertToDto(CatalogItem item)
  {
    // Perform the conversion logic here
    ItemDto itemDto = new ItemDto
    {
      // Set the properties of the itemDto object based on the item object
      Id = item.Id,
      Name = item.Name,
      Description = item.Description,
      // Add more properties as needed
    };

    return itemDto;
  }
  public static CatalogItem ConvertCreateItemToItem(CreateItemDto item)
  {
    // Perform the conversion logic here
    CatalogItem itemDto = new CatalogItem
    {
      // Set the properties of the itemDto object based on the item object
      Name = item.Name,
      Description = item.Description,
      Price = item.Price
      // Add more properties as needed
    };

    return itemDto;
  }

  public static CatalogItem ConvertUpdateItemToItem(Guid Id, UpdateItemDto item)
  {
    // Perform the conversion logic here
    CatalogItem itemDto = new CatalogItem
    {
      // Set the properties of the itemDto object based on the item object
      Id = Id,
      Name = item.Name,
      Description = item.Description,
      Price = item.Price
      // Add more properties as needed
    };

    return itemDto;
  }

  public static List<ItemDto> ConvertToDtoList(IEnumerable<CatalogItem> items)
  {
    List<ItemDto> itemDtos = new List<ItemDto>();

    foreach (CatalogItem item in items)
    {
      ItemDto itemDto = ConvertToDto(item);
      itemDtos.Add(itemDto);
    }

    return itemDtos;
  }

  // Add more conversion methods for other lists if needed
}