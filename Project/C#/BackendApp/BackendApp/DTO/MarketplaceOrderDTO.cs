using System;

namespace BackendApp.DTOs
{
    public class MarketplaceOrderDTO
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = null!;

        public string Marketplace { get; set; } = null!;

        public string? Status { get; set; }

        public DateTime? OrderDate { get; set; }
    }
}