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



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
