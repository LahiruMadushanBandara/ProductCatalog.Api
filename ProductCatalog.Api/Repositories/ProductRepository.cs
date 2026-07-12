using Microsoft.Data.SqlClient;
using ProductCatalog.Api.Models;
using System.Text.Json;
using System.Data;

namespace ProductCatalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT Id, Title, Category, Brand, Price, Stock
                             FROM dbo.Products ORDER BY Id;";

            var results = new List<Product>();
            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(sql, conn);

            await conn.OpenAsync(cancellationToken);

            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                results.Add(Map(reader));
            }
            return results;
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = @"SELECT Id, Title, Category, Brand, Price, Stock
                             FROM dbo.Products WHERE Id = @Id;";

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await conn.OpenAsync(cancellationToken);

            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            return await reader.ReadAsync(cancellationToken) ? Map(reader) : null;
        }

        public async Task InsertAsync(Product product, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                IF NOT EXISTS 
                    (SELECT 1 FROM dbo.Products WHERE Id = @Id)
                        INSERT INTO dbo.Products (Id, Title, Category, Brand, Price, Stock)
                        VALUES (@Id, @Title, @Category, @Brand, @Price, @Stock);";

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = product.Id;
            cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = product.Title;
            cmd.Parameters.Add("@Category", SqlDbType.VarChar).Value = (object?)product.Category ?? DBNull.Value;
            cmd.Parameters.Add("@Brand", SqlDbType.VarChar).Value = (object?)product.Brand ?? DBNull.Value;
            cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = product.Price;
            cmd.Parameters.Add("@Stock", SqlDbType.Int).Value = product.Stock;

            await conn.OpenAsync(cancellationToken);
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }


        public async Task InsertManyAsync(IEnumerable<Product> products)
        {
            const string sql = @"
            INSERT INTO dbo.Products (Id, Title, Category, Brand, Price, Stock)
            SELECT j.Id, j.Title, j.Category, j.Brand, j.Price, j.Stock
            FROM OPENJSON(@Json)
            WITH (
                Id       INT            '$.Id',
                Title    NVARCHAR(300)  '$.Title',
                Category NVARCHAR(100)  '$.Category',
                Brand    NVARCHAR(150)  '$.Brand',
                Price    DECIMAL(10,2)  '$.Price',
                Stock    INT            '$.Stock'
            ) AS j
            WHERE NOT EXISTS (SELECT 1 FROM dbo.Products p WHERE p.Id = j.Id);";

            var json = JsonSerializer.Serialize(products);

            await using var conn = new SqlConnection(_connectionString);
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Json", SqlDbType.NVarChar).Value = json;
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        private static Product Map(SqlDataReader r)
        {
            var id = r.GetOrdinal("Id");
            var title = r.GetOrdinal("Title");
            var category = r.GetOrdinal("Category");
            var brand = r.GetOrdinal("Brand");
            var price = r.GetOrdinal("Price");
            var stock = r.GetOrdinal("Stock");

            return new Product
            {
                Id = r.GetInt32(id),
                Title = r.GetString(title),
                Category = r.IsDBNull(category) ? null : r.GetString(category),
                Brand = r.IsDBNull(brand) ? null : r.GetString(brand),
                Price = r.GetDecimal(price),
                Stock = r.GetInt32(stock)
            };
        }
    }
}
