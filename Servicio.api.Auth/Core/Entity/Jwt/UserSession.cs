namespace Servicio.api.Auth.Core.Entities.Jwt;


public class UserSesion : IUserSesion
{
    private readonly IHttpContextAccessor _httpcontextaccesor;

    public UserSesion(IHttpContextAccessor httpcontextaccesor)
    {
        _httpcontextaccesor = httpcontextaccesor;
    }
    public string GetUserSession()
    {
        var username = _httpcontextaccesor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "username")?.Value;
        return username;
    }
}



public interface IUserSesion
{
    string GetUserSession();

}

