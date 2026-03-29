using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApp
{
    [Table("Products")]
    public class Product
    {
        public int id { get; set; }
        public string? sku { get; set; }
        public string? barcode { get; set; }
        public string? name { get; set; } = null!;
        [Column(TypeName = "decimal(10,2)")]
        public double? cost_price { get; set; }
        public int weight_grams { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
        
        public int category_id { get; set; }
        public virtual Category? Category { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; } 
        
        public Product()
        {
            Inventories = new HashSet<Inventory>();
        }   
    }
}
