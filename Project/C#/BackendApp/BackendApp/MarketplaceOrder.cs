using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApp
{
    [Table("marketplace_orders")]
    public class MarketplaceOrder
    {
        public int id { get; set; }
        public string? order_number { get; set; }
        public string? marketplace { get; set; }
        public string? status { get; set; } = "new";
        public DateTime order_date { get; set; } = DateTime.Now;
    }
}
