using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Conf ocelot

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("ocelot.json").Build();

// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// conf CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsRule", rule =>
    {
        // rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://mipagina.com");
        rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
    });
});


// Add ocelot
builder.Services.AddOcelot(configuration);

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Add mw cors
app.UseCors("CorsRule");
// Add mw auth jwt
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
// Add middelware Ocelot
app.UseOcelot().Wait();

app.Run();
