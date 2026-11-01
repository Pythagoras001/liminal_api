using CloudinaryDotNet;
using liminal_api.Modules.Image.Application.Port.Out;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Cloudinary;
using liminal_api.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Lee la sección "Cloudinary" y valida al arrancar (como los throw de constants.ts)
builder.Services.AddOptions<CloudinaryOptions>()
    .BindConfiguration(CloudinaryOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Equivale al useFactory: crea y configura el cliente una sola vez
builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<CloudinaryOptions>>().Value;
    var cloudinary = new Cloudinary(new Account(options.CloudName, options.ApiKey, options.ApiSecret));
    cloudinary.Api.Secure = true;
    return cloudinary;
});

builder.Services.AddScoped<ICloudinaryImageRepositoryPort, CloudinaryImageStorageAdapter>();

// Configure database context with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
