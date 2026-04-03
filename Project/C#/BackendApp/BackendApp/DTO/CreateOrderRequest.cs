namespace BackendApp.DTOs
{
    public class CreateOrderRequest
    {
        public string OrderNumber { get; set; } = null!;
        public string Marketplace { get; set; } = null!;

        public int ProductId { get; set; }
        public int WarehouseId { get; set; } 
        public int EmployeeId { get; set; }  
        public int Quantity { get; set; }    
    }
}