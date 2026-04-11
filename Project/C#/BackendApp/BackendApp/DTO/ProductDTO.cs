namespace BackendApp.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public double? CostPrice { get; set; }
    }
}