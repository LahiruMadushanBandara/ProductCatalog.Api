using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task InsertAsync(Product product);
    }
}
