using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System.Runtime.ConstrainedExecution;
using Xunit.Abstractions;

namespace StockAppUnitTest
{
    public class StocksServiceUnitTest
    {
        private readonly ITestOutputHelper _output;
        private readonly IStocksService _stocksService;

        public StocksServiceUnitTest(ITestOutputHelper testOutputHelper) 
        { 
            _output = testOutputHelper;
            _stocksService = new StocksService(new StocksMarketDbContext(new DbContextOptionsBuilder<StocksMarketDbContext>().Options));
        }

        #region CreateBuyOrder
        //When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateBuyOrder_NullBuyOrderRequest()
        {
            //Arrange
            BuyOrderRequest? request = null;

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            });
        }

        //When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderQuantityIs0()
        {
            //Arrange
            BuyOrderRequest? request = new BuyOrderRequest()
            {
                Quantity = 0,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 10,
                StockName = "Test",
                StockSymbol = "MSFT"
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            });
        }

        //When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderQuantityIs100001()
        {
            //Arrange
            BuyOrderRequest? request = new BuyOrderRequest()
            {
                Quantity = 100001,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 10,
                StockName = "Test",
                StockSymbol = "MSFT"
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            });
        }

        //When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderPriceIs0()
        {
            //Arrange
            BuyOrderRequest? request = new BuyOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 0,
                StockName = "Test",
                StockSymbol = "MSFT"
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            });
        }

        //When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderPriceIs10001()
        {
            //Arrange
            BuyOrderRequest? request = new BuyOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 10001,
                StockName = "Test",
                StockSymbol = "MSFT"
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            });
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_StockSymbolIsNull()
        {
            //Arrange
            BuyOrderRequest? request = new BuyOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 10,
                StockName = "Test",
                StockSymbol = null
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            });
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_InvalidDateAndTimeOfOrder()
        {
            //Arrange
            BuyOrderRequest? request = new BuyOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = new DateTime(1999, 12, 31),
                Price = 10,
                StockName = "Test",
                StockSymbol = "MSFT"
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);
            });
        }

        //If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        [Fact]
        public async void CreateBuyOrder_ValidBuyOrderRequest()
        {
            //Arrange
            BuyOrderRequest? request = new BuyOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = new DateTime(2001, 12, 31),
                Price = 10,
                StockName = "Test",
                StockSymbol = "MSFT"
            };

            //Act
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(request);

            //Assert
            Assert.NotNull(buyOrderResponse);
            Assert.True(buyOrderResponse?.BuyOrderID != null);
        }
        #endregion

        #region CreateSellOrder
        //When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateSellOrder_NullSellOrderRequest()
        {
            //Arrange
            SellOrderRequest? request = null;

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            });
        }

        //When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_SellOrderQuantityIs0()
        {
            //Arrange
            SellOrderRequest? request = new SellOrderRequest()
            {
                Quantity = 0,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 10,
                StockName = "Test",
                StockSymbol = "MSFT"
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            });
        }

        //When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_SellOrderQuantityIs100001()
        {
            //Arrange
            SellOrderRequest? request = new SellOrderRequest()
            {
                Quantity = 100001,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 10,
                StockName = "Test",
                StockSymbol = "MSFT"
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            });
        }

        //When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_SellOrderPriceIs0()
        {
            //Arrange
            SellOrderRequest? request = new SellOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 0,
                StockName = "Test",
                StockSymbol = "MSFT"
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            });
        }

        //When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_SellOrderPriceIs10001()
        {
            //Arrange
            SellOrderRequest? request = new SellOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 10001,
                StockName = "Test",
                StockSymbol = "MSFT"
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            });
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_StockSymbolIsNull()
        {
            //Arrange
            SellOrderRequest? request = new SellOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = DateTime.UtcNow,
                Price = 10,
                StockName = "Test",
                StockSymbol = null
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            });
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_InvalidDateAndTimeOfOrder()
        {
            //Arrange
            SellOrderRequest? request = new SellOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = new DateTime(1999, 12, 31),
                Price = 10,
                StockName = "Test",
                StockSymbol = "MSFT"
            };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                SellOrderResponse buyOrderResponse = await _stocksService.CreateSellOrder(request);
            });
        }

        //If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        [Fact]
        public async void CreateSellOrder_ValidSellOrderRequest()
        {
            //Arrange
            SellOrderRequest? request = new SellOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = new DateTime(2001, 12, 31),
                Price = 10,
                StockName = "Test",
                StockSymbol = "MSFT"
            };

            //Act
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(request);

            //Assert
            Assert.NotNull(sellOrderResponse);
            Assert.True(sellOrderResponse?.SellOrderID != null);
        }
        #endregion

        #region GetAllBuyOrders
        // When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllBuyOrders_EmptyList()
        {
            //Act
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetAllBuyOrders();

            //Assert
            Assert.Empty(buyOrderResponses);
        }

        // When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async void GetAllBuyOrders_AddFewBuyOrders()
        {
            //Arrange
            BuyOrderRequest? request1 = new BuyOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = new DateTime(2001, 12, 31),
                Price = 10,
                StockName = "Microsoft",
                StockSymbol = "MSFT"
            };

            BuyOrderRequest? request2 = new BuyOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = new DateTime(2001, 12, 31),
                Price = 10,
                StockName = "Apple",
                StockSymbol = "Appl"
            };

            BuyOrderRequest? request3 = new BuyOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = new DateTime(2001, 12, 31),
                Price = 10,
                StockName = "Google",
                StockSymbol = "Ggl"
            };

            List<BuyOrderRequest> buyOrderRequests = new List<BuyOrderRequest>() { request1, request2, request3 };
            List<BuyOrderResponse> buyOrderResponsesFromAdd = new List<BuyOrderResponse>();
            foreach (var request in buyOrderRequests)
            {
                BuyOrderResponse response = await _stocksService.CreateBuyOrder(request);
                buyOrderResponsesFromAdd.Add(response);
            }

            //Act
            List<BuyOrderResponse> buyOrderResponsesFromGet = await _stocksService.GetAllBuyOrders();

            //Assert
            foreach (var response in buyOrderResponsesFromGet)
            {
                Assert.Contains(response, buyOrderResponsesFromAdd);
            }
        }
        #endregion

        #region GetAllSellOrders
        // When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllSellOrders_EmptyList()
        {
            //Act
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetAllSellOrders();

            //Assert
            Assert.Empty(sellOrderResponses);
        }

        // When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async void GetAllSellOrders_AddFewSellOrders()
        {
            //Arrange
            SellOrderRequest? request1 = new SellOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = new DateTime(2001, 12, 31),
                Price = 10,
                StockName = "Microsoft",
                StockSymbol = "MSFT"
            };

            SellOrderRequest? request2 = new SellOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = new DateTime(2001, 12, 31),
                Price = 10,
                StockName = "Apple",
                StockSymbol = "Appl"
            };

            SellOrderRequest? request3 = new SellOrderRequest()
            {
                Quantity = 10,
                DateAndTimeOfOrder = new DateTime(2001, 12, 31),
                Price = 10,
                StockName = "Google",
                StockSymbol = "Ggl"
            };

            List<SellOrderRequest> sellOrderRequests = new List<SellOrderRequest>() { request1, request2, request3 };
            List<SellOrderResponse> sellOrderResponsesFromAdd = new List<SellOrderResponse>();
            foreach (var request in sellOrderRequests)
            {
                SellOrderResponse response = await _stocksService.CreateSellOrder(request);
                sellOrderResponsesFromAdd.Add(response);
            }

            //Act
            List<SellOrderResponse> sellOrderResponsesFromGet = await _stocksService.GetAllSellOrders();

            //Assert
            foreach (var response in sellOrderResponsesFromGet)
            {
                Assert.Contains(response, sellOrderResponsesFromAdd);
            }
        }
        #endregion
    }
}