using BackendApp.AutoGenModels;
using BackendApp.DTO;
using BackendApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> repo;

        public ProductController(IRepository<Product> repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public async Task<IEnumerable<Product>> GetProducts(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await repo.RetrieveAllAsync();
            }
            else
            {
                return (await repo.RetrieveAllAsync())
                .Where(product => product.Name != null && product.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
        }

        [HttpGet("{id}", Name = nameof(GetProduct))]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            Product? product = await repo.RetrieveAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Product>> Create(ProductDTO productDto)
        {
            if (productDto == null) return BadRequest();

            var product = new Product
            {
                Name = productDto.Name,
                CategoryId = productDto.CategoryId,
                CostPrice = productDto.CostPrice
            };

            Product? addedProduct = await repo.CreateAsync(product);

            if (addedProduct == null)
            {
                return BadRequest("Failed to create product.");
            }

            return CreatedAtRoute(
                routeName: nameof(GetProduct),
                routeValues: new { id = addedProduct.Id },
                value: addedProduct);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Product>> Update(int id, ProductDTO productDto)
        {
            if (productDto == null) return BadRequest();

            Product? existing = await repo.RetrieveAsync(id);
            if (existing == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(productDto.Name))
            {
                existing.Name = productDto.Name;
            }

            if (productDto.CategoryId.HasValue)
            {
                existing.CategoryId = productDto.CategoryId.Value;
            }

            if (productDto.CostPrice.HasValue)
            {
                existing.CostPrice = productDto.CostPrice.Value;
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
            Product? existing = await repo.RetrieveAsync(id);
            if (existing == null) return NotFound();

            bool? deleted = await repo.DeleteAsync(id);
            if (deleted == true)
            {
                return Ok($"Product {id} was deleted");
            }

            return BadRequest($"Product {id} was found but failed to delete.");
        }
    }
}