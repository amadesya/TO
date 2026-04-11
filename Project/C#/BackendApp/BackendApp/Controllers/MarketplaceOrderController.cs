using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendApp.AutoGenModels;
using BackendApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketplaceOrderController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public MarketplaceOrderController(WarehouseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarketplaceOrderDTO>>> GetOrders()
        {
            var orders = await _context.MarketplaceOrders
                .Select(o => new MarketplaceOrderDTO
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    Marketplace = o.Marketplace,
                    Status = o.Status,
                    OrderDate = o.OrderDate
                })
                .ToListAsync();

            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == request.ProductId && i.WarehouseId == request.WarehouseId);

            if (inventory == null)
                return BadRequest("Товар не найден на этом складе.");

            int available = (int)(inventory.Quantity - inventory.ReservedQuantity);
            if (available < request.Quantity)
                return BadRequest($"Недостаточно товара. Доступно: {available}, запрошено: {request.Quantity}");

            var newOrder = new MarketplaceOrder
            {
                OrderNumber = request.OrderNumber,
                Marketplace = request.Marketplace,
                Status = "new", 
                OrderDate = DateTime.Now
            };

            _context.MarketplaceOrders.Add(newOrder);

            inventory.ReservedQuantity += request.Quantity;

            var transaction = new Transaction
            {
                ProductId = request.ProductId,
                FromWarehouseId = request.WarehouseId, 
                EmployeeId = request.EmployeeId,
                Order = newOrder, 
                Quantity = request.Quantity,
                TransactionType = "Резерв",
                TransactionDate = DateTime.Now
            };

            _context.Transactions.Add(transaction);

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Заказ успешно создан и товар зарезервирован!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPatch("{id}/status")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatusUpdateDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Status))
            {
                return BadRequest();
            }

            var existingOrder = await _context.MarketplaceOrders.FindAsync(id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            existingOrder.Status = dto.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}