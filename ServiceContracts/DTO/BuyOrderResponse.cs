using Entities;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse : IOrderResponse
    {
        public Guid? BuyOrderID { get; set; }
        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }

        public DateTime? DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        public double TradeAmount { get; set; }

        public OrderType OrderType => OrderType.BuyOrder;

        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            BuyOrderResponse? other = obj as BuyOrderResponse;
            if(other == null) return false;

            return StockName == other.StockName && StockSymbol == other.StockSymbol &&
                Price == other.Price && TradeAmount == other.TradeAmount &&
                Quantity == other.Quantity && Price == other.Price && BuyOrderID == other.BuyOrderID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class BuyOrderExtentions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse()
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                Price = buyOrder.Price,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                TradeAmount = buyOrder.Quantity * buyOrder.Price
            };
        }
    }
}
