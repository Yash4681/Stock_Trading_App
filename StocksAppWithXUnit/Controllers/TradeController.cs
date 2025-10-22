using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksAppWithXUnit.Filters.ActionFilters;
using StocksAppWithXUnit.Models;
using System.Threading.Tasks;

namespace StocksAppWithXUnit.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksGetterService _stocksGetterService;
        private readonly IStocksCreaterService _stocksCreaterService;
        private readonly TradingOptionsModel _options;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptionsModel> options, IConfiguration configuration, IStocksGetterService stocksGetterService, IStocksCreaterService stocksCreaterService, ILogger<TradeController> logger)
        {
            _finnhubService = finnhubService;
            _options = options.Value;
            _configuration = configuration;
            _stocksGetterService = stocksGetterService;
            _logger = logger;
            _stocksCreaterService = stocksCreaterService;
        }

        [Route("/")]
        [Route("[action]/{stockSymbol?}")]
        public async Task<IActionResult> Index(string? stockSymbol)
        {
            _logger.LogInformation("Index method is called from TradeController");
            _logger.LogDebug($"stockSymbol: {stockSymbol}");

            if (stockSymbol == null)
            {
                stockSymbol = _options.DefaultStockSymbol;
            }

            Dictionary<string,object>? getStockPriceQuote = await _finnhubService.GetStockPriceQuote(stockSymbol);
            Dictionary<string,object>? getCompanyProfile = await _finnhubService.GetCompanyProfile(stockSymbol);

            StockTrade stockTrade = new StockTrade()
            {
                StockName = Convert.ToString(getCompanyProfile["name"]),
                StockSymbol = _options.DefaultStockSymbol,
                Price = Convert.ToDouble(Convert.ToString(getStockPriceQuote["c"])),
                Quantity = Convert.ToUInt32(_options.DefaultOrderQuantity)
            };

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }

        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            _logger.LogInformation("Orders method is called from TradeController");

            Orders orders = new Orders()
            {
                BuyOrders = await _stocksGetterService.GetAllBuyOrders(),
                SellOrders = await _stocksGetterService.GetAllSellOrders()
            };
            return View(orders);
        }

        [Route("[action]")]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            _logger.LogInformation("SellOrder method is called from TradeController");
            _logger.LogDebug($"sellOrderRequest: {orderRequest}");

            SellOrderResponse sellOrderResponse = await _stocksCreaterService.CreateSellOrder(orderRequest);

            return RedirectToAction("Orders", "Trade");
        }

        [Route("[action]")]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            _logger.LogInformation("BuyOrder method is called from TradeController");
            _logger.LogDebug($"buyOrderRequest: {orderRequest}");
            
            BuyOrderResponse buyOrderResponse = await _stocksCreaterService.CreateBuyOrder(orderRequest);

            return RedirectToAction("Orders", "Trade");
        }

        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            _logger.LogInformation("OrdersPDF method is called from TradeController");

            Orders orders = new Orders()
            {
                BuyOrders = await _stocksGetterService.GetAllBuyOrders(),
                SellOrders = await _stocksGetterService.GetAllSellOrders(),
            };

            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Margins() { Bottom = 20, Left = 20, Right = 20, Top = 20 },
                PageOrientation = Orientation.Landscape
            };
        }
    }
}
