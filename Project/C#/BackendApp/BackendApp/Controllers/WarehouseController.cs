using BackendApp.AutoGenModels;
using BackendApp.DTO;
using BackendApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly IRepository<Warehouse> repo;

        public WarehouseController(IRepository<Warehouse> repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Warehouse>))]
        public async Task<IEnumerable<Warehouse>> GetWarehouses(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await repo.RetrieveAllAsync();
            }
            else
            {
                return (await repo.RetrieveAllAsync())
                .Where(w => w.Name != null && w.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
        }

        [HttpGet("{id}", Name = nameof(GetWarehouse))]
        [ProducesResponseType(200, Type = typeof(Warehouse))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Warehouse>> GetWarehouse(int id)
        {
            Warehouse? warehouse = await repo.RetrieveAsync(id);
            if (warehouse == null)
            {
                return NotFound();
            }
            return Ok(warehouse);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Warehouse))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Warehouse>> Create(WarehouseDTO warehouseDto)
        {
            if (warehouseDto == null) return BadRequest();

            if (string.IsNullOrWhiteSpace(warehouseDto.Name))
            {
                return BadRequest("The Name field is required for a warehouse.");
            }

            var warehouse = new Warehouse
            {
                Name = warehouseDto.Name,
                Address = warehouseDto.Address,
                WarehouseType = warehouseDto.WarehouseType
            };

            Warehouse? addedWarehouse = await repo.CreateAsync(warehouse);

            if (addedWarehouse == null)
            {
                return BadRequest("Failed to create warehouse.");
            }

            return CreatedAtRoute(
                routeName: nameof(GetWarehouse),
                routeValues: new { id = addedWarehouse.Id },
                value: addedWarehouse);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Warehouse>> Update(int id, WarehouseDTO warehouseDto)
        {
            if (warehouseDto == null) return BadRequest();

            Warehouse? existing = await repo.RetrieveAsync(id);
            if (existing == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(warehouseDto.Name))
            {
                existing.Name = warehouseDto.Name;
            }
            if (warehouseDto.Address != null)
            {
                existing.Address = warehouseDto.Address;
            }
            if (warehouseDto.WarehouseType != null)
            {
                existing.WarehouseType = warehouseDto.WarehouseType;
            }

            await repo.UpdateAsync(id, existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            Warehouse? existing = await repo.RetrieveAsync(id);
            if (existing == null) return NotFound();

            bool? deleted = await repo.DeleteAsync(id);
            if (deleted == true)
            {
                return Ok($"Warehouse {id} was deleted");
            }

            return BadRequest($"Warehouse {id} was found but failed to delete.");
        }
    }
}