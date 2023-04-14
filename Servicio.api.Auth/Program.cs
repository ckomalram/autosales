using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Servicio.api.Auth.Core.Application;
using Servicio.api.Auth.Core.Context;
using Servicio.api.Auth.Core.Entities;
using Servicio.api.Auth.Core.Entities.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Register>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//conf Cors
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsRule", rule =>
    {
        // rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://mipagina.com");
        rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
    });
});

// conf sql server
builder.Services.AddSqlServer<SecurityContext>(builder.Configuration.GetConnectionString("cnSecurityDb"));

// conf identity core: autenticacion y autorización
builder.Services.AddIdentityCore<User>()
.AddRoles<IdentityRole>() // Roles
.AddEntityFrameworkStores<SecurityContext>()
.AddSignInManager<SignInManager<User>>();

// conf system clock: forma de obtener la hora actual y se utiliza en la autenticación de ASP.NET Core, en particular para validar los tokens JWT
builder.Services.TryAddSingleton<ISystemClock, SystemClock>();

// Useless for Register.class
//conf mediaTR
builder.Services.AddMediatR(typeof(Register.UserRegisterCommand).Assembly);
//conf automapper
builder.Services.AddAutoMapper(typeof(Register.UserRegisterHandler));

//conf token
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
//conf userSession
builder.Services.AddScoped<IUserSesion, UserSesion>();

//conf auth JWT
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RrF1XwA6ke5nApomZfCzrflviFtkxgqj"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // options.RequireHttpsMetadata = false;
        // options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false, // Verificar el dominio de donde se genera el token
            ValidateAudience = false, // true ciertos ip puedan acceder a mi aplicacion
        };
    });


var app = builder.Build();


// conf Roles
using (var context = app.Services.CreateScope())
{
    var services = context.ServiceProvider;

    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        SecurityContextData.InsertRoles(roleManager).Wait();

    }
    catch (Exception e)
    {
        var logging = services.GetRequiredService<ILogger<Program>>();
        logging.LogError(e, "Error al registrar Roles...");
    }

}

// conf user for testing
using (var context = app.Services.CreateScope())
{
    var services = context.ServiceProvider;

    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var contextEf = services.GetRequiredService<SecurityContext>();

        SecurityContextData.InsertUser(contextEf, userManager).Wait();

    }
    catch (Exception e)
    {
        var logging = services.GetRequiredService<ILogger<Program>>();
        logging.LogError(e, "Error al registrar usuario de prueba");
    }

}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsRule");

app.UseAuthorization();

app.MapControllers();

app.Run();
