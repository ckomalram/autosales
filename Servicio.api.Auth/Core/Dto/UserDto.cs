namespace Servicio.api.Auth.Core.Dto;

public class UserDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }

    public string Token { get; set; }
}