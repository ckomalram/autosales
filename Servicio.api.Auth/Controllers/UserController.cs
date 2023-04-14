using MediatR;
using Microsoft.AspNetCore.Mvc;
using Servicio.api.Auth.Core.Application;
using Servicio.api.Auth.Core.Dto;

namespace Servicio.api.Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetUserSession()
    {
        return await _mediator.Send(new UserCurrent.UserCurrentCommand());
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(Register.UserRegisterCommand parametros)
    {
        return await _mediator.Send(parametros);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(Login.UserLoginCommand parametros)
    {
        return await _mediator.Send(parametros);
    }


}