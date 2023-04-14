using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicio.api.Auth.Core.Entities;

namespace Servicio.api.Auth.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _rolemanager;


    public RoleController(RoleManager<IdentityRole> rolemanager)
    {
        _rolemanager = rolemanager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> GetRoles()
    {
        var roles = await _rolemanager.Roles.ToListAsync();

        return roles.Select(r => r.Name).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<string>> GetRole(string id)
    {
        var role = await _rolemanager.FindByIdAsync(id);

        if (role == null)
        {
            return NotFound();
        }

        return role.Name;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleEntity newRole)
    {
        var role = new IdentityRole(newRole.RoleName);
        var result = await _rolemanager.CreateAsync(role);

        if (result.Succeeded)
        {

            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, newRole.RoleName);
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, RoleEntity newData)
    {
        var role = await _rolemanager.FindByIdAsync(id);

        if (role == null)
        {
            return NotFound();
        }

        role.Name = newData.RoleName;
        var result = await _rolemanager.UpdateAsync(role);

        if (result.Succeeded)
        {
            return NoContent();
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _rolemanager.FindByIdAsync(id);

        if (role == null)
        {
            return NotFound();
        }

        var result = await _rolemanager.DeleteAsync(role);

        if (result.Succeeded)
        {
            return NoContent();
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }


}