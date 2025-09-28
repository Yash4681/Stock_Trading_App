using Entities;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse
    {
        public Guid? SellOrderID { get; set; }
        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }

        public DateTime? DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        public double TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            SellOrderResponse? other = obj as SellOrderResponse;
            if (other == null) return false;

            return StockName == other.StockName && StockSymbol == other.StockSymbol &&
                Price == other.Price && TradeAmount == other.TradeAmount &&
                Quantity == other.Quantity && Price == other.Price && SellOrderID == other.SellOrderID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class SellOrderExtentions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                Price = sellOrder.Price,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                TradeAmount = sellOrder.Quantity * sellOrder.Price
            };
        }
    }
}
