using System;

namespace BackendApp.DTOs
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? FromWarehouseName { get; set; }
        public string? ToWarehouseName { get; set; }
        public string? EmployeeName { get; set; }
        public int Quantity { get; set; }
        public string TransactionType { get; set; } = null!;
        public DateTime? TransactionDate { get; set; }
    }
}