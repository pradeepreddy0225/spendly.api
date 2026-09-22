using Microsoft.EntityFrameworkCore;
using Spendly.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SpendlyDbContext>(options =>
{
    options.UseMySQL(connectionString!);
});

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("SpendlyFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

// IMPORTANT:
// No UseHttpsRedirection for this local browser test.

app.UseCors("SpendlyFrontend");

app.MapControllers();

app.Run();