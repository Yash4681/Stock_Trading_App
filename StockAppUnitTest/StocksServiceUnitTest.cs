using AutoFixture;
using Entities;
using FluentAssertions;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;

namespace StockAppUnitTest
{
    public class StocksServiceUnitTest
    {
        private readonly ITestOutputHelper _output;
        private readonly IStocksService _stocksService;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly IStocksRepository _stocksRepository;
        private readonly IFixture _fixture;

        public StocksServiceUnitTest(ITestOutputHelper testOutputHelper) 
        { 
            _fixture = new Fixture();
            _output = testOutputHelper;
            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;

            _stocksService = new StocksService(_stocksRepository);
        }

        #region CreateBuyOrder
        //When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateBuyOrder_NullBuyOrderRequest_ToBeArgumentNullException()
        {
            //Arrange
            BuyOrderRequest? request = null;

            //Act
            Func<Task> action = async () =>
            {
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderQuantityIs0_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>().With(temp => temp.Quantity, (uint)0).Create();

            //Act
            Func<Task> action = async () =>
            {
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderQuantityIs100001_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>().With(temp => temp.Quantity, (uint)100001).Create();

            //Act
            Func<Task> action = async () =>
            {
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderPriceIs0_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>().With(temp => temp.Price, 0).Create();

            //Act
            Func<Task> action = async () =>
            {
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderPriceIs10001_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>().With(temp => temp.Price, 10001).Create();

            //Act
            Func<Task> action = async () =>
            {
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>().With(temp => temp.StockSymbol, null as string).Create();

            //Act
            Func<Task> action = async () =>
            {
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_InvalidDateAndTimeOfOrder_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? request = _fixture.Build<BuyOrderRequest>().With(temp => temp.DateAndTimeOfOrder, new DateTime(1999, 12, 31)).Create();

            //Act
            Func<Task> action = async () =>
            {
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }

        //If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        [Fact]
        public async void CreateBuyOrder_ValidBuyOrderRequest_ToBeSuccessful()
        {
            //Arrange
            BuyOrderRequest? request = _fixture.Create<BuyOrderRequest>();
            BuyOrder buyOrder = request.ToBuyOrder();
            BuyOrderResponse expectedBuyOrderResponse = buyOrder.ToBuyOrderResponse();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            BuyOrderResponse actualBuyOrderResponse = await _stocksService.CreateBuyOrder(request);
            expectedBuyOrderResponse.BuyOrderID = actualBuyOrderResponse.BuyOrderID;

            //Assert
            actualBuyOrderResponse.Should().NotBeNull();
            actualBuyOrderResponse.Should().BeEquivalentTo(expectedBuyOrderResponse);
        }
        #endregion

        #region CreateSellOrder
        //When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateSellOrder_NullSellOrderRequest_ToBeArgumentNullException()
        {
            //Arrange
            SellOrderRequest? request = null;

            //Act
            Func<Task> action = async () =>
            {
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_SellOrderQuantityIs0_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>().With(temp => temp.Quantity, (uint)0).Create();

            //Act
            Func<Task> action = async () =>
            {
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_SellOrderQuantityIs100001_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>().With(temp => temp.Quantity, (uint)100001).Create();

            //Act
            Func<Task> action = async () =>
            {
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_SellOrderPriceIs0_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>().With(temp => temp.Price, 0).Create();

            //Act
            Func<Task> action = async () =>
            {
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_SellOrderPriceIs10001_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>().With(temp => temp.Price, 10001).Create();

            //Act
            Func<Task> action = async () =>
            {
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>().With(temp => temp.StockSymbol, null as string).Create();

            //Act
            Func<Task> action = async () =>
            {
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_InvalidDateAndTimeOfOrder_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? request = _fixture.Build<SellOrderRequest>().With(temp => temp.DateAndTimeOfOrder, new DateTime(1999, 12, 31)).Create();

            //Act
            Func<Task> action = async () =>
            {
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            };

            //Assert
            action.Should().ThrowAsync<ArgumentException>();
        }

        //If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        [Fact]
        public async void CreateSellOrder_ValidSellOrderRequest_ToBeSuccessful()
        {
            //Arrange
            SellOrderRequest? request = _fixture.Create<SellOrderRequest>();
            SellOrder sellOrder = request.ToSellOrder();
            SellOrderResponse expectedSellOrderResponse = sellOrder.ToSellOrderResponse();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            SellOrderResponse actualSellOrderResponse = await _stocksService.CreateSellOrder(request);
            expectedSellOrderResponse.SellOrderID = actualSellOrderResponse.SellOrderID;

            //Assert
            actualSellOrderResponse.Should().NotBeNull();
            actualSellOrderResponse.Should().BeEquivalentTo(expectedSellOrderResponse);
        }
        #endregion

        #region GetAllBuyOrders
        // When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllBuyOrders_ToBeEmptyList()
        {
            //Arrange
            List<BuyOrder> buyOrders = new List<BuyOrder>();
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrders);

            //Act
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetAllBuyOrders();

            //Assert
            buyOrderResponses.Should().BeEmpty();
        }

        // When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async void GetAllBuyOrders_AddFewBuyOrders_ToBeSuccessful()
        {
            //Arrange
            List<BuyOrder>? buyOrders = _fixture.Create<List<BuyOrder>>();

            List<BuyOrderResponse> expectedBuyOrderResponses = buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();

            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrders);

            //Act
            List<BuyOrderResponse> actualBuyOrderResponses = await _stocksService.GetAllBuyOrders();

            //Assert
            actualBuyOrderResponses.Should().BeEquivalentTo(expectedBuyOrderResponses);
        }
        #endregion

        #region GetAllSellOrders
        // When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllSellOrders_ToBeEmptyList()
        {
            //Arrange
            List<SellOrder> sellOrders = new List<SellOrder>();

            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrders);

            //Act
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetAllSellOrders();

            //Assert
            sellOrderResponses.Should().BeEmpty();
        }

        // When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async void GetAllSellOrders_AddFewSellOrders_ToBeSuccessful()
        {
            //Arrange
            List<SellOrder>? sellOrders = _fixture.Create<List<SellOrder>>();

            List<SellOrderResponse> expectedSellOrderResponses = sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();

            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrders);

            //Act
            List<SellOrderResponse> actualSellOrderResponses = await _stocksService.GetAllSellOrders();

            //Assert
            actualSellOrderResponses.Should().BeEquivalentTo(expectedSellOrderResponses);
        }
        #endregion
    }
}