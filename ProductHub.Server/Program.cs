using ProductHub.Common.Extensions;
using ProductHub.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using ProductHub.Business.Interfaces;
using ProductHub.Business.Services;
using ProductHub.Common.Interfaces;
using ProductHub.Common.Models;
using ProductHub.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductHubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductHubSqlConnection")));

builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();