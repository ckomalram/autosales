using Microsoft.AspNetCore.Mvc;
using Servicio.api.Cliente.Core.Entity;
using Servicio.api.Cliente.Core.Repository;
using Servicio.api.Cliente.Helper;

namespace Servicio.api.Cliente.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IMongoRepository<CustomerEntity> _repository;

    public CustomerController(IMongoRepository<CustomerEntity> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerEntity>>> Get()
    {
        try
        {
            var rta = await _repository.GetAll();
            return Ok(rta);
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerEntity>> GetById(string Id)
    {
        try
        {
            var rta = await _repository.GetById(Id);
            if (rta == null)
            {
                return NotFound();
            }
            return Ok(rta);
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerEntity parameters)
    {
        try
        {
            await _repository.InsertDocument(parameters);
            return Ok();
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost("pagination")]
    public async Task<ActionResult<PaginationEntity<CustomerEntity>>> PostPagination(PaginationEntity<CustomerEntity> pagination)
    {

        try
        {
            var rta = await _repository.PaginationByFilter(
            pagination
            );

            return Ok(rta);
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }



    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string Id, CustomerEntity parameters)
    {
        if (Id != parameters.Id)
        {
            return BadRequest("httput --> Id does not match.");
        }

        try
        {
            parameters.Id = Id;
            await _repository.UpdateDocument(parameters);

            return Ok();
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string Id)
    {

        try
        {
            var rta = await _repository.GetById(Id);
            if (rta == null)
            {
                return NotFound();
            }
            await _repository.DeleteDocument(Id);
            return Ok();
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }

    }


}