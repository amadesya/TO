using Microsoft.EntityFrameworkCore;
using static System.Console;

namespace BackendApp
{
    public class WarehouseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "warehouse.db");

            string connection = $"Filename={path}";

            ConsoleColor previousColor = ForegroundColor;
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine($"Connection: {connection}");
            ForegroundColor = previousColor;

            optionsBuilder.UseSqlite(connection);
        }
    }
}
