using Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Client
{
  public class CatalogClient
  {
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    public CatalogClient(HttpClient httpClient, IConfiguration configuration)
    {
      _httpClient = httpClient;
      _baseUrl = configuration["CatalogBaseUrl"];
    }
    public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemAsync()
    {
      Console.WriteLine(_baseUrl);
      var items = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>($"{_baseUrl}/api/items");
      return items;
    }
  }
}