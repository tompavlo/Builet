using Builet.BaseRepository;
using Builet.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString") 
                      ?? builder.Configuration["DbConnectionString"]));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

