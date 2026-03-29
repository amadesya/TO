using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApp
{
    [Table("inventory")]
    public class Inventory
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public virtual Product? Product { get; set; }
        public int warehouse_id { get; set; }
        public virtual Warehouse? Warehouse { get; set; }
        public int quantity { get; set; }
    }
}
