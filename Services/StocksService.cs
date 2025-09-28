using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly StocksMarketDbContext _db;

        public StocksService(StocksMarketDbContext stocksMarketDbContext)
        {
            _db = stocksMarketDbContext;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if(buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();
            await _db.AddAsync(buyOrder);
            await _db.SaveChangesAsync();

            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if(sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();
            await _db.AddAsync(sellOrder);
            await _db.SaveChangesAsync();

            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetAllBuyOrders()
        {
            return await _db.BuyOrders.Select(temp => temp.ToBuyOrderResponse()).ToListAsync();
        }

        public async Task<List<SellOrderResponse>> GetAllSellOrders()
        {
            return await _db.SellOrders.Select(temp => temp.ToSellOrderResponse()).ToListAsync();
        }
    }
}
