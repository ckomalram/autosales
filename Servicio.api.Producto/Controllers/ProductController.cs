namespace Servicio.api.Producto.Controllers;


using Microsoft.AspNetCore.Mvc;
using Servicio.api.Producto.Core.Entity;
using Servicio.api.Producto.Core.Repository;
using Servicio.api.Producto.Helpers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMongoRepository<ProductEntity> _repository;

    public ProductController(IMongoRepository<ProductEntity> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductEntity>>> Get()
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
    public async Task<ActionResult<ProductEntity>> GetById(string Id)
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
    public async Task<IActionResult> Create(ProductEntity parameters)
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
    public async Task<ActionResult<PaginationEntity<ProductEntity>>> PostPagination(PaginationEntity<ProductEntity> pagination)
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
    public async Task<IActionResult> Update(string Id, ProductEntity parameters)
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