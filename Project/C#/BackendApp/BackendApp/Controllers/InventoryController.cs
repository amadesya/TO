using BackendApp.AutoGenModels;
using BackendApp.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public InventoryController(WarehouseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryDTO>>> GetInventory()
        {
            var inventory = await _context.Inventories
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .Select(i => new InventoryDTO
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    WarehouseId = i.WarehouseId,
                    WarehouseName = i.Warehouse != null ? i.Warehouse.Name : "-",
                    Quantity = i.Quantity,
                    ReservedQuantity = i.ReservedQuantity
                })
                .ToListAsync();

            return Ok(inventory);
        }

        [HttpGet("by-product/{productId}")]
        public async Task<ActionResult<IEnumerable<InventoryDTO>>> GetInventoryByProduct(int productId)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .Where(i => i.ProductId == productId)
                .Select(i => new InventoryDTO
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    WarehouseId = i.WarehouseId,
                    WarehouseName = i.Warehouse != null ? i.Warehouse.Name : "-",
                    Quantity = i.Quantity,
                    ReservedQuantity = i.ReservedQuantity
                })
                .ToListAsync();

            if (!inventory.Any()) return NotFound("Товар не найден на складах");

            return Ok(inventory);
        }

        [HttpGet("by-warehouse/{warehouseId}")]
        public async Task<ActionResult<IEnumerable<InventoryDTO>>> GetInventoryByWarehouse(int warehouseId)
        {
            var inventory = await _context.Inventories
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .Where(i => i.WarehouseId == warehouseId)
                .Select(i => new InventoryDTO
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    WarehouseId = i.WarehouseId,
                    WarehouseName = i.Warehouse != null ? i.Warehouse.Name : "-",
                    Quantity = i.Quantity,
                    ReservedQuantity = i.ReservedQuantity
                })
                .ToListAsync();

            if (!inventory.Any()) return NotFound("Склад пуст или не найден");

            return Ok(inventory);
        }
    }
}