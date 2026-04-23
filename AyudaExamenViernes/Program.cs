using AyudaExamenViernes.Data;
using AyudaExamenViernes.Helpers;
using AyudaExamenViernes.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
// REGISTRO DE TU HELPER
builder.Services.AddTransient<HelperFotoTransform>();

// REGISTRO DE TU REPOSITORIO (seguramente ya lo tienes así)
builder.Services.AddTransient<RepositoryComics>();
// Add services to the container.

string connectionString = builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddDbContext<ComicContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapOpenApi();
app.MapScalarApiReference();


app.MapGet("/", context =>
{
    context.Response.Redirect("/scalar");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Fotos")),
    RequestPath = "/Fotos"
});

app.UseAuthorization();

app.MapControllers();

app.Run();
