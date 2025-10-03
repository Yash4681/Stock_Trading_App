using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksAppWithXUnit.Models;

namespace StocksAppWithXUnit.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly TradingOptionsModel _options;
        private readonly IFinnhubService _finnhubService;

        public StocksController(IFinnhubService finnhubService, IOptions<TradingOptionsModel> options)
        {
            _options = options.Value;
            _finnhubService = finnhubService;
        }

        [Route("[action]")]
        public async Task<IActionResult> Explore(string? stock, bool showAll = false)
        {
            List<Dictionary<string, string>>? getStocks = await _finnhubService.GetStocks();
            List<Stock> stocks;
            if (!showAll && _options.Top25PopularStocks != null)
            {
                string[] top25PopularStocks = _options.Top25PopularStocks.Split(',');
                getStocks = getStocks?.Where(temp => top25PopularStocks.Contains(Convert.ToString(temp["symbol"]))).ToList();

            }
            stocks = getStocks.Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) }).ToList();

            ViewBag.Stock = stock;

            return View(stocks);
        }
    }
}
