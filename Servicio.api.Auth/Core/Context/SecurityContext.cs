using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Servicio.api.Auth.Core.Entities;

namespace Servicio.api.Auth.Core.Context;

public class SecurityContext : IdentityDbContext<User>
{
    public SecurityContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}