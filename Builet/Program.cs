using Builet.BaseRepository;
using Builet.Database;
using Builet.Inventory;
using Builet.Stock;
using Builet.Transaction;
using Builet.User;
using Builet.Wallet;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString") 
                      ?? builder.Configuration["DbConnectionString"]));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<WalletService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<StockService>();
builder.Services.AddScoped<InventoryService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Builet API",
        Version = "v1",
        Description = "API for managing users, stocks, wallets, and transactions"
    });
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI( c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Builet API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();