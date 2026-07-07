using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
    }
}
