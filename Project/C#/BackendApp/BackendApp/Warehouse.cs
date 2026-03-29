using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApp
{
    [Table("warehouses")]
    public class Warehouse
    {
        public int id { get; set; }
        public string? name { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? address { get; set; }
        public string? warehouse_type { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public Warehouse()
        {
            Inventories = new HashSet<Inventory>();
        }
    }
}
