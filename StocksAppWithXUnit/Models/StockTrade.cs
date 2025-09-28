using System.ComponentModel.DataAnnotations;

namespace StocksAppWithXUnit.Models
{
    public class StockTrade
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public double Price { get; set; }

        [Required(ErrorMessage = "Quantity can't be blank")]
        [Range(1, 10000, ErrorMessage = "Quantity must be between 1 and 10000")]
        public uint Quantity { get; set; }
    }
}
