using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Servicio.api.Cliente.Core;
using Servicio.api.Cliente.Core.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// adding CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsRule", rule =>
    {
        rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
    });
});

// conf mongo
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoDb")
);

// conf singleton
builder.Services.AddSingleton<MongoSettings>();


// Inyectando mongo repo generico
builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

// No pedir ID al insertar datos
builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
