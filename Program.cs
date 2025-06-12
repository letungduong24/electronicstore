using ElectronicStore.Repositories;
using ElectronicStore.Factories;
using ElectronicStore.Mappers;
using ElectronicStore.Mappers.Strategies;
using ElectronicStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Register Repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ProductRepository>();

builder.Services.AddAuthorization();

// Đăng ký Decorator kiểm tra tồn kho
builder.Services.AddScoped<IProductRepository>(sp =>
    new StockValidationProductRepositoryDecorator(
        sp.GetRequiredService<ProductRepository>()
    )
);

// Register Factory
builder.Services.AddScoped<IProductFactory, ProductFactory>();

// Register Mapping Strategies
builder.Services.AddScoped<IProductMappingStrategy, AirConditionerMappingStrategy>();
builder.Services.AddScoped<IProductMappingStrategy, TelevisionMappingStrategy>();
builder.Services.AddScoped<IProductMappingStrategy, WashingMachineMappingStrategy>();

// Register Mapper
builder.Services.AddScoped<ProductMapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
