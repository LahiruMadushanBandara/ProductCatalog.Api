using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Integrations
{
    public interface IProductApiClient
    {
        Task<List<Product>> FetchAllAsync();
        Task<Product?> FetchByIdAsync(int id);
    }
}
