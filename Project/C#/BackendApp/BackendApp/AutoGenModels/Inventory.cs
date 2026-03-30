using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.AutoGenModels;

[Table("inventory")]
[Index("ProductId", "WarehouseId", IsUnique = true)]
public partial class Inventory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [Column("warehouse_id")]
    public int? WarehouseId { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("reserved_quantity")]
    public int? ReservedQuantity { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Inventories")]
    public virtual Product? Product { get; set; }

    [ForeignKey("WarehouseId")]
    [InverseProperty("Inventories")]
    public virtual Warehouse? Warehouse { get; set; }
}
