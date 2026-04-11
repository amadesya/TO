using BackendApp.AutoGenModels;
using BackendApp.DTO;
using BackendApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public TransactionController(WarehouseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetTransactions()
        {
            var transactions = await _context.Transactions
                .Include(t => t.Product)
                .Include(t => t.FromWarehouse)
                .Include(t => t.ToWarehouse)
                .Include(t => t.Employee)
                .Select(t => new TransactionDTO
                {
                    Id = t.Id,
                    ProductName = t.Product != null ? t.Product.Name : "Неизвестный товар",
                    FromWarehouseName = t.FromWarehouse != null ? t.FromWarehouse.Name : "-",
                    ToWarehouseName = t.ToWarehouse != null ? t.ToWarehouse.Name : "-",
                    EmployeeName = t.Employee != null ? t.Employee.FullName : "Система",
                    Quantity = t.Quantity,
                    TransactionType = t.TransactionType,
                    TransactionDate = t.TransactionDate
                })
                .ToListAsync();

            return Ok(transactions);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Transaction))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Transaction>> Create(TransactionCreateDTO dto)
        {
            if (dto == null) return BadRequest();

            var transaction = new Transaction
            {
                ProductId = dto.ProductId,
                FromWarehouseId = dto.FromWarehouseId,
                ToWarehouseId = dto.ToWarehouseId,
                Quantity = dto.Quantity,
                TransactionType = dto.TransactionType,
                TransactionDate = DateTime.UtcNow
            };

            await _context.Transactions.AddAsync(transaction);

            if (dto.ToWarehouseId.HasValue)
            {
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.ProductId == dto.ProductId && i.WarehouseId == dto.ToWarehouseId);

                if (inventory == null)
                {
                    inventory = new Inventory
                    {
                        ProductId = dto.ProductId,
                        WarehouseId = dto.ToWarehouseId.Value, // Не забудь .Value
                        Quantity = dto.Quantity,
                        ReservedQuantity = 0
                    };
                    await _context.Inventories.AddAsync(inventory);
                }
                else
                {
                    // Увеличиваем количество
                    inventory.Quantity += dto.Quantity;

                    // ЯВНО ГОВОРИМ БАЗЕ ОБНОВИТЬ ЭТУ ЗАПИСЬ (добавь эту строчку!)
                    _context.Inventories.Update(inventory);
                }
            }

            if (dto.FromWarehouseId.HasValue)
            {
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(i => i.ProductId == dto.ProductId && i.WarehouseId == dto.FromWarehouseId);

                if (inventory != null)
                {
                    inventory.Quantity -= dto.Quantity;
                }
            }

            int affected = await _context.SaveChangesAsync();

            if (affected == 0)
            {
                return BadRequest();
            }

            return Created("", transaction);
        }
    }
}