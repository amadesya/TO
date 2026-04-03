namespace BackendApp.DTO
{
    public class TransactionCreateDTO
    {
        public int ProductId { get; set; }
        public int? FromWarehouseId { get; set; }
        public int? ToWarehouseId { get; set; }
        public int? EmployeeId { get; set; } 
        public int Quantity { get; set; }
        public string TransactionType { get; set; } = null!;
    }
}
