using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;

namespace Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly StocksMarketDbContext _db;
        private readonly ILogger<StocksRepository> _logger;
        public StocksRepository(StocksMarketDbContext db, ILogger<StocksRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _logger.LogInformation("CreateBuyOrder method is called from StocksRepository");
            _logger.LogDebug($"buyOrder: {buyOrder}");

            _db.Add(buyOrder);
            await _db.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            _logger.LogInformation("CreateSellOrder method is called from StocksRepository");
            _logger.LogDebug($"sellOrder: {sellOrder}");

            _db.Add(sellOrder);
            await _db.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            _logger.LogInformation("GetBuyOrders method is called from StocksRepository");

            return await _db.BuyOrders.ToListAsync();
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            _logger.LogInformation("GetSellOrders method is called from StocksRepository");

            return await _db.SellOrders.ToListAsync();
        }
    }
}
