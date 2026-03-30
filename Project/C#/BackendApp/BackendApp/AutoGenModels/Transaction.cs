using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.AutoGenModels;

[Table("transactions")]
public partial class Transaction
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [Column("from_warehouse_id")]
    public int? FromWarehouseId { get; set; }

    [Column("to_warehouse_id")]
    public int? ToWarehouseId { get; set; }

    [Column("employee_id")]
    public int? EmployeeId { get; set; }

    [Column("order_id")]
    public int? OrderId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("transaction_type", TypeName = "VARCHAR(50)")]
    public string TransactionType { get; set; } = null!;

    [Column("transaction_date", TypeName = "TIMESTAMP")]
    public DateTime? TransactionDate { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("Transactions")]
    public virtual Employee? Employee { get; set; }

    [ForeignKey("FromWarehouseId")]
    [InverseProperty("TransactionFromWarehouses")]
    public virtual Warehouse? FromWarehouse { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Transactions")]
    public virtual MarketplaceOrder? Order { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Transactions")]
    public virtual Product? Product { get; set; }

    [ForeignKey("ToWarehouseId")]
    [InverseProperty("TransactionToWarehouses")]
    public virtual Warehouse? ToWarehouse { get; set; }
}
