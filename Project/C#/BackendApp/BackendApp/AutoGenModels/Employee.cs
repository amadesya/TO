using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.AutoGenModels;

[Table("employees")]
[Index("Email", IsUnique = true)]
public partial class Employee
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("role_id")]
    public int? RoleId { get; set; }

    [Column("full_name", TypeName = "VARCHAR(150)")]
    public string FullName { get; set; } = null!;

    [Column("email", TypeName = "VARCHAR(100)")]
    public string Email { get; set; } = null!;

    [Column("password_hash", TypeName = "VARCHAR(255)")]
    public string PasswordHash { get; set; } = null!;

    [Column("is_active", TypeName = "BOOLEAN")]
    public bool? IsActive { get; set; }

    [Column("created_at", TypeName = "TIMESTAMP")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Employees")]
    public virtual Role? Role { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
