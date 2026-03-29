using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApp
{
    [Table("categories")]
    public class Category
    {
        public int id { get; set; }
        public string? name { get; set; }
        [Column(TypeName = "text")]
        public string? description { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public Category(){
            Products = new HashSet<Product>();
        }
    }
}
