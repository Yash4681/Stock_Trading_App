using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using ServiceContracts;
using Services;
using StocksAppWithXUnit.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptionsModel>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddHttpClient();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddScoped<IStocksService, StocksService>();

builder.Services.AddDbContext<StocksMarketDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
