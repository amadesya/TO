using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.AutoGenModels;

[Table("marketplace_orders")]
[Index("OrderNumber", IsUnique = true)]
public partial class MarketplaceOrder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_number", TypeName = "VARCHAR(100)")]
    public string OrderNumber { get; set; } = null!;

    [Column("marketplace", TypeName = "VARCHAR(50)")]
    public string Marketplace { get; set; } = null!;

    [Column("status", TypeName = "VARCHAR(50)")]
    public string? Status { get; set; }

    [Column("order_date", TypeName = "TIMESTAMP")]
    public DateTime? OrderDate { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
