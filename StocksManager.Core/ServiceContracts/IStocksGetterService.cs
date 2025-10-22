using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IStocksGetterService
    {
        Task<List<BuyOrderResponse>> GetAllBuyOrders();
        Task<List<SellOrderResponse>> GetAllSellOrders();
    }
}
