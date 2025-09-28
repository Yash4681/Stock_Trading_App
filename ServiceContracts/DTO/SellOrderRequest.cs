using Entities;
using ServiceContracts.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class SellOrderRequest
    {
        [Required(ErrorMessage = "Stock Symbol can't be blank")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name can't be blank")]
        public string? StockName { get; set; }

        [DateAndTimeOfOrderValidation]
        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity should be between 1 and 100000")]
        public uint Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "Quantity should be between 1 and 10000")]
        public double Price { get; set; }

        public SellOrder ToSellOrder()
        {
            return new SellOrder()
            {
                StockName = StockName,
                StockSymbol = StockSymbol,
                Quantity = Quantity,
                Price = Price,
                DateAndTimeOfOrder = DateAndTimeOfOrder
            };
        }
    }
}
