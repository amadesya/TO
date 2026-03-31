using BackendApp.AutoGenModels;
using BackendApp.DTO;
using BackendApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository repo;

        public RoleController(IRoleRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public async Task<IEnumerable<Role>> GetRoles(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await repo.RetrieveAllAsync();
            }
            else
            {
                return (await repo.RetrieveAllAsync())
                .Where(role => role.Name == name);
            }
        }

        [HttpGet("{id}", Name = nameof(GetRole))]
        [ProducesResponseType(200, Type = typeof(Role))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            Role? role = await repo.RetrieveAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Role))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Role>> Create(RoleDTO roleDto)
        {
            if (roleDto == null)
            {
                return BadRequest();
            }
            var role = new Role
            {
                Name = roleDto.Name,
                Description = roleDto.Description
            };

            Role? addedRole = await repo.CreateAsync(role);

            if (addedRole == null)
            {
                return BadRequest("Failed to create role.");
            }
            else
            {
                return CreatedAtRoute(
                    routeName: nameof(GetRole),
                    routeValues: new
                    {
                        id = addedRole.Id
                    },
                    value: addedRole);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Role>> Update(
    int id, RoleDTO roleDto)
        {
            if (roleDto == null)
            {
                return BadRequest();
            }
            Role? existing = await repo.RetrieveAsync(id);
            if (existing == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(roleDto.Name))
            {
                existing.Name = roleDto.Name;
            }
            if (roleDto.Description != null)
            {
                existing.Description = roleDto.Description;
            }
            existing.Description = roleDto.Description;
            await repo.UpdateAsync(id, existing);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            Role? existing = await repo.RetrieveAsync(id);
            if (existing == null)
            {
                return NotFound();
            }
            bool? deleted = await repo.DeleteAsync(id);
            if (deleted.HasValue && deleted.Value)
            {
                return Ok(
                    $"Role {id} was deleted");
            }
            else
            {
                return BadRequest(
                    $"Role {id} was found but failed to delete.");
            }
        }
    }
}
