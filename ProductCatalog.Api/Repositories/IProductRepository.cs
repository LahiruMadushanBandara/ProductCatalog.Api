using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task InsertAsync(Product product, CancellationToken cancellationToken = default);
    }
}
