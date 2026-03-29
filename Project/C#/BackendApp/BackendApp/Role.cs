using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApp
{
    [Table("roles")]
    public class Role
    {
        public int id { get; set; }
        public string? name { get; set; }
        [Column(TypeName = "text")]
        public string? description { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public Role()
        {
            Employees = new HashSet<Employee>();
        }
    }
}
