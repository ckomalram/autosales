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
                RoleName = "Admin",
                UserName = "ckomalram",
                Email = "glaw14@gmail.com"
            };

            await userManager.CreateAsync(newUser, "Pass12345$$$");
            await userManager.AddToRoleAsync(newUser, "Admin");
        }
    }

    public static async Task InsertRoles(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Admin", "Normal", "Customer" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var newRole = new IdentityRole(role);
                await roleManager.CreateAsync(newRole);
            }
        }

    }
}
