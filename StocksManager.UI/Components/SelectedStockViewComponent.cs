using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using StocksAppWithXUnit.Models;

namespace StocksAppWithXUnit.Components
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubService _finnhubService;
        private readonly ILogger<SelectedStockViewComponent> _logger;

        public SelectedStockViewComponent(IFinnhubService finnhubService, ILogger<SelectedStockViewComponent> logger)
        {
            _finnhubService = finnhubService;
            _logger = logger;
        }
        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            _logger.LogInformation("SelectedStockViewComponent is called");
            _logger.LogDebug($"stockSymbol: {stockSymbol}");

            if (stockSymbol == null) return null;

            Dictionary<string, object>? profile = await _finnhubService.GetCompanyProfile(stockSymbol);
            Dictionary<string, object>? priceQuote = await _finnhubService.GetStockPriceQuote(stockSymbol);

            SelectedStockModel selectedStockModel = new SelectedStockModel()
            {
                Logo = Convert.ToString(profile["logo"]),
                StockName = Convert.ToString(profile["name"]),
                StockSymbol = stockSymbol,
                Industry = Convert.ToString(profile["finnhubIndustry"]),
                Exchange = Convert.ToString(profile["exchange"]),
                Price = Convert.ToDouble(Convert.ToString(priceQuote["c"]))
            };

            return View(selectedStockModel);
        }
    }
}
