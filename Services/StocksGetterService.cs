using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class StocksGetterService : IStocksGetterService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<StocksGetterService> _logger;

        public StocksGetterService(IStocksRepository stocksRepository, ILogger<StocksGetterService> logger)
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
        }

        public async Task<List<BuyOrderResponse>> GetAllBuyOrders()
        {
            _logger.LogInformation("GetAllBuyOrders method is called from StocksService");

            List<BuyOrder> buyOrders = await _stocksRepository.GetBuyOrders();
            return buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetAllSellOrders()
        {
            _logger.LogInformation("GetAllSellOrders method is called from StocksService");

            List<SellOrder> sellOrders = await _stocksRepository.GetSellOrders();
            return sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}
