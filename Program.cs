using System.Text;
using CloudinaryDotNet;
using liminal_api.Modules.Image.Application.Port.Out;
using liminal_api.Modules.Image.Infraestructure.Adapter.Out.Cloudinary;
using liminal_api.Modules.LevelClass.Application.Port.In;
using liminal_api.Modules.LevelClass.Application.Port.Out;
using liminal_api.Modules.LevelClass.Application.Service;
using liminal_api.Modules.LevelClass.Infraestructure.Adapter.Out.Persistence;
using liminal_api.Modules.Report.Application.Port.Out;
using liminal_api.Modules.Report.Application.Service;
using liminal_api.Modules.Report.Infraestructure.Adapter.Out.Persistence;
using liminal_api.Modules.User.Application.Port.In;
using liminal_api.Modules.User.Application.Port.Out;
using liminal_api.Modules.User.Application.Service;
using liminal_api.Modules.User.Infraestructure.Adapter.Out.Persistence;
using liminal_api.Modules.User.Infraestructure.Adapter.Out.Security;
using liminal_api.Shared.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields =
        Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestMethod |
        Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPath |
        Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponseStatusCode;
});
builder.Logging.AddFilter(
    "Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware",
    LogLevel.Information);
builder.Services.AddCors(options => options.AddPolicy("Frontend", policy =>
    policy
        .WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()));

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

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
        };
    });

builder.Services.AddAuthorization();

// User module DI
builder.Services.AddScoped<IPasswordHasherPort, BCryptPasswordHasherAdapter>();
builder.Services.AddScoped<IUserRespositoryPort, UserRespositoryAdapter>();
builder.Services.AddScoped<ITokenServicePort, JwtTokenServiceAdapter>();
builder.Services.AddScoped<ILoginUserUseCase>(sp => sp.GetRequiredService<UserService>());
builder.Services.AddScoped<IRegisterUserUseCase>(sp => sp.GetRequiredService<UserService>());
builder.Services.AddScoped<IUpdateUserUseCase>(sp => sp.GetRequiredService<UserService>());
builder.Services.AddScoped<UserService>();

// Level class module DI
builder.Services.AddScoped<ILevelClassRepositoryPort, LevelClassRepositoryAdapter>();
builder.Services.AddScoped<ILevelClassUseCase, LevelClassService>();

// Report module DI
builder.Services.AddScoped<IReportRepositoryPort, ReportRepositoryAdapter>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<ReportQueryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using IServiceScope scope = app.Services.CreateScope();
    AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseHttpLogging();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
