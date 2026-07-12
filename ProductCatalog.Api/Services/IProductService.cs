using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync(CancellationToken cancellationToken);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}
