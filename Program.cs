using Microsoft.EntityFrameworkCore;
using UnauthorizedSWAPI.Data;
using UnauthorizedSWAPI.Repositories;
using UnauthorizedSWAPI.Repositories.Interfaces;
using UnauthorizedSWAPI.Services;
using UnauthorizedSWAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework (commented out for now since we're using Mock repository)
// Uncomment and configure connection string when ready to use database
/*
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
*/

// Register repositories - using Mock implementation for now
// To switch to database implementation, comment out MockUserRepository and uncomment DbUserRepository
builder.Services.AddScoped<IUserRepository, MockUserRepository>();
// builder.Services.AddScoped<IUserRepository, DbUserRepository>();

// Register services
builder.Services.AddScoped<IUserService, UserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "UnauthorizedSWAPI",
        Version = "v1",
        Description = "A clean architecture Web API for user management",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "API Support",
            Email = "support@example.com"
        }
    });
    
    // Include XML comments for better Swagger documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

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
