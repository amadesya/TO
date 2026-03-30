using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.AutoGenModels;

[Table("warehouses")]
public partial class Warehouse
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name", TypeName = "VARCHAR(100)")]
    public string Name { get; set; } = null!;

    [Column("address")]
    public string? Address { get; set; }

    [Column("warehouse_type", TypeName = "VARCHAR(50)")]
    public string? WarehouseType { get; set; }

    [InverseProperty("Warehouse")]
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [InverseProperty("FromWarehouse")]
    public virtual ICollection<Transaction> TransactionFromWarehouses { get; set; } = new List<Transaction>();

    [InverseProperty("ToWarehouse")]
    public virtual ICollection<Transaction> TransactionToWarehouses { get; set; } = new List<Transaction>();
}
