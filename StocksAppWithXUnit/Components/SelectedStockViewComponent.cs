using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using StocksAppWithXUnit.Models;

namespace StocksAppWithXUnit.Components
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubService _finnhubService;

        public SelectedStockViewComponent(IFinnhubService finnhubService)
        {
            _finnhubService = finnhubService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
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
