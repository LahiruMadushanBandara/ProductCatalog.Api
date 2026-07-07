using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Integrations
{
    public interface IProductApiClient
    {
        Task<List<Product>> FetchAllAsync(CancellationToken cancellationToken = default);
        Task<Product?> FetchByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
