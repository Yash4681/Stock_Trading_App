using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksAppWithXUnit.Models;
using System.Threading.Tasks;

namespace StocksAppWithXUnit.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;
        private readonly TradingOptionsModel _options;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptionsModel> options, IConfiguration configuration, IStocksService stocksService, ILogger<TradeController> logger)
        {
            _finnhubService = finnhubService;
            _options = options.Value;
            _configuration = configuration;
            _stocksService = stocksService;
            _logger = logger;
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
                BuyOrders = await _stocksService.GetAllBuyOrders(),
                SellOrders = await _stocksService.GetAllSellOrders()
            };
            return View(orders);
        }

        [Route("[action]")]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            _logger.LogInformation("SellOrder method is called from TradeController");
            _logger.LogDebug($"sellOrderRequest: {sellOrderRequest}");

            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(sellOrderRequest);

            if(!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = sellOrderRequest.StockName, StockSymbol = sellOrderRequest.StockSymbol, Price = sellOrderRequest.Price, Quantity = sellOrderRequest.Quantity };
                return View("Index", stockTrade);
            }
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);
            return RedirectToAction("Orders", "Trade");
        }

        [Route("[action]")]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            _logger.LogInformation("BuyOrder method is called from TradeController");
            _logger.LogDebug($"buyOrderRequest: {buyOrderRequest}");

            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(buyOrderRequest);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = buyOrderRequest.StockName, StockSymbol = buyOrderRequest.StockSymbol, Price = buyOrderRequest.Price, Quantity = buyOrderRequest.Quantity };
                return View("Index", stockTrade);
            }
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);
            return RedirectToAction("Orders", "Trade");
        }

        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            _logger.LogInformation("OrdersPDF method is called from TradeController");

            Orders orders = new Orders()
            {
                BuyOrders = await _stocksService.GetAllBuyOrders(),
                SellOrders = await _stocksService.GetAllSellOrders(),
            };

            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Margins() { Bottom = 20, Left = 20, Right = 20, Top = 20 },
                PageOrientation = Orientation.Landscape
            };
        }
    }
}
