using BackendApp;
using static System.Console;

WarehouseContext db = new();
WriteLine($"Provider: {db.Database.ProviderName}");