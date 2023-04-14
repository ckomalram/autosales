using Microsoft.AspNetCore.Identity;
using Servicio.api.Auth.Core.Entities;

namespace Servicio.api.Auth.Core.Context;


public class SecurityContextData
{

    public static async Task InsertUser(SecurityContext context, UserManager<User> userManager)
    {
        if (!userManager.Users.Any())
        {
            var newUser = new User
            {
                Name = "Carlyle",
                LastName = "Komalram",
                Direccion = "Vacamonte, La hacienda",
                UserName = "ckomalram",
                Email = "glaw14@gmail.com"
            };
            await userManager.CreateAsync(newUser, "Pass12345$$$");

        }
    }
}