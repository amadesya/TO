using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApp.DTO
{
    public class CategoryDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
