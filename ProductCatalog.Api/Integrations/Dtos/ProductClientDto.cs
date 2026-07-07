namespace ProductCatalog.Api.Integrations.Dtos
{
    public class ProductClientDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }


    public class ProductClientListDto
    {
        public List<ProductClientDto> Products { get; set; } = new();
    }
}
