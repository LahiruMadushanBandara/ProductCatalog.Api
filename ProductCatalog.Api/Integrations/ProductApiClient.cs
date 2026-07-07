using ProductCatalog.Api.Integrations.Dtos;
using ProductCatalog.Api.Models;
using System.Net;

namespace ProductCatalog.Api.Integrations
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly HttpClient _httpClient;
        private const string ProductsEndpoint = "products?limit=0";
        public ProductApiClient(HttpClient httpClient) => _httpClient = httpClient;


        public async Task<List<Product>> FetchAllAsync(CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync(ProductsEndpoint, cancellationToken);
            response.EnsureSuccessStatusCode();

            var dto = await response.Content.ReadFromJsonAsync<ProductClientListDto>(cancellationToken);

            return dto?.Products?.Select(MapToProduct)
                .ToList() ?? [];
        }



        public async Task<Product?> FetchByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync($"products/{id}", cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var dto = await response.Content.ReadFromJsonAsync<ProductClientDto>(cancellationToken);
            return dto is null ? null : MapToProduct(dto);
        }



        private static Product MapToProduct(ProductClientDto dto) => new()
        {
            Id = dto.Id,
            Title = dto.Title,
            Category = dto.Category,
            Brand = dto.Brand,
            Price = dto.Price,
            Stock = dto.Stock
        };
    }
}
