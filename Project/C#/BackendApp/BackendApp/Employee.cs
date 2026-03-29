using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApp
{
    [Table("employees")]
    public class Employee
    {
        public int id { get; set; }
        [Required]
        [StringLength(150)]
        public string? full_name { get; set; }
        public string? email { get; set; }
        public string? password_hash { get; set; }
        public bool? is_active { get; set; } = true;
        public DateTime? created_at { get; set; } = DateTime.Now;

        public int role_id { get; set; }
        public virtual Role? Role { get; set; } = null!;

    }
}
