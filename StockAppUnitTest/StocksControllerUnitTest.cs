using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ServiceContracts;
using StocksAppWithXUnit.Controllers;
using StocksAppWithXUnit.Models;

namespace StockAppUnitTest
{
    public class StocksControllerUnitTest
    {
        private readonly IFinnhubService _finnhubService;
        private readonly Mock<IFinnhubService> _finnhubServiceMock;
        private readonly StocksController _stocksController;
        private readonly IFixture _fixture;

        public StocksControllerUnitTest()
        {
            _fixture = new Fixture();
            _finnhubServiceMock = new Mock<IFinnhubService>();
            _finnhubService = _finnhubServiceMock.Object;

            var tradingOptions = new TradingOptionsModel
            {
                DefaultOrderQuantity = "100",
                DefaultStockSymbol = "MSFT",
                Top25PopularStocks = "AAPL,MSFT,AMZN,TSLA,GOOGL,GOOG,NVDA,BRK.B,META,UNH,JNJ,JPM,V,PG,XOM,HD,CVX,MA,BAC,ABBV,PFE,AVGO,COST,DIS,KO"
            };
            var options = Options.Create(tradingOptions);

            var loggerMock = new Mock<ILogger<StocksController>>();

            _stocksController = new StocksController(_finnhubService, options, loggerMock.Object);
        }

        [Fact]
        public async Task Explore_StockIsNull_ToReturnListOfStock()
        {
            //Arrange
            var expectedStocks = new List<Dictionary<string, string>>
            {
                new() { { "symbol", "AAPL" }, { "description", "Apple Inc." } },
                new() { { "symbol", "MSFT" }, { "description", "Microsoft Corporation" } },
                new() { { "symbol", "AMZN" }, { "description", "Amazon.com Inc." } },
                new() { { "symbol", "TSLA" }, { "description", "Tesla Inc." } },
                new() { { "symbol", "GOOGL" }, { "description", "Alphabet Inc. (Class A)" } },
                new() { { "symbol", "META" }, { "description", "Meta Platforms Inc." } },
                new() { { "symbol", "UNH" }, { "description", "UnitedHealth Group Inc." } },
            };

            _finnhubServiceMock.Setup(temp => temp.GetStocks()).ReturnsAsync(expectedStocks);

            //Act
            IActionResult actionResult = await _stocksController.Explore(null, false);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(actionResult);
            viewResult.ViewData.Model.Should().BeAssignableTo<List<Stock>>();
        }
    }
}
