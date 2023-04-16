using Microsoft.AspNetCore.Mvc;
using Servicio.api.Pedido.Core.Entity;
using Servicio.api.Pedido.Core.Repository;
using Servicio.api.Pedido.Helper;

namespace Servicio.api.Pedido.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMongoRepository<OrderEntity> _repository;

    public OrderController(IMongoRepository<OrderEntity> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderEntity>>> Get()
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
    public async Task<ActionResult<OrderEntity>> GetById(string Id)
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
    public async Task<IActionResult> Create(OrderEntity parameters)
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
    public async Task<ActionResult<PaginationEntity<OrderEntity>>> PostPagination(PaginationEntity<OrderEntity> pagination)
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
    public async Task<IActionResult> Update(string Id, OrderEntity parameters)
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