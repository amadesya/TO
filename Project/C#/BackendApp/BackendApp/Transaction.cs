using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApp
{
    [Table("transactions")]
    public class Transaction
    {
        public int id { get; set; }
        public int product_id { get; set; }
    }
}
