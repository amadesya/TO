using BackendApp.AutoGenModels;
using BackendApp.DTO;
using BackendApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<Category> repo;

        public CategoryController(IRepository<Category> repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public async Task<IEnumerable<Category>> GetCategories(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await repo.RetrieveAllAsync();
            }
            else
            {
                return (await repo.RetrieveAllAsync())
                .Where(category => category.Name == name);
            }
        }

        [HttpGet("{id}", Name = nameof(GetCategory))]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            Category? category = await repo.RetrieveAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Category>> Create(CategoryDTO categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest();
            }

            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            Category? addedCategory = await repo.CreateAsync(category);

            if (addedCategory == null)
            {
                return BadRequest("Failed to create category.");
            }
            else
            {
                return CreatedAtRoute(
                    routeName: nameof(GetCategory),
                    routeValues: new
                    {
                        id = addedCategory.Id
                    },
                    value: addedCategory);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Category>> Update(int id, CategoryDTO categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest();
            }

            Category? existing = await repo.RetrieveAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                existing.Name = categoryDto.Name;
            }
            if (categoryDto.Description != null)
            {
                existing.Description = categoryDto.Description;
            }

            await repo.UpdateAsync(id, existing);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            Category? existing = await repo.RetrieveAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            bool? deleted = await repo.DeleteAsync(id);
            if (deleted.HasValue && deleted.Value)
            {
                return Ok($"Category {id} was deleted");
            }
            else
            {
                return BadRequest($"Category {id} was found but failed to delete.");
            }
        }
    }
}