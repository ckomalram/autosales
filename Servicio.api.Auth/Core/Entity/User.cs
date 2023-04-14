using Microsoft.AspNetCore.Identity;

namespace Servicio.api.Auth.Core.Entities;

public class User : IdentityUser
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string RoleName { get; set; }

}